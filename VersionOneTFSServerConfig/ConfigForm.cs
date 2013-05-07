using System;
using System.Collections;
using System.Drawing;
using System.Security.Principal;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Integrations.Core.DTO;
using Integrations.Core.Structures;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using System.DirectoryServices;
using VersionOne.TFS2010.DataLayer;
using VersionOneTFSServerConfig.Configuration;
using Environment = System.Environment;

namespace VersionOneTFSServerConfig
{
    public partial class ConfigForm : Form
    {

        internal static TfsTeamProjectCollection TfsServer;
        internal static TfsServerConfiguration _config;

        public ConfigForm()
        {
            InitializeComponent();

            btnTestV1Connection.Click += btnTestV1Connection_Click;
            btnSaveVersionOneSettings.Click += btnSaveVersionOneSettings_Click;
            TFSConnectB.Click += TFSConnectB_Click;
            TFSUpdateB.Click += TFSUpdateB_Click;
            UnsubscribeB.Click += UnsubscribeB_Click;
            SaveSettingsB.Click += SaveSettingsB_Click;
            chkUseProxy.CheckedChanged += chkUseProxy_CheckedChanged;
            UseIntegratedAuthenticationCB.CheckedChanged += chkUseIntegrationAuth_CheckChanged;

            _config = new ConfigurationProxy().Retrieve();

            //Advanced setup
            RegExTB.Text = _config.TfsWorkItemRegex;

            txtDebugDescription.Text = string.Format("Debug information is written to {0}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            //V1 setup
            V1URLTB.Text = _config.VersionOneUrl;

            V1UsernameTB.Text = _config.VersionOneUserName;
            V1PasswordTB.Text = _config.VersionOnePassword;
            UseIntegratedAuthenticationCB.Checked = _config.IsWindowsIntegratedSecurity;

            chkUseProxy.Checked = _config.ProxyIsEnabled;
            txtProxyUrl.Text = _config.ProxyUrl;
            txtProxyUsername.Text = _config.ProxyUsername;
            txtProxyPassword.Text = _config.ProxyPassword;
            txtProxyDomain.Text = _config.ProxyDomain;
            SetProxyRelatedFieldsEnabled(_config.ProxyIsEnabled);

            //TFS setup
            TFSURLTB.Text = _config.TfsUrl;
            TFSUsernameTB.Text = _config.TfsUserName;
            TFSPasswordTB.Text = _config.TfsPassword;
            ListenerURLTB.Text = _config.ListenerUrl;

			// Debug Mode
            chkDebugMode.Checked = _config.DebugMode;
            UpdateStatus();
        }

        private void SetProxyRelatedFieldsEnabled(bool enabled) {
            txtProxyUrl.Enabled = txtProxyUsername.Enabled = txtProxyPassword.Enabled = txtProxyDomain.Enabled = enabled;
        }

        #region Event Handlers

        private void chkUseProxy_CheckedChanged(object sender, EventArgs e) {
            SetProxyRelatedFieldsEnabled(chkUseProxy.Checked);
        }

        private void btnSaveVersionOneSettings_Click(object sender, EventArgs e)
        {

            var configToSave = new TfsServerConfiguration()
                {
                    VersionOneUrl = V1URLTB.Text,
                    VersionOneUserName = V1UsernameTB.Text,
                    VersionOnePassword = V1PasswordTB.Text,
                    IsWindowsIntegratedSecurity = UseIntegratedAuthenticationCB.Checked,
                    ProxyIsEnabled = chkUseProxy.Checked,
                    ProxyUrl = txtProxyUrl.Text,
                    ProxyUsername = txtProxyUsername.Text,
                    ProxyPassword = txtProxyPassword.Text,
                    ProxyDomain = txtProxyDomain.Text
                };

            var results = new ConfigurationProxy().Store(configToSave);
            if(results[StatusKey.Status] == StatusCode.Ok)
            {
                V1StatusLabel.Text = "Settings saved successfully.";
                return;
            }

            var missingFields = string.Join(", ", results.Keys.Where(key => key != "status"));
            V1StatusLabel.Text = string.Format("The following values must be present in order to save settings:  {0}.", missingFields);
        }

        private void btnTestV1Connection_Click(object sender, EventArgs e)
        {
            V1StatusLabel.ForeColor = Color.Black;

            V1StatusLabel.Text = "Connecting to " + V1URLTB.Text+ "...";
            V1StatusLabel.Refresh();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var versionOneSettings = new VersionOneSettings() 
                {
                    Path = V1URLTB.Text,
                    Username = V1UsernameTB.Text,
                    Password = V1PasswordTB.Text,
                    Integrated = UseIntegratedAuthenticationCB.Checked,
                    ProxySettings = GetProxySettings()
                };

                var v1Component = new V1Component(versionOneSettings);
                var connectionStatus = v1Component.ValidateConnection();
                DisplayConnectionValidationStatus(connectionStatus);
            }
            catch (Exception ex)
            {
                DisplayConnectionValidationStatus(false, ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void DisplayConnectionValidationStatus(bool status, string message = null) 
        {
            if(!status) 
            {
                V1StatusLabel.ForeColor = Color.Red;
                V1StatusLabel.Text = string.Format("Error connecting to {0}{1}", V1URLTB.Text, string.IsNullOrEmpty(message) ? string.Empty : ": " + message);
            } 
            else 
            {
                V1StatusLabel.ForeColor = Color.Black;
                V1StatusLabel.Text = string.Format("Successfully connected to {0}", V1URLTB.Text);
            }
        }

        private void TFSConnectB_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;


                var proxy = new ConfigurationProxy();
                var config = proxy.Retrieve();

                config.TfsUrl = TFSURLTB.Text;
                config.TfsUserName = TFSUsernameTB.Text;
                config.TfsPassword = TFSPasswordTB.Text;

                proxy.Store(config);

                TFSStatusLabel.Text = "Not connected";

                TfsServer = null;
                // Connect to TFS
                TfsServer = Utils.ConnectToTFS();

                TFSStatusLabel.Text = "Connected to " + TFSURLTB.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                UpdateStatus();
            }
        }

        private void TFSUpdateB_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var proxy = new ConfigurationProxy();
                var config = proxy.Retrieve();

                config.TfsUrl = ListenerURLTB.Text;
                proxy.Store(config);

                // Get Eventing Service
                var eventService = (IEventService)TfsServer.GetService(typeof(IEventService));

                // Set delivery preferences
                var dPref = new DeliveryPreference { Schedule = DeliverySchedule.Immediate, Address = config.TfsUrl, Type = DeliveryType.Soap };

                const string tag = "VersionOneTFSServer";

                // Unsubscribe to all events
                foreach (var s in eventService.GetEventSubscriptions(TfsServer.AuthorizedIdentity.Descriptor, tag))
                {
                    eventService.UnsubscribeEvent(s.ID);
                }

                // Subscribe to checked events
                var filter = string.Empty;
                eventService.SubscribeEvent("CheckinEvent", filter, dPref, tag);
                eventService.SubscribeEvent("BuildCompletionEvent2", filter, dPref, tag);

                UpdateStatus();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void UnsubscribeB_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                TFSUnsubscribe();
                UpdateStatus();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        private ProxyConnectionSettings GetProxySettings() 
        {

            if (!chkUseProxy.Checked) return null;

            return new ProxyConnectionSettings 
            { 
                ProxyIsEnabled = chkUseProxy.Checked, 
                Url = new Uri(txtProxyUrl.Text), 
                Username = txtProxyUsername.Text, 
                Password = txtProxyPassword.Text,
                Domain = txtProxyDomain.Text
            };
        }

        private void UpdateStatus()
        {
            if (TfsServer != null)
            {
                ListenerURLTB.Enabled = true;
                TFSUpdateB.Enabled = true;
                SubscriptionsLV.Enabled = true;
                UnsubscribeB.Enabled = true;
                lblCurrentSubscriptions.Enabled = true;
                lblListenerUrl.Enabled = true;

                // Get Eventing Service
                var eventService = (IEventService) TfsServer.GetService(typeof(IEventService));

                // Subscribe to event
                const string tag = "VersionOneTFSServer";
                SubscriptionsLV.Items.Clear();
                
                foreach (var s in eventService.GetEventSubscriptions(TfsServer.AuthorizedIdentity.Descriptor,tag))
                {
                    var item = new ListViewItem(s.EventType.ToString());
                    item.SubItems.Add(s.DeliveryPreference.Address);
                    SubscriptionsLV.Items.Add(item);
                }

                if (SubscriptionsLV.Items.Count > 0)
                {
                    TFSUpdateB.Text = "Update Subscriptions";
                    UnsubscribeB.Enabled = true;
                }
                else
                {
                    TFSUpdateB.Text = "Subscribe";
                    UnsubscribeB.Enabled = false;
                }
            }
            else
            {
                SubscriptionsLV.Items.Clear();
                SubscriptionsLV.Items.Add(new ListViewItem("Not connected"));
                SubscriptionsLV.Enabled = false;
                ListenerURLTB.Enabled = false;
                TFSUpdateB.Enabled = false;
                UnsubscribeB.Enabled = false;
                lblCurrentSubscriptions.Enabled = false;
                lblListenerUrl.Enabled = false;
            }
        }

        public static void TFSUnsubscribe()
        {
            if(TfsServer == null)
            {
                TfsServer = Utils.ConnectToTFS();
            }

            // Get Eventing Service
            var eventService = (IEventService) TfsServer.GetService(typeof(IEventService));

            const string tag = "VersionOneTFSServer";

            // Unsubscribe to all events
            foreach (var s in eventService.GetEventSubscriptions(TfsServer.AuthorizedIdentity.Descriptor, tag))
            {
                eventService.UnsubscribeEvent(s.ID);
            }
        }

        public static void SetNetVersion(DirectoryEntry e, Version v)
        {
            var version = "Framework\\v" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build+"\\";

            var newMaps = new ArrayList();
            var versionRegex = new Regex(@"Framework\\v\d+\.\d+\.\d+\\", RegexOptions.IgnoreCase);

            var changed = false;
            
            foreach (string str in e.Properties["ScriptMaps"])
            {
                var temp = str;
                var match = versionRegex.Match(str);
                
                if (match.Success)
                {
                    if (match.Value != version)
                    {
                        temp = versionRegex.Replace(str, version);
                        changed = true;
                    }
                }
                newMaps.Add(temp);
            }
            
            if(changed)
            {
                e.Properties["ScriptMaps"].Value = newMaps.ToArray();
                e.CommitChanges();
            }
        }

        public static void Install()
        {
            var w3svc = new DirectoryEntry("IIS://localhost/w3svc");

            const string folder = "VersionOne TFS Listener";

            foreach (DirectoryEntry e in w3svc.Children)
            {
                if (e.SchemaClassName == "IIsWebServer" && e.Properties["ServerComment"].Value.ToString() == folder)
                {
                    SetNetVersion(e, Environment.Version);
                    
                    foreach (DirectoryEntry f in e.Children)
                    {
                        if (f.SchemaClassName == "IIsWebVirtualDir")
                        {
                            SetNetVersion(f, Environment.Version);
                        }
                    }
                }
            }    
        }

        private void chkUseIntegrationAuth_CheckChanged(object sender, EventArgs e)
        {
            ToggleV1Credentials();
        }

        private byte _bit = 0;
        private void ToggleV1Credentials()
        {
            if (_bit == 0)
            {
                V1PasswordTB.Clear();
                if (WindowsIdentity.GetCurrent() == null) return;
                var windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null) V1UsernameTB.Text = windowsIdentity.Name;
                _bit = 1;
            }
            else
            {
                V1PasswordTB.Text = _config.VersionOnePassword;
                V1UsernameTB.Text = _config.VersionOneUserName;
                _bit = 0;
            }
        }

        private void SaveSettingsB_Click(object sender, EventArgs e)
        {

            var proxy = new ConfigurationProxy();
            var config = proxy.Retrieve();

            config.TfsWorkItemRegex = RegExTB.Text;
            config.DebugMode = chkDebugMode.Checked;

            proxy.Store(config);

        }

    }
}