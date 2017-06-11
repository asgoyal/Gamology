using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gamology.UserInterface.Utils.Commands;

namespace Gamology.UserInterface.Services
{
    public class DefaultCommandHandler : ICommandHandler, ICommand
    {
        private IDictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return true;
            }

            Tuple<string, object> parameters = parameter as Tuple<string, object>;
            string commandName = parameters.Item1;
            object parameterToPass = parameters.Item2;

            return getCommand(commandName).CanExecute(parameterToPass);
        }

        public void Execute(object parameter)
        {
            Tuple<string, object> parameters = parameter as Tuple<string, object>;
            string commandName = parameters.Item1;
            object parameterToPass = parameters.Item2;

            getCommand(commandName).Execute(parameterToPass);
        }

        public ICommand RegisterCommand(string commandName, ICommand command)
        {
            if (_commands.ContainsKey(commandName))
            {
                throw new NotSupportedException("Action Name: " + commandName + " already exists!");
            }

            _commands.Add(commandName, command);

            return command;
        }

        public ICommand RegisterAction<TParam>(string commandName, Action<TParam> action, Func<bool> canExecute = null)
        {
            return RegisterCommand(commandName, new RelayCommand<TParam>(action, canExecute));
        }

        public ICommand RegisterAction(string commandName, Action action, Func<bool> canExecute = null)
        {
            return RegisterCommand(commandName, new RelayCommand(action, canExecute));
        }

        private ICommand getCommand(string commandName)
        {
            ICommand command;
            if (!_commands.TryGetValue(commandName, out command))
            {
                throw new ArgumentException("Invalid Command: " + commandName + " not found");
            }

            return command;
        }

        public void DelegateAction(string commandName, object parameter)
        {
            ICommand command = getCommand(commandName);
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
    }
}
