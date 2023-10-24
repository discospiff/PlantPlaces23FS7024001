using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantPlacesPlants;
using PlantPlacesSpecimens;

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

            Task<HttpResponseMessage> task = httpClient.GetAsync("https://plantplaces.com/perl/mobile/specimenlocations.pl?Lat=39.1455&Lng=-84.509&Range=0.5&Source=location");
            HttpResponseMessage result =  task.Result;
            List<Specimen> specimens = new List<Specimen>();    
            if (result.IsSuccessStatusCode)
            {
                Task<string> readString = result.Content.ReadAsStringAsync();
                string jsonString = readString.Result;
                specimens = Specimen.FromJson(jsonString);
            }
            ViewData["Specimens"] = specimens;

            Task<HttpResponseMessage> plantTask = httpClient.GetAsync("https://plantplaces.com/perl/mobile/viewplantsjsonarray.pl?WetTolerant=on");
            HttpResponseMessage plantResult = plantTask.Result;
            Task<string> plantTaskString = plantResult.Content.ReadAsStringAsync();
            string plantJson = plantTaskString.Result;
            List<Plant> plants = Plant.FromJson(plantJson);

        }
    }
}