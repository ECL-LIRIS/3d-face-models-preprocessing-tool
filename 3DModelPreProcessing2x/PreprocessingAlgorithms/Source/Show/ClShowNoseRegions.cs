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
*   @file       ClShowNoseRegions.cs
*   @brief      Show ClShowNoseRegions
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       12-06-2008
*
*   @history
*   @item		13-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClShowNoseRegions : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClShowNoseRegions();
        }

        public static string ALGORITHM_NAME = @"Show\Nose Regions";

        private double m_ThresholdK = 0.002;

        public ClShowNoseRegions() : base(ALGORITHM_NAME) { }

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

            if(iter.IsValid())
            {
                do
                {
                    double H;
                    double K;
                    double ShapeIndex;

                    if (!iter.GetSpecificValue("Gaussian_25", out K))
                        continue;
                    if (!iter.GetSpecificValue("Mean_25", out H))
                        continue;

                    if (H < 0 && K > m_ThresholdK) //  Nose
                    {
                        iter.Color = Color.Green;
                    }
                }
                while (iter.MoveToNext());
            }
        }
    }
}
