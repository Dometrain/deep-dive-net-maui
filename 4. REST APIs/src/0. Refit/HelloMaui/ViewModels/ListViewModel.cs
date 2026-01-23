using CommunityToolkit.Maui.Alerts;
using HelloMaui.Services;
using Refit;

namespace HelloMaui;

partial class ListViewModel : BaseViewModel
{
	readonly IDispatcher _dispatcher;
	readonly MauiLibrariesApiService _mauiLibrariesApiService;

	IReadOnlyList<LibraryModel>? _cachedLibraries;

	public ListViewModel(IDispatcher dispatcher, MauiLibrariesApiService mauiLibrariesApiService)
	{
		_dispatcher = dispatcher;
		_mauiLibrariesApiService = mauiLibrariesApiService;
	}

	public ObservableCollection<LibraryModel> MauiLibraries { get; } = [];

	[ObservableProperty]
	public partial bool IsRefreshing { get; set; } = false;

	[ObservableProperty]
	public partial string SearchBarText { get; set; } = string.Empty;

	[ObservableProperty]
	public partial bool IsSearchBarEnabled { get; private set; } = true;

	[RelayCommand]
	async Task RefreshAction()
	{
		IsSearchBarEnabled = false;

		var minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(1.5));

		try
		{
			_cachedLibraries ??= await _mauiLibrariesApiService.GetMauiLibraries().ConfigureAwait(false);
		}
		catch (ApiException)
		{
			await Toast.Make("Internet Connection Failed").Show();
		}
		finally
		{
			await minimumRefreshTimeTask.ConfigureAwait(false);
		}

		foreach (var library in _cachedLibraries ?? Array.Empty<LibraryModel>())
		{
			if (!MauiLibraries.Any(x => x.Title == library.Title))
			{
				await _dispatcher.DispatchAsync(() => MauiLibraries.Add(library));
			}
		}

		IsRefreshing = false;
		IsSearchBarEnabled = true;
	}

	[RelayCommand]
	async Task UserStoppedTyping()
	{
		var searchText = SearchBarText;

		var existingLibraries = new List<LibraryModel>(MauiLibraries);
		var originalLibraries = _cachedLibraries ?? Array.Empty<LibraryModel>();

		var distinctLibraries = existingLibraries.Concat(originalLibraries).DistinctBy(x => x.Title);

		await _dispatcher.DispatchAsync(MauiLibraries.Clear).ConfigureAwait(false);

		foreach (var library in distinctLibraries.Where(x => x.Title.Contains(searchText)))
		{
			await _dispatcher.DispatchAsync(() => MauiLibraries.Add(library)).ConfigureAwait(false);
		}
	}
}