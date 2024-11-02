var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar VotingService para la inyección de dependencias
builder.Services.AddScoped<VotingService>(provider =>
{
    string rpcUrl = "http://127.0.0.1:7545"; // URL de Ganache
    string contractAddress = "0x79F67915818Ce5Ca7a7134fC86c348c720EF50Ef"; // Dirección de tu contrato
    return new VotingService(rpcUrl, contractAddress);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
