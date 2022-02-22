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
    public partial class mgetquoteadd : System.Web.UI.Page
    {
        public string Symbol
        {
            get
            {
                if (DropDownListStock.SelectedIndex >= 0)
                    return DropDownListStock.SelectedValue;
                else
                    return "";
            }
        }
        public string CompanyName
        {
            get
            {
                if (DropDownListStock.SelectedIndex >= 0)
                    //return (DropDownListStock.SelectedItem.Text.Split(':')[1]).Trim();
                    return DropDownListStock.SelectedItem.Text.Trim();
                else
                    return "";
            }
        }
        public string ExchangeCode
        {
            get
            {
                return textboxExch.Text.Trim();
            }
        }
        public string ExchangeDisplay
        {
            get
            {
                return textboxExchDisp.Text.Trim();
            }
        }
        public string InvestmentType
        {
            get
            {
                return textboxType.Text.Trim();
            }
        }
        public string InvestmentTypeDisplay
        {
            get
            {
                return textboxTypeDisp.Text.Trim();
            }
        }

        public string QuoteDateTime
        {
            get
            {
                return System.Convert.ToDateTime(textboxLatestDay.Text.ToString()).ToString();
            }
        }

        public string Price
        {
            get
            {
                return textboxPrice.Text;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EMAILID"] != null)
            //{
            //    Master.UserID = Session["EMAILID"].ToString();
            //}

            //if((Session["EMAILID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            if ((Session["EMAILID"] != null))
            {
                //Master.Portfolio = Session["STOCKPORTFOLIONAME"].ToString();
                if (!IsPostBack)
                {
                    ViewState["FetchedData"] = null;
                    ViewState["STOCKMASTER"] = null;
                    FillExchangeList();
                    FillInvestmentTypeList();
                    FillSymbolList();

                    bool isAddAllowed = true;
                    if (Request.QueryString["addallowed"] != null)
                        isAddAllowed = System.Convert.ToBoolean(Request.QueryString["addallowed"]);

                    if (isAddAllowed)
                    {
                        buttonAddStock.Enabled = true;
                    }
                    else
                    {
                        buttonAddStock.Enabled = false;
                    }

                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
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
                            TextBoxSearch.Text = "";
                            textboxSelectedSymbol.Text = "";

                            textboxOpen.Text = "";
                            textboxHigh.Text = "";
                            textboxLow.Text = "";
                            textboxPrice.Text = "";
                            textboxVolume.Text = "";
                            textboxLatestDay.Text = "";
                            textboxPrevClose.Text = "";
                            textboxChange.Text = "";
                            textboxChangePercent.Text = "";
                            textboxExch.Text = "";
                            textboxExchDisp.Text = "";
                            textboxType.Text = "";
                            textboxTypeDisp.Text = "";

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
            if (DropDownListStock.SelectedIndex > 0)
            {
                //labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
                //Session["STOCKPORTFOLIOSCRIPTNAME"] = DropDownListStock.SelectedValue;
                StockManager stockManager = new StockManager();
                DataTable quoteTable = stockManager.GetQuote(DropDownListStock.SelectedValue);
                if ((quoteTable != null) && (quoteTable.Rows.Count > 0))
                {
                    textboxOpen.Text = quoteTable.Rows[0]["open"].ToString();
                    textboxHigh.Text = quoteTable.Rows[0]["high"].ToString();
                    textboxLow.Text = quoteTable.Rows[0]["low"].ToString();
                    textboxPrice.Text = quoteTable.Rows[0]["price"].ToString();
                    textboxVolume.Text = quoteTable.Rows[0]["volume"].ToString();
                    textboxLatestDay.Text = System.Convert.ToDateTime(quoteTable.Rows[0]["latestDay"].ToString()).ToString("yyyy-MM-ddThh:mm:ss");
                    textboxPrevClose.Text = quoteTable.Rows[0]["previousClose"].ToString();
                    textboxChange.Text = quoteTable.Rows[0]["change"].ToString();
                    textboxChangePercent.Text = quoteTable.Rows[0]["changePercent"].ToString();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Not able to fetch quote at this moment. You can enter date & price manually or please try again later.');", true);
                }

                DataTable stockTable = stockManager.SearchStock(DropDownListStock.SelectedValue);
                if ((stockTable != null) && (stockTable.Rows.Count > 0))
                {
                    ViewState["StockMasterRowId"] = stockTable.Rows[0]["ROWID"];
                    ddlExchange.SelectedValue = stockTable.Rows[0]["EXCHANGE"].ToString();
                    ddlInvestmentType.SelectedValue = stockTable.Rows[0]["SERIES"].ToString();
                    textboxExch.Text = ddlExchange.SelectedValue;
                    textboxExchDisp.Text = ddlExchange.Text;
                    textboxType.Text = ddlInvestmentType.SelectedValue;//scriptRows[0]["Type"].ToString();
                    textboxTypeDisp.Text = ddlInvestmentType.SelectedValue;// scriptRows[0]["TypeDisplay"].ToString();
                }
                else
                {
                    ddlExchange.SelectedIndex = 0;
                    ddlInvestmentType.SelectedIndex = 0;
                    ViewState["StockPortfolioScriptId"] = null;
                }

            }
            else
            {
                textboxSelectedSymbol.Text = "Please select stock to get quote for";

                textboxOpen.Text = "";
                textboxHigh.Text = "";
                textboxLow.Text = "";
                textboxPrice.Text = "";
                textboxVolume.Text = "";
                textboxLatestDay.Text = "";
                textboxPrevClose.Text = "";
                textboxChange.Text = "";
                textboxChangePercent.Text = "";
                textboxExch.Text = "";
                textboxExchDisp.Text = "";
                textboxType.Text = "";
                textboxTypeDisp.Text = "";
            }

        }

        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
        }

        protected void buttonAddStock_Click(object sender, EventArgs e)
        {
            //Server.Transfer("~/addnewscript.aspx");
            if (ViewState["StockMasterRowId"] != null)
            {
                Session["STOCKPORTFOLIOSCRIPTID"] = ViewState["StockMasterRowId"];
                Response.Redirect("~/maddnewscript.aspx?symbol=" + Symbol + "&date=" + QuoteDateTime + "&price=" + Price + "&companyname=" + Server.UrlEncode(CompanyName) +
                    "&exch=" + ddlExchange.SelectedValue + "&type=" + ddlInvestmentType.SelectedValue);
            }
        }
        protected void buttonGoBack_Click(object sender, EventArgs e)
        {
            bool isAddAllowed = true;
            if (Request.QueryString["addallowed"] != null)
                isAddAllowed = System.Convert.ToBoolean(Request.QueryString["addallowed"]);

            if (isAddAllowed)
            {
                Response.Redirect("~/mopenportfolio.aspx");
            }
            else
            {
                Response.Redirect("~/mselectportfolio.aspx");
                //StockManager stockManager = new StockManager();
                //if (stockManager.getPortfolioCount(Session["EMAILID"].ToString()) > 0)
                //    Response.Redirect("~/mselectportfolio.aspx");
                //else
                //    Response.Redirect("~/mnewportfolio.aspx");
            }
        }



        protected void ButtonGetQuote_Click(object sender, EventArgs e)
        {
            StockSelectedAction();
        }

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
                            textboxSelectedSymbol.Text = "";
                            textboxOpen.Text = "";
                            textboxHigh.Text = "";
                            textboxLow.Text = "";
                            textboxPrice.Text = "";
                            textboxVolume.Text = "";
                            textboxLatestDay.Text = "";
                            textboxPrevClose.Text = "";
                            textboxChange.Text = "";
                            textboxChangePercent.Text = "";
                            textboxExch.Text = "";
                            textboxExchDisp.Text = "";
                            textboxType.Text = "";//scriptRows[0]["Type"].ToString();
                            textboxTypeDisp.Text = "";// scriptRows[0]["TypeDisplay"].ToString();
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
            textboxSelectedSymbol.Text = "";
            textboxOpen.Text = "";
            textboxHigh.Text = "";
            textboxLow.Text = "";
            textboxPrice.Text = "";
            textboxVolume.Text = "";
            textboxLatestDay.Text = "";
            textboxPrevClose.Text = "";
            textboxChange.Text = "";
            textboxChangePercent.Text = "";
            textboxExch.Text = "";
            textboxExchDisp.Text = "";
            textboxType.Text = "";//scriptRows[0]["Type"].ToString();
            textboxTypeDisp.Text = "";// scriptRows[0]["TypeDisplay"].ToString();
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
                            textboxSelectedSymbol.Text = "";
                            textboxOpen.Text = "";
                            textboxHigh.Text = "";
                            textboxLow.Text = "";
                            textboxPrice.Text = "";
                            textboxVolume.Text = "";
                            textboxLatestDay.Text = "";
                            textboxPrevClose.Text = "";
                            textboxChange.Text = "";
                            textboxChangePercent.Text = "";
                            textboxExch.Text = "";
                            textboxExchDisp.Text = "";
                            textboxType.Text = "";//scriptRows[0]["Type"].ToString();
                            textboxTypeDisp.Text = "";// scriptRows[0]["TypeDisplay"].ToString();
                        }
                    }
                }
            }
        }
    }
}