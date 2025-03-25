using BlazorRealTimeApp.Application;
using BlazorRealTimeApp.Infrastructure;
using BlazorRealTimeApp.Infrastructure.Hubs;
using BlazorRealTimeApp.WebUI.Server.Components;
using Radzen;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Csak a Server modult adjuk hozz�



// Register services
// Dependency injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration: builder.Configuration);

// Radzen szolg�ltat�sok hozz�ad�sa
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

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

app.MapHub<DataHub>("/datahub");

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
    .AddInteractiveServerRenderMode(); // Ezt kell hozz�adni, hogy az interakt�v komponensek m�k�djenek! (pl.: Articles gombok)


app.Run();
