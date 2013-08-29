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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.exia.commands
{
    class TorqueCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float p1 = UnitConverter.HPtoW(pObj._spec.MaxPower) / UnitConverter.RPMtoRADs(pObj._spec.MaxPowerAtRPM);
            float p2 = UnitConverter.HPtoW(pObj._spec.MaxPower) / (float)Math.Pow((double)UnitConverter.RPMtoRADs(pObj._spec.MaxPowerAtRPM), 2);
            float p3 = -UnitConverter.HPtoW(pObj._spec.MaxPower) / (float)Math.Pow((double)UnitConverter.RPMtoRADs(pObj._spec.MaxPowerAtRPM), 3);

            float we = 0;

            we = ((pObj._spec.Gears[pObj._currentGear] * pObj._spec.FinalDrive) / (pObj._spec.WheelRadius / 100)) * pObj._velocity.Z;

            float tE = p1 + (p2 * we) + (p3 * (float)Math.Pow((double)we, 2));

            pObj._we = we;
            pObj._currentTorque = tE * pObj._throttlePosition;
        }
    }
}
