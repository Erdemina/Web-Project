namespace Web_Project.models
{
    public class House
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<string> Features { get; set; }
        public List<HouseImage> Images { get; set; }
    }

    public class HouseImage
    {
        public string Url { get; set; }
    }
}

