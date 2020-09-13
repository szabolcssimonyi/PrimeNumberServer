namespace PrimeNumber.Extensibility.Dto
{
    /// <summary>
    /// Is prime number result transfer object
    /// </summary>
    public class IsPrimeNumberDto
    {
        /// <summary>
        /// Origin number
        /// </summary>
        public int TestNumber { get; set; }
        /// <summary>
        /// Is prime calculation result
        /// </summary>
        public bool IsPrimeNumber { get; set; }
    }
}
