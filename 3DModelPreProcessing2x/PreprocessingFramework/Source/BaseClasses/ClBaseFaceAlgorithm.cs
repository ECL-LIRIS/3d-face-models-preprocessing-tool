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
using System.Linq;
using System.Text;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClBaseFaceAlgorithm.cs
*   @brief      Base Class for all the Cl3DModel processing algorithm
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       26-10-2007
*
*   @history
*   @item		26-10-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace PreprocessingFramework
{
    public abstract class ClBaseFaceAlgorithm : IFaceAlgorithm
    {
        private static int m_sNextAlgorithmNumber = 0;
        private string m_sAlgorithmFullPath;
        private string m_sAlgorithmName;

        private int m_iAlgorithmID;
        private TimeSpan m_AlgorithmTime = new TimeSpan();

        public ClBaseFaceAlgorithm(string p_sAlgorithmName)
        {
            m_sAlgorithmFullPath = p_sAlgorithmName;
            m_iAlgorithmID = m_sNextAlgorithmNumber++;
            
            string[] nodes = m_sAlgorithmFullPath.Split('\\');
            if (nodes.Length == 0)
                m_sAlgorithmName = m_sAlgorithmFullPath;

            m_sAlgorithmName = nodes[nodes.Length - 1];
        }

        public virtual string GetAlgorithmFullPath()
        {
            return m_sAlgorithmFullPath;
        }

        public virtual string GetAlgorithmName()
        {
            return m_sAlgorithmName;
        }

        public virtual int GetAlgorithmID()
        {
            return m_iAlgorithmID;
        }

        public virtual void MakeAlgorithm(Cl3DModel p_Model)
        {
            MakeAlgorithm(ref p_Model);
        }

        public virtual void MakeAlgorithm(ref Cl3DModel p_Model)
        {
            if (p_Model == null)
                throw new Exception("NULL model");

            ClInformationSender.SendInformation("-> In progress: " + m_sAlgorithmName +" for: " +p_Model.ModelFileName, ClInformationSender.eInformationType.eTextInternal);
            p_Model.ResetVisitedPoints();
            DateTime start = DateTime.Now;
            m_AlgorithmTime = TimeSpan.Zero;

                Algorithm(ref p_Model);

            DateTime stop = DateTime.Now;
            m_AlgorithmTime = (stop - start);
            p_Model.IsModelChanged = true;
            ClInformationSender.SendInformation("-] Finished: " + m_sAlgorithmName + " for: " + p_Model.ModelFileName+" [time: " + m_AlgorithmTime.TotalSeconds + " sec.]", ClInformationSender.eInformationType.eTextInternal);
            
            string fullAlgorithmAdditionalData = GetAlgorithmFullPath() + "\n";
            foreach (KeyValuePair<string, string> prop in GetProperitis())
                fullAlgorithmAdditionalData += "\t" + prop.Key + " -> " + prop.Value + "\n";
            p_Model.AddDoneProcessingAlgorithm(fullAlgorithmAdditionalData);
        }

        protected abstract void Algorithm(ref Cl3DModel p_Model);

        public virtual void SetProperitis(string p_sProperity, string p_sValue)
        {
            throw new Exception("Unknown properity: " + p_sProperity);
        }

        public virtual List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            return list;
        }

        public virtual TimeSpan GetComputationTime()
        {
            return m_AlgorithmTime;
        }
    }
}
