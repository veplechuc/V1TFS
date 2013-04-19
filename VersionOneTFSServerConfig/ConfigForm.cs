using System;
using System.Collections;
using System.Drawing;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using System.DirectoryServices;
using VersionOne.TFS2010.DataLayer;
using Environment = System.Environment;

namespace VersionOneTFSServerConfig
{
    public partial class ConfigForm : Form
    {

        internal static TfsTeamProjectCollection TfsServer;

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

            //Advanced setup
            RegExTB.Text = RegistryProcessor.GetString(RegistryProcessor.V1RegexParameter, "[A-Z]{1,2}-[0-9]+");

            txtDebugDescription.Text = string.Format("Debug information is written to {0}",
                                                     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            //V1 setup
            V1URLTB.Text = RegistryProcessor.GetString(RegistryProcessor.V1UrlParameter, "http://localhost/VersionOne/");
            var username = WindowsIdentity.GetCurrent().Name;
            var pos = username.IndexOf("\\");

            if(pos >= 0) 
            {
                username = username.Substring(pos + 1);
            }

            V1UsernameTB.Text = RegistryProcessor.GetString(RegistryProcessor.V1UsernameParameter, username);
            V1PasswordTB.Text = RegistryProcessor.GetPassword(RegistryProcessor.V1PasswordParameter, string.Empty);
            UseIntegratedAuthenticationCB.Checked = RegistryProcessor.GetBool(RegistryProcessor.V1WindowsAuthParameter, false);

            chkUseProxy.Checked = RegistryProcessor.GetBool(RegistryProcessor.V1UseProxyParameter, false);
            txtProxyUrl.Text = RegistryProcessor.GetString(RegistryProcessor.V1ProxyUrlParameter, string.Empty);
            txtProxyUsername.Text = RegistryProcessor.GetString(RegistryProcessor.V1ProxyUsernameParameter, string.Empty);
            txtProxyPassword.Text = RegistryProcessor.GetString(RegistryProcessor.V1ProxyPasswordParameter, string.Empty);
            txtProxyDomain.Text = RegistryProcessor.GetString(RegistryProcessor.V1ProxyDomainParameter, string.Empty);
            SetProxyRelatedFieldsEnabled(chkUseProxy.Checked);

            //TFS setup
            TFSURLTB.Text = RegistryProcessor.GetString(RegistryProcessor.TfsUrlParameter, "http://localhost:8080");
            TFSUsernameTB.Text = RegistryProcessor.GetString(RegistryProcessor.TfsUsernameParameter, WindowsIdentity.GetCurrent().Name);
            TFSPasswordTB.Text = RegistryProcessor.GetPassword(RegistryProcessor.TfsPasswordParameter, string.Empty);
            var port = RegistryProcessor.GetString(RegistryProcessor.ListenerPortParameter, "9090");
            var folder = RegistryProcessor.GetString(RegistryProcessor.ListenerNameParameter, "VersionOne TFS Listener");
            var host = Dns.GetHostName();
            ListenerURLTB.Text = RegistryProcessor.GetString(RegistryProcessor.ListenerUrlParameter, "http://" + host + ":" + port + "/Service.svc");

			// Debug Mode
            chkDebugMode.Checked = RegistryProcessor.GetBool(RegistryProcessor.DebugEnabledParameter, false);
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
            RegistryProcessor.SetString(RegistryProcessor.V1UrlParameter, V1URLTB.Text);
            RegistryProcessor.SetString(RegistryProcessor.V1UsernameParameter, V1UsernameTB.Text);
            RegistryProcessor.SetPassword(RegistryProcessor.V1PasswordParameter, V1PasswordTB.Text);
            RegistryProcessor.SetBool(RegistryProcessor.V1WindowsAuthParameter, UseIntegratedAuthenticationCB.Checked);

            RegistryProcessor.SetBool(RegistryProcessor.V1UseProxyParameter, chkUseProxy.Checked);
            RegistryProcessor.SetString(RegistryProcessor.V1ProxyUrlParameter, txtProxyUrl.Text);
            RegistryProcessor.SetString(RegistryProcessor.V1ProxyUsernameParameter, txtProxyUsername.Text);
            RegistryProcessor.SetString(RegistryProcessor.V1ProxyPasswordParameter, txtProxyPassword.Text);
            RegistryProcessor.SetString(RegistryProcessor.V1ProxyDomainParameter, txtProxyDomain.Text);

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

                RegistryProcessor.SetString(RegistryProcessor.TfsUrlParameter, TFSURLTB.Text);
                RegistryProcessor.SetString(RegistryProcessor.TfsUsernameParameter, TFSUsernameTB.Text);
                RegistryProcessor.SetPassword(RegistryProcessor.TfsPasswordParameter, TFSPasswordTB.Text);

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

                var url = ListenerURLTB.Text;
                RegistryProcessor.SetString(RegistryProcessor.ListenerUrlParameter, url);

                // Get Eventing Service
                var eventService = (IEventService)TfsServer.GetService(typeof(IEventService));

                // Set delivery preferences
                var dPref = new DeliveryPreference { Schedule = DeliverySchedule.Immediate, Address = url, Type = DeliveryType.Soap };

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
            if(!chkUseProxy.Checked) 
            {
                return null;
            }

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

            var folder = RegistryProcessor.GetString("ListenerName", "VersionOne TFS Listener");

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

        private void SaveSettingsB_Click(object sender, EventArgs e)
        {
            RegistryProcessor.SetString(RegistryProcessor.V1RegexParameter, RegExTB.Text);
            RegistryProcessor.SetBool(RegistryProcessor.DebugEnabledParameter, chkDebugMode.Checked);
        }
    }
}