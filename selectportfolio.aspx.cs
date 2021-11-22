using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class selectportfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    StockManager stockManager = new StockManager();

                    DataTable portfolioTable = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());
                    if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                    {
                        //ViewState["STOCKMASTER"] = portfolioTable;
                        ListItem li = new ListItem("Select Portfolio", "-1");
                        ddlPortfolios.Items.Add(li);
                        foreach (DataRow rowitem in portfolioTable.Rows)
                        {
                            li = new ListItem(rowitem["PORTFOLIO_NAME"].ToString(), rowitem["ROWID"].ToString());
                            ddlPortfolios.Items.Add(li);
                        }
                    }

                    bool isValuation = false;
                    if (Request.QueryString["valuation"] != null)
                        isValuation = System.Convert.ToBoolean(Request.QueryString["valuation"]);

                    if(isValuation)
                    {
                            buttonLoad.Text = "Show Portfolio Valuation";
                    }
                    else
                    {
                        buttonLoad.Text = "Open Portfolio";
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
        }
        protected void buttonLoad_Click(object sender, EventArgs e)
        {
            //string selectedFile = listboxFiles.SelectedValue;
            if (ddlPortfolios.SelectedIndex > 0)
            {
                Session["STOCKPORTFOLIOMASTERROWID"] = ddlPortfolios.SelectedValue;
                Session["STOCKPORTFOLIONAME"] = ddlPortfolios.SelectedItem.Text;
                bool isValuation = false;
                if (Request.QueryString["valuation"] != null)
                    isValuation = System.Convert.ToBoolean(Request.QueryString["valuation"]);

                if (isValuation == false)
                {
                    //Server.Transfer("~/openportfolio.aspx");
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/openportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Response.Redirect("~/mopenportfolio.aspx");
                    else
                        Response.Redirect("~/mopenportfolio.aspx");
                }
                else
                {
                    string url = "~/portfoliovaluation.aspx" + "?";

                    if (this.MasterPageFile.Contains("Site.Master"))
                    {
                        url += "parent=openportfolio.aspx";
                        ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                    }
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    {
                        url += "parent=mopenportfolio.aspx";
                        ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                    }
                }
            }
            else
            {
                labelSelectedFile.Text = "Selected Portfolio: Please select valid portfolio to open";
                //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioNameToOpen + "');", true);
            }
        }
        protected void ddlPortfolios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPortfolios.SelectedValue.Equals("-1") == true)
            {
                labelSelectedFile.Text = "Selected Portfolio: Please select valid portfolio to open";
            }
            else
            {
                labelSelectedFile.Text = "Selected Portfolio: " + ddlPortfolios.SelectedItem.Text;
            }
        }
    }
}