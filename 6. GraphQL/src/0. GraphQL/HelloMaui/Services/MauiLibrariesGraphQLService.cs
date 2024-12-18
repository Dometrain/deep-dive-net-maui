using StrawberryShake;

namespace HelloMaui.Services;

class MauiLibrariesGraphQLService(LibrariesGraphQLClient client)
{
	public async IAsyncEnumerable<LibraryModel> GetLibraries([EnumeratorCancellation] CancellationToken token)
	{
		var librariesResponse = await client.GetAllLibraries.ExecuteAsync(token).ConfigureAwait(false);
		librariesResponse.EnsureNoErrors();

		if (librariesResponse.Data?.Libraries is null)
			throw new InvalidOperationException();

		foreach (var library in librariesResponse.Data.Libraries)
		{
			yield return new LibraryModel
			{
				Description = library.Description,
				Title = library.Title,
				ImageSource = library.ImageSource
			};
		}
	}
}