using HelloMaui.Database;
using HelloMaui.Services;
using HelloMaui.UnitTests.Mocks;

namespace HelloMaui.UnitTests;

class Services
{
	static readonly Lazy<IServiceProvider> _serviceProviderHolder = new(CreateCollection);

	public static IServiceProvider Provider => _serviceProviderHolder.Value;

	static IServiceProvider CreateCollection()
	{
		var container = new ServiceCollection();

		// Services
		container.AddSingleton<IPreferences, MockPreferences>();
		container.AddSingleton<MauiLibrariesApiService>();
		container.AddSingleton<MauiLibrariesGraphQLService>();
		container.AddSingleton<WelcomePreferences>();
		container.AddSingleton<LibraryModelDatabase>();

		// ViewModels
		container.AddTransient<ListViewModel>();
		container.AddTransient<DetailsViewModel>();

		return container.BuildServiceProvider();
	}
}