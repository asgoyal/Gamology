using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils;

namespace Gamology.UserInterface.UserControls.Canvas
{
    class CanvasViewModel : BindableBase
    {
        public CanvasViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
        }
    }
}
