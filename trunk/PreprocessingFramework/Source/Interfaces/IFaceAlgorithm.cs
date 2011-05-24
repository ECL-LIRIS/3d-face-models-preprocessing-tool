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
