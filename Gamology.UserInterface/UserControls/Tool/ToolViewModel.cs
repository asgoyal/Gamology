using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils;

namespace Gamology.UserInterface.UserControls.Tool
{
    class ToolViewModel : BindableBase
    {
        public ToolViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
        }
    }
}
