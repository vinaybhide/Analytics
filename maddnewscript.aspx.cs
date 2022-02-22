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
                    ViewState["STOCKMASTER"] = null;
                    FillExchangeList();
                    FillInvestmentTypeList();
                    FillSymbolList();

                    if (Request.QueryString.Count > 0)
                    {
                        if (Request.QueryString["symbol"] != null)
                        {
                            DropDownListStock.SelectedValue = Request.QueryString["symbol"].ToString();
                            labelSelectedSymbol.Text = Request.QueryString["symbol"].ToString();
                        }

                        if (Request.QueryString["exch"] != null)
                            ddlExchange.SelectedValue = Request.QueryString["exch"].ToString();

                        if (Request.QueryString["type"] != null)
                            ddlInvestmentType.SelectedValue = Request.QueryString["type"].ToString();

                        if (Request.QueryString["companyname"] != null)
                            LabelCompanyName.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["companyname"].ToString());

                        //when you want to set the textbox with textmode = datetimelocal use following format
                        //textboxPurchaseDate.Text = System.Convert.ToDateTime(Request.QueryString["date"].ToString()).ToString("yyyy-MM-ddThh:mm:ss");
                        if (Request.QueryString["date"] != null)
                            textboxPurchaseDate.Text = System.Convert.ToDateTime(Request.QueryString["date"].ToString()).ToString("yyyy-MM-dd");

                        if (Request.QueryString["price"] != null)
                            textboxPurchasePrice.Text = Request.QueryString["price"].ToString();


                        ViewState["StockPortfolioScriptId"] = Session["STOCKPORTFOLIOSCRIPTID"];
                    }
                    else
                    {
                        //textboxPurchaseDate.Text = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                        textboxPurchaseDate.TextMode = TextBoxMode.Date;
                        textboxPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
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

        public void FillInvestmentTypeList()
        {
            StockManager stockManager = new StockManager();
            DataTable tableInvestmentType = stockManager.GetInvestmentTypeList();
            if ((tableInvestmentType != null) && (tableInvestmentType.Rows.Count > 0))
            {
                ddlInvestmentType.Items.Clear();
                ddlInvestmentType.DataTextField = "SERIES";
                ddlInvestmentType.DataValueField = "SERIES";
                ddlInvestmentType.DataSource = tableInvestmentType;
                ddlInvestmentType.DataBind();
                ListItem li = new ListItem("Filter By Investment Type", "-1");
                ddlInvestmentType.Items.Insert(0, li);
                ddlInvestmentType.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Gets full list of symbols from STOCKMASTER and populates the Drop Down with additional "-1" entry
        /// </summary>
        public void FillSymbolList()
        {
            DropDownListStock.Items.Clear();

            StockManager stockManager = new StockManager();
            DataTable tableStockMaster;

            tableStockMaster = stockManager.getStockMaster();

            if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
            {
                ViewState["STOCKMASTER"] = tableStockMaster;
                DropDownListStock.Items.Clear();
                DropDownListStock.DataTextField = "COMP_NAME";
                DropDownListStock.DataValueField = "SYMBOL";
                DropDownListStock.DataSource = tableStockMaster;
                DropDownListStock.DataBind();
                ListItem li = new ListItem("Select Investment", "-1");
                DropDownListStock.Items.Insert(0, li);
            }
        }

        public bool SearchPopulateStocksDropDown(string searchStr)
        {
            bool breturn = false;
            DataTable stockMaster;
            try
            {
                if (ViewState["STOCKMASTER"] != null)
                {
                    stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();
                        if (!(string.IsNullOrEmpty(TextBoxSearch.Text.ToUpper())))
                            filter.Append("COMP_NAME Like '%" + searchStr.ToUpper() + "%'");
                        DataView dv = stockMaster.DefaultView;
                        dv.RowFilter = filter.ToString();
                        if (dv.Count > 0)
                        {
                            DropDownListStock.Items.Clear();
                            ListItem li = new ListItem("Select Investment", "-1");
                            DropDownListStock.Items.Add(li);
                            foreach (DataRow rowitem in dv.ToTable().Rows)
                            {
                                li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                                DropDownListStock.Items.Add(li);
                            }
                            breturn = true;
                            ddlExchange.SelectedIndex = 0;
                            ddlInvestmentType.SelectedIndex = 0;
                            textboxPurchaseDate.TextMode = TextBoxMode.Date;
                            textboxPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                            textboxPurchasePrice.Text = "";
                            textboxCommission.Text = "";
                            textboxQuantity.Text = "";
                            labelTotalCost.Text = "";
                            LabelCompanyName.Text = "";
                            labelSelectedSymbol.Text = "";
                        }
                        else
                        {
                            breturn = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
                breturn = false;
            }
            return breturn;
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            //first try & find the user given string in currently loaded stock drop down
            bool bfound = false; //SearchPopulateStocksDropDown(TextBoxSearch.Text);

            if (bfound == false)
            {
                //if not found in current drop down then try and search online and insert new result in db and then re-load the exchange & stock dropdown
                StockManager stockManager = new StockManager();
                bfound = stockManager.SearchOnlineInsertInDB(TextBoxSearch.Text.ToUpper());
                if (bfound)
                {
                    FillSymbolList();
                    FillExchangeList();
                    FillInvestmentTypeList();

                    bfound = SearchPopulateStocksDropDown(TextBoxSearch.Text);
                }
            }

            if (bfound == false)
            {
                //if we still do not find the user given search string then show error message
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }
        }

        public void StockSelectedAction()
        {
            if ( (DropDownListStock.SelectedIndex > 0) && (string.IsNullOrEmpty(textboxPurchaseDate.Text) == false))
            {
                labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                LabelCompanyName.Text = DropDownListStock.SelectedItem.Text.Trim();
                textboxQuantity.Text = "0.00";
                textboxCommission.Text = "0.00";
                labelTotalCost.Text = "0.00";
                StockManager stockManager = new StockManager();
                DateTime nextDate = System.Convert.ToDateTime(textboxPurchaseDate.Text).AddDays(1);
                //DataTable quoteTable = stockManager.GetQuote(DropDownListStock.SelectedValue);
                DataTable quoteTable = stockManager.GetHistoryQuote(DropDownListStock.SelectedValue, textboxPurchaseDate.Text, nextDate.ToShortDateString(), "1d", "1d", "true");
                if ((quoteTable != null) && (quoteTable.Rows.Count > 0))
                {
                    //textboxPurchaseDate.Text = System.Convert.ToDateTime(quoteTable.Rows[0]["latestDay"].ToString()).ToString("yyyy-MM-ddThh:mm:ss");
                    //textboxPurchaseDate.Text = System.Convert.ToDateTime(quoteTable.Rows[0]["latestDay"].ToString()).ToString("yyyy-MM-dd");
                    textboxPurchasePrice.Text = quoteTable.Rows[0]["price"].ToString();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Not able to fetch quote at this moment. You can enter date & price manually or please try again later.');", true);
                    textboxPurchaseDate.TextMode = TextBoxMode.Date;
                }

                DataTable stockTable = stockManager.SearchStock(DropDownListStock.SelectedValue);
                if ((stockTable != null) && (stockTable.Rows.Count > 0))
                {
                    ViewState["StockPortfolioScriptId"] = stockTable.Rows[0]["ROWID"];
                    ddlExchange.SelectedValue = stockTable.Rows[0]["EXCHANGE"].ToString();
                    ddlInvestmentType.SelectedValue = stockTable.Rows[0]["SERIES"].ToString();
                    //Session["STOCKPORTFOLIOSCRIPTID"] = stockTable.Rows[0]["ROWID"];
                }
                else
                {
                    ddlExchange.SelectedIndex = 0;
                    ddlInvestmentType.SelectedIndex = 0;
                    ViewState["StockPortfolioScriptId"] = null;
                }
            }
        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
        }

        protected void textboxPurchaseDate_TextChanged(object sender, EventArgs e)
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
            Response.Redirect("~/mopenportfolio.aspx");
        }

        /// <summary>
        /// Use the currently selected exchange as filter to show only symbols belonging to the selected exchange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                if (ViewState["STOCKMASTER"] != null)
                {
                    DataTable stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();
                        filter.Append("EXCHANGE = '" + ddlExchange.SelectedValue + "'");

                        DataView dv = stockMaster.DefaultView;
                        dv.RowFilter = filter.ToString();
                        if (dv.Count > 0)
                        {
                            DropDownListStock.Items.Clear();
                            ListItem li = new ListItem("Select Investment", "-1");
                            DropDownListStock.Items.Add(li);
                            foreach (DataRow rowitem in dv.ToTable().Rows)
                            {
                                li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                                DropDownListStock.Items.Add(li);
                            }

                            ddlInvestmentType.SelectedIndex = 0;
                            TextBoxSearch.Text = "";
                            textboxPurchaseDate.TextMode = TextBoxMode.Date;
                            textboxPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                            textboxPurchasePrice.Text = "";
                            textboxCommission.Text = "";
                            textboxQuantity.Text = "";
                            labelTotalCost.Text = "";
                            LabelCompanyName.Text = "";
                            labelSelectedSymbol.Text = "";
                        }
                    }
                }
            }
        }

        protected void buttonReset_Click(object sender, EventArgs e)
        {
            FillExchangeList();
            FillInvestmentTypeList();
            FillSymbolList();
            TextBoxSearch.Text = "";
            textboxPurchaseDate.TextMode = TextBoxMode.Date;
            textboxPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            textboxPurchasePrice.Text = "";
            textboxCommission.Text = "";
            textboxQuantity.Text = "";
            labelTotalCost.Text = "";
            LabelCompanyName.Text = "";
            labelSelectedSymbol.Text = "";
        }

        protected void ddlInvestmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInvestmentType.SelectedIndex > 0)
            {
                if (ViewState["STOCKMASTER"] != null)
                {
                    DataTable stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();
                        filter.Append("SERIES = '" + ddlInvestmentType.SelectedValue + "'");

                        DataView dv = stockMaster.DefaultView;
                        dv.RowFilter = filter.ToString();
                        if (dv.Count > 0)
                        {
                            DropDownListStock.Items.Clear();
                            ListItem li = new ListItem("Select Investment", "-1");
                            DropDownListStock.Items.Add(li);
                            foreach (DataRow rowitem in dv.ToTable().Rows)
                            {
                                li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                                DropDownListStock.Items.Add(li);
                            }

                            ddlExchange.SelectedIndex = 0;
                            TextBoxSearch.Text = "";
                            textboxPurchaseDate.TextMode = TextBoxMode.Date;
                            textboxPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                            textboxPurchasePrice.Text = "";
                            textboxCommission.Text = "";
                            textboxQuantity.Text = "";
                            labelTotalCost.Text = "";
                            LabelCompanyName.Text = "";
                            labelSelectedSymbol.Text = "";
                        }
                    }
                }
            }
        }

    }
}