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
    public partial class mdeleteportfolioMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if ((Session["EmailId"] != null) || (Session["PortfolioFolderMF"] != null))
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    //string folder = Session["PortfolioFolder"].ToString();
                    //string[] filelist = Directory.GetFiles(folder, "*.mfl");

                    DataTable portfolioTable = DataManager.getPortfolioTable(Session["emailid"].ToString());
                    if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                    {
                        ddlFiles.DataTextField = "PORTFOLIO_NAME";
                        ddlFiles.DataValueField = "PORTFOLIO_NAME";
                        ddlFiles.DataSource = portfolioTable;
                        ddlFiles.DataBind();
                    }

                    ListItem li = new ListItem("Select Portfolio", "-1");
                    ddlFiles.Items.Insert(0, li);

                    //foreach (string filename in filelist)
                    //{
                    //    string portfolioName = filename.Remove(0, filename.LastIndexOf('\\') + 1);
                    //    ListItem filenameItem = new ListItem(portfolioName, filename);
                    //    ddlFiles.Items.Add(filenameItem);
                    //}
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
            string deletePortfolioName = ddlFiles.SelectedValue;
            //if (deletePortfolioName.Equals("-1") == false)
            //if (ddlFiles.SelectedIndex > 0)
            if (ddlFiles.SelectedValue.Equals("-1") == false)
            {
                DataManager.deletePortfolio(Session["emailid"].ToString(), ddlFiles.SelectedValue);
                DataTable portfolioTable = DataManager.getPortfolioTable(Session["emailid"].ToString());

                if((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                {
                    Response.Redirect("~/mselectportfolioMF.aspx");
                }
                else
                {
                    Response.Redirect("~/mnewportfolioMF.aspx");
                }

                //string folder = Session["PortfolioFolderMF"].ToString();

                //File.Delete(deletePortfolioName);
                //Session["PortfolioNameMF"] = null;
                //if ((Directory.GetFiles(folder, "*")).Length > 0)
                //{
                //    Response.Redirect("~/mselectportfolioMF.aspx");
                //}
                //else
                //{
                //    Response.Redirect("~/mnewportfolioMF.aspx");
                //}
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
                labelSelectedFile.Text = "Selected File: Please select valid portfolio to delete";
            }
            else
            {
                labelSelectedFile.Text = $"{"Selected portfolio:"}{ddlFiles.SelectedItem.Text}";
            }
        }
        protected void buttonBack_Click(object sender, EventArgs e)
        {
            string folder = Session["PortfolioFolderMF"].ToString();
            if ((Directory.GetFiles(folder, "*")).Length > 0)
            {
                //Server.Transfer("~/openportfolio.aspx");
                //if (this.MasterPageFile.Contains("Site.Master"))
                //    Response.Redirect("~/selectportfolio.aspx");
                //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                //    Response.Redirect("~/mselectportfolio.aspx");
                //else
                    Response.Redirect("~/mselectportfolioMF.aspx");
            }
            else
            {
                //if (this.MasterPageFile.Contains("Site.Master"))
                //    Response.Redirect("~/newportfolio.aspx");
                //else if (this.MasterPageFile.Contains("Site.Master"))
                //    Response.Redirect("~/mnewportfolio.aspx");
                //else
                    Response.Redirect("~/mnewportfolioMF.aspx");
            }
        }

    }
}