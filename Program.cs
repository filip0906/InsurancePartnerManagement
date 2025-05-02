using System.Data.SqlClient;
using InsurancePartnerManagement;
using InsurancePartnerManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registracija DatabaseContext-a
builder.Services.AddSingleton<DatabaseContext>();

// Registracija repozitorija
builder.Services.AddScoped<PartnerRepository>();
builder.Services.AddScoped<PolicyRepository>();

// Testiraj vezu s bazom
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
try
{
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    Console.WriteLine("Uspješno povezivanje na bazu podataka!");
}
catch (Exception ex)
{
    Console.WriteLine($"Greška prilikom povezivanja na bazu: {ex.Message}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Ukloni HTTPS preusmjeravanje
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Partner}/{action=Index}/{id?}"); // Partner je sada zadani kontroler

app.Run();
