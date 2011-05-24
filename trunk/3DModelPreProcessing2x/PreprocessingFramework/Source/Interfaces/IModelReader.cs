using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreprocessingFramework
{
    public interface IModelReader
    {
        void ReadModel(Cl3DModel p_mModel3D, string p_sFilePath);

        string GetFileExtension();
    }
}
