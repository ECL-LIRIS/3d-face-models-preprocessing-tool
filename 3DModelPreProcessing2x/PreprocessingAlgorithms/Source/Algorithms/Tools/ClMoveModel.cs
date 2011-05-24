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
*   @file       ClMoveModel.cs
*   @brief      AClMoveModel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       17-12-2008
*
*   @history
*   @item		17-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClMoveModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClMoveModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Move Model";

        public ClMoveModel() : base(ALGORITHM_NAME) { }

        private float m_fMoveX = 0;
        private float m_fMoveY = 0;
        private float m_fMoveZ = 0;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Move X"))
            {
                m_fMoveX = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Move Y"))
            {
                m_fMoveY = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Move Z"))
            {
                m_fMoveZ = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Move X", m_fMoveX.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Move Y", m_fMoveY.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Move Z", m_fMoveZ.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            while(iter.IsValid())
            {
                float X = iter.X;
                float Y = iter.Y;
                float Z = iter.Z;

                iter.X = X - m_fMoveX;
                iter.Y = Y - m_fMoveY;
                iter.Z = Z - m_fMoveZ;
                
                if (!iter.MoveToNext())
                    break;
            }
        }
    }
}
