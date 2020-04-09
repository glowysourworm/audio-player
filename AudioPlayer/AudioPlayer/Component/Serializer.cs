using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AudioPlayer.Component
{
    public static class Serializer
    {
        /// <summary>
        /// NOTE*** No try / catch - please wrap execution
        /// </summary>
        public static void Serialize<T>(T graph, string file)
        {
            var formatter = new BinaryFormatter();

            if (File.Exists(file))
                File.Delete(file);

            using (var stream = File.OpenWrite(file))
            {
                formatter.Serialize(stream, graph);
            }
        }

        /// <summary>
        /// NOTE*** No try / catch - please wrap execution
        /// </summary>
        public static void Serialize<T>(T graph, Stream stream)
        {
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, graph);
        }

        /// <summary>
        /// NOTE*** No try / catch - please wrap execution
        /// </summary>
        public static T Deserialize<T>(string file)
        {
            var formatter = new BinaryFormatter();

            using (var stream = File.OpenRead(file))
            {
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// NOTE*** No try / catch - please wrap execution
        /// </summary>
        public static T Deserialize<T>(Stream stream)
        {
            var formatter = new BinaryFormatter();

            return (T)formatter.Deserialize(stream);
        }

        /// <summary>
        /// NOTE*** No try / catch - please wrap execution
        /// </summary>
        public static T Deserialize<T>(byte[] buffer)
        {
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream(buffer))
            {
                return (T)formatter.Deserialize(stream);
            }
        }

    }
}
