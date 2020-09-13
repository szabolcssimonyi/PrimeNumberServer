using Newtonsoft.Json;
using NUnit.Framework;
using PrimeNumber.Extensibility.Dto;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimeNumber.Test.Integration
{
    public class PrimeNumberControllerTest
    {
        protected TestApplicationFactory factory = new TestApplicationFactory();
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            client = factory.CreateClient();
        }

        [TestCase(100, TestName = "isprime - performace test")]
        public void IsPrimeNumber_Performance_test(int numberOfRequests)
        {
            var tasks = new Task[numberOfRequests];
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < numberOfRequests; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    var next = new Random().Next(100000);
                    var url = $@"/api/primenumber/isprime/{next}";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var value = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IsPrimeNumberDto>(value);
                    Assert.IsNotNull(result);
                });
            }
            Task.WaitAll(tasks);
            watch.Stop();
            Assert.Less(watch.ElapsedMilliseconds, 5000);
        }

        [TestCase(100, TestName = "getnext - performace test")]
        public void NextPrime_Performance_test(int numberOfRequests)
        {
            var tasks = new Task[numberOfRequests];
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < numberOfRequests; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    var next = new Random().Next(100000);
                    var url = $@"/api/primenumber/getnext/{next}";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var value = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<NextPrimeNumberDto>(value);
                    Assert.IsNotNull(result);
                });
            }
            Task.WaitAll(tasks);
            watch.Stop();
            Assert.Less(watch.ElapsedMilliseconds, 5000);
        }
    }
}
