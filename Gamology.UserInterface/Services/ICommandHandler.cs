using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gamology.UserInterface.Services
{
    public interface ICommandHandler
    {
        ICommand RegisterCommand(string commandName, ICommand command);
        ICommand RegisterAction<TParam>(string commandName, Action<TParam> action, Func<bool> canExecute = null);
        ICommand RegisterAction(string commandName, Action action, Func<bool> canExecute = null);
        void DelegateAction(string commandName, object parameter);
    }
}
