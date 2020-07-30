using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class SiteMaster : MasterPage
    {
        public bool setDownloaddata
        {
            get
            {
                return downloaddatalink.Visible;
            }

            set
            {
                downloaddatalink.Visible = value;
            }
        }

        public string s;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}