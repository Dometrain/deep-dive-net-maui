using System.Collections.Frozen;
using CommunityToolkit.Maui.Alerts;
using HelloMaui.Database;
using HelloMaui.Services;
using Refit;

namespace HelloMaui;

public partial class ListViewModel : BaseViewModel
{
	readonly IDispatcher _dispatcher;
	readonly MauiLibrariesApiService _mauiLibrariesApiService;
	readonly LibraryModelDatabase _libraryModelDatabase;
	readonly MauiLibrariesGraphQLService _librariesGraphQlService;

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