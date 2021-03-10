using Microsoft.Xna.Framework;

namespace Game1.GameManagment.Collisions
{
    class CollisionCone : ICollidable
    {
        public CollisionCone()
        {

        }

        public virtual bool CollidesWith(ICollidable collidable)
        {
            return true;
        }

        public virtual bool Contains(int x, int y)
        {
            return true;
        }

        public virtual bool Contains(Point p)
        {
            return true;
        }

        public virtual Vector2 IntersectionDepth(ICollidable collidable)
        {
            return Vector2.Zero;
        }
    }
}
