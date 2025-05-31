using Avalonia.Media.Imaging;

using System;
using System.IO;
using System.Runtime.Serialization;

using TagLib;

namespace AudioPlayer.Model.Database
{
    [Serializable]
    public class SerializableBitmap : Bitmap, ISerializable
    {
        public SerializableBitmap(string fileName) : base(fileName)
        {
        }

        public SerializableBitmap(Stream stream) : base(stream)
        {

        }

        public static SerializableBitmap ReadIPicture(IPicture picture)
        {
            using (var stream = new MemoryStream(picture.Data.Data))
            {
                return new SerializableBitmap(stream);
            }
        }

        public SerializableBitmap(SerializationInfo info, StreamingContext context) :
            this(new MemoryStream((byte[])info.GetValue("Buffer", typeof(byte[]))))
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            using (var stream = new MemoryStream())
            {
                Save(stream);

                info.AddValue("Buffer", stream.GetBuffer());
            }
        }
    }
}
