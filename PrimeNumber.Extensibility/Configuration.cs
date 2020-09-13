namespace PrimeNumber.Extensibility
{

    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class Configuration
    {
        public string CacheName { get; set; }
        public Range Range { get; set; }
    }
}
