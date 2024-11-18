using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace AsteroidsAppWPF
{
    public partial class APIInputPage : Page
    {
        private const string ApiFilePath = "api_key.txt";
        private Frame _mainFrame;

        public APIInputPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private async void SaveApiKey_Click(object sender, RoutedEventArgs e)
        {
            string apiKey = ApiKeyTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Pole API klíče nesmí být prázdné!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (await ValidateApiKey(apiKey))
            {
                File.WriteAllText(ApiFilePath, apiKey);
                MessageBox.Show("API klíč byl úspěšně uložen.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainFrame.Navigate(new MainPage(_mainFrame));
            }
            else
            {
                MessageBox.Show("Neplatný klíč API. Zkontrolujte správnost a zkuste to znovu.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> ValidateApiKey(string apiKey)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://api.nasa.gov/planetary/apod?api_key={apiKey}");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        private void OpenApiLink(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://api.nasa.gov",
                UseShellExecute = true
            });
        }
    }
}
