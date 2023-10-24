using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            if (result.IsSuccessStatusCode)
            {
                Task<string> readString = result.Content.ReadAsStringAsync();
                string jsonString = readString.Result;
                List<Specimen> specimens = Specimen.FromJson(jsonString);
            }
        }
    }
}