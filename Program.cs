using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccesoUsuariosAPI.Data;
using AccesoUsuariosAPI.Dtos;
using AccesoUsuariosAPI.Repositories;
using AccesoUsuariosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// HttpClient para consumir API externa
builder.Services.AddHttpClient();

// Base de datos en memoria
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AccesoUsuariosDb"));

// Validaciones + Servicios + Repositorios
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Autorización
builder.Services.AddAuthorization();

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "AccesoUsuariosAPI",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token así: Bearer TU_TOKEN"
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
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

//  Registro de usuario
app.MapPost("/api/accesousuarios/register", async (
    RegisterUserRequest request,
    IUserService userService,
    IValidator<RegisterUserRequest> validator) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
        return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));

    try
    {
        var result = await userService.RegisterAsync(request);
        return Results.Created($"/api/accesousuarios/{result.Id}", result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Login
app.MapPost("/api/accesousuarios/login", async (
    LoginRequest request,
    IUserService userService,
    IValidator<LoginRequest> validator) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
        return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));

    try
    {
        var result = await userService.LoginAsync(request);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Endpoint externo
app.MapGet("/api/external/posts", async (HttpClient http) =>
{
    var response = await http.GetAsync("https://jsonplaceholder.typicode.com/posts");

    if (!response.IsSuccessStatusCode)
        return Results.StatusCode((int)response.StatusCode);

    var data = await response.Content.ReadAsStringAsync();

    return Results.Ok(data);

}).RequireAuthorization(); 

//Inserta datos endpoint externo
app.MapPost("/api/external/posts", async (HttpClient http, object body) =>
{
    var json = System.Text.Json.JsonSerializer.Serialize(body);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await http.PostAsync(
        "https://jsonplaceholder.typicode.com/posts",
        content
    );

    var result = await response.Content.ReadAsStringAsync();

    return Results.Ok(result);

}).RequireAuthorization();  
app.Run();

public partial class Program { }
