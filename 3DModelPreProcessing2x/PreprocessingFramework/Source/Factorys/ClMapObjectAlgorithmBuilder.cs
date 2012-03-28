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
using System.Collections.Generic;
using System.Text;

namespace PreprocessingFramework
{

    static public class ClMapObjectAlgorithmBuilder
    {
        public delegate IFaceAlgorithm BuildAlgorithm();

        static private Dictionary<string, BuildAlgorithm> m_CreateMethodsDictionary = new Dictionary<string, BuildAlgorithm>();

        static public void RegisterNewAlgorithm(BuildAlgorithm p_Algorithm, string p_sAlgorithmName)
        {
            m_CreateMethodsDictionary.Add(p_sAlgorithmName, p_Algorithm);
            
            ClInformationSender.SendInformation(p_sAlgorithmName, ClInformationSender.eInformationType.eNewAlgorithm);
        }

        static public IFaceAlgorithm CreateNewAlgorithm(string p_AlgorithmName)
        {
            BuildAlgorithm tmp;
            if (m_CreateMethodsDictionary.TryGetValue(p_AlgorithmName, out tmp))
                return tmp();
            else
                throw new Exception("Cannot find algorithm named: " + p_AlgorithmName);
        }

    }
}
