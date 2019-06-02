using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinDriverStation
{
    class DriverStationState
    {
        private bool m_comms;
        private bool m_has_robot_code;
        private Mode m_mode;
        private bool m_enabled;
        private List<DSJoystick> m_joysticks;
        private bool m_joysticks_dirty;


        public enum Mode
        {
            Auto,
            Teleop,
            Test
        };

        public event EventHandler<DriverStationStateChangedArgs> StateChanged;
        public event EventHandler<EventArgs> Connected;
        public event EventHandler<EventArgs> Disconnected;

        public DriverStationState()
        {
            m_joysticks_dirty = false;
            m_joysticks = new List<DSJoystick>(4);

            for (int i = 0; i < 4; i++)
                m_joysticks.Add(new DSJoystick());
        }

        public void Reset()
        {
            Communications = false;
            HasRobotCode = false;
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public List<DSJoystick> Joysticks
        {
            get
            {
                if (!m_joysticks_dirty)
                    return null;

                m_joysticks_dirty = false;
                return m_joysticks;
            }
        }


        public Mode RobotMode
        {
            get { return m_mode; }
            set
            {
                if (m_mode != value)
                {
                    m_mode = value;
                    OnStateChanged(true);
                }
            }
        }

        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                if (m_enabled != value)
                {
                    m_enabled = value;
                    OnStateChanged(true);
                }
            }
        }

        public bool HasRobotCode
        {
            get { return m_has_robot_code; }
            set
            {
                if (m_has_robot_code != value)
                {
                    m_has_robot_code = value;
                    OnStateChanged(false);
                    if (m_has_robot_code && m_comms)
                        OnConnected();
                    else if (m_comms)
                        OnDisconnected();
                }
            }
        }

        public bool Communications
        {
            get { return m_comms; }
            set
            {
                if (m_comms != value)
                {
                    m_comms = value;
                    OnStateChanged(false);
                    if (m_has_robot_code && m_comms)
                        OnConnected();
                    else if (m_has_robot_code)
                        OnDisconnected();
                }
            }
        }

        public void SetJoystick(int which, DSJoystick ds)
        {
            m_joysticks[which] = ds;
            m_joysticks_dirty = true;
            OnStateChanged(true);
        }

        protected void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        protected void OnStateChanged(bool torobot)
        {
            StateChanged?.Invoke(this, new DriverStationStateChangedArgs(this, torobot));
        }
    }
}
