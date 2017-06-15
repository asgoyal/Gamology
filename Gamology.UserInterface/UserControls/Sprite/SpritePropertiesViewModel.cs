using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils;

namespace Gamology.UserInterface.UserControls.Sprite
{
    class SpritePropertiesViewModel : BindableBase
    {
        public SpritePropertiesViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
        }
    }
}
