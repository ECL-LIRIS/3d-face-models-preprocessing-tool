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
    public class ClSaveSpecificPointsWithIDs : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveSpecificPointsWithIDs();
        }

        public static string ALGORITHM_NAME = @"Save\Specific points\Save landmarks with IDs+1 (like 'm' files) (.pts)";

        public ClSaveSpecificPointsWithIDs() : base(ALGORITHM_NAME) { }

        public ClSaveSpecificPointsWithIDs(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        string m_sFilePostFix = "";
        bool OnlyIDs = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Only IDs"))
            {
                OnlyIDs = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("PostFix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Only IDs", OnlyIDs.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("PostFix", m_sFilePostFix.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix;

            if (!OnlyIDs)
            {
                name += ".pts";
                List<KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>> specPoints = p_Model.GetAllSpecificPoints();
                using (TextWriter tw = new StreamWriter(name, false))
                {
                    tw.WriteLine("@----------------------------------------");
                    tw.WriteLine("@      Przemyslaw Szeptycki LIRIS 2009");
                    tw.WriteLine("@           pszeptycki@gmail.com");
                    tw.WriteLine("@     Model Specific Points Coordinates");
                    tw.WriteLine("@    ID = ID + 1 the same like in M file");
                    tw.WriteLine("@   Model name: " + p_Model.ModelFileName);
                    tw.WriteLine("@----------------------------------------");
                    tw.WriteLine("@ Name Id X Y Z");
                    foreach (KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator> point in specPoints)
                    {
                        uint ID = point.Value.PointID + 1; // in the M  file all IDs starts from 1 and in our case from 0
                        string line = point.Key + " " + ID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + point.Value.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + point.Value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + point.Value.Z.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        tw.WriteLine(line);
                    }
                    tw.Close();
                }
            }
            else
            {
                List<Cl3DModel.Cl3DModelPointIterator> PointsToSave = new List<Cl3DModel.Cl3DModelPointIterator>();
                PointsToSave.Add(p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip));
                PointsToSave.Add(p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner));
                PointsToSave.Add(p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner));

                string nameID = name + ".ptsID";
                using (TextWriter tw = new StreamWriter(nameID, false))
                {
                    tw.WriteLine(PointsToSave.Count.ToString());
                    foreach (Cl3DModel.Cl3DModelPointIterator point in PointsToSave)
                    {
                        uint ID = point.PointID + 1;
                        tw.WriteLine(ID.ToString());
                    }
                    tw.Close();
                }
            }
        }
    }
}
