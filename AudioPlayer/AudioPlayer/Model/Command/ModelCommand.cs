using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AudioPlayer.Model.Command
{
    public class ModelCommand : ModelBase, ICommand
    {
        Action _action;
        Func<bool> _canExecute;

        bool _isReady;

        public bool IsReady
        {
            get { return _isReady; }
            set { this.SetProperty(ref _isReady, value); }
        }

        public ModelCommand(Action action)
        {
            _action = action;
            this.IsReady = true;
        }

        public ModelCommand(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event System.EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            this.IsReady = _canExecute == null ? true : _canExecute();

            return this.IsReady;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            this.IsReady = _canExecute == null ? true : _canExecute();

            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, new EventArgs());
        }
    }

    public class ModelCommand<T> : ModelBase, ICommand
    {
        Action<T> _action;
        Func<T, bool> _canExecute;

        bool _isReady;

        public bool IsReady
        {
            get { return _isReady; }
            set { this.SetProperty(ref _isReady, value); }
        }

        public ModelCommand(Action<T> action)
        {
            _action = action;
            this.IsReady = true;
        }

        public ModelCommand(Action<T> action, Func<T, bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event System.EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            this.IsReady = _canExecute == null ? true : _canExecute((T)parameter);

            return this.IsReady;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action.Invoke((T)parameter);
        }

        public void RaiseCanExecuteChanged(T parameter)
        {
            this.IsReady = _canExecute == null ? true : _canExecute(parameter);

            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, new EventArgs());
        }
    }
}
