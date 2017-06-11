using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Gamology.UserInterface.Utils.Commands
{
    public class GeneralizedRelayCommand<TAction, T> : ICommand
    {
        protected TAction TargetExecuteMethod;
        protected Func<bool> TargetCanExecuteMethod;

        public GeneralizedRelayCommand(TAction executeMethod, Func<bool> canExecuteMethod = null)
        {
            TargetExecuteMethod = executeMethod;
            TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            if (TargetCanExecuteMethod != null)
            {
                return TargetCanExecuteMethod();
            }

            if (TargetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public virtual void Execute(object parameter)
        {
            if (TargetExecuteMethod == null)
            {
                return;
            }

            if (typeof(TAction) == typeof(Action))
            {
                var action = TargetExecuteMethod as Action;
                action();
            }
            else
            {
                var action = TargetExecuteMethod as Action<T>;
                action((T)parameter);
            }
        }
    }
}
