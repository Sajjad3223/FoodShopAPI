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
    /// Interaction logic for FoodCard.xaml
    /// </summary>
    public partial class FoodCard : UserControl
    {

        public event EventHandler<FoodCard> OnFoodAddedToCart;

        public FoodCard()
        {
            InitializeComponent();
        }

        public string ImageName;

        private int _foodId;

        public int FoodId
        {
            get { return _foodId; }
            set { _foodId = value; }
        }



        private int _foodCount = 1;

        public int FoodCount 
        {
            get { return _foodCount; }
            set { _foodCount = value; } 
        }


        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(FoodCard));



        public string FoodName
        {
            get { return (string)GetValue(FoodNameProperty); }
            set { SetValue(FoodNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodNameProperty =
            DependencyProperty.Register("FoodName", typeof(string), typeof(FoodCard));




        public int FoodPrice
        {
            get { return (int)GetValue(FoodPriceProperty); }
            set { SetValue(FoodPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodPriceProperty =
            DependencyProperty.Register("FoodPrice", typeof(int), typeof(FoodCard));




        public Visibility UserIsAdmin
        {
            get { return (Visibility)GetValue(UserIsAdminProperty); }
            set { SetValue(UserIsAdminProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserIsAdmin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserIsAdminProperty =
            DependencyProperty.Register("UserIsAdmin", typeof(Visibility), typeof(FoodCard));



        private int GetCount()
        {
            string text = foodCount.Text;

            int.TryParse(text, out int value);
            return value;
        }

        private void HandleInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexValidation.IsNumeric(e.Text);
        }

        private void IncreaseCount(object sender, RoutedEventArgs e)
        {
            FoodCount++;
            foodCount.Text = FoodCount.ToString();
        }

        private void DecreaseCount(object sender, RoutedEventArgs e)
        {
            if (GetCount() < 2)
                return;
            FoodCount--;
            foodCount.Text = FoodCount.ToString();
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            OnFoodAddedToCart?.Invoke(this, this);
        }

        private void EditFood(object sender, RoutedEventArgs e)
        {
            CreateFoodWindow editFood = new CreateFoodWindow(FoodId);
            editFood.foodName.Text = FoodName;
            editFood.foodPrice.Text = FoodPrice.ToString();
            editFood.photoPath.Text = "برای تغییر تصویر غذا عکس دیگری انتخاب کنید";
            editFood.ImageName = ImageName;
            
            editFood.ShowDialog();
            FoodName = editFood.foodName.Text;
            FoodPrice = int.Parse(editFood.foodPrice.Text);
            Image = new BitmapImage(new Uri($"{Informations.FOODS_IMAGE_PATH}{editFood.ImageName}", UriKind.RelativeOrAbsolute));
        }

        private async void DeleteFood(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"آیا از حذف {FoodName} مطمئن هستید؟",
                $"حذف {FoodName}",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No,
                MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
            {
                try
                {
                    using var client = new HttpClient();

                    string actionUrl = $"{Informations.API_URL}/Food/{FoodId}";

                    client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", Informations.Token);

                    var response = await client.DeleteAsync(actionUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        MessageBox.Show("غذا با موفقیت حذف شد");
                        this.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show(await response.Content.ReadAsStringAsync());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

}
