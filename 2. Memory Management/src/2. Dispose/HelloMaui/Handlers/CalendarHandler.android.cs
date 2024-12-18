using Microsoft.Maui.Handlers;
using Calendar = Android.Widget.CalendarView;

namespace HelloMaui.Handlers;

public partial class CalendarHandler : ViewHandler<ICalendarView, Calendar>
{
	protected override Calendar CreatePlatformView()
	{
		return new Calendar(Context);
	}

	protected override void ConnectHandler(Calendar platformView)
	{
		base.ConnectHandler(platformView);
		platformView.DateChange += HandleDateChanged;
	}

	protected override void DisconnectHandler(Calendar platformView)
	{
		base.DisconnectHandler(platformView);
		platformView.DateChange -= HandleDateChanged;
	}

	static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.FirstDayOfWeek = (int)virtualView.FirstDayOfWeek;
	}
	
	static void MapMinDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.MinDate = virtualView.MinDate.ToUnixTimeMilliseconds();
	}

	static void MapMaxDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.MaxDate = virtualView.MaxDate.ToUnixTimeMilliseconds();
	}
	
	static void MapSelectedDate(CalendarHandler handler, ICalendarView virtualView)
	{
		if (virtualView.SelectedDate is null)
		{
			return;
		}

		handler.PlatformView.SetDate(virtualView.SelectedDate.Value.ToUnixTimeMilliseconds(), true, true);
	}
	
	void HandleDateChanged(object? sender, Calendar.DateChangeEventArgs e)
	{
		PlatformView.DateChange -= HandleDateChanged;
		
		VirtualView.SelectedDate = new DateTime(e.Year, e.Month + 1, e.DayOfMonth, 0, 0, 0);
		VirtualView.OnSelectedDateChanged(VirtualView.SelectedDate);
		
		PlatformView.DateChange += HandleDateChanged;
	}
}