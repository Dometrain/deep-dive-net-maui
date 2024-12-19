using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
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
        _calendarSelection = new UICalendarSelectionSingleDate(new CalendarSelectionSingleDateDelegate(VirtualView));
        return new UICalendarView
        {
            Calendar = new(NSCalendarType.Gregorian),
            SelectionBehavior = _calendarSelection
        };
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

    sealed class CalendarSelectionSingleDateDelegate(ICalendarView calendarView) : UICalendarSelectionSingleDateDelegate
    {
        public override bool CanSelectDate(UICalendarSelectionSingleDate selection, NSDateComponents? dateComponents) => true;

        public override void DidSelectDate(UICalendarSelectionSingleDate selection, NSDateComponents? dateComponents)
        {
            selection.SelectedDate = dateComponents;
            calendarView.SelectedDate = dateComponents?.Date.ToDateTime();
            calendarView.OnSelectedDateChanged(dateComponents?.Date.ToDateTime());
        }
    }
}