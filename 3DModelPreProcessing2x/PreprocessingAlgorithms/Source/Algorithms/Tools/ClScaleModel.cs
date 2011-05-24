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
*   @file       ClScaleModel.cs
*   @brief      ClScaleModel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClScaleModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClScaleModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Scale Model";

        public ClScaleModel() : base(ALGORITHM_NAME) { }

        private float m_fScale = 2.0f;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Scale"))
            {
                m_fScale = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Scale", m_fScale.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            while (iter.IsValid())
            {
                iter.X = iter.X * m_fScale;
                iter.Y = iter.Y * m_fScale;
                iter.Z = iter.Z * m_fScale;

                if (!iter.MoveToNext())
                    break;
            }
        }
    }
}
