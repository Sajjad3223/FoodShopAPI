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
using System.Windows.Shapes;

namespace ResturantProgram
{
    /// <summary>
    /// Interaction logic for UserAddressWindow.xaml
    /// </summary>
    public partial class UserAddressWindow : Window
    {
        public UserAddressWindow()
        {
            InitializeComponent();
            User user = Informations.User;
            firstName.Text = user.FirstName;
            lastName.Text = user.LastName;
            addressInput.Text = user.Address;
            phoneNumber.Text = user.PhoneNumber;
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

        private async void SubmitAddress(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(addressInput.Text))
            {
                MessageBox.Show("لطفا آدرس را پر کنید");
                return;
            }

            try
            {
                UpdateUserDTO user = new UpdateUserDTO()
                {
                    FirstName = firstName.Text,
                    LastName = lastName.Text,
                    PhoneNumber = phoneNumber.Text,
                    Address = addressInput.Text,
                    Email = Informations.User.Email
                };

                var json = JsonConvert.SerializeObject(user);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using HttpClient client = new HttpClient();

                string actionUrl = $"{Informations.API_URL}/Account";

                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", Informations.Token);

                var response = await client.PutAsync(actionUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("مشخصات کاربر با موفقیت به روز شد");
                    Informations.User = new User(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
