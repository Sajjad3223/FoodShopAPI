using DataLayer.DTOs;
using Newtonsoft.Json;
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
        public event EventHandler<UserDTO> OnRegistered;

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

        private async void Register(object sender, RoutedEventArgs e)
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
            user.Roles = new List<string> { "User" };

            using HttpClient client = new HttpClient();

            string json = JsonConvert.SerializeObject(user);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync($"{Informations.API_URL}/Account/register", content);

            if (result.IsSuccessStatusCode)
            {
                MessageBox.Show("حساب شما با موفقیت ساخته شد \n حالا می توانید وارد حساب خود شوید.");
                OnRegistered?.Invoke(this, user);
                Close();
            }
            else
            {
                MessageBox.Show(await result.Content.ReadAsStringAsync());
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
