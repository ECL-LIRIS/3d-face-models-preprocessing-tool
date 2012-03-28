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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClSaveManualLandmarks.cs
*   @brief      ClSaveManualLandmarks
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       18-10-2008
*
*   @history
*   @item		18-10-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveManualLandmarks : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveManualLandmarks();
        }

        public static string ALGORITHM_NAME = @"Save\Specific points\Save landmarks as manual (.cor)";

        public ClSaveManualLandmarks() : base(ALGORITHM_NAME) { }

        public ClSaveManualLandmarks(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        string m_sFilePostFix = "";

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("FilePostFix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("FilePostFix", m_sFilePostFix.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix+ ".cor";

            List<KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator>> specPoints = p_Model.GetAllSpecificPoints();
            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@           3DModelsPreprocessing");
                tw.WriteLine("@      Przemyslaw Szeptycki LIRIS 2008");
                tw.WriteLine("@ Manual landmarks fo:");
                tw.WriteLine("@   Model name: " + p_Model.ModelFileName);
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@ Model landmarked by: 3DModelsPreprocessing_application");
                tw.WriteLine("@ File Version 1.1 generated: " + DateTime.Now.ToString());

                foreach(KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator> point in specPoints)
                {
                    string line = point.Key;

                    line += " X: " + point.Value.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Y: " + point.Value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Z: " + point.Value.Z.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    tw.WriteLine(line);               
                }
                tw.Close();
            }
        }
    }
}
