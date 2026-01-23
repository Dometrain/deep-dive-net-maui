using Microsoft.Maui.Handlers;

namespace HelloMaui.Handlers;

partial class CalendarHandler : ViewHandler<ICalendarView, View>
{
	protected override View CreatePlatformView()
	{
		throw new NotSupportedException();
	}

	static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView virtualView)
	{
		throw new NotSupportedException();
	}

	static void MapMinDate(CalendarHandler handler, ICalendarView virtualView)
	{
		throw new NotSupportedException();
	}

	static void MapMaxDate(CalendarHandler handler, ICalendarView virtualView)
	{
		throw new NotSupportedException();
	}

	static void MapSelectedDate(CalendarHandler handler, ICalendarView virtualView)
	{
		throw new NotSupportedException();
	}
}