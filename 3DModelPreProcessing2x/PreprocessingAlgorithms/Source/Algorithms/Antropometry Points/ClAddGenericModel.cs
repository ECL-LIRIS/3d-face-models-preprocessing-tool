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
*   @file       ClAddGenericModel.cs
*   @brief      Algorithm to Add Generic Model
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       04-09-2008
*
*   @history
*   @item		04-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClAddGenericModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClAddGenericModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Anthropometry Points\Add GenericModel";

        public ClAddGenericModel() : base(ALGORITHM_NAME) { }

        private float m_MinDistanceToAddPoint = 50;

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeRightCorner = null;
            Cl3DModel.Cl3DModelPointIterator SavedLeftCornerOfRightEye = null;
            Cl3DModel.Cl3DModelPointIterator SavedNoseTip = null;

            if (!p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner.ToString(), ref SavedLeftEyeRightCorner))
                throw new Exception("Cannot find specific point EyePoint");
            if (!p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner.ToString(), ref SavedLeftCornerOfRightEye))
                throw new Exception("Cannot find specific point EyePoint");
            if (!p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString(), ref SavedNoseTip))
                throw new Exception("Cannot find specific point NosePoint");

            // Normalized Generic Model (distance between eyes = 1) from IV2
            Vector GenNoseTip = new Vector(new double[]{0f, 0f, 0f});
            Vector GenLeftEyeRightCorner = new Vector(new double[]{-0.5f, 0.88f, -1f});
            Vector GenLeftCornerOfRightEye = new Vector(new double[]{0.5f, 0.88f, -1f});

            Vector GenLeftCornerOfLeftEye = new Vector(new double[]{-1.2f, 0.85f, -1f});
            Vector GenRightCornerOfRightEye = new Vector(new double[]{1.2f, 0.85f, -1f});

            Vector GenLeftCornerOfNose = new Vector(new double[]{-0.45f, -0.05f, -0.73f});
            Vector GenRightCornerOfNose = new Vector(new double[]{0.45f, -0.05f, -0.73f});

            Vector GenLeftCornerOfLips = new Vector(new double[]{-0.66f, -0.8f, -0.82f});
            Vector GenRightCornerOfLips = new Vector(new double[]{0.66f, -0.8f, -0.82f});

            float DistBetweenEyes = (float)Math.Sqrt(Math.Pow(SavedLeftCornerOfRightEye.X - SavedLeftEyeRightCorner.X, 2) + Math.Pow(SavedLeftCornerOfRightEye.Y - SavedLeftEyeRightCorner.Y, 2) + Math.Pow(SavedLeftCornerOfRightEye.Z - SavedLeftEyeRightCorner.Z, 2));
            float SavedDistBetweenEyeAndNose = (float)Math.Sqrt(Math.Pow(SavedNoseTip.X - SavedLeftEyeRightCorner.X, 2) + Math.Pow(SavedNoseTip.Y - SavedLeftEyeRightCorner.Y, 2) + Math.Pow(SavedNoseTip.Z - SavedLeftEyeRightCorner.Z, 2));
            GenNoseTip *= DistBetweenEyes;
            GenLeftEyeRightCorner *= DistBetweenEyes;
            GenLeftCornerOfRightEye *= DistBetweenEyes;
            GenLeftCornerOfLeftEye *= DistBetweenEyes;
            GenRightCornerOfRightEye *= DistBetweenEyes;
            GenLeftCornerOfNose *= DistBetweenEyes;
            GenRightCornerOfNose *= DistBetweenEyes;
            GenLeftCornerOfLips *= DistBetweenEyes;
            GenRightCornerOfLips *= DistBetweenEyes;

            float DistBetweenEyeAndNoseAfterScale = (float)Math.Sqrt(Math.Pow(GenNoseTip[0] - GenLeftEyeRightCorner[0], 2) + Math.Pow(GenNoseTip[1] - GenLeftEyeRightCorner[1], 2) + Math.Pow(GenNoseTip[2] - GenLeftEyeRightCorner[2], 2));

            float muntipltBy = SavedDistBetweenEyeAndNose / DistBetweenEyeAndNoseAfterScale;

            GenNoseTip *= muntipltBy;
            GenLeftEyeRightCorner[1] *= muntipltBy;
            GenLeftEyeRightCorner[2] *= muntipltBy;
            
            GenLeftCornerOfRightEye[1] *= muntipltBy;
            GenLeftCornerOfRightEye[2] *= muntipltBy;

            GenLeftCornerOfLeftEye[1] *= muntipltBy;
            GenLeftCornerOfLeftEye[2] *= muntipltBy;

            GenRightCornerOfRightEye[1] *= muntipltBy;
            GenRightCornerOfRightEye[2] *= muntipltBy;

            GenLeftCornerOfNose[1] *= muntipltBy;
            GenLeftCornerOfNose[2] *= muntipltBy;

            GenRightCornerOfNose[1] *= muntipltBy;
            GenRightCornerOfNose[2] *= muntipltBy;

            GenLeftCornerOfLips[1] *= muntipltBy;
            GenLeftCornerOfLips[2] *= muntipltBy;

            GenRightCornerOfLips[1] *= muntipltBy;
            GenRightCornerOfLips[2] *= muntipltBy;

            Vector MeanValOfGenModel = new Vector(new double[]{(GenNoseTip[0] + GenLeftEyeRightCorner[0] + GenLeftCornerOfRightEye[0]) / 3,
                                                                                        (GenNoseTip[1] + GenLeftEyeRightCorner[1] + GenLeftCornerOfRightEye[1]) / 3,
                                                                                        (GenNoseTip[2] + GenLeftEyeRightCorner[2] + GenLeftCornerOfRightEye[2]) / 3});

            Vector MeanValOfModel = new Vector(new double[]{(SavedNoseTip.X + SavedLeftEyeRightCorner.X + SavedLeftCornerOfRightEye.X) / 3,
                                                                                        (SavedNoseTip.Y + SavedLeftEyeRightCorner.Y + SavedLeftCornerOfRightEye.Y) / 3,
                                                                                        (SavedNoseTip.Z + SavedLeftEyeRightCorner.Z + SavedLeftCornerOfRightEye.Z) / 3});

            Iridium.Numerics.LinearAlgebra.Matrix Model = new Iridium.Numerics.LinearAlgebra.Matrix(3, 3);
            Iridium.Numerics.LinearAlgebra.Matrix ModelGen = new Iridium.Numerics.LinearAlgebra.Matrix(3, 3);
            // Row Column
            Model[0, 0] = (SavedNoseTip.X - MeanValOfModel[0]);
            Model[1, 0] = (SavedNoseTip.Y - MeanValOfModel[1]);
            Model[2, 0] = (SavedNoseTip.Z - MeanValOfModel[2]);

            Model[0, 1] = (SavedLeftEyeRightCorner.X - MeanValOfModel[0]);
            Model[1, 1] = (SavedLeftEyeRightCorner.Y - MeanValOfModel[1]);
            Model[2, 1] = (SavedLeftEyeRightCorner.Z - MeanValOfModel[2]);

            Model[0, 2] = (SavedLeftCornerOfRightEye.X - MeanValOfModel[0]);
            Model[1, 2] = (SavedLeftCornerOfRightEye.Y - MeanValOfModel[1]);
            Model[2, 2] = (SavedLeftCornerOfRightEye.Z - MeanValOfModel[2]);

            ModelGen[0, 0] = (GenNoseTip[0] - MeanValOfGenModel[0]);
            ModelGen[1, 0] = (GenNoseTip[1] - MeanValOfGenModel[1]);
            ModelGen[2, 0] = (GenNoseTip[2] - MeanValOfGenModel[2]);

            ModelGen[0, 1] = (GenLeftEyeRightCorner[0] - MeanValOfGenModel[0]);
            ModelGen[1, 1] = (GenLeftEyeRightCorner[1] - MeanValOfGenModel[1]);
            ModelGen[2, 1] = (GenLeftEyeRightCorner[2] - MeanValOfGenModel[2]);

            ModelGen[0, 2] = (GenLeftCornerOfRightEye[0] - MeanValOfGenModel[0]);
            ModelGen[1, 2] = (GenLeftCornerOfRightEye[1] - MeanValOfGenModel[1]);
            ModelGen[2, 2] = (GenLeftCornerOfRightEye[2] - MeanValOfGenModel[2]);

            ModelGen.Transpose();

            Iridium.Numerics.LinearAlgebra.Matrix Covariance = Model * ModelGen;

            Iridium.Numerics.LinearAlgebra.SingularValueDecomposition SVD = new Iridium.Numerics.LinearAlgebra.SingularValueDecomposition(Covariance);
            Iridium.Numerics.LinearAlgebra.Matrix U = SVD.LeftSingularVectors;
            Iridium.Numerics.LinearAlgebra.Matrix V = SVD.RightSingularVectors;

            Iridium.Numerics.LinearAlgebra.Matrix s = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1.0);

            if (Covariance.Rank() < 2)
                throw new Exception("Cannot allign generic model (cov rank is less than 2)");

            if (Covariance.Rank() == 2) // m-1 where m is dimension space (3D)
            {
                double detU = Math.Round(U.Determinant());
                double detV = Math.Round(V.Determinant());
                double detC = Covariance.Determinant();
                if ((int)detU * (int)detV == 1)
                    s[2, 2] = 1;
                else if ((int)detU * (int)detV == -1)
                    s[2, 2] = -1;
                else
                    throw new Exception("Determinant of U and V are not in conditions");
            }
            else
            {
                if (Covariance.Determinant() < 0)
                    s[2, 2] = -1;
            }            

            V.Transpose();
            Iridium.Numerics.LinearAlgebra.Matrix Roatation = U * s * V;


            Iridium.Numerics.LinearAlgebra.Matrix MeanValOfGenModelMatrix = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1);
            MeanValOfGenModelMatrix[0, 0] = MeanValOfGenModel[0];
            MeanValOfGenModelMatrix[1, 0] = MeanValOfGenModel[1];
            MeanValOfGenModelMatrix[2, 0] = MeanValOfGenModel[2];

            Iridium.Numerics.LinearAlgebra.Matrix MeanValOfModelMatrix = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1);
            MeanValOfModelMatrix[0, 0] = MeanValOfModel[0];
            MeanValOfModelMatrix[1, 0] = MeanValOfModel[1];
            MeanValOfModelMatrix[2, 0] = MeanValOfModel[2];

            Iridium.Numerics.LinearAlgebra.Matrix Translation = MeanValOfModelMatrix - Roatation * MeanValOfGenModelMatrix;
            

            Iridium.Numerics.LinearAlgebra.Matrix q = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1);
            Iridium.Numerics.LinearAlgebra.Matrix NewQ = null;

            //------------- Code to show triangles created to check distance between them and features points -----------------------------------
            /*
            q[0, 0] = GenNoseTip.X;
            q[1, 0] = GenNoseTip.Y;
            q[2, 0] = GenNoseTip.Z;
            NewQ = Roatation * q + Translation;
            Cl3DModel.Cl3DModelPointIterator iter1 = p_Model.AddPointToModel((float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0]);
            iter1.Color = Color.Green;

            q[0, 0] = GenLeftEyeRightCorner.X;
            q[1, 0] = GenLeftEyeRightCorner.Y;
            q[2, 0] = GenLeftEyeRightCorner.Z;
            NewQ = Roatation * q + Translation;
            Cl3DModel.Cl3DModelPointIterator iter2 = p_Model.AddPointToModel((float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0]);
            iter2.Color = Color.White;

            q[0, 0] = GenLeftCornerOfRightEye.X;
            q[1, 0] = GenLeftCornerOfRightEye.Y;
            q[2, 0] = GenLeftCornerOfRightEye.Z;
            NewQ = Roatation * q + Translation;
            Cl3DModel.Cl3DModelPointIterator iter3 = p_Model.AddPointToModel((float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0]);
            iter3.Color = Color.Orange;

            iter1.AddNeighbor(iter2);
            iter1.AddNeighbor(iter3);
            iter2.AddNeighbor(iter3);
            */
            //---------------------------------------------------------------------------
             
            Cl3DModel.Cl3DModelPointIterator iter = null;
            float MinDistance = 0;
            Cl3DModel.Cl3DModelPointIterator MinIter = null;

            //------------------ Code to correct nose tip and eyes position -----------------------------------
            /*
            GenNoseTip.X = (float)NewQ[0, 0];
            GenNoseTip.Y = (float)NewQ[1, 0];
            GenNoseTip.Z = (float)NewQ[2, 0];

            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenNoseTip.X - iter.X, 2) + Math.Pow(GenNoseTip.Y - iter.Y, 2) + Math.Pow(GenNoseTip.Z - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenNoseTip.X - iter.X, 2) + Math.Pow(GenNoseTip.Y - iter.Y, 2) + Math.Pow(GenNoseTip.Z - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, MinIter);
            
            q[0, 0] = GenLeftEyeRightCorner.X;
            q[1, 0] = GenLeftEyeRightCorner.Y;
            q[2, 0] = GenLeftEyeRightCorner.Z;
            NewQ = Roatation * q + Translation;
            GenLeftEyeRightCorner.X = (float)NewQ[0, 0];
            GenLeftEyeRightCorner.Y = (float)NewQ[1, 0];
            GenLeftEyeRightCorner.Z = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenLeftEyeRightCorner.X - iter.X, 2) + Math.Pow(GenLeftEyeRightCorner.Y - iter.Y, 2) + Math.Pow(GenLeftEyeRightCorner.Z - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenLeftEyeRightCorner.X - iter.X, 2) + Math.Pow(GenLeftEyeRightCorner.Y - iter.Y, 2) + Math.Pow(GenLeftEyeRightCorner.Z - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner, MinIter);

            q[0, 0] = GenLeftCornerOfRightEye.X;
            q[1, 0] = GenLeftCornerOfRightEye.Y;
            q[2, 0] = GenLeftCornerOfRightEye.Z;
            NewQ = Roatation * q + Translation;
            GenLeftCornerOfRightEye.X = (float)NewQ[0, 0];
            GenLeftCornerOfRightEye.Y = (float)NewQ[1, 0];
            GenLeftCornerOfRightEye.Z = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfRightEye.X - iter.X, 2) + Math.Pow(GenLeftCornerOfRightEye.Y - iter.Y, 2) + Math.Pow(GenLeftCornerOfRightEye.Z - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfRightEye.X - iter.X, 2) + Math.Pow(GenLeftCornerOfRightEye.Y - iter.Y, 2) + Math.Pow(GenLeftCornerOfRightEye.Z - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner, MinIter);
            */
            //---------------------------------------------------------------------------------------------------------

            q[0, 0] = GenLeftCornerOfLeftEye[0];
            q[1, 0] = GenLeftCornerOfLeftEye[1];
            q[2, 0] = GenLeftCornerOfLeftEye[2];
            NewQ = Roatation * q + Translation;
            GenLeftCornerOfLeftEye[0] = (float)NewQ[0, 0];
            GenLeftCornerOfLeftEye[1] = (float)NewQ[1, 0];
            GenLeftCornerOfLeftEye[2] = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfLeftEye[0] - iter.X, 2) + Math.Pow(GenLeftCornerOfLeftEye[1] - iter.Y, 2) + Math.Pow(GenLeftCornerOfLeftEye[2] - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfLeftEye[0] - iter.X, 2) + Math.Pow(GenLeftCornerOfLeftEye[1] - iter.Y, 2) + Math.Pow(GenLeftCornerOfLeftEye[2] - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            if(MinDistance < m_MinDistanceToAddPoint)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeLeftCorner, MinIter);

            q[0, 0] = GenRightCornerOfRightEye[0];
            q[1, 0] = GenRightCornerOfRightEye[1];
            q[2, 0] = GenRightCornerOfRightEye[2];
            NewQ = Roatation * q + Translation;
            GenRightCornerOfRightEye[0] = (float)NewQ[0, 0];
            GenRightCornerOfRightEye[1] = (float)NewQ[1, 0];
            GenRightCornerOfRightEye[2] = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenRightCornerOfRightEye[0] - iter.X, 2) + Math.Pow(GenRightCornerOfRightEye[1] - iter.Y, 2) + Math.Pow(GenRightCornerOfRightEye[2] - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenRightCornerOfRightEye[0] - iter.X, 2) + Math.Pow(GenRightCornerOfRightEye[1] - iter.Y, 2) + Math.Pow(GenRightCornerOfRightEye[2] - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            if (MinDistance < m_MinDistanceToAddPoint)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeRightCorner, MinIter);


            q[0, 0] = GenLeftCornerOfNose[0];
            q[1, 0] = GenLeftCornerOfNose[1];
            q[2, 0] = GenLeftCornerOfNose[2];
            NewQ = Roatation * q + Translation;
            GenLeftCornerOfNose[0] = (float)NewQ[0, 0];
            GenLeftCornerOfNose[1] = (float)NewQ[1, 0];
            GenLeftCornerOfNose[2] = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfNose[0] - iter.X, 2) + Math.Pow(GenLeftCornerOfNose[1] - iter.Y, 2) + Math.Pow(GenLeftCornerOfNose[2] - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfNose[0] - iter.X, 2) + Math.Pow(GenLeftCornerOfNose[1] - iter.Y, 2) + Math.Pow(GenLeftCornerOfNose[2] - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            if (MinDistance < m_MinDistanceToAddPoint)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfNose, MinIter);


            q[0, 0] = GenRightCornerOfNose[0];
            q[1, 0] = GenRightCornerOfNose[1];
            q[2, 0] = GenRightCornerOfNose[2];
            NewQ = Roatation * q + Translation;
            GenRightCornerOfNose[0] = (float)NewQ[0, 0];
            GenRightCornerOfNose[1] = (float)NewQ[1, 0];
            GenRightCornerOfNose[2] = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenRightCornerOfNose[0] - iter.X, 2) + Math.Pow(GenRightCornerOfNose[1] - iter.Y, 2) + Math.Pow(GenRightCornerOfNose[2] - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenRightCornerOfNose[0] - iter.X, 2) + Math.Pow(GenRightCornerOfNose[1] - iter.Y, 2) + Math.Pow(GenRightCornerOfNose[2] - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            if (MinDistance < m_MinDistanceToAddPoint)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfNose, MinIter);

            q[0, 0] = GenLeftCornerOfLips[0];
            q[1, 0] = GenLeftCornerOfLips[1];
            q[2, 0] = GenLeftCornerOfLips[2];
            NewQ = Roatation * q + Translation;
            GenLeftCornerOfLips[0] = (float)NewQ[0, 0];
            GenLeftCornerOfLips[1] = (float)NewQ[1, 0];
            GenLeftCornerOfLips[2] = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfLips[0] - iter.X, 2) + Math.Pow(GenLeftCornerOfLips[1] - iter.Y, 2) + Math.Pow(GenLeftCornerOfLips[2] - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenLeftCornerOfLips[0] - iter.X, 2) + Math.Pow(GenLeftCornerOfLips[1] - iter.Y, 2) + Math.Pow(GenLeftCornerOfLips[2] - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            if (MinDistance < m_MinDistanceToAddPoint)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips, MinIter);

            q[0, 0] = GenRightCornerOfLips[0];
            q[1, 0] = GenRightCornerOfLips[1];
            q[2, 0] = GenRightCornerOfLips[2];
            NewQ = Roatation * q + Translation;
            GenRightCornerOfLips[0] = (float)NewQ[0, 0];
            GenRightCornerOfLips[1] = (float)NewQ[1, 0];
            GenRightCornerOfLips[2] = (float)NewQ[2, 0];
            iter = p_Model.GetIterator();
            MinDistance = (float)Math.Sqrt(Math.Pow(GenRightCornerOfLips[0] - iter.X, 2) + Math.Pow(GenRightCornerOfLips[1] - iter.Y, 2) + Math.Pow(GenRightCornerOfLips[2] - iter.Z, 2));
            MinIter = iter;
            do
            {
                float NewDistance = (float)Math.Sqrt(Math.Pow(GenRightCornerOfLips[0] - iter.X, 2) + Math.Pow(GenRightCornerOfLips[1] - iter.Y, 2) + Math.Pow(GenRightCornerOfLips[2] - iter.Z, 2));
                if (NewDistance < MinDistance)
                {
                    MinDistance = NewDistance;
                    MinIter = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
            if (MinDistance < m_MinDistanceToAddPoint)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips, MinIter);



        }
    }
}
