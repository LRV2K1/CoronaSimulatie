using System.Collections.Generic;

namespace Game1.GameManagment.IO
{
    public class FileManager
    {
        Dictionary<string, File> files;

        public FileManager()
        {
            files = new Dictionary<string, File>();
        }

        /// <summary>
        /// Makes a new file.
        /// </summary>
        /// <param name="filename">The name of the file to be created.</param>
        /// <returns>The new file.</returns>
        public File NewFile(string filename, string blockname)
        {
            File file = new File(filename, blockname);
            files.Add(filename, file);
            return file;
        }

        /// <summary>
        /// Get a file.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <returns>The file.</returns>
        public File GetFile(string filename)
        {
            if (files.ContainsKey(filename))
            {
                files[filename].Load();
                return files[filename];
            }

            try
            {
                File file = new File(filename);
                files.Add(filename, file);
                return file;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a file.
        /// </summary>
        /// <param name="filename">The name of the file to be loaded.</param>
        public void LoadFile(string filename)
        {
            if (files.ContainsKey(filename))
                return;

            File file = new File(filename);
            files.Add(filename, file);
        }

        /// <summary>
        /// Unloads a file.
        /// </summary>
        /// <param name="filename">The name of the file to be unloaded.</param>
        public void UnloadFile(string filename)
        {
            if (!files.ContainsKey(filename))
                return;

            files[filename].Unload();
            files.Remove(filename);
        }

        /// <summary>
        /// Unloads all files.
        /// </summary>
        public void UnLoad()
        {
            foreach (KeyValuePair<string, File> file in files)
                file.Value.Unload();
            files.Clear();
        }
    }
}
