using Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(VbContext)));

  // Enable lazy loading.
  options.UseLazyLoadingProxies();
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Seed data.
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  try
  {
    // Seed Payee data
    SeedData.Initialize(services);

  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB.");
  }
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

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
