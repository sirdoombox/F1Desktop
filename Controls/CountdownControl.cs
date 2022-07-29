using System;
using System.Windows;
using System.Windows.Controls;

namespace F1Desktop.Controls;

public class CountdownControl : Control
{
    public static readonly DependencyProperty TimeRemainingProperty = DependencyProperty.Register(
        "TimeRemaining", typeof(TimeSpan), typeof(CountdownControl), new PropertyMetadata(default(TimeSpan)));

    public TimeSpan TimeRemaining
    {
        get => (TimeSpan)GetValue(TimeRemainingProperty);
        set => SetValue(TimeRemainingProperty, value);
    }
}