using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SharpDX.DirectInput;
using System.Diagnostics;

namespace WinDriverStation
{
    class JoystickManager
    {
        private DirectInput m_di;
        private IDictionary<int, Joystick> m_joysticks;
        private DriverStationState m_state;
        private Thread m_thread;
        private bool m_monitor;

        public JoystickManager(DriverStationState st)
        {
            m_state = st;
            m_di = new DirectInput();
            m_joysticks = new Dictionary<int, Joystick>();

            SearchForJoysticks();

            m_monitor = true;
            m_thread = new Thread(new ThreadStart(MonitorJoysticks));
            m_thread.Start();
        }

        public void Stop()
        {
            m_monitor = false;
        }

        private DSJoystick ReadJoystick(Joystick j)
        {
            DSJoystick ds = new DSJoystick();

            j.Poll();

            JoystickState st = j.GetCurrentState();

            // Process buttons
            for (int i = 0; i < st.Buttons.Length; i++)
            {
                ds.SetButtons(i + 1, st.Buttons[i]);
            }

            // Process axis
            ds.SetAxis(0, (st.X / 32768.0) - 1.0);
            ds.SetAxis(1, (st.Y / 32768.0) - 1.0);
            ds.SetAxis(4, (st.RotationX / 32768.0) - 1.0);
            ds.SetAxis(5, (st.RotationY / 32768.0) - 1.0);

            if (st.Z < 256)
            {
                // Right trigger
                ds.SetAxis(3, 1.0);
                ds.SetAxis(2, 0.0);
            }
            else if (st.Z > 65000)
            {
                // Left trigger
                ds.SetAxis(3, 0.0);
                ds.SetAxis(2, 1.0);
            }
            else
            {
                // No (or both) triggers
                ds.SetAxis(3, 0.0);
                ds.SetAxis(2, 0.0);
            }

            // Process POVs
            int[] povs = st.PointOfViewControllers;
            for (int i = 0; i < povs.Length; i++)
            {
                if (povs[i] == -1)
                    ds.SetPOV(i, 0xffff);
                else
                {
                    ushort deg = (ushort)(povs[i] / 100);
                    ds.SetPOV(i, deg);
                }
            }

            return ds;
        }

        private void MonitorJoysticks()
        {
            while (m_monitor)
            {
                for(int i = 0; i < m_joysticks.Count; i++)
                {
                    DSJoystick dsj = ReadJoystick(m_joysticks[i]);
                    m_state.SetJoystick(i, dsj);
                }
                Thread.Sleep(1);
            }
        }

        public int Count
        {
            get { return m_joysticks.Count; }
        }

        private void SearchForJoysticks()
        {
            foreach(var di in m_di.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                CheckOneDevice(di);

            foreach (var di in m_di.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                CheckOneDevice(di);
        }

        private void CheckOneDevice(DeviceInstance di)
        {
            int index = m_joysticks.Count;
            if (index == 2)
                return;

            Joystick j = new Joystick(m_di, di.InstanceGuid);
            j.Acquire();
            m_joysticks[index] = j;
        }
    }
}
