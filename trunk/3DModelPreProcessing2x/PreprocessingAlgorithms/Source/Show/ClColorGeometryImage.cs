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
*   @file       ClColorMainPointsNeighborhood.cs
*   @brief      Show ClColorMainPointsNeighborhood
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       27-03-2009
*
*   @history
*   @item		27-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClColorGeometryImage : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClColorGeometryImage();
        }

        public static string ALGORITHM_NAME = @"Show\Color Like Geometry Image";

        public ClColorGeometryImage() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            float maxX = iter.X;
            float minX = iter.X;
            float maxY = iter.Y;
            float minY = iter.Y;
            float maxZ = iter.Z;
            float minZ = iter.Z;

            do
            {
                if (maxX < iter.X)
                    maxX = iter.X;
                if (maxY < iter.Y)
                    maxY = iter.Y;
                if (maxZ < iter.Z)
                    maxZ = iter.Z;
                

                if (minX > iter.X)
                    minX = iter.X;
                if (minY > iter.Y)
                    minY = iter.Y;
                if (minZ > iter.Z)
                    minZ = iter.Z;

            } while (iter.MoveToNext());

            iter = p_Model.GetIterator();
            do
            {
                int R = (int)(((iter.X - minX) / (maxX - minX)) * 255);
                int G = (int)(((iter.Y - minY) / (maxY - minY)) * 255);
                int B = (int)(((iter.Z - minZ) / (maxZ - minZ)) * 255);

                iter.Color = Color.FromArgb(R, G, B);
            } while (iter.MoveToNext());
        }
    }
}
