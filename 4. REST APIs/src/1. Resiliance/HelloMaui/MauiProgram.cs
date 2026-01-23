using HelloMaui.Handlers;
using HelloMaui.Services;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;

namespace HelloMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddHandler<CalendarView, CalendarHandler>();
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddRefitClient<IMauiLibraries>()
			.ConfigureHttpClient(client => client.BaseAddress = new Uri("https://6dhbgfw1de.execute-api.us-west-1.amazonaws.com"))
			.AddStandardResilienceHandler(options => options.Retry = new MobileHttpRetryStrategyOptions());

		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<MauiLibrariesApiService>();

		builder.Services.AddTransient<CalendarPage>();
		builder.Services.AddTransient<ListPage, ListViewModel>();
		builder.Services.AddTransient<DetailsPage, DetailsViewModel>();

		return builder.Build();
	}

	sealed class MobileHttpRetryStrategyOptions : HttpRetryStrategyOptions
	{
		public MobileHttpRetryStrategyOptions()
		{
			BackoffType = DelayBackoffType.Exponential;
			MaxRetryAttempts = 3;
			UseJitter = true;
			Delay = TimeSpan.FromSeconds(1.5);
		}
	}
}