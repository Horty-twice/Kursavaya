namespace LR4.GraphQL.Types
{
    public class CarType
    {
        public long Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public string? Confirmed { get; set; }
    }
}