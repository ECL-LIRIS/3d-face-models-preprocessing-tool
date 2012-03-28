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
*   @file       ClRifModelReader.cs
*   @brief      Class responsible for reading models from RIF files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       25-04-2009
*
*   @history
*   @item		25-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRifModelReader : ClBaseModelReader
    {
        public ClRifModelReader()
            : base("rif")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            Cl3DModel.Cl3DModelPointIterator MaxPoint = null;
            
                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {
                    int iTableWidth = 0;
                    int iTableHeight = 0;
                    bool endOfHeader = false;

                    string line = "";
                    while ((line = FileStream.ReadLine()) != null) // Search for ROWS
                    {
                        if(line.Equals("|"))
                        {
                            endOfHeader = true;
                            break;
                        }
                    }
                    
                    if(!endOfHeader)
                        throw new Exception("Wrong header");

                    string precision = "";
                    string format = "";
                    string width = "";
                    string height = "";
                    string sX = "";
                    string sY = "";
                    string sZ = "";
                    string sValidPixels = "";

                    if ((precision = FileStream.ReadLine()) == null)
                        throw new Exception("Wrong file format, file is to short, cannot get precision");

                    if ((format = FileStream.ReadLine()) == null)
                        throw new Exception("Wrong file format, file is to short, cannot get format");

                    if ((width = FileStream.ReadLine()) == null)
                        throw new Exception("Wrong file format, file is to short, cannot get width");

                    if ((height = FileStream.ReadLine()) == null)
                        throw new Exception("Wrong file format, file is to short, cannot get height");

                    iTableWidth = Int32.Parse(width);
                    iTableHeight = Int32.Parse(height);
                    
                    string tmp = "";
                    for(int i = 0; i< iTableHeight; i++)
                    {
                        if (((tmp = FileStream.ReadLine()) == null))
                            throw new Exception("Wrong file format, file is to short, cannot get sX");
                        sX += tmp;
                    }

                    for (int i = 0; i < iTableHeight; i++)
                    {
                        if (((tmp = FileStream.ReadLine()) == null))
                            throw new Exception("Wrong file format, file is to short, cannot get sY");
                        sY += tmp;
                    }

                    for (int i = 0; i < iTableHeight; i++)
                    {
                        if (((tmp = FileStream.ReadLine()) == null))
                            throw new Exception("Wrong file format, file is to short, cannot get sZ");
                        sZ += tmp;
                    }

                    for (int i = 0; i < iTableHeight; i++)
                    {
                        if (((tmp = FileStream.ReadLine()) == null))
                            throw new Exception("Wrong file format, file is to short, cannot get sValidPixels");
                        sValidPixels += tmp;
                    }
                    
                    FileStream.Close();

                    string[] sArrayPixels = sValidPixels.Remove(sValidPixels.Length - 1).Split(' ');
                    string[] sArrayX = sX.Remove(sX.Length - 1).Split(' ');
                    string[] sArrayY = sY.Remove(sY.Length - 1).Split(' ');
                    string[] sArrayZ = sZ.Remove(sZ.Length - 1).Split(' ');

                    

                    float vectorLength = iTableWidth * iTableHeight;

                    if (sArrayX.Length != vectorLength ||
                        sArrayY.Length != vectorLength ||
                        sArrayZ.Length != vectorLength ||
                        sArrayPixels.Length != vectorLength)
                        throw new Exception("Arrays length are different");

                    int ax = 0;
                    int ay = 0;
                    Cl3DModel.Cl3DModelPointIterator[,] tabOfElements = new Cl3DModel.Cl3DModelPointIterator[iTableWidth, iTableHeight];
                    //p_mModel3D.ResetModel();
                    for (int i = 0; i < sArrayPixels.Length; i++)
                    {
                        if (ax == iTableWidth)
                        {
                            ax = 0;
                            ay++;
                        }

                        if (ax >= iTableWidth || ay >= iTableHeight)
                            throw new Exception("One of acces operators is bigger than should be");

                        if (sArrayPixels[i].Equals("0"))
                        {
                            string Xstring = sArrayX[i];
                            string Ystring = sArrayY[i];
                            string Zstring = sArrayZ[i];

                            float x = System.Single.Parse(Xstring, System.Globalization.CultureInfo.InvariantCulture);
                            float y = System.Single.Parse(Ystring, System.Globalization.CultureInfo.InvariantCulture);
                            float z = System.Single.Parse(Zstring, System.Globalization.CultureInfo.InvariantCulture);

                            tabOfElements[ax,ay] = p_mModel3D.AddPointToModel(x, y, z, ax, ay);
                            if (MaxPoint == null)
                                MaxPoint = tabOfElements[ax, ay];
                            else if (tabOfElements[ax, ay].Z > MaxPoint.Z)
                                MaxPoint = tabOfElements[ax, ay];
                        }
                        ax++;
                    }
                    float minusX = MaxPoint.X;
                    float minusY = MaxPoint.Y;
                    float minusZ = MaxPoint.Z;
                    for (int x = 0; x < iTableWidth; x++)
                    {
                        for (int y = 0; y < iTableHeight; y++)
                        {
                            if (tabOfElements[x, y] == null)
                                continue;

                            if (x - 1 >= 0 && y + 1 < iTableHeight && tabOfElements[x - 1, y + 1] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x - 1, y + 1]);

                            if (x - 1 >= 0 && tabOfElements[x - 1, y] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x - 1, y]);

                            if (x - 1 >= 0 && y - 1 >= 0 && tabOfElements[x - 1, y - 1] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x - 1, y - 1]);

                            if (y - 1 >= 0 && tabOfElements[x, y - 1] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x, y - 1]);

                            if (x + 1 < iTableWidth && y - 1 >= 0 && tabOfElements[x + 1, y - 1] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x + 1, y - 1]);

                            if (x + 1 < iTableWidth && tabOfElements[x + 1, y] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x + 1, y]);

                            if (x + 1 < iTableWidth && y + 1 < iTableHeight && tabOfElements[x + 1, y + 1] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x + 1, y + 1]);

                            if (y + 1 < iTableHeight && tabOfElements[x, y + 1] != null)
                                tabOfElements[x, y].AddNeighbor(tabOfElements[x, y + 1]);
                        }
                    }
                }
        }
    }
}