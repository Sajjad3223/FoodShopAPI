using DataLayer.DTOs;
using Newtonsoft.Json;
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

namespace ResturantProgram.User_Controlls
{
    /// <summary>
    /// Interaction logic for OrderItem.xaml
    /// </summary>
    public partial class OrderItem : UserControl
    {
        public event EventHandler<OrderItem> OrderItemUpdated;

        public OrderItem()
        {
            InitializeComponent();
        }

        private int _itemId;

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        private int _foodId;

        public int FoodId
        {
            get { return _foodId; }
            set { _foodId = value; }
        }

        private int _orderId;

        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }


        public int FoodCount
        {
            get { return (int)GetValue(FoodCountProperty); }
            set { SetValue(FoodCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodCountProperty =
            DependencyProperty.Register("FoodCount", typeof(int), typeof(OrderItem));



        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(OrderItem));



        public int FoodPrice
        {
            get { return (int)GetValue(FoodPriceProperty); }
            set { SetValue(FoodPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodPriceProperty =
            DependencyProperty.Register("FoodPrice", typeof(int), typeof(OrderItem));



        public int TotalPrice
        {
            get { return FoodCount * FoodPrice; }
            set { SetValue(TotalPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalPriceProperty =
            DependencyProperty.Register("TotalPrice", typeof(int), typeof(OrderItem));


        public string FoodName
        {
            get { return (string)GetValue(FoodNameProperty); }
            set { SetValue(FoodNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodNameProperty =
            DependencyProperty.Register("FoodName", typeof(string), typeof(OrderItem));

        private int GetCount()
        {
            string text = foodCount.Text;

            int.TryParse(text, out int value);
            return value;
        }

        private void HandleInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexValidation.IsNumeric(e.Text);
            totalPrice.Text = (FoodPrice * FoodCount).ToString("N0");
        }

        private void IncreaseCount(object sender, RoutedEventArgs e)
        {
            FoodCount++;
            foodCount.Text = FoodCount.ToString();
            totalPrice.Text = (FoodPrice * FoodCount).ToString("N0");
            UpdateOrderItem();
        }

        private void DecreaseCount(object sender, RoutedEventArgs e)
        {
            if (GetCount() < 2)
                return;
            FoodCount--;
            foodCount.Text = FoodCount.ToString();
            totalPrice.Text = (FoodPrice * FoodCount).ToString("N0");
            UpdateOrderItem();
        }

        private async void UpdateOrderItem()
        {
            using HttpClient client = new HttpClient();

            OrderItemDTO itemDTO = new OrderItemDTO()
            {
                Id = ItemId,
                Count = GetCount(),
                Price = FoodPrice,
                FoodId = FoodId,
                OrderId = OrderId
            };

            string json = JsonConvert.SerializeObject(itemDTO);
            HttpContent content = new StringContent(json,Encoding.UTF8,"application/json");

            string actionUrl = $"{Informations.API_URL}/Order";

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Informations.Token);

            var response = await client.PutAsync(actionUrl,content);

            if (response.IsSuccessStatusCode)
                OrderItemUpdated?.Invoke(this, this);
        }

        private async void DeleteItem(object sender, RoutedEventArgs e)
        {
            using HttpClient client = new HttpClient();

            string actionUrl = $"{Informations.API_URL}/Order/{ItemId}";

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Informations.Token);

            var response = await client.DeleteAsync(actionUrl);

            if (response.IsSuccessStatusCode)
            {
                Visibility = Visibility.Hidden;
                OrderItemUpdated?.Invoke(this,this);
            }
        }

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            TotalPrice = (FoodPrice * FoodCount);
            totalPrice.Text = TotalPrice.ToString("N0");
        }
    }
}
