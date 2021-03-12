using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.GameManagment.Assets
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch, Matrix transform, GameTime gameTime);

        Vector2 Origin { get; set; }

        Vector2 Size { get; }

        Vector2 Center { get; }

        Color Color { get; set; }

        Rectangle SpritePart { get; set; }

        SpriteEffects SpriteEffects { get; set; }

        Effect Shader { get; set; }

        IDrawable Base { get; }
    }
}