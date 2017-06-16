using Perforce.P4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PerforceWeb
{
    public partial class DepotList : System.Web.UI.Page
    {
        //Declare P4 Repository 
        internal Repository rep = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                //Connect to P4 Repository
                checkConnect();
                //Connect to P4 and return initial Depot Directory
                p4Tree_Init(rep);                
            }
        }

        private void checkConnect()
        {
            if (rep != null)
            {
                //release any old connection
                rep.Dispose();
            }

            //*** Need to look into this. Doesnt seem to reliably be connecting ***

            String conStr = WebConfigurationManager.AppSettings["p4Server"]; //Pull server info from web.config
            String user = "";//Username
            String password = ""; //Part of system settings on server
            try
            {
                Server server = new Server(new ServerAddress(conStr));

                rep = new Repository(server);

                rep.Connection.UserName = user;
                Options options = new Options();
                //options["Password"] = password;

                rep.Connection.Client = new Client();
                rep.Connection.Connect(options);

            }
            catch (Exception ex)
            {
                rep = null;
                //*** Setup an exception pop up or an error label ***
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void p4Tree_Init(Repository rep)
        {
            if (rep == null)
            {
                p4Tree.Enabled = false;
                return;
            }

            // Initialize the depot tree view
            P4Directory root = new P4Directory(rep, null, "depot", "//depot", null, null);

            TreeNode rootNode = new TreeNode("Depot");

            // Load P4Directory object into Session state object to reference when Treenode is clicked
            Session[root.Name] = root;

            //rootNode.ImageIndex = 0;
            //rootNode.SelectedImageIndex = 0;
            rootNode.ChildNodes.Add(new TreeNode("empty"));
            p4Tree.Nodes.Clear();
            p4Tree.Nodes.Add(rootNode);
        }

        protected void p4Tree_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {

            TreeNode node = (TreeNode)e.Node;
            // clear any old data
            node.ChildNodes.Clear();

            //Load the selected P4Directory object from SessionState
            P4Directory p4dir = (P4Directory)Session[node.Text];

            //*** Do some check to see if P4dir is null.


            if (String.IsNullOrEmpty(p4dir.DepotPath) || !p4dir.Expand())
            {
                //e.Cancel = true;
                return;
            }
            //Iterate through files in this directory
            if ((p4dir.Files != null) && (p4dir.Files.Count > 0))
            {
                foreach (FileMetaData file in p4dir.Files)
                {
                    //Restrict files to relevant SQL & SSRS file extensions                    
                    if ((Path.GetExtension(file.DepotPath.Path) == ".rdl" || Path.GetExtension(file.DepotPath.Path) == ".rds" || Path.GetExtension(file.DepotPath.Path) == ".rsd" || Path.GetExtension(file.DepotPath.Path) == ".sql"))
                    {
                        //Dont return any deleted or moved items
                        if (file.HeadAction.Equals(Perforce.P4.FileAction.Delete) || file.HeadAction.Equals(Perforce.P4.FileAction.MoveDelete))
                        {
                            //   return;
                        }
                        else
                        {
                            TreeNode child = new TreeNode(file.DepotPath.Path);
                            //child.Tag = file;

                            // Load file object into Session state object
                            Session[file.DepotPath.Path] = file;
                            // Allow files to be selectable
                            child.ShowCheckBox = true;
                            child.Text = p4fileShort(file.DepotPath.Path);
                            child.Value = file.DepotPath.Path;
                            //child.ImageIndex = 2;
                            //child.SelectedImageIndex = 2;
                            e.Node.ChildNodes.Add(child);
                        }
                    }
                }
            }
            //Iterate through sub directories
            if ((p4dir.Subdirectories != null) && (p4dir.Subdirectories.Count > 0))
            {
                foreach (P4Directory p4SubDir in p4dir.Subdirectories)
                {
                    if (!p4SubDir.InDepot)
                        continue;

                    TreeNode child = new TreeNode(p4SubDir.Name);
                    //child.Tag = p4SubDir;

                    // Load P4Directory object into Session state object to reference when Treenode is clicked
                    Session[p4SubDir.Name] = p4SubDir;
                    //child.ImageIndex = 1;
                    //child.SelectedImageIndex = 1;
                    child.ChildNodes.Add(new TreeNode("<empty>"));
                    e.Node.ChildNodes.Add(child);
                }
            }
        }
    }
}