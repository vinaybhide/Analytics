using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            //if (Session["EMAILID"] != null)
            //{
            //    Master.UserID = Session["EMAILID"].ToString();
            //}

            if ((Session["STOCKPORTFOLIONAME"] != null) && (Session["STOCKPORTFOLIOMASTERROWID"] != null))
            {
                //Master.Portfolio = Session["STOCKPORTFOLIONAME"].ToString();
                if (!IsPostBack)
                {
                    ViewState["FetchedData"] = null;

                    if (Request.QueryString.Count > 0)
                    {
                        labelSelectedSymbol.Text = Request.QueryString["symbol"].ToString();
                        LabelCompanyName.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["companyname"].ToString());
                        textboxPurchasePrice.Text = Request.QueryString["price"].ToString();
                        ddlExchange.SelectedValue = Request.QueryString["exch"].ToString();

                        ViewState["StockPortfolioScriptId"] = Session["STOCKPORTFOLIOSCRIPTID"];
                    }

                    DropDownListStock.Items.Clear();

                    StockManager stockManager = new StockManager();

                    DataTable tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());

                    if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
                    {
                        ViewState["STOCKMASTER"] = tableStockMaster;
                        ListItem li = new ListItem("Select Stock or Company", "-1");
                        DropDownListStock.Items.Add(li);
                        foreach (DataRow rowitem in tableStockMaster.Rows)
                        {
                            li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString() + "." + ddlExchange.SelectedValue);//rowitem["EXCHANGE"].ToString());
                            DropDownListStock.Items.Add(li);
                        }
                    }
                    if (Request.QueryString.Count > 0)
                    {
                        DropDownListStock.SelectedValue = Request.QueryString["symbol"].ToString();
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
            if (ViewState["STOCKMASTER"] != null)
            {
                DataTable stockMaster = (DataTable)ViewState["STOCKMASTER"];
                if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                {
                    StringBuilder filter = new StringBuilder();
                    if (!(string.IsNullOrEmpty(TextBoxSearch.Text)))
                        filter.Append("COMP_NAME Like '%" + TextBoxSearch.Text + "%'");
                    DataView dv = stockMaster.DefaultView;
                    dv.RowFilter = filter.ToString();
                    if (dv.Count > 0)
                    {
                        DropDownListStock.Items.Clear();
                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Add(li);
                        foreach (DataRow rowitem in dv.ToTable().Rows)
                        {
                            li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString() + "." + ddlExchange.SelectedValue);//rowitem["EXCHANGE"].ToString());
                            DropDownListStock.Items.Add(li);
                        }
                    }
                    else
                    {
                        //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                    }

                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }
        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStock.SelectedValue.Equals("-1") == false)
            {
                labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                LabelCompanyName.Text = DropDownListStock.SelectedItem.Text.Trim();
                textboxQuantity.Text = "0.00";
                textboxCommission.Text = "0.00";
                labelTotalCost.Text = "0.00";
                StockManager stockManager = new StockManager();
                DataTable quoteTable = stockManager.GetQuote(DropDownListStock.SelectedValue);
                if ((quoteTable != null) && (quoteTable.Rows.Count > 0))
                {
                    textboxPurchaseDate.Text = quoteTable.Rows[0]["latestDay"].ToString();
                    textboxPurchasePrice.Text = quoteTable.Rows[0]["price"].ToString();

                    DataTable stockTable = stockManager.SearchStock(labelSelectedSymbol.Text.Split('.')[0], ddlExchange.SelectedValue);
                    if ((stockTable != null) && (stockTable.Rows.Count > 0))
                    {
                        ViewState["StockPortfolioScriptId"] = stockTable.Rows[0]["ROWID"];
                        //Session["STOCKPORTFOLIOSCRIPTID"] = stockTable.Rows[0]["ROWID"];
                    }
                    else
                    {
                        ViewState["StockMasterRowId"] = null;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Not able to fetch quote. Please try again later.');", true);
                }
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
            if (ViewState["StockPortfolioScriptId"] != null)
            {
                if (labelSelectedSymbol.Text.Length > 0 && textboxPurchaseDate.Text.Length > 0 && textboxPurchasePrice.Text.Length > 0 &&
                      textboxQuantity.Text.Length > 0 && textboxCommission.Text.Length > 0 && labelTotalCost.Text.Length > 0 &&
                      LabelCompanyName.Text.Length > 0)
                {
                    buttonCalCost_Click(null, null);
                    //Server.Transfer("~/openportfolio.aspx");
                    try
                    {
                        Session["STOCKPORTFOLIOSCRIPTID"] = ViewState["StockPortfolioScriptId"];
                        StockManager stockManager = new StockManager();
                        stockManager.insertNode(Session["STOCKPORTFOLIOMASTERROWID"].ToString(), Session["STOCKPORTFOLIOSCRIPTID"].ToString(), Symbol, PurchasePrice, PurchaseDate,
                            PurchaseQty, CommissionPaid, TotalCost);
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

        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListStock.Items.Clear();
            TextBoxSearch.Text = "";
            labelSelectedSymbol.Text = "";
            LabelCompanyName.Text = "";
            textboxQuantity.Text = "0.00";
            textboxCommission.Text = "0.00";
            labelTotalCost.Text = "0.00";
 
            StockManager stockManager = new StockManager();

            DataTable tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());

            if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
            {
                ViewState["STOCKMASTER"] = tableStockMaster;
                ListItem li = new ListItem("Select Stock", "-1");
                DropDownListStock.Items.Add(li);
                foreach (DataRow rowitem in tableStockMaster.Rows)
                {
                    li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString() + "." + ddlExchange.SelectedValue);//rowitem["EXCHANGE"].ToString());
                    DropDownListStock.Items.Add(li);
                }
            }
            else
            {
                ViewState["STOCKMASTER"] = null;
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }
        }
    }
}