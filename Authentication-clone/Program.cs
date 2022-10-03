using Authentication_clone.Auth;
using Authentication_clone.Db;
using Authentication_clone.Extentions;
using Authentication_clone.Helpers;
using Authentication_clone.ModelServices;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(Log.Logger);
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton(MapperHelper.GetMapper(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"));
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<UsersRepo>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("RedisConfig");
    options.InstanceName = "RedisInstance";
});
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
