﻿using System;
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
*   @file       ClCropFaceBySphere.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCropFaceByGeodesicDistance : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCropFaceByGeodesicDistance();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Crop Face (Geodesic)";

        public ClCropFaceByGeodesicDistance() : base(ALGORITHM_NAME) { }

        public ClCropFaceByGeodesicDistance(float p_fSphereRadious)
            : base(ALGORITHM_NAME)
        {
            m_fDistence = p_fSphereRadious;
        }

        private float m_fDistence = 100f;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Geodesic Distance"))
            {
                m_fDistence = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Geodesic Distance", m_fDistence.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator nose = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString());

            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(nose, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString());

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double distance = 0;
                if (iter.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip, out distance))
                {
                    if (distance > m_fDistence)
                        iter = p_Model.RemovePointFromModel(iter);
                    else
                    {
                        if (!iter.MoveToNext())
                            break;
                    }
                }
                else
                    iter = p_Model.RemovePointFromModel(iter);

            }while(iter.IsValid());
        }
    }
}

