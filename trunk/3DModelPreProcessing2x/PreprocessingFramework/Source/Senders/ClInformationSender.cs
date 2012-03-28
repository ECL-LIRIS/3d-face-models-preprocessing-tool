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
