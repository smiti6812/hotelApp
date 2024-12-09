
using accomondationApp.Utilities;
using Microsoft.EntityFrameworkCore;
using accomondationApp.Repositories;
using Newtonsoft.Json;
using System.Globalization;
using accomondationApp.Interfaces;
using accomondationApp.ViewModel;
using accomondationApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using accomondationApp.AuthModel;




var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


builder.Services.AddControllersWithViews().AddNewtonsoftJson(x =>
 x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
//builder.Services.AddControllers();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationViewProperties, ReservationViewProperties>();
builder.Services.AddScoped<IReservationService>(i => new ReservationService(i.GetRequiredService<IReservationRepository>(),
    i.GetRequiredService<IRoomRepository>()));
builder.Services.AddScoped<IReservationViewWrapper>(ip =>new ReservationViewWrapper(ip.GetRequiredService<IReservationService>(),
    ip.GetRequiredService<IReservationViewProperties>()));
builder.Services.AddScoped<IReservationViewService>(i => new ReservationViewService(i.GetRequiredService<IReservationViewWrapper>()));
builder.Services.AddScoped<ISlideRepository, SlideRepository>();
builder.Services.AddScoped<ISlideService>(i => new SlideService(i.GetRequiredService<ISlideRepository>()));

builder.Services.AddDbContext<accomondationApp.Models.HotelAppDBContext>(options =>
options.UseSqlServer(DBSettingProvider.ReturnDBConnectionString("hotelDBSetting")));

builder.Services.AddDbContext<accomondationApp.AuthModel.UserContext>(opts =>
    opts.UseSqlServer(DBSettingProvider.ReturnDBConnectionString("userDBSetting")));

builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7246",
            ValidAudience = "https://localhost:7246",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_very_long_and_superSecretKey@345"))
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var supportedCultures = new[]
{
    new CultureInfo("hu-HU")
};

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("hu-HU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("EnableCORS");

//app.UseRouting();


//app.MapControllers();

app.MapControllerRoute(
      name: "room",
      pattern: "Reservation/{*ReturnRoom}",
      defaults: "{controller=Reservation}/{action=ReturnRoom}");
app.MapControllerRoute(
      name: "reservation",
      pattern: "reservation/{*get}",
      defaults: "{controller=Reservation}/{action=Get}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

//app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
