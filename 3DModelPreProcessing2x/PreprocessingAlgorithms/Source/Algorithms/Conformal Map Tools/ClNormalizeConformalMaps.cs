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
using Iridium.Numerics.LinearAlgebra;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClNormalizeConformalMaps.cs
*   @brief      Algorithm to correct face pose based on eyes
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       04-09-2008
*
*   @history
*   @item		04-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClNormalizeConformalMaps : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClNormalizeConformalMaps();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Conformal Map Tools\Normalize UV conformal parametrization";

        public ClNormalizeConformalMaps() : base(ALGORITHM_NAME) { }

        int m_RadiousOfConformalMap = 50;
        bool centerConformalMaps = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Radious Of Conformal Map"))
            {
                m_RadiousOfConformalMap = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Center Conforma Map"))
            {
                centerConformalMaps = bool.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Radious Of Conformal Map", m_RadiousOfConformalMap.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Center Conforma Map", centerConformalMaps.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            Cl3DModel.Cl3DModelPointIterator max = iter.CopyIterator();
            float distanceMax = 0;

            do
            {
                float distance = (float)Math.Sqrt(Math.Pow(iter.U, 2) + Math.Pow(iter.V, 2));
                if (distanceMax < distance)
                    distanceMax = distance;

            } while (iter.MoveToNext());

            iter = p_Model.GetIterator();
            do
            {
                iter.U = (iter.U / distanceMax) * m_RadiousOfConformalMap;
                iter.V = (iter.V / distanceMax) * m_RadiousOfConformalMap;
            } while (iter.MoveToNext());
     
            Cl3DModel.Cl3DModelPointIterator LeftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);
            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);

            Cl3DModel.Cl3DModelPointIterator tmp;
            if (LeftEye.X > RightEye.X)
            {
                tmp = LeftEye.CopyIterator();
                RightEye = LeftEye.CopyIterator();
                LeftEye = tmp.CopyIterator();
            }


            double EyesDistance = Math.Sqrt(Math.Pow(LeftEye.U - RightEye.U,2) + Math.Pow(LeftEye.V - RightEye.V,2));
            double NoseLeftEyeDistance = Math.Sqrt(Math.Pow(LeftEye.U - NoseTip.U, 2) + Math.Pow(LeftEye.V - NoseTip.V, 2));
            double NoseRightEyeDistance = Math.Sqrt(Math.Pow(NoseTip.U - RightEye.U, 2) + Math.Pow(NoseTip.V - RightEye.V, 2));

            double AverageNoseTipEyeDistance = (NoseLeftEyeDistance + NoseRightEyeDistance) / 2;


            Matrix GenModel = new Matrix(2, 3);
            GenModel[0, 0] = 0;
            GenModel[1, 0] = 0; // nose

            GenModel[0, 1] = (EyesDistance / 2);
            GenModel[1, 1] = AverageNoseTipEyeDistance; // left eye

            GenModel[0, 2] = -(EyesDistance / 2);
            GenModel[1, 2] = AverageNoseTipEyeDistance; // right eye

            Matrix Model = new Matrix(2, 3);

            Model[0, 0] = NoseTip.U;
            Model[1, 0] = NoseTip.V; // nose

            Model[0, 1] = LeftEye.U;
            Model[1, 1] = LeftEye.V; // left eye

            Model[0, 2] = RightEye.U;
            Model[1, 2] = RightEye.V; // right eye

            Matrix rotationMatrix = null;
            Matrix translationMatrix = null;

            ClTools.CalculateRotationAndTranslation(GenModel, Model, out rotationMatrix, out translationMatrix);

            Iridium.Numerics.LinearAlgebra.Matrix q = new Iridium.Numerics.LinearAlgebra.Matrix(2, 1);
            iter = p_Model.GetIterator();
            do
            {
                q[0, 0] = iter.U;
                q[1, 0] = iter.V;
                Iridium.Numerics.LinearAlgebra.Matrix NewQ = rotationMatrix * q +translationMatrix;
                iter.U = (float)NewQ[0, 0];
                iter.V = (float)NewQ[1, 0];
            } while (iter.MoveToNext());
            
            float U = NoseTip.U;
            float V = NoseTip.V;
            iter = p_Model.GetIterator();
            do
            {
                iter.U -= U;
                iter.V -= V;
            } while (iter.MoveToNext());

            if (centerConformalMaps)
            {
                float middleU = 0;
                float middleV = 0;
                iter = p_Model.GetIterator();
                do
                {
                    middleU += iter.U;
                    middleV += iter.V;
                } while (iter.MoveToNext());

                middleU /= p_Model.ModelPointsCount;
                middleV /= p_Model.ModelPointsCount;
                iter = p_Model.GetIterator();
                do
                {
                    iter.U -= middleU;
                    iter.V -= middleV;
                } while (iter.MoveToNext());



            }
        }
    }
}
