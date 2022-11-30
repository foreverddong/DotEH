using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using DotEH.Services;
using Microsoft.Extensions.DependencyInjection;
using DotEH.Model;

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
		builder.Services.AddLogging(logging => 
		{
			logging.AddDebug();
		});
#endif
		builder.Services.AddMudServices();
		builder.Services.AddSingleton((sp) => {
			var res = new OptionsStorageService();
            Task.Run(() => {
                res.UpdateFromStorageAsync().Wait();
            }).Wait();
			return res;
        });
		builder.Services.AddScoped<EhSearchingService>();
        builder.Services.AddScoped<HttpClient>((services) =>
        {
            var client = new HttpClient();
            var settings = services.GetService<OptionsStorageService>();
			if (settings.UseEx)
			{
				client.DefaultRequestHeaders.Add("Cookie", settings.Cookies);
			}
			return client;
        });
		builder.Services.AddSingleton<TagStorageService>();
		var app = builder.Build();

		
        return app;
	}
}
