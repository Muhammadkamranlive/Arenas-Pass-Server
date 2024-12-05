using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class PdfDataModel
    {
        public string Title { get; set; }
        public Dictionary<string, string> Header { get; set; }
        public List<string> TableHeaders { get; set; }
        public List<List<string>> TableData { get; set; }
        public string Footer { get; set; }
    }
}
