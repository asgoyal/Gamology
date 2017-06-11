using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils.Commands;

namespace Gamology.UserInterface.Utils
{
    public class BindableBase : INotifyPropertyChanged
    {
        public InterpretingCommand Command { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ICommandHandler _commandHandler;
        protected ICommandHandler CommandHandler => _commandHandler;

        public BindableBase(ICommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            RegisterActions(CommandHandler);
            Command = new InterpretingCommand(_commandHandler as ICommand);
        }

        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(member, val))
            {
                return;
            }

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RegisterActions(ICommandHandler handler)
        {
            return;
        }
    }
}
