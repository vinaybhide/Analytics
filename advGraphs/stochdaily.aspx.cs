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
    public partial class stochdaily : System.Web.UI.Page
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
                if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null) &&
                    (Request.QueryString["seriestype"] != null) && (Request.QueryString["outputsize"] != null) &&
                    (Request.QueryString["interval"] != null) && (Request.QueryString["fastkperiod"] != null) &&
                        (Request.QueryString["slowdperiod"] != null) && (Request.QueryString["period"] != null))
                {
                    this.Title = "Buy Sell Indicator graph : " + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();

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
                        textboxSTOCH_Fastkperiod.Text = Request.QueryString["fastkperiod"].ToString();
                        textboxSTOCH_Slowdperiod.Text = Request.QueryString["slowdperiod"].ToString();
                        textboxPeriod.Text = Request.QueryString["period"].ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);

                    ShowGraph();
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
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
            Master.checkboxlistLines.Visible = true;
            ListItem li = new ListItem("K-FastLine", "K-FastLine");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("D-SlowLine", "D-SlowLine");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("RSI", "RSI");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Open", "Open");
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
            //li = new ListItem("Volume", "Volume");
            //li.Selected = true;
            //Master.checkboxlistLines.Items.Add(li);


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
            Master.bulletedlistDesc.Items.Add("Stochastics are used to show when a stock has moved into an overbought or oversold position.");
            Master.bulletedlistDesc.Items.Add("The premise of stochastics is that when a stock trends upwards, its closing price tends to trade at the high end of the day's range or price action. Price action refers to the range of prices at which a stock trades throughout the daily session.");
            Master.bulletedlistDesc.Items.Add("The K line is faster than the D line; the D line is the slower of the two.");
            Master.bulletedlistDesc.Items.Add("The investor needs to watch as the D line and the price of the issue begin to change and move into either the overbought (over the 80 line) or the oversold(under the 20 line) positions.");
            Master.bulletedlistDesc.Items.Add("The investor needs to consider selling the stock when the indicator moves above the 80 levels.");
            Master.bulletedlistDesc.Items.Add("Conversely, the investor needs to consider buying an issue that is below the 20 line and is starting to move up with increased volume.");
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

            string fastkperiod = textboxSTOCH_Fastkperiod.Text;
            string slowdperiod = textboxSTOCH_Slowdperiod.Text;

            string period = textboxPeriod.Text;

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
                dailyData = stockManager.GetStockPriceData(symbol, exchange, seriestype, outputsize, interval,
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

        void AdjustSeriesPoints(int pointstomove, System.Web.UI.DataVisualization.Charting.Chart sourceChart)
        {
            //int pointtomove = Int32.Parse(textboxPeriod.Text) - 1;
            for (int i = 0; i < pointstomove; i++)
            {
                sourceChart.Series["OHLC"].Points.RemoveAt(0);
                sourceChart.Series["Open"].Points.RemoveAt(0);
                sourceChart.Series["Close"].Points.RemoveAt(0);
                sourceChart.Series["Low"].Points.RemoveAt(0);
                sourceChart.Series["High"].Points.RemoveAt(0);
                //sourceChart.Series["Volume"].Points.RemoveAt(0);
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

                if (scriptData != null)
                {
                    chartAdvGraph.DataSource = scriptData;
                    chartAdvGraph.DataBind();
                    //The following example takes input from Series1's Y values for the daily high, low, and close prices (Series1:Y,Series1:Y2,Series1:Y4),
                    //and outputs %K on Series3 (Series3:Y) and %D on Series4 (Series4:Y). It uses a period of 15 days to calculate both %K and %D.
                    //Chart1.DataManipulator.FinancialFormula(FinancialFormula.StochasticIndicator, "15,15", "Series1:Y,Series1:Y2,Series1:Y4", "Series3:Y,Series4:Y")
                    chartAdvGraph.DataManipulator.FinancialFormula(FinancialFormula.StochasticIndicator,
                        textboxSTOCH_Fastkperiod.Text + "," + textboxSTOCH_Slowdperiod.Text, "OHLC:Y,OHLC:Y2,OHLC:Y4", "K-FastLine:Y,D-SlowLine:Y");

                    chartAdvGraph.DataManipulator.FinancialFormula(FinancialFormula.RelativeStrengthIndex,
                        textboxSTOCH_Fastkperiod.Text + "," + textboxPeriod.Text, "OHLC:Y4", "RSI:Y");

                    int pointstomove = Int32.Parse(textboxSTOCH_Fastkperiod.Text) + Int32.Parse(textboxSTOCH_Slowdperiod.Text) - 2;

                    AdjustSeriesPoints(pointstomove, chartAdvGraph);
                    chartAdvGraph.Series["RSI"].Points.RemoveAt(0);

                    if (chartAdvGraph.Series.FindByName("Open") != null)
                    {
                        chartAdvGraph.Series["Open"].PostBackValue = "Open," + symbol + "," + "#VALX,#VALY";

                    }
                    if (chartAdvGraph.Series.FindByName("High") != null)
                    {
                        chartAdvGraph.Series["High"].PostBackValue = "High," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("Low") != null)
                    {
                        chartAdvGraph.Series["Low"].PostBackValue = "Low," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("Close") != null)
                    {
                        chartAdvGraph.Series["Close"].PostBackValue = "Close," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("OHLC") != null)
                    {
                        chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + "," + "#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    //if (chartAdvGraph.Series.FindByName("Volume") != null)
                    //{
                    //    chartAdvGraph.Series["Volume"].PostBackValue = "Volume," + symbol + "," + "#VALX,#VALY";
                    //}

                    if (chartAdvGraph.Series.FindByName("K-FastLine") != null)
                    {
                        chartAdvGraph.Series["K-FastLine"].PostBackValue = "K-FastLine:" + textboxSTOCH_Fastkperiod.Text + "," + symbol + "," + "#VALX,#VALY{0.##}";

                    }
                    if (chartAdvGraph.Series.FindByName("D-SlowLine") != null)
                    {
                        chartAdvGraph.Series["D-SlowLine"].PostBackValue = "D-SlowLine:" + textboxSTOCH_Slowdperiod.Text + "," + symbol + "," + "#VALX,#VALY{0.##}";
                    }
                    if (chartAdvGraph.Series.FindByName("RSI") != null)
                    {
                        chartAdvGraph.Series["RSI"].PostBackValue = "RSI:" + textboxPeriod.Text + "," + symbol + "," + "#VALX,#VALY{0.##}";
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

                        (chartAdvGraph.Series[symbol]).XValueMember = "TIMESTAMP";
                        (chartAdvGraph.Series[symbol]).XValueType = ChartValueType.Date;
                        (chartAdvGraph.Series[symbol]).YValueMembers = "CLOSE";
                        (chartAdvGraph.Series[symbol]).YValueType = ChartValueType.Double;

                        chartAdvGraph.Series[symbol].LegendText = symbol;
                        chartAdvGraph.Series[symbol].LegendToolTip = symbol;
                        chartAdvGraph.Series[symbol].ToolTip = symbol + ":  Date:#VALX; CLOSE:#VALY (Click to see details)";
                        chartAdvGraph.Series[symbol].PostBackValue = symbol + "," + "#VALX,#VALY";
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

                        chartAdvGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
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

                if (seriesName.Contains("K-FastLine"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                }
                else if (seriesName.Contains("D-SlowLine"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                }
                else if (seriesName.Contains("RSI"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[2].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[2].Name;
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

        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }

        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}