using System;
using System.IO;
using System.IO.Compression;

using AudioPlayer.Model;

namespace AudioPlayer.Component
{
    /// <summary>
    /// Component for creating zip archive from library file
    /// </summary>
    public static class LibraryArchiver
    {
        /// <summary>
        /// Saves library file to specified path with specified name (no extension)
        /// </summary>
        public static void Save(Library library, string fileName)
        {
            using (var stream = File.Create(fileName))
            {
                using (var zipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    try
                    {
                        Serializer.Serialize(library, zipStream);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Opens library file from specified path
        /// </summary>
        public static Library Open(string fileName)
        {
            var compressedBuffer = File.ReadAllBytes(fileName);

            using (var inputStream = new MemoryStream(compressedBuffer))
            {
                using (var zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        zipStream.Flush();
                        zipStream.Close();

                        var buffer = resultStream.GetBuffer();

                        try
                        {
                            return Serializer.Deserialize<Library>(buffer);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
