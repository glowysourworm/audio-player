using ReactiveUI;

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AudioPlayer.Model
{
    [Serializable]
    public class ModelBase : ReactiveObject
    {
        public ModelBase() { }

        protected void Update<T>(ref T value, T newValue, [CallerMemberName] string propertyName = null)
        {
            this.RaiseAndSetIfChanged(ref value, newValue, propertyName);
        }
    }
}
