using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AddressStructurationApi.Configuration
{
    public static class SwaggerConfig
    {
        /// <summary>
        /// Configure la documentation Swagger pour l'API.
        /// </summary>
        /// <param name="services">Collection de services de l'application </param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            // Configuration de Swagger pour la documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Génération de la documentation Swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath); // Assure que Swagger lit les commentaires

                // Titre de la documentation Swagger
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "API de structuration d'adresse",
                    Description = "API qui structure une adresse reçue en paramètre selon la norme ISO 20022"
                });

                // Intègre un test de connexion dans la documentation Swagger
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-API-Key",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Passez la clé API dans l'en-tête."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        }, new string[] {} }
                });
            });
        }

        /// <summary>
        /// Active la documentation Swagger dans le pipeline de l'application
        /// </summary>
        /// <param name="app">Instance de l'application</param>
        /// <param name="env">Environnement d'exécution de l'application</param>
        public static void UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
                app.UseSwagger();
                app.UseSwaggerUI();
        }
    }
}
