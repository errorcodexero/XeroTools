using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPlotter
{
    class DataManagerEventArgs: EventArgs
    {
        public readonly DataSession Session;

        public DataManagerEventArgs(DataSession session)
        {
            Session = session;
        }
    }
}
