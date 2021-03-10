using System.Collections.Generic;
using Game1.GameManagment.Assets;

namespace Game1.GameManagment.GameObjects
{
    public class MultiDrawGameObject : DrawGameObject
    {
        protected Dictionary<string, IDrawable> drawables;
        string currentid;

        public MultiDrawGameObject(int layer, string id)
            : base(null, layer, id)
        {
            drawables = new Dictionary<string, IDrawable>();
            currentid = "";
        }

        /// <summary>
        /// Add a drawable to the list.
        /// </summary>
        /// <param name="id">Id of the drawable, used to activate it later.</param>
        /// <param name="drawable">The drawable to be added.</param>
        public void Add(string id, IDrawable drawable)
        {
            drawables[id] = drawable;
        }

        /// <summary>
        /// Remove a drawable from the list.
        /// </summary>
        /// <param name="id">The id of the drawable to be removed.</param>
        public void Remove(string id)
        {
            if (drawables.ContainsKey(id))
                drawables.Remove(id);
        }

        /// <summary>
        /// Switch to the drawable with the give id.
        /// </summary>
        /// <param name="id">The id of the drawable that needs to be switched to.</param>
        public void Switch(string id)
        {
            if (drawables.ContainsKey(id))
                drawable = drawables[id];
        }

        /// <summary>
        /// Checks if the id is already in the list.
        /// </summary>
        /// <param name="id">The id of the drawable to be checked.</param>
        /// <returns></returns>
        public bool Contains(string id)
        {
            return drawables.ContainsKey(id);
        }

        /// <summary>
        /// Returns the drawable belonging to the id, returns null if the id does not exist.
        /// </summary>
        /// <param name="id">The id of the drawable.</param>
        /// <returns></returns>
        public IDrawable GetDrawable(string id)
        {
            if (drawables.ContainsKey(id))
                return drawables[id];
            return null;
        }

        /// <summary>
        /// Id of the current active drawable, "" at the start.
        /// </summary>
        public string CurrentID
        {
            get { return currentid; }
        }

        /// <summary>
        /// The currently active drawable.
        /// </summary>
        public IDrawable CurrentDrawable
        {
            get { return drawable; }
        }

        /// <summary>
        /// The list with all the stored drawables.
        /// </summary>
        public Dictionary<string, IDrawable> Drawables
        {
            get { return drawables; }
        }


    }
}
