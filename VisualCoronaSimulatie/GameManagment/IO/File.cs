using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Game1.GameManagment.IO
{
    public class File
    {
        class Block
        {
            public string name;
            public int start;
            public int size;
            public int index;

            public Block(int index)
            {
                this.index = index;
            }

            public override string ToString()
            {
                return "[" + name + "] " + start + " " + size;
            }

            public string Name()
            {
                return "[" + name + "]";
            }
        }

        struct Command
        {
            public string line;
            public int pos;
            public Func<string, int, string[], string[]> command;

            public Command(string line, int pos, Func<string, int, string[], string[]> command)
            {
                this.line = line;
                this.pos = pos;
                this.command = command;
            }
        }

        string filename;
        int numberBlocks;
        Dictionary<string, Block> blocks;
        List<Block> blockorder;
        int users;

        public File(string filename)
        {
            this.filename = filename;
            blocks = new Dictionary<string, Block>();
            blockorder = new List<Block>();
            numberBlocks = 0;
            users = 0;

            Load();
        }

        public File(string filename, string blockname)
        {
            this.filename = filename;
            blocks = new Dictionary<string, Block>();
            blockorder = new List<Block>();
            numberBlocks = 0;
            users = 0;

            //work, new file
            using (StreamWriter writer = new StreamWriter(filename))
            {
            }

            AddBlock(blockname);

            users++;
        }

        /// <summary>
        /// Load the file.
        /// </summary>
        public void Load()
        {
            if (users > 0)
                return;
            using (StreamReader streamReader = new StreamReader(filename))
            {
                string line = streamReader.ReadLine();
                while (line != "" && line != null)                       //read data for block. 
                {
                    string[] namedata = line.Split(']');                //data for the name.
                    if (namedata.Length < 2)                            //incomplete.
                    {
                        line = streamReader.ReadLine();
                        continue;
                    }
                    string name = namedata[0].Trim('[');
                    string[] blockdata = namedata[1].Split(' ');        //data for the block.
                    if (blockdata.Length < 2)                           //incomplete.
                    {
                        line = streamReader.ReadLine();
                        continue;
                    }

                    int start, size;
                    try
                    {
                        start = int.Parse(blockdata[1]);
                        size = int.Parse(blockdata[2]);
                    }
                    catch
                    {
                        line = streamReader.ReadLine();
                        continue;
                    }
                    Block block = new Block(numberBlocks);
                    block.name = name;
                    block.start = start;
                    block.size = size;

                    blocks.Add(name, block);                            //save data for block.
                    blockorder.Add(block);
                    numberBlocks++;

                    line = streamReader.ReadLine();                     //read next line.
                }
            }
            users++;
        }

        /// <summary>
        /// Unloads the currently loaded file.
        /// </summary>
        public void Unload()
        {
            blocks.Clear();
            blockorder.Clear();
            numberBlocks = 0;
            users = 0;
        }

        /// <summary>
        /// Close the file.
        /// </summary>
        /// <param name="file"></param>
        public void Close(ref File file)
        {
            if (file == this)
            {
                users--;
                file = null;
            }
            if (users == 0)
                Unload();
        }

        /// <summary>
        /// Reads the data in a specific block of the file.
        /// </summary>
        /// <param name="name">The name of the block.</param>
        /// <returns></returns>
        public string[] ReadBlock(string name)
        {
            if (!blocks.ContainsKey(name))                          //If block does not exist.
                return null;

            string[] olines = ReadFile();

            Block block = blocks[name];
            string[] lines = new string[block.size];
            for (int i = 0; i < block.size; i++)
                lines[i] = olines[i + block.start + 1];

            return lines;
        }

        public bool HasBlock(string name)
        {
            return blocks.ContainsKey(name);
        }

        /// <summary>
        /// Writes data to a specific block in the file.
        /// </summary>
        /// <param name="name">The name of the block.</param>
        /// <param name="lines">Data to be written in the block.</param>
        public void WriteBlock(string name, string[] lines)
        {
            if (!blocks.ContainsKey(name))
                return;

            List<Command> commands = new List<Command>();
            Block block = blocks[name];

            //remove old lines
            for (int i = 0; i < block.size; i++)
                commands.Add(new Command("", block.start+1, RemoveLine));

            block.size = lines.Length;

            //add new lines
            for (int i = 0; i < block.size; i++)
                commands.Add(new Command(lines[lines.Length - 1 - i], block.start + 1, InsertLine));

            commands.Add(new Command(block.ToString(), block.index, ChangeLine));
            int start = block.start + block.size + 2;
            for (int i= block.index+1; i < blockorder.Count; i++)
            {
                blockorder[i].start = start;
                start += blockorder[i].size + 2;
                commands.Add(new Command(blockorder[i].ToString(), i, ChangeLine));
            }

            string[] filelines = ReadFile();
            ExecuteCommands(commands, ref filelines);
            WriteFile(filelines);
        }

        /// <summary>
        /// Adds a block to the file.
        /// </summary>
        /// <param name="name">Name of the block to be added.</param>
        public void AddBlock(string name)
        {
            if (blocks.ContainsKey(name))
                return;

            List<Command> commands = new List<Command>();
            Block block = new Block(numberBlocks);
            block.name = name;
            block.size = 0;
            int latest = 2;                                         //start new block

            int i = 0;
            foreach(Block b in blockorder)
            {
                b.start++;                                    //start other blocks
                latest = Math.Max(latest, b.start + b.size + 2);
                commands.Add(new Command(b.ToString(), i, ChangeLine));
                i++;
            }

            block.start = latest;

            commands.Add(new Command(block.ToString(), numberBlocks, InsertLine));
            commands.Add(new Command(block.Name(), latest, InsertLine));

            blocks.Add(name, block);                                //add block
            blockorder.Add(block);
            numberBlocks++;

            string[] lines = ReadFile();
            ExecuteCommands(commands, ref lines);
            WriteFile(lines);
        }

        /// <summary>
        /// Removes a block from the file.
        /// </summary>
        /// <param name="name">Name of the block to be removed.</param>
        public void RemoveBlock(string name)
        {
            if (!blocks.ContainsKey(name))
                return;

            List<Command> commands = new List<Command>();
            Block block = blocks[name];
            blocks.Remove(name);
            blockorder.RemoveAt(block.index);

            for(int i= 0; i < block.size + 2; i++)
                commands.Add(new Command("", block.start, RemoveLine));
            commands.Add(new Command("", block.index, RemoveLine));

            numberBlocks--;

            int start = block.start;
            for (int i = 0; i < blockorder.Count; i++)
            {
                if (i >= block.index)
                {
                    blockorder[i].start = start;
                    blockorder[i].index = i;
                    start += blockorder[i].size + 2;
                }
                blockorder[i].start--;
                commands.Add(new Command(blockorder[i].ToString(), i, ChangeLine));
            }

            string[] lines = ReadFile();
            ExecuteCommands(commands, ref lines);
            WriteFile(lines);
        }

        private void ExecuteCommands(List<Command> commands, ref string[] lines)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                lines = commands[i].command(commands[i].line, commands[i].pos, lines);
            }
        }

        private string[] InsertLine(string line, int pos, string[] lines)
        {
            for (int i = lines.Length; i < pos; i++)
                lines = InsertLine("", i, lines);                   //add blank lines.
            
            List<string> newlines = lines.ToList();

            newlines.Insert(pos, line);
            lines = newlines.ToArray();
            return lines;
        }

        private string[] ChangeLine(string line, int pos, string[] lines)
        {
            if (pos >= lines.Length)
                return lines;
            lines[pos] = line;
            return lines;
        }

        private string[] RemoveLine(string line, int pos, string[] lines)
        {
            if (pos >= lines.Length)
                return lines;
            List<string> newlines = lines.ToList();

            newlines.RemoveAt(pos);
            lines = newlines.ToArray();
            return lines;
        }

        private string[] ReadFile()
        {
            List<string> lines = new List<string>();
            
            using(StreamReader reader = new StreamReader(filename))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return lines.ToArray();
        }

        private void WriteFile(string[] lines)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < lines.Length; i++)
                    writer.WriteLine(lines[i]);
            }
        }

        public int Users
        {
            get { return users; }
            set { users = value; }
        }
    }
}