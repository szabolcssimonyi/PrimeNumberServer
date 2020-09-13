namespace PrimeNumber.Extensibility.Dto
{
    /// <summary>
    /// Next prime number result transfer object
    /// </summary>
    public class NextPrimeNumberDto
    {
        /// <summary>
        /// Next prime number
        /// </summary>
        public int NextPrimeNumber { get; set; }

        /// <summary>
        /// Origin number
        /// </summary>
        public int TestNumber { get; set; }
    }
}
