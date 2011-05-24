using System;
using System.Collections.Generic;
using System.Text;

using PreprocessingFramework;

namespace ModelPreProcessing
{
    public class ClNewRecognitionAlgorithmReciver : PreprocessingFramework.IInformationReciver
    {
        System.Windows.Forms.ToolStripSplitButton m_recognitionButton;

        public ClNewRecognitionAlgorithmReciver(System.Windows.Forms.ToolStripSplitButton p_recognitionButton)
        {
            m_recognitionButton = p_recognitionButton;
            m_recognitionButton.DropDownItems.Clear();
        }

        public virtual void NewInformation(string p_sInformation, ClInformationSender.eInformationType p_eType)
        {
            m_recognitionButton.DropDownItems.Add(p_sInformation);
        }
    }
}
