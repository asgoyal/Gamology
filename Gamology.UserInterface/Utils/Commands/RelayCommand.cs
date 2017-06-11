using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamology.UserInterface.Utils.Commands
{
    public class RelayCommand : GeneralizedRelayCommand<Action, object>
    {
        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod = null)
            : base(executeMethod, canExecuteMethod)
        {
        }
    }

    public class RelayCommand<T> : GeneralizedRelayCommand<Action<T>, T>
    {
        public RelayCommand(Action<T> executeMethod, Func<bool> canExecuteMethod = null)
            : base(executeMethod, canExecuteMethod)
        {
        }
    }
}
