using HelloMaui.Resources.Styles;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace HelloMaui;

class MauiLibrariesDataTemplate : DataTemplate
{
	const int imageRadius = 25;
	const int imagePadding = 8;

	public MauiLibrariesDataTemplate() : base(() => CreateTemplate())
	{
	}

	static Grid CreateTemplate() => new()
	{
		RowDefinitions = Rows.Define(
			(Row.Title, 22),
			(Row.Description, 44),
			(Row.BottomPadding, 8)),

		ColumnDefinitions = Columns.Define(
			(Column.Icon, (imageRadius * 2) + (imagePadding * 2)),
			(Column.Text, Star)),

		RowSpacing = 4,

		Children =
		{
			new Image()
				.Row(Row.Title).RowSpan(2)
				.Column(Column.Icon)
				.Center()
				.Aspect(Aspect.AspectFit)
				.Size(imageRadius * 2)
				.Bind(Image.SourceProperty, static (LibraryModel model) => model.ImageSource, mode: BindingMode.OneWay),

			new DataTemplateLabel
				{
					Style = AppStyles.GetResource<Style>("LargeFontLabel")
				}.Row(Row.Title).Column(Column.Text)
				.AppThemeColorBinding(Label.TextColorProperty, Color.FromArgb("#262626"), Color.FromArgb("#c9c9c9"))
				.Font(bold: true)
				.Bind(Label.TextProperty, static (LibraryModel model) => model.Title, mode: BindingMode.OneWay),

			new DataTemplateLabel(12)
				.Row(Row.Description).Column(Column.Text)
				.AppThemeColorBinding(Label.TextColorProperty, Color.FromArgb("#595959"), Color.FromArgb("#b8b6b6"))
				.Bind(Label.TextProperty, static (LibraryModel model) => model.Description, mode: BindingMode.OneWay)
		}
	};

	enum Column
	{
		Icon,
		Text
	}

	enum Row
	{
		Title,
		Description,
		BottomPadding
	}

	class DataTemplateLabel : Label
	{
		public DataTemplateLabel(in double? fontSize = null)
		{
			MaxLines = 2;

			this.Font(size: fontSize)
				.TextTop()
				.TextStart();
		}
	}
}