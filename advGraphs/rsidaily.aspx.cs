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
    public partial class rsidaily : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            Master.OnDoEventToggleParameters += new complexgraphs.DoEventToggleParameters(buttonShowHideParam_Click);
            Master.buttonShowHideParam.Visible = true;

            if (Session["EMAILID"] != null)
            {
                if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null) && (Request.QueryString["period"] != null) &&
                    (Request.QueryString["seriestype"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["outputsize"] != null))
                {
                    this.Title = "Momentum Indicator: " + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();
                    if (!IsPostBack)
                    {
                        //ViewState["counter"] = 0;
                        ViewState["FromDate"] = null;
                        ViewState["ToDate"] = null;
                        ViewState["FetchedData"] = null;
                        ViewState["VALUATION_TABLE"] = null;
                        ViewState["SENSEX"] = null;
                        ViewState["NIFTY50"] = null;
                        fillDesc();
                        fillLinesCheckBoxes();
                        ddlRSIDaily_Outputsize.SelectedValue = Request.QueryString["outputsize"].ToString();
                        ddlRSIDaily_Interval.SelectedValue = Request.QueryString["interval"].ToString();
                        textboxPeriod.Text = Request.QueryString["period"].ToString();
                        ddlRSIDaily_SeriesType.SelectedValue = Request.QueryString["seriestype"].ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph();
                    //headingtext.InnerText = "RSI Vs Daily Price: " + Request.QueryString["script"].ToString();

                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
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
            ListItem li;

            li = new ListItem("RSI", "RSI");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Open", "OPEN");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("High", "HIGH");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Low", "LOW");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Close", "CLOSE");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);

            if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            {
                if (Request.QueryString["symbol"] != null)
                {
                    li = new ListItem("Valuation :" + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString(),
                        Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString());
                    li.Selected = true;
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
            Master.bulletedlistDesc.Items.Add("The relative strength index(RSI) is a momentum indicator used in technical analysis that measures the magnitude of recent price changes to evaluate overbought or oversold conditions in the price of a stock or other asset. The RSI is displayed as an oscillator (a line graph that moves between two extremes) and can have a reading from 0 to 100.");
            Master.bulletedlistDesc.Items.Add("RSI of 70 or above indicate that a security is becoming overbought or overvalued and may be primed for a trend reversal or corrective pullback in price.");
            Master.bulletedlistDesc.Items.Add("An RSI reading of 30 or below indicates an oversold or undervalued condition.");
        }

        public void FillData()
        {
            DataTable tempData = null;
            DataTable dailyCloseRSI = null;
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataRow[] filteredRows = null;
            string expression = "";

            StockManager stockManager = new StockManager();
            string fromDate = null;

            string symbol = Request.QueryString["symbol"].ToString();
            string exchange = Request.QueryString["exchange"].ToString();

            string interval = ddlRSIDaily_Interval.SelectedValue;
            string period = textboxPeriod.Text;
            string seriestype = ddlRSIDaily_SeriesType.SelectedValue;
            string outputsize = ddlRSIDaily_Outputsize.SelectedValue;

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
                    tempData = stockManager.GetPortfolio_ValuationLineGraph(Session["STOCKPORTFOLIOMASTERROWID"].ToString());
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
                dailyCloseRSI = stockManager.getRSIDataTableFromDaily(symbol, exchange, seriestype, outputsize, interval,
                                fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate, period: period);
                if (dailyCloseRSI != null)
                {
                    ViewState["FetchedData"] = dailyCloseRSI;
                }
            }

            if ((Master.checkboxlistLines.Items.FindByValue("^BSESN") != null) && (Master.checkboxlistLines.Items.FindByValue("^BSESN").Selected))
            {
                if ((ViewState["SENSEX"] == null) || (((DataTable)ViewState["SENSEX"]).Rows.Count <= 0))
                {
                    sensexTable = stockManager.GetStockPriceData("^BSESN",
                          fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                    ViewState["SENSEX"] = sensexTable;
                }
            }
            if ((Master.checkboxlistLines.Items.FindByValue("^NSEI") != null) && (Master.checkboxlistLines.Items.FindByValue("^NSEI").Selected))
            {
                if ((ViewState["NIFTY50"] == null) || (((DataTable)ViewState["NIFTY50"]).Rows.Count <= 0))
                {
                    niftyTable = stockManager.GetStockPriceData("^NSEI",
                          fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                    ViewState["NIFTY50"] = niftyTable;
                }
            }
        }

        public void ShowGraph()
        {
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataTable valuationTable = null;
            DataTable dailyrsiTable = null;
            Series tempSeries = null;

            int portfolioTxnNumber = 1;

            string symbol = Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();
            //string exchange = Request.QueryString["exchange"].ToString();
            //string period = Request.QueryString["period"].ToString();
            //string seriestype = Request.QueryString["seriestype"].ToString();
            //string interval = Request.QueryString["interval"].ToString();
            //string outputsize = Request.QueryString["outputsize"].ToString();

            FillData();

            dailyrsiTable = (DataTable)ViewState["FetchedData"];
            valuationTable = (DataTable)ViewState["VALUATION_TABLE"];
            sensexTable = (DataTable)ViewState["SENSEX"];
            niftyTable = (DataTable)ViewState["NIFTY50"];


            GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewDaily.DataBind();
            GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewData.DataBind();

            if (chartAdvGraph.Annotations.Count > 0)
                chartAdvGraph.Annotations.Clear();

            if ((dailyrsiTable != null) && (dailyrsiTable.Rows.Count > 0))
            {
                if (chartAdvGraph.Series.FindByName("OPEN") == null)
                {
                    chartAdvGraph.Series.Add("OPEN");

                    chartAdvGraph.Series["OPEN"].Name = "OPEN";
                    (chartAdvGraph.Series["OPEN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartAdvGraph.Series["OPEN"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["OPEN"].Legend = chartAdvGraph.Legends[0].Name;

                    (chartAdvGraph.Series["OPEN"]).XAxisType = AxisType.Secondary;
                    (chartAdvGraph.Series["OPEN"]).YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["OPEN"]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series["OPEN"]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series["OPEN"]).YValueMembers = "OPEN";
                    (chartAdvGraph.Series["OPEN"]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series["OPEN"].LegendText = "OPEN";
                    chartAdvGraph.Series["OPEN"].LegendToolTip = "OPEN";
                    chartAdvGraph.Series["OPEN"].ToolTip = "OPEN Price: Date:#VALX; OPEN:#VALY (Click to see details)";
                    chartAdvGraph.Series["OPEN"].PostBackValue = "OPEN," + symbol + ",#VALX,#VALY";
                }

                (chartAdvGraph.Series["OPEN"]).Points.Clear();
                (chartAdvGraph.Series["OPEN"]).Points.DataBind(dailyrsiTable.AsEnumerable(), "TIMESTAMP", "OPEN", "");

                if (chartAdvGraph.Series.FindByName("HIGH") == null)
                {
                    chartAdvGraph.Series.Add("HIGH");

                    chartAdvGraph.Series["HIGH"].Name = "HIGH";
                    (chartAdvGraph.Series["HIGH"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartAdvGraph.Series["HIGH"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["HIGH"].Legend = chartAdvGraph.Legends[0].Name;

                    (chartAdvGraph.Series["HIGH"]).XAxisType = AxisType.Secondary;
                    (chartAdvGraph.Series["HIGH"]).YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["HIGH"]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series["HIGH"]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series["HIGH"]).YValueMembers = "HIGH";
                    (chartAdvGraph.Series["HIGH"]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series["HIGH"].LegendText = "HIGH";
                    chartAdvGraph.Series["HIGH"].LegendToolTip = "HIGH";
                    chartAdvGraph.Series["HIGH"].ToolTip = "HIGH Price: Date:#VALX; HIGH:#VALY (Click to see details)";
                    chartAdvGraph.Series["HIGH"].PostBackValue = "HIGH,"+symbol + ",#VALX,#VALY";
                }

                (chartAdvGraph.Series["HIGH"]).Points.Clear();
                (chartAdvGraph.Series["HIGH"]).Points.DataBind(dailyrsiTable.AsEnumerable(), "TIMESTAMP", "HIGH", "");

                if (chartAdvGraph.Series.FindByName("LOW") == null)
                {
                    chartAdvGraph.Series.Add("LOW");

                    chartAdvGraph.Series["LOW"].Name = "LOW";
                    (chartAdvGraph.Series["LOW"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartAdvGraph.Series["LOW"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["LOW"].Legend = chartAdvGraph.Legends[0].Name;

                    (chartAdvGraph.Series["LOW"]).XAxisType = AxisType.Secondary;
                    (chartAdvGraph.Series["LOW"]).YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["LOW"]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series["LOW"]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series["LOW"]).YValueMembers = "LOW";
                    (chartAdvGraph.Series["LOW"]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series["LOW"].LegendText = "LOW";
                    chartAdvGraph.Series["LOW"].LegendToolTip = "LOW";
                    chartAdvGraph.Series["LOW"].ToolTip = "LOW Price: Date:#VALX; LOW:#VALY (Click to see details)";
                    chartAdvGraph.Series["LOW"].PostBackValue = "LOW," + symbol + ",#VALX,#VALY";
                }

                (chartAdvGraph.Series["LOW"]).Points.Clear();
                (chartAdvGraph.Series["LOW"]).Points.DataBind(dailyrsiTable.AsEnumerable(), "TIMESTAMP", "LOW", "");


                if (chartAdvGraph.Series.FindByName("CLOSE") == null)
                {
                    chartAdvGraph.Series.Add("CLOSE");

                    chartAdvGraph.Series["CLOSE"].Name = "CLOSE";
                    (chartAdvGraph.Series["CLOSE"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartAdvGraph.Series["CLOSE"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["CLOSE"].Legend = chartAdvGraph.Legends[0].Name;

                    (chartAdvGraph.Series["CLOSE"]).XAxisType = AxisType.Secondary;
                    (chartAdvGraph.Series["CLOSE"]).YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["CLOSE"]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series["CLOSE"]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series["CLOSE"]).YValueMembers = "CLOSE";
                    (chartAdvGraph.Series["CLOSE"]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series["CLOSE"].LegendText = "CLOSE";
                    chartAdvGraph.Series["CLOSE"].LegendToolTip = "CLOSE";
                    chartAdvGraph.Series["CLOSE"].ToolTip = "CLOSE Price: Date:#VALX; CLOSE:#VALY (Click to see details)";
                    chartAdvGraph.Series["CLOSE"].PostBackValue = "CLOSE," + symbol + ",#VALX,#VALY";
                }

                (chartAdvGraph.Series["CLOSE"]).Points.Clear();
                (chartAdvGraph.Series["CLOSE"]).Points.DataBind(dailyrsiTable.AsEnumerable(), "TIMESTAMP", "CLOSE", "");

                if (chartAdvGraph.Series.FindByName("OHLC") == null)
                {
                    chartAdvGraph.Series.Add("OHLC");

                    chartAdvGraph.Series["OHLC"].Name = "OHLC";
                    (chartAdvGraph.Series["OHLC"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Candlestick;
                    (chartAdvGraph.Series["OHLC"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                    chartAdvGraph.Series["OHLC"].Legend = chartAdvGraph.Legends[0].Name;

                    (chartAdvGraph.Series["OHLC"]).XAxisType = AxisType.Secondary;
                    (chartAdvGraph.Series["OHLC"]).YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["OHLC"]).YValuesPerPoint = 4;
                    (chartAdvGraph.Series["OHLC"]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series["OHLC"]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series["OHLC"]).YValueMembers = "HIGH,LOW,OPEN,CLOSE";
                    (chartAdvGraph.Series["OHLC"]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series["OHLC"].LegendText = "OHLC";
                    chartAdvGraph.Series["OHLC"].LegendToolTip = "OHLC";
                    chartAdvGraph.Series["OHLC"].ToolTip = "OHLC: Date:#VALX; Open:#VALY1,High:#VALY2,Low:#VALY3,Close:#VALY4 (Click to see details)";
                    chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";

                    chartAdvGraph.Series["OHLC"].BorderColor = System.Drawing.Color.Black;
                    chartAdvGraph.Series["OHLC"].Color = System.Drawing.Color.Black;
                    chartAdvGraph.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red";
                    chartAdvGraph.Series["OHLC"]["OpenCloseStyle"] = "Triangle";
                    chartAdvGraph.Series["OHLC"]["ShowOpenClose"] = "Both";
                }
                (chartAdvGraph.Series["OHLC"]).Points.Clear();
                (chartAdvGraph.Series["OHLC"]).Points.DataBind(dailyrsiTable.AsEnumerable(), "TIMESTAMP", "HIGH,LOW,OPEN,CLOSE", "");


                if (chartAdvGraph.Series.FindByName("RSI") == null)
                {
                    chartAdvGraph.Series.Add("RSI");

                    chartAdvGraph.Series["RSI"].Name = "RSI";
                    (chartAdvGraph.Series["RSI"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartAdvGraph.Series["RSI"]).ChartArea = chartAdvGraph.ChartAreas[1].Name;

                    chartAdvGraph.Series["RSI"].Legend = chartAdvGraph.Legends[0].Name;

                    (chartAdvGraph.Series["RSI"]).XAxisType = AxisType.Primary;
                    (chartAdvGraph.Series["RSI"]).YAxisType = AxisType.Primary;

                    (chartAdvGraph.Series["RSI"]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series["RSI"]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series["RSI"]).YValueMembers = "RSI";
                    (chartAdvGraph.Series["RSI"]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series["RSI"].LegendText = "RSI";
                    chartAdvGraph.Series["RSI"].LegendToolTip = "RSI";
                    chartAdvGraph.Series["RSI"].ToolTip = "Date:#VALX; RSI:#VALY (Click to see details)";
                    chartAdvGraph.Series["RSI"].PostBackValue = "RSI," + symbol + ",#VALX,#VALY";
                }

                (chartAdvGraph.Series["RSI"]).Points.Clear();
                (chartAdvGraph.Series["RSI"]).Points.DataBind(dailyrsiTable.AsEnumerable(), "TIMESTAMP", "RSI_CLOSE", "");
            }
            else
            {
                tempSeries = chartAdvGraph.Series.FindByName("OPEN");
                if (tempSeries != null)
                    chartAdvGraph.Series.Remove(tempSeries);
                tempSeries = chartAdvGraph.Series.FindByName("HIGH");
                if (tempSeries != null)
                    chartAdvGraph.Series.Remove(tempSeries);
                tempSeries = chartAdvGraph.Series.FindByName("LOW");
                if (tempSeries != null)
                    chartAdvGraph.Series.Remove(tempSeries);
                tempSeries = chartAdvGraph.Series.FindByName("CLOSE");
                if (tempSeries != null)
                    chartAdvGraph.Series.Remove(tempSeries);
                tempSeries = chartAdvGraph.Series.FindByName("OHLC");
                if (tempSeries != null)
                    chartAdvGraph.Series.Remove(tempSeries);
                tempSeries = chartAdvGraph.Series.FindByName("RSI");
                if (tempSeries != null)
                    chartAdvGraph.Series.Remove(tempSeries);
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

                    (chartAdvGraph.Series[symbol]).XValueMember = "TIMESTAMP";
                    (chartAdvGraph.Series[symbol]).XValueType = ChartValueType.Date;
                    (chartAdvGraph.Series[symbol]).YValueMembers = "CLOSE";
                    (chartAdvGraph.Series[symbol]).YValueType = ChartValueType.Double;

                    chartAdvGraph.Series[symbol].LegendText = symbol;
                    chartAdvGraph.Series[symbol].LegendToolTip = symbol;
                    chartAdvGraph.Series[symbol].ToolTip = "CLOSE Price: Date:#VALX; CLOSE:#VALY (Click to see details)";
                    chartAdvGraph.Series[symbol].PostBackValue = symbol +"," + "#VALX,#VALY";
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
                    chartAdvGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                    chartAdvGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                }
                (chartAdvGraph.Series["^BSESN"]).Points.Clear();
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
                    chartAdvGraph.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                    chartAdvGraph.Series["^NSEI"].PostBackValue = "^NSEI," + "NIFTY50" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
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
        //public void ShowGraph(string scriptName)
        //{
        //    DataTable rsiData = null;
        //    DataTable tempData = null;
        //    string expression = "";
        //    string outputSize;
        //    string interval;
        //    string period;
        //    string series_type;
        //    string fromDate = "", toDate = "";
        //    DataRow[] filteredRows = null;

        //    try
        //    {
        //        if (((ViewState["FetchedDataDaily"] == null) || (ViewState["FetchedDataRSI"] == null))
        //             ||
        //            ((((DataTable)ViewState["FetchedDataDaily"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataRSI"]).Rows.Count == 0))
        //           )
        //        {
        //            if (Session["IsTestOn"] != null)
        //            {
        //                bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
        //            }

        //            if (Session["DATAFOLDER"] != null)
        //            {
        //                folderPath = Session["DATAFOLDER"].ToString();
        //            }
        //            if ((Request.QueryString["size"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null)
        //                && (Request.QueryString["seriestype"] != null))
        //            {
        //                outputSize = Request.QueryString["size"].ToString();
        //                interval = Request.QueryString["interval"];
        //                period = Request.QueryString["period"];
        //                series_type = Request.QueryString["seriestype"];

        //                //dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
        //                //if (dailyData == null)
        //                //{
        //                //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
        //                dailyData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
        //                                        bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
        //                //}

        //                ViewState["FetchedDataDaily"] = dailyData;

        //                //rsiData = StockApi.getRSI(folderPath, scriptName, day_interval: interval, period: period, seriestype: series_type,
        //                //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
        //                rsiData = StockApi.getRSIalternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: series_type,
        //                                            outputsize: outputSize, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(),
        //                                            dailyTable: dailyData);
        //                ViewState["FetchedDataRSI"] = rsiData;

        //            }
        //            else
        //            {
        //                ViewState["FetchedDataDaily"] = null;
        //                dailyData = null;
        //                ViewState["FetchedDataRSI"] = null;
        //                rsiData = null;
        //            }
        //            GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
        //            GridViewDaily.DataBind();
        //            GridViewData.DataSource = (DataTable)ViewState["FetchedDataRSI"];
        //            GridViewData.DataBind();
        //        }
        //        //else
        //        //{
        //        if (ViewState["FromDate"] != null)
        //            fromDate = ViewState["FromDate"].ToString();
        //        if (ViewState["ToDate"] != null)
        //            toDate = ViewState["ToDate"].ToString();

        //        if ((fromDate.Length > 0) && (toDate.Length > 0))
        //        {
        //            tempData = (DataTable)ViewState["FetchedDataDaily"];
        //            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
        //            filteredRows = tempData.Select(expression);
        //            if ((filteredRows != null) && (filteredRows.Length > 0))
        //                dailyData = filteredRows.CopyToDataTable();

        //            tempData.Clear();
        //            tempData = null;

        //            tempData = (DataTable)ViewState["FetchedDataRSI"];
        //            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
        //            filteredRows = tempData.Select(expression);
        //            if ((filteredRows != null) && (filteredRows.Length > 0))
        //                rsiData = filteredRows.CopyToDataTable();
        //        }
        //        else
        //        {
        //            dailyData = (DataTable)ViewState["FetchedDataDaily"];
        //            rsiData = (DataTable)ViewState["FetchedDataRSI"];
        //        }
        //        //}

        //        if ((dailyData != null) && (rsiData != null))
        //        {
        //            //showCandleStickGraph(intraData);
        //            //showVWAP(vwapData);
        //            chartAdvGraph.Series["Open"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open", "");
        //            chartAdvGraph.Series["High"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High", "");
        //            chartAdvGraph.Series["Low"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Low", "");
        //            chartAdvGraph.Series["Close"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Close", "");
        //            chartAdvGraph.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High,Low,Open,Close", "");
        //            chartAdvGraph.Series["RSI"].Points.DataBind(rsiData.AsEnumerable(), "Date", "RSI", "");

        //            //chartAdvGraph.ChartAreas[1].AlignWithChartArea = chartAdvGraph.ChartAreas[0].Name;
        //            chartAdvGraph.ChartAreas[1].AxisX.IsStartedFromZero = true;
        //            chartAdvGraph.ChartAreas[0].AxisX.IsStartedFromZero = true;
        //            foreach (ListItem item in Master.checkboxlistLines.Items)
        //            {
        //                chartAdvGraph.Series[item.Value].Enabled = item.Selected;
        //                if (item.Selected == false)
        //                {
        //                    if (chartAdvGraph.Annotations.FindByName(item.Value) != null)
        //                        chartAdvGraph.Annotations.Clear();
        //                }
        //            }
        //            //Master.headingtext.Text = "Momentum Indicator: " + Request.QueryString["script"].ToString();
        //            Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
        //        }
        //        else
        //        {
        //            if (expression.Length == 0)
        //            {
        //                Master.headingtext.Text = "Momentum Indicator-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
        //            }
        //            else
        //            {
        //                Master.headingtext.Text = "Momentum Indicator-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
        //            }
        //            //Master.headingtext.BackColor = Color.Red;
        //            Master.headingtext.CssClass = "blinking blinkingText";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
        //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
        //    }
        //}

        protected void chartAdvGraph_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            //string legendName;
            int chartIndex;

            //DataPoint p;
            //double lineHeight = -35;

            try
            {
                postBackValues = e.PostBackValue.Split(',');

                if (chartAdvGraph.Annotations.Count > 0)
                    chartAdvGraph.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }

                xDate = System.Convert.ToDateTime(postBackValues[2]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[3]);

                seriesName = postBackValues[0];


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //HA.Name = seriesName;
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("RSI"))
                {
                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    chartIndex = 1;

                }
                else
                {
                    HA.AxisX = chartAdvGraph.ChartAreas[0].AxisX;
                    HA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;

                    VA.AxisX = chartAdvGraph.ChartAreas[0].AxisX;
                    VA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;

                    ra.AxisX = chartAdvGraph.ChartAreas[0].AxisX;
                    ra.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    chartIndex = 0;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartAdvGraph.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                HA.ToolTip = postBackValues[3];
                chartAdvGraph.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartAdvGraph.ChartAreas[chartIndex].Name;
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
                {
                    //0-OHLC,1-Date,2-High,3-Low,4-Open,5-Close
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + "Open:" + postBackValues[5] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[6];
                }
                else if(seriesName.Equals("Portfolio"))
                {
                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase Price:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];

                    HA.ToolTip = "Close Price: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("^BSESN") || seriesName.Equals("^NSEI"))
                {
                    ra.Text = seriesName + "\n" + "Date:" + postBackValues[2] + "\n" + "Close:" + postBackValues[3] + "\n" + "Open:" + postBackValues[4] + "\n" + 
                        "High:" + postBackValues[5] + "\n" + "Low:" + postBackValues[6];
                }
                else
                {
                    ra.Text = "Date:" + postBackValues[2] + "\n" + seriesName + ":" + postBackValues[3];
                }
                //ra.SmartLabelStyle = sl;

                chartAdvGraph.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }

        //protected void buttonShowGraph_Click(object sender, EventArgs e)
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
            if ((GridViewDaily.Visible) || (GridViewData.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                GridViewDaily.Visible = true;
                GridViewData.Visible = true;
            }
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewDaily.DataBind();
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewData.DataBind();
        }

        //protected void buttonDesc_Click(object sender, EventArgs e)
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

        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    timertest.Text = ViewState["counter"].ToString();
        //    ViewState["counter"] = Int32.Parse(ViewState["counter"].ToString()) + 1;
        //}
    }
}