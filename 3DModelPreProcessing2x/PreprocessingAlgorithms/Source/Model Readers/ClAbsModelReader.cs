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
    public class ClAbsModelReader : ClBaseModelReader
    {
        public ClAbsModelReader()
            : base("abs")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            Cl3DModel.Cl3DModelPointIterator MaxPoint = null;
            try
            {
                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {
                    int iTableWidth = 0;
                    int iTableHeight = 0;

                    #region HeaderParse

                    String header = "";
                    if ((header = FileStream.ReadLine()) != null) // Search for ROWS
                    {
                        string[] val = header.Split(' ');
                        if (val.Length != 2)
                            throw new Exception("Wrong file format");

                        if (!val[1].Equals("rows"))
                            throw new Exception("Wrong file format, cannot find 'rows'");

                        iTableHeight = Int32.Parse(val[0]);
                    }
                    else
                        throw new Exception("Wrong file format, file is to short");

                    if ((header = FileStream.ReadLine()) != null) // Search for COLUMNS
                    {
                        string[] val = header.Split(' ');
                        if (val.Length != 2)
                            throw new Exception("Wrong file format");

                        if (!val[1].Equals("columns"))
                            throw new Exception("Wrong file format, cannot find 'rows'");

                        iTableWidth = Int32.Parse(val[0]);
                    }
                    else
                        throw new Exception("Wrong file format, file is to short");

                    if ((header = FileStream.ReadLine()) != null)// Search for PIXELS
                    {
                        if (!header.StartsWith("pixels (flag X Y Z):"))
                            throw new Exception("Wrong file format, cannot find 'pixels (flag X Y Z):', only this flags are available");
                    }
                    else
                        throw new Exception("Wrong file format, file is to short");
                    #endregion

                    string sValidPixels = "";
                    string sX = "";
                    string sY = "";
                    string sZ = "";
                    if ((sValidPixels = FileStream.ReadLine()) == null)// Search for sValidPixels
                        throw new Exception("Wrong file format, file is to short, cannot get Valid Pixels line");

                    if ((sX = FileStream.ReadLine()) == null)// Search for X
                        throw new Exception("Wrong file format, file is to short, cannot get X line");

                    if ((sY = FileStream.ReadLine()) == null)// Search for Y
                        throw new Exception("Wrong file format, file is to short, cannot get Y line");

                    if ((sZ = FileStream.ReadLine()) == null)// Search for Z
                        throw new Exception("Wrong file format, file is to short, cannot get Z line");

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

                        if (sArrayPixels[i].Equals("1"))
                        {
                            string Xstring = sArrayX[i];//.Replace(".", ",");
                            string Ystring = sArrayY[i];//.Replace(".", ",");
                            string Zstring = sArrayZ[i];//.Replace(".", ",");

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
            catch (Exception)
            {
                p_mModel3D.ResetModel();
                throw;
            }
        }
    }
}
