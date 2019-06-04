using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PathViewer
{
    [JsonObject(MemberSerialization.OptIn)]
    class RecentFiles
    {
        [JsonProperty(PropertyName = "recent")]
        public List<string> RecentFileList;

        public RecentFiles()
        {
            RecentFileList = new List<string>();
        }
    }
}
