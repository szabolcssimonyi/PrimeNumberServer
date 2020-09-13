using Moq;
using NUnit.Framework;
using PrimeNumber.Extensibility.Interfaces;
using PrimeNumber.Service.Proxy;
using System.Threading.Tasks;

namespace PrimeNumber.Test.Unit
{
    public class PrimeNumberCacheProxyServiceTest
    {
        private Mock<ICacheRepository> mockRepository;
        private Mock<IPrimeNumberService> mockPrimeNumberService;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<ICacheRepository>();
            mockPrimeNumberService = new Mock<IPrimeNumberService>();
        }

        [TestCase(TestName = "IsPrimeNumberAsync - test number is in the cache, return cached value, prime number service never called")]
        public async Task IsPrimeNumberAsync_value_cached_should_return_service_never_called()
        {
            var testValue = 5;
            var key = $@"{testValue}_is_prime";
            var service = new PrimeNumberCacheProxyService(mockRepository.Object, mockPrimeNumberService.Object);

            mockRepository.Setup(mock => mock.Has(key)).Returns(true);
            mockRepository.Setup(mock => mock.GetAsync<bool>(key)).Returns(Task.FromResult(true));
            var result = await service.IsPrimeNumberAsync(testValue);
            Assert.IsTrue(result);
            mockRepository.Verify(mock => mock.Has(key), Times.Once);
            mockRepository.Verify(mock => mock.GetAsync<bool>(key), Times.Once);
            mockPrimeNumberService.Verify(mock => mock.IsPrimeNumberAsync(It.IsAny<int>()), Times.Never);

            mockRepository = new Mock<ICacheRepository>();
            mockPrimeNumberService = new Mock<IPrimeNumberService>();
            service = new PrimeNumberCacheProxyService(mockRepository.Object, mockPrimeNumberService.Object);
            mockRepository.Setup(mock => mock.Has(key)).Returns(true);
            mockRepository.Setup(mock => mock.GetAsync<bool>(key)).Returns(Task.FromResult(false));
            result = await service.IsPrimeNumberAsync(testValue);
            Assert.IsFalse(result);
            mockRepository.Verify(mock => mock.Has(key), Times.Once);
            mockRepository.Verify(mock => mock.GetAsync<bool>(key), Times.Once);
            mockPrimeNumberService.Verify(mock => mock.IsPrimeNumberAsync(It.IsAny<int>()), Times.Never);
            mockPrimeNumberService.Verify(mock => mock.GetNextPrimeNumberAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(TestName = "IsPrimeNumberAsync - test number is in the cache, return cached value false, prime number service never called")]
        public async Task IsPrimeNumberAsync_value_not_cached_should_return_service_should_be_called_once()
        {
            var testValue = 5;
            var key = $@"{testValue}_is_prime";
            var service = new PrimeNumberCacheProxyService(mockRepository.Object, mockPrimeNumberService.Object);

            mockRepository.Setup(mock => mock.Has(key)).Returns(false);
            mockPrimeNumberService.Setup(mock => mock.IsPrimeNumberAsync(testValue)).Returns(Task.FromResult(true));
            var result = await service.IsPrimeNumberAsync(testValue);
            Assert.IsTrue(result);
            mockRepository.Verify(mock => mock.Has(key), Times.Once);
            mockRepository.Verify(mock => mock.GetAsync<bool>(key), Times.Never);
            mockPrimeNumberService.Verify(mock => mock.IsPrimeNumberAsync(It.IsAny<int>()), Times.Once);

            mockRepository = new Mock<ICacheRepository>();
            mockPrimeNumberService = new Mock<IPrimeNumberService>();
            service = new PrimeNumberCacheProxyService(mockRepository.Object, mockPrimeNumberService.Object);
            mockRepository.Setup(mock => mock.Has(key)).Returns(false);
            mockPrimeNumberService.Setup(mock => mock.IsPrimeNumberAsync(testValue)).Returns(Task.FromResult(false));
            result = await service.IsPrimeNumberAsync(testValue);
            Assert.IsFalse(result);
            mockRepository.Verify(mock => mock.Has(key), Times.Once);
            mockRepository.Verify(mock => mock.GetAsync<bool>(key), Times.Never);
            mockPrimeNumberService.Verify(mock => mock.IsPrimeNumberAsync(testValue), Times.Once);
            mockPrimeNumberService.Verify(mock => mock.GetNextPrimeNumberAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(TestName = "GetNextPrimeNumberAsync - test number is in the cache, return cached value, prime number service never called")]
        public async Task GetNextPrimeNumberAsync_value_cached_should_return_service_never_called()
        {
            var testValue = 5;
            var testResult = 7;
            var key = $@"{testValue}_next";
            var service = new PrimeNumberCacheProxyService(mockRepository.Object, mockPrimeNumberService.Object);

            mockRepository.Setup(mock => mock.Has(key)).Returns(true);
            mockRepository.Setup(mock => mock.GetAsync<int>(key)).Returns(Task.FromResult(testResult));
            var result = await service.GetNextPrimeNumberAsync(testValue);
            Assert.AreEqual(result, testResult);
            mockRepository.Verify(mock => mock.Has(key), Times.Once);
            mockRepository.Verify(mock => mock.GetAsync<int>(key), Times.Once);
            mockPrimeNumberService.Verify(mock => mock.GetNextPrimeNumberAsync(It.IsAny<int>()), Times.Never);
            mockPrimeNumberService.Verify(mock => mock.IsPrimeNumberAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(TestName = "GetNextPrimeNumberAsync - test number is not in the cache, return value from service, cache get never called")]
        public async Task GetNextPrimeNumberAsync_value_not_cached_should_return_service_should_be_called_once()
        {
            var testValue = 5;
            var testResult = 7;
            var key = $@"{testValue}_next";
            var service = new PrimeNumberCacheProxyService(mockRepository.Object, mockPrimeNumberService.Object);

            mockRepository.Setup(mock => mock.Has(key)).Returns(false);
            mockPrimeNumberService.Setup(mock => mock.GetNextPrimeNumberAsync(testValue)).Returns(Task.FromResult(testResult));
            var result = await service.GetNextPrimeNumberAsync(testValue);
            Assert.AreEqual(result, testResult);
            mockRepository.Verify(mock => mock.Has(key), Times.Once);
            mockRepository.Verify(mock => mock.GetAsync<int>(key), Times.Never);
            mockPrimeNumberService.Verify(mock => mock.GetNextPrimeNumberAsync(testValue), Times.Once);
            mockPrimeNumberService.Verify(mock => mock.IsPrimeNumberAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
