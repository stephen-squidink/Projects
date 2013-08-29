using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ExiaProject.com.exia.commands
{
    class RPMCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float rpm = 0;

            rpm = ((pObj._velocity.Z/ ( UnitConverter.CMtoMM(pObj._spec.WheelRadius))) * pObj._spec.Gears[pObj._currentGear] * pObj._spec.FinalDrive * 60 )/ MathHelper.TwoPi;

            if (rpm < 1000)
                rpm = 1000;

            pObj._currentRPM = rpm;
        }
    }
}
