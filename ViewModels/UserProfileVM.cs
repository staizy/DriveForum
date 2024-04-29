using DriveForum.Models;

namespace DriveForum.ViewModels
{
    public class UserProfileVM
    {
        public User? User { get; set; }
        public List<Car>? Cars { get; set; }
    }
}
