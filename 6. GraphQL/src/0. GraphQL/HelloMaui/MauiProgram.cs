using HelloMaui.Database;
using HelloMaui.Handlers;
using HelloMaui.Services;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;
using StrawberryShake;

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
			.ConfigureMauiHandlers(handlers => { handlers.AddHandler<CalendarView, CalendarHandler>(); })
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddRefitClient<IMauiLibraries>()
			.ConfigureHttpClient(static client => client.BaseAddress = new Uri("https://6dhbgfw1de.execute-api.us-west-1.amazonaws.com"))
			.AddStandardResilienceHandler(static options => options.Retry = new MobileHttpRetryStrategyOptions());


		builder.Services.AddLibrariesGraphQLClient()
			.ConfigureHttpClient(static client => client.BaseAddress = new Uri("https://t41fbiwwda.execute-api.us-west-1.amazonaws.com/graphql"),
				static builder => builder.AddStandardResilienceHandler(options => options.Retry = new MobileHttpRetryStrategyOptions()));

		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<MauiLibrariesApiService>();
		builder.Services.AddSingleton<MauiLibrariesGraphQLService>();
		builder.Services.AddSingleton<WelcomePreferences>();
		builder.Services.AddSingleton<LibraryModelDatabase>();
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		builder.Services.AddSingleton<IFileSystem>(FileSystem.Current);

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