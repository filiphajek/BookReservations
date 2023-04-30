using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.Controls;

public partial class LoadingButton : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomImageButton), string.Empty);

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(IAsyncRelayCommand), typeof(CustomImageButton), null);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomImageButton), null);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public IAsyncRelayCommand Command
    {
        get => (IAsyncRelayCommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public LoadingButton()
    {
        InitializeComponent();
    }
}