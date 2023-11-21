using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantPlaces23FS7024001.Pages
{
    public class AutoCompletePlantNamesModel : PageModel
    {
        public JsonResult OnGet(String term)
        {
            IList<string> plantNames = new List<string>();
            plantNames.Add("Redbud");
            plantNames.Add("Red Maple");
            plantNames.Add("Red Oak");
            plantNames.Add("Red Lily");
            plantNames.Add("Red Rose");

            IList<string> matchingPlantNames = new List<string>();

            foreach (string plantName in plantNames)
            {
                if (plantName.Contains(term)) 
                {  
                    matchingPlantNames.Add(plantName); 
                }
            }

            return new JsonResult(matchingPlantNames);
        }
    }
}
