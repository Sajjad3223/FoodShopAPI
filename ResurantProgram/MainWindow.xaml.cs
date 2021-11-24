using DataLayer.DTOs;
using Newtonsoft.Json;
using ResturantProgram;
using ResturantProgram.User_Controlls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResurantProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        
        private int _pageId = 1;
        private readonly int foodTake = 6;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window Manager

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Cart Functions

        private async void AddToCart(object sender, ResturantProgram.User_Controlls.FoodCard e)
        {
            if (string.IsNullOrEmpty(Informations.Token))
            {
                MessageBox.Show("برای خرید باید وارد حساب خود شوید");
                Login(null,null);
                return;
            }

            using HttpClient client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Order/AddToCart/{e.FoodId}/{e.FoodCount}";
            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", Informations.Token);

            var response = await client.GetAsync(actionUrl);

            if (response.IsSuccessStatusCode)
            {
                OrderDTO order = JsonConvert.DeserializeObject<OrderDTO>(await response.Content.ReadAsStringAsync()) ?? null;
                if(order != null)
                {
                    cartBadge.Badge = order.OrderItems.Count;
                }
            }
        }

        private void ShowCart(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            Visibility = Visibility.Collapsed;
            orderWindow.ShowDialog();

            Visibility = Visibility.Visible;
        }

        private async void ManageCart()
        {
            cartBadge.Visibility = Visibility.Visible;

            using HttpClient client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Order";

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Informations.Token);

            var response = await client.GetAsync(actionUrl);

            if (response.IsSuccessStatusCode)
            {
                OrderDTO userOrder = JsonConvert.DeserializeObject<OrderDTO>(await response.Content.ReadAsStringAsync());
                if (userOrder != null)
                {
                    if (userOrder.OrderItems.Any())
                        cartBadge.Badge = userOrder.OrderItems.Count;
                }
            }
        }

        #endregion

        #region User Functions

        private void Login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            grayPanel.Visibility = Visibility.Visible;
            loginWindow.OnLoginSucceeded += (s, e) =>
            {
                loginBtn.Visibility = Visibility.Collapsed;
                ManageAuthotiy();
                ManageCart();
            };
            loginWindow.ShowDialog();

            grayPanel.Visibility = Visibility.Collapsed;
        }

        private async void ManageAuthotiy()
        {
            using var client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Account";
            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Informations.Token);
            var response = await client.GetAsync(actionUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDTO>(json);

                addressEdit.Content = user.FullName;
                addressEdit.Visibility = Visibility.Visible;

                Informations.User = new User(user);

                if (user.Roles.Contains("Administrator"))
                {
                    Informations.User.UserRole = Informations.EUserRole.Admin;
                    AddFoodBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    Informations.User.UserRole = Informations.EUserRole.User;
                    AddFoodBtn.Visibility = Visibility.Collapsed;
                }
                GetFoodsAsync(null, null);
            }
        }

        private void EditAddress(object sender, RoutedEventArgs e)
        {
            UserAddressWindow addressWindow = new UserAddressWindow();
            grayPanel.Visibility = Visibility.Visible;

            addressWindow.ShowDialog();

            grayPanel.Visibility = Visibility.Collapsed;
            addressEdit.Content = Informations.User.FullName;
        }


        #endregion

        #region Food Functions


        private void AddFood(object sender, RoutedEventArgs e)
        {
            CreateFoodWindow foodWindow = new CreateFoodWindow();
            foodWindow.ShowDialog();
        }

        private async void GetFoodsAsync(object sender, RoutedEventArgs e)
        {
            using var client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Food";
            var response = await client.GetAsync(actionUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                FillFoodPanel(json);
            }
        }

        private async void SearchFood(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                GetFoodsAsync(sender, null);
                return;
            }

            using var client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Food/search/{searchBox.Text}";

            var response = await client.GetAsync(actionUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                FillFoodPanel(json);
            }
        }

        public void FillFoodPanel(string json)
        {
            var allFoodDtos = JsonConvert.DeserializeObject<List<FoodDTO>>(json);
            var foodDtos = ManagePaginations(allFoodDtos);
            if (foodDtos.Any())
            {
                foodsPanel.Children.Clear();
                foreach (var food in foodDtos)
                {
                    FoodCard foodCard = new FoodCard()
                    {
                        FoodName = food.FoodName,
                        FoodId = food.Id,
                        FoodPrice = food.Price,
                        Image = new BitmapImage(new Uri(Informations.FOODS_IMAGE_PATH + food.ImageName, UriKind.RelativeOrAbsolute)),
                        ImageName = food.ImageName,
                        Margin = new Thickness(10),
                        UserIsAdmin = (Informations.User.UserRole == Informations.EUserRole.Admin) ? Visibility.Visible : Visibility.Collapsed
                    };

                    foodCard.OnFoodAddedToCart += AddToCart;

                    foodsPanel.Children.Add(foodCard);
                }
            }
        }

        private List<FoodDTO> ManagePaginations(List<FoodDTO> foodDtos)
        {
            int pageCount = Convert.ToInt32(Math.Ceiling(((float)foodDtos.Count / foodTake)));
            List<FoodDTO> pageFoods = foodDtos.Skip((_pageId - 1) * foodTake).Take(foodTake).ToList();
            pagination.Children.Clear();
            for (int i = 1; i < pageCount + 1; i++)
            {
                PageButton pageButton = new PageButton();

                if (_pageId != i)
                    pageButton.Background = Brushes.Transparent;

                pageButton.ChangePage += GoToPage;

                pageButton.PageNumber = i.ToString();

                pagination.Children.Add(pageButton);
            }

            return pageFoods;
        }

        private void GoToPage(object sender, int pageNumber)
        {
            _pageId = pageNumber;
            SearchFood(null, null);
        }

        #endregion
    }
}
