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
    /// Interaction logic for PageButton.xaml
    /// </summary>
    public partial class PageButton : UserControl
    {
        public event EventHandler<int> ChangePage;

        public PageButton()
        {
            InitializeComponent();
        }

        public string PageNumber
        {
            get { return (string)GetValue(PageNumberProperty); }
            set { SetValue(PageNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNumberProperty =
            DependencyProperty.Register("PageNumber", typeof(string), typeof(PageButton));

        private void ChangePageClick(object sender, RoutedEventArgs e)
        {
            int pageNumber = int.Parse(PageNumber);
            ChangePage?.Invoke(this, pageNumber);
        }
    }
}
