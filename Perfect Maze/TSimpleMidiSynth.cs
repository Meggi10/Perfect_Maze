using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TSimpleMidiSynth : ISampleProvider
    {
        private readonly double attackTime, decayTime, sustainLevel, releaseTime;
        private readonly MidiFile midi;
        private readonly List<MidiEvent> allEvents;
        private readonly List<TSquareOscillator> squactiveVoices = new List<TSquareOscillator>(32);
        private readonly List<TTriangleOscilator> triactiveVoices = new List<TTriangleOscilator>(32);
        private readonly double samplesPerTick;
        private readonly float[] tempBuffer = new float[1];
        private int eventIndex = 0;
        private long currentSample = 0;

        public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

        public TSimpleMidiSynth(string path, double attack, double decay, double sustain, double release)
        {
            (attackTime, decayTime, sustainLevel, releaseTime) = (attack, decay, sustain, release);

            midi = new MidiFile(path, false);
            allEvents = midi.Events.SelectMany(t => t).OrderBy(e => e.AbsoluteTime).ToList();

            var tempoEvent = allEvents.OfType<TempoEvent>().FirstOrDefault();
            double bpm = tempoEvent?.Tempo ?? 120.0;
            double secondsPerQuarter = 60.0 / bpm;
            samplesPerTick = (secondsPerQuarter / midi.DeltaTicksPerQuarterNote) * WaveFormat.SampleRate;

            if (samplesPerTick <= 0 || double.IsNaN(samplesPerTick) || double.IsInfinity(samplesPerTick))
                samplesPerTick = 45.0;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (allEvents.Count == 0)
                return 0;

            for (int n = 0; n < count; n++)
            {
                RemoveDoneVoices();
                ProcessPendingEvents();
                if (eventIndex >= allEvents.Count && squactiveVoices.Count == 0)
                    return n;
                buffer[offset + n] = Mix();
                currentSample++;
            }
            return count;
        }

        private void RemoveDoneVoices()
        {
            for (int i = squactiveVoices.Count - 1; i >= 0; i--)
                if (squactiveVoices[i].IsDone)
                    squactiveVoices.RemoveAt(i);
        }

        private void ProcessPendingEvents()
        {
            while (eventIndex < allEvents.Count && allEvents[eventIndex].AbsoluteTime * samplesPerTick <= currentSample)
            {
                var midiEvent = allEvents[eventIndex++];

                if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0)
                    squactiveVoices.Add(new TSquareOscillator(noteOn.NoteNumber, attackTime, decayTime, sustainLevel, releaseTime));
                    //triactiveVoices.Add(new TTriangleOscilator(noteOn.NoteNumber, attackTime, decayTime, sustainLevel, releaseTime));
                else if (midiEvent is NoteEvent noteOff)
                    foreach (var voice in squactiveVoices)
                        if (voice.MidiNote == noteOff.NoteNumber)
                            voice.IsReleasing = true;
            }
        }

        private float Mix()
        {
            float mix = 0f;
            foreach (var voice in squactiveVoices)
            {
                voice.Read(tempBuffer, 0, 1);
                mix += tempBuffer[0];
            }
            return Math.Min(1.0f, Math.Max(-1.0f, mix * 1.5f));
        }
    }
}
