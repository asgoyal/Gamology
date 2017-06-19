using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.Utils;

namespace Gamology.UserInterface.UserControls.Sprite
{
    public class SimpleEditableSprite : BindableBase
    {
        public SimpleEditableSprite(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
        }

        private long _id;
        public long Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private float _positionX;
        public float PositionX
        {
            get { return _positionX; }
            set { SetProperty(ref _positionX, value); }
        }

        private float _positionY;
        public float PositionY
        {
            get { return _positionY; }
            set { SetProperty(ref _positionY, value); }
        }

        private string _textureFilename;
        public string TextureFilename
        {
            get { return _textureFilename; }
            set { SetProperty(ref _textureFilename, value); }
        }

        private bool _isSolid;
        public bool IsSolid
        {
            get { return _isSolid; }
            set { SetProperty(ref _isSolid, value); }
        }
    }
}
