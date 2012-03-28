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
*   @file       ClShowEyesRegions.cs
*   @brief      Show ClShowEyesRegions
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-07-2008
*
*   @history
*   @item		16-07-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClShowEyesRegions : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClShowEyesRegions();
        }

        public static string ALGORITHM_NAME = @"Show\Eyes Regions";

        private double m_ThresholdK = 0.00005;

        public ClShowEyesRegions() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Threshold K"))
            {
                m_ThresholdK = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Threshold K", m_ThresholdK.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            if (iter.IsValid())
            {
                do
                {
                    double H;
                    double K;

                    if (!iter.GetSpecificValue("Gaussian_25", out K))
                        continue;
                    if (!iter.GetSpecificValue("Mean_25", out H))
                        continue;

                    if (H > 0 && K > m_ThresholdK) //  Nose
                    {
                        iter.Color = Color.Red;
                    }
                 //   else
                 //       iter.Color = Color.White;

                }
                while (iter.MoveToNext());
            }
        }
    }
}
