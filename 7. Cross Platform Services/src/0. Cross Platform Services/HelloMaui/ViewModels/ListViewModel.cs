using System.Collections.Frozen;
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
	readonly MauiLibrariesGraphQLService _librariesGraphQlService;

	[RelayCommand]
	async Task RefreshAction()
	{
		IsSearchBarEnabled = false;

		var minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(1.5));

		var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));

		var databaseLibraries = await _libraryModelDatabase.GetLibraries(tokenSource.Token).ConfigureAwait(false);

		try
		{
			if (!databaseLibraries.Any())
			{
				await foreach (var library in _librariesGraphQlService.GetLibraries(tokenSource.Token).ConfigureAwait(false))
				{
					if (MauiLibraries.All(x => x.Title != library.Title))
					{
						await _dispatcher.DispatchAsync(() => MauiLibraries.Add(library));
					}
				}

				await _libraryModelDatabase.InsertAllLibraries(MauiLibraries, tokenSource.Token);
			}
		}
		catch (Exception e)
		{
			await _dispatcher.DispatchAsync(() => Toast.Make("Internet Connection Failed").Show(tokenSource.Token)).ConfigureAwait(false);
		}
		finally
		{
			await minimumRefreshTimeTask.ConfigureAwait(false);
			IsRefreshing = false;
			IsSearchBarEnabled = true;
		}
	}

	[ObservableProperty]
	bool _isSearchBarEnabled = true,
		_isRefreshing = false;

	[ObservableProperty] string _searchBarText = string.Empty;

	public ListViewModel(IDispatcher dispatcher,
							MauiLibrariesApiService mauiLibrariesApiService,
							LibraryModelDatabase libraryModelDatabase,
							MauiLibrariesGraphQLService librariesGraphQlService)
	{
		_dispatcher = dispatcher;
		_mauiLibrariesApiService = mauiLibrariesApiService;
		_libraryModelDatabase = libraryModelDatabase;
		_librariesGraphQlService = librariesGraphQlService;
	}

	public ObservableCollection<LibraryModel> MauiLibraries { get; } = [];

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