using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public partial class RegisterWindow : Window
    {
        private const string API_URL = "https://localhost:4050/";

        public RegisterWindow()
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
            if (string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text) || string.IsNullOrWhiteSpace(email.Text) ||
                string.IsNullOrWhiteSpace(phoneNumber.Text) || string.IsNullOrWhiteSpace(password.Password) || string.IsNullOrWhiteSpace(confirmedPassword.Password))
            {
                MessageBox.Show("لطفا همه اطلاعات را تکمیل کنید");
                return;
            }

            if (password.Password != password.Password)
            {
                MessageBox.Show("پسورد و تکرار آن مطابقت ندارند");
                return;
            }

            UserDTO user = new UserDTO();
            user.FirstName = firstName.Text;
            user.LastName = lastName.Text;
            user.PhoneNumber = phoneNumber.Text;
            user.Email = email.Text;
            user.Password = password.Password;

            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
                {
                    { nameof(user.FirstName), user.FirstName },
                    { nameof(user.LastName), user.LastName },
                    { nameof(user.PhoneNumber), user.PhoneNumber },
                    { nameof(user.Email), user.Email },
                    { nameof(user.Password), user.Password }
                };

            var content = new FormUrlEncodedContent(values);
            client.PostAsync($"{API_URL}/account/register", content);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
