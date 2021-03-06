using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameManagment.Assets;

namespace Engine.GameManagment.GameObjects
{
    public class SpriteGameObject : DrawGameObject
    {
        public SpriteGameObject(string assetName, int layer = 0, string id = "")
            : base(new SpriteSheet(assetName), layer, id)
        {
        }

        public SpriteGameObject(SpriteSheet sprite, int layer = 0, string id = "")
            : base(sprite, layer, id)
        {
        }

        /// <summary>
        /// The sprite.
        /// </summary>
        public SpriteSheet Sprite
        {
            get { return drawable as SpriteSheet; }
            set { drawable = value; }
        }
    }
}