namespace Introduction.Model
{
    public class Car
    {
        public Guid? Id { get; set; }
        public string? Make { get; set; }

        public string? Model { get; set; }
        public Guid? CarType { get; set; }
        public int? Year { get; set; }
        public int? Mileage { get; set; }
        public string? Description { get; set; }
        public DateOnly? InputDate { get; set; }

        public Car() { }
    }
}
