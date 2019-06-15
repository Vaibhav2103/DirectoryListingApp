using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DLWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DirectoryTree directoryTree = new DirectoryTree();
                DataTable dtDirectory = directoryTree.GetDataTableFromQuery("select * from dbo.directory where parent_id is null order by id");
                PopulateTreeView(dtDirectory, 0, null);
            }
        }

        private void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
        {
            foreach (DataRow row in dtParent.Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = row["Name"].ToString(),
                    Value = row["Id"].ToString()
                };
                if (parentId == 0)
                {
                    DirectoryTree directoryTree = new DirectoryTree();
                    TreeView1.Nodes.Add(child);
                    DataTable dtChild = directoryTree.GetDataTableFromQuery("select * from dbo.directory where parent_id =" + child.Value);
                    PopulateTreeView(dtChild, int.Parse(child.Value), child);
                }
                else
                {
                    DirectoryTree directoryTree = new DirectoryTree();
                    treeNode.ChildNodes.Add(child);
                    DataTable dtChild = directoryTree.GetDataTableFromQuery("select * from dbo.directory where parent_id =" + child.Value);
                    if (dtChild.Rows.Count > 0)
                        PopulateTreeView(dtChild, int.Parse(child.Value), child);
                }
            }
        }
    }
}