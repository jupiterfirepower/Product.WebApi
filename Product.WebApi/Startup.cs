using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Product.WebApi.DataAccess;
using Product.WebApi.Repository;
using Product.WebApi.Services;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApiContrib.Core.Formatter.Bson;
using WebApiContrib.Core.Formatter.MessagePack;
using WebApiContrib.Core.Formatter.Protobuf;
using WebApiContrib.Core.Formatter.Yaml;
using WebApiContrib.Core.Formatter.Csv;
using WebApiContrib.Core.Formatter.PlainText;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Product.WebApi.Mappings;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http;

namespace Product.WebApi
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add framework services.
            services.AddMvc()
                .AddCsvSerializerFormatters()
                .AddPlainTextFormatters()
                .AddBsonSerializerFormatters()
                .AddProtobufFormatters()
                .AddMessagePackFormatters()
                .AddYamlFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(a =>
                    a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            var logFactory = new LoggerFactory();
            logFactory.AddProvider(new SqliteLoggerProvider());

            services.AddDbContext<ProductsContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(logFactory));

            // Start Registering and Initializing AutoMapper

            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
            services.AddAutoMapper();

            //using Dependency Injection
            services.AddScoped<ProductsContext, ProductsContext>();
            services.AddTransient<IUnitOfWork<ProductsContext>, UnitOfWork<ProductsContext>>();
            services.AddTransient<IRepository<Models.Product, ProductsContext>, Repository<Models.Product, ProductsContext>>();
            services.AddTransient<IRepository<Models.ProductOwner, ProductsContext>, Repository<Models.ProductOwner, ProductsContext>>();
            services.AddTransient<IRepository<Models.Manufacturer, ProductsContext>, Repository<Models.Manufacturer, ProductsContext>>();
            services.AddTransient<IRepository<Models.User, ProductsContext>, Repository<Models.User, ProductsContext>>();
            //services.AddSingleton<IProductsService, ProductsService>();
            services.AddScoped<IProductsService, ProductsService>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                /*options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };*/
                options.EnableForHttps = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Priduct Web API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //var options = new RewriteOptions()
            //    .AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 44338);

            //app.UseRewriter(options);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
            });

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
