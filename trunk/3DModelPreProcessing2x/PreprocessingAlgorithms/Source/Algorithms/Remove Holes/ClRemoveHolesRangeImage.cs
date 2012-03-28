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
*   @file       ClRemoveHolesRangeImage.cs
*   @brief      Show ClRemoveHolesRangeImage
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       27-03-2009
*
*   @history
*   @item		27-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveHolesRangeImage : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveHolesRangeImage();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Remove Holes\Remove Holes (Interpolation RangeImage)";

        public ClRemoveHolesRangeImage() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            return list;
        }




        // mu parameter = (float)(ix - beginHole) / (endHole - beginHole); procent between interpolated points
        private float CubicInterpolate(float y0, float y1, float y2, float y3, float mu)
        {
            float a0, a1, a2, a3, mu2;

            mu2 = mu * mu;
            a0 = y3 - y2 - y0 + y1;
            a1 = y0 - y1 - a0;
            a2 = y2 - y0;
            a3 = y1;

            return (a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3);
        }
        private float LinearInterpolate(float y1, float y2, float mu)
        {
            return (y1 * (1 - mu) + y2 * mu);
        }

        public float InterpolateCubic(int p_iHoleBegin, int p_iHoleEnd, int p_iRow, Cl3DModel.Cl3DModelPointIterator[,] p_map, float p_fProcent, int p_Height, int p_Width)
        {
            float beforeBeginHoleVal = p_map[p_iHoleBegin, p_iRow].Z;
            float afterBeginHoleVal = p_map[p_iHoleEnd, p_iRow].Z;
            if (p_iHoleBegin > 2)
                if (p_map[p_iHoleBegin - 1, p_iRow] != null)
                    beforeBeginHoleVal = p_map[p_iHoleBegin - 1, p_iRow].Z;

            if (p_iHoleEnd < p_Width - 1)
                if (p_map[p_iHoleEnd + 1, p_iRow] != null)
                    afterBeginHoleVal = p_map[p_iHoleEnd + 1, p_iRow].Z;

            return CubicInterpolate(beforeBeginHoleVal,
                                        p_map[p_iHoleBegin, p_iRow].Z,
                                        p_map[p_iHoleEnd, p_iRow].Z,
                                        afterBeginHoleVal,
                                        p_fProcent);
        }
        
        protected override void Algorithm(ref Cl3DModel p_Model)
        {
          //  if (!(p_Model.ModelType == "abs" || p_Model.ModelType == "binaryModel" || p_Model.ModelType == "model" || p_Model.ModelType == "bnt"))
          //      throw new Exception("Remove Holes based on Range Image works only for ABS and BNT like aslo for binaryModel and model files with range image informations");

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Model iterator not Valid");

            int MinX;
            int MaxX;
            int MinY;
            int MaxY;

            MinX = iter.RangeImageX;
            MaxX = iter.RangeImageX;
            MinY = iter.RangeImageY;
            MaxY = iter.RangeImageY;

            do
            {
                if (MinX > iter.RangeImageX)
                    MinX = iter.RangeImageX;
                if (MaxX < iter.RangeImageX)
                    MaxX = iter.RangeImageX;

                if (MinY > iter.RangeImageY)
                    MinY = iter.RangeImageY;
                if (MaxY < iter.RangeImageY)
                    MaxY = iter.RangeImageY;

            } while (iter.MoveToNext());

            int Width = (MaxX - MinX)+1;
            int Height = (MaxY - MinY) +1;
            Cl3DModel.Cl3DModelPointIterator[,] map = new Cl3DModel.Cl3DModelPointIterator[Width, Height];

            iter = p_Model.GetIterator();
            do
            {
                map[iter.RangeImageX-MinX, iter.RangeImageY-MinY] = iter.CopyIterator();
            } while (iter.MoveToNext());


            for (int y = 0; y < Height; y++)
            {
                bool bFoundHole = false;
                bool bFaceStarted = false;
                int iHoleBegin = 0;
                int iHoleEnd = 0;
                for (int x = 0; x < Width; x++)
                {
                    if (map[x, y] == null && bFaceStarted)
                    {
                        // found hole begin
                        iHoleBegin = x - 1;
                        for (int j = x + 1; j < Width; j++) // try to find hole end
                        {
                            if (map[j, y] != null)
                            {
                                iHoleEnd = j;
                                bFoundHole = true;
                                break;
                            }
                        }
                    }
                    else if (bFaceStarted == false && map[x, y] != null)
                        bFaceStarted = true;

                    if (bFoundHole)
                    {
                        for (int ix = iHoleBegin + 1; ix < iHoleEnd; ix++)
                        {
                            float proc = (float)(ix - iHoleBegin) / (iHoleEnd - iHoleBegin);
                            float addZ = InterpolateCubic(iHoleBegin, iHoleEnd, y, map, proc, Height, Width);
                            float addX = LinearInterpolate(map[iHoleBegin, y].X, map[iHoleEnd, y].X, proc);

                            int R = (int)LinearInterpolate(map[iHoleBegin, y].Color.R, map[iHoleEnd, y].Color.R, proc);
                            int G = (int)LinearInterpolate(map[iHoleBegin, y].Color.G, map[iHoleEnd, y].Color.G, proc);
                            int B = (int)LinearInterpolate(map[iHoleBegin, y].Color.B, map[iHoleEnd, y].Color.B, proc);

                            map[ix, y] = p_Model.AddPointToModel(addX, map[iHoleBegin, y].Y, addZ, MinX + ix, MinY + y);
                            map[ix, y].AlreadyVisited = true;
                            map[ix, y].Color = Color.FromArgb(R, G, B);
                          //  map[ix, y].Color = Color.Green;
                            
                            Cl3DModel.Cl3DModelPointIterator testBegin = map[iHoleBegin, y];
                            Cl3DModel.Cl3DModelPointIterator testEnd = map[iHoleEnd, y];

                            if (ix - 1 > 0 && y - 1 > 0 && map[ix - 1, y - 1] != null)
                                map[ix - 1, y -1 ].AddNeighbor(map[ix, y]);

                            if (ix - 1 > 0 && map[ix - 1, y] != null)
                                map[ix - 1, y].AddNeighbor(map[ix, y]);

                            if (ix - 1 > 0 && y + 1 < Height && map[ix - 1, y + 1] != null)
                                map[ix - 1, y + 1].AddNeighbor(map[ix, y]);

                            if (y - 1 > 0 && map[ix, y - 1] != null)
                                map[ix, y - 1].AddNeighbor(map[ix, y]);

                            if (y + 1 < Height && map[ix, y + 1] != null)
                                map[ix, y + 1].AddNeighbor(map[ix, y]);

                            if (ix + 1 < Width && y - 1 > 0 && map[ix + 1, y - 1] != null)
                                map[ix + 1, y - 1].AddNeighbor(map[ix, y]);

                            if (ix + 1 < Width && map[ix + 1, y] != null)
                                map[ix + 1, y].AddNeighbor(map[ix, y]);

                            if (ix + 1 < Width && y + 1 < Height && map[ix + 1, y + 1] != null)
                                map[ix + 1, y + 1].AddNeighbor(map[ix, y]);
                        }
                        bFoundHole = false;
                    }
                }
            }



            for (int x = 0; x < Width; x++)
            {
                bool bFoundHole = false;
                bool bFaceStarted = false;
                int iHoleBegin = 0;
                int iHoleEnd = 0;
                for (int y = 0; y < Height; y++)
                {
                    if ((map[x, y] == null || map[x,y].AlreadyVisited) && bFaceStarted)
                    {
                        // found hole begin
                        iHoleBegin = y - 1;
                        for (int j = y + 1; j < Height; j++) // try to find hole end
                        {
                            if (map[x, j] != null)
                            {
                                if (map[x, j].AlreadyVisited)
                                    continue;

                                iHoleEnd = j;
                                bFoundHole = true;
                                break;
                            }
                        }
                    }
                    else if (bFaceStarted == false && (map[x, y] != null))
                        bFaceStarted = true;

                    if (bFoundHole)
                    {
                        for (int iy = iHoleBegin + 1; iy < iHoleEnd; iy++)
                        {
                            float proc = (float)(iy - iHoleBegin) / (iHoleEnd - iHoleBegin);
                            
                            
                            float beforeBeginHoleVal = map[x, iHoleBegin].Z;
                            float afterBeginHoleVal = map[x, iHoleEnd].Z;
                            if (iHoleBegin > 2)
                                if (map[x,iHoleBegin - 1] != null)
                                    beforeBeginHoleVal = map[x, iHoleBegin - 1].Z;

                            if (iHoleEnd < Height - 1)
                                if (map[x, iHoleEnd + 1] != null)
                                    afterBeginHoleVal = map[x, iHoleEnd + 1].Z;

                           
                            float addZ = CubicInterpolate(beforeBeginHoleVal,
                                        map[x, iHoleBegin].Z,
                                        map[x, iHoleEnd].Z,
                                        afterBeginHoleVal,
                                        proc);

                            if(map[x, iy] == null)
                            {
                                float addY = LinearInterpolate(map[x, iHoleBegin].Y, map[x, iHoleEnd].Y, proc);

                                map[x, iy] = p_Model.AddPointToModel(map[x, iHoleBegin].X, addY, addZ, MinX + x, MinY + iy);
                           //     map[x, iy].Color = Color.Red;

                                int R = (int)LinearInterpolate(map[x, iHoleBegin].Color.R, map[x, iHoleEnd].Color.R, proc);
                                int G = (int)LinearInterpolate(map[x, iHoleBegin].Color.G, map[x, iHoleEnd].Color.G, proc);
                                int B = (int)LinearInterpolate(map[x, iHoleBegin].Color.B, map[x, iHoleEnd].Color.B, proc);

                                map[x, iy].AlreadyVisited = true;
                                map[x, iy].Color = Color.FromArgb(R, G, B);


                                Cl3DModel.Cl3DModelPointIterator testBegin = map[x, iHoleBegin];
                                Cl3DModel.Cl3DModelPointIterator testEnd = map[x, iHoleEnd];

                                if (x - 1 > 0 && iy - 1 > 0 && map[x - 1, iy - 1] != null)
                                    map[x - 1, iy - 1].AddNeighbor(map[x, iy]);

                                if (x - 1 > 0 && map[x - 1, iy] != null)
                                    map[x - 1, iy].AddNeighbor(map[x, iy]);

                                if (x - 1 > 0 && iy + 1 < Height && map[x - 1, iy + 1] != null)
                                    map[x - 1, iy + 1].AddNeighbor(map[x, iy]);

                                if (iy - 1 > 0 && map[x, iy - 1] != null)
                                    map[x, iy - 1].AddNeighbor(map[x, iy]);

                                if (iy + 1 < Height && map[x, iy + 1] != null)
                                    map[x, iy + 1].AddNeighbor(map[x, iy]);

                                if (x + 1 < Width && iy - 1 > 0 && map[x + 1, iy - 1] != null)
                                    map[x + 1, iy - 1].AddNeighbor(map[x, iy]);

                                if (x + 1 < Width && map[x + 1, iy] != null)
                                    map[x + 1, iy].AddNeighbor(map[x, iy]);

                                if (x + 1 < Width && iy + 1 < Height && map[x + 1, iy + 1] != null)
                                    map[x + 1, iy + 1].AddNeighbor(map[x, iy]);
                            }
                            else
                            {
                                map[x, iy].Z = (map[x, iy].Z+addZ)/2;
                       //         map[x, iy].Color = Color.Orange;
                            }
                        }
                        bFoundHole = false;
                    }
                }
            }





        }
    }
}