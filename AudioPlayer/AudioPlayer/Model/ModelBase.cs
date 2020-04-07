using ReactiveUI;
using System;
using System.Runtime.CompilerServices;

namespace AudioPlayer.Model
{
    [Serializable]
    public class ModelBase : ReactiveObject
    {
        public ModelBase() { }

        public void Update<T>(ref T value, T newValue, [CallerMemberName] string propertyName = null)
        {
            this.RaiseAndSetIfChanged(ref value, newValue, propertyName);
        }
    }
}
