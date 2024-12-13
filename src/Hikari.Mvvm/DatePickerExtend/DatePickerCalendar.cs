using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Hikari.Mvvm.DatePickerExtend
{
    /// <summary>
    /// 扩展DatePicker选择到年、月日期控件
    /// </summary>
    public class DatePickerCalendar
    {
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.RegisterAttached("DisplayMode", typeof(DatePickerMode), typeof(DatePickerCalendar),
                new PropertyMetadata(OnDisplayModeChanged));

        public static DatePickerMode GetDisplayMode(DependencyObject dobj)
        {
            return (DatePickerMode)dobj.GetValue(DisplayModeProperty);
        }

        public static void SetDisplayMode(DependencyObject dobj, DatePickerMode value)
        {
            dobj.SetValue(DisplayModeProperty, value);
        }

        private static void OnDisplayModeChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            Application.Current.Dispatcher
                .BeginInvoke(DispatcherPriority.Loaded,
                             new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetCalendarEventHandlers),
                             datePicker, e);
        }

        private static void SetCalendarEventHandlers(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            if ((DatePickerMode)e.NewValue == DatePickerMode.Month || (DatePickerMode)e.NewValue == DatePickerMode.Year)
            {
                datePicker.CalendarOpened += DatePickerOnCalendarOpened;
                datePicker.CalendarClosed += DatePickerOnCalendarClosed;
            }
            else
            {
                datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
                datePicker.CalendarClosed -= DatePickerOnCalendarClosed;
            }
        }

        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            var datePicker = (DatePicker)sender;
            var calendar = GetDatePickerCalendar(sender);
            var displayMode = GetDisplayMode(datePicker);

            if (displayMode == DatePickerMode.Month)
            {
                calendar.DisplayMode = CalendarMode.Year;
                calendar.DisplayModeChanged += CalendarOnDisplayModeChangedForMonth;
            }
            else if (displayMode == DatePickerMode.Year)
            {
                calendar.DisplayMode = CalendarMode.Decade;
                calendar.DisplayModeChanged += CalendarOnDisplayModeChangedForYear;
            }
            //calendar.DisplayMode = CalendarMode.Year;

            //calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
        }

        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            var datePicker = (DatePicker)sender;
            var calendar = GetDatePickerCalendar(sender);
            datePicker.SelectedDate = calendar.SelectedDate;

            if (GetDisplayMode(datePicker) == DatePickerMode.Month)
            {
                datePicker.SelectedDate = calendar.SelectedDate.HasValue
                    ? new DateTime(calendar.SelectedDate.Value.Year, calendar.SelectedDate.Value.Month, 1)
                    : null;
                calendar.DisplayModeChanged -= CalendarOnDisplayModeChangedForMonth;
            }
            else if (GetDisplayMode(datePicker) == DatePickerMode.Year)
            {
                datePicker.SelectedDate = calendar.SelectedDate.HasValue
                    ? new DateTime(calendar.SelectedDate.Value.Year, 1, 1)
                    : null;
                calendar.DisplayModeChanged -= CalendarOnDisplayModeChangedForYear;
            }
            //calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
        }

        private static void CalendarOnDisplayModeChangedForMonth(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            if (calendar.DisplayMode != CalendarMode.Month)
                return;

            calendar.SelectedDate = GetSelectedCalendarMonth(calendar.DisplayDate);

            var datePicker = GetCalendarsDatePicker(calendar);
            datePicker.IsDropDownOpen = false;
        }
        private static void CalendarOnDisplayModeChangedForYear(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            if (calendar.DisplayMode != CalendarMode.Year)
                return;

            calendar.SelectedDate = GetSelectedCalendarYear(calendar.DisplayDate);

            var datePicker = GetCalendarsDatePicker(calendar);
            datePicker.IsDropDownOpen = false;
        }
        private static Calendar GetDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
            return ((Calendar)popup.Child);
        }

        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            var parent = (FrameworkElement)child.Parent;
            if (parent.Name == "PART_Root")
                return (DatePicker)parent.TemplatedParent;
            return GetCalendarsDatePicker(parent);
        }

        private static DateTime? GetSelectedCalendarMonth(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue)
                return null;
            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }
        private static DateTime? GetSelectedCalendarYear(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue)
                return null;
            return new DateTime(selectedDate.Value.Year, 1, 1);
        }

        public enum DatePickerMode
        {
            None,
            /// <summary>The <see cref="T:System.Windows.Controls.DatePicker" /> displays a month at a time.</summary>
            Month,
            /// <summary>The <see cref="T:System.Windows.Controls.DatePicker" /> displays a year at a time.</summary>
            Year
        }
    }

}
