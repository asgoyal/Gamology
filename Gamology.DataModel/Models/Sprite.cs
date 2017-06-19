using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamology.DataModel.Models
{
    public class Sprite : IModificationHistory
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsDirty { get; set; }

        public bool IsSolid { get; set; }

        public string TextureFilename { get; set; }
    }
}
