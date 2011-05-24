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
