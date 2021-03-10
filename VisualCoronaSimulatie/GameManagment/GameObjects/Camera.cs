using Microsoft.Xna.Framework;

namespace Game1.GameManagment.GameObjects
{
    public class Camera : GameObject
    {
        Point screensize;

        public Camera(int layer = 0, string id = "")
            : base(layer , id)
        {

        }

        public void GotoPosition(Vector2 position, float distance)
        {

        }

        public void GotoPosition(Vector2 position, Vector2 speed)
        {

        }

        public bool OnScreen()
        {
            return false;
        }

        public Point ScreenSize
        {
            get { return screensize; }
            set { screensize = value; }
        }
    }
}