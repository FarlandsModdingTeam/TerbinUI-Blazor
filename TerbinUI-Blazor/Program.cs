using ElectronNET.API;

Console.WriteLine("Iniciando TerbinUI-Blazor...");

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);
builder.Services.AddElectron();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


Console.WriteLine("Construyendo Programa...");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

Console.WriteLine("Configurando Programa...");

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<TerbinUI_Blazor.Components.App>()
    .AddInteractiveServerRenderMode();

Console.WriteLine("Iniciando Electron...");

app.Lifetime.ApplicationStarted.Register(async () =>
{
    await Electron.WindowManager.CreateBrowserViewAsync();
    var window = await Electron.WindowManager.CreateWindowAsync();
    window.OnClosed += () => { Electron.App.Quit(); };
});

app.Run();