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
*   @file       ClShowHKClassification.cs
*   @brief      Show HKClassification
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       12-06-2008
*
*   @history
*   @item		13-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClShowHKClassification : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClShowHKClassification();
        }

        public static string ALGORITHM_NAME = @"Show\HK-Classification";

        public ClShowHKClassification() : base(ALGORITHM_NAME) { }

        string NeighborhoodSize = "25";

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Neighborhood Size"))
            {
                NeighborhoodSize = p_sValue;
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

            if(iter.IsValid())
            {
                do
                {
                    double H;
                    double K;

                    if (!iter.GetSpecificValue("Gaussian_"+NeighborhoodSize, out K))
                        continue;
                    if (!iter.GetSpecificValue("Mean_" + NeighborhoodSize, out H))
                        continue;

                    if (H < 0 && K < 0) // Saddle ridge
                        iter.Color = Color.FromArgb(0,0,255);//Blue;
                    else if (H == 0 && K < 0) // Minimal Surface
                        iter.Color = Color.Orange;
                    else if (H > 0 && K < 0) // Saddle Valley
                        iter.Color = Color.FromArgb(255,255,0);//Yellow;

                    else if (H < 0 && K == 0) // Ridge, Cylindrical convex (wypukłość)
                        iter.Color = Color.FromArgb(0,255,255);//SkyBlue;
                    else if (H == 0 && K == 0) // Plane
                        iter.Color = Color.FromArgb(255,255,255);
                    else if (H > 0 && K == 0) // Valley, Cylindrical concave (wklęsłość)
                        iter.Color = Color.FromArgb(255,0,255);//Pink;

                    else if (H < 0 && K > 0) // Peak, Elliptical convex (wypukłość)
                        iter.Color = Color.FromArgb(0,255,0);//Green;
                    else if (H == 0 && K > 0) // !!! Impossible
                        iter.Color = Color.Black;
                    else if (H > 0 && K > 0) // Pit, Elliptical concave (wklęsłość)
                        iter.Color = Color.FromArgb(255,0,0);//Red;
                    else
                        iter.Color = Color.White;
                }
                while (iter.MoveToNext());
            }
        }
    }
}
