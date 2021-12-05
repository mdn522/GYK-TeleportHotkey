using System;
using System.Collections.Generic;
using System.IO;

namespace TeleportHotkey {
    public static class Helper {
        public static string logPath = ".\\QMods\\TeleportHotkey\\log.txt";
        public static string errorPath = ".\\QMods\\TeleportHotkey\\error.txt";

        public static void Log(string v, bool error = false) {
            using (StreamWriter streamWriter = File.AppendText(error ? Helper.errorPath : Helper.logPath))
                streamWriter.WriteLine(v);
        }
    }
}
