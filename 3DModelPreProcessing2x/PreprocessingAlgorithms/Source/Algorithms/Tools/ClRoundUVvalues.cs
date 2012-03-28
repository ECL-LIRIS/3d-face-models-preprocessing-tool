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
    public class ClRoundUVvalues : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRoundUVvalues();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Round UV Values (Simlify Parametrization)";

        public ClRoundUVvalues() : base(ALGORITHM_NAME) { }

        private int m_Width = 50;
        private int m_Height = 50;
        private bool m_CreateBmp = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Width"))
            {
                m_Width = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Height"))
            {
                m_Height = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Create Bitmap"))
            {
                m_CreateBmp = bool.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Width", m_Width.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Height", m_Height.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Create Bitmap", m_CreateBmp.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            
            return list;
        }

        private void SampleUVParametriztion(Cl3DModel p_Model, out Cl3DModel.Cl3DModelPointIterator[,] SampledModel, int MatrixWidth, int MatrixHeight)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            float maxU = iter.U;
            float minU = iter.U;
            float maxV = iter.V;
            float minV = iter.V;

            do
            {
                if (maxU < iter.U)
                    maxU = iter.U;
                if (maxV < iter.V)
                    maxV = iter.V;

                if (minU > iter.U)
                    minU = iter.U;
                if (minV > iter.V)
                    minV = iter.V;
            } while (iter.MoveToNext());

            List<Cl3DModel.Cl3DModelPointIterator>[,] SampledModelList = new List<Cl3DModel.Cl3DModelPointIterator>[MatrixWidth,MatrixHeight];
            SampledModel = new Cl3DModel.Cl3DModelPointIterator[MatrixWidth, MatrixHeight];

            iter = p_Model.GetIterator();
            do
            {
                float U = iter.U - minU;
                float V = iter.V - minV;

                int RoundU = (int)((U / (maxU - minU)) * (MatrixWidth - 1));
                int RoundV = (int)((V / (maxV - minV)) * (MatrixHeight - 1));

                if(SampledModelList[RoundU, RoundV] == null)
                    SampledModelList[RoundU, RoundV] = new List<Cl3DModel.Cl3DModelPointIterator>();

                SampledModelList[RoundU, RoundV].Add(iter.CopyIterator());

           } while (iter.MoveToNext());

            for(int i=0; i< MatrixWidth; i++)
            {
                for(int j=0; j<MatrixHeight; j++)
                {
                    if(SampledModelList[i,j]!= null)
                    {
                        float x = 0;
                        float y = 0;
                        float z = 0;
                        float u = 0;
                        float v = 0;
                        int ColorR = 0;
                        int ColorG = 0;
                        int ColorB = 0;
                        string specPoint = Cl3DModel.eSpecificPoints.UnspecifiedPoint.ToString();
                        bool isSpecific = false;

                        Dictionary<string, double> SumOfSpecificValues = new Dictionary<string, double>();
                        foreach(Cl3DModel.Cl3DModelPointIterator pts in SampledModelList[i,j])
                        {
                            pts.AlreadyVisited = true;
                            if (!isSpecific)
                                isSpecific = p_Model.IsThisPointInSpecificPoints(pts, ref specPoint);

                            List<string> specValues = pts.GetListOfSpecificValues();
                            foreach (string valueName in specValues)
                            {
                                double currentVal = 0;
                                pts.GetSpecificValue(valueName, out currentVal);
                                double countedVal = 0;
                                SumOfSpecificValues.TryGetValue(valueName, out countedVal);

                                SumOfSpecificValues.Remove(valueName);
                                SumOfSpecificValues.Add(valueName, countedVal + currentVal);
                            }
                            x += pts.X;
                            y += pts.Y;
                            z += pts.Z;
                            u += pts.U;
                            v += pts.V;
                            ColorR += pts.ColorR;
                            ColorG += pts.ColorG;
                            ColorB += pts.ColorB;
                        }

                        int Count = SampledModelList[i, j].Count;

                        List<Cl3DModel.Cl3DModelPointIterator> connections = new List<Cl3DModel.Cl3DModelPointIterator>();

                        foreach (Cl3DModel.Cl3DModelPointIterator point in SampledModelList[i, j])
                            foreach (Cl3DModel.Cl3DModelPointIterator InnerPoint in point.GetListOfNeighbors())
                                if (InnerPoint.AlreadyVisited == false)
                                    connections.Add(InnerPoint);

                        Cl3DModel.Cl3DModelPointIterator pt = p_Model.AddPointToModel(x / Count,
                                                                                        y / Count,
                                                                                        z / Count);
                        pt.Color = Color.FromArgb(ColorR / Count,
                                                    ColorG / Count,
                                                    ColorB / Count);

                        pt.U = (int)((u / SampledModelList[i, j].Count) / maxU * MatrixWidth);
                        pt.V = (int)((v / SampledModelList[i, j].Count) / maxV * MatrixHeight);

                        foreach (KeyValuePair<string, double> SpecVal in SumOfSpecificValues)
                            pt.AddSpecificValue(SpecVal.Key, SpecVal.Value / Count);

                        SampledModel[i, j] = pt;

                        if (isSpecific)
                            p_Model.AddSpecificPoint(specPoint, pt);

                        foreach(Cl3DModel.Cl3DModelPointIterator point in SampledModelList[i, j])
                            p_Model.RemovePointFromModel(point);

                        foreach (Cl3DModel.Cl3DModelPointIterator point in connections)
                            pt.AddNeighbor(point);
                                                
                    }
                }
            }
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator[,] SampledModel;
            SampleUVParametriztion(p_Model, out SampledModel, m_Width, m_Height);
           
            if (m_CreateBmp)
            {
                Bitmap image = new Bitmap(m_Width, m_Height);

                for (int i = 0; i < m_Width; i++)
                    for (int j = 0; j < m_Height; j++)
                        if (SampledModel[i, j] != null)
                            image.SetPixel(i, j, SampledModel[i, j].Color);

                image.Save(p_Model.ModelFilePath + "_Image.bmp");
            }
        }
    }
}
