using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.exia.commands
{
    class WeightTransferCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float Wf = 0;
            float Wr = 0;

            float b = (pObj._spec.WeightDistributionRear / 100);
            float c = (pObj._spec.WeightDistributionFront / 100);

            Wf = (0.5f * b * pObj._spec.CurbWeight * PhysicsConstants.g) - (0.5f * (1.0f / (UnitConverter.MMtoM(pObj._spec.WheelBase))) * pObj._spec.CurbWeight * (pObj._acceleration.Z));
            Wr = (0.5f * c * pObj._spec.CurbWeight * PhysicsConstants.g) + (0.5f * (1.0f / (UnitConverter.MMtoM(pObj._spec.WheelBase))) * pObj._spec.CurbWeight *(pObj._acceleration.Z));

            pObj._Wf = Wf;
            pObj._Wr = Wr;
        }
    }
}
