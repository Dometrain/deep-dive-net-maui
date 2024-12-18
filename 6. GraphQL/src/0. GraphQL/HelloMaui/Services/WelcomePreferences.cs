namespace HelloMaui.Services;

class WelcomePreferences(IPreferences preferences)
{
	public bool IsFirstRun
	{
		get => preferences.Get(nameof(IsFirstRun), true);
		set => preferences.Set(nameof(IsFirstRun), value);
	}
}