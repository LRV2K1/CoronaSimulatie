using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.GameManagment.GameObjects
{
    public class GameObjectGrid<T> : GameObject where T : GameObject
    {
        protected T[,] grid;
        protected Dictionary<string, Point> ids;
        protected int cellWidth, cellHeight;

        public GameObjectGrid(int columns, int rows, int layer = 0, string id = "")
            : base(layer, id)
        {
            cellWidth = 0;
            cellHeight = 0;

            grid = new T[columns,rows];
            ids = new Dictionary<string, Point>();

            //initialize empty grid
            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                    grid[x, y] = null;
        }

        /// <summary>
        /// Returns the grid of gameobjects.
        /// </summary>
        public T[,] Grid
        {
            get { return grid; }
        }
        
        /// <summary>
        /// Returns the dictionary with the gameobject positions in the grid with their ids.
        /// </summary>
        public Dictionary<string, Point> IDS
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
                return Find(ids[id].X, ids[id].Y);
            return null;
        }

        /// <summary>
        /// Returns the gameobject stored in the specified grid position.
        /// </summary>
        /// <param name="x">The x position in the grid.</param>
        /// <param name="y">The y position in the grid.</param>
        /// <returns>Returns the gameobject in the specified position, if the point is outside the grid it returns null.</returns>
        public virtual T Find(int x, int y)
        {
            if (x < grid.GetLength(0) && y < grid.GetLength(1) && x >= 0 && y >= 0)
                return grid[x, y];
            return null;
        }

        /// <summary>
        /// Returns the gameobject stored in the specified grid position.
        /// </summary>
        /// <param name="p">The position in the grid.</param>
        /// <returns>Returns the gameobject in the specified position, if the point is outside the grid it returns null.</returns>
        public virtual T Find(Point p)
        { return Find(p.X, p.Y); }

        /// <summary>
        /// Returns the gameobject's location in the grid based on it's id.
        /// </summary>
        /// <param name="id">The id of the gameobject.</param>
        /// <returns>Returns the location of the gamobject as a point, if the gameobject does not exist it returns a -one point.</returns>
        public virtual Point FindLoc(string id)
        {
            if (ids.ContainsKey(id))
                return ids[id];
            return new Point(-1, -1);
        }

        /// <summary>
        /// Returns the gameobject's location in the grid based on the gameobject.
        /// Slow!
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns the location of the gameobject as a point, if the gameobject does not exist it returns a - one point.</returns>
        public virtual Point FindLoc(GameObject obj)
        {
            //check for id.
            if (ids.ContainsKey(obj.ID))
                return FindLoc(obj.ID);

            //check world location.
            int px = (int)((obj.Position2.X) / cellWidth);
            int py = (int)((obj.Position2.Y) / CellHeight);
            if (Find(px,py) == obj)
                return new Point(px, py);

            //go over every object in the grid.
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] == obj)
                        return new Point(x, y);

            return new Point(-1, -1);
        }

        /// <summary>
        /// Add a gameobject to a specific location in the grid.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        /// <param name="x">The x position in the grid.</param>
        /// <param name="y">The y position in the grid.</param>
        /// <param name="cellsize">True if the position of the object needs to be changed based on the position of the grid and the cellsize.</param>
        public virtual void Add(T obj, int x, int y , bool cellsize = true)
        {
            if (x < grid.GetLength(0) && y < grid.GetLength(1) && x >= 0 && y >= 0)
            {
                //Remove old object.
                if (grid[x, y] != null)
                    Remove(x, y);

                obj.Parent = this;
                grid[x, y] = obj;

                if (obj.ID != "")
                    ids[obj.ID] = new Point(x,y);

                if (cellsize)
                    obj.Position2 = new Vector2(x * cellWidth, y * cellHeight);
            }
        }
        
        /// <summary>
        /// Add a gameobject to a specific location in the grid.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        /// <param name="p">The position in the grid.</param>
        /// <param name="cellsize">True if the position of the object needs to be changed based on the position of the grid and the cellsize.</param>
        public virtual void Add(T obj, Point p, bool cellsize = true)
        { Add(obj, p.X, p.Y, cellsize); }

        /// <summary>
        /// Remove an object based on it's position in the grid.
        /// </summary>
        /// <param name="x">The x position in the grid.</param>
        /// <param name="y">The y position in the grid.</param>
        public virtual void Remove(int x, int y)
        {
            if (x < grid.GetLength(0) && y < grid.GetLength(1) && x >= 0 && y >= 0)
            {
                grid[x, y].Parent = null;

                //Remove id from dictionary.
                if (ids.ContainsKey(grid[x, y].ID))
                    ids.Remove(grid[x, y].ID);

                grid[x, y] = null;
            }
        }

        /// <summary>
        /// Remove an object based on it's position in the grid.
        /// </summary>
        /// <param name="p">The position in the grid.</param>
        public virtual void Remove(Point p)
        { Remove(p.X, p.Y); }

        /// <summary>
        /// Remove an object based on it's id.
        /// </summary>
        /// <param name="id">The id of the object.</param>
        public virtual void Remove(string id)
        {
            if (ids.ContainsKey(id))
                Remove(ids[id].X, ids[id].Y);
        }

        /// <summary>
        /// Remove an object based on the object.
        /// Slow!
        /// </summary>
        /// <param name="obj">The object to be removed.</param>
        public virtual void Remove(T obj)
        {
            Point p = FindLoc(obj);
            Remove(p);
        }

        /// <summary>
        /// Resizes the grid to a new size, this also clears the entire grid.
        /// </summary>
        /// <param name="columns">The number of columns in the grid (X).</param>
        /// <param name="rows">The number of rows in the grid (Y).</param>
        public virtual void Resize(int columns, int rows)
        {
            Clear();

            grid = new T[columns, rows];

            //initialize empty grid
            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                    grid[x, y] = null;
        }

        /// <summary>
        /// Reposition all gameobjects based on the cellsize.
        /// </summary>
        public virtual void Reposition()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x,y] != null)
                        grid[x,y].Position2 = new Vector2(x * cellWidth, y * cellHeight);
        }

        /// <summary>
        /// Clears the entire grid.
        /// </summary>
        public virtual void Clear()
        {
            ids.Clear();
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    Remove(x, y);
        }

        /// <summary>
        /// The number of columns in the grid (X).
        /// </summary>
        public int Columns
        {
            get { return grid.GetLength(0); }
        }

        /// <summary>
        /// The number of rows in the grid (Y).
        /// </summary>
        public int Rows
        {
            get { return grid.GetLength(1); }
        }

        /// <summary>
        /// The width of a cell, used to determine object positions in the grid.
        /// </summary>
        public int CellWidth
        {
            get { return cellWidth; }
            set { CellHeight = value; }
        }

        /// <summary>
        /// The height of a cell, used to determine object positions in the grid.
        /// </summary>
        public int CellHeight
        {
            get { return cellHeight; }
            set { cellHeight = value; }
        }

        /// <summary>
        /// The size of a cell, used to determine object positions in the grid.
        /// </summary>
        public Point CellSize
        {
            get { return new Point(cellWidth, CellHeight); }
            set
            {
                cellWidth = value.X;
                CellHeight = value.Y;
            }
        }

        public override void Load()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x,y] != null)
                        grid[x, y].Load();
        }

        public override void UnLoad()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] != null)
                        grid[x, y].UnLoad();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] != null)
                        grid[x, y].HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] != null)
                        grid[x, y].Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            Matrix newtransform = this.transform * transform;
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] != null)
                        grid[x, y].Draw(gameTime, spriteBatch, newtransform);
        }

        public override void Reset()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] != null)
                        grid[x, y].Reset();
        }
    }
}