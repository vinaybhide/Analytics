using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PortfolioFolder"] = null;
            Session["EMAILID"] = null;
            Session["DATAFOLDER"] = null;
            Session["IsTestOn"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKPORTFOLIOSCRIPTNAME"] = null;

            if (this.MasterPageFile.Contains("Site.Master"))
            {
                loginlink.HRef = "mlogin.aspx";
                registerlink.HRef = "mlogin.aspx";
            }
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            {
                loginlink.HRef = "mlogin.aspx";
                registerlink.HRef = "mlogin.aspx";
            }
            else
            {
                loginlink.HRef = "mlogin.aspx";
                registerlink.HRef = "mlogin.aspx";
            }
        }
    }
}