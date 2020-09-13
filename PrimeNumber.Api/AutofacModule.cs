using Autofac;
using PrimeNumber.Domain.Repository;
using PrimeNumber.Extensibility;
using PrimeNumber.Extensibility.Interfaces;
using PrimeNumber.Service;
using PrimeNumber.Service.Proxy;
using StackExchange.Redis;

namespace PrimeNumber.Api
{
    public class AutofacModule : Module
    {
        protected readonly Configuration configuration;

        public AutofacModule(Configuration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register<IConnectionMultiplexer>(sp =>
            {
                var cacheConfig = ConfigurationOptions.Parse(configuration.CacheName, true);
                cacheConfig.ResolveDns = true;
                cacheConfig.AbortOnConnectFail = false;

                return ConnectionMultiplexer.Connect(cacheConfig);
            }).SingleInstance();

            RegisterRepositories(builder);
            RegisterServices(builder);
        }

        protected void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<RedisCacheRepository>().As<ICacheRepository>();
        }

        protected void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<PrimeNumberService>().As<IPrimeNumberService>();
            builder.RegisterType<PrimeNumberCacheProxyService>().As<IPrimeNumberCacheProxyService>();
        }
    }
}
