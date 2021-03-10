using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1.GameManagment.GameObjects
{
    public class GameObjectList<T> : GameObject where T: GameObject
    {
        protected List<T> objects;
        protected Dictionary<string, T> ids;

        public GameObjectList(int layer = 0, string id = "")
            : base(layer, id)
        {
            objects = new List<T>();
            ids = new Dictionary<string, T>();
        }

        /// <summary>
        /// Returns the list with gameobjects.
        /// </summary>
        public List<T> Objects
        {
            get { return objects; }
        }

        /// <summary>
        /// Returns the dictionary with the gameobjects with their ids.
        /// </summary>
        public Dictionary<string, T> IDS
        {
            get { return ids; }
        }

        /// <summary>
        /// Returns the gameobject based on it's id.
        /// </summary>
        /// <param name="id">The id of the gameobject.</param>
        /// <returns>Returns the gameobject, if the gameobject does not exist it returns null.</returns>
        public virtual T Find(string id)
        {
            if (ids.ContainsKey(id))
                return ids[id];
            return null;
        }

        /// <summary>
        /// Add a gameobject to the list, sorts the object based on layer.
        /// </summary>
        /// <param name="obj">Object to be added.</param>
        public virtual void Add(T obj)
        {
            obj.Parent = this;
            if (obj.ID != "")
                //store id values
                ids[obj.ID] = obj;

            for (int i = objects.Count; i > 0; i--)
            {
                //check for layer
                if (objects[i - 1].Layer <= obj.Layer)
                {
                    objects.Insert(i, obj);
                    return;
                }
            }
            objects.Insert(0, obj);
        }

        /// <summary>
        /// Add a gameobject to the list, does not sort.
        /// </summary>
        /// <param name="obj">Object to be added.</param>
        public virtual void QuickAdd(T obj)
        {
            obj.Parent = this;
            objects.Add(obj);

            if (obj.ID != "")
            {
                //store id values
                ids[obj.ID] = obj;
            }
        }

        /// <summary>
        /// Remove an object from the list.
        /// </summary>
        /// <param name="obj">The object to be removed.</param>
        public virtual void Remove(T obj)
        {
            obj.Parent = null;
            objects.Remove(obj);

            if (obj.ID != "" && ids.ContainsKey(obj.ID))
            {
                ids.Remove(obj.ID);
            }
        }

        /// <summary>
        /// Remove an object based on it's id.
        /// </summary>
        /// <param name="id">The id of the object to be removed.</param>
        public virtual void Remove(string id)
        {
            if (id != "" && !ids.ContainsKey(id))
            {
                return;
            }

            T obj = ids[id];
            ids.Remove(id);
            obj.Parent = null;
            objects.Remove(obj);
        }

        /// <summary>
        /// Sort the list of objects based on layer. Uses bucket sort.
        /// </summary>
        public virtual void Sort()
        {
            //make buckets
            Dictionary<int, List<T>> buckets;
            buckets = new Dictionary<int, List<T>>();

            //fill buckets
            for (int i = 0; i < objects.Count; i++)
            {
                if (!buckets.ContainsKey(objects[i].Layer))
                {
                    buckets.Add(objects[i].Layer, new List<T>());
                }
                buckets[objects[i].Layer].Add(objects[i]);
            }

            //sort buckets /insertion sort/
            List<KeyValuePair<int, List<T>>> sortedbuckets;
            sortedbuckets = new List<KeyValuePair<int, List<T>>>();
            foreach(KeyValuePair<int, List<T>> pair in buckets)
            {
                bool inserted = false;
                for (int i = 0; i < sortedbuckets.Count; i++)
                {
                    if (sortedbuckets[i].Key > pair.Key)
                    {
                        sortedbuckets.Insert(i, pair);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                    sortedbuckets.Add(pair);
            }

            //copy buckets to original list
            objects.Clear();
            for (int i = 0; i < sortedbuckets.Count; i++)
            {
                for (int j = 0; j < sortedbuckets[i].Value.Count; j++)
                {
                    objects.Add(sortedbuckets[i].Value[j]);
                }
            }
        }

        /// <summary>
        /// Clears all items from the list.
        /// </summary>
        public virtual void Clear()
        {
            ids.Clear();
            for (int i = objects.Count - 1; i >= 0; i--)
                Remove(objects[i]);
            objects.Clear();
        }

        public override void Load()
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i] != null)
                    objects[i].Load();
        }

        public override void UnLoad()
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i] != null)
                    objects[i].UnLoad();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i] != null)
                    objects[i].HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i] != null)
                    objects[i].Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            Matrix newtransform = this.transform * transform;
            for (int i = 0; i < objects.Count; i++)
                if (objects[i] != null)
                    objects[i].Draw(gameTime, spriteBatch, newtransform);
        }

        public override void Reset()
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i] != null)
                    objects[i].Reset();
        }
    }
}