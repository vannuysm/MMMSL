using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using mmmsl.Models;
using mmmsl.RouteConstraints;
using mmmsl.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
                options.Events = new OpenIdConnectEvents {
                    OnRedirectToIdentityProviderForSignOut = HandleRedirectToIdentityProviderForSignOut,
                    OnTicketReceived = HandleTicketReceived
                };
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc();
            services.AddAuthorization(options => {
                options.AddPolicy("CanManageLeague", policy => policy.RequireRole("admin"));
            });
            services.AddDbContext<MmmslDatabase>(options => options.UseSqlServer(Configuration.GetConnectionString("mmmsl")));
            services.AddOptions();
            services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
        }

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

        private async Task HandleTicketReceived(TicketReceivedContext context)
        {
            var identity = context.Principal.Identity as ClaimsIdentity;
            if (identity == null) {
                return;
            }

            if (!identity.HasClaim(claim => claim.Type == "email")) {
                return;
            }

            var database = context.HttpContext.RequestServices.GetService<MmmslDatabase>();
            var email = identity.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

            var roles = await database.Profiles
                .Include(profile => profile.Roles)
                .Where(profile => profile.Email == email)
                .Select(profile => profile.Roles.Select(role =>
                    new Claim("role", role.Name)
                ))
                .SingleOrDefaultAsync();

            if (!roles.Any()) {
                return;
            }

            identity.AddClaims(roles);
        }
    }
}
