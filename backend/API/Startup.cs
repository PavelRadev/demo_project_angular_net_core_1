using System;
using System.IO;
using System.Reflection;
using System.Text;
using API.Classes.ExceptionFilters;
using DB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        #region DB related configs

        private string DbConnectionString => Configuration.GetConnectionString("DemoDBConnection");

        private bool DbRunMigrationsOnStart =>
            Configuration.GetSection("DbContextSettings").GetValue<bool>("RunMigrationsOnStart");

        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddDbContext<DemoDbContext>(
                options => options.UseNpgsql(
                    DbConnectionString,
                    x => x.MigrationsAssembly("DB.Data")));

            DependenciesRegistrar.Inject(services);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("JwtOptions").GetValue<string>("Issuer"),

                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("JwtOptions").GetValue<string>("Audience"),

                ValidateLifetime = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JwtOptions").GetValue<string>("Key"))),
                ValidateIssuerSigningKey = true,
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(ApiExceptionFilter));
            }).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                o.SerializerSettings.MaxDepth = 100;
            });


            services.AddSwaggerGen(c => {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DemoDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            if (DbRunMigrationsOnStart)
                dbContext.Database.Migrate();
        }
    }
}
