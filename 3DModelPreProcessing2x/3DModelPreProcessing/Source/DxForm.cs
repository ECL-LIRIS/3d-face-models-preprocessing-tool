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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using System.IO;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       DxForm.cs
*   @brief      Main application form
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       24-10-2007
*
*   @history
*   @item		24-10-2007 Przemyslaw Szeptycki     created at ECL
*/
namespace ModelPreProcessing
{
    public partial class DxForm : Form
    {
        private string APPLICATION_NAME = "3D Model Pre-processing & Analysis";
        private string APPLICATION_VERSION = "x.x";
        private Cl3DModel m_Model3D = null;
        private List<IFaceAlgorithm> m_lAlgorithmsList = new List<IFaceAlgorithm>();
        private string m_sTestDirectory = "";

        public Color GetColorGray(float p_iProcent, float p_fPower) // Procent form 0 to 1
        {
            if (p_iProcent < 0 || p_iProcent > 1)
                throw new Exception("Wrong procent value, should be <0,1>");

            float rgb = (float)Math.Pow((double)p_iProcent, (double)p_fPower) * 255;
            if ((int)rgb > 255 || (int)rgb < 0)
                throw new Exception("Wrong color value ");

            return Color.FromArgb((int)rgb, (int)rgb, (int)rgb);
        }

        public DxForm(string ApplicationName, string ApplicationVersion)
        {
            APPLICATION_NAME = ApplicationName;
            APPLICATION_VERSION = ApplicationVersion;
#if RENDER_0
            APPLICATION_VERSION += " [version without 3D models rendering]";
#endif

            InitializeComponent();
#if RENDER_1
            Application.AddMessageFilter(new CScrollPanelMessageFilter(this.PanelDirectX)); // neaded to get event about scroll wheal in the panel
            this.PanelDirectX.MouseMove += new System.Windows.Forms.MouseEventHandler(ClEventSender.getInstance().BroadcastMouseMoveEvent);
            this.PanelDirectX.MouseDown += new System.Windows.Forms.MouseEventHandler(ClEventSender.getInstance().BroadcastMouseButtonDownEvent);
            this.PanelDirectX.MouseUp += new System.Windows.Forms.MouseEventHandler(ClEventSender.getInstance().BroadcastMouseButtonUpEvent);
            this.PanelDirectX.MouseWheel += new System.Windows.Forms.MouseEventHandler(ClEventSender.getInstance().BroadcastMouseWheelEvent);
#endif
            ClInformationReciver viewer = new ClInformationReciver(this);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eDebugText);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eError);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eProgress);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eTextInternal);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eTextExternal);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eStartProcessing);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eStopProcessing);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eColorMapChanged);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eNextRecognitionScore);
            ClInformationSender.RegisterReceiver(viewer, ClInformationSender.eInformationType.eWindowInfo);

            this.Text = APPLICATION_NAME+" ver. "+APPLICATION_VERSION;

#if RENDER_1
            ClRender Render = ClRender.getInstance();
            Render.CreateDevice(this.PanelDirectX);

            ClCamera Camera = new ClCamera();   // Create camera object
            Camera.RegisterForEvent(ClEventSender.eEvents.e_MouseMove);
            Camera.RegisterForEvent(ClEventSender.eEvents.e_MouseButtonDown);
            Camera.RegisterForEvent(ClEventSender.eEvents.e_MouseButtonUp);
            Camera.RegisterForEvent(ClEventSender.eEvents.e_MouseWheel);
            Render.SetCamera(Camera);   // Register camera as a render object
            
          //  Render.AddRenderObj(new ClCoordinateSystem());
#endif
            List<string> fileTypes = Cl3DModel.sm_ListManagedFilesExtensions;
            string filter = "";
            string AllNames = "All supported types";
            string AllExt = "";
            foreach (string type in fileTypes)
            {
                AllExt += "*." + type + ";";
                filter += type.ToUpper() + "|*." + type+"|";
            }
            filter = filter.Remove(filter.Length - 1);
            AllExt = AllExt.Remove(AllExt.Length - 1);
            openFileDialog.Filter = AllNames+"|"+AllExt+"|"+filter;
        }

        Bitmap pictureCorrelationMatrix = null;
        public void UpdateUI(string p_sInfo, ClInformationSender.eInformationType p_eType)
        {
            if (p_eType == ClInformationSender.eInformationType.eError || p_eType == ClInformationSender.eInformationType.eDebugText || p_eType == ClInformationSender.eInformationType.eTextInternal || p_eType == ClInformationSender.eInformationType.eTextExternal)
            {
                this.DebugTextBox.Text += "\n-> "+p_sInfo;
            }
            if (p_eType == ClInformationSender.eInformationType.eError)
            {
                MessageBox.Show(p_sInfo, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (p_eType == ClInformationSender.eInformationType.eTextExternal)
            {
                this.toolStripStatusLabel1.Text = p_sInfo;
            }
            if (p_eType == ClInformationSender.eInformationType.eTextInternal)
            {
                this.toolStripStatusLabel2.Text = p_sInfo;
            }
            if (p_eType == ClInformationSender.eInformationType.eProgress)
            {
                int proc = Convert.ToInt32(p_sInfo);
                this.toolStripProgressBar1.Value = proc;
            }
            if (p_eType == ClInformationSender.eInformationType.eStartProcessing)
            {
                this.toolStripButtonOpen.Enabled = false;
                this.toolStripButtonOpenFolder.Enabled = false;
                this.toolStripButtonOpen.Enabled = false;
                pictureCorrelationMatrix = null;
            }
            if (p_eType == ClInformationSender.eInformationType.eStopProcessing)
            {
                this.toolStripButtonOpen.Enabled = true;
                this.toolStripButtonOpenFolder.Enabled = true;
                this.toolStripButtonOpen.Enabled = true;
                MessageBox.Show("Processing has ended !", "END of processing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (p_eType == ClInformationSender.eInformationType.eColorMapChanged)
            {
                //"ColorMap: Max: 2.343455 Min: -2.54333" 
                string[] splitedInfo = p_sInfo.Split(' ');
                float MaxValue = Single.Parse(splitedInfo[2]);
                float MinValue = Single.Parse(splitedInfo[4]);
                
                Bitmap info = new Bitmap(200,300);
                int WidthOfField = 40;
                int StartField = 20;
                for (int j = StartField; j < info.Height; j++)
                {
                    Color newColor = Preprocessing.ClTools.GetColorRGB(((float)j - StartField) / (info.Height - StartField - 1), 1.0f);

                    for (int i = 0; i < Math.Min(info.Width, WidthOfField); i++)
                    {
                        info.SetPixel(i, j, newColor);
                    }
                }

                Graphics graphic = Graphics.FromImage(info);

                string NameOfPlot = splitedInfo[0];
                graphic.DrawString(NameOfPlot, new Font("Arial", 10, FontStyle.Bold), new System.Drawing.SolidBrush(Color.Black), 5, 1);

                graphic.DrawString(MaxValue.ToString(), new Font("Arial", 10, FontStyle.Bold), new System.Drawing.SolidBrush(Preprocessing.ClTools.GetColorRGB(1.0f, 1.0f)), WidthOfField + 5, info.Height - 15);
                graphic.DrawString(MinValue.ToString(), new Font("Arial", 10, FontStyle.Bold), new System.Drawing.SolidBrush(Preprocessing.ClTools.GetColorRGB(0.0f, 1.0f)), WidthOfField + 5, StartField);

                int ZeroPosition = (int)(((0 - MinValue) / (MaxValue - MinValue)) * info.Height);
                if(ZeroPosition >=0)
                    graphic.DrawString("0.0", new Font("Arial", 10, FontStyle.Bold), new System.Drawing.SolidBrush(info.GetPixel(0, ZeroPosition)), WidthOfField + 5, ZeroPosition);

                pictureBoxAdditionalInfo.Image = info;
            }
            if (p_eType == ClInformationSender.eInformationType.eNextRecognitionScore)
            {
                string[] splitedInfo = p_sInfo.Split(' ');
                string[] score1 = splitedInfo[0].Split('/');
                string[] score2 = splitedInfo[1].Split('/');

                int x = Int32.Parse(score1[0]);
                int width = Int32.Parse(score1[1]);

                int y = Int32.Parse(score2[0]);
                int height = Int32.Parse(score2[1]);

                if (pictureCorrelationMatrix == null)
                {
                    pictureCorrelationMatrix = new Bitmap(width, height);
                    for(int i=0; i< width; i++)
                        for(int j=0; j< height; j++)
                            pictureCorrelationMatrix.SetPixel(i,j,Color.White);
                }

                pictureCorrelationMatrix.SetPixel(x, y, Color.Red);
                pictureBoxAdditionalInfo.Image = pictureCorrelationMatrix;
            }
            if (p_eType == ClInformationSender.eInformationType.eWindowInfo)
            {
                MessageBox.Show(p_sInfo,"Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    folderBrowserDialog.SelectedPath = "";
                    this.Text = APPLICATION_NAME+ " <" + openFileDialog.FileName + ">";

                    m_Model3D = new Cl3DModel();
                    m_Model3D.LoadModel(openFileDialog.FileName);
                    #if RENDER_1
                    ClRender.getInstance().AddRenderObj(new Cl3DRenderModel(m_Model3D));
                    #endif
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
            
        }

        private void changeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            try
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    #if RENDER_1
                    ClRender.getInstance().SetBackgroundColor(colorDialog.Color);
                    #endif
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            } 
        }

        private void toolStripMenuItemChangeFaceColor_Click(object sender, EventArgs e)
        {
            try
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    if (m_Model3D != null)
                    {
                        m_Model3D.ResetColor(colorDialog.Color);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void toolStripButtonShowDebug_Click(object sender, EventArgs e)
        {
            groupBoxDebugInformation.Visible = !groupBoxDebugInformation.Visible;
            if (groupBoxDebugInformation.Visible)
            {
                groupBoxDebugInformation.BringToFront();
            }
            if (groupBoxDebugInformation.Visible)
                toolStripButtonShowDebug.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Up1;
            else
                toolStripButtonShowDebug.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Down1;
        }

        private void treeViewOperations_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (treeViewOperations.SelectedNode != null)
                {
                    if (treeViewOperations.SelectedNode.Nodes.Count == 0) // last node so it is algorithm
                    {
                        listViewAlgorythm.Items.Add(treeViewOperations.SelectedNode.Text, 0);
                        string algName = treeViewOperations.SelectedNode.FullPath.ToString();
                        m_lAlgorithmsList.Add(ClMapObjectAlgorithmBuilder.CreateNewAlgorithm(algName));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\n\n Call Stack:\n\n"+ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClFasadaPreprocessing.ResetFasade();
#if RENDER_1
                ClRender.getInstance().DeleteAllRenderObj();
#endif
                if (m_Model3D != null)
                {
                    m_Model3D.ResetModel();
                    m_Model3D = null;
                }
                m_sTestDirectory = "";
                this.Text = APPLICATION_NAME + " ver. " + APPLICATION_VERSION;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }


        private void toolStripButtonOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Text = APPLICATION_NAME + " <" + folderBrowserDialog.SelectedPath +">";
                    m_sTestDirectory = folderBrowserDialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void toolStripButtonStartAlgorithm_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClFasadaPreprocessing.IsPreProcessing())
                {
                    ClFasadaPreprocessing.ResetFasade();
                    return;
                }

                if (m_lAlgorithmsList.Count == 0)
                {
                    MessageBox.Show("Choose PreProcessing Algorithm(s)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (m_Model3D == null)
                {
                    if(m_sTestDirectory.Length == 0)
                    {
                        MessageBox.Show("Choose Test Directory", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    FilesExtensionForm form = new FilesExtensionForm(Cl3DModel.sm_ListManagedFilesExtensions);
                    form.ShowDialog();
                    if (form.OutExtensionList.Count == 0)
                    {
                        MessageBox.Show("No files Extension has been chosen", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    Cursor.Current = Cursors.WaitCursor;
                    ClFasadaPreprocessing.RunAlgorithms(m_sTestDirectory, m_lAlgorithmsList, form.OutExtensionList, checkBoxLogging.Checked, checkBoxSubfolders.Checked); // w przyszlosci mozna wybrac formaty ktore chce sie przetwarzac
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ClFasadaPreprocessing.RunAlgorithms(m_Model3D, m_lAlgorithmsList);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if(listViewAlgorythm.SelectedItems.Count == 1)
                {
                    int index = listViewAlgorythm.SelectedItems[0].Index;
                    listViewAlgorythm.Items.RemoveAt(index);
                    m_lAlgorithmsList.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            try
            {
                listViewAlgorythm.Clear();
                m_lAlgorithmsList.Clear();
                listViewAlgorithmProperity.Items.Clear();
                comboBoxProperityName.Items.Clear();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void listViewAlgorythm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listViewAlgorythm.SelectedItems.Count == 1)
                {
                    comboBoxProperityName.Items.Clear();
                    listViewAlgorithmProperity.Items.Clear();
                    int index = listViewAlgorythm.SelectedItems[0].Index;
                    List<KeyValuePair<string, string>> properities = m_lAlgorithmsList[index].GetProperitis();
                    
                    foreach(KeyValuePair<string, string> prop in properities)
                    {
                        ListViewItem it = new ListViewItem(prop.Key);
                        it.SubItems.Add(prop.Value);
                        listViewAlgorithmProperity.Items.Add(it);
                        comboBoxProperityName.Items.Add(prop.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void buttonSaveProperity_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewAlgorythm.SelectedItems.Count == 1)
                {
                    int index = listViewAlgorythm.SelectedItems[0].Index;
                    if (comboBoxProperityName.SelectedIndex != -1)
                    {
                        string SelectedObj = comboBoxProperityName.Items[comboBoxProperityName.SelectedIndex].ToString();
                        m_lAlgorithmsList[index].SetProperitis(SelectedObj, textBoxProperityValue.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void comboBoxProperityName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(comboBoxProperityName.SelectedIndex != -1)
                {
                    int index = comboBoxProperityName.SelectedIndex;
                    textBoxProperityValue.Text = listViewAlgorithmProperity.Items[index].SubItems[1].Text;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void DxForm_Resize(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Width = (int)(this.Width * 0.3);
                toolStripStatusLabel2.Width = (int)(this.Width * 0.4);
                toolStripStatusLabelLast.Width = (int)(this.Width * 0.1);
                toolStripProgressBar1.Width = (int)(this.Width * 0.2);
                
            }
            catch (Exception ex)
            {
                //Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void DxForm_Activated(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void DxForm_Deactivate(object sender, EventArgs e)
        {
            try
            {
#if RENDER_1
                if (!ClFasadaPreprocessing.IsPreProcessing())
                    ClRender.getInstance().StopRendering();
#endif
            }
            catch (Exception ex)
            {
                //Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

       
        private void sAVEToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // m_lAlgorithmsList
            try
            {
                if (saveFileDialogAlgorithm.ShowDialog() == DialogResult.OK)
                {
                    using (TextWriter tw = new StreamWriter(saveFileDialogAlgorithm.FileName, false))
                    {
                        foreach (IFaceAlgorithm alg in m_lAlgorithmsList)
                        {
                            tw.WriteLine("Algorithm");
                            tw.WriteLine(alg.GetAlgorithmFullPath());
                            tw.WriteLine(alg.GetProperitis().Count.ToString());
                            foreach (KeyValuePair<string, string> par in alg.GetProperitis())
                            {
                                tw.WriteLine(par.Key + "\t" + par.Value);
                            }
                        }
                        tw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void lOADToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // m_lAlgorithmsList
            try
            {
                if (openFileDialogAlgorithm.ShowDialog() == DialogResult.OK)
                {
                    using (TextReader tr = new StreamReader(openFileDialogAlgorithm.FileName)) 
                    {
                        String line = "";
                        while ((line = tr.ReadLine()) != null)
                        {
                            if(line.Equals("Algorithm"))
                            {
                                line = tr.ReadLine();
                                IFaceAlgorithm alg = ClMapObjectAlgorithmBuilder.CreateNewAlgorithm(line);
                                
                                int no = Int32.Parse(tr.ReadLine());
                                for (int i = 0; i < no; i++)
                                {
                                    string InnerLine = tr.ReadLine();
                                    if(InnerLine == null)
                                        continue;

                                    string[] keyV = InnerLine.Split('\t');
                                    alg.SetProperitis(keyV[0], keyV[1]);
                                }

                                m_lAlgorithmsList.Add(alg);

                                listViewAlgorythm.Items.Add(alg.GetAlgorithmName(), 0);
                            }
                            else
                            {
                                throw new Exception("Unknown tocken");
                            }
                        }
                        tr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //Cursor.Current = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Exception !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClInformationSender.SendInformation("EXCEPTION\n" + ex.Message + "\n\n Call Stack:\n\n" + ex.StackTrace, ClInformationSender.eInformationType.eDebugText);
            }
        }

        private void DxForm_Load(object sender, EventArgs e)
        {

        }

        private void PanelDirectX_MouseEnter(object sender, EventArgs e)
        {
            #if RENDER_1
            if (!ClFasadaPreprocessing.IsPreProcessing())
                    ClRender.getInstance().StartRendering();
            #endif
        }

        private void PanelDirectX_MouseLeave(object sender, EventArgs e)
        {
            #if RENDER_1
            if (!ClFasadaPreprocessing.IsPreProcessing())
                    ClRender.getInstance().StopRendering();
            #endif
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            richTextBoxModelProperties.Size = new Size((int)(PanelDirectX.Size.Width * 0.9f), (int)(PanelDirectX.Size.Height * 0.9f));

            richTextBoxModelProperties.Top = 5;
            richTextBoxModelProperties.Left = 5;

            richTextBoxModelProperties.Text = "MODEL's PROPERTIES";

            richTextBoxModelProperties.Visible = !richTextBoxModelProperties.Visible;
            if (m_Model3D != null)
                richTextBoxModelProperties.Text += "\n" + m_Model3D.GetModelInfo();
            else
                richTextBoxModelProperties.Text += "\nNo model loaded";

            if(richTextBoxModelProperties.Visible)
                toolStripButton1.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Up1;
            else
                toolStripButton1.Image = global::ModelPreProcessing.Properties.Resources.Arrow_Down1;
           
        }



    }
}