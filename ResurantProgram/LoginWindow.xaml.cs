using DataLayer.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ResturantProgram
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public event EventHandler OnLoginSucceeded;

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
            registerWindow.OnRegistered += (s, user) =>
            {
                email.Text = user.Email;
                password.Password = user.Password;
            };

            registerWindow.ShowDialog();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("لطفا همه اطلاعات را تکمیل کنید");
                return;
            }

            LoginDTO loginDTO = new LoginDTO();

            loginDTO.Email = email.Text;
            loginDTO.Password = password.Password;

            using HttpClient client = new HttpClient();

            string json = JsonConvert.SerializeObject(loginDTO);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");


            var result = await client.PostAsync($"{Informations.API_URL}/Account/login", content);

            if (result.IsSuccessStatusCode)
            {
                string contents = await result.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<AuthorizationToken>(contents);
                Informations.Token = token.Token;
                OnLoginSucceeded?.Invoke(this, null);
                MessageBox.Show("ورود با موفقیت انجام شد");
                Close();
            }
        }


    }

    public class AuthorizationToken
    {
        public string Token { get; set; }
    }
}
