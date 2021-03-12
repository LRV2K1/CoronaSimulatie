using System.Collections.Generic;

namespace Engine.GameManagment
{
    public class GameSettingsManager
    {
        protected Dictionary<string, string> stringSettings;

        public GameSettingsManager()
        {
            stringSettings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the dictionary that holds the string settings.
        /// </summary>
        public Dictionary<string, string> StringSettings
        {
            get { return stringSettings; }
        }

        /// <summary>
        /// Sets the value for a specific key.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void SetValue(string key, string value)
        {
            stringSettings[key] = value;
        }

        /// <summary>
        /// Gives the saved value of the given key.
        /// </summary>
        /// <param name="key">Key to find value.</param>
        /// <returns>Returns the value for the key, when key does not exist returns null.</returns>
        public string GetValue(string key)
        {
            if (stringSettings.ContainsKey(key))
            {
                return stringSettings[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns if the settings contain the given key.
        /// </summary>
        /// <param name="key">The key that needs to be found.</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return stringSettings.ContainsKey(key);
        }

        /// <summary>
        /// Remove setting based on the key.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        public void Remove(string key)
        {
            if (stringSettings.ContainsKey(key))
            {
                stringSettings.Remove(key);
            }
        }

        /// <summary>
        /// Clear the entire settings list.
        /// </summary>
        public void UnLoad()
        {
            stringSettings.Clear();
        }
    }
}