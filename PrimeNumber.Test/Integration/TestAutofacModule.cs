using Autofac;
using Moq;
using PrimeNumber.Extensibility.Interfaces;
using PrimeNumber.Service;
using PrimeNumber.Service.Proxy;
using StackExchange.Redis;

namespace PrimeNumber.Test.Integration
{
    public class TestAutofacModule : Module
    {
        public TestAutofacModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterRepositories(builder);
            RegisterServices(builder);
        }
        protected void RegisterRepositories(ContainerBuilder builder)
        {
            builder.Register(sp =>
           {
               var repository = new Mock<ICacheRepository>();
               repository.Setup(mock => mock.Has(It.IsAny<string>())).Returns(false);
               return repository.Object;
           });
        }

        protected void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<PrimeNumberService>().As<IPrimeNumberService>();
            builder.RegisterType<PrimeNumberCacheProxyService>().As<IPrimeNumberCacheProxyService>();
        }
    }
}
