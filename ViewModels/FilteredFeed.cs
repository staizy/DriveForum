using DriveForum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DriveForum.ViewModels
{
    public class FilteredFeed
    {
        /*public List<UserPost> UserPosts { get; set; }
        public List<CarBrand> AvailableCarBrands { get; set; }
        public CarBrand CarBrand { get; set; }
        public List<CarModel> AvailableCarModels { get; set; }
        public CarModel CarModel { get; set; }
        public List<CarEngine> AvailableCarEngines { get; set; }
        public CarEngine CarEngine { get; set; }*/

        public List<UserPost> UserPosts { get; set; }
        public List<SelectListItem> AvailableCarBrands { get; set; }
        public string? CarBrand { get; set; }
        public List<SelectListItem> AvailableCarModels { get; set; }
        public string? CarModel { get; set; }
        public List<SelectListItem> AvailableCarEngines { get; set; }
        public string? CarEngine { get; set; }
        public bool? IsModerated { get; set; }
    }
}
