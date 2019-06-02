using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinDriverStation
{
    public partial class Form1 : Form
    {
        private delegate void StateChangedDelegate(DriverStationState st);

        private DriverStationNetworkConnection m_robot_connection = null;
        private DriverStationState m_state = null;
        private JoystickManager m_joystick_mgr;
        private double[] m_autovalues = new double[] { -1.0, -0.8, -0.6, -0.4, -0.2, 0.1, 0.3, 0.5, 0.7, 0.9 };

        public Form1()
        {
            m_state = new DriverStationState();
            m_joystick_mgr = new JoystickManager(m_state);
            InitializeComponent();
            m_disabled.Checked = true;
            m_auto.Checked = true;

            m_state.StateChanged += StateChanged;
            m_state.Connected += RobotConnected;

            if (m_joystick_mgr.Count > 0)
                m_joysticks.Red = false;

            m_mode.Enabled = false;
            m_control.Enabled = false;

            m_automode.ValueChanged += AutoModeValueChanged;
            m_servo.ValueChanged += AutoModeValueChanged;
        }

        private void RobotConnected(object sender, EventArgs e)
        {
            AutoModeValueChanged(this);
        }

        private void AutoModeValueChanged(object Sender)
        {
            int v = m_automode.Value;
            if (v < 0)
                v = 0;

            if (v > 9)
                v = 0;

            double automodev = m_autovalues[v];
            double servov = m_servo.Value / 90.0;

            if (m_state != null)
            {
                DSJoystick joy = new DSJoystick();
                for (int i = 0; i < 10; i++)
                {
                    if (i == 6)
                        joy.SetAxis(i, m_autovalues[v]);
                    else if (i == 0)
                    {
                        Debug.WriteLine("servo " + servov.ToString());
                        joy.SetAxis(i, servov);
                    }
                }
                m_state.SetJoystick(2, joy);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            m_joystick_mgr.Stop();
            if (m_robot_connection != null)
                m_robot_connection.Stop();
            m_state.StateChanged -= StateChanged;
        }

        private void StateChanged(DriverStationState st)
        {
            m_comms.Red = !st.Communications;
            m_robot.Red = !st.HasRobotCode;

            if (st.Communications && st.HasRobotCode)
            {
                m_mode.Enabled = true;
                m_control.Enabled = true;
            }
            else
            {
                m_mode.Enabled = false;
                m_control.Enabled = false;
            }
        }

        private void StateChanged(object sender, DriverStationStateChangedArgs e)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new StateChangedDelegate(StateChanged), new object[] { e.state });
                }
                catch
                {
                }
            }
            else
            {
                StateChanged(e.state);
            }
        }

        private void ConnectButtonClicked(object sender, EventArgs e)
        {
            if (m_robot_connection == null)
            {
                try
                {
                    string str = m_robot_name.Text;
                    if (!str.Contains("."))
                        str += ".local";
                    m_robot_connection = new DriverStationNetworkConnection(str, m_state);
                    m_robot_connection.Start();
                    m_robot_name.Enabled = false;
                    m_connect.Text = "Disconnect";

                    m_control.Enabled = true;
                    m_mode.Enabled = true;
                    m_state.RobotMode = DriverStationState.Mode.Auto;
                }
                catch (Exception ex)
                {
                    string msg = "Cannot connect to roboto '" + m_robot_name + "' - " + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_state.Reset();
                }
            }
            else
            {
                m_robot_connection.Stop();
                m_robot_connection.Close();
                m_robot_connection = null;
                m_connect.Text = "Connect";
                m_robot_name.Enabled = true;

                m_control.Enabled = true;
                m_mode.Enabled = true;
                m_state.Reset();
            }
        }

        private void TeleopButtonClicked(object sender, EventArgs e)
        {
            if (m_state != null)
                m_state.RobotMode = DriverStationState.Mode.Teleop;
        }

        private void AutoButtonClicked(object sender, EventArgs e)
        {
            if (m_state != null)
                m_state.RobotMode = DriverStationState.Mode.Auto;
        }

        private void TestButtonClicked(object sender, EventArgs e)
        {
            if (m_state != null)
                m_state.RobotMode = DriverStationState.Mode.Teleop;
        }

        private void EnableButtonClicked(object sender, EventArgs e)
        {
            if (m_state != null)
                m_state.Enabled = true;
        }

        private void DisableButtonClicked(object sender, EventArgs e)
        {
            if (m_state != null)
                m_state.Enabled = false;
        }
    }
}
