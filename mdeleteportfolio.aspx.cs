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
    public partial class mdeleteportfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EMAILID"] != null)
            //{
            //    Master.UserID = Session["EMAILID"].ToString();
            //}

            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    StockManager stockManager = new StockManager();

                    DataTable portfolioTable = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());
                    if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                    {
                        //ViewState["STOCKMASTER"] = portfolioTable;
                        ListItem li = new ListItem("Select Portfolio", "-1");
                        ddlFiles.Items.Add(li);
                        foreach (DataRow rowitem in portfolioTable.Rows)
                        {
                            li = new ListItem(rowitem["PORTFOLIO_NAME"].ToString(), rowitem["ROWID"].ToString());
                            ddlFiles.Items.Add(li);
                        }
                    }
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }

        }
        protected void buttonDelete_Click(object sender, EventArgs e)
        {
            //if (deletePortfolioName.Equals("-1") == false)
            if (ddlFiles.SelectedIndex > 0)
            {
                string portfolioMasterId = ddlFiles.SelectedValue;
                StockManager stockManager = new StockManager();
                if (stockManager.DeletePortfolio(portfolioMasterId))
                {
                    if (stockManager.getPortfolioCount(Session["EMAILID"].ToString()) > 0)
                    {
                        Response.Redirect("~/mselectportfolio.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/mnewportfolio.aspx");
                    }

                }
                else
                {
                    labelSelectedFile.Text = "Problem encountered while trying to delete portfolio. Please try again later";
                    //Response.Write("<script language=javascript>alert('"+ common.noPortfolioSelectedToDelete +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Problem encountered while trying to delete portfolio. Please try again later');", true);
                }

            }
            else
            {
                labelSelectedFile.Text = "Selected File: Please select portfolio to delete";
                //Response.Write("<script language=javascript>alert('"+ common.noPortfolioSelectedToDelete +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelectedToDelete + "');", true);
            }
        }
        protected void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiles.SelectedValue.Equals("-1") == true)
            {
                labelSelectedFile.Text = "Selected portfolio: Please select valid portfolio to delete";
            }
            else
            {
                labelSelectedFile.Text = $"{"Selected portfolio:"}{ddlFiles.SelectedItem.Text}";
            }
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            StockManager stockManager = new StockManager();
            if (stockManager.getPortfolioCount(Session["EMAILID"].ToString()) > 0)
            {
                Response.Redirect("~/mselectportfolio.aspx");
            }
            else
            {
                Response.Redirect("~/mnewportfolio.aspx");
            }
        }
    }
}