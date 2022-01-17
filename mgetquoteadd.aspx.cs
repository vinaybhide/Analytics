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
                    FillExchangeList();

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

                    bool isAddAllowed = true;
                    if (Request.QueryString["addallowed"] != null)
                        isAddAllowed = System.Convert.ToBoolean(Request.QueryString["valuation"]);

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

        protected void buttonAddStock_Click(object sender, EventArgs e)
        {
            //Server.Transfer("~/addnewscript.aspx");
            if (ViewState["StockMasterRowId"] != null)
            {
                Session["STOCKPORTFOLIOSCRIPTID"] = ViewState["StockMasterRowId"];
                if (this.MasterPageFile.Contains("Site.Master"))
                    Response.Redirect("~/addnewscript.aspx?symbol=" + Symbol + "&date=" + QuoteDateTime + "&price=" + Price + "&companyname=" + Server.UrlEncode(CompanyName) +
                        "&exch=" + ddlExchange.SelectedValue);
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    Response.Redirect("~/maddnewscript.aspx?symbol=" + Symbol + "&date=" + QuoteDateTime + "&price=" + Price + "&companyname=" + Server.UrlEncode(CompanyName) +
                        "&exch=" + ddlExchange.SelectedValue);
                else
                    Response.Redirect("~/maddnewscript.aspx?symbol=" + Symbol + "&date=" + QuoteDateTime + "&price=" + Price + "&companyname=" + Server.UrlEncode(CompanyName) +
                        "&exch=" + ddlExchange.SelectedValue);
            }
        }
        protected void buttonGoBack_Click(object sender, EventArgs e)
        {
            if (this.MasterPageFile.Contains("Site.Master"))
                Response.Redirect("~/openportfolio.aspx");
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect("~/mopenportfolio.aspx");
            else
                Response.Redirect("~/mopenportfolio.aspx");
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedValue != "-1")
            {
                //labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
                Session["STOCKPORTFOLIOSCRIPTNAME"] = DropDownListStock.SelectedValue;
                textboxExch.Text = ddlExchange.SelectedValue;
                textboxExchDisp.Text = ddlExchange.Text;
                textboxType.Text = "";//scriptRows[0]["Type"].ToString();
                textboxTypeDisp.Text = "";// scriptRows[0]["TypeDisplay"].ToString();
                StockManager stockManager = new StockManager();
                DataTable stockTable = stockManager.SearchStock(textboxSelectedSymbol.Text);
                if ((stockTable != null) && (stockTable.Rows.Count > 0))
                {
                    ViewState["StockMasterRowId"] = stockTable.Rows[0]["ROWID"];
                }
            }
            else
            {
                textboxSelectedSymbol.Text = "Please select stock to get quote for";
            }

        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
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
                DataTable tableSearch = stockManager.SearchStock(TextBoxSearch.Text.ToUpper());
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
        protected void ButtonGetQuote_Click(object sender, EventArgs e)
        {
            string selectedSymbol = "";
            if (DropDownListStock.SelectedIndex >= 0)
            {
                //selectedSymbol = DropDownListStock.SelectedValue;
                selectedSymbol = textboxSelectedSymbol.Text;

                //DataTable quoteTable = StockApi.globalQuote(folderPath, selectedSymbol, bIsTestOn, apiKey: Session["ApiKey"].ToString());
                //will try to ALWAYS get quote from market 
                //DataTable quoteTable = StockApi.globalQuoteAlternate(folderPath, selectedSymbol, bIsTestOn, apiKey: Session["ApiKey"].ToString());
                //DataTable quoteTable = StockApi.globalQuoteAlternate(folderPath, selectedSymbol, bIsTestModeOn:false, apiKey: Session["ApiKey"].ToString());
                //column names = symbol,open,high,low,price,volume,latestDay,previousClose,change,changePercent

                StockManager stockManager = new StockManager();
                DataTable quoteTable = stockManager.GetQuote(selectedSymbol);
                if (quoteTable != null)
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
                    //Response.Write("<script language=javascript>alert('" + common.noQuoteAvailable + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noQuoteAvailable + "');", true);
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('"+ common.noStockSelectedToGetQuote +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToGetQuote + "');", true);
            }

        }

        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";

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
                                              //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
        }
    }
}