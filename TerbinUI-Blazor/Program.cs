using ElectronNET.API;
using ElectronNET.API.Entities;

Console.WriteLine("(Terbin-UI: Program.cs): Iniciando TerbinUI-Blazor...");

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseElectron(args);
builder.Services.AddElectron();

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


Console.WriteLine("(Terbin-UI: Program.cs): Construyendo Programa...");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

Console.WriteLine("(Terbin-UI: Program.cs): Configurando Programa...");

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<TerbinUI_Blazor.Components.App>()
    .AddInteractiveServerRenderMode();

Console.WriteLine("(Terbin-UI: Program.cs): Iniciando Electron...");

app.Lifetime.ApplicationStarted.Register(async () =>
{
    await Electron.WindowManager.CreateBrowserViewAsync();
    var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
    {
        WebPreferences = new WebPreferences
        {
            ContextIsolation = false,
            NodeIntegration = false
        }
    });
    window.OnClosed += () =>
    {
        Console.WriteLine("(Terbin-UI: Program.cs): Saliendo de Terbin-UI...");
        Electron.App.Quit();
    };
});

app.Run();