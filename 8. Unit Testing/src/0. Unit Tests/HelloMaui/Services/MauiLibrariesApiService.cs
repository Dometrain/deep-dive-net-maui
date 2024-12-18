using Refit;

namespace HelloMaui.Services;

public class MauiLibrariesApiService(IMauiLibraries client)
{
	public Task<List<LibraryModel>> GetMauiLibraries(CancellationToken token) => client.GetMauiLibraries(token);
}

public interface IMauiLibraries
{
	[Get("/default/MauiLibraries")]
	public Task<List<LibraryModel>> GetMauiLibraries(CancellationToken token);
}