namespace HelloMaui.Database;

class LibraryModelDatabase(IFileSystem fileSystem) : BaseDatabase(fileSystem)
{
	public Task<List<LibraryModel>> GetLibraries(CancellationToken token) => 
		Execute<List<LibraryModel>, LibraryModel>(databaseConnection => databaseConnection.Table<LibraryModel>().ToListAsync(), token);

	public Task InsertAllLibraries(IEnumerable<LibraryModel> libraryModels, CancellationToken token) =>
		Execute<int, LibraryModel>(databaseConnection => databaseConnection.InsertAllAsync(libraryModels), token);
}