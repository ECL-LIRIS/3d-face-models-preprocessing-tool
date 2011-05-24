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
*   @file       ClRemoveNoseTipVertex.cs
*   @brief      ClRemoveNoseTipVertex
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       28-03-2009
*
*   @history
*   @item		28-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveOneVertex : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveOneVertex();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove One Vertex";

        public ClRemoveOneVertex() : base(ALGORITHM_NAME) { }

        private string m_point = "NoseTip";
        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Point name to remove"))
            {
                m_point = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Point name to remove", m_point.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }
        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator NoseTip = null;
            if (!p_Model.GetSpecificPoint(m_point, ref NoseTip))
                throw new Exception("Cannot get point " + m_point);

         //   p_Model.RemoveAllSpecificPoints();
            p_Model.RemovePointFromModel(NoseTip);
        }
    }
}
