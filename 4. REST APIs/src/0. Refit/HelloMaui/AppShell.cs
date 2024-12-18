namespace HelloMaui;

class AppShell : Shell
{
	
	public AppShell(ListPage listPage)
	{
		Items.Add(listPage);

		RegisterRoutes();
	}

	public static string GetRoute<T>() where T : Page
	{
		var pageType = typeof(T);
		
		if (pageType == typeof(ListPage))
		{
			return $"//{nameof(ListPage)}";
		}
		
		if (pageType == typeof(DetailsPage))
		{
			return $"//{nameof(ListPage)}/{nameof(DetailsPage)}";
		}
		
		if (pageType == typeof(CalendarPage))
		{
			return $"//{nameof(ListPage)}/{nameof(CalendarPage)}";
		}

		throw new NotSupportedException($"Page {pageType.FullName} Not Found in Routing Table");
	}
	
	static void RegisterRoutes()
	{
		Routing.RegisterRoute(GetRoute<ListPage>(), typeof(ListPage));
		Routing.RegisterRoute(GetRoute<DetailsPage>(), typeof(DetailsPage));
		Routing.RegisterRoute(GetRoute<CalendarPage>(), typeof(CalendarPage));
	}
}