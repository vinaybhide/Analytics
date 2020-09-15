using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analytics
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class SearchResult
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string exch { get; set; }
        public string type { get; set; }
        public string exchDisp { get; set; }
        public string typeDisp { get; set; }
    }

    public class ResultSet
    {
        public string Query { get; set; }
        public List<SearchResult> Result { get; set; }
    }

    public class SearchRoot
    {
        public ResultSet ResultSet { get; set; }
    }
}