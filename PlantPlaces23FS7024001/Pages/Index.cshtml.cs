using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PlantPlacesPlants;
using PlantPlacesSpecimens;
using System.Runtime.CompilerServices;
using WeatherFeed;

namespace PlantPlaces23FS7024001.Pages
{
    public class IndexModel : PageModel
    {
        static readonly HttpClient httpClient = new HttpClient();
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            string brand = "My Plant Diary";
            string inBrand = Request.Query["Brand"];
            if (inBrand != null && inBrand.Length > 0) {
                brand = inBrand;
            }
            ViewData["Brand"] = brand;
            Task<List<Specimen>> task = GetData();
            List<Specimen> specimens = task.Result;
            ViewData["Specimens"] = specimens;
        }

        /// <summary>
        /// Fetch our JSON Data
        /// </summary>
        private async Task<List<Specimen>> GetData() {
            return await Task.Run(async () =>
            {

                var config = new ConfigurationBuilder()
                 .AddUserSecrets<Program>()
                 .Build();
                string apikey = config["weatherapikey"];

                Task<HttpResponseMessage> plantTask = httpClient.GetAsync("https://plantplaces.com/perl/mobile/viewplantsjsonarray.pl?WetTolerant=on");
                Task<HttpResponseMessage> specimenTask = httpClient.GetAsync("https://plantplaces.com/perl/mobile/specimenlocations.pl?Lat=39.1455&Lng=-84.509&Range=0.5&Source=location");
                Task<HttpResponseMessage> weatherTask = httpClient.GetAsync("https://api.weatherbit.io/v2.0/current?&city=Cincinnati&country=USA&key=" + apikey);

                HttpResponseMessage result = specimenTask.Result;
                List<Specimen> specimens = new List<Specimen>();
                if (result.IsSuccessStatusCode)
                {
                    Task<string> readString = result.Content.ReadAsStringAsync();
                    string jsonString = readString.Result;

                    // Validate our JSON against a schema.
                    JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("specimenschema.json"));

                    // get the initial array that starts our JSON.
                    JArray jsonArray = JArray.Parse(jsonString);

                    // A collection of strings that will hold any validation errors.
                    IList<string> validationEvents = new List<string>();

                    if (jsonArray.IsValid(schema, out validationEvents))
                    {
                        specimens = Specimen.FromJson(jsonString);
                    }
                    else
                    {
                        // iterate over the error messages
                        foreach (string evt in validationEvents)
                        {
                            Console.WriteLine(evt);
                        }
                    }

                }
                // assign the value of the specimens variable to the ViewData["Specimens"] variable.
                ViewData["Specimens"] = specimens;

                HttpResponseMessage plantResult = await plantTask;
                Task<string> plantTaskString = plantResult.Content.ReadAsStringAsync();
                string plantJson = plantTaskString.Result;
                List<Plant> plants = Plant.FromJson(plantJson);

                IDictionary<long, Plant> waterLovingPlants = new Dictionary<long, Plant>();
                foreach (Plant plant in plants)
                {
                    waterLovingPlants[plant.Id] = plant;
                }
                // make a sub collection of water loving specimens at the Zoo.
                List<Specimen> waterLovingSpecimens = new List<Specimen>();
                // find out which specimens are related to plants that like water.
                foreach (Specimen specimen in specimens)
                {
                    if (waterLovingPlants.ContainsKey(specimen.PlantId))
                    {
                        waterLovingSpecimens.Add(specimen);
                    }
                }
                ViewData["Specimens"] = waterLovingSpecimens;

                HttpResponseMessage weatherResponse = await weatherTask;
                ProcessWeather(weatherResponse);
                return waterLovingSpecimens;
            });
        }

        /// <summary>
        /// Process weather information to find precipitation.  
        /// That will help us to advise our user on watering plants.
        /// </summary>
        /// <param name="weatherResponse">A response from our weather API.</param>
        private void ProcessWeather(HttpResponseMessage weatherResponse)
        {
            Task<string> weatherReadTask = weatherResponse.Content.ReadAsStringAsync();
            string weatherJson = weatherReadTask.Result;

            // parse the weather data
            Weather weather = Weather.FromJson(weatherJson);
            List<Datum> weatherData = weather.Data;
            long precip = 0;

            foreach (Datum datum in weatherData)
            {
                precip = datum.Precip;
            }
            if (precip < 1)
            {
                ViewData["Message"] = "It's dry!  Water these plants.";
            }
            else
            {
                ViewData["Message"] = "Rain expected.  No need to water.";
            }
        }
    }
}