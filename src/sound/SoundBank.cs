using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
 * Sound bank using SoundPlayer, with direct hook into Windows Multimedia to control
 * volume.
 * 
 * see: https://social.msdn.microsoft.com/Forums/vstudio/en-US/42b46e40-4d4a-48f8-8681-9b0167cfe781/attenuating-soundplayer-volume?forum=csharpgeneral
 */

namespace MouseCrank.src.sound
{
    internal class SoundBank
    {
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(nint hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(nint hwo, uint dwVolume);

        private static readonly string FILE_CRANK_ON = "sounds/crank_on.wav";
        private static readonly string FILE_CRANK_OFF = "sounds/crank_off.wav";

        private double _volume;
        private SoundPlayer _sndCrankOn;
        private SoundPlayer _sndCrankOff;

        public SoundBank()
        {
            try {
                _sndCrankOn = new SoundPlayer(FILE_CRANK_ON);
                _sndCrankOn.LoadAsync();
            } catch {
                _sndCrankOn?.Dispose();
                _sndCrankOn = null;
            }

            try { 
                _sndCrankOff = new SoundPlayer(FILE_CRANK_OFF);
                _sndCrankOff.LoadAsync();
            } catch {
                _sndCrankOff?.Dispose();
                _sndCrankOff = null;
            }

            _volume = GetVolume();
        }

        public void Dispose()
        {
            _sndCrankOn?.Dispose();
            _sndCrankOff?.Dispose();
        }

        private double GetVolume()
        {
            uint CurrVol = 0;
            waveOutGetVolume(nint.Zero, out CurrVol);
            return (ushort)(CurrVol & 0x0000ffff) / (double)ushort.MaxValue;
        }

        public void SetVolume(double vol)
        {
            _volume = vol;
            double lnVol = vol > 0.0 ? Math.Pow(10, -1.0 + _volume) : 0.0;

            ushort newVol = (ushort)(ushort.MaxValue * lnVol);
            uint NewVolumeAllChannels = (uint)newVol & 0x0000ffff | (uint)newVol << 16;
            waveOutSetVolume(nint.Zero, NewVolumeAllChannels);
        }

        public void PlayCrankOn()
        {
            if (_volume > 0.0)
            {
                try
                {
                    _sndCrankOn?.Play();
                }
                catch (Exception e)
                {
                    // Silently fail
                }
            }
        }

        public void PlayCrankOff()
        {
            if (_volume > 0.0)
            {
                try
                {
                    _sndCrankOff?.Play();
                }
                catch (Exception e)
                {
                    // Silently fail
                }
            }
        }
    }
}
