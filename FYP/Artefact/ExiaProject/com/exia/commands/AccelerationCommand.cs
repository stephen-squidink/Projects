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
    class AccelerationCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            pObj._acceleration.Z = pObj._force.Z / pObj._spec.CurbWeight;
            pObj._acceleration.X = pObj._force.X / pObj._spec.CurbWeight;
        }
    }
}
