using _2TDSPK.Database.Models;
using _2TDSPK.Repository.Interface;
using _2TDSPK.Repository;
using _2TDSPK.Services.CEP;
using _2TDSPK.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using _2TDSPK.Database;
using Microsoft.EntityFrameworkCore;
using _2TDSPK.API.Configuration;
using _2TDSPK.API.Service;
using _2TDSPK.ML;

namespace _2TDSPK.API.Extensions
{
    public static class ServiceColletionsExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.AddScoped<ICEPService, CEPService>();
            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();

            services.AddSingleton<RecommendationEngine>();

            return services;
        }

        public static IServiceCollection AddDbContexts(this IServiceCollection services, APPConfiguration configuration)
        {
            services.AddDbContext<FIAPDBContext>(options =>
            {
                options.UseOracle(configuration.OracleFIAP.Connection);
            });

            services.AddDbContext<MongoDbContext>(options => {
                options.UseMongoDB(configuration.ConnectionStrings.ProductsMongoDb, "2TDSPK");
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, MongoDBRepository<User>>();
            services.AddScoped<IRepository<Product>, MongoDBRepository<Product>>();
            services.AddScoped<IRepository<UserLike>, MongoDBRepository<UserLike>>();

            return services;
        }

        public static IServiceCollection AddRepositoriesMongo(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }

        public  static IServiceCollection AddSwagger(this IServiceCollection services, APPConfiguration configuration)
        {

           services.AddSwaggerGen(swagger =>
            {
                //Adiciona a possibilidade de enviar token para o controller
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                //Codigo para mudar a documentação do Swagger
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = configuration.Swagger.Title,
                    Description = configuration.Swagger.Description,
                    Contact = new OpenApiContact()
                    {
                        Email = configuration.Swagger.Email,
                        Name = configuration.Swagger.Name
                    }
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                swagger.IncludeXmlComments(xmlPath);

            });

            return services;

        }

        public static IServiceCollection AddHealthCheck(this IServiceCollection services, APPConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddOracle(configuration.OracleFIAP.Connection, name: configuration.OracleFIAP.Name)
                .AddMongoDb(configuration.ConnectionStrings.ProductsMongoDb, name: "MONGODB FIAP")
                .AddUrlGroup(new Uri("https://viacep.com.br/"), name: "VIA CEP");

            return services;
        }
    }
}
