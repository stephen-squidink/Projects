using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ExiaProject.com.exia.util
{
    class UnitConverter
    {
        public static float RPMtoRADs(float RPM)
        {
            float RADs = (RPM * MathHelper.TwoPi)/60;

            return RADs;
        }

        public static float HPtoW(float HP)
        {
            float W = HP * 745.7f;

            return W;
        }

        public static int MStoMPH(float MS)
        {
            int MPH = (int)(MS * 2.24f);

            return MPH;
        }

        public static float CMtoMM(float CM)
        {
            float MM = (float)(CM / 100);

            return MM;
        }

        public static float MMtoM(float MM)
        {
            float M = MM / 1000;

            return M;
        }
    }
}
