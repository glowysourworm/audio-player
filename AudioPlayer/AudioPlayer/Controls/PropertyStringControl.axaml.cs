using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AudioPlayer.Controls;

public partial class PropertyStringControl : UserControl
{
    public static readonly StyledProperty<string> LabelTextProperty = StyledProperty<string>.Register<PropertyStringControl, string>("LabelText", "Label");
    public static readonly StyledProperty<double> LabelColumnWidthProperty = StyledProperty<double>.Register<PropertyStringControl, double>("LabelColumnWidth", 150);
    public static readonly StyledProperty<IImmutableSolidColorBrush> LabelForegroundProperty = StyledProperty<IImmutableSolidColorBrush>.Register<PropertyStringControl, IImmutableSolidColorBrush>("LabelForeground", Brushes.Black);
    public static readonly StyledProperty<string> ValueProperty = StyledProperty<string>.Register<PropertyStringControl, string>("Value", "");
    public static readonly StyledProperty<bool> IsReadOnlyProperty = StyledProperty<bool>.Register<PropertyStringControl, bool>("IsReadOnly", false);

    public string LabelText
    {
        get { return (string)GetValue(LabelTextProperty); }
        set { SetValue(LabelTextProperty, value); }
    }
    public double LabelColumnWidth
    {
        get { return (double)GetValue(LabelColumnWidthProperty); }
        set { SetValue(LabelColumnWidthProperty, value); }
    }
    public IImmutableSolidColorBrush LabelForeground
    {
        get { return (IImmutableSolidColorBrush)GetValue(LabelForegroundProperty); }
        set { SetValue(LabelForegroundProperty, value); }
    }
    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public PropertyStringControl()
    {
        InitializeComponent();
    }
}