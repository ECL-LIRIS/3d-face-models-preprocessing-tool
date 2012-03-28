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
*   @file       ClSaveModelToObj.cs
*   @brief      ClSaveModelToObj
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       03-12-2008
*
*   @history
*   @item		03-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveModelToObj : ClBaseFaceAlgorithm
    {
        private class ClSortNeighborhoodClockwize : IComparer<Cl3DModel.Cl3DModelPointIterator>
        {
            public virtual int Compare(Cl3DModel.Cl3DModelPointIterator PointOne, Cl3DModel.Cl3DModelPointIterator PointTwo)
            {
                return 1;
            }
        }

        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveModelToObj();
        }

        public static string ALGORITHM_NAME = @"Save\Model\Save Model (.obj)";

        private string m_sFilePostFix = "";

        public ClSaveModelToObj() : base(ALGORITHM_NAME) { }
        public ClSaveModelToObj(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
           // if (p_Model.ModelType != "abs")
             //   throw new Exception("Saveing to .obj files works only for ABS files");

            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + ".obj";
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in the model is not valid");

            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine("#----------------------------------------");
                tw.WriteLine("#     Przemyslaw Szeptycki LIRIS 2009");
                tw.WriteLine("#            Object Face model");
                tw.WriteLine("#  Model name: " + p_Model.ModelFileName);
                tw.WriteLine("#         pszeptycki@gmail.com");
                tw.WriteLine("#----------------------------------------");

                uint vertexNO = 1;
                Dictionary<uint, uint> MapVertexNo = new Dictionary<uint, uint>();
                do
                {
                    string line = "v " + iter.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.Z.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    MapVertexNo.Add(iter.PointID, vertexNO++);

                    tw.WriteLine(line);

                } while (iter.MoveToNext());

                // --- create faces
                iter = p_Model.GetIterator();
                do
                {
                    iter.AlreadyVisited = true;
                    
                    List<Cl3DModel.Cl3DModelPointIterator> neighbors = iter.GetListOfNeighbors();

                    uint MainPointID;
                    if (!MapVertexNo.TryGetValue(iter.PointID, out MainPointID))
                        throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);

                    uint point1 = 0;
                    uint point2 = 0;
                    uint point3 = 0;
                    
                    foreach (Cl3DModel.Cl3DModelPointIterator point in neighbors)
                    {
                        //if (point.AlreadyVisited)
                        //    continue;

                        if (point.RangeImageY == iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // first point
                        {
                            if (!MapVertexNo.TryGetValue(point.PointID, out point1))
                                throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // second point
                        {
                            if (!MapVertexNo.TryGetValue(point.PointID, out point2))
                                throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX == iter.RangeImageX) // third point
                        {
                            if (!MapVertexNo.TryGetValue(point.PointID, out point3))
                                throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);
                        }
                    }

                    string line = "";
                    if (point1 != 0 && point2 != 0)
                    {
                        line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        if (point3 != 0)
                        {
                            line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        }
                    }
                    else if (point1 != 0 && point3 != 0)
                    {
                        line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    }
                    else if (point2 != 0 && point3 != 0)
                    {
                        line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    }

                    tw.Write(line);

                } while (iter.MoveToNext());
                tw.Close();
            }
        }
    }
}
