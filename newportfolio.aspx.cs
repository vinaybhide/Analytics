using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class newportfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["emailid"] != null)
            //{
            //    Master.UserID = Session["emailid"].ToString();
            //}

            if((Session["emailid"] != null) ||(Session["PortfolioFolder"] != null))
            {
                if (!IsPostBack)
                {
                    textboxPortfolioName.Text = "";
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void buttonNewPortfolio_Click(object sender, EventArgs e)
        {
            string fileName = Session["PortfolioFolder"].ToString() + "\\" + textboxPortfolioName.Text + ".xml";

            if (textboxPortfolioName.Text.Length > 0)
            {
                if (File.Exists(fileName))
                {
                    Response.Write("<script language=javascript>alert('Portfolio already exists.')</script>");
                }
                else
                {
                    StockApi.createNewPortfolio(fileName);
                    Session["PortfolioName"] = fileName;
                    Session["ShortPortfolioName"] = textboxPortfolioName.Text;
                    if(this.MasterPageFile.Contains("Site.Master"))
                        Server.Transfer("~/openportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Server.Transfer("~/mopenportfolio.aspx");
                    else
                        Server.Transfer("~/mopenportfolio.aspx");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noValidNewPortfolioName +"')</script>");
            }
        }
    }
}