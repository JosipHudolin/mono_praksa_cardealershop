namespace IntroductionWebAPI
{
    public class Car
    {
        public int Id { get; set; }
        public required string Make { get; set; }

        public required string Model { get; set; }

        public int Year {  get; set; }
        public int Mileage { get; set; }

        public string? Decription { get; set; }
        public DateOnly? InputDate { get; set;  }
    }
}
