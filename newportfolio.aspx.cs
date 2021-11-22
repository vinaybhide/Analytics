using DataAccessLayer;
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
            if (Session["EMAILID"] != null)
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
            if (textboxPortfolioName.Text.Length > 0)
            {
                StockManager stockManager = new StockManager();

                if (stockManager.getPortfolioId(Session["EMAILID"].ToString(), textboxPortfolioName.Text) <= 0)
                {
                    long stockportfolio_rowid = stockManager.createNewPortfolio(Session["EMAILID"].ToString(), textboxPortfolioName.Text);
                    Session["STOCKPORTFOLIONAME"] = textboxPortfolioName.Text;
                    Session["STOCKPORTFOLIOROWID"] = stockportfolio_rowid;
                    Server.Transfer("~/mopenportfolio.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.portfolioExists + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noValidNewPortfolioName + "');", true);
            }
        }
    }
}