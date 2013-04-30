using System.Collections.Generic;
using System.IO;

namespace Integrations.Core.Adapters
{
    public class SettingsFileAdapter
    {
        public static void SaveSettings(Dictionary<string, string> configToSave, string directory, string fileName)
        {

            if (Directory.Exists(directory) == false) Directory.CreateDirectory(directory);
            using (var sw = new StreamWriter(Path.Combine(directory, fileName)))
            {
                foreach (var entry in configToSave)
                {
                    sw.WriteLine(string.Format("{0}, {1}", entry.Key, entry.Value));
                }
            }
        }
    }
}
