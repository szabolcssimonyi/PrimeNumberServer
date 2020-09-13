using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PrimeNumber.Service;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumber.Test.Unit
{
    public class PrimeNumberServiceTest
    {
        private Mock<ILogger<PrimeNumberService>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<PrimeNumberService>>();
        }

        [TestCase(new int[] { 4, 21, 33, 649, 4692, 122 }, TestName = "IsPrimeNumberAsync - test number is not prime number, should return false")]
        public void IsPrimeNumberAsync_number_is_not_prime_should_return_false(int[] values)
        {
            var service = new PrimeNumberService(mockLogger.Object);
            values.ToList().ForEach(value =>
            {
                var result = service.IsPrimeNumberAsync(value).GetAwaiter().GetResult();
                Assert.IsFalse(result);
            });
        }

        [TestCase(new int[] { 2, 13, 157, 1019, 3931, 9973 }, TestName = "IsPrimeNumberAsync - test number is prime number, should return true")]
        public void IsPrimeNumberAsync_number_is_prime_should_return_true(int[] values)
        {
            var service = new PrimeNumberService(mockLogger.Object);
            values.ToList().ForEach(value =>
            {
                var result = service.IsPrimeNumberAsync(value).GetAwaiter().GetResult();
                Assert.IsTrue(result);
            });
        }

        [TestCase(new int[] { 1, 0, -1, -1500 }, TestName = "IsPrimeNumberAsync - test number is below 1, should throw out of range exception")]
        public void IsPrimeNumberAsync_number_is_below_or_equel_one_should_throw_exception(int[] values)
        {
            var service = new PrimeNumberService(mockLogger.Object);
            values.ToList().ForEach(value =>
            {
                var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.IsPrimeNumberAsync(value));
                Assert.AreEqual(exception.Message, "Specified argument was out of the range of valid values. (Parameter 'Prime number')");
            });
        }

        [TestCase(2, 3, TestName = "GetNextPrimeNumberAsync - test number is 2, should return 3")]
        [TestCase(8, 11, TestName = "GetNextPrimeNumberAsync - test number is 8, should return 11")]
        [TestCase(90, 97, TestName = "GetNextPrimeNumberAsync - test number is 90, should return 97")]
        [TestCase(149, 151, TestName = "GetNextPrimeNumberAsync - test number is 149, should return 151")]
        [TestCase(810, 811, TestName = "GetNextPrimeNumberAsync - test number is 810, should return 811")]
        [TestCase(809, 811, TestName = "GetNextPrimeNumberAsync - test number is 809, should return 811")]
        [TestCase(4079, 4091, TestName = "GetNextPrimeNumberAsync - test number is 4079, should return 4091")]
        public async Task GetNextPrimeNumberAsync_number_in_range_should_return_next_number(int testValue, int nextPrimeNumber)
        {
            var service = new PrimeNumberService(mockLogger.Object);
            var result = await service.GetNextPrimeNumberAsync(testValue);

            Assert.AreEqual(result, nextPrimeNumber);
        }

        [TestCase(new int[] { -50, -1, 0, 1 }, TestName = "GetNextPrimeNumberAsync - test number is below or equel 1, should return 2")]
        public void GetNextPrimeNumberAsync_number_below_one_range_should_return_two(int[] values)
        {
            var service = new PrimeNumberService(mockLogger.Object);
            values.ToList().ForEach(value =>
            {
                var result = service.GetNextPrimeNumberAsync(value).GetAwaiter().GetResult();
                Assert.AreEqual(result, 2);
            });
        }


        [TestCase(Int32.MaxValue - 2, TestName = "GetNextPrimeNumberAsync - search overflows, should throw out of range exception")]
        public void GetNextPrimeNumberAsync_search_overflows_should_should_throw_exception(int value)
        {
            var service = new PrimeNumberService(mockLogger.Object);
            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetNextPrimeNumberAsync(value));
            Assert.AreEqual(exception.Message, "Specified argument was out of the range of valid values. (Parameter 'Prime number')");
        }
    }
}
