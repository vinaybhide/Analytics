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
                return labelSelectedSymbol.Text;
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
                Response.Redirect(".\\Default.aspx");
            }
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextBoxSearch.Text.Length > 0)
            {
                DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text);

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
                    Response.Write("<script language=javascript>alert('No matching symbols found')</script>");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('Enter text in Search Stock to search stock symbol')</script>");
            }
        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
        }
        protected void buttonCalCost_Click(object sender, EventArgs e)
        {
            double purchasePrice = (double)System.Convert.ToDouble(textboxPurchasePrice.Text);
            int purchaseQty = (int)System.Convert.ToInt32(textboxQuantity.Text);
            double commissionPaid = (double)System.Convert.ToDouble(textboxCommission.Text);

            double totalCost = (purchasePrice * purchaseQty) + commissionPaid;

            labelTotalCost.Text = System.Convert.ToString(totalCost);
        }
        protected void buttonAddStock_Click(object sender, EventArgs e)
        {
            if (labelSelectedSymbol.Text.Length > 0)
            {
                buttonCalCost_Click(null, null);
                //Server.Transfer("~/openportfolio.aspx");
                StockApi.insertNode(Session["PortfolioName"].ToString(), Symbol, PurchasePrice, PurchaseDate, PurchaseQty, CommissionPaid, TotalCost);
                if(this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect(".\\openportfolio.aspx");
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    Response.Redirect(".\\mopenportfolio.aspx");
                else
                    Response.Redirect(".\\openportfolio.aspx");
            }
            else
            {
                Response.Write("<script language=javascript>alert('Please search & then select script to add.')</script>");
            }
        }
        protected void buttonBack_Click(object sender, EventArgs e)
        {
            if (this.MasterPageFile.Contains("Site.Master"))
                Response.Redirect(".\\openportfolio.aspx");
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect(".\\mopenportfolio.aspx");
            else
                Response.Redirect(".\\openportfolio.aspx");
        }


    }
}