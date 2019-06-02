using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathViewer
{
    public partial class XeroPathAppDefaultsForm : Form
    {
        public XeroPathAppDefaults Defaults;

        public XeroPathAppDefaultsForm(XeroPathAppDefaults defs)
        {
            Defaults = defs;
            InitializeComponent();

            foreach (string units in UnitConverter.SupportedUnits)
            {
                m_units.Items.Add(units);
                if (defs.DefaultUnits == units)
                    m_units.SelectedItem = units;
            }

            m_undostack.Value = defs.UndoStackSize;

            m_ok.Click += M_ok_Click;
        }

        private void M_ok_Click(object sender, EventArgs e)
        {
            Defaults.UndoStackSize = Decimal.ToInt32(m_undostack.Value);
            Defaults.DefaultUnits = (string)m_units.SelectedItem;
        }
    }
}
