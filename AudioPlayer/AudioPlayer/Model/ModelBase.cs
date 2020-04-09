using ReactiveUI;

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AudioPlayer.Model
{
    // TODO: Figure out a better way...
    public interface IModelBase { }

    [Serializable]
    public class ModelBase<TModel> : ReactiveObject, IModelBase
    {
        public ModelBase() { }

        protected void Update<T>(ref T value, T newValue, [CallerMemberName] string propertyName = null)
        {
            this.RaiseAndSetIfChanged(ref value, newValue, propertyName);
        }

        protected void OnPropertyChanged<TProperty>(Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;

            this.RaisePropertyChanged(propertyInfo.Name);
        }
    }
}
