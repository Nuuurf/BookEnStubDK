using Microsoft.OpenApi.Models;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using RestfulApi.Utilities;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7115") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddSignalR()
    .AddHubOptions<DateHub>(options =>
    {
        options.EnableDetailedErrors = true;
    })
    .AddJsonProtocol();

// AddToken services to the container.
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var connection = new SqlConnection(connectionString);
    connection.Open();
    return connection;
});

builder.Services.AddScoped<IBookingData, BookingDataControl>();
builder.Services.AddScoped<ICustomerData, CustomerDataControl>();
builder.Services.AddScoped<IDBCustomer, DBCustomer>();
builder.Services.AddScoped<IDBBooking, DBBooking>();
builder.Services.AddScoped<IJwtDataControl, JwtDataControl>();
builder.Services.AddScoped<IDBRefreshToken, DBRefreshToken>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<DateHub>("/dateHub");

app.MapControllers();

app.Run();
