using Microsoft.Maui.Controls;

namespace DogOrCatMaui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}