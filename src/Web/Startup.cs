using AutoMapper;

using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Extensions;
using ECommerceAPI.Web.Filters;
using ECommerceAPI.Web.Helpers;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAPI.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();

            // Extension method in Infrastructure project
            services.AddInfrastructure(Configuration);

            services.AddMvc(options =>
            {
                options.Filters.Add<JsonExceptionFilter>();
                options.Filters.Add<LinkRewritingFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper(options =>
            {
                options.AddProfile<ResourceMappingProfile>();
                options.AddInfrastructureProfile();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorResponse = new ApiError(context.ModelState);
                    return new BadRequestObjectResult(errorResponse);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseMvc();
        }
    }
}
