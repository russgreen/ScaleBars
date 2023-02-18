using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;


namespace ScaleBars;
public class App : IExternalApplication
{
    // get the absolute path of this assembly
    public static readonly string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

    // class instance
    public static App ThisApp;

    public static UIControlledApplication CachedUiCtrApp;
    public static UIApplication CachedUiApp;
    public static ControlledApplication CtrApp;
    public static Autodesk.Revit.DB.Document RevitDocument;

    private AppDocEvents _appEvents;
    public Result OnStartup(UIControlledApplication application)
    {
        ThisApp = this;
        CachedUiCtrApp = application;
        CtrApp = application.ControlledApplication;

        AddAppDocEvents();

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        RemoveAppDocEvents();

        return Result.Succeeded;
    }

    #region Event Handling
    private void AddAppDocEvents()
    {
        _appEvents = new AppDocEvents();
        _appEvents.EnableEvents();
    }
    private void RemoveAppDocEvents()
    {
        _appEvents.DisableEvents();
    }
    #endregion

}
