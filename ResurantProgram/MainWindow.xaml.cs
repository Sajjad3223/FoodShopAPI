using ResturantProgram;
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
            //LoginWindow loginWindow = new LoginWindow();
            UserAddressWindow addressWindow = new UserAddressWindow();
            grayPanel.Visibility = Visibility.Visible;
            addressWindow.ShowDialog();


        }
    }
}
