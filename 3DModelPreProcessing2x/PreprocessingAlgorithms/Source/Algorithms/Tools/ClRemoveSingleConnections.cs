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

