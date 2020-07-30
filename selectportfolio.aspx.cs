using System;
using System.Collections.Generic;
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
            //if (Session["EmailId"] != null)
            //{
            //    Master.UserID = Session["emailid"].ToString();
            //}

            //if (Session["PortfolioName"] != null)
            //{
            //    Master.Portfolio = Session["PortfolioName"].ToString();
            //}

            if (Session["PortfolioFolder"] != null)
            {
                if (!IsPostBack)
                {
                    string folder = Session["PortfolioFolder"].ToString();
                    string[] filelist = Directory.GetFiles(folder, "*");

                    int lstwidth = 0;

                    ListItem li = new ListItem("Select Portfolio", "-1");
                    ddlPortfolios.Items.Insert(0, li);

                    foreach (string filename in filelist)
                    {
                        string portfolioName = filename.Remove(0, filename.LastIndexOf('\\') + 1);
                        ListItem filenameItem = new ListItem(portfolioName, filename);
                        ddlPortfolios.Items.Add(filenameItem);
                    }
                    //listboxFiles.Width = lstwidth * 10;
                }
            }
            else
            {
                Response.Redirect(".\\Default.aspx");
            }
        }
        protected void buttonLoad_Click(object sender, EventArgs e)
        {
            //string selectedFile = listboxFiles.SelectedValue;
            if (ddlPortfolios.SelectedIndex > 0)
            {
                Session["PortfolioName"] = ddlPortfolios.SelectedValue;
                Session["ShortPortfolioName"] = ddlPortfolios.SelectedItem.Text;
                //Server.Transfer("~/openportfolio.aspx");
                if(this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect(".\\openportfolio.aspx");
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    Response.Redirect(".\\mopenportfolio.aspx");
                else
                    Response.Redirect(".\\openportfolio.aspx");
            }
            else
            {
                labelSelectedFile.Text = "Selected Portfolio: Please select valid portfolio to open";
                Response.Write("<script language=javascript>alert('Please select valid portfolio to open.')</script>");
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