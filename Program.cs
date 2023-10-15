using BlazBeaver.Data;
using MudBlazor.Services;
using MudBlazor;
using BlazBeaver.Interfaces;
using BlazBeaver.DataAccess;
using BlazBeaver.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

///MudBlazor
builder.Services.AddMudServices();   

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});


builder.Services.AddSingleton<IDataIO, DataIO>();
builder.Services.AddSingleton<DataSourceConverter<Requirement>>();
builder.Services.AddSingleton<IRequirementsRepository, RequirementsRepository>();
builder.Services.AddSingleton<IBasicsSettingsRepository, BasicSettingsRepository>();
builder.Services.AddSingleton<ISearchRequirements, SearchRequirements>();
builder.Services.AddSingleton<IApiClient, ApiClient>();
builder.Services.AddSingleton<IIdProvider, IdProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
