using DriveForum.Models;

namespace DriveForum.ViewModels
{
    public class CarsForModerator
    {
        public List<Car> Cars { get; set; }
        public List<CarBrand> Brands { get; set; }
        public List<CarEngine> Engines { get; set; }
    }
}
