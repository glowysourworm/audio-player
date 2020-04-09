using AudioPlayer.Model.Database;
using System;
using System.IO;
using System.IO.Compression;

namespace AudioPlayer.Component
{
    /// <summary>
    /// Component for creating zip archive from library file
    /// </summary>
    public static class LibraryArchiver
    {
        const string LIBRARY_FILE = ".library";

        /// <summary>
        /// Saves library file to specified path with specified name (no extension)
        /// </summary>
        public static void Save(LibraryFile libraryFile, string folder)
        {
            var filePath = Path.Combine(folder, LIBRARY_FILE);

            using (var stream = File.Create(filePath))
            {
                using (var zipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    try
                    {
                        Serializer.Serialize(libraryFile, zipStream);
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
        public static LibraryFile Open(string folder)
        {
            var filePath = Path.Combine(folder, LIBRARY_FILE);

            var compressedBuffer = File.ReadAllBytes(filePath);

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
                            return Serializer.Deserialize<LibraryFile>(buffer);
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
