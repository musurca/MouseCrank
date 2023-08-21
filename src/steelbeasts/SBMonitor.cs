using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/* 
 * Determines the current state of the Steel Beasts Pro PE/PE Server application:
 *  -is it running?
 *  -is it in the foreground?
 *  -what are the dimensions of its window?
 *  -what is DPI of the monitor on which it is placed?
 * 
 */

namespace MouseCrank.src.steelbeasts
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WinRect {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    internal class SBState {
        public bool Running { get; set; }
        public bool InForeground { get; set; }

        public uint DPI_X { get; set; }
        public uint DPI_Y { get; set; }

        public WinRect Rect { get; set; }

        public SBState() {
            Running = false;
            InForeground = false;
            DPI_X = 96;
            DPI_Y = 96;
            Rect = new WinRect();
        }
    }

    internal class SBMonitor {
        public enum DpiType {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }

        [DllImport("User32.dll")]
        private static extern nint GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(nint hWnd, out WinRect lpRect);

        [DllImport("User32.dll")]
        private static extern nint MonitorFromPoint([In] Point pt, [In] uint dwFlags);

        [DllImport("Shcore.dll")]
        private static extern nint GetDpiForMonitor([In] nint hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

        private static readonly string[] APP_NAMES = { "sbprope64cm", "sbpropeserver64cm" };
        private static WinRect APP_RECT = new WinRect();
        private static Point WND_POINT = new Point(0, 0);

        // https://stackoverflow.com/questions/29438430/how-to-get-dpi-scale-for-all-screens
        private static void GetDpi(Point pnt, DpiType dpiType, out uint dpiX, out uint dpiY) {
            var mon = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
            GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);
        }

        public static void CheckState(SBState state) {
            WinRect rect = state.Rect;

            Process p = null;
            // Check for both PE & PE Server
            foreach (string appName in APP_NAMES) {
                p = Process.GetProcessesByName(appName).FirstOrDefault();
                if (p != null) {
                    break;
                }
            }

            if (p == null) {
                // No matching process found, Steel Beasts is not running
                state.Running = false;
                state.InForeground = false;

                rect.Left = 0;
                rect.Top = 0;
                rect.Right = 0;
                rect.Bottom = 0;
                state.Rect = rect;
            } else {
                // Steel Beasts is running
                state.Running = true;
                nint mwnd = p.MainWindowHandle;

                // Determine window extents
                GetWindowRect(mwnd, out APP_RECT);
                rect.Left = APP_RECT.Left;
                rect.Top = APP_RECT.Top;
                rect.Right = APP_RECT.Right;
                rect.Bottom = APP_RECT.Bottom;
                state.Rect = rect;

                // Detect DPI
                WND_POINT.X = rect.Left + 1;
                WND_POINT.Y = rect.Top + 1;
                foreach (Screen screen in Screen.AllScreens) {
                    if (screen.Bounds.Contains(WND_POINT)) {
                        uint d_x, d_y;
                        GetDpi(WND_POINT, DpiType.Angular, out d_x, out d_y);
                        state.DPI_X = d_x;
                        state.DPI_Y = d_y;
                        break;
                    }
                }

                // Determine whether window is foregrounded
                state.InForeground = GetForegroundWindow() == mwnd;
            }
        }
    }
}
