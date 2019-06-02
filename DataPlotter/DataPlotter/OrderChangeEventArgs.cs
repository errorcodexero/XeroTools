using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPlotter
{
    class OrderChangeEventArgs : EventArgs
    {
        public readonly int Source;
        public readonly int Destination;

        public OrderChangeEventArgs(int src, int dest)
        {
            Source = src;
            Destination = dest;
        }
    }
}
