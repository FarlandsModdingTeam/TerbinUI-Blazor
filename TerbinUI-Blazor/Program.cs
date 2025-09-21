using TerbinUI_Blazor.Components;
using ElectronNET.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.WebHost.UseElectron(args);

var app = builder.Build();

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

app.MapRazorComponents<TerbinUI_Blazor.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();


await Task.Run(async () =>
{
    await Electron.WindowManager.CreateBrowserViewAsync(); // Debug // Salta un error: 404
    var window = await Electron.WindowManager.CreateWindowAsync();
    window.OnClosed += () => { Electron.App.Quit(); };
});