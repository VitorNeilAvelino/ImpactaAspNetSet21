using ExpoCenter.Dominio.Interfaces;
using ExpoCenter.Mvc.Data;
using ExpoCenter.Repositorios.Http;
using ExpoCenter.Repositorios.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ExpoCenter.Mvc
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
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("IdentityConnection")));

            services.AddDbContext<ExpoCenterDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("ExpoCenterConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => { 
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI();

            services.AddControllersWithViews();

            services.AddAuthorization(o => o
                //.AddPolicy("ParticipantesExcluir", p => p
                //    .RequireRole("Gerente")
                //    .RequireClaim("Participantes", "Excluir"))
                .AddPolicy("ParticipantesExcluir", ParticipantesExcluirPolicy)
                );

            services.AddHttpClient<IPagamentoRepositorio, PagamentoRepositorio>(c => 
            {
                c.BaseAddress = new Uri(Configuration.GetSection("Endpoints:ApiExpoCenter").Value.TrimEnd('/') + "/");
                c.DefaultRequestHeaders.Add("Authorization", $"Bearer {Configuration.GetSection("Token").Value}");
            });
        }

        private void ParticipantesExcluirPolicy(AuthorizationPolicyBuilder builder)
        {
            builder.RequireAssertion(c => c.User.IsInRole("Gerente") || c.User.HasClaim("Participantes", "Excluir"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            loggerFactory.AddLog4Net("log4net.config"); // SeriLog, Splunk, Elk.
        }
    }
}