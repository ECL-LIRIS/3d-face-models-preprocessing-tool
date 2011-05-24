using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;  // TextWriter, StreamWriter
using System.Threading;

namespace PreprocessingFramework
{
    public class ClFileLogger : IInformationReciver
    {
        private string m_sFileName;
        private static Mutex mut = new Mutex();
        
        private bool m_HardLogging = true;
        private int m_logInterval = 100;
        private int m_logCount = 0;
        private string m_LogMem = "";

        public ClFileLogger()
        {
            m_sFileName = "LOG.txt";
        }
        ~ClFileLogger()
        {
            if (m_LogMem.Length != 0)
            {
                using (TextWriter tw = new StreamWriter(m_sFileName, true))
                {
                    string date = DateTime.Now.ToString();
                    tw.WriteLine(date + "\t" + m_LogMem);
                    tw.Close();
                }
            }
        }

        public ClFileLogger(string p_sFileName, bool p_HardLogging)
        {
            if (File.Exists(p_sFileName))
                File.Delete(p_sFileName);

            m_sFileName = p_sFileName;
            m_HardLogging = p_HardLogging;
        }

        public virtual void NewInformation(string p_sInformation, ClInformationSender.eInformationType p_eType)
        {
            mut.WaitOne();
            try
            {
                if (p_sInformation != null)
                {
                    if (p_eType == ClInformationSender.eInformationType.eDebugText)
                    {
                        p_sInformation = "\t" + p_sInformation;
                        p_sInformation = p_sInformation.Replace("\n", "\n\t");
                    }

                    if (m_HardLogging)
                    {
                        using (TextWriter tw = new StreamWriter(m_sFileName, true))
                        {
                            string date = DateTime.Now.ToString();
                            tw.WriteLine(date + "\t" + p_sInformation);
                            tw.Close();
                        }
                    }
                    else
                    {
                        string date = DateTime.Now.ToString();
                        m_LogMem += date + "\t" + p_sInformation + "\n";
                        m_logCount++;
                        if (m_logCount >= m_logInterval)
                        {
                            using (TextWriter tw = new StreamWriter(m_sFileName, true))
                            {
                                tw.Write(m_LogMem);
                                tw.Close();
                            }
                            m_logCount = 0;
                            m_LogMem = "";
                        }
                    }
                }
            }
            catch (Exception)
            {}

            mut.ReleaseMutex();
        }

        public void LastLog()
        {
            if (m_LogMem.Length != 0)
            {
                using (TextWriter tw = new StreamWriter(m_sFileName, true))
                {
                    string date = DateTime.Now.ToString();
                    tw.WriteLine(date + "\t" + m_LogMem);
                    tw.Close();
                }
            }
        }
    }
}
