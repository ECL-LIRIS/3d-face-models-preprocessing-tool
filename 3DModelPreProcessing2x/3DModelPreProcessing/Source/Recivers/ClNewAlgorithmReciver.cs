using System;
using System.Collections.Generic;
using System.Text;

using PreprocessingFramework;

namespace ModelPreProcessing
{
    public class ClNewAlgorithmViewer : PreprocessingFramework.IInformationReciver
    {
        System.Windows.Forms.TreeView m_tree;

        public ClNewAlgorithmViewer(System.Windows.Forms.TreeView p_tree)
        {
            m_tree = p_tree;
            p_tree.Nodes.Clear();
        }

        public virtual void NewInformation(string p_sInformation, ClInformationSender.eInformationType p_eType)
        {
            if (p_eType != ClInformationSender.eInformationType.eNewAlgorithm)
                return;

            string[] nodes = p_sInformation.Split('\\');
            System.Windows.Forms.TreeNode currentNode = null;
            bool firstSetUp = true;

            for(int i=0; i<nodes.Length; i++)
            {
                System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
                node.Text = nodes[i];
                node.Name = nodes[i];

                System.Windows.Forms.TreeNode[] finded;
                if(firstSetUp)
                    finded = m_tree.Nodes.Find(nodes[i], false);
                else
                    finded = currentNode.Nodes.Find(nodes[i],false);

                if (finded.Length == 1)
                {
                    currentNode = finded[0];
                    firstSetUp = false;
                }
                else
                {
                    if (firstSetUp)
                    {
                        m_tree.Nodes.Add(node);
                        firstSetUp = false;
                    }
                    else
                        currentNode.Nodes.Add(node);
                    currentNode = node;
                }
            }
        }
    }
}
