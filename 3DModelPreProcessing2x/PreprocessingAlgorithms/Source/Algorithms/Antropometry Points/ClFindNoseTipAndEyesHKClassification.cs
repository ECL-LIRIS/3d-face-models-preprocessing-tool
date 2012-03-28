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
*   @file       ClFindNoseTipAndEyesHKClassification.cs
*   @brief      Algorithm to find nose tip and eyes based on HK-Classification
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClFindNoseTipAndEyesHKClassification : ClBaseFaceAlgorithm
    {
        public class ClCompareCount : IComparer<List<Cl3DModel.Cl3DModelPointIterator>>
        {
            public ClCompareCount()
            { }

            public virtual int Compare(List<Cl3DModel.Cl3DModelPointIterator> x, List<Cl3DModel.Cl3DModelPointIterator> y)
            {
                return y.Count - x.Count;
            }
        }

        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClFindNoseTipAndEyesHKClassification();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Anthropometry Points\Nose Tip, Eyes (Curvature analysis)";

        public ClFindNoseTipAndEyesHKClassification() : base(ALGORITHM_NAME) { }

        private double m_NoseThresholdK = 0.002;
        private double m_EyesThresholdK = 0.00005;
        private float m_fDistanceMinValueFromGenModel = 95;
        private int m_iMaxCountOfEyeRegions = 12; // mostely 2x eyes and 2x ears and some cloaths regions
        private int m_iMaxCountOfNoseRegions = 3; // mostly nose, chin, lips
        private int m_iNeighborhoodSize = 25;
        private string NameOfNoseTip = "AutomaticNoseTip";
        private string NameLeftEyeRightCorner = "AutomaticLeftEyeRightCorner";
        private string NameRightEyeLeftCorner = "AutomaticRightEyeLeftCorner";

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Nose Threshold K"))
            {
                m_NoseThresholdK = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Eyes Threshold K"))
            {
                m_EyesThresholdK = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Min Distance From Gen Model"))
            {
                m_fDistanceMinValueFromGenModel = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Max Count Of Eye Regions"))
            {
                m_iMaxCountOfEyeRegions = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Max Count Of Nose Regions"))
            {
                m_iMaxCountOfNoseRegions = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Used Neighborhood"))
            {
                m_iNeighborhoodSize = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Name NoseTip"))
            {
                NameOfNoseTip = p_sValue;
            }
            else if (p_sProperity.Equals("Name LeftEyeRightCorner"))
            {
                NameLeftEyeRightCorner = p_sValue;
            }
            else if (p_sProperity.Equals("Name RightEyeLeftCorner"))
            {
                NameRightEyeLeftCorner = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Used Neighborhood", m_iNeighborhoodSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            list.Add(new KeyValuePair<string, string>("Nose Threshold K", m_NoseThresholdK.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Eyes Threshold K", m_EyesThresholdK.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            list.Add(new KeyValuePair<string, string>("Min Distance From Gen Model", m_fDistanceMinValueFromGenModel.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Max Count Of Eye Regions", m_iMaxCountOfEyeRegions.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Max Count Of Nose Regions", m_iMaxCountOfNoseRegions.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            list.Add(new KeyValuePair<string, string>("Name NoseTip", NameOfNoseTip.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Name LeftEyeRightCorner", NameLeftEyeRightCorner.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Name RightEyeLeftCorner", NameRightEyeLeftCorner.ToString(System.Globalization.CultureInfo.InvariantCulture)));            
            
            return list;
        }

        private void DivideToTheRegions(List<Cl3DModel.Cl3DModelPointIterator> ListOfPoints, out List<List<Cl3DModel.Cl3DModelPointIterator>> Regions)
        {
            Regions = new List<List<Cl3DModel.Cl3DModelPointIterator>>();
            foreach (Cl3DModel.Cl3DModelPointIterator point in ListOfPoints)
                point.AlreadyVisited = true;

            foreach (Cl3DModel.Cl3DModelPointIterator point in ListOfPoints)
            {
                if (!point.AlreadyVisited)
                    continue;

                List<Cl3DModel.Cl3DModelPointIterator> newRegion = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> listToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                listToCheck.Add(point);

                do
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator pointToCheck in listToCheck)
                    {
                        if (pointToCheck.AlreadyVisited)
                        {
                            pointToCheck.AlreadyVisited = false;
                            newRegion.Add(pointToCheck);
                            foreach (Cl3DModel.Cl3DModelPointIterator Neighbor in pointToCheck.GetListOfNeighbors())
                            {
                                if (Neighbor.AlreadyVisited)
                                    NewListToCheck.Add(Neighbor);
                            }
                        }
                    }
                    listToCheck.Clear();
                    foreach (Cl3DModel.Cl3DModelPointIterator newPoint in NewListToCheck)
                        listToCheck.Add(newPoint);
                    NewListToCheck.Clear();

                } while (listToCheck.Count != 0);
                Regions.Add(newRegion);
            }
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            if (!iter.IsValid())
                return;

            List<Cl3DModel.Cl3DModelPointIterator> ListOfNosePoints = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<Cl3DModel.Cl3DModelPointIterator> ListOfEyesPoints = new List<Cl3DModel.Cl3DModelPointIterator>();
            string gausString  = "Gaussian_" + m_iNeighborhoodSize.ToString();
            string meanString  = "Mean_" + m_iNeighborhoodSize.ToString();
            do
            {
                double H = 0;
                double K = 0;

                if (!iter.GetSpecificValue(gausString, out K))
                    continue;
                if (!iter.GetSpecificValue(meanString, out H))
                    continue;

                if (H < 0 && K > m_NoseThresholdK) //  Nose
                {
                    ListOfNosePoints.Add(iter.CopyIterator());
                }
                else if (H > 0 && K > m_EyesThresholdK) //  Eyes
                {
                    ListOfEyesPoints.Add(iter.CopyIterator());
                }
            }
            while (iter.MoveToNext());

            List<List<Cl3DModel.Cl3DModelPointIterator>> NoseRegions = null;
            List<List<Cl3DModel.Cl3DModelPointIterator>> EyesRegions = null;

            // NOSE
            DivideToTheRegions(ListOfNosePoints, out NoseRegions);
            
            // EYES
            DivideToTheRegions(ListOfEyesPoints, out EyesRegions);
            
            // COLOR IT
            /*for (int i = 0; i < NoseRegions.Count; i++)
            {
                foreach (Cl3DModel.Cl3DModelPointIterator point in NoseRegions[i])
                    point.Color = ClTools.GetColorRGB((float)i / (NoseRegions.Count + EyesRegions.Count), 1);
            }
            for (int i = 0; i < EyesRegions.Count; i++)
            {
                foreach (Cl3DModel.Cl3DModelPointIterator point in EyesRegions[i])
                    point.Color = ClTools.GetColorRGB((float)(i + NoseRegions.Count) / (EyesRegions.Count + NoseRegions.Count), 1);
            }
            */

            // -----------------  NOSE
            // remove smallest regions leave only 3 biggest one
            List<Cl3DModel.Cl3DModelPointIterator> NoseTipsPoints = new List<Cl3DModel.Cl3DModelPointIterator>();
            if (NoseRegions.Count > m_iMaxCountOfNoseRegions)
            {
                NoseRegions.Sort(new ClCompareCount());
                for (int i = NoseRegions.Count - 1; i >= m_iMaxCountOfNoseRegions; i--)
                {
                    NoseRegions.RemoveAt(i);
                }
            }
            //----------- NOSE searching for one point in each region
            for (int i = 0; i < NoseRegions.Count; i++)
            {
                double MaxKVal = 0;
                Cl3DModel.Cl3DModelPointIterator MaxPoint = null;
                foreach (Cl3DModel.Cl3DModelPointIterator point in NoseRegions[i])
                {
                    double K;
                    if (!point.GetSpecificValue(gausString, out K))
                        continue;

                    if(MaxPoint == null)
                    {
                        MaxPoint = point;
                        MaxKVal = K;
                        continue;
                    }

                    if (MaxKVal < K)
                    {
                        MaxKVal = K;
                        MaxPoint = point;
                    }
                }
                NoseTipsPoints.Add(MaxPoint);
            }

            // ---------------  EYES
            // remove smallest regions leave only 5 biggest one
            List<Cl3DModel.Cl3DModelPointIterator> EyesPoints = new List<Cl3DModel.Cl3DModelPointIterator>();
            if (EyesRegions.Count > m_iMaxCountOfEyeRegions)
            {
                EyesRegions.Sort(new ClCompareCount());
                for (int i = EyesRegions.Count - 1; i >= m_iMaxCountOfEyeRegions; i--)
                {
                    EyesRegions.RemoveAt(i);
                }
            }
            //--------------------------- EYES searching for one point in each region
            for (int i = 0; i < EyesRegions.Count; i++)
            {
                double MaxKval = 0;
                Cl3DModel.Cl3DModelPointIterator MinPoint = null;

                foreach (Cl3DModel.Cl3DModelPointIterator point in EyesRegions[i])
                {
                    double val;
                    if (!point.GetSpecificValue(gausString, out val))
                        continue;

                    if (MinPoint == null)
                    {
                        MinPoint = point;
                        MaxKval = val;
                        continue;
                    }

                    if (MaxKval < val)
                    {
                        MaxKval = val;
                        MinPoint = point;
                    }
                }
                EyesPoints.Add(MinPoint);
            }
            //------------- Looking for three face points ------------------------------
            Iridium.Numerics.LinearAlgebra.Matrix Model = new Iridium.Numerics.LinearAlgebra.Matrix(3, 3);
            Iridium.Numerics.LinearAlgebra.Matrix ModelGen = new Iridium.Numerics.LinearAlgebra.Matrix(3, 3);

            // Generic model
            Vector GenericNoseTip = new Vector(new double[]{0, 0, 0});
            Vector GenericLeftEye = new Vector(new double[]{-21f, 40.5f, -41.5f});
            Vector GenericRightEye = new Vector(new double[]{21f, 40.5f, -41.5f});

            Vector GenLeftCornerOfLeftEye = new Vector(new double[]{-44f, 40f, -44f});
            Vector GenRightCornerOfRightEye = new Vector(new double[] { 44f, 40f, -44f });

            Vector GenLeftCornerOfNose = new Vector(new double[]{-20f, -2f, -31f});
            Vector GenRightCornerOfNose = new Vector(new double[] { 20f, -2f, -31f });

            Vector GenLeftCornerOfLips = new Vector(new double[]{-28f, -32f, -34f});
            Vector GenRightCornerOfLips = new Vector(new double[]{28f, -32f, -34f});


            Vector MeanGenericModel = new Vector(new double[]{(GenericNoseTip[0] + GenericLeftEye[0] + GenericRightEye[0]) / 3,
                                                                                        (GenericNoseTip[1] + GenericLeftEye[1] + GenericRightEye[1]) / 3,
                                                                                        (GenericNoseTip[2] + GenericLeftEye[2] + GenericRightEye[2]) / 3});
            // Centered generic model (generic model - mean)
            ModelGen[0, 0] = (GenericNoseTip[0] - MeanGenericModel[0]);
            ModelGen[1, 0] = (GenericNoseTip[1] - MeanGenericModel[1]);
            ModelGen[2, 0] = (GenericNoseTip[2] - MeanGenericModel[2]);

            ModelGen[0, 1] = (GenericLeftEye[0] - MeanGenericModel[0]);
            ModelGen[1, 1] = (GenericLeftEye[1] - MeanGenericModel[1]);
            ModelGen[2, 1] = (GenericLeftEye[2] - MeanGenericModel[2]);

            ModelGen[0, 2] = (GenericRightEye[0] - MeanGenericModel[0]);
            ModelGen[1, 2] = (GenericRightEye[1] - MeanGenericModel[1]);
            ModelGen[2, 2] = (GenericRightEye[2] - MeanGenericModel[2]);
            ModelGen.Transpose();

            List<KeyValuePair<float,List<Cl3DModel.Cl3DModelPointIterator>>> deltaOfDistanceBetweenModelAndPoints = new List<KeyValuePair<float,List<Cl3DModel.Cl3DModelPointIterator>>>();

            for(int iEye = 0 ; iEye < EyesPoints.Count-1 ; iEye++)
            {
                for (int jEye = iEye + 1; jEye < EyesPoints.Count; jEye++)
                {

                    for (int iNose = 0; iNose < NoseTipsPoints.Count; iNose++)
                    {   
                        Cl3DModel.Cl3DModelPointIterator eyeLeft = EyesPoints[iEye];
                        Cl3DModel.Cl3DModelPointIterator eyeRight = EyesPoints[jEye];
                        Cl3DModel.Cl3DModelPointIterator noseTip = NoseTipsPoints[iNose];

                        if (eyeLeft.Y < noseTip.Y || eyeRight.Y < noseTip.Y)
                            continue;

                        for (int c = 0; c < 2; c++)
                        {
                            if (c == 1)
                            {
                                eyeLeft = EyesPoints[jEye]; // zmiana oczu nie wiemy ktore punkty to oczy prawdziwe sa
                                eyeRight = EyesPoints[iEye];
                            }

                            float MeanValX = (noseTip.X + eyeLeft.X + eyeRight.X) / 3.0f;
                            float MeanValY = (noseTip.Y + eyeLeft.Y + eyeRight.Y) / 3.0f;
                            float MeanValZ = (noseTip.Z + eyeLeft.Z + eyeRight.Z) / 3.0f;

                            Model[0, 0] = (noseTip.X - MeanValX);
                            Model[1, 0] = (noseTip.Y - MeanValY);
                            Model[2, 0] = (noseTip.Z - MeanValZ);

                            Model[0, 1] = (eyeLeft.X - MeanValX);
                            Model[1, 1] = (eyeLeft.Y - MeanValY);
                            Model[2, 1] = (eyeLeft.Z - MeanValZ);

                            Model[0, 2] = (eyeRight.X - MeanValX);
                            Model[1, 2] = (eyeRight.Y - MeanValY);
                            Model[2, 2] = (eyeRight.Z - MeanValZ);


                            //----------------- Finding best rotation and translation SVD algorithm

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
                            MeanValOfGenModelMatrix[0, 0] = MeanGenericModel[0];
                            MeanValOfGenModelMatrix[1, 0] = MeanGenericModel[1];
                            MeanValOfGenModelMatrix[2, 0] = MeanGenericModel[2];

                            Iridium.Numerics.LinearAlgebra.Matrix MeanValOfModelMatrix = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1);
                            MeanValOfModelMatrix[0, 0] = MeanValX;
                            MeanValOfModelMatrix[1, 0] = MeanValY;
                            MeanValOfModelMatrix[2, 0] = MeanValZ;

                            Iridium.Numerics.LinearAlgebra.Matrix Translation = MeanValOfModelMatrix - Roatation * MeanValOfGenModelMatrix;

                            //--------------------------- End now we have translation and rotation

                            float GenericModelDistance = 0; ;

                            Iridium.Numerics.LinearAlgebra.Matrix q = new Iridium.Numerics.LinearAlgebra.Matrix(3, 1);
                            q[0, 0] = GenericNoseTip[0];
                            q[1, 0] = GenericNoseTip[1];
                            q[2, 0] = GenericNoseTip[2];
                            Iridium.Numerics.LinearAlgebra.Matrix NewQ = Roatation * q + Translation;
                            GenericModelDistance += (float)Math.Sqrt(Math.Pow(noseTip.X - NewQ[0, 0], 2) + Math.Pow(noseTip.Y - NewQ[1, 0], 2) + Math.Pow(noseTip.Z - NewQ[2, 0], 2));
                            //---------
                            //    Cl3DModel.Cl3DModelPointIterator it1 = p_Model.AddPointToModel((float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0]);
                            //it1.AddNeighbor(noseTip);
                            //    it1.Color = Color.Green;
                            //---------
                            q[0, 0] = GenericLeftEye[0];
                            q[1, 0] = GenericLeftEye[1];
                            q[2, 0] = GenericLeftEye[2];
                            NewQ = Roatation * q + Translation;
                            GenericModelDistance += (float)Math.Sqrt(Math.Pow(eyeLeft.X - NewQ[0, 0], 2) + Math.Pow(eyeLeft.Y - NewQ[1, 0], 2) + Math.Pow(eyeLeft.Z - NewQ[2, 0], 2));
                            //---------                        
                            //    Cl3DModel.Cl3DModelPointIterator it2 = p_Model.AddPointToModel((float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0]);
                            //it2.AddNeighbor(eyeLeft);
                            //    it2.Color = Color.White;
                            //---------

                            q[0, 0] = GenericRightEye[0];
                            q[1, 0] = GenericRightEye[1];
                            q[2, 0] = GenericRightEye[2];
                            NewQ = Roatation * q + Translation;
                            GenericModelDistance += (float)Math.Sqrt(Math.Pow(eyeRight.X - NewQ[0, 0], 2) + Math.Pow(eyeRight.Y - NewQ[1, 0], 2) + Math.Pow(eyeRight.Z - NewQ[2, 0], 2));
                            //---------    
                            //    Cl3DModel.Cl3DModelPointIterator it3 = p_Model.AddPointToModel((float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0]);
                            //it3.AddNeighbor(eyeRight);
                            //    it3.Color = Color.Orange;

                            //    it1.AddNeighbor(it2);
                            //    it1.AddNeighbor(it3);
                            //    it2.AddNeighbor(it3);
                            //---------

                            // ---------- Rest of generic model to find closest points and count distance;
                            q[0, 0] = GenLeftCornerOfLeftEye[0];
                            q[1, 0] = GenLeftCornerOfLeftEye[1];
                            q[2, 0] = GenLeftCornerOfLeftEye[2];
                            NewQ = Roatation * q + Translation;
                            Cl3DModel.Cl3DModelPointIterator closestPoint = null;
                            float MinDistance;
                            ClTools.FindClosestPointInModel(p_Model, (float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0], out closestPoint, out MinDistance);
                            GenericModelDistance += MinDistance;

                            q[0, 0] = GenRightCornerOfRightEye[0];
                            q[1, 0] = GenRightCornerOfRightEye[1];
                            q[2, 0] = GenRightCornerOfRightEye[2];
                            NewQ = Roatation * q + Translation;
                            ClTools.FindClosestPointInModel(p_Model, (float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0], out closestPoint, out MinDistance);
                            GenericModelDistance += MinDistance;

                            q[0, 0] = GenLeftCornerOfNose[0];
                            q[1, 0] = GenLeftCornerOfNose[1];
                            q[2, 0] = GenLeftCornerOfNose[2];
                            NewQ = Roatation * q + Translation;
                            ClTools.FindClosestPointInModel(p_Model, (float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0], out closestPoint, out MinDistance);
                            GenericModelDistance += MinDistance;

                            q[0, 0] = GenRightCornerOfNose[0];
                            q[1, 0] = GenRightCornerOfNose[1];
                            q[2, 0] = GenRightCornerOfNose[2];
                            NewQ = Roatation * q + Translation;
                            ClTools.FindClosestPointInModel(p_Model, (float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0], out closestPoint, out MinDistance);
                            GenericModelDistance += MinDistance;

                            q[0, 0] = GenLeftCornerOfLips[0];
                            q[1, 0] = GenLeftCornerOfLips[1];
                            q[2, 0] = GenLeftCornerOfLips[2];
                            NewQ = Roatation * q + Translation;
                            ClTools.FindClosestPointInModel(p_Model, (float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0], out closestPoint, out MinDistance);
                            GenericModelDistance += MinDistance;

                            q[0, 0] = GenRightCornerOfLips[0];
                            q[1, 0] = GenRightCornerOfLips[1];
                            q[2, 0] = GenRightCornerOfLips[2];
                            NewQ = Roatation * q + Translation;
                            ClTools.FindClosestPointInModel(p_Model, (float)NewQ[0, 0], (float)NewQ[1, 0], (float)NewQ[2, 0], out closestPoint, out MinDistance);
                            GenericModelDistance += MinDistance;
                            // ---------- 


                            ClInformationSender.SendInformation("One of distances between generic model and face before thresholding: " + GenericModelDistance.ToString(System.Globalization.CultureInfo.InvariantCulture), ClInformationSender.eInformationType.eDebugText);

                            if (GenericModelDistance > m_fDistanceMinValueFromGenModel)
                                continue;

                            // Check if between eyes and nose there is other region (it should be not)
                            /*bool isBetweenNoseRegion = false;
                            foreach (Cl3DModel.Cl3DModelPointIterator noseTmp in NoseTipsPoints)
                            {
                                if (noseTmp.PointID == noseTip.PointID)
                                    continue;

                                if (noseTmp.X > eyeLeft.X && noseTmp.X < eyeRight.X && noseTmp.Y > noseTip.Y && (noseTip.Y < eyeLeft.Y || noseTip.Y < eyeRight.Y))
                                    isBetweenNoseRegion = true;
                            }
                            if (isBetweenNoseRegion)
                                continue;
                            */
                            List<Cl3DModel.Cl3DModelPointIterator> list = new List<Cl3DModel.Cl3DModelPointIterator>();
                            list.Add(noseTip);
                            list.Add(eyeLeft);
                            list.Add(eyeRight);

                            KeyValuePair<float, List<Cl3DModel.Cl3DModelPointIterator>> KeyVal = new KeyValuePair<float, List<Cl3DModel.Cl3DModelPointIterator>>(GenericModelDistance, list);

                            deltaOfDistanceBetweenModelAndPoints.Add(KeyVal);
                        }
                    }
                }
            }
                
            if(deltaOfDistanceBetweenModelAndPoints.Count == 0)
                throw new Exception("Cannot find nose and eyes points");

            float min = deltaOfDistanceBetweenModelAndPoints[0].Key;
            int no = 0;
            for(int i = 1; i< deltaOfDistanceBetweenModelAndPoints.Count; i++)
            {
                if (deltaOfDistanceBetweenModelAndPoints[i].Key < min)
                {
                    min = deltaOfDistanceBetweenModelAndPoints[i].Key;
                    no = i;
                }
            }

            ClInformationSender.SendInformation("Correct MIN distance between generic model and face: " + min.ToString(System.Globalization.CultureInfo.InvariantCulture), ClInformationSender.eInformationType.eDebugText); 

            p_Model.AddSpecificPoint(NameOfNoseTip, deltaOfDistanceBetweenModelAndPoints[no].Value[0]);
            p_Model.AddSpecificPoint(NameLeftEyeRightCorner, deltaOfDistanceBetweenModelAndPoints[no].Value[1]);
            p_Model.AddSpecificPoint(NameRightEyeLeftCorner, deltaOfDistanceBetweenModelAndPoints[no].Value[2]);

        }
    }
}
