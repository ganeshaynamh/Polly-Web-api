using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Swashbuckle.AspNetCore.Swagger;
using WebApiDemo.Models;

namespace WebApiDemo
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
            // add swagger by jagdish

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("s1", new Info { Title = "web Api", Description = "web api information" });

                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"WebApiDemo.xml";
                c.IncludeXmlComments(xmlPath);
            });


            //services.AddDbContext<DemoDbContext>(options => 
            //                      options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"),
            //                      sqlServerOptionsAction: sqlOptions => 
            //                      {
            //                          sqlOptions.EnableRetryOnFailure(
            //                          maxRetryCount: 2,
            //                          maxRetryDelay: TimeSpan.FromSeconds(30),
            //                          errorNumbersToAdd: null);
            //                      }
            //                      )
            //                      );

            services.AddDbContext<DemoDbContext>(
                        options => options.UseSqlServer(
                        "Data Source=NIDHI-0044;database=DemoData;Trusted_Connection=true",
                                providerOptions => providerOptions.EnableRetryOnFailure(
                                    maxRetryCount: 2, 
                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                    errorNumbersToAdd: null

                                    )));

            //polly retry policy
            services.AddHttpClient("csharpcorner").SetHandlerLifetime(TimeSpan.FromMinutes(1)).AddPolicyHandler(GetRetryPolicy());

            services.AddScoped<IDemoModel<DemoModel>, DemoRepository>();
            //services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("TodoList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().
                OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound).
                WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/s1/swagger.json", "web Api");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc(); 
        }
      
    }
}
