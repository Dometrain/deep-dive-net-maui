namespace HelloMaui;

class DetailsPage : BaseContentPage<DetailsViewModel>
{
	readonly WeakEventManager _detailsPageDisappearingEventManager = new();
	
	public DetailsPage(DetailsViewModel detailsViewModel) : base(detailsViewModel)
	{
		this.Bind(ContentPage.TitleProperty,
					getter: (DetailsViewModel vm) => vm.LibraryTitle);
		
		Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
		{
			TextOverride = "List"
		});
		
		Content = new VerticalStackLayout
		{
			Spacing = 12,
			
			Children =
			{
				new Image()
					.Center()
					.Size(250)
					.Bind(Image.SourceProperty,
							getter: static (DetailsViewModel vm) => vm.LibraryImageSource),

				new Label()
					.TextCenter()
					.Center()
					.Font(bold: true, size: 24)
					.Bind(Label.TextProperty,
						getter: static (DetailsViewModel vm) => vm.LibraryTitle),

				new Label()
					.TextCenter()
					.Center()
					.Font(italic: true, size: 16)
					.Bind(Label.TextProperty,
						getter: static (DetailsViewModel vm) => vm.LibraryDescription),
				
				new Button()
					.Text("Back")
					.Bind(Button.CommandProperty,
							getter: static (DetailsViewModel vm) => vm.BackButtonTappedCommand,
							mode: BindingMode.OneTime)
			}
		}.Center()
		 .Padding(12);
	}

	public event EventHandler? DetailsPageDisappearing
	{
		add => _detailsPageDisappearingEventManager.AddEventHandler(value);
		remove => _detailsPageDisappearingEventManager.RemoveEventHandler(value);
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		OnDetailsPageDisappearing();
	}

	void OnDetailsPageDisappearing() => _detailsPageDisappearingEventManager.HandleEvent(this, EventArgs.Empty, nameof(DetailsPageDisappearing));
}