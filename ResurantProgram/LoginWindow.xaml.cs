using DataLayer.DTOs;
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
using System.Windows.Shapes;

namespace ResturantProgram
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private const string API_URL = "https://localhost:4050/";

        public LoginWindow()
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

        private void Register(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            Close();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("لطفا همه اطلاعات را تکمیل کنید");
                return;
            }

            LoginDTO loginDTO = new LoginDTO();

            loginDTO.Email = email.Text;
            loginDTO.Password = password.Password;

            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
                {
                    { nameof(loginDTO.Email), loginDTO.Email },
                    { nameof(loginDTO.Password), loginDTO.Password }
                };

            var content = new FormUrlEncodedContent(values);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "Your Oauth token");

            client.PostAsync($"{API_URL}/account/login", content);
        }
    }
}
