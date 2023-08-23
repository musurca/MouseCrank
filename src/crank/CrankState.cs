using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseCrank.src.crank
{
    internal class CrankState {
        private readonly object _crankLock = new object();
        private bool _crankActivated;
        private AutoResetEvent _crankSleepEvent;
        private CancellationTokenSource _crankControl;
        private float _sensitivity_x;
        private float _sensitivity_y;
        private float _curve_x;
        private float _curve_y;

        public CrankState() {
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
            if (_crankActivated && IsRunning()) {
                lock (_crankLock) {
                    _crankActivated = false;
                    _crankSleepEvent.Reset();
                }
            }
        }

        public void ActivateCrank() {
            if (!_crankActivated && IsRunning()) {
                lock (_crankLock) {
                    _crankActivated = true;
                    _crankSleepEvent.Set();
                }
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
            lock (_crankLock) {
                return _crankActivated;
            }
        }

        public void Dispose() {
            _crankControl.Dispose();
            _crankSleepEvent.Dispose();
        }
    }
}