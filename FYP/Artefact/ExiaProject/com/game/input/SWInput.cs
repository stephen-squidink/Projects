using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ExiaProject.com.game.input
{
    class SWInput
    {
        private Device _controller;
        private SWInputStates inputStates;

        public SWInput()
        {
            inputStates = new SWInputStates();
        }

        public void initialize(IntPtr handle)
        {
            DeviceList controllers = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);

            foreach (DeviceInstance gameCtrlDev in controllers)
            {
                if (gameCtrlDev.ProductName == "Logitech G25 Racing Wheel USB")
                {
                    Console.WriteLine(gameCtrlDev.ProductName);

                    _controller = new Device(gameCtrlDev.InstanceGuid);

                    break;
                }
            }

            if (_controller != null)
            {
                _controller.SetCooperativeLevel(handle, CooperativeLevelFlags.Exclusive | CooperativeLevelFlags.Background);

                _controller.Properties.AxisModeAbsolute = true;

                foreach (DeviceObjectInstance doi in _controller.Objects)
                {
                    Console.WriteLine(doi.Name);

                    int rangeMin = 0, rangeMax = 0;

                    switch (doi.Name)
                    {
                        case "Accelerator":
                            rangeMin = -1000;
                            rangeMax = 0;
                            break;
                        case "Brake":
                            rangeMin = -1000;
                            rangeMax = 0;
                            break;
                        case "Clutch":
                            rangeMin = 0;
                            rangeMax = 1;
                            break;
                        default:
                            rangeMin = -1000;
                            rangeMax = 1000;
                            break;
                    }

                    if ((doi.ObjectId & (int)DeviceObjectTypeFlags.Axis) != 0)
                    {
                        _controller.Properties.SetRange(ParameterHow.ById, doi.ObjectId, new InputRange(rangeMin, rangeMax));
                        _controller.Properties.SetDeadZone(ParameterHow.ById, doi.ObjectId, 200);
                        _controller.Properties.SetSaturation(ParameterHow.ById, doi.ObjectId, 10000);
                    }
                }
                
                _controller.Acquire();
            }
        }

        public SWInputStates GetState()
        {
            JoystickState state = _controller.CurrentJoystickState;

            inputStates.SetGear("First" , state.GetButtons()[8]);
            inputStates.SetGear("Second", state.GetButtons()[9]);
            inputStates.SetGear("Third", state.GetButtons()[10]);
            inputStates.SetGear("Fourth", state.GetButtons()[11]);
            inputStates.SetGear("Fifth", state.GetButtons()[12]);
            inputStates.SetGear("Sixth", state.GetButtons()[13]);
            inputStates.SetGear("Reverse" , state.GetButtons()[14]);

            inputStates.Accelerator = state.Y;
            inputStates.Brake = state.Rz;
            inputStates.Steering = state.X;
            inputStates.Clutch = state.GetSlider()[1];

            return inputStates;

        }

    }
}
