using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContextBD.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiSharedMed
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });

            });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SharedMed",
                    Description = "SharedMed"
                });
            });


            var connectionString = Configuration["ConnectionString"];


            services.AddDbContext<devsharedmedContext>(
                    options => options.UseSqlServer(connectionString)
            );
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
                app.UseHsts();
            }
            app.UseCors(builder =>
               builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               );


            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api SharedMed");
            });
        }
    }
}
