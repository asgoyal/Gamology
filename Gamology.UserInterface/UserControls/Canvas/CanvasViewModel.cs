using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils;
using Gamology.UserInterface.Utils.Constants;
using Gamology.UserInterface.Utils.Helpers;
using Microsoft.Practices.Unity;
using GamologySprite = Gamology.DataModel.Models.Sprite;

namespace Gamology.UserInterface.UserControls.Canvas
{
    public class CanvasViewModel : BindableBase
    {
        private SpriteService _spriteService;
        
        public ObservableCollection<GamologySprite> Sprites => new ObservableCollection<GamologySprite>(_spriteService.Sprites);

        public CanvasViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
            _spriteService = ContainerHelper.Container.Resolve<SpriteService>();
        }

        protected override void RegisterActions(ICommandHandler handler)
        {
            handler.RegisterAction(CommandNames.CanvasRefreshCommand, Refresh);
        }

        public event Action OnRefresh = delegate { };

        private void Refresh()
        {
            OnRefresh();
        }
    }
}
