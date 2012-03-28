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
*   @file       ClSaveSpecificPointsCoordinates.cs
*   @brief      ClSaveSpecificPointsCoordinates
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       18-10-2008
*
*   @history
*   @item		18-10-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveSpecificPointsCoordinates : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveSpecificPointsCoordinates();
        }

        public static string ALGORITHM_NAME = @"Save\Specific points\Save Specific Points Coordinates (.crr)";

        public ClSaveSpecificPointsCoordinates() : base(ALGORITHM_NAME) { }

        public ClSaveSpecificPointsCoordinates(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        string m_sFilePostFix = "";


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix+ ".crr";

            List<KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator>> specPoints = p_Model.GetAllSpecificPoints();
            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@      Przemyslaw Szeptycki LIRIS 2008");
                tw.WriteLine("@ Automatic Model Specific Points Coordinates");
                tw.WriteLine("@   Model name: " + p_Model.ModelFileName);
                tw.WriteLine("@----------------------------------------");
                foreach(KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator> point in specPoints)
                {
                    string line = point.Key + " X: " + point.Value.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Y: " + point.Value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Z: " + point.Value.Z.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    tw.WriteLine(line);               
                }
                tw.Close();
            }
        }
    }
}

