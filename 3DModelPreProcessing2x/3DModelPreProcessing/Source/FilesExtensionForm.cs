using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelPreProcessing
{
    public partial class FilesExtensionForm : Form
    {
        private List<string> m_ExtensionList = null;
        private List<CheckBox> checkList = new List<CheckBox>();

        public List<string> OutExtensionList = new List<string>();

        public FilesExtensionForm(List<string> extensionList)
        {
            InitializeComponent();

            int nextX = 15;
            foreach (string name in extensionList)
            {
                CheckBox chec = new CheckBox();
                chec.Parent = this.groupBoxExtensions;
                chec.Text = name.ToUpper()+ " (*."+name+")";
                chec.Name = name;
                chec.Location = new Point(3, nextX);
                checkList.Add(chec);
                nextX += chec.Height;
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (CheckBox check in checkList)
                if (check.Checked)
                    OutExtensionList.Add(check.Name);

            this.Close();
        }
    }
}
