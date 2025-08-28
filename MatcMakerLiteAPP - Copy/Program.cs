using MatcMakerLiteAPP.Areas.Identity;
using MatcMakerLiteAPP.Data;
using MatchMakerLib.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Azure.Identity;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MatchMakerLib.MatchMakerModel;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();
//connbuilder["Data Source"] = builder.Configuration["ConnectionParams:AzureMySql:Server"];
//connbuilder.UserID = builder.Configuration["ConnectionParams:AzureMySql:UserId"];
//connbuilder.Password = builder.Configuration["ConnectionParams:AzureMySql:Password"];
//connbuilder["Database"] = builder.Configuration["ConnectionParams:AzureMySql:IdentityDatabase"];
////string identityconnstr = connbuilder.ConnectionString;
//System.Diagnostics.Debug.WriteLine(connbuilder.ConnectionString);

//string identityconnstr = "augatestmysql.mysql.database.azure.com; UserID = augaadmin; Password = Skimpole1; Database = identity";
string identityconnstr = "Data Source = augatestmysql.mysql.database.azure.com; Initial Catalog = identity; User ID = augaadmin; Password = Skimpole1";
builder.Services.AddDbContext<MyIdentityDbContext>(opt => opt.UseMySQL(identityconnstr));
//connbuilder["Database"] = builder.Configuration["ConnectionParams:AzureMySql:MatchMakerDatabase"];
//string mmconnstr = connbuilder.ConnectionString;
string mmconnstr = "Data Source = augatestmysql.mysql.database.azure.com; Initial Catalog = matchmaker; User ID = augaadmin; Password = Skimpole1";

//builder.Services.AddDbContext<MatchMakerDbContext>((sp,opt) => opt.UseMySQL(mmconnstr).UseInternalServiceProvider(sp));
builder.Services.AddDbContext<MatchMakerDbContext>(opt => opt.UseMySQL(mmconnstr));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<MyIdentityDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
//builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<Tournament>();
builder.Services.AddScoped<MatchMakerDbContext>();
builder.Services.AddMudServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MatchMakerDbContext>();
	if (db.Database.EnsureCreated())
	{
		if(db.Players.Count() < 1)
			PlayerSeedData.Initialize(db);
        if (db.Mainevents.Count() < 1)
            MaineventSeedData.Initialize(db);
        if (db.Courts.Count() < 1)
            CourtSeedData.Initialize(db);
        if (db.Clubs.Count() < 1)
            ClubSeedData.Initialize(db);
    }
    //db.Players.Include(players => players.Teams);
}

app.Run();
