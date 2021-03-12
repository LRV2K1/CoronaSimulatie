using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assets = Engine.GameManagment.Assets;

namespace Engine.GameManagment.GameObjects
{
    public class DrawGameObject : GameObject
    {
        protected Assets.IDrawable drawable;

        public DrawGameObject(Assets.IDrawable drawable = null, int layer = 0, string id = "")
            : base(layer, id)
        {
            this.drawable = drawable;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            if (!visible || drawable == null)
                return;

            Matrix newtransform = this.transform * transform;
            drawable.Draw(spriteBatch, newtransform, gameTime);
        }

        public Assets.IDrawable Drawable
        {
            get { return drawable; }
            set { drawable = value; }
        }
    }
}
