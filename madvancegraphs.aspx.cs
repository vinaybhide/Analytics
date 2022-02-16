using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class madvancegraphs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;

            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["GraphScript"] = null;
                    ViewState["STOCKMASTER"] = null;
                    FillExchangeList();
                    FillInvestmentTypeList();
                    FillSymbolList();
                    LoadPortfolioList();

                    textboxSelectedSymbol.Text = "";
                }
                else
                {
                    if (ViewState["GraphScript"] != null)
                        //labelSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                        textboxSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                //Response.Flush();
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                //Response.Redirect("~/Default.aspx");
                Server.Transfer("~/Default.aspx");
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

        public void FillSymbolList()
        {
            DropDownListStock.Items.Clear();
            textboxSelectedSymbol.Text = "";
            ViewState["GraphScript"] = "";
            ddlExchange.SelectedIndex = 0;
            ddlInvestmentType.SelectedIndex = 0;

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

        public void LoadPortfolioList()
        {
            try
            {
                StockManager stockManager = new StockManager();
                DataTable tablePortfolioList = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());
                if ((tablePortfolioList != null) && (tablePortfolioList.Rows.Count > 0))
                {
                    ddlPortfolios.Enabled = true;
                    ButtonSearchPortfolio.Enabled = true;
                    ddlPortfolios.Items.Clear();
                    ListItem li = new ListItem("Filter By Portfolio", "-1");
                    ddlPortfolios.Items.Insert(0, li);
                    foreach (DataRow rowPortfolio in tablePortfolioList.Rows)
                    {
                        li = new ListItem(rowPortfolio["PORTFOLIO_NAME"].ToString(), rowPortfolio["ROWID"].ToString());
                        ddlPortfolios.Items.Add(li);
                    }
                }
                else
                {
                    //ButtonSearchPortfolio.Enabled = false;
                    ddlPortfolios.Enabled = false;
                    ButtonSearchPortfolio.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error: " + ex.Message + "');", true);
            }
        }

        public void LoadPortfolioStockList()
        {
            if (ddlPortfolios.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlExchange.SelectedIndex = 0;
                ddlInvestmentType.SelectedIndex = 0;
                //ViewState["STOCKMASTER"] = null;

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfolios.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    //ViewState["STOCKMASTER"] = symbolTable;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = symbolTable;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Investment", "-1");
                    DropDownListStock.Items.Insert(0, li);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
        }

        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;
                ddlInvestmentType.SelectedIndex = 0;

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
                        }
                    }
                }
            }
        }

        protected void ddlInvestmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInvestmentType.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;
                ddlExchange.SelectedIndex = 0;

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
                        }
                    }
                }
            }
        }

        protected void ButtonGetAllForExchange_Click(object sender, EventArgs e)
        {
            FillExchangeList();
            FillInvestmentTypeList();
            FillSymbolList();
            LoadPortfolioList();
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
                            //filter.Append("COMP_NAME Like '%" + searchStr.ToUpper() + "%'");
                            filter.Append("COMP_NAME Like '" + searchStr + "%'");
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
                            if (ddlPortfolios.Items.Count > 0)
                            {
                                ddlPortfolios.SelectedIndex = 0;
                            }
                            textboxSelectedSymbol.Text = "";
                            ViewState["GraphScript"] = "";
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
        /// <summary>
        /// search from the company list. use the filter entered in the search text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            //first try & find the user given string in currently loaded stock drop down
            bool bfound = SearchPopulateStocksDropDown(TextBoxSearch.Text);

            if (bfound == false)
            {
                //if not found in current drop down then try and search online and insert new result in db and then re-load the exchange & stock dropdown
                StockManager stockManager = new StockManager();
                bfound = stockManager.SearchOnlineInsertInDB(TextBoxSearch.Text);
                if (bfound)
                {
                    FillExchangeList();
                    FillInvestmentTypeList();
                    FillSymbolList();

                    bfound = SearchPopulateStocksDropDown(TextBoxSearch.Text);
                }
            }

            if (bfound == false)
            {
                //if we still do not find the user given search string then show error message
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }
        }
        protected void ddlPortfolios_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPortfolioStockList();
        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            LoadPortfolioStockList();
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedIndex > 0)
            {
                ViewState["GraphScript"] = DropDownListStock.SelectedValue;
                //labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
                StockManager stockManager = new StockManager();
                DataTable stockTable = stockManager.SearchStock(DropDownListStock.SelectedValue);
                if ((stockTable != null) && (stockTable.Rows.Count > 0))
                {
                    ddlExchange.SelectedValue = stockTable.Rows[0]["EXCHANGE"].ToString();
                    ddlInvestmentType.SelectedValue = stockTable.Rows[0]["SERIES"].ToString();
                }
                else
                {
                    ddlExchange.SelectedIndex = 0;
                    ddlInvestmentType.SelectedIndex = 0;
                }
            }
            else
            {
                ViewState["GraphScript"] = null;
                //labelSelectedSymbol.Text = "Search & Select script";
                textboxSelectedSymbol.Text = "Search & Select investment";
            }

        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
        }

        #region button graph methods
        protected void buttonVWAPIntra_Click(object sender, EventArgs e)
        {
            string outputSize = ddlIntraday_outputsize.SelectedValue;
            string interval = ddlIntraday_Interval.SelectedValue;

            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/pricevalidator.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                            "&interval=" + interval + "&seriestype=CLOSE" + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonCrossover_Click(object sender, EventArgs e)
        {
            string outputSize = ddlDaily_OutputSize.SelectedValue;
            string interval1 = ddlSMA1_Interval.SelectedValue;
            string period1 = textboxSMA1_Period.Text;
            string seriestype1 = ddlSMA1_Series.SelectedValue;
            string period2 = textboxSMA2_Period.Text;
            string scriptName = textboxSelectedSymbol.Text;
            string buyspan = textboxBuySpan.Text;
            string sellspan = textboxSellSpan.Text;
            string simulationqty = textboxSimulationQty.Text;
            string regressiontype = ddlRegressionType.SelectedValue;
            string forecastperiod = textboxForecastPeriod.Text;
            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/backtestsma_stocks.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue +
        "&smasmall=" + period1 + "&smalong=" + period2 + "&buyspan=" + buyspan + "&sellspan=" + sellspan + "&simulationqty=" + simulationqty +
        "&regressiontype=" + regressiontype + "&forecastperiod=" + forecastperiod + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonMACD_EMA_Daily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlMACDEMADaily_outputsize.SelectedValue;
            string interval = ddlMACDEMADail_interval1.SelectedValue;
            string seriestype = ddlMACDEMADail_seriestype1.SelectedValue;
            string fastperiod = textboxMACDEMADaily_fastperiod.Text;
            string slowperiod = textboxMACDEMADaily_slowperiod.Text;
            string signalperiod = textboxMACDEMADaily_signalperiod.Text;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;
            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/trendidentifier.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                    "&interval=" + interval + "&seriestype=" + seriestype +
                    "&fastperiod=" + fastperiod +
                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonRSIDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlRSIDaily_Outputsize.SelectedValue;
            string interval = ddlRSIDaily_Interval.SelectedValue;
            string period = textboxRSIDaily_Period.Text;
            string seriestype = ddlRSIDaily_SeriesType.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/momentumidentifier.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&period=" + period +
                        "&seriestype=" + seriestype + "&interval=" + interval + "&outputSize=" + outputSize + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonBBandsDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlBBandsDaily_Outputsize.SelectedValue;
            string interval = ddlBBandsDaily_Interval.SelectedValue;
            string period = textboxBBandsDaily_Period.Text;
            string seriestype = ddlBBandsDaily_SeriesType.SelectedValue;
            string stddev = textboxBBandsDaily_StdDev.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/trendgauger.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + interval +
                    "&period=" + period + "&seriestype=" + seriestype + "&stddev=" + stddev + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonStochDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlStochDaily_OutuputSize.SelectedValue;
            string interval = ddlStochDaily_Interval.SelectedValue;
            string fastkperiod = textboxSTOCHDaily_Fastkperiod.Text;
            string slowdperiod = textboxSTOCHDaily_Slowdperiod.Text;
            string rsi_interval = ddlStochDaily_Interval.SelectedValue;
            string rsi_period = textboxStochDailyRSI_Period.Text;
            string rsi_seriestype = ddlStochDailyRSI_SeriesType.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;
            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/buysellindicator.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=" + rsi_seriestype +
                        "&outputsize=" + outputSize + "&interval=" + interval +
                        "&fastkperiod=" + fastkperiod + "&slowdperiod=" + slowdperiod + "&period=" + rsi_period + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonDMI_Click(object sender, EventArgs e)
        {
            string outputSize = ddlDMIDaily_Outputsize.SelectedValue;
            string interval = ddlDMIMINUSDI_Interval.SelectedValue;
            string period = textboxDMIMINUSDI_Period.Text;

            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;
            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/trenddirection.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=CLOSE" +
                        "&outputsize=" + outputSize + "&interval=" + interval + "&period=" + period + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }
        protected void buttonPrice_Click(object sender, EventArgs e)
        {
            string outputSize = ddlPrice_Outputsize.SelectedValue;
            string interval = ddlDMIDX_Interval.SelectedValue;
            string period = textboxDMIDX_Period.Text;

            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;
            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/pricedirection.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=CLOSE" +
                        "&outputsize=" + outputSize + "&interval=" + interval + "&period=" + period + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=madvancegraphs.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }
        #endregion

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/mselectportfolio.aspx");
        }
    }
}