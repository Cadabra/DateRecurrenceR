using DateRecurrenceR.Docs;
using DateRecurrenceR.Docs.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped<RecurrenceService>();
builder.Services.AddSingleton<CodeStyleState>();

await builder.Build().RunAsync();
