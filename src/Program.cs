using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MouseCrank.src
{
    internal static class Program {
        private static SingleInstanceEnforcer singleInstance = new SingleInstanceEnforcer("c1046468-8f8f-4469-a08e-4cf5ccc8cf23");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (singleInstance.Claim()) {
                // Start a new instance--
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new MouseCrank_MainWindow());
                
                singleInstance.Release();
            } else {
                // Send a message to the existing instance to show it or restore it from the tray
                singleInstance.NotifyInstance();
            }
        }

        public static string GetVersion() {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return "v" + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build;
        }
    }
}