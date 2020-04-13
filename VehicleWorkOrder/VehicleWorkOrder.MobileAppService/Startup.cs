namespace VehicleWorkOrder.MobileAppService
{
    using AutoMapper;
    using Database;
    using Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Models;
    using Newtonsoft.Json.Serialization;
    using Okta.AspNetCore;
    using Serilog;
    using Services;

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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.Configure<ConfigurationService>(options =>
                Configuration.GetSection("ConnectionStrings").Bind(options));

            services.AddAutoMapper(typeof(MapperProfile));

            ConfigureDependencyInjection(services);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            }).AddOktaWebApi(new OktaWebApiOptions()
            {
                OktaDomain = Configuration["OktaServer"]
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkOrders", Version = "v1" });
            });
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.ConfigureExceptionHandler(Log.Logger);
            }
            else
            {
                //app.UseHsts();
                app.ConfigureExceptionHandler(Log.Logger);
            }

            app.UseStaticFiles();

           // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseMvc();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "WorkOrders");
            });
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddDbContextPool<WorkOrderContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:WorkOrderConnection"]));
            services.AddSingleton<IItemRepository, ItemRepository>();

            services.AddTransient<IClaimsService, ClaimsService>();
            services.AddTransient<IWorkOrderService, WorkOrderService>();
            services.AddTransient<ITechnicianService, TechnicianService>();
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IWorkOrderService, WorkOrderService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<ILocationService, LocationService>();
        }
    }
}