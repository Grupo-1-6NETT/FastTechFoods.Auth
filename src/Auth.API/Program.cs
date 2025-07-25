using Auth.API.Filters;
using Auth.Application.DependencyInjection;
using Auth.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Auth.Application.Services.TokenJwt;
using Auth.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddApplicationDI();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.RegisterApplicationServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });


builder.Services.AddControllers(opt => opt.Filters.Add<ExceptionFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FastTechFoods Auth API",
        Version = "v1",
        Description = "API de autenticação para o sistema FastTechFoods"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    c.AddServer(new OpenApiServer
    {
        Url = "/auth",
        Description = "Via Ingress - Prefixo /auth"
    });

    c.AddServer(new OpenApiServer
    {
        Url = "/",
        Description = "Via Local - Sem Prefixo"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.ApplyMigrations();

app.Run();
