using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinDriverStation
{
    class DSJoystick
    {
        private double [] m_axis;
        private bool[] m_buttons;
        private ushort[] m_povs;

        public DSJoystick()
        {
            m_axis = new double[0];
            m_buttons = new bool[0];
            m_povs = new ushort[0];
        }

        public double [] Axis
        {
            get { return m_axis; }
        }

        public bool [] Buttons
        {
            get { return m_buttons; }
        }

        public ushort [] Povs
        {
            get { return m_povs; }
        }

        public void SetAxis(int index, double v)
        {
            if (m_axis.Length <= index)
                Array.Resize<double>(ref m_axis, index + 1);

            m_axis[index] = v;
        }

        public void SetButtons(int index, bool v)
        {
            if (m_buttons.Length <= index)
                Array.Resize<bool>(ref m_buttons, index + 1);
            m_buttons[index] = v;
        }

        public void SetPOV(int index, ushort v)
        {
            if (m_povs.Length <= index)
                Array.Resize<ushort>(ref m_povs, index + 1);

            m_povs[index] = v;
        }
    }
}
