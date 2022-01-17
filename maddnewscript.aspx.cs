using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Data;
using System.Text;

namespace Analytics
{
    public partial class maddnewscript : System.Web.UI.Page
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
                //for datetimelocal text mode use following to read the value from texbox
                return System.Convert.ToDateTime(textboxPurchaseDate.Text.ToString()).ToString();
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
                    FillExchangeList();

                    if (Request.QueryString.Count > 0)
                    {
                        if (Request.QueryString["symbol"] != null)
                            labelSelectedSymbol.Text = Request.QueryString["symbol"].ToString();

                        if (Request.QueryString["companyname"] != null)
                            LabelCompanyName.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["companyname"].ToString());

                        //when you want to set the textbox with textmode = datetimelocal use following format
                        //textboxPurchaseDate.Text = System.Convert.ToDateTime(Request.QueryString["date"].ToString()).ToString("yyyy-MM-ddThh:mm:ss");
                        if (Request.QueryString["date"] != null)
                            textboxPurchaseDate.Text = System.Convert.ToDateTime(Request.QueryString["date"].ToString()).ToString("yyyy-MM-dd");

                        if (Request.QueryString["price"] != null)
                            textboxPurchasePrice.Text = Request.QueryString["price"].ToString();

                        if (Request.QueryString["exch"] != null)
                            ddlExchange.SelectedValue = Request.QueryString["exch"].ToString();

                        ViewState["StockPortfolioScriptId"] = Session["STOCKPORTFOLIOSCRIPTID"];
                    }
                    else
                    {
                        //textboxPurchaseDate.Text = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                        textboxPurchaseDate.TextMode = TextBoxMode.Date;
                        textboxPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    }

                    DropDownListStock.Items.Clear();

                    StockManager stockManager = new StockManager();
                    DataTable tableStockMaster;
                    if (ddlExchange.SelectedIndex == 0)
                    {
                        tableStockMaster = stockManager.getStockMaster();
                    }
                    else
                    {
                        tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());
                    }

                    if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
                    {
                        ViewState["STOCKMASTER"] = tableStockMaster;

                        DropDownListStock.Items.Clear();
                        DropDownListStock.DataTextField = "COMP_NAME";
                        DropDownListStock.DataValueField = "SYMBOL";
                        DropDownListStock.DataSource = tableStockMaster;
                        DropDownListStock.DataBind();
                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Insert(0, li);
                    }
                    if (Request.QueryString["symbol"] != null)
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

        public void FillExchangeList()
        {
            StockManager stockManager = new StockManager();
            DataTable tableExchange = stockManager.GetExchangeList();
            if ((tableExchange != null) && (tableExchange.Rows.Count > 0))
            {
                ddlExchange.Items.Clear();
                ddlExchange.DataTextField = "EXCHANGE";
                ddlExchange.DataValueField = "EXCHANGE";
                ddlExchange.DataSource = tableExchange;
                ddlExchange.DataBind();
                ListItem li = new ListItem("Filter By Exchange", "-1");
                ddlExchange.Items.Insert(0, li);
                ddlExchange.SelectedIndex = 0;
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            bool bfound = false;
            DataTable stockMaster = null;
            if (ViewState["STOCKMASTER"] != null)
            {
                stockMaster = (DataTable)ViewState["STOCKMASTER"];
                if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                {
                    StringBuilder filter = new StringBuilder();
                    if (!(string.IsNullOrEmpty(TextBoxSearch.Text.ToUpper())))
                        filter.Append("COMP_NAME Like '%" + TextBoxSearch.Text.ToUpper() + "%'");
                    DataView dv = stockMaster.DefaultView;
                    dv.RowFilter = filter.ToString();
                    if (dv.Count > 0)
                    {
                        DropDownListStock.Items.Clear();
                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Add(li);
                        foreach (DataRow rowitem in dv.ToTable().Rows)
                        {
                            li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                            DropDownListStock.Items.Add(li);
                        }
                        bfound = true;
                    }
                    else
                    {
                        bfound = false;
                        //Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                    }

                }
            }
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            //}

            if (bfound == false)
            {
                StockManager stockManager = new StockManager();
                //try to see if this is new stock that we do not have in DB
                DataTable tableSearch = stockManager.SearchStock(TextBoxSearch.Text.ToUpper().ToUpper());
                if ((tableSearch == null) || (tableSearch.Rows.Count <= 0))
                {
                    //try to add new stock
                    tableSearch = stockManager.InsertNewStockIfNotFoundInDB(TextBoxSearch.Text.ToUpper());
                    if ((tableSearch != null) && (tableSearch.Rows.Count > 0))
                    {
                        stockMaster = stockManager.getStockMaster(tableSearch.Rows[0]["EXCHANGE"].ToString());
                        ViewState["STOCKMASTER"] = stockMaster;
                        DropDownListStock.Items.Clear();
                        DropDownListStock.DataTextField = "COMP_NAME";
                        DropDownListStock.DataValueField = "SYMBOL";
                        DropDownListStock.DataSource = stockMaster;
                        DropDownListStock.DataBind();


                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Insert(0, li);
                        DropDownListStock.SelectedValue = TextBoxSearch.Text.ToUpper();
                        FillExchangeList();

                        ddlExchange.SelectedValue = tableSearch.Rows[0]["EXCHANGE"].ToString();
                        StockSelectedAction();
                        bfound = true;
                    }
                }
                else
                {
                    //we found stock
                    stockMaster = stockManager.getStockMaster(tableSearch.Rows[0]["EXCHANGE"].ToString());
                    ViewState["STOCKMASTER"] = stockMaster;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = stockMaster;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                    DropDownListStock.SelectedValue = TextBoxSearch.Text.ToUpper();
                    FillExchangeList();

                    ddlExchange.SelectedValue = tableSearch.Rows[0]["EXCHANGE"].ToString();
                    StockSelectedAction();
                    bfound = true;

                }

            }

            if (bfound == false)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedIndex > 0)
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
                    //textboxPurchaseDate.Text = System.Convert.ToDateTime(quoteTable.Rows[0]["latestDay"].ToString()).ToString("yyyy-MM-ddThh:mm:ss");
                    textboxPurchaseDate.Text = System.Convert.ToDateTime(quoteTable.Rows[0]["latestDay"].ToString()).ToString("yyyy-MM-dd");
                    textboxPurchasePrice.Text = quoteTable.Rows[0]["price"].ToString();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Not able to fetch quote at this moment. You can enter date & price manually or please try again later.');", true);
                    textboxPurchaseDate.TextMode = TextBoxMode.Date;
                }

                DataTable stockTable = stockManager.SearchStock(labelSelectedSymbol.Text);
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
        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
        }
        protected void buttonCalCost_Click(object sender, EventArgs e)
        {
            if (textboxPurchasePrice.Text.Length > 0 && textboxQuantity.Text.Length > 0 && textboxCommission.Text.Length > 0)
            {
                double purchasePrice = (double)System.Convert.ToDouble(textboxPurchasePrice.Text);
                int purchaseQty = (int)System.Convert.ToInt32(textboxQuantity.Text);
                double commissionPaid = (double)System.Convert.ToDouble(textboxCommission.Text);

                double totalCost = (purchasePrice + commissionPaid) * purchaseQty;

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
            if (ddlExchange.SelectedIndex > 0)
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
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = tableStockMaster;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);

                }
                else
                {
                    ViewState["STOCKMASTER"] = null;
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
        }
    }
}