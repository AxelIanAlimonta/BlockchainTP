using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaDeVotacion.web.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

DotNetEnv.Env.Load();

// A�adir variables de entorno a la configuraci�n
builder.Configuration.AddEnvironmentVariables();

// Configurar VotingService para la inyecci�n de dependencias
builder.Services.AddScoped<VotingService>(provider =>
{
    string rpcUrl = "http://127.0.0.1:7545"; // URL de Ganache
    string contractAddress = "0x79F67915818Ce5Ca7a7134fC86c348c720EF50Ef"; // Direcci�n de tu contrato
    return new VotingService(rpcUrl, contractAddress);
});

builder.Services.AddDbContext<BlockchainDbContext>(opt => opt.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BlockchainDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

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
