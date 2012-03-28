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
    public class ClOFFModelReader : ClBaseModelReader
    {
        public ClOFFModelReader()
            : base("off")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {
                    string line = "";
                    int NoOfVertexes = 0;
                    int NoOfFaces = 0;

                    if ((line = FileStream.ReadLine()) == null || !line.Equals("OFF"))
                        throw new Exception("Incorrect Header");

                    if ((line = FileStream.ReadLine()) == null)
                       throw new Exception("Incerrect Header");

                    string[] splitted = line.Split(' ');
                    NoOfVertexes = Int32.Parse(splitted[0]);
                    NoOfFaces = Int32.Parse(splitted[1]);

                    for (int i = 0; i < NoOfVertexes; i++)
                    {
                        if ((line = FileStream.ReadLine()) == null)
                            throw new Exception("Incerrect Header");

                        splitted = line.Split(' ');
                        float X = float.Parse(splitted[0].Replace('.',','));
                        float Y = float.Parse(splitted[1].Replace('.', ','));
                        float Z = float.Parse(splitted[2].Replace('.', ','));
                        p_mModel3D.AddPointToModel(X, Y, Z);
                    }
                    for (int i = 0; i < NoOfFaces; i++)
                    {
                        if ((line = FileStream.ReadLine()) == null)
                            throw new Exception("Incorrect Header");

                        splitted = line.Split(' ');
                        int noOfVect = int.Parse(splitted[0]);
                        if (noOfVect != 3)
                            throw new Exception("Wrong number of vertexes in the face");

                        int vertex1 = int.Parse(splitted[1]);
                        int vertex2 = int.Parse(splitted[2]);
                        int vertex3 = int.Parse(splitted[3]);

                        Cl3DModel.Cl3DModelPointIterator iter = p_mModel3D.GetIterator();
                        Cl3DModel.Cl3DModelPointIterator iter2 = p_mModel3D.GetIterator();

                        if(!iter.MoveToPoint((uint)vertex1))
                            throw new Exception("Cannot find the point with ID: " + vertex1.ToString());
                        if(!iter2.MoveToPoint((uint)vertex2))
                            throw new Exception("Cannot find the point with ID: " + vertex2.ToString());

                        iter.AddNeighbor(iter2.CopyIterator());
                        if(!iter2.MoveToPoint((uint)vertex3))
                            throw new Exception("Cannot find the point with ID: " + vertex3.ToString());

                        iter.AddNeighbor(iter2.CopyIterator());
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
