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
*   @file       ClDoSomething.cs
*   @brief      Templorary class for algorithms testing :)
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       07-12-2010
*
*   @history
*   @item		07-12-2010 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClDoSomething : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClDoSomething();
        }

        public static string ALGORITHM_NAME = @"Tools\DO SOMETHING (TMP alg)!!!";

        public ClDoSomething() : base(ALGORITHM_NAME) { }

        double MeanDistance = 0;
        double RangeMeanDistance = 0;
        int noOfFiles = 0;

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            Cl3DModel.Cl3DModelPointIterator InnerRightEyeCorner = p_Model.GetSpecificPoint("Inner right eye corner");
            Cl3DModel.Cl3DModelPointIterator OuterLeftEyeCorner = p_Model.GetSpecificPoint("Outer left eye corner");
            Cl3DModel.Cl3DModelPointIterator InnerLeftEyeCorner = p_Model.GetSpecificPoint("Inner left eye corner");
            Cl3DModel.Cl3DModelPointIterator OuterRightEyeCorner = p_Model.GetSpecificPoint("Outer right eye corner");

         //   List<Cl3DModel.Cl3DModelPointIterator> InnerPath= null;
         //   List<Cl3DModel.Cl3DModelPointIterator> OuterPath = null;
         //   ClTools.GetPathBetweenPoints(InnerRightEyeCorner, InnerLeftEyeCorner, out InnerPath);
         //   ClTools.GetPathBetweenPoints(OuterLeftEyeCorner, OuterRightEyeCorner, out OuterPath);

            float InnerDistance = InnerRightEyeCorner - InnerLeftEyeCorner;
            float OuterDistance = OuterRightEyeCorner - OuterLeftEyeCorner;
            float distanceBetweenCenter = InnerDistance + ((OuterDistance - InnerDistance) / 2);

            double RangeInnerDistance = Math.Sqrt(Math.Pow(InnerRightEyeCorner.X - InnerLeftEyeCorner.X, 2) + Math.Pow(InnerRightEyeCorner.Y - InnerLeftEyeCorner.Y, 2));
            double RangeOuterDistance = Math.Sqrt(Math.Pow(OuterRightEyeCorner.X - OuterLeftEyeCorner.X, 2) + Math.Pow(OuterRightEyeCorner.Y - OuterLeftEyeCorner.Y, 2));
            double RangedistanceBetweenCenter = RangeInnerDistance + ((RangeOuterDistance - RangeInnerDistance) / 2);

            noOfFiles++;
            MeanDistance += distanceBetweenCenter;
            RangeMeanDistance += RangedistanceBetweenCenter;

            TextWriter rr = new StreamWriter("d:\\Distance between eyes.txt", true);
            rr.WriteLine("\n[" + p_Model.ModelFileName + "] Euclidean 3D distance between center of eyes: " + distanceBetweenCenter.ToString() + " \t Mean distance using: " + noOfFiles.ToString() +" models - equals: " + (MeanDistance/noOfFiles).ToString());
            rr.WriteLine("[" + p_Model.ModelFileName + "] Euclidean 2D distance between center of eyes: " + RangedistanceBetweenCenter.ToString() + " \t Mean distance using: " + noOfFiles.ToString() + " models - equals: " + (RangeMeanDistance / noOfFiles).ToString());
            rr.Close();

            


            /*	Outer left eyebrow(ID: 26920)
     Middle left eyebrow(ID: 30354)
     Inner left eyebrow(ID: 29548)
     Inner right eyebrow(ID: 29249)
     Middle right eyebrow(ID: 30463)
     Outer right eyebrow(ID: 26540)
     Outer left eye corner(ID: 24998)
     Inner left eye corner(ID: 25394)
     Inner right eye corner(ID: 25612)
     Outer right eye corner(ID: 24768)
     Nose saddle left(ID: 23625)
     Nose saddle right(ID: 23641)
     Left nose peak(ID: 17116)
     Nose tip(ID: 18581)
     Right nose peak(ID: 17161)
     Left mouth corner(ID: 8618)
     Upper lip outer middle(ID: 10022)
     Right mouth corner(ID: 8854)
     Upper lip inner middle(ID: 9332)
     Lower lip inner middle(ID: 8819)
     Lower lip outer middle(ID: 7971)
     Chin middle(ID: 1082)/*


 //            File.Delete(p_Model.ModelFilePath);

             /*

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

             while (iter.MoveToNext())
             {
                 double GroundK = iter.GetSpecificValue("GroundK");
                 double GroundH = iter.GetSpecificValue("GroundH");

                 double K_25 = iter.GetSpecificValue("Gaussian_25");
                 double H_25 = iter.GetSpecificValue("Mean_25");

                 iter.AddSpecificValue("DiffK", GroundK - K_25);
                 iter.AddSpecificValue("DiffH", GroundH - Math.Sqrt(H_25));
             }
             */
           // List<Cl3DModel.Cl3DModelPointIterator> Neighb = maxIter.GetListOfNeighbors();
      //      ClTools.GetNeighborhoodWithGeodesicDistance(out Neighb, maxIter, 30);
          //  foreach (Cl3DModel.Cl3DModelPointIterator it in Neighb)
           //     it.Color = Color.Red;

       //     if (maxIter.IsValid())
       //     {
       //         p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString(), maxIter);
       //     }
            

            /*
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                if (iter.Z < 0)
                {
                    iter = p_Model.RemovePointFromModel(iter);
                }
                else
                    iter.MoveToNext();

            } while (iter.IsValid());
            */


          /*  Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double mean25 = 0;
                double meanmena25 = 0;

                double Gaussian = 0;
                double meanGaussian = 0;

                if(!iter.GetSpecificValue("Mean_15",out mean25) || !iter.GetSpecificValue("Mean_Mean_15", out meanmena25))
                    continue;
                if (!iter.GetSpecificValue("Gaussian_15", out Gaussian) || !iter.GetSpecificValue("Mean_Gaussian_15", out meanGaussian))
                    continue;

                if (mean25 * meanmena25 < 0.0f)
                    iter.Color = Color.Red;

                if (Gaussian * meanGaussian < 0.0f)
                    iter.Color = Color.Blue;

            } while (iter.MoveToNext());


            return;
            */
            /*
            //------------------------------
            double distances = 0;
            int count = 0;
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                foreach (Cl3DModel.Cl3DModelPointIterator neighb in iter.GetListOfNeighbors())
                {
                    if (!neighb.AlreadyVisited)
                    {
                        double tmpDist = iter - neighb;
                        distances += tmpDist;
                        count++;
                        neighb.AlreadyVisited = true;
                    }
                }

            } while (iter.MoveToNext());

            TextWriter tw = new StreamWriter("d:\\CurvatureStability.txt", true);
            tw.WriteLine("\n"+p_Model.ModelFileName+" Vetrex count: "+p_Model.ModelPointsCount.ToString());
            tw.WriteLine("Mean distance between points: " + (distances / count).ToString());
            tw.WriteLine("Distances between points: " + distances.ToString());
            tw.WriteLine("No of added distances: " + count.ToString());
            List<KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>> SpecificPoints = new List<KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>>();
            SpecificPoints.Add(new KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>("NoseTip", p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip)));
            SpecificPoints.Add(new KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>("LeftEye", p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner)));
            SpecificPoints.Add(new KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>("RightEye", p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner)));

            foreach (KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator> sp in SpecificPoints)
            {
                tw.WriteLine(sp.Key);
                Cl3DModel.Cl3DModelPointIterator iter2 = sp.Value;
                foreach (string valueName in iter2.GetListOfSpecificValues())
                {
                    double value = iter2.GetSpecificValue(valueName);
                    tw.WriteLine("\t"+valueName + " " + value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            tw.Close();
            */


            //------------------------------------ POINTS DISTANCES FO NEAR ISOMETRIC FACE TREATING
            /*Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint("NoseTip");
             Cl3DModel.Cl3DModelPointIterator point5 = p_Model.GetSpecificPoint("point4");
             Cl3DModel.Cl3DModelPointIterator point8 = p_Model.GetSpecificPoint("point7");
             Cl3DModel.Cl3DModelPointIterator point2 = p_Model.GetSpecificPoint("point3");
             Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
             List<Cl3DModel.Cl3DModelPointIterator> Boundarys = new List<Cl3DModel.Cl3DModelPointIterator>();
             do
             {
                 if (iter.GetListOfNeighbors().Count < 6 && 
                     iter.Y < NoseTip.Y && iter.Y > point5.Y &&
                     iter.X > point8.X && iter.X < point2.X
                     )
                 {
                     Boundarys.Add(iter.CopyIterator());
                 //    iter.Color = Color.Red;
                 }
             } while (iter.MoveToNext());

             Cl3DModel.Cl3DModelPointIterator MaxLeft = Boundarys[0];
             Cl3DModel.Cl3DModelPointIterator MaxRight = Boundarys[0];
             foreach (Cl3DModel.Cl3DModelPointIterator pt in Boundarys)
             {
                 if (pt.X < MaxLeft.X)
                     MaxLeft = pt;
                 if (pt.X > MaxRight.X)
                     MaxRight = pt;
             }

             int size = (int)Math.Abs(MaxLeft.X - MaxRight.X) + 1;
             List<Cl3DModel.Cl3DModelPointIterator>[] histogram = new List<Cl3DModel.Cl3DModelPointIterator>[size];

             foreach (Cl3DModel.Cl3DModelPointIterator pt in Boundarys)
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
                        
                         Color cl = ClTools.GetColorRGB(((float)i) / size, 1.0f);
                         LowerPoint.Color = cl;
                         UpperPoint.Color = cl;
                         movingPoints.Add(UpperPoint.PointID, LowerPoint.PointID);
                     }
                 }
             }

             ClTools.CalculateGeodesicDistanceFromSourcePointToAllPointsWithMovement(NoseTip, "GeodesicWithTheMovingPoints", movingPoints);
             */
/*
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(NoseTip, "GeodesicWith");
            return;

            List<Cl3DModel.Cl3DModelPointIterator> InterestingPoint = new List<Cl3DModel.Cl3DModelPointIterator>();
            InterestingPoint.Add(p_Model.GetSpecificPoint("point1"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point2"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point3"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point4"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point5"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point6"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point7"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point8"));
            InterestingPoint.Add(p_Model.GetSpecificPoint("point9"));
           

            TextWriter tw = new StreamWriter("d:\\PointsDistanceEuclidean.txt", true);
            tw.WriteLine(p_Model.ModelFileName);
            foreach (Cl3DModel.Cl3DModelPointIterator pt in InterestingPoint)
            {
                double distance = pt - NoseTip;//.GetSpecificValue("GeodesicWith");
                string label = "";
                pt.IsLabeled(out label);
                tw.WriteLine(label + " " + distance.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            tw.Close();
 */
        }
    }
}