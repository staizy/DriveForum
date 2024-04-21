namespace DriveForum.Models
{
    public class CarModel
    {
        public CarModel() { }

        public int Id { get; set; }
        /// <summary>
        /// Айди марки машины
        /// </summary>
        //public int BrandId { get; set; }
        public CarBrand Brand { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        /// <summary>
        /// Айди двигателя машины
        /// </summary>
        //public int EngineId { get; set; }
        public CarEngine Engine { get; set; }
    }
}