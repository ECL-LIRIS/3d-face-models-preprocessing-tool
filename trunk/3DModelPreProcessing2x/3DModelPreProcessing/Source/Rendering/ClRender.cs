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
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClRender.cs
*   @brief      Implementation of class ClRender; Class allows us to easily Render the scene, we need only to register for a rendering and eavents
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       25-10-2007
*
*   @history
*   @item		25-10-2007 Przemyslaw Szeptycki     created at ECL
*/

namespace ModelPreProcessing
{

    class ClRender
    {
        //--------------------- Attributes -----------------
        private Device m_dDevice = null;
        private Control m_cRenderWindow = null;
        private static volatile ClRender m_instance = null;

        private Color m_cBackgroundColor = Color.Black;
        private ClCamera m_rCamera = null;

        private List<CustomVertex.PositionColored> m_lOrigin = new List<CustomVertex.PositionColored>();

        private bool m_bRenderScene = false;
        System.Threading.Thread m_tRenderingThread = null;

        private List<ClBaseRenderObject> m_ListRenderObjects = new List<ClBaseRenderObject>();

        //----------------------- Methods ------------------
        static public ClRender getInstance()
        {
            if (m_instance == null)
                m_instance = new ClRender();

            return m_instance;
        }

        private ClRender()
        {
        }
      /*  private void CreateLights()
        {
            m_dDevice.RenderState.Lighting = false;
            m_dDevice.RenderState.CullMode = Cull.None;

            m_dDevice.Lights[0].Type = LightType.Directional;
            m_dDevice.Lights[0].Diffuse = Color.White;
            m_dDevice.Lights[0].Direction = new Vector3(0, 0, 0);
            m_dDevice.Lights[0].Position = new Vector3(-200, 0, 0);
            m_dDevice.Lights[0].Enabled = true;

            Light light = m_dDevice.Lights[0];

            light.Type = LightType.Spot;
            light.Position = new Vector3(m_fLightPosX, m_fLightPosY, m_fLightPosZ);
            light.Direction = new Vector3(m_fLightDirectionX, m_fLightDirectionY, m_fLightDirectionZ);
            light.InnerConeAngle = 0.5f;
            light.OuterConeAngle = 1.0f;
            light.Diffuse = Color.LightBlue;
            light.Specular = Color.White;
            light.Range = 1000.0f;
            light.Falloff = 1.0f;
            light.Attenuation0 = 1.0f;
            light.Enabled = true;
            //light.Commit();

            m_dDevice.RenderState.Lighting = true;
            m_dDevice.RenderState.DitherEnable = false;
            m_dDevice.RenderState.SpecularEnable = true;
            m_dDevice.RenderState.Ambient = Color.FromArgb(0, 20, 20, 20);
             
        }
    */
        public void CreateDevice(Control p_cRenderWindow)
        {
            try
            {
                m_cRenderWindow = p_cRenderWindow;
                PresentParameters presentParams = new PresentParameters();
                presentParams.Windowed = true;
                presentParams.SwapEffect = SwapEffect.Discard;
                m_dDevice = new Device(0, DeviceType.Hardware, m_cRenderWindow, CreateFlags.SoftwareVertexProcessing | CreateFlags.FpuPreserve, presentParams); // CreateFlags.FpuPreserve not to change double precision to single

                m_dDevice.VertexFormat = CustomVertex.PositionColored.Format;
                //CreateLights();

                m_lOrigin.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Red.ToArgb()));
                m_lOrigin.Add(new CustomVertex.PositionColored(-300, 0, 0, Color.Red.ToArgb()));

                m_lOrigin.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Green.ToArgb()));
                m_lOrigin.Add(new CustomVertex.PositionColored(0, 300, 0, Color.Green.ToArgb()));

                m_lOrigin.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Blue.ToArgb()));
                m_lOrigin.Add(new CustomVertex.PositionColored(0, 0, 300, Color.Blue.ToArgb()));
            }
            catch (Exception)
            {
                m_dDevice = null;
            }
        }

       /* public void ChangeLightPosition(float p_LightPosX, float p_LightPosY, float p_LightPosZ)
        {
            if (m_dDevice.Lights.Count == 0)
                throw new Exception("No lights in device");

            m_dDevice.Lights[0].Position = new Vector3(p_LightPosX, p_LightPosY, p_LightPosZ);
        }

        public void ChangeLightDirection(float p_LightDirectionX, float p_LightDirectionY, float p_LightDirectionZ)
        {
            if (m_dDevice.Lights.Count == 0)
                throw new Exception("No lights in device");

            m_dDevice.Lights[0].Direction = new Vector3(p_LightDirectionX, p_LightDirectionY, p_LightDirectionZ);
        }
        */
        
        public void SetCamera(ClCamera p_RenderObject)
        {
            m_rCamera = p_RenderObject;
        }
        public ClCamera getCamera()
        {
            return m_rCamera;
        }

        public bool IsRendering()
        {
            return m_bRenderScene;
        }
        public void StopRendering()
        {
            if (m_bRenderScene != false)
            {
                m_bRenderScene = false;
                if (m_tRenderingThread != null)
                    while (m_tRenderingThread.IsAlive)
                    {
                        //waiting for the end of the thread;
                    }
                m_tRenderingThread = null;
                ClInformationSender.SendInformation("Rendering OFF", ClInformationSender.eInformationType.eTextExternal);
            }
        }
        public void StartRendering()
        {
            if (m_bRenderScene || m_dDevice == null)
                return;

            m_tRenderingThread = new System.Threading.Thread(this.RenderScene);
            m_bRenderScene = true;
            m_tRenderingThread.Priority = System.Threading.ThreadPriority.Lowest;
            m_tRenderingThread.Start();
            ClInformationSender.SendInformation("Rendering ON", ClInformationSender.eInformationType.eTextExternal);
        }

        public ClBaseRenderObject GetObject(int p_iObjectId)
        {
            for(int i=0;i<m_ListRenderObjects.Count;i++)
            {
                if (m_ListRenderObjects[i].GetObjectID() == p_iObjectId)
                    return m_ListRenderObjects[i];
            }
            return null;
        }

        private void RenderScene()
        {
            try
            {
                while (m_bRenderScene)
                {
                    if (m_dDevice == null)
                    {
                        m_bRenderScene = false;
                        return;
                    }
                    m_dDevice.Clear(ClearFlags.Target, m_cBackgroundColor, 1.0f, 0);

                    m_dDevice.BeginScene();//Lock GPU
                    //Render primitives 
                    if (m_rCamera != null)
                        m_rCamera.Render(m_dDevice, m_cRenderWindow);

                    m_dDevice.DrawUserPrimitives(PrimitiveType.LineList, m_lOrigin.Count / 2, m_lOrigin.ToArray());

                    for (int i = 0; i < m_ListRenderObjects.Count; i++)
                        m_ListRenderObjects[i].Render(m_dDevice, m_cRenderWindow);

                    m_dDevice.EndScene();//Unlock GPU 
                    m_dDevice.Present();//Swap old with new
                }
            }
            catch (Exception ex)
            {
                m_bRenderScene = false;
                ClInformationSender.SendInformation("RENDER: " + ex.ToString(), ClInformationSender.eInformationType.eDebugText);
                return;
            }
        }

        public void AddRenderObj(ClBaseRenderObject p_RenderObject)
        {
            m_ListRenderObjects.Add(p_RenderObject);
        }

        public void DeleteAllRenderObj()
        {
            m_ListRenderObjects.Clear();
        }

        public void SetBackgroundColor(Color p_cBackgroundColor)
        {
            m_cBackgroundColor = p_cBackgroundColor;
        }
        
    }
}
