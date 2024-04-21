namespace DriveForum.Models
{
    public class UserCar
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Car Car { get; set;}
    }
}
