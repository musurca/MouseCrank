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
        private float _sensitivity_x;
        private float _sensitivity_y;
        private float _curve_x;
        private float _curve_y;

        public CrankState() {
            _crankActivated = false;
        }

        public void SetSensitivityX(int sens) {
            lock (_crankLock) {
                _sensitivity_x = sens / 100.0f;
            }
        }

        public float GetSensitivityX() {
            return _sensitivity_x;
        }

        public void SetSensitivityY(int sens) {
            lock (_crankLock) {
                _sensitivity_y = sens / 100.0f;
            }
        }

        public float GetSensitivityY() {
            return _sensitivity_y;
        }

        public void SetCurveX(int sens) {
            lock (_crankLock) {
                _curve_x = sens / 100.0f;
            }
        }

        public float GetCurveX() {
            return _curve_x;
        }

        public void SetCurveY(int sens) {
            lock (_crankLock) {
                _curve_y = sens / 100.0f;
            }
        }

        public float GetCurveY() {
            return _curve_y;
        }

        public void ToggleCrank() {
            lock (_crankLock) {
                _crankActivated = !_crankActivated;
            }
        }

        public void DeactivateCrank() {
            lock (_crankLock) {
                _crankActivated = false;
            }
        }

        public void ActivateCrank() {
            lock (_crankLock) {
                _crankActivated = true;
            }
        }

        public bool IsCrankActivated() {
            return _crankActivated;
        }
    }
}