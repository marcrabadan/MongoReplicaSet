using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbFramework;
using MongoDbFramework.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoReplicaSet.WebApp.Data.Contexts;

namespace MongoReplicaSet.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMongoDbContext<SocialContext>(c =>
            {
                c.Configure(x =>
                {
                    x.Servers = new[]
                    {
                        new MongoServerAddress("mongo-rs-01", 27017),
                        new MongoServerAddress("mongo-rs-02", 27017),
                        new MongoServerAddress("mongo-rs-03", 27017)
                    };
                    x.ConnectionMode = ConnectionMode.ReplicaSet;
                    x.ReplicaSetName = "rs0";
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
