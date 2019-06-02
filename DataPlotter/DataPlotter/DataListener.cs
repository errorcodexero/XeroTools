using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPlotter
{
    abstract class DataListener
    {
        public abstract string WaitForData();
        public abstract void ShutDown();
    }
}
