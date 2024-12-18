using CommunityToolkit.Maui.Alerts;
using HelloMaui.Database;
using HelloMaui.Services;
using Refit;

namespace HelloMaui;

partial class ListViewModel : BaseViewModel
{
	readonly IDispatcher _dispatcher;
	readonly MauiLibrariesApiService _mauiLibrariesApiService;
	readonly LibraryModelDatabase _libraryModelDatabase;

	[ObservableProperty] bool _isSearchBarEnabled = true,
		_isRefreshing = false;

	[ObservableProperty] string _searchBarText = string.Empty;

	public ListViewModel(IDispatcher dispatcher, MauiLibrariesApiService mauiLibrariesApiService, LibraryModelDatabase libraryModelDatabase)
	{
		_dispatcher = dispatcher;
		_mauiLibrariesApiService = mauiLibrariesApiService;
		_libraryModelDatabase = libraryModelDatabase;
	}

	public ObservableCollection<LibraryModel> MauiLibraries { get; } = [];

	[RelayCommand]
	async Task RefreshAction()
	{
		IsSearchBarEnabled = false;

		var minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(1.5));

		var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
		
		var cachedLibraries = await _libraryModelDatabase.GetLibraries(tokenSource.Token).ConfigureAwait(false);

		try
		{
			if (!cachedLibraries.Any())
			{
				cachedLibraries = await _mauiLibrariesApiService.GetMauiLibraries(tokenSource.Token).ConfigureAwait(false);
				await _libraryModelDatabase.InsertAllLibraries(cachedLibraries, tokenSource.Token);
			}
		}
		catch (Exception)
		{
			await _dispatcher.DispatchAsync(() => Toast.Make("Internet Connection Failed").Show());
		}
		finally
		{
			await minimumRefreshTimeTask.ConfigureAwait(false);
		}

		foreach (var library in cachedLibraries)
		{
			if (MauiLibraries.All(x => x.Title != library.Title))
			{
				await _dispatcher.DispatchAsync(() => MauiLibraries.Add(library));
			}
		}

		IsRefreshing = false;
		IsSearchBarEnabled = true;
	}

	[RelayCommand]
	async Task UserStoppedTyping(CancellationToken token)
	{
		var searchText = SearchBarText;

		var existingLibraries = new List<LibraryModel>(MauiLibraries);
		var originalLibraries = await _libraryModelDatabase.GetLibraries(token);

		var distinctLibraries = existingLibraries.Concat(originalLibraries).DistinctBy(x => x.Title);

		await _dispatcher.DispatchAsync(MauiLibraries.Clear).ConfigureAwait(false);

		foreach (var library in distinctLibraries.Where(x => x.Title.Contains(searchText)))
		{
			await _dispatcher.DispatchAsync(() => MauiLibraries.Add(library)).ConfigureAwait(false);
		}
	}
}