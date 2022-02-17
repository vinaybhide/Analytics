using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccessLayer;

namespace Analytics
{
    public partial class mselectportfolioMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if ((Session["EMAILID"] != null) || (Session["PortfolioFolderMF"] != null))
            //Session["MFPORTFOLIONAME"] = null;
            //Session["MFPORTFOLIOMASTERROWID"] = null;
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    Session["MFPORTFOLIONAME"] = null;
                    Session["MFPORTFOLIOMASTERROWID"] = null;
                    Session["MFSELECTEDINDEXPORTFOLIO"] = null;
                    ViewState["MASTER_DATA"] = null;
                    
                    lblDashboard.Text = "MF Portfolio Manager for: " + Session["EMAILID"].ToString();
                    GetPortfolios();
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

        public void GetPortfolios()
        {
            DataTable dt;
            DataManager dataManager = new DataManager();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);
            try
            {
                if (Session["EMAILID"] != null)
                {
                    if ((ViewState["MASTER_DATA"] == null) || (((DataTable)ViewState["MASTER_DATA"]).Rows.Count == 0))
                    {
                        dt = dataManager.getAllMFPortfolioTableForUserId(Session["EMAILID"].ToString());
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
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);
        }

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

                    Session["MFPORTFOLIONAME"] = tableSource.Rows[gvr.RowIndex]["PORTFOLIO_NAME"].ToString(); ;
                    Session["MFPORTFOLIOMASTERROWID"] = tableSource.Rows[gvr.RowIndex]["ID"].ToString();
                    Session["MFSELECTEDINDEXPORTFOLIO"] = null;

                    Response.Redirect("~/mopenportfolioMF.aspx");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to open portfolio. Please try again. :" + ex.Message + "');", true);
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
                    string portfolioMasterId = tableSource.Rows[gvr.RowIndex]["ID"].ToString();
                    DataManager dataManager = new DataManager();
                    if (dataManager.deletePortfolio(Session["EMAILID"].ToString(), portfolioMasterId))
                    {
                        Session["MFPORTFOLIONAME"] = null;
                        Session["MFPORTFOLIOMASTERROWID"] = null;
                        Session["MFSELECTEDINDEXPORTFOLIO"] = null;
                        ViewState["MASTER_DATA"] = null;

                        GetPortfolios();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to delete portfolio. Please try again. :" + ex.Message + "');", true);
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

                    Session["MFPORTFOLIONAME"] = tableSource.Rows[gvr.RowIndex]["PORTFOLIO_NAME"].ToString(); ;
                    Session["MFPORTFOLIOMASTERROWID"] = tableSource.Rows[gvr.RowIndex]["ID"].ToString();
                    Session["MFSELECTEDINDEXPORTFOLIO"] = null;

                    string url = "~/advGraphs/mfvaluationline.aspx" + "?";

                    url += "parent=mselectportfolioMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to show portfolio valuation. Please try again. :" + ex.Message + "');", true);
            }
        }
        protected void btnBarGraph_Click(object sender, EventArgs e)
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

                    Session["MFPORTFOLIONAME"] = tableSource.Rows[gvr.RowIndex]["PORTFOLIO_NAME"].ToString(); ;
                    Session["MFPORTFOLIOMASTERROWID"] = tableSource.Rows[gvr.RowIndex]["ID"].ToString();
                    Session["MFSELECTEDINDEXPORTFOLIO"] = null;

                    string url = "~/advGraphs/mfvaluationbar.aspx" + "?";

                    url += "parent=mselectportfolioMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to show portfolio valuation. Please try again. :" + ex.Message + "');", true);
            }
        }
        protected void buttonNew_Click(object sender, EventArgs e)
        {
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOMASTERROWID"] = null;
            Session["MFSELECTEDINDEXPORTFOLIO"] = null;
            //Response.Redirect("~/mnewportfolioMF.aspx");
            gvPortfolioMaster_RowEditing(null, null);
        }

        protected void buttonImport_Click(object sender, EventArgs e)
        {
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOMASTERROWID"] = null;
            Session["MFSELECTEDINDEXPORTFOLIO"] = null;
            //Response.Redirect("~/mimportstockfolio.aspx");
        }

        protected void buttonStdIndicators_Click(object sender, EventArgs e)
        {
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOMASTERROWID"] = null;
            Session["MFSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/Graphs/mfgraphmain.aspx", "_blank", "");
        }

        protected void buttonGlobalIndices_Click(object sender, EventArgs e)
        {
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOMASTERROWID"] = null;
            Session["MFSELECTEDINDEXPORTFOLIO"] = null;
            Response.Redirect("~/advGraphs/globalindex.aspx", "_blank", "");
        }

        protected void gvPortfolioMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (ViewState["MASTER_DATA"] != null)
            {
                if(gvPortfolioMaster.EditIndex != -1)
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
            if (ViewState["MASTER_DATA"] != null)
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
                    DataManager dataManager = new DataManager();

                    if (dataManager.getPortfolioId(textboxPortfolioName.Text, Session["EMAILID"].ToString()) <= 0)
                    {
                        long mfportfolio_rowid = dataManager.createnewMFPortfolio(Session["EMAILID"].ToString(), textboxPortfolioName.Text);
                        Session["MFPORTFOLIONAME"] = null;
                        Session["MFPORTFOLIOMASTERROWID"] = null;
                        Session["MFSELECTEDINDEXPORTFOLIO"] = null;
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

        ///// <summary>
        ///// This method is used to open the selected portfolio
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void gvPortfolioMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];

        //            Session["MFPORTFOLIONAME"] = tableSource.Rows[e.RowIndex]["PORTFOLIO_NAME"].ToString(); ;
        //            Session["MFPORTFOLIOMASTERROWID"] = tableSource.Rows[e.RowIndex]["ID"].ToString();
        //            Session["MFSELECTEDINDEXPORTFOLIO"] = null;

        //            Response.Redirect("~/mopenportfolioMF.aspx");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to call portfolio oppen. Please try again. :" + ex.Message + "');", true);
        //    }
        //}

        ///// <summary>
        ///// We will use this to show valuation graph for the clicked portfolio row
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void gvPortfolioMaster_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    try
        //    {
        //        if (e.NewEditIndex >= 0)
        //        {
        //            DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];

        //            Session["MFPORTFOLIONAME"] = tableSource.Rows[e.NewEditIndex]["PORTFOLIO_NAME"].ToString(); ;
        //            Session["MFPORTFOLIOMASTERROWID"] = tableSource.Rows[e.NewEditIndex]["ID"].ToString();
        //            Session["MFSELECTEDINDEXPORTFOLIO"] = null;

        //            string url = "~/advGraphs/mfvaluationline.aspx" + "?";

        //            url += "parent=mselectportfolioMF.aspx";
        //            ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to show portfolio valuation. Please try again. :" + ex.Message + "');", true);
        //    }
        //}


        ///// <summary>
        ///// THis method deletes the portfolio
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void gvPortfolioMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            DataTable tableSource = (DataTable)ViewState["MASTER_DATA"];
        //            string portfolioMasterId = tableSource.Rows[e.RowIndex]["ID"].ToString();
        //            DataManager dataManager = new DataManager();
        //            if (dataManager.deletePortfolio(Session["EMAILID"].ToString(), portfolioMasterId))
        //            {
        //                Session["MFPORTFOLIONAME"] = null;
        //                Session["MFPORTFOLIOMASTERROWID"] = null;
        //                Session["MFSELECTEDINDEXPORTFOLIO"] = null;
        //                ViewState["MASTER_DATA"] = null;

        //                GetPortfolios();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while trying to delete portfolio. Please try again. :" + ex.Message + "');", true);
        //    }
        //}


        //protected void buttonLoad_Click(object sender, EventArgs e)
        //{
        //    //string selectedFile = listboxFiles.SelectedValue;
        //    //if (ddlPortfolios.SelectedIndex > 0)
        //    if(ddlPortfolios.SelectedValue.Equals("-1") == false)
        //    {
        //        //Session["PortfolioNameMF"] = ddlPortfolios.SelectedValue;
        //        Session["MFPORTFOLIONAME"] = ddlPortfolios.SelectedItem.Text;
        //        Session["MFPORTFOLIOMASTERROWID"] = ddlPortfolios.SelectedValue;
        //        Session["MFSELECTEDINDEXPORTFOLIO"] = null;

        //        bool isValuation = false;
        //        if (Request.QueryString["valuation"] != null)
        //            isValuation = System.Convert.ToBoolean(Request.QueryString["valuation"]);

        //        if (isValuation == false)
        //        {
        //            //Server.Transfer("~/openportfolio.aspx");
        //            //if (this.MasterPageFile.Contains("Site.Master"))
        //            //    Response.Redirect("~/openportfolioMF.aspx");
        //            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
        //            //    Response.Redirect("~/mopenportfolioMF.aspx");
        //            //else
        //                Response.Redirect("~/mopenportfolioMF.aspx");
        //        }
        //        else
        //        {
        //            string url = "";
        //            if (Request.QueryString["line"] != null)
        //            {
        //                if (System.Convert.ToBoolean(Request.QueryString["line"]) == true)
        //                {
        //                    url = "~/advGraphs/mfvaluationline.aspx" + "?";
        //                }
        //                else
        //                {
        //                    url = "~/advGraphs/mfvaluationbar.aspx" + "?";
        //                }
        //            }


        //            //if (this.MasterPageFile.Contains("Site.Master"))
        //            //{
        //            //    url += "parent=openportfolioMF.aspx";
        //            //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
        //            //}
        //            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
        //            //{
        //            //    url += "parent=mopenportfolioMF.aspx";
        //            //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
        //            //}
        //            url += "parent=mopenportfolioMF.aspx";
        //            ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
        //        }
        //    }
        //    else
        //    {
        //        labelSelectedFile.Text = "Selected Portfolio: Please select valid portfolio to open";
        //        //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen +"')</script>");
        //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioNameToOpen + "');", true);
        //    }
        //}
        //protected void ddlPortfolios_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlPortfolios.SelectedValue.Equals("-1") == true)
        //    {
        //        labelSelectedFile.Text = "Selected Portfolio: Please select valid portfolio to open";
        //    }
        //    else
        //    {
        //        labelSelectedFile.Text = "Selected Portfolio: " + ddlPortfolios.SelectedItem.Text;
        //    }
        //}

    }
}