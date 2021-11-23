using DataLayer.DTOs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ResturantProgram
{
    /// <summary>
    /// Interaction logic for CreateFoodWindow.xaml
    /// </summary>
    public partial class CreateFoodWindow : Window
    {
        private string _selectedImage = null;

        private int _foodId = 0;
        public string ImageName = string.Empty;
        public CreateFoodWindow(int foodId = 0)
        {
            InitializeComponent();
            _foodId = foodId;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HandleInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexValidation.IsNumeric(e.Text);
        }

        private void ChooseFoodImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.PNG;*.BMP;*.JPG;*.GIF)|*.PNG;*.BMP;*.JPG;*.GIF";
            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(fileDialog.FileName))
            {
                _selectedImage  = photoPath.Text = fileDialog.FileName;
            }
        }

        private async Task<string> UploadFoodImage()
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedImage))
                {
                    return string.Empty;
                }

                using (Stream fileStream = new FileStream(_selectedImage, FileMode.Open))
                {
                    HttpContent imageFile = new StreamContent(fileStream);

                    using var client = new HttpClient();

                    if (!string.IsNullOrEmpty(ImageName))
                    {
                        string deleteUrl = $"{Informations.API_URL}/Food/DeleteImage/{ImageName}";
                        await client.DeleteAsync(deleteUrl);
                    }

                    using var formData = new MultipartFormDataContent();

                    formData.Add(imageFile, "image",_selectedImage);
                    string actionUrl = $"{Informations.API_URL}/Food/UploadImage";
                    var response = await client.PostAsync(actionUrl, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        MessageBox.Show(await response.Content.ReadAsStringAsync());
                    }
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.InnerException);
                return string.Empty;
            }
        }

        private async void InsertFood(object sender, RoutedEventArgs e)
        {
            try
            {
                string imageName = await UploadFoodImage();

                if (string.IsNullOrEmpty(imageName) && !string.IsNullOrEmpty(ImageName))
                    imageName = ImageName;
                else if (string.IsNullOrEmpty(imageName) && string.IsNullOrEmpty(ImageName))
                {
                    MessageBox.Show("لطفا تصویری برای غذا انتخاب کنید");
                    return;
                }

                CreateFoodDTO foodDto = new CreateFoodDTO
                {
                    FoodName = foodName.Text,
                    Price = int.Parse(foodPrice.Text),
                    ImageName = imageName
                };

                ImageName = imageName;

                using var client = new HttpClient();

                string json = JsonConvert.SerializeObject(foodDto);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Informations.Token);

                string actionUrl = $"{Informations.API_URL}/Food";
                HttpResponseMessage response;
                if(_foodId != 0)
                    response = await client.PutAsync($"{actionUrl}/{_foodId}", content);
                else
                    response = await client.PostAsync(actionUrl, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    MessageBox.Show("غذا با موفقیت ثبت شد");
                    Close();
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("برای ثبت غذا باید وارد حساب خود شوید.");
                    LoginWindow login = new LoginWindow();
                    login.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
            }
        }
    }
}
