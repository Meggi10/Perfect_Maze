using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TLoopStream : WaveStream
    {
        private readonly WaveStream Stream;
        public TLoopStream(WaveStream source)
        {
            Stream = source;
        }
        public override WaveFormat WaveFormat => Stream.WaveFormat;
        public override long Length => Stream.Length;
        public override long Position
        {
            get => Stream.Position;
            set => Stream.Position = value;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < count)
            {
                int bytesRead = Stream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (Stream.Position == 0)
                        break;
                    Stream.Position = 0;
                }
                else
                    totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Stream?.Dispose();
            base.Dispose(disposing);
        }
    }
}
