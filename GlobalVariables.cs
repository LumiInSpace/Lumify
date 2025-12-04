using System;
using System.Collections.Generic;
using System.Text;

namespace Lumify
{
    public class GlobalVariables
    {
        public const string AppName = "Lumify";
        public const string Version = ""; //in development

        //Path
        public static readonly string BaseDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lumify");
                //Windows: C:\Users\<User>\AppData\Roaming\Lumify\Lists
                //Linux: /home/<user>/.config/Lumify/Lists
                //MacOS: /Users/<user>/Library/Application Support/Lumify/Lists

        //public static readonly string ConfigPath =
        //    Path.Combine(BaseDirectory, "config.json");

        //public static readonly string LogDirectory =
        //    Path.Combine(BaseDirectory, "logs");

        public static readonly string MaterialListPath =
            Path.Combine(BaseDirectory, "Lists");
    }
}
