using DriveForum.DatabaseContext;
using DriveForum.Models;

namespace DriveForum.ViewModels
{
    public class CarsAndUserPost
    {
        public List<CarBrand>? CarBrands { get; set; }
        public List<CarEngine>? CarEngines { get; set; }
        public List<CarModel>? CarModels { get; set; }
        public List<Car>? Cars { get; set; }
        public int CarId { get; set; }
        public string Title { get; set; }
        public string Main { get; set; }
        public string? MainPhotoUrl { get; set; } = null;
    }
}
