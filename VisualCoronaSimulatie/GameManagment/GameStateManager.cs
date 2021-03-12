using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameManagment.GameObjects;

namespace Engine.GameManagment
{
    public class GameStateManager : IGameLoopObject
    {
        Dictionary<string, IGameLoopObject> gameStates;
        IGameLoopObject currentGameState;
        string currentID;

        public GameStateManager()
        {
            gameStates = new Dictionary<string, IGameLoopObject>();
            currentGameState = null;
            currentID = null;
        }

        /// <summary>
        /// Add a gamestate to the gamestate manager.
        /// </summary>
        /// <param name="name">Name of the gamestate.</param>
        /// <param name="state">The gamestate object.</param>
        public void AddGameState(string name, IGameLoopObject state)
        {
            gameStates[name] = state;
        }

        /// <summary>
        /// Removes the gamestate with the name from the list.
        /// </summary>
        /// <param name="name">Name of the gamestate to be removed.</param>
        public void RemoveGameState(string name)
        {
            if (gameStates.ContainsKey(name))
            {
                if (currentGameState == gameStates[name])
                {
                    currentID = null;
                    currentGameState = null;
                }
                gameStates.Remove(name);
            }
        }

        /// <summary>
        /// Clears the entire gamestate list.
        /// </summary>
        public void Clear()
        {
            foreach (KeyValuePair<string, IGameLoopObject> state in gameStates)
                state.Value.UnLoad();
            gameStates.Clear();
            currentGameState = null;
        }

        /// <summary>
        /// Returns the gamestate belonging to the name.
        /// </summary>
        /// <param name="name">Name of the gamestate.</param>
        /// <returns>Returns the gamestate, if the gamestate does not exist it returns null.</returns>
        public IGameLoopObject GetGameState(string name)
        {
            if (gameStates.ContainsKey(name))
            {
                return gameStates[name];
            }
            return null;
        }

        /// <summary>
        /// Switches the gamestate to the new gamestate, if the gamestate is not found no gamestate is switched.
        /// </summary>
        /// <param name="name">Name of the new gamestate.</param>
        public void SwitchTo(string name)
        {
            if (name != null && gameStates.ContainsKey(name))
            {
                UnLoad();
                currentID = name;
                currentGameState = gameStates[name];
                Load();
            }
            else
            {
                Console.WriteLine("Could not find game state: " + name);
            }
        }

        /// <summary>
        /// Returns the current active gamestate.
        /// </summary>
        public IGameLoopObject CurrentGameState
        {
            get
            {
                return currentGameState;
            }
        }

        /// <summary>
        /// The id of the current active gamestate.
        /// </summary>
        public string CurrentID
        {
            get { return currentID; }
        }

        public void Load() 
        {
            if (currentGameState != null)
            {
                currentGameState.Load();
            }
        }

        public void UnLoad() 
        {
            if (currentGameState != null)
            {
                currentGameState.UnLoad();
            }
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (currentGameState != null)
            {
                currentGameState.HandleInput(inputHelper);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (currentGameState != null)
            {
                currentGameState.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            if (currentGameState != null)
            {
                currentGameState.Draw(gameTime, spriteBatch, transform);
            }
        }

        public void Reset()
        {
            if (currentGameState != null)
            {
                currentGameState.Reset();
            }
        }
    }
}