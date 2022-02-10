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
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOMASTERROWID"] = null;
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    DataManager dataMgr = new DataManager();
                    DataTable portfolioTable = dataMgr.getPortfolioTable(Session["EMAILID"].ToString());
                    if((portfolioTable != null) && (portfolioTable.Rows.Count >0))
                    {
                        ddlPortfolios.DataTextField = "PORTFOLIO_NAME";
                        ddlPortfolios.DataValueField = "ID";
                        ddlPortfolios.DataSource = portfolioTable;
                        ddlPortfolios.DataBind();
                    }
                    ListItem li = new ListItem("Select MF Portfolio", "-1");
                    ddlPortfolios.Items.Insert(0, li);

                    //string folder = Session["PortfolioFolderMF"].ToString();
                    //string[] filelist = Directory.GetFiles(folder, "*.mfl");
                    //Session["MFPORTFOLIOFUNDNAME"] = null;

                    //ListItem li = new ListItem("Select MF Portfolio", "-1");
                    //ddlPortfolios.Items.Insert(0, li);

                    //foreach (string filename in filelist)
                    //{
                    //    string portfolioName = filename.Remove(0, filename.LastIndexOf('\\') + 1);
                    //    ListItem filenameItem = new ListItem(portfolioName, filename);
                    //    ddlPortfolios.Items.Add(filenameItem);
                    //}


                    //listboxFiles.Width = lstwidth * 10;
                    bool isValuation = false;
                    if (Request.QueryString["valuation"] != null)
                        isValuation = System.Convert.ToBoolean(Request.QueryString["valuation"]);

                    if (isValuation)
                    {
                        if(Request.QueryString["line"] != null)
                        {
                            if(System.Convert.ToBoolean(Request.QueryString["line"]) == true)
                            {
                                buttonLoad.Text = "Portfolio: Show Fund Valuation";
                            }
                            else
                            {
                                buttonLoad.Text = "Portfolio: Show Cost Vs Value";
                            }
                        }
                    }
                    else
                    {
                        buttonLoad.Text = "Open MF Portfolio";
                    }
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

        protected void buttonLoad_Click(object sender, EventArgs e)
        {
            //string selectedFile = listboxFiles.SelectedValue;
            //if (ddlPortfolios.SelectedIndex > 0)
            if(ddlPortfolios.SelectedValue.Equals("-1") == false)
            {
                //Session["PortfolioNameMF"] = ddlPortfolios.SelectedValue;
                Session["MFPORTFOLIONAME"] = ddlPortfolios.SelectedItem.Text;
                Session["MFPORTFOLIOMASTERROWID"] = ddlPortfolios.SelectedValue;
                Session["MFSELECTEDINDEXPORTFOLIO"] = null;

                bool isValuation = false;
                if (Request.QueryString["valuation"] != null)
                    isValuation = System.Convert.ToBoolean(Request.QueryString["valuation"]);

                if (isValuation == false)
                {
                    //Server.Transfer("~/openportfolio.aspx");
                    //if (this.MasterPageFile.Contains("Site.Master"))
                    //    Response.Redirect("~/openportfolioMF.aspx");
                    //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    //    Response.Redirect("~/mopenportfolioMF.aspx");
                    //else
                        Response.Redirect("~/mopenportfolioMF.aspx");
                }
                else
                {
                    string url = "";
                    if (Request.QueryString["line"] != null)
                    {
                        if (System.Convert.ToBoolean(Request.QueryString["line"]) == true)
                        {
                            url = "~/advGraphs/mfvaluationline.aspx" + "?";
                        }
                        else
                        {
                            url = "~/advGraphs/mfvaluationbar.aspx" + "?";
                        }
                    }


                    //if (this.MasterPageFile.Contains("Site.Master"))
                    //{
                    //    url += "parent=openportfolioMF.aspx";
                    //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                    //}
                    //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    //{
                    //    url += "parent=mopenportfolioMF.aspx";
                    //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
                    //}
                    url += "parent=mopenportfolioMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
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