using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExiaProject.com.exia.objects
{

    class Specification
    {
        //Dimensions
        public float OverallLength;
        public float OverallWidth;
        public float OverallHeight;
        public float WheelBase;
        public float TrackFront;
        public float TrackRear;

        //Dynamics
        public float CurbWeight;
        public float DragCoefficient;
        public float FrontalArea;
        public float WeightDistributionFront;
        public float WeightDistributionRear;

        //Engine
        public float Displacement;
        public float MaxPower;
        public float MaxPowerAtRPM;
        public float MaxTorque;
        public float MaxTorqueAtRPM;
        public float RedLine;

        //Gears
        public Dictionary<String, float> Gears = new Dictionary<string,float>();

        public float FinalDrive;

        public float TransmissionEfficiency;

        //Performance
        public float TopSpeed;
        public float LateralAccel;

        //Wheels
        public float WheelRadius;


        public void AddGear(String name, float ratio)
        {
            Gears.Add(name, ratio);
        }
    }
}
