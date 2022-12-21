
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NajlONline.Models;
using NajlONline.Services;
using NajlONline.Services.Interfaces;
using NajlONlineData.DTOs.Profiles;
using NajlONlineServices;
using NajlONlineServices.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NajlONline
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NajlONline", Version = "v1" });
            });

           
          
            var key = Configuration["JWT:Key"].ToString();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });

          

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            services.AddDbContext<DataBaseContext>();
            services.AddScoped<IKorisnikService, KorisnikService>();
            services.AddScoped<IAdresaService, AdresaService>();
            services.AddScoped<IKarticaService, KarticaService>();
            services.AddScoped<IKategorijaService, KategorijaService>();
            services.AddScoped<IKupovinaService, KupovineService>();
            services.AddScoped<IProizvodService, ProizvodService>();
            services.AddScoped<ISezonaService, SezonaService>();
            services.AddScoped<IUlogaService, UlogaService>();
            services.AddScoped<IVrstaProizvodaService, VrstaProizvodaService>();
            services.AddScoped<IJWTAuth, Auth>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.ApiKey = "sk_test_51L6ZWlGAfZ7v7CsZrkwMNpMl3Djn2VIHl23LDvJAP4bMR0eSs11EMhq0LenBI4NtDFRGw66rhDxgsb8aVg92AfEg00VDeFwBAr";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NajlONline v1"));
            }

            app.UseAuthentication();

           // app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
