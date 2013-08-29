using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExiaProject.com.exia.core;

namespace ExiaProject.com.exia.commands
{
    class TractionForceCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float tractionForce = 0;

            tractionForce = (pObj._currentTorque * pObj._spec.Gears[pObj._currentGear] * pObj._spec.FinalDrive * pObj._spec.TransmissionEfficiency) / (pObj._spec.WheelRadius / 100);


            if (tractionForce > pObj._Wr)
            {
               // Console.WriteLine("SLIP");
            }

            pObj._tractionForce = tractionForce;
        }
    }
}
