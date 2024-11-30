
using accomondationApp.Utilities;
using Microsoft.EntityFrameworkCore;
using accomondationApp.Repositories;
using Newtonsoft.Json;
using System.Globalization;
using accomondationApp.Interfaces;
using accomondationApp.ViewModel;
using accomondationApp;




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
options.UseSqlServer(DBSettingProvider.ReturnConnectionString()));
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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("EnableCORS");
app.UseStaticFiles();
app.UseRouting();


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

app.MapFallbackToFile("index.html");

app.Run();
