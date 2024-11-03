using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaDeVotacion.web.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

DotNetEnv.Env.Load();

// Añadir variables de entorno a la configuración
builder.Configuration.AddEnvironmentVariables();

// Configurar VotingService para la inyección de dependencias
builder.Services.AddScoped<VotingService>(provider =>
{
    string rpcUrl = "http://127.0.0.1:7545"; // URL de Ganache
    string contractAddress = "0x50deAC9220BB4ecBb450E6B424e7c11559F10621"; // Dirección de tu contrato
    return new VotingService(rpcUrl, contractAddress);
});

builder.Services.AddDbContext<BlockchainDbContext>(opt => opt.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlockchainDbContext>().AddDefaultTokenProviders();

var app = builder.Build();


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
