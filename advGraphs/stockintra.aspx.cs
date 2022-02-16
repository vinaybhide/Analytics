using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics.advGraphs
{
    public partial class stockintra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                Master.OnDoEventShowGraph += new advancegraphs.DoEventShowGraph(buttonShowGraph_Click);
                Master.OnDoEventShowGrid += new advancegraphs.DoEventShowGrid(buttonShowGrid_Click);
                //Master.OnDoEventToggleDesc += new advancegraphs.DoEventToggleDesc(buttonDesc_Click);
                //Master.OnDoEventToggleParameters += new advancegraphs.DoEventToggleParameters(buttonShowHideParam_Click);
                Master.OnDoEventRemoveSelectedIndicatorGraph += new advancegraphs.DoEventRemoveSelectedIndicatorGraph(buttonRemoveSelectedIndicatorGraph_Click);
                Master.OnDoEventShowSelectedIndicatorGraph += new advancegraphs.DoEventShowSelectedIndicatorGraph(buttonShowSelectedIndicatorGraph_Click);
                if (!IsPostBack)
                {
                    ViewState["MAIN_DATA"] = null;
                    ViewState["VALUATION_DATA"] = null;

                    ManagePanels();
                    fillDesc();
                    fillGraphList();

                    //if caller to this page may send us stock in querystring for which back test needs to be done, else user will select stock on this page
                    //first load stock & portfolio list
                    Master.LoadStockMaster();
                    Master.LoadPortfolioList();

                    if (Request.QueryString["outputsize"] != null)
                    {
                        Master.dropdownOutputSize.SelectedValue = Request.QueryString["outputsize"].ToString();
                    }
                    if (Request.QueryString["seriestype"] != null)
                    {
                        Master.dropdownSeries.SelectedValue = Request.QueryString["seriestype"];
                    }
                    if (Request.QueryString["interval"] != null)
                    {
                        Master.dropdownInterval.SelectedValue = Request.QueryString["interval"].ToString();
                    }

                    if (Request.QueryString["fromdate"] != null)
                    {
                        Master.textboxFromDate.Text = System.Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Master.textboxFromDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    }

                    if (Request.QueryString["todate"] != null)
                    {
                        //we do not need todate from caller, but still...
                        Master.textboxToDate.Text = System.Convert.ToDateTime(Request.QueryString["todate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Master.textboxToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        Master.textboxToDate.Enabled = false;
                    }

                    if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null) && (Request.QueryString["type"] != null))
                    {
                        if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
                        {
                            //Master.panelOnlineStocks.Visible = false;
                            Master.dropdownPortfolioList.SelectedValue = Session["STOCKPORTFOLIOMASTERROWID"].ToString();
                            Master.LoadPortfolioStockList();
                        }
                        //if caller sent symbol & exchange while calling then

                        Master.textbox_SelectedExchange.Text = Request.QueryString["exchange"].ToString();
                        Master.dropdownExchangeList.SelectedValue = Request.QueryString["exchange"].ToString();
                        Master.dropdownInvestmentTypeList.SelectedValue = Request.QueryString["type"].ToString();

                        Master.textbox_SelectedSymbol.Text = Request.QueryString["symbol"].ToString();

                        Master.dropdownStockList.SelectedValue = Master.textbox_SelectedSymbol.Text;

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);
                        ShowDaily();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);
                        Master.dropdownGraphList.SelectedValue = "OHLC";
                    }

                }
                this.Title = "Price Validator Graph";

                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    //ShowGraph(scriptName);
                    chartAdvGraph.Visible = true;
                    chartAdvGraph.Width = int.Parse(Master.panelWidth.Value);
                    chartAdvGraph.Height = int.Parse(Master.panelHeight.Value);
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Server.Transfer("~/Default.aspx");
                //Response.Redirect("~/Default.aspx");
            }
        }
        public void ManagePanels()
        {
            Master.InitializePanels(bStocksGraph: true);
            //now handle this graph specific panels

            Master.panelCommonParameters.Enabled = true;
            Master.panelCommonParameters.Visible = true;

            Master.panelMainParam.Enabled = true;
            Master.panelMainParam.Visible = true;

            Master.panelCommonParameters.Enabled = true;
            Master.panelCommonParameters.Visible = true;

            Master.labelToDate.Enabled = false;
            Master.labelToDate.Visible = false;
            Master.textboxToDate.Enabled = false;
            Master.textboxToDate.Visible = false;


            Master.dropdownInterval.Items.Clear();

            ListItem li;

            li = new ListItem("1 minute", "1m");
            Master.dropdownInterval.Items.Add(li);

            li = new ListItem("5 minute", "5m");
            li.Selected = true;
            Master.dropdownInterval.Items.Add(li);

            li = new ListItem("15 minute", "15m");
            Master.dropdownInterval.Items.Add(li);

            li = new ListItem("30 minute", "30m");
            Master.dropdownInterval.Items.Add(li);

            li = new ListItem("60 minute", "60m");
            Master.dropdownInterval.Items.Add(li);

            Master.labelSeriesType.Enabled = false;
            Master.labelSeriesType.Visible = false;
            Master.dropdownSeries.Enabled = false;
            Master.dropdownSeries.Visible = false;
        }
        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("Intraday means \"within the day.\" In the financial world, the term is shorthand used to describe securities that trade on the markets during regular business hours.");
            Master.bulletedlistDesc.Items.Add("Short-term traders typically use one-, five-, 15-, 30- and 60-minute intraday charts when trading within the market day.");
            Master.bulletedlistDesc.Items.Add("Typically, intraday scalping uses one- and five-minute charts for high-speed trading. Scalping is a strategy of transacting many trades per day that hopes to profit from small movements in a stock's price. ");
            Master.bulletedlistDesc.Items.Add("The most significant benefit of intraday trading is that positions are not affected by the possibility of negative overnight news that has the potential to impact the price of securities materially. Such news includes vital economic and earnings reports, as well as broker upgrades and downgrades that occur either before the market opens or after the market closes.");
        }
        public void fillGraphList()
        {
            Master.dropdownGraphList.Visible = true;

            Master.dropdownGraphList.Items.Clear();

            ListItem li;

            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("Open", "Open");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("High", "High");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("Low", "Low");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("Close", "Close");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("Volume", "Volume");
            Master.dropdownGraphList.Items.Add(li);

            if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            {
                if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null))
                {
                    li = new ListItem("Valuation :" + Request.QueryString["symbol"].ToString(),
                        Request.QueryString["symbol"].ToString());
                    Master.dropdownGraphList.Items.Add(li);
                }
            }

            li = new ListItem("BSE SENSEX", "^BSESN");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("NIFTY 50", "^NSEI");
            Master.dropdownGraphList.Items.Add(li);
        }
        public void ShowDaily()
        {
            DataTable dailyData = null;
            StockManager stockManager = new StockManager();

            string fromDate = Master.textboxFromDate.Text;

            string symbol = Master.textbox_SelectedSymbol.Text;
            string exchange = Master.textbox_SelectedExchange.Text;

            string outputsize = Master.dropdownOutputSize.SelectedValue;
            string interval = Master.dropdownInterval.SelectedValue;

            try
            {
                dailyData = stockManager.GetStockPriceData(symbol, exchange, outputsize: outputsize, time_interval: interval,
                                fromDate: fromDate);


                if (chartAdvGraph.Annotations.Count > 0)
                    chartAdvGraph.Annotations.Clear();

                ViewState["MAIN_DATA"] = null;

                if ((dailyData != null) && (dailyData.Rows.Count > 0))
                {
                    dailyData.Rows.RemoveAt(0);
                    dailyData.Rows.RemoveAt(dailyData.Rows.Count - 1);
                    GridViewData.DataSource = dailyData;
                    GridViewData.DataBind();

                    ViewState["MAIN_DATA"] = dailyData;

                    if (Master.dropdownInterval.SelectedValue.Contains("m"))
                    {
                        chartAdvGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "g";
                        chartAdvGraph.ChartAreas[1].AxisX.LabelStyle.Format = "g";
                    }
                    else
                    {
                        chartAdvGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "dd-MM-yyyy";
                        chartAdvGraph.ChartAreas[1].AxisX.LabelStyle.Format = "dd-MM-yyyy";
                    }

                    chartAdvGraph.DataSource = dailyData;
                    chartAdvGraph.DataBind();

                    if (chartAdvGraph.Series.FindByName("Open") != null)
                    {
                        if (Master.dropdownInterval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["Open"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["Open"].ToolTip = "Date:#VALX{g}; Open:#VALY";
                            chartAdvGraph.Series["Open"].PostBackValue = "Open," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartAdvGraph.Series["Open"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["Open"].ToolTip = "Date:#VALX; Open:#VALY";
                            chartAdvGraph.Series["Open"].PostBackValue = "Open," + symbol + ",#VALX,#VALY";
                        }
                    }
                    if (chartAdvGraph.Series.FindByName("High") != null)
                    {
                        if (Master.dropdownInterval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["High"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["High"].ToolTip = "Date:#VALX{g}; High:#VALY";
                            chartAdvGraph.Series["High"].PostBackValue = "High," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartAdvGraph.Series["High"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["High"].ToolTip = "Date:#VALX; High:#VALY";
                            chartAdvGraph.Series["High"].PostBackValue = "High," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartAdvGraph.Series.FindByName("Low") != null)
                    {
                        if (Master.dropdownInterval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["Low"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["Low"].ToolTip = "Date:#VALX{g}; Low:#VALY";
                            chartAdvGraph.Series["Low"].PostBackValue = "Low," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartAdvGraph.Series["Low"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["Low"].ToolTip = "Date:#VALX; High:#VALY";
                            chartAdvGraph.Series["Low"].PostBackValue = "Low," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartAdvGraph.Series.FindByName("Close") != null)
                    {
                        if (Master.dropdownInterval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["Close"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["Close"].ToolTip = "Date:#VALX{g}; Close:#VALY";
                            chartAdvGraph.Series["Close"].PostBackValue = "Close," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartAdvGraph.Series["Close"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["Close"].ToolTip = "Date:#VALX; Close:#VALY";
                            chartAdvGraph.Series["Close"].PostBackValue = "Close," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartAdvGraph.Series.FindByName("OHLC") != null)
                    {
                        chartAdvGraph.Series.FindByName("OHLC").Enabled = true;
                        if (Master.dropdownInterval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["OHLC"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["OHLC"].ToolTip = "Date:#VALX{g}; OHLC:#VALY";
                            chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                        }
                        else
                        {
                            chartAdvGraph.Series["OHLC"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["OHLC"].ToolTip = "Date:#VALX; OHLC:#VALY";
                            chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + ",#VALX,#VALY";
                        }
                    }
                    if (chartAdvGraph.Series.FindByName("Volume") != null)
                    {
                        chartAdvGraph.Series.FindByName("Volume").Enabled = true;
                        if (Master.dropdownInterval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["Volume"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["Volume"].ToolTip = "Date:#VALX{g}; Volume:#VALY";
                            chartAdvGraph.Series["Volume"].PostBackValue = "Volume," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartAdvGraph.Series["Volume"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["Volume"].ToolTip = "Date:#VALX; Volume:#VALY";
                            chartAdvGraph.Series["Volume"].PostBackValue = "Volume," + symbol + ",#VALX,#VALY";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void ShowSymbolValuation()
        {
            string symbol = Master.textbox_SelectedSymbol.Text;
            string exchange = Master.textbox_SelectedExchange.Text;
            string expression = "";
            DataTable tempData = null, valuationTable = null;
            DataRow[] filteredRows = null;
            int portfolioTxnNumber = 1;

            string fromDate = Master.textboxFromDate.Text;
            StockManager stockManager = new StockManager();
            ViewState["VALUATION_DATA"] = null;


            if (Master.dropdownPortfolioList.SelectedIndex > 0)
            {
                tempData = stockManager.GetPortfolio_ValuationLineGraph(Master.dropdownPortfolioList.SelectedValue);

                expression = "EXCHANGE = '" + exchange + "' and SYMBOL = '" + symbol + "'";

                filteredRows = tempData.Select(expression);
                if ((filteredRows != null) && (filteredRows.Length > 0))
                {
                    valuationTable = (DataTable)filteredRows.CopyToDataTable();
                }
            }

            if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
            {

                ViewState["VALUATION_DATA"] = valuationTable;

                if (chartAdvGraph.Series.FindByName(symbol) == null)
                {
                    chartAdvGraph.Series.Add(symbol);

                    chartAdvGraph.Series[symbol].Name = symbol; // "Portfolio";
                    chartAdvGraph.Series[symbol].ChartType = SeriesChartType.Line;
                    chartAdvGraph.Series[symbol].ChartArea = chartAdvGraph.ChartAreas[0].Name;
                    chartAdvGraph.Series[symbol].Legend = chartAdvGraph.Legends[0].Name;

                    chartAdvGraph.Series[symbol].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series[symbol].YAxisType = AxisType.Primary;

                    chartAdvGraph.Series[symbol].XValueType = ChartValueType.DateTime;
                    chartAdvGraph.Series[symbol].YValueType = ChartValueType.Double;
                }
                else
                {
                    chartAdvGraph.Series[symbol].Enabled = true;
                }
                chartAdvGraph.Series[symbol].LegendText = symbol;//valuationTable.Rows[0]["SYMBOL"].ToString() + "." + valuationTable.Rows[0]["EXCHANGE"].ToString();
                chartAdvGraph.Series[symbol].LegendToolTip = symbol;// valuationTable.Rows[0]["SYMBOL"].ToString();
                chartAdvGraph.Series[symbol].ToolTip = "Portfolio: " + "Date:#VALX{g}; Close:#VALY (Click to see details)";
                chartAdvGraph.Series[symbol].PostBackValue = "Portfolio:" + ",#VALX{g},#VALY";

                chartAdvGraph.Series[symbol].Points.Clear();

                for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                {
                    //(chartAdvGraph.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                    chartAdvGraph.Series[symbol].Points.AddXY(valuationTable.Rows[rownum]["TIMESTAMP"], valuationTable.Rows[rownum]["CLOSE"]);
                    chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].PostBackValue =
                                        "Portfolio," +
                                        valuationTable.Rows[rownum]["SYMBOL"] + "," + valuationTable.Rows[rownum]["TIMESTAMP"] + "," +
                                        valuationTable.Rows[rownum]["CLOSE"] + "," +
                                        valuationTable.Rows[rownum]["PURCHASE_DATE"] + "," + valuationTable.Rows[rownum]["PURCHASE_PRICE"] + "," +
                                        valuationTable.Rows[rownum]["PURCHASE_QTY"] + "," +
                                        valuationTable.Rows[rownum]["INVESTMENT_COST"] + "," + valuationTable.Rows[rownum]["CumulativeQty"] + "," +
                                        valuationTable.Rows[rownum]["CumulativeCost"] + "," + valuationTable.Rows[rownum]["CumulativeValue"];

                    //if (valuationTable.Rows[rownum]["PURCHASE_DATE"].ToString().Equals(valuationTable.Rows[rownum]["TIMESTAMP"].ToString())) // || ((rownum + 1) == valuationTable.Rows.Count))
                    if ((valuationTable.Rows[rownum]["PORTFOLIO_FLAG"].Equals("True")) || ((rownum + 1) == valuationTable.Rows.Count))
                    {
                        chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].MarkerSize = 11;
                        chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].MarkerStyle = MarkerStyle.Diamond;
                        chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].MarkerColor = Color.Black;
                        chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                    }
                }
                chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].MarkerSize = 10;
                chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].MarkerStyle = MarkerStyle.Diamond;
                chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].MarkerColor = Color.Black;
                chartAdvGraph.Series[symbol].Points[chartAdvGraph.Series[symbol].Points.Count - 1].ToolTip = "Click to see latest valuation";

            }
        }
        public void ShowBSE()
        {
            StockManager stockManager = new StockManager();
            string fromDate = Master.textboxFromDate.Text;
            string outputsize = Master.dropdownOutputSize.SelectedValue;
            string interval = Master.dropdownInterval.SelectedValue;

            DataTable sensexTable = stockManager.GetStockPriceData("^BSESN", outputsize: outputsize, time_interval: interval, fromDate: fromDate);
            if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
            {
                if (chartAdvGraph.Series.FindByName("^BSESN") == null)
                {
                    chartAdvGraph.Series.Add("^BSESN");

                    chartAdvGraph.Series["^BSESN"].Name = "^BSESN";
                    (chartAdvGraph.Series["^BSESN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartAdvGraph.Series["^BSESN"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["^BSESN"].Legend = chartAdvGraph.Legends[0].Name;
                    chartAdvGraph.Series["^BSESN"].LegendText = "BSE SENSEX";
                    chartAdvGraph.Series["^BSESN"].LegendToolTip = "BSE SENSEX";

                    chartAdvGraph.Series["^BSESN"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["^BSESN"].YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["^BSESN"]).YValuesPerPoint = 4;

                    if (Master.dropdownInterval.SelectedValue.Contains("m"))
                    {
                        chartAdvGraph.Series["^BSESN"].XValueType = ChartValueType.DateTime;

                        chartAdvGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX{g}; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    else
                    {
                        chartAdvGraph.Series["^BSESN"].XValueType = ChartValueType.Date;

                        chartAdvGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                }
                else
                {
                    chartAdvGraph.Series["^BSESN"].Enabled = true;
                }
                (chartAdvGraph.Series["^BSESN"]).Points.Clear();
                (chartAdvGraph.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
            }
        }

        public void ShowNSE()
        {
            StockManager stockManager = new StockManager();
            string fromDate = Master.textboxFromDate.Text;
            string outputsize = Master.dropdownOutputSize.SelectedValue;
            string interval = Master.dropdownInterval.SelectedValue;

            DataTable niftyTable = stockManager.GetStockPriceData("^NSEI", outputsize: outputsize, time_interval: interval, fromDate: fromDate);

            if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
            {
                if (chartAdvGraph.Series.FindByName("^NSEI") == null)
                {
                    chartAdvGraph.Series.Add("^NSEI");

                    chartAdvGraph.Series["^NSEI"].Name = "^NSEI";
                    chartAdvGraph.Series["^NSEI"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    chartAdvGraph.Series["^NSEI"].ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["^NSEI"].Legend = chartAdvGraph.Legends[0].Name;
                    chartAdvGraph.Series["^NSEI"].LegendText = "NIFTY 50";
                    chartAdvGraph.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                    chartAdvGraph.Series["^NSEI"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["^NSEI"].YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["^NSEI"]).YValuesPerPoint = 4;

                    if (Master.dropdownInterval.SelectedValue.Contains("m"))
                    {
                        chartAdvGraph.Series["^NSEI"].XValueType = ChartValueType.DateTime;

                        chartAdvGraph.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX{g}; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^NSEI"].PostBackValue = "^NSEI," + "NIFTY50" + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    else
                    {
                        chartAdvGraph.Series["^NSEI"].XValueType = ChartValueType.Date;

                        chartAdvGraph.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^NSEI"].PostBackValue = "^NSEI," + "NIFTY50" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                }
                else
                {
                    chartAdvGraph.Series["^NSEI"].Enabled = true;
                }
                chartAdvGraph.Series["^NSEI"].Points.Clear();
                chartAdvGraph.Series["^NSEI"].Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
            }
        }
        public void buttonShowSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false;
            string symbol = Master.textbox_SelectedSymbol.Text;
            string exchange = Master.textbox_SelectedExchange.Text;

            if (chartAdvGraph.Series.FindByName(graphName) != null)
            {
                chartAdvGraph.Series[graphName].Enabled = true;
            }
            else if (graphName.Equals("^BSESN"))
            {
                ShowBSE();
                chartAdvGraph.Series["^BSESN"].Enabled = true;
            }
            else if (graphName.Equals("^NSEI"))
            {
                ShowNSE();
                chartAdvGraph.Series["^NSEI"].Enabled = true;
            }
            else if (graphName.Equals(symbol))
            {
                ShowSymbolValuation();
                chartAdvGraph.Series[symbol].Enabled = true;
            }

            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                if ((chartAdvGraph.Series.FindByName(Master.dropdownGraphList.Items[i].Value) != null) &&
                    (chartAdvGraph.Series[Master.dropdownGraphList.Items[i].Value].Enabled))
                {
                    if (Master.dropdownGraphList.Items[i].Value.Equals("Volume"))
                    {
                        bArea1 = true;
                    }
                    else
                    {
                        bArea0 = true;
                    }
                }
            }
            chartAdvGraph.ChartAreas[0].Visible = bArea0;
            chartAdvGraph.ChartAreas[1].Visible = bArea1;
        }
        public void buttonRemoveSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false;
            string symbol = Master.textbox_SelectedSymbol.Text;
            string exchange = Master.textbox_SelectedExchange.Text;

            if (chartAdvGraph.Series.FindByName(graphName) != null)
            {
                chartAdvGraph.Series[graphName].Enabled = false;
            }

            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                if ((chartAdvGraph.Series.FindByName(Master.dropdownGraphList.Items[i].Value) != null) &&
                    (chartAdvGraph.Series[Master.dropdownGraphList.Items[i].Value].Enabled))
                {
                    if (Master.dropdownGraphList.Items[i].Value.Equals("Volume"))
                    {
                        bArea1 = true;
                    }
                    else
                    {
                        bArea0 = true;
                    }
                }
            }
            chartAdvGraph.ChartAreas[0].Visible = bArea0;
            chartAdvGraph.ChartAreas[1].Visible = bArea1;
        }
        public void buttonShowGraph_Click()
        {
            ShowDaily();

            if (chartAdvGraph.Series.FindByName("^BSESN") != null)// && (chartAdvGraph.Series["^BSESN"].Enabled))
            {
                ShowBSE();
            }
            if (chartAdvGraph.Series.FindByName("^NSEI") != null)// && (chartAdvGraph.Series["^NSEI"].Enabled))
            {
                ShowNSE();
            }

            //if we were called from portfolio page
            string portfolioentry = "";
            //if we were called from portfolio page then we need to remove the current valuation graph
            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                //find list item with valuation
                if (Master.dropdownGraphList.Items[i].Text.Contains("Valuation :"))
                {
                    //we found an entry that means there was a valuation graph existing, save its schemecode
                    portfolioentry = Master.dropdownGraphList.Items[i].Value; //this gives us existing symbol.exchange

                    //we need to handle case where user may use load stocks from exchange, where portfolio is unselected (done at exchange selection change)
                    if (Master.dropdownPortfolioList.SelectedIndex > 0)
                    {
                        //we will change the current list item to show new stock selection
                        Master.dropdownGraphList.Items[i].Text = "Valuation :" + Master.textbox_SelectedSymbol.Text;
                        Master.dropdownGraphList.Items[i].Value = Master.textbox_SelectedSymbol.Text;
                    }
                    break;
                }
            }
            if (portfolioentry.Equals(string.Empty) == false)
            {
                //means we had an entry in the graph list for symbol in a portfolio
                if (chartAdvGraph.Series.FindByName(portfolioentry) != null)
                {
                    //first remove the existing valuation graph for old symbol.exchange
                    chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName(portfolioentry));
                    //now add the new valuation graph
                    if (Master.dropdownPortfolioList.SelectedIndex > 0)
                    {
                        //if portfolio is selected then we need to show the valuation graph
                        ShowSymbolValuation();
                    }
                }
            }
        }
        protected void buttonShowGrid_Click()
        {
            GridViewData.Enabled = !GridViewData.Enabled;
            GridViewData.Visible = !GridViewData.Visible;
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["MAIN_DATA"];
            GridViewData.DataBind();
        }

        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
        protected void chartAdvGraph_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartAdvGraph.Annotations.Count > 0)
                    chartAdvGraph.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                seriesName = postBackValues[0];

                xDate = System.Convert.ToDateTime(postBackValues[2]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[3]);

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("Volume"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                    //VA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                    //VA.IsInfinitive = true;
                }
                else
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[0].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    VA.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    ra.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[0].Name;
                    //VA.ClipToChartArea = chartAdvGraph.ChartAreas[0].Name;
                    //VA.IsInfinitive = true;
                }

                //HA.Name = seriesName;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                HA.ToolTip = postBackValues[3];
                chartAdvGraph.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                VA.ToolTip = postBackValues[2];
                chartAdvGraph.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                if (seriesName.Equals("OHLC"))
                {   //high,low,open,close
                    //"OHLC," + symbol + "," + "#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + "Open:" + postBackValues[5] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[6];
                }
                else if (seriesName.Equals("Portfolio"))
                {
                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase Price:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];

                    HA.ToolTip = "Close: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("^BSESN") || seriesName.Equals("^NSEI"))
                {
                    ra.Text = seriesName + "\n" + "Date:" + postBackValues[2] + "\n" + "Close:" + postBackValues[3] + "\n" + "Open:" + postBackValues[4] + "\n" +
                        "High:" + postBackValues[5] + "\n" + "Low:" + postBackValues[6];
                }
                else
                {
                    //0-Volume, 1-Date, 2-Volume/Open/High/Low/Close
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + seriesName + ":" + postBackValues[3];
                }
                //ra.SmartLabelStyle = sl;

                chartAdvGraph.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }

    }
}