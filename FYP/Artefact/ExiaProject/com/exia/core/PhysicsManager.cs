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
using ExiaProject.com.exia.commands;

namespace ExiaProject.com.exia.core
{
    class PhysicsManager
    {
        AccelerationCommand accelerationCommand = new AccelerationCommand();
        VelocityCommand velocityCommand = new VelocityCommand();
        DisplacementCommand displacementCommand = new DisplacementCommand();
        AngularOrintationCommand angularOrientationCommand = new AngularOrintationCommand();
        RPMCommand rpmCommand = new RPMCommand();
        TorqueCommand torqueCommand = new TorqueCommand();
        WeightTransferCommand weightTransferCommand = new WeightTransferCommand();
        BrakeForceCommand brakingForceCommand = new BrakeForceCommand();
        TractionForceCommand engineForceCommand = new TractionForceCommand();
        DragForceCommand dragForceCommand = new DragForceCommand();
        RollingResistanceCommand rollingResistanceCommand = new RollingResistanceCommand();
        ForceCommand forceCommand = new ForceCommand();
        AngularVelocityCommand angularVelocityCommand = new AngularVelocityCommand();
        AngularAccelerationCommand angularAccelerationCommand = new AngularAccelerationCommand();
        ForceLateralCommand forceLateralCommand = new ForceLateralCommand();

        public PhysicsManager()
        {

        }

        public void update(GameTime gameTime, PhysicsObject pObj)
        {
            torqueCommand.execute(pObj);
            rpmCommand.execute(pObj);
            weightTransferCommand.execute(pObj);
            engineForceCommand.execute(pObj);
            dragForceCommand.execute(pObj);
            rollingResistanceCommand.execute(pObj);
            brakingForceCommand.execute(pObj);
            forceCommand.execute(pObj);
            angularVelocityCommand.execute(pObj);
            angularAccelerationCommand.execute(pObj);
            forceLateralCommand.execute(pObj);
            angularOrientationCommand.execute(pObj);
            accelerationCommand.execute(pObj);
            velocityCommand.execute(pObj);
            displacementCommand.execute(pObj);
        }
    }
}
