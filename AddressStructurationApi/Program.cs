using AddressStructurationApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services de l'application
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Documentation Swagger
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Utilise Swagger
    app.UseSwaggerConfiguration(app.Environment); 

    app.MapOpenApi();
}


// Ajout du middleware pour la vérification de la clé API
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
