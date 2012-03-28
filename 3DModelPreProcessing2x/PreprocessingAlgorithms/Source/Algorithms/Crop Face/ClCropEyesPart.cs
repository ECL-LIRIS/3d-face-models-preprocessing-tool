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
*   @file       ClCropFaceBySphere.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCropEyesPart : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCropEyesPart();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Crop Eyes Part";

        public ClCropEyesPart() : base(ALGORITHM_NAME) { }

        private float m_fEyesPartRadious = 50f;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Eyes Part Radious"))
            {
                m_fEyesPartRadious = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Eyes Part Radious", m_fEyesPartRadious.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator RightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner.ToString());
            Cl3DModel.Cl3DModelPointIterator LeftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner.ToString());

            Cl3DModel.Cl3DModelPointIterator[,] Map = null;
            int Width, Height, Xoffset, Yoffset;
            ClTools.CreateGridBasedOnRangeValues(p_Model, out Map, out Width, out Height, out Xoffset, out Yoffset);

            int RightEyeX = RightEye.RangeImageX - Xoffset;
            int RightEyeY = RightEye.RangeImageY - Yoffset;
            int LeftEyeX = LeftEye.RangeImageX - Xoffset;
            int LeftEyeY = LeftEye.RangeImageY - Yoffset;

            int MiddleX = (RightEyeX + LeftEyeX) / 2;
            int MiddleY = (RightEyeY + LeftEyeY) / 2;

            Cl3DModel.Cl3DModelPointIterator MiddlePoint = Map[MiddleX, MiddleY];
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.UnspecifiedPoint, MiddlePoint);

            List<Cl3DModel.Cl3DModelPointIterator> Neighborhood = null;
            ClTools.GetNeighborhoodWithGeodesicDistance(out Neighborhood, MiddlePoint, m_fEyesPartRadious);
            foreach (Cl3DModel.Cl3DModelPointIterator point in Neighborhood)
                point.AlreadyVisited = true;

            MiddlePoint.AlreadyVisited = true;

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                if (!iter.IsValid())
                    break;

                if (!iter.AlreadyVisited)
                    iter = p_Model.RemovePointFromModel(iter);
                else
                    if (!iter.MoveToNext())
                        break;

            } while (true);
        }
    }
}
