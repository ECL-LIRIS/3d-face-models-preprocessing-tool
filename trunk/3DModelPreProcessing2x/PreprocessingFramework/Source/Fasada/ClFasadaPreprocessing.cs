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
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClFasadaPreprocessing.cs
*   @brief      Fasade created to facilitate access to processing, hadles threads managing
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       26-10-2007
*
*   @history
*   @item		26-10-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace PreprocessingFramework
{
    public class ClFasadaPreprocessing
    {
        private static Cl3DModel m_sProcessedModel = null;
        private static List<IFaceAlgorithm> m_lAlgorithmsList = new List<IFaceAlgorithm>();
        private static List<String> m_lFilesList = new List<string>();
        
        private static Thread m_sProcessingThread = null;
        private static bool m_bIsRunning = false;
        private static bool m_bLogging = false;

        private static string m_sTestDirectory = @"c:\";

        //------------------------------------------------------

        static public Cl3DModel GetProcessedModel()
        {
            return m_sProcessedModel;
        }

        static public bool IsPreProcessing()
        {
            return m_bIsRunning;
        }

        static private void RunAlgorithmsForFolder()
        {
            if (m_bIsRunning)
                return;

            m_bIsRunning = true;

            ClFileLogger FileLogger = new ClFileLogger(m_sTestDirectory + "log.txt", false);
            if (m_bLogging)
            {
                ClInformationSender.RegisterReceiver(FileLogger, ClInformationSender.eInformationType.eDebugText);
                ClInformationSender.RegisterReceiver(FileLogger, ClInformationSender.eInformationType.eTextExternal);
                ClInformationSender.RegisterReceiver(FileLogger, ClInformationSender.eInformationType.eTextInternal);
                ClInformationSender.RegisterReceiver(FileLogger, ClInformationSender.eInformationType.eError);
            }
            try
            {
                ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStartProcessing);

                int current = 1;
                int howMany = m_lFilesList.Count;

                ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStartProcessing);

                string fullAlgorithmAdditionalData = "";
                foreach (IFaceAlgorithm alg in m_lAlgorithmsList)
                {

                    fullAlgorithmAdditionalData += "\n- " + alg.GetAlgorithmFullPath();
                    fullAlgorithmAdditionalData += "\n";
                    foreach (KeyValuePair<string, string> prop in alg.GetProperitis())
                        fullAlgorithmAdditionalData += "\t" + prop.Key + " -> " + prop.Value + "\n";
                }
                fullAlgorithmAdditionalData += "\n------------------------------";
                ClInformationSender.SendInformation(fullAlgorithmAdditionalData, ClInformationSender.eInformationType.eDebugText);

                TimeSpan mean = TimeSpan.Zero;
                foreach (String file in m_lFilesList)
                {
                    DateTime start = DateTime.Now;
                    ClInformationSender.SendInformation(((current * 100) / howMany).ToString(), ClInformationSender.eInformationType.eProgress);
                    try
                    {
                        Cl3DModel newModel = new Cl3DModel();

                        newModel.LoadModel(file);
                        
                        foreach (IFaceAlgorithm alg in m_lAlgorithmsList)
                        {
                            alg.MakeAlgorithm(ref newModel);
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        ClInformationSender.SendInformation("Processing Interrupted", ClInformationSender.eInformationType.eTextInternal);
                        break;
                    }
                    catch (Exception ex)
                    {
                        ClInformationSender.SendInformation("ERROR:\n(" + file + ")\n" + ex.ToString(), ClInformationSender.eInformationType.eDebugText);
                    }
                    DateTime stop = DateTime.Now;
                    mean += (stop - start);
                    double MeanTotalHours = mean.TotalHours;
                    double MeanTotalMin = mean.TotalMinutes;

                    int TotalTimeHours = (int)((MeanTotalHours / current) * (howMany - current));
                    int TotalTimeMin = (int)((MeanTotalMin / current) * (howMany - current));

                    TotalTimeMin = TotalTimeMin - (TotalTimeHours * 60);

                    ClInformationSender.SendInformation("Time Remaining: " + TotalTimeHours.ToString() + "h" + TotalTimeMin.ToString("00")+"min [" + current.ToString() + "/" + howMany.ToString()+"]", ClInformationSender.eInformationType.eTextExternal);                    
                    current++;
                }
            }
            catch (Exception e)
            {
                ClInformationSender.SendInformation("ERROR:\n" + e.ToString(), ClInformationSender.eInformationType.eError);
            }

            m_bIsRunning = false;
            ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStopProcessing);

            ClInformationSender.UnRegisterReceiver(FileLogger, ClInformationSender.eInformationType.eDebugText);
            ClInformationSender.UnRegisterReceiver(FileLogger, ClInformationSender.eInformationType.eTextExternal);
            ClInformationSender.UnRegisterReceiver(FileLogger, ClInformationSender.eInformationType.eTextInternal);
            ClInformationSender.UnRegisterReceiver(FileLogger, ClInformationSender.eInformationType.eError);
        }
        static private void RunAlgorithmsForModel()
        {
            try
            {
                if (m_bIsRunning)
                    return;

                ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStartProcessing);

                m_bIsRunning = true;
                DateTime start = DateTime.Now;
                foreach (IFaceAlgorithm alg in m_lAlgorithmsList)
                {
                    alg.MakeAlgorithm(ref m_sProcessedModel);
                }
                DateTime stop = DateTime.Now;

                ClInformationSender.SendInformation("DONE in: " + (stop - start).TotalSeconds + " Sec.", ClInformationSender.eInformationType.eTextInternal);

                string algTimes = "";
                foreach (IFaceAlgorithm alg in m_lAlgorithmsList)
                {
                    algTimes += alg.GetAlgorithmFullPath() + " time[ms]:\t" + alg.GetComputationTime().TotalMilliseconds + "\n";
                }

                ClInformationSender.SendInformation(algTimes, ClInformationSender.eInformationType.eDebugText);
                ClInformationSender.SendInformation(algTimes, ClInformationSender.eInformationType.eStopProcessing);
                m_bIsRunning = false;
            }
            catch (ThreadAbortException)
            {
                ClInformationSender.SendInformation("Processing Interrupted", ClInformationSender.eInformationType.eTextInternal);
                ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStopProcessing);
                m_bIsRunning = false;
            }
            catch (Exception e)
            {
                ClInformationSender.SendInformation("ERROR:\n" + e.ToString(), ClInformationSender.eInformationType.eError);
                ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStopProcessing);
                m_bIsRunning = false;
            }
        }
       
        static public void RunAlgorithms(string p_sFacesFolder, List<IFaceAlgorithm> p_lListOfAlgorithms, List<string> p_lFileTypes, bool p_bLogging, bool p_bSubfolders)
        {
            if (m_bIsRunning)
                throw new Exception("The previous processing is still running...");

            m_lAlgorithmsList = p_lListOfAlgorithms;
            m_bLogging = p_bLogging;

            m_sTestDirectory = p_sFacesFolder + "\\";

            DirectoryInfo di = new DirectoryInfo(p_sFacesFolder);
            m_lFilesList.Clear();

            foreach (string ext in p_lFileTypes)
            {
                FileInfo[] files = di.GetFiles("*." + ext);
                foreach (FileInfo fi in files)
                    m_lFilesList.Add(fi.FullName);
            }

            if (p_bSubfolders)
            {
                List<DirectoryInfo[]> FoldersToCheck = new List<DirectoryInfo[]>();
                List<DirectoryInfo[]> FoldersToCheckTmp = new List<DirectoryInfo[]>();
                FoldersToCheckTmp.Add(di.GetDirectories());

                while (FoldersToCheckTmp.Count != 0)
                {
                    FoldersToCheck = FoldersToCheckTmp;
                    FoldersToCheckTmp = new List<DirectoryInfo[]>();

                    foreach (DirectoryInfo[] Directories in FoldersToCheck)
                    {
                        foreach (DirectoryInfo dir in Directories)
                        {
                            FoldersToCheckTmp.Add(dir.GetDirectories());
                            foreach (string ext in p_lFileTypes)
                            {
                                FileInfo[] files = dir.GetFiles("*." + ext);
                                foreach (FileInfo fi in files)
                                    m_lFilesList.Add(fi.FullName);
                            }
                        }
                    }
                }
            }
            
            if (m_bLogging)
            {
                DateTime data = DateTime.Now;
                string Day = data.Day.ToString();
                string Month = data.Month.ToString();
                string Year = data.Year.ToString();
                string Hour = data.Hour.ToString();
                string Minute = data.Minute.ToString();
                string Second = data.Second.ToString();

                if (Day.Length < 2)
                    Day = "0" + Day;
                if (Month.Length < 2)
                    Month = "0" + Month;
                if (Hour.Length < 2)
                    Hour = "0" + Hour;
                if (Minute.Length < 2)
                    Minute = "0" + Minute;
                if (Second.Length < 2)
                    Second = "0" + Second;

                m_sTestDirectory += "Test_" + Day + "_" + Month + "_" + Year + " " + Hour + "_" + Minute + "_" + Second + "\\";
                if (!Directory.Exists(m_sTestDirectory))
                    Directory.CreateDirectory(m_sTestDirectory);
            }

            m_sProcessingThread = new Thread(RunAlgorithmsForFolder);
            m_sProcessingThread.Priority = ThreadPriority.Highest;
            m_sProcessingThread.Start();

        }
        static public void RunAlgorithms(Cl3DModel p_mModel3D, List<IFaceAlgorithm> p_lListOfAlgorithms)
        {
            if (m_bIsRunning)
                throw new Exception("The previous processing is still running ...");

            m_lAlgorithmsList = p_lListOfAlgorithms;

            if (p_mModel3D != null && p_lListOfAlgorithms != null)
            {
                m_sProcessedModel = p_mModel3D;
                m_sProcessingThread = new Thread(RunAlgorithmsForModel);
                m_sProcessingThread.Priority = ThreadPriority.Highest;
                m_sProcessingThread.Start();
            }
        }

        static public void ResetFasade()
        {
            m_sProcessedModel = null;
            m_bIsRunning = false;
            m_lAlgorithmsList = null;
            m_lFilesList.Clear();
            m_sTestDirectory = @"c:\";

            if (m_sProcessingThread != null)
            {
                if (m_sProcessingThread.IsAlive)
                    m_sProcessingThread.Abort();
                while (m_sProcessingThread.ThreadState != ThreadState.AbortRequested &&
                        m_sProcessingThread.ThreadState != ThreadState.Stopped)
                { }
                m_sProcessingThread = null;
                ClInformationSender.SendInformation(null, ClInformationSender.eInformationType.eStopProcessing);
            }
        }

        static public List<IFaceAlgorithm> GetAlgorithms()
        {
            return m_lAlgorithmsList;
        }
    }
}
