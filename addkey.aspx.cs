﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class addkey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["EMAILID"] != null) || (Session["PortfolioFolder"] != null))
            {
                if (!IsPostBack)
                {
                    textboxKey.Text = "";
                }
            }
            else
            {
                //Response.Redirect(".\\Default.aspx");
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }

        }

        protected void buttonAddKey_Click(object sender, EventArgs e)
        {
            if (textboxKey.Text.Length > 0)
            {
                string emailId = Session["EMAILID"].ToString();
                string fileName = Session["PortfolioFolder"].ToString() + "\\" + emailId + ".key";
                StockApi.createKey(fileName, textboxKey.Text);
            }
            else
            {
                //Response.Redirect(".\\Default.aspx");
                //Response.Write("<script language=javascript>alert('Enter valid key')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noValidKey + "');", true);

            }

        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            string folder = Session["PortfolioFolder"].ToString();
            if ((Directory.GetFiles(folder, "*")).Length > 0)
            {
                //Server.Transfer("~/openportfolio.aspx");
                if (this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect("~/selectportfolio.aspx");
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    Response.Redirect("~/mselectportfolio.aspx");
                else
                    Response.Redirect("~/mselectportfolio.aspx");
            }
            else
            {
                if (this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect("~/newportfolio.aspx");
                else if (this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect("~/mnewportfolio.aspx");
                else
                    Response.Redirect("~/mnewportfolio.aspx");
            }
        }
    }
}