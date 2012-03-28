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
*   @file       ClLoadModelCurvaturesValues.cs
*   @brief      ClLoadModelCurvaturesValues
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       08-01-2009
*
*   @history
*   @item		08-01-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateDistanceOnXYZFrom3Points : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateDistanceOnXYZFrom3Points();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Calculations\Calculate XYZ distances from main points";

        public ClCalculateDistanceOnXYZFrom3Points() : base(ALGORITHM_NAME) { }

        bool m_bEuclidean = true;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Euclidean Or Geodesic"))
            {
                if(p_sValue.Equals("Geodesic"))
                    m_bEuclidean = false;
                else if(p_sValue.Equals("Euclidean"))
                    m_bEuclidean = true;
                else
                    throw new Exception("Unknown value: " + p_sValue); 
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string value = "Euclidean";
            if(!m_bEuclidean)
                value = "Geodesic";

            list.Add(new KeyValuePair<string, string>("Euclidean Or Geodesic", value));
            return list;
        }

        private void CalculateGeodesicDistancesFromPoint(string DistanceName, Cl3DModel.Cl3DModelPointIterator p_BasicPoint, Cl3DModel p_Model)
        {
            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = null;
            List<Cl3DModel.Cl3DModelPointIterator> NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

            NewListToCheck.Add(p_BasicPoint);
            p_BasicPoint.AddSpecificValue(DistanceName, 0.0f);

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                ListToCheck = NewListToCheck;

                NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

                foreach (Cl3DModel.Cl3DModelPointIterator PointToCheck in ListToCheck)
                {
                    double PointToCheckDistance = 0;
                    if (!PointToCheck.GetSpecificValue(DistanceName, out PointToCheckDistance))
                        throw new Exception("Cannot get distance value");

                    PointToCheck.AlreadyVisited = true;

                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = PointToCheck.GetListOfNeighbors();
                    foreach (Cl3DModel.Cl3DModelPointIterator NeighboorPoint in Neighbors)
                    {
                        double CurrentDistance = PointToCheck - NeighboorPoint;

                        double newDistance = PointToCheckDistance + CurrentDistance;

                        double NeighborOldDistance = 0;
                        if (NeighboorPoint.GetSpecificValue(DistanceName, out NeighborOldDistance))// if point was already visited, check if we dont have sometimes shorter path to it
                        {
                            if (NeighborOldDistance > newDistance)
                                NeighboorPoint.AddSpecificValue(DistanceName, newDistance);
                        }
                        else
                        {
                            NeighboorPoint.AddSpecificValue(DistanceName, newDistance);
                        }

                        if (!NeighboorPoint.AlreadyVisited)
                        {
                            NeighboorPoint.AlreadyVisited = true;
                            NewListToCheck.Add(NeighboorPoint);
                        }
                    }
                }
            } while (NewListToCheck.Count != 0);

            iter = p_Model.GetIterator();
            do
            {
                iter.AlreadyVisited = false;
            } while (iter.MoveToNext());

        }

        private void CalculateEuclideanDistancesFromPoint(string DistanceName, Cl3DModel.Cl3DModelPointIterator point, Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double distance = Math.Sqrt(Math.Pow(point.X - iter.X, 2) + Math.Pow(point.Y - iter.Y, 2) + Math.Pow(point.Z - iter.Z, 2));
                iter.AddSpecificValue(DistanceName, distance);
            } while (iter.MoveToNext());
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator LeftEyeRightCornerPoint = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEyeLeftCornerPoint = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);
            Cl3DModel.Cl3DModelPointIterator NoseTipPoint = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);

            if (m_bEuclidean)
            {
                CalculateEuclideanDistancesFromPoint("XYZ_LeftEyeRightCornerDistance", LeftEyeRightCornerPoint, p_Model);
                CalculateEuclideanDistancesFromPoint("XYZ_RightEyeLeftCornerDistance", RightEyeLeftCornerPoint, p_Model);
                CalculateEuclideanDistancesFromPoint("XYZ_NoseTipDistance", NoseTipPoint, p_Model);
            }
            else
            {
                Cl3DModel.Cl3DModelPointIterator iterRmoeve = p_Model.GetIterator();
                do
                {
                    iterRmoeve.RemoveSpecificValue("XYZ_LeftEyeRightCornerDistance");
                    iterRmoeve.RemoveSpecificValue("XYZ_RightEyeLeftCornerDistance");
                    iterRmoeve.RemoveSpecificValue("XYZ_NoseTipDistance");
                } while (iterRmoeve.MoveToNext());
                CalculateGeodesicDistancesFromPoint("XYZ_LeftEyeRightCornerDistance", LeftEyeRightCornerPoint, p_Model);
                CalculateGeodesicDistancesFromPoint("XYZ_RightEyeLeftCornerDistance", RightEyeLeftCornerPoint, p_Model);
                CalculateGeodesicDistancesFromPoint("XYZ_NoseTipDistance", NoseTipPoint, p_Model);
            }

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double XYZ_LeftEyeRightCornerDistance = 0;
                iter.GetSpecificValue("XYZ_LeftEyeRightCornerDistance", out XYZ_LeftEyeRightCornerDistance);

                double XYZ_RightEyeLeftCornerDistance = 0;
                iter.GetSpecificValue("XYZ_RightEyeLeftCornerDistance", out XYZ_RightEyeLeftCornerDistance);

                double XYZ_NoseTipDistance = 0;
                iter.GetSpecificValue("XYZ_NoseTipDistance", out XYZ_NoseTipDistance);

                double alldistances = Math.Sqrt(Math.Pow(XYZ_LeftEyeRightCornerDistance, 2) + Math.Pow(XYZ_RightEyeLeftCornerDistance, 2) + Math.Pow(XYZ_NoseTipDistance, 2));
                iter.AddSpecificValue("AllDistancesXYZ", alldistances);
            } while (iter.MoveToNext());
        }
    }
}
