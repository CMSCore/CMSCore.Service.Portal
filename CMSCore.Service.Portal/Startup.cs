namespace CMSCore.Service.Portal
{
    using System;
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Hosting;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IClusterClient>(CreateClusterClient);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CMSCore.Service.Portal", Version = "v1" , Description = "Portal service for mananging content in CMSCore"});
            });
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMSCore.Service.Portal");
            });

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetService<ILogger<Startup>>();

            var client = new ClientBuilder()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IManageContentGrain).Assembly).WithCodeGeneration())
                //.Configure<ClusterOptions>(options =>
                //{
                //    options.ClusterId = "orleans-docker";
                //    options.ServiceId = "AspNetSampleApp";
                //})
                .UseLocalhostClustering()
                .Build();

            client.Connect(RetryFilter).GetAwaiter().GetResult();
            return client;

            async Task<bool> RetryFilter(Exception exception)
            {
                log?.LogWarning("Exception while attempting to connect to Orleans cluster: {Exception}", exception);
                await Task.Delay(TimeSpan.FromSeconds(2));
                return true;
            }
        }
    }
}