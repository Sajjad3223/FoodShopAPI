using System;
using System.Collections.Generic;
using System.Linq;
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
    }

}
