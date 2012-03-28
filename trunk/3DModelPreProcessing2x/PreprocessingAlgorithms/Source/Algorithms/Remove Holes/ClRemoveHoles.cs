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
*   @file       ClFindHoles.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveHoles : ClBaseFaceAlgorithm
    {
        private class Compare2DDistance : IComparer<Cl3DModel.Cl3DModelPointIterator>
        {
            Cl3DModel.Cl3DModelPointIterator m_Orygin;

            public Compare2DDistance(Cl3DModel.Cl3DModelPointIterator Orygin)
            {
                m_Orygin = Orygin;
            }
            public virtual int Compare(Cl3DModel.Cl3DModelPointIterator One, Cl3DModel.Cl3DModelPointIterator Two)
            {
                double dist1 = Math.Sqrt(Math.Pow(One.X - m_Orygin.X, 2) + Math.Pow(One.Y - m_Orygin.Y, 2));
                double dist2 = Math.Sqrt(Math.Pow(Two.X - m_Orygin.X, 2) + Math.Pow(Two.Y - m_Orygin.Y, 2));
                if (dist1 < dist2)
                    return -1;
                else if (dist1 > dist2)
                    return 1;
                else
                    return 0;
            }
        }

        
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveHoles();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Remove Holes\Remove Holes IV2 (Surface Estimation)";

        public ClRemoveHoles() : base(ALGORITHM_NAME) { }

        private bool m_bColorIt = true;
        private double m_Delta = 0.99;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Color It"))
            {
                if (p_sValue.Equals("True"))
                    m_bColorIt = true;
                else if (p_sValue.Equals("False"))
                    m_bColorIt = false;
                else
                    throw new Exception("Unknown value: " + p_sValue);
            }
            else if (p_sProperity.Equals("Delta"))
            {
                m_Delta = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Color It", m_bColorIt.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Delta", m_Delta.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }


        private Cl3DModel.Cl3DModelPointIterator GetRandomUnseenPointOfHole(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                return null;
            do
            {
                if (iter.GetListOfNeighbors().Count < 8 && !iter.AlreadyVisited)
                    return iter;
            } while (iter.MoveToNext());
            return null;
        }
        private Cl3DModel.Cl3DModelPointIterator findNextUnseenElement(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (iter.IsValid())
            {
                while (iter.AlreadyVisited)
                {
                    if (!iter.MoveToNext())
                        return null;
                }
                return iter;
            }
            else
                return null;
        }


        private void RemoveUnconnectedParts(ref Cl3DModel p_Model)
        {
            if (p_Model.ModelType != "wrl")
                throw new Exception("Only WRL models are supported by this method - IV2 Data set");

            List<List<Cl3DModel.Cl3DModelPointIterator>> listOfRegions = new List<List<Cl3DModel.Cl3DModelPointIterator>>();

            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<Cl3DModel.Cl3DModelPointIterator> newListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            Cl3DModel.Cl3DModelPointIterator iter = null;
            int listNo = 0;

            while ((iter = findNextUnseenElement(ref p_Model)) != null)
            {
                ListToCheck.Add(iter);
                listOfRegions.Add(new List<Cl3DModel.Cl3DModelPointIterator>());
                do
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator ElementFromListToCheck in ListToCheck)
                    {
                        if (!ElementFromListToCheck.AlreadyVisited)
                        {
                            listOfRegions[listNo].Add(ElementFromListToCheck);
                            ElementFromListToCheck.AlreadyVisited = true;
                            foreach (Cl3DModel.Cl3DModelPointIterator it in ElementFromListToCheck.GetListOfNeighbors())
                            {
                                if (!it.AlreadyVisited)
                                    newListToCheck.Add(it);
                            }
                        }
                    }

                    ListToCheck.Clear();
                    foreach (Cl3DModel.Cl3DModelPointIterator it in newListToCheck)
                        ListToCheck.Add(it);
                    newListToCheck.Clear();

                } while (ListToCheck.Count != 0);
                ListToCheck.Clear();
                listNo++;
            }

            if (listOfRegions.Count <= 1)
                return;

            int maxCount = listOfRegions[0].Count; ;
            int maxI = 0;
            for (int i = 1; i < listOfRegions.Count; i++)
            {
                if (listOfRegions[i].Count > maxCount)
                {
                    maxI = i;
                    maxCount = listOfRegions[i].Count;
                }
            }
            
            for (int i = 0; i < listOfRegions.Count; i++)
            {
                if (i == maxI)
                    continue;

                for (int j = 0; j < listOfRegions[i].Count; j++)
                {
                    p_Model.RemovePointFromModel(listOfRegions[i][j]);
                }
            }

        }
        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            RemoveUnconnectedParts(ref p_Model);
            p_Model.ResetVisitedPoints();
            List<List<Cl3DModel.Cl3DModelPointIterator>> listOfHoles = new List<List<Cl3DModel.Cl3DModelPointIterator>>();

            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<Cl3DModel.Cl3DModelPointIterator> newListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            Cl3DModel.Cl3DModelPointIterator iter = null;
            int listNo = 0;

            while ((iter = GetRandomUnseenPointOfHole(ref p_Model)) != null)
            {
                ListToCheck.Add(iter);
                listOfHoles.Add(new List<Cl3DModel.Cl3DModelPointIterator>());
                do
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator ElementFromListToCheck in ListToCheck)
                    {
                        if (!ElementFromListToCheck.AlreadyVisited && ElementFromListToCheck.GetListOfNeighbors().Count < 8)
                        {
                            listOfHoles[listNo].Add(ElementFromListToCheck);
                            ElementFromListToCheck.AlreadyVisited = true;
                            foreach (Cl3DModel.Cl3DModelPointIterator it in ElementFromListToCheck.GetListOfNeighbors())
                            {
                                if (!it.AlreadyVisited && it.GetListOfNeighbors().Count < 8)
                                    newListToCheck.Add(it);
                            }
                        }
                    }

                    ListToCheck.Clear();
                    foreach (Cl3DModel.Cl3DModelPointIterator it in newListToCheck)
                        ListToCheck.Add(it);
                    newListToCheck.Clear();

                } while (ListToCheck.Count != 0);
                ListToCheck.Clear();
                listNo++;
            }
            //-------------------------------- End of searching holes

            //-------------------------------- Removing the longest hole - border of the face
            if (listOfHoles.Count == 0)
                return;
            int maxCount = listOfHoles[0].Count; ;
            int maxI = 0;
            for (int i = 1; i < listOfHoles.Count; i++)
            {
                if (listOfHoles[i].Count > maxCount)
                {
                    maxI = i;
                    maxCount = listOfHoles[i].Count;
                }
            }
            
            listOfHoles.RemoveAt(maxI);
            //-------------------------------- End of removing the longest hole

            if (m_bColorIt)
            {
                for (int i = 0; i < listOfHoles.Count; i++)
                {
                    
                    for (int j = 0; j < listOfHoles[i].Count; j++)
                    {
                        listOfHoles[i][j].Color = Color.FromArgb(255,0,255);//ClTools.GetColorRGB(((float)i) / listOfHoles.Count, 1);
                    }
                }
            }
            
            p_Model.ResetVisitedPoints();
            //----------------- Fill holes
            foreach (List<Cl3DModel.Cl3DModelPointIterator> hole in listOfHoles)
            {
                double A = 0;
                double B = 0;
                double C = 0;
                double D = 0;
                double E = 0;
                double F = 0;
                List<Cl3DModel.Cl3DModelPointIterator> holeToEstimateSurface = new List<Cl3DModel.Cl3DModelPointIterator>();
                foreach (Cl3DModel.Cl3DModelPointIterator it in hole) // we are adding new points to other list becouse all the time its used to aproximate surface
                {
                    holeToEstimateSurface.Add(it.CopyIterator());
                    it.AlreadyVisited = true;
                    List<Cl3DModel.Cl3DModelPointIterator> Neighborhood;
                    ClTools.GetNeighborhoodWithGeodesicDistance(out Neighborhood, it, 20);
                    foreach (Cl3DModel.Cl3DModelPointIterator Neighbor in Neighborhood)
                    {
                        if (!Neighbor.AlreadyVisited)
                        {
                            holeToEstimateSurface.Add(Neighbor);
                            Neighbor.AlreadyVisited = true;
                        }
                    }
                }

                List<Cl3DModel.Cl3DModelPointIterator> PointsOfHole = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> SortedPointsOfHole = new List<Cl3DModel.Cl3DModelPointIterator>();

                foreach (Cl3DModel.Cl3DModelPointIterator itOut in hole)
                {
                    SortedPointsOfHole.Add(itOut.CopyIterator());

                    foreach (Cl3DModel.Cl3DModelPointIterator itIn in hole)
                    {

                        if (itIn.PointID == itOut.PointID)
                            continue;

                        float NewPointX = itOut.X;
                        float NewPointY = itIn.Y;
                        
                        bool exist = false;
                        Cl3DModel.Cl3DModelPointIterator ExistPoint = p_Model.GetIterator();
                        if (!ExistPoint.IsValid())
                            throw new Exception("Iterator in Model isn't valid");

                        do
                        {
                            if (Math.Abs(ExistPoint.X - NewPointX) < m_Delta && Math.Abs(ExistPoint.Y - NewPointY) < m_Delta)
                            {
                                exist = true;
                                break;
                            }
                        } while (ExistPoint.MoveToNext());

                        if (!exist)
                        {
                            if (!ClTools.CountSurfaceCoefficients(holeToEstimateSurface, ref A, ref B, ref C, ref D, ref E, ref F))
                                throw new Exception("Cannot calculate surface coefficients");

                            float NewPointZ = (float)(A + B * NewPointX + C * NewPointY + D * NewPointX * NewPointX + E * NewPointX * NewPointY + F * NewPointY * NewPointY);

                            Cl3DModel.Cl3DModelPointIterator newPoint = p_Model.AddPointToModel(NewPointX, NewPointY, NewPointZ);
                            holeToEstimateSurface.Add(newPoint.CopyIterator());

                            PointsOfHole.Add(newPoint.CopyIterator());
                            SortedPointsOfHole.Add(newPoint.CopyIterator());
                            if (m_bColorIt)
                                newPoint.Color = Color.Pink;
                        }
                    }
                }
                foreach (Cl3DModel.Cl3DModelPointIterator iterPoint in PointsOfHole)
                {
                    SortedPointsOfHole.Sort(new Compare2DDistance(iterPoint));
                    iterPoint.AddNeighbor(SortedPointsOfHole[1]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[2]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[3]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[4]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[5]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[6]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[7]);
                    iterPoint.AddNeighbor(SortedPointsOfHole[8]);
                }
            }
        }
    }
}
