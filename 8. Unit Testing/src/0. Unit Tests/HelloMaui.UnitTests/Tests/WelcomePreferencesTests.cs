using HelloMaui.Services;
using HelloMaui.UnitTests.Mocks;

namespace HelloMaui.UnitTests.Tests;

class WelcomePreferencesTests : BaseTest
{
	public override Task SetUp()
	{
		var preferences = Services.Provider.GetRequiredService<IPreferences>();
		preferences.Clear();
		
		return base.SetUp();
	}

	public override Task TearDown()
	{
		var preferences = Services.Provider.GetRequiredService<IPreferences>();
		preferences.Clear();
		
		return base.TearDown();
	}

	[Test]
	public void IsFirstRun_DefaultValueIsTrue()
	{
		// Arrange
		var welcomePreferences = Services.Provider.GetRequiredService<WelcomePreferences>();

		// Act 

		// Assert
		Assert.That(welcomePreferences.IsFirstRun, Is.True);
	}
	
	[Test]
	public void IsFirstRun_ChangingValueToFalse()
	{
		// Arrange
		var welcomePreferences = Services.Provider.GetRequiredService<WelcomePreferences>();

		// Act 
		welcomePreferences.IsFirstRun = false;

		// Assert
		Assert.That(welcomePreferences.IsFirstRun, Is.False);
	}
	
	[Test]
	public void IsFirstRun_ChangingValueToFalseThenTrue()
	{
		// Arrange
		var welcomePreferences = Services.Provider.GetRequiredService<WelcomePreferences>();

		// Act 
		welcomePreferences.IsFirstRun = false;
		welcomePreferences.IsFirstRun = true;

		// Assert
		Assert.That(welcomePreferences.IsFirstRun, Is.True);
	}
}