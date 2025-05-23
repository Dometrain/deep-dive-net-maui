using Refit;

namespace HelloMaui.Services;

class MauiLibrariesApiService(IMauiLibraries client)
{
	public Task<List<LibraryModel>> GetMauiLibraries() => client.GetMauiLibraries();
}

interface IMauiLibraries
{
	[Get("/default/MauiLibraries")]
	Task<List<LibraryModel>> GetMauiLibraries();
}