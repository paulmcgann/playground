using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.OpenIDConnect;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using playground.Extensions;
using playground.Features.Email;

namespace playground
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;

        public Startup(IWebHostEnvironment webHostingEnvironment)
        {
            _webHostingEnvironment = webHostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_webHostingEnvironment.IsDevelopment())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

                services.Configure<SchedulerOptions>(options => options.Enabled = false);
            }

            services
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddAlloy()
                .AddAdminUserRegistration()
                .AddEmbeddedLocalization<Startup>();

            // Required by Wangkanai.Detection
            services.AddDetection();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Content Delivery API
            services.AddContentDeliveryApi()
                .WithFriendlyUrl()
                .WithSiteBasedCors();

            string[] origins = ["https://localhost:3000", "https://localhost:5000"];

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                    {
                        builder
                            .WithOrigins(origins)
                            .WithExposedContentDeliveryApiHeaders()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.ConfigureContentApiOptions(options =>
            {
                options.FlattenPropertyModel = true;
            });
            services.AddContentDeliveryApi(options =>
            {
                options.SiteDefinitionApiEnabled = false;
                options.CorsPolicyName = "CorsPolicy";
            });
            services.AddContentSearchApi(options =>
            {
                options.MaximumSearchResults = 20;
                options.SearchCacheDuration = TimeSpan.FromMinutes(10);
            });

            // Content Delivery Search API
            services.AddContentSearchApi(o =>
            {
                o.MaximumSearchResults = 100;
            });

            // Services registered for Email Service
            services
                .AddFluentEmail("fromEmailGoesHere")
                .AddRazorRenderer()
                .AddSmtpSender("host", 587, "username", "password");

            services.AddSingleton<IEmailService, EmailService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Required by Wangkanai.Detection
            app.UseDetection();
            app.UseSession();


            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContent();
            });
        }
    }
}
