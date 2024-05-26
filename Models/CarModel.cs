namespace DriveForum.Models
{
    public class CarModel
    {
        public CarModel() { }

        public int Id { get; set; }
        public CarBrand Brand { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }
}