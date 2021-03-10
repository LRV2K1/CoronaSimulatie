using Microsoft.Xna.Framework;

namespace Game1.GameManagment.Collisions
{
    public interface ICollidable
    {
        bool CollidesWith(ICollidable collidable);

        bool Contains(int x, int y);

        bool Contains(Point p);

        Vector2 IntersectionDepth(ICollidable collidable);
    }
}
