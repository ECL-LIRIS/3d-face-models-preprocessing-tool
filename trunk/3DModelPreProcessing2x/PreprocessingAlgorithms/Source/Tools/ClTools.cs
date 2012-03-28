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
using Iridium.Numerics.LinearAlgebra;

/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClTools.cs
*   @brief      Small algorithms needed many times (common one)
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       29-05-2008
*
*   @history
*   @item		29-05-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    static public class ClTools
    {
        public class MainPoint3D : Cl3DPoint
        {
            public MainPoint3D(float X, float Y, float Z, string Name)
            {
                this.m_X = X;
                this.m_Y = Y;
                this.m_Z = Z;
                this.Name = Name;
            }
            private float m_X;
            private float m_Y;
            private float m_Z;

            public string Name = "";

            public float MinDistance = -1;
            public Cl3DModel.Cl3DModelPointIterator ClosestPoint = null;

            public float X
            {
                get
                {
                    return m_X;
                }
                set
                {
                    m_X = value;
                }
            }

            public float Y
            {
                get
                {
                    return m_Y;
                }
                set
                {
                    m_Y = value;
                }
            }

            public float Z
            {
                get
                {
                    return m_Z;
                }
                set
                {
                    m_Z = value;
                }
            }

            public void CheckClosest(Cl3DModel.Cl3DModelPointIterator point)
            {
                if (MinDistance == -1)
                {
                    MinDistance = this - point;
                    ClosestPoint = point.CopyIterator();
                }
                else
                {
                    float tmpDistance = this - point;
                    if (tmpDistance < MinDistance)
                    {
                        MinDistance = tmpDistance;
                        ClosestPoint = point.CopyIterator();
                    }
                }
            }

            public static float operator -(MainPoint3D point1, Cl3DModel.Cl3DModelPointIterator point2)
            {
                float diff = (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2) + Math.Pow(point1.Z - point2.Z, 2));
                return diff;
            }
        }

        public static string ApplicationDirectory = "";
        public class ClTriangle
        {
            public Cl3DModel.Cl3DModelPointIterator m_point1 = null;
            public Cl3DModel.Cl3DModelPointIterator m_point2 = null;
            public Cl3DModel.Cl3DModelPointIterator m_point3 = null;

            public bool AlreadyVisited
            {
                get
                {
                    return m_point1.AlreadyVisited && m_point2.AlreadyVisited && m_point3.AlreadyVisited;
                }
                set
                {
                    m_point1.AlreadyVisited = value;
                    m_point2.AlreadyVisited = value;
                    m_point3.AlreadyVisited = value;
                }
            }

            public ClTriangle(Cl3DModel.Cl3DModelPointIterator point1, Cl3DModel.Cl3DModelPointIterator point2, Cl3DModel.Cl3DModelPointIterator point3)
            {
                m_point1 = point1.CopyIterator();
                m_point2 = point2.CopyIterator();
                m_point3 = point3.CopyIterator();
            }
        }

        public static class ClExpressionFRGC
        {
            private static Dictionary<string, string> Expressions = new Dictionary<string, string>();

            static ClExpressionFRGC()
            {
                Expressions.Add("04418d203", "NoExpression");
                Expressions.Add("04473d108", "NoExpression");
                Expressions.Add("04380d237", "NoExpression");
                Expressions.Add("04435d252", "NoExpression");
                Expressions.Add("04422d194", "NoExpression");
                Expressions.Add("04569d209", "NoExpression");
                Expressions.Add("04349d224", "NoExpression");
                Expressions.Add("04442d167", "NoExpression");
                Expressions.Add("04376d222", "NoExpression");
                Expressions.Add("04308d186", "NoExpression");
                Expressions.Add("04514d226", "NoExpression");
                Expressions.Add("04429d246", "NoExpression");
                Expressions.Add("04428d197", "NoExpression");
                Expressions.Add("04560d179", "NoExpression");
                Expressions.Add("04453d229", "NoExpression");
                Expressions.Add("04531d198", "NoExpression");
                Expressions.Add("04419d174", "NoExpression");
                Expressions.Add("04337d224", "NoExpression");
                Expressions.Add("04327d216", "NoExpression");
                Expressions.Add("04322d92", "NoExpression");
                Expressions.Add("02463d452", "NoExpression");
                Expressions.Add("04623d07", "NoExpression");
                Expressions.Add("04212d346", "NoExpression");
                Expressions.Add("04319d120", "NoExpression");
                Expressions.Add("04577d194", "NoExpression");
                Expressions.Add("04388d189", "NoExpression");
                Expressions.Add("04635d25", "NoExpression");
                Expressions.Add("04282d89", "NoExpression");
                Expressions.Add("04414d214", "NoExpression");
                Expressions.Add("04331d114", "NoExpression");
                Expressions.Add("04456d183", "NoExpression");
                Expressions.Add("04202d344", "NoExpression");
                Expressions.Add("04256d311", "NoExpression");
                Expressions.Add("04243d330", "NoExpression");
                Expressions.Add("04229d350", "NoExpression");
                Expressions.Add("04537d241", "NoExpression");
                Expressions.Add("04515d91", "NoExpression");
                Expressions.Add("04482d231", "NoExpression");
                Expressions.Add("04261d255", "NoExpression");
                Expressions.Add("04221d343", "NoExpression");
                Expressions.Add("04437d172", "NoExpression");
                Expressions.Add("04444d158", "NoExpression");
                Expressions.Add("04589d152", "NoExpression");
                Expressions.Add("04321d58", "NoExpression");
                Expressions.Add("04666d07", "NoExpression");
                Expressions.Add("04478d147", "NoExpression");
                Expressions.Add("04400d216", "NoExpression");
                Expressions.Add("04257d128", "NoExpression");
                Expressions.Add("04533d181", "NoExpression");
                Expressions.Add("04344d201", "NoExpression");
                Expressions.Add("04302d102", "NoExpression");
                Expressions.Add("04222d345", "NoExpression");
                Expressions.Add("04471d219", "NoExpression");
                Expressions.Add("04201d302", "NoExpression");
                Expressions.Add("04530d221", "NoExpression");
                Expressions.Add("04279d235", "NoExpression");
                Expressions.Add("04213d241", "NoExpression");
                Expressions.Add("04557d241", "NoExpression");
                Expressions.Add("04489d232", "NoExpression");
                Expressions.Add("04449d140", "NoExpression");
                Expressions.Add("04317d58", "NoExpression");
                Expressions.Add("04484d133", "NoExpression");
                Expressions.Add("04488d197", "NoExpression");
                Expressions.Add("04504d69", "NoExpression");
                Expressions.Add("04434d124", "NoExpression");
                Expressions.Add("04297d208", "NoExpression");
                Expressions.Add("04203d340", "NoExpression");
                Expressions.Add("04431d230", "NoExpression");
                Expressions.Add("04676d117", "NoExpression");
                Expressions.Add("04211d337", "NoExpression");
                Expressions.Add("04360d211", "NoExpression");
                Expressions.Add("04509d184", "NoExpression");
                Expressions.Add("04309d83", "NoExpression");
                Expressions.Add("04387d241", "NoExpression");
                Expressions.Add("04575d216", "NoExpression");
                Expressions.Add("04487d120", "NoExpression");
                Expressions.Add("04395d116", "NoExpression");
                Expressions.Add("04523d248", "NoExpression");
                Expressions.Add("04350d191", "NoExpression");
                Expressions.Add("04382d126", "NoExpression");
                Expressions.Add("04385d237", "NoExpression");
                Expressions.Add("04512d242", "NoExpression");
                Expressions.Add("02463d454", "NoExpression");
                Expressions.Add("04347d207", "NoExpression");
                Expressions.Add("04288d180", "NoExpression");
                Expressions.Add("04556d215", "NoExpression");
                Expressions.Add("04368d168", "NoExpression");
                Expressions.Add("04376d224", "NoExpression");
                Expressions.Add("04202d346", "NoExpression");
                Expressions.Add("04569d211", "NoExpression");
                Expressions.Add("04419d176", "NoExpression");
                Expressions.Add("04514d228", "NoExpression");
                Expressions.Add("04549d149", "NoExpression");
                Expressions.Add("04435d254", "NoExpression");
                Expressions.Add("04560d181", "NoExpression");
                Expressions.Add("04343d230", "NoExpression");
                Expressions.Add("04388d191", "NoExpression");
                Expressions.Add("04308d188", "NoExpression");
                Expressions.Add("04228d333", "NoExpression");
                Expressions.Add("04548d142", "NoExpression");
                Expressions.Add("04635d27", "NoExpression");
                Expressions.Add("04350d193", "NoExpression");
                Expressions.Add("04577d196", "NoExpression");
                Expressions.Add("04537d243", "NoExpression");
                Expressions.Add("04305d253", "NoExpression");
                Expressions.Add("04243d332", "NoExpression");
                Expressions.Add("04222d347", "NoExpression");
                Expressions.Add("04535d167", "NoExpression");
                Expressions.Add("04479d186", "NoExpression");
                Expressions.Add("04575d218", "NoExpression");
                Expressions.Add("04525d138", "NoExpression");
                Expressions.Add("04524d114", "NoExpression");
                Expressions.Add("04400d218", "NoExpression");
                Expressions.Add("04320d198", "NoExpression");
                Expressions.Add("04589d154", "NoExpression");
                Expressions.Add("04233d308", "NoExpression");
                Expressions.Add("04361d143", "NoExpression");
                Expressions.Add("04533d183", "NoExpression");
                Expressions.Add("04478d149", "NoExpression");
                Expressions.Add("04431d232", "NoExpression");
                Expressions.Add("04274d164", "NoExpression");
                Expressions.Add("04391d32", "NoExpression");
                Expressions.Add("04482d233", "NoExpression");
                Expressions.Add("04352d127", "NoExpression");
                Expressions.Add("04424d153", "NoExpression");
                Expressions.Add("04344d203", "NoExpression");
                Expressions.Add("04679d07", "NoExpression");
                Expressions.Add("04451d187", "NoExpression");
                Expressions.Add("04530d223", "NoExpression");
                Expressions.Add("04448d70", "NoExpression");
                Expressions.Add("04379d192", "NoExpression");
                Expressions.Add("04489d234", "NoExpression");
                Expressions.Add("04495d214", "NoExpression");
                Expressions.Add("04213d243", "NoExpression");
                Expressions.Add("04327d218", "NoExpression");
                Expressions.Add("04449d142", "NoExpression");
                Expressions.Add("04362d55", "NoExpression");
                Expressions.Add("04493d182", "NoExpression");
                Expressions.Add("04286d184", "NoExpression");
                Expressions.Add("04481d211", "NoExpression");
                Expressions.Add("04331d116", "NoExpression");
                Expressions.Add("04456d185", "NoExpression");
                Expressions.Add("04229d352", "NoExpression");
                Expressions.Add("04323d153", "NoExpression");
                Expressions.Add("04675d174", "NoExpression");
                Expressions.Add("04297d210", "NoExpression");
                Expressions.Add("04437d174", "NoExpression");
                Expressions.Add("04557d243", "NoExpression");
                Expressions.Add("04341d133", "NoExpression");
                Expressions.Add("04592d39", "NoExpression");
                Expressions.Add("04221d345", "NoExpression");
                Expressions.Add("04211d339", "NoExpression");
                Expressions.Add("04433d147", "NoExpression");
                Expressions.Add("04402d217", "NoExpression");
                Expressions.Add("04408d190", "NoExpression");
                Expressions.Add("04217d331", "NoExpression");
                Expressions.Add("04239d302", "NoExpression");
                Expressions.Add("04374d185", "NoExpression");
                Expressions.Add("04527d137", "NoExpression");
                Expressions.Add("04461d210", "NoExpression");
                Expressions.Add("04334d214", "NoExpression");
                Expressions.Add("04387d243", "NoExpression");
                Expressions.Add("04434d126", "NoExpression");
                Expressions.Add("04639d25", "NoExpression");
                Expressions.Add("04382d128", "NoExpression");
                Expressions.Add("04300d170", "NoExpression");
                Expressions.Add("04485d194", "NoExpression");
                Expressions.Add("04306d44", "NoExpression");
                Expressions.Add("04464d53", "NoExpression");
                Expressions.Add("04647d133", "NoExpression");
                Expressions.Add("04299d166", "NoExpression");
                Expressions.Add("04494d129", "NoExpression");
                Expressions.Add("04427d174", "NoExpression");
                Expressions.Add("04316d210", "NoExpression");
                Expressions.Add("04511d135", "NoExpression");
                Expressions.Add("04505d136", "NoExpression");
                Expressions.Add("04472d154", "NoExpression");
                Expressions.Add("04605d167", "NoExpression");
                Expressions.Add("04425d166", "NoExpression");
                Expressions.Add("04454d137", "NoExpression");
                Expressions.Add("04622d144", "NoExpression");
                Expressions.Add("04467d91", "NoExpression");
                Expressions.Add("04657d138", "NoExpression");
                Expressions.Add("04453d231", "NoExpression");
                Expressions.Add("04339d184", "NoExpression");
                Expressions.Add("04370d155", "NoExpression");
                Expressions.Add("04404d135", "NoExpression");
                Expressions.Add("04529d93", "NoExpression");
                Expressions.Add("04366d74", "NoExpression");
                Expressions.Add("04600d155", "NoExpression");
                Expressions.Add("04203d342", "NoExpression");
                Expressions.Add("04274d166", "NoExpression");
                Expressions.Add("04422d196", "NoExpression");
                Expressions.Add("04561d227", "NoExpression");
                Expressions.Add("04572d128", "NoExpression");
                Expressions.Add("04288d182", "NoExpression");
                Expressions.Add("04569d213", "NoExpression");
                Expressions.Add("04397d246", "NoExpression");
                Expressions.Add("04385d239", "NoExpression");
                Expressions.Add("04308d190", "NoExpression");
                Expressions.Add("04376d226", "NoExpression");
                Expressions.Add("04392d217", "NoExpression");
                Expressions.Add("04527d139", "NoExpression");
                Expressions.Add("04560d183", "NoExpression");
                Expressions.Add("04388d193", "NoExpression");
                Expressions.Add("02463d456", "NoExpression");
                Expressions.Add("04470d209", "NoExpression");
                Expressions.Add("04537d245", "NoExpression");
                Expressions.Add("04577d198", "NoExpression");
                Expressions.Add("04211d341", "NoExpression");
                Expressions.Add("04256d313", "NoExpression");
                Expressions.Add("04347d209", "NoExpression");
                Expressions.Add("04337d226", "NoExpression");
                Expressions.Add("04323d155", "NoExpression");
                Expressions.Add("04509d186", "NoExpression");
                Expressions.Add("04309d85", "NoExpression");
                Expressions.Add("04243d334", "NoExpression");
                Expressions.Add("04414d216", "NoExpression");
                Expressions.Add("04535d169", "NoExpression");
                Expressions.Add("04400d220", "NoExpression");
                Expressions.Add("04374d187", "NoExpression");
                Expressions.Add("04429d248", "NoExpression");
                Expressions.Add("04513d219", "NoExpression");
                Expressions.Add("04514d230", "NoExpression");
                Expressions.Add("04533d185", "NoExpression");
                Expressions.Add("04407d182", "NoExpression");
                Expressions.Add("04395d118", "NoExpression");
                Expressions.Add("04350d195", "NoExpression");
                Expressions.Add("04360d213", "NoExpression");
                Expressions.Add("04324d203", "NoExpression");
                Expressions.Add("04489d236", "NoExpression");
                Expressions.Add("04461d212", "NoExpression");
                Expressions.Add("04418d205", "NoExpression");
                Expressions.Add("04435d256", "NoExpression");
                Expressions.Add("04368d170", "NoExpression");
                Expressions.Add("04288d184", "NoExpression");
                Expressions.Add("04485d196", "NoExpression");
                Expressions.Add("04392d219", "NoExpression");
                Expressions.Add("04429d250", "NoExpression");
                Expressions.Add("04556d217", "NoExpression");
                Expressions.Add("04549d151", "NoExpression");
                Expressions.Add("04580d209", "NoExpression");
                Expressions.Add("04336d207", "NoExpression");
                Expressions.Add("04320d200", "NoExpression");
                Expressions.Add("04644d132", "NoExpression");
                Expressions.Add("04453d233", "NoExpression");
                Expressions.Add("04509d188", "NoExpression");
                Expressions.Add("04581d134", "NoExpression");
                Expressions.Add("04507d204", "NoExpression");
                Expressions.Add("04535d171", "NoExpression");
                Expressions.Add("04512d244", "NoExpression");
                Expressions.Add("04417d202", "NoExpression");
                Expressions.Add("04352d129", "NoExpression");
                Expressions.Add("04431d234", "NoExpression");
                Expressions.Add("04533d187", "NoExpression");
                Expressions.Add("04202d348", "NoExpression");
                Expressions.Add("04471d221", "NoExpression");
                Expressions.Add("04424d155", "NoExpression");
                Expressions.Add("04370d157", "NoExpression");
                Expressions.Add("04559d238", "NoExpression");
                Expressions.Add("04495d216", "NoExpression");
                Expressions.Add("04530d225", "NoExpression");
                Expressions.Add("04538d67", "NoExpression");
                Expressions.Add("04675d176", "NoExpression");
                Expressions.Add("04481d213", "NoExpression");
                Expressions.Add("04444d160", "NoExpression");
                Expressions.Add("04222d349", "NoExpression");
                Expressions.Add("04563d66", "NoExpression");
                Expressions.Add("04576d32", "NoExpression");
                Expressions.Add("04374d189", "NoExpression");
                Expressions.Add("04286d186", "NoExpression");
                Expressions.Add("04638d161", "NoExpression");
                Expressions.Add("04637d162", "NoExpression");
                Expressions.Add("04600d157", "NoExpression");
                Expressions.Add("04557d245", "NoExpression");
                Expressions.Add("04482d235", "NoExpression");
                Expressions.Add("04436d232", "NoExpression");
                Expressions.Add("04589d156", "NoExpression");
                Expressions.Add("04654d102", "NoExpression");
                Expressions.Add("04379d194", "NoExpression");
                Expressions.Add("04641d119", "NoExpression");
                Expressions.Add("04305d255", "NoExpression");
                Expressions.Add("04319d122", "NoExpression");
                Expressions.Add("04203d344", "NoExpression");
                Expressions.Add("04226d329", "NoExpression");
                Expressions.Add("04282d91", "NoExpression");
                Expressions.Add("04219d341", "NoExpression");
                Expressions.Add("04315d185", "NoExpression");
                Expressions.Add("04409d111", "NoExpression");
                Expressions.Add("04450d42", "NoExpression");
                Expressions.Add("04404d137", "NoExpression");
                Expressions.Add("04575d220", "NoExpression");
                Expressions.Add("04446d197", "NoExpression");
                Expressions.Add("04324d205", "NoExpression");
                Expressions.Add("04252d169", "NoExpression");
                Expressions.Add("04479d188", "NoExpression");
                Expressions.Add("04496d168", "NoExpression");
                Expressions.Add("04372d194", "NoExpression");
                Expressions.Add("04545d102", "NoExpression");
                Expressions.Add("04488d199", "NoExpression");
                Expressions.Add("04410d154", "NoExpression");
                Expressions.Add("04525d140", "NoExpression");
                Expressions.Add("04511d137", "NoExpression");
                Expressions.Add("04505d138", "NoExpression");
                Expressions.Add("04225d207", "NoExpression");
                Expressions.Add("04454d139", "NoExpression");
                Expressions.Add("04361d145", "NoExpression");
                Expressions.Add("04464d55", "NoExpression");
                Expressions.Add("04386d119", "NoExpression");
                Expressions.Add("04451d189", "NoExpression");
                Expressions.Add("04494d131", "NoExpression");
                Expressions.Add("04239d304", "NoExpression");
                Expressions.Add("04478d151", "NoExpression");
                Expressions.Add("04647d135", "NoExpression");
                Expressions.Add("04524d116", "NoExpression");
                Expressions.Add("04408d192", "NoExpression");
                Expressions.Add("04467d93", "NoExpression");
                Expressions.Add("04544d51", "NoExpression");
                Expressions.Add("04233d310", "NoExpression");
                Expressions.Add("04385d241", "NoExpression");
                Expressions.Add("04418d207", "NoExpression");
                Expressions.Add("04435d258", "NoExpression");
                Expressions.Add("04556d219", "NoExpression");
                Expressions.Add("04569d215", "NoExpression");
                Expressions.Add("04349d226", "NoExpression");
                Expressions.Add("04512d246", "NoExpression");
                Expressions.Add("04212d348", "NoExpression");
                Expressions.Add("02463d458", "NoExpression");
                Expressions.Add("04606d128", "NoExpression");
                Expressions.Add("04376d228", "NoExpression");
                Expressions.Add("04239d306", "NoExpression");
                Expressions.Add("04392d221", "NoExpression");
                Expressions.Add("04419d178", "NoExpression");
                Expressions.Add("04531d200", "NoExpression");
                Expressions.Add("04561d229", "NoExpression");
                Expressions.Add("04618d132", "NoExpression");
                Expressions.Add("04656d121", "NoExpression");
                Expressions.Add("04337d228", "NoExpression");
                Expressions.Add("04428d199", "NoExpression");
                Expressions.Add("04315d187", "NoExpression");
                Expressions.Add("04673d137", "NoExpression");
                Expressions.Add("04683d141", "NoExpression");
                Expressions.Add("04626d141", "NoExpression");
                Expressions.Add("04211d343", "NoExpression");
                Expressions.Add("04530d227", "NoExpression");
                Expressions.Add("04222d351", "NoExpression");
                Expressions.Add("04202d350", "NoExpression");
                Expressions.Add("04627d89", "NoExpression");
                Expressions.Add("04347d211", "NoExpression");
                Expressions.Add("04334d216", "NoExpression");
                Expressions.Add("04323d157", "NoExpression");
                Expressions.Add("04380d239", "NoExpression");
                Expressions.Add("04256d315", "NoExpression");
                Expressions.Add("04577d200", "NoExpression");
                Expressions.Add("04388d195", "NoExpression");
                Expressions.Add("04274d168", "NoExpression");
                Expressions.Add("04352d131", "NoExpression");
                Expressions.Add("04478d153", "NoExpression");
                Expressions.Add("04482d237", "NoExpression");
                Expressions.Add("04344d205", "NoExpression");
                Expressions.Add("04311d174", "NoExpression");
                Expressions.Add("04598d173", "NoExpression");
                Expressions.Add("04371d164", "NoExpression");
                Expressions.Add("04243d336", "NoExpression");
                Expressions.Add("04513d221", "NoExpression");
                Expressions.Add("04484d135", "NoExpression");
                Expressions.Add("04431d236", "NoExpression");
                Expressions.Add("04675d178", "NoExpression");
                Expressions.Add("04495d218", "NoExpression");
                Expressions.Add("04449d144", "NoExpression");
                Expressions.Add("04514d232", "NoExpression");
                Expressions.Add("04286d188", "NoExpression");
                Expressions.Add("04378d142", "NoExpression");
                Expressions.Add("04507d206", "NoExpression");
                Expressions.Add("04612d37", "NoExpression");
                Expressions.Add("04485d198", "NoExpression");
                Expressions.Add("04350d197", "NoExpression");
                Expressions.Add("04553d196", "NoExpression");
                Expressions.Add("04540d232", "NoExpression");
                Expressions.Add("04365d251", "NoExpression");
                Expressions.Add("04331d118", "NoExpression");
                Expressions.Add("04637d164", "NoExpression");
                Expressions.Add("04638d163", "NoExpression");
                Expressions.Add("04557d247", "NoExpression");
                Expressions.Add("04654d104", "NoExpression");
                Expressions.Add("04297d212", "NoExpression");
                Expressions.Add("04361d147", "NoExpression");
                Expressions.Add("04671d46", "NoExpression");
                Expressions.Add("04539d155", "NoExpression");
                Expressions.Add("04233d312", "NoExpression");
                Expressions.Add("04308d192", "NoExpression");
                Expressions.Add("04560d185", "NoExpression");
                Expressions.Add("04527d141", "NoExpression");
                Expressions.Add("04265d211", "NoExpression");
                Expressions.Add("04622d146", "NoExpression");
                Expressions.Add("04427d176", "NoExpression");
                Expressions.Add("04558d171", "NoExpression");
                Expressions.Add("04584d108", "NoExpression");
                Expressions.Add("04417d204", "NoExpression");
                Expressions.Add("04659d12", "NoExpression");
                Expressions.Add("04360d215", "NoExpression");
                Expressions.Add("04414d218", "NoExpression");
                Expressions.Add("04687d68", "NoExpression");
                Expressions.Add("04581d136", "NoExpression");
                Expressions.Add("04261d257", "NoExpression");
                Expressions.Add("04279d237", "NoExpression");
                Expressions.Add("04489d238", "NoExpression");
                Expressions.Add("04387d245", "NoExpression");
                Expressions.Add("04505d140", "NoExpression");
                Expressions.Add("04511d139", "NoExpression");
                Expressions.Add("04437d176", "NoExpression");
                Expressions.Add("04506d158", "NoExpression");
                Expressions.Add("04604d64", "NoExpression");
                Expressions.Add("04651d100", "NoExpression");
                Expressions.Add("04226d331", "NoExpression");
                Expressions.Add("04523d250", "NoExpression");
                Expressions.Add("04522d243", "NoExpression");
                Expressions.Add("04446d199", "NoExpression");
                Expressions.Add("04504d71", "NoExpression");
                Expressions.Add("04228d335", "NoExpression");
                Expressions.Add("04229d354", "NoExpression");
                Expressions.Add("04324d207", "NoExpression");
                Expressions.Add("04382d130", "NoExpression");
                Expressions.Add("04252d171", "NoExpression");
                Expressions.Add("04316d212", "NoExpression");
                Expressions.Add("04320d202", "NoExpression");
                Expressions.Add("04599d115", "NoExpression");
                Expressions.Add("04479d190", "NoExpression");
                Expressions.Add("04496d170", "NoExpression");
                Expressions.Add("04374d191", "NoExpression");
                Expressions.Add("04487d122", "NoExpression");
                Expressions.Add("04404d139", "NoExpression");
                Expressions.Add("04509d190", "NoExpression");
                Expressions.Add("04461d214", "NoExpression");
                Expressions.Add("04605d169", "NoExpression");
                Expressions.Add("04586d12", "NoExpression");
                Expressions.Add("04410d156", "NoExpression");
                Expressions.Add("04520d82", "NoExpression");
                Expressions.Add("04367d82", "NoExpression");
                Expressions.Add("04524d118", "NoExpression");
                Expressions.Add("04444d162", "NoExpression");
                Expressions.Add("04525d142", "NoExpression");
                Expressions.Add("04299d168", "NoExpression");
                Expressions.Add("04203d346", "NoExpression");
                Expressions.Add("04408d194", "NoExpression");
                Expressions.Add("04339d186", "NoExpression");
                Expressions.Add("04368d172", "NoExpression");
                Expressions.Add("04385d243", "NoExpression");
                Expressions.Add("04261d259", "NoExpression");
                Expressions.Add("04392d223", "NoExpression");
                Expressions.Add("04435d260", "NoExpression");
                Expressions.Add("04315d189", "NoExpression");
                Expressions.Add("04556d221", "NoExpression");
                Expressions.Add("04485d200", "NoExpression");
                Expressions.Add("04626d143", "NoExpression");
                Expressions.Add("04607d14", "NoExpression");
                Expressions.Add("04337d230", "NoExpression");
                Expressions.Add("04256d317", "NoExpression");
                Expressions.Add("04618d134", "NoExpression");
                Expressions.Add("04656d123", "NoExpression");
                Expressions.Add("04265d213", "NoExpression");
                Expressions.Add("04537d247", "NoExpression");
                Expressions.Add("04481d215", "NoExpression");
                Expressions.Add("04323d159", "NoExpression");
                Expressions.Add("04683d143", "NoExpression");
                Expressions.Add("04514d234", "NoExpression");
                Expressions.Add("04222d353", "NoExpression");
                Expressions.Add("04540d234", "NoExpression");
                Expressions.Add("04239d308", "NoExpression");
                Expressions.Add("04202d352", "NoExpression");
                Expressions.Add("04577d202", "NoExpression");
                Expressions.Add("04388d197", "NoExpression");
                Expressions.Add("04525d144", "NoExpression");
                Expressions.Add("04495d220", "NoExpression");
                Expressions.Add("04531d202", "NoExpression");
                Expressions.Add("04658d115", "NoExpression");
                Expressions.Add("04300d172", "NoExpression");
                Expressions.Add("02463d460", "NoExpression");
                Expressions.Add("04585d118", "NoExpression");
                Expressions.Add("04429d252", "NoExpression");
                Expressions.Add("04444d164", "NoExpression");
                Expressions.Add("04212d350", "NoExpression");
                Expressions.Add("04512d248", "NoExpression");
                Expressions.Add("04600d159", "NoExpression");
                Expressions.Add("04437d178", "NoExpression");
                Expressions.Add("04589d158", "NoExpression");
                Expressions.Add("04211d345", "NoExpression");
                Expressions.Add("04410d158", "NoExpression");
                Expressions.Add("04461d216", "NoExpression");
                Expressions.Add("04370d159", "NoExpression");
                Expressions.Add("04524d120", "NoExpression");
                Expressions.Add("04387d247", "NoExpression");
                Expressions.Add("04369d196", "NoExpression");
                Expressions.Add("04408d196", "NoExpression");
                Expressions.Add("04417d206", "NoExpression");
                Expressions.Add("04360d217", "NoExpression");
                Expressions.Add("04343d232", "NoExpression");
                Expressions.Add("04651d102", "NoExpression");
                Expressions.Add("04596d28", "NoExpression");
                Expressions.Add("04350d199", "NoExpression");
                Expressions.Add("04308d194", "NoExpression");
                Expressions.Add("04463d149", "NoExpression");
                Expressions.Add("04233d314", "NoExpression");
                Expressions.Add("04496d172", "NoExpression");
                Expressions.Add("04581d138", "NoExpression");
                Expressions.Add("04305d257", "NoExpression");
                Expressions.Add("04522d245", "NoExpression");
                Expressions.Add("04523d252", "NoExpression");
                Expressions.Add("04217d333", "NoExpression");
                Expressions.Add("04593d108", "NoExpression");
                Expressions.Add("04404d141", "NoExpression");
                Expressions.Add("04316d214", "NoExpression");
                Expressions.Add("04427d178", "NoExpression");
                Expressions.Add("04446d201", "NoExpression");
                Expressions.Add("04297d214", "NoExpression");
                Expressions.Add("04622d148", "NoExpression");
                Expressions.Add("04687d70", "NoExpression");
                Expressions.Add("04407d184", "NoExpression");
                Expressions.Add("04647d137", "NoExpression");
                Expressions.Add("04644d134", "NoExpression");
                Expressions.Add("04509d192", "NoExpression");
                Expressions.Add("04479d192", "NoExpression");
                Expressions.Add("04513d223", "NoExpression");
                Expressions.Add("04219d343", "NoExpression");
                Expressions.Add("04299d170", "NoExpression");
                Expressions.Add("04511d141", "NoExpression");
                Expressions.Add("04425d168", "NoExpression");
                Expressions.Add("04454d141", "NoExpression");
                Expressions.Add("04605d171", "NoExpression");
                Expressions.Add("04500d110", "NoExpression");
                Expressions.Add("04203d348", "NoExpression");
                Expressions.Add("04301d156", "NoExpression");
                Expressions.Add("04428d201", "NoExpression");
                Expressions.Add("04460d192", "NoExpression");
                Expressions.Add("04470d211", "NoExpression");
                Expressions.Add("04288d186", "NoExpression");
                Expressions.Add("04349d228", "NoExpression");
                Expressions.Add("04414d220", "NoExpression");
                Expressions.Add("04385d245", "NoExpression");
                Expressions.Add("04569d217", "NoExpression");
                Expressions.Add("04341d135", "NoExpression");
                Expressions.Add("04308d196", "NoExpression");
                Expressions.Add("04422d198", "NoExpression");
                Expressions.Add("04397d248", "NoExpression");
                Expressions.Add("04530d229", "NoExpression");
                Expressions.Add("04619d131", "NoExpression");
                Expressions.Add("04212d352", "NoExpression");
                Expressions.Add("04392d225", "NoExpression");
                Expressions.Add("04376d230", "NoExpression");
                Expressions.Add("04435d262", "NoExpression");
                Expressions.Add("04626d145", "NoExpression");
                Expressions.Add("04453d235", "NoExpression");
                Expressions.Add("04633d98", "NoExpression");
                Expressions.Add("04598d175", "NoExpression");
                Expressions.Add("04347d213", "NoExpression");
                Expressions.Add("04417d208", "NoExpression");
                Expressions.Add("04337d232", "NoExpression");
                Expressions.Add("04400d222", "NoExpression");
                Expressions.Add("04320d204", "NoExpression");
                Expressions.Add("04495d222", "NoExpression");
                Expressions.Add("04589d160", "NoExpression");
                Expressions.Add("04676d119", "NoExpression");
                Expressions.Add("04317d60", "NoExpression");
                Expressions.Add("04286d190", "NoExpression");
                Expressions.Add("04431d238", "NoExpression");
                Expressions.Add("04545d104", "NoExpression");
                Expressions.Add("04424d157", "NoExpression");
                Expressions.Add("04582d123", "NoExpression");
                Expressions.Add("04637d166", "NoExpression");
                Expressions.Add("04638d165", "NoExpression");
                Expressions.Add("04380d241", "NoExpression");
                Expressions.Add("04430d197", "NoExpression");
                Expressions.Add("04370d161", "NoExpression");
                Expressions.Add("04456d187", "NoExpression");
                Expressions.Add("04311d176", "NoExpression");
                Expressions.Add("04309d87", "NoExpression");
                Expressions.Add("04476d48", "NoExpression");
                Expressions.Add("04481d217", "NoExpression");
                Expressions.Add("04628d135", "NoExpression");
                Expressions.Add("04507d208", "NoExpression");
                Expressions.Add("04488d201", "NoExpression");
                Expressions.Add("04533d189", "NoExpression");
                Expressions.Add("04243d338", "NoExpression");
                Expressions.Add("04386d121", "NoExpression");
                Expressions.Add("04402d219", "NoExpression");
                Expressions.Add("04201d304", "NoExpression");
                Expressions.Add("04221d347", "NoExpression");
                Expressions.Add("04539d157", "NoExpression");
                Expressions.Add("04437d180", "NoExpression");
                Expressions.Add("04475d42", "NoExpression");
                Expressions.Add("04297d216", "NoExpression");
                Expressions.Add("04603d53", "NoExpression");
                Expressions.Add("04379d196", "NoExpression");
                Expressions.Add("04624d106", "NoExpression");
                Expressions.Add("04654d106", "NoExpression");
                Expressions.Add("04686d41", "NoExpression");
                Expressions.Add("04203d350", "NoExpression");
                Expressions.Add("04461d218", "NoExpression");
                Expressions.Add("04343d234", "NoExpression");
                Expressions.Add("04675d180", "NoExpression");
                Expressions.Add("04323d161", "NoExpression");
                Expressions.Add("04408d198", "NoExpression");
                Expressions.Add("04226d333", "NoExpression");
                Expressions.Add("04360d219", "NoExpression");
                Expressions.Add("04622d150", "NoExpression");
                Expressions.Add("04361d149", "NoExpression");
                Expressions.Add("04597d85", "NoExpression");
                Expressions.Add("04228d337", "NoExpression");
                Expressions.Add("04229d356", "NoExpression");
                Expressions.Add("04419d180", "NoExpression");
                Expressions.Add("04606d130", "NoExpression");
                Expressions.Add("04581d140", "NoExpression");
                Expressions.Add("04519d154", "NoExpression");
                Expressions.Add("04219d345", "NoExpression");
                Expressions.Add("04279d239", "NoExpression");
                Expressions.Add("04387d249", "NoExpression");
                Expressions.Add("04489d240", "NoExpression");
                Expressions.Add("04365d253", "NoExpression");
                Expressions.Add("04559d240", "NoExpression");
                Expressions.Add("04451d191", "NoExpression");
                Expressions.Add("04305d259", "NoExpression");
                Expressions.Add("04523d254", "NoExpression");
                Expressions.Add("04217d335", "NoExpression");
                Expressions.Add("04670d81", "NoExpression");
                Expressions.Add("04372d196", "NoExpression");
                Expressions.Add("04641d121", "NoExpression");
                Expressions.Add("04407d186", "NoExpression");
                Expressions.Add("04427d180", "NoExpression");
                Expressions.Add("04324d209", "NoExpression");
                Expressions.Add("04506d160", "NoExpression");
                Expressions.Add("04382d132", "NoExpression");
                Expressions.Add("04599d117", "NoExpression");
                Expressions.Add("04647d139", "NoExpression");
                Expressions.Add("04560d187", "NoExpression");
                Expressions.Add("04267d141", "NoExpression");
                Expressions.Add("04479d194", "NoExpression");
                Expressions.Add("04496d174", "NoExpression");
                Expressions.Add("04454d143", "NoExpression");
                Expressions.Add("04657d140", "NoExpression");
                Expressions.Add("04410d160", "NoExpression");
                Expressions.Add("04374d193", "NoExpression");
                Expressions.Add("04593d110", "NoExpression");
                Expressions.Add("04505d142", "NoExpression");
                Expressions.Add("04558d173", "NoExpression");
                Expressions.Add("04335d179", "NoExpression");
                Expressions.Add("04444d166", "NoExpression");
                Expressions.Add("04460d194", "NoExpression");
                Expressions.Add("02463d462", "NoExpression");
                Expressions.Add("04626d147", "NoExpression");
                Expressions.Add("04414d222", "NoExpression");
                Expressions.Add("04288d188", "NoExpression");
                Expressions.Add("04569d219", "NoExpression");
                Expressions.Add("04385d247", "NoExpression");
                Expressions.Add("04560d189", "NoExpression");
                Expressions.Add("04394d227", "NoExpression");
                Expressions.Add("04349d230", "NoExpression");
                Expressions.Add("04610d128", "NoExpression");
                Expressions.Add("04392d227", "NoExpression");
                Expressions.Add("04376d232", "NoExpression");
                Expressions.Add("04397d250", "NoExpression");
                Expressions.Add("04485d202", "NoExpression");
                Expressions.Add("04556d223", "NoExpression");
                Expressions.Add("04580d211", "NoExpression");
                Expressions.Add("04435d264", "NoExpression");
                Expressions.Add("04682d56", "NoExpression");
                Expressions.Add("04673d139", "NoExpression");
                Expressions.Add("04598d177", "NoExpression");
                Expressions.Add("04453d237", "NoExpression");
                Expressions.Add("04336d209", "NoExpression");
                Expressions.Add("04618d136", "NoExpression");
                Expressions.Add("04531d204", "NoExpression");
                Expressions.Add("04631d98", "NoExpression");
                Expressions.Add("04374d195", "NoExpression");
                Expressions.Add("04202d354", "NoExpression");
                Expressions.Add("04222d355", "NoExpression");
                Expressions.Add("04388d199", "NoExpression");
                Expressions.Add("04577d204", "NoExpression");
                Expressions.Add("04239d310", "NoExpression");
                Expressions.Add("04347d215", "NoExpression");
                Expressions.Add("04417d210", "NoExpression");
                Expressions.Add("04301d158", "NoExpression");
                Expressions.Add("04646d33", "NoExpression");
                Expressions.Add("04360d221", "NoExpression");
                Expressions.Add("04256d319", "NoExpression");
                Expressions.Add("04403d136", "NoExpression");
                Expressions.Add("04535d173", "NoExpression");
                Expressions.Add("04482d239", "NoExpression");
                Expressions.Add("04512d250", "NoExpression");
                Expressions.Add("04575d222", "NoExpression");
                Expressions.Add("04591d104", "NoExpression");
                Expressions.Add("04201d306", "NoExpression");
                Expressions.Add("04211d347", "NoExpression");
                Expressions.Add("04683d145", "NoExpression");
                Expressions.Add("04334d218", "NoExpression");
                Expressions.Add("04311d178", "NoExpression");
                Expressions.Add("04339d188", "NoExpression");
                Expressions.Add("04243d340", "NoExpression");
                Expressions.Add("04305d261", "NoExpression");
                Expressions.Add("04212d354", "NoExpression");
                Expressions.Add("04628d137", "NoExpression");
                Expressions.Add("04233d316", "NoExpression");
                Expressions.Add("04352d133", "NoExpression");
                Expressions.Add("04371d166", "NoExpression");
                Expressions.Add("04380d243", "NoExpression");
                Expressions.Add("04400d224", "NoExpression");
                Expressions.Add("04478d155", "NoExpression");
                Expressions.Add("04561d231", "NoExpression");
                Expressions.Add("04402d221", "NoExpression");
                Expressions.Add("04456d189", "NoExpression");
                Expressions.Add("04687d72", "NoExpression");
                Expressions.Add("04286d192", "NoExpression");
                Expressions.Add("04487d124", "NoExpression");
                Expressions.Add("04471d223", "NoExpression");
                Expressions.Add("04431d240", "NoExpression");
                Expressions.Add("04649d53", "NoExpression");
                Expressions.Add("04344d207", "NoExpression");
                Expressions.Add("04427d182", "NoExpression");
                Expressions.Add("04316d216", "NoExpression");
                Expressions.Add("04472d156", "NoExpression");
                Expressions.Add("04378d144", "NoExpression");
                Expressions.Add("04225d209", "NoExpression");
                Expressions.Add("04343d236", "NoExpression");
                Expressions.Add("04656d125", "NoExpression");
                Expressions.Add("04584d110", "NoExpression");
                Expressions.Add("04341d137", "NoExpression");
                Expressions.Add("04320d206", "NoExpression");
                Expressions.Add("04579d233", "NoExpression");
                Expressions.Add("04387d251", "NoExpression");
                Expressions.Add("04602d84", "NoExpression");
                Expressions.Add("04587d28", "NoExpression");
                Expressions.Add("04350d201", "NoExpression");
                Expressions.Add("04513d225", "NoExpression");
                Expressions.Add("04589d162", "NoExpression");
                Expressions.Add("04423d126", "NoExpression");
                Expressions.Add("04203d352", "NoExpression");
                Expressions.Add("04489d242", "NoExpression");
                Expressions.Add("04460d196", "NoExpression");
                Expressions.Add("04442d169", "NoExpression");
                Expressions.Add("04461d220", "NoExpression");
                Expressions.Add("04437d182", "NoExpression");
                Expressions.Add("04335d181", "NoExpression");
                Expressions.Add("04425d170", "NoExpression");
                Expressions.Add("04669d39", "NoExpression");
                Expressions.Add("04605d173", "NoExpression");
                Expressions.Add("04372d198", "NoExpression");
                Expressions.Add("04470d213", "NoExpression");
                Expressions.Add("04349d232", "NoExpression");
                Expressions.Add("04569d221", "NoExpression");
                Expressions.Add("04327d220", "NoExpression");
                Expressions.Add("04341d139", "NoExpression");
                Expressions.Add("04418d209", "NoExpression");
                Expressions.Add("04422d200", "NoExpression");
                Expressions.Add("02463d464", "NoExpression");
                Expressions.Add("04530d231", "NoExpression");
                Expressions.Add("04610d130", "NoExpression");
                Expressions.Add("04619d133", "NoExpression");
                Expressions.Add("04392d229", "NoExpression");
                Expressions.Add("04673d141", "NoExpression");
                Expressions.Add("04626d149", "NoExpression");
                Expressions.Add("04442d171", "NoExpression");
                Expressions.Add("04513d227", "NoExpression");
                Expressions.Add("04453d239", "NoExpression");
                Expressions.Add("04656d127", "NoExpression");
                Expressions.Add("04618d138", "NoExpression");
                Expressions.Add("04428d203", "NoExpression");
                Expressions.Add("04575d224", "NoExpression");
                Expressions.Add("04256d321", "NoExpression");
                Expressions.Add("04315d191", "NoExpression");
                Expressions.Add("04683d147", "NoExpression");
                Expressions.Add("04334d220", "NoExpression");
                Expressions.Add("04629d56", "NoExpression");
                Expressions.Add("04233d318", "NoExpression");
                Expressions.Add("04608d68", "NoExpression");
                Expressions.Add("04577d206", "NoExpression");
                Expressions.Add("04388d201", "NoExpression");
                Expressions.Add("04301d160", "NoExpression");
                Expressions.Add("04360d223", "NoExpression");
                Expressions.Add("04495d224", "NoExpression");
                Expressions.Add("04463d151", "NoExpression");
                Expressions.Add("04400d226", "NoExpression");
                Expressions.Add("04535d175", "NoExpression");
                Expressions.Add("04514d236", "NoExpression");
                Expressions.Add("04512d252", "NoExpression");
                Expressions.Add("04652d114", "NoExpression");
                Expressions.Add("04537d249", "NoExpression");
                Expressions.Add("04323d163", "NoExpression");
                Expressions.Add("04600d161", "NoExpression");
                Expressions.Add("04628d139", "NoExpression");
                Expressions.Add("04379d198", "NoExpression");
                Expressions.Add("04311d180", "NoExpression");
                Expressions.Add("04385d249", "NoExpression");
                Expressions.Add("04300d174", "NoExpression");
                Expressions.Add("04202d356", "NoExpression");
                Expressions.Add("04456d191", "NoExpression");
                Expressions.Add("04461d222", "NoExpression");
                Expressions.Add("04308d198", "NoExpression");
                Expressions.Add("04632d42", "NoExpression");
                Expressions.Add("04419d182", "NoExpression");
                Expressions.Add("04533d191", "NoExpression");
                Expressions.Add("04411d156", "NoExpression");
                Expressions.Add("04219d347", "NoExpression");
                Expressions.Add("04403d138", "NoExpression");
                Expressions.Add("04343d238", "NoExpression");
                Expressions.Add("04581d142", "NoExpression");
                Expressions.Add("04228d339", "NoExpression");
                Expressions.Add("04229d358", "NoExpression");
                Expressions.Add("04644d136", "NoExpression");
                Expressions.Add("04509d194", "NoExpression");
                Expressions.Add("04226d335", "NoExpression");
                Expressions.Add("04217d337", "NoExpression");
                Expressions.Add("04523d256", "NoExpression");
                Expressions.Add("04522d247", "NoExpression");
                Expressions.Add("04324d211", "NoExpression");
                Expressions.Add("04620d62", "NoExpression");
                Expressions.Add("04605d175", "NoExpression");
                Expressions.Add("04540d236", "NoExpression");
                Expressions.Add("04222d357", "NoExpression");
                Expressions.Add("04243d342", "NoExpression");
                Expressions.Add("04336d211", "NoExpression");
                Expressions.Add("04431d242", "NoExpression");
                Expressions.Add("04427d184", "NoExpression");
                Expressions.Add("04316d218", "NoExpression");
                Expressions.Add("04599d119", "NoExpression");
                Expressions.Add("04387d253", "NoExpression");
                Expressions.Add("04647d141", "NoExpression");
                Expressions.Add("04553d198", "NoExpression");
                Expressions.Add("04372d200", "NoExpression");
                Expressions.Add("04583d109", "NoExpression");
                Expressions.Add("04601d83", "NoExpression");
                Expressions.Add("04506d162", "NoExpression");
                Expressions.Add("04496d176", "NoExpression");
                Expressions.Add("04479d196", "NoExpression");
                Expressions.Add("04472d158", "NoExpression");
                Expressions.Add("04584d112", "NoExpression");
                Expressions.Add("04560d191", "NoExpression");
                Expressions.Add("04482d241", "NoExpression");
                Expressions.Add("04203d354", "NoExpression");
                Expressions.Add("04444d168", "NoExpression");
                Expressions.Add("04593d112", "NoExpression");
                Expressions.Add("04418d211", "NoExpression");
                Expressions.Add("04392d231", "NoExpression");
                Expressions.Add("04376d234", "NoExpression");
                Expressions.Add("04414d224", "NoExpression");
                Expressions.Add("04626d151", "NoExpression");
                Expressions.Add("04667d120", "NoExpression");
                Expressions.Add("04470d215", "NoExpression");
                Expressions.Add("04606d132", "NoExpression");
                Expressions.Add("04619d135", "NoExpression");
                Expressions.Add("04327d222", "NoExpression");
                Expressions.Add("04561d233", "NoExpression");
                Expressions.Add("04288d190", "NoExpression");
                Expressions.Add("04388d203", "NoExpression");
                Expressions.Add("04556d225", "NoExpression");
                Expressions.Add("04503d31", "NoExpression");
                Expressions.Add("02463d466", "NoExpression");
                Expressions.Add("04316d220", "NoExpression");
                Expressions.Add("04605d177", "NoExpression");
                Expressions.Add("04618d140", "NoExpression");
                Expressions.Add("04683d149", "NoExpression");
                Expressions.Add("04656d129", "NoExpression");
                Expressions.Add("04663d70", "NoExpression");
                Expressions.Add("04592d41", "NoExpression");
                Expressions.Add("04225d211", "NoExpression");
                Expressions.Add("04212d356", "NoExpression");
                Expressions.Add("04481d219", "NoExpression");
                Expressions.Add("04540d238", "NoExpression");
                Expressions.Add("04222d359", "NoExpression");
                Expressions.Add("04653d28", "NoExpression");
                Expressions.Add("04256d323", "NoExpression");
                Expressions.Add("04374d197", "NoExpression");
                Expressions.Add("04202d358", "NoExpression");
                Expressions.Add("04233d320", "NoExpression");
                Expressions.Add("04661d117", "NoExpression");
                Expressions.Add("04655d97", "NoExpression");
                Expressions.Add("04403d140", "NoExpression");
                Expressions.Add("04513d229", "NoExpression");
                Expressions.Add("04569d223", "NoExpression");
                Expressions.Add("04360d225", "NoExpression");
                Expressions.Add("04530d233", "NoExpression");
                Expressions.Add("04495d226", "NoExpression");
                Expressions.Add("04286d194", "NoExpression");
                Expressions.Add("04514d238", "NoExpression");
                Expressions.Add("04681d65", "NoExpression");
                Expressions.Add("04621d81", "NoExpression");
                Expressions.Add("04311d182", "NoExpression");
                Expressions.Add("04577d208", "NoExpression");
                Expressions.Add("04600d163", "NoExpression");
                Expressions.Add("04650d106", "NoExpression");
                Expressions.Add("04378d146", "NoExpression");
                Expressions.Add("04599d121", "NoExpression");
                Expressions.Add("04627d91", "NoExpression");
                Expressions.Add("04228d341", "NoExpression");
                Expressions.Add("04229d360", "NoExpression");
                Expressions.Add("04297d218", "NoExpression");
                Expressions.Add("04371d168", "NoExpression");
                Expressions.Add("04589d164", "NoExpression");
                Expressions.Add("04320d208", "NoExpression");
                Expressions.Add("04431d244", "NoExpression");
                Expressions.Add("04471d225", "NoExpression");
                Expressions.Add("04487d126", "NoExpression");
                Expressions.Add("04588d49", "NoExpression");
                Expressions.Add("04582d125", "NoExpression");
                Expressions.Add("04221d349", "NoExpression");
                Expressions.Add("04615d28", "NoExpression");
                Expressions.Add("04511d143", "NoExpression");
                Expressions.Add("04507d210", "NoExpression");
                Expressions.Add("04211d349", "NoExpression");
                Expressions.Add("04523d258", "NoExpression");
                Expressions.Add("04575d226", "NoExpression");
                Expressions.Add("04476d50", "NoExpression");
                Expressions.Add("04617d28", "NoExpression");
                Expressions.Add("04243d344", "NoExpression");
                Expressions.Add("04537d251", "NoExpression");
                Expressions.Add("04460d198", "NoExpression");
                Expressions.Add("04557d249", "NoExpression");
                Expressions.Add("04323d165", "NoExpression");
                Expressions.Add("04637d168", "NoExpression");
                Expressions.Add("04548d144", "NoExpression");
                Expressions.Add("04549d153", "NoExpression");
                Expressions.Add("04365d255", "NoExpression");
                Expressions.Add("04319d124", "NoExpression");
                Expressions.Add("04461d224", "NoExpression");
                Expressions.Add("04341d141", "NoExpression");
                Expressions.Add("04553d200", "NoExpression");
                Expressions.Add("04579d235", "NoExpression");
                Expressions.Add("04456d193", "NoExpression");
                Expressions.Add("04337d234", "NoExpression");
                Expressions.Add("04411d158", "NoExpression");
                Expressions.Add("04387d255", "NoExpression");
                Expressions.Add("04572d130", "NoExpression");
                Expressions.Add("04496d178", "NoExpression");
                Expressions.Add("04451d193", "NoExpression");
                Expressions.Add("04252d173", "NoExpression");
                Expressions.Add("04372d202", "NoExpression");
                Expressions.Add("04437d184", "NoExpression");
                Expressions.Add("04622d152", "NoExpression");
                Expressions.Add("04226d337", "NoExpression");
                Expressions.Add("04324d213", "NoExpression");
                Expressions.Add("04379d200", "NoExpression");
                Expressions.Add("04676d121", "NoExpression");
                Expressions.Add("04590d33", "NoExpression");
                Expressions.Add("04576d34", "NoExpression");
                Expressions.Add("04641d123", "NoExpression");
                Expressions.Add("04461d291", "NoExpression");
                Expressions.Add("04581d192", "NoExpression");
                Expressions.Add("04496d240", "NoExpression");
                Expressions.Add("04530d313", "NoExpression");
                Expressions.Add("04495d307", "NoExpression");
                Expressions.Add("04507d291", "NoExpression");
                Expressions.Add("04381d106", "NoExpression");
                Expressions.Add("04319d186", "NoExpression");
                Expressions.Add("04585d184", "NoExpression");
                Expressions.Add("04661d167", "NoExpression");
                Expressions.Add("04456d267", "NoExpression");
                Expressions.Add("04775d72", "NoExpression");
                Expressions.Add("04336d291", "NoExpression");
                Expressions.Add("04388d283", "NoExpression");
                Expressions.Add("04577d282", "NoExpression");
                Expressions.Add("04301d240", "NoExpression");
                Expressions.Add("04202d438", "NoExpression");
                Expressions.Add("04472d220", "NoExpression");
                Expressions.Add("04815d78", "NoExpression");
                Expressions.Add("04603d133", "NoExpression");
                Expressions.Add("04424d219", "NoExpression");
                Expressions.Add("04288d252", "NoExpression");
                Expressions.Add("04557d329", "NoExpression");
                Expressions.Add("04514d318", "NoExpression");
                Expressions.Add("04488d280", "NoExpression");
                Expressions.Add("04629d136", "NoExpression");
                Expressions.Add("02463d546", "NoExpression");
                Expressions.Add("04580d291", "NoExpression");
                Expressions.Add("04419d250", "NoExpression");
                Expressions.Add("04796d72", "NoExpression");
                Expressions.Add("04221d429", "NoExpression");
                Expressions.Add("04681d145", "NoExpression");
                Expressions.Add("04444d206", "NoExpression");
                Expressions.Add("04777d72", "NoExpression");
                Expressions.Add("04361d175", "NoExpression");
                Expressions.Add("04631d166", "NoExpression");
                Expressions.Add("04261d297", "NoExpression");
                Expressions.Add("04418d285", "NoExpression");
                Expressions.Add("04626d231", "NoExpression");
                Expressions.Add("04572d138", "NoExpression");
                Expressions.Add("04334d300", "NoExpression");
                Expressions.Add("04321d108", "NoExpression");
                Expressions.Add("04670d161", "NoExpression");
                Expressions.Add("04473d183", "NoExpression");
                Expressions.Add("04537d320", "NoExpression");
                Expressions.Add("04618d160", "NoExpression");
                Expressions.Add("04673d184", "NoExpression");
                Expressions.Add("04265d261", "NoExpression");
                Expressions.Add("04600d243", "NoExpression");
                Expressions.Add("04723d06", "NoExpression");
                Expressions.Add("04481d287", "NoExpression");
                Expressions.Add("04349d312", "NoExpression");
                Expressions.Add("04422d238", "NoExpression");
                Expressions.Add("04695d66", "NoExpression");
                Expressions.Add("04749d72", "NoExpression");
                Expressions.Add("04237d139", "NoExpression");
                Expressions.Add("04830d78", "NoExpression");
                Expressions.Add("04447d125", "NoExpression");
                Expressions.Add("04397d332", "NoExpression");
                Expressions.Add("04683d229", "NoExpression");
                Expressions.Add("04714d78", "NoExpression");
                Expressions.Add("04633d178", "NoExpression");
                Expressions.Add("04471d263", "NoExpression");
                Expressions.Add("04449d171", "NoExpression");
                Expressions.Add("04774d66", "NoExpression");
                Expressions.Add("04226d357", "NoExpression");
                Expressions.Add("04225d291", "NoExpression");
                Expressions.Add("04485d282", "NoExpression");
                Expressions.Add("04203d436", "NoExpression");
                Expressions.Add("04423d188", "NoExpression");
                Expressions.Add("04662d119", "NoExpression");
                Expressions.Add("04750d48", "NoExpression");
                Expressions.Add("04453d285", "NoExpression");
                Expressions.Add("04785d66", "NoExpression");
                Expressions.Add("04343d319", "NoExpression");
                Expressions.Add("04385d323", "NoExpression");
                Expressions.Add("04493d202", "NoExpression");
                Expressions.Add("04446d269", "NoExpression");
                Expressions.Add("04556d305", "NoExpression");
                Expressions.Add("04470d289", "NoExpression");
                Expressions.Add("04605d239", "NoExpression");
                Expressions.Add("04773d78", "NoExpression");
                Expressions.Add("04370d223", "NoExpression");
                Expressions.Add("04560d265", "NoExpression");
                Expressions.Add("04664d143", "NoExpression");
                Expressions.Add("04410d180", "NoExpression");
                Expressions.Add("04427d264", "NoExpression");
                Expressions.Add("04754d72", "NoExpression");
                Expressions.Add("04609d92", "NoExpression");
                Expressions.Add("04675d247", "NoExpression");
                Expressions.Add("04593d192", "NoExpression");
                Expressions.Add("04324d276", "NoExpression");
                Expressions.Add("04429d331", "NoExpression");
                Expressions.Add("04519d204", "NoExpression");
                Expressions.Add("04710d06", "NoExpression");
                Expressions.Add("04697d78", "NoExpression");
                Expressions.Add("04311d226", "NoExpression");
                Expressions.Add("04451d243", "NoExpression");
                Expressions.Add("04719d78", "NoExpression");
                Expressions.Add("04624d114", "NoExpression");
                Expressions.Add("04832d78", "NoExpression");
                Expressions.Add("04757d71", "NoExpression");
                Expressions.Add("04768d66", "NoExpression");
                Expressions.Add("04568d89", "NoExpression");
                Expressions.Add("04598d251", "NoExpression");
                Expressions.Add("04297d261", "NoExpression");
                Expressions.Add("04233d390", "NoExpression");
                Expressions.Add("04236d154", "NoExpression");
                Expressions.Add("04579d258", "NoExpression");
                Expressions.Add("04201d368", "NoExpression");
                Expressions.Add("04622d232", "NoExpression");
                Expressions.Add("04379d280", "NoExpression");
                Expressions.Add("04742d78", "NoExpression");
                Expressions.Add("04430d271", "NoExpression");
                Expressions.Add("04839d78", "NoExpression");
                Expressions.Add("04559d310", "NoExpression");
                Expressions.Add("04407d261", "NoExpression");
                Expressions.Add("04378d201", "NoExpression");
                Expressions.Add("04657d154", "NoExpression");
                Expressions.Add("04667d194", "NoExpression");
                Expressions.Add("04606d176", "NoExpression");
                Expressions.Add("04434d152", "NoExpression");
                Expressions.Add("04347d289", "NoExpression");
                Expressions.Add("04512d320", "NoExpression");
                Expressions.Add("04608d76", "NoExpression");
                Expressions.Add("04595d87", "NoExpression");
                Expressions.Add("04531d283", "NoExpression");
                Expressions.Add("04506d194", "NoExpression");
                Expressions.Add("04642d46", "NoExpression");
                Expressions.Add("04637d194", "NoExpression");
                Expressions.Add("04569d280", "NoExpression");
                Expressions.Add("04638d191", "NoExpression");
                Expressions.Add("04529d101", "NoExpression");
                Expressions.Add("04758d59", "NoExpression");
                Expressions.Add("04782d77", "NoExpression");
                Expressions.Add("04505d216", "NoExpression");
                Expressions.Add("04395d192", "NoExpression");
                Expressions.Add("04701d66", "NoExpression");
                Expressions.Add("04803d72", "NoExpression");
                Expressions.Add("04588d129", "NoExpression");
                Expressions.Add("04628d219", "NoExpression");
                Expressions.Add("04734d78", "NoExpression");
                Expressions.Add("04369d246", "NoExpression");
                Expressions.Add("04350d258", "NoExpression");
                Expressions.Add("04428d241", "NoExpression");
                Expressions.Add("04650d144", "NoExpression");
                Expressions.Add("04652d152", "NoExpression");
                Expressions.Add("04546d71", "NoExpression");
                Expressions.Add("04764d24", "NoExpression");
                Expressions.Add("04509d274", "NoExpression");
                Expressions.Add("04309d161", "NoExpression");
                Expressions.Add("04797d42", "NoExpression");
                Expressions.Add("04689d24", "NoExpression");
                Expressions.Add("04372d269", "NoExpression");
                Expressions.Add("04476d118", "NoExpression");
                Expressions.Add("04436d308", "NoExpression");
                Expressions.Add("04589d238", "NoExpression");
                Expressions.Add("04400d294", "NoExpression");
                Expressions.Add("04696d36", "NoExpression");
                Expressions.Add("04461d293", "NoExpression");
                Expressions.Add("04789d12", "NoExpression");
                Expressions.Add("04225d293", "NoExpression");
                Expressions.Add("04589d240", "NoExpression");
                Expressions.Add("04557d331", "NoExpression");
                Expressions.Add("04675d249", "NoExpression");
                Expressions.Add("04754d74", "NoExpression");
                Expressions.Add("04319d188", "NoExpression");
                Expressions.Add("04202d440", "NoExpression");
                Expressions.Add("04644d198", "NoExpression");
                Expressions.Add("04485d284", "NoExpression");
                Expressions.Add("04495d309", "NoExpression");
                Expressions.Add("04530d315", "NoExpression");
                Expressions.Add("04327d290", "NoExpression");
                Expressions.Add("04626d233", "NoExpression");
                Expressions.Add("04507d293", "NoExpression");
                Expressions.Add("04419d252", "NoExpression");
                Expressions.Add("04796d74", "NoExpression");
                Expressions.Add("04435d338", "NoExpression");
                Expressions.Add("04394d295", "NoExpression");
                Expressions.Add("04479d222", "NoExpression");
                Expressions.Add("04770d24", "NoExpression");
                Expressions.Add("04434d154", "NoExpression");
                Expressions.Add("04320d270", "NoExpression");
                Expressions.Add("04381d108", "NoExpression");
                Expressions.Add("04652d154", "NoExpression");
                Expressions.Add("04387d322", "NoExpression");
                Expressions.Add("04682d118", "NoExpression");
                Expressions.Add("04496d242", "NoExpression");
                Expressions.Add("04388d285", "NoExpression");
                Expressions.Add("04577d284", "NoExpression");
                Expressions.Add("04456d269", "NoExpression");
                Expressions.Add("04775d74", "NoExpression");
                Expressions.Add("04301d242", "NoExpression");
                Expressions.Add("04370d225", "NoExpression");
                Expressions.Add("04347d291", "NoExpression");
                Expressions.Add("04650d146", "NoExpression");
                Expressions.Add("04514d320", "NoExpression");
                Expressions.Add("04773d80", "NoExpression");
                Expressions.Add("04828d06", "NoExpression");
                Expressions.Add("04408d266", "NoExpression");
                Expressions.Add("04329d100", "NoExpression");
                Expressions.Add("04600d245", "NoExpression");
                Expressions.Add("04801d60", "NoExpression");
                Expressions.Add("04424d221", "NoExpression");
                Expressions.Add("04756d67", "NoExpression");
                Expressions.Add("04535d213", "NoExpression");
                Expressions.Add("04397d334", "NoExpression");
                Expressions.Add("04603d135", "NoExpression");
                Expressions.Add("04815d80", "NoExpression");
                Expressions.Add("04400d296", "NoExpression");
                Expressions.Add("04632d132", "NoExpression");
                Expressions.Add("04472d222", "NoExpression");
                Expressions.Add("04222d391", "NoExpression");
                Expressions.Add("04540d253", "NoExpression");
                Expressions.Add("04843d71", "NoExpression");
                Expressions.Add("04217d399", "NoExpression");
                Expressions.Add("04777d74", "NoExpression");
                Expressions.Add("04778d48", "NoExpression");
                Expressions.Add("04279d283", "NoExpression");
                Expressions.Add("04433d180", "NoExpression");
                Expressions.Add("04334d302", "NoExpression");
                Expressions.Add("02463d548", "NoExpression");
                Expressions.Add("04695d68", "NoExpression");
                Expressions.Add("04670d163", "NoExpression");
                Expressions.Add("04836d43", "NoExpression");
                Expressions.Add("04745d72", "NoExpression");
                Expressions.Add("04556d307", "NoExpression");
                Expressions.Add("04418d287", "NoExpression");
                Expressions.Add("04633d180", "NoExpression");
                Expressions.Add("04482d304", "NoExpression");
                Expressions.Add("04299d191", "NoExpression");
                Expressions.Add("04484d185", "NoExpression");
                Expressions.Add("04473d185", "NoExpression");
                Expressions.Add("04343d321", "NoExpression");
                Expressions.Add("04830d80", "NoExpression");
                Expressions.Add("04349d314", "NoExpression");
                Expressions.Add("04749d74", "NoExpression");
                Expressions.Add("04385d325", "NoExpression");
                Expressions.Add("04237d141", "NoExpression");
                Expressions.Add("04449d173", "NoExpression");
                Expressions.Add("04719d80", "NoExpression");
                Expressions.Add("04322d130", "NoExpression");
                Expressions.Add("04287d45", "NoExpression");
                Expressions.Add("04508d79", "NoExpression");
                Expressions.Add("04735d30", "NoExpression");
                Expressions.Add("04471d265", "NoExpression");
                Expressions.Add("04261d299", "NoExpression");
                Expressions.Add("04288d254", "NoExpression");
                Expressions.Add("04453d287", "NoExpression");
                Expressions.Add("04785d68", "NoExpression");
                Expressions.Add("04774d68", "NoExpression");
                Expressions.Add("04662d121", "NoExpression");
                Expressions.Add("04750d50", "NoExpression");
                Expressions.Add("04423d190", "NoExpression");
                Expressions.Add("04297d263", "NoExpression");
                Expressions.Add("04221d431", "NoExpression");
                Expressions.Add("04409d137", "NoExpression");
                Expressions.Add("04481d289", "NoExpression");
                Expressions.Add("04596d78", "NoExpression");
                Expressions.Add("04201d370", "NoExpression");
                Expressions.Add("04645d87", "NoExpression");
                Expressions.Add("04776d30", "NoExpression");
                Expressions.Add("04446d271", "NoExpression");
                Expressions.Add("04797d44", "NoExpression");
                Expressions.Add("04683d231", "NoExpression");
                Expressions.Add("04714d80", "NoExpression");
                Expressions.Add("04629d138", "NoExpression");
                Expressions.Add("04782d79", "NoExpression");
                Expressions.Add("04622d234", "NoExpression");
                Expressions.Add("04560d267", "NoExpression");
                Expressions.Add("04684d226", "NoExpression");
                Expressions.Add("04239d378", "NoExpression");
                Expressions.Add("04664d145", "NoExpression");
                Expressions.Add("04580d293", "NoExpression");
                Expressions.Add("04311d228", "NoExpression");
                Expressions.Add("04427d266", "NoExpression");
                Expressions.Add("04631d168", "NoExpression");
                Expressions.Add("04406d86", "NoExpression");
                Expressions.Add("04336d293", "NoExpression");
                Expressions.Add("04379d282", "NoExpression");
                Expressions.Add("04430d273", "NoExpression");
                Expressions.Add("04609d94", "NoExpression");
                Expressions.Add("04697d80", "NoExpression");
                Expressions.Add("04681d147", "NoExpression");
                Expressions.Add("04768d68", "NoExpression");
                Expressions.Add("04598d253", "NoExpression");
                Expressions.Add("04407d263", "NoExpression");
                Expressions.Add("04568d91", "NoExpression");
                Expressions.Add("04803d74", "NoExpression");
                Expressions.Add("04742d80", "NoExpression");
                Expressions.Add("04701d68", "NoExpression");
                Expressions.Add("04667d196", "NoExpression");
                Expressions.Add("04758d61", "NoExpression");
                Expressions.Add("04575d294", "NoExpression");
                Expressions.Add("04429d333", "NoExpression");
                Expressions.Add("04236d156", "NoExpression");
                Expressions.Add("04203d438", "NoExpression");
                Expressions.Add("04451d245", "NoExpression");
                Expressions.Add("04581d194", "NoExpression");
                Expressions.Add("04470d291", "NoExpression");
                Expressions.Add("04344d245", "NoExpression");
                Expressions.Add("04605d241", "NoExpression");
                Expressions.Add("04757d73", "NoExpression");
                Expressions.Add("04832d80", "NoExpression");
                Expressions.Add("04286d263", "NoExpression");
                Expressions.Add("04226d359", "NoExpression");
                Expressions.Add("04324d278", "NoExpression");
                Expressions.Add("04273d246", "NoExpression");
                Expressions.Add("04727d66", "NoExpression");
                Expressions.Add("04422d240", "NoExpression");
                Expressions.Add("04657d156", "NoExpression");
                Expressions.Add("04839d80", "NoExpression");
                Expressions.Add("04647d160", "NoExpression");
                Expressions.Add("04687d111", "NoExpression");
                Expressions.Add("04606d178", "NoExpression");
                Expressions.Add("04595d89", "NoExpression");
                Expressions.Add("04395d194", "NoExpression");
                Expressions.Add("04693d48", "NoExpression");
                Expressions.Add("04725d24", "NoExpression");
                Expressions.Add("04537d322", "NoExpression");
                Expressions.Add("04593d194", "NoExpression");
                Expressions.Add("04818d36", "NoExpression");
                Expressions.Add("04531d285", "NoExpression");
                Expressions.Add("04628d221", "NoExpression");
                Expressions.Add("04588d131", "NoExpression");
                Expressions.Add("04734d80", "NoExpression");
                Expressions.Add("04637d196", "NoExpression");
                Expressions.Add("04638d193", "NoExpression");
                Expressions.Add("04488d282", "NoExpression");
                Expressions.Add("04463d201", "NoExpression");
                Expressions.Add("04505d218", "NoExpression");
                Expressions.Add("04476d120", "NoExpression");
                Expressions.Add("04219d415", "NoExpression");
                Expressions.Add("04233d392", "NoExpression");
                Expressions.Add("04213d280", "NoExpression");
                Expressions.Add("04546d73", "NoExpression");
                Expressions.Add("04764d26", "NoExpression");
                Expressions.Add("04689d26", "NoExpression");
                Expressions.Add("04372d271", "NoExpression");
                Expressions.Add("04509d276", "NoExpression");
                Expressions.Add("04309d163", "NoExpression");
                Expressions.Add("04428d243", "NoExpression");
                Expressions.Add("04350d260", "NoExpression");
                Expressions.Add("04512d322", "NoExpression");
                Expressions.Add("04696d38", "NoExpression");
                Expressions.Add("04559d312", "NoExpression");
                Expressions.Add("04365d320", "NoExpression");
                Expressions.Add("04369d248", "NoExpression");
                Expressions.Add("04472d224", "NoExpression");
                Expressions.Add("04327d292", "NoExpression");
                Expressions.Add("04385d327", "NoExpression");
                Expressions.Add("04418d289", "NoExpression");
                Expressions.Add("04485d286", "NoExpression");
                Expressions.Add("04754d76", "NoExpression");
                Expressions.Add("04644d200", "NoExpression");
                Expressions.Add("04329d102", "NoExpression");
                Expressions.Add("04434d156", "NoExpression");
                Expressions.Add("04435d340", "NoExpression");
                Expressions.Add("04495d311", "NoExpression");
                Expressions.Add("04470d293", "NoExpression");
                Expressions.Add("04530d317", "NoExpression");
                Expressions.Add("04730d54", "NoExpression");
                Expressions.Add("04705d42", "NoExpression");
                Expressions.Add("04394d297", "NoExpression");
                Expressions.Add("04507d295", "NoExpression");
                Expressions.Add("04222d393", "NoExpression");
                Expressions.Add("04531d287", "NoExpression");
                Expressions.Add("04320d272", "NoExpression");
                Expressions.Add("04381d110", "NoExpression");
                Expressions.Add("02463d550", "NoExpression");
                Expressions.Add("04778d50", "NoExpression");
                Expressions.Add("04652d156", "NoExpression");
                Expressions.Add("04632d134", "NoExpression");
                Expressions.Add("04650d148", "NoExpression");
                Expressions.Add("04807d06", "NoExpression");
                Expressions.Add("04775d76", "NoExpression");
                Expressions.Add("04456d271", "NoExpression");
                Expressions.Add("04301d244", "NoExpression");
                Expressions.Add("04388d287", "NoExpression");
                Expressions.Add("04577d286", "NoExpression");
                Expressions.Add("04540d255", "NoExpression");
                Expressions.Add("04447d127", "NoExpression");
                Expressions.Add("04514d322", "NoExpression");
                Expressions.Add("04773d82", "NoExpression");
                Expressions.Add("04408d268", "NoExpression");
                Expressions.Add("04782d81", "NoExpression");
                Expressions.Add("04756d69", "NoExpression");
                Expressions.Add("04424d223", "NoExpression");
                Expressions.Add("04419d254", "NoExpression");
                Expressions.Add("04796d76", "NoExpression");
                Expressions.Add("04629d140", "NoExpression");
                Expressions.Add("04683d233", "NoExpression");
                Expressions.Add("04461d295", "NoExpression");
                Expressions.Add("04580d295", "NoExpression");
                Expressions.Add("04633d182", "NoExpression");
                Expressions.Add("04593d196", "NoExpression");
                Expressions.Add("04603d137", "NoExpression");
                Expressions.Add("04535d215", "NoExpression");
                Expressions.Add("04682d120", "NoExpression");
                Expressions.Add("04471d267", "NoExpression");
                Expressions.Add("04221d433", "NoExpression");
                Expressions.Add("04557d333", "NoExpression");
                Expressions.Add("04202d442", "NoExpression");
                Expressions.Add("04350d262", "NoExpression");
                Expressions.Add("04667d198", "NoExpression");
                Expressions.Add("04830d82", "NoExpression");
                Expressions.Add("04670d165", "NoExpression");
                Expressions.Add("04815d82", "NoExpression");
                Expressions.Add("04553d232", "NoExpression");
                Expressions.Add("04347d293", "NoExpression");
                Expressions.Add("04661d169", "NoExpression");
                Expressions.Add("04585d186", "NoExpression");
                Expressions.Add("04777d76", "NoExpression");
                Expressions.Add("04556d309", "NoExpression");
                Expressions.Add("04484d187", "NoExpression");
                Expressions.Add("04349d316", "NoExpression");
                Expressions.Add("04725d26", "NoExpression");
                Expressions.Add("04343d323", "NoExpression");
                Expressions.Add("04286d265", "NoExpression");
                Expressions.Add("04749d76", "NoExpression");
                Expressions.Add("04805d56", "NoExpression");
                Expressions.Add("04698d80", "NoExpression");
                Expressions.Add("04321d110", "NoExpression");
                Expressions.Add("04440d91", "NoExpression");
                Expressions.Add("04237d143", "NoExpression");
                Expressions.Add("04745d74", "NoExpression");
                Expressions.Add("04449d175", "NoExpression");
                Expressions.Add("04261d301", "NoExpression");
                Expressions.Add("04662d123", "NoExpression");
                Expressions.Add("04750d52", "NoExpression");
                Expressions.Add("04719d82", "NoExpression");
                Expressions.Add("04581d196", "NoExpression");
                Expressions.Add("04774d70", "NoExpression");
                Expressions.Add("04203d440", "NoExpression");
                Expressions.Add("04400d298", "NoExpression");
                Expressions.Add("04714d82", "NoExpression");
                Expressions.Add("04615d78", "NoExpression");
                Expressions.Add("04322d132", "NoExpression");
                Expressions.Add("04341d155", "NoExpression");
                Expressions.Add("04739d06", "NoExpression");
                Expressions.Add("04344d247", "NoExpression");
                Expressions.Add("04386d159", "NoExpression");
                Expressions.Add("04446d273", "NoExpression");
                Expressions.Add("04225d295", "NoExpression");
                Expressions.Add("04560d269", "NoExpression");
                Expressions.Add("04427d268", "NoExpression");
                Expressions.Add("04217d401", "NoExpression");
                Expressions.Add("04684d228", "NoExpression");
                Expressions.Add("04477d103", "NoExpression");
                Expressions.Add("04609d96", "NoExpression");
                Expressions.Add("04481d291", "NoExpression");
                Expressions.Add("04334d304", "NoExpression");
                Expressions.Add("04776d32", "NoExpression");
                Expressions.Add("04675d251", "NoExpression");
                Expressions.Add("04797d46", "NoExpression");
                Expressions.Add("04605d243", "NoExpression");
                Expressions.Add("04598d255", "NoExpression");
                Expressions.Add("04697d82", "NoExpression");
                Expressions.Add("04757d75", "NoExpression");
                Expressions.Add("04832d82", "NoExpression");
                Expressions.Add("04681d149", "NoExpression");
                Expressions.Add("04370d227", "NoExpression");
                Expressions.Add("04373d54", "NoExpression");
                Expressions.Add("04219d417", "NoExpression");
                Expressions.Add("04297d265", "NoExpression");
                Expressions.Add("04768d70", "NoExpression");
                Expressions.Add("04493d204", "NoExpression");
                Expressions.Add("04694d06", "NoExpression");
                Expressions.Add("04579d260", "NoExpression");
                Expressions.Add("04816d06", "NoExpression");
                Expressions.Add("04311d230", "NoExpression");
                Expressions.Add("04430d275", "NoExpression");
                Expressions.Add("04575d296", "NoExpression");
                Expressions.Add("04429d335", "NoExpression");
                Expressions.Add("04645d89", "NoExpression");
                Expressions.Add("04201d372", "NoExpression");
                Expressions.Add("04742d82", "NoExpression");
                Expressions.Add("04265d263", "NoExpression");
                Expressions.Add("04379d284", "NoExpression");
                Expressions.Add("04646d41", "NoExpression");
                Expressions.Add("04508d81", "NoExpression");
                Expressions.Add("04735d32", "NoExpression");
                Expressions.Add("04755d18", "NoExpression");
                Expressions.Add("04482d306", "NoExpression");
                Expressions.Add("04839d82", "NoExpression");
                Expressions.Add("04387d324", "NoExpression");
                Expressions.Add("04298d67", "NoExpression");
                Expressions.Add("04803d76", "NoExpression");
                Expressions.Add("04701d70", "NoExpression");
                Expressions.Add("04239d380", "NoExpression");
                Expressions.Add("04687d113", "NoExpression");
                Expressions.Add("04740d12", "NoExpression");
                Expressions.Add("04559d314", "NoExpression");
                Expressions.Add("04365d322", "NoExpression");
                Expressions.Add("04789d14", "NoExpression");
                Expressions.Add("04664d147", "NoExpression");
                Expressions.Add("04628d223", "NoExpression");
                Expressions.Add("04512d324", "NoExpression");
                Expressions.Add("04641d173", "NoExpression");
                Expressions.Add("04606d180", "NoExpression");
                Expressions.Add("04411d190", "NoExpression");
                Expressions.Add("04395d196", "NoExpression");
                Expressions.Add("04273d248", "NoExpression");
                Expressions.Add("04513d299", "NoExpression");
                Expressions.Add("04444d208", "NoExpression");
                Expressions.Add("04758d63", "NoExpression");
                Expressions.Add("04397d336", "NoExpression");
                Expressions.Add("04727d68", "NoExpression");
                Expressions.Add("04721d42", "NoExpression");
                Expressions.Add("04509d278", "NoExpression");
                Expressions.Add("04834d06", "NoExpression");
                Expressions.Add("04336d295", "NoExpression");
                Expressions.Add("04300d218", "NoExpression");
                Expressions.Add("04372d273", "NoExpression");
                Expressions.Add("04436d310", "NoExpression");
                Expressions.Add("04589d242", "NoExpression");
                Expressions.Add("04463d203", "NoExpression");
                Expressions.Add("04488d284", "NoExpression");
                Expressions.Add("04537d324", "NoExpression");
                Expressions.Add("04568d93", "NoExpression");
                Expressions.Add("04588d133", "NoExpression");
                Expressions.Add("04473d187", "NoExpression");
                Expressions.Add("04734d82", "NoExpression");
                Expressions.Add("04324d280", "NoExpression");
                Expressions.Add("04801d62", "NoExpression");
                Expressions.Add("04233d394", "NoExpression");
                Expressions.Add("04505d220", "NoExpression");
                Expressions.Add("04407d265", "NoExpression");
                Expressions.Add("04460d260", "NoExpression");
                Expressions.Add("04764d28", "NoExpression");
                Expressions.Add("04546d75", "NoExpression");
                Expressions.Add("04309d165", "NoExpression");
                Expressions.Add("04569d282", "NoExpression");
                Expressions.Add("04288d256", "NoExpression");
                Expressions.Add("04287d47", "NoExpression");
                Expressions.Add("04602d104", "NoExpression");
                Expressions.Add("04385d329", "NoExpression");
                Expressions.Add("04299d193", "NoExpression");
                Expressions.Add("04782d83", "NoExpression");
                Expressions.Add("04530d319", "NoExpression");
                Expressions.Add("04386d161", "NoExpression");
                Expressions.Add("04626d235", "NoExpression");
                Expressions.Add("04495d313", "NoExpression");
                Expressions.Add("04507d297", "NoExpression");
                Expressions.Add("04319d190", "NoExpression");
                Expressions.Add("04485d288", "NoExpression");
                Expressions.Add("04265d265", "NoExpression");
                Expressions.Add("04683d235", "NoExpression");
                Expressions.Add("04447d129", "NoExpression");
                Expressions.Add("04202d444", "NoExpression");
                Expressions.Add("04652d158", "NoExpression");
                Expressions.Add("04650d150", "NoExpression");
                Expressions.Add("04714d84", "NoExpression");
                Expressions.Add("04387d326", "NoExpression");
                Expressions.Add("04770d26", "NoExpression");
                Expressions.Add("04397d338", "NoExpression");
                Expressions.Add("04472d226", "NoExpression");
                Expressions.Add("04673d188", "NoExpression");
                Expressions.Add("04661d171", "NoExpression");
                Expressions.Add("04585d188", "NoExpression");
                Expressions.Add("04301d246", "NoExpression");
                Expressions.Add("04336d297", "NoExpression");
                Expressions.Add("04388d289", "NoExpression");
                Expressions.Add("04577d288", "NoExpression");
                Expressions.Add("04773d84", "NoExpression");
                Expressions.Add("04408d270", "NoExpression");
                Expressions.Add("04644d202", "NoExpression");
                Expressions.Add("04756d71", "NoExpression");
                Expressions.Add("04424d225", "NoExpression");
                Expressions.Add("04633d184", "NoExpression");
                Expressions.Add("04629d142", "NoExpression");
                Expressions.Add("04361d177", "NoExpression");
                Expressions.Add("04777d78", "NoExpression");
                Expressions.Add("04628d225", "NoExpression");
                Expressions.Add("04580d297", "NoExpression");
                Expressions.Add("04815d84", "NoExpression");
                Expressions.Add("04603d139", "NoExpression");
                Expressions.Add("04470d295", "NoExpression");
                Expressions.Add("04852d60", "NoExpression");
                Expressions.Add("04557d335", "NoExpression");
                Expressions.Add("04300d220", "NoExpression");
                Expressions.Add("04667d200", "NoExpression");
                Expressions.Add("04502d54", "NoExpression");
                Expressions.Add("04506d196", "NoExpression");
                Expressions.Add("04600d247", "NoExpression");
                Expressions.Add("04682d122", "NoExpression");
                Expressions.Add("04423d192", "NoExpression");
                Expressions.Add("04707d52", "NoExpression");
                Expressions.Add("04418d291", "NoExpression");
                Expressions.Add("04618d162", "NoExpression");
                Expressions.Add("04496d244", "NoExpression");
                Expressions.Add("04479d224", "NoExpression");
                Expressions.Add("04670d167", "NoExpression");
                Expressions.Add("04836d45", "NoExpression");
                Expressions.Add("04473d189", "NoExpression");
                Expressions.Add("04537d326", "NoExpression");
                Expressions.Add("04686d49", "NoExpression");
                Expressions.Add("04410d182", "NoExpression");
                Expressions.Add("04456d273", "NoExpression");
                Expressions.Add("04349d318", "NoExpression");
                Expressions.Add("04814d18", "NoExpression");
                Expressions.Add("04717d41", "NoExpression");
                Expressions.Add("04436d312", "NoExpression");
                Expressions.Add("04589d244", "NoExpression");
                Expressions.Add("04477d105", "NoExpression");
                Expressions.Add("04775d78", "NoExpression");
                Expressions.Add("04806d36", "NoExpression");
                Expressions.Add("04347d295", "NoExpression");
                Expressions.Add("04320d274", "NoExpression");
                Expressions.Add("04556d311", "NoExpression");
                Expressions.Add("04695d70", "NoExpression");
                Expressions.Add("04830d84", "NoExpression");
                Expressions.Add("04237d145", "NoExpression");
                Expressions.Add("04749d78", "NoExpression");
                Expressions.Add("04698d82", "NoExpression");
                Expressions.Add("04321d112", "NoExpression");
                Expressions.Add("04805d58", "NoExpression");
                Expressions.Add("04745d76", "NoExpression");
                Expressions.Add("04343d325", "NoExpression");
                Expressions.Add("04569d284", "NoExpression");
                Expressions.Add("04327d294", "NoExpression");
                Expressions.Add("04370d229", "NoExpression");
                Expressions.Add("04373d56", "NoExpression");
                Expressions.Add("04719d84", "NoExpression");
                Expressions.Add("04322d134", "NoExpression");
                Expressions.Add("04287d49", "NoExpression");
                Expressions.Add("04378d203", "NoExpression");
                Expressions.Add("04225d297", "NoExpression");
                Expressions.Add("04785d70", "NoExpression");
                Expressions.Add("04662d125", "NoExpression");
                Expressions.Add("04622d236", "NoExpression");
                Expressions.Add("04222d395", "NoExpression");
                Expressions.Add("04217d403", "NoExpression");
                Expressions.Add("04435d342", "NoExpression");
                Expressions.Add("04440d93", "NoExpression");
                Expressions.Add("04846d72", "NoExpression");
                Expressions.Add("04482d308", "NoExpression");
                Expressions.Add("04394d299", "NoExpression");
                Expressions.Add("04560d271", "NoExpression");
                Expressions.Add("04531d289", "NoExpression");
                Expressions.Add("04631d170", "NoExpression");
                Expressions.Add("04641d175", "NoExpression");
                Expressions.Add("04766d24", "NoExpression");
                Expressions.Add("04760d76", "NoExpression");
                Expressions.Add("04763d66", "NoExpression");
                Expressions.Add("04595d91", "NoExpression");
                Expressions.Add("04481d293", "NoExpression");
                Expressions.Add("04818d38", "NoExpression");
                Expressions.Add("04797d48", "NoExpression");
                Expressions.Add("04697d84", "NoExpression");
                Expressions.Add("04684d230", "NoExpression");
                Expressions.Add("04593d198", "NoExpression");
                Expressions.Add("04753d18", "NoExpression");
                Expressions.Add("04598d257", "NoExpression");
                Expressions.Add("04513d301", "NoExpression");
                Expressions.Add("04514d324", "NoExpression");
                Expressions.Add("04681d151", "NoExpression");
                Expressions.Add("04519d206", "NoExpression");
                Expressions.Add("04768d72", "NoExpression");
                Expressions.Add("04226d361", "NoExpression");
                Expressions.Add("04579d262", "NoExpression");
                Expressions.Add("04609d98", "NoExpression");
                Expressions.Add("04446d275", "NoExpression");
                Expressions.Add("04379d286", "NoExpression");
                Expressions.Add("04221d435", "NoExpression");
                Expressions.Add("04350d264", "NoExpression");
                Expressions.Add("04742d84", "NoExpression");
                Expressions.Add("04605d245", "NoExpression");
                Expressions.Add("04334d306", "NoExpression");
                Expressions.Add("04512d326", "NoExpression");
                Expressions.Add("04776d34", "NoExpression");
                Expressions.Add("04581d198", "NoExpression");
                Expressions.Add("04596d80", "NoExpression");
                Expressions.Add("04274d176", "NoExpression");
                Expressions.Add("04782d85", "NoExpression");
                Expressions.Add("04805d60", "NoExpression");
                Expressions.Add("04286d267", "NoExpression");
                Expressions.Add("04698d84", "NoExpression");
                Expressions.Add("04508d83", "NoExpression");
                Expressions.Add("04735d34", "NoExpression");
                Expressions.Add("04287d51", "NoExpression");
                Expressions.Add("04809d48", "NoExpression");
                Expressions.Add("04602d106", "NoExpression");
                Expressions.Add("04822d48", "NoExpression");
                Expressions.Add("04485d290", "NoExpression");
                Expressions.Add("04824d54", "NoExpression");
                Expressions.Add("04774d72", "NoExpression");
                Expressions.Add("04754d78", "NoExpression");
                Expressions.Add("04423d194", "NoExpression");
                Expressions.Add("04203d442", "NoExpression");
                Expressions.Add("04225d299", "NoExpression");
                Expressions.Add("04477d107", "NoExpression");
                Expressions.Add("04714d86", "NoExpression");
                Expressions.Add("04683d237", "NoExpression");
                Expressions.Add("04765d54", "NoExpression");
                Expressions.Add("04482d310", "NoExpression");
                Expressions.Add("04681d153", "NoExpression");
                Expressions.Add("04349d320", "NoExpression");
                Expressions.Add("04730d56", "NoExpression");
                Expressions.Add("04670d169", "NoExpression");
                Expressions.Add("04479d226", "NoExpression");
                Expressions.Add("04507d299", "NoExpression");
                Expressions.Add("04530d321", "NoExpression");
                Expressions.Add("04418d293", "NoExpression");
                Expressions.Add("04495d315", "NoExpression");
                Expressions.Add("04652d160", "NoExpression");
                Expressions.Add("04650d152", "NoExpression");
                Expressions.Add("04265d267", "NoExpression");
                Expressions.Add("04327d296", "NoExpression");
                Expressions.Add("04461d297", "NoExpression");
                Expressions.Add("04300d222", "NoExpression");
                Expressions.Add("04513d303", "NoExpression");
                Expressions.Add("04394d301", "NoExpression");
                Expressions.Add("04705d44", "NoExpression");
                Expressions.Add("04385d331", "NoExpression");
                Expressions.Add("04221d437", "NoExpression");
                Expressions.Add("04301d248", "NoExpression");
                Expressions.Add("04388d291", "NoExpression");
                Expressions.Add("04577d290", "NoExpression");
                Expressions.Add("04512d328", "NoExpression");
                Expressions.Add("04347d297", "NoExpression");
                Expressions.Add("04669d47", "NoExpression");
                Expressions.Add("04456d275", "NoExpression");
                Expressions.Add("04778d52", "NoExpression");
                Expressions.Add("04408d272", "NoExpression");
                Expressions.Add("04472d228", "NoExpression");
                Expressions.Add("04603d141", "NoExpression");
                Expressions.Add("04852d62", "NoExpression");
                Expressions.Add("04600d249", "NoExpression");
                Expressions.Add("04329d104", "NoExpression");
                Expressions.Add("04773d86", "NoExpression");
                Expressions.Add("04715d12", "NoExpression");
                Expressions.Add("04813d42", "NoExpression");
                Expressions.Add("04756d73", "NoExpression");
                Expressions.Add("04473d191", "NoExpression");
                Expressions.Add("04537d328", "NoExpression");
                Expressions.Add("04319d192", "NoExpression");
                Expressions.Add("04815d86", "NoExpression");
                Expressions.Add("04514d326", "NoExpression");
                Expressions.Add("04744d48", "NoExpression");
                Expressions.Add("04851d54", "NoExpression");
                Expressions.Add("04580d299", "NoExpression");
                Expressions.Add("04818d40", "NoExpression");
                Expressions.Add("04397d340", "NoExpression");
                Expressions.Add("04632d136", "NoExpression");
                Expressions.Add("04471d269", "NoExpression");
                Expressions.Add("04361d179", "NoExpression");
                Expressions.Add("04626d237", "NoExpression");
                Expressions.Add("04334d308", "NoExpression");
                Expressions.Add("04435d344", "NoExpression");
                Expressions.Add("04506d198", "NoExpression");
                Expressions.Add("04718d18", "NoExpression");
                Expressions.Add("04700d18", "NoExpression");
                Expressions.Add("04557d337", "NoExpression");
                Expressions.Add("04378d205", "NoExpression");
                Expressions.Add("04629d144", "NoExpression");
                Expressions.Add("04836d47", "NoExpression");
                Expressions.Add("04202d446", "NoExpression");
                Expressions.Add("04496d246", "NoExpression");
                Expressions.Add("04830d86", "NoExpression");
                Expressions.Add("04758d65", "NoExpression");
                Expressions.Add("04484d189", "NoExpression");
                Expressions.Add("04709d54", "NoExpression");
                Expressions.Add("04847d54", "NoExpression");
                Expressions.Add("04842d54", "NoExpression");
                Expressions.Add("04560d273", "NoExpression");
                Expressions.Add("04806d38", "NoExpression");
                Expressions.Add("04717d43", "NoExpression");
                Expressions.Add("04386d163", "NoExpression");
                Expressions.Add("04814d20", "NoExpression");
                Expressions.Add("04699d42", "NoExpression");
                Expressions.Add("04593d200", "NoExpression");
                Expressions.Add("04682d124", "NoExpression");
                Expressions.Add("04775d80", "NoExpression");
                Expressions.Add("04615d80", "NoExpression");
                Expressions.Add("04339d226", "NoExpression");
                Expressions.Add("04777d80", "NoExpression");
                Expressions.Add("04585d190", "NoExpression");
                Expressions.Add("04644d204", "NoExpression");
                Expressions.Add("04661d173", "NoExpression");
                Expressions.Add("04719d86", "NoExpression");
                Expressions.Add("04604d96", "NoExpression");
                Expressions.Add("04745d78", "NoExpression");
                Expressions.Add("04651d128", "NoExpression");
                Expressions.Add("04749d80", "NoExpression");
                Expressions.Add("04321d114", "NoExpression");
                Expressions.Add("04343d327", "NoExpression");
                Expressions.Add("04379d288", "NoExpression");
                Expressions.Add("04201d374", "NoExpression");
                Expressions.Add("04470d297", "NoExpression");
                Expressions.Add("02463d552", "NoExpression");
                Expressions.Add("04556d313", "NoExpression");
                Expressions.Add("04440d95", "NoExpression");
                Expressions.Add("04395d198", "NoExpression");
                Expressions.Add("04542d112", "NoExpression");
                Expressions.Add("04597d105", "NoExpression");
                Expressions.Add("04776d36", "NoExpression");
                Expressions.Add("04446d277", "NoExpression");
                Expressions.Add("04742d86", "NoExpression");
                Expressions.Add("04219d419", "NoExpression");
                Expressions.Add("04790d42", "NoExpression");
                Expressions.Add("04553d234", "NoExpression");
                Expressions.Add("04239d382", "NoExpression");
                Expressions.Add("04595d93", "NoExpression");
                Expressions.Add("04848d42", "NoExpression");
                Expressions.Add("04769d06", "NoExpression");
                Expressions.Add("04631d172", "NoExpression");
                Expressions.Add("04667d202", "NoExpression");
                Expressions.Add("04436d314", "NoExpression");
                Expressions.Add("04753d20", "NoExpression");
                Expressions.Add("04589d246", "NoExpression");
                Expressions.Add("04684d232", "NoExpression");
                Expressions.Add("04826d24", "NoExpression");
                Expressions.Add("04217d405", "NoExpression");
                Expressions.Add("04708d54", "NoExpression");
                Expressions.Add("04785d72", "NoExpression");
                Expressions.Add("04406d88", "NoExpression");
                Expressions.Add("04336d299", "NoExpression");
                Expressions.Add("04701d72", "NoExpression");
                Expressions.Add("04641d177", "NoExpression");
                Expressions.Add("04803d78", "NoExpression");
                Expressions.Add("04505d222", "NoExpression");
                Expressions.Add("04451d247", "NoExpression");
                Expressions.Add("04581d200", "NoExpression");
                Expressions.Add("04622d238", "NoExpression");
                Expressions.Add("04645d91", "NoExpression");
                Expressions.Add("04817d06", "NoExpression");
                Expressions.Add("04273d250", "NoExpression");
                Expressions.Add("04839d84", "NoExpression");
                Expressions.Add("04732d06", "NoExpression");
                Expressions.Add("04693d50", "NoExpression");
                Expressions.Add("04430d277", "NoExpression");
                Expressions.Add("04687d115", "NoExpression");
                Expressions.Add("04819d12", "NoExpression");
                Expressions.Add("04427d270", "NoExpression");
                Expressions.Add("04400d300", "NoExpression");
                Expressions.Add("04846d74", "NoExpression");
                Expressions.Add("04502d56", "NoExpression");
                Expressions.Add("04664d149", "NoExpression");
                Expressions.Add("04750d54", "NoExpression");
                Expressions.Add("04662d127", "NoExpression");
                Expressions.Add("04596d82", "NoExpression");
                Expressions.Add("04796d78", "NoExpression");
                Expressions.Add("04419d256", "NoExpression");
                Expressions.Add("04311d232", "NoExpression");
                Expressions.Add("04444d210", "NoExpression");
                Expressions.Add("04374d211", "NoExpression");
                Expressions.Add("04411d192", "NoExpression");
                Expressions.Add("04288d258", "NoExpression");
                Expressions.Add("04531d291", "NoExpression");
                Expressions.Add("04642d48", "NoExpression");
                Expressions.Add("04309d167", "NoExpression");
                Expressions.Add("04721d44", "NoExpression");
                Expressions.Add("04690d42", "NoExpression");
                Expressions.Add("04792d41", "NoExpression");
                Expressions.Add("04605d247", "NoExpression");
                Expressions.Add("04519d208", "NoExpression");
                Expressions.Add("04760d78", "NoExpression");
                Expressions.Add("04840d53", "NoExpression");
                Expressions.Add("04829d42", "NoExpression");
                Expressions.Add("04488d286", "NoExpression");
                Expressions.Add("04233d396", "NoExpression");
                Expressions.Add("04801d64", "NoExpression");
                Expressions.Add("04213d282", "NoExpression");
                Expressions.Add("04757d77", "NoExpression");
                Expressions.Add("04832d84", "NoExpression");
                Expressions.Add("04476d122", "NoExpression");
                Expressions.Add("04369d250", "NoExpression");
                Expressions.Add("04763d68", "NoExpression");
                Expressions.Add("04504d85", "NoExpression");
                Expressions.Add("04372d275", "NoExpression");
                Expressions.Add("04689d28", "NoExpression");
                Expressions.Add("04637d198", "NoExpression");
                Expressions.Add("04638d195", "NoExpression");
                Expressions.Add("04727d70", "NoExpression");
                Expressions.Add("04696d40", "NoExpression");
                Expressions.Add("04433d182", "NoExpression");
                Expressions.Add("04279d285", "NoExpression");
                Expressions.Add("04324d282", "NoExpression");
                Expressions.Add("04298d69", "NoExpression");
                Expressions.Add("04559d316", "NoExpression");
                Expressions.Add("04365d324", "NoExpression");
                Expressions.Add("04797d50", "NoExpression");
                Expressions.Add("04509d280", "NoExpression");
                Expressions.Add("04350d266", "NoExpression");
                Expressions.Add("04755d20", "NoExpression");
                Expressions.Add("04707d54", "NoExpression");
                Expressions.Add("04633d186", "NoExpression");
                Expressions.Add("04327d298", "Happiness");
                Expressions.Add("04749d82", "Happiness");
                Expressions.Add("04805d62", "Happiness");
                Expressions.Add("04774d74", "Happiness");
                Expressions.Add("04673d190", "Happiness");
                Expressions.Add("04843d73", "Happiness");
                Expressions.Add("04378d207", "Happiness");
                Expressions.Add("04366d82", "Happiness");
                Expressions.Add("04767d36", "Happiness");
                Expressions.Add("04225d301", "Happiness");
                Expressions.Add("04641d179", "Happiness");
                Expressions.Add("04782d87", "Happiness");
                Expressions.Add("04713d06", "Happiness");
                Expressions.Add("04750d56", "Happiness");
                Expressions.Add("04502d58", "Happiness");
                Expressions.Add("04719d88", "Happiness");
                Expressions.Add("04840d55", "Happiness");
                Expressions.Add("04203d444", "Happiness");
                Expressions.Add("04796d80", "Happiness");
                Expressions.Add("04419d258", "Happiness");
                Expressions.Add("04394d303", "Happiness");
                Expressions.Add("04633d188", "Happiness");
                Expressions.Add("04388d293", "Happiness");
                Expressions.Add("04792d43", "Happiness");
                Expressions.Add("04507d301", "Happiness");
                Expressions.Add("04670d171", "Happiness");
                Expressions.Add("02463d554", "Happiness");
                Expressions.Add("04397d342", "Happiness");
                Expressions.Add("04470d299", "Happiness");
                Expressions.Add("04349d322", "Happiness");
                Expressions.Add("04222d397", "Happiness");
                Expressions.Add("04818d42", "Happiness");
                Expressions.Add("04682d126", "Happiness");
                Expressions.Add("04618d164", "Happiness");
                Expressions.Add("04530d323", "Happiness");
                Expressions.Add("04495d317", "Happiness");
                Expressions.Add("04810d42", "Happiness");
                Expressions.Add("04472d230", "Happiness");
                Expressions.Add("04430d279", "Happiness");
                Expressions.Add("04815d88", "Happiness");
                Expressions.Add("04577d292", "Happiness");
                Expressions.Add("04301d250", "Happiness");
                Expressions.Add("04408d274", "Happiness");
                Expressions.Add("04773d88", "Happiness");
                Expressions.Add("04603d143", "Happiness");
                Expressions.Add("04852d64", "Happiness");
                Expressions.Add("04683d239", "Happiness");
                Expressions.Add("04714d88", "Happiness");
                Expressions.Add("04461d299", "Happiness");
                Expressions.Add("04606d182", "Happiness");
                Expressions.Add("04619d161", "Happiness");
                Expressions.Add("04600d251", "Happiness");
                Expressions.Add("04329d106", "Happiness");
                Expressions.Add("04777d82", "Happiness");
                Expressions.Add("04580d301", "Happiness");
                Expressions.Add("04756d75", "Happiness");
                Expressions.Add("04650d154", "Happiness");
                Expressions.Add("04652d162", "Happiness");
                Expressions.Add("04851d56", "Happiness");
                Expressions.Add("04730d58", "Happiness");
                Expressions.Add("04823d48", "Happiness");
                Expressions.Add("04741d06", "Happiness");
                Expressions.Add("04844d06", "Happiness");
                Expressions.Add("04853d48", "Happiness");
                Expressions.Add("04708d56", "Happiness");
                Expressions.Add("04711d47", "Happiness");
                Expressions.Add("04822d50", "Happiness");
                Expressions.Add("04488d288", "Happiness");
                Expressions.Add("04629d146", "Happiness");
                Expressions.Add("04435d346", "Happiness");
                Expressions.Add("04418d295", "Happiness");
                Expressions.Add("04557d339", "Happiness");
                Expressions.Add("04798d18", "Happiness");
                Expressions.Add("04847d56", "Happiness");
                Expressions.Add("04842d56", "Happiness");
                Expressions.Add("04709d56", "Happiness");
                Expressions.Add("04456d277", "Happiness");
                Expressions.Add("04531d293", "Happiness");
                Expressions.Add("04785d74", "Happiness");
                Expressions.Add("04202d448", "Happiness");
                Expressions.Add("04496d248", "Happiness");
                Expressions.Add("04484d191", "Happiness");
                Expressions.Add("04473d193", "Happiness");
                Expressions.Add("04400d302", "Happiness");
                Expressions.Add("04320d276", "Happiness");
                Expressions.Add("04836d49", "Happiness");
                Expressions.Add("04385d333", "Happiness");
                Expressions.Add("04819d14", "Happiness");
                Expressions.Add("04735d36", "Happiness");
                Expressions.Add("04508d85", "Happiness");
                Expressions.Add("04775d82", "Happiness");
                Expressions.Add("04661d175", "Happiness");
                Expressions.Add("04626d239", "Happiness");
                Expressions.Add("04765d56", "Happiness");
                Expressions.Add("04395d200", "Happiness");
                Expressions.Add("04745d80", "Happiness");
                Expressions.Add("04766d26", "Happiness");
                Expressions.Add("04321d116", "Happiness");
                Expressions.Add("04830d88", "Happiness");
                Expressions.Add("04695d72", "Happiness");
                Expressions.Add("04237d149", "Happiness");
                Expressions.Add("04698d86", "Happiness");
                Expressions.Add("04343d329", "Happiness");
                Expressions.Add("04742d88", "Happiness");
                Expressions.Add("04344d249", "Happiness");
                Expressions.Add("04440d97", "Happiness");
                Expressions.Add("04556d315", "Happiness");
                Expressions.Add("04201d376", "Happiness");
                Expressions.Add("04645d93", "Happiness");
                Expressions.Add("04676d159", "Happiness");
                Expressions.Add("04794d24", "Happiness");
                Expressions.Add("04762d41", "Happiness");
                Expressions.Add("04702d48", "Happiness");
                Expressions.Add("04379d290", "Happiness");
                Expressions.Add("04797d52", "Happiness");
                Expressions.Add("04446d279", "Happiness");
                Expressions.Add("04537d330", "Happiness");
                Expressions.Add("04217d407", "Happiness");
                Expressions.Add("04726d42", "Happiness");
                Expressions.Add("04684d234", "Happiness");
                Expressions.Add("04261d303", "Happiness");
                Expressions.Add("04560d275", "Happiness");
                Expressions.Add("04790d44", "Happiness");
                Expressions.Add("04743d48", "Happiness");
                Expressions.Add("04451d249", "Happiness");
                Expressions.Add("04728d42", "Happiness");
                Expressions.Add("04811d48", "Happiness");
                Expressions.Add("04481d295", "Happiness");
                Expressions.Add("04831d48", "Happiness");
                Expressions.Add("04406d90", "Happiness");
                Expressions.Add("04336d301", "Happiness");
                Expressions.Add("04334d310", "Happiness");
                Expressions.Add("04485d292", "Happiness");
                Expressions.Add("04802d18", "Happiness");
                Expressions.Add("04667d204", "Happiness");
                Expressions.Add("04753d22", "Happiness");
                Expressions.Add("04761d42", "Happiness");
                Expressions.Add("04407d269", "Happiness");
                Expressions.Add("04841d48", "Happiness");
                Expressions.Add("04542d114", "Happiness");
                Expressions.Add("04838d48", "Happiness");
                Expressions.Add("04757d79", "Happiness");
                Expressions.Add("04832d86", "Happiness");
                Expressions.Add("04697d86", "Happiness");
                Expressions.Add("04768d74", "Happiness");
                Expressions.Add("04825d12", "Happiness");
                Expressions.Add("04681d155", "Happiness");
                Expressions.Add("04373d58", "Happiness");
                Expressions.Add("04370d231", "Happiness");
                Expressions.Add("04675d253", "Happiness");
                Expressions.Add("04588d135", "Happiness");
                Expressions.Add("04734d84", "Happiness");
                Expressions.Add("04609d100", "Happiness");
                Expressions.Add("04605d249", "Happiness");
                Expressions.Add("04754d80", "Happiness");
                Expressions.Add("04429d339", "Happiness");
                Expressions.Add("04575d298", "Happiness");
                Expressions.Add("04535d217", "Happiness");
                Expressions.Add("04824d56", "Happiness");
                Expressions.Add("04436d316", "Happiness");
                Expressions.Add("04589d248", "Happiness");
                Expressions.Add("04622d240", "Happiness");
                Expressions.Add("04554d71", "Happiness");
                Expressions.Add("04279d287", "Happiness");
                Expressions.Add("04233d398", "Happiness");
                Expressions.Add("04763d70", "Happiness");
                Expressions.Add("04784d36", "Happiness");
                Expressions.Add("04839d86", "Happiness");
                Expressions.Add("04593d202", "Happiness");
                Expressions.Add("04387d328", "Happiness");
                Expressions.Add("04731d35", "Happiness");
                Expressions.Add("04596d84", "Happiness");
                Expressions.Add("04286d269", "Happiness");
                Expressions.Add("04727d72", "Happiness");
                Expressions.Add("04738d42", "Happiness");
                Expressions.Add("04477d109", "Happiness");
                Expressions.Add("04424d227", "Happiness");
                Expressions.Add("04751d40", "Happiness");
                Expressions.Add("04664d151", "Happiness");
                Expressions.Add("04854d48", "Happiness");
                Expressions.Add("04846d76", "Happiness");
                Expressions.Add("04833d48", "Happiness");
                Expressions.Add("04691d48", "Happiness");
                Expressions.Add("04631d174", "Happiness");
                Expressions.Add("04514d328", "Happiness");
                Expressions.Add("04806d40", "Happiness");
                Expressions.Add("04722d42", "Happiness");
                Expressions.Add("04381d112", "Happiness");
                Expressions.Add("04699d44", "Happiness");
                Expressions.Add("04812d42", "Happiness");
                Expressions.Add("04724d42", "Happiness");
                Expressions.Add("04760d80", "Happiness");
                Expressions.Add("04595d95", "Happiness");
                Expressions.Add("04298d71", "Happiness");
                Expressions.Add("04322d136", "Happiness");
                Expressions.Add("04569d286", "Happiness");
                Expressions.Add("04382d170", "Happiness");
                Expressions.Add("04581d202", "Happiness");
                Expressions.Add("04703d42", "Happiness");
                Expressions.Add("04837d06", "Happiness");
                Expressions.Add("04736d48", "Happiness");
                Expressions.Add("04712d47", "Happiness");
                Expressions.Add("04788d12", "Happiness");
                Expressions.Add("04752d06", "Happiness");
                Expressions.Add("04519d210", "Happiness");
                Expressions.Add("04845d06", "Happiness");
                Expressions.Add("04799d36", "Happiness");
                Expressions.Add("04644d206", "Happiness");
                Expressions.Add("04239d384", "Happiness");
                Expressions.Add("04693d52", "Happiness");
                Expressions.Add("04733d30", "Happiness");
                Expressions.Add("04808d30", "Happiness");
                Expressions.Add("04803d80", "Happiness");
                Expressions.Add("04339d228", "Happiness");
                Expressions.Add("04503d51", "Happiness");
                Expressions.Add("04513d305", "Happiness");
                Expressions.Add("04779d48", "Happiness");
                Expressions.Add("04690d44", "Happiness");
                Expressions.Add("04787d12", "Happiness");
                Expressions.Add("04221d439", "Happiness");
                Expressions.Add("04511d176", "Happiness");
                Expressions.Add("04632d138", "Happiness");
                Expressions.Add("04780d42", "Happiness");
                Expressions.Add("04795d36", "Happiness");
                Expressions.Add("04721d46", "Happiness");
                Expressions.Add("04309d169", "Happiness");
                Expressions.Add("04427d272", "Happiness");
                Expressions.Add("04453d291", "Happiness");
                Expressions.Add("04849d48", "Happiness");
                Expressions.Add("04696d42", "Happiness");
                Expressions.Add("04692d42", "Happiness");
                Expressions.Add("04737d36", "Happiness");
                Expressions.Add("04615d82", "Happiness");
                Expressions.Add("04509d282", "Happiness");
                Expressions.Add("04759d29", "Happiness");
                Expressions.Add("04827d42", "Happiness");
                Expressions.Add("04701d74", "Happiness");
                Expressions.Add("04476d124", "Happiness");
                Expressions.Add("04747d36", "Happiness");
                Expressions.Add("04746d42", "Happiness");
                Expressions.Add("04715d14", "Happiness");
                Expressions.Add("04428d245", "Happiness");
                Expressions.Add("04505d224", "Happiness");
                Expressions.Add("04804d06", "Happiness");
                Expressions.Add("04704d18", "Happiness");
                Expressions.Add("04820d36", "Happiness");
                Expressions.Add("04288d260", "Happiness");
                Expressions.Add("04729d24", "Happiness");
                Expressions.Add("04786d48", "Happiness");
                Expressions.Add("04688d36", "Happiness");
                Expressions.Add("04311d234", "Happiness");
                Expressions.Add("04821d42", "Happiness");
                Expressions.Add("04771d06", "Happiness");
                Expressions.Add("04720d36", "Happiness");
                Expressions.Add("04772d06", "Happiness");
                Expressions.Add("04835d06", "Happiness");
                Expressions.Add("04783d36", "Happiness");
                Expressions.Add("04848d44", "Happiness");
                Expressions.Add("04793d12", "Happiness");
                Expressions.Add("04791d12", "Happiness");
                Expressions.Add("04324d284", "Happiness");
                Expressions.Add("04433d184", "Happiness");
                Expressions.Add("04461d301", "Other");
                Expressions.Add("04485d294", "Other");
                Expressions.Add("04388d295", "Other");
                Expressions.Add("04782d89", "Other");
                Expressions.Add("04708d58", "Other");
                Expressions.Add("04349d324", "Other");
                Expressions.Add("04514d330", "Other");
                Expressions.Add("04747d38", "Other");
                Expressions.Add("04507d303", "Other");
                Expressions.Add("04670d173", "Other");
                Expressions.Add("04673d192", "Other");
                Expressions.Add("04418d297", "Other");
                Expressions.Add("04626d241", "Other");
                Expressions.Add("04447d131", "Other");
                Expressions.Add("04397d344", "Other");
                Expressions.Add("02463d556", "Other");
                Expressions.Add("04854d50", "Other");
                Expressions.Add("04477d111", "Other");
                Expressions.Add("04809d50", "Other");
                Expressions.Add("04301d252", "Other");
                Expressions.Add("04577d294", "Other");
                Expressions.Add("04408d276", "Other");
                Expressions.Add("04688d38", "Other");
                Expressions.Add("04801d66", "Other");
                Expressions.Add("04347d299", "Other");
                Expressions.Add("04629d148", "Other");
                Expressions.Add("04773d90", "Other");
                Expressions.Add("04847d58", "Other");
                Expressions.Add("04842d58", "Other");
                Expressions.Add("04424d229", "Other");
                Expressions.Add("04756d77", "Other");
                Expressions.Add("04619d163", "Other");
                Expressions.Add("04202d450", "Other");
                Expressions.Add("04667d206", "Other");
                Expressions.Add("04603d145", "Other");
                Expressions.Add("04852d66", "Other");
                Expressions.Add("04851d58", "Other");
                Expressions.Add("04823d50", "Other");
                Expressions.Add("04838d50", "Other");
                Expressions.Add("04488d290", "Other");
                Expressions.Add("04374d213", "Other");
                Expressions.Add("04815d90", "Other");
                Expressions.Add("04495d319", "Other");
                Expressions.Add("04557d341", "Other");
                Expressions.Add("04785d76", "Other");
                Expressions.Add("04506d200", "Other");
                Expressions.Add("04718d20", "Other");
                Expressions.Add("04700d20", "Other");
                Expressions.Add("04849d50", "Other");
                Expressions.Add("04833d50", "Other");
                Expressions.Add("04429d341", "Other");
                Expressions.Add("04530d325", "Other");
                Expressions.Add("04763d72", "Other");
                Expressions.Add("04760d82", "Other");
                Expressions.Add("04456d279", "Other");
                Expressions.Add("04722d44", "Other");
                Expressions.Add("04496d250", "Other");
                Expressions.Add("04830d90", "Other");
                Expressions.Add("04695d74", "Other");
                Expressions.Add("04484d193", "Other");
                Expressions.Add("04800d12", "Other");
                Expressions.Add("04843d75", "Other");
                Expressions.Add("04810d44", "Other");
                Expressions.Add("04846d78", "Other");
                Expressions.Add("04754d82", "Other");
                Expressions.Add("04848d46", "Other");
                Expressions.Add("04683d241", "Other");
                Expressions.Add("04698d88", "Other");
                Expressions.Add("04719d90", "Other");
                Expressions.Add("04343d331", "Other");
                Expressions.Add("04775d84", "Other");
                Expressions.Add("04745d82", "Other");
                Expressions.Add("04385d335", "Other");
                Expressions.Add("04502d60", "Other");
                Expressions.Add("04811d50", "Other");
                Expressions.Add("04749d84", "Other");
                Expressions.Add("04724d44", "Other");
                Expressions.Add("04300d224", "Other");
                Expressions.Add("04382d172", "Other");
                Expressions.Add("04737d38", "Other");
                Expressions.Add("04341d157", "Other");
                Expressions.Add("04580d303", "Other");
                Expressions.Add("04217d409", "Other");
                Expressions.Add("04632d140", "Other");
                Expressions.Add("04831d50", "Other");
                Expressions.Add("04774d76", "Other");
                Expressions.Add("04600d253", "Other");
                Expressions.Add("04378d209", "Other");
                Expressions.Add("04596d86", "Other");
                Expressions.Add("04203d446", "Other");
                Expressions.Add("04742d90", "Other");
                Expressions.Add("04556d317", "Other");
                Expressions.Add("04581d204", "Other");
                Expressions.Add("04628d227", "Other");
                Expressions.Add("04684d236", "Other");
                Expressions.Add("04379d292", "Other");
                Expressions.Add("04237d151", "Other");
                Expressions.Add("04451d251", "Other");
                Expressions.Add("04395d202", "Other");
                Expressions.Add("04622d242", "Other");
                Expressions.Add("04440d99", "Other");
                Expressions.Add("04286d271", "Other");
                Expressions.Add("04287d53", "Other");
                Expressions.Add("04644d208", "Other");
                Expressions.Add("04825d14", "Other");
                Expressions.Add("04693d54", "Other");
                Expressions.Add("04387d330", "Other");
                Expressions.Add("04808d32", "Other");
                Expressions.Add("04239d386", "Other");
                Expressions.Add("04473d195", "Other");
                Expressions.Add("04712d49", "Other");
                Expressions.Add("04320d278", "Other");
                Expressions.Add("04569d288", "Other");
                Expressions.Add("04738d44", "Other");
                Expressions.Add("04633d190", "Other");
                Expressions.Add("04631d176", "Other");
                Expressions.Add("04731d37", "Other");
                Expressions.Add("04821d44", "Other");
                Expressions.Add("04721d48", "Other");
                Expressions.Add("04427d274", "Other");
                Expressions.Add("04691d50", "Other");
                Expressions.Add("04435d348", "Other");
                Expressions.Add("04687d117", "Other");
                Expressions.Add("04740d14", "Other");
                Expressions.Add("04714d90", "Other");
                Expressions.Add("04298d73", "Other");
                Expressions.Add("04840d57", "Other");
                Expressions.Add("04839d88", "Other");
                Expressions.Add("04273d252", "Other");
                Expressions.Add("04820d38", "Other");
                Expressions.Add("04802d20", "Other");
                Expressions.Add("04803d82", "Other");
                Expressions.Add("04701d76", "Other");
                Expressions.Add("04792d45", "Other");
                Expressions.Add("04799d38", "Other");
                Expressions.Add("04796d82", "Other");
                Expressions.Add("04419d260", "Other");
                Expressions.Add("04372d277", "Other");
                Expressions.Add("04593d204", "Other");
                Expressions.Add("04505d226", "Other");
                Expressions.Add("04766d28", "Other");
                Expressions.Add("04829d44", "Other");
                Expressions.Add("04560d277", "Other");
                Expressions.Add("04519d212", "Other");
                Expressions.Add("04743d50", "Other");
                Expressions.Add("04780d44", "Other");
                Expressions.Add("04302d116", "Other");
                Expressions.Add("04309d171", "Other");
                Expressions.Add("04690d46", "Other");
                Expressions.Add("04553d236", "Other");
                Expressions.Add("04779d50", "Other");
                Expressions.Add("04453d293", "Other");
                Expressions.Add("04449d177", "Other");
                Expressions.Add("04509d284", "Other");
                Expressions.Add("04827d44", "Other");
                Expressions.Add("04759d31", "Other");
                Expressions.Add("04605d251", "Other");
                Expressions.Add("04324d286", "Other");
                Expressions.Add("04726d44", "Other");
                Expressions.Add("04841d50", "Other");
                Expressions.Add("04734d86", "Other");
                Expressions.Add("04850d30", "Other");
                Expressions.Add("04588d137", "Other");
                Expressions.Add("04568d95", "Other");
                Expressions.Add("04433d186", "Other");
                Expressions.Add("04705d46", "Other");
                Expressions.Add("04279d289", "Other");
                Expressions.Add("04813d44", "Other");
                Expressions.Add("04786d50", "Other");
                Expressions.Add("04508d87", "Other");
                Expressions.Add("04460d262", "Other");
                Expressions.Add("04233d400", "Other");
                Expressions.Add("04704d20", "Other");
                Expressions.Add("04542d116", "Other");
                Expressions.Add("04806d42", "Other");
                Expressions.Add("04643d20", "Other");
                Expressions.Add("04812d44", "Other");
                Expressions.Add("04575d300", "Other");
                Expressions.Add("04559d318", "Other");
                Expressions.Add("04735d38", "Other");
                Expressions.Add("04461d303", "Surprise");
                Expressions.Add("04427d276", "Surprise");
                Expressions.Add("04513d307", "Surprise");
                Expressions.Add("04423d196", "Surprise");
                Expressions.Add("04626d243", "Surprise");
                Expressions.Add("04782d91", "Surprise");
                Expressions.Add("04682d128", "Surprise");
                Expressions.Add("04507d305", "Surprise");
                Expressions.Add("04485d296", "Surprise");
                Expressions.Add("04670d175", "Surprise");
                Expressions.Add("04836d51", "Surprise");
                Expressions.Add("04202d452", "Surprise");
                Expressions.Add("02463d558", "Surprise");
                Expressions.Add("04418d299", "Surprise");
                Expressions.Add("04632d142", "Surprise");
                Expressions.Add("04435d350", "Surprise");
                Expressions.Add("04684d238", "Surprise");
                Expressions.Add("04615d84", "Surprise");
                Expressions.Add("04319d194", "Surprise");
                Expressions.Add("04667d208", "Surprise");
                Expressions.Add("04512d330", "Surprise");
                Expressions.Add("04809d52", "Surprise");
                Expressions.Add("04388d297", "Surprise");
                Expressions.Add("04385d337", "Surprise");
                Expressions.Add("04577d296", "Surprise");
                Expressions.Add("04301d254", "Surprise");
                Expressions.Add("04779d52", "Surprise");
                Expressions.Add("04778d54", "Surprise");
                Expressions.Add("04840d59", "Surprise");
                Expressions.Add("04815d92", "Surprise");
                Expressions.Add("04603d147", "Surprise");
                Expressions.Add("04347d301", "Surprise");
                Expressions.Add("04852d68", "Surprise");
                Expressions.Add("04777d84", "Surprise");
                Expressions.Add("04644d210", "Surprise");
                Expressions.Add("04801d68", "Surprise");
                Expressions.Add("04408d278", "Surprise");
                Expressions.Add("04329d108", "Surprise");
                Expressions.Add("04600d255", "Surprise");
                Expressions.Add("04225d303", "Surprise");
                Expressions.Add("04580d305", "Surprise");
                Expressions.Add("04730d60", "Surprise");
                Expressions.Add("04851d60", "Surprise");
                Expressions.Add("04711d49", "Surprise");
                Expressions.Add("04853d50", "Surprise");
                Expressions.Add("04265d269", "Surprise");
                Expressions.Add("04749d86", "Surprise");
                Expressions.Add("04633d192", "Surprise");
                Expressions.Add("04823d52", "Surprise");
                Expressions.Add("04343d333", "Surprise");
                Expressions.Add("04506d202", "Surprise");
                Expressions.Add("04718d22", "Surprise");
                Expressions.Add("04700d22", "Surprise");
                Expressions.Add("04606d184", "Surprise");
                Expressions.Add("04593d206", "Surprise");
                Expressions.Add("04843d77", "Surprise");
                Expressions.Add("04349d326", "Surprise");
                Expressions.Add("04683d243", "Surprise");
                Expressions.Add("04724d46", "Surprise");
                Expressions.Add("04846d80", "Surprise");
                Expressions.Add("04692d44", "Surprise");
                Expressions.Add("04471d271", "Surprise");
                Expressions.Add("04785d78", "Surprise");
                Expressions.Add("04699d46", "Surprise");
                Expressions.Add("04814d22", "Surprise");
                Expressions.Add("04775d86", "Surprise");
                Expressions.Add("04830d92", "Surprise");
                Expressions.Add("04495d321", "Surprise");
                Expressions.Add("04847d60", "Surprise");
                Expressions.Add("04842d60", "Surprise");
                Expressions.Add("04709d58", "Surprise");
                Expressions.Add("04717d45", "Surprise");
                Expressions.Add("04447d133", "Surprise");
                Expressions.Add("04838d52", "Surprise");
                Expressions.Add("04470d301", "Surprise");
                Expressions.Add("04477d113", "Surprise");
                Expressions.Add("04708d60", "Surprise");
                Expressions.Add("04745d84", "Surprise");
                Expressions.Add("04530d327", "Surprise");
                Expressions.Add("04743d52", "Surprise");
                Expressions.Add("04324d288", "Surprise");
                Expressions.Add("04237d153", "Surprise");
                Expressions.Add("04695d76", "Surprise");
                Expressions.Add("04805d64", "Surprise");
                Expressions.Add("04394d305", "Surprise");
                Expressions.Add("04698d90", "Surprise");
                Expressions.Add("04744d50", "Surprise");
                Expressions.Add("04818d44", "Surprise");
                Expressions.Add("04854d52", "Surprise");
                Expressions.Add("04810d46", "Surprise");
                Expressions.Add("04831d52", "Surprise");
                Expressions.Add("04774d78", "Surprise");
                Expressions.Add("04327d300", "Surprise");
                Expressions.Add("04429d343", "Surprise");
                Expressions.Add("04719d92", "Surprise");
                Expressions.Add("04553d238", "Surprise");
                Expressions.Add("04754d84", "Surprise");
                Expressions.Add("04400d304", "Surprise");
                Expressions.Add("04557d343", "Surprise");
                Expressions.Add("04645d95", "Surprise");
                Expressions.Add("04410d184", "Surprise");
                Expressions.Add("04707d60", "Surprise");
                Expressions.Add("04556d319", "Surprise");
                Expressions.Add("04297d267", "Surprise");
                Expressions.Add("04714d92", "Surprise");
                Expressions.Add("04203d448", "Surprise");
                Expressions.Add("04514d332", "Surprise");
                Expressions.Add("04822d52", "Surprise");
                Expressions.Add("04797d54", "Surprise");
                Expressions.Add("04409d139", "Surprise");
                Expressions.Add("04596d88", "Surprise");
                Expressions.Add("04727d74", "Surprise");
                Expressions.Add("04736d50", "Surprise");
                Expressions.Add("04703d44", "Surprise");
                Expressions.Add("04314d53", "Surprise");
                Expressions.Add("04790d46", "Surprise");
                Expressions.Add("04811d52", "Surprise");
                Expressions.Add("04373d60", "Surprise");
                Expressions.Add("04370d233", "Surprise");
                Expressions.Add("04451d253", "Surprise");
                Expressions.Add("04496d252", "Surprise");
                Expressions.Add("04560d279", "Surprise");
                Expressions.Add("04406d92", "Surprise");
                Expressions.Add("04761d44", "Surprise");
                Expressions.Add("04758d67", "Surprise");
                Expressions.Add("04675d255", "Surprise");
                Expressions.Add("04631d178", "Surprise");
                Expressions.Add("04776d38", "Surprise");
                Expressions.Add("04622d244", "Surprise");
                Expressions.Add("04598d259", "Surprise");
                Expressions.Add("04505d228", "Surprise");
                Expressions.Add("04697d88", "Surprise");
                Expressions.Add("04221d441", "Surprise");
                Expressions.Add("04628d229", "Surprise");
                Expressions.Add("04559d320", "Surprise");
                Expressions.Add("04365d328", "Surprise");
                Expressions.Add("04803d84", "Surprise");
                Expressions.Add("04788d14", "Surprise");
                Expressions.Add("04701d78", "Surprise");
                Expressions.Add("04605d253", "Surprise");
                Expressions.Add("04773d92", "Surprise");
                Expressions.Add("04344d251", "Surprise");
                Expressions.Add("04201d378", "Surprise");
                Expressions.Add("04726d46", "Surprise");
                Expressions.Add("04412d107", "Surprise");
                Expressions.Add("04338d80", "Surprise");
                Expressions.Add("04681d157", "Surprise");
                Expressions.Add("04765d58", "Surprise");
                Expressions.Add("04419d262", "Surprise");
                Expressions.Add("04839d90", "Surprise");
                Expressions.Add("04731d39", "Surprise");
                Expressions.Add("04286d273", "Surprise");
                Expressions.Add("04722d46", "Surprise");
                Expressions.Add("04738d46", "Surprise");
                Expressions.Add("04786d52", "Surprise");
                Expressions.Add("04712d51", "Surprise");
                Expressions.Add("04397d346", "Surprise");
                Expressions.Add("04747d40", "Surprise");
                Expressions.Add("04757d81", "Surprise");
                Expressions.Add("04832d88", "Surprise");
                Expressions.Add("04585d192", "Surprise");
                Expressions.Add("04661d177", "Surprise");
                Expressions.Add("04456d281", "Surprise");
                Expressions.Add("04482d312", "Surprise");
                Expressions.Add("04784d38", "Surprise");
                Expressions.Add("04702d50", "Surprise");
                Expressions.Add("04382d174", "Surprise");
                Expressions.Add("04737d40", "Surprise");
                Expressions.Add("04629d150", "Surprise");
                Expressions.Add("04535d219", "Surprise");
                Expressions.Add("04662d131", "Surprise");
                Expressions.Add("04481d297", "Surprise");
                Expressions.Add("04750d58", "Surprise");
                Expressions.Add("04693d56", "Surprise");
                Expressions.Add("04848d48", "Surprise");
                Expressions.Add("04850d32", "Surprise");
                Expressions.Add("04808d34", "Surprise");
                Expressions.Add("04395d204", "Surprise");
                Expressions.Add("04444d212", "Surprise");
                Expressions.Add("04688d40", "Surprise");
                Expressions.Add("04387d332", "Surprise");
                Expressions.Add("04239d388", "Surprise");
                Expressions.Add("04473d197", "Surprise");
                Expressions.Add("04569d290", "Surprise");
                Expressions.Add("04397d348", "Disgust");
                Expressions.Add("04320d280", "Disgust");
                Expressions.Add("04321d118", "Disgust");
                Expressions.Add("04745d86", "Disgust");
                Expressions.Add("04767d38", "Disgust");
                Expressions.Add("04203d450", "Disgust");
                Expressions.Add("04831d54", "Disgust");
                Expressions.Add("04737d42", "Disgust");
                Expressions.Add("04382d176", "Disgust");
                Expressions.Add("04754d86", "Disgust");
                Expressions.Add("04557d345", "Disgust");
                Expressions.Add("04472d232", "Disgust");
                Expressions.Add("04217d411", "Disgust");
                Expressions.Add("04423d198", "Disgust");
                Expressions.Add("04447d135", "Disgust");
                Expressions.Add("04714d94", "Disgust");
                Expressions.Add("02463d560", "Disgust");
                Expressions.Add("04836d53", "Disgust");
                Expressions.Add("04782d93", "Disgust");
                Expressions.Add("04427d278", "Disgust");
                Expressions.Add("04481d299", "Disgust");
                Expressions.Add("04349d328", "Disgust");
                Expressions.Add("04202d454", "Disgust");
                Expressions.Add("04507d307", "Disgust");
                Expressions.Add("04778d56", "Disgust");
                Expressions.Add("04495d323", "Disgust");
                Expressions.Add("04530d329", "Disgust");
                Expressions.Add("04810d48", "Disgust");
                Expressions.Add("04381d114", "Disgust");
                Expressions.Add("04580d307", "Disgust");
                Expressions.Add("04684d240", "Disgust");
                Expressions.Add("04334d312", "Disgust");
                Expressions.Add("04513d309", "Disgust");
                Expressions.Add("04593d208", "Disgust");
                Expressions.Add("04477d115", "Disgust");
                Expressions.Add("04512d332", "Disgust");
                Expressions.Add("04626d245", "Disgust");
                Expressions.Add("04809d54", "Disgust");
                Expressions.Add("04388d299", "Disgust");
                Expressions.Add("04577d298", "Disgust");
                Expressions.Add("04779d54", "Disgust");
                Expressions.Add("04225d305", "Disgust");
                Expressions.Add("04777d86", "Disgust");
                Expressions.Add("04765d60", "Disgust");
                Expressions.Add("04801d70", "Disgust");
                Expressions.Add("04408d280", "Disgust");
                Expressions.Add("04319d196", "Disgust");
                Expressions.Add("04301d256", "Disgust");
                Expressions.Add("04773d94", "Disgust");
                Expressions.Add("04633d194", "Disgust");
                Expressions.Add("04756d79", "Disgust");
                Expressions.Add("04424d231", "Disgust");
                Expressions.Add("04329d110", "Disgust");
                Expressions.Add("04708d62", "Disgust");
                Expressions.Add("04852d70", "Disgust");
                Expressions.Add("04603d149", "Disgust");
                Expressions.Add("04815d94", "Disgust");
                Expressions.Add("04461d305", "Disgust");
                Expressions.Add("04619d165", "Disgust");
                Expressions.Add("04843d79", "Disgust");
                Expressions.Add("04851d62", "Disgust");
                Expressions.Add("04853d52", "Disgust");
                Expressions.Add("04629d152", "Disgust");
                Expressions.Add("04428d247", "Disgust");
                Expressions.Add("04823d54", "Disgust");
                Expressions.Add("04347d303", "Disgust");
                Expressions.Add("04833d52", "Disgust");
                Expressions.Add("04488d292", "Disgust");
                Expressions.Add("04818d46", "Disgust");
                Expressions.Add("04838d54", "Disgust");
                Expressions.Add("04407d271", "Disgust");
                Expressions.Add("04749d88", "Disgust");
                Expressions.Add("04361d181", "Disgust");
                Expressions.Add("04846d82", "Disgust");
                Expressions.Add("04744d52", "Disgust");
                Expressions.Add("04683d245", "Disgust");
                Expressions.Add("04632d144", "Disgust");
                Expressions.Add("04422d242", "Disgust");
                Expressions.Add("04746d44", "Disgust");
                Expressions.Add("04730d62", "Disgust");
                Expressions.Add("04785d80", "Disgust");
                Expressions.Add("04496d254", "Disgust");
                Expressions.Add("04435d352", "Disgust");
                Expressions.Add("04847d62", "Disgust");
                Expressions.Add("04585d194", "Disgust");
                Expressions.Add("04470d303", "Disgust");
                Expressions.Add("04717d47", "Disgust");
                Expressions.Add("04842d62", "Disgust");
                Expressions.Add("04709d60", "Disgust");
                Expressions.Add("04339d230", "Disgust");
                Expressions.Add("04784d40", "Disgust");
                Expressions.Add("04429d345", "Disgust");
                Expressions.Add("04537d332", "Disgust");
                Expressions.Add("04821d46", "Disgust");
                Expressions.Add("04297d269", "Disgust");
                Expressions.Add("04300d226", "Disgust");
                Expressions.Add("04711d51", "Disgust");
                Expressions.Add("04385d339", "Disgust");
                Expressions.Add("04725d28", "Disgust");
                Expressions.Add("04719d94", "Disgust");
                Expressions.Add("04394d307", "Disgust");
                Expressions.Add("04343d335", "Disgust");
                Expressions.Add("04628d231", "Disgust");
                Expressions.Add("04237d155", "Disgust");
                Expressions.Add("04830d94", "Disgust");
                Expressions.Add("04482d314", "Disgust");
                Expressions.Add("04698d92", "Disgust");
                Expressions.Add("04695d78", "Disgust");
                Expressions.Add("04379d294", "Disgust");
                Expressions.Add("04556d321", "Disgust");
                Expressions.Add("04531d295", "Disgust");
                Expressions.Add("04560d281", "Disgust");
                Expressions.Add("04790d48", "Disgust");
                Expressions.Add("04736d52", "Disgust");
                Expressions.Add("04703d46", "Disgust");
                Expressions.Add("04635d47", "Disgust");
                Expressions.Add("04664d153", "Disgust");
                Expressions.Add("04742d92", "Disgust");
                Expressions.Add("04707d62", "Disgust");
                Expressions.Add("04720d38", "Disgust");
                Expressions.Add("04728d44", "Disgust");
                Expressions.Add("04406d94", "Disgust");
                Expressions.Add("04336d303", "Disgust");
                Expressions.Add("04535d221", "Disgust");
                Expressions.Add("04505d230", "Disgust");
                Expressions.Add("04761d46", "Disgust");
                Expressions.Add("04757d83", "Disgust");
                Expressions.Add("04832d90", "Disgust");
                Expressions.Add("04682d130", "Disgust");
                Expressions.Add("04598d261", "Disgust");
                Expressions.Add("04697d90", "Disgust");
                Expressions.Add("04681d159", "Disgust");
                Expressions.Add("04768d76", "Disgust");
                Expressions.Add("04221d443", "Disgust");
                Expressions.Add("04440d101", "Disgust");
                Expressions.Add("04236d158", "Disgust");
                Expressions.Add("04600d257", "Disgust");
                Expressions.Add("04605d255", "Disgust");
                Expressions.Add("04822d54", "Disgust");
                Expressions.Add("04704d22", "Disgust");
                Expressions.Add("04722d48", "Disgust");
                Expressions.Add("04324d290", "Disgust");
                Expressions.Add("04400d306", "Disgust");
                Expressions.Add("04813d46", "Disgust");
                Expressions.Add("04712d53", "Disgust");
                Expressions.Add("04604d98", "Disgust");
                Expressions.Add("04651d130", "Disgust");
                Expressions.Add("04644d212", "Disgust");
                Expressions.Add("04811d54", "Disgust");
                Expressions.Add("04387d334", "Disgust");
                Expressions.Add("04738d48", "Disgust");
                Expressions.Add("04820d40", "Disgust");
                Expressions.Add("04286d275", "Disgust");
                Expressions.Add("04473d199", "Disgust");
                Expressions.Add("04622d246", "Disgust");
                Expressions.Add("04724d48", "Disgust");
                Expressions.Add("04692d46", "Disgust");
                Expressions.Add("04775d88", "Disgust");
                Expressions.Add("04456d283", "Disgust");
                Expressions.Add("04411d194", "Disgust");
                Expressions.Add("04774d80", "Disgust");
                Expressions.Add("04676d161", "Disgust");
                Expressions.Add("04670d177", "Disgust");
                Expressions.Add("04783d38", "Disgust");
                Expressions.Add("04662d133", "Disgust");
                Expressions.Add("04702d52", "Disgust");
                Expressions.Add("04750d60", "Disgust");
                Expressions.Add("04631d180", "Disgust");
                Expressions.Add("04595d99", "Disgust");
                Expressions.Add("04606d186", "Disgust");
                Expressions.Add("04799d40", "Disgust");
                Expressions.Add("04273d254", "Disgust");
                Expressions.Add("04581d206", "Disgust");
                Expressions.Add("04839d92", "Disgust");
                Expressions.Add("04463d205", "Disgust");
                Expressions.Add("04751d46", "Disgust");
                Expressions.Add("04727d76", "Disgust");
                Expressions.Add("04848d50", "Disgust");
                Expressions.Add("04460d264", "Disgust");
                Expressions.Add("04780d46", "Disgust");
                Expressions.Add("04514d334", "Disgust");
                Expressions.Add("04762d43", "Disgust");
                Expressions.Add("04733d32", "Disgust");
                Expressions.Add("04239d390", "Disgust");
                Expressions.Add("04511d178", "Disgust");
                Expressions.Add("04309d173", "Disgust");
                Expressions.Add("04829d46", "Disgust");
                Expressions.Add("04395d206", "Disgust");
                Expressions.Add("04796d84", "Disgust");
                Expressions.Add("04476d126", "Disgust");
                Expressions.Add("04824d58", "Disgust");
                Expressions.Add("04827d46", "Disgust");
                Expressions.Add("04509d286", "Disgust");
                Expressions.Add("04854d54", "Disgust");
                Expressions.Add("04453d295", "Disgust");
                Expressions.Add("04369d252", "Disgust");
                Expressions.Add("04760d84", "Disgust");
                Expressions.Add("04849d52", "Disgust");
                Expressions.Add("04743d54", "Disgust");
                Expressions.Add("04786d54", "Disgust");
                Expressions.Add("04766d30", "Disgust");
                Expressions.Add("04691d52", "Disgust");
                Expressions.Add("04461d307", "Sadness");
                Expressions.Add("04754d88", "Sadness");
                Expressions.Add("04386d165", "Sadness");
                Expressions.Add("04670d179", "Sadness");
                Expressions.Add("04507d309", "Sadness");
                Expressions.Add("04684d242", "Sadness");
                Expressions.Add("04349d330", "Sadness");
                Expressions.Add("04588d139", "Sadness");
                Expressions.Add("04682d132", "Sadness");
                Expressions.Add("04334d314", "Sadness");
                Expressions.Add("04202d456", "Sadness");
                Expressions.Add("02463d562", "Sadness");
                Expressions.Add("04319d198", "Sadness");
                Expressions.Add("04615d86", "Sadness");
                Expressions.Add("04779d56", "Sadness");
                Expressions.Add("04394d309", "Sadness");
                Expressions.Add("04809d56", "Sadness");
                Expressions.Add("04418d301", "Sadness");
                Expressions.Add("04683d247", "Sadness");
                Expressions.Add("04430d281", "Sadness");
                Expressions.Add("04714d96", "Sadness");
                Expressions.Add("04577d300", "Sadness");
                Expressions.Add("04597d107", "Sadness");
                Expressions.Add("04619d167", "Sadness");
                Expressions.Add("04301d258", "Sadness");
                Expressions.Add("04435d354", "Sadness");
                Expressions.Add("04300d228", "Sadness");
                Expressions.Add("04408d282", "Sadness");
                Expressions.Add("04801d72", "Sadness");
                Expressions.Add("04782d95", "Sadness");
                Expressions.Add("04603d151", "Sadness");
                Expressions.Add("04815d96", "Sadness");
                Expressions.Add("04852d72", "Sadness");
                Expressions.Add("04730d64", "Sadness");
                Expressions.Add("04773d96", "Sadness");
                Expressions.Add("04756d81", "Sadness");
                Expressions.Add("04424d233", "Sadness");
                Expressions.Add("04329d112", "Sadness");
                Expressions.Add("04225d307", "Sadness");
                Expressions.Add("04626d247", "Sadness");
                Expressions.Add("04823d56", "Sadness");
                Expressions.Add("04580d309", "Sadness");
                Expressions.Add("04575d302", "Sadness");
                Expressions.Add("04711d53", "Sadness");
                Expressions.Add("04388d301", "Sadness");
                Expressions.Add("04708d64", "Sadness");
                Expressions.Add("04347d305", "Sadness");
                Expressions.Add("04422d244", "Sadness");
                Expressions.Add("04667d210", "Sadness");
                Expressions.Add("04722d50", "Sadness");
                Expressions.Add("04273d256", "Sadness");
                Expressions.Add("04593d210", "Sadness");
                Expressions.Add("04485d298", "Sadness");
                Expressions.Add("04847d64", "Sadness");
                Expressions.Add("04709d62", "Sadness");
                Expressions.Add("04842d64", "Sadness");
                Expressions.Add("04724d50", "Sadness");
                Expressions.Add("04481d301", "Sadness");
                Expressions.Add("04688d42", "Sadness");
                Expressions.Add("04470d305", "Sadness");
                Expressions.Add("04777d88", "Sadness");
                Expressions.Add("04557d347", "Sadness");
                Expressions.Add("04530d331", "Sadness");
                Expressions.Add("04784d42", "Sadness");
                Expressions.Add("04745d88", "Sadness");
                Expressions.Add("04838d56", "Sadness");
                Expressions.Add("04512d334", "Sadness");
                Expressions.Add("04843d81", "Sadness");
                Expressions.Add("04237d157", "Sadness");
                Expressions.Add("04344d253", "Sadness");
                Expressions.Add("04698d94", "Sadness");
                Expressions.Add("04695d80", "Sadness");
                Expressions.Add("04830d96", "Sadness");
                Expressions.Add("04495d325", "Sadness");
                Expressions.Add("04338d82", "Sadness");
                Expressions.Add("04749d90", "Sadness");
                Expressions.Add("04203d452", "Sadness");
                Expressions.Add("04596d90", "Sadness");
                Expressions.Add("04854d56", "Sadness");
                Expressions.Add("04767d40", "Sadness");
                Expressions.Add("04471d273", "Sadness");
                Expressions.Add("04531d297", "Sadness");
                Expressions.Add("04502d62", "Sadness");
                Expressions.Add("04831d56", "Sadness");
                Expressions.Add("04810d50", "Sadness");
                Expressions.Add("04427d280", "Sadness");
                Expressions.Add("04600d259", "Sadness");
                Expressions.Add("04840d63", "Sadness");
                Expressions.Add("04758d69", "Sadness");
                Expressions.Add("04774d82", "Sadness");
                Expressions.Add("04447d137", "Sadness");
                Expressions.Add("04775d90", "Sadness");
                Expressions.Add("04629d154", "Sadness");
                Expressions.Add("04379d296", "Sadness");
                Expressions.Add("04841d52", "Sadness");
                Expressions.Add("04742d94", "Sadness");
                Expressions.Add("04850d34", "Sadness");
                Expressions.Add("04559d322", "Sadness");
                Expressions.Add("04451d255", "Sadness");
                Expressions.Add("04622d248", "Sadness");
                Expressions.Add("04429d347", "Sadness");
                Expressions.Add("04633d196", "Sadness");
                Expressions.Add("04631d182", "Sadness");
                Expressions.Add("04482d316", "Sadness");
                Expressions.Add("04765d62", "Sadness");
                Expressions.Add("04598d263", "Sadness");
                Expressions.Add("04697d92", "Sadness");
                Expressions.Add("04761d48", "Sadness");
                Expressions.Add("04407d273", "Sadness");
                Expressions.Add("04641d181", "Sadness");
                Expressions.Add("04587d54", "Sadness");
                Expressions.Add("04446d281", "Sadness");
                Expressions.Add("04609d102", "Sadness");
                Expressions.Add("04444d214", "Sadness");
                Expressions.Add("04221d445", "Sadness");
                Expressions.Add("04373d62", "Sadness");
                Expressions.Add("04370d235", "Sadness");
                Expressions.Add("04436d318", "Sadness");
                Expressions.Add("04589d250", "Sadness");
                Expressions.Add("04236d160", "Sadness");
                Expressions.Add("04821d48", "Sadness");
                Expressions.Add("04628d233", "Sadness");
                Expressions.Add("04738d50", "Sadness");
                Expressions.Add("04387d336", "Sadness");
                Expressions.Add("04744d54", "Sadness");
                Expressions.Add("04662d135", "Sadness");
                Expressions.Add("04378d211", "Sadness");
                Expressions.Add("04717d49", "Sadness");
                Expressions.Add("04513d311", "Sadness");
                Expressions.Add("04477d117", "Sadness");
                Expressions.Add("04676d163", "Sadness");
                Expressions.Add("04664d155", "Sadness");
                Expressions.Add("04463d207", "Sadness");
                Expressions.Add("04693d58", "Sadness");
                Expressions.Add("04217d413", "Sadness");
                Expressions.Add("04785d82", "Sadness");
                Expressions.Add("04751d48", "Sadness");
                Expressions.Add("04733d34", "Sadness");
                Expressions.Add("04569d292", "Sadness");
                Expressions.Add("04286d277", "Sadness");
                Expressions.Add("04635d49", "Sadness");
                Expressions.Add("04848d52", "Sadness");
                Expressions.Add("04795d38", "Sadness");
                Expressions.Add("04829d48", "Sadness");
                Expressions.Add("04514d336", "Sadness");
                Expressions.Add("04309d175", "Sadness");
                Expressions.Add("04233d402", "Sadness");
                Expressions.Add("04727d78", "Sadness");
                Expressions.Add("04832d92", "Sadness");
                Expressions.Add("04757d85", "Sadness");
                Expressions.Add("04689d30", "Sadness");
                Expressions.Add("04476d128", "Sadness");
                Expressions.Add("04701d80", "Sadness");
                Expressions.Add("04803d86", "Sadness");
                Expressions.Add("04743d56", "Sadness");
                Expressions.Add("04759d35", "Sadness");
                Expressions.Add("04509d288", "Sadness");
                Expressions.Add("04731d43", "Sadness");
                Expressions.Add("04849d54", "Sadness");
                Expressions.Add("04833d54", "Sadness");
                Expressions.Add("04311d236", "Sadness");
                Expressions.Add("04737d44", "Sadness");
                Expressions.Add("04505d232", "Sadness");
                Expressions.Add("04699d48", "Sadness");
                Expressions.Add("04453d297", "Sadness");
                Expressions.Add("04746d46", "Sadness");
                Expressions.Add("04824d60", "Sadness");
                Expressions.Add("04792d47", "Sadness");
                Expressions.Add("04786d56", "Sadness");
                Expressions.Add("04460d266", "Sadness");
                Expressions.Add("04839d94", "Sadness");
                Expressions.Add("04542d118", "Sadness");
                Expressions.Add("04696d44", "Sadness");
                Expressions.Add("04535d223", "Sadness");
                Expressions.Add("04350d268", "Sadness");
                Expressions.Add("04813d48", "Sadness");
                Expressions.Add("04343d337", "Sadness");
                Expressions.Add("04319d264", "NoExpression");
                Expressions.Add("04682d214", "NoExpression");
                Expressions.Add("04615d178", "NoExpression");
                Expressions.Add("04385d431", "NoExpression");
                Expressions.Add("04470d397", "NoExpression");
                Expressions.Add("04797d144", "NoExpression");
                Expressions.Add("04418d386", "NoExpression");
                Expressions.Add("04397d444", "NoExpression");
                Expressions.Add("04334d410", "NoExpression");
                Expressions.Add("04921d40", "NoExpression");
                Expressions.Add("04288d292", "NoExpression");
                Expressions.Add("04495d421", "NoExpression");
                Expressions.Add("04575d394", "NoExpression");
                Expressions.Add("04595d143", "NoExpression");
                Expressions.Add("04823d134", "NoExpression");
                Expressions.Add("04853d146", "NoExpression");
                Expressions.Add("04851d158", "NoExpression");
                Expressions.Add("04711d149", "NoExpression");
                Expressions.Add("04600d351", "NoExpression");
                Expressions.Add("04273d288", "NoExpression");
                Expressions.Add("04842d158", "NoExpression");
                Expressions.Add("04847d160", "NoExpression");
                Expressions.Add("04670d269", "NoExpression");
                Expressions.Add("04899d78", "NoExpression");
                Expressions.Add("04580d405", "NoExpression");
                Expressions.Add("04833d150", "NoExpression");
                Expressions.Add("04782d191", "NoExpression");
                Expressions.Add("04777d182", "NoExpression");
                Expressions.Add("04737d68", "NoExpression");
                Expressions.Add("04691d122", "NoExpression");
                Expressions.Add("04894d64", "NoExpression");
                Expressions.Add("04843d151", "NoExpression");
                Expressions.Add("04387d425", "NoExpression");
                Expressions.Add("04744d142", "NoExpression");
                Expressions.Add("04846d140", "NoExpression");
                Expressions.Add("04763d152", "NoExpression");
                Expressions.Add("04429d443", "NoExpression");
                Expressions.Add("04768d144", "NoExpression");
                Expressions.Add("04787d50", "NoExpression");
                Expressions.Add("04708d159", "NoExpression");
                Expressions.Add("04726d132", "NoExpression");
                Expressions.Add("04881d90", "NoExpression");
                Expressions.Add("04865d90", "NoExpression");
                Expressions.Add("04593d260", "NoExpression");
                Expressions.Add("04866d84", "NoExpression");
                Expressions.Add("04603d247", "NoExpression");
                Expressions.Add("04372d331", "NoExpression");
                Expressions.Add("04267d149", "NoExpression");
                Expressions.Add("04557d443", "NoExpression");
                Expressions.Add("04447d157", "NoExpression");
                Expressions.Add("04370d295", "NoExpression");
                Expressions.Add("04675d339", "NoExpression");
                Expressions.Add("04709d153", "NoExpression");
                Expressions.Add("04692d136", "NoExpression");
                Expressions.Add("04856d84", "NoExpression");
                Expressions.Add("04297d305", "NoExpression");
                Expressions.Add("04556d409", "NoExpression");
                Expressions.Add("04596d166", "NoExpression");
                Expressions.Add("04683d339", "NoExpression");
                Expressions.Add("04784d66", "NoExpression");
                Expressions.Add("04361d195", "NoExpression");
                Expressions.Add("04512d428", "NoExpression");
                Expressions.Add("04514d432", "NoExpression");
                Expressions.Add("04609d191", "NoExpression");
                Expressions.Add("04347d387", "NoExpression");
                Expressions.Add("04631d254", "NoExpression");
                Expressions.Add("04629d244", "NoExpression");
                Expressions.Add("04758d125", "NoExpression");
                Expressions.Add("04724d134", "NoExpression");
                Expressions.Add("04849d126", "NoExpression");
                Expressions.Add("04485d394", "NoExpression");
                Expressions.Add("04460d326", "NoExpression");
                Expressions.Add("04461d403", "NoExpression");
                Expressions.Add("04481d395", "NoExpression");
                Expressions.Add("04427d362", "NoExpression");
                Expressions.Add("04885d46", "NoExpression");
                Expressions.Add("04754d176", "NoExpression");
                Expressions.Add("04832d140", "NoExpression");
                Expressions.Add("04848d136", "NoExpression");
                Expressions.Add("04203d536", "NoExpression");
                Expressions.Add("04382d190", "NoExpression");
                Expressions.Add("04475d114", "NoExpression");
                Expressions.Add("04917d88", "NoExpression");
                Expressions.Add("04687d131", "NoExpression");
                Expressions.Add("04320d340", "NoExpression");
                Expressions.Add("04400d376", "NoExpression");
                Expressions.Add("04626d343", "NoExpression");
                Expressions.Add("04641d241", "NoExpression");
                Expressions.Add("04531d387", "NoExpression");
                Expressions.Add("04714d186", "NoExpression");
                Expressions.Add("04598d353", "NoExpression");
                Expressions.Add("04632d230", "NoExpression");
                Expressions.Add("04581d247", "NoExpression");
                Expressions.Add("04633d288", "NoExpression");
                Expressions.Add("04745d166", "NoExpression");
                Expressions.Add("04221d541", "NoExpression");
                Expressions.Add("04537d410", "NoExpression");
                Expressions.Add("04530d425", "NoExpression");
                Expressions.Add("04810d119", "NoExpression");
                Expressions.Add("04869d52", "NoExpression");
                Expressions.Add("04924d66", "NoExpression");
                Expressions.Add("04451d311", "NoExpression");
                Expressions.Add("04217d455", "NoExpression");
                Expressions.Add("04815d192", "NoExpression");
                Expressions.Add("04667d302", "NoExpression");
                Expressions.Add("04653d36", "NoExpression");
                Expressions.Add("04327d392", "NoExpression");
                Expressions.Add("04394d395", "NoExpression");
                Expressions.Add("04284d53", "NoExpression");
                Expressions.Add("04493d224", "NoExpression");
                Expressions.Add("04472d314", "NoExpression");
                Expressions.Add("04919d06", "NoExpression");
                Expressions.Add("04841d140", "NoExpression");
                Expressions.Add("04776d95", "NoExpression");
                Expressions.Add("04349d406", "NoExpression");
                Expressions.Add("04830d140", "NoExpression");
                Expressions.Add("04507d402", "NoExpression");
                Expressions.Add("04916d82", "NoExpression");
                Expressions.Add("04511d240", "NoExpression");
                Expressions.Add("04884d53", "NoExpression");
                Expressions.Add("04736d70", "NoExpression");
                Expressions.Add("04703d112", "NoExpression");
                Expressions.Add("04928d90", "NoExpression");
                Expressions.Add("04379d357", "NoExpression");
                Expressions.Add("04742d106", "NoExpression");
                Expressions.Add("04336d393", "NoExpression");
                Expressions.Add("04560d376", "NoExpression");
                Expressions.Add("04831d152", "NoExpression");
                Expressions.Add("04892d88", "NoExpression");
                Expressions.Add("04900d88", "NoExpression");
                Expressions.Add("04225d396", "NoExpression");
                Expressions.Add("04312d207", "NoExpression");
                Expressions.Add("04780d100", "NoExpression");
                Expressions.Add("04202d552", "NoExpression");
                Expressions.Add("04488d384", "NoExpression");
                Expressions.Add("02463d652", "NoExpression");
                Expressions.Add("04482d406", "NoExpression");
                Expressions.Add("04261d329", "NoExpression");
                Expressions.Add("04813d138", "NoExpression");
                Expressions.Add("04878d78", "NoExpression");
                Expressions.Add("04765d156", "NoExpression");
                Expressions.Add("04792d65", "NoExpression");
                Expressions.Add("04449d245", "NoExpression");
                Expressions.Add("04302d142", "NoExpression");
                Expressions.Add("04875d06", "NoExpression");
                Expressions.Add("04909d06", "NoExpression");
                Expressions.Add("04902d06", "NoExpression");
                Expressions.Add("04695d98", "NoExpression");
                Expressions.Add("04476d212", "NoExpression");
                Expressions.Add("04286d367", "NoExpression");
                Expressions.Add("04719d177", "NoExpression");
                Expressions.Add("04929d42", "NoExpression");
                Expressions.Add("04915d48", "NoExpression");
                Expressions.Add("04701d153", "NoExpression");
                Expressions.Add("04773d179", "NoExpression");
                Expressions.Add("04446d365", "NoExpression");
                Expressions.Add("04613d176", "NoExpression");
                Expressions.Add("04721d80", "NoExpression");
                Expressions.Add("04409d165", "NoExpression");
                Expressions.Add("04430d367", "NoExpression");
                Expressions.Add("04838d152", "NoExpression");
                Expressions.Add("04785d94", "NoExpression");
                Expressions.Add("04931d06", "NoExpression");
                Expressions.Add("04585d266", "NoExpression");
                Expressions.Add("04859d24", "NoExpression");
                Expressions.Add("04839d176", "NoExpression");
                Expressions.Add("04854d140", "NoExpression");
                Expressions.Add("04309d245", "NoExpression");
                Expressions.Add("04697d178", "NoExpression");
                Expressions.Add("04239d480", "NoExpression");
                Expressions.Add("04505d322", "NoExpression");
                Expressions.Add("04801d80", "NoExpression");
                Expressions.Add("04727d162", "NoExpression");
                Expressions.Add("04746d85", "NoExpression");
                Expressions.Add("04605d279", "NoExpression");
                Expressions.Add("04731d131", "NoExpression");
                Expressions.Add("04862d06", "NoExpression");
                Expressions.Add("04509d384", "NoExpression");
                Expressions.Add("04734d182", "NoExpression");
                Expressions.Add("04738d92", "NoExpression");
                Expressions.Add("04886d06", "NoExpression");
                Expressions.Add("04783d74", "NoExpression");
                Expressions.Add("04889d51", "NoExpression");
                Expressions.Add("04891d75", "NoExpression");
                Expressions.Add("04343d427", "NoExpression");
                Expressions.Add("04794d38", "NoExpression");
                Expressions.Add("04762d93", "NoExpression");
                Expressions.Add("04233d498", "NoExpression");
                Expressions.Add("04388d373", "NoExpression");
                Expressions.Add("04751d60", "NoExpression");
                Expressions.Add("04733d48", "NoExpression");
                Expressions.Add("04860d06", "NoExpression");
                Expressions.Add("04861d06", "NoExpression");
                Expressions.Add("04803d164", "NoExpression");
                Expressions.Add("04867d72", "NoExpression");
                Expressions.Add("04319d266", "NoExpression");
                Expressions.Add("02463d654", "NoExpression");
                Expressions.Add("04682d216", "NoExpression");
                Expressions.Add("04856d86", "NoExpression");
                Expressions.Add("04840d148", "NoExpression");
                Expressions.Add("04615d180", "NoExpression");
                Expressions.Add("04347d389", "NoExpression");
                Expressions.Add("04888d36", "NoExpression");
                Expressions.Add("04577d342", "NoExpression");
                Expressions.Add("04388d375", "NoExpression");
                Expressions.Add("04313d56", "NoExpression");
                Expressions.Add("04871d58", "NoExpression");
                Expressions.Add("04575d396", "NoExpression");
                Expressions.Add("04612d63", "NoExpression");
                Expressions.Add("04370d297", "NoExpression");
                Expressions.Add("04851d160", "NoExpression");
                Expressions.Add("04853d148", "NoExpression");
                Expressions.Add("04729d50", "NoExpression");
                Expressions.Add("04288d294", "NoExpression");
                Expressions.Add("04394d397", "NoExpression");
                Expressions.Add("04408d360", "NoExpression");
                Expressions.Add("04430d369", "NoExpression");
                Expressions.Add("04495d423", "NoExpression");
                Expressions.Add("04621d89", "NoExpression");
                Expressions.Add("04691d124", "NoExpression");
                Expressions.Add("04894d66", "NoExpression");
                Expressions.Add("04581d249", "NoExpression");
                Expressions.Add("04593d262", "NoExpression");
                Expressions.Add("04603d249", "NoExpression");
                Expressions.Add("04419d318", "NoExpression");
                Expressions.Add("04796d138", "NoExpression");
                Expressions.Add("04631d256", "NoExpression");
                Expressions.Add("04300d258", "NoExpression");
                Expressions.Add("04609d193", "NoExpression");
                Expressions.Add("04511d242", "NoExpression");
                Expressions.Add("04865d92", "NoExpression");
                Expressions.Add("04881d92", "NoExpression");
                Expressions.Add("04744d144", "NoExpression");
                Expressions.Add("04708d161", "NoExpression");
                Expressions.Add("04884d55", "NoExpression");
                Expressions.Add("04557d445", "NoExpression");
                Expressions.Add("04632d232", "NoExpression");
                Expressions.Add("04822d144", "NoExpression");
                Expressions.Add("04815d194", "NoExpression");
                Expressions.Add("04711d151", "NoExpression");
                Expressions.Add("04512d430", "NoExpression");
                Expressions.Add("04481d397", "NoExpression");
                Expressions.Add("04580d407", "NoExpression");
                Expressions.Add("04385d433", "NoExpression");
                Expressions.Add("04482d408", "NoExpression");
                Expressions.Add("04673d273", "NoExpression");
                Expressions.Add("04907d76", "NoExpression");
                Expressions.Add("04878d80", "NoExpression");
                Expressions.Add("04202d554", "NoExpression");
                Expressions.Add("04239d482", "NoExpression");
                Expressions.Add("04626d345", "NoExpression");
                Expressions.Add("04831d154", "NoExpression");
                Expressions.Add("04787d52", "NoExpression");
                Expressions.Add("04727d164", "NoExpression");
                Expressions.Add("04903d30", "NoExpression");
                Expressions.Add("04560d378", "NoExpression");
                Expressions.Add("04395d265", "NoExpression");
                Expressions.Add("04485d396", "NoExpression");
                Expressions.Add("04514d434", "NoExpression");
                Expressions.Add("04683d341", "NoExpression");
                Expressions.Add("04714d188", "NoExpression");
                Expressions.Add("04344d335", "NoExpression");
                Expressions.Add("04832d142", "NoExpression");
                Expressions.Add("04848d138", "NoExpression");
                Expressions.Add("04885d48", "NoExpression");
                Expressions.Add("04754d178", "NoExpression");
                Expressions.Add("04427d364", "NoExpression");
                Expressions.Add("04866d86", "NoExpression");
                Expressions.Add("04556d411", "NoExpression");
                Expressions.Add("04726d134", "NoExpression");
                Expressions.Add("04699d106", "NoExpression");
                Expressions.Add("04644d254", "NoExpression");
                Expressions.Add("04382d192", "NoExpression");
                Expressions.Add("04446d367", "NoExpression");
                Expressions.Add("04899d80", "NoExpression");
                Expressions.Add("04537d412", "NoExpression");
                Expressions.Add("04475d116", "NoExpression");
                Expressions.Add("04203d538", "NoExpression");
                Expressions.Add("04821d102", "NoExpression");
                Expressions.Add("04629d246", "NoExpression");
                Expressions.Add("04784d68", "NoExpression");
                Expressions.Add("04823d136", "NoExpression");
                Expressions.Add("04843d153", "NoExpression");
                Expressions.Add("04667d304", "NoExpression");
                Expressions.Add("04786d142", "NoExpression");
                Expressions.Add("04221d543", "NoExpression");
                Expressions.Add("04763d154", "NoExpression");
                Expressions.Add("04916d84", "NoExpression");
                Expressions.Add("04530d427", "NoExpression");
                Expressions.Add("04810d121", "NoExpression");
                Expressions.Add("04869d54", "NoExpression");
                Expressions.Add("04858d06", "NoExpression");
                Expressions.Add("04912d06", "NoExpression");
                Expressions.Add("04924d68", "NoExpression");
                Expressions.Add("04451d313", "NoExpression");
                Expressions.Add("04891d77", "NoExpression");
                Expressions.Add("04600d353", "NoExpression");
                Expressions.Add("04896d88", "NoExpression");
                Expressions.Add("04936d88", "NoExpression");
                Expressions.Add("04670d271", "NoExpression");
                Expressions.Add("04418d388", "NoExpression");
                Expressions.Add("04588d231", "NoExpression");
                Expressions.Add("04436d360", "NoExpression");
                Expressions.Add("04589d268", "NoExpression");
                Expressions.Add("04286d369", "NoExpression");
                Expressions.Add("04225d398", "NoExpression");
                Expressions.Add("04596d168", "NoExpression");
                Expressions.Add("04404d209", "NoExpression");
                Expressions.Add("04900d90", "NoExpression");
                Expressions.Add("04712d97", "NoExpression");
                Expressions.Add("04918d12", "NoExpression");
                Expressions.Add("04904d36", "NoExpression");
                Expressions.Add("04692d138", "NoExpression");
                Expressions.Add("04476d214", "NoExpression");
                Expressions.Add("04641d243", "NoExpression");
                Expressions.Add("04932d36", "NoExpression");
                Expressions.Add("04926d12", "NoExpression");
                Expressions.Add("04217d457", "NoExpression");
                Expressions.Add("04493d226", "NoExpression");
                Expressions.Add("04453d357", "NoExpression");
                Expressions.Add("04334d412", "NoExpression");
                Expressions.Add("04470d399", "NoExpression");
                Expressions.Add("04765d158", "NoExpression");
                Expressions.Add("04461d405", "NoExpression");
                Expressions.Add("04846d142", "NoExpression");
                Expressions.Add("04488d386", "NoExpression");
                Expressions.Add("04747d100", "NoExpression");
                Expressions.Add("04675d341", "NoExpression");
                Expressions.Add("04633d290", "NoExpression");
                Expressions.Add("04261d331", "NoExpression");
                Expressions.Add("04743d134", "NoExpression");
                Expressions.Add("04728d92", "NoExpression");
                Expressions.Add("04782d193", "NoExpression");
                Expressions.Add("04472d316", "NoExpression");
                Expressions.Add("04449d247", "NoExpression");
                Expressions.Add("04758d127", "NoExpression");
                Expressions.Add("04302d144", "NoExpression");
                Expressions.Add("04507d404", "NoExpression");
                Expressions.Add("04724d136", "NoExpression");
                Expressions.Add("04777d184", "NoExpression");
                Expressions.Add("04702d126", "NoExpression");
                Expressions.Add("04929d44", "NoExpression");
                Expressions.Add("04833d152", "NoExpression");
                Expressions.Add("04915d50", "NoExpression");
                Expressions.Add("04324d346", "NoExpression");
                Expressions.Add("04701d155", "NoExpression");
                Expressions.Add("04770d46", "NoExpression");
                Expressions.Add("04798d78", "NoExpression");
                Expressions.Add("04773d181", "NoExpression");
                Expressions.Add("04927d36", "NoExpression");
                Expressions.Add("04803d166", "NoExpression");
                Expressions.Add("04585d268", "NoExpression");
                Expressions.Add("04519d232", "NoExpression");
                Expressions.Add("04349d408", "NoExpression");
                Expressions.Add("04839d178", "NoExpression");
                Expressions.Add("04343d429", "NoExpression");
                Expressions.Add("04813d140", "NoExpression");
                Expressions.Add("04883d84", "NoExpression");
                Expressions.Add("04874d24", "NoExpression");
                Expressions.Add("04734d184", "NoExpression");
                Expressions.Add("04397d446", "NoExpression");
                Expressions.Add("04777d186", "NoExpression");
                Expressions.Add("04334d414", "NoExpression");
                Expressions.Add("04840d150", "NoExpression");
                Expressions.Add("04888d38", "NoExpression");
                Expressions.Add("04881d94", "NoExpression");
                Expressions.Add("04511d244", "NoExpression");
                Expressions.Add("04884d57", "NoExpression");
                Expressions.Add("04336d395", "NoExpression");
                Expressions.Add("04631d258", "NoExpression");
                Expressions.Add("04851d162", "NoExpression");
                Expressions.Add("04853d150", "NoExpression");
                Expressions.Add("04724d138", "NoExpression");
                Expressions.Add("04575d398", "NoExpression");
                Expressions.Add("04626d347", "NoExpression");
                Expressions.Add("04847d162", "NoExpression");
                Expressions.Add("04842d160", "NoExpression");
                Expressions.Add("04430d371", "NoExpression");
                Expressions.Add("04797d146", "NoExpression");
                Expressions.Add("04273d290", "NoExpression");
                Expressions.Add("04911d65", "NoExpression");
                Expressions.Add("04385d435", "NoExpression");
                Expressions.Add("04849d128", "NoExpression");
                Expressions.Add("04894d68", "NoExpression");
                Expressions.Add("04691d126", "NoExpression");
                Expressions.Add("04675d343", "NoExpression");
                Expressions.Add("04782d195", "NoExpression");
                Expressions.Add("04370d299", "NoExpression");
                Expressions.Add("04692d140", "NoExpression");
                Expressions.Add("04865d94", "NoExpression");
                Expressions.Add("04815d196", "NoExpression");
                Expressions.Add("04603d251", "NoExpression");
                Expressions.Add("04744d146", "NoExpression");
                Expressions.Add("04878d82", "NoExpression");
                Expressions.Add("04763d156", "NoExpression");
                Expressions.Add("04749d174", "NoExpression");
                Expressions.Add("04327d394", "NoExpression");
                Expressions.Add("04831d156", "NoExpression");
                Expressions.Add("04314d61", "NoExpression");
                Expressions.Add("04896d90", "NoExpression");
                Expressions.Add("04936d90", "NoExpression");
                Expressions.Add("04418d390", "NoExpression");
                Expressions.Add("04891d79", "NoExpression");
                Expressions.Add("04593d264", "NoExpression");
                Expressions.Add("04667d306", "NoExpression");
                Expressions.Add("04394d399", "NoExpression");
                Expressions.Add("04629d248", "NoExpression");
                Expressions.Add("04600d355", "NoExpression");
                Expressions.Add("04557d447", "NoExpression");
                Expressions.Add("04453d359", "NoExpression");
                Expressions.Add("04615d182", "NoExpression");
                Expressions.Add("04493d228", "NoExpression");
                Expressions.Add("04495d425", "NoExpression");
                Expressions.Add("02463d656", "NoExpression");
                Expressions.Add("04711d153", "NoExpression");
                Expressions.Add("04609d195", "NoExpression");
                Expressions.Add("04477d159", "NoExpression");
                Expressions.Add("04580d409", "NoExpression");
                Expressions.Add("04872d42", "NoExpression");
                Expressions.Add("04908d42", "NoExpression");
                Expressions.Add("04756d111", "NoExpression");
                Expressions.Add("04907d78", "NoExpression");
                Expressions.Add("04347d391", "NoExpression");
                Expressions.Add("04787d54", "NoExpression");
                Expressions.Add("04889d53", "NoExpression");
                Expressions.Add("04864d06", "NoExpression");
                Expressions.Add("04868d72", "NoExpression");
                Expressions.Add("04934d50", "NoExpression");
                Expressions.Add("04506d226", "NoExpression");
                Expressions.Add("04773d183", "NoExpression");
                Expressions.Add("04887d72", "NoExpression");
                Expressions.Add("04758d129", "NoExpression");
                Expressions.Add("04933d54", "NoExpression");
                Expressions.Add("04873d41", "NoExpression");
                Expressions.Add("04202d556", "NoExpression");
                Expressions.Add("04485d398", "NoExpression");
                Expressions.Add("04880d35", "NoExpression");
                Expressions.Add("04748d107", "NoExpression");
                Expressions.Add("04429d445", "NoExpression");
                Expressions.Add("04768d146", "NoExpression");
                Expressions.Add("04514d436", "NoExpression");
                Expressions.Add("04284d55", "NoExpression");
                Expressions.Add("04203d540", "NoExpression");
                Expressions.Add("04885d50", "NoExpression");
                Expressions.Add("04754d180", "NoExpression");
                Expressions.Add("04427d366", "NoExpression");
                Expressions.Add("04344d337", "NoExpression");
                Expressions.Add("04726d136", "NoExpression");
                Expressions.Add("04866d88", "NoExpression");
                Expressions.Add("04475d118", "NoExpression");
                Expressions.Add("04644d256", "NoExpression");
                Expressions.Add("04446d369", "NoExpression");
                Expressions.Add("04767d96", "NoExpression");
                Expressions.Add("04832d144", "NoExpression");
                Expressions.Add("04848d140", "NoExpression");
                Expressions.Add("04588d233", "NoExpression");
                Expressions.Add("04708d163", "NoExpression");
                Expressions.Add("04850d66", "NoExpression");
                Expressions.Add("04895d30", "NoExpression");
                Expressions.Add("04683d343", "NoExpression");
                Expressions.Add("04784d70", "NoExpression");
                Expressions.Add("04632d234", "NoExpression");
                Expressions.Add("04673d275", "NoExpression");
                Expressions.Add("04843d155", "NoExpression");
                Expressions.Add("04221d545", "NoExpression");
                Expressions.Add("04397d448", "NoExpression");
                Expressions.Add("04400d378", "NoExpression");
                Expressions.Add("04921d42", "NoExpression");
                Expressions.Add("04451d315", "NoExpression");
                Expressions.Add("04772d38", "NoExpression");
                Expressions.Add("04846d144", "NoExpression");
                Expressions.Add("04876d66", "NoExpression");
                Expressions.Add("04670d273", "NoExpression");
                Expressions.Add("04892d90", "NoExpression");
                Expressions.Add("04900d92", "NoExpression");
                Expressions.Add("04920d06", "NoExpression");
                Expressions.Add("04879d06", "NoExpression");
                Expressions.Add("04925d30", "NoExpression");
                Expressions.Add("04419d320", "NoExpression");
                Expressions.Add("04796d140", "NoExpression");
                Expressions.Add("04482d410", "NoExpression");
                Expressions.Add("04569d374", "NoExpression");
                Expressions.Add("04664d187", "NoExpression");
                Expressions.Add("04239d484", "NoExpression");
                Expressions.Add("04470d401", "NoExpression");
                Expressions.Add("04507d406", "NoExpression");
                Expressions.Add("04867d74", "NoExpression");
                Expressions.Add("04404d211", "NoExpression");
                Expressions.Add("04841d142", "NoExpression");
                Expressions.Add("04535d259", "NoExpression");
                Expressions.Add("04922d62", "NoExpression");
                Expressions.Add("04928d92", "NoExpression");
                Expressions.Add("04703d114", "NoExpression");
                Expressions.Add("04918d14", "NoExpression");
                Expressions.Add("04622d324", "NoExpression");
                Expressions.Add("04213d338", "NoExpression");
                Expressions.Add("04839d180", "NoExpression");
                Expressions.Add("04379d359", "NoExpression");
                Expressions.Add("04461d407", "NoExpression");
                Expressions.Add("04776d97", "NoExpression");
                Expressions.Add("04719d179", "NoExpression");
                Expressions.Add("04297d307", "NoExpression");
                Expressions.Add("04813d142", "NoExpression");
                Expressions.Add("04537d414", "NoExpression");
                Expressions.Add("04883d86", "NoExpression");
                Expressions.Add("04822d146", "NoExpression");
                Expressions.Add("04351d96", "NoExpression");
                Expressions.Add("04387d427", "NoExpression");
                Expressions.Add("04488d388", "NoExpression");
                Expressions.Add("04407d315", "NoExpression");
                Expressions.Add("04682d218", "NoExpression");
                Expressions.Add("04597d121", "NoExpression");
                Expressions.Add("04225d400", "NoExpression");
                Expressions.Add("04905d54", "NoExpression");
                Expressions.Add("04780d102", "NoExpression");
                Expressions.Add("04743d136", "NoExpression");
                Expressions.Add("04932d38", "NoExpression");
                Expressions.Add("04926d14", "NoExpression");
                Expressions.Add("04923d48", "NoExpression");
                Expressions.Add("04720d70", "NoExpression");
                Expressions.Add("04728d94", "NoExpression");
                Expressions.Add("04803d168", "NoExpression");
                Expressions.Add("04786d144", "NoExpression");
                Expressions.Add("04770d48", "NoExpression");
                Expressions.Add("04899d82", "NoExpression");
                Expressions.Add("04897d06", "NoExpression");
                Expressions.Add("04721d82", "NoExpression");
                Expressions.Add("04765d160", "NoExpression");
                Expressions.Add("04829d108", "NoExpression");
                Expressions.Add("04916d86", "NoExpression");
                Expressions.Add("04727d166", "NoExpression");
                Expressions.Add("04449d249", "NoExpression");
                Expressions.Add("04929d46", "NoExpression");
                Expressions.Add("04915d52", "NoExpression");
                Expressions.Add("04702d128", "NoExpression");
                Expressions.Add("04695d100", "NoExpression");
                Expressions.Add("04476d216", "NoExpression");
                Expressions.Add("04877d18", "NoExpression");
                Expressions.Add("04408d362", "NoExpression");
                Expressions.Add("04701d157", "NoExpression");
                Expressions.Add("04714d190", "NoExpression");
                Expressions.Add("04302d146", "NoExpression");
                Expressions.Add("04927d38", "NoExpression");
                Expressions.Add("04613d178", "NoExpression");
                Expressions.Add("04734d186", "NoExpression");
                Expressions.Add("04857d72", "NoExpression");
                Expressions.Add("04855d84", "NoExpression");
                Expressions.Add("04265d337", "NoExpression");
                Expressions.Add("04914d42", "NoExpression");
                Expressions.Add("04311d280", "NoExpression");
                Expressions.Add("04349d410", "NoExpression");
                Expressions.Add("04689d92", "NoExpression");
                Expressions.Add("04893d66", "NoExpression");
                Expressions.Add("04233d500", "NoExpression");
                Expressions.Add("04699d108", "NoExpression");
                Expressions.Add("04456d349", "NoExpression");
                Expressions.Add("04530d429", "NoExpression");
                Expressions.Add("04595d145", "NoExpression");
                Expressions.Add("04798d80", "NoExpression");
                Expressions.Add("04531d389", "NoExpression");
                Expressions.Add("04882d64", "NoExpression");
                Expressions.Add("04863d58", "NoExpression");
                Expressions.Add("04372d333", "NoExpression");
                Expressions.Add("04697d180", "NoExpression");
                Expressions.Add("04436d362", "NoExpression");
                Expressions.Add("04589d270", "NoExpression");
                Expressions.Add("04712d99", "NoExpression");
                Expressions.Add("04917d90", "NoExpression");
                Expressions.Add("04811d108", "NoExpression");
                Expressions.Add("04731d133", "NoExpression");
                Expressions.Add("04715d74", "NoExpression");
                Expressions.Add("04827d118", "NoExpression");
                Expressions.Add("04509d386", "NoExpression");
                Expressions.Add("04479d262", "NoExpression");
                Expressions.Add("04496d288", "NoExpression");
                Expressions.Add("04870d84", "NoExpression");
                Expressions.Add("04339d290", "NoExpression");
                Expressions.Add("04760d166", "NoExpression");
                Expressions.Add("04816d60", "NoExpression");
                Expressions.Add("04585d270", "NoExpression");
                Expressions.Add("04542d190", "NoExpression");
                Expressions.Add("04904d38", "NoExpression");
                Expressions.Add("04460d328", "NoExpression");
                Expressions.Add("04709d155", "NoExpression");
                Expressions.Add("04343d431", "NoExpression");
                Expressions.Add("04869d56", "NoExpression");
                Expressions.Add("04898d60", "NoExpression");
                Expressions.Add("04200d74", "NoExpression");
                Expressions.Add("04838d154", "NoExpression");
                Expressions.Add("04854d142", "NoExpression");
                Expressions.Add("04890d72", "NoExpression");
                Expressions.Add("04901d72", "NoExpression");
                Expressions.Add("04553d262", "NoExpression");
                Expressions.Add("04581d251", "NoExpression");
                Expressions.Add("04350d322", "NoExpression");
                Expressions.Add("04641d245", "NoExpression");
                Expressions.Add("04312d209", "NoExpression");
                Expressions.Add("04577d344", "NoExpression");
                Expressions.Add("04388d377", "NoExpression");
                Expressions.Add("04313d58", "NoExpression");
                Expressions.Add("04871d60", "NoExpression");
                Expressions.Add("04341d189", "NoExpression");
                Expressions.Add("04749d176", "NoExpression");
                Expressions.Add("04856d88", "NoExpression");
                Expressions.Add("04881d96", "NoExpression");
                Expressions.Add("04840d152", "NoExpression");
                Expressions.Add("04394d401", "NoExpression");
                Expressions.Add("04327d396", "NoExpression");
                Expressions.Add("04878d84", "NoExpression");
                Expressions.Add("04430d373", "NoExpression");
                Expressions.Add("04629d250", "NoExpression");
                Expressions.Add("04334d416", "NoExpression");
                Expressions.Add("04847d164", "NoExpression");
                Expressions.Add("04842d162", "NoExpression");
                Expressions.Add("04798d82", "NoExpression");
                Expressions.Add("04797d148", "NoExpression");
                Expressions.Add("04851d164", "NoExpression");
                Expressions.Add("04853d152", "NoExpression");
                Expressions.Add("04782d197", "NoExpression");
                Expressions.Add("04866d90", "NoExpression");
                Expressions.Add("04288d296", "NoExpression");
                Expressions.Add("04729d52", "NoExpression");
                Expressions.Add("04675d345", "NoExpression");
                Expressions.Add("04609d197", "NoExpression");
                Expressions.Add("04708d165", "NoExpression");
                Expressions.Add("04418d392", "NoExpression");
                Expressions.Add("04385d437", "NoExpression");
                Expressions.Add("04777d188", "NoExpression");
                Expressions.Add("04850d68", "NoExpression");
                Expressions.Add("04598d355", "NoExpression");
                Expressions.Add("04692d142", "NoExpression");
                Expressions.Add("04600d357", "NoExpression");
                Expressions.Add("02463d658", "NoExpression");
                Expressions.Add("04868d74", "NoExpression");
                Expressions.Add("04934d52", "NoExpression");
                Expressions.Add("04378d229", "NoExpression");
                Expressions.Add("04865d96", "NoExpression");
                Expressions.Add("04833d154", "NoExpression");
                Expressions.Add("04831d158", "NoExpression");
                Expressions.Add("04485d400", "NoExpression");
                Expressions.Add("04822d148", "NoExpression");
                Expressions.Add("04673d277", "NoExpression");
                Expressions.Add("04936d92", "NoExpression");
                Expressions.Add("04891d81", "NoExpression");
                Expressions.Add("04846d146", "NoExpression");
                Expressions.Add("04815d198", "NoExpression");
                Expressions.Add("04603d253", "NoExpression");
                Expressions.Add("04557d449", "NoExpression");
                Expressions.Add("04711d155", "NoExpression");
                Expressions.Add("04482d412", "NoExpression");
                Expressions.Add("04512d434", "NoExpression");
                Expressions.Add("04911d67", "NoExpression");
                Expressions.Add("04756d113", "NoExpression");
                Expressions.Add("04312d211", "NoExpression");
                Expressions.Add("04906d06", "NoExpression");
                Expressions.Add("04470d403", "NoExpression");
                Expressions.Add("04813d144", "NoExpression");
                Expressions.Add("04615d184", "NoExpression");
                Expressions.Add("04908d44", "NoExpression");
                Expressions.Add("04872d44", "NoExpression");
                Expressions.Add("04347d393", "NoExpression");
                Expressions.Add("04887d74", "NoExpression");
                Expressions.Add("04605d281", "NoExpression");
                Expressions.Add("04929d48", "NoExpression");
                Expressions.Add("04758d131", "NoExpression");
                Expressions.Add("04408d364", "NoExpression");
                Expressions.Add("04719d181", "NoExpression");
                Expressions.Add("04714d192", "NoExpression");
                Expressions.Add("04553d264", "NoExpression");
                Expressions.Add("04300d260", "NoExpression");
                Expressions.Add("04202d558", "NoExpression");
                Expressions.Add("04319d268", "NoExpression");
                Expressions.Add("04514d438", "NoExpression");
                Expressions.Add("04727d168", "NoExpression");
                Expressions.Add("04370d301", "NoExpression");
                Expressions.Add("04320d342", "NoExpression");
                Expressions.Add("04898d62", "NoExpression");
                Expressions.Add("04633d292", "NoExpression");
                Expressions.Add("04885d52", "NoExpression");
                Expressions.Add("04427d368", "NoExpression");
                Expressions.Add("04754d182", "NoExpression");
                Expressions.Add("04631d260", "NoExpression");
                Expressions.Add("04344d339", "NoExpression");
                Expressions.Add("04284d57", "NoExpression");
                Expressions.Add("04580d411", "NoExpression");
                Expressions.Add("04530d431", "NoExpression");
                Expressions.Add("04644d258", "NoExpression");
                Expressions.Add("04792d67", "NoExpression");
                Expressions.Add("04767d98", "NoExpression");
                Expressions.Add("04475d120", "NoExpression");
                Expressions.Add("04456d351", "NoExpression");
                Expressions.Add("04507d408", "NoExpression");
                Expressions.Add("04667d308", "NoExpression");
                Expressions.Add("04446d371", "NoExpression");
                Expressions.Add("04722d62", "NoExpression");
                Expressions.Add("04537d416", "NoExpression");
                Expressions.Add("04632d236", "NoExpression");
                Expressions.Add("04773d185", "NoExpression");
                Expressions.Add("04726d138", "NoExpression");
                Expressions.Add("04203d542", "NoExpression");
                Expressions.Add("04691d128", "NoExpression");
                Expressions.Add("04894d70", "NoExpression");
                Expressions.Add("04763d158", "NoExpression");
                Expressions.Add("04760d168", "NoExpression");
                Expressions.Add("04477d161", "NoExpression");
                Expressions.Add("04495d427", "NoExpression");
                Expressions.Add("04810d123", "NoExpression");
                Expressions.Add("04924d70", "NoExpression");
                Expressions.Add("04915d54", "NoExpression");
                Expressions.Add("04876d68", "NoExpression");
                Expressions.Add("04816d62", "NoExpression");
                Expressions.Add("04687d133", "NoExpression");
                Expressions.Add("04556d413", "NoExpression");
                Expressions.Add("04682d220", "NoExpression");
                Expressions.Add("04221d547", "NoExpression");
                Expressions.Add("04626d349", "NoExpression");
                Expressions.Add("04588d235", "NoExpression");
                Expressions.Add("04744d148", "NoExpression");
                Expressions.Add("04748d109", "NoExpression");
                Expressions.Add("04880d37", "NoExpression");
                Expressions.Add("04569d376", "NoExpression");
                Expressions.Add("04239d486", "NoExpression");
                Expressions.Add("04933d56", "NoExpression");
                Expressions.Add("04596d170", "NoExpression");
                Expressions.Add("04622d326", "NoExpression");
                Expressions.Add("04883d88", "NoExpression");
                Expressions.Add("04874d26", "NoExpression");
                Expressions.Add("04747d102", "NoExpression");
                Expressions.Add("04832d146", "NoExpression");
                Expressions.Add("04768d148", "NoExpression");
                Expressions.Add("04429d447", "NoExpression");
                Expressions.Add("04451d317", "NoExpression");
                Expressions.Add("04488d390", "NoExpression");
                Expressions.Add("04213d340", "NoExpression");
                Expressions.Add("04867d76", "NoExpression");
                Expressions.Add("04404d213", "NoExpression");
                Expressions.Add("04765d162", "NoExpression");
                Expressions.Add("04560d382", "NoExpression");
                Expressions.Add("04703d116", "NoExpression");
                Expressions.Add("04928d94", "NoExpression");
                Expressions.Add("04903d32", "NoExpression");
                Expressions.Add("04336d397", "NoExpression");
                Expressions.Add("04472d318", "NoExpression");
                Expressions.Add("04776d99", "NoExpression");
                Expressions.Add("04481d399", "NoExpression");
                Expressions.Add("04463d255", "NoExpression");
                Expressions.Add("04895d32", "NoExpression");
                Expressions.Add("04397d450", "NoExpression");
                Expressions.Add("04892d92", "NoExpression");
                Expressions.Add("04925d32", "NoExpression");
                Expressions.Add("04273d292", "NoExpression");
                Expressions.Add("04461d409", "NoExpression");
                Expressions.Add("04419d322", "NoExpression");
                Expressions.Add("04796d142", "NoExpression");
                Expressions.Add("04786d146", "NoExpression");
                Expressions.Add("04575d400", "NoExpression");
                Expressions.Add("04351d98", "NoExpression");
                Expressions.Add("04593d266", "NoExpression");
                Expressions.Add("04387d429", "NoExpression");
                Expressions.Add("04400d380", "NoExpression");
                Expressions.Add("04743d138", "NoExpression");
                Expressions.Add("04762d95", "NoExpression");
                Expressions.Add("04720d72", "NoExpression");
                Expressions.Add("04460d330", "NoExpression");
                Expressions.Add("04728d96", "NoExpression");
                Expressions.Add("04449d251", "NoExpression");
                Expressions.Add("04905d56", "NoExpression");
                Expressions.Add("04712d101", "NoExpression");
                Expressions.Add("04641d247", "NoExpression");
                Expressions.Add("04829d110", "NoExpression");
                Expressions.Add("04877d20", "NoExpression");
                Expressions.Add("04476d218", "NoExpression");
                Expressions.Add("04286d371", "NoExpression");
                Expressions.Add("04849d130", "NoExpression");
                Expressions.Add("04932d40", "NoExpression");
                Expressions.Add("04395d267", "NoExpression");
                Expressions.Add("04757d153", "NoExpression");
                Expressions.Add("04585d272", "NoExpression");
                Expressions.Add("04613d180", "NoExpression");
                Expressions.Add("04855d86", "NoExpression");
                Expressions.Add("04857d74", "NoExpression");
                Expressions.Add("04265d339", "NoExpression");
                Expressions.Add("04923d50", "NoExpression");
                Expressions.Add("04309d247", "NoExpression");
                Expressions.Add("04927d40", "NoExpression");
                Expressions.Add("04301d349", "NoExpression");
                Expressions.Add("04506d228", "NoExpression");
                Expressions.Add("04699d110", "NoExpression");
                Expressions.Add("04882d66", "NoExpression");
                Expressions.Add("04863d60", "NoExpression");
                Expressions.Add("04531d391", "NoExpression");
                Expressions.Add("04505d324", "NoExpression");
                Expressions.Add("04697d182", "NoExpression");
                Expressions.Add("04917d92", "NoExpression");
                Expressions.Add("04848d142", "NoExpression");
                Expressions.Add("04715d76", "NoExpression");
                Expressions.Add("04731d135", "NoExpression");
                Expressions.Add("04907d80", "NoExpression");
                Expressions.Add("04479d264", "NoExpression");
                Expressions.Add("04496d290", "NoExpression");
                Expressions.Add("04407d317", "NoExpression");
                Expressions.Add("04803d170", "NoExpression");
                Expressions.Add("04904d40", "NoExpression");
                Expressions.Add("04749d178", "Happiness");
                Expressions.Add("04856d90", "Happiness");
                Expressions.Add("04777d190", "Happiness");
                Expressions.Add("04615d186", "Happiness");
                Expressions.Add("04388d379", "Happiness");
                Expressions.Add("04202d560", "Happiness");
                Expressions.Add("04394d403", "Happiness");
                Expressions.Add("04327d398", "Happiness");
                Expressions.Add("04667d310", "Happiness");
                Expressions.Add("04840d154", "Happiness");
                Expressions.Add("04370d303", "Happiness");
                Expressions.Add("04871d62", "Happiness");
                Expressions.Add("04577d346", "Happiness");
                Expressions.Add("04313d60", "Happiness");
                Expressions.Add("04575d402", "Happiness");
                Expressions.Add("04709d157", "Happiness");
                Expressions.Add("04842d164", "Happiness");
                Expressions.Add("04847d166", "Happiness");
                Expressions.Add("04881d98", "Happiness");
                Expressions.Add("04797d150", "Happiness");
                Expressions.Add("04288d298", "Happiness");
                Expressions.Add("04923d52", "Happiness");
                Expressions.Add("04911d69", "Happiness");
                Expressions.Add("04729d54", "Happiness");
                Expressions.Add("04297d309", "Happiness");
                Expressions.Add("04519d234", "Happiness");
                Expressions.Add("04512d436", "Happiness");
                Expressions.Add("04580d413", "Happiness");
                Expressions.Add("04514d440", "Happiness");
                Expressions.Add("04849d132", "Happiness");
                Expressions.Add("04833d156", "Happiness");
                Expressions.Add("04495d429", "Happiness");
                Expressions.Add("04868d76", "Happiness");
                Expressions.Add("04934d54", "Happiness");
                Expressions.Add("04927d42", "Happiness");
                Expressions.Add("04470d405", "Happiness");
                Expressions.Add("04865d98", "Happiness");
                Expressions.Add("04603d255", "Happiness");
                Expressions.Add("04850d70", "Happiness");
                Expressions.Add("04895d34", "Happiness");
                Expressions.Add("04626d351", "Happiness");
                Expressions.Add("04831d160", "Happiness");
                Expressions.Add("04472d320", "Happiness");
                Expressions.Add("04936d94", "Happiness");
                Expressions.Add("04896d92", "Happiness");
                Expressions.Add("04301d351", "Happiness");
                Expressions.Add("04600d359", "Happiness");
                Expressions.Add("04765d164", "Happiness");
                Expressions.Add("04612d65", "Happiness");
                Expressions.Add("04397d452", "Happiness");
                Expressions.Add("04557d451", "Happiness");
                Expressions.Add("04385d439", "Happiness");
                Expressions.Add("04822d150", "Happiness");
                Expressions.Add("04711d157", "Happiness");
                Expressions.Add("04632d238", "Happiness");
                Expressions.Add("04334d418", "Happiness");
                Expressions.Add("04756d115", "Happiness");
                Expressions.Add("04878d86", "Happiness");
                Expressions.Add("04908d46", "Happiness");
                Expressions.Add("04872d46", "Happiness");
                Expressions.Add("04347d395", "Happiness");
                Expressions.Add("04727d170", "Happiness");
                Expressions.Add("02463d660", "Happiness");
                Expressions.Add("04873d43", "Happiness");
                Expressions.Add("04719d183", "Happiness");
                Expressions.Add("04708d167", "Happiness");
                Expressions.Add("04605d283", "Happiness");
                Expressions.Add("04408d366", "Happiness");
                Expressions.Add("04929d50", "Happiness");
                Expressions.Add("04915d56", "Happiness");
                Expressions.Add("04560d384", "Happiness");
                Expressions.Add("04485d402", "Happiness");
                Expressions.Add("04473d241", "Happiness");
                Expressions.Add("04869d58", "Happiness");
                Expressions.Add("04460d332", "Happiness");
                Expressions.Add("04609d199", "Happiness");
                Expressions.Add("04319d270", "Happiness");
                Expressions.Add("04748d111", "Happiness");
                Expressions.Add("04880d39", "Happiness");
                Expressions.Add("04344d341", "Happiness");
                Expressions.Add("04885d54", "Happiness");
                Expressions.Add("04754d184", "Happiness");
                Expressions.Add("04530d433", "Happiness");
                Expressions.Add("04726d140", "Happiness");
                Expressions.Add("04866d92", "Happiness");
                Expressions.Add("04743d140", "Happiness");
                Expressions.Add("04446d373", "Happiness");
                Expressions.Add("04644d260", "Happiness");
                Expressions.Add("04767d100", "Happiness");
                Expressions.Add("04815d200", "Happiness");
                Expressions.Add("04531d393", "Happiness");
                Expressions.Add("04481d401", "Happiness");
                Expressions.Add("04633d294", "Happiness");
                Expressions.Add("04760d170", "Happiness");
                Expressions.Add("04691d130", "Happiness");
                Expressions.Add("04894d72", "Happiness");
                Expressions.Add("04786d148", "Happiness");
                Expressions.Add("04682d222", "Happiness");
                Expressions.Add("04907d82", "Happiness");
                Expressions.Add("04461d411", "Happiness");
                Expressions.Add("04782d199", "Happiness");
                Expressions.Add("04870d86", "Happiness");
                Expressions.Add("04916d88", "Happiness");
                Expressions.Add("04387d431", "Happiness");
                Expressions.Add("04217d459", "Happiness");
                Expressions.Add("04832d148", "Happiness");
                Expressions.Add("04848d144", "Happiness");
                Expressions.Add("04488d392", "Happiness");
                Expressions.Add("04675d347", "Happiness");
                Expressions.Add("04400d382", "Happiness");
                Expressions.Add("04811d110", "Happiness");
                Expressions.Add("04507d410", "Happiness");
                Expressions.Add("04724d140", "Happiness");
                Expressions.Add("04476d220", "Happiness");
                Expressions.Add("04324d348", "Happiness");
                Expressions.Add("04933d58", "Happiness");
                Expressions.Add("04388d381", "Surprise");
                Expressions.Add("04871d64", "Surprise");
                Expressions.Add("04577d348", "Surprise");
                Expressions.Add("04313d62", "Surprise");
                Expressions.Add("04749d180", "Surprise");
                Expressions.Add("04673d279", "Surprise");
                Expressions.Add("04840d156", "Surprise");
                Expressions.Add("04633d296", "Surprise");
                Expressions.Add("04667d312", "Surprise");
                Expressions.Add("04495d431", "Surprise");
                Expressions.Add("04385d441", "Surprise");
                Expressions.Add("04202d562", "Surprise");
                Expressions.Add("04334d420", "Surprise");
                Expressions.Add("04888d40", "Surprise");
                Expressions.Add("04709d159", "Surprise");
                Expressions.Add("04327d400", "Surprise");
                Expressions.Add("04394d405", "Surprise");
                Expressions.Add("04881d100", "Surprise");
                Expressions.Add("04575d404", "Surprise");
                Expressions.Add("04797d152", "Surprise");
                Expressions.Add("04923d54", "Surprise");
                Expressions.Add("04831d162", "Surprise");
                Expressions.Add("04288d300", "Surprise");
                Expressions.Add("04729d56", "Surprise");
                Expressions.Add("04782d201", "Surprise");
                Expressions.Add("04370d305", "Surprise");
                Expressions.Add("04519d236", "Surprise");
                Expressions.Add("04815d202", "Surprise");
                Expressions.Add("04796d144", "Surprise");
                Expressions.Add("04419d324", "Surprise");
                Expressions.Add("02463d662", "Surprise");
                Expressions.Add("04730d126", "Surprise");
                Expressions.Add("04777d192", "Surprise");
                Expressions.Add("04721d84", "Surprise");
                Expressions.Add("04418d394", "Surprise");
                Expressions.Add("04615d188", "Surprise");
                Expressions.Add("04600d361", "Surprise");
                Expressions.Add("04865d100", "Surprise");
                Expressions.Add("04878d88", "Surprise");
                Expressions.Add("04347d397", "Surprise");
                Expressions.Add("04933d60", "Surprise");
                Expressions.Add("04896d94", "Surprise");
                Expressions.Add("04936d96", "Surprise");
                Expressions.Add("04915d58", "Surprise");
                Expressions.Add("04932d42", "Surprise");
                Expressions.Add("04201d434", "Surprise");
                Expressions.Add("04301d353", "Surprise");
                Expressions.Add("04683d345", "Surprise");
                Expressions.Add("04626d353", "Surprise");
                Expressions.Add("04557d453", "Surprise");
                Expressions.Add("04682d224", "Surprise");
                Expressions.Add("04221d549", "Surprise");
                Expressions.Add("04632d240", "Surprise");
                Expressions.Add("04482d414", "Surprise");
                Expressions.Add("04823d138", "Surprise");
                Expressions.Add("04851d166", "Surprise");
                Expressions.Add("04609d201", "Surprise");
                Expressions.Add("04822d152", "Surprise");
                Expressions.Add("04580d415", "Surprise");
                Expressions.Add("04711d159", "Surprise");
                Expressions.Add("04872d48", "Surprise");
                Expressions.Add("04908d48", "Surprise");
                Expressions.Add("04470d407", "Surprise");
                Expressions.Add("04787d56", "Surprise");
                Expressions.Add("04889d57", "Surprise");
                Expressions.Add("04868d78", "Surprise");
                Expressions.Add("04758d133", "Surprise");
                Expressions.Add("04724d142", "Surprise");
                Expressions.Add("04898d64", "Surprise");
                Expressions.Add("04719d185", "Surprise");
                Expressions.Add("04319d272", "Surprise");
                Expressions.Add("04833d158", "Surprise");
                Expressions.Add("04485d404", "Surprise");
                Expressions.Add("04324d350", "Surprise");
                Expressions.Add("04603d257", "Surprise");
                Expressions.Add("04225d402", "Surprise");
                Expressions.Add("04233d502", "Surprise");
                Expressions.Add("04512d438", "Surprise");
                Expressions.Add("04745d168", "Surprise");
                Expressions.Add("04339d292", "Surprise");
                Expressions.Add("04429d449", "Surprise");
                Expressions.Add("04768d150", "Surprise");
                Expressions.Add("04664d189", "Surprise");
                Expressions.Add("04217d461", "Surprise");
                Expressions.Add("04598d357", "Surprise");
                Expressions.Add("04883d90", "Surprise");
                Expressions.Add("04830d142", "Surprise");
                Expressions.Add("04928d96", "Surprise");
                Expressions.Add("04703d118", "Surprise");
                Expressions.Add("04829d112", "Surprise");
                Expressions.Add("04856d92", "Surprise");
                Expressions.Add("04748d113", "Surprise");
                Expressions.Add("04556d415", "Surprise");
                Expressions.Add("04593d268", "Surprise");
                Expressions.Add("04757d155", "Surprise");
                Expressions.Add("04463d257", "Surprise");
                Expressions.Add("04922d64", "Surprise");
                Expressions.Add("04511d246", "Surprise");
                Expressions.Add("04884d59", "Surprise");
                Expressions.Add("04407d319", "Surprise");
                Expressions.Add("04790d96", "Surprise");
                Expressions.Add("04813d146", "Surprise");
                Expressions.Add("04488d394", "Surprise");
                Expressions.Add("04537d418", "Surprise");
                Expressions.Add("04786d150", "Surprise");
                Expressions.Add("04848d146", "Surprise");
                Expressions.Add("04397d454", "Surprise");
                Expressions.Add("04767d102", "Surprise");
                Expressions.Add("04434d189", "Surprise");
                Expressions.Add("04697d184", "Surprise");
                Expressions.Add("04904d42", "Surprise");
                Expressions.Add("04756d117", "Surprise");
                Expressions.Add("04762d97", "Surprise");
                Expressions.Add("04239d488", "Surprise");
                Expressions.Add("04720d74", "Surprise");
                Expressions.Add("04743d142", "Surprise");
                Expressions.Add("04876d70", "Surprise");
                Expressions.Add("04811d112", "Surprise");
                Expressions.Add("04827d120", "Surprise");
                Expressions.Add("04509d388", "Surprise");
                Expressions.Add("04727d172", "Surprise");
                Expressions.Add("04803d172", "Surprise");
                Expressions.Add("04780d104", "Surprise");
                Expressions.Add("04702d130", "Surprise");
                Expressions.Add("04476d222", "Surprise");
                Expressions.Add("04880d41", "Surprise");
                Expressions.Add("04701d159", "Surprise");
                Expressions.Add("04286d373", "Surprise");
                Expressions.Add("04560d386", "Surprise");
                Expressions.Add("04585d274", "Surprise");
                Expressions.Add("04839d182", "Surprise");
                Expressions.Add("04899d84", "Surprise");
                Expressions.Add("04453d361", "Surprise");
                Expressions.Add("04461d413", "Surprise");
                Expressions.Add("04430d375", "Surprise");
                Expressions.Add("04514d442", "Surprise");
                Expressions.Add("04857d76", "Surprise");
                Expressions.Add("04914d44", "Surprise");
                Expressions.Add("04387d433", "Surprise");
                Expressions.Add("04927d44", "Surprise");
                Expressions.Add("04887d76", "Surprise");
                Expressions.Add("04587d110", "Surprise");
                Expressions.Add("04542d192", "Surprise");
                Expressions.Add("04734d188", "Surprise");
                Expressions.Add("04588d237", "Surprise");
                Expressions.Add("04423d272", "Surprise");
                Expressions.Add("04895d36", "Surprise");
                Expressions.Add("04901d74", "Surprise");
                Expressions.Add("04890d74", "Surprise");
                Expressions.Add("04841d144", "Surprise");
                Expressions.Add("04905d58", "Surprise");
                Expressions.Add("04689d94", "Surprise");
                Expressions.Add("04350d324", "Surprise");
                Expressions.Add("04893d68", "Surprise");
                Expressions.Add("04783d76", "Surprise");
                Expressions.Add("04812d72", "Surprise");
                Expressions.Add("04747d104", "Surprise");
                Expressions.Add("04838d156", "Surprise");
                Expressions.Add("04341d191", "Other");
                Expressions.Add("04379d361", "Other");
                Expressions.Add("04749d182", "Other");
                Expressions.Add("04470d409", "Other");
                Expressions.Add("04840d158", "Other");
                Expressions.Add("04818d54", "Other");
                Expressions.Add("04612d67", "Other");
                Expressions.Add("04667d314", "Other");
                Expressions.Add("04580d417", "Other");
                Expressions.Add("04418d396", "Other");
                Expressions.Add("04388d383", "Other");
                Expressions.Add("04888d42", "Other");
                Expressions.Add("04797d154", "Other");
                Expressions.Add("04923d56", "Other");
                Expressions.Add("04575d406", "Other");
                Expressions.Add("04682d226", "Other");
                Expressions.Add("04709d161", "Other");
                Expressions.Add("04842d168", "Other");
                Expressions.Add("04847d168", "Other");
                Expressions.Add("04273d294", "Other");
                Expressions.Add("04615d190", "Other");
                Expressions.Add("04911d73", "Other");
                Expressions.Add("04866d94", "Other");
                Expressions.Add("04726d142", "Other");
                Expressions.Add("04815d204", "Other");
                Expressions.Add("04394d407", "Other");
                Expressions.Add("04320d346", "Other");
                Expressions.Add("04782d203", "Other");
                Expressions.Add("04833d160", "Other");
                Expressions.Add("04708d171", "Other");
                Expressions.Add("04921d46", "Other");
                Expressions.Add("04334d422", "Other");
                Expressions.Add("02463d664", "Other");
                Expressions.Add("04796d146", "Other");
                Expressions.Add("04419d326", "Other");
                Expressions.Add("04763d160", "Other");
                Expressions.Add("04777d194", "Other");
                Expressions.Add("04239d490", "Other");
                Expressions.Add("04447d159", "Other");
                Expressions.Add("04927d46", "Other");
                Expressions.Add("04603d259", "Other");
                Expressions.Add("04347d399", "Other");
                Expressions.Add("04378d231", "Other");
                Expressions.Add("04286d375", "Other");
                Expressions.Add("04865d102", "Other");
                Expressions.Add("04481d405", "Other");
                Expressions.Add("04831d164", "Other");
                Expressions.Add("04301d355", "Other");
                Expressions.Add("04936d98", "Other");
                Expressions.Add("04822d154", "Other");
                Expressions.Add("04609d203", "Other");
                Expressions.Add("04349d412", "Other");
                Expressions.Add("04385d443", "Other");
                Expressions.Add("04504d105", "Other");
                Expressions.Add("04816d66", "Other");
                Expressions.Add("04201d436", "Other");
                Expressions.Add("04745d170", "Other");
                Expressions.Add("04714d196", "Other");
                Expressions.Add("04670d275", "Other");
                Expressions.Add("04475d124", "Other");
                Expressions.Add("04427d372", "Other");
                Expressions.Add("04754d188", "Other");
                Expressions.Add("04344d345", "Other");
                Expressions.Add("04507d414", "Other");
                Expressions.Add("04495d433", "Other");
                Expressions.Add("04644d262", "Other");
                Expressions.Add("04767d104", "Other");
                Expressions.Add("04626d355", "Other");
                Expressions.Add("04683d347", "Other");
                Expressions.Add("04537d420", "Other");
                Expressions.Add("04446d377", "Other");
                Expressions.Add("04760d172", "Other");
                Expressions.Add("04894d76", "Other");
                Expressions.Add("04691d134", "Other");
                Expressions.Add("04633d298", "Other");
                Expressions.Add("04512d440", "Other");
                Expressions.Add("04440d121", "Other");
                Expressions.Add("04312d215", "Other");
                Expressions.Add("04851d168", "Other");
                Expressions.Add("04203d546", "Other");
                Expressions.Add("04773d189", "Other");
                Expressions.Add("04869d62", "Other");
                Expressions.Add("04910d65", "Other");
                Expressions.Add("04924d72", "Other");
                Expressions.Add("04891d85", "Other");
                Expressions.Add("04530d437", "Other");
                Expressions.Add("04472d324", "Other");
                Expressions.Add("04876d72", "Other");
                Expressions.Add("04839d184", "Other");
                Expressions.Add("04213d344", "Other");
                Expressions.Add("04900d96", "Other");
                Expressions.Add("04892d96", "Other");
                Expressions.Add("04463d259", "Other");
                Expressions.Add("04813d148", "Other");
                Expressions.Add("04629d254", "Other");
                Expressions.Add("04596d174", "Other");
                Expressions.Add("04775d138", "Other");
                Expressions.Add("04867d80", "Other");
                Expressions.Add("04404d217", "Other");
                Expressions.Add("04598d359", "Other");
                Expressions.Add("04622d330", "Other");
                Expressions.Add("04748d115", "Other");
                Expressions.Add("04485d406", "Other");
                Expressions.Add("04397d456", "Other");
                Expressions.Add("04829d114", "Other");
                Expressions.Add("04928d98", "Other");
                Expressions.Add("04429d451", "Other");
                Expressions.Add("04768d152", "Other");
                Expressions.Add("04511d248", "Other");
                Expressions.Add("04884d61", "Other");
                Expressions.Add("04871d66", "Other");
                Expressions.Add("04476d224", "Other");
                Expressions.Add("04233d504", "Other");
                Expressions.Add("04790d98", "Other");
                Expressions.Add("04514d444", "Other");
                Expressions.Add("04883d92", "Other");
                Expressions.Add("04874d28", "Other");
                Expressions.Add("04631d264", "Other");
                Expressions.Add("04202d564", "Other");
                Expressions.Add("04351d100", "Other");
                Expressions.Add("04848d148", "Other");
                Expressions.Add("04488d396", "Other");
                Expressions.Add("04907d84", "Other");
                Expressions.Add("04461d415", "Other");
                Expressions.Add("04702d132", "Other");
                Expressions.Add("04697d186", "Other");
                Expressions.Add("04899d86", "Other");
                Expressions.Add("04786d152", "Other");
                Expressions.Add("04762d99", "Other");
                Expressions.Add("04343d433", "Other");
                Expressions.Add("04430d377", "Other");
                Expressions.Add("04703d120", "Other");
                Expressions.Add("04712d103", "Other");
                Expressions.Add("04843d159", "Other");
                Expressions.Add("04881d102", "Other");
                Expressions.Add("04444d240", "Other");
                Expressions.Add("04780d106", "Other");
                Expressions.Add("04449d253", "Other");
                Expressions.Add("04724d144", "Other");
                Expressions.Add("04821d104", "Other");
                Expressions.Add("04738d94", "Other");
                Expressions.Add("04933d62", "Other");
                Expressions.Add("04641d249", "Other");
                Expressions.Add("04560d388", "Other");
                Expressions.Add("04613d182", "Other");
                Expressions.Add("04556d417", "Other");
                Expressions.Add("04914d46", "Other");
                Expressions.Add("04857d78", "Other");
                Expressions.Add("04336d401", "Other");
                Expressions.Add("04765d166", "Other");
                Expressions.Add("04302d148", "Other");
                Expressions.Add("04395d269", "Other");
                Expressions.Add("04728d98", "Other");
                Expressions.Add("04746d89", "Other");
                Expressions.Add("04887d78", "Other");
                Expressions.Add("04593d270", "Other");
                Expressions.Add("04585d276", "Other");
                Expressions.Add("04597d123", "Other");
                Expressions.Add("04387d435", "Other");
                Expressions.Add("04309d249", "Other");
                Expressions.Add("04372d335", "Other");
                Expressions.Add("04338d90", "Other");
                Expressions.Add("04505d326", "Other");
                Expressions.Add("04917d94", "Other");
                Expressions.Add("04595d147", "Other");
                Expressions.Add("04870d88", "Other");
                Expressions.Add("04531d395", "Other");
                Expressions.Add("04265d341", "Other");
                Expressions.Add("04882d68", "Other");
                Expressions.Add("04734d190", "Other");
                Expressions.Add("04588d239", "Other");
                Expressions.Add("04731d137", "Other");
                Expressions.Add("04715d78", "Other");
                Expressions.Add("04774d158", "Other");
                Expressions.Add("04509d390", "Other");
                Expressions.Add("04827d122", "Other");
                Expressions.Add("04850d72", "Other");
                Expressions.Add("04895d38", "Other");
                Expressions.Add("04854d144", "Other");
                Expressions.Add("04747d106", "Other");
                Expressions.Add("04905d60", "Other");
                Expressions.Add("04689d96", "Other");
                Expressions.Add("04893d70", "Other");
                Expressions.Add("04423d274", "Other");
                Expressions.Add("04838d158", "Other");
                Expressions.Add("04581d253", "Other");
                Expressions.Add("04339d294", "Other");
                Expressions.Add("04407d321", "Other");
                Expressions.Add("04436d364", "Other");
                Expressions.Add("04890d76", "Other");
                Expressions.Add("04901d76", "Other");
                Expressions.Add("04916d90", "Other");
                Expressions.Add("04887d80", "Other");
                Expressions.Add("04749d184", "Other");
                Expressions.Add("04217d463", "Other");
                Expressions.Add("04777d196", "Other");
                Expressions.Add("04840d160", "Other");
                Expressions.Add("04418d398", "Other");
                Expressions.Add("04394d409", "Other");
                Expressions.Add("04667d316", "Other");
                Expressions.Add("04327d402", "Other");
                Expressions.Add("04933d64", "Other");
                Expressions.Add("04888d44", "Other");
                Expressions.Add("04629d256", "Other");
                Expressions.Add("04336d403", "Other");
                Expressions.Add("04459d74", "Other");
                Expressions.Add("04580d419", "Other");
                Expressions.Add("04797d156", "Other");
                Expressions.Add("04873d47", "Other");
                Expressions.Add("04682d228", "Other");
                Expressions.Add("04847d170", "Other");
                Expressions.Add("04709d163", "Other");
                Expressions.Add("04911d75", "Other");
                Expressions.Add("02463d666", "Other");
                Expressions.Add("04472d326", "Other");
                Expressions.Add("04851d170", "Other");
                Expressions.Add("04560d390", "Other");
                Expressions.Add("04684d302", "Other");
                Expressions.Add("04334d424", "Other");
                Expressions.Add("04470d411", "Other");
                Expressions.Add("04782d205", "Other");
                Expressions.Add("04842d170", "Other");
                Expressions.Add("04849d134", "Other");
                Expressions.Add("04730d128", "Other");
                Expressions.Add("04727d174", "Other");
                Expressions.Add("04482d416", "Other");
                Expressions.Add("04856d94", "Other");
                Expressions.Add("04239d492", "Other");
                Expressions.Add("04557d455", "Other");
                Expressions.Add("04673d281", "Other");
                Expressions.Add("04701d161", "Other");
                Expressions.Add("04347d401", "Other");
                Expressions.Add("04460d334", "Other");
                Expressions.Add("04732d14", "Other");
                Expressions.Add("04449d255", "Other");
                Expressions.Add("04758d135", "Other");
                Expressions.Add("04929d52", "Other");
                Expressions.Add("04915d60", "Other");
                Expressions.Add("04821d106", "Other");
                Expressions.Add("04600d363", "Other");
                Expressions.Add("04397d458", "Other");
                Expressions.Add("04734d192", "Other");
                Expressions.Add("04841d146", "Other");
                Expressions.Add("04745d172", "Other");
                Expressions.Add("04670d277", "Other");
                Expressions.Add("04596d176", "Other");
                Expressions.Add("04810d129", "Other");
                Expressions.Add("04495d435", "Other");
                Expressions.Add("04344d347", "Other");
                Expressions.Add("04626d357", "Other");
                Expressions.Add("04446d379", "Other");
                Expressions.Add("04475d126", "Other");
                Expressions.Add("04427d374", "Other");
                Expressions.Add("04754d190", "Other");
                Expressions.Add("04899d88", "Other");
                Expressions.Add("04708d173", "Other");
                Expressions.Add("04400d384", "Other");
                Expressions.Add("04714d198", "Other");
                Expressions.Add("04891d87", "Other");
                Expressions.Add("04843d161", "Other");
                Expressions.Add("04588d241", "Other");
                Expressions.Add("04691d136", "Other");
                Expressions.Add("04894d78", "Other");
                Expressions.Add("04683d349", "Other");
                Expressions.Add("04622d332", "Other");
                Expressions.Add("04632d242", "Other");
                Expressions.Add("04773d191", "Other");
                Expressions.Add("04633d300", "Other");
                Expressions.Add("04320d348", "Other");
                Expressions.Add("04530d439", "Other");
                Expressions.Add("04816d68", "Other");
                Expressions.Add("04910d67", "Other");
                Expressions.Add("04924d74", "Other");
                Expressions.Add("04892d98", "Other");
                Expressions.Add("04900d98", "Other");
                Expressions.Add("04485d408", "Other");
                Expressions.Add("04925d34", "Other");
                Expressions.Add("04775d140", "Other");
                Expressions.Add("04429d453", "Other");
                Expressions.Add("04768d154", "Other");
                Expressions.Add("04932d44", "Other");
                Expressions.Add("04202d566", "Other");
                Expressions.Add("04514d446", "Other");
                Expressions.Add("04711d161", "Other");
                Expressions.Add("04867d82", "Other");
                Expressions.Add("04404d219", "Other");
                Expressions.Add("04569d380", "Other");
                Expressions.Add("04286d377", "Other");
                Expressions.Add("04798d84", "Other");
                Expressions.Add("04813d150", "Other");
                Expressions.Add("04419d328", "Other");
                Expressions.Add("04312d217", "Other");
                Expressions.Add("04444d242", "Other");
                Expressions.Add("04922d68", "Other");
                Expressions.Add("04387d437", "Other");
                Expressions.Add("04822d156", "Other");
                Expressions.Add("04839d186", "Other");
                Expressions.Add("04430d379", "Other");
                Expressions.Add("04297d311", "Other");
                Expressions.Add("04748d117", "Other");
                Expressions.Add("04829d116", "Other");
                Expressions.Add("04790d100", "Other");
                Expressions.Add("04585d278", "Other");
                Expressions.Add("04461d417", "Other");
                Expressions.Add("04763d162", "Other");
                Expressions.Add("04512d442", "Other");
                Expressions.Add("04697d188", "Other");
                Expressions.Add("04744d152", "Other");
                Expressions.Add("04440d123", "Other");
                Expressions.Add("04351d102", "Other");
                Expressions.Add("04372d337", "Other");
                Expressions.Add("04866d96", "Other");
                Expressions.Add("04726d144", "Other");
                Expressions.Add("04743d144", "Other");
                Expressions.Add("04762d101", "Other");
                Expressions.Add("04772d40", "Other");
                Expressions.Add("04876d74", "Other");
                Expressions.Add("04877d22", "Other");
                Expressions.Add("04476d226", "Other");
                Expressions.Add("04703d122", "Other");
                Expressions.Add("04712d105", "Other");
                Expressions.Add("04780d108", "Other");
                Expressions.Add("04201d438", "Other");
                Expressions.Add("04505d328", "Other");
                Expressions.Add("04408d368", "Other");
                Expressions.Add("04699d112", "Other");
                Expressions.Add("04738d96", "Other");
                Expressions.Add("04456d353", "Other");
                Expressions.Add("04747d108", "Other");
                Expressions.Add("04720d76", "Other");
                Expressions.Add("04324d354", "Other");
                Expressions.Add("04675d349", "Other");
                Expressions.Add("04553d266", "Other");
                Expressions.Add("04615d192", "Other");
                Expressions.Add("04848d150", "Other");
                Expressions.Add("04702d134", "Other");
                Expressions.Add("04613d184", "Other");
                Expressions.Add("04903d34", "Other");
                Expressions.Add("04811d114", "Other");
                Expressions.Add("04715d80", "Other");
                Expressions.Add("04265d343", "Other");
                Expressions.Add("04855d88", "Other");
                Expressions.Add("04863d62", "Other");
                Expressions.Add("04531d397", "Other");
                Expressions.Add("04917d96", "Other");
                Expressions.Add("04774d160", "Other");
                Expressions.Add("04916d92", "Other");
                Expressions.Add("04854d146", "Other");
                Expressions.Add("04870d90", "Other");
                Expressions.Add("04776d103", "Other");
                Expressions.Add("04233d506", "Other");
                Expressions.Add("04203d548", "Other");
                Expressions.Add("04343d435", "Other");
                Expressions.Add("04893d72", "Other");
                Expressions.Add("04757d159", "Other");
                Expressions.Add("04556d419", "Other");
                Expressions.Add("04339d296", "Other");
                Expressions.Add("04901d78", "Other");
                Expressions.Add("04890d78", "Other");
                Expressions.Add("04838d160", "Other");
                Expressions.Add("04509d392", "Other");
                Expressions.Add("04827d124", "Other");
                Expressions.Add("04853d156", "Other");
                Expressions.Add("04481d407", "NoExpression");
                Expressions.Add("04887d82", "NoExpression");
                Expressions.Add("04440d125", "NoExpression");
                Expressions.Add("04512d444", "NoExpression");
                Expressions.Add("04418d400", "NoExpression");
                Expressions.Add("04633d302", "NoExpression");
                Expressions.Add("04840d162", "NoExpression");
                Expressions.Add("04856d96", "NoExpression");
                Expressions.Add("04385d445", "NoExpression");
                Expressions.Add("04580d421", "NoExpression");
                Expressions.Add("04719d189", "NoExpression");
                Expressions.Add("04361d197", "NoExpression");
                Expressions.Add("04667d318", "NoExpression");
                Expressions.Add("04797d158", "NoExpression");
                Expressions.Add("04923d58", "NoExpression");
                Expressions.Add("04626d359", "NoExpression");
                Expressions.Add("04726d146", "NoExpression");
                Expressions.Add("04866d98", "NoExpression");
                Expressions.Add("04815d206", "NoExpression");
                Expressions.Add("04782d207", "NoExpression");
                Expressions.Add("04334d426", "NoExpression");
                Expressions.Add("04684d304", "NoExpression");
                Expressions.Add("04911d77", "NoExpression");
                Expressions.Add("04847d172", "NoExpression");
                Expressions.Add("04842d172", "NoExpression");
                Expressions.Add("04673d283", "NoExpression");
                Expressions.Add("04851d172", "NoExpression");
                Expressions.Add("04615d194", "NoExpression");
                Expressions.Add("04833d162", "NoExpression");
                Expressions.Add("04327d404", "NoExpression");
                Expressions.Add("04394d411", "NoExpression");
                Expressions.Add("04470d413", "NoExpression");
                Expressions.Add("04868d80", "NoExpression");
                Expressions.Add("04896d96", "NoExpression");
                Expressions.Add("04482d418", "NoExpression");
                Expressions.Add("04936d100", "NoExpression");
                Expressions.Add("04603d261", "NoExpression");
                Expressions.Add("04855d90", "NoExpression");
                Expressions.Add("04881d104", "NoExpression");
                Expressions.Add("04749d186", "NoExpression");
                Expressions.Add("04787d58", "NoExpression");
                Expressions.Add("04202d568", "NoExpression");
                Expressions.Add("04319d274", "NoExpression");
                Expressions.Add("04587d112", "NoExpression");
                Expressions.Add("04865d104", "NoExpression");
                Expressions.Add("04724d146", "NoExpression");
                Expressions.Add("04408d370", "NoExpression");
                Expressions.Add("04588d243", "NoExpression");
                Expressions.Add("04822d158", "NoExpression");
                Expressions.Add("04609d205", "NoExpression");
                Expressions.Add("04557d457", "NoExpression");
                Expressions.Add("04831d166", "NoExpression");
                Expressions.Add("04670d279", "NoExpression");
                Expressions.Add("04708d175", "NoExpression");
                Expressions.Add("04870d92", "NoExpression");
                Expressions.Add("04320d350", "NoExpression");
                Expressions.Add("04400d386", "NoExpression");
                Expressions.Add("04727d176", "NoExpression");
                Expressions.Add("04344d349", "NoExpression");
                Expressions.Add("04560d392", "NoExpression");
                Expressions.Add("04446d381", "NoExpression");
                Expressions.Add("04284d61", "NoExpression");
                Expressions.Add("04644d264", "NoExpression");
                Expressions.Add("04530d441", "NoExpression");
                Expressions.Add("04816d70", "NoExpression");
                Expressions.Add("04427d376", "NoExpression");
                Expressions.Add("04754d192", "NoExpression");
                Expressions.Add("04745d174", "NoExpression");
                Expressions.Add("04849d136", "NoExpression");
                Expressions.Add("04429d455", "NoExpression");
                Expressions.Add("04768d156", "NoExpression");
                Expressions.Add("04730d130", "NoExpression");
                Expressions.Add("04773d193", "NoExpression");
                Expressions.Add("04691d138", "NoExpression");
                Expressions.Add("04894d80", "NoExpression");
                Expressions.Add("04203d550", "NoExpression");
                Expressions.Add("04312d219", "NoExpression");
                Expressions.Add("02463d668", "NoExpression");
                Expressions.Add("04869d64", "NoExpression");
                Expressions.Add("04810d131", "NoExpression");
                Expressions.Add("04683d351", "NoExpression");
                Expressions.Add("04451d319", "NoExpression");
                Expressions.Add("04790d102", "NoExpression");
                Expressions.Add("04472d328", "NoExpression");
                Expressions.Add("04434d191", "NoExpression");
                Expressions.Add("04351d104", "NoExpression");
                Expressions.Add("04744d154", "NoExpression");
                Expressions.Add("04488d398", "NoExpression");
                Expressions.Add("04892d100", "NoExpression");
                Expressions.Add("04928d100", "NoExpression");
                Expressions.Add("04725d54", "NoExpression");
                Expressions.Add("04786d154", "NoExpression");
                Expressions.Add("04853d158", "NoExpression");
                Expressions.Add("04711d163", "NoExpression");
                Expressions.Add("04803d174", "NoExpression");
                Expressions.Add("04796d148", "NoExpression");
                Expressions.Add("04883d94", "NoExpression");
                Expressions.Add("04876d76", "NoExpression");
                Expressions.Add("04743d146", "NoExpression");
                Expressions.Add("04905d62", "NoExpression");
                Expressions.Add("04728d100", "NoExpression");
                Expressions.Add("04449d257", "NoExpression");
                Expressions.Add("04309d251", "NoExpression");
                Expressions.Add("04882d70", "NoExpression");
                Expressions.Add("04917d98", "NoExpression");
                Expressions.Add("04505d330", "NoExpression");
                Expressions.Add("04239d494", "NoExpression");
                Expressions.Add("04731d139", "NoExpression");
                Expressions.Add("04595d149", "NoExpression");
                Expressions.Add("04509d394", "NoExpression");
                Expressions.Add("04827d126", "NoExpression");
                Expressions.Add("04350d326", "NoExpression");
                Expressions.Add("04233d508", "NoExpression");
                Expressions.Add("04843d163", "NoExpression");
                Expressions.Add("04916d94", "NoExpression");
                Expressions.Add("04507d418", "NoExpression");
                Expressions.Add("04798d86", "NoExpression");
                Expressions.Add("04848d152", "NoExpression");
                Expressions.Add("04701d163", "NoExpression");
                Expressions.Add("04760d174", "NoExpression");
                Expressions.Add("04901d80", "NoExpression");
                Expressions.Add("04890d80", "NoExpression");
                Expressions.Add("04854d148", "NoExpression");
                Expressions.Add("04430d381", "NoExpression");
                Expressions.Add("04423d276", "NoExpression");
                Expressions.Add("04838d162", "NoExpression");
                Expressions.Add("04757d161", "NoExpression");
                Expressions.Add("04556d421", "NoExpression");
                Expressions.Add("04542d194", "NoExpression");
                Expressions.Add("04830d144", "NoExpression");
                Expressions.Add("04479d266", "NoExpression");
                Expressions.Add("04749d188", "NoExpression");
                Expressions.Add("04719d191", "NoExpression");
                Expressions.Add("04840d164", "NoExpression");
                Expressions.Add("04626d361", "NoExpression");
                Expressions.Add("04470d415", "NoExpression");
                Expressions.Add("04327d406", "NoExpression");
                Expressions.Add("04394d413", "NoExpression");
                Expressions.Add("04615d196", "NoExpression");
                Expressions.Add("04667d320", "NoExpression");
                Expressions.Add("04911d79", "NoExpression");
                Expressions.Add("04856d98", "NoExpression");
                Expressions.Add("04336d405", "NoExpression");
                Expressions.Add("04202d570", "NoExpression");
                Expressions.Add("04575d410", "NoExpression");
                Expressions.Add("04797d160", "NoExpression");
                Expressions.Add("04633d304", "NoExpression");
                Expressions.Add("04726d148", "NoExpression");
                Expressions.Add("04334d428", "NoExpression");
                Expressions.Add("04923d60", "NoExpression");
                Expressions.Add("04430d383", "NoExpression");
                Expressions.Add("04832d150", "NoExpression");
                Expressions.Add("04782d209", "NoExpression");
                Expressions.Add("04847d174", "NoExpression");
                Expressions.Add("04851d174", "NoExpression");
                Expressions.Add("04708d177", "NoExpression");
                Expressions.Add("04684d306", "NoExpression");
                Expressions.Add("04870d94", "NoExpression");
                Expressions.Add("04730d132", "NoExpression");
                Expressions.Add("04580d423", "NoExpression");
                Expressions.Add("04385d447", "NoExpression");
                Expressions.Add("04833d164", "NoExpression");
                Expressions.Add("04849d138", "NoExpression");
                Expressions.Add("02463d670", "NoExpression");
                Expressions.Add("04514d448", "NoExpression");
                Expressions.Add("04903d36", "NoExpression");
                Expressions.Add("04767d106", "NoExpression");
                Expressions.Add("04482d420", "NoExpression");
                Expressions.Add("04400d388", "NoExpression");
                Expressions.Add("04214d155", "NoExpression");
                Expressions.Add("04868d82", "NoExpression");
                Expressions.Add("04429d457", "NoExpression");
                Expressions.Add("04871d68", "NoExpression");
                Expressions.Add("04701d165", "NoExpression");
                Expressions.Add("04629d258", "NoExpression");
                Expressions.Add("04473d243", "NoExpression");
                Expressions.Add("04603d263", "NoExpression");
                Expressions.Add("04896d98", "NoExpression");
                Expressions.Add("04936d102", "NoExpression");
                Expressions.Add("04301d357", "NoExpression");
                Expressions.Add("04692d144", "NoExpression");
                Expressions.Add("04588d245", "NoExpression");
                Expressions.Add("04675d351", "NoExpression");
                Expressions.Add("04530d443", "NoExpression");
                Expressions.Add("04451d321", "NoExpression");
                Expressions.Add("04628d275", "NoExpression");
                Expressions.Add("04673d285", "NoExpression");
                Expressions.Add("04878d90", "NoExpression");
                Expressions.Add("04453d363", "NoExpression");
                Expressions.Add("04743d148", "NoExpression");
                Expressions.Add("04907d86", "NoExpression");
                Expressions.Add("04347d405", "NoExpression");
                Expressions.Add("04727d178", "NoExpression");
                Expressions.Add("04758d137", "NoExpression");
                Expressions.Add("04711d165", "NoExpression");
                Expressions.Add("04531d399", "NoExpression");
                Expressions.Add("04600d367", "NoExpression");
                Expressions.Add("04821d108", "NoExpression");
                Expressions.Add("04853d160", "NoExpression");
                Expressions.Add("04777d200", "NoExpression");
                Expressions.Add("04560d394", "NoExpression");
                Expressions.Add("04734d194", "NoExpression");
                Expressions.Add("04461d419", "NoExpression");
                Expressions.Add("04609d207", "NoExpression");
                Expressions.Add("04221d551", "NoExpression");
                Expressions.Add("04512d446", "NoExpression");
                Expressions.Add("04495d437", "NoExpression");
                Expressions.Add("04203d552", "NoExpression");
                Expressions.Add("04481d409", "NoExpression");
                Expressions.Add("04910d69", "NoExpression");
                Expressions.Add("04786d156", "NoExpression");
                Expressions.Add("04754d194", "NoExpression");
                Expressions.Add("04427d378", "NoExpression");
                Expressions.Add("04670d281", "NoExpression");
                Expressions.Add("04312d221", "NoExpression");
                Expressions.Add("04815d208", "NoExpression");
                Expressions.Add("04446d383", "NoExpression");
                Expressions.Add("04475d128", "NoExpression");
                Expressions.Add("04419d330", "NoExpression");
                Expressions.Add("04796d150", "NoExpression");
                Expressions.Add("04436d366", "NoExpression");
                Expressions.Add("04760d176", "NoExpression");
                Expressions.Add("04773d195", "NoExpression");
                Expressions.Add("04823d140", "NoExpression");
                Expressions.Add("04418d402", "NoExpression");
                Expressions.Add("04622d334", "NoExpression");
                Expressions.Add("04883d96", "NoExpression");
                Expressions.Add("04933d66", "NoExpression");
                Expressions.Add("04810d133", "NoExpression");
                Expressions.Add("04745d176", "NoExpression");
                Expressions.Add("04569d382", "NoExpression");
                Expressions.Add("04714d200", "NoExpression");
                Expressions.Add("04831d168", "NoExpression");
                Expressions.Add("04830d146", "NoExpression");
                Expressions.Add("04587d114", "NoExpression");
                Expressions.Add("04841d148", "NoExpression");
                Expressions.Add("04765d168", "NoExpression");
                Expressions.Add("04598d361", "NoExpression");
                Expressions.Add("04776d105", "NoExpression");
                Expressions.Add("04709d165", "NoExpression");
                Expressions.Add("04472d330", "NoExpression");
                Expressions.Add("04922d70", "NoExpression");
                Expressions.Add("04928d102", "NoExpression");
                Expressions.Add("04379d363", "NoExpression");
                Expressions.Add("04803d176", "NoExpression");
                Expressions.Add("04557d459", "NoExpression");
                Expressions.Add("04757d163", "NoExpression");
                Expressions.Add("04556d423", "NoExpression");
                Expressions.Add("04225d404", "NoExpression");
                Expressions.Add("04900d100", "NoExpression");
                Expressions.Add("04892d102", "NoExpression");
                Expressions.Add("04319d276", "NoExpression");
                Expressions.Add("04901d82", "NoExpression");
                Expressions.Add("04890d82", "NoExpression");
                Expressions.Add("04233d510", "NoExpression");
                Expressions.Add("04842d174", "NoExpression");
                Expressions.Add("04488d400", "NoExpression");
                Expressions.Add("04914d48", "NoExpression");
                Expressions.Add("04857d80", "NoExpression");
                Expressions.Add("04509d396", "NoExpression");
                Expressions.Add("04827d128", "NoExpression");
                Expressions.Add("04866d100", "NoExpression");
                Expressions.Add("04780d110", "NoExpression");
                Expressions.Add("04585d280", "NoExpression");
                Expressions.Add("04239d496", "NoExpression");
                Expressions.Add("04876d78", "NoExpression");
                Expressions.Add("04865d106", "NoExpression");
                Expressions.Add("04898d66", "NoExpression");
                Expressions.Add("04449d259", "NoExpression");
                Expressions.Add("04507d420", "NoExpression");
                Expressions.Add("04724d148", "NoExpression");
                Expressions.Add("04311d282", "NoExpression");
                Expressions.Add("04476d228", "NoExpression");
                Expressions.Add("04408d372", "NoExpression");
                Expressions.Add("04702d136", "NoExpression");
                Expressions.Add("04697d190", "NoExpression");
                Expressions.Add("04881d106", "NoExpression");
                Expressions.Add("04286d379", "NoExpression");
                Expressions.Add("04397d460", "NoExpression");
                Expressions.Add("04855d92", "NoExpression");
                Expressions.Add("04921d48", "NoExpression");
                Expressions.Add("04351d106", "NoExpression");
                Expressions.Add("04388d385", "NoExpression");
                Expressions.Add("04887d84", "NoExpression");
                Expressions.Add("04641d251", "NoExpression");
                Expressions.Add("04261d333", "NoExpression");
                Expressions.Add("04485d410", "NoExpression");
                Expressions.Add("04715d82", "NoExpression");
                Expressions.Add("04813d152", "NoExpression");
                Expressions.Add("04596d178", "NoExpression");
                Expressions.Add("04265d345", "NoExpression");
                Expressions.Add("04349d414", "NoExpression");
                Expressions.Add("04854d150", "NoExpression");
                Expressions.Add("04350d328", "NoExpression");
                Expressions.Add("04839d188", "NoExpression");
                Expressions.Add("04774d162", "NoExpression");
                Expressions.Add("04505d332", "NoExpression");
                Expressions.Add("04917d100", "NoExpression");
                Expressions.Add("04882d72", "NoExpression");
                Expressions.Add("04811d116", "NoExpression");
                Expressions.Add("04731d141", "NoExpression");
                Expressions.Add("04387d439", "NoExpression");
                Expressions.Add("04343d437", "NoExpression");
                Expressions.Add("04838d164", "NoExpression");
                Expressions.Add("04683d353", "NoExpression");
                Expressions.Add("04916d96", "NoExpression");
                Expressions.Add("04748d119", "NoExpression");
                Expressions.Add("04763d164", "NoExpression");
                Expressions.Add("04423d278", "NoExpression");
                Expressions.Add("04716d24", "NoExpression");
                Expressions.Add("04460d336", "NoExpression");
                Expressions.Add("04542d196", "NoExpression");
                Expressions.Add("04887d86", "NoExpression");
                Expressions.Add("04664d191", "NoExpression");
                Expressions.Add("04749d190", "NoExpression");
                Expressions.Add("04512d448", "NoExpression");
                Expressions.Add("04609d209", "NoExpression");
                Expressions.Add("04856d100", "NoExpression");
                Expressions.Add("04202d572", "NoExpression");
                Expressions.Add("04319d278", "NoExpression");
                Expressions.Add("04385d449", "NoExpression");
                Expressions.Add("04881d108", "NoExpression");
                Expressions.Add("04777d202", "NoExpression");
                Expressions.Add("04336d407", "NoExpression");
                Expressions.Add("04667d322", "NoExpression");
                Expressions.Add("04575d412", "NoExpression");
                Expressions.Add("04334d430", "NoExpression");
                Expressions.Add("04684d308", "NoExpression");
                Expressions.Add("04797d162", "NoExpression");
                Expressions.Add("04531d401", "NoExpression");
                Expressions.Add("04847d176", "NoExpression");
                Expressions.Add("04851d176", "NoExpression");
                Expressions.Add("04726d150", "NoExpression");
                Expressions.Add("04514d450", "NoExpression");
                Expressions.Add("04615d198", "NoExpression");
                Expressions.Add("04842d176", "NoExpression");
                Expressions.Add("04782d211", "NoExpression");
                Expressions.Add("04481d411", "NoExpression");
                Expressions.Add("04786d158", "NoExpression");
                Expressions.Add("04870d96", "NoExpression");
                Expressions.Add("04730d134", "NoExpression");
                Expressions.Add("04833d166", "NoExpression");
                Expressions.Add("04708d179", "NoExpression");
                Expressions.Add("04681d177", "NoExpression");
                Expressions.Add("04934d58", "NoExpression");
                Expressions.Add("04868d84", "NoExpression");
                Expressions.Add("04435d372", "NoExpression");
                Expressions.Add("04865d108", "NoExpression");
                Expressions.Add("04603d265", "NoExpression");
                Expressions.Add("04802d33", "NoExpression");
                Expressions.Add("04587d116", "NoExpression");
                Expressions.Add("04301d359", "NoExpression");
                Expressions.Add("04347d407", "NoExpression");
                Expressions.Add("04822d160", "NoExpression");
                Expressions.Add("04221d553", "NoExpression");
                Expressions.Add("04482d422", "NoExpression");
                Expressions.Add("04810d135", "NoExpression");
                Expressions.Add("04633d306", "NoExpression");
                Expressions.Add("04580d425", "NoExpression");
                Expressions.Add("02463d672", "NoExpression");
                Expressions.Add("04709d167", "NoExpression");
                Expressions.Add("04430d385", "NoExpression");
                Expressions.Add("04853d162", "NoExpression");
                Expressions.Add("04600d369", "NoExpression");
                Expressions.Add("04557d461", "NoExpression");
                Expressions.Add("04673d287", "NoExpression");
                Expressions.Add("04878d92", "NoExpression");
                Expressions.Add("04743d150", "NoExpression");
                Expressions.Add("04821d110", "NoExpression");
                Expressions.Add("04628d277", "NoExpression");
                Expressions.Add("04907d88", "NoExpression");
                Expressions.Add("04530d445", "NoExpression");
                Expressions.Add("04560d396", "NoExpression");
                Expressions.Add("04831d170", "NoExpression");
                Expressions.Add("04737d70", "NoExpression");
                Expressions.Add("04682d232", "NoExpression");
                Expressions.Add("04859d26", "NoExpression");
                Expressions.Add("04670d283", "NoExpression");
                Expressions.Add("04495d439", "NoExpression");
                Expressions.Add("04344d351", "NoExpression");
                Expressions.Add("04840d166", "NoExpression");
                Expressions.Add("04745d178", "NoExpression");
                Expressions.Add("04446d385", "NoExpression");
                Expressions.Add("04203d554", "NoExpression");
                Expressions.Add("04629d260", "NoExpression");
                Expressions.Add("04765d170", "NoExpression");
                Expressions.Add("04400d390", "NoExpression");
                Expressions.Add("04896d100", "NoExpression");
                Expressions.Add("04936d104", "NoExpression");
                Expressions.Add("04312d223", "NoExpression");
                Expressions.Add("04711d167", "NoExpression");
                Expressions.Add("04622d336", "NoExpression");
                Expressions.Add("04632d246", "NoExpression");
                Expressions.Add("04891d89", "NoExpression");
                Expressions.Add("04924d76", "NoExpression");
                Expressions.Add("04925d36", "NoExpression");
                Expressions.Add("04900d102", "NoExpression");
                Expressions.Add("04485d412", "NoExpression");
                Expressions.Add("04692d146", "NoExpression");
                Expressions.Add("04596d180", "NoExpression");
                Expressions.Add("04748d121", "NoExpression");
                Expressions.Add("04535d261", "NoExpression");
                Expressions.Add("04922d72", "NoExpression");
                Expressions.Add("04471d285", "NoExpression");
                Expressions.Add("04286d381", "NoExpression");
                Expressions.Add("04453d365", "NoExpression");
                Expressions.Add("04404d223", "NoExpression");
                Expressions.Add("04867d86", "NoExpression");
                Expressions.Add("04883d98", "NoExpression");
                Expressions.Add("04747d110", "NoExpression");
                Expressions.Add("04327d408", "NoExpression");
                Expressions.Add("04324d356", "NoExpression");
                Expressions.Add("04470d417", "NoExpression");
                Expressions.Add("04585d282", "NoExpression");
                Expressions.Add("04776d107", "NoExpression");
                Expressions.Add("04813d154", "NoExpression");
                Expressions.Add("04928d104", "NoExpression");
                Expressions.Add("04803d178", "NoExpression");
                Expressions.Add("04473d245", "NoExpression");
                Expressions.Add("04394d415", "NoExpression");
                Expressions.Add("04387d441", "NoExpression");
                Expressions.Add("04463d261", "NoExpression");
                Expressions.Add("04767d108", "NoExpression");
                Expressions.Add("04841d150", "NoExpression");
                Expressions.Add("04578d22", "NoExpression");
                Expressions.Add("04757d165", "NoExpression");
                Expressions.Add("04556d425", "NoExpression");
                Expressions.Add("04631d268", "NoExpression");
                Expressions.Add("04598d363", "NoExpression");
                Expressions.Add("04760d178", "NoExpression");
                Expressions.Add("04429d459", "NoExpression");
                Expressions.Add("04768d158", "NoExpression");
                Expressions.Add("04790d104", "NoExpression");
                Expressions.Add("04901d84", "NoExpression");
                Expressions.Add("04890d84", "NoExpression");
                Expressions.Add("04815d210", "NoExpression");
                Expressions.Add("04626d363", "NoExpression");
                Expressions.Add("04488d402", "NoExpression");
                Expressions.Add("04460d338", "NoExpression");
                Expressions.Add("04763d166", "NoExpression");
                Expressions.Add("04351d108", "NoExpression");
                Expressions.Add("04693d70", "NoExpression");
                Expressions.Add("04647d170", "NoExpression");
                Expressions.Add("04744d156", "NoExpression");
                Expressions.Add("04892d104", "NoExpression");
                Expressions.Add("04823d142", "NoExpression");
                Expressions.Add("04829d118", "NoExpression");
                Expressions.Add("04569d384", "NoExpression");
                Expressions.Add("04876d80", "NoExpression");
                Expressions.Add("04933d68", "NoExpression");
                Expressions.Add("04436d368", "NoExpression");
                Expressions.Add("04339d298", "NoExpression");
                Expressions.Add("04724d150", "NoExpression");
                Expressions.Add("04379d365", "NoExpression");
                Expressions.Add("04449d261", "NoExpression");
                Expressions.Add("04507d422", "NoExpression");
                Expressions.Add("04882d74", "NoExpression");
                Expressions.Add("04702d138", "NoExpression");
                Expressions.Add("04714d202", "NoExpression");
                Expressions.Add("04476d230", "NoExpression");
                Expressions.Add("04201d442", "NoExpression");
                Expressions.Add("04349d416", "NoExpression");
                Expressions.Add("04848d154", "NoExpression");
                Expressions.Add("04397d462", "NoExpression");
                Expressions.Add("04762d103", "NoExpression");
                Expressions.Add("04863d64", "NoExpression");
                Expressions.Add("04905d64", "NoExpression");
                Expressions.Add("04701d167", "NoExpression");
                Expressions.Add("04343d439", "NoExpression");
                Expressions.Add("04613d188", "NoExpression");
                Expressions.Add("04734d196", "NoExpression");
                Expressions.Add("04697d192", "NoExpression");
                Expressions.Add("04699d114", "NoExpression");
                Expressions.Add("04456d357", "NoExpression");
                Expressions.Add("04588d247", "NoExpression");
                Expressions.Add("04444d244", "NoExpression");
                Expressions.Add("04854d152", "NoExpression");
                Expressions.Add("04505d334", "NoExpression");
                Expressions.Add("04917d102", "NoExpression");
                Expressions.Add("04811d118", "NoExpression");
                Expressions.Add("04239d498", "NoExpression");
                Expressions.Add("04461d421", "NoExpression");
                Expressions.Add("04866d102", "NoExpression");
                Expressions.Add("04857d82", "NoExpression");
                Expressions.Add("04855d94", "NoExpression");
                Expressions.Add("04683d355", "NoExpression");
                Expressions.Add("04827d130", "NoExpression");
                Expressions.Add("04372d341", "NoExpression");
                Expressions.Add("04509d398", "NoExpression");
                Expressions.Add("04311d284", "NoExpression");
                Expressions.Add("04265d347", "NoExpression");
                Expressions.Add("04233d512", "NoExpression");
                Expressions.Add("04916d98", "NoExpression");
                Expressions.Add("04595d151", "NoExpression");
                Expressions.Add("04899d90", "NoExpression");
                Expressions.Add("04472d332", "NoExpression");
                Expressions.Add("04350d330", "NoExpression");
                Expressions.Add("04542d198", "NoExpression");
                Expressions.Add("04838d166", "NoExpression");
                Expressions.Add("04893d74", "NoExpression");
                Expressions.Add("04689d98", "NoExpression");
                Expressions.Add("04911d81", "NoExpression");
                Expressions.Add("04385d451", "NoExpression");
                Expressions.Add("04822d162", "NoExpression");
                Expressions.Add("04840d168", "NoExpression");
                Expressions.Add("04749d192", "NoExpression");
                Expressions.Add("04379d367", "NoExpression");
                Expressions.Add("04341d193", "NoExpression");
                Expressions.Add("04394d417", "NoExpression");
                Expressions.Add("04803d180", "NoExpression");
                Expressions.Add("04512d450", "NoExpression");
                Expressions.Add("04336d409", "NoExpression");
                Expressions.Add("02463d674", "NoExpression");
                Expressions.Add("04777d204", "NoExpression");
                Expressions.Add("04327d410", "NoExpression");
                Expressions.Add("04797d164", "NoExpression");
                Expressions.Add("04898d68", "NoExpression");
                Expressions.Add("04575d414", "NoExpression");
                Expressions.Add("04870d98", "NoExpression");
                Expressions.Add("04708d181", "NoExpression");
                Expressions.Add("04782d213", "NoExpression");
                Expressions.Add("04711d169", "NoExpression");
                Expressions.Add("04853d164", "NoExpression");
                Expressions.Add("04851d178", "NoExpression");
                Expressions.Add("04730d136", "NoExpression");
                Expressions.Add("04847d178", "NoExpression");
                Expressions.Add("04833d168", "NoExpression");
                Expressions.Add("04849d140", "NoExpression");
                Expressions.Add("04866d104", "NoExpression");
                Expressions.Add("04682d234", "NoExpression");
                Expressions.Add("04453d367", "NoExpression");
                Expressions.Add("04481d413", "NoExpression");
                Expressions.Add("04603d267", "NoExpression");
                Expressions.Add("04202d574", "NoExpression");
                Expressions.Add("04692d148", "NoExpression");
                Expressions.Add("04514d452", "NoExpression");
                Expressions.Add("04681d179", "NoExpression");
                Expressions.Add("04842d178", "NoExpression");
                Expressions.Add("04435d374", "NoExpression");
                Expressions.Add("04626d365", "NoExpression");
                Expressions.Add("04334d432", "NoExpression");
                Expressions.Add("04600d371", "NoExpression");
                Expressions.Add("04684d310", "NoExpression");
                Expressions.Add("04868d86", "NoExpression");
                Expressions.Add("04934d60", "NoExpression");
                Expressions.Add("04482d424", "NoExpression");
                Expressions.Add("04831d172", "NoExpression");
                Expressions.Add("04765d172", "NoExpression");
                Expressions.Add("04896d102", "NoExpression");
                Expressions.Add("04936d106", "NoExpression");
                Expressions.Add("04301d361", "NoExpression");
                Expressions.Add("04790d106", "NoExpression");
                Expressions.Add("04587d118", "NoExpression");
                Expressions.Add("04622d338", "NoExpression");
                Expressions.Add("04221d555", "NoExpression");
                Expressions.Add("04593d272", "NoExpression");
                Expressions.Add("04629d262", "NoExpression");
                Expressions.Add("04881d110", "NoExpression");
                Expressions.Add("04724d152", "NoExpression");
                Expressions.Add("04675d353", "NoExpression");
                Expressions.Add("04349d418", "NoExpression");
                Expressions.Add("04829d120", "NoExpression");
                Expressions.Add("04580d427", "NoExpression");
                Expressions.Add("04673d289", "NoExpression");
                Expressions.Add("04865d110", "NoExpression");
                Expressions.Add("04628d279", "NoExpression");
                Expressions.Add("04821d112", "NoExpression");
                Expressions.Add("04731d143", "NoExpression");
                Expressions.Add("04883d100", "NoExpression");
                Expressions.Add("04747d112", "NoExpression");
                Expressions.Add("04225d408", "NoExpression");
                Expressions.Add("04773d197", "NoExpression");
                Expressions.Add("04878d94", "NoExpression");
                Expressions.Add("04727d180", "NoExpression");
                Expressions.Add("04907d90", "NoExpression");
                Expressions.Add("04719d193", "NoExpression");
                Expressions.Add("04922d74", "NoExpression");
                Expressions.Add("04408d374", "NoExpression");
                Expressions.Add("04286d383", "NoExpression");
                Expressions.Add("04716d26", "NoExpression");
                Expressions.Add("04841d152", "NoExpression");
                Expressions.Add("04899d92", "NoExpression");
                Expressions.Add("04641d253", "NoExpression");
                Expressions.Add("04475d130", "NoExpression");
                Expressions.Add("04203d556", "NoExpression");
                Expressions.Add("04557d463", "NoExpression");
                Expressions.Add("04588d249", "NoExpression");
                Expressions.Add("04485d414", "NoExpression");
                Expressions.Add("04461d423", "NoExpression");
                Expressions.Add("04670d285", "NoExpression");
                Expressions.Add("04201d444", "NoExpression");
                Expressions.Add("04446d387", "NoExpression");
                Expressions.Add("04427d380", "NoExpression");
                Expressions.Add("04754d196", "NoExpression");
                Expressions.Add("04560d398", "NoExpression");
                Expressions.Add("04344d353", "NoExpression");
                Expressions.Add("04726d152", "NoExpression");
                Expressions.Add("04530d447", "NoExpression");
                Expressions.Add("04910d73", "NoExpression");
                Expressions.Add("04734d198", "NoExpression");
                Expressions.Add("04856d102", "NoExpression");
                Expressions.Add("04892d106", "NoExpression");
                Expressions.Add("04900d104", "NoExpression");
                Expressions.Add("04615d200", "NoExpression");
                Expressions.Add("04772d42", "NoExpression");
                Expressions.Add("04542d200", "NoExpression");
                Expressions.Add("04495d441", "NoExpression");
                Expressions.Add("04869d66", "NoExpression");
                Expressions.Add("04924d78", "NoExpression");
                Expressions.Add("04470d419", "NoExpression");
                Expressions.Add("04683d357", "NoExpression");
                Expressions.Add("04596d182", "NoExpression");
                Expressions.Add("04213d348", "NoExpression");
                Expressions.Add("04775d142", "NoExpression");
                Expressions.Add("04608d84", "NoExpression");
                Expressions.Add("04786d160", "NoExpression");
                Expressions.Add("04312d225", "NoExpression");
                Expressions.Add("04339d300", "NoExpression");
                Expressions.Add("04904d44", "NoExpression");
                Expressions.Add("04631d270", "NoExpression");
                Expressions.Add("04598d365", "NoExpression");
                Expressions.Add("04744d158", "NoExpression");
                Expressions.Add("04535d263", "NoExpression");
                Expressions.Add("04578d24", "NoExpression");
                Expressions.Add("04343d441", "NoExpression");
                Expressions.Add("04473d247", "NoExpression");
                Expressions.Add("04395d273", "NoExpression");
                Expressions.Add("04505d336", "NoExpression");
                Expressions.Add("04633d308", "NoExpression");
                Expressions.Add("04472d334", "NoExpression");
                Expressions.Add("04813d156", "NoExpression");
                Expressions.Add("04488d404", "NoExpression");
                Expressions.Add("04738d98", "NoExpression");
                Expressions.Add("04595d153", "NoExpression");
                Expressions.Add("04449d263", "NoExpression");
                Expressions.Add("04714d204", "NoExpression");
                Expressions.Add("04667d324", "NoExpression");
                Expressions.Add("04815d212", "NoExpression");
                Expressions.Add("04400d392", "NoExpression");
                Expressions.Add("04444d246", "NoExpression");
                Expressions.Add("04914d50", "NoExpression");
                Expressions.Add("04857d84", "NoExpression");
                Expressions.Add("04511d250", "NoExpression");
                Expressions.Add("04917d104", "NoExpression");
                Expressions.Add("04859d28", "NoExpression");
                Expressions.Add("04921d50", "NoExpression");
                Expressions.Add("04832d152", "NoExpression");
                Expressions.Add("04763d168", "NoExpression");
                Expressions.Add("04397d464", "NoExpression");
                Expressions.Add("04350d332", "NoExpression");
                Expressions.Add("04309d253", "NoExpression");
                Expressions.Add("04855d96", "NoExpression");
                Expressions.Add("04569d386", "NoExpression");
                Expressions.Add("04709d169", "NoExpression");
                Expressions.Add("04757d167", "NoExpression");
                Expressions.Add("04556d427", "NoExpression");
                Expressions.Add("04848d156", "NoExpression");
                Expressions.Add("04689d100", "NoExpression");
                Expressions.Add("04893d76", "NoExpression");
                Expressions.Add("04507d424", "NoExpression");
                Expressions.Add("04811d120", "NoExpression");
                Expressions.Add("04928d106", "NoExpression");
                Expressions.Add("04372d343", "NoExpression");
                Expressions.Add("04745d180", "NoExpression");
                Expressions.Add("04233d514", "NoExpression");
                Expressions.Add("04429d461", "NoExpression");
                Expressions.Add("04839d192", "NoExpression");
                Expressions.Add("04760d180", "NoExpression");
                Expressions.Add("04387d443", "NoExpression");
                Expressions.Add("04509d400", "NoExpression");
                Expressions.Add("04774d164", "NoExpression");
                Expressions.Add("04748d123", "NoExpression");
                Expressions.Add("04838d168", "NoExpression");
                Expressions.Add("04916d100", "NoExpression");
            }

            public static bool GetExpression(string fileName, out string sExpression)
            {
                return Expressions.TryGetValue(fileName, out sExpression);
            }
        }

        static public float CalculateMeanValue(List<Cl3DModel.Cl3DModelPointIterator> PointsList)
        {
            List<float> points = new List<float>();
            foreach (Cl3DModel.Cl3DModelPointIterator pt in PointsList)
                points.Add(pt.Z);

            points.Sort();

            float result = 0;
            if (points.Count % 2 == 0)// even
            {
                int first = (int)(points.Count / 2);
                result = (points[first - 1] + points[first]) / 2;
            }
            else // odd
            {
                int element = (int)(points.Count / 2);
                result = points[element];
            }
            return result;
        }

        static public void MoveReferenceModelBasedOnCorelatedPoints(List<Cl3DModel.Cl3DModelPointIterator> ReferenceModel, List<Cl3DModel.Cl3DModelPointIterator> MovingModel, Cl3DModel Model)
        {
            Matrix RotationMatrix = null;
            Matrix TranslationMatrix = null;
            ClTools.CalculateRotationAndTranslation(ReferenceModel, MovingModel, out RotationMatrix, out TranslationMatrix);

            Cl3DModel.Cl3DModelPointIterator iterToMove = Model.GetIterator();
            do
            {
                Matrix V = RotationMatrix * iterToMove + TranslationMatrix;
                iterToMove.X = (float)V[0, 0];
                iterToMove.Y = (float)V[1, 0];
                iterToMove.Z = (float)V[2, 0];
            } while (iterToMove.MoveToNext());
        }

        static public float doICP(Cl3DModel p_referenceModel, Cl3DModel p_modelToMove)
        {
            // utworzenie skorelowanych punktow wszystko do reference model dopasowane
            List<Cl3DModel.Cl3DModelPointIterator> ReferenceModelPoints = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<Cl3DModel.Cl3DModelPointIterator> ConnectedPointsWithreference = new List<Cl3DModel.Cl3DModelPointIterator>();

            Cl3DModel.Cl3DModelPointIterator RefModelIter = p_referenceModel.GetIterator();

            List<Cl3DModel.Cl3DModelPointIterator>[,] MapOfMovingModel = null;
            int width = 0;
            int height = 0;
            float MinusOffsetX = 0;
            float MinusOffsetY = 0;
            ClTools.CreateGridBasedOnRealXY(p_modelToMove, out MapOfMovingModel, out width, out height, out MinusOffsetX, out MinusOffsetY);
            do
            {
                //Search for the closes point in next model
                int RefX = (int)(RefModelIter.X - MinusOffsetX);
                int RefY = (int)(RefModelIter.Y - MinusOffsetY);

                if (RefX >= width)
                    RefX = width - 1;
                if (RefY >= height)
                    RefY = height - 1;
                if (RefX < 0)
                    RefX = 0;
                if (RefY < 0)
                    RefY = 0;


                List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                if (MapOfMovingModel[RefX, RefY] != null)
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX, RefY])
                        ListToCheck.Add(iter);
                }
                if (RefX - 1 >= 0 && RefY - 1 >= 0)
                    if (MapOfMovingModel[RefX - 1, RefY - 1] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX - 1, RefY - 1])
                            ListToCheck.Add(iter);
                    }
                if (RefX - 1 >= 0)
                    if (MapOfMovingModel[RefX - 1, RefY] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX - 1, RefY])
                            ListToCheck.Add(iter);
                    }
                if (RefX - 1 >= 0 && RefY + 1 < height)
                    if (MapOfMovingModel[RefX - 1, RefY + 1] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX - 1, RefY + 1])
                            ListToCheck.Add(iter);
                    }
                if (RefY - 1 >= 0)
                    if (MapOfMovingModel[RefX, RefY - 1] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX, RefY - 1])
                            ListToCheck.Add(iter);
                    }
                if (RefY + 1 < height)
                    if (MapOfMovingModel[RefX, RefY + 1] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX, RefY + 1])
                            ListToCheck.Add(iter);
                    }
                if (RefY - 1 >= 0 && RefX + 1 < width)
                    if (MapOfMovingModel[RefX + 1, RefY - 1] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX + 1, RefY - 1])
                            ListToCheck.Add(iter);
                    }
                if (RefX + 1 < width)
                    if (MapOfMovingModel[RefX + 1, RefY] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX + 1, RefY])
                            ListToCheck.Add(iter);
                    }
                if (RefY + 1 < height && RefX + 1 < width)
                    if (MapOfMovingModel[RefX + 1, RefY + 1] != null)
                    {
                        foreach (Cl3DModel.Cl3DModelPointIterator iter in MapOfMovingModel[RefX + 1, RefY + 1])
                            ListToCheck.Add(iter);
                    }

                float minValue = 0;
                Cl3DModel.Cl3DModelPointIterator ClosesPoint = null;

                foreach (Cl3DModel.Cl3DModelPointIterator iterToCheck in ListToCheck)
                {
                    if (ClosesPoint == null)
                    {
                        minValue = RefModelIter - iterToCheck;
                        ClosesPoint = iterToCheck.CopyIterator();
                    }
                    else
                    {
                        float currDistance = RefModelIter - iterToCheck;
                        if (currDistance < minValue)
                        {
                            minValue = currDistance;
                            ClosesPoint = iterToCheck.CopyIterator();
                        }
                    }
                }
                if (ClosesPoint == null)
                    continue;

                ConnectedPointsWithreference.Add(ClosesPoint);
                ReferenceModelPoints.Add(RefModelIter.CopyIterator());

            } while (RefModelIter.MoveToNext() && RefModelIter.MoveToNext() && RefModelIter.MoveToNext() && RefModelIter.MoveToNext());

            Matrix RotationMatrix = null;
            Matrix TranslationMatrix = null;
            ClTools.CalculateRotationAndTranslation(ReferenceModelPoints, ConnectedPointsWithreference, out RotationMatrix, out TranslationMatrix);

            Cl3DModel.Cl3DModelPointIterator iterToMove = p_modelToMove.GetIterator();
            do
            {
                Matrix V = RotationMatrix * iterToMove + TranslationMatrix;
                iterToMove.X = (float)V[0, 0];
                iterToMove.Y = (float)V[1, 0];
                iterToMove.Z = (float)V[2, 0];

            } while (iterToMove.MoveToNext());

            float Distance = 0;
            for (int i = 0; i < ConnectedPointsWithreference.Count; i++)
            {
                Distance += ConnectedPointsWithreference[i] - ReferenceModelPoints[i];
            }
            return Distance;
        }

        static public string ExtractOryginalFileNameFRGC(string fileName)
        {
            string OryginalFileName = fileName;

            int i = 0;
            for (i = 0; i < OryginalFileName.Length; i++)
            {
                if (Char.IsDigit(OryginalFileName[i]) || OryginalFileName[i].Equals('d'))
                    continue;
                else
                    break;
            }
            OryginalFileName = OryginalFileName.Substring(0, i);
            return OryginalFileName;
        }

        static public string GetModelNameFromFRGCFileName(string p_filePath)
        {
            string modelName = "";
            int indexOfDot = p_filePath.LastIndexOf('.');
            int indexOfBackslesh = p_filePath.LastIndexOf('\\');
            int indexOfSlesh = p_filePath.LastIndexOf('/');
            if (indexOfDot == -1)
                throw new Exception("Unknown file type");

            if (indexOfBackslesh == -1 && indexOfSlesh != -1)
                modelName = p_filePath.Substring(++indexOfSlesh, indexOfDot - indexOfSlesh);
            else if (indexOfBackslesh != -1 && indexOfSlesh == -1)
                modelName = p_filePath.Substring(++indexOfBackslesh, indexOfDot - indexOfBackslesh);
            else
                throw new Exception("Wrong file name");

            string[] nameAndID = modelName.Split('d');

            if (nameAndID.Length == 0)
                throw new Exception("Wrong length of Name and ID");

            return nameAndID[0];
        }

        static public void TakePointsBetween(int X0, int Y0, int X1, int Y1, int OffsetX, int OffsetY, ref Cl3DModel.Cl3DModelPointIterator[,] Map, ref List<Cl3DModel.Cl3DModelPointIterator> pList)
        {
            bool Switched = false;
            if (X0 > X1)
            {
                X1++;
                if (Y0 < Y1)
                    Y0++;
                else
                    Y0--;

                int tmp = X0;
                X0 = X1;
                X1 = tmp;

                tmp = Y0;
                Y0 = Y1;
                Y1 = tmp;


                Switched = true;
            }
            else
            {
                X1--;
                if (Y0 < Y1)
                    Y0++;
                else
                    Y0--;
            }

            for (int x = X0; x <= X1; x++)
            {
                float procentX = (float)(x - X0) / (float)(X1 - X0);
                int y = (int)(((Y1 - Y0) * procentX) + Y0);

                if (!Switched)
                {
                    if (y > Y0)
                        for (int i = y; i >= Y0; i--)
                        {
                            Map[x - OffsetX, i - OffsetY].AlreadyVisited = true;
                            pList.Add(Map[x - OffsetX, i - OffsetY]);
                        }
                    else
                        for (int i = y; i <= Y0; i++)
                        {
                            Map[x - OffsetX, i - OffsetY].AlreadyVisited = true;
                            pList.Add(Map[x - OffsetX, i - OffsetY]);
                        }
                }
                else
                {
                    if (y > Y1)
                        for (int i = y; i >= Y1; i--)
                        {
                            Map[x - OffsetX, i - OffsetY].AlreadyVisited = true;
                            pList.Add(Map[x - OffsetX, i - OffsetY]);
                        }
                    else
                        for (int i = y; i <= Y1; i++)
                        {
                            Map[x - OffsetX, i - OffsetY].AlreadyVisited = true;
                            pList.Add(Map[x - OffsetX, i - OffsetY]);
                        }
                }

                Map[x - OffsetX, y - OffsetY].AlreadyVisited = true;
                pList.Add(Map[x - OffsetX, y - OffsetY]);
            }
        }

        static public float CalculateTriangleArea(ClTriangle pTriangle)
        {
            float x12 = pTriangle.m_point2.X - pTriangle.m_point1.X;
            float y12 = pTriangle.m_point2.Y - pTriangle.m_point1.Y;
            float z12 = pTriangle.m_point2.Z - pTriangle.m_point1.Z;

            float x13 = pTriangle.m_point3.X - pTriangle.m_point1.X;
            float y13 = pTriangle.m_point3.Y - pTriangle.m_point1.Y;
            float z13 = pTriangle.m_point3.Z - pTriangle.m_point1.Z;

            float X = y12 * z13 - z12 * y13;
            float Y = z12 * x13 - x12 * z13;
            float Z = x12 * y13 - y12 * x13;

            double parallelogramArea = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            float triangleArea = (float)(parallelogramArea / 2);

            return triangleArea;
        }

        static public void GetListOfTriangles(out List<ClTriangle> triangles, Cl3DModel.Cl3DModelPointIterator iter)
        {
            triangles = new List<ClTriangle>();

            List<Cl3DModel.Cl3DModelPointIterator> Neighbors = iter.GetListOfNeighbors();
            for (int i = 0; i < Neighbors.Count; i++)
            {
                for (int j = i; j < Neighbors.Count; j++)
                {
                    if (Neighbors[i].IsPointInNeighbors(Neighbors[j].PointID))
                    {
                        triangles.Add(new ClTriangle(iter.CopyIterator(), Neighbors[i].CopyIterator(), Neighbors[j].CopyIterator()));
                    }
                }
            }
        }

        static public void CreateGridBasedOnRealXY(Cl3DModel p_Model, out List<Cl3DModel.Cl3DModelPointIterator>[,] p_Map, out int p_Width, out int p_Height, out float p_MinusOffsetX, out float p_MinusOffsetY)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Model iterator not Valid");

            float MinX;
            float MaxX;
            float MinY;
            float MaxY;

            MinX = iter.X;
            MaxX = iter.X;
            MinY = iter.Y;
            MaxY = iter.Y;

            do
            {
                if (MinX > iter.X)
                    MinX = iter.X;
                if (MaxX < iter.X)
                    MaxX = iter.X;

                if (MinY > iter.Y)
                    MinY = iter.Y;
                if (MaxY < iter.Y)
                    MaxY = iter.Y;
            } while (iter.MoveToNext());

            p_Width = (int)(MaxX - MinX) + 1;
            p_Height = (int)(MaxY - MinY) + 1;
            p_Map = new List<Cl3DModel.Cl3DModelPointIterator>[p_Width, p_Height];

            p_MinusOffsetX = MinX;
            p_MinusOffsetY = MinY;

            iter = p_Model.GetIterator();
            do
            {
                int x = (int)(iter.X - MinX);
                int y = (int)(iter.Y - MinY);
                if (p_Map[x, y] == null)
                    p_Map[x, y] = new List<Cl3DModel.Cl3DModelPointIterator>();

                p_Map[x, y].Add(iter.CopyIterator());
            } while (iter.MoveToNext());
        }

        static public void CreateGridBasedOnRangeValues(Cl3DModel p_Model, out Cl3DModel.Cl3DModelPointIterator[,] p_Map, out int p_Width, out int p_Height, out int p_OffsetX, out int p_OffsetY)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Model iterator is not Valid");

            int MinX;
            int MaxX;
            int MinY;
            int MaxY;

            MinX = iter.RangeImageX;
            MaxX = iter.RangeImageX;
            MinY = iter.RangeImageY;
            MaxY = iter.RangeImageY;

            do
            {
                if (MinX > iter.RangeImageX)
                    MinX = iter.RangeImageX;
                if (MaxX < iter.RangeImageX)
                    MaxX = iter.RangeImageX;

                if (MinY > iter.RangeImageY)
                    MinY = iter.RangeImageY;
                if (MaxY < iter.RangeImageY)
                    MaxY = iter.RangeImageY;
            } while (iter.MoveToNext());

            p_Width = (MaxX - MinX) + 1;
            p_Height = (MaxY - MinY) + 1;
            p_Map = new Cl3DModel.Cl3DModelPointIterator[p_Width, p_Height];

            p_OffsetX = MinX;
            p_OffsetY = MinY;

            iter = p_Model.GetIterator();
            do
            {
                p_Map[iter.RangeImageX - MinX, iter.RangeImageY - MinY] = iter.CopyIterator();
            } while (iter.MoveToNext());
        }

        static public void sortModelPoints(Cl3DModel p_model, out Dictionary<int, List<Cl3DModel.Cl3DModelPointIterator>> p_SortedDictionary)
        {
            p_SortedDictionary = new Dictionary<int, List<Cl3DModel.Cl3DModelPointIterator>>();
            Cl3DModel.Cl3DModelPointIterator iter = p_model.GetIterator();
            do
            {
                List<Cl3DModel.Cl3DModelPointIterator> SameDistancePoints = null;

                int dist = (int)Math.Sqrt(Math.Pow(iter.X, 2) + Math.Pow(iter.Y, 2) + Math.Pow(iter.Z, 2));
                if (!p_SortedDictionary.TryGetValue(dist, out SameDistancePoints))
                {
                    SameDistancePoints = new List<Cl3DModel.Cl3DModelPointIterator>();
                    p_SortedDictionary.Add(dist, SameDistancePoints);
                }
                SameDistancePoints.Add(iter.CopyIterator());

            } while (iter.MoveToNext());
        }

        static public float DescriptorsDistance(List<float> p_descriptor1, List<float> p_descriptor2)
        {
            if (p_descriptor1.Count != p_descriptor2.Count)
                throw new Exception("Cannot compare two descriptors, they have different length");

            double difference = 0;
            for (int i = 0; i < p_descriptor1.Count; i++)
            {
                difference += Math.Pow(p_descriptor1[i] - p_descriptor2[i], 2);
            }
            difference = Math.Sqrt(difference);
            return (float)difference;
        }

        static public float DescriptorsDistance(float[] p_descriptor1, float[] p_descriptor2)
        {
            if (p_descriptor1.Length != p_descriptor2.Length)
                throw new Exception("Cannot compare two descriptors, they have different length");

            double difference = 0;
            for (int i = 0; i < p_descriptor1.Length; i++)
            {
                difference += Math.Pow(p_descriptor1[i] - p_descriptor2[i], 2);
            }
            difference = Math.Sqrt(difference);
            return (float)difference;
        }
        static public float DescriptorsDistance(Matrix p_descriptor1, Matrix p_descriptor2)
        {
            if (p_descriptor1.RowCount != p_descriptor2.RowCount || p_descriptor1.ColumnCount != p_descriptor2.ColumnCount)
                throw new Exception("Cannot compare two descriptors, they have different length");

            double difference = 0;
            for (int i = 0; i < p_descriptor1.RowCount; i++)
            {
                for (int j = 0; j < p_descriptor1.ColumnCount; j++)
                {
                    difference += Math.Pow(p_descriptor1[i, j] - p_descriptor2[i, j], 2);
                }
            }
            difference = Math.Sqrt(difference);
            return (float)difference;
        }

        static public void GetPathBetweenPoints(Cl3DModel.Cl3DModelPointIterator p_StartPoint, Cl3DModel.Cl3DModelPointIterator p_EndPoint, out List<Cl3DModel.Cl3DModelPointIterator> p_Path)
        {
            p_Path = new List<Cl3DModel.Cl3DModelPointIterator>();
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(p_StartPoint, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceFromPoint.ToString());
            Cl3DModel.Cl3DModelPointIterator iter = p_EndPoint.CopyIterator();
            while (iter.IsSpecificValueCalculated("Path"))
            {
                uint PiD = (uint)iter.GetSpecificValue("Path");
                iter.MoveToPoint(PiD);
                p_Path.Add(iter.CopyIterator());
            }
            p_Path.Add(p_StartPoint);
            Cl3DModel.Cl3DModelPointIterator itRemove = p_StartPoint.GetManagedModel().GetIterator();
            do
            {
                itRemove.RemoveSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceFromPoint);
                itRemove.RemoveSpecificValue("Path");
            } while (itRemove.MoveToNext());
        }

        static public void DividePointsToRegions(List<Cl3DModel.Cl3DModelPointIterator> p_ListOfPoints, out List<List<Cl3DModel.Cl3DModelPointIterator>> p_Regions)
        {
            foreach (Cl3DModel.Cl3DModelPointIterator point in p_ListOfPoints)
                point.AlreadyVisited = true;

            p_Regions = new List<List<Cl3DModel.Cl3DModelPointIterator>>();
            foreach (Cl3DModel.Cl3DModelPointIterator point in p_ListOfPoints)
            {
                if (!point.AlreadyVisited)
                    continue;

                List<Cl3DModel.Cl3DModelPointIterator> newRegion = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> listToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                List<Cl3DModel.Cl3DModelPointIterator> NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                listToCheck.Add(point);

                do
                {
                    foreach (Cl3DModel.Cl3DModelPointIterator pointToCheck in listToCheck)
                    {
                        if (pointToCheck.AlreadyVisited)
                        {
                            pointToCheck.AlreadyVisited = false;
                            newRegion.Add(pointToCheck);
                            foreach (Cl3DModel.Cl3DModelPointIterator Neighbor in pointToCheck.GetListOfNeighbors())
                            {
                                if (Neighbor.AlreadyVisited)
                                    NewListToCheck.Add(Neighbor);
                            }
                        }
                    }
                    listToCheck.Clear();
                    foreach (Cl3DModel.Cl3DModelPointIterator newPoint in NewListToCheck)
                        listToCheck.Add(newPoint);
                    NewListToCheck.Clear();

                } while (listToCheck.Count != 0);
                p_Regions.Add(newRegion);
            }
            // sort regions according to the dimensionality
            p_Regions.Sort(new ClFindNoseTipAndEyesHKClassification.ClCompareCount());
            foreach (Cl3DModel.Cl3DModelPointIterator point in p_ListOfPoints)
                point.AlreadyVisited = false;
        }

        static public void FindMaxGaussianCurvaturePointInRegion(List<Cl3DModel.Cl3DModelPointIterator> p_Region, ref Cl3DModel.Cl3DModelPointIterator p_MaxPoint)
        {
            double MaxCurv = 0;
            bool first = true;

            foreach (Cl3DModel.Cl3DModelPointIterator Point in p_Region)
            {
                if (first)
                {
                    p_MaxPoint = Point.CopyIterator();
                    if (!Point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Gaussian_25, out MaxCurv))
                        continue;
                    first = false;
                }
                else
                {
                    double localMax;
                    if (!Point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Gaussian_25, out localMax))
                        continue;

                    if (localMax > MaxCurv)
                    {
                        MaxCurv = localMax;
                        p_MaxPoint = Point.CopyIterator();
                    }
                }
            }
        }

        // --------------------- Geting of Neighborhoood -----------------------------
        static private void GetNeighborhoodForEuclideanDistance(ref List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, Cl3DModel.Cl3DModelPointIterator p_CheckingPoint, Cl3DModel.Cl3DModelPointIterator p_CenterPoint, float p_fDistance)
        {
            List<Cl3DModel.Cl3DModelPointIterator> neighbors = p_CheckingPoint.GetListOfNeighbors();
            foreach (Cl3DModel.Cl3DModelPointIterator point in neighbors)
            {
                if (!point.AlreadyVisited)
                {
                    if (IsItInsideSphere(point.X, point.Y, point.Z, p_CenterPoint.X, p_CenterPoint.Y, p_CenterPoint.Z, p_fDistance))
                    {
                        point.AlreadyVisited = true;
                        p_pNeighborhood.Add(point);
                        GetNeighborhoodForEuclideanDistance(ref p_pNeighborhood, point, p_CenterPoint, p_fDistance);
                    }
                }
            }
        }
        static public void GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, Cl3DModel.Cl3DModelPointIterator p_BasicPoint, float p_fDistance)
        {
            p_BasicPoint.AlreadyVisited = true;
            p_pNeighborhood = new List<Cl3DModel.Cl3DModelPointIterator>();

            GetNeighborhoodForEuclideanDistance(ref p_pNeighborhood, p_BasicPoint, p_BasicPoint, p_fDistance);

            foreach (Cl3DModel.Cl3DModelPointIterator point in p_pNeighborhood)
                point.AlreadyVisited = false;
            p_BasicPoint.AlreadyVisited = false;
        }
        static public void GetNeighborhoodWithEuclideanDistanceCheckAllModel(out List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, Cl3DModel.Cl3DModelPointIterator p_BasicPoint, float p_fDistance)
        {
            p_pNeighborhood = new List<Cl3DModel.Cl3DModelPointIterator>();
            Cl3DModel.Cl3DModelPointIterator iter = p_BasicPoint.GetManagedModel().GetIterator();
            if (iter.IsValid())
            {
                do
                {
                    float disance = CalculateEuclideanDistance(iter, p_BasicPoint);
                    if (disance <= p_fDistance && iter.PointID != p_BasicPoint.PointID)
                    {
                        p_pNeighborhood.Add(iter.CopyIterator());
                    }

                } while (iter.MoveToNext());

            }
        }

        static public void GetNeighborhoodWithGeodesicDistanceSlower(out List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, Cl3DModel.Cl3DModelPointIterator p_BasicPoint, float p_fMaxDistance)
        {
            p_BasicPoint.AlreadyVisited = true;
            p_pNeighborhood = new List<Cl3DModel.Cl3DModelPointIterator>();

            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<float> Distances = new List<float>();

            List<Cl3DModel.Cl3DModelPointIterator> NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
            List<float> NewDistances = new List<float>();

            NewListToCheck.Add(p_BasicPoint);
            NewDistances.Add(0);
            bool bAddedToNewCheckList = false;
            do
            {
                ListToCheck = NewListToCheck;
                Distances = NewDistances;

                NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();
                NewDistances = new List<float>();

                bAddedToNewCheckList = false;
                for (int i = 0; i < ListToCheck.Count; i++)
                {
                    ListToCheck[i].AlreadyVisited = true;
                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = ListToCheck[i].GetListOfNeighbors();
                    foreach (Cl3DModel.Cl3DModelPointIterator point in Neighbors)
                    {
                        float CurrentDistance = ListToCheck[i] - point;

                        if (point.AlreadyVisited)// if point was already visited, check if we dont have sometimes shorter path to it
                        {
                            for (int innerI = 0; innerI < NewListToCheck.Count; innerI++)
                            {
                                if (NewListToCheck[innerI].PointID == point.PointID)
                                {
                                    if (NewDistances[innerI] > Distances[i] + CurrentDistance)
                                        NewDistances[innerI] = Distances[i] + CurrentDistance;
                                    break;
                                }
                            }
                        }
                        else if (Distances[i] + CurrentDistance < p_fMaxDistance)
                        {
                            point.AlreadyVisited = true;
                            p_pNeighborhood.Add(point);
                            NewListToCheck.Add(point);
                            NewDistances.Add(Distances[i] + CurrentDistance);
                            bAddedToNewCheckList = true;
                        }
                    }
                }
            } while (bAddedToNewCheckList);


            foreach (Cl3DModel.Cl3DModelPointIterator point in p_pNeighborhood)
                point.AlreadyVisited = false;
            p_BasicPoint.AlreadyVisited = false;

        }

        static public void GetNeighborhoodWithGeodesicDistance(out List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, Cl3DModel.Cl3DModelPointIterator p_BasicPoint, float p_fMaxDistance)
        {
            p_pNeighborhood = new List<Cl3DModel.Cl3DModelPointIterator>();

            Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>> ListToCheck = null;
            Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>> NewListToCheck = new Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>>();

            NewListToCheck.Add(p_BasicPoint.PointID, new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>(p_BasicPoint, 0.0f));
            bool bAddedToNewCheckList = false;
            do
            {
                ListToCheck = NewListToCheck;

                NewListToCheck = new Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>>();

                bAddedToNewCheckList = false;
                foreach (KeyValuePair<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>> PointToCheckPair in ListToCheck)
                {
                    Cl3DModel.Cl3DModelPointIterator PointToCheck = PointToCheckPair.Value.Key;
                    float PointToCheckDistance = PointToCheckPair.Value.Value;

                    PointToCheck.AlreadyVisited = true;

                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = PointToCheck.GetListOfNeighbors();
                    foreach (Cl3DModel.Cl3DModelPointIterator NeighboorPoint in Neighbors)
                    {
                        float CurrentDistance = PointToCheck - NeighboorPoint;

                        if (NeighboorPoint.AlreadyVisited)// if point was already visited, check if we dont have sometimes shorter path to it
                        {
                            if (NewListToCheck.ContainsKey(NeighboorPoint.PointID))
                            {
                                KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float> OldVal = NewListToCheck[NeighboorPoint.PointID];
                                if (OldVal.Value > PointToCheckDistance + CurrentDistance)
                                {
                                    float newVal = PointToCheckDistance + CurrentDistance;
                                    NewListToCheck[NeighboorPoint.PointID] = new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>(OldVal.Key, newVal);
                                }
                            }
                        }
                        else if (PointToCheckDistance + CurrentDistance < p_fMaxDistance)
                        {
                            NeighboorPoint.AlreadyVisited = true;
                            p_pNeighborhood.Add(NeighboorPoint);
                            NewListToCheck.Add(NeighboorPoint.PointID, new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>(NeighboorPoint, PointToCheckDistance + CurrentDistance));
                            bAddedToNewCheckList = true;
                        }
                    }
                }
            } while (bAddedToNewCheckList);


            foreach (Cl3DModel.Cl3DModelPointIterator point in p_pNeighborhood)
                point.AlreadyVisited = false;
            p_BasicPoint.AlreadyVisited = false;

        }
        static public void GetNeighborhoodWithGeodesicDistanceWithFlagsCheck(out List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, Cl3DModel.Cl3DModelPointIterator p_BasicPoint, float p_fMaxDistance)
        {
            p_pNeighborhood = new List<Cl3DModel.Cl3DModelPointIterator>();

            Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>> ListToCheck = null;
            Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>> NewListToCheck = new Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>>();

            NewListToCheck.Add(p_BasicPoint.PointID, new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>(p_BasicPoint, 0.0f));
            bool bAddedToNewCheckList = false;
            do
            {
                ListToCheck = NewListToCheck;

                NewListToCheck = new Dictionary<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>>();

                bAddedToNewCheckList = false;
                foreach (KeyValuePair<uint, KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>> PointToCheckPair in ListToCheck)
                {
                    Cl3DModel.Cl3DModelPointIterator PointToCheck = PointToCheckPair.Value.Key;
                    float PointToCheckDistance = PointToCheckPair.Value.Value;

                    PointToCheck.AlreadyVisited = true;

                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = PointToCheck.GetListOfNeighbors();
                    foreach (Cl3DModel.Cl3DModelPointIterator NeighboorPoint in Neighbors)
                    {
                        float CurrentDistance = 0;
                        if (!PointToCheck.GetFlag(0) || !NeighboorPoint.GetFlag(0))
                            CurrentDistance = PointToCheck - NeighboorPoint;

                        if (NeighboorPoint.AlreadyVisited)// if point was already visited, check if we dont have sometimes shorter path to it
                        {
                            if (NewListToCheck.ContainsKey(NeighboorPoint.PointID))
                            {
                                KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float> OldVal = NewListToCheck[NeighboorPoint.PointID];
                                if (OldVal.Value > PointToCheckDistance + CurrentDistance)
                                {
                                    float newVal = PointToCheckDistance + CurrentDistance;
                                    NewListToCheck[NeighboorPoint.PointID] = new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>(OldVal.Key, newVal);
                                }
                            }
                        }
                        else if (PointToCheckDistance + CurrentDistance < p_fMaxDistance)
                        {
                            NeighboorPoint.AlreadyVisited = true;
                            p_pNeighborhood.Add(NeighboorPoint);
                            NewListToCheck.Add(NeighboorPoint.PointID, new KeyValuePair<Cl3DModel.Cl3DModelPointIterator, float>(NeighboorPoint, PointToCheckDistance + CurrentDistance));
                            bAddedToNewCheckList = true;
                        }
                    }
                }
            } while (bAddedToNewCheckList);


            foreach (Cl3DModel.Cl3DModelPointIterator point in p_pNeighborhood)
                point.AlreadyVisited = false;
            p_BasicPoint.AlreadyVisited = false;

        }
        //----------------------------------------------------------------------------
        static public void CalculateGeodesicDistanceFromSourcePointToAllPoints(Cl3DModel.Cl3DModelPointIterator p_BasicPoint, string p_SepcificPointNameToWhichDistanceWillBeCalculated)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_BasicPoint.GetManagedModel().GetIterator();
            do
            {
                if (iter.IsSpecificValueCalculated(p_SepcificPointNameToWhichDistanceWillBeCalculated))
                    iter.RemoveSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated);
            } while (iter.MoveToNext());

            p_BasicPoint.AlreadyVisited = true;

            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = null;
            List<Cl3DModel.Cl3DModelPointIterator> NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

            NewListToCheck.Add(p_BasicPoint);
            p_BasicPoint.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, 0.0f);

            do
            {
                ListToCheck = NewListToCheck;
                NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

                for (int i = 0; i < ListToCheck.Count; i++)
                {
                    Cl3DModel.Cl3DModelPointIterator currentPoint = ListToCheck[i];

                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = currentPoint.GetListOfNeighbors();
                    foreach (Cl3DModel.Cl3DModelPointIterator point in Neighbors)
                    {
                        float CurrentDistance = currentPoint - point;

                        double OldDistance = 0;
                        if (!currentPoint.GetSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, out OldDistance))
                            throw new Exception("Cannot get specific value: " + p_SepcificPointNameToWhichDistanceWillBeCalculated.ToString());

                        if (point.IsSpecificValueCalculated(p_SepcificPointNameToWhichDistanceWillBeCalculated))// if point was already visited, check if we dont have sometimes shorter path to it
                        {
                            double SavedPointDistance = 0;
                            if (!point.GetSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, out SavedPointDistance))
                                throw new Exception("Cannot get specific value: " + p_SepcificPointNameToWhichDistanceWillBeCalculated.ToString());

                            if (SavedPointDistance > OldDistance + CurrentDistance) // check if we maybe have shorter path to this point
                            {
                                point.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, OldDistance + CurrentDistance);
                                point.AddSpecificValue("Path", currentPoint.PointID);
                                NewListToCheck.Add(point);
                            }
                        }
                        else
                        {
                            //can be corrected because we have right now possibility to save distance in the point not different list
                            point.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, OldDistance + CurrentDistance);
                            point.AddSpecificValue("Path", currentPoint.PointID);
                            NewListToCheck.Add(point);
                        }
                    }
                }
            } while (NewListToCheck.Count != 0);
            p_BasicPoint.GetManagedModel().ResetVisitedPoints();
        }

        static public void CalculateGeodesicDistanceFromSourcePointToAllPointsWithMovement(Cl3DModel.Cl3DModelPointIterator p_BasicPoint, string p_SepcificPointNameToWhichDistanceWillBeCalculated, Dictionary<uint, uint> movement)
        {
            List<Cl3DModel.Cl3DModelPointIterator> ListToCheck = null;
            List<Cl3DModel.Cl3DModelPointIterator> NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

            NewListToCheck.Add(p_BasicPoint);
            p_BasicPoint.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, 0.0f);

            do
            {
                ListToCheck = NewListToCheck;
                NewListToCheck = new List<Cl3DModel.Cl3DModelPointIterator>();

                for (int i = 0; i < ListToCheck.Count; i++)
                {
                    Cl3DModel.Cl3DModelPointIterator currentPoint = ListToCheck[i];
                    if (movement != null)
                    {
                        uint testID;
                        if (movement.TryGetValue(currentPoint.PointID, out testID))
                        {
                            double value = currentPoint.GetSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated);
                            uint OldId = currentPoint.PointID;
                            if (!currentPoint.MoveToPoint(testID))
                                throw new Exception("Cannot move a point in to the point with no: " + testID.ToString());

                            currentPoint.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, value);
                            currentPoint.AddSpecificValue("Path", OldId);
                        }
                    }
                    
                    List<Cl3DModel.Cl3DModelPointIterator> Neighbors = currentPoint.GetListOfNeighbors();
                    foreach (Cl3DModel.Cl3DModelPointIterator point in Neighbors)
                    {
                        float CurrentDistance = currentPoint - point;

                        double OldDistance = 0;
                        if (!currentPoint.GetSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, out OldDistance))
                            throw new Exception("Cannot get specific value: " + p_SepcificPointNameToWhichDistanceWillBeCalculated.ToString());

                        double SavedPointDistance = 0;
                        if (point.GetSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, out SavedPointDistance))
                        {
                            if (SavedPointDistance > OldDistance + CurrentDistance) // check if we maybe have shorter path to this point
                            {
                                point.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, OldDistance + CurrentDistance);
                                point.AddSpecificValue("Path", currentPoint.PointID);
                                NewListToCheck.Add(point);
                            }
                        }
                        else
                        {
                            //can be corrected because we have right now possibility to save distance in the point not different list
                            point.AddSpecificValue(p_SepcificPointNameToWhichDistanceWillBeCalculated, OldDistance + CurrentDistance);
                            point.AddSpecificValue("Path", currentPoint.PointID);
                            NewListToCheck.Add(point);
                        }
                    }
                }
            } while (NewListToCheck.Count != 0);
            p_BasicPoint.GetManagedModel().ResetVisitedPoints();
        }

        static public void CalculateNormalVectorInPoint(double p_x, double p_y, double p_z, List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, out Vector p_NormalVector)
        {
            p_NormalVector = new Vector(new double[] { 0, 0, 0 });
            for (int i = 0; i < p_pNeighborhood.Count; i++)
            {
                Vector tmpVector1 = new Vector(new double[] { (p_pNeighborhood[i].X - p_x), (p_pNeighborhood[i].Y - p_y), (p_pNeighborhood[i].Z - p_z) });
                Vector tmpVector2;
                if (i == p_pNeighborhood.Count - 1)
                    tmpVector2 = new Vector(new double[] { (p_pNeighborhood[0].X - p_x), (p_pNeighborhood[0].Y - p_y), (p_pNeighborhood[0].Z - p_z) });
                else
                    tmpVector2 = new Vector(new double[] { (p_pNeighborhood[i + 1].X - p_x), (p_pNeighborhood[i + 1].Y - p_y), (p_pNeighborhood[i + 1].Z - p_z) });

                p_NormalVector[0] += (tmpVector1[1] * tmpVector2[2] - tmpVector1[2] * tmpVector2[1]);
                p_NormalVector[1] += (tmpVector1[2] * tmpVector2[0] - tmpVector1[0] * tmpVector2[2]);
                p_NormalVector[2] += (tmpVector1[0] * tmpVector2[1] - tmpVector1[1] * tmpVector2[0]);

                p_NormalVector[0] /= 2.0f;
                p_NormalVector[1] /= 2.0f;
                p_NormalVector[2] /= 2.0f;
            }
        }

        static public void CalculateNormalVectorInPointUsingPCA(double p_x, double p_y, double p_z, List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, out Vector p_NormalVector)
        {
            p_NormalVector = new Vector(new double[] { 0, 0, 0 });

            float MeanX = 0;
            float MeanY = 0;
            float MeanZ = 0;
            Matrix xy = new Matrix(3, p_pNeighborhood.Count);
            for (int i = 0; i < p_pNeighborhood.Count; i++)
            {
                xy[0, i] = p_pNeighborhood[i].X;
                xy[1, i] = p_pNeighborhood[i].Y;
                xy[2, i] = p_pNeighborhood[i].Z;
                
                MeanX += p_pNeighborhood[i].X;
                MeanY += p_pNeighborhood[i].Y;
                MeanZ += p_pNeighborhood[i].Z;
                
            }
            MeanX /= p_pNeighborhood.Count;
            MeanY /= p_pNeighborhood.Count;
            MeanZ /= p_pNeighborhood.Count;

            for (int i = 0; i < p_pNeighborhood.Count; i++)
            {
                xy[0, i] -= MeanX;
                xy[1, i] -= MeanY;
                xy[2, i] -= MeanZ;
            }


            Matrix Transpose = xy.Clone();
            Transpose.Transpose();
            Matrix Corelation = xy * Transpose;

            SingularValueDecomposition SVD = new SingularValueDecomposition(Corelation);

            p_NormalVector[0] = SVD.RightSingularVectors[0, 2];
            p_NormalVector[1] = SVD.RightSingularVectors[1, 2];
            p_NormalVector[2] = SVD.RightSingularVectors[2, 2];
            
        }

        static public bool IsItInsideSphere(float p_x, float p_y, float p_z, float p_fSphereCenterX, float p_fSphereCenterY, float p_fSphereCenterZ, float p_fSphereRadious)
        {
            float distance = (float)Math.Sqrt(
                                        Math.Pow((p_x - p_fSphereCenterX), 2) +
                                        Math.Pow((p_y - p_fSphereCenterY), 2) +
                                        Math.Pow((p_z - p_fSphereCenterZ), 2)
                                        );

            if (distance <= p_fSphereRadious)
                return true;
            else
                return false;
        }

        static public void RotateXDirection(float p_x, float p_y, float p_z, float p_angle, out float p_XRotated, out float p_YRotated, out float p_ZRotated)
        {
            p_XRotated = p_x;
            p_YRotated = (float)(p_y * Math.Cos(p_angle) - p_z * Math.Sin(p_angle));
            p_ZRotated = (float)(p_y * Math.Sin(p_angle) + p_z * Math.Cos(p_angle));
        }

        static public void RotateYDirection(float p_x, float p_y, float p_z, float p_angle, out float p_XRotated, out float p_YRotated, out float p_ZRotated)
        {
            p_XRotated = (float)(p_z * Math.Sin(p_angle) + p_x * Math.Cos(p_angle));
            p_YRotated = p_y;
            p_ZRotated = (float)(p_z * Math.Cos(p_angle) - p_x * Math.Sin(p_angle));
        }

        static public void RotateZDirection(float p_x, float p_y, float p_z, float p_angle, out float p_XRotated, out float p_YRotated, out float p_ZRotated)
        {
            p_XRotated = (float)(p_x * Math.Cos(p_angle) - p_y * Math.Sin(p_angle));
            p_YRotated = (float)(p_x * Math.Sin(p_angle) + p_y * Math.Cos(p_angle));
            p_ZRotated = p_z;
        }

        static public float GetAngle2D(float x1, float y1, float x2, float y2)
        {
            float opp;
            float adj;
            float ang1;

            //calculate Vector differences
            opp = y2 - y1;
            adj = x2 - x1;

            if (x1 == x2 && y1 == y2) return (0);

            //trig function to calculate angle
            if (adj == 0) // to catch vertical co-ord to prevent division by 0
            {
                if (opp >= 0)
                {
                    return (0);
                }
                else
                {
                    return (180);
                }
            }
            else
            {
                ang1 = (float)((Math.Atan(opp / adj)) * 180 / Math.PI);
                //the angle calculated will range from +90 degrees to -90 degrees
                //so the angle needs to be adjusted if point x1 is less or greater then x2
                if (x1 >= x2)
                {
                    ang1 = 90 - ang1;
                }
                else
                {
                    ang1 = 270 - ang1;
                }
            }
            return (ang1);
        }
        /// <summary>
        /// For X, x doesn't exist for Y, y doesn't exist, for Z, z doesn't exist
        /// </summary>
        /// <param name="p_Vector1"></param>
        /// <param name="p_Vector2"></param>
        /// <param name="p_RotationAngleX"></param>
        /// <param name="p_RotationAngleY"></param>
        /// <param name="p_RotationAngleZ"></param>
        static public void CalculateAnglesBetweenVectors(Vector p_Vector1, Vector p_Vector2, out float p_RotationAngleX, out float p_RotationAngleY, out float p_RotationAngleZ)
        {
            if (p_Vector1.Length != 3 || p_Vector2.Length != 3)
                throw new Exception("Vector should contains X, Y, Z coordintates");

            double valX = (p_Vector1[1] * p_Vector2[1] + p_Vector1[2] * p_Vector2[2]) / (Math.Sqrt(Math.Pow(p_Vector1[1], 2) + Math.Pow(p_Vector1[2], 2)) * Math.Sqrt(Math.Pow(p_Vector2[1], 2) + Math.Pow(p_Vector2[2], 2)));
            double valY = (p_Vector1[0] * p_Vector2[0] + p_Vector1[2] * p_Vector2[2]) / (Math.Sqrt(Math.Pow(p_Vector1[0], 2) + Math.Pow(p_Vector1[2], 2)) * Math.Sqrt(Math.Pow(p_Vector2[0], 2) + Math.Pow(p_Vector2[2], 2)));
            double valZ = (p_Vector1[0] * p_Vector2[0] + p_Vector1[1] * p_Vector2[1]) / (Math.Sqrt(Math.Pow(p_Vector1[0], 2) + Math.Pow(p_Vector1[1], 2)) * Math.Sqrt(Math.Pow(p_Vector2[0], 2) + Math.Pow(p_Vector2[1], 2)));

            float signX = -1;
            float signY = -1;
            float signZ = -1;

            if (Math.Atan(valX) > 0)
                signX = 1;
            if (Math.Atan(valY) > 0)
                signY = 1;
            if (Math.Atan(valZ) > 0)
                signZ = 1;

            p_RotationAngleX = signX * (float)Math.Acos(valX);

            p_RotationAngleY = signY * (float)Math.Acos(valY);

            p_RotationAngleZ = signZ * (float)Math.Acos(valZ);
        }

        static public float Calculate3DAngleBetweenVectors(Vector p_Vector1, Vector p_Vector2)
        {
            double sumOfSquares = Math.Sqrt(p_Vector1[0] * p_Vector1[0] + p_Vector1[1] * p_Vector1[1] + p_Vector1[2] * p_Vector1[2]) * Math.Sqrt(p_Vector2[0] * p_Vector2[0] + p_Vector2[1] * p_Vector2[1] + p_Vector2[2] * p_Vector2[2]);
            double dotProduct = p_Vector1[0] * p_Vector2[0] + p_Vector1[1] * p_Vector2[1] + p_Vector1[2] * p_Vector2[2];
            double angle = Math.Acos(dotProduct / sumOfSquares);
            
           // throw new Exception("Function has hanged, test before use!");

            if (dotProduct < 0)
            {
                if (angle > 0)
                    angle += Math.PI;
              else
                    angle -= Math.PI;
            }

            return (float)angle;
        }

        static public Matrix CalculateRotationMatrix(Vector p_Vector1, Vector p_Vector2)
        {
            Matrix matrix = new Matrix(3, 3);
            Vector Vaxis = p_Vector1.CrossMultiply(p_Vector2);

            double Vangle = Math.Acos(p_Vector1 * p_Vector2);

            double rcos = Math.Cos(Vangle);
            double rsin = Math.Sin(Vangle);
            double u = Vaxis[0];
            double v = Vaxis[1];
            double w = Vaxis[2];

            matrix[0, 0] = rcos + u * u * (1 - rcos);
            matrix[1, 0] = w * rsin + v * u * (1 - rcos);
            matrix[2, 0] = -v * rsin + w * u * (1 - rcos);
            matrix[0, 1] = -w * rsin + u * v * (1 - rcos);
            matrix[1, 1] = rcos + v * v * (1 - rcos);
            matrix[2, 1] = u * rsin + w * v * (1 - rcos);
            matrix[0, 2] = v * rsin + u * w * (1 - rcos);
            matrix[1, 2] = -u * rsin + v * w * (1 - rcos);
            matrix[2, 2] = rcos + w * w * (1 - rcos);

            return matrix;
        }
        
        static public float Calculate2DAngleBetweenVectors(Vector p_Vector1, Vector p_Vector2)
        {
            double sumOfSquares = Math.Sqrt(p_Vector1[0] * p_Vector1[0] + p_Vector1[1] * p_Vector1[1]) * Math.Sqrt(p_Vector2[0] * p_Vector2[0] + p_Vector2[1] * p_Vector2[1]);
            double dotProduct = p_Vector1[0] * p_Vector2[0] + p_Vector1[1] * p_Vector2[1];
            double angle = Math.Acos(dotProduct / sumOfSquares);
            return (float)angle;
        }

        static public void CalculateAnglesToZAxis(Vector p_Vector, out float p_RotationAngleX, out float p_RotationAngleY, out float p_RotationAngleZ)
        {
            p_RotationAngleX = (float)Math.Atan(p_Vector[1] / p_Vector[2]);
            p_RotationAngleY = (float)-Math.Atan(p_Vector[0] / p_Vector[2]);
            p_RotationAngleZ = (float)-Math.Atan(p_Vector[1] / p_Vector[0]);
        }

        static public bool CountSurfaceCoefficients(List<ClTools.MainPoint3D> p_pNeighborhood, ref double p_A, ref double p_B, ref double p_C, ref double p_D, ref double p_E, ref double p_F)
        {
            if (p_pNeighborhood.Count < 3)
                return false;

            double x = 0;
            double y = 0;
            double z = 0;
            double xy = 0;
            double xz = 0;
            double yz = 0;
            double xyz = 0;
            double x2 = 0;
            double y2 = 0;
            double x2y = 0;
            double x2z = 0;
            double y2z = 0;
            double xy2 = 0;
            double x3 = 0;
            double y3 = 0;
            double x4 = 0;
            double y4 = 0;
            double x2y2 = 0;
            double xy3 = 0;
            double x3y = 0;
            double N = 0;

            for (int i = 0; i < p_pNeighborhood.Count; i++)
            {
                double iX = p_pNeighborhood[i].X;
                double iY = p_pNeighborhood[i].Y;
                double iZ = p_pNeighborhood[i].Z;

                x += iX;
                y += iY;
                z += iZ;
                xy += iX * iY;
                xz += iX * iZ;
                yz += iY * iZ;
                xyz += iX * iY * iZ;
                x2 += iX * iX;
                y2 += iY * iY;
                x2y += iX * iX * iY;
                x2z += iX * iX * iZ;
                y2z += iY * iY * iZ;
                //xy2 += iX * iY * iZ * iZ; //gives strange but good results
                xy2 += iX * iY * iY;
                x3 += iX * iX * iX;
                y3 += iY * iY * iY;
                x4 += iX * iX * iX * iX;
                y4 += iY * iY * iY * iY;
                x2y2 += iX * iX * iY * iY;
                xy3 += iX * iY * iY * iY;
                x3y += iX * iX * iX * iY;
                N++;
            }

            Iridium.Numerics.LinearAlgebra.Matrix MatrixLeft = new Iridium.Numerics.LinearAlgebra.Matrix(6, 6);
            Iridium.Numerics.LinearAlgebra.Matrix MatrixRight = new Iridium.Numerics.LinearAlgebra.Matrix(6, 1);

            MatrixLeft[0, 0] = N;
            MatrixLeft[0, 1] = x;
            MatrixLeft[0, 2] = y;
            MatrixLeft[0, 3] = x2;
            MatrixLeft[0, 4] = xy;
            MatrixLeft[0, 5] = y2;
            MatrixLeft[1, 0] = x;
            MatrixLeft[1, 1] = x2;
            MatrixLeft[1, 2] = xy;
            MatrixLeft[1, 3] = x3;
            MatrixLeft[1, 4] = x2y;
            MatrixLeft[1, 5] = xy2;
            MatrixLeft[2, 0] = y;
            MatrixLeft[2, 1] = xy;
            MatrixLeft[2, 2] = y2;
            MatrixLeft[2, 3] = x2y;
            MatrixLeft[2, 4] = xy2;
            MatrixLeft[2, 5] = y3;
            MatrixLeft[3, 0] = x2;
            MatrixLeft[3, 1] = x3;
            MatrixLeft[3, 2] = x2y;
            MatrixLeft[3, 3] = x4;
            MatrixLeft[3, 4] = x3y;
            MatrixLeft[3, 5] = x2y2;
            MatrixLeft[4, 0] = xy;
            MatrixLeft[4, 1] = x2y;
            MatrixLeft[4, 2] = xy2;
            MatrixLeft[4, 3] = x3y;
            MatrixLeft[4, 4] = x2y2;
            MatrixLeft[4, 5] = xy3;
            MatrixLeft[5, 0] = y2;
            MatrixLeft[5, 1] = xy2;
            MatrixLeft[5, 2] = y3;
            MatrixLeft[5, 3] = x2y2;
            MatrixLeft[5, 4] = xy3;
            MatrixLeft[5, 5] = y4;

            MatrixRight[0, 0] = z;
            MatrixRight[1, 0] = xz;
            MatrixRight[2, 0] = yz;
            MatrixRight[3, 0] = x2z;
            MatrixRight[4, 0] = xyz;
            MatrixRight[5, 0] = y2z;

            Iridium.Numerics.LinearAlgebra.Matrix MatrixOut = null;
            try
            {
                MatrixLeft = MatrixLeft.Inverse();
                MatrixOut = MatrixLeft * MatrixRight;
            }
            catch (Exception)
            {
                return false;
            }

            p_A = MatrixOut[0, 0];
            p_B = MatrixOut[1, 0];
            p_C = MatrixOut[2, 0];
            p_D = MatrixOut[3, 0];
            p_E = MatrixOut[4, 0];
            p_F = MatrixOut[5, 0];

            return true;
        }

        static public bool CountSurfaceCoefficients(List<Cl3DModel.Cl3DModelPointIterator> p_pNeighborhood, ref double p_A, ref double p_B, ref double p_C, ref double p_D, ref double p_E, ref double p_F)
        {
            if (p_pNeighborhood.Count < 3)
                return false;

            double x = 0;
            double y = 0;
            double z = 0;
            double xy = 0;
            double xz = 0;
            double yz = 0;
            double xyz = 0;
            double x2 = 0;
            double y2 = 0;
            double x2y = 0;
            double x2z = 0;
            double y2z = 0;
            double xy2 = 0;
            double x3 = 0;
            double y3 = 0;
            double x4 = 0;
            double y4 = 0;
            double x2y2 = 0;
            double xy3 = 0;
            double x3y = 0;
            double N = 0;

            for (int i = 0; i < p_pNeighborhood.Count; i++)
            {
                double iX = p_pNeighborhood[i].X;
                double iY = p_pNeighborhood[i].Y;
                double iZ = p_pNeighborhood[i].Z;

                x += iX;
                y += iY;
                z += iZ;
                xy += iX * iY;
                xz += iX * iZ;
                yz += iY * iZ;
                xyz += iX * iY * iZ;
                x2 += iX * iX;
                y2 += iY * iY;
                x2y += iX * iX * iY;
                x2z += iX * iX * iZ;
                y2z += iY * iY * iZ;
                //xy2 += iX * iY * iZ * iZ; //gives strange but good results
                xy2 += iX * iY * iY;
                x3 += iX * iX * iX;
                y3 += iY * iY * iY;
                x4 += iX * iX * iX * iX;
                y4 += iY * iY * iY * iY;
                x2y2 += iX * iX * iY * iY;
                xy3 += iX * iY * iY * iY;
                x3y += iX * iX * iX * iY;
                N++;
            }

            Iridium.Numerics.LinearAlgebra.Matrix MatrixLeft = new Iridium.Numerics.LinearAlgebra.Matrix(6, 6);
            Iridium.Numerics.LinearAlgebra.Matrix MatrixRight = new Iridium.Numerics.LinearAlgebra.Matrix(6, 1);

            MatrixLeft[0, 0] = N;
            MatrixLeft[0, 1] = x;
            MatrixLeft[0, 2] = y;
            MatrixLeft[0, 3] = x2;
            MatrixLeft[0, 4] = xy;
            MatrixLeft[0, 5] = y2;
            MatrixLeft[1, 0] = x;
            MatrixLeft[1, 1] = x2;
            MatrixLeft[1, 2] = xy;
            MatrixLeft[1, 3] = x3;
            MatrixLeft[1, 4] = x2y;
            MatrixLeft[1, 5] = xy2;
            MatrixLeft[2, 0] = y;
            MatrixLeft[2, 1] = xy;
            MatrixLeft[2, 2] = y2;
            MatrixLeft[2, 3] = x2y;
            MatrixLeft[2, 4] = xy2;
            MatrixLeft[2, 5] = y3;
            MatrixLeft[3, 0] = x2;
            MatrixLeft[3, 1] = x3;
            MatrixLeft[3, 2] = x2y;
            MatrixLeft[3, 3] = x4;
            MatrixLeft[3, 4] = x3y;
            MatrixLeft[3, 5] = x2y2;
            MatrixLeft[4, 0] = xy;
            MatrixLeft[4, 1] = x2y;
            MatrixLeft[4, 2] = xy2;
            MatrixLeft[4, 3] = x3y;
            MatrixLeft[4, 4] = x2y2;
            MatrixLeft[4, 5] = xy3;
            MatrixLeft[5, 0] = y2;
            MatrixLeft[5, 1] = xy2;
            MatrixLeft[5, 2] = y3;
            MatrixLeft[5, 3] = x2y2;
            MatrixLeft[5, 4] = xy3;
            MatrixLeft[5, 5] = y4;

            MatrixRight[0, 0] = z;
            MatrixRight[1, 0] = xz;
            MatrixRight[2, 0] = yz;
            MatrixRight[3, 0] = x2z;
            MatrixRight[4, 0] = xyz;
            MatrixRight[5, 0] = y2z;

            Iridium.Numerics.LinearAlgebra.Matrix MatrixOut = null;
            try
            {
                MatrixLeft = MatrixLeft.Inverse();
                MatrixOut = MatrixLeft * MatrixRight;
            }
            catch (Exception)
            {
                return false;
            }

            p_A = MatrixOut[0, 0];
            p_B = MatrixOut[1, 0];
            p_C = MatrixOut[2, 0];
            p_D = MatrixOut[3, 0];
            p_E = MatrixOut[4, 0];
            p_F = MatrixOut[5, 0];

            return true;
        }
        /// <summary>
        /// Procent between 0 and 1
        /// </summary>
        /// <param name="p_fProcent"> between 0 and 1</param>
        /// <param name="p_fPower">power value for different decompozition</param>
        /// <returns></returns>
        static public Color GetColorRGB(float p_fProcent, float p_fPower) // Procent from 0 to 1
        {
            if (p_fProcent >= 0 && p_fProcent <= 1)
            {
                p_fProcent = (float)Math.Pow((double)p_fProcent, (double)p_fPower);
                int R = 0;
                int G = 0;
                int B = 0;
                if (p_fProcent <= 1 / 5f)
                {
                    float tmp = p_fProcent;
                    tmp /= 1 / 5f;

                    R = (int)((1f * (1 - tmp) + 0f * tmp) * 255f);//spada
                    G = 0;
                    B = 255;

                }
                else if (p_fProcent > 1 / 5f && p_fProcent <= 2 / 5f)
                {
                    float tmp = p_fProcent;
                    tmp -= 1 / 5f;
                    tmp /= 1 / 5f;
                    R = 0;
                    G = (int)((0f * (1 - tmp) + 1f * tmp) * 255f);//rosnie
                    B = 255;
                }
                else if (p_fProcent > 2 / 5f && p_fProcent <= 3 / 5f)
                {
                    float tmp = p_fProcent;
                    tmp -= 2 / 5f;
                    tmp /= 1 / 5f;

                    R = 0;
                    G = 255;
                    B = (int)((1f * (1 - tmp) + 0f * tmp) * 255f);//spada
                }
                else if (p_fProcent > 3 / 5f && p_fProcent <= 4 / 5f)
                {
                    float tmp = p_fProcent;
                    tmp -= 3 / 5f;
                    tmp /= 1 / 5f;

                    R = (int)((0f * (1 - tmp) + 1f * tmp) * 255f); //rosnie
                    G = 255;
                    B = 0;
                }
                else
                {
                    float tmp = p_fProcent;
                    tmp -= 4 / 5f;
                    tmp /= 1 / 5f;

                    R = 255;
                    G = (int)((1f * (1 - tmp) + 0f * tmp) * 255f); //spada
                    B = 0;
                }

                System.Diagnostics.Debug.Assert((R <= 255 && R >= 0) && (G <= 255 && G >= 0) && (B <= 255 && B >= 0), "Wrong value RGB");

                return Color.FromArgb(R, G, B);
            }
            else if (p_fProcent < 0)
            {
                return Color.White;
            }
            else
            {
                return Color.Black;
            }
        }

        static public Color GetColorGray(float p_iProcent, float p_fPower) // Procent form 0 to 1
        {
            float rgb = (float)Math.Pow((double)p_iProcent, (double)p_fPower) * 255;
            if ((int)rgb > 255)
                rgb = 255;
            else if ((int)rgb < 0)
                rgb = 0;

            return Color.FromArgb((int)rgb, (int)rgb, (int)rgb);
        }

        static public float CalculateEuclideanDistance(Cl3DModel.Cl3DModelPointIterator p_Point1, Cl3DModel.Cl3DModelPointIterator p_Point2)
        {
            double result = Math.Sqrt(Math.Pow(p_Point1.X - p_Point2.X, 2) + Math.Pow(p_Point1.Y - p_Point2.Y, 2) + Math.Pow(p_Point1.Z - p_Point2.Z, 2));
            return (float)result;
        }

        static public void FindClosestPointInModel(Cl3DModel p_Model, float p_X, float p_Y, float p_Z, out Cl3DModel.Cl3DModelPointIterator p_ClosestPoint, out float p_Distance)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in model is not valid");

            p_ClosestPoint = iter.CopyIterator();
            p_Distance = (float)Math.Sqrt(Math.Pow(iter.X - p_X, 2) + Math.Pow(iter.Y - p_Y, 2) + Math.Pow(iter.Z - p_Z, 2));
            Cl3DModel.Cl3DModelPointIterator closest = iter.CopyIterator();
            do
            {
                float distance = (float)Math.Sqrt(Math.Pow(iter.X - p_X, 2) + Math.Pow(iter.Y - p_Y, 2) + Math.Pow(iter.Z - p_Z, 2));
                if (distance < p_Distance)
                {
                    p_Distance = distance;
                    p_ClosestPoint = iter.CopyIterator();
                }
            } while (iter.MoveToNext());
        }

        static public void FindClosestPointInNeighborhood(List<Cl3DModel.Cl3DModelPointIterator> p_Neighborhood, float p_X, float p_Y, float p_Z, out Cl3DModel.Cl3DModelPointIterator p_ClosestPoint, out float p_Distance)
        {
            p_Distance = float.MaxValue;
            p_ClosestPoint = null;

            foreach (Cl3DModel.Cl3DModelPointIterator pt in p_Neighborhood)
            {
                float distance = (float)Math.Sqrt(Math.Pow(pt.X - p_X, 2) + Math.Pow(pt.Y - p_Y, 2) + Math.Pow(pt.Z - p_Z, 2));
                if (distance < p_Distance)
                {
                    p_Distance = distance;
                    p_ClosestPoint = pt.CopyIterator();
                }
            }
        }

        static public void CalculateRotationAndTranslation(List<Cl3DModel.Cl3DModelPointIterator> p_ListOfPointsFrom1Model, List<Cl3DModel.Cl3DModelPointIterator> p_ListOfPointsFrom2Model, out Iridium.Numerics.LinearAlgebra.Matrix p_mRotationMatrix, out Iridium.Numerics.LinearAlgebra.Matrix p_mTranslationMatrix)
        {
            if (p_ListOfPointsFrom1Model.Count != p_ListOfPointsFrom2Model.Count)
                throw new Exception("Both models should have the sime number of points");

            Matrix Model1 = new Matrix(3, p_ListOfPointsFrom1Model.Count);
            Matrix Model2 = new Matrix(3, p_ListOfPointsFrom2Model.Count);

            for (int i = 0; i < p_ListOfPointsFrom1Model.Count; i++)
            {
                Model1[0, i] = p_ListOfPointsFrom1Model[i].X;
                Model1[1, i] = p_ListOfPointsFrom1Model[i].Y;
                Model1[2, i] = p_ListOfPointsFrom1Model[i].Z;

                Model2[0, i] = p_ListOfPointsFrom2Model[i].X;
                Model2[1, i] = p_ListOfPointsFrom2Model[i].Y;
                Model2[2, i] = p_ListOfPointsFrom2Model[i].Z;
            }

            CalculateRotationAndTranslation(Model1, Model2, out p_mRotationMatrix, out p_mTranslationMatrix);

        }
        /// <summary>
        /// each colomn is one vertex - CORRECTED 
        /// </summary>
        /// <param name="PointsModel1"></param>
        /// <param name="PointsModel2"></param>
        /// <param name="p_mRotationMatrix"></param>
        /// <param name="p_mTranslationMatrix"></param>
        static public void CalculateRotationAndTranslation(Matrix PointsModel1, Matrix PointsModel2, out Iridium.Numerics.LinearAlgebra.Matrix p_mRotationMatrix, out Iridium.Numerics.LinearAlgebra.Matrix p_mTranslationMatrix)
        {

            if (PointsModel1.ColumnCount != PointsModel2.ColumnCount || PointsModel1.RowCount != PointsModel2.RowCount)
                throw new Exception("Two Matrixes do not have the same count");

            Matrix MeanValModel1 = new Matrix(PointsModel1.RowCount, 1);
            Matrix MeanValModel2 = new Matrix(PointsModel1.RowCount, 1);

            for (int j = 0; j < PointsModel1.RowCount; j++)
            {
                for (int i = 0; i < PointsModel1.ColumnCount; i++)
                {
                    MeanValModel1[j, 0] += PointsModel1[j, i];
                    MeanValModel2[j, 0] += PointsModel2[j, i];
                }
            }
            MeanValModel1 *= (1.0f / PointsModel1.ColumnCount);
            MeanValModel2 *= (1.0f / PointsModel1.ColumnCount);

            Matrix CenteredModel1 = new Matrix(PointsModel1.RowCount, PointsModel1.ColumnCount);
            Matrix CenteredModel2 = new Matrix(PointsModel1.RowCount, PointsModel1.ColumnCount);

            for (int i = 0; i < PointsModel1.RowCount; i++)
            {
                for (int j = 0; j < PointsModel1.ColumnCount; j++)
                {
                    CenteredModel1[i, j] = PointsModel1[i, j] - MeanValModel1[i, 0];
                    CenteredModel2[i, j] = PointsModel2[i, j] - MeanValModel2[i, 0];
                }
            }

            CenteredModel2.Transpose();

            Iridium.Numerics.LinearAlgebra.Matrix Covariance = CenteredModel1 * CenteredModel2;

            Iridium.Numerics.LinearAlgebra.SingularValueDecomposition SVD = new Iridium.Numerics.LinearAlgebra.SingularValueDecomposition(Covariance);
            Iridium.Numerics.LinearAlgebra.Matrix U = SVD.LeftSingularVectors;
            Iridium.Numerics.LinearAlgebra.Matrix V = SVD.RightSingularVectors;

            Iridium.Numerics.LinearAlgebra.Matrix s = new Iridium.Numerics.LinearAlgebra.Matrix(PointsModel1.RowCount, 1.0);

            if (Covariance.Rank() < 2)
                throw new Exception("Cannot allign generic model (cov rank is less than 2)");

            if (Covariance.Rank() == 2) // m-1 where m is dimension space (3D)
            {
                double detU = Math.Round(U.Determinant());
                double detV = Math.Round(V.Determinant());
                double detC = Covariance.Determinant();
                if ((int)detU * (int)detV == 1)
                    s[PointsModel1.RowCount - 1, PointsModel1.RowCount - 1] = 1;
                else if ((int)detU * (int)detV == -1)
                    s[PointsModel1.RowCount - 1, PointsModel1.RowCount - 1] = -1;
                else
                    throw new Exception("Determinant of U and V are not in conditions");
            }
            else
            {
                if (Covariance.Determinant() < 0)
                    s[PointsModel1.RowCount - 1, PointsModel1.RowCount - 1] = -1;
            }

            V.Transpose();
            Iridium.Numerics.LinearAlgebra.Matrix Rotation = U * s * V;

            Iridium.Numerics.LinearAlgebra.Matrix Translation = MeanValModel1 - Rotation * MeanValModel2;

            p_mRotationMatrix = Rotation;
            p_mTranslationMatrix = Translation;
        }

        static public bool CalculateDescriptorInSpecificValue(Cl3DModel.Cl3DModelPointIterator p_Point, ref string descriptor)
        {
            descriptor +=   p_Point.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                            p_Point.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                            p_Point.Z.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                            p_Point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Mean_25).ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                            p_Point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Gaussian_25).ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                            p_Point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.ShapeIndex_25).ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                            p_Point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.CurvednessIndex_25).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";

            bool ret = true;
            List<Cl3DModel.Cl3DModelPointIterator> neighborhood = null;
            for (int i = 5; i <= 10; i += 5)
            {
                ClTools.GetNeighborhoodWithGeodesicDistance(out neighborhood, p_Point, i);
                double Mean = 0;
                double Gaussian = 0;
                double SI = 0;
                double Curvadness = 0;
                int count = 0;
                double[] Histogram = new double[10];
                foreach (Cl3DModel.Cl3DModelPointIterator point in neighborhood)
                {
                    if (!point.GetFlag(0))
                    {
                        try
                        {
                            double SITemp = point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.ShapeIndex_25);
                            Histogram[(int)(SITemp * 9)]++;
                            Mean += point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Mean_25);
                            Gaussian += point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Gaussian_25);
                            SI += SITemp;
                            Curvadness += point.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.CurvednessIndex_25);
                            count++;
                        }
                        catch (Exception)
                        { }
                        point.SetFlag(0, true);
                       // point.Color = ClTools.GetColorRGB(((float)i) / 25, 1f);
                    }
                }
                if (count == 0)
                {
                    ret = false;
                }
                else
                {
                    descriptor += (Mean /= count).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
                    descriptor += (Gaussian /= count).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
                    descriptor += (SI /= count).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
                    descriptor += (Curvadness /= count).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
                    for (int j = 0; j < 10; j++)
                    {
                        descriptor += (Histogram[j] /= count).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
                    }

                }
            }
            foreach (Cl3DModel.Cl3DModelPointIterator point in neighborhood)
            {
                point.SetFlag(0, false);
            }

            return ret;
        }
    }
}
