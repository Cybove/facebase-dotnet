namespace Server.Models
{
    public class Image
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public byte[] ImageData { get; set; }
    }
}
