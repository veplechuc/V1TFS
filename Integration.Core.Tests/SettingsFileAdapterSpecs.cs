using System;
using System.Collections.Generic;
using System.IO;
using Integrations.Core.Adapters;
using Integrations.Core.Structures;
using NSpec;

namespace Integration.Core.Tests
{
    public class SettingsFileAdapterSpecs : nspec
    {

        public void given_settings_are_being_saved_to_a_file()
        {
            context["when a dictionary of strings is saved to the settings file"] = () =>
                {
                    var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "V1TFSServerTests");
                    const string fileName = "settings.ini";
                    var settings = new Dictionary<string, string> {{"Setting1", "Value1"}, {"Setting2", "Value2"}};
                    var qualifiedPath = Path.Combine(directory, fileName);
                    if (File.Exists(qualifiedPath)) File.Delete(qualifiedPath);
                    SettingsFileAdapter.SaveSettings(settings, directory, fileName);

                    it["then the file exists"] = () => File.Exists(qualifiedPath).should_be(true);

                    it["and the expected settings are present"] = () =>
                    {
                        var retrievedSettings = new Dictionary<string, string>();
                        using (var reader = new StreamReader(qualifiedPath))
                        {
                            string commaDelimitedLine;
                            while ((commaDelimitedLine = reader.ReadLine()) != null)
                            {
                                var parsedValues = commaDelimitedLine.Split(Seperators.Primary);
                                retrievedSettings.Add(parsedValues[0], parsedValues[1]);
                            }
                        }

                        retrievedSettings.ContainsKey("Setting1").should_be(true);
                        retrievedSettings.ContainsKey("Setting2").should_be(true);

                        retrievedSettings["Setting1"].Trim().should_be("Value1");
                        retrievedSettings["Setting2"].Trim().should_be("Value2");

                    };

                };
        }

    }
}
