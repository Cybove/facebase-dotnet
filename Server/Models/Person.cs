namespace Server.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] MainImage { get; set; }
        public List<Image> Images { get; set; }
    }
}
