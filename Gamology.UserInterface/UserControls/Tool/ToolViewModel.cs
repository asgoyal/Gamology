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
using Microsoft.Practices.Unity;
using GamologySprite = Gamology.DataModel.Models.Sprite;

namespace Gamology.UserInterface.UserControls.Tool
{
    public class ToolViewModel : BindableBase
    {
        private SpritePropertiesViewModel _spritePropertiesViewModel;
        private SpriteService _spriteService;

        private BindableBase _localRegion;
        public BindableBase LocalRegion
        {
            get { return _localRegion; }
            set { SetProperty(ref _localRegion, value); }
        }

        public ToolViewModel(ICommandHandler commandHandler)
            : base(commandHandler)
        {
            _spritePropertiesViewModel = ContainerHelper.Container.Resolve<SpritePropertiesViewModel>();
            _spriteService = ContainerHelper.Container.Resolve<SpriteService>();
        }

        protected override void RegisterActions(ICommandHandler handler)
        {
            handler.RegisterAction(CommandNames.AddNewSpriteCommand, AddNewSprite);
            handler.RegisterAction<string>(CommandNames.ToolCancelCommand, Close);
            handler.RegisterAction<GamologySprite>(CommandNames.EditSpriteCommand, EditSprite);
        }

        #region Command handlers
        private void AddNewSprite()
        {
            _spritePropertiesViewModel.EditMode = false;
            _spritePropertiesViewModel.SetSprite(_spriteService.CreateNewSprite());
            NavigateTo(ViewNames.SpritePropertiesView);
        }

        private void EditSprite(GamologySprite spriteToEdit)
        {
            _spritePropertiesViewModel.EditMode = true;
            _spritePropertiesViewModel.SetSprite(_spriteService.GetSprite(spriteToEdit.Name));
            NavigateTo(ViewNames.SpritePropertiesView);
        }

        private void Close(string currentView)
        {
            NavigateTo(currentView, true);
        }

        private void NavigateTo(string destinationView)
        {
            NavigateTo(destinationView, false);
        }

        private void NavigateTo(string destinationView, bool cancel)
        {
            switch (destinationView)
            {
                case ViewNames.SpritePropertiesView:
                    LocalRegion = cancel ? null : _spritePropertiesViewModel;
                    break;
            }
        }
        #endregion
    }
}
