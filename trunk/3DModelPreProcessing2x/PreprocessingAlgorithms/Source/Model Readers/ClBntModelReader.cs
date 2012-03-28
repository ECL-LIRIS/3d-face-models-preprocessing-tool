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
*   @file       ClBntModelReader.cs
*   @brief      Class responsible for reading models from BNT files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       09-04-2009
*
*   @history
*   @item		09-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClBntModelReader : ClBaseModelReader
    {
        public ClBntModelReader()
            : base("bnt")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                using (FileStream fs = File.OpenRead(p_sFilePath))
                {
                    try
                    {
                        BinaryReader ReaderBinary = new BinaryReader(fs);
                        ushort nRows = ReaderBinary.ReadUInt16();
                        ushort nCols = ReaderBinary.ReadUInt16();
                        double zMin = ReaderBinary.ReadDouble();

                        ushort len = ReaderBinary.ReadUInt16();

                        char[] imFile = ReaderBinary.ReadChars((int)len);

                        p_mModel3D.ModelExpression = p_mModel3D.ModelFileName.Substring(6);
                        
                        uint dataLen = ReaderBinary.ReadUInt32();

                        uint oneRead = dataLen / 5;

                        double[] X = new double[oneRead];
                        double[] Y = new double[oneRead];
                        double[] Z = new double[oneRead];
                        double[] RangeX = new double[oneRead];
                        double[] RangeY = new double[oneRead];

                        for (int i = 0; i < oneRead; i++)
                            X[i] = ReaderBinary.ReadDouble();

                        for (int i = 0; i < oneRead; i++)
                            Y[i] = ReaderBinary.ReadDouble();

                        for (int i = 0; i < oneRead; i++)
                            Z[i] = ReaderBinary.ReadDouble();

                        for (int i = 0; i < oneRead; i++)
                            RangeX[i] = ReaderBinary.ReadDouble();

                        for (int i = 0; i < oneRead; i++)
                            RangeY[i] = ReaderBinary.ReadDouble();

                        Cl3DModel.Cl3DModelPointIterator[,] tabOfElements = new Cl3DModel.Cl3DModelPointIterator[nCols, nRows];

                        for (int i = 0; i < oneRead; i++)
                        {
                            if (X[i] != zMin && Y[i] != zMin && Z[i] != zMin && RangeX[i] != zMin && RangeY[i] != zMin)
                            {
                                int rangeX = (int)(RangeX[i] * nCols);
                                int rangeY = (int)(RangeY[i] * nRows);
                                if (tabOfElements[rangeX, rangeY] == null)
                                    tabOfElements[rangeX, rangeY] = p_mModel3D.AddPointToModel((float)X[i], (float)Y[i], (float)Z[i], rangeX, rangeY);
                                else
                                    throw new Exception("The element is set to current position");
                            }
                        }

                        for (int x = 0; x < nCols; x++)
                        {
                            for (int y = 0; y < nRows; y++)
                            {
                                if (tabOfElements[x, y] == null)
                                    continue;

                                if (x - 1 >= 0 && y + 1 < nRows && tabOfElements[x - 1, y + 1] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x - 1, y + 1]);

                                if (x - 1 >= 0 && tabOfElements[x - 1, y] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x - 1, y]);

                                if (x - 1 >= 0 && y - 1 >= 0 && tabOfElements[x - 1, y - 1] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x - 1, y - 1]);

                                if (y - 1 >= 0 && tabOfElements[x, y - 1] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x, y - 1]);

                                if (x + 1 < nCols && y - 1 >= 0 && tabOfElements[x + 1, y - 1] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x + 1, y - 1]);

                                if (x + 1 < nCols && tabOfElements[x + 1, y] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x + 1, y]);

                                if (x + 1 < nCols && y + 1 < nRows && tabOfElements[x + 1, y + 1] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x + 1, y + 1]);

                                if (y + 1 < nRows && tabOfElements[x, y + 1] != null)
                                    tabOfElements[x, y].AddNeighbor(tabOfElements[x, y + 1]);
                            }
                        }

                    }
                    catch (System.IO.EndOfStreamException)
                    {
                    }
                    fs.Close();
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
