using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * Prevents the application from starting more than one instance--
 * and sends a message to the application that another instance has been tried.
 * 
 * see: http://sanity-free.org/143/csharp_dotnet_single_instance_application.html
 */

namespace MouseCrank.src {
    internal class SingleInstanceEnforcer {
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public const int HWND_BROADCAST = 0xffff;
        public static readonly int NOTIFY_MSG = RegisterWindowMessage("WM_SHOWME");

        private Mutex _mutex;
        
        public SingleInstanceEnforcer(string uuid) {
            _mutex = new Mutex(true, uuid);
        }

        public bool Claim() {
            return _mutex.WaitOne(TimeSpan.Zero, true);
        }

        public void Release() {
            _mutex.ReleaseMutex();
        }

        public void NotifyInstance() {
            // Send a message to the existing instance that the user tried to start it again
            PostMessage(
                (IntPtr)HWND_BROADCAST,
                NOTIFY_MSG,
                IntPtr.Zero,
                IntPtr.Zero
            );
        }
    }
}
