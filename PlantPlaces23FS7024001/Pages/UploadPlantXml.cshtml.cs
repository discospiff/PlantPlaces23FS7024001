using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Schema;
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

            // Let's Validate this file.
            ValidateXml(file);

        }

        private void ValidateXml(string file)
        {
            // let's define the schema we're going to use to validate our XML.
            XmlReaderSettings settings = new XmlReaderSettings();
            string xsdPath = Path.Combine(environment.ContentRootPath, "uploads", "plants.xsd");
            settings.Schemas.Add(null, xsdPath);

            // we want to validate with XSD
            settings.ValidationType = ValidationType.Schema;

            // set validation flags
            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;

            // if something goes wrong, use ValidationEventHandler method to report it.
            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandler);

            XmlReader xmlReader = XmlReader.Create(file, settings);

            try { 
                while (xmlReader.Read())
                {

                }
                ViewData["UploadParseResult"] = "Validation Passed.";
            } catch (Exception ex)
            {
                ViewData["UploadParseResult"] = ex.Message;
            }
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            throw new Exception ("Validation Failed.  Message: " + args);
        }
    }
}
