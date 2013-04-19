using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using VersionOne.ServerConnector.Entities;
using VersionOne.TFS2010.DataLayer;
using System.Threading;
using log4net;

using Task = System.Threading.Tasks.Task;

namespace VersionOne.TFS.Policy
{
    // TODO extract Registry operations
    public partial class CheckInForm : Form
    {
        private readonly IV1ComponentContainerProvider containerProvider;
        private readonly V1ComponentContainer container;
        private readonly ILog logger = LogManager.GetLogger(typeof(CheckInForm));

        private IDictionary<PrimaryWorkitem, List<Workitem>> myWorkitems;
        private IDictionary<PrimaryWorkitem, List<Workitem>> allWorkitems;

        private readonly List<Workitem> selectedWorkitems = new List<Workitem>();

        private string v1Url = string.Empty;
        private string username = string.Empty;
        private string password = string.Empty;
        private bool integratedAuthentication = true;
        private bool hasCachedCredentials;
        private bool useProxy = false;
        private string proxyUrl = string.Empty;
        private string proxyUsername = string.Empty;
        private string proxyPassword = string.Empty;
        private string proxyDomain = string.Empty;

        public CheckInForm(IV1ComponentContainerProvider containerProvider) {
            this.containerProvider = containerProvider;
            container = containerProvider.GetContainer();
            InitializeComponent();
        }

        private string comment;

        public void SetComment(string value)
        {
            comment = value;
        }

        public string GetComment()
        {
            var newComment = comment;

            var newIDs = selectedWorkitems.Select(workitem => workitem.Number).ToList();

            //get items in comment
            var re = new Regex(" ?[A-Z]{1,2}-[0-9]+");

            //for each item in the comment, check if it is still selected.  If it is not selected and does exist in the 
            //workitem tree, remove the item from the comment
            var pos = 0;
            
            while (pos < newComment.Length)
            {
                var match = re.Match(newComment, pos);
                
                if (match.Success)
                {
                    var id = match.Value.Trim();
                    var node = FindNode(AllWorkitemsTV.Nodes, id);
                    pos = match.Index + match.Length;
                    
                    if (node != null)
                    {
                        if (newIDs.Contains(id))
                        {
                            newIDs.Remove(id);
                        }
                        else
                        {
                            newComment = newComment.Remove(match.Index, match.Length);
                            pos = match.Index;
                        }
                    }
                }
                else 
                {
                    break;
                }
            }

            foreach(var id in newIDs) 
            {
                newComment += " " + id;
            }

            return newComment;
        }

        private void LoadSettings()
        {
            try
            {
                var rk = Registry.CurrentUser;
                rk = rk.OpenSubKey("Software\\VersionOne\\TFSPolicy");
                
                if (rk != null)
                {
                    v1Url = rk.GetValue("V1URL", string.Empty).ToString();
                }
            }
            catch (Exception)
            {
                v1Url = string.Empty;
            }
        }

        private bool LoadCredentials()
        {
            hasCachedCredentials = false;
            
            try
            {
                var rk = Registry.CurrentUser;
                rk = rk.OpenSubKey("Software\\VersionOne\\TFSPolicy\\Credentials");
                
                if (rk != null)
                {
                    username = rk.GetValue("Username", string.Empty).ToString();
                    password = Encoding.Unicode.GetString(
                        ProtectedData.Unprotect((byte[])rk.GetValue("Password", string.Empty),
                        Entropy, DataProtectionScope.CurrentUser));
                    integratedAuthentication = bool.Parse(rk.GetValue("IntegratedAuthentication", false).ToString());

                    useProxy = bool.Parse(rk.GetValue("UseProxy", false).ToString());
                    proxyUrl = rk.GetValue("ProxyUrl", string.Empty).ToString();
                    proxyUsername = rk.GetValue("ProxyUsername", string.Empty).ToString();
                    proxyPassword = Encoding.Unicode.GetString(
                        ProtectedData.Unprotect((byte[])rk.GetValue("ProxyPassword", string.Empty),
                        Entropy, DataProtectionScope.CurrentUser));
                    proxyDomain = rk.GetValue("ProxyDomain", string.Empty).ToString();

                    hasCachedCredentials = true;
                    return true;
                }
            }
            catch (Exception)
            {
                username = string.Empty;
                password = string.Empty;
                useProxy = false;
                proxyUrl = string.Empty;
                proxyUsername = string.Empty;
                proxyPassword = string.Empty;
                proxyDomain = string.Empty;

                integratedAuthentication = false;
                hasCachedCredentials = false;
            }

            return false;
        }

        static readonly byte[] Entropy = { 160, 156, 208, 179, 150, 63, 48, 159, 31, 202 };

        private void SaveSettings()
        {
            var rk = Registry.CurrentUser;
            rk = rk.CreateSubKey("Software\\VersionOne\\TFSPolicy");
            
            if (rk != null)
            {
                rk.SetValue("V1URL", v1Url);
            }
        }

        private void SaveCredentials()
        {
            var rk = Registry.CurrentUser;
            rk = rk.CreateSubKey("Software\\VersionOne\\TFSPolicy\\Credentials");
            
            if (rk != null)
            {
                rk.SetValue("Username", username);
                rk.SetValue("Password",
                    ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(password),
                    Entropy, DataProtectionScope.CurrentUser));
                rk.SetValue("IntegratedAuthentication", integratedAuthentication);

                rk.SetValue("UseProxy", useProxy);
                rk.SetValue("ProxyUrl", proxyUrl);
                rk.SetValue("ProxyUsername", proxyUsername);
                rk.SetValue("ProxyPassword",
                    ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(proxyPassword),
                    Entropy, DataProtectionScope.CurrentUser));
                rk.SetValue("ProxyDomain", proxyDomain);
            }

            hasCachedCredentials = true;
        }

        private void ClearCredentials()
        {
            RegistryKey rk = Registry.CurrentUser;
            rk.DeleteSubKey("Software\\VersionOne\\TFSPolicy\\Credentials", false);
            hasCachedCredentials = false;
        }

        private bool GetCredentials()
        {
            var form = new LoginForm
            {
                V1Url = v1Url,
                Username = username,
                Password = password,
                IntegratedAuthentication = integratedAuthentication,
                CacheCredentials = hasCachedCredentials,
                UseProxy = useProxy,
                ProxyUrl = proxyUrl,
                ProxyUsername = proxyUsername,
                ProxyPassword = proxyPassword,
                ProxyDomain = proxyDomain,
            };

            if (form.ShowDialog() == DialogResult.OK)
            {
                v1Url = form.V1Url;
                username = form.Username;
                password = form.Password;
                integratedAuthentication = form.IntegratedAuthentication;
                useProxy = form.UseProxy;
                proxyUrl = form.ProxyUrl;
                proxyUsername = form.ProxyUsername;
                proxyPassword = form.ProxyPassword;
                proxyDomain = form.ProxyDomain;

                SaveSettings();

                if(form.CacheCredentials) 
                {
                    SaveCredentials();
                } 
                else 
                {
                    ClearCredentials();
                }

                return true;
            }
            return false;
        }

        private static TreeNode FindNode(TreeNodeCollection nodes, string id)
        {
            foreach (TreeNode node in nodes)
            {
                if(((Workitem)node.Tag).Number.Equals(id, StringComparison.InvariantCultureIgnoreCase)) 
                {
                    return node;
                }

                var subnode = FindNode(node.Nodes, id);

                if(subnode != null) 
                {
                    return subnode;
                }
            }
            return null;
        }

        private void CheckWorkitemsInComment()
        {
            var re = new Regex(VersionOneTFSPolicy.VersionOneIdRegex);

            foreach (Match match in re.Matches(comment))
            {
                var node = FindNode(MyWorkitemsTV.Nodes, match.Value);

                if(node != null) 
                {
                    node.Checked = true;
                }

                node = FindNode(AllWorkitemsTV.Nodes, match.Value);

                if(node != null) 
                {
                    node.Checked = true;
                }
            }
        }

        private void CheckInForm_Shown(object sender, EventArgs e)
        {
            try
            {            
                LoadSettings();
                
                if (!LoadCredentials())
                {
                    if (!GetCredentials())
                    {
                        DialogResult = DialogResult.Cancel;
                        return;
                    }
                }

                LoginAndFetchWorkItems();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Error");
            }
        }

        private void LoginAndFetchWorkItems()
        {
            Cursor.Current = Cursors.WaitCursor;
            StatusLabel.Text = "Connecting to " + v1Url + "...";
            LoadingPanel.Visible = true;
            Refresh();

            Task.Factory.StartNew(() => {
                                      try 
                                      {
                                          UserLabel.Text = "Hello " + GetV1Component().GetLoggedInMemberUsername();
                                          GetWorkitems();
                                      } 
                                      catch (Exception ex) 
                                      {
                                          HandleException(ex, "Failed to obtain workitems");
                                      }
                                  }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void HandleException(Exception ex, string label)
        {
            MessageBox.Show(ex.Message, label);
            Cursor.Current = Cursors.Default;
            LoadingPanel.Visible = false;
        }

        private void ClearWorkitems()
        {
            selectedWorkitems.Clear();
            AllWorkitemsTV.Nodes.Clear();
            MyWorkitemsTV.Nodes.Clear();
        }

        private void GetWorkitems()
        {
            StatusLabel.Text = "Retrieving Workitems, Please Wait...";
            Cursor.Current = Cursors.WaitCursor;
            LoadingPanel.Visible = true;
            Refresh();

            Task.Factory
                .StartNew(RetrieveWorkitems, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(parentTask =>
                    {
                        if (parentTask.Exception != null)
                        {
                            HandleException(parentTask.Exception, "Error Retrieving Work Items");
                        }
                        else
                        {
                            AddWorkitemsToTree(allWorkitems, AllWorkitemsTV);
                            AddWorkitemsToTree(myWorkitems, MyWorkitemsTV);

                            Cursor.Current = Cursors.Default;
                            LoadingPanel.Visible = false;
                            CheckWorkitemsInComment();
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AddWorkitemsToTree(IDictionary<PrimaryWorkitem, List<Workitem>> workitems, TreeView tree)
        {
            tree.Nodes.Clear();
            
            foreach (var primaryWorkitem in workitems.Keys)
            {
                var primaryWorkitemDisplay = string.Format("{0} ({1})", primaryWorkitem.Name, primaryWorkitem.Number);

                var primaryWorkitemNode = new TreeNode(primaryWorkitemDisplay);
                //@note removed description since it contains html markup
                //primaryWorkitemNode.ToolTipText = primaryWorkitem.Description;
                SetIcon(primaryWorkitemNode, primaryWorkitem);
                primaryWorkitemNode.Tag = primaryWorkitem;
                tree.Nodes.Add(primaryWorkitemNode);

                foreach (var secondaryWorkitem in workitems[primaryWorkitem])
                {
                    var taskDisplay = string.Format("{0} ({1})", secondaryWorkitem.Name, secondaryWorkitem.Number);

                    var taskNode = new TreeNode(taskDisplay);
                    //@note removed description since it contains html markup
                    //taskNode.ToolTipText = task.Description;
                    SetIcon(taskNode, secondaryWorkitem);
                    taskNode.Tag = secondaryWorkitem;
                    primaryWorkitemNode.Nodes.Add(taskNode);
                }

                primaryWorkitemNode.ExpandAll();
            }
        }

        private void SetIcon(TreeNode node, Workitem workitem)
        {
            int index;

            if(workitem.TypeToken.Equals("Story")) 
            {
                index = 0;
            } 
            else if(workitem.TypeToken.Equals("Defect")) 
            {
                index = 1;
            } 
            else if(workitem.TypeToken.Equals("Task")) 
            {
                index = 2;
            }
            else 
            {
                index = 3;
            }

            node.ImageIndex = index;
            node.SelectedImageIndex = index;
        }

        private V1Component GetV1Component()
        {
            var settings = new VersionOneSettings
            {
                Path = v1Url,
                Username = username,
                Password = password,
                Integrated = integratedAuthentication,
                ProxySettings = !useProxy ? null : new ProxyConnectionSettings
                {
                    ProxyIsEnabled = useProxy,
                    Url = new Uri(proxyUrl),
                    Username = proxyUsername,
                    Password = proxyPassword,
                    Domain = proxyDomain,
                }
            };

            return container.GetV1Component(settings);
        }

        private void RetrieveWorkitems()
        {
            logger.Debug("Retrieving workitems...");

            allWorkitems = new Dictionary<PrimaryWorkitem, List<Workitem>>();
            myWorkitems = new Dictionary<PrimaryWorkitem, List<Workitem>>();

            var currentUser = GetV1Component().GetLoggedInMemberUsername();

            var primaryWorkitems = GetV1Component().GetActivePrimaryWorkitems();
            var secondaryWorkitems = GetV1Component().GetActiveSecondaryWorkitems();

            foreach (var workitem in primaryWorkitems)
            {

                if (workitem.Owners.Any(x => string.Equals(x.Username, currentUser)))
                {
                    myWorkitems.Add(workitem, new List<Workitem>());
                }
                else
                {
                    allWorkitems.Add(workitem, new List<Workitem>());
                }

                var taskAndTest = secondaryWorkitems.Where(
                        x => x.GetProperty<string>(V1Component.ParentNumberProperty) == workitem.Number).ToList();

                foreach (var secondaryWorkitem in taskAndTest)
                {
                    if (secondaryWorkitem.Owners.Any(x => string.Equals(x.Username, currentUser)))
                    {
                        if (!myWorkitems.ContainsKey(workitem))
                        {
                            myWorkitems.Add(workitem, new List<Workitem>());
                        }
                        myWorkitems[workitem].Add(secondaryWorkitem);
                    }
                    else
                    {
                        if (!allWorkitems.ContainsKey(workitem))
                        {
                            allWorkitems.Add(workitem, new List<Workitem>());
                        }
                        allWorkitems[workitem].Add(secondaryWorkitem);
                    }
                }
            }

            logger.DebugFormat("Downloaded {0} primary and {1} secondary items", primaryWorkitems.Count, secondaryWorkitems.Count);
        }

        private void ChangeUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!GetCredentials())
                {
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                ClearWorkitems();

                LoginAndFetchWorkItems();

            }
            catch (Exception ex)
            {
                HandleException(ex, "Error");
            }
        }

        private void MyWorkitemsTV_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var wi = (Workitem)e.Node.Tag;
            
            if (e.Node.Checked)
            {
                if(!selectedWorkitems.Contains(wi)) 
                {
                    selectedWorkitems.Add(wi);
                }
            }
            else 
            {
                selectedWorkitems.Remove(wi);
            }

            var node = FindNode(MyWorkitemsTV.Nodes, wi.Number);

            if(node != null && node.Checked != e.Node.Checked) 
            {
                node.Checked = e.Node.Checked;
            }

            node = FindNode(AllWorkitemsTV.Nodes, wi.Number);

            if(node != null && node.Checked != e.Node.Checked) 
            {
                node.Checked = e.Node.Checked;
            }
        }
    }
}