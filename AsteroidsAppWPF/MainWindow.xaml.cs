using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;

namespace AsteroidsAppWPF
{
    public partial class MainWindow : Window
    {
        private const string ApiFilePath = "api_key.txt";

        public MainWindow()
        {
            InitializeComponent();

            InitializeApp();

            
        }
        private async void InitializeApp()
        {
            if (File.Exists(ApiFilePath))
            {
                string apiKey = File.ReadAllText(ApiFilePath).Trim();

                if (!string.IsNullOrWhiteSpace(apiKey) && await ValidateApiKey(apiKey))
                {
                    // API klíč je platný, načti hlavní stránku
                    MainFrame.Navigate(new MainPage(MainFrame));
                    return;
                }
            }

            // Pokud API klíč neexistuje nebo není platný, načti stránku pro zadání API
            MainFrame.Navigate(new APIInputPage(MainFrame));
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

    }
}