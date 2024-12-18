namespace HelloMaui;

partial class DetailsViewModel : BaseViewModel, IQueryAttributable
{
	public const string LibraryQueryKey = nameof(LibraryQueryKey);
	
	[ObservableProperty]
	public partial string LibraryTitle { get; private set; } = string.Empty;

	[ObservableProperty]
	public partial string LibraryDescription { get; private set; } = string.Empty;

	[ObservableProperty]
	public partial ImageSource? LibraryImageSource { get; private set; }
	
	[RelayCommand]
	Task BackButtonTapped() => Shell.Current.GoToAsync("..", true);

	void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var library = (LibraryModel)query[LibraryQueryKey];

		LibraryTitle = library.Title;
		LibraryImageSource = library.ImageSource;
		LibraryDescription = library.Description;
	}
}