﻿using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics.advGraphs
{
    public partial class advancegraphs : System.Web.UI.MasterPage
    {
        public HiddenField panelWidth
        {
            get
            {
                // Return the textbox on the master page
                return this.panelWidthM;
            }
        }

        public HiddenField panelHeight
        {
            get
            {
                // Return the textbox on the master page
                return this.panelHeightM;
            }
        }

        #region panel top section
        public Panel panelTopMost
        {
            get
            {
                return panelTopSection;
            }
        }
        #endregion
        #region panels stocks
        public Panel panelStocksMain
        {
            get
            {
                return this.panelStocksM;
            }
        }

        public Panel panelOnlineStocks
        {
            get
            {
                return this.panelOnlineStocksM;
            }
        }

        public Panel panelPortfolioStocks
        {
            get
            {
                return this.panelPortfolioStocksM;
            }
        }
        public Panel panelSearchListStocks
        {
            get
            {
                return this.panelSearchListStocksM;
            }
        }

        #endregion

        #region panel MF
        public Panel panelMFMain
        {
            get
            {
                return this.panelMFM;
            }
        }
        public Panel panelOnlineFundHouse
        {
            get
            {
                return this.panelOnlineFundHouseM;
            }
        }

        public Panel panelPortfolioMF
        {
            get
            {
                return panelPortfolioMFM;
            }
        }
        public Panel panelSearchListFundName
        {
            get
            {
                return panelSearchListFundNameM;
            }
        }
        #endregion

        #region panel Indexes
        public Panel panelGlobalIndex
        {
            get
            {
                return panelGlobalIndexM;
            }
        }
        #endregion
        #region panel common controls
        public Panel panelCommonControls
        {
            get
            {
                return this.panelCommonControlsM;
            }
        }
        public Panel panelGraphList
        {
            get
            {
                return this.panelGraphListM;
            }
        }
        public Panel panelFromTo
        {
            get
            {
                return this.panelFromToM;
            }
        }

        public Panel panelDescription
        {
            get
            {
                return this.panelDescriptionM;
            }
        }
        #endregion

        #region panel parameters
        public Panel panelMainParam
        {
            get
            {
                return this.panelMainParamM;
            }
        }

        public Panel panelParamSMA
        {
            get
            {
                return this.panelParamSMAM;
            }
        }

        public Panel panelParamStockBackTest
        {
            get
            {
                return this.panelParamStockBackTestM;
            }
        }
        
        public Panel panelCommonParameters
        {
            get
            {
                return panelCommonParametersM;
            }
        }
        
        public Panel panelSMADaily
        {
            get
            {
                return panelSMADailyM;
            }
        }
        public Panel panelMacdDaily
        {
            get
            {
                return panelMacdDailyM;
            }
        }
        public Panel panelRsiDaily
        {
            get
            {
                return panelRsiDailyM;
            }
        }
        public Panel panelBbandsDaily
        {
            get
            {
                return panelBbandsDailyM;
            }
        }

        public Panel panelStochRsiDaily
        {
            get
            {
                return panelStochRsiDailyM;
            }
        }
        public Panel panelDiAdxDaily
        {
            get
            {
                return panelDiAdxDailyM;
            }
        }
        public Panel panelDxDmDaily
        {
            get
            {
                return panelDxDmDailyM;
            }
        }
        #endregion

        #region common controls
        public Label headingtext
        {
            get
            {
                // Return the textbox on the master page
                return this.headingtextM;
            }
        }

        public TextBox textboxFromDate
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxFromDateM;
            }
        }

        public Label labelToDate
        {
            get
            {
                return labelToDateM;
            }
        }
        public TextBox textboxToDate
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxToDateM;
            }
        }
        public BulletedList bulletedlistDesc
        {
            get
            {
                // Return the textbox on the master page
                return this.bulletedlistDescM;
            }
        }
        public DropDownList dropdownGraphList
        {
            get
            {
                // Return the textbox on the master page
                return this.ddlIndicator;
            }
        }
        #endregion
        
        #region buttons exposed
        public Button buttonDesc
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonDescM;
            }
        }
        public Button buttonShowGrid
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonShowGridM;
            }
        }

        public Button buttonShowHideParam
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonParametersM;
            }
        }

        public Button buttonTopSecion
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonTopSecionM;
            }
        }

        public Button buttonSearchStockPortfolio
        {
            get
            {
                return ButtonSearchPortfolio;
            }
        }
        #endregion

        #region controls stock
        public DropDownList dropdownExchangeList
        {
            get
            {
                // Return the textbox on the master page
                return this.ddlExchange;
            }
        }

        public DropDownList dropdownPortfolioList
        {
            get
            {
                // Return the textbox on the master page
                return this.ddlPortfoliosStocks;
            }
        }

        public DropDownList dropdownStockList
        {
            get
            {
                // Return the textbox on the master page
                return this.DropDownListStock;
            }
        }

        public TextBox textbox_SelectedExchange
        {
            get
            {
                return this.textboxSelectedExchange;
            }
        }

        public TextBox textbox_SelectedSymbol
        {
            get
            {
                return this.textboxSelectedSymbol;
            }
        }

        #endregion

        #region controls backtest param
        public TextBox textboxSMASmallPeriod
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxSMASmallPeriodM;
            }
        }

        public TextBox textboxSMALongPeriod
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxSMALongPeriodM;
            }
        }

        public TextBox textboxBuySpan
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxBuySpanM;
            }
        }

        public TextBox textboxSellSpan
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxSellSpanM;
            }
        }

        public TextBox textboxSimulationQty
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxSimulationQtyM;
            }
        }

        public TextBox textboxForecastPeriod
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxForecastPeriodM;
            }
        }

        public DropDownList ddlRegressionType
        {
            get
            {
                // Return the textbox on the master page
                return this.ddlRegressionTypeM;
            }
        }
        #endregion//end backtest param

        #region MF controls
        public DropDownList dropdownFundhouseList
        {
            get
            {
                return ddlFundHouse;
            }
        }
        public DropDownList dropdownPortfolioMF
        {
            get
            {
                return ddlPortfolioMF;
            }
        }

        public TextBox textbox_selectedFundName
        {
            get
            {
                return textboxSelectedFundName;
            }
        }

        public TextBox textbox_selectedSchemeCode
        {
            get
            {
                return textboxSchemeCode;
            }
        }

        public DropDownList dropdownFundNameList
        {
            get
            {
                return ddlFundName;
            }
        }
        #endregion

        #region common param controls exposed
        public DropDownList dropdownOutputSize
        {
            get
            {
                return ddl_Outputsize;
            }
        }

        public Label labelInterval
        {
            get
            {
                return labelIntervalM;
            }
        }

        public Label labelSeriesType
        {
            get
            {
                return labelSeriesTypeM;
            }
        }
        public DropDownList dropdownInterval
        {
            get
            {
                return ddl_Interval;
            }
        }
        public DropDownList dropdownSeries
        {
            get
            {
                return ddl_Series;
            }
        }
        #endregion

        #region controls SMA
        public TextBox textboxSMADailyFastPeriod
        {
            get
            {
                return textboxSMADailyFastPeriodM;
            }
        }

        public TextBox textboxSMADailySlowPeriod
        {
            get
            {
                return textboxSMADailySlowPeriodM;
            }
        }
        #endregion
        #region controls MACD_DAILY
        public TextBox textboxMACDEMADaily_fastperiod
        {
            get
            {
                return textboxMACDEMADaily_fastperiodM;
            }
        }
        public TextBox textboxMACDEMADaily_slowperiod
        {
            get
            {
                return textboxMACDEMADaily_slowperiodM;
            }
        }
        public TextBox textboxMACDEMADaily_signalperiod
        {
            get
            {
                return textboxMACDEMADaily_signalperiodM;
            }
        }
        #endregion

        #region controls RSI_DAILY
        public TextBox textboxRSIDaily_Period
        {
            get
            {
                return textboxRSIDaily_PeriodM;
            }
        }
        #endregion

        #region controls BBANDS_DAILY
        public TextBox textboxBBandsDaily_Period
        {
            get
            {
                return textboxBBandsDaily_PeriodM;
            }
        }
        public TextBox textboxBBandsDaily_StdDev
        {
            get
            {
                return textboxBBandsDaily_StdDevM;
            }
        }
        #endregion

        #region controls indexes
        public DropDownList ddlIndexList
        {
            get
            {
                return ddlIndexListM;
            }
        }
        #endregion
        #region controls Stoch_Rsi_Daily
        public TextBox textboxSTOCHDaily_Fastkperiod
        {
            get
            {
                return textboxSTOCHDaily_FastkperiodM;
            }
        }
        public TextBox textboxSTOCHDaily_Slowdperiod
        {
            get
            {
                return textboxSTOCHDaily_SlowdperiodM;
            }
        }

        public Label labelStochDailyRSI_Period
        {
            get
            {
                return labelStochDailyRSI_PeriodM;
            }
        }
        public TextBox textboxStochDailyRSI_Period
        {
            get
            {
                return textboxStochDailyRSI_PeriodM;
            }
        }
        #endregion

        #region controls Di_Adx_Daily
        public TextBox textboxDMIMINUSDI_Period
        {
            get
            {
                return textboxDMIMINUSDI_PeriodM;
            }
        }
        #endregion

        #region controls Dx_Dm_Daily
        public TextBox textboxDMIDX_Period
        {
            get
            {
                return textboxDMIDX_PeriodM;
            }
        }
        #endregion
        public delegate void DoEventShowGraph();
        public event DoEventShowGraph OnDoEventShowGraph;

        public delegate void DoEventShowGrid();
        public event DoEventShowGrid OnDoEventShowGrid;

        //public delegate void DoEventToggleDesc();
        //public event DoEventToggleDesc OnDoEventToggleDesc;

        //public delegate void DoEventToggleParameters();
        //public event DoEventToggleParameters OnDoEventToggleParameters;

        public delegate void DoEventRemoveSelectedIndicatorGraph();
        public event DoEventRemoveSelectedIndicatorGraph OnDoEventRemoveSelectedIndicatorGraph;

        public delegate void DoEventShowSelectedIndicatorGraph();
        public event DoEventShowSelectedIndicatorGraph OnDoEventShowSelectedIndicatorGraph;

        #region Common Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetIndexValues(null, null);
            }

        }
        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowGraph != null)
            {
                OnDoEventShowGraph();
            }
        }

        protected void buttonShowGrid_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowGrid != null)
            {
                OnDoEventShowGrid();
            }
        }

        protected void buttonDescM_Click(object sender, EventArgs e)
        {
            panelDescriptionM.Enabled = !panelDescriptionM.Enabled;
            panelDescriptionM.Visible = !panelDescriptionM.Visible;
            //if (OnDoEventToggleDesc != null)
            //{
            //    OnDoEventToggleDesc();
            //}
        }
        protected void buttonTopSectionM_Click(object sender, EventArgs e)
        {
            panelTopSection.Enabled = !panelTopSection.Enabled;
            panelTopSection.Visible = !panelTopSection.Visible;
        }

        protected void buttonParametersM_Click(object sender, EventArgs e)
        {
            panelMainParam.Enabled = !panelMainParam.Enabled;
            panelMainParam.Visible = !panelMainParam.Visible;

            //if (OnDoEventToggleParameters != null)
            //{
            //    OnDoEventToggleParameters();
            //}
        }

        protected void buttonRemoveSelectedIndicatorGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventRemoveSelectedIndicatorGraph != null)
            {
                OnDoEventRemoveSelectedIndicatorGraph();
            }
        }

        protected void buttonShowSelectedIndicatorGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowSelectedIndicatorGraph != null)
            {
                OnDoEventShowSelectedIndicatorGraph();
            }
        }

        protected void GetIndexValues(object sender, EventArgs e)
        {
            //Use myQuote.close.Last() - myMeta.chartPreviousClose to show difference
            //(myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100 to show percentage diff

            Root myDeserializedClass = StockApi.getIndexIntraDayAlternate("^BSESN", time_interval: "1min", outputsize: "compact");

            if (myDeserializedClass != null)
            {
                Chart myChart = myDeserializedClass.chart;

                Result myResult = myChart.result[0];

                Meta myMeta = myResult.meta;

                Indicators myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                Quote myQuote = myIndicators.quote[0];

                ////this will be typically only 1 row and adjClose will have list of adjClose
                //Adjclose myAdjClose = null;
                //myAdjClose = myIndicators.adjclose[0];

                //DateTime myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp.Last()).ToLocalTime();
                DateTime myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);

                StringBuilder indexString = new StringBuilder();
                indexString.Append(string.Format("SENSEX@{0:HH:mm}: ", myDate));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last()));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last() - myMeta.chartPreviousClose));
                indexString.Append(string.Format("{0:0.00}% ", (myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100));

                myDeserializedClass = StockApi.getIndexIntraDayAlternate("^NSEI", time_interval: "1min", outputsize: "compact");

                myChart = myDeserializedClass.chart;

                myResult = myChart.result[0];

                myMeta = myResult.meta;

                myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                myQuote = myIndicators.quote[0];

                //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp.Last()).ToLocalTime();
                myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);

                indexString.Append(string.Format("| NIFTY@{0:HH:mm}: ", myDate));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last()));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last() - myMeta.chartPreviousClose));
                indexString.Append(string.Format("{0:0.00}%", (myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100));

                headingtext.Text = indexString.ToString();
                headingtext.CssClass = headingtext.CssClass.Replace("fade", "");
            }
        }
        protected void ClearHeading(object sender, EventArgs e)
        {
            headingtext.CssClass = "fade";
            //headingtext.Text = "";
        }

        /// <summary>
        /// OnPageLoad !postback - We need to enable & show following panels for this graph 
        /// panelTopSection, panelStocksM, panelOnlineStocksM, panelPortfolioStocksM, panelSearchListStocksM
        /// 
        /// OnPageLoad !postback - we need to disable & hide following panels for this graph
        /// panelMFM, panelOnlineFundHouseM, panelPortfolioMFM, panelSearchListFundNameM
        /// </summary>
        public void InitializePanels(bool bStocksGraph = true)
        {
            //panelTopSection.Enabled = true;
            //panelTopSection.Visible = true;
            if (bStocksGraph)
            {
                //we are called from a stocks graphs page - show & enable following panels for stocks
                panelStocksM.Enabled = true;
                panelOnlineStocksM.Enabled = true;
                panelPortfolioStocksM.Enabled = true;
                panelSearchListStocksM.Enabled = true;

                panelStocksM.Visible = true;
                panelOnlineStocksM.Visible = true;
                panelPortfolioStocksM.Visible = true;
                panelSearchListStocksM.Visible = true;

                //we are called from a stocks graphs page - hide & disbale following panels for MF
                panelMFM.Enabled = false;
                panelOnlineFundHouseM.Enabled = false;
                panelPortfolioMFM.Enabled = false;
                panelSearchListFundNameM.Enabled = false;

                panelMFM.Visible = false;
                panelOnlineFundHouseM.Visible = false;
                panelPortfolioMFM.Visible = false;
                panelSearchListFundNameM.Visible = false;

                FillExchangeList();
            }
            else
            {
                //we are called from a MF graphs page - show & enable following panels for MF
                panelMFM.Enabled = true;
                panelOnlineFundHouseM.Enabled = true;
                panelPortfolioMFM.Enabled = true;
                panelSearchListFundNameM.Enabled = true;

                panelMFM.Visible = true;
                panelOnlineFundHouseM.Visible = true;
                panelPortfolioMFM.Visible = true;
                panelSearchListFundNameM.Visible = true;

                //we are called from a MF graphs page - hide & disable following panels for stocks
                panelStocksM.Enabled = false;
                panelOnlineStocksM.Enabled = false;
                panelPortfolioStocksM.Enabled = false;
                panelSearchListStocksM.Enabled = false;

                panelStocksM.Visible = false;
                panelOnlineStocksM.Visible = false;
                panelPortfolioStocksM.Visible = false;
                panelSearchListStocksM.Visible = false;
            }

            //now manage common panels - show & enable all below common panels
            panelCommonControlsM.Enabled = true;
            panelGraphListM.Enabled = true;
            panelFromToM.Enabled = true;
            panelDescriptionM.Enabled = true;

            panelCommonControlsM.Visible = true;
            panelGraphListM.Visible = true;
            panelFromToM.Visible = true;
            panelDescriptionM.Visible = true;
        }


        #endregion

        #region Stocks Methods
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

        public void LoadStockMaster()
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                try
                {
                    DropDownListStock.Items.Clear();
                    TextBoxSearch.Text = "";
                    textboxSelectedSymbol.Text = "";

                    ViewState["STOCKMASTER"] = null;
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
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error: " + ex.Message + "');", true);
                }
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
                    ddlPortfoliosStocks.Items.Clear();
                    ListItem li = new ListItem("Select Portfolio", "-1");
                    ddlPortfoliosStocks.Items.Insert(0, li);
                    foreach (DataRow rowPortfolio in tablePortfolioList.Rows)
                    {
                        li = new ListItem(rowPortfolio["PORTFOLIO_NAME"].ToString(), rowPortfolio["ROWID"].ToString());
                        ddlPortfoliosStocks.Items.Add(li);
                    }
                }
                else
                {
                    //ButtonSearchPortfolio.Enabled = false;
                    ddlPortfoliosStocks.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error: " + ex.Message + "');", true);
            }
        }

        public void LoadPortfolioStockList()
        {
            if (ddlPortfoliosStocks.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                textboxSelectedExchange.Text = "";

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfoliosStocks.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    ViewState["STOCKMASTER"] = symbolTable;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = symbolTable;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Stock", "-1");
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
                if (ddlPortfolioMF.Items.Count > 0)
                {
                    ddlPortfoliosStocks.SelectedIndex = 0;
                }

                LoadStockMaster();
            }
        }

        protected void ButtonGetAllForExchange_Click(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                if (ddlPortfolioMF.Items.Count > 0)
                {
                    ddlPortfoliosStocks.SelectedIndex = 0;
                }

                LoadStockMaster();
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

        protected void ddlPortfoliosStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPortfolioStockList();
        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            LoadPortfolioStockList();
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedIndex > 0 )
            {
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedExchange.Text = ddlExchange.SelectedValue;
                OnDoEventShowGraph.Invoke();
            }
            else
            {
                textboxSelectedSymbol.Text = "Search & Select script";
                textboxSelectedExchange.Text = "";
            }

        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
        }

        #endregion
        
        #region Mutual Funds Methods
        public void LoadFundHouseList()
        {
            ViewState["MFSchemeTable"] = null;
            DataManager dataMgr = new DataManager();
            DataTable fundHouseTable = dataMgr.getFundHouseTable();
            if ((fundHouseTable != null) && (fundHouseTable.Rows.Count > 0))
            {
                // Columns - FUNDHOUSECODE, NAME
                ddlFundHouse.DataTextField = "NAME";
                ddlFundHouse.DataValueField = "FUNDHOUSECODE";
                ddlFundHouse.DataSource = fundHouseTable;
                ddlFundHouse.DataBind();
            }
        }

        public void LoadPortfolioMF()
        {
            DataManager dataMgr = new DataManager();
            DataTable portfolioTable = dataMgr.getPortfolioTable(Session["EMAILID"].ToString());
            if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
            {
                ddlPortfolioMF.DataTextField = "PORTFOLIO_NAME";
                ddlPortfolioMF.DataValueField = "ID";
                ddlPortfolioMF.DataSource = portfolioTable;
                ddlPortfolioMF.DataBind();
            }
            ListItem li = new ListItem("Select MF Portfolio", "-1");
            ddlPortfolioMF.Items.Insert(0, li);
        }

        public void LoadFundNameList()
        {
            ViewState["MFSchemeTable"] = null;

            ddlFundName.Enabled = true;
            ddlFundName.Items.Clear();
            textboxSchemeCode.Text = "";

            DataManager dataMgr = new DataManager();
            DataTable mfSchemeTable = dataMgr.getSchemesTable(fundhousecode: System.Convert.ToInt32(ddlFundHouse.SelectedValue));
            if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
            {
                //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
                ddlFundName.DataTextField = "SCHEMENAME";
                ddlFundName.DataValueField = "SCHEMECODE";
                ddlFundName.DataSource = mfSchemeTable;
                ddlFundName.DataBind();

                ListItem li = new ListItem("-- Select Fund Name --", "-1");
                ddlFundName.Items.Insert(0, li);
                ViewState["MFSchemeTable"] = mfSchemeTable;
            }
        }

        public void LoadPortfolioFundNameList()
        {
            if (ddlPortfolioMF.SelectedIndex > 0)
            {
                if (ddlFundHouse.Items.Count > 0)
                {
                    ddlFundHouse.SelectedIndex = 0;
                }
                ViewState["MFSchemeTable"] = null;

                ddlFundName.Enabled = true;
                ddlFundName.Items.Clear();
                textboxSchemeCode.Text = "";
                textboxSelectedFundName.Text = "";

                DataManager dataMgr = new DataManager();
                DataTable mfSchemeTable = dataMgr.getPortfolioSchemesTable(Int32.Parse(ddlPortfolioMF.SelectedValue));
                if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
                {
                    //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
                    ddlFundName.DataTextField = "SCHEMENAME";
                    ddlFundName.DataValueField = "SCHEMECODE";
                    ddlFundName.DataSource = mfSchemeTable;
                    ddlFundName.DataBind();

                    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                    ddlFundName.Items.Insert(0, li);
                    ViewState["MFSchemeTable"] = mfSchemeTable;
                }
            }
        }
        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedValue != "-1")
            {
                if (ddlPortfolioMF.Items.Count > 0)
                {
                    ddlPortfolioMF.SelectedIndex = 0;
                }
                LoadFundNameList();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund data not found for selected fund house. Please select another fund house.');", true);
            }
        }

        /// <summary>
        /// Get the list of unique fund names from selected portfolio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPortfolioMF_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPortfolioFundNameList();
            //if (ddlPortfolioMF.SelectedIndex > 0)
            //{
            //    ddlFundHouse.SelectedIndex = 0;

            //    ViewState["MFSchemeTable"] = null;

            //    ddlFundName.Enabled = true;
            //    ddlFundName.Items.Clear();
            //    textboxSchemeCode.Text = "";

            //    DataManager dataMgr = new DataManager();
            //    DataTable mfSchemeTable = dataMgr.getPortfolioSchemesTable(Int32.Parse(ddlPortfolioMF.SelectedValue));
            //    if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
            //    {
            //        //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
            //        ddlFundName.DataTextField = "SCHEMENAME";
            //        ddlFundName.DataValueField = "SCHEMECODE";
            //        ddlFundName.DataSource = mfSchemeTable;
            //        ddlFundName.DataBind();

            //        ListItem li = new ListItem("-- Select Fund Name --", "-1");
            //        ddlFundName.Items.Insert(0, li);
            //        ViewState["MFSchemeTable"] = mfSchemeTable;
            //    }
            //}
        }

        protected void buttonSearchFUndName_Click(object sender, EventArgs e)
        {
            if (ViewState["MFSchemeTable"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund house and then search for fund name');", true);
            }
            else
            {
                DataTable fundNameTable = (DataTable)ViewState["MFSchemeTable"];
                StringBuilder filter = new StringBuilder();
                if (!(string.IsNullOrEmpty(textboxSelectedFundName.Text)))
                    filter.Append("SCHEMENAME Like '%" + textboxSelectedFundName.Text + "%'");
                DataView dv = fundNameTable.DefaultView;
                dv.RowFilter = filter.ToString();
                if (dv.Count > 0)
                {
                    ddlFundName.Items.Clear();
                    ddlFundName.DataTextField = "SCHEMENAME";
                    ddlFundName.DataValueField = "SCHEMECODE";
                    ddlFundName.DataSource = dv;//mfFundList.DefaultView;
                    ddlFundName.DataBind();
                    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                    ddlFundName.Items.Insert(0, li);
                }
            }
        }

        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundName.SelectedValue != "-1")
            {
                textboxSchemeCode.Text = ddlFundName.SelectedValue;
                textboxSelectedFundName.Text = ddlFundName.Items[ddlFundName.SelectedIndex].Text;
                OnDoEventShowGraph.Invoke();
            }
        }


        #endregion
    }
}