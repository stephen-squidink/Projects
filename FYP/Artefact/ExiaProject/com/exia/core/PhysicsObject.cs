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
using ExiaProject.com.exia.objects;

namespace ExiaProject.com.exia.core
{
    class PhysicsObject
    {
        public static bool ON = true;
        public static bool OFF = true;
        
        public float _delta;

        public Specification _spec;

        public Vector3 _position;
        public Quaternion _rotation;

        public float _angle;

        public Vector3 _flatF;
        public Vector3 _flatR;

        public float _steeringAngle;
        public float _steeringRadius;
        public float _steeringPosition;

        public Vector3 _force;            //N
        public Vector3 _velocity;         //ms
        public Vector3 _acceleration;     //ms^2
        public Vector3 _speed;

        public float _angularVelocity;
        public float _angularAcceleration;

        public float _we;

        public float _throttlePosition;  // between 0 - 1
        public float _brakingPosition;

        public float _brakingForce;
        public float _tractionForce;
        public float _dragForce;
        public float _cDrag;
        public float _rollingForce;

        public float _Wf;
        public float _Wr;

        public float _currentTorque;

        public bool _engineState;
        public String _currentGear;
        public String _previousGear;
        public float _currentRPM;
    }
}
