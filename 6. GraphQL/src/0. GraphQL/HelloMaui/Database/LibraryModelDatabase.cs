using System.Collections.Frozen;

namespace HelloMaui.Database;

class LibraryModelDatabase(IFileSystem fileSystem) : BaseDatabase(fileSystem)
{
	public async Task<FrozenSet<LibraryModel>> GetLibraries(CancellationToken token)
	{
		var response = await Execute<List<LibraryModel>, LibraryModel>(databaseConnection => databaseConnection.Table<LibraryModel>().ToListAsync(), token).ConfigureAwait(false);
		return response.ToFrozenSet();
	}

	public Task InsertAllLibraries(IEnumerable<LibraryModel> libraryModels, CancellationToken token) =>
		Execute<int, LibraryModel>(databaseConnection => databaseConnection.InsertAllAsync(libraryModels), token);
}