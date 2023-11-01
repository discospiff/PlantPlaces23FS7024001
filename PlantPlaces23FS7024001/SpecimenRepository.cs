using PlantPlacesSpecimens;

namespace PlantPlaces23FS7024001
{
    public class SpecimenRepository
    {
        static SpecimenRepository() {
            allSpecimens = new List<Specimen>();
        }

        public static IList<Specimen> allSpecimens {  get; set; }   

    }
}
