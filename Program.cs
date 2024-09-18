using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;
using RapidRescue.Data.Seeders;
using RapidRescue.Filters;
using RapidRescue.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RapidRescueContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;                 
    options.Cookie.IsEssential = true;              
});
// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register the filter as a service
builder.Services.AddScoped<IsAdminLoggedIn>();

builder.Services.AddHttpClient();

builder.Services.AddTransient<EmailService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RapidRescueContext>();
    RolesSeeder.SeedRoles(dbContext);
    UsersSeeder.SeedUsers(dbContext);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
