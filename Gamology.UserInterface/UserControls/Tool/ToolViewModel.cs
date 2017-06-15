using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.UserControls.Sprite;
using Gamology.UserInterface.Utils;
using Gamology.UserInterface.Utils.Constants;
using Gamology.UserInterface.Utils.Helpers;

namespace Gamology.UserInterface.UserControls.Tool
{
    public class ToolViewModel : BindableBase
    {
        private BindableBase _localRegion;
        public BindableBase LocalRegion
        {
            get { return _localRegion; }
            set { SetProperty(ref _localRegion, value); }
        }

        public ToolViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
        }

        protected override void RegisterActions(ICommandHandler handler)
        {
            var service = new ToolService(this);
            handler.RegisterAction<string>(CommandNames.AddNewSpriteCommand, service.NavigateTo);
        }
    }
}
