using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1.GameManagment.Assets
{
    public abstract class SingleDecorator : IDrawable
    {
        protected IDrawable drawable;

        public SingleDecorator(IDrawable drawable)
        {
            if (drawable == null)
                throw new ArgumentNullException("drawable is null!");
            this.drawable = drawable;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Matrix transform, GameTime gameTime)
        {
            drawable.Draw(spriteBatch, transform, gameTime);
        }

        public virtual Vector2 Origin
        {
            get { return drawable.Origin; }
            set { drawable.Origin = value; }
        }

        public virtual Vector2 Size
        {
            get { return drawable.Size; }
        }

        public virtual Vector2 Center
        {
            get { return drawable.Center; }
        }

        public virtual Color Color
        {
            get { return drawable.Color; }
            set { drawable.Color = value; }
        }

        public virtual Rectangle SpritePart
        {
            get { return drawable.SpritePart; }
            set { drawable.SpritePart = value; }
        }

        public virtual SpriteEffects SpriteEffects
        {
            get { return drawable.SpriteEffects; }
            set { drawable.SpriteEffects = value; }
        }

        public virtual Effect Shader
        {
            get { return drawable.Shader; }
            set { drawable.Shader = value; }
        }

        public virtual IDrawable Base
        {
            get { return drawable; }
        }
    }
}