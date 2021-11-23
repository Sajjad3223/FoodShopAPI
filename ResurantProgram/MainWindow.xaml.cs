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
        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void AddToCart(object sender, ResturantProgram.User_Controlls.FoodCard e)
        {
            var count = int.Parse(cartBadge.Badge.ToString());
            cartBadge.Badge = count + 1;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            grayPanel.Visibility = Visibility.Visible;
            loginWindow.OnLoginSucceeded += (s, e) =>
            {
                loginBtn.Visibility = Visibility.Collapsed;
                ManageAuthotiy();
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
                else {
                    Informations.User.UserRole = Informations.EUserRole.User;
                    AddFoodBtn.Visibility = Visibility.Collapsed;
                }
                GetFoodsAsync(null,null);
            }
        }

        private void ShowCart(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            Visibility = Visibility.Collapsed;
            orderWindow.ShowDialog();

            Visibility = Visibility.Visible;
        }

        private void EditAddress(object sender, RoutedEventArgs e)
        {
            UserAddressWindow addressWindow = new UserAddressWindow();
            grayPanel.Visibility = Visibility.Visible;
            
            addressWindow.ShowDialog();

            grayPanel.Visibility = Visibility.Collapsed;
            addressEdit.Content = Informations.User.FullName;
        }

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
            var foodDtos = JsonConvert.DeserializeObject<List<FoodDTO>>(json);
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
                ManagePaginations(foodDtos);
            }
        }

        private void ManagePaginations(List<FoodDTO> foodDtos)
        {
            throw new NotImplementedException();
        }
    }
}
