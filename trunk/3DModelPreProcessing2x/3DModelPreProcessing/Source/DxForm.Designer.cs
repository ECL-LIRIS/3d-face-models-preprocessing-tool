/*  Copyright (C) 2011 Przemyslaw Szeptycki <pszeptycki@gmail.com>, Ecole Centrale de Lyon,

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ModelPreProcessing
{
    partial class DxForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DxForm));
            this.splitContainerAllApplication = new System.Windows.Forms.SplitContainer();
            this.groupBoxRender = new System.Windows.Forms.GroupBox();
            this.PanelDirectX = new System.Windows.Forms.Panel();
            this.richTextBoxModelProperties = new System.Windows.Forms.RichTextBox();
            this.checkBoxSubfolders = new System.Windows.Forms.CheckBox();
            this.checkBoxLogging = new System.Windows.Forms.CheckBox();
            this.pictureBoxAdditionalInfo = new System.Windows.Forms.PictureBox();
            this.groupBoxPreparedAlgotithm = new System.Windows.Forms.GroupBox();
            this.listViewAlgorythm = new System.Windows.Forms.ListView();
            this.contextMenuStripAlg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.sAVEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lOADToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxDepthMap = new System.Windows.Forms.GroupBox();
            this.listViewAlgorithmProperity = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnValue = new System.Windows.Forms.ColumnHeader();
            this.buttonSaveProperity = new System.Windows.Forms.Button();
            this.textBoxProperityValue = new System.Windows.Forms.TextBox();
            this.comboBoxProperityName = new System.Windows.Forms.ComboBox();
            this.pictureBoxLiris = new System.Windows.Forms.PictureBox();
            this.groupBoxOperations = new System.Windows.Forms.GroupBox();
            this.treeViewOperations = new System.Windows.Forms.TreeView();
            this.labelMyName = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStartAlgorithm = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.changeBackgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemChangeFaceColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonShowDebug = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.DebugTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBoxDebugInformation = new System.Windows.Forms.GroupBox();
            this.saveFileDialogBMP = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogTexture = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogAlgorithm = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelLast = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialogAlgorithm = new System.Windows.Forms.OpenFileDialog();
            this.splitContainerAllApplication.Panel1.SuspendLayout();
            this.splitContainerAllApplication.Panel2.SuspendLayout();
            this.splitContainerAllApplication.SuspendLayout();
            this.groupBoxRender.SuspendLayout();
            this.PanelDirectX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAdditionalInfo)).BeginInit();
            this.groupBoxPreparedAlgotithm.SuspendLayout();
            this.contextMenuStripAlg.SuspendLayout();
            this.groupBoxDepthMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLiris)).BeginInit();
            this.groupBoxOperations.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBoxDebugInformation.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerAllApplication
            // 
            resources.ApplyResources(this.splitContainerAllApplication, "splitContainerAllApplication");
            this.splitContainerAllApplication.Name = "splitContainerAllApplication";
            // 
            // splitContainerAllApplication.Panel1
            // 
            this.splitContainerAllApplication.Panel1.Controls.Add(this.groupBoxRender);
            // 
            // splitContainerAllApplication.Panel2
            // 
            this.splitContainerAllApplication.Panel2.Controls.Add(this.checkBoxSubfolders);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.checkBoxLogging);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.pictureBoxAdditionalInfo);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.groupBoxPreparedAlgotithm);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.groupBoxDepthMap);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.pictureBoxLiris);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.groupBoxOperations);
            this.splitContainerAllApplication.Panel2.Controls.Add(this.labelMyName);
            // 
            // groupBoxRender
            // 
            this.groupBoxRender.Controls.Add(this.PanelDirectX);
            resources.ApplyResources(this.groupBoxRender, "groupBoxRender");
            this.groupBoxRender.Name = "groupBoxRender";
            this.groupBoxRender.TabStop = false;
            // 
            // PanelDirectX
            // 
            resources.ApplyResources(this.PanelDirectX, "PanelDirectX");
            this.PanelDirectX.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PanelDirectX.Controls.Add(this.richTextBoxModelProperties);
            this.PanelDirectX.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.PanelDirectX.ForeColor = System.Drawing.Color.White;
            this.PanelDirectX.Name = "PanelDirectX";
            this.PanelDirectX.Tag = "";
            this.PanelDirectX.MouseLeave += new System.EventHandler(this.PanelDirectX_MouseLeave);
            this.PanelDirectX.MouseEnter += new System.EventHandler(this.PanelDirectX_MouseEnter);
            // 
            // richTextBoxModelProperties
            // 
            this.richTextBoxModelProperties.BackColor = System.Drawing.Color.DarkSalmon;
            resources.ApplyResources(this.richTextBoxModelProperties, "richTextBoxModelProperties");
            this.richTextBoxModelProperties.Name = "richTextBoxModelProperties";
            // 
            // checkBoxSubfolders
            // 
            resources.ApplyResources(this.checkBoxSubfolders, "checkBoxSubfolders");
            this.checkBoxSubfolders.Name = "checkBoxSubfolders";
            this.checkBoxSubfolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogging
            // 
            resources.ApplyResources(this.checkBoxLogging, "checkBoxLogging");
            this.checkBoxLogging.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxLogging.Checked = true;
            this.checkBoxLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.UseVisualStyleBackColor = false;
            // 
            // pictureBoxAdditionalInfo
            // 
            resources.ApplyResources(this.pictureBoxAdditionalInfo, "pictureBoxAdditionalInfo");
            this.pictureBoxAdditionalInfo.Name = "pictureBoxAdditionalInfo";
            this.pictureBoxAdditionalInfo.TabStop = false;
            // 
            // groupBoxPreparedAlgotithm
            // 
            resources.ApplyResources(this.groupBoxPreparedAlgotithm, "groupBoxPreparedAlgotithm");
            this.groupBoxPreparedAlgotithm.Controls.Add(this.listViewAlgorythm);
            this.groupBoxPreparedAlgotithm.Name = "groupBoxPreparedAlgotithm";
            this.groupBoxPreparedAlgotithm.TabStop = false;
            // 
            // listViewAlgorythm
            // 
            this.listViewAlgorythm.BackColor = System.Drawing.SystemColors.MenuBar;
            this.listViewAlgorythm.ContextMenuStrip = this.contextMenuStripAlg;
            resources.ApplyResources(this.listViewAlgorythm, "listViewAlgorythm");
            this.listViewAlgorythm.MultiSelect = false;
            this.listViewAlgorythm.Name = "listViewAlgorythm";
            this.listViewAlgorythm.ShowGroups = false;
            this.listViewAlgorythm.SmallImageList = this.imageList;
            this.listViewAlgorythm.StateImageList = this.imageList;
            this.listViewAlgorythm.TileSize = new System.Drawing.Size(50, 32);
            this.listViewAlgorythm.UseCompatibleStateImageBehavior = false;
            this.listViewAlgorythm.View = System.Windows.Forms.View.List;
            this.listViewAlgorythm.SelectedIndexChanged += new System.EventHandler(this.listViewAlgorythm_SelectedIndexChanged);
            // 
            // contextMenuStripAlg
            // 
            this.contextMenuStripAlg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Delete,
            this.Reset,
            this.sAVEToolStripMenuItem,
            this.lOADToolStripMenuItem});
            this.contextMenuStripAlg.Name = "contextMenuStripAlg";
            resources.ApplyResources(this.contextMenuStripAlg, "contextMenuStripAlg");
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Reset
            // 
            this.Reset.Image = global::ModelPreProcessing.Properties.Resources.redo_round48_h;
            this.Reset.Name = "Reset";
            resources.ApplyResources(this.Reset, "Reset");
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // sAVEToolStripMenuItem
            // 
            this.sAVEToolStripMenuItem.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Down1;
            this.sAVEToolStripMenuItem.Name = "sAVEToolStripMenuItem";
            resources.ApplyResources(this.sAVEToolStripMenuItem, "sAVEToolStripMenuItem");
            this.sAVEToolStripMenuItem.Click += new System.EventHandler(this.sAVEToolStripMenuItem_Click);
            // 
            // lOADToolStripMenuItem
            // 
            this.lOADToolStripMenuItem.Image = global::ModelPreProcessing.Properties.Resources.folder_open_48;
            this.lOADToolStripMenuItem.Name = "lOADToolStripMenuItem";
            resources.ApplyResources(this.lOADToolStripMenuItem, "lOADToolStripMenuItem");
            this.lOADToolStripMenuItem.Click += new System.EventHandler(this.lOADToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            // 
            // groupBoxDepthMap
            // 
            resources.ApplyResources(this.groupBoxDepthMap, "groupBoxDepthMap");
            this.groupBoxDepthMap.Controls.Add(this.listViewAlgorithmProperity);
            this.groupBoxDepthMap.Controls.Add(this.buttonSaveProperity);
            this.groupBoxDepthMap.Controls.Add(this.textBoxProperityValue);
            this.groupBoxDepthMap.Controls.Add(this.comboBoxProperityName);
            this.groupBoxDepthMap.Name = "groupBoxDepthMap";
            this.groupBoxDepthMap.TabStop = false;
            // 
            // listViewAlgorithmProperity
            // 
            resources.ApplyResources(this.listViewAlgorithmProperity, "listViewAlgorithmProperity");
            this.listViewAlgorithmProperity.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnValue});
            this.listViewAlgorithmProperity.Name = "listViewAlgorithmProperity";
            this.listViewAlgorithmProperity.UseCompatibleStateImageBehavior = false;
            this.listViewAlgorithmProperity.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            resources.ApplyResources(this.columnName, "columnName");
            // 
            // columnValue
            // 
            resources.ApplyResources(this.columnValue, "columnValue");
            // 
            // buttonSaveProperity
            // 
            resources.ApplyResources(this.buttonSaveProperity, "buttonSaveProperity");
            this.buttonSaveProperity.Name = "buttonSaveProperity";
            this.buttonSaveProperity.UseVisualStyleBackColor = true;
            this.buttonSaveProperity.Click += new System.EventHandler(this.buttonSaveProperity_Click);
            // 
            // textBoxProperityValue
            // 
            resources.ApplyResources(this.textBoxProperityValue, "textBoxProperityValue");
            this.textBoxProperityValue.Name = "textBoxProperityValue";
            // 
            // comboBoxProperityName
            // 
            resources.ApplyResources(this.comboBoxProperityName, "comboBoxProperityName");
            this.comboBoxProperityName.FormattingEnabled = true;
            this.comboBoxProperityName.Name = "comboBoxProperityName";
            this.comboBoxProperityName.SelectedIndexChanged += new System.EventHandler(this.comboBoxProperityName_SelectedIndexChanged);
            // 
            // pictureBoxLiris
            // 
            resources.ApplyResources(this.pictureBoxLiris, "pictureBoxLiris");
            this.pictureBoxLiris.Image = global::ModelPreProcessing.Properties.Resources.liris2;
            this.pictureBoxLiris.Name = "pictureBoxLiris";
            this.pictureBoxLiris.TabStop = false;
            // 
            // groupBoxOperations
            // 
            resources.ApplyResources(this.groupBoxOperations, "groupBoxOperations");
            this.groupBoxOperations.Controls.Add(this.treeViewOperations);
            this.groupBoxOperations.Name = "groupBoxOperations";
            this.groupBoxOperations.TabStop = false;
            // 
            // treeViewOperations
            // 
            resources.ApplyResources(this.treeViewOperations, "treeViewOperations");
            this.treeViewOperations.ImageList = this.imageList;
            this.treeViewOperations.Name = "treeViewOperations";
            this.treeViewOperations.DoubleClick += new System.EventHandler(this.treeViewOperations_DoubleClick);
            // 
            // labelMyName
            // 
            resources.ApplyResources(this.labelMyName, "labelMyName");
            this.labelMyName.Name = "labelMyName";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Model 3D";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonOpenFolder,
            this.toolStripSeparator2,
            this.toolStripButtonReset,
            this.toolStripButtonStartAlgorithm,
            this.toolStripSeparator1,
            this.toolStripSplitButton1,
            this.toolStripButtonShowDebug,
            this.toolStripButton1});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.Image = global::ModelPreProcessing.Properties.Resources.folder_open_48;
            resources.ApplyResources(this.toolStripButtonOpen, "toolStripButtonOpen");
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonOpenFolder
            // 
            this.toolStripButtonOpenFolder.Image = global::ModelPreProcessing.Properties.Resources.folder_open_48;
            resources.ApplyResources(this.toolStripButtonOpenFolder, "toolStripButtonOpenFolder");
            this.toolStripButtonOpenFolder.Name = "toolStripButtonOpenFolder";
            this.toolStripButtonOpenFolder.Click += new System.EventHandler(this.toolStripButtonOpenFolder_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButtonReset
            // 
            this.toolStripButtonReset.Image = global::ModelPreProcessing.Properties.Resources.redo_round48_h;
            resources.ApplyResources(this.toolStripButtonReset, "toolStripButtonReset");
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripButtonReset_Click);
            // 
            // toolStripButtonStartAlgorithm
            // 
            this.toolStripButtonStartAlgorithm.Image = global::ModelPreProcessing.Properties.Resources.hand;
            resources.ApplyResources(this.toolStripButtonStartAlgorithm, "toolStripButtonStartAlgorithm");
            this.toolStripButtonStartAlgorithm.Name = "toolStripButtonStartAlgorithm";
            this.toolStripButtonStartAlgorithm.Click += new System.EventHandler(this.toolStripButtonStartAlgorithm_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripSplitButton1
            // 
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBackgroundColorToolStripMenuItem,
            this.toolStripMenuItemChangeFaceColor});
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            // 
            // changeBackgroundColorToolStripMenuItem
            // 
            this.changeBackgroundColorToolStripMenuItem.Name = "changeBackgroundColorToolStripMenuItem";
            resources.ApplyResources(this.changeBackgroundColorToolStripMenuItem, "changeBackgroundColorToolStripMenuItem");
            this.changeBackgroundColorToolStripMenuItem.Click += new System.EventHandler(this.changeBackgroundColorToolStripMenuItem_Click);
            // 
            // toolStripMenuItemChangeFaceColor
            // 
            this.toolStripMenuItemChangeFaceColor.Name = "toolStripMenuItemChangeFaceColor";
            resources.ApplyResources(this.toolStripMenuItemChangeFaceColor, "toolStripMenuItemChangeFaceColor");
            this.toolStripMenuItemChangeFaceColor.Click += new System.EventHandler(this.toolStripMenuItemChangeFaceColor_Click);
            // 
            // toolStripButtonShowDebug
            // 
            this.toolStripButtonShowDebug.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonShowDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowDebug.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Down1;
            resources.ApplyResources(this.toolStripButtonShowDebug, "toolStripButtonShowDebug");
            this.toolStripButtonShowDebug.Name = "toolStripButtonShowDebug";
            this.toolStripButtonShowDebug.Click += new System.EventHandler(this.toolStripButtonShowDebug_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Down1;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DebugTextBox.DetectUrls = false;
            resources.ApplyResources(this.DebugTextBox, "DebugTextBox");
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.ReadOnly = true;
            // 
            // groupBoxDebugInformation
            // 
            resources.ApplyResources(this.groupBoxDebugInformation, "groupBoxDebugInformation");
            this.groupBoxDebugInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.groupBoxDebugInformation.Controls.Add(this.DebugTextBox);
            this.groupBoxDebugInformation.ForeColor = System.Drawing.Color.Gray;
            this.groupBoxDebugInformation.Name = "groupBoxDebugInformation";
            this.groupBoxDebugInformation.TabStop = false;
            // 
            // saveFileDialogBMP
            // 
            this.saveFileDialogBMP.DefaultExt = "bmp";
            this.saveFileDialogBMP.FileName = "FaceBMP";
            resources.ApplyResources(this.saveFileDialogBMP, "saveFileDialogBMP");
            // 
            // openFileDialogTexture
            // 
            this.openFileDialogTexture.FileName = "Texture";
            resources.ApplyResources(this.openFileDialogTexture, "openFileDialogTexture");
            // 
            // saveFileDialogAlgorithm
            // 
            this.saveFileDialogAlgorithm.DefaultExt = "txt";
            this.saveFileDialogAlgorithm.FileName = "Algorithm";
            resources.ApplyResources(this.saveFileDialogAlgorithm, "saveFileDialogAlgorithm");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1,
            this.toolStripStatusLabelLast});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
            // 
            // toolStripStatusLabelLast
            // 
            resources.ApplyResources(this.toolStripStatusLabelLast, "toolStripStatusLabelLast");
            this.toolStripStatusLabelLast.IsLink = true;
            this.toolStripStatusLabelLast.Name = "toolStripStatusLabelLast";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // openFileDialogAlgorithm
            // 
            this.openFileDialogAlgorithm.FileName = "Algorithm";
            resources.ApplyResources(this.openFileDialogAlgorithm, "openFileDialogAlgorithm");
            // 
            // DxForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainerAllApplication);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBoxDebugInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DxForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Deactivate += new System.EventHandler(this.DxForm_Deactivate);
            this.Load += new System.EventHandler(this.DxForm_Load);
            this.Activated += new System.EventHandler(this.DxForm_Activated);
            this.Resize += new System.EventHandler(this.DxForm_Resize);
            this.splitContainerAllApplication.Panel1.ResumeLayout(false);
            this.splitContainerAllApplication.Panel2.ResumeLayout(false);
            this.splitContainerAllApplication.Panel2.PerformLayout();
            this.splitContainerAllApplication.ResumeLayout(false);
            this.groupBoxRender.ResumeLayout(false);
            this.groupBoxRender.PerformLayout();
            this.PanelDirectX.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAdditionalInfo)).EndInit();
            this.groupBoxPreparedAlgotithm.ResumeLayout(false);
            this.contextMenuStripAlg.ResumeLayout(false);
            this.groupBoxDepthMap.ResumeLayout(false);
            this.groupBoxDepthMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLiris)).EndInit();
            this.groupBoxOperations.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBoxDebugInformation.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem changeBackgroundColorToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.RichTextBox DebugTextBox;
        private System.Windows.Forms.Label labelMyName;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.GroupBox groupBoxDebugInformation;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowDebug;
        private System.Windows.Forms.SplitContainer splitContainerAllApplication;
        private System.Windows.Forms.TreeView treeViewOperations;
        public System.Windows.Forms.TreeView TreeView
        {
            get
            {
                return this.treeViewOperations;
            }
        }
        private System.Windows.Forms.GroupBox groupBoxOperations;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangeFaceColor;
        private System.Windows.Forms.SaveFileDialog saveFileDialogBMP;
        private System.Windows.Forms.OpenFileDialog openFileDialogTexture;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.PictureBox pictureBoxLiris;
        private System.Windows.Forms.GroupBox groupBoxDepthMap;
        private System.Windows.Forms.SaveFileDialog saveFileDialogAlgorithm;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenFolder;
        private System.Windows.Forms.ToolStripButton toolStripButtonStartAlgorithm;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLast;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAlg;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripMenuItem Reset;
        private System.Windows.Forms.Button buttonSaveProperity;
        private System.Windows.Forms.TextBox textBoxProperityValue;
        private System.Windows.Forms.ComboBox comboBoxProperityName;
        private System.Windows.Forms.GroupBox groupBoxPreparedAlgotithm;
        private System.Windows.Forms.ListView listViewAlgorythm;
        private System.Windows.Forms.GroupBox groupBoxRender;
        private System.Windows.Forms.Panel PanelDirectX;
        private System.Windows.Forms.ListView listViewAlgorithmProperity;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnValue;
        private System.Windows.Forms.PictureBox pictureBoxAdditionalInfo;
        private System.Windows.Forms.CheckBox checkBoxLogging;
        private System.Windows.Forms.ToolStripMenuItem sAVEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lOADToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog openFileDialogAlgorithm;
        private System.Windows.Forms.CheckBox checkBoxSubfolders;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.RichTextBox richTextBoxModelProperties;
    }
}

