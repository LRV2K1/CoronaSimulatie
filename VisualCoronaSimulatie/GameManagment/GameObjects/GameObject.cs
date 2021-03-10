using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1.GameManagment.GameObjects
{
    public abstract class GameObject : IGameLoopObject
    {
        protected GameObject parent;
        protected int layer;
        protected string id;
        protected bool visible;
        protected Matrix transform;

        Vector3 position, velocity;
        float rotation, scale;

        public GameObject(int layer = 0, string id = "")
        {
            this.id = id;
            this.layer = layer;
            position = Vector3.Zero;
            velocity = Vector3.Zero;
            visible = true;
            parent = null;
            rotation = 0;
            scale = 1;

            UpdateTransform();
        }

        public virtual void HandleInput(InputHelper inputHelper) { }

        public virtual void Update(GameTime gameTime) 
        {
            if (velocity != Vector3.Zero)
                Position3 += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected virtual void UpdateTransform()
        {
            transform = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(position);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform) { }

        public virtual void Reset() { }

        public virtual void Load() { }

        public virtual void UnLoad() { }

        /// <summary>
        /// The position of the gameobject.
        /// </summary>
        public virtual Vector2 Position2
        {
            get { return new Vector2(position.X, position.Y); }
            set { position.X = value.X; position.Y = value.Y; UpdateTransform(); }
        }

        /// <summary>
        /// The position of the gameobject.
        /// </summary>
        public virtual Vector3 Position3
        {
            get { return position; }
            set { position = value; UpdateTransform(); }
        }

        /// <summary>
        /// The velocity of the gameobject.
        /// </summary>
        public virtual Vector2 Velocity2
        {
            get { return new Vector2(velocity.X, velocity.Y); }
            set { velocity.X = value.X; velocity.Y = value.Y; }
        }

        /// <summary>
        /// The velocity of the gameobject.
        /// </summary>
        public virtual Vector3 Velocity3
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// The scale of the gameobject.
        /// </summary>
        public virtual float Scale
        {
            get { return scale; }
            set { scale = value; UpdateTransform(); }
        }

        /// <summary>
        /// The roation of the gameobject.
        /// </summary>
        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value % (float)(Math.PI * 2); UpdateTransform(); }
        }

        /// <summary>
        /// The transform of the gameobject.
        /// </summary>
        public virtual Matrix Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        /// <summary>
        /// Returns the GlobalTransform of the gameobject in the world.
        /// </summary>
        public virtual Matrix GlobalTransform
        {
            get
            {
                if (parent != null)
                    return transform * parent.GlobalTransform;
                return transform;
            }
        }

        /// <summary>
        /// Returns the GlobalPosition of the gameobject in the world.
        /// </summary>
        public virtual Vector2 GlobalPosition
        {
            get { return new Vector2(GlobalTransform.Translation.X, GlobalTransform.Translation.Y); }
        }

        /// <summary>
        /// Returns the GlobalScale of the gameobject in the world.
        /// </summary>
        public virtual float GlobalScale
        {
            get
            {
                if (parent != null)
                {
                    return parent.GlobalScale * scale;
                }
                return scale;
            }
        }

        /// <summary>
        /// Returns the GlobalRotation of the gameobject in the world.
        /// </summary>
        public virtual float GlobalRotation
        {
            get
            {
                if (parent != null)
                {
                    return (parent.GlobalRotation + rotation) % (float)(Math.PI * 2);
                }
                return rotation;
            }
        }

        /// <summary>
        /// Returns the parent of the gameobject.
        /// </summary>
        public virtual GameObject Parent
        {
            get { return parent; }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// Returns the root gameobject, most times the gamestate.
        /// </summary>
        public virtual GameObject Root
        {
            get
            {
                if (parent != null)
                {
                    return parent.Root;
                }
                return this;
            }
        }

        /// <summary>
        /// Id of the gameobject.
        /// </summary>
        public virtual string ID
        {
            get { return id; }
        }

        /// <summary>
        /// The layer the gameobject is on.
        /// </summary>
        public virtual int Layer
        {
            get { return layer; }
        }

        /// <summary>
        /// Determense the gameobjects visibility.
        /// </summary>
        public virtual bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// BoundingBox of a gameobject.
        /// </summary>
        public virtual Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 0, 0);
            }
        }
    }
}
