using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PlantPlaces23FS7024001.Pages
{
    public class UploadPlantXmlModel : PageModel
    {
        private readonly IHostingEnvironment environment;

        public UploadPlantXmlModel(IHostingEnvironment iHostingEnvironment) {
            this.environment = iHostingEnvironment;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        public void OnGet()
        {
        }
        public void OnPost() {
            // Let's save this uploaded file to our disk, on the server side.
            string fileName = Upload.FileName;
            // Let's make a full path where we can save this file.
            string file = Path.Combine(environment.ContentRootPath, "uploads", fileName);

            using (var fileStream = new FileStream(file, FileMode.Create)) {
                Upload.CopyTo(fileStream);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode? xmlNode = doc.SelectSingleNode("/plant/genus");
            int foo = 1 + 1;

        }

    }
}
