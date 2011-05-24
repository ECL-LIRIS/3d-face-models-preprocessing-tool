using System;
using System.Collections.Generic;
using System.Text;

using PreprocessingFramework;

namespace ModelPreProcessing
{
    public class ClInformationReciver : PreprocessingFramework.IInformationReciver
    {
        delegate void UpdateUIDelegate(string p_iInfo, ClInformationSender.eInformationType p_eType);

        private DxForm m_form;
        private UpdateUIDelegate d;

        public ClInformationReciver(DxForm p_form)
        {
            m_form = p_form;
            d = m_form.UpdateUI;
        }

        public virtual void NewInformation(string p_sInformation, ClInformationSender.eInformationType p_eType)
        {
            if (p_eType == ClInformationSender.eInformationType.eStartProcessing)
            {
#if RENDER_1
                ClRender.getInstance().StopRendering();
#endif
            }
            else if (p_eType == ClInformationSender.eInformationType.eStopProcessing)
            {
#if RENDER_1
             //   ClRender.getInstance().StartRendering();
#endif
            }

            m_form.Invoke(d, new object[] { p_sInformation, p_eType });
        }
    }
}
