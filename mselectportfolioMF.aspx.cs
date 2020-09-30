using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class mselectportfolioMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["EmailId"] != null) || (Session["PortfolioFolderMF"] != null))
            {
                if (!IsPostBack)
                {
                    string folder = Session["PortfolioFolderMF"].ToString();
                    string[] filelist = Directory.GetFiles(folder, "*.mfl");
                    Session["MFName"] = null;
                    //int lstwidth = 0;

                    ListItem li = new ListItem("Select MF Portfolio", "-1");
                    ddlPortfolios.Items.Insert(0, li);

                    foreach (string filename in filelist)
                    {
                        string portfolioName = filename.Remove(0, filename.LastIndexOf('\\') + 1);
                        ListItem filenameItem = new ListItem(portfolioName, filename);
                        ddlPortfolios.Items.Add(filenameItem);
                    }
                    //listboxFiles.Width = lstwidth * 10;
                    bool isValuation = false;
                    if (Request.QueryString["valuation"] != null)
                        isValuation = System.Convert.ToBoolean(Request.QueryString["valuation"]);

                    if (isValuation)
                    {
                        buttonLoad.Text = "Show MF Portfolio Valuation";
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
            if (ddlPortfolios.SelectedIndex > 0)
            {
                Session["PortfolioNameMF"] = ddlPortfolios.SelectedValue;
                Session["ShortPortfolioNameMF"] = ddlPortfolios.SelectedItem.Text;
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
                    string url = "~/portfoliovaluationMF.aspx" + "?";

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