using System.Diagnostics;
using System.Windows;

namespace PdfProtectionRemoverGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var procName = Process.GetCurrentProcess().ProcessName;
            var processes = Process.GetProcessesByName(procName);

            if (processes.Length <= 1) return;
            MessageBox.Show("Процесс " + procName + " уже запущен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            Shutdown();
        }
    }
}
