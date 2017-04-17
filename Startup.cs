using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;
using System.Linq;
using mmmsl.RouteConstraints;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System;
using System.Threading.Tasks;
using mmmsl.Settings;

namespace mmmsl
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
                
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.Configure<OpenIdConnectOptions>(options => {
                options.AuthenticationScheme = "Auth0";
                options.Authority = $"https://{Configuration["auth0:domain"]}";
                options.ClientId = Configuration["auth0:clientId"];
                options.ClientSecret = Configuration["auth0:clientSecret"];
                options.AutomaticAuthenticate = false;
                options.AutomaticChallenge = false;
                options.ResponseType = "code";
                options.CallbackPath = new PathString("/signin-auth0");
                options.ClaimsIssuer = "Auth0";
                options.Events = new OpenIdConnectEvents {
                    OnRedirectToIdentityProviderForSignOut = HandleRedirectToIdentityProviderForSignOut
                };
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc();
            services.AddDbContext<MmmslDatabase>(options => options.UseSqlServer(Configuration.GetConnectionString("mmmsl")));
            services.AddOptions();
            services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<OpenIdConnectOptions> oidcOptions, MmmslDatabase database)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseOpenIdConnectAuthentication(oidcOptions.Value);
            
            app.UseMvc(routes => {
                var defaultDivisionId = database.Divisions.FirstOrDefault();

                routes.MapRoute(
                    name: "areaIndexRoute",
                    template: "{area:exists}/{controller=Home}/{id?}",
                    defaults: new { action = "Index", id = defaultDivisionId },
                    constraints: new { id = new DivisionRouteConstraint(database) });

                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "defaultRoute",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "divisionRoute",
                    template: "{divisionId}/{controller=Home}/{action=Index}"
                );
            });
        }

        private Task HandleRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            var logoutUri = $"https://{Configuration["auth0:domain"]}/v2/logout?client_id={Configuration["auth0:clientId"]}";

            var postLogoutUri = context.Properties.RedirectUri;
            if (!string.IsNullOrEmpty(postLogoutUri)) {
                if (postLogoutUri.StartsWith("/")) {
                    var request = context.Request;
                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                }

                logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
            }

            context.Response.Redirect(logoutUri);
            context.HandleResponse();

            return Task.CompletedTask;
        }
    }
}
