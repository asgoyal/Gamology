using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.DataModel.DatabaseContexts;
using GamologySprite = Gamology.DataModel.Models.Sprite;

namespace Gamology.DataModel.Repositories.Sprite
{
    public interface ISpriteRepository : IRepository<SQLServerDbContext, GamologySprite>
    {
    }
}
