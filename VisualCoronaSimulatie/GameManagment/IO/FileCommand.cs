using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GameManagment.IO
{
    public class FileCommand
    {
        File file;
        List<string> wlines;
        string block;

        public FileCommand()
        {
            wlines = new List<string>();
            block = "";
            file = null;

            GameEnvironment.CommandManager.AddCommand("loadfile", LoadFile);
            GameEnvironment.CommandManager.AddCommand("newfile", NewFile);
            GameEnvironment.CommandManager.AddCommand("closefile", CloseFile);
            GameEnvironment.CommandManager.AddCommand("readblock", ReadBlock);
            GameEnvironment.CommandManager.AddCommand("writeblock", WriteBlock);
            GameEnvironment.CommandManager.AddCommand("newblock", NewBlock);
        }

        public bool NewFile(string command)
        {
            string[] var = command.Split(' ');
            if (var.Length > 2)
                file = GameEnvironment.FileManager.NewFile(var[1], var[2]);
            else
            {
                GameEnvironment.CommandManager.WriteLine("No file name, or no block name");
                return false;
            }
            if (file == null)
            {
                GameEnvironment.CommandManager.WriteLine("File not found");
                return false;
            }

            return true;
        }

        public bool LoadFile(string command)
        {
            string[] var = command.Split(' ');
            if (var.Length > 1)
                file = GameEnvironment.FileManager.GetFile(var[1]);
            else
            {
                GameEnvironment.CommandManager.WriteLine("No file name, or no block name");
                return false;
            }
            if (file == null)
            {
                GameEnvironment.CommandManager.WriteLine("File not found");
            }

            return true;
        }

        public bool CloseFile(string command)
        {
            if (file != null)
                file = null;
            else
                GameEnvironment.CommandManager.WriteLine("No file loaded");
            return true;
        }

        public bool ReadBlock(string command)
        {
            if (file == null)
            {
                GameEnvironment.CommandManager.WriteLine("No file loaded");
                return false;
            }
            string[] var = command.Split(' ');
            if (var.Length < 1)
            {
                GameEnvironment.CommandManager.WriteLine("No block name");
                return false;
            }

            string[] lines = file.ReadBlock(var[1]);
            if (lines == null)
            {
                GameEnvironment.CommandManager.WriteLine("Block not found");
                return false;
            }

            for (int i = 0; i < lines.Length; i++)
                GameEnvironment.CommandManager.WriteLine(lines[i]);

            return true;
        }

        public bool WriteBlock(string command)
        {
            if (file == null)
            {
                GameEnvironment.CommandManager.WriteLine("No file loaded");
                return false;
            }
            string[] var = command.Split(' ');
            if (var.Length < 1)
            {
                GameEnvironment.CommandManager.WriteLine("No block name");
                return false;
            }

            if (!file.HasBlock(var[1]))
            {
                GameEnvironment.CommandManager.WriteLine("Block not found");
                return false;
            }

            block = var[1];
            //GameEnvironment.CommandManager.RequestCommand(Write);

            return true;
        }

        public bool Write(string command)
        {
            string[] var = command.Split(' ');
            if (var[0] == "end")
            {
                file.WriteBlock(block, wlines.ToArray());
                block = "";
                wlines.Clear();

                GameEnvironment.CommandManager.ReturnCommand();
                GameEnvironment.CommandManager.Active = false;
                return true;
            }
            wlines.Add(command);

            return true;
        }

        public bool NewBlock(string command)
        {
            if (file == null)
            {
                GameEnvironment.CommandManager.WriteLine("No file loaded");
                return false;
            }
            string[] var = command.Split(' ');
            if (var.Length < 1)
            {
                GameEnvironment.CommandManager.WriteLine("No block name");
                return false;
            }

            if (file.HasBlock(var[1]))
            {
                GameEnvironment.CommandManager.WriteLine("Block already exists.");
                return false;
            }

            file.AddBlock(var[1]);

            return true;
        }
    }
}
