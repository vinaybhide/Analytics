using Analytics.Graphs;
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
    public partial class vwaprice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            Master.OnDoEventToggleParameters += new standardgraphs.DoEventToggleParameters(buttonShowHideParam_Click);
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
                        chartStdGraph.Visible = true;
                        chartStdGraph.Width = int.Parse(Master.panelWidth.Value);
                        chartStdGraph.Height = int.Parse(Master.panelHeight.Value);
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
            Master.bulletedlistDesc.Items.Add("The volume weighted average price(VWAP) is a trading benchmark used by traders that gives the average price a security has traded at throughout the day, based on both volume and price.");
            Master.bulletedlistDesc.Items.Add("Let’s face it, at its fundamental level, if we had to compare two seemingly good securities, more often than not, we would check its price trend and the trading volume. Price is obvious, but why the volume? Volume is important as we don’t want to get stuck with a stock which has few takers, even if you think it is priced attractively. Thus, the VWAP was created to take into account both volume as well as Price so that the potential investor would make the trading decision or not.");
            Master.bulletedlistDesc.Items.Add("It is important because it provides traders with insight into both the trend and value of a security.");
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

                if(ddl_Interval.SelectedValue.Contains("m"))
                {
                    chartStdGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "g";
                    chartStdGraph.ChartAreas[1].AxisX.LabelStyle.Format = "g";
                    chartStdGraph.ChartAreas[2].AxisX.LabelStyle.Format = "g";
                }
                else
                {
                    chartStdGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "dd-MM-yyyy";
                    chartStdGraph.ChartAreas[1].AxisX.LabelStyle.Format = "dd-MM-yyyy";
                    chartStdGraph.ChartAreas[2].AxisX.LabelStyle.Format = "dd-MM-yyyy";
                }
                if (scriptData != null)
                {
                    chartStdGraph.DataSource = scriptData;
                    chartStdGraph.DataBind();
                    if (chartStdGraph.Series.FindByName("Open") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["Open"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["Open"].ToolTip = "Date:#VALX{g}; Open:#VALY";
                            chartStdGraph.Series["Open"].PostBackValue = "Open," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartStdGraph.Series["Open"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["Open"].ToolTip = "Date:#VALX; Open:#VALY";
                            chartStdGraph.Series["Open"].PostBackValue = "Open," + symbol + ",#VALX,#VALY";
                        }
                    }
                    if (chartStdGraph.Series.FindByName("High") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["High"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["High"].ToolTip = "Date:#VALX{g}; High:#VALY";
                            chartStdGraph.Series["High"].PostBackValue = "High," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartStdGraph.Series["High"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["High"].ToolTip = "Date:#VALX; High:#VALY";
                            chartStdGraph.Series["High"].PostBackValue = "High," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartStdGraph.Series.FindByName("Low") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["Low"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["Low"].ToolTip = "Date:#VALX{g}; Low:#VALY";
                            chartStdGraph.Series["Low"].PostBackValue = "Low," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartStdGraph.Series["Low"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["Low"].ToolTip = "Date:#VALX; High:#VALY";
                            chartStdGraph.Series["Low"].PostBackValue = "Low," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartStdGraph.Series.FindByName("Close") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["Close"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["Close"].ToolTip = "Date:#VALX{g}; Close:#VALY";
                            chartStdGraph.Series["Close"].PostBackValue = "Close," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartStdGraph.Series["Close"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["Close"].ToolTip = "Date:#VALX; Close:#VALY";
                            chartStdGraph.Series["Close"].PostBackValue = "Close," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartStdGraph.Series.FindByName("OHLC") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["OHLC"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["OHLC"].ToolTip = "Date:#VALX{g}; OHLC:#VALY";
                            chartStdGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                        }
                        else
                        {
                            chartStdGraph.Series["OHLC"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["OHLC"].ToolTip = "Date:#VALX; OHLC:#VALY";
                            chartStdGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + ",#VALX,#VALY";
                        }
                    }
                    if (chartStdGraph.Series.FindByName("Volume") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["Volume"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["Volume"].ToolTip = "Date:#VALX{g}; Volume:#VALY";
                            chartStdGraph.Series["Volume"].PostBackValue = "Volume," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartStdGraph.Series["Volume"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["Volume"].ToolTip = "Date:#VALX; Volume:#VALY";
                            chartStdGraph.Series["Volume"].PostBackValue = "Volume," + symbol + ",#VALX,#VALY";
                        }

                    }
                    if (chartStdGraph.Series.FindByName("VWAP") != null)
                    {
                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["VWAP"].XValueType = ChartValueType.DateTime;
                            chartStdGraph.Series["VWAP"].ToolTip = "Date:#VALX{g}; VWAP:#VALY";
                            chartStdGraph.Series["VWAP"].PostBackValue = "VWAP," + symbol + ",#VALX{g},#VALY";
                        }
                        else
                        {
                            chartStdGraph.Series["VWAP"].XValueType = ChartValueType.Date;
                            chartStdGraph.Series["VWAP"].ToolTip = "Date:#VALX; VWAP:#VALY";
                            chartStdGraph.Series["VWAP"].PostBackValue = "VWAP," + symbol + ",#VALX,#VALY";
                        }
                    }
                }
                if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    if (chartStdGraph.Series.FindByName(symbol) == null)
                    {
                        chartStdGraph.Series.Add(symbol);

                        chartStdGraph.Series[symbol].Name = symbol;
                        (chartStdGraph.Series[symbol]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartStdGraph.Series[symbol]).ChartArea = chartStdGraph.ChartAreas[0].Name;

                        chartStdGraph.Series[symbol].Legend = chartStdGraph.Legends[0].Name;

                        (chartStdGraph.Series[symbol]).XAxisType = AxisType.Secondary;
                        (chartStdGraph.Series[symbol]).YAxisType = AxisType.Primary;

                        (chartStdGraph.Series[symbol]).YValuesPerPoint = 1;
                        (chartStdGraph.Series[symbol]).XValueMember = "TIMESTAMP";
                        (chartStdGraph.Series[symbol]).XValueType = ChartValueType.Date;
                        (chartStdGraph.Series[symbol]).YValueMembers = "CLOSE";
                        (chartStdGraph.Series[symbol]).YValueType = ChartValueType.Double;

                        chartStdGraph.Series[symbol].LegendText = symbol;
                        chartStdGraph.Series[symbol].LegendToolTip = symbol;
                        chartStdGraph.Series[symbol].ToolTip = symbol + ":  Date:#VALX; CLOSE:#VALY (Click to see details)";
                        chartStdGraph.Series[symbol].PostBackValue = symbol + "," + "#VALX,#VALY";
                    }
                    (chartStdGraph.Series[symbol]).Points.Clear();
                    for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                    {
                        //(chartStdGraph.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                        (chartStdGraph.Series[symbol]).Points.AddXY(valuationTable.Rows[rownum]["TIMESTAMP"], valuationTable.Rows[rownum]["CLOSE"]);
                        (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].PostBackValue =
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
                            (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].MarkerSize = 11;
                            (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                            (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                        }
                    }
                    (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].MarkerSize = 10;
                    (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                    (chartStdGraph.Series[symbol]).Points[(chartStdGraph.Series[symbol]).Points.Count - 1].ToolTip = "Click to see latest valuation";

                }
                else
                {
                    tempSeries = chartStdGraph.Series.FindByName(symbol);
                    if (tempSeries != null)
                        chartStdGraph.Series.Remove(tempSeries);
                }

                if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
                {
                    if (chartStdGraph.Series.FindByName("^BSESN") == null)
                    {
                        chartStdGraph.Series.Add("^BSESN");

                        chartStdGraph.Series["^BSESN"].Name = "^BSESN";
                        (chartStdGraph.Series["^BSESN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartStdGraph.Series["^BSESN"]).ChartArea = chartStdGraph.ChartAreas[0].Name;

                        chartStdGraph.Series["^BSESN"].Legend = chartStdGraph.Legends[0].Name;
                        chartStdGraph.Series["^BSESN"].LegendText = "BSE SENSEX";
                        chartStdGraph.Series["^BSESN"].LegendToolTip = "BSE SENSEX";

                        (chartStdGraph.Series["^BSESN"]).YValuesPerPoint = 4;

                        chartStdGraph.Series["^BSESN"].XAxisType = AxisType.Secondary;
                        chartStdGraph.Series["^BSESN"].YAxisType = AxisType.Primary;

                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["^BSESN"].XValueType = ChartValueType.DateTime;

                            chartStdGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX{g}; Close:#VALY1 (Click to see details)";
                            chartStdGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                        }
                        else
                        {
                            chartStdGraph.Series["^BSESN"].XValueType = ChartValueType.Date;

                            chartStdGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                            chartStdGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                        }
                    }
                    chartStdGraph.Series["^BSESN"].Points.Clear();
                    (chartStdGraph.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartStdGraph.Series.FindByName("^BSESN");
                    if (tempSeries != null)
                        chartStdGraph.Series.Remove(tempSeries);
                }

                if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
                {
                    if (chartStdGraph.Series.FindByName("^NSEI") == null)
                    {
                        chartStdGraph.Series.Add("^NSEI");

                        chartStdGraph.Series["^NSEI"].Name = "^NSEI";
                        (chartStdGraph.Series["^NSEI"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartStdGraph.Series["^NSEI"]).ChartArea = chartStdGraph.ChartAreas[0].Name;

                        chartStdGraph.Series["^NSEI"].Legend = chartStdGraph.Legends[0].Name;
                        chartStdGraph.Series["^NSEI"].LegendText = "NIFTY 50";
                        chartStdGraph.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                        (chartStdGraph.Series["^NSEI"]).YValuesPerPoint = 4;

                        chartStdGraph.Series["^NSEI"].XAxisType = AxisType.Secondary;
                        chartStdGraph.Series["^NSEI"].YAxisType = AxisType.Primary;

                        if (ddl_Interval.SelectedValue.Contains("m"))
                        {
                            chartStdGraph.Series["^NSEI"].XValueType = ChartValueType.DateTime;

                            chartStdGraph.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX{g}; Close:#VALY1 (Click to see details)";
                            chartStdGraph.Series["^NSEI"].PostBackValue = "^NSEI," + "NIFTY50" + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                        }
                        else
                        {
                            chartStdGraph.Series["^NSEI"].XValueType = ChartValueType.Date;

                            chartStdGraph.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                            chartStdGraph.Series["^NSEI"].PostBackValue = "^NSEI," + "NIFTY50" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                        }
                    }
                    (chartStdGraph.Series["^NSEI"]).Points.Clear();
                    (chartStdGraph.Series["^NSEI"]).Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartStdGraph.Series.FindByName("^NSEI");
                    if (tempSeries != null)
                        chartStdGraph.Series.Remove(tempSeries);
                }

                foreach (ListItem item in Master.checkboxlistLines.Items)
                {
                    if (chartStdGraph.Series.FindByName(item.Value) != null)
                    {
                        chartStdGraph.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartStdGraph.Annotations.FindByName(item.Value) != null)
                                chartStdGraph.Annotations.Clear();
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

        protected void chartStdGraph_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartStdGraph.Annotations.Count > 0)
                    chartStdGraph.Annotations.Clear();

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
                    HA.AxisY = chartStdGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartStdGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartStdGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartStdGraph.ChartAreas[1].AxisX;
                    VA.AxisX = chartStdGraph.ChartAreas[1].AxisX;
                    ra.AxisX = chartStdGraph.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartStdGraph.ChartAreas[1].Name;
                    //VA.ClipToChartArea = chartStdGraph.ChartAreas[1].Name;
                    //VA.IsInfinitive = true;
                }
                else if (seriesName.Equals("VWAP"))
                {
                    HA.AxisY = chartStdGraph.ChartAreas[2].AxisY;
                    VA.AxisY = chartStdGraph.ChartAreas[2].AxisY;
                    ra.AxisY = chartStdGraph.ChartAreas[2].AxisY;

                    HA.AxisX = chartStdGraph.ChartAreas[2].AxisX;
                    VA.AxisX = chartStdGraph.ChartAreas[2].AxisX;
                    ra.AxisX = chartStdGraph.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartStdGraph.ChartAreas[2].Name;
                    //VA.ClipToChartArea = chartStdGraph.ChartAreas[2].Name;
                    //VA.IsInfinitive = true;
                }
                else
                {
                    HA.AxisY = chartStdGraph.ChartAreas[0].AxisY;
                    VA.AxisY = chartStdGraph.ChartAreas[0].AxisY;
                    ra.AxisY = chartStdGraph.ChartAreas[0].AxisY;

                    HA.AxisX = chartStdGraph.ChartAreas[0].AxisX2;
                    VA.AxisX = chartStdGraph.ChartAreas[0].AxisX2;
                    ra.AxisX = chartStdGraph.ChartAreas[0].AxisX2;
                    HA.ClipToChartArea = chartStdGraph.ChartAreas[0].Name;
                    //VA.ClipToChartArea = chartStdGraph.ChartAreas[0].Name;
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
                chartStdGraph.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                VA.ToolTip = postBackValues[2];
                chartStdGraph.Annotations.Add(VA);

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

                chartStdGraph.Annotations.Add(ra);

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

        public void buttonShowGrid_Click()
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                GridViewData.Visible = true;
                Master.buttonShowGrid.Text = "Hide Raw Data";
            }

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
    }
}