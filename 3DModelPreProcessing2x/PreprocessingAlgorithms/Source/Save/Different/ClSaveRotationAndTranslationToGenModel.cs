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
*   @file       RoundAllxyValuesSimplifyModel.cs
*   @brief      RoundAllxyValuesSimplifyModel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveRotationAndTranslationToGenModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveRotationAndTranslationToGenModel();
        }

        public static string ALGORITHM_NAME = @"Save\Different\Save Rotation and Translation to generic model (*.rt)";

        public ClSaveRotationAndTranslationToGenModel() : base(ALGORITHM_NAME) { }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Matrix GenModel = new Matrix(3,3);
            GenModel[0, 0] = 0;
            GenModel[1, 0] = 0; // nose
            GenModel[2, 0] = 0;

            GenModel[0, 1] = -22f;
            GenModel[1, 1] = 34f; // left eye
            GenModel[2, 1] = -35f;

            GenModel[0, 2] = 22f;
            GenModel[1, 2] = 34f; // right eye
            GenModel[2, 2] = -35f;
            Matrix Model = new Matrix(3,3);

            Cl3DModel.Cl3DModelPointIterator point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString());
            Model[0, 0] = point.X;
            Model[1, 0] = point.Y; // nose
            Model[2, 0] = point.Z;

            point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner.ToString());
            Model[0, 1] = point.X;
            Model[1, 1] = point.Y; // left eye
            Model[2, 1] = point.Z;

            point = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner.ToString());
            Model[0, 2] = point.X;
            Model[1, 2] = point.Y; // right eye
            Model[2, 2] = point.Z;

            Matrix rotationMatrix = null;
            Matrix translationMatrix = null;

            ClTools.CalculateRotationAndTranslation(GenModel, Model, out rotationMatrix, out translationMatrix);

            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + ".rt";

           // List<KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator>> specPoints = p_Model.GetAllSpecificPoints();
            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@           3DModelsPreprocessing");
                tw.WriteLine("@      Przemyslaw Szeptycki LIRIS 2009");
                tw.WriteLine("@ Rotation and Translation Matrix to normalize face model");
                tw.WriteLine("@ Rotation:");
                tw.WriteLine(rotationMatrix.ToString());
                tw.WriteLine("@ Translation:");
                tw.WriteLine(translationMatrix.ToString());
                tw.Close();
            }
           /* 
            Iridium.Numerics.LinearAlgebra.Matrix q = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1);
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                q[0, 0] = iter.X;
                q[1, 0] = iter.Y;
                q[2, 0] = iter.Z;
                Iridium.Numerics.LinearAlgebra.Matrix NewQ = rotationMatrix * q + translationMatrix;
                iter.X = (float)NewQ[0, 0];
                iter.Y = (float)NewQ[1, 0];
                iter.Z = (float)NewQ[2, 0];                
            } while (iter.MoveToNext());
             */
        }
    }
}
