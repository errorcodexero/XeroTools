using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinDriverStation
{
    public partial class RedGreenControl : UserControl
    {
        private bool m_red;

        public RedGreenControl()
        {
            m_red = true;
            InitializeComponent();
            DoubleBuffered = true;
        }

        public bool Red
        {
            get { return m_red; }
            set { m_red = value; Refresh(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color c;
            base.OnPaint(e);

            if (m_red)
                c = Color.Red;
            else
                c = Color.Green;

            using (SolidBrush b = new SolidBrush(c))
            {
                Rectangle r = new Rectangle(0, 0, Width, Height);
                e.Graphics.FillRectangle(b, r);
            }

        }
    }
}
