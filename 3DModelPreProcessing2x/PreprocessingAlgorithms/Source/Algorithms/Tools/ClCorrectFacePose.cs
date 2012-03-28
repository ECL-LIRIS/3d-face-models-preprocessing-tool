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
*   @file       ClCorrectFacePose.cs
*   @brief      Algorithm to correct face pose based on eyes
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       04-09-2008
*
*   @history
*   @item		04-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCorrectFacePose : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCorrectFacePose();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Correct pose and move face (based on eyes na nose)";

        public ClCorrectFacePose() : base(ALGORITHM_NAME) { }

        string NoseLabel = Cl3DModel.eSpecificPoints.NoseTip.ToString();
        string LeftEyeLabel = Cl3DModel.eSpecificPoints.LeftEyeRightCorner.ToString();
        string RightEyeLabel = Cl3DModel.eSpecificPoints.RightEyeLeftCorner.ToString();

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Nose Tip Label"))
            {
                NoseLabel = p_sValue;
            }
            else if (p_sProperity.Equals("Left eye label"))
            {
                LeftEyeLabel = p_sValue;
            }
            else if (p_sProperity.Equals("Right eye label"))
            {
                RightEyeLabel = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Nose Tip Label", NoseLabel));
            list.Add(new KeyValuePair<string, string>("Left eye label", LeftEyeLabel));
            list.Add(new KeyValuePair<string, string>("Right eye label", RightEyeLabel));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator EyePoint1 = null;
            Cl3DModel.Cl3DModelPointIterator EyePoint2 = null;
            Cl3DModel.Cl3DModelPointIterator NosePoint = null;

            if (!p_Model.GetSpecificPoint(LeftEyeLabel, ref EyePoint1))
                throw new Exception("Cannot find specific point labeled as: " + LeftEyeLabel);
            if (!p_Model.GetSpecificPoint(RightEyeLabel, ref EyePoint2))
                throw new Exception("Cannot find specific point labeled as: " + RightEyeLabel);
            if (!p_Model.GetSpecificPoint(NoseLabel, ref NosePoint))
                throw new Exception("Cannot find specific point labeled as: " + NoseLabel);

            Matrix GenModel = new Matrix(3, 3);
            GenModel[0, 0] = 0;
            GenModel[1, 0] = 0; // nose
            GenModel[2, 0] = 0;

            GenModel[0, 1] = -22f;
            GenModel[1, 1] = 34f; // left eye
            GenModel[2, 1] = -35f;

            GenModel[0, 2] = 22f;
            GenModel[1, 2] = 34f; // right eye
            GenModel[2, 2] = -35f;

            Matrix Model = new Matrix(3, 3);

            Cl3DModel.Cl3DModelPointIterator point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            Model[0, 0] = point.X;
            Model[1, 0] = point.Y; // nose
            Model[2, 0] = point.Z;

            point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Model[0, 1] = point.X;
            Model[1, 1] = point.Y; // left eye
            Model[2, 1] = point.Z;

            point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);
            Model[0, 2] = point.X;
            Model[1, 2] = point.Y; // right eye
            Model[2, 2] = point.Z;

            Matrix rotationMatrix = null;
            Matrix translationMatrix = null;

            ClTools.CalculateRotationAndTranslation(GenModel, Model, out rotationMatrix, out translationMatrix);

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                Matrix V = rotationMatrix * iter + translationMatrix;
                iter.X = (float)V[0, 0];
                iter.Y = (float)V[1, 0];
                iter.Z = (float)V[2, 0];

            } while (iter.MoveToNext());

            point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            float x = point.X;
            float y = point.Y;
            float z = point.Z;
            iter = p_Model.GetIterator();
            do
            {
                iter.X = iter.X - x;
                iter.Y = iter.Y - y;
                iter.Z = iter.Z - z;
            } while (iter.MoveToNext());
        }
    }
}
