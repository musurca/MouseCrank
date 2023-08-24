using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using MouseCrank.src.steelbeasts;

/*
 * Thread that checks the position of the mouse relative to the center
 * of the Steel Beasts window, and sends arrow key taps based on the
 * distance to simulate hand-cranking the turret.
 * 
 */

namespace MouseCrank.src.crank
{
    internal class CrankThread {
        private static readonly int SLOW_UPDATE_SLICE = 1000 / 10;
        private static readonly int UPDATE_SLICE = 1000 / 60;
        private static readonly float MAX_TAP_RATE = 30.0f; // taps/sec
        private static readonly float TRIGGER_THRESHOLD = 0.5f;
        private static readonly int HD_REFERENCE_WIDTH = 1920 / 2;

        static string[] keysHorizontal = new string[] {
            "{LEFT}", "{RIGHT}"
        };
        static string[] keysVertical = new string[] {
            "{UP}", "{DOWN}"
        };

        private Thread _crankThread;
        private CrankState _crankState;

        public CrankThread(CrankState crankState) {
            _crankState = crankState;

            _crankThread = new(
                () => Run(_crankState)
            );
        }

        public void Start() {
            _crankThread.Start();
        }

        public void Stop() {
            // Signal the thread to stop
            _crankState.KillCrank();
            // Wait for the thread to complete
            _crankThread.Join();
        }

        private static int signToKey(float sign) {
            return sign < 0 ? 0 : 1;
        }

        // Convert a value in pixels to DIP (device-independent pixels)
        // see: https://learn.microsoft.com/en-us/windows/win32/learnwin32/mouse-movement
        private static float pixelToDIP(int pix, uint dpi) {
            float scale = dpi / 96.0f;
            return pix / scale;
        }

        public static void Run(CrankState crankState) {
            float horizTaps = 0.0f;
            float vertTaps = 0.0f;
            int timeToNextTap_Horiz = 0;
            int timeToNextTap_Vert = 0;

            float sensitivity_x, sensitivity_y;
            float curve_x, curve_y;

            SBState sbState = new SBState();

            int width = 0, height = 0;
            int half_width = 0, half_height = 0;
            float half_width_dip = 0.0f, half_height_dip = 0.0f;
            int center_x = 0, center_y = 0;
            int cursor_x = 0, cursor_y = 0;

            while (crankState.IsRunning()) {
                SBMonitor.CheckState(sbState);
                WinRect rect = sbState.Rect;

                // Put thread to sleep if the crank is deactivated, or
                // Steel Beasts is not running
                bool crankActivated = crankState.IsCrankActivated();
                if (!crankActivated || !sbState.Running) {
                    if (crankActivated) {
                        // Deactivate the crank if it's still activated
                        crankState.DeactivateCrank();
                    }

                    // Wait until the crank is reactivated
                    // or thread is woken up to be killed
                    crankState.WaitForActivation();
                    
                    // If no longer running, kill the thread
                    if(!crankState.IsRunning()) { return; }

                    horizTaps = 0.0f;
                    vertTaps = 0.0f;
                    timeToNextTap_Horiz = 0;
                    timeToNextTap_Vert = 0;

                    // Refresh the state of Steel Beasts
                    SBMonitor.CheckState(sbState);
                }

                bool willSkipUpdate = false;
                // Determine whether we're going to try to send cranks
                // to Steel Beasts, based on whether the window is available
                // and the cursor is within the window bounds
                if (sbState.InForeground) {
                    width = rect.Right - rect.Left;
                    height = rect.Bottom - rect.Top;
                    if (width <= 0 || height <= 0) {
                        // Malformed window
                        willSkipUpdate = true;
                    } else {
                        // Using an HD resolution screen as a reference
                        half_width_dip = pixelToDIP(HD_REFERENCE_WIDTH, sbState.DPI_X);
                        half_height_dip = pixelToDIP(HD_REFERENCE_WIDTH, sbState.DPI_Y);

                        half_width = width / 2;
                        half_height = height / 2;
                        center_x = rect.Left + half_width;
                        center_y = rect.Top + half_height;
                        cursor_x = Cursor.Position.X;
                        cursor_y = Cursor.Position.Y;

                        if (
                            cursor_x < rect.Left ||
                            cursor_x >= rect.Right ||
                            cursor_y < rect.Top ||
                            cursor_y >= rect.Right
                        ) {
                            // Cursor outside window bounds
                            willSkipUpdate = true;
                        }
                    }
                } else {
                    // Window not in foreground -- we've alt-tabbed away
                    // Turn off the crank
                    crankState.DeactivateCrank();
                    continue;
                }

                if (!willSkipUpdate) {
                    sensitivity_x   = crankState.GetSensitivityX();
                    sensitivity_y   = crankState.GetSensitivityY();
                    curve_x         = crankState.GetCurveX();
                    curve_y         = crankState.GetCurveY();

                    // Determine distance from center in DIPs
                    float x_dist = Math.Clamp(
                        10.0f * sensitivity_x * (
                            pixelToDIP(cursor_x - center_x + 1, sbState.DPI_X) / half_width_dip
                        ),
                        -1.0f, 1.0f
                    );
                    float y_dist = Math.Clamp(
                        10.0f * sensitivity_y * (
                            pixelToDIP(cursor_y - center_y + 1, sbState.DPI_Y) / half_height_dip
                        ),
                        -1.0f, 1.0f
                    );

                    // Use weighted cubic/linear curve to control magnitude of cranking
                    float nextHorizTaps = Math.Min(
                        MAX_TAP_RATE,
                        Math.Abs(
                            (
                                curve_x * x_dist * x_dist * x_dist + (1.0f - curve_x) * x_dist
                            ) * MAX_TAP_RATE + 0.5f
                        )
                    );

                    float nextVertTaps = Math.Min(
                        MAX_TAP_RATE,
                        Math.Abs(
                            (
                                curve_y * y_dist * y_dist * y_dist + (1.0f - curve_y) * y_dist
                            ) * MAX_TAP_RATE + 0.5f
                        )
                    );

                    // Start the clock for tap rate if we've crossed the input threshold
                    if (nextHorizTaps >= TRIGGER_THRESHOLD) {
                        timeToNextTap_Horiz += UPDATE_SLICE;
                    } else {
                        timeToNextTap_Horiz = 0;
                    }
                    if (nextVertTaps >= TRIGGER_THRESHOLD) {
                        timeToNextTap_Vert += UPDATE_SLICE;
                    } else {
                        timeToNextTap_Vert = 0;
                    }

                    int newTaps_horiz = 0;
                    int newTaps_vert = 0;

                    // If we've just crossed the input threshold, generate a tap immediately
                    if (horizTaps == 0.0f && nextHorizTaps >= TRIGGER_THRESHOLD) {
                        newTaps_horiz++;
                        timeToNextTap_Horiz = 0;
                    }
                    if (vertTaps == 0.0f && nextVertTaps >= TRIGGER_THRESHOLD) {
                        newTaps_vert++;
                        timeToNextTap_Vert = 0;
                    }

                    horizTaps = nextHorizTaps;
                    vertTaps = nextVertTaps;

                    if (horizTaps > 0.0f) {
                        int tapRate_horiz = (int)Math.Round(1000.0f / horizTaps);
                        while (timeToNextTap_Horiz > tapRate_horiz) {
                            timeToNextTap_Horiz -= tapRate_horiz;
                            newTaps_horiz++;
                        }
                    }

                    if (vertTaps > 0.0f) {
                        int tapRate_vert = (int)Math.Round(1000.0f / vertTaps);
                        while (timeToNextTap_Vert > tapRate_vert) {
                            timeToNextTap_Vert -= tapRate_vert;
                            newTaps_vert++;
                        }
                    }

                    SendTaps(
                        keysHorizontal[signToKey(x_dist)], newTaps_horiz,
                        keysVertical[signToKey(y_dist)], newTaps_vert
                    );

                    Thread.Sleep(UPDATE_SLICE);
                } else {
                    // Conditions are not correct to check for input
                    // We'll check state again at the next update,
                    // but we'll pulse slower to save resources.

                    horizTaps = 0.0f;
                    vertTaps = 0.0f;
                    timeToNextTap_Horiz = 0;
                    timeToNextTap_Vert = 0;

                    Thread.Sleep(SLOW_UPDATE_SLICE);
                }
            }
        }

        private static void SendTaps(
            string horiz_key, int horiz_presses,
            string vert_key, int vert_presses
        ) {
            if (horiz_presses > 0 || vert_presses > 0) {
                for (int i = 0; i < horiz_presses; i++) {
                    SendKeys.SendWait(horiz_key);
                }
                for (int i = 0; i < vert_presses; i++) {
                    SendKeys.SendWait(vert_key);
                }
            }
        }
    }
}
