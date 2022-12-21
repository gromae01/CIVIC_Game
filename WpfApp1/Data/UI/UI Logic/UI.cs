using NAudio.Wave;
using System;
using System.Windows.Controls;
using WinFormsApp1.Data.UI.UI_Interfaces;

namespace WinFormsApp1.Data.UI.UI_Logic
{
    public class UI : IUI
    {
        public WaveOut waveOut;
        public virtual void DoMethod(Control UIElement)
        {
            throw new NotImplementedException();
        }

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
