using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PrimeNumber.Api;
using PrimeNumber.Api.Middleware;
using PrimeNumber.Extensibility;
using PrimeNumber.Extensibility.Converter;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PrimeNumber.Test.Integration
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuration>(Configuration);
            services.AddOptions();
            services.AddAutoMapper(typeof(MapperProfile));
            var configuration = Configuration.Get<Configuration>();
            services.AddHealthChecks().AddRedis(configuration.CacheName);

            services.AddCors(options => options.AddPolicy(name: "CorsPolicy", b =>
            {
                b.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
            }));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Prime number server",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "szabolcs.simonyi@hotmail.com",
                        Name = "Szabolcs Simonyi"
                    },
                    Description = "Test application Infinite Lambda, 13. Sept. 2020"
                });
            });
            services.ConfigureSwaggerGen(options =>
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new AssemblyName[] { currentAssembly.GetName() })
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                .Where(f => File.Exists(f)).ToArray();
                Array.ForEach(xmlDocs, (d) =>
                {
                    options.IncludeXmlComments(d);
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            var configuration = Configuration.Get<Configuration>();
            builder.RegisterModule(new AutofacModule(configuration));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prime number server V1");
                });
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
