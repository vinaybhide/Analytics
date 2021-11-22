using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            Session["EMAILID"] = null;
            Session["USERROWID"] = null;
            Session["DATAFOLDER"] = UserManager.GetDataFolder();

            //Stock Portfolio related
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIOROWID"] = null;
            Session["STOCKPORTFOLIOEXCHANGE"] = null;
            Session["STOCKPORTFOLIOSCRIPTNAME"] = null;
            Session["STOCKPORTFOLIOSCRIPTID"] = null;
            Session["STOCKPORTFOLIOCOMPNAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;

            //Session["STOCKMASTERROWID"] = null;

            //MF Portfolio related
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOROWID"] = null;
            Session["MFSELECTEDINDEXPORTFOLIO"] = null;

            Session["MFPORTFOLIOFUNDNAME"] = null;
            Session["MFPORTFOLIOFUNDHOUSECODE"] = null;
            Session["MFPORTFOLIOFUNDHOUSE"] = null;
            Session["MFPORTFOLIOSCHEMECODE"] = null;

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
                UserManager userManager = new UserManager();
                long usermaster_rowid = userManager.CheckUserExists(emailId, pwd);
                if(usermaster_rowid > 0)
                {
                    Session["EMAILID"] = emailId;
                    Session["USERROWID"] = usermaster_rowid;
                    Session["DATAFOLDER"] = UserManager.GetDataFolder();
                    StockManager stockManager = new StockManager();
                    if (stockManager.getPortfolioCount(emailId) > 0)
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
                    //Response.Write("<script language=javascript>alert('" + common.noUserMatch +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noUserMatch + "');", true);
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noUserMatch +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid email id & password to login or register!');", true);
            }

            //if ((emailId.Length > 0) && (pwd.Length >0))
            //{
            //    string folderPath = Server.MapPath("~/portfolio/" + emailId + "_" + pwd);
            //    //string testDatafolderPath = Server.MapPath("~/portfolio/") + emailId + "_" + pwd + "\\" + "ScriptData\\";
            //    //string testDatafolderPath = Server.MapPath("~/portfolio/" + emailId + "_" + pwd + "/" + "ScriptData/");
            //    string testDatafolderPath = Server.MapPath("~/portfolio/ScriptData/");
            //    string testDatafolderPathMF = Server.MapPath("~/portfolio/MFData/");
            //    if ((Directory.Exists(folderPath) == true) && (Directory.Exists(testDatafolderPath) == true) && 
            //        (Directory.Exists(testDatafolderPathMF) == true))
            //    {
            //        Session["EMAILID"] = textboxEmail.Text;
            //        Session["PortfolioFolder"] = folderPath;
            //        Session["PortfolioFolderMF"] = folderPath;
            //        Session["DATAFOLDER"] = testDatafolderPath;
            //        Session["TestDataFolderMF"] = testDatafolderPathMF;
            //        Session["IsTestOn"] = checkboxTestMode.Checked;
            //        string key = StockApi.readKey(folderPath + "\\" + emailId + ".key");
            //        if(key == null)
            //        {
            //            string fileName = folderPath + "\\" + emailId + ".key";
            //            StockApi.createKey(fileName, "UV6KQA6735QZKBTV");
            //            key = "UV6KQA6735QZKBTV";
            //        }
            //        Session["ApiKey"] = key;

            //        //Master.UserID = textboxEmail.Text;

            //        //Master.enableDisableDownloadDataMenu(checkboxTestMode.Checked);

            //        //if (Directory.GetFiles(folderPath, "*.xml").Length > 0)
            //        if(stockManager.getPortfolioCount(textboxEmail.Text) > 0)
            //        {
            //            //if(this.MasterPageFile.Contains("Site.Master"))
            //            //    Response.Redirect("~/mselectportfolio.aspx");
            //            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //            //    Response.Redirect("~/mselectportfolio.aspx");
            //            //else
            //                Response.Redirect("~/mselectportfolio.aspx");
            //        }
            //        else
            //        {
            //            //if(this.MasterPageFile.Contains("Site.Master"))
            //            //    Response.Redirect("~/mnewportfolio.aspx");
            //            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //            //    Response.Redirect("~/mnewportfolio.aspx");
            //            //else
            //                Response.Redirect("~/mnewportfolio.aspx");
            //            //Response.Redirect(".\\Default.aspx");
            //        }
            //    }
            //    else
            //    {
            //        //Response.Write("<script language=javascript>alert('" + common.noUserMatch +"')</script>");
            //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noUserMatch + "');", true);
            //    }
            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('" + common.noUserMatch +"')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid email id & password to login or register!');", true);
            //}

        }

        protected void mbuttonRegister_Click(object sender, EventArgs e)
        {
            string emailId = textboxEmail.Text;

            if ((emailId.Length > 0) && (textboxPwd.Text.Length >0))
            {
                UserManager userManager = new UserManager();
                if(userManager.CheckUserExists(emailId) <= 0)
                {
                    long usermaster_rowid = userManager.RegisterUser(emailId, textboxPwd.Text.ToString());
                    if(usermaster_rowid > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.registrationComplete + "');", true);

                        //treat this as new valid login
                        Session["EMAILID"] = emailId;
                        Session["USERROWID"] = usermaster_rowid;
                        Session["DATAFOLDER"] = UserManager.GetDataFolder();
                        StockManager stockManager = new StockManager();

                        if (stockManager.getPortfolioCount(emailId) > 0)
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
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Problem while registering user. Please try later.');", true);
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.userExists + "');", true);
                }

            }
        }
    }
}