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
using Iridium.Numerics.LinearAlgebra;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClCreateRegularGrid.cs
*   @brief      Algorithm to ClCalculateNormalVectors
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-12-2008
*
*   @history
*   @item		10-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCreateRegularGrid : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCreateRegularGrid();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Create 2.5D regular grid";

        public ClCreateRegularGrid() : base(ALGORITHM_NAME) { }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            List<Cl3DModel.Cl3DModelPointIterator>[,] Map = null;

            int width = 0;
            int heinght = 0;
            float MinusXoffset = 0.0f;
            float MinusYoffset = 0.0f;

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                foreach (Cl3DModel.Cl3DModelPointIterator neighbor in iter.GetListOfNeighbors())
                    iter.RemoveNeighbor(neighbor);

            } while (iter.MoveToNext());

            ClTools.CreateGridBasedOnRealXY(p_Model, out Map, out width, out heinght, out MinusXoffset, out MinusYoffset);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < heinght; y++)
                {
                    if (Map[x, y] != null)
                    {
                        float MeanValue = ClTools.CalculateMeanValue(Map[x, y]);

                        Cl3DModel.Cl3DModelPointIterator Point = Map[x, y][0];

                        foreach (Cl3DModel.Cl3DModelPointIterator pt in Map[x, y])
                        {
                            string label = "";
                            if (pt.IsLabeled(out label))
                            {
                                Point = pt;
                                break;
                            }
                        }
                        foreach (Cl3DModel.Cl3DModelPointIterator pt in Map[x, y])
                        {
                            if (pt.PointID != Point.PointID)
                            {
                                p_Model.RemovePointFromModel(pt);
                       //         Map[x, y].Remove(pt);
                            }
                        }

                        Point.RangeImageX = x;
                        Point.RangeImageY = y;
                        Point.Z = MeanValue;
                        Point.X = x + MinusXoffset;
                        Point.Y = y + MinusYoffset;
                    }

                }
            }



            

        }
    }
}
