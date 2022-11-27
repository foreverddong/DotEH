using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using DotEH.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotEH;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		builder.Services.AddMudServices();
		builder.Services.AddSingleton((options) => 
		{ 
			var res = new OptionsStorageService();
			return res;
		});
		builder.Services.AddScoped<EhSearchingService>();
        builder.Services.AddHttpClient<EhSearchingService>((services, client) =>
        {
			var settings = services.GetService<OptionsStorageService>();
            client.BaseAddress = settings.EhBaseAddress;
			if (settings.UseEx)
			{
				client.DefaultRequestHeaders.Add("Cookie", settings.Cookies);
			}
        });
		var app = builder.Build();
		//has to resolve once here for the sake of initialization...
		var optionsInitial = app.Services.GetService<OptionsStorageService>();
		optionsInitial.UpdateFromStorageAsync();
        return app;
	}
}
