using NAudio.Wave;
using System;

namespace WinFormsApp1.Data.Instaces
{
    public class SoundPlayer:IDisposable
    {
        private WaveOut waveOut = new WaveOut();
        public SoundPlayer(string soundname)
        {
            var reader = new WaveFileReader(soundname);
            waveOut.Init(reader);
            waveOut.Volume = 0.2F;
            waveOut.Play();  
        }

        public void Dispose()
        {
            waveOut.Dispose();
        }
    }
}
