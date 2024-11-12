
using accomondationApp.Utilities;
using Microsoft.EntityFrameworkCore;
using accomondationApp.Repositories;
using Newtonsoft.Json;




var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson(x =>
 x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

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
var app = builder.Build();

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

/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
*/
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
