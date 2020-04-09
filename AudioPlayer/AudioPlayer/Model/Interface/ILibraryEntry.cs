using System;
using System.Runtime.Serialization;

namespace AudioPlayer.Model.Interface
{
    public interface ILibraryEntry : ILibraryMetaEntry, ISerializable
    {
        public string FileName { get; }
    }
}
