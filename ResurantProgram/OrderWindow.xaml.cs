using DataLayer.DTOs;
using Newtonsoft.Json;
using ResturantProgram.User_Controlls;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ResturantProgram
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
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

        private void EditAddress(object sender, RoutedEventArgs e)
        {
            UserAddressWindow addressWindow = new UserAddressWindow();
            addressWindow.addressInput.Text = address.Text;
            grayPanel.Visibility = Visibility.Visible;
            addressWindow.ShowDialog();

            grayPanel.Visibility = Visibility.Collapsed;
            userName.Text = Informations.User.FullName;
            address.Text = Informations.User.Address;
        }

        private void SubmitAndPayOrder(object sender, RoutedEventArgs e)
        {

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                userName.Text = Informations.User.FullName;
                address.Text = Informations.User.Address;

                var response = await GetOrder();

                if (response.IsSuccessStatusCode)
                {
                    OrderDTO userOrder = JsonConvert.DeserializeObject<OrderDTO>(await response.Content.ReadAsStringAsync());
                    if (userOrder != null)
                    {
                        if (userOrder.OrderItems.Any())
                        {
                            orderItemsPanel.Children.Clear();
                            foreach (var item in userOrder.OrderItems)
                            {
                                OrderItem orderItem = new OrderItem()
                                {
                                    ItemId = item.Id,
                                    FoodId = item.FoodId,
                                    OrderId = item.OrderId,
                                    FoodName = item.Food.FoodName,
                                    FoodPrice = item.Price,
                                    FoodCount = item.Count,
                                    Image = new BitmapImage(new Uri(Informations.FOODS_IMAGE_PATH + item.Food.ImageName, UriKind.RelativeOrAbsolute)),
                                    Margin = new Thickness(0, 5, 0, 5)
                                };

                                orderItem.OrderItemUpdated += OrderItem_OrderItemUpdated;

                                orderItemsPanel.Children.Add(orderItem);
                            }

                            totalPrice.Text = userOrder.TotalPrice.ToString("N0");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException.Message);
            }
        }

        private async Task<HttpResponseMessage> GetOrder()
        {
            using HttpClient client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Order";

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Informations.Token);

            var response = await client.GetAsync(actionUrl);

            return response;
        }

        private async void OrderItem_OrderItemUpdated(object? sender, OrderItem e)
        {
            var response = await GetOrder();
            if (response.IsSuccessStatusCode)
            {
                OrderDTO userOrder = JsonConvert.DeserializeObject<OrderDTO>(await response.Content.ReadAsStringAsync());
                if (userOrder != null)
                {
                    totalPrice.Text = userOrder.TotalPrice.ToString("N0");
                }
            }


        }
    }
}
