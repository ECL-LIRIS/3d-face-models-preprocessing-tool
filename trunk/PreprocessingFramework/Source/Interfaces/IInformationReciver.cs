using System;
using System.Collections.Generic;
using System.Text;

namespace PreprocessingFramework
{
    public interface IInformationReciver
    {
        void NewInformation(string p_sInformation, ClInformationSender.eInformationType p_eType);
    }
}
