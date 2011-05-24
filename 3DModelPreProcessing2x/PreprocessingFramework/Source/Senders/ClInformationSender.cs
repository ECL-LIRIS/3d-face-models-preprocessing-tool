using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PreprocessingFramework
{
    static public class ClInformationSender
    {
        public enum eInformationType
        {
            eNewAlgorithm,
            eTextInternal,
            eTextExternal,
            eDebugText,
            eProgress,
            eStartProcessing,
            eStopProcessing,
            eError,
            eColorMapChanged,
            eNextRecognitionScore,
            eWindowInfo,
        }
        public delegate void NewInformationDelegate(string p_sInformation, ClInformationSender.eInformationType p_eType);

        private static Dictionary<eInformationType, NewInformationDelegate> m_lReceivers = new Dictionary<eInformationType, NewInformationDelegate>();

        static public void RegisterReceiver(IInformationReciver p_rReceiver, eInformationType p_eInformationType)
        {
            RegisterReceiver(p_rReceiver.NewInformation, p_eInformationType);
        }
        
        static public void RegisterReceiver(NewInformationDelegate p_rReceiver, eInformationType p_eInformationType)
        {
            NewInformationDelegate Delegate;
            if (!m_lReceivers.TryGetValue(p_eInformationType, out Delegate))
            {
                Delegate += new NewInformationDelegate(p_rReceiver);
                m_lReceivers.Add(p_eInformationType, Delegate);
            }
            else
            {
                Delegate += new NewInformationDelegate(p_rReceiver);
                m_lReceivers.Remove(p_eInformationType);
                m_lReceivers.Add(p_eInformationType, Delegate);
            }
        }

        static public void UnRegisterReceiver(IInformationReciver p_rReceiver, eInformationType p_eInformationType)
        {
            UnRegisterReceiver(p_rReceiver.NewInformation, p_eInformationType);
        }
        static public void UnRegisterReceiver(NewInformationDelegate p_rReceiver, eInformationType p_eInformationType)
        {
            NewInformationDelegate Delegate;
            if (!m_lReceivers.TryGetValue(p_eInformationType, out Delegate))
            {
                Delegate -= p_rReceiver;
                m_lReceivers.Remove(p_eInformationType);
                m_lReceivers.Add(p_eInformationType, Delegate);
            }
        }

        static public void SendInformation(string p_sInformation, eInformationType p_eInformationType)
        {
            try
            {
                NewInformationDelegate Delegate;
                if (m_lReceivers.TryGetValue(p_eInformationType, out Delegate))
                {
                    Delegate(p_sInformation, p_eInformationType);
                }
            }
            catch (Exception){}
        }
    }
}
