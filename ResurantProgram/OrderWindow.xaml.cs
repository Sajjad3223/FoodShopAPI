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
using System.Windows.Shapes;

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
        }

        private void SubmitAndPayOrder(object sender, RoutedEventArgs e)
        {

        }
    }
}
