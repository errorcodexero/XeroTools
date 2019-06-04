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
        [JsonProperty(PropertyName = "length_units")]
        public string DefaultLengthUnits;

        [JsonProperty(PropertyName = "angle_units")]
        public string DefaultAngleUnits;

        [JsonProperty(PropertyName = "undo_stack_size")]
        public int UndoStackSize;

        public XeroPathAppDefaults()
        {
        }
    }
}
