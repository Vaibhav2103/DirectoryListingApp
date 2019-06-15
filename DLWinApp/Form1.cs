using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLWinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtPath.Text = "";
        }



        private void btnPath_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo root = new DirectoryInfo(txtPath.Text.Trim());
                using (DirectoryTreeLoader loader = new DirectoryTreeLoader())
                {
                    loader.Load(root);
                }
                MessageBox.Show("Sub Directories Stored Successfully.");
                txtPath.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message.ToString());
            }
        }

    }
}
