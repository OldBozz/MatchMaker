using MatchMakerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MatchMakerMySQLContext>(opt =>
    opt.UseInMemoryDatabase("MatchMaker"));

SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();
connbuilder["Data Source"] = builder.Configuration["ConnectionParams:AzureMySql:Server"];
connbuilder.UserID = builder.Configuration["ConnectionParams:AzureMySql:UserId"];
connbuilder.Password = builder.Configuration["ConnectionParams:AzureMySql:Password"];
connbuilder["Database"] = builder.Configuration["ConnectionParams:AzureMySql:Database"];
Debug.WriteLine(connbuilder.ConnectionString);
builder.Services.AddDbContext<MatchMakerMySQLContext>(opt => opt.UseMySQL(connbuilder.ConnectionString));

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
