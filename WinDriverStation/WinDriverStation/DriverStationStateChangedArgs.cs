using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinDriverStation
{
    class DriverStationStateChangedArgs : EventArgs
    {
        public DriverStationState state;
        public bool robot;

        public DriverStationStateChangedArgs(DriverStationState st, bool torobot)
        {
            state = st;
            robot = torobot;
        }
    }
}
