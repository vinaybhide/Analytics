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
                    FillExchangeList();

                    textboxSelectedSymbol.Text = "";
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

                    DataTable tablePortfolioList = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());
                    if ((tablePortfolioList != null) && (tablePortfolioList.Rows.Count > 0))
                    {
                        ddlPortfolios.Items.Clear();
                        ListItem li = new ListItem("Select Portfolio", "-1");
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
                    }
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

        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;

                StockManager stockManager = new StockManager();

                DataTable tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());

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
                else
                {
                    ViewState["STOCKMASTER"] = null;
                    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
        }

        protected void ButtonGetAllForExchange_Click(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;

                StockManager stockManager = new StockManager();

                DataTable tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());

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
                else
                {
                    ViewState["STOCKMASTER"] = null;
                    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
        }

        /// <summary>
        /// search from the company list. use the filter entered in the search text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //replacing below code as yahoo url is not working with NSE code and new DataAccessLayer.StockManager
            //DataTable resultTable = StockApi.symbolSearchAltername(TextBoxSearch.Text.ToUpper(), apiKey: Session["ApiKey"].ToString());
            //if (resultTable != null)
            //{
            //    DropDownListStock.DataTextField = "Name";
            //    DropDownListStock.DataValueField = "Symbol";
            //    DropDownListStock.DataSource = resultTable;
            //    DropDownListStock.DataBind();
            //    ListItem li = new ListItem("Select Stock", "-1");
            //    DropDownListStock.Items.Insert(0, li);
            //    ViewState["GraphScript"] = "Search & Select script";
            //    labelSelectedSymbol.Text = "Search & Select script";
            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            //}

            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('"+ common.noTextSearchSymbol +"')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTextSearchSymbol + "');", true);
            //}

        }
        protected void ddlPortfolios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPortfolios.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfolios.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataSource = null;

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);

                    foreach (DataRow rowSymbols in symbolTable.Rows)
                    {
                        li = new ListItem(rowSymbols["COMP_NAME"].ToString(), rowSymbols["SYMBOL"].ToString());// + "." + rowSymbols["EXCHANGE"].ToString());
                        DropDownListStock.Items.Add(li);
                    }
                    ViewState["STOCKMASTER"] = symbolTable;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelected + "');", true);
            }
        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            if (ddlPortfolios.SelectedIndex >= 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfolios.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataSource = null;

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);

                    foreach (DataRow rowSymbols in symbolTable.Rows)
                    {
                        li = new ListItem(rowSymbols["COMP_NAME"].ToString(), rowSymbols["SYMBOL"].ToString());// + "." + rowSymbols["EXCHANGE"].ToString());
                        DropDownListStock.Items.Add(li);
                    }
                    ViewState["STOCKMASTER"] = symbolTable;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelected + "');", true);
            }
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedIndex > 0)
            {
                ViewState["GraphScript"] = DropDownListStock.SelectedValue;
                //labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
            }
            else
            {
                ViewState["GraphScript"] = "Search & Select script";
                //labelSelectedSymbol.Text = "Search & Select script";
                textboxSelectedSymbol.Text = "Search & Select script";
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
                //url = "\\ vwap_intra.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval_intra=" + interval_intra + "&interval_vwap=" + interval_vwap;
                //url = "~/advgraphs/vwap_intra.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                //    "&interval=" + interval + "&seriestype=CLOSE";
                url = "~/advgraphs/pricevalidator.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                            "&interval=" + interval + "&seriestype=CLOSE";


                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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

                //url = "~/advgraphs/crossover.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&smallperiod=" + period1 + "&longperiod=" + period2 +
                //    "&seriestype=" + seriestype1 + "&interval=" + interval1 + "&outputSize=" + outputSize;
                //url = "~/advgraphs/stockbacktestsma.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue +
                //        "&smasmall=" + period1 + "&smalong=" + period2 + "&buyspan=" + buyspan + "&sellspan=" + sellspan + "&simulationqty=" + simulationqty +
                //        "&regressiontype=" + regressiontype + "&forecastperiod=" + forecastperiod;
                url = "~/advgraphs/backtestsma_stocks.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue +
        "&smasmall=" + period1 + "&smalong=" + period2 + "&buyspan=" + buyspan + "&sellspan=" + sellspan + "&simulationqty=" + simulationqty +
        "&regressiontype=" + regressiontype + "&forecastperiod=" + forecastperiod;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/advgraphs/macdemadaily.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                //    "&interval=" + interval + "&seriestype=" + seriestype +
                //    "&fastperiod=" + fastperiod +
                //                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod;
                url = "~/advgraphs/trendidentifier.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                    "&interval=" + interval + "&seriestype=" + seriestype +
                    "&fastperiod=" + fastperiod +
                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/advgraphs/rsidaily.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&period=" + period +
                //    "&seriestype=" + seriestype + "&interval=" + interval + "&outputSize=" + outputSize;
                url = "~/advgraphs/momentumidentifier.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&period=" + period +
                        "&seriestype=" + seriestype + "&interval=" + interval + "&outputSize=" + outputSize;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/advgraphs/bbandsdaily.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                //    "&interval=" + interval +
                //    "&period=" + period + "&seriestype=" + seriestype + "&stddev=" + stddev;
                url = "~/advgraphs/trendgauger.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + interval +
                    "&period=" + period + "&seriestype=" + seriestype + "&stddev=" + stddev;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/advgraphs/stochdaily.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=" + rsi_seriestype +
                //    "&outputsize=" + outputSize + "&interval=" + interval +
                //    "&fastkperiod=" + fastkperiod + "&slowdperiod=" + slowdperiod + "&period=" + rsi_period;
                url = "~/advgraphs/buysellindicator.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=" + rsi_seriestype +
                        "&outputsize=" + outputSize + "&interval=" + interval +
                        "&fastkperiod=" + fastkperiod + "&slowdperiod=" + slowdperiod + "&period=" + rsi_period;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/advgraphs/dx.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=CLOSE" +
                //    "&outputsize=" + outputSize + "&interval=" + interval + "&period=" + period;
                url = "~/advgraphs/trenddirection.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=CLOSE" +
                        "&outputsize=" + outputSize + "&interval=" + interval + "&period=" + period;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/advgraphs/dmi.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=CLOSE" +
                //    "&outputsize=" + outputSize + "&interval=" + interval + "&period=" + period;
                url = "~/advgraphs/pricedirection.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&seriestype=CLOSE" +
                        "&outputsize=" + outputSize + "&interval=" + interval + "&period=" + period;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }
        #endregion
    }
}