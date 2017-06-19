using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.DataModel.Models;

namespace Gamology.UserInterface.Services
{
    public class SpriteService
    {
        private Dictionary<string, Sprite> _spriteLookup = new Dictionary<string, Sprite>();

        public Sprite GetSprite(string spriteName)
        {
            Sprite sprite;
            if (!_spriteLookup.TryGetValue(spriteName, out sprite))
            {
                return null;
            }

            return sprite;
        }

        public void AddOrUpdateSprite(Sprite sprite)
        {
            // TODO before adding to dictionary save the sprite in repository
            _spriteLookup[sprite.Name] = sprite;
        }

        public Sprite CreateNewSprite()
        {
            // TODO this should use the repository way of creating the new sprite
            return new Sprite()
            {
                Name = "New Sprite"
            };
        }

        public ICollection<Sprite> Sprites => _spriteLookup.Values;
    }
}
