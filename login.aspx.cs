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

        protected void Pre_Init(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["EmailId"] = null;
            Session["IsTestOn"] = null;
            Session["ApiKey"] = null;

            Session["PortfolioFolder"] = null;
            Session["PortfolioName"] = null;
            Session["ShortPortfolioName"] = null;
            Session["TestDataFolder"] = null;
            Session["ScriptName"] = null;

            Session["PortfolioFolderMF"] = null;
            Session["PortfolioNameMF"] = null;
            Session["ShortPortfolioNameMF"] = null;
            Session["PortfolioRowId"] = null;
            Session["TestDataFolderMF"] = null;
            Session["MFName"] = null;


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
                string folderPath = Server.MapPath("~/portfolio/" + emailId + "_" + pwd);
                //string testDatafolderPath = Server.MapPath("~/portfolio/") + emailId + "_" + pwd + "\\" + "ScriptData\\";
                //string testDatafolderPath = Server.MapPath("~/portfolio/" + emailId + "_" + pwd + "/" + "ScriptData/");
                string testDatafolderPath = Server.MapPath("~/portfolio/ScriptData/");
                string testDatafolderPathMF = Server.MapPath("~/portfolio/MFData/");
                if ((Directory.Exists(folderPath) == true) && (Directory.Exists(testDatafolderPath) == true) && 
                    (Directory.Exists(testDatafolderPathMF) == true))
                {
                    Session["EmailId"] = textboxEmail.Text;
                    Session["PortfolioFolder"] = folderPath;
                    Session["PortfolioFolderMF"] = folderPath;
                    Session["TestDataFolder"] = testDatafolderPath;
                    Session["TestDataFolderMF"] = testDatafolderPathMF;
                    Session["IsTestOn"] = checkboxTestMode.Checked;
                    string key = StockApi.readKey(folderPath + "\\" + emailId + ".key");
                    if(key == null)
                    {
                        string fileName = folderPath + "\\" + emailId + ".key";
                        StockApi.createKey(fileName, "UV6KQA6735QZKBTV");
                        key = "UV6KQA6735QZKBTV";
                    }
                    Session["ApiKey"] = key;

                    //Master.UserID = textboxEmail.Text;

                    //Master.enableDisableDownloadDataMenu(checkboxTestMode.Checked);

                    if (Directory.GetFiles(folderPath, "*.xml").Length > 0)
                    {
                        //if(this.MasterPageFile.Contains("Site.Master"))
                        //    Response.Redirect("~/mselectportfolio.aspx");
                        //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        //    Response.Redirect("~/mselectportfolio.aspx");
                        //else
                            Response.Redirect("~/mselectportfolio.aspx");
                    }
                    else
                    {
                        //if(this.MasterPageFile.Contains("Site.Master"))
                        //    Response.Redirect("~/mnewportfolio.aspx");
                        //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        //    Response.Redirect("~/mnewportfolio.aspx");
                        //else
                            Response.Redirect("~/mnewportfolio.aspx");
                        //Response.Redirect(".\\Default.aspx");
                    }
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.noUserMatch +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noUserMatch + "');", true);
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noUserMatch +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid email id & password to login or register!');", true);
            }
        }

        protected void mbuttonRegister_Click(object sender, EventArgs e)
        {
            string emailId = textboxEmail.Text;
            if ((emailId.Length > 0) && (textboxPwd.Text.Length >0))
            {
                //string folderPath = Server.MapPath("~/portfolio/") + emailId + "_" + textboxPwd.Text;
                string folderPath = Server.MapPath("~/portfolio/" + emailId + "_" + textboxPwd.Text);
                //string testDatafolderPath = Server.MapPath("~/portfolio/" + emailId + "_" + textboxPwd.Text + "/" + "ScriptData/");
                //string testDatafolderPathMF = Server.MapPath("~/portfolio/" + emailId + "_" + textboxPwd.Text + "/" + "MFData/");
                if (Directory.Exists(folderPath) == false)
                {
                    Directory.CreateDirectory(folderPath);
                    //if (Directory.Exists(testDatafolderPath) == false)
                    //{
                    //    Directory.CreateDirectory(testDatafolderPath);
                    //}
                    //create temp key
                    string fileName = folderPath + "\\" + emailId + ".key";
                    StockApi.createKey(fileName, "UV6KQA6735QZKBTV");
                    //Response.Write("<script language=javascript>alert('Registration complete with free Alpha Vantage API key. You can now login to application. Free Alpha Vantage key has limitations. Please use Admin->Add Key to add your AlphaVantage API key.')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.registrationComplete + "');", true);


                    Session["PortfolioFolder"] = null;
                    Session["EmailId"] = null;
                    Session["TestDataFolder"] = null;
                    Session["TestDataFolderMF"] = null;
                    Session["IsTestOn"] = null;
                    Session["PortfolioName"] = null;
                    Session["ShortPortfolioName"] = null;
                    Session["ScriptName"] = null;
                    Session["MFName"] = null;
                    Session["ApiKey"] = null;

                    //Server.Transfer("~/login.aspx");
                    //Response.Redirect("~/login.aspx");
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.userExists + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.userExists + "');", true);
                }
            }
        }
    }
}