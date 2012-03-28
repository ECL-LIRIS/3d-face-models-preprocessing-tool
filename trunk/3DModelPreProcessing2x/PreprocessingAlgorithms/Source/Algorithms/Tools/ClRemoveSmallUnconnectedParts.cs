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
*   @file       ClRemoveSmallUnconnectedParts.cs
*   @brief      Algorithm to Remove Small Unconnected Parts
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       01-07-2008
*
*   @history
*   @item		01-07-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveSmallUnconnectedParts : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveSmallUnconnectedParts();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove small unconnected parts";

        public ClRemoveSmallUnconnectedParts() : base(ALGORITHM_NAME) { }


        private bool m_bColorIt = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Color It"))
            {
                if (p_sValue.Equals("True"))
                    m_bColorIt = true;
                else if (p_sValue.Equals("False"))
                    m_bColorIt = false;
                else
                    throw new Exception("Unknown value: " + p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Color It", m_bColorIt.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }



        private Cl3DModel.Cl3DModelPointIterator findNextUnseenElement(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (iter.IsValid())
            {
                while (iter.AlreadyVisited)
                {
                    if (!iter.MoveToNext())
                        return null;
                }
                return iter;
            }
            else
                return null;
        }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator tmpIter = p_Model.GetIterator();

            while (tmpIter.IsValid())
            {
                if (tmpIter.GetListOfNeighbors().Count <= 1)
                    tmpIter = p_Model.RemovePointFromModel(tmpIter);
                else
                {
                    if (!tmpIter.MoveToNext())
                        break;
                }
            }

            List<List<Cl3DModel.Cl3DModelPointIterator>> listOfRegions = new List<List<Cl3DModel.Cl3DModelPointIterator>>();

            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<Cl3DModel.Cl3DModelPointIterator> newListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            Cl3DModel.Cl3DModelPointIterator iter = null;
            int listNo = 0;

            while ((iter = findNextUnseenElement(ref p_Model)) != null)
            {
                ListToCheck.Add(iter);
                listOfRegions.Add(new List<Cl3DModel.Cl3DModelPointIterator>());
                do
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator ElementFromListToCheck in ListToCheck)
                    {
                        if (!ElementFromListToCheck.AlreadyVisited)
                        {
                            listOfRegions[listNo].Add(ElementFromListToCheck);
                            ElementFromListToCheck.AlreadyVisited = true;
                            foreach (Cl3DModel.Cl3DModelPointIterator it in ElementFromListToCheck.GetListOfNeighbors())
                            {
                                if(!it.AlreadyVisited)
                                    newListToCheck.Add(it);
                            }
                        }
                    }
                    
                    ListToCheck.Clear();
                    foreach (Cl3DModel.Cl3DModelPointIterator it in newListToCheck)
                        ListToCheck.Add(it);
                    newListToCheck.Clear();

                } while (ListToCheck.Count != 0);
                ListToCheck.Clear();
                listNo++;
            }

            if (listOfRegions.Count <= 1)
                return;

            int maxCount = listOfRegions[0].Count; ;
            int maxI = 0;
            for (int i = 1; i < listOfRegions.Count; i++)
            {
                if (listOfRegions[i].Count > maxCount)
                {
                    maxI = i;
                    maxCount = listOfRegions[i].Count;
                }
            }

            if (m_bColorIt)
            {
                for (int i = 0; i < listOfRegions.Count; i++ )
                {
                    if (i == maxI)
                        continue;

                    for (int j = 0; j < listOfRegions[i].Count; j++)
                    {
                        listOfRegions[i][j].Color = ClTools.GetColorRGB(((float)i) / listOfRegions.Count, 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listOfRegions.Count; i++)
                {
                    if (i == maxI)
                        continue;

                    for (int j = 0; j < listOfRegions[i].Count; j++)
                    {
                        p_Model.RemovePointFromModel(listOfRegions[i][j]);
                    }
                }
            }
        }

    }
}
