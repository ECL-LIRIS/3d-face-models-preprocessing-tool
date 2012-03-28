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
    public class ClRemeshModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemeshModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remesh model";

        public ClRemeshModel() : base(ALGORITHM_NAME) { }
        
        protected override void Algorithm(ref Cl3DModel p_Model)
        {
           // if (!(p_Model.ModelType == "abs" || p_Model.ModelType == "binaryModel" || p_Model.ModelType == "model"))
           //     throw new Exception("Remove Holes based on Range Image works only for abs, binaryModel and model files");

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
                foreach (Cl3DModel.Cl3DModelPointIterator pts in iter.GetListOfNeighbors())
                    iter.RemoveNeighbor(pts);
            } while (iter.MoveToNext());

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[x, y] == null)
                        continue;

                    if (x - 1 >= 0 && y + 1 < Height && map[x - 1, y + 1] != null)
                        map[x, y].AddNeighbor(map[x - 1, y + 1]);

                    if (x - 1 >= 0 && map[x - 1, y] != null)
                        map[x, y].AddNeighbor(map[x - 1, y]);

                    if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] != null)
                        map[x, y].AddNeighbor(map[x - 1, y - 1]);

                    if (y - 1 >= 0 && map[x, y - 1] != null)
                        map[x, y].AddNeighbor(map[x, y - 1]);

                    if (x + 1 < Width && y - 1 >= 0 && map[x + 1, y - 1] != null)
                        map[x, y].AddNeighbor(map[x + 1, y - 1]);

                    if (x + 1 < Width && map[x + 1, y] != null)
                        map[x, y].AddNeighbor(map[x + 1, y]);

                    if (x + 1 < Width && y + 1 < Height && map[x + 1, y + 1] != null)
                        map[x, y].AddNeighbor(map[x + 1, y + 1]);

                    if (y + 1 < Height && map[x, y + 1] != null)
                        map[x, y].AddNeighbor(map[x, y + 1]);
                }
            }
        }
    }
}

