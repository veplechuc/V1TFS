using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersionOneTFSServer.Collections
{
    /// <summary>
    /// Provides access to a recursable set of the AppSettingsKeys structure.
    /// </summary>
    public class AppSettingKeyCollection : Dictionary<string, string>
    {

        public AppSettingKeyCollection()
        {
            var instance = new AppSettingKeys();
            foreach (var field in typeof(AppSettingKeys).GetFields())
            {
                if (field.IsPrivate) continue;
                this.Add(field.Name, field.GetValue(instance) as string);
            }
        }
    }
}