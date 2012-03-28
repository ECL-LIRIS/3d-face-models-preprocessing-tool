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
