using System.Configuration;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/*
 * MouseCrank main window UI
 */

namespace MouseCrank {
    public partial class MouseCrank_MainWindow : Form {
        private static Dictionary<string, Keys> keyMaps = new Dictionary<string, Keys> {
            { "A", Keys.A },
            { "B", Keys.B },
            { "C", Keys.C },
            { "D", Keys.D },
            { "E", Keys.E },
            { "F", Keys.F },
            { "G", Keys.G },
            { "H", Keys.H },
            { "I", Keys.I },
            { "J", Keys.J },
            { "K", Keys.K },
            { "L", Keys.L },
            { "M", Keys.M },
            { "N", Keys.N },
            { "O", Keys.O },
            { "P", Keys.P },
            { "Q", Keys.Q },
            { "R", Keys.R },
            { "S", Keys.S },
            { "T", Keys.T },
            { "U", Keys.U },
            { "V", Keys.V },
            { "W", Keys.W },
            { "X", Keys.X },
            { "Y", Keys.Y },
            { "Z", Keys.Z },
            { "[", Keys.OemOpenBrackets },
            { "]", Keys.OemCloseBrackets },
            { ";", Keys.OemSemicolon },
            { "'", Keys.OemQuotes },
            { ",", Keys.Oemcomma },
            { ".", Keys.OemPeriod },
            { "/", Keys.OemBackslash },
            { "Tab", Keys.Tab },
            { "Caps Lock", Keys.CapsLock },
            { "L Ctrl", Keys.LControlKey },
            { "L Shift", Keys.LShiftKey },
            { "R Ctrl", Keys.RControlKey },
            { "R Shift", Keys.RShiftKey },
            { "Alt", Keys.Alt },
            { "Delete", Keys.Delete },
            { "Num 0", Keys.NumPad0 },
            { "Enter", Keys.Enter },
        };

        private Thread _mouseCrankThread;
        private CancellationTokenSource _mouseCrankThread_Control;
        private AutoResetEvent _mouseCrankSleepEvent;

        private SBState _sbState;
        private CrankState _crankState;
        private GlobalKeyboardHook _globalKeyboardHook;
        private bool _togglePressed;

        private SoundBank _soundBank;

        public MouseCrank_MainWindow() {
            InitializeComponent();

            // Allocate memory for the Steel Beasts state struct
            _sbState = new SBState();

            // Load crank on/off sounds
            _soundBank = new SoundBank();

            // Initialize the global keypress hook
            _togglePressed = false;
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnRawKeyPressed;

            // Set the initial state of the UI and thread communication
            _crankState = new CrankState();
            SetInitialState();

            // Start the mouse-to-cranks thread
            StartCrankThread();
        }

        private void SetVolume(double vol) {
            _soundBank.SetVolume(vol);
            User.Default.SoundVolume = (float)vol;
        }

        private void OnRawKeyPressed(object sender, GlobalKeyboardHookEventArgs e) {
            SBMonitor.CheckState(_sbState);

            if (_sbState.Running && _sbState.InForeground) {
                if (
                    e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown &&
                    !_togglePressed
                ) {
                    if (_crankState.IsCrankActivated()) {
                        _crankState.DeactivateCrank();

                        // Put crank thread to sleep
                        _mouseCrankSleepEvent.Reset();

                        _soundBank.PlayCrankOff();
                    } else {
                        _crankState.ActivateCrank();

                        // Wake up crank thread
                        _mouseCrankSleepEvent.Set();

                        _soundBank.PlayCrankOn();
                    }

                    _togglePressed = true;
                } else if (
                    e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp
                ) {
                    _togglePressed = false;
                }
            }
        }

        private void StartCrankThread() {
            _mouseCrankThread_Control = new();
            _mouseCrankSleepEvent = new AutoResetEvent(false);
            _mouseCrankThread = new(
                () => MouseToKeys.Run(
                    _mouseCrankThread_Control.Token,
                    _mouseCrankSleepEvent,
                    _crankState
                )
            );
            _mouseCrankThread.Start();
        }

        private void StopCrankThread() {
            _mouseCrankThread_Control.Cancel();
            _mouseCrankSleepEvent.Set(); // wake up crank thread, time to die
            _mouseCrankThread.Join();
            _mouseCrankThread_Control.Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);

            StopCrankThread();

            _globalKeyboardHook.Dispose();

            _soundBank.Dispose();

            User.Default.Save();
        }

        private void SetInitialState() {
            // Set initial calibration values
            SetSensX(User.Default.SensitivityX);
            SetSensY(User.Default.SensitivityY);
            SetCurveX(User.Default.CurveX);
            SetCurveY(User.Default.CurveY);

            // Key toggle values
            string selectedToggleKey = User.Default.ToggleKey;
            toggleKey.BeginUpdate();
            foreach (string key in keyMaps.Keys) {
                toggleKey.Items.Add(key);
            }
            toggleKey.EndUpdate();
            int index = toggleKey.Items.IndexOf(selectedToggleKey);
            if (index != -1) {
                toggleKey.SelectedIndex = index;
            } else {
                toggleKey.SelectedIndex = toggleKey.Items.IndexOf("Caps Lock");
            }

            // Sounds
            sndVolume.Value = (int)(User.Default.SoundVolume * sndVolume.Maximum);
            SetVolume(User.Default.SoundVolume);

            // Minimize to tray
            checkTrayMinimize.Checked = User.Default.MinimizeToTray;
        }

        private void RefreshKeyHook() {
            int index = toggleKey.SelectedIndex;
            string keyID = (string)toggleKey.Items[index];

            if (keyMaps.ContainsKey(keyID)) {
                _globalKeyboardHook.SetRegisteredKeys(
                    new Keys[] { keyMaps[keyID] }
                );
            }

            User.Default.ToggleKey = keyID;
        }

        // CALIBRATION SETTINGS
        private void SetSensX(int val) {
            if ((int)SensX.Value != val) {
                SensX.Value = val;
            }

            _crankState.SetSensitivityX(val);
            SensXValue.Text = val.ToString();

            User.Default.SensitivityX = val;
        }

        private void SensX_Scroll(object sender, EventArgs e) {
            int sensx = (int)SensX.Value;
            SetSensX(sensx);
        }

        private void SetSensY(int val) {
            if ((int)SensY.Value != val) {
                SensY.Value = val;
            }

            _crankState.SetSensitivityY(val);
            SensYValue.Text = val.ToString();

            User.Default.SensitivityY = val;
        }

        private void SensY_Scroll(object sender, EventArgs e) {
            int sensy = (int)SensY.Value;
            SetSensY(sensy);
        }

        private void SetCurveX(int val) {
            if ((int)CurveX.Value != val) {
                CurveX.Value = val;
            }

            _crankState.SetCurveX(val);
            CurveValueX.Text = val.ToString();

            User.Default.CurveX = val;
        }

        private void CurveX_Scroll(object sender, EventArgs e) {
            int curvx = (int)CurveX.Value;
            SetCurveX(curvx);
        }

        private void SetCurveY(int val) {
            if ((int)CurveY.Value != val) {
                CurveY.Value = val;
            }

            _crankState.SetCurveY(val);
            CurveValueY.Text = val.ToString();

            User.Default.CurveY = val;
        }

        private void CurveY_Scroll(object sender, EventArgs e) {
            int curvy = (int)CurveY.Value;
            SetCurveY(curvy);
        }

        // RESET HORIZONTAL
        private void btnResetX_Click(object sender, EventArgs e) {
            SetSensX(User.Default.SensitivityX_Default);
            SetCurveX(User.Default.CurveX_Default);
        }

        // RESET VERTICAL
        private void btnResetY_Click(object sender, EventArgs e) {
            SetSensY(User.Default.SensitivityY_Default);
            SetCurveY(User.Default.CurveY_Default);
        }

        // CHANGE TOGGLE KEY
        private void toggleKey_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshKeyHook();
        }

        // SHOW "ABOUT..." MENU
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            About abt = new About();
            abt.ShowDialog();
            abt.Dispose();
        }

        // RETURN FROM TRAY
        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            MouseCrank_TrayIcon.Visible = false;
        }

        // MINIMIZE TO TRAY
        private void MouseCrank_MainWindow_Resize(object sender, EventArgs e) {
            if (User.Default.MinimizeToTray) {
                if (this.WindowState == FormWindowState.Minimized) {
                    Hide();
                    MouseCrank_TrayIcon.Visible = true;
                    MouseCrank_TrayIcon.ShowBalloonTip(1000);
                }
            }
        }

        // CHANGE SOUND VOLUME
        private void sndVolume_Scroll(object sender, EventArgs e) {
            double newVol = sndVolume.Value / (double)sndVolume.Maximum;
            SetVolume(newVol);
        }

        // PLAY TEST OF SOUND VOLUME
        private void sndVolume_MouseUp(object sender, EventArgs e) {
            _soundBank.PlayCrankOn();
        }

        // CHANGE TRAY MINIMIZATION PREFERENCE
        private void checkTrayMinimize_CheckedChanged(object sender, EventArgs e) {
            User.Default.MinimizeToTray = checkTrayMinimize.Checked;
        }
    }
}