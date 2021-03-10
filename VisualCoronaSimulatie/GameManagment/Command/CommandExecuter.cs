using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameManagment.Command
{
    public class CommandExecuter
    {
        Dictionary<string, Func<string, bool>> commands;

        public CommandExecuter()
        {
            commands = new Dictionary<string, Func<string, bool>>();
        }

        public Dictionary<string, Func<string, bool>> Commands
        {
            get { return commands; }
        }

        /// <summary>
        /// Add command to command exectuer.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="function">Function to be activated by command.</param>
        public void AddCommand(string command, Func<string, bool> function)
        {
            commands[command] = function;
        }

        /// <summary>
        /// Remove a command from the command executer.
        /// </summary>
        /// <param name="command">Command to be removed.</param>
        public void RemoveCommand(string command)
        {
            if (commands.ContainsKey(command))
                commands.Remove(command);
        }

        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="command">CommandLine.</param>
        /// <param name="manager">CommandManager.</param>
        public virtual bool Execute(CommandLine command, CommandManager manager)
        {
            string com = command.ToCommand();
            manager.WriteLine(com);

            if (com.Trim(' ') != "")
            {
                string[] c = com.Split();
                if (commands.ContainsKey(c[0]))
                    commands[c[0]](com);
            }
            return false;
        }

        public void Clear()
        {
            commands.Clear();
        }
    }
}
