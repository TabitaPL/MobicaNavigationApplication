using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace Mobica
{
    //ta klasa mogłaby być singletonem
    public class StorageSetting
    {
        #region variables
        IsolatedStorageSettings settings;

        //keys
        const string RouteMode = "RouteMode";

        //default values for keys
        const string RouteModeDefault = "Drive mode";
        #endregion

        //constructor gets settings
        public StorageSetting()
		{
			settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains(RouteMode))
            {
                settings.Add(RouteMode, RouteModeDefault);
                settings.Save();
            }
		}

        public bool UpdateValue(string key, Object value)
        {
            bool ifChanged = false;
            // If the key exists
            if (settings.Contains(key))
            {
                if (settings[key] != value)
                {
                    settings[key] = value;
                    ifChanged = true;
                    settings.Save();
                }
            }
            else
            {
                System.Console.WriteLine("Key not found");
            }
            return ifChanged;
        }

        public String GetValue(String key)
        {
            return Convert.ToString(settings[key]);
        }

        //return a list of keys
        public List<string> GetKeyList()
        {
            List<string> keyList = new List<string>();
            foreach (string key in settings.Keys)
            {
                keyList.Add(key);
            }
            return keyList;
        }
    }
}
