using BlazorRealTimeApp.Application;
using BlazorRealTimeApp.Infrastructure;
using BlazorRealTimeApp.WebUI.Server.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Csak a Server modult adjuk hozz�

// SignalR �s az �rtes�t� szolg�ltat�s regisztr�l�sa
builder.Services.AddSignalR();

// Register services
// Dependency injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration: builder.Configuration);

// CORS be�ll�t�sok hozz�ad�sa
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
    .AddInteractiveServerRenderMode(); // Ezt kell hozz�adni, hogy az interakt�v komponensek m�k�djenek! (pl.: click)


app.Run();
