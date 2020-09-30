using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class meditmftrans : System.Web.UI.Page
    {
        public string FundHouse
        {
            get
            {
                return textboxFundHouse.Text.Trim();
            }
        }

        public string FundNameSelected
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundName.SelectedValue;
            }
        }

        public string FundName
        {
            get
            {
                return textboxFundName.Text.Trim();
            }
        }

        public string SchemeCode
        {
            get
            {
                return textboxSchemeCode.Text.Trim();
            }
        }

        public string PurchaseDate
        {
            get
            {
                return System.Convert.ToDateTime(textboxPurchaseDate.Text.Trim()).ToShortDateString();
            }
        }
        public string PurchaseUnits
        {
            get
            {
                return textboxUnits.Text.Trim();
            }
        }
        public string PurchaseNAV
        {
            get
            {
                return textboxPurchaseNAV.Text.Trim();
            }
        }
        public string ValueAtCost
        {
            get
            {
                return textboxValueAtCost.Text.Trim();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortfolioNameMF"] != null)
            {
                //Master.Portfolio = Session["PortfolioName"].ToString();
                if (!IsPostBack)
                {
                    if (Request.QueryString.Count > 0)
                    {
                        string folderPath = Server.MapPath("~/mfdata/");
                        if (Session["TestDataFolderMF"] != null)
                        {
                            folderPath = Session["TestDataFolderMF"].ToString();
                        }

                        textboxFundHouse.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["fundhouse"].ToString());
                        textboxFundName.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["fundname"].ToString());
                        
                        DataTable fundNameTable = MFAPI.getALLMFforFundHouse(folderPath, FundHouse, bExactMatch: true);

                        ddlFundName.DataTextField = "SCHEME_NAME";
                        ddlFundName.DataValueField = "SCHEME_NAME";
                        ddlFundName.DataSource = fundNameTable;
                        ddlFundName.DataBind();
                        ListItem li = new ListItem("Select Fund to change", "-1");
                        ddlFundName.Items.Insert(0, li);

                        textboxSchemeCode.Text = Request.QueryString["schemecode"].ToString();
                        textboxPurchaseDate.Text = System.Convert.ToDateTime(Request.QueryString["purchasedate"].ToString()).ToString("yyyy-MM-dd");
                        textboxPurchaseNAV.Text = Request.QueryString["purchasenav"].ToString();
                        textboxUnits.Text = Request.QueryString["purchaseunits"].ToString();
                        textboxValueAtCost.Text = Request.QueryString["valueatcost"].ToString();
                    }
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noPortfolioName + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioName + "');", true);
                //Response.Redirect(".\\Default.aspx");
                Response.Redirect("~/Default.aspx");
            }

        }

        protected void textboxUnits_TextChanged(object sender, EventArgs e)
        {
            if (textboxPurchaseNAV.Text.Length > 0 && textboxUnits.Text.Length > 0)
            {
                double purchaseNAV = (double)System.Convert.ToDouble(textboxPurchaseNAV.Text);
                double purchaseUnits = (int)System.Convert.ToDouble(textboxUnits.Text);

                double valueatcost = (purchaseNAV * purchaseUnits);

                textboxValueAtCost.Text = System.Convert.ToString(valueatcost);
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid NAV and Unit values');", true);
        }

        protected void textboxPurchaseNAV_TextChanged(object sender, EventArgs e)
        {
            if (textboxPurchaseNAV.Text.Length > 0 && textboxUnits.Text.Length > 0)
            {
                double purchaseNAV = (double)System.Convert.ToDouble(textboxPurchaseNAV.Text);
                double purchaseUnits = (int)System.Convert.ToDouble(textboxUnits.Text);

                double valueatcost = (purchaseNAV * purchaseUnits);

                textboxValueAtCost.Text = System.Convert.ToString(valueatcost);
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid NAV and Unit values');", true);
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            //if (this.MasterPageFile.Contains("Site.Master"))
            //    Response.Redirect("~/openportfolio.aspx");
            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //    Response.Redirect("~/mopenportfolio.aspx");
            //else
                Response.Redirect("~/mopenportfolioMF.aspx");
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            bool breturn = false;
            if (textboxPurchaseDate.Text.Length > 0 && textboxPurchaseNAV.Text.Length > 0 &&
                    textboxUnits.Text.Length > 0 && textboxValueAtCost.Text.Length > 0)

            {
                textboxPurchaseNAV_TextChanged(null, null);
                //Server.Transfer("~/openportfolio.aspx");
                try
                {
                    breturn = MFAPI.updateTransaction(Session["PortfolioNameMF"].ToString(), Request.QueryString["fundhouse"].ToString(),
                        Request.QueryString["fundname"].ToString(), Request.QueryString["schemecode"].ToString(),
                        Request.QueryString["purchasedate"].ToString(), Request.QueryString["purchasenav"].ToString(), 
                        Request.QueryString["purchaseunits"].ToString(), Request.QueryString["valueatcost"].ToString(),
                        FundName, PurchaseDate, PurchaseNAV, PurchaseUnits, ValueAtCost);
                }
                catch (Exception ex)
                {
                    //Response.Write("<script language=javascript>alert('" + msg + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
                }
                if (breturn)
                {
                    //if (this.MasterPageFile.Contains("Site.Master"))
                    //    Response.Redirect("~/openportfolio.aspx");
                    //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    //    Response.Redirect("~/mopenportfolio.aspx");
                    //else
                        Response.Redirect("~/mopenportfolioMF.aspx");
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('Error while updating the transaction. Please try again or hit back.')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.errorEditScript + "');", true);
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('All fields are mandatory.')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.errorAllFieldsMandatory + "');", true);

            }
        }

        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundNameSelected != "-1")
            {
                textboxFundName.Text = FundNameSelected;
            }
        }
    }
}