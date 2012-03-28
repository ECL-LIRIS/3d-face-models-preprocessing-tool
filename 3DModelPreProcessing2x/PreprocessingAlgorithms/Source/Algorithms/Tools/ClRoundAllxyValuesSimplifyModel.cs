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
*   @file       RoundAllxyValuesSimplifyModel.cs
*   @brief      RoundAllxyValuesSimplifyModel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRoundAllxyValuesSimplifyModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRoundAllxyValuesSimplifyModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Round XY Values (Simlify Model)";

        public ClRoundAllxyValuesSimplifyModel() : base(ALGORITHM_NAME) { }

        bool m_bMean = true;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Mean Value"))
            {
                m_bMean = bool.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Mean Value", m_bMean.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            int maxX = (int)iter.X;
            int minX = (int)iter.X;
            int maxY = (int)iter.Y;
            int minY = (int)iter.Y;

            do
            {
                if (maxX < iter.X)
                    maxX = (int)iter.X;
                if (maxY < iter.Y)
                    maxY = (int)iter.Y;

                if (minX > iter.X)
                    minX = (int)iter.X;
                if (minY > iter.Y)
                    minY = (int)iter.Y;
            } while (iter.MoveToNext());

            int width = (maxX - minX) + 1;
            int height = (maxY - minY) + 1;

            List<Cl3DModel.Cl3DModelPointIterator>[,] map = new List<Cl3DModel.Cl3DModelPointIterator>[width, height];

            iter = p_Model.GetIterator();
            do
            {
                int x = (int)iter.X - minX;
                int y = (int)iter.Y - minY;
                if(map[x,y] == null)
                    map[x,y] = new List<Cl3DModel.Cl3DModelPointIterator>();

                map[x,y].Add(iter.CopyIterator());
            } while (iter.MoveToNext());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] != null)
                    {
                        int MeanR = 0;
                        int MeanG = 0;
                        int MeanB = 0;
                        float MeanX = 0;
                        float MeanY = 0;
                        float MeanZ = 0;

                        float MeanU = 0;
                        float MeanV = 0;

                        float MeanRangeX = 0;
                        float MeanRangeY = 0;

                        Dictionary<string, List<double>> MeanSpecificValues = new Dictionary<string, List<double>>();

                        Cl3DModel.Cl3DModelPointIterator MaxPoint = null;
                        
                        string specPoint = Cl3DModel.eSpecificPoints.UnspecifiedPoint.ToString();
                        bool isSpecific = false;
                        foreach (Cl3DModel.Cl3DModelPointIterator point in map[x, y])
                        {
                            point.AlreadyVisited = true;
                            if(!isSpecific)
                                isSpecific = p_Model.IsThisPointInSpecificPoints(point, ref specPoint);

                            if (m_bMean)
                            {
                                MeanR += point.ColorR;
                                MeanG += point.ColorG;
                                MeanB += point.ColorB;
                                MeanX += point.X;
                                MeanY += point.Y;
                                MeanZ += point.Z;

                                MeanU += point.U;
                                MeanV += point.V;

                                MeanRangeX += point.RangeImageX;
                                MeanRangeY += point.RangeImageY;

                                List<string> listOfValuesNames = point.GetListOfSpecificValues();
                                foreach(string name in listOfValuesNames)
                                {
                                    List<double> listOfValues = null;
                                    if (!MeanSpecificValues.TryGetValue(name, out listOfValues))
                                    {
                                        listOfValues = new List<double>();
                                        MeanSpecificValues.Add(name, listOfValues);
                                    }
                                    double val = point.GetSpecificValue(name);
                                    listOfValues.Add(val);
                                }
                            }
                            else
                            {// search for max Z
                                if (MaxPoint == null)
                                    MaxPoint = point;
                                else if(MaxPoint.Z < point.Z)
                                    MaxPoint = point;
                            }
                        }

                        if (m_bMean)
                        {
                            MeanR /= map[x, y].Count;
                            MeanG /= map[x, y].Count;
                            MeanB /= map[x, y].Count;
                            MeanX /= map[x, y].Count;
                            MeanY /= map[x, y].Count;
                            MeanZ /= map[x, y].Count;

                            MeanU /= map[x, y].Count;
                            MeanV /= map[x, y].Count;

                            MeanRangeX /= map[x, y].Count;
                            MeanRangeY /= map[x, y].Count;
                        }
                        else
                        {
                            MeanR = MaxPoint.ColorR;
                            MeanG = MaxPoint.ColorG;
                            MeanB = MaxPoint.ColorB;
                            MeanX = MaxPoint.X;
                            MeanY = MaxPoint.Y;
                            MeanZ = MaxPoint.Z;

                            MeanU = MaxPoint.U;
                            MeanV = MaxPoint.V;

                            MeanRangeX = MaxPoint.RangeImageX;
                            MeanRangeY = MaxPoint.RangeImageY;
                        }

                        List<Cl3DModel.Cl3DModelPointIterator> connections = new List<Cl3DModel.Cl3DModelPointIterator>();

                        foreach (Cl3DModel.Cl3DModelPointIterator point in map[x, y])
                            foreach (Cl3DModel.Cl3DModelPointIterator InnerPoint in point.GetListOfNeighbors())
                                if(InnerPoint.AlreadyVisited == false)
                                    connections.Add(InnerPoint);

                        Cl3DModel.Cl3DModelPointIterator pt = p_Model.AddPointToModel((int)MeanX, (int)MeanY, MeanZ, x, width - y);
                        pt.Color = Color.FromArgb(MeanR, MeanG, MeanB);
                        pt.U = MeanU;
                        pt.V = MeanV;

                        foreach (KeyValuePair<string, List<double>> kvValues in MeanSpecificValues)
                        {
                            string valueName = kvValues.Key;
                            List<double> values = kvValues.Value;
                            double meanVal = 0;
                            foreach (double val in values)
                            {
                                meanVal += val;
                            }
                            meanVal /= values.Count;
                            pt.AddSpecificValue("Mean_" + valueName, meanVal);
                        }
                        

                        if (isSpecific)
                            p_Model.AddSpecificPoint(specPoint, pt);

                        List<Cl3DModel.Cl3DModelPointIterator> lista =  map[x, y];
                        for (int i = 0; i < lista.Count; i++)
                        {
                            p_Model.RemovePointFromModel(lista[i]);
                        }

                        foreach (Cl3DModel.Cl3DModelPointIterator point in connections)
                            pt.AddNeighbor(point);
                    }
                }
            }
        }
    }
}
