using Introduction.Model;

namespace IntroductionWebAPI.Models
{
    public class CarCreate
    {
        public string? Make { get; set; }

        public string? Model { get; set; }
        public CarType? CarType { get; set; }
        public int? Year { get; set; }
        public int? Mileage { get; set; }
        public string? Description { get; set; }
    }
}
