using Refit;

namespace HelloMaui.Services;

class MauiLibrariesApiService(IMauiLibraries client)
{
	public Task<List<LibraryModel>> GetMauiLibraries(CancellationToken token) => client.GetMauiLibraries(token);
}

interface IMauiLibraries
{
	[Get("/default/MauiLibraries")]
	Task<List<LibraryModel>> GetMauiLibraries(CancellationToken token);
}