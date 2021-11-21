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
    /// Interaction logic for UserAddressWindow.xaml
    /// </summary>
    public partial class UserAddressWindow : Window
    {
        public string Address = string.Empty;

        public UserAddressWindow()
        {
            InitializeComponent();
            addressInput.Text = Address;
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

        private void SubmitAddress(object sender, RoutedEventArgs e)
        {

        }
    }
}
