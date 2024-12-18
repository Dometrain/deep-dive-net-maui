namespace HelloMaui;

class App : Application
{
    readonly AppShell _appShell;
    
    public App(AppShell shell)
    {
        _appShell = shell;
        
        Resources.MergedDictionaries.Add(new HelloMaui.Resources.Styles.Colors());
        Resources.MergedDictionaries.Add(new HelloMaui.Resources.Styles.Styles());
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(_appShell);

    protected override void OnStart()
    {
        base.OnStart();
        
        Trace.WriteLine("*****App Started*****");
    }

    protected override void OnResume()
    {
        base.OnResume();
        
        Trace.WriteLine("*****App Resumed*****");
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        
        Trace.WriteLine("*****App Backgrounded*****");
    }
}
