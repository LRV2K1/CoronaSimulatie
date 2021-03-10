using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.GameManagment.GameObjects
{
    public interface IGameLoopObject
    {
        void Load();

        void UnLoad();

        void HandleInput(InputHelper inputHelper);

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform);

        void Reset();
    }
}