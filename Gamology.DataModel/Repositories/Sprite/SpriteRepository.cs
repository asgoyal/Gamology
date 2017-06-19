using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.DataModel.Models;
using GamologySprite = Gamology.DataModel.Models.Sprite;

namespace Gamology.DataModel.Repositories.Sprite
{
    public class SpriteRepository : SQLServerEntityRepository<GamologySprite>, ISpriteRepository
    {
        public override DbSet<GamologySprite> DbSet => DbContext.Sprites;
    }
}
