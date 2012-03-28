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
    public class ClSaveNoseTipXYandEyesAngle : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveNoseTipXYandEyesAngle();
        }

        public static string ALGORITHM_NAME = @"Save\Specific points\Save Noset Tip and RotationAngle (.Angle)";

        public ClSaveNoseTipXYandEyesAngle() : base(ALGORITHM_NAME) { }

        private string m_sFilePostFix = "";

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("PostFix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("PostFix", m_sFilePostFix.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix +".Angle";

            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString());
            Cl3DModel.Cl3DModelPointIterator rightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner.ToString());
            Cl3DModel.Cl3DModelPointIterator leftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner.ToString());

            float MeanX = (rightEye.X + leftEye.X) / 2;
            float MeanY = (rightEye.Y + leftEye.Y) / 2;

            float rotation = ClTools.GetAngle2D(MeanX, MeanY, 0, 1);

            rotation = rotation * ((float)Math.PI / 180);


            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine(
                    NoseTip.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                    NoseTip.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));

                tw.WriteLine(rotation.ToString(System.Globalization.CultureInfo.InvariantCulture));                            

                tw.Close();
            }
         }
    }
}
