using MouseCrank.src.sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseCrank.src.crank
{
    internal class CrankState {
        private readonly object _crankLock = new object();
        
        // Activation flag
        private bool _crankActivated;

        // Thread control
        private AutoResetEvent _crankSleepEvent;
        private CancellationTokenSource _crankControl;

        // Input calibration settings
        private float _sensitivity_x;
        private float _sensitivity_y;
        private float _curve_x;
        private float _curve_y;

        // Sounds to play on activation/deactivation
        SoundBank _soundbank;

        public CrankState(SoundBank soundBank) {
            _soundbank = soundBank;

            _crankActivated = false;
            _crankSleepEvent = new AutoResetEvent(false);
            _crankControl = new CancellationTokenSource();
        }

        public void SetSensitivityX(int sens) {
            _sensitivity_x = sens / 100.0f;
        }

        public float GetSensitivityX() {
            return _sensitivity_x;
        }

        public void SetSensitivityY(int sens) {
            _sensitivity_y = sens / 100.0f;
        }

        public float GetSensitivityY() {
            return _sensitivity_y;
        }

        public void SetCurveX(int sens) {
            _curve_x = sens / 100.0f;
        }

        public float GetCurveX() {
            return _curve_x;
        }

        public void SetCurveY(int sens) {
            _curve_y = sens / 100.0f;
        }

        public float GetCurveY() {
            return _curve_y;
        }

        public void DeactivateCrank() {
            lock (_crankLock) {
                if (_crankActivated && IsRunning()) {
                    _crankActivated = false;
                    _crankSleepEvent.Reset();
                }

                _soundbank.PlayCrankOff();
            }
        }

        public void ActivateCrank() {
            lock (_crankLock) {
                if (!_crankActivated && IsRunning()) {
                    _crankActivated = true;
                    _crankSleepEvent.Set();
                }

                _soundbank.PlayCrankOn();
            }
        }

        public bool IsRunning() {
            return !_crankControl.IsCancellationRequested;
        }

        public void KillCrank() {
            lock (_crankLock) {
                _crankControl.Cancel();
                // wake thread up to kill it
                _crankSleepEvent.Set();
            }
        }

        public bool WaitForActivation() {
            return _crankSleepEvent.WaitOne();
        }

        public bool IsCrankActivated() {
            bool crankOn;
            lock (_crankLock) {
                crankOn = _crankActivated;
            }
            return crankOn;
        }

        public void Dispose() {
            _crankControl.Dispose();
            _crankSleepEvent.Dispose();
        }
    }
}