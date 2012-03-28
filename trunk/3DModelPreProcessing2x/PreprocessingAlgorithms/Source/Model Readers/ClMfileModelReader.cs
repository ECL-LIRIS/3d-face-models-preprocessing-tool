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
*   @file       ClXYZModelReader.cs
*   @brief      Class responsible for reading models from XYZ files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       30-03-2009
*
*   @history
*   @item		30-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClMfileModelReader : ClBaseModelReader
    {
        public ClMfileModelReader()
            : base("m")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {
                    string line = "";
                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line.StartsWith("#"))
                            continue;

                        char[] delimiterChars = { ' ', '{', '}', '(', ')' };
                        string[] splited = line.Split(delimiterChars);
                        //splited.
                        if (splited[0].Equals("Vertex"))
                        {
                            int no=1;
                            uint ID = UInt32.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                            if (splited[no].Equals(""))
                                no++;
                            float x = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                            float y = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                            float z = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);

                            Cl3DModel.Cl3DModelPointIterator iter = p_mModel3D.AddPointToModel(x, y, z, ID);

                            if (splited.Length > 6)
                            {
                                if (splited[no].Equals(""))
                                    no++;

                                if (splited.Length > 6 && splited[no++].Equals("rgb="))
                                {
                                    float r = 1;
                                    float g = 1;
                                    float b = 1;

                                    r = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                                    g = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                                    b = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);

                                    iter.Color = Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
                                    iter.AddSpecificValue("Texture", iter.Color.ToArgb());
                                    no++;
                                }
                                if (splited.Length > 11 && splited[no++].Equals("uv="))
                                {
                                    float U = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                                    float V = Single.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);

                                    iter.U = U;
                                    iter.V = V;
                                }
                            }

                        }
                        else if (splited[0].Equals("Face"))
                        {
                            int no = 2;
                            if (splited[no].Equals(""))
                                no++;

                            uint ID1 = UInt32.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                            uint ID2 = UInt32.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);
                            uint ID3 = UInt32.Parse(splited[no++], System.Globalization.CultureInfo.InvariantCulture);

                           Cl3DModel.Cl3DModelPointIterator iter1 = p_mModel3D.GetIterator();
                           Cl3DModel.Cl3DModelPointIterator iter2 = p_mModel3D.GetIterator();
                           Cl3DModel.Cl3DModelPointIterator iter3 = p_mModel3D.GetIterator();

                           if (!iter1.MoveToPoint(ID1))
                               throw new Exception("Cannot find point with ID: " + ID1.ToString());

                           if (!iter2.MoveToPoint(ID2))
                               throw new Exception("Cannot find point with ID: " + ID2.ToString());

                           if (!iter3.MoveToPoint(ID3))
                               throw new Exception("Cannot find point with ID: " + ID3.ToString());

                           iter1.AddNeighbor(iter2);
                           iter2.AddNeighbor(iter3);
                           iter3.AddNeighbor(iter1);
                        }
                        else
                        {
                            throw new Exception("Unknown tocken: " + splited[0]);
                        }
                    }
                    FileStream.Close();
                }
            }
            catch (Exception e)
            {
                p_mModel3D.ResetModel();
                Exception ex = new Exception("Wrong M File Format: " + p_mModel3D.ModelFilePath, e);
                throw ex;
            }
        }
    }
}
