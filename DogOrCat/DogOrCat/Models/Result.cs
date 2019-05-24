namespace DogOrCat.Models
{
    public class Result
    {
        public bool IsDog { get; set; }

        public bool IsCat { get; set; }

        public float Confidence { get; set; }

        public byte[] PhotoBytes { get; set; }
    }
}