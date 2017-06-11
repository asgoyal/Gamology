using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Gamology.UserInterface.Utils.Commands
{
    public class InterpretingCommand : ICommand
    {
        private ICommand _commandHandler;

        public event EventHandler CanExecuteChanged = delegate { };

        public InterpretingCommand(ICommand commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public bool CanExecute(object parameter)
        {
            return _commandHandler.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _commandHandler.Execute(parameter);
        }
    }
}
