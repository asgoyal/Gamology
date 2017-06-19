using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils;
using Gamology.UserInterface.Utils.Constants;
using Gamology.UserInterface.Utils.Helpers;
using Microsoft.Practices.Unity;
using GamologySprite = Gamology.DataModel.Models.Sprite;

namespace Gamology.UserInterface.UserControls.Sprite
{
    public class SpritePropertiesViewModel : BindableBase
    {
        private SpriteService _spriteService;

        public SpritePropertiesViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
            _spriteService = ContainerHelper.Container.Resolve<SpriteService>();
        }

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private GamologySprite _editingSprite = null;
        public void SetSprite(GamologySprite sprite)
        {
            _editingSprite = sprite;
            Sprite = new SimpleEditableSprite(this.CommandHandler);
            CopyItem(sprite, Sprite);
        }

        private SimpleEditableSprite _sprite;
        public SimpleEditableSprite Sprite
        {
            get { return _sprite; }
            set { SetProperty(ref _sprite, value); }
        }

        protected override void RegisterActions(ICommandHandler handler)
        {
            handler.RegisterAction<string>(CommandNames.SpritePropertiesCancelCommand, Close);
            handler.RegisterAction(CommandNames.SaveCommand, SaveSpriteProperties);
        }

        private void CopyItem(GamologySprite source, SimpleEditableSprite target)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            if (EditMode)
            {
                target.PositionX = source.PositionX;
                target.PositionY = source.PositionY;
                target.TextureFilename = source.TextureFilename;
                target.IsSolid = source.IsSolid;
            }
        }

        #region command handlers
        private void UpdateItem(SimpleEditableSprite source, GamologySprite target)
        {
            target.Name = source.Name;
            target.PositionX = source.PositionX;
            target.PositionY = source.PositionY;
            target.TextureFilename = source.TextureFilename;
            target.IsSolid = source.IsSolid;
        }

        private void SaveSpriteProperties()
        {
            UpdateItem(Sprite, _editingSprite);
            _spriteService.AddOrUpdateSprite(_editingSprite);
            CommandHandler.DelegateAction(CommandNames.CanvasRefreshCommand, "");
            CommandHandler.DelegateAction(CommandNames.EditSpriteCommand, _editingSprite);
        }

        private void Close(string currentView)
        {
            _editingSprite.IsDirty = true;
            // sprite properties view is passed so that the delegating command will know which view to close
            CommandHandler.DelegateAction(CommandNames.ToolCancelCommand, currentView);
        }
        #endregion
    }
}
