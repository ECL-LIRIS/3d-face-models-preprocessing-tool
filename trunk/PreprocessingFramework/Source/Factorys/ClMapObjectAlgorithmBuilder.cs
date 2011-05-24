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
