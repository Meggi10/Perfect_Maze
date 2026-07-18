using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TSquareOscillator : ISampleProvider
    {
        private double phase;
        private readonly double phaseStep;
        private readonly int sampleRate = 44100;
        private double envelope = 0.0;
        private readonly double sustainLevel;
        private readonly double attackStep, decayStep, releaseStep;
        private AdsrPhase adsrPhase = AdsrPhase.Attack;

        private enum AdsrPhase { Attack, Decay, Sustain, Release, Done }

        public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
        public int MidiNote { get; }
        public float Volume { get; set; } = 0.1f;
        public bool IsReleasing { get; set; } = false;
        public bool IsDone => adsrPhase == AdsrPhase.Done;

        public TSquareOscillator(int midiNote, double attack, double decay, double sustain, double release)
        {
            MidiNote = midiNote;
            sustainLevel = Math.Min(0.0, Math.Max(1.0, sustain));
            phaseStep = 440.0 * Math.Pow(2, (midiNote - 69) / 12.0) / sampleRate;
            attackStep = attack > 0 ? 1.0 / (attack * sampleRate) : 1.0;
            decayStep = decay > 0 ? 1.0 / (decay * sampleRate) : 1.0;
            releaseStep = release > 0 ? 1.0 / (release * sampleRate) : 1.0;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                TickEnvelope();
                float square = phase < 0.5 ? Volume : -Volume;
                buffer[offset + i] = square * (float)envelope;
                phase = (phase + phaseStep) % 1.0;
            }
            return count;
        }

        private void TickEnvelope()
        {
            if (IsReleasing && adsrPhase != AdsrPhase.Release && adsrPhase != AdsrPhase.Done)
                adsrPhase = AdsrPhase.Release;

            switch (adsrPhase)
            {
                case AdsrPhase.Attack:
                    envelope += attackStep;
                    if (envelope >= 1.0) 
                    { envelope = 1.0; adsrPhase = AdsrPhase.Decay; }
                    break;
                case AdsrPhase.Decay:
                    envelope -= decayStep;
                    if (envelope <= sustainLevel) 
                    { envelope = sustainLevel; adsrPhase = AdsrPhase.Sustain; }
                    break;
                case AdsrPhase.Release:
                    envelope -= releaseStep;
                    if (envelope <= 0.0) 
                    { envelope = 0.0; adsrPhase = AdsrPhase.Done; }
                    break;
            }
        }
    }
}
