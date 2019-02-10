using System;
using System.IO;
using System.Linq;

namespace StreamCutter
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] stream_array = ReadFully(Cutter(new MemoryStream(File.ReadAllBytes("xxx.mp3")), 0, 10));
            File.WriteAllBytes("sss.mp3", stream_array);
            Console.Read();
        }

        private static Stream Cutter(Stream stream, int start = 0, int end = 0)
        {
            byte[] stream_array = ReadFully(stream);
            int OGKBytes = stream_array.Length / 1000;
            int OGseconds = OGKBytes / 2;
            int OGBytes = stream_array.Length;
            if ((start + end) >= OGseconds)
                throw new Exception("You trimming more seconds than the actual audio has");
            if(start == 0 && end == 0)
                return new MemoryStream(stream_array);
            if(start > 0)
                stream_array = stream_array.Skip(start * 2 * 1000).ToArray();
            if (end > 0)
                stream_array = stream_array.Reverse().ToArray().Skip(end * 2 * 1000).ToArray().Reverse().ToArray();
            return new MemoryStream(stream_array);
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
