using System.Reflection;

namespace MouseCrank.src
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MouseCrank_MainWindow());
        }

        public static string GetVersion()
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return "v" + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build;
        }
    }
}