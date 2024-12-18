using HelloMaui.Handlers;

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

		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<AppShell>();

		builder.Services.AddTransient<CalendarPage>();
		builder.Services.AddTransient<ListPage, ListViewModel>();
		builder.Services.AddTransient<DetailsPage, DetailsViewModel>();

		return builder.Build();
	}
}
