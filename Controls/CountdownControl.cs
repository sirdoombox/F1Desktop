using System.Windows;
using System.Windows.Controls;
using F1Desktop.Misc.Extensions;

namespace F1Desktop.Controls;

public class CountdownControl : Control
{
    public static readonly DependencyProperty TimeRemainingProperty = DependencyProperty.Register(
        nameof(TimeRemaining), typeof(TimeSpan), typeof(CountdownControl), new PropertyMetadata(default(TimeSpan)));

    public TimeSpan TimeRemaining
    {
        get => (TimeSpan)GetValue(TimeRemainingProperty);
        set => SetValue(TimeRemainingProperty, value);
    }

    public static readonly DependencyProperty WeeksRemainingProperty = DependencyProperty.Register(
        nameof(WeeksRemaining), typeof(int), typeof(CountdownControl), new PropertyMetadata(default(int)));

    public int WeeksRemaining
    {
        get => (int)GetValue(WeeksRemainingProperty);
        set => SetValue(WeeksRemainingProperty, value);
    }

    public static readonly DependencyProperty DaysRemainingProperty = DependencyProperty.Register(
        nameof(DaysRemaining), typeof(int), typeof(CountdownControl), new PropertyMetadata(default(int)));

    public int DaysRemaining
    {
        get => (int)GetValue(DaysRemainingProperty);
        set => SetValue(DaysRemainingProperty, value);
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property.Name is not nameof(TimeRemaining)) return;
        WeeksRemaining = TimeRemaining.Weeks();
        DaysRemaining = TimeRemaining.Days();
    }
}