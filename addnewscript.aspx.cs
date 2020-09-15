using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class addnewscript : System.Web.UI.Page
    {
        public string Symbol
        {
            get
            {
                return labelSelectedSymbol.Text.Trim();
            }
        }

        public string CompanyName
        {
            get
            {
                return LabelCompanyName.Text.Trim();
            }
        }

        public string PurchasePrice
        {
            get
            {
                return textboxPurchasePrice.Text;
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
                return textboxQuantity.Text;
            }
        }

        public string CommissionPaid
        {
            get
            {
                return textboxCommission.Text;
            }
        }
        public string TotalCost
        {
            get
            {
                return labelTotalCost.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EmailId"] != null)
            //{
            //    Master.UserID = Session["emailid"].ToString();
            //}

            if (Session["PortfolioName"] != null)
            {
                //Master.Portfolio = Session["PortfolioName"].ToString();
                if (!IsPostBack)
                {
                    DropDownListStock.Items.Clear();
                    if (Request.QueryString.Count > 0)
                    {
                        labelSelectedSymbol.Text = Request.QueryString["symbol"].ToString();
                        LabelCompanyName.Text = Request.QueryString["companyname"].ToString();
                        textboxPurchasePrice.Text = Request.QueryString["price"].ToString();

                        DropDownListStock.Items.Add(labelSelectedSymbol.Text);
                        DropDownListStock.SelectedIndex = 0;

                        TextBoxSearch.Enabled = false;
                        ButtonSearch.Enabled = false;
                        DropDownListStock.Enabled = false;
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
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextBoxSearch.Text.Length > 0)
            {
                //DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());
                DataTable resultTable = StockApi.symbolSearchAltername(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());
                if (resultTable != null)
                {
                    DropDownListStock.DataTextField = "Name";
                    DropDownListStock.DataValueField = "Symbol";
                    DropDownListStock.DataSource = resultTable;
                    DropDownListStock.DataBind();
                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);

                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noTextSearchSymbol + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTextSearchSymbol + "');", true);
            }
        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStock.SelectedValue.Equals("-1") == false)
            {
                labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                LabelCompanyName.Text = (DropDownListStock.SelectedItem.Text.Split(':')[1]).Trim();
            }
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
        protected void buttonAddStock_Click(object sender, EventArgs e)
        {
            if (labelSelectedSymbol.Text.Length > 0 && textboxPurchaseDate.Text.Length > 0 && textboxPurchasePrice.Text.Length > 0 &&
                    textboxQuantity.Text.Length > 0 && textboxCommission.Text.Length > 0 && labelTotalCost.Text.Length > 0 &&
                    LabelCompanyName.Text.Length > 0)

            {
                buttonCalCost_Click(null, null);
                //Server.Transfer("~/openportfolio.aspx");
                try
                {
                    StockApi.insertNode(Session["PortfolioName"].ToString(), Symbol, PurchasePrice, PurchaseDate, PurchaseQty, CommissionPaid, TotalCost, companyname:CompanyName);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    //Response.Write("<script language=javascript>alert('" + msg + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + msg + "');", true);

                }
                if (this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect("~/openportfolio.aspx");
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    Response.Redirect("~/mopenportfolio.aspx");
                else
                    Response.Redirect("~/mopenportfolio.aspx");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('Please make sure you have selected script and entered all information.')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptSelectedInformationEntered + "');", true);
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


    }
}