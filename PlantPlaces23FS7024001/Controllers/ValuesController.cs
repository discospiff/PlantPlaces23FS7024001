using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantPlacesSpecimens;

namespace PlantPlaces23FS7024001.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        public IList<Specimen> Get()
        {
            return SpecimenRepository.allSpecimens;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]

        public Specimen Get(int id)
        {
            return SpecimenRepository.allSpecimens[id];
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            value = value + "";
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id) { 
        }
    }
}
