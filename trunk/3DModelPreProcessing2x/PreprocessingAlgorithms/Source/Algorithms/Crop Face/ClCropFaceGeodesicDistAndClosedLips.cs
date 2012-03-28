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
*   @file       ClCropFaceGeodesicDistAndClosedLips.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCropFaceGeodesicDistAndClosedLips : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCropFaceGeodesicDistAndClosedLips();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Crop Face Avoid Holes(Geodesic)";

        public ClCropFaceGeodesicDistAndClosedLips() : base(ALGORITHM_NAME) { }

        public ClCropFaceGeodesicDistAndClosedLips(float p_fSphereRadious)
            : base(ALGORITHM_NAME)
        {
            m_fDistence = p_fSphereRadious;
        }

        private float m_fDistence = 100f;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Geodesic Distance"))
            {
                m_fDistence = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Geodesic Distance", m_fDistence.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            // Looking for a hole and connecting points ---- Later should be removed
            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            Cl3DModel.Cl3DModelPointIterator LeftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);

            float distanceBetweenEyes = LeftEye - RightEye;
            float ossfest = distanceBetweenEyes * 0.4f; //20% of distance between eyes will be taken as a stripe down to search for the mouth hole
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            Dictionary<uint, Cl3DModel.Cl3DModelPointIterator> pointsLowerBoundary = new Dictionary<uint, Cl3DModel.Cl3DModelPointIterator>();
            do
            {
                if (iter.X > LeftEye.X + ossfest && iter.X < RightEye.X - ossfest && iter.Y < NoseTip.Y - 20 && iter.Y > NoseTip.Y - 40)
                {
                   // iter.Color = Color.LightGreen;
                    if (iter.GetListOfNeighbors().Count != 8)
                    {
                       // iter.Color = Color.LightBlue;
                        pointsLowerBoundary.Add(iter.PointID, iter.CopyIterator());
                    }
                }

            } while (iter.MoveToNext());

            List<List<Cl3DModel.Cl3DModelPointIterator>> ListOfBoundarie = new List<List<Cl3DModel.Cl3DModelPointIterator>>();

            while (pointsLowerBoundary.Count != 0)
            {
                Cl3DModel.Cl3DModelPointIterator test = new List<Cl3DModel.Cl3DModelPointIterator>(pointsLowerBoundary.Values)[0];
                pointsLowerBoundary.Remove(test.PointID);

                bool toRemove = false;
                List<Cl3DModel.Cl3DModelPointIterator> CurrentList = new List<Cl3DModel.Cl3DModelPointIterator>();
                ListOfBoundarie.Add(CurrentList);

                List<Cl3DModel.Cl3DModelPointIterator> ToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                ToCheck.Add(test);
                test.AlreadyVisited = true;
                do
                {
                    List<Cl3DModel.Cl3DModelPointIterator> NewToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                    foreach (Cl3DModel.Cl3DModelPointIterator pt in ToCheck)
                    {
                        CurrentList.Add(pt.CopyIterator());
                        if(pt.Y > NoseTip.Y)
                            toRemove = true;

                        foreach (Cl3DModel.Cl3DModelPointIterator ptNb in pt.GetListOfNeighbors())
                        {
                            if (ptNb.GetListOfNeighbors().Count != 8 && ptNb.AlreadyVisited == false)
                            {
                                ptNb.AlreadyVisited = true;
                                NewToCheck.Add(ptNb.CopyIterator());
                                pointsLowerBoundary.Remove(ptNb.PointID);
                            }
                        }
                    }
                    ToCheck = NewToCheck;
                    NewToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                } while (ToCheck.Count != 0);
                if (toRemove)
                    ListOfBoundarie.Remove(CurrentList);
            }

            List<Cl3DModel.Cl3DModelPointIterator> BiggestBoundary = null;
            int SizeOfTheBiggest = int.MinValue;
            for (int i = 0; i < ListOfBoundarie.Count; i++)
            {
                if(ListOfBoundarie[i].Count > SizeOfTheBiggest)
                {
                    SizeOfTheBiggest = ListOfBoundarie[i].Count;
                    BiggestBoundary = ListOfBoundarie[i];
                }
                
            }

            if (BiggestBoundary == null)
            {
                ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(NoseTip, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString());
                iter = p_Model.GetIterator();
                while (iter.IsValid())
                {
                    if(!iter.IsSpecificValueCalculated(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip))
                    {
                        iter = p_Model.RemovePointFromModel(iter);
                    }
                    else
                    {
                        double distance = iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip);
                        if (distance > m_fDistence)
                            iter = p_Model.RemovePointFromModel(iter);
                        else
                            iter.MoveToNext();
                    }
                }
                return;
            }

            Cl3DModel.Cl3DModelPointIterator MaxLeft = BiggestBoundary[0];
            Cl3DModel.Cl3DModelPointIterator MaxRight = BiggestBoundary[0];
            foreach (Cl3DModel.Cl3DModelPointIterator pt in BiggestBoundary)
            {
                if (pt.X < MaxLeft.X)
                    MaxLeft = pt;
                if (pt.X > MaxRight.X)
                    MaxRight = pt;
            }

            int size = (int)Math.Abs(MaxLeft.X - MaxRight.X)+1;
            List<Cl3DModel.Cl3DModelPointIterator>[] histogram = new List<Cl3DModel.Cl3DModelPointIterator>[size];

            foreach (Cl3DModel.Cl3DModelPointIterator pt in BiggestBoundary)
            {
                int pos = (int)(pt.X - MaxLeft.X);
                if (histogram[pos] == null)
                    histogram[pos] = new List<Cl3DModel.Cl3DModelPointIterator>();

                histogram[pos].Add(pt);
            }

            Dictionary<uint, uint> movingPoints = new Dictionary<uint, uint>();
            for (int i = 0; i < size; i++)
            {
                if (histogram[i] != null && histogram[i].Count != 0)
                {
                    //Color cl = ClTools.GetColorRGB(((float)i) / size, 1.0f);
                    Cl3DModel.Cl3DModelPointIterator UpperPoint = histogram[i][0];
                    Cl3DModel.Cl3DModelPointIterator LowerPoint = histogram[i][0];
                    foreach (Cl3DModel.Cl3DModelPointIterator pts in histogram[i])
                    {
                      //  pts.Color = cl;
                        if (UpperPoint.Y < pts.Y)
                            UpperPoint = pts;
                        if (LowerPoint.Y > pts.Y)
                            LowerPoint = pts;
                    }
                    //UpperPoint from this one
                    if (UpperPoint.PointID != LowerPoint.PointID)
                    {
                        float distance = Math.Min(LowerPoint - MaxLeft, LowerPoint - MaxRight);
                        List<Cl3DModel.Cl3DModelPointIterator> neighborhood = null;
                        ClTools.GetNeighborhoodWithGeodesicDistance(out neighborhood, LowerPoint, distance);
                        Cl3DModel.Cl3DModelPointIterator ClosestPoint = LowerPoint;
                        float MinDistance = LowerPoint - UpperPoint;
                        foreach (Cl3DModel.Cl3DModelPointIterator ptNeighb in neighborhood)
                        {
                           // ptNeighb.Color = Color.Pink;
                            float newDistance = ptNeighb - UpperPoint;
                            if (newDistance < MinDistance)
                            {
                                MinDistance = newDistance;
                                ClosestPoint = ptNeighb;
                            }
                        }
                        Color cl = ClTools.GetColorRGB(((float)i) / size, 1.0f);
                        ClosestPoint.Color = cl;
                        UpperPoint.Color = cl;
                        movingPoints.Add(UpperPoint.PointID, ClosestPoint.PointID);
                    }
                }
            }
            p_Model.ResetVisitedPoints();
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPointsWithMovement(NoseTip, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString(), movingPoints);

            iter = p_Model.GetIterator();
            while (iter.IsValid())
            {
                if (!iter.IsSpecificValueCalculated(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip))
                {
                    iter = p_Model.RemovePointFromModel(iter);
                }
                else
                {
                    double distance = iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip);
                    if (distance > m_fDistence)
                        iter = p_Model.RemovePointFromModel(iter);
                    else
                        iter.MoveToNext();
                }
            }
            return;

            //------------------------ PCA ----------------------
            /*
            float MeanX = 0;
            float MeanY = 0;
            float MeanZ = 0;
            Matrix xy = new Matrix(3, BiggestBoundary.Count);
            for (int i = 0; i < BiggestBoundary.Count; i++)
            {
                xy[0,i] = BiggestBoundary[i].X;
                xy[1,i] = BiggestBoundary[i].Y
                xy[2, i] = BiggestBoundary[i].Z;
                
                MeanX += BiggestBoundary[i].X;
                MeanY += BiggestBoundary[i].Y;
                MeanZ += BiggestBoundary[i].Z;
                
                BiggestBoundary[i].Color = Color.Red;
            }
            MeanX /= BiggestBoundary.Count;
            MeanY /= BiggestBoundary.Count;
            MeanZ /= BiggestBoundary.Count;

            for (int i = 0; i < BiggestBoundary.Count; i++)
            {
                xy[0, i] -= MeanX;
                xy[1, i] -= MeanY;
                xy[2, i] -= MeanZ;
            }


            Matrix Transpose = xy.Clone();
            Transpose.Transpose();
            Matrix Corelation = xy * Transpose;

            SingularValueDecomposition SVD = new SingularValueDecomposition(Corelation);

            Cl3DModel.Cl3DModelPointIterator point = p_Model.AddPointToModel(MeanX, MeanY, MeanZ);
            Cl3DModel.Cl3DModelPointIterator point2 = p_Model.AddPointToModel(MeanX + (float)SVD.LeftSingularVectors[0, 0] * 10, MeanY + (float)SVD.LeftSingularVectors[1, 0] * 10, MeanZ + (float)SVD.LeftSingularVectors[2, 0] * 10);
            Cl3DModel.Cl3DModelPointIterator point3 = p_Model.AddPointToModel(MeanX + (float)SVD.LeftSingularVectors[0, 1] * 10, MeanY + (float)SVD.LeftSingularVectors[1, 1] * 10, MeanZ + (float)SVD.LeftSingularVectors[2, 1] * 10);
            Cl3DModel.Cl3DModelPointIterator point4 = p_Model.AddPointToModel(MeanX + (float)SVD.LeftSingularVectors[0, 2] * 10, MeanY + (float)SVD.LeftSingularVectors[1, 2] * 10, MeanZ + (float)SVD.LeftSingularVectors[2, 2] * 10);
            point2.Color = Color.Red;
            point3.Color = Color.Green;
            point4.Color = Color.Blue;
            point.AddNeighbor(point2);
            point.AddNeighbor(point3);
            point.AddNeighbor(point4);
            */
            //---------------------------------------------------------------------------
            
        }
    }
}

