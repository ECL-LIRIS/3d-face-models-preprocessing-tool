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
    public class ClCropFaceFrequencyFromTheNoseTip : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCropFaceFrequencyFromTheNoseTip();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Crop Face Apperance Frequency from Nose tip";

        public ClCropFaceFrequencyFromTheNoseTip() : base(ALGORITHM_NAME) { }

        private bool m_SaveScreenShots = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if(p_sProperity.Equals("Save Screen Shots"))
            {
                m_SaveScreenShots = Boolean.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Save Screen Shots", m_SaveScreenShots.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator NoseTip = null;

            if (!p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, ref NoseTip))
                throw new Exception("Cannot find specific point NoseTip");

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            float MaxZ = float.MinValue;
            float MinZ = float.MaxValue;
            do
            {
                if(MaxZ < iter.Z)
                    MaxZ = iter.Z;
                if(MinZ > iter.Z)
                    MinZ = iter.Z;
            }while(iter.MoveToNext());

            float step = 1f;
            int name = (int)step;
            List<Cl3DModel.Cl3DModelPointIterator> PointsToRemove = new List< Cl3DModel.Cl3DModelPointIterator>();
            for (float Threshold = NoseTip.Z - step; Threshold > MinZ; Threshold -= step)
            {
                List<Cl3DModel.Cl3DModelPointIterator> PointsToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> NewPoinsToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> VisitedPoints = new List<Cl3DModel.Cl3DModelPointIterator>();
                PointsToCheck.Add(NoseTip.CopyIterator());
                while (PointsToCheck.Count != 0)
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator point in PointsToCheck)
                    {
                        if(point.IsSpecificValueCalculated("ToRemove"))
                            continue;

                        point.AlreadyVisited = true;
                        VisitedPoints.Add(point.CopyIterator());
                        
                        List<Cl3DModel.Cl3DModelPointIterator> Neighbors = point.GetListOfNeighbors();
                        foreach (Cl3DModel.Cl3DModelPointIterator Neighb in Neighbors)
                        {
                            if(Neighb.IsSpecificValueCalculated("ToRemove"))
                                continue;
                            if (Neighb.Z > Threshold && !Neighb.AlreadyVisited)
                            {
                                Neighb.AlreadyVisited = true;
                                NewPoinsToCheck.Add(Neighb.CopyIterator());
                            }
                        }
                    }
                    PointsToCheck = NewPoinsToCheck;
                    NewPoinsToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                }
                Cl3DModel.Cl3DModelPointIterator unconnected = p_Model.GetIterator();
                do
                {
                    if (!unconnected.AlreadyVisited && unconnected.Z > Threshold && !unconnected.IsSpecificValueCalculated("ToRemove"))
                    {
                        if (m_SaveScreenShots)
                            unconnected.Color = Color.Red;
                        unconnected.AddSpecificValue("ToRemove", 1.0f);
                        PointsToRemove.Add(unconnected.CopyIterator());
                    }
                } while (unconnected.MoveToNext());

                foreach (Cl3DModel.Cl3DModelPointIterator pts in VisitedPoints)
                {
                    if (m_SaveScreenShots)
                        pts.Color = Color.Green;
                    pts.AlreadyVisited = false;
                }
                if (m_SaveScreenShots)
                {
                    Bitmap mapa;
                    p_Model.GetBMPImage(out mapa, 1.0f);
                    mapa.Save(p_Model.ModelFilePath + name.ToString() + ".bmp");
                    name += (int)step;
                }
            }
            List<Cl3DModel.Cl3DModelPointIterator> RemPointsVisited = new List<Cl3DModel.Cl3DModelPointIterator>();
            foreach (Cl3DModel.Cl3DModelPointIterator RemPoint in PointsToRemove)
            {
                if(!RemPoint.IsValid() || RemPoint.AlreadyVisited)
                    continue;

                List<Cl3DModel.Cl3DModelPointIterator> PointsToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

                PointsToCheck.Add(RemPoint);
                bool remove = false;
                while (PointsToCheck.Count != 0)
                {
                    List<Cl3DModel.Cl3DModelPointIterator> NewPtsToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                    foreach (Cl3DModel.Cl3DModelPointIterator pt in PointsToCheck)
                    {
                        if (!pt.IsValid() || pt.AlreadyVisited)
                            continue;
                        pt.AlreadyVisited = true;

                        RemPointsVisited.Add(pt);
                        
                        List<Cl3DModel.Cl3DModelPointIterator> Neighbors = pt.GetListOfNeighbors();
                        if (Neighbors.Count != 8)
                        {
                            remove = true;
                        }
                        else
                        {
                            foreach(Cl3DModel.Cl3DModelPointIterator ne in Neighbors)
                            {
                                if (ne.IsSpecificValueCalculated("ToRemove") && !ne.AlreadyVisited)
                                    NewPtsToCheck.Add(ne);
                            }
                        }
                    }
                    PointsToCheck = NewPtsToCheck;
                }
                if (remove)
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator ppt in RemPointsVisited)
                        p_Model.RemovePointFromModel(ppt);
                }
                RemPointsVisited = new List<Cl3DModel.Cl3DModelPointIterator>();
            }
        }
    }
}

