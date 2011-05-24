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
*   @file       ClColorNoseTipNeighborhood.cs
*   @brief      Show ClColorNoseTipNeighborhood
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       17-04-2009
*
*   @history
*   @item		17-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClColorNoseTipNeighborhood : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClColorNoseTipNeighborhood();
        }

        public static string ALGORITHM_NAME = @"Show\Color Nose Tip Neighborhood";

        private float m_NeighborhoodSize = 40.0f;

        public ClColorNoseTipNeighborhood() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Neighborhood Size"))
            {
                m_NeighborhoodSize = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Neighborhood Size", m_NeighborhoodSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator NoseTip = null;
            if (!p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString(), ref NoseTip))
                throw new Exception("Cannot get Nose Tip");

            List<Cl3DModel.Cl3DModelPointIterator> Neighborhood = null;
            ClTools.GetNeighborhoodWithGeodesicDistance(out Neighborhood, NoseTip, m_NeighborhoodSize);

            foreach (Cl3DModel.Cl3DModelPointIterator point in Neighborhood)
                point.Color = Color.Red;
        }
    }
}

