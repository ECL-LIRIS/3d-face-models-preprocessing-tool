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

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClModelModelReader.cs
*   @brief      Class responsible for reading models from Model files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       09-01-2009
*
*   @history
*   @item		09-01-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClModelModelReader : ClBaseModelReader
    {
        public ClModelModelReader()
            : base("model")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, List<uint>>> modelNeighbors = new Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, List<uint>>>();
                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {
                    string line;
                    bool Landmarks = false;
                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (line.Length == 0)
                            continue;
                        if (line[0].Equals('@'))
                            continue;

                        if (line.Contains("Landmark points"))
                        {
                            Landmarks = true;
                            continue;
                        }

                        if (Landmarks)
                        {
                            //Landmark points (ptID): "+nop
                            string[] splitedLine = line.Split(' ');
                            String PointLabel = splitedLine[0];
                            uint PointID = UInt32.Parse(splitedLine[1]);

                            Cl3DModel.Cl3DModelPointIterator iter = p_mModel3D.GetIterator();
                            if(!iter.MoveToPoint(PointID))
                                throw new Exception("Cannot find point no: "+PointID.ToString());

                            p_mModel3D.AddSpecificPoint(PointLabel, iter);
                        }
                        else
                        {
                            string[] splitedLine = line.Split(' ');
                            uint PointId = UInt32.Parse(splitedLine[0], System.Globalization.CultureInfo.InvariantCulture);
                            float X = Single.Parse(splitedLine[1], System.Globalization.CultureInfo.InvariantCulture);
                            float Y = Single.Parse(splitedLine[2], System.Globalization.CultureInfo.InvariantCulture);
                            float Z = Single.Parse(splitedLine[3], System.Globalization.CultureInfo.InvariantCulture);

                            int XImage = Int32.Parse(splitedLine[5], System.Globalization.CultureInfo.InvariantCulture);
                            int YImage = Int32.Parse(splitedLine[6], System.Globalization.CultureInfo.InvariantCulture);

                            Cl3DModel.Cl3DModelPointIterator iter = p_mModel3D.AddPointToModel(X, Y, Z, XImage, YImage, PointId);
                            List<uint> neighbors = new List<uint>();
                            for (int i = 9; i < splitedLine.Length - 1; i++)
                            {
                                neighbors.Add(UInt32.Parse(splitedLine[i], System.Globalization.CultureInfo.InvariantCulture));
                            }
                            modelNeighbors.Add(PointId, new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, List<uint>>(iter, neighbors));
                        }
                    }
                }
                foreach(KeyValuePair<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, List<uint>>> onePoint in modelNeighbors)
                {
                    foreach(uint neighboorNo in onePoint.Value.Value)
                    {
                        KeyValuePair<Cl3DModel.Cl3DModelPointIterator, List<uint>> list;
                        if(modelNeighbors.TryGetValue(neighboorNo, out list))
                            onePoint.Value.Key.AddNeighbor(list.Key);
                    }
                }
            }
            catch (Exception)
            {
                p_mModel3D.ResetModel();
                throw;
            }
        }
    }
}

