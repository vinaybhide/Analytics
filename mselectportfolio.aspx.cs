using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccessLayer;

namespace Analytics
{
    public partial class mselectportfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    Session["STOCKPORTFOLIOMASTERROWID"] = null;
                    Session["STOCKPORTFOLIONAME"] = null;
                    Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
                    ViewState["MASTER_DATA"] = null;

                    lblDashboard.Text = "Global Investment Manager for: " + Session["EMAILID"].ToString();
                    GetPortfolios();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
        }

        public void GetPortfolios()
        {
            DataTable dt;
            StockManager stockManager = new StockManager();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);

            try
            {
                if (Session["EMAILID"] != null)
                {
                    if ((ViewState["MASTER_DATA"] == null) || (((DataTable)ViewState["MASTER_DATA"]).Rows.Count == 0))
                    {
                        dt = stockManager.getAllPortfolioTableForUserId(Session["EMAILID"].ToString());
                        ViewState["MASTER_DATA"] = dt;
                    }
                    else
                    {
                        dt = (DataTable)ViewState["MASTER_DATA"];
                    }
                    gvPortfolioMaster.DataSource = dt;
                    gvPortfolioMaster.DataBind();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('You need to login before you can select portfolios!');", true);
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while opening portfolio: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while opening portfolio:" + ex.Message + "');", true);
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);
        }

        /// <summary>
        /// This method is used to open the selected portfolio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                if (gvr.RowIndex >= 0)
                {
                    DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];
                    Session["STOCKPORTFOLIOMASTERROWID"] = tableSource.Rows[gvr.RowIndex]["ROWID"].ToString();
                    Session["STOCKPORTFOLIONAME"] = tableSource.Rows[gvr.RowIndex]["PORTFOLIO_NAME"].ToString();
                    Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
                    Response.Redirect("~/mopenportfolio.aspx");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying open portfolio. Please try again. :" + ex.Message + "');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                if (gvr.RowIndex >= 0)
                {
                    DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];
                    string portfolioMasterId = tableSource.Rows[gvr.RowIndex]["ROWID"].ToString();
                    StockManager stockManager = new StockManager();
                    if (stockManager.DeletePortfolio(portfolioMasterId))
                    {
                        Session["STOCKPORTFOLIOMASTERROWID"] = null;
                        Session["STOCKPORTFOLIONAME"] = null;
                        Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
                        ViewState["MASTER_DATA"] = null;
                        GetPortfolios();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while deleting portfolio. Please try again. :" + ex.Message + "');", true);
            }
            gvPortfolioMaster.EditIndex = -1;
            gvPortfolioMaster.Columns[3].Visible = false;
        }

        protected void btnValuation_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                if (gvr.RowIndex >= 0)
                {
                    DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];
                    Session["STOCKPORTFOLIOMASTERROWID"] = tableSource.Rows[gvr.RowIndex]["ROWID"].ToString();
                    Session["STOCKPORTFOLIONAME"] = tableSource.Rows[gvr.RowIndex]["PORTFOLIO_NAME"].ToString();
                    Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
                    string url = "~/advGraphs/stockvaluationline.aspx" + "?";

                    url += "parent=mselectportfolio.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while generating portfolio valuation graph. Please try again. :" + ex.Message + "');", true);
            }
        }

        protected void buttonNew_Click(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
            //Response.Redirect("~/mnewportfolio.aspx");
            gvPortfolioMaster_RowEditing(null, null);
        }

        protected void buttonImport_Click(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/mimportstockfolio.aspx");
        }

        protected void buttonGetQuote_Click(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/mgetquoteadd.aspx");
        }

        protected void buttonStdIndicators_Click(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/mshowgraph.aspx");
        }

        protected void buttonAdvIndicators_Click(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/madvancegraphs.aspx");
        }

        protected void buttonGlobalIndices_Click(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/advGraphs/globalindex.aspx", "_blank", "");
        }


        /// <summary>
        /// This method will create blank row in Portfolio Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPortfolioMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if(ViewState["MASTER_DATA"] != null)
            {
                if (gvPortfolioMaster.EditIndex != -1)
                {
                    ViewState["MASTER_DATA"] = null;
                    GetPortfolios();
                }

                DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];
                DataRow newRow = tableSource.NewRow();
                newRow["PORTFOLIO_NAME"] = "";
                newRow["CumulativeCost"] = 0.00;
                newRow["CumulativeValue"] = 0.00;
                tableSource.Rows.Add(newRow);
                ViewState["MASTER_DATA"] = tableSource;
                gvPortfolioMaster.EditIndex = gvPortfolioMaster.Rows.Count;
                GetPortfolios();
                gvPortfolioMaster.Columns[3].Visible = true;
            }
        }

        protected void gvPortfolioMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if(ViewState["MASTER_DATA"] != null)
            {
                DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];
                tableSource.Rows.RemoveAt(e.RowIndex);
                ViewState["MASTER_DATA"] = tableSource;
                gvPortfolioMaster.EditIndex = -1;
                GetPortfolios();
                gvPortfolioMaster.Columns[3].Visible = false;
            }
        }

        protected void gvPortfolioMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ViewState["MASTER_DATA"] != null)
            {
                TextBox textboxPortfolioName = (TextBox)gvPortfolioMaster.Rows[e.RowIndex].FindControl("textboxPortfolioName");
                
                if (string.IsNullOrWhiteSpace(textboxPortfolioName.Text) == false) //&& textboxPortfolioName.Text.Contains(c => (char.IsLetterOrDigit(c))))
                {
                    StockManager stockManager = new StockManager();

                    if (stockManager.getPortfolioId(Session["EMAILID"].ToString(), textboxPortfolioName.Text) <= 0)
                    {
                        long stockportfolio_rowid = stockManager.createNewPortfolio(Session["EMAILID"].ToString(), textboxPortfolioName.Text);
                        Session["STOCKPORTFOLIOMASTERROWID"] = null;
                        Session["STOCKPORTFOLIONAME"] = null;
                        Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;
                        ViewState["MASTER_DATA"] = null;
                        gvPortfolioMaster.EditIndex = -1;
                        GetPortfolios();
                        gvPortfolioMaster.Columns[3].Visible = false;
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
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Unhandled exception. Please restart the application');", true);
            }
        }
    }
}