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
            //if ((Session["EMAILID"] != null) || (Session["PortfolioFolderMF"] != null))
            Session["MFPORTFOLIOMASTERROWID"] = null;
            Session["MFPORTFOLIONAME"] = null;
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    //string folder = Session["PortfolioFolder"].ToString();
                    //string[] filelist = Directory.GetFiles(folder, "*.mfl");
                    DataManager dataMgr = new DataManager();
                    DataTable portfolioTable = dataMgr.getPortfolioTable(Session["EMAILID"].ToString());
                    if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                    {
                        ddlFiles.DataTextField = "PORTFOLIO_NAME";
                        ddlFiles.DataValueField = "ID";
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
            string deletePortfolioMasterRowId = ddlFiles.SelectedValue;
            //if (deletePortfolioName.Equals("-1") == false)
            //if (ddlFiles.SelectedIndex > 0)
            if (ddlFiles.SelectedValue.Equals("-1") == false)
            {
                DataManager dataMgr = new DataManager();
                dataMgr.deletePortfolio(Session["EMAILID"].ToString(), deletePortfolioMasterRowId);
                DataTable portfolioTable = dataMgr.getPortfolioTable(Session["EMAILID"].ToString());

                if((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                {
                    Response.Redirect("~/mselectportfolioMF.aspx");
                }
                else
                {
                    Response.Redirect("~/mnewportfolioMF.aspx");
                }
            }
            else
            {
                labelSelectedFile.Text = "Please select portfolio to delete";
                //Response.Write("<script language=javascript>alert('"+ common.noPortfolioSelectedToDelete +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelectedToDelete + "');", true);
            }
        }
        protected void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiles.SelectedValue.Equals("-1") == true)
            {
                labelSelectedFile.Text = "Please select valid portfolio to delete";
            }
            else
            {
                labelSelectedFile.Text = $"{"Selected portfolio:"}{ddlFiles.SelectedItem.Text}";
            }
        }
        protected void buttonBack_Click(object sender, EventArgs e)
        {
            DataManager dataMgr = new DataManager();
            DataTable portfolioTable = dataMgr.getPortfolioTable(Session["EMAILID"].ToString());

            if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
            {
                Response.Redirect("~/mselectportfolioMF.aspx");
            }
            else
            {
                Response.Redirect("~/mnewportfolioMF.aspx");
            }
        }

    }
}