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
*   @file       ClColorModelDifference.cs
*   @brief      ClColorModelDifference
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-12-2008
*
*   @history
*   @item		16-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClColorModelSpecificValue : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClColorModelSpecificValue();
        }

        public static string ALGORITHM_NAME = @"Show\Color Model Based On Specific Value";

        private enum ColorType
        {
            RGB,
            R,
            G,
            B,
            Gray,
        }
        private bool m_ResetColor = false;
        private ColorType m_Color = ColorType.RGB;
        private bool m_RangeColor = false;
        private bool m_bMaxMinFromNoseAndEyes = false;
        private string m_sMinValuePointName = "Nose";
        private string m_sMaxValuePointName = "Eye";
        private bool m_bMinMaxPreset = false;
        private double m_dMinValue = 0;
        private double m_dMaxValue = 1;

        private Dictionary<string, KeyValuePair<double, double>> SavedMinMaxForSubject = new Dictionary<string, KeyValuePair<double, double>>();

        public ClColorModelSpecificValue() : base(ALGORITHM_NAME) { }
        public ClColorModelSpecificValue(bool p_ResetColor)
            : base(ALGORITHM_NAME) 
        {
            m_ResetColor = p_ResetColor;
        }

        public ClColorModelSpecificValue(double p_MinValue, double p_MaxValue)
            : base(ALGORITHM_NAME)
        {
            m_dMinValue = p_MinValue;
            m_dMaxValue = p_MaxValue;
            m_bMinMaxPreset = true;
        }

        private string m_SpecificValue = "ShapeIndex_25";

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if(p_sProperity.Equals("Reset Color"))
            {
                m_ResetColor = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Color"))
            {
                if (p_sValue.Equals("RGB"))
                    m_Color = ColorType.RGB;
                else if (p_sValue.Equals("R"))
                    m_Color = ColorType.R;
                else if (p_sValue.Equals("G"))
                    m_Color = ColorType.G;
                else if (p_sValue.Equals("B"))
                    m_Color = ColorType.B;
                else if (p_sValue.Equals("Gray"))
                    m_Color = ColorType.Gray;
                else if (p_sValue.Equals("True"))
                    m_Color = ColorType.RGB;
                else if (p_sValue.Equals("False"))
                    m_Color = ColorType.Gray;
                else
                    throw new Exception("Unknown color type");
            }
            else if (p_sProperity.Equals("Specific value"))
            {
                m_SpecificValue = p_sValue;
            }
            else if (p_sProperity.Equals("RangeColor"))
            {
                m_RangeColor = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Max Min from NoseTip and Eyes"))
            {
                m_bMaxMinFromNoseAndEyes = Boolean.Parse(p_sValue);                
            }
            else if (p_sProperity.Equals("Min Value Point Name"))
            {
                if (p_sValue.Equals("Nose"))
                {
                    m_sMinValuePointName = "Nose";
                    m_sMaxValuePointName = "Eye";
                }
                else if (p_sValue.Equals("Eye"))
                {
                    m_sMinValuePointName = "Eye";
                    m_sMaxValuePointName = "Nose";
                }
                else
                    throw new Exception("Only 'Nose' or 'Eye' allowed");
            }
            else if (p_sProperity.Equals("Max Value Point Name"))
            {
                if (p_sValue.Equals("Nose"))
                {
                    m_sMinValuePointName = "Eye";
                    m_sMaxValuePointName = "Nose";
                }
                else if (p_sValue.Equals("Eye"))
                {
                    m_sMinValuePointName = "Nose";
                    m_sMaxValuePointName = "Eye";
                }
                else
                    throw new Exception("Only 'Nose' or 'Eye' allowed");
            }
            else if (p_sProperity.Equals("Min Max Preset"))
            {
                m_bMinMaxPreset = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Min Value"))
            {
                m_dMinValue = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("Max Value"))
            {
                m_dMaxValue = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Reset Color", m_ResetColor.ToString()));
            list.Add(new KeyValuePair<string, string>("Specific value", m_SpecificValue));
            list.Add(new KeyValuePair<string, string>("Color", m_Color.ToString()));
            list.Add(new KeyValuePair<string, string>("RangeColor", m_RangeColor.ToString()));

            list.Add(new KeyValuePair<string, string>("Max Min from NoseTip and Eyes", m_bMaxMinFromNoseAndEyes.ToString()));
            list.Add(new KeyValuePair<string, string>("Min Value Point Name", m_sMinValuePointName.ToString()));
            list.Add(new KeyValuePair<string, string>("Max Value Point Name", m_sMaxValuePointName.ToString()));

            list.Add(new KeyValuePair<string, string>("Min Max Preset", m_bMinMaxPreset.ToString()));
            list.Add(new KeyValuePair<string, string>("Min Value", m_dMinValue.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Max Value", m_dMaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            if (!iter.IsValid())
                throw new Exception("Iter is not valid");

            if (m_ResetColor)
            {
                do
                {
                    iter.Color = Color.White;
                } while (iter.MoveToNext());
                return;
            }

            if (!m_bMinMaxPreset)
            {
                bool firstTime = true;
                do
                {
                    double Value;

                    if (m_RangeColor)
                        Value = iter.Z;
                    else if (!iter.GetSpecificValue(m_SpecificValue, out Value))
                        continue;

                    if (firstTime)
                    {
                        m_dMaxValue = Value;
                        m_dMinValue = Value;
                        firstTime = false;
                        continue;
                    }

                    if (Value > m_dMaxValue)
                        m_dMaxValue = Value;
                    if (Value < m_dMinValue)
                        m_dMinValue = Value;
                } while (iter.MoveToNext());

            }


            if (m_SpecificValue.Contains("ShapeIndex") && !m_RangeColor)
            {
                m_dMinValue = 0;
                m_dMaxValue = 1;
            }


            if (m_bMaxMinFromNoseAndEyes)
            {
                
                Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
                List<Cl3DModel.Cl3DModelPointIterator> NeighborhoodNose = null;
                ClTools.GetNeighborhoodWithGeodesicDistance(out NeighborhoodNose, NoseTip, 10f);

                if (m_sMinValuePointName.Equals("Nose"))
                {
                    m_dMinValue = NoseTip.GetSpecificValue(m_SpecificValue);
                    foreach (Cl3DModel.Cl3DModelPointIterator nb in NeighborhoodNose)
                    {
                        double currVal = nb.GetSpecificValue(m_SpecificValue);
                        if (currVal < m_dMinValue)
                            m_dMinValue = currVal;
                    }
                }
                else if (m_sMaxValuePointName.Equals("Nose"))
                {
                    m_dMaxValue = NoseTip.GetSpecificValue(m_SpecificValue);
                    foreach (Cl3DModel.Cl3DModelPointIterator nb in NeighborhoodNose)
                    {
                        double currVal = nb.GetSpecificValue(m_SpecificValue);
                        if (currVal > m_dMaxValue)
                            m_dMaxValue = currVal;
                    }
                }
                NeighborhoodNose = null;

                Cl3DModel.Cl3DModelPointIterator Eye1 = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
                Cl3DModel.Cl3DModelPointIterator Eye2 = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);
                List<Cl3DModel.Cl3DModelPointIterator> NeighborhoodEye1 = null;
                List<Cl3DModel.Cl3DModelPointIterator> NeighborhoodEye2 = null;
                ClTools.GetNeighborhoodWithGeodesicDistance(out NeighborhoodEye1, Eye1, 10f);
                ClTools.GetNeighborhoodWithGeodesicDistance(out NeighborhoodEye2, Eye2, 10f);

                NeighborhoodEye1.Add(Eye2);
                foreach (Cl3DModel.Cl3DModelPointIterator ne in NeighborhoodEye2)
                    NeighborhoodEye1.Add(ne);


                if (m_sMinValuePointName.Equals("Eye"))
                {
                    m_dMinValue = Eye1.GetSpecificValue(m_SpecificValue);
                    foreach (Cl3DModel.Cl3DModelPointIterator nb in NeighborhoodEye1)
                    {
                        double currVal = nb.GetSpecificValue(m_SpecificValue);
                        if (currVal < m_dMinValue)
                            m_dMinValue = currVal;
                    }
                }
                else if (m_sMaxValuePointName.Equals("Eye"))
                {
                    m_dMaxValue = Eye1.GetSpecificValue(m_SpecificValue);
                    foreach (Cl3DModel.Cl3DModelPointIterator nb in NeighborhoodEye1)
                    {
                        double currVal = nb.GetSpecificValue(m_SpecificValue);
                        if (currVal > m_dMaxValue)
                            m_dMaxValue = currVal;
                    }
                }
             }

            iter = p_Model.GetIterator();
            do
            {
                double Value;
                if (m_RangeColor)
                    Value = iter.Z;
                else if (!iter.GetSpecificValue(m_SpecificValue, out Value))
                    continue;

                
                double divide = 1;
                if (m_dMaxValue != m_dMinValue)
                    divide = (m_dMaxValue - m_dMinValue);

                if(m_Color == ColorType.RGB)
                    iter.Color = ClTools.GetColorRGB((float)((Value - m_dMinValue) / divide), 1.0f);
                else if(m_Color == ColorType.Gray)
                    iter.Color = ClTools.GetColorGray((float)((Value - m_dMinValue) / divide), 1.0f);
                else if (m_Color == ColorType.R)
                {
                    Color PartialColor = ClTools.GetColorRGB((float)((Value - m_dMinValue) / divide), 1.0f);
                    
                    int R = 0; R = (R << 8) + (int)PartialColor.R;
                    int G = 0; G = (G << 8) + (int)PartialColor.G;
                    int B = 0; B = (B << 8) + (int)PartialColor.B;

                    Color newColor = Color.FromArgb((R+G+B)/3, iter.Color.G, iter.Color.B);

                    iter.Color = newColor;
                }
                else if (m_Color == ColorType.G)
                {
                    Color PartialColor = ClTools.GetColorRGB((float)((Value - m_dMinValue) / divide), 1.0f);

                    int R = 0; R = (R << 8) + (int)PartialColor.R;
                    int G = 0; G = (G << 8) + (int)PartialColor.G;
                    int B = 0; B = (B << 8) + (int)PartialColor.B;

                    Color newColor = Color.FromArgb(iter.Color.R, (R + G + B) / 3, iter.Color.B);
                    iter.Color = newColor;
                }
                else if (m_Color == ColorType.B)
                {
                    Color PartialColor = ClTools.GetColorRGB((float)((Value - m_dMinValue) / divide), 1.0f);

                    int R = 0; R = (R << 8) + (int)PartialColor.R;
                    int G = 0; G = (G << 8) + (int)PartialColor.G;
                    int B = 0; B = (B << 8) + (int)PartialColor.B;

                    Color newColor = Color.FromArgb(iter.Color.R, iter.Color.G, (R + G + B) / 3);
                    iter.Color = newColor;
                }
            } while (iter.MoveToNext());
            ClInformationSender.SendInformation(m_SpecificValue.ToString() + " Max: " + m_dMaxValue.ToString() + " Min: " + m_dMinValue.ToString(), ClInformationSender.eInformationType.eColorMapChanged);
        }
    }
}

