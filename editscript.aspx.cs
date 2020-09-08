using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class editscript : System.Web.UI.Page
    {
        public string Symbol
        {
            get
            {
                return textboxSymbol.Text.Trim();
            }
        }

        public string CompanyName
        {
            get
            {
                return textboxCompaName.Text.Trim();
            }
        }

        public string PurchasePrice
        {
            get
            {
                return textboxPurchasePrice.Text.Trim();
            }
        }

        public string PurchaseDate
        {
            get
            {
                return textboxPurchaseDate.Text.ToString();
            }
        }

        public string PurchaseQty
        {
            get
            {
                return textboxQuantity.Text.Trim();
            }
        }

        public string CommissionPaid
        {
            get
            {
                return textboxCommission.Text.Trim();
            }
        }
        public string TotalCost
        {
            get
            {
                return labelTotalCost.Text.Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortfolioName"] != null)
            {
                //Master.Portfolio = Session["PortfolioName"].ToString();
                if (!IsPostBack)
                {
                    if (Request.QueryString.Count > 0)
                    {
                        textboxSymbol.Text = Request.QueryString["symbol"].ToString();
                        textboxCompaName.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["companyname"].ToString());
                        textboxPurchasePrice.Text = Request.QueryString["price"].ToString();
                        textboxPurchaseDate.Text = Request.QueryString["date"].ToString();
                        textboxQuantity.Text = Request.QueryString["qty"].ToString();
                        textboxCommission.Text = Request.QueryString["comission"].ToString();
                        labelTotalCost.Text = Request.QueryString["cost"].ToString();
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

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            bool breturn = false;
            if (textboxSymbol.Text.Length > 0 && textboxPurchaseDate.Text.Length > 0 && textboxPurchasePrice.Text.Length > 0 &&
                    textboxQuantity.Text.Length > 0 && textboxCommission.Text.Length > 0 && labelTotalCost.Text.Length > 0 &&
                    textboxCompaName.Text.Length > 0)

            {
                buttonCalCost_Click(null, null);
                //Server.Transfer("~/openportfolio.aspx");
                try
                {
                    breturn = StockApi.updateNode(Session["PortfolioName"].ToString(), Request.QueryString["symbol"].ToString(), 
                        Request.QueryString["price"].ToString(), Request.QueryString["date"].ToString(),
                        Request.QueryString["qty"].ToString(), Request.QueryString["comission"].ToString(),
                        Request.QueryString["cost"].ToString(), Request.QueryString["companyname"].ToString(),
                        Symbol, PurchasePrice, PurchaseDate, PurchaseQty, CommissionPaid, TotalCost, CompanyName);
                }
                catch (Exception ex)
                {
                    //Response.Write("<script language=javascript>alert('" + msg + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
                }
                if (breturn)
                {
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/openportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Response.Redirect("~/mopenportfolio.aspx");
                    else
                        Response.Redirect("~/mopenportfolio.aspx");
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

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            if (this.MasterPageFile.Contains("Site.Master"))
                Response.Redirect("~/openportfolio.aspx");
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect("~/mopenportfolio.aspx");
            else
                Response.Redirect("~/mopenportfolio.aspx");
        }

        protected void buttonCalCost_Click(object sender, EventArgs e)
        {
            if (textboxPurchasePrice.Text.Length > 0 && textboxQuantity.Text.Length > 0 && textboxCommission.Text.Length > 0)
            {
                double purchasePrice = (double)System.Convert.ToDouble(textboxPurchasePrice.Text);
                int purchaseQty = (int)System.Convert.ToInt32(textboxQuantity.Text);
                double commissionPaid = (double)System.Convert.ToDouble(textboxCommission.Text);

                double totalCost = (purchasePrice * purchaseQty) + commissionPaid;

                labelTotalCost.Text = System.Convert.ToString(totalCost);
            }
            else
                labelTotalCost.Text = "0.00";

        }
    }
}