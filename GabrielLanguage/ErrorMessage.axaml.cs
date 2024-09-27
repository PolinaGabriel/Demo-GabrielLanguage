using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GabrielLanguage;

public partial class ErrorMessage : Window
{
    public ErrorMessage()
    {
        InitializeComponent();
    }

    public void FillErrors(string message)
    {
        errors.Text = message;
    }

    public void Ok(object sender, RoutedEventArgs routedEventArgs)
    {
        this.Close();
    }
}