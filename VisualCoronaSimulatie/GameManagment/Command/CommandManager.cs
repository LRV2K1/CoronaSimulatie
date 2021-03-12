using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Engine.GameManagment.GameObjects;
using Engine.GameManagment.Assets;

namespace Engine.GameManagment.Command
{
    public class CommandManager : GameObject
    {
        LinkedListNode<TextLine> currentLine;

        string savedCommand;

        bool active;

        CommandLine commandLine;
        WrittenLines writtenLines;
        CommandExecuter commandExecuter;
        CommandExecuter currentExecuter;

        public CommandManager()
        {
            active = false;
            currentLine = null;
            Position2 = Vector2.Zero;
            savedCommand = "";

            commandExecuter = new CommandExecuter();
            currentExecuter = commandExecuter;

            AddCommand("clear", ClearL);
        }

        public void Initialize()
        {
            commandLine = new CommandLine("Fonts/Hud");
            writtenLines = new WrittenLines("Fonts/Hud");
        }

        /// <summary>
        /// Add a command to the list of commands.
        /// </summary>
        /// <param name="command">The command to activate the function.</param>
        /// <param name="function">The function to be activated.</param>
        public void AddCommand(string command, Func<string, bool> function)
        {
            commandExecuter.AddCommand(command, function);
        }

        /// <summary>
        /// Remove a command from the commands list.
        /// </summary>
        /// <param name="command">The command to be removed.</param>
        public void RemoveCommand(string command)
        {
            commandExecuter.RemoveCommand(command);
        }

        /// <summary>
        /// Clears all the preveous written lines.
        /// </summary>
        private bool ClearL(string command)
        {
            writtenLines.Reset();
            return true;
        }

        /// <summary>
        /// Clears the command list.
        /// </summary>
        public void Clear()
        {
            commandExecuter.Clear();
            writtenLines.Reset();
            commandLine.Reset();
        }

        /// <summary>
        /// Change the command
        /// </summary>
        /// <param name="up"></param>
        private void ChangeLine(bool up)
        {
            if (up)
            {
                if (currentLine == null)
                {
                    savedCommand = commandLine.ToCommand();
                    currentLine = writtenLines.Lines.First;
                }
                else if (currentLine != writtenLines.Lines.Last)
                    currentLine = currentLine.Next;
            }
            else if (currentLine != null)
            {
                if (currentLine == writtenLines.Lines.First)
                {
                    currentLine = null;
                    commandLine.ToLine(savedCommand);
                }
                else
                {
                    currentLine = currentLine.Previous;
                }
            }

            if (currentLine != null)
                commandLine.ToLine(currentLine.Value.Text);
        }

        public void WriteLine(string line)
        {
            writtenLines.WriteLine(line);
        }

        /// <summary>
        /// Cleans the commandline.
        /// </summary>
        private void Clean()
        {
            commandLine.Reset();
            currentLine = null;
            savedCommand = "";
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (!active)
                return;

            //read commands
            if (inputHelper.KeyPressed(Keys.Enter))                                             //Finish command
            {
                Active = commandExecuter.Execute(commandLine, this);
                Clean();
                return;
            }

            if (inputHelper.KeyPressed(Keys.F1) || inputHelper.KeyPressed(Keys.Escape))    //Deactivate command line
            {
                Clean();
                Active = false;
                return;
            }

            if (inputHelper.KeyPressed(Keys.Up) || inputHelper.KeyPressed(Keys.Down))
            {
                ChangeLine(inputHelper.KeyPressed(Keys.Up));
            }

            commandLine.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            writtenLines.Update(gameTime);

            if (active)
                commandLine.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            Matrix newtransform = this.transform * transform;

            //Line offset
            Vector2 offset = Vector2.Zero;
            if (active)
            {
                if (!writtenLines.Up)
                    offset.Y = -commandLine.TextFont.MeasureString(commandLine.StartCursor).Y;

                //draw active written command
                commandLine.Draw(gameTime, spriteBatch, Matrix.CreateTranslation(new Vector3(offset, 0)) * newtransform); ;
            }

            offset.X = writtenLines.TextFont.MeasureString(commandLine.StartCursor).X;
            writtenLines.Draw(gameTime, spriteBatch, Matrix.CreateTranslation(new Vector3(offset, 0)) * newtransform);
        }

        public void RequestCommand(CommandExecuter executer)
        {
            if (currentExecuter == commandExecuter && executer != null)
                currentExecuter = executer;
        }

        public void ReturnCommand()
        {
            currentExecuter = commandExecuter;
        }

        /// <summary>
        /// Dictates if the commandline is active or not;
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { 
                active = value;
                writtenLines.ActiveCommand = value;
            }
        }

        public CommandExecuter CommandExecuter
        {
            get { return commandExecuter; }
        }

        public WrittenLines WrittenLines
        {
            get { return writtenLines; }
        }

        public CommandLine CommandLine
        {
            get { return commandLine; }
        }
    }
}