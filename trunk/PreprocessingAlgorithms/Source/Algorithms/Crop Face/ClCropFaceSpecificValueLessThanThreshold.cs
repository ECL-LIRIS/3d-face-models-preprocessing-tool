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
*   @file       ClLoadModelCurvaturesValues.cs
*   @brief      ClLoadModelCurvaturesValues
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       08-01-2009
*
*   @history
*   @item		08-01-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCropFaceSpecificValueLessThanThreshold : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCropFaceSpecificValueLessThanThreshold();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Crop Face Specific value less than threshold";

        public ClCropFaceSpecificValueLessThanThreshold() : base(ALGORITHM_NAME) { }


        float m_ThresholdValue = 100;
        string m_SpecificValue = "AllDistancesXYZ";
        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Specific Value"))
            {
                m_SpecificValue = p_sValue;
            }
            else if (p_sProperity.Equals("Threshold"))
            {
                m_ThresholdValue = Single.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Specific Value", m_SpecificValue.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Threshold", m_ThresholdValue.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double value = 0;
                if (!iter.GetSpecificValue(m_SpecificValue, out value))
                {
                    iter.MoveToNext();
                    continue;
                }

                if (value > m_ThresholdValue)
                {
                    iter = p_Model.RemovePointFromModel(iter);
                }
                else
                {
                    iter.MoveToNext();
                }

            } while (iter.IsValid());
        }
    }
}
