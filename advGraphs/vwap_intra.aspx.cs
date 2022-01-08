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

namespace Analytics
{
    public partial class vwap_intra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            Master.OnDoEventToggleParameters += new complexgraphs.DoEventToggleParameters(buttonShowHideParam_Click);
            Master.buttonShowHideParam.Visible = true;
            //this.Title = "Daily Price Graph";
            if (Session["EMAILID"] != null)
            {
                if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null) && (Request.QueryString["interval"] != null) &&
                    (Request.QueryString["seriestype"] != null) && (Request.QueryString["outputsize"] != null))
                {
                    this.Title = "VWAP : " + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();

                    if (!IsPostBack)
                    {
                        ViewState["FromDate"] = null;
                        ViewState["ToDate"] = null;
                        ViewState["FetchedData"] = null;
                        ViewState["VALUATION_TABLE"] = null;
                        ViewState["SENSEX"] = null;
                        ViewState["NIFTY50"] = null;

                        fillLinesCheckBoxes();
                        fillDesc();

                        ddl_Outputsize.SelectedValue = Request.QueryString["outputsize"].ToString();
                        ddl_SeriesType.SelectedValue = Request.QueryString["seriestype"].ToString();
                        ddl_Interval.SelectedValue = Request.QueryString["interval"].ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph();
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
                    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
                    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
                    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
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

        public void fillLinesCheckBoxes()
        {
            //Master.checkboxlistLines.Visible = false;
            //return;
            Master.checkboxlistLines.Visible = true;
            ListItem li = new ListItem("Open", "Open");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("High", "High");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Low", "Low");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Close", "Close");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Volume", "Volume");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("VWAP", "VWAP");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            {
                if (Request.QueryString["symbol"] != null)
                {
                    li = new ListItem("Valuation :" + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString(),
                        Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString());
                    li.Selected = false;
                    Master.checkboxlistLines.Items.Add(li);
                }
            }

            li = new ListItem("BSE SENSEX", "^BSESN");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("NIFTY 50", "^NSEI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);

        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("The volume weighted average price(VWAP) is a trading benchmark that gives the average price a security has traded at throughout the day, based on both volume and price.It is important because it provides you with insight into both the trend and value of a security.");
            Master.bulletedlistDesc.Items.Add("The VWAP represents the true average price of the stock and does not affect its closing price. The VWAP calculation is based on historical data so it is better suited for intraday trading.");
            Master.bulletedlistDesc.Items.Add("VWAP is a popular tool among investors because it can indicate if a market is bullish or bearish and whether it is a good time to sell or buy. The VWAP is also considered a superior tool to moving averages.");
            Master.bulletedlistDesc.Items.Add("One common strategy for a bullish trader is to wait for a clean VWAP cross above, then enter long. When there is a VWAP cross above, the stock shows that buyers may be stepping in, signaling there may be upward momentum. When a stock's price breaks above the VWAP, the previous time frame's VWAP can be thought of as a support level.");
            Master.bulletedlistDesc.Items.Add("If traders are bearish on a stock, they may look to short that stock on a VWAP cross below. This signals that buyers may be stepping away and taking profits, or there is a seller.");
            Master.bulletedlistDesc.Items.Add("A VWAP cross is a trading indicator that occurs when a security’s price crosses the volume-weighted average price (VWAP).");
            Master.bulletedlistDesc.Items.Add("Large institutional buyers will try to buy below the VWAP, or sell above it. This way their actions push the price back toward the average, instead of away from it.");
            Master.bulletedlistDesc.Items.Add("Traders may use VWAP as a trend confirmation tool, and build trading rules around it.");
            Master.bulletedlistDesc.Items.Add("For example, when the price is above VWAP they may prefer to initiate long positions.  When the price is below VWAP they may prefer to initiate short positions.");
        }

        public void FillData()
        {
            DataTable tempData = null;
            DataTable dailyData = null;
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataRow[] filteredRows = null;
            string expression = "";

            StockManager stockManager = new StockManager();
            string fromDate = null;

            string symbol = Request.QueryString["symbol"].ToString();
            string exchange = Request.QueryString["exchange"].ToString();

            string seriestype = ddl_SeriesType.SelectedValue;
            string outputsize = ddl_Outputsize.SelectedValue;
            string interval = ddl_Interval.SelectedValue;

            ViewState["FromDate"] = Master.textboxFromDate.Text;

            if (ViewState["FromDate"] != null)
                fromDate = ViewState["FromDate"].ToString();

            if ((fromDate != null) && (fromDate.Equals(string.Empty) == false))
            {
                expression = "TIMESTAMP >= '" + fromDate + "'";
            }

            //if we were called from portfolio page then get the portfolio data for selected scheme
            //if (Request.QueryString["schemecode"] != null)
            if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            {
                //if ((ddlShowHidePortfolio.SelectedIndex == 0) && ((ViewState["VALUATION_TABLE"] == null) || (((DataTable)ViewState["VALUATION_TABLE"]).Rows.Count == 0)))
                if ((ViewState["VALUATION_TABLE"] == null) || (((DataTable)ViewState["VALUATION_TABLE"]).Rows.Count == 0))
                {
                    tempData = stockManager.GetPortfolio_ValuationLineGraph(Session["STOCKPORTFOLIOMASTERROWID"].ToString(), interval);
                    if (expression == string.Empty)
                    {
                        expression = "SYMBOL = '" + symbol + "'";
                    }
                    else
                    {
                        expression += " and SYMBOL = '" + symbol + "'";
                    }
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                    {
                        ViewState["VALUATION_TABLE"] = (DataTable)filteredRows.CopyToDataTable();
                    }
                }
            }

            if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
            {
                dailyData = stockManager.getVWAPDataTableFromDaily(symbol, exchange, seriestype, outputsize, time_interval: interval,
                                fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                if (dailyData != null)
                {
                    ViewState["FetchedData"] = dailyData;
                }
            }

            if ((Master.checkboxlistLines.Items.FindByValue("^BSESN") != null) && (Master.checkboxlistLines.Items.FindByValue("^BSESN").Selected))
            {
                if ((ViewState["SENSEX"] == null) || (((DataTable)ViewState["SENSEX"]).Rows.Count <= 0))
                {
                    sensexTable = stockManager.GetStockPriceData("^BSESN", time_interval: interval,
                          fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                    ViewState["SENSEX"] = sensexTable;
                }
            }
            if ((Master.checkboxlistLines.Items.FindByValue("^NSEI") != null) && (Master.checkboxlistLines.Items.FindByValue("^NSEI").Selected))
            {
                if ((ViewState["NIFTY50"] == null) || (((DataTable)ViewState["NIFTY50"]).Rows.Count <= 0))
                {
                    niftyTable = stockManager.GetStockPriceData("^NSEI", time_interval: interval,
                          fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                    ViewState["NIFTY50"] = niftyTable;
                }
            }
        }
        public void ShowGraph()
        {
            DataTable scriptData = null, valuationTable = null, sensexTable = null, niftyTable = null;
            int portfolioTxnNumber = 1;
            Series tempSeries = null;

            string symbol = Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();

            try
            {
                FillData();

                scriptData = (DataTable)ViewState["FetchedData"];
                valuationTable = (DataTable)ViewState["VALUATION_TABLE"];
                sensexTable = (DataTable)ViewState["SENSEX"];
                niftyTable = (DataTable)ViewState["NIFTY50"];


                GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                GridViewData.DataBind();

                if (ddl_Interval.SelectedValue.Contains("m"))
                {
                    chartAdvGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "g";
                    chartAdvGraph.ChartAreas[1].AxisX.LabelStyle.Format = "g";
                    chartAdvGraph.ChartAreas[2].AxisX.LabelStyle.Format = "g";
                }
                else
                {
                    chartAdvGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "dd-MM-yyyy";
                    chartAdvGraph.ChartAreas[1].AxisX.LabelStyle.Format = "dd-MM-yyyy";
                    chartAdvGraph.ChartAreas[2].AxisX.LabelStyle.Format = "dd-MM-yyyy";
                }
                if (scriptData != null)
                {
                    chartAdvGraph.DataSource = scriptData;
                    chartAdvGraph.DataBind();
                    if (chartAdvGraph.Series.FindByName("Open") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                    if (chartAdvGraph.Series.FindByName("VWAP") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartAdvGraph.Series["VWAP"].XValueType = ChartValueType.DateTime;
                            chartAdvGraph.Series["VWAP"].ToolTip = "Date:#VALX{g}; VWAP:#VALY";
                            chartAdvGraph.Series["VWAP"].PostBackValue = "VWAP," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartAdvGraph.Series["VWAP"].XValueType = ChartValueType.Date;
                            chartAdvGraph.Series["VWAP"].ToolTip = "Date:#VALX; VWAP:#VALY";
                            chartAdvGraph.Series["VWAP"].PostBackValue = "VWAP," + symbol + ",#VALX,#VALY";
                        }
                    }
                }
                if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    if (chartAdvGraph.Series.FindByName(symbol) == null)
                    {
                        chartAdvGraph.Series.Add(symbol);

                        chartAdvGraph.Series[symbol].Name = symbol;
                        (chartAdvGraph.Series[symbol]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartAdvGraph.Series[symbol]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                        chartAdvGraph.Series[symbol].Legend = chartAdvGraph.Legends[0].Name;

                        (chartAdvGraph.Series[symbol]).XAxisType = AxisType.Secondary;
                        (chartAdvGraph.Series[symbol]).YAxisType = AxisType.Primary;

                        (chartAdvGraph.Series[symbol]).YValuesPerPoint = 1;
                        (chartAdvGraph.Series[symbol]).XValueMember = "TIMESTAMP";
                        (chartAdvGraph.Series[symbol]).XValueType = ChartValueType.DateTime;
                        (chartAdvGraph.Series[symbol]).YValueMembers = "CLOSE";
                        (chartAdvGraph.Series[symbol]).YValueType = ChartValueType.Double;

                        chartAdvGraph.Series[symbol].LegendText = symbol;
                        chartAdvGraph.Series[symbol].LegendToolTip = symbol;
                        chartAdvGraph.Series[symbol].ToolTip = symbol + ":  Date:#VALX{g}; CLOSE:#VALY (Click to see details)";
                        chartAdvGraph.Series[symbol].PostBackValue = symbol + "," + "#VALX{g},#VALY";
                    }
                    (chartAdvGraph.Series[symbol]).Points.Clear();
                    for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                    {
                        //(chartAdvGraph.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                        (chartAdvGraph.Series[symbol]).Points.AddXY(valuationTable.Rows[rownum]["TIMESTAMP"], valuationTable.Rows[rownum]["CLOSE"]);
                        (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].PostBackValue =
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
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerSize = 11;
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                        }
                    }
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerSize = 10;
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].ToolTip = "Click to see latest valuation";

                }
                else
                {
                    tempSeries = chartAdvGraph.Series.FindByName(symbol);
                    if (tempSeries != null)
                        chartAdvGraph.Series.Remove(tempSeries);
                }

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

                        (chartAdvGraph.Series["^BSESN"]).YValuesPerPoint = 4;

                        chartAdvGraph.Series["^BSESN"].XAxisType = AxisType.Secondary;
                        chartAdvGraph.Series["^BSESN"].YAxisType = AxisType.Primary;

                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                    chartAdvGraph.Series["^BSESN"].Points.Clear();
                    (chartAdvGraph.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartAdvGraph.Series.FindByName("^BSESN");
                    if (tempSeries != null)
                        chartAdvGraph.Series.Remove(tempSeries);
                }

                if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
                {
                    if (chartAdvGraph.Series.FindByName("^NSEI") == null)
                    {
                        chartAdvGraph.Series.Add("^NSEI");

                        chartAdvGraph.Series["^NSEI"].Name = "^NSEI";
                        (chartAdvGraph.Series["^NSEI"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartAdvGraph.Series["^NSEI"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                        chartAdvGraph.Series["^NSEI"].Legend = chartAdvGraph.Legends[0].Name;
                        chartAdvGraph.Series["^NSEI"].LegendText = "NIFTY 50";
                        chartAdvGraph.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                        (chartAdvGraph.Series["^NSEI"]).YValuesPerPoint = 4;

                        chartAdvGraph.Series["^NSEI"].XAxisType = AxisType.Secondary;
                        chartAdvGraph.Series["^NSEI"].YAxisType = AxisType.Primary;

                        if (ddl_Interval.SelectedValue.Contains("m"))
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
                    (chartAdvGraph.Series["^NSEI"]).Points.Clear();
                    (chartAdvGraph.Series["^NSEI"]).Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartAdvGraph.Series.FindByName("^NSEI");
                    if (tempSeries != null)
                        chartAdvGraph.Series.Remove(tempSeries);
                }

                foreach (ListItem item in Master.checkboxlistLines.Items)
                {
                    if (chartAdvGraph.Series.FindByName(item.Value) != null)
                    {
                        chartAdvGraph.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartAdvGraph.Annotations.FindByName(item.Value) != null)
                                chartAdvGraph.Annotations.Clear();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
            }
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
                    HA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[2].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[2].Name;
                    //VA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                    //VA.IsInfinitive = true;
                }
                else if (seriesName.Equals("VWAP"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX2;
                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX2;
                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX2;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                    //VA.ClipToChartArea = chartAdvGraph.ChartAreas[2].Name;
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

        public void buttonShowGraph_Click()
        {
            ViewState["FetchedData"] = null;
            ViewState["SENSEX"] = null;
            ViewState["NIFTY50"] = null;
            ViewState["VALUATION_TABLE"] = null;
            ShowGraph();
        }
        protected void buttonShowHideParam_Click()
        {
            panelParam.Visible = !panelParam.Visible;
        }

        void buttonShowGrid_Click()
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                GridViewData.Visible = true;
            }
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewData.DataBind();
        }

        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }

        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }

        //public void showCandleStickGraph(DataTable scriptData)
        //{
        //    //chartVWAP_Intra.DataSource = scriptData;
        //    chartVWAP_Intra.Series["OHLC"].Points.DataBind(scriptData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
        //    //chartVWAP_Intra.DataBind();
        //    chartVWAP_Intra.Series["OHLC"].XValueMember = "Date";
        //    chartVWAP_Intra.Series["OHLC"].XValueType = ChartValueType.DateTime;
        //    chartVWAP_Intra.Series["OHLC"].YValueMembers = "Open,High,Low,Close";

        //    chartVWAP_Intra.Series["OHLC"].BorderColor = System.Drawing.Color.Black;
        //    chartVWAP_Intra.Series["OHLC"].Color = System.Drawing.Color.Black;
        //    chartVWAP_Intra.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red";
        //    chartVWAP_Intra.Series["OHLC"].XValueType = ChartValueType.DateTime;
        //    chartVWAP_Intra.Series["OHLC"]["OpenCloseStyle"] = "Triangle";
        //    chartVWAP_Intra.Series["OHLC"]["ShowOpenClose"] = "Both";
        //    //chartVWAP_Intra.Series["OHLC"]["PriceDownColor"] = "Triangle";
        //    //chartVWAP_Intra.Series["OHLC"]["PriceUpColor"] = "Both";

        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.MajorGrid.LineWidth = 1;
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.MajorGrid.LineWidth = 1;
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Minimum = 0;
        //    //chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Maximum = chartdailyGraph.Series["OHLC"].Points.FindMaxByValue("Y1", 0).YValues[0];
        //    chartVWAP_Intra.DataManipulator.IsStartFromFirst = true;

        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.Title = "Date";
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Title = "OHLC";
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.LabelStyle.Format = "g";

        //    chartVWAP_Intra.Series["OHLC"].Enabled = true;

        //    if (chartVWAP_Intra.Annotations.Count > 0)
        //        chartVWAP_Intra.Annotations.Clear();
        //}

        //public void showVWAP(DataTable scriptData)
        //{
        //    chartVWAP_Intra.Series["VWAP"].Points.DataBind(scriptData.AsEnumerable(), "Date", "VWAP", "");
        //    (chartVWAP_Intra.Series["VWAP"]).XValueMember = "Date";
        //    (chartVWAP_Intra.Series["VWAP"]).XValueType = ChartValueType.DateTime;
        //    (chartVWAP_Intra.Series["VWAP"]).YValueMembers = "VWAP";
        //    //(chartVWAP_Intra.Series["VWAP"]).ToolTip = "VWAP: Date:#VALX;   Value:#VALY";

        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX2.Title = "Date";
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX2.TitleAlignment = System.Drawing.StringAlignment.Center;
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY2.Title = "VWAP";
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY2.TitleAlignment = System.Drawing.StringAlignment.Center;
        //    chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX2.LabelStyle.Format = "g";

        //    chartVWAP_Intra.Series["VWAP"].Enabled = true;

        //    //chartVWAP.Titles["titleVWAP"].Text = $"{"Volume Weighted Average Price - "}{scriptName}";
        //    if (chartVWAP_Intra.Annotations.Count > 0)
        //        chartVWAP_Intra.Annotations.Clear();
        //}
    }
}