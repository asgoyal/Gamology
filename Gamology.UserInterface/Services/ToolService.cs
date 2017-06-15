using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.UserControls.Sprite;
using Gamology.UserInterface.UserControls.Tool;
using Gamology.UserInterface.Utils;
using Gamology.UserInterface.Utils.Constants;
using Gamology.UserInterface.Utils.Helpers;
using Microsoft.Practices.Unity;

namespace Gamology.UserInterface.Services
{
    public class ToolService : AbstractUserControlService<ToolViewModel>
    {
        public ToolService(ToolViewModel model) : base(model)
        {
        }

        public void AddNewSprite()
        {

        }

        public override void NavigateTo(string destinationView)
        {
            switch(destinationView)
            {
                case ViewNames.SpritePropertiesView:
                    Model.LocalRegion = ContainerHelper.Container.Resolve<SpritePropertiesViewModel>();
                    break;
            }
        }
    }
}
