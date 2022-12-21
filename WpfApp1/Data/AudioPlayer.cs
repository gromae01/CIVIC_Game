using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Data
{
    
    public class AudioPlayer
    {
        public WaveOut waveOut;
        public void PlaySound(string soundname)
        {
            var reader = new WaveFileReader(soundname);
            if (waveOut == null)
                waveOut = new WaveOut();
            waveOut.Init(reader);
            waveOut.Play();
        }
    }
}
