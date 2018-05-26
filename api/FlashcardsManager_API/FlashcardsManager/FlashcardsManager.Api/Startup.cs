using System.IO;
using FlashcardsManager.Api.Services;
using FlashcardsManager.Core.EF;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Options;
using FlashcardsManager.Core.Repositories;
using FlashcardsManager.Core.Repositories.Interfaces;
using FlashcardsManager.Core.Services;
using FlashcardsManager.Core.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace FlashcardsManager.Api
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
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<User>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Config.HOST_URL + "/";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "flashcards";
                    options.ApiSecret = "flashcardsSecret";
                    options.SupportedTokens = SupportedTokens.Jwt;
                });
            //}).AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.Authority = Config.HOST_URL + "/";
            //    options.ApiName = "flashcards";
            //    //        options.ApiSecret = "flashcardsSecret";
            //    //        options.SupportedTokens = SupportedTokens.Jwt;
            //});

            //services.AddAuthentication(scheme .AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = Config.HOST_URL + "/";
            //        options.RequireHttpsMetadata = false;
            //        options.ApiName = "flashcards";
            //        options.ApiSecret = "flashcardsSecret";
            //        options.SupportedTokens = SupportedTokens.Jwt;
            //    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("myPolicy", builder =>
                {
                    // require scope1
                    builder.RequireScope("flashcardsScope");
                });
            });
            //{
            //    options.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
            //    {
            //        policyAdmin.RequireClaim("role", "dataEventRecords.admin");
            //    });
            //options.AddPolicy("admin", policyAdmin =>
            //{
            //    policyAdmin.RequireClaim("role", "admin");
            //});
            //    options.AddPolicy("dataEventRecordsUser", policyUser =>
            //    {
            //        policyUser.RequireClaim("role", "dataEventRecords.user");
            //    });

            //});

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<FilteringServices>();
            services.AddScoped<Seed>();
            services.AddTransient<ILearningService, LearningService>();
            services.Configure<LearningServiceOptions>(Configuration.GetSection("Learning"));
            services.Configure<RoleNamesOptions>(Configuration.GetSection("RoleNames"));
            services.AddTransient<UserServices>();
            services.AddTransient<FlashcardsServices>();
            services.AddTransient<CategoryServices>();
            services.AddTransient<AdministratorServices>();


            services.AddSwaggerGen(c =>
            {
                var apiInfo = new Info();
                Configuration.GetSection("Swagger").Bind(apiInfo);
                var version = apiInfo.Version ?? "no version";
                c.SwaggerDoc(version, apiInfo);
                if (bool.Parse(Configuration.GetSection("Swagger")["DescribeEnumsAsStrings"]))
                    c.DescribeAllEnumsAsStrings();
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "FlashcardsManager.Api.xml");
                c.IncludeXmlComments(xmlPath);

            });
            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = new RewriteOptions()
                .AddRedirectToHttps();
            app.UseRewriter(options);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            Seed.Run(app.ApplicationServices).Wait();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flashcards Manager API");
            });
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
