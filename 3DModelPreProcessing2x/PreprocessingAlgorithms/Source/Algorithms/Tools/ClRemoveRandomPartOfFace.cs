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
*   @file       ClAddGenericModel.cs
*   @brief      Algorithm to Add Generic Model
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       04-09-2008
*
*   @history
*   @item		04-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveRandomPartOfFace : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveRandomPartOfFace();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove random part of face";

        public ClRemoveRandomPartOfFace() : base(ALGORITHM_NAME) { }

        int m_SphereSize = 20;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Sphere size"))
            {
                m_SphereSize = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }

            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Sphere size", m_SphereSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

           // uint no = (uint)p_Model.ModelPointsCount / 2;

          //  if (!iter.MoveToPoint(no))
          //      throw new Exception("Cannot find point no: " + no.ToString());

            List<Cl3DModel.Cl3DModelPointIterator> PointNeighborhood;
            ClTools.GetNeighborhoodWithGeodesicDistance(out PointNeighborhood, iter, m_SphereSize);
            foreach (Cl3DModel.Cl3DModelPointIterator p in PointNeighborhood)
            {
                p_Model.RemovePointFromModel(p);
            }
            p_Model.RemovePointFromModel(iter);
        }
    }
}
