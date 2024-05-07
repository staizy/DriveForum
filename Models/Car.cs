namespace DriveForum.Models
{
    public class Car
    {
        public int Id { get; set; }
        public CarEngine Engine { get; set; }
        public CarModel Model { get; set; }

        public override string ToString()
        {
            return $"{Model.Brand.Name} {Model.Name} {Model.Year} {Engine.Name} {Engine.Capacity}L";
        }
    }
}
