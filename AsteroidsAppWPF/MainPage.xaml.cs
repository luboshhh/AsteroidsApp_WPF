using Newtonsoft.Json;
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

namespace AsteroidsAppWPF
{
    /// <summary>
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame _mainFrame;

        public MainPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;

            string apiKeyFilePath = "api_key.txt";
            if (File.Exists(apiKeyFilePath))
            {
                string apiKey = File.ReadAllText(apiKeyFilePath);
                LoadHazardousAsteroids(apiKey);
            }

        }

        private async Task LoadHazardousAsteroids(string apiKey)
        {
            try
            {
                await AsteroidService.FetchAndSaveHazardousAsteroids(apiKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public static class AsteroidService
    {
        private const string OutputFilePath = "hazardous_asteroids.json";

        public static async Task FetchAndSaveHazardousAsteroids(string apiKey)
        {
            string apiUrl = $"https://api.nasa.gov/neo/rest/v1/neo/browse?api_key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Chyba při získávání dat z API.");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<NeoFeedResponse>(jsonResponse);

                List<NearEarthObject> hazardousAsteroids = new List<NearEarthObject>();

                

                foreach (var asteroid in data.NearEarthObjects)
                {
                    if (asteroid.IsPotentiallyHazardousAsteroid)
                    {
                        // Filtrujeme pouze první budoucí přiblížení
                        CloseApproachData firstFutureApproach = GetFirstFutureApproach(asteroid.CloseApproachData);
                        if (firstFutureApproach != null)
                        {
                            hazardousAsteroids.Add(new NearEarthObject
                            {
                                Id = asteroid.Id,
                                Name = asteroid.Name,
                                IsPotentiallyHazardousAsteroid = asteroid.IsPotentiallyHazardousAsteroid,
                                EstimatedDiameter = asteroid.EstimatedDiameter,
                                CloseApproachData = new List<CloseApproachData> { firstFutureApproach }
                            });
                        }
                    }
                }

                // Uložení do souboru
                File.WriteAllText(OutputFilePath, JsonConvert.SerializeObject(hazardousAsteroids, Formatting.Indented));
                Console.WriteLine("Nebezpečné asteroidy byly uloženy.");
            }
        }

        private static CloseApproachData GetFirstFutureApproach(List<CloseApproachData> approaches)
        {
            // Předpokládáme, že CloseApproachData je seřazena chronologicky, první přiblížení bude nejbližší budoucí datum
            foreach (var approach in approaches)
            {
                if (DateTime.TryParse(approach.CloseApproachDate, out DateTime closeApproachDate) && closeApproachDate > DateTime.Now)
                {
                    return approach;
                }
            }

            return null; // Pokud žádné budoucí přiblížení nenajdeme
        }
    }

}
