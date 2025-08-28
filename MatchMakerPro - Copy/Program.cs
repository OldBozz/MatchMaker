using Azure.Core.Diagnostics;
using Azure.Identity;
using MatchMakerLib.MatchMakerModel;
using MatchMakerPro.Areas.Identity;
using MatchMakerPro.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using System.Text.Json.Serialization;

//using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();
//DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions()
//{
//    Diagnostics =
//    {
//        LoggedHeaderNames = { "x-ms-request-id" },
//        LoggedQueryParameters = { "api-version" },
//        IsLoggingContentEnabled = true
//    }
//};

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("azure_sqldb"));
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

//// Add services to the container.
//var mysqlIdentityConnectionString = builder.Configuration.GetConnectionString("MySQLIdentityConnection") ?? throw new InvalidOperationException("Connection string 'MySQLIdentityConnection' not found.");
var mysqlMatchmakerConnectionString = builder.Configuration.GetConnectionString("MySQLMatchmakerConnection") ?? throw new InvalidOperationException("Connection string 'MySQLMatchmakerConnection' not found.");

//builder.Services.AddDbContext<MatchMakerIdentityDbContext>(options =>
//    options.UseMySQL(mysqlIdentityConnectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<MatchMakerIdentityDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();


builder.Services.AddDbContext<MatchMakerDbContext>(opt => opt.UseMySQL(mysqlMatchmakerConnectionString));
builder.Services.AddScoped<MatchMakerDbContext>();
builder.Services.AddSingleton<Club>();

builder.Services.AddMudServices();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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

//Initial creation. Migrations dosn't work
//var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
//using (var scope = scopeFactory.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<MatchMakerDbContext>();
//    if (db.Database.EnsureCreated())
//    {
//        if (db.Clubs.Count() < 1)
//            ClubSeedData.Initialize(db);
//        if (db.Players.Count() < 1)
//        {
//            PlayerSeedData.Initialize(db);
//        }
//        if (db.Mainevents.Count() < 1)
//        {
//            MaineventSeedData.Initialize(db);

//        }
//        if (db.Courts.Count() < 1)
//            CourtSeedData.Initialize(db);
//    }
//    //foreach (Player player in db.Players)
//    //{
//    //    player.Clubs.Add(db.Clubs.FirstOrDefault());
//    //}
//    //foreach (Mainevent mainevent in db.Mainevents)
//    //{
//    //    mainevent.Club = db.Clubs.FirstOrDefault();
//    //}

//}

app.Run();
