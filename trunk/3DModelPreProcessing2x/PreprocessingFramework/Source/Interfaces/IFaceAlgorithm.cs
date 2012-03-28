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
    public interface IFaceAlgorithm
    {
        string GetAlgorithmName();

        string GetAlgorithmFullPath();

        int GetAlgorithmID();

        void MakeAlgorithm(ref Cl3DModel p_Face); // use if You would like to allow changes in the model reference, the model can be switched in the pipeline

        void MakeAlgorithm(Cl3DModel p_Face); // used in case of recognition, model cannot be switched in the pipeline

        void SetProperitis(string p_sProperity, string p_sValue);

        List<KeyValuePair<string, string>> GetProperitis();

        TimeSpan GetComputationTime();
    }
}
