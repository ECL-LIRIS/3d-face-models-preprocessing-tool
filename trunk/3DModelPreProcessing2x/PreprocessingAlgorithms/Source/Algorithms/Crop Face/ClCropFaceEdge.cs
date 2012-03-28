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
    public class ClCropFaceEdge : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCropFaceEdge();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Mark High Edges (req. curv. 10-25 & 40) & Crop by Geodesic";

        public ClCropFaceEdge() : base(ALGORITHM_NAME) { }

        private float m_fDistence = 100f;
        private bool m_bMarkRegion = false;
        private bool m_bRemoveRegions = true;
        private bool m_bCrop = true;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Geodesic Distance"))
            {
                m_fDistence = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Color Localized Mouth Region"))
            {
                m_bMarkRegion = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Remove ALL High Curv. Regions"))
            {
                m_bRemoveRegions = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Crop by geodesic"))
            {
                m_bCrop = Boolean.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Geodesic Distance", m_fDistence.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Color Localized Mouth Region", m_bMarkRegion.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Remove ALL High Curv. Regions", m_bRemoveRegions.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Crop by geodesic", m_bCrop.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        private void MarkHighCurvEdges(ref Cl3DModel Model)
        {
            Cl3DModel.Cl3DModelPointIterator NoseTip = Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            Cl3DModel.Cl3DModelPointIterator LeftEye = Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEye = Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);

            float diistance = (float)Math.Sqrt(Math.Pow(NoseTip - LeftEye, 2) + Math.Pow(NoseTip - RightEye, 2) + Math.Pow(LeftEye - RightEye, 2)) * 1.15f;

            List<Cl3DModel.Cl3DModelPointIterator> pointsIn = new List<Cl3DModel.Cl3DModelPointIterator>();
            Cl3DModel.Cl3DModelPointIterator iter = Model.GetIterator();
            do
            {
                float dist = (float)Math.Sqrt(Math.Pow((float)(iter - NoseTip), 2) + Math.Pow((float)(iter - LeftEye), 2) + Math.Pow((float)(iter - RightEye), 2));
                if (dist < diistance)
                    pointsIn.Add(iter.CopyIterator());

            } while (iter.MoveToNext());

            double MaxK1_10 = double.MinValue;
            double MinK1_10 = double.MaxValue;

            double MaxK1_15 = double.MinValue;
            double MinK1_15 = double.MaxValue;

            double MaxK1_20 = double.MinValue;
            double MinK1_20 = double.MaxValue;

            double MaxK1_25 = double.MinValue;
            double MinK1_25 = double.MaxValue;

            double MaxK1_55 = double.MinValue;
            double MinK1_40 = double.MaxValue;

            foreach (Cl3DModel.Cl3DModelPointIterator PointIn in pointsIn)
            {
                double valK1_10;
                if (!PointIn.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_10, out valK1_10))
                {
                    ClInformationSender.SendInformation("K1 10 unavailable for the point "+PointIn.PointID.ToString(), ClInformationSender.eInformationType.eDebugText);
                    continue;
                }

                double valK1_15;
                if (!PointIn.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_15, out valK1_15))
                {
                    ClInformationSender.SendInformation("K1 15 unavailable for the point "+PointIn.PointID.ToString(), ClInformationSender.eInformationType.eDebugText);
                    continue;
                }

                double valK1_20;
                if (!PointIn.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_20, out valK1_20))
                {
                    ClInformationSender.SendInformation("K1 20 unavailable for the point " + PointIn.PointID.ToString(), ClInformationSender.eInformationType.eDebugText);
                    continue;
                }

                double valK1_25;
                if (!PointIn.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_25, out valK1_25))
                {
                    ClInformationSender.SendInformation("K1 25 unavailable for the point " + PointIn.PointID.ToString(), ClInformationSender.eInformationType.eDebugText);
                    continue;
                }

                double valK1_40;
                if (!PointIn.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_40, out valK1_40))
                {
                    ClInformationSender.SendInformation("K1 40 unavailable for the point " + PointIn.PointID.ToString(), ClInformationSender.eInformationType.eDebugText);
                    continue;
                }

                if (MinK1_10 > valK1_10)
                    MinK1_10 = valK1_10;
                if (MaxK1_10 < valK1_10)
                    MaxK1_10 = valK1_10;

                if (MinK1_15 > valK1_15)
                    MinK1_15 = valK1_15;
                if (MaxK1_15 < valK1_15)
                    MaxK1_15 = valK1_15;

                if (MinK1_20 > valK1_20)
                    MinK1_20 = valK1_20;
                if (MaxK1_20 < valK1_20)
                    MaxK1_20 = valK1_20;

                if (MinK1_25 > valK1_25)
                    MinK1_25 = valK1_25;
                if (MaxK1_25 < valK1_25)
                    MaxK1_25 = valK1_25;

                if (MinK1_40 > valK1_40)
                    MinK1_40 = valK1_40;
                if (MaxK1_55 < valK1_40)
                    MaxK1_55 = valK1_40;
            }

            iter = Model.GetIterator();
            do
            {
                double valK1_10;
                if (!iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_10, out valK1_10))
                    continue;

                double valK1_15;
                if (!iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_15, out valK1_15))
                    continue;

                double valK1_20;
                if (!iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_20, out valK1_20))
                    continue;

                double valK1_25;
                if (!iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_25, out valK1_25))
                    continue;

                double valK1_40;
                if (!iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_40, out valK1_40))
                    continue;

                if (valK1_10 < MinK1_10 || valK1_10 > MaxK1_10 ||
                    valK1_15 < MinK1_15 || valK1_15 > MaxK1_15 ||
                    valK1_20 < MinK1_20 || valK1_20 > MaxK1_20 ||
                    valK1_25 < MinK1_25 || valK1_25 > MaxK1_25 ||
                    valK1_40 < MinK1_40 || valK1_40 > MaxK1_55
                    )
                {
                   // iter.Color = Color.LightBlue;
                    iter.AddSpecificValue("ToRemove_HighCurvature", 1.0f);
                }


            } while (iter.MoveToNext());
        }

        Dictionary<string, int[,]> FaceMaps = new Dictionary<string, int[,]>();
        Dictionary<string, int[,]> RemovedMaps = new Dictionary<string, int[,]>();
        Dictionary<string, int[]> mouthSize = new Dictionary<string, int[]>();

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            MarkHighCurvEdges(ref p_Model);

            p_Model.ResetVisitedPoints();
            //-------------------------------- Cropping
            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            Cl3DModel.Cl3DModelPointIterator LeftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);
           
            float distanceBetweenEyes = LeftEye - RightEye;
            float ossfest = distanceBetweenEyes * 0.4f; //20% of distance between eyes will be taken as a stripe down to search for the mouth hole
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            Dictionary<uint, Cl3DModel.Cl3DModelPointIterator> pointsToRemove = new Dictionary<uint, Cl3DModel.Cl3DModelPointIterator>();
            do
            {
                if (iter.X > LeftEye.X + ossfest && iter.X < RightEye.X - ossfest && iter.Y < NoseTip.Y - 20 && iter.Y > NoseTip.Y - 43)
                {
                   // iter.Color = Color.LightBlue;
                    if (iter.IsSpecificValueCalculated("ToRemove_HighCurvature"))
                    {
                        pointsToRemove.Add(iter.PointID, iter.CopyIterator());
                    }
                }

            } while (iter.MoveToNext());

            List<List<Cl3DModel.Cl3DModelPointIterator>> ListOfRegions = new List<List<Cl3DModel.Cl3DModelPointIterator>>();

            while (pointsToRemove.Count != 0)
            {
                Cl3DModel.Cl3DModelPointIterator test = new List<Cl3DModel.Cl3DModelPointIterator>(pointsToRemove.Values)[0];
                pointsToRemove.Remove(test.PointID);

                bool toRemove = false;
                List<Cl3DModel.Cl3DModelPointIterator> CurrentList = new List<Cl3DModel.Cl3DModelPointIterator>();
                ListOfRegions.Add(CurrentList);

                List<Cl3DModel.Cl3DModelPointIterator> ToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                ToCheck.Add(test);
                test.AlreadyVisited = true;
                do
                {
                    List<Cl3DModel.Cl3DModelPointIterator> NewToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                    foreach (Cl3DModel.Cl3DModelPointIterator pt in ToCheck)
                    {
                        CurrentList.Add(pt.CopyIterator());

                        foreach (Cl3DModel.Cl3DModelPointIterator ptNb in pt.GetListOfNeighbors())
                        {
                            if (ptNb.IsSpecificValueCalculated("ToRemove_HighCurvature") && ptNb.AlreadyVisited == false)
                            {
                                ptNb.AlreadyVisited = true;
                                NewToCheck.Add(ptNb.CopyIterator());
                                pointsToRemove.Remove(ptNb.PointID);
                                List<Cl3DModel.Cl3DModelPointIterator> Neighbors = ptNb.GetListOfNeighbors();

                             //   if (Neighbors.Count < 8) // that means the points from marked region went to the end of the model very rare happens that the open mouth region is the externatl region
                             //       toRemove = true;
                            }
                        }
                    }
                    ToCheck = NewToCheck;
                    NewToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                } while (ToCheck.Count != 0);
                if (toRemove)
                    ListOfRegions.Remove(CurrentList);
            }

            List<Cl3DModel.Cl3DModelPointIterator> BiggestRegion = null;
            int SizeOfTheBiggest = int.MinValue;
            for (int i = 0; i < ListOfRegions.Count; i++)
            {
                if (ListOfRegions[i].Count > SizeOfTheBiggest)
                {
                    SizeOfTheBiggest = ListOfRegions[i].Count;
                    BiggestRegion = ListOfRegions[i];
                }
            }

            //------------------- Creating image of localized mouth part
         /*   int NoseX = NoseTip.RangeImageX;
            int NoseY = NoseTip.RangeImageY;
            //BiggestRegion
            int[,] FMap = null;
            if (!FaceMaps.TryGetValue(p_Model.m_sExpression, out FMap))
            {
                FMap = new int[200, 200];
                FaceMaps.Add(p_Model.m_sExpression, FMap);
            }
            Cl3DModel.Cl3DModelPointIterator its = p_Model.GetIterator();
            do
            {
                int currX = its.RangeImageX;
                int currY = its.RangeImageY;


                int x = currX - NoseX + 100;
                int y = currY - NoseY + 100;

                FMap[x, y]++;
            } while (its.MoveToNext());

            int MaxF = int.MinValue;
            for (int i = 0; i < 200; i++)
                for(int j=0; j< 200; j++)
                    if (FMap[i, j] > MaxF)
                        MaxF = FMap[i, j];


            //-
            int[,] FRem = null;
            if (!RemovedMaps.TryGetValue(p_Model.m_sExpression, out FRem))
            {
                FRem = new int[200, 200];
                RemovedMaps.Add(p_Model.m_sExpression, FRem);
            }
            
            if (BiggestRegion != null)
            {
                foreach (Cl3DModel.Cl3DModelPointIterator pp in BiggestRegion)
                {
                    int currX = pp.RangeImageX;
                    int currY = pp.RangeImageY;


                    int x = currX - NoseX + 100;
                    int y = currY - NoseY + 100;

                    FRem[x, y]++;
                }
            }

            FRem[NoseTip.RangeImageX - NoseX + 100, NoseTip.RangeImageY - NoseY + 100]++;
          //  FRem[LeftEye.RangeImageX - NoseX + 100, LeftEye.RangeImageY - NoseY + 100]++;
          //  FRem[RightEye.RangeImageX - NoseX + 100, RightEye.RangeImageY - NoseY + 100]++;


            int MaxR = int.MinValue;
            for (int i = 0; i < 200; i++)
                for (int j = 0; j < 200; j++)
                    if (FRem[i, j] > MaxR)
                        MaxR = FRem[i, j];









            Bitmap map = new Bitmap(200, 200);
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    map.SetPixel(i, j, ClTools.GetColorGray(((float)FMap[i, j]) / MaxF, 1.0f));
                    if(FRem[i, j] != 0)
                        map.SetPixel(i, j, ClTools.GetColorGray( 1 - ((float)FRem[i, j]) / MaxR, 1.0f));
                }
            }
            map.Save("d:\\" + p_Model.m_sExpression + ".bmp");
            return;
          */
            //------------------------------------------------------------

          /*  TextWriter tw = new StreamWriter("d:\\MouthSizeBosphorus2.txt", false);

            
            int ssize = 0;
            if (BiggestRegion != null)
                ssize = BiggestRegion.Count;

            int[] OldSize;
            if(!mouthSize.TryGetValue(p_Model.m_sExpression, out OldSize))
            {
                OldSize = new int[2];
                mouthSize.Add(p_Model.m_sExpression, OldSize);
            }
            OldSize[0] += ssize;
            OldSize[1]++;
            
            foreach(KeyValuePair<string, int[]> val in mouthSize)
                tw.WriteLine(val.Key+" "+ val.Value[0].ToString()+ " " + val.Value[1].ToString());

            tw.Close();

            return;
           */ 
            //----------------------------------------------------------------

            //---------------------- NO mouth REGION LOCALIZED
            if (BiggestRegion == null)
            {
                ClInformationSender.SendInformation("No mouth localized, normal Geodesic distance calculation", ClInformationSender.eInformationType.eDebugText);
                if (m_bRemoveRegions) // still there can be vertexes with high curvature (edges between hair and the face)
                {
                    Cl3DModel.Cl3DModelPointIterator remover = p_Model.GetIterator();
                    while (remover.IsValid())
                    {
                        if (remover.IsSpecificValueCalculated("ToRemove_HighCurvature"))
                            remover = p_Model.RemovePointFromModel(remover);
                        else
                            remover.MoveToNext();
                    }
                }

                ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(NoseTip, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString());
                
                if (!m_bCrop)
                    return;
                
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
            }

            //------------------------- Mouth REGION LOCALiZED
            p_Model.ResetVisitedPoints();
            List<Cl3DModel.Cl3DModelPointIterator> BorderOfTheBigestRegion = new List<Cl3DModel.Cl3DModelPointIterator>();
            foreach (Cl3DModel.Cl3DModelPointIterator pts in BiggestRegion)
            {
                if (m_bMarkRegion)
                    pts.Color = Color.Red;

                List<Cl3DModel.Cl3DModelPointIterator> neighb = pts.GetListOfNeighbors();
                foreach (Cl3DModel.Cl3DModelPointIterator ppt in neighb)
                {
                    if(!ppt.AlreadyVisited && !ppt.IsSpecificValueCalculated("ToRemove_HighCurvature"))
                    {
                        BorderOfTheBigestRegion.Add(ppt);
                        ppt.AlreadyVisited = true;
                    }
                }
            }

            if (m_bRemoveRegions) // After we have border of the mouth region we can remove them
            {
                Cl3DModel.Cl3DModelPointIterator remover = p_Model.GetIterator();
                while (remover.IsValid())
                {
                    if (remover.IsSpecificValueCalculated("ToRemove_HighCurvature"))
                        remover = p_Model.RemovePointFromModel(remover);
                    else
                        remover.MoveToNext();
                }
            }

            Cl3DModel.Cl3DModelPointIterator MaxLeft = BorderOfTheBigestRegion[0];
            Cl3DModel.Cl3DModelPointIterator MaxRight = BorderOfTheBigestRegion[0];
            foreach (Cl3DModel.Cl3DModelPointIterator pt in BorderOfTheBigestRegion)
            {
                if (pt.X < MaxLeft.X)
                    MaxLeft = pt;
                if (pt.X > MaxRight.X)
                    MaxRight = pt;
            }

            int size = (int)Math.Abs(MaxLeft.X - MaxRight.X) + 1;
            List<Cl3DModel.Cl3DModelPointIterator>[] histogram = new List<Cl3DModel.Cl3DModelPointIterator>[size];

            foreach (Cl3DModel.Cl3DModelPointIterator pt in BorderOfTheBigestRegion)
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
                    //    ClosestPoint.Color = cl;
                    //    UpperPoint.Color = cl;
                        movingPoints.Add(UpperPoint.PointID, ClosestPoint.PointID);
                    }
                }
            }

            //-------------------------------- Calculation of the geodesic using movement points
            p_Model.ResetVisitedPoints();
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPointsWithMovement(NoseTip, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString(), movingPoints);

            if (!m_bCrop)
                return;

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











           
        }
    }
}
