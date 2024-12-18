using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using UIKit;

namespace HelloMaui.Handlers;

public partial class CalendarHandler : ViewHandler<ICalendarView, UICalendarView>, IDisposable
{
	UICalendarSelection? _calendarSelection;
	
	~CalendarHandler()
	{
		Dispose(false);
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	protected override UICalendarView CreatePlatformView()
	{
		return new UICalendarView();
	}

	protected override void ConnectHandler(UICalendarView platformView)
	{
		base.ConnectHandler(platformView);

		_calendarSelection = new UICalendarSelectionSingleDate(new CalendarSelectionSingleDateDelegate(VirtualView));
	}
	
	protected virtual void Dispose(bool disposing)
	{
		ReleaseUnmanagedResources();
		
		if (disposing)
		{
			_calendarSelection?.Dispose();
			_calendarSelection = null;
		}
	}

	static void MapSelectedDate(CalendarHandler handler, ICalendarView virtualView)
	{
		if (handler._calendarSelection is UICalendarSelectionSingleDate calendarSelection)
		{
			MapSingleDateSelection(calendarSelection, virtualView);
		}
	}
	
	static void MapSingleDateSelection(UICalendarSelectionSingleDate calendarSelection, ICalendarView virtualView)
	{
		if (virtualView.SelectedDate is null)
		{
			calendarSelection.SetSelectedDate(null, true);
			return;
		}

		calendarSelection.SetSelectedDate(new NSDateComponents
		{
			Day = virtualView.SelectedDate.Value.Day,
			Month = virtualView.SelectedDate.Value.Month,
			Year = virtualView.SelectedDate.Value.Year
		}, true);
	}


	static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.Calendar.FirstWeekDay = (nuint)virtualView.FirstDayOfWeek;
	}
	
	static void MapMinDate(CalendarHandler handler, ICalendarView virtualView)
	{
		SetDateRange(handler, virtualView);
	}
	
	static void MapMaxDate(CalendarHandler handler, ICalendarView virtualView)
	{
		SetDateRange(handler, virtualView);
	}
	
	static void SetDateRange(CalendarHandler handler, ICalendarView virtualView)
	{
		var fromDateComponents = virtualView.MinDate.Date.ToNSDate();
		var toDateComponents = virtualView.MaxDate.Date.ToNSDate();

		var calendarViewDateRange = new NSDateInterval(fromDateComponents, toDateComponents);
		handler.PlatformView.AvailableDateRange = calendarViewDateRange;
	}

	void ReleaseUnmanagedResources()
	{
		// TODO release unmanaged resources here
	}
	
	sealed class CalendarSelectionSingleDateDelegate(ICalendarView calendarView) : IUICalendarSelectionSingleDateDelegate
	{
		public NativeHandle Handle { get; }
		
		public void Dispose()
		{
			
		}

		public void DidSelectDate(UICalendarSelectionSingleDate calendarSelection, NSDateComponents? date)
		{
			calendarSelection.SelectedDate = date;
			calendarView.SelectedDate = date?.Date.ToDateTime();
			calendarView.OnSelectedDateChanged(date?.Date.ToDateTime());
		}
	}
}