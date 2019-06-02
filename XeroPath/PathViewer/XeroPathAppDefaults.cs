using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PathViewer
{
    public class XeroPathAppDefaults
    {
        [JsonProperty(PropertyName = "units")]
        public string DefaultUnits;

        [JsonProperty(PropertyName = "undo_stack_size")]
        public int UndoStackSize;

        public XeroPathAppDefaults()
        {
        }
    }
}
