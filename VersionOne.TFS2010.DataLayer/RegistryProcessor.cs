using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace VersionOne.TFS2010.DataLayer 
{
    public class RegistryProcessor 
    {
        #region Registry parameter names

        public const string V1RegexParameter = "V1RegEx";
        public const string V1UrlParameter = "V1URL";
        public const string V1UsernameParameter = "V1Username";
        public const string V1PasswordParameter = "V1Password";
        public const string V1WindowsAuthParameter = "V1WindowsAuth";

        public const string V1UseProxyParameter = "V1UseProxy";
        public const string V1ProxyUrlParameter = "V1ProxyUrl";
        public const string V1ProxyUsernameParameter = "V1ProxyUsername";
        public const string V1ProxyPasswordParameter = "V1ProxyPassword";
        public const string V1ProxyDomainParameter = "V1ProxyDomain";

        public const string TfsUrlParameter = "TFSURL";
        public const string TfsUsernameParameter = "TFSUsername";
        public const string TfsPasswordParameter = "TFSPassword";
        public const string ListenerUrlParameter = "ListenerURL";
        public const string ListenerPortParameter = "ListenerPort";
        public const string ListenerNameParameter = "ListenerName";
        
        public const string DebugEnabledParameter = "Debug";

        #endregion

        private const string RegistryPath = "Software\\VersionOne\\TFSListener";
        internal static readonly byte[] Entropy = { 160, 156, 208, 179, 150, 63, 48, 159, 31, 202 };

        public static object ReadRegistryKey(string key, string name, object def)
        {
            try
            {
                var rk = Registry.LocalMachine;
                rk = rk.OpenSubKey(key);

                if (rk != null)
                {
                    return rk.GetValue(name, def);
                }
            }
            catch (Exception) { }

            return def;
        }

        public static void WriteRegistryKey(string key, string name, object value)
        {
            var rk = Registry.LocalMachine;
            rk = rk.CreateSubKey(key);
            rk.SetValue(name, value);
        }

        public static void ClearValue(string name)
        {
            try
            {
                var rk = Registry.LocalMachine;
                rk = rk.OpenSubKey(RegistryPath);

                if (rk != null)
                {
                    rk.DeleteValue(name, false);
                }
            }
            catch (Exception) { }
        }

        public static void SetString(string name, string value)
        {
            WriteRegistryKey(RegistryPath, name, value);
        }

        public static void SetPassword(string name, string value)
        {
            WriteRegistryKey(RegistryPath, name, ProtectedData.Protect(Encoding.Unicode.GetBytes(value), Entropy, DataProtectionScope.LocalMachine));
        }

        public static void SetInt(string name, int value)
        {
            WriteRegistryKey(RegistryPath, name, value);
        }

        public static void SetBool(string name, bool value)
        {
            WriteRegistryKey(RegistryPath, name, value);
        }

        public static string GetString(string name, string def = "")
        {
            return ReadRegistryKey(RegistryPath, name, def).ToString();
        }

        public static string GetPassword(string name, string def = "")
        {
            var data = (byte[]) ReadRegistryKey(RegistryPath, name, new byte[0]);
            return data.Length > 0 ? Encoding.Unicode.GetString(ProtectedData.Unprotect(data, Entropy, DataProtectionScope.LocalMachine)) : def;
        }

        public static int GetInt(string name, int def = 0)
        {
            int result;
            return int.TryParse(ReadRegistryKey(RegistryPath, name, def.ToString()).ToString(), out result) ? result : def;
        }

        public static bool GetBool(string name, bool def = false)
        {
            bool result;
            return bool.TryParse(ReadRegistryKey(RegistryPath, name, def.ToString()).ToString(), out result) ? result : def;
        }
    }
}
