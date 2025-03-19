using BlazorRealTimeApp.Application;
using BlazorRealTimeApp.Infrastructure;
using BlazorRealTimeApp.WebUI.Server.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Csak a Server modult adjuk hozzá

// SignalR és az értesítõ szolgáltatás regisztrálása
builder.Services.AddSignalR();

// Register services
// Dependency injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration: builder.Configuration);

// CORS beállítások hozzáadása
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7105")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Ezt kell hozzáadni, hogy az interaktív komponensek mûködjenek! (pl.: click)


app.Run();
