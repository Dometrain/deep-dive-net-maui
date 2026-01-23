namespace HelloMaui;

class LibraryModel
{
	public string Title { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public string ImageSource { get; init; } = "appicon";

	public override string ToString() => $"""
	                                      {nameof(Title)}: {Title}
	                                      {nameof(Description)}: {Description}
	                                      """;
}