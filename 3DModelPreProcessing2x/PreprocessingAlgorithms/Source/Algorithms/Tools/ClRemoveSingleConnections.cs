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
*   @file       ClRemoveSpikesMedianFilter.cs
*   @brief      Algorithm to remove spikes
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-11-2007
*
*   @history
*   @item		16-11-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveSingleConnections : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveSingleConnections();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove single connections";

        public ClRemoveSingleConnections() : base(ALGORITHM_NAME) { }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            while (iter.IsValid())
            {
                List<Cl3DModel.Cl3DModelPointIterator> neighboors = iter.GetListOfNeighbors();

                foreach (Cl3DModel.Cl3DModelPointIterator neighbor1Ring in neighboors)
                {
                    bool founded = false;
                    foreach (Cl3DModel.Cl3DModelPointIterator neighbor2Ring in neighbor1Ring.GetListOfNeighbors())
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator neighbor3Ring in neighbor2Ring.GetListOfNeighbors())
                        {
                            if (neighbor3Ring.PointID == iter.PointID)
                            {
                                founded = true;
                                break;
                            }
                        }
                    }
                    if (!founded)
                    {
                        iter.RemoveNeighbor(neighbor1Ring);
                    }
                }

                if (!iter.MoveToNext())
                    break;
            }
        }  
    }
}

