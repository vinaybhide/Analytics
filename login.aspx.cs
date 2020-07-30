using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PortfolioFolder"] = null;
            Session["EmailId"] = null;
            Session["TestDataFolder"] = null;
            Session["IsTestOn"] = null;
            Session["PortfolioName"] = null;
            Session["ShortPortfolioName"] = null;
            Session["ScriptName"] = null;
            
            
            //Master.UserID = "";
            //Master.Portfolio = "";

            if (!IsPostBack)
            {
                textboxEmail.Text = "";
                textboxPwd.Text = "";
            }

        }

        protected void mbuttonLogin_Click(object sender, EventArgs e)
        {
            string emailId = textboxEmail.Text;
            string pwd = textboxPwd.Text;

            if ((emailId.Length > 0) && (pwd.Length >0))
            {
                string folderPath = Server.MapPath("~/portfolio/") + emailId + "_" + pwd;
                string testDatafolderPath = Server.MapPath("~/portfolio/") + emailId + "_" + pwd + "\\" + "ScriptData\\";
                
                if ((Directory.Exists(folderPath) == true) && (Directory.Exists(testDatafolderPath) == true))
                {
                    Session["EmailId"] = textboxEmail.Text;
                    Session["PortfolioFolder"] = folderPath;
                    Session["TestDataFolder"] = testDatafolderPath;
                    Session["IsTestOn"] = checkboxTestMode.Checked;


                    //Master.UserID = textboxEmail.Text;

                    //Master.enableDisableDownloadDataMenu(checkboxTestMode.Checked);

                    if (Directory.GetFiles(folderPath).Length > 0)
                    {
                        if(this.MasterPageFile.Contains("Site.Master"))
                            Response.Redirect(".\\selectportfolio.aspx");
                        else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                            Response.Redirect(".\\mselectportfolio.aspx");
                        else
                            Response.Redirect(".\\selectportfolio.aspx");
                    }
                    else
                    {
                        if(this.MasterPageFile.Contains("Site.Master"))
                            Response.Redirect(".\\newportfolio.aspx");
                        else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                            Response.Redirect(".\\mnewportfolio.aspx");
                        else
                            Response.Redirect(".\\newportfolio.aspx");
                        //Response.Redirect(".\\Default.aspx");
                    }
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Incorrect Email Id or Password')</script>");
                }
            }

        }

        protected void mbuttonRegister_Click(object sender, EventArgs e)
        {
            string emailId = textboxEmail.Text;
            if ((emailId.Length > 0) && (textboxPwd.Text.Length >0))
            {
                string folderPath = Server.MapPath("~/portfolio/") + emailId + "_" + textboxPwd.Text;
                string testDatafolderPath = Server.MapPath("~/portfolio/") + emailId + "_" + textboxPwd.Text + "\\" + "ScriptData\\";
                if (Directory.Exists(folderPath) == false)
                {
                    Directory.CreateDirectory(folderPath);
                    if (Directory.Exists(testDatafolderPath) == false)
                    {
                        Directory.CreateDirectory(testDatafolderPath);
                    }
                    //Server.Transfer("~/login.aspx");
                    Response.Redirect(".\\login.aspx");
                }
                else
                {
                    Response.Write("<script language=javascript>alert('User exists!')</script>");
                }
            }
        }
    }
}