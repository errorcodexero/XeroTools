using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;

//
// Packets
//

//
// Data from robot to driver station
//
// Offset           Name
// 0                packet index upper 8 bits
// 1                packet index lower 8 bits
// 2                cTagGeneral
// 3                control
// 4                rstatus (bits: cRobotHasCode)
// 5                voltage upper 8 bits
// 6                voltage lower 8 bits
// 7                request
//

//
// Data from driver station to robot
//
// Offset           Name
// 0                packet index upper 8 bits
// 1                packet index lower 8 bits
// 2                ????
// 3                control (bits: cTest, cAutonomous, cEnabled)
// 4                request (values: cRequestRestartCode, cRequestReboot)
// 5                station (values: cRed1, cRed2, cRed3, cBlue1, cBlue2, cBlue3)
// 6                tag (values: cTagJoystick, cTagDate, cTagTimezone)
// 7 .. N           data block based on tag types
//

namespace WinDriverStation
{
    class DriverStationNetworkConnection
    {
        #region private constants
        private const byte cTest = 0x01;
        private const byte cEnabled = 0x04;
        private const byte cAutonomous = 0x02;
        private const byte cTeleoperated = 0x00;
        private const byte cFMS_Attached = 0x08;
        private const byte cEmergencyStop = 0x80;
        private const byte cRequestReboot = 0x08;
        private const byte cRequestNormal = 0x80;
        private const byte cRequestUnconnected = 0x00;
        private const byte cRequestRestartCode = 0x04;
        private const byte cFMS_RadioPing = 0x10;
        private const byte cFMS_RobotPing = 0x08;
        private const byte cFMS_RobotComms = 0x20;
        private const byte cFMS_DS_Version = 0x00;
        private const byte cTagDate = 0x0f;
        private const byte cTagGeneral = 0x01;
        private const byte cTagJoystick = 0x0c;
        private const byte cTagTimezone = 0x10;
        private const byte cRed1 = 0x00;
        private const byte cRed2 = 0x01;
        private const byte cRed3 = 0x02;
        private const byte cBlue1 = 0x03;
        private const byte cBlue2 = 0x04;
        private const byte cBlue3 = 0x05;
        private const byte cRTagCANInfo = 0x0e;
        private const byte cRTagCPUInfo = 0x05;
        private const byte cRTagRAMInfo = 0x06;
        private const byte cRTagDiskInfo = 0x04;
        private const byte cRequestTime = 0x01;
        private const byte cRobotHasCode = 0x20;
        #endregion

        #region private members
        private DriverStationState m_dsstate;
        private UdpClient m_listener;
        private UdpClient m_sender;
        private Thread m_timeout;
        private Thread m_pingthread;
        private bool m_running;
        private DateTime m_last_comms;
        private object m_last_comms_lock;
        private string m_address;
        #endregion

        #region constants
        public const int ROBOT_IN_PORT = 1110;              // Data from driver station to robot
        public const int ROBOT_OUT_PORT = 1150;             // Data from robot to driver station
        #endregion

        #region public constructors
        public DriverStationNetworkConnection(string addr, DriverStationState st)
        {
            m_address = addr;
            m_dsstate = st;
            m_dsstate.StateChanged += DriverStationStateChanged;
            m_listener = new UdpClient(ROBOT_OUT_PORT);
            m_sender = new UdpClient(ROBOT_IN_PORT);
            m_sender.Connect(addr, ROBOT_IN_PORT);
            m_last_comms_lock = new object();
        }
        #endregion


        #region public methods
        public void Start()
        {
            m_listener.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            m_last_comms = DateTime.Now;
            m_running = true;
            m_timeout = new Thread(new ThreadStart(TimeOutThread));
            m_timeout.Start();
            m_pingthread = new Thread(new ThreadStart(PingThread));
            m_pingthread.Start();
        }

        public void Stop()
        {
            m_dsstate.StateChanged -= DriverStationStateChanged;
            m_running = false;

        }

        public void Close()
        {
            m_listener.Dispose();
            m_sender.Dispose();
        }
        #endregion

        #region private methods
        private void SendCallback(IAsyncResult ar)
        {
        }

        private byte DoubleToByte(double d, double max)
        {
            double v = (d / max) * 255;
            return (byte)v;
        }

        private int JoyLength(double[] axis, bool[] buttons, ushort[] povs)
        {
            int len = 2 + 3;            // Header + buttons

            len++;                      // Number of axis
            len += axis.Length;         // One byte per axis

            len++;                      // Number of POVs
            len += povs.Length * 2;     // Two bytes per POV

            return len;
        }

        private void AddJoystickData(ref byte[] data, double[] axis, bool[] buttons, ushort[] povs)
        {
            int jstart = data.Length;
            int index = data.Length;
            ushort buttonmask = 0;

            Array.Resize<byte>(ref data, data.Length + JoyLength(axis, buttons, povs));
            data[index++] = 0;
            data[index++] = cTagJoystick;
            data[index++] = (byte)axis.Length;
            foreach (double d in axis)
            {
                data[index++] = DoubleToByte(d + 1.0, 2.0);
            }

            data[index++] = (byte)buttons.Length;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i])
                    buttonmask = (ushort)(buttonmask | (1 << i));
            }
            data[index++] = (byte)(buttonmask >> 8);
            data[index++] = (byte)buttonmask;

            data[index++] = (byte)povs.Length;
            foreach (ushort pov in povs)
            {
                data[index++] = (byte)(pov >> 8);
                data[index++] = (byte)(pov);
            }

            data[jstart] = (byte)(index - jstart);
        }

        private void DriverStationStateChanged(object sender, DriverStationStateChangedArgs e)
        {
            byte[] data = new byte[6];
            if (e.robot == false)
                return;

            switch (e.state.RobotMode)
            {
                case DriverStationState.Mode.Auto:
                    data[3] |= cAutonomous;
                    break;
                case DriverStationState.Mode.Teleop:
                    data[3] |= cTeleoperated;
                    break;
                case DriverStationState.Mode.Test:
                    data[3] |= cTest;
                    break;
            }

            if (e.state.Enabled)
                data[3] |= cEnabled;

            List<DSJoystick> joys = e.state.Joysticks;
            if (joys != null)
            {
                foreach (DSJoystick ds in joys)
                    AddJoystickData(ref data, ds.Axis, ds.Buttons, ds.Povs);
            }

            try
            {
                m_sender.BeginSend(data, data.Length, new AsyncCallback(SendCallback), null);
            }
            catch
            {

            }
        }

        private void TimeOutThread()
        {
            TimeSpan span;

            while (m_running)
            {
                Thread.Sleep(500);

                lock(m_last_comms_lock)
                {
                    span = DateTime.Now - m_last_comms;
                }

                if (span.TotalSeconds > 3)
                {
                    m_dsstate.HasRobotCode = false;
                    m_dsstate.Enabled = false;
                }
            }
        }

        private void PingThread()
        {
            Ping p = new Ping();
            while (m_running)
            {
                try
                {
                    PingReply pr = p.Send(m_address);
                    if (pr.Status == IPStatus.Success)
                        m_dsstate.Communications = true;
                    else
                        m_dsstate.Communications = false;
                }
                catch
                {
                    m_dsstate.Communications = false;
                }
                Thread.Sleep(250);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            byte[] bytes;
            IPEndPoint remote = null;
            try
            {
                bytes = m_listener.EndReceive(ar, ref remote);

                if (bytes[2] == cTagGeneral)
                {
                    if ((bytes[4] & cRobotHasCode) != 0)
                        m_dsstate.HasRobotCode = true;
                    else
                        m_dsstate.HasRobotCode = false;
                }

                lock (m_last_comms_lock)
                {
                    m_last_comms = DateTime.Now;
                }
                m_listener.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            }
            catch
            {
            }
        }
        #endregion
    }
}
