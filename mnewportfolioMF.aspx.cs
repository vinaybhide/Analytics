using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class mnewportfolioMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["EMAILID"] != null) || (Session["PortfolioFolderMF"] != null))
            {
                if (!IsPostBack)
                {
                    textboxPortfolioName.Text = "";
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
        }
        protected void buttonNewPortfolio_Click(object sender, EventArgs e)
        {
            string fileName = Session["PortfolioFolderMF"].ToString() + "\\" + textboxPortfolioName.Text + ".mfl";

            if (textboxPortfolioName.Text.Length > 0)
            {
                //if (File.Exists(fileName))
                DataManager dataMgr = new DataManager();
                if(dataMgr.getPortfolioId(textboxPortfolioName.Text, Session["EMAILID"].ToString(), sqlite_cmd: null) > 0)
                {
                    //Response.Write("<script language=javascript>alert('Portfolio already exists.')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.portfolioExists + "');", true);
                }
                else
                {
                    //MFAPI.createnewMFPortfolio(fileName);
                    long portfolioRowId = dataMgr.createnewMFPortfolio(Session["EMAILID"].ToString(), textboxPortfolioName.Text.Trim());
                    //Session["PortfolioNameMF"] = fileName;
                    Session["MFPORTFOLIONAME"] = textboxPortfolioName.Text;
                    Session["MFPORTFOLIOROWID"] = portfolioRowId.ToString();
                    Server.Transfer("~/mopenportfolioMF.aspx");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noValidNewPortfolioName +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noValidNewPortfolioName + "');", true);
            }
        }

    }
}