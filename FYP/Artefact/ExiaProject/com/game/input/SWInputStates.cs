using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExiaProject.com.game.input
{
    class SWInputStates
    {
        private int _accelerator;
        private int _brake;
        private int _steering;
        private int _clutch;
        private Dictionary<String,int> _gears;
        private Dictionary<String, int> _buttons;
        
        public SWInputStates()
        {
            _gears = new Dictionary<string, int>();
            _buttons = new Dictionary<string, int>();
        }

        public int Accelerator
        {
            get {return _accelerator; }
            set {_accelerator = value; }
        }

        public int Brake
        {
            get { return _brake; }
            set { _brake = value; }
        }

        public int Clutch
        {
            get { return _clutch; }
            set { _clutch = value; }
        }
        
        public int Steering
        {
            get { return _steering; }
            set { _steering = value; }
        }

        public Dictionary<String,int> Gears
        {
            get { return _gears; }
        }

        public String CurrentGear
        {
            get
            {
                foreach (KeyValuePair<String, int> i in Gears)
                {
                    if (i.Value != 0)
                    {
                        return i.Key;
                    }
                }

                return "Neutral";
            }
        }

        public void SetGear(String name, int value)
        {
            if(Gears.ContainsKey(name))
                Gears[name] = value;
            else
                Gears.Add(name, value);
        }



    }
}
