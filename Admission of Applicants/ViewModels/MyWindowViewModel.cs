using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.Input;

namespace Admission_of_Applicants.ViewModels;

public partial class MyWindowViewModel : ViewModelBase
{
    [RelayCommand]
    private void OpenGithub()
    {
        OpenUrl("https://github.com/NorthVaryag/New_Admission_of_Applicants");
    }

    [RelayCommand]
    private void OpenTelegram()
    {
        OpenUrl("https://t.me/North_Varyag");
    }

    private void OpenUrl(string url)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) Process.Start("open", url);
        }
        catch
        {
        }
    }
}