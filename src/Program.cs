using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MouseCrank.src
{
    internal static class Program
    {
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        private static Mutex mutex = new Mutex(true, "c1046468-8f8f-4469-a08e-4cf5ccc8cf23");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true)) {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new MouseCrank_MainWindow());
                mutex.ReleaseMutex();
            } else {
                PostMessage(
                    (IntPtr)HWND_BROADCAST,
                    WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero
                );
            }
        }

        public static string GetVersion() {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return "v" + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build;
        }
    }
}