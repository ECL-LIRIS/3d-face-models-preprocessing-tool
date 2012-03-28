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
*   @file       ClWrmlModelReader.cs
*   @brief      Class responsible for reading models from VRML files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       05-06-2008
*
*   @history
*   @item		05-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClDATModelReader : ClBaseModelReader
    {
        public ClDATModelReader()
            : base("dat")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                Dictionary<string, Cl3DModel.Cl3DModelPointIterator> Points = new Dictionary<string, Cl3DModel.Cl3DModelPointIterator>();
                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {
                    string line = "";
                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (line.Contains("    [POLYGON [PLANE"))
                        {
                            Cl3DModel.Cl3DModelPointIterator[] polygon = new Cl3DModel.Cl3DModelPointIterator[3];
                            for (int i = 0; i < 3; i++)
                            {
                                if ((line = FileStream.ReadLine()) != null)
                                {
                                    string[] param = line.Replace('[',' ').Replace(']',' ').Split(' ');
                                    double H = Double.Parse(param[3]);
                                    double K = Double.Parse(param[7]);

                                    if ((line = FileStream.ReadLine()) != null)
                                    {
                                        param = line.Replace('[', ' ').Replace(']', ' ').Replace('"', ' ').Split(' ');
                                        float U = Single.Parse(param[3]);
                                        float V = Single.Parse(param[4]);

                                        float X = 0;
                                        float Y = 0;
                                        float Z = 0;

                                        string ID = "";
                                        if(line.Contains("[NORMAL"))
                                        {
                                            X = Single.Parse(param[9]);
                                            Y = Single.Parse(param[10]);
                                            Z = Single.Parse(param[11]);
                                            ID = param[9]+param[10]+param[11];
                                        }
                                        else
                                        {
                                            X = Single.Parse(param[7]);
                                            Y = Single.Parse(param[8]);
                                            Z = Single.Parse(param[9]);
                                            ID = param[7] + param[8] + param[9];
                                        }

                                        Cl3DModel.Cl3DModelPointIterator it = null;
                                        if (!Points.TryGetValue(ID, out it))
                                        {
                                            it = p_mModel3D.AddPointToModel(X, Y, Z);
                                            it.U = U;
                                            it.V = V;
                                            it.AddSpecificValue("GroundK", K);
                                            it.AddSpecificValue("GroundH", H);

                                            Points.Add(ID, it);
                                        }

                                        polygon[i] = it;
                                    }
                                    else
                                        throw new Exception("Something wrong 2");

                                }
                                else
                                    throw new Exception("Something wrong 1");
                            }
                            polygon[0].AddNeighbor(polygon[1]);
                            polygon[0].AddNeighbor(polygon[2]);
                            polygon[1].AddNeighbor(polygon[2]);

                        }
                        else
                        {
                            continue;
                        }
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
