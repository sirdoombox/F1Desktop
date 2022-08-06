using System.Windows;
using System.Windows.Controls;

namespace F1Desktop.Controls;

public class CountdownElement : Control
{
    public static readonly DependencyProperty CountdownValueProperty = DependencyProperty.Register(
        nameof(CountdownValue), typeof(int), typeof(CountdownElement), new PropertyMetadata(default(int)));

    public int CountdownValue
    {
        get => (int)GetValue(CountdownValueProperty);
        set => SetValue(CountdownValueProperty, value);
    }

    public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(
        nameof(DisplayText), typeof(string), typeof(CountdownElement), new PropertyMetadata(default(string)));

    public string DisplayText
    {
        get => (string)GetValue(DisplayTextProperty);
        set => SetValue(DisplayTextProperty, value);
    }

    public static readonly DependencyProperty IsLightOutProperty = DependencyProperty.Register(
        nameof(IsLightOut), typeof(bool), typeof(CountdownElement), new PropertyMetadata(default(bool)));

    public bool IsLightOut
    {
        get => (bool)GetValue(IsLightOutProperty);
        set => SetValue(IsLightOutProperty, value);
    }

    public static readonly DependencyProperty IsPreviousLightOutProperty = DependencyProperty.Register(
        nameof(IsPreviousLightOut), typeof(bool), typeof(CountdownElement), new PropertyMetadata(default(bool)));

    public bool IsPreviousLightOut
    {
        get => (bool)GetValue(IsPreviousLightOutProperty);
        set => SetValue(IsPreviousLightOutProperty, value);
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property.Name is nameof(CountdownValue))
            IsLightOut = IsPreviousLightOut && CountdownValue == 0;
    }
}