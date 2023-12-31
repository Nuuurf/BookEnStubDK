using Microsoft.OpenApi.Models;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
//builder.Services.AddScoped<IDBBooking>(serviceProvider => {
//    var connection = serviceProvider.GetRequiredService<SqlConnection>(); // Assuming you have a SqlConnection registered

//    // Explicitly specify the constructor to use
//    return ActivatorUtilities.CreateInstance<DBBooking>(serviceProvider, connection);
//});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
