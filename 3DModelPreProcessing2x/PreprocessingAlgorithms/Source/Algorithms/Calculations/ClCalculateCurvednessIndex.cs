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
*   @file       ClCurvatureSubstractionFromTheSamePoints.cs
*   @brief      Algorithm to ClCurvatureSubstractionFromTheSamePoints
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-12-2008
*
*   @history
*   @item		10-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateCurvednessIndex : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateCurvednessIndex();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Calculations\Calculate Curvedness Index";

        public ClCalculateCurvednessIndex() : base(ALGORITHM_NAME) { }

        // ------------------------- Properities
        int NeighborhoodSize = 25;
        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Neighborhood Size"))
            {
                NeighborhoodSize = Int32.Parse(p_sValue);
            }
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Neighborhood Size", NeighborhoodSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            string k1 = "K1_"+NeighborhoodSize.ToString();
            string k2 = "K2_"+NeighborhoodSize.ToString();
            string CurvednessIndex = "CurvednessIndex_"+NeighborhoodSize.ToString();

            do
            {
                double vK1, vK2;
                if (!iter.GetSpecificValue(k1, out vK1) || !iter.GetSpecificValue(k2, out vK2))
                    continue;

                double CurvednessIndexValue = Math.Sqrt(Math.Pow(vK1, 2) + Math.Pow(vK2, 2)) / 2;
                iter.AddSpecificValue("CurvednessIndex_" + NeighborhoodSize.ToString(), CurvednessIndexValue);

            } while (iter.MoveToNext());
        }
    }
}
