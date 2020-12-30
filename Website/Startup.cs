
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Security.Claims;

namespace Website
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
            var identityServerUrl = "https://localhost:5001";
            services.AddAuthentication(o =>
            {
                //Preverja, ce smo prijavljeni
                o.DefaultAuthenticateScheme = "cookie";
                //Ko je uporabnik vpisan se naredi cookie
                o.DefaultSignInScheme = "cookie";
                //preverja ali lahko prijavljen racun nekaj naredi (authorize)
                o.DefaultChallengeScheme = "oAuth";
            })
                .AddCookie("cookie", options =>
                {

                })
                .AddOAuth("oAuth", options =>
                {
                    //spodnje nastavitve so nastavljene v IdentityServer client-u
                    //kam se preusmeri uspesna prijava
                    options.CallbackPath = "/oauth/callback";

                    //ime klienta registriranega v IdentityServer
                    options.ClientId = "mvc";

                    //geslo klienta registriranega v IdentityServer
                    options.ClientSecret = "secret";

                    //seznam scope-ov, ki jih uporabnik uporablja v okviru prijave
                    options.Scope.Add("mvc.pravica1");
                    options.Scope.Add("openid");

                    //endpointi Identity serverja - so na /.well-known/openid-configuration
                    options.AuthorizationEndpoint = identityServerUrl + "/connect/authorize";
                    options.TokenEndpoint = identityServerUrl + "/connect/token";
                    options.UserInformationEndpoint = identityServerUrl + "/connect/userinfo";

                    //PKCE
                    options.UsePkce = true;

                    //mapiranja podatkov iz openid v User.Claims
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "givenName");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "familyName");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "givenName");
                });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
