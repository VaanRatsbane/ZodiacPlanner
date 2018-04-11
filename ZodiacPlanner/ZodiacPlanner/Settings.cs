using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZodiacPlanner
{
    class Settings
    {

        const string SETTINGSFILE = "settings.json";

        public bool darkMode { get; set; }
        public bool saveLog { get; set; }
        public Point location { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool doubleClick { get; set; }
        public bool rightClick { get; set; }

        public static Settings Load()
        {
            try
            {
                var json = File.ReadAllText(SETTINGSFILE);
                var obj = JsonConvert.DeserializeObject<Settings>(json);
                return obj;
            }
            catch
            {
                return new Settings()
                {
                    darkMode = false,
                    saveLog = false,
                    location = new Point(-1, -1),
                    doubleClick = false,
                    rightClick = false
                };
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SETTINGSFILE, json);
            }
            catch (Exception e)
            {
                Program.Log("Failed to save settings.\n" + e.ToString(), null);
            }
        }

    }
}
