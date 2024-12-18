namespace HelloMaui.Resources.Styles;

static class AppStyles
{
	public static T GetResource<T>(string resourceName)
	{
		if (Application.Current?.Resources.TryGetValue(resourceName, out var resource) is true)
		{
			return (T)resource;
		}

		foreach (var resourceDictionary in Application.Current?.Resources.MergedDictionaries ?? Array.Empty<ResourceDictionary>())
		{
			if (resourceDictionary.TryGetValue(resourceName, out var resourceValue))
			{
				return (T)resourceValue;
			}
		}

		throw new KeyNotFoundException($"Resource {resourceName} not found");
	}
}