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
    internal class MouseToKeys
    {
        private static readonly int UPDATE_SLICE = 1000 / 60;
        private static readonly int MAX_TAP_RATE = 30; // taps/sec
        private static readonly int HD_REFERENCE_WIDTH = 1920 / 2;

        static string[] keysHorizontal = new string[] {
            "{LEFT}", "{RIGHT}"
        };
        static string[] keysVertical = new string[] {
            "{UP}", "{DOWN}"
        };

        private static int signToKey(float sign)
        {
            return sign < 0 ? 0 : 1;
        }

        // Convert a value in pixels to DIP (device-independent pixels)
        // see: https://learn.microsoft.com/en-us/windows/win32/learnwin32/mouse-movement
        private static float pixelToDIP(int pix, uint dpi)
        {
            float scale = dpi / 96.0f;
            return pix / scale;
        }

        public static void Run(CancellationToken token, AutoResetEvent sleepEvent, CrankState state)
        {
            int horizTaps = 0;
            int vertTaps = 0;
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

            while (!token.IsCancellationRequested)
            {
                SBMonitor.CheckState(sbState);
                WinRect rect = sbState.Rect;

                // Put thread to sleep if the crank is deactivated, or
                // Steel Beasts is not running
                if (!state.IsCrankActivated() || !sbState.Running)
                {
                    sleepEvent.Reset();
                    state.DeactivateCrank();
                    sleepEvent.WaitOne();

                    horizTaps = 0;
                    vertTaps = 0;
                    timeToNextTap_Horiz = 0;
                    timeToNextTap_Vert = 0;
                }

                bool willSkipUpdate = false;
                // Determine whether we're going to try to send cranks
                // to Steel Beasts, based on whether the window is available
                // and the cursor is within the window bounds
                if (sbState.InForeground)
                {
                    width = rect.Right - rect.Left;
                    height = rect.Bottom - rect.Top;
                    if (width <= 0 || height <= 0)
                    {
                        // Malformed window
                        willSkipUpdate = true;
                    }
                    else
                    {
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
                        )
                        {
                            // Cursor outside window bounds
                            willSkipUpdate = true;
                        }
                    }
                }
                else
                {
                    // Window not in foreground
                    willSkipUpdate = true;
                }

                if (!willSkipUpdate)
                {
                    sensitivity_x = state.GetSensitivityX();
                    sensitivity_y = state.GetSensitivityY();
                    curve_x = state.GetCurveX();
                    curve_y = state.GetCurveY();

                    // Determine distance from center in DIPs
                    float x_dist = Math.Clamp(
                        10.0f * sensitivity_x * (pixelToDIP(cursor_x - center_x + 1, sbState.DPI_X) / half_width_dip),
                        -1.0f, 1.0f
                    );
                    float y_dist = Math.Clamp(
                        10.0f * sensitivity_y * (pixelToDIP(cursor_y - center_y + 1, sbState.DPI_Y) / half_height_dip),
                        -1.0f, 1.0f
                    );

                    // Use weighted cubic/linear curve to control magnitude of cranking
                    int nextHorizTaps = Math.Min(
                        MAX_TAP_RATE,
                        (int)Math.Abs(
                            (
                                curve_x * x_dist * x_dist * x_dist + (1.0f - curve_x) * x_dist
                            ) * MAX_TAP_RATE + 0.5f
                        )
                    );

                    int nextVertTaps = Math.Min(
                        MAX_TAP_RATE,
                        (int)Math.Abs(
                            (
                                curve_y * y_dist * y_dist * y_dist + (1.0f - curve_y) * y_dist
                            ) * MAX_TAP_RATE + 0.5f
                        )
                    );

                    if (nextHorizTaps > 0)
                    {
                        timeToNextTap_Horiz += UPDATE_SLICE;
                    }
                    else
                    {
                        timeToNextTap_Horiz = 0;
                    }
                    if (nextVertTaps > 0)
                    {
                        timeToNextTap_Vert += UPDATE_SLICE;
                    }
                    else
                    {
                        timeToNextTap_Vert = 0;
                    }

                    int newTaps_horiz = 0;
                    int newTaps_vert = 0;

                    if (horizTaps == 0 && nextHorizTaps > 0)
                    {
                        newTaps_horiz++;
                        timeToNextTap_Horiz = 0;
                    }
                    if (vertTaps == 0 && nextVertTaps > 0)
                    {
                        newTaps_vert++;
                        timeToNextTap_Vert = 0;
                    }

                    horizTaps = nextHorizTaps;
                    vertTaps = nextVertTaps;

                    if (horizTaps > 0)
                    {
                        int tapRate_horiz = 1000 / horizTaps;
                        while (timeToNextTap_Horiz > tapRate_horiz)
                        {
                            timeToNextTap_Horiz -= tapRate_horiz;
                            newTaps_horiz++;
                        }
                    }

                    if (vertTaps > 0)
                    {
                        int tapRate_vert = 1000 / vertTaps;
                        while (timeToNextTap_Vert > tapRate_vert)
                        {
                            timeToNextTap_Vert -= tapRate_vert;
                            newTaps_vert++;
                        }
                    }

                    SendTaps(
                        keysHorizontal[signToKey(x_dist)], newTaps_horiz,
                        keysVertical[signToKey(y_dist)], newTaps_vert
                    );
                }
                else
                {
                    // Skipping this update...
                    // Check screen state again at the next update slice
                    horizTaps = 0;
                    vertTaps = 0;
                    timeToNextTap_Horiz = 0;
                    timeToNextTap_Vert = 0;
                }

                Thread.Sleep(UPDATE_SLICE);
            }
        }

        private static void SendTaps(
            string horiz_key, int horiz_presses,
            string vert_key, int vert_presses
        )
        {
            if (horiz_presses > 0 || vert_presses > 0)
            {
                for (int i = 0; i < horiz_presses; i++)
                {
                    SendKeys.SendWait(horiz_key);
                }
                for (int i = 0; i < vert_presses; i++)
                {
                    SendKeys.SendWait(vert_key);
                }
            }
        }
    }
}
