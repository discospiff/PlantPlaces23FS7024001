using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantPlacesPlants;
using PlantPlacesSpecimens;
using System.Runtime.CompilerServices;

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
                Task<HttpResponseMessage> plantTask = httpClient.GetAsync("https://plantplaces.com/perl/mobile/viewplantsjsonarray.pl?WetTolerant=on");
                Task<HttpResponseMessage> task = httpClient.GetAsync("https://plantplaces.com/perl/mobile/specimenlocations.pl?Lat=39.1455&Lng=-84.509&Range=0.5&Source=location");
                HttpResponseMessage result = task.Result;
                List<Specimen> specimens = new List<Specimen>();
                if (result.IsSuccessStatusCode)
                {
                    Task<string> readString = result.Content.ReadAsStringAsync();
                    string jsonString = readString.Result;
                    specimens = Specimen.FromJson(jsonString);
                }
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
                return waterLovingSpecimens;
            });
        }
    }
}