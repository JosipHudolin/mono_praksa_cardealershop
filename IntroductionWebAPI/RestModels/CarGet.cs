namespace IntroductionWebAPI.RestModels
{
    public class CarGet
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? CarTypeName { get; set; }
        public int? Year { get; set; }
        public int? Mileage { get; set; }
    }
}
