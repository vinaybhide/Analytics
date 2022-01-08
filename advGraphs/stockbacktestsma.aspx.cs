using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics.advGraphs
{
    public partial class stockbacktestsma : System.Web.UI.Page
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
                if (Request.QueryString["symbol"] != null)
                {
                    this.Title = "Forecast and Back Test using SMA for : " + Request.QueryString["symbol"].ToString();

                    if (!IsPostBack)
                    {
                        ViewState["FromDate"] = null;
                        ViewState["ToDate"] = null;
                        ViewState["FetchedData"] = null;
                        ViewState["VALUATION_TABLE"] = null;
                        ViewState["SENSEX"] = null;
                        ViewState["NIFTY50"] = null;
                        fillDesc();
                        fillLinesCheckBoxes();

                        if (Request.QueryString["smasmall"] != null)
                        {
                            textboxSMASmallPeriod.Text = Request.QueryString["smasmall"].ToString();
                        }
                        if (Request.QueryString["smalong"] != null)
                        {
                            textboxSMALongPeriod.Text = Request.QueryString["smalong"].ToString();
                        }
                        if (Request.QueryString["buyspan"] != null)
                        {
                            textboxBuySpan.Text = Request.QueryString["buyspan"].ToString();
                        }
                        if (Request.QueryString["sellspan"] != null)
                        {
                            textboxSellSpan.Text = Request.QueryString["sellspan"].ToString();
                        }
                        if (Request.QueryString["simulationqty"] != null)
                        {
                            textboxSimulationQty.Text = Request.QueryString["simulationqty"].ToString();
                        }
                        if (Request.QueryString["regressiontype"] != null)
                        {
                            ddlRegressionType.SelectedValue = Request.QueryString["regressiontype"];
                        }
                        if (Request.QueryString["forecastperiod"] != null)
                        {
                            textboxForecastPeriod.Text = Request.QueryString["forecastperiod"];
                        }

                        ShowGraph();
                        //if (!IsPostBack)
                        //{
                        ViewState["FromDate"] = Master.textboxFromDate.Text;
                        ViewState["ToDate"] = Master.textboxToDate.Text;
                        //}
                    }
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartBackTest.Visible = true;
                        chartBackTest.Width = int.Parse(Master.panelWidth.Value);
                        chartBackTest.Height = int.Parse(Master.panelHeight.Value);
                    }
                }
                else
                {
                    //Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
                    //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select stock to perform backtest');", true);
                    Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
        }
        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("Back test example: Go long on 100 stocks (i.e. buy 100 stocks), when the short term moving average crosses above the long term moving average. This is known as golden cross.");
            Master.bulletedlistDesc.Items.Add("Sell the stock a few days later.For instance, we will keep the stock 20 days and then sell them. Compute the profit");
        }

        public void fillLinesCheckBoxes()
        {
            //Master.checkboxlistLines.Visible = false;
            //return;
            Master.checkboxlistLines.Visible = true;
            ListItem li;

            li = new ListItem("Daily Price", "Daily");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("SMA Small", "SMA_SMALL");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("SMA Long", "SMA_LONG");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Future_Price", "Future_Price");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("SMA_SMALL_Future", "SMA_SMALL_Future");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("SMA_LONG_Future", "SMA_LONG_Future");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Approximation_Error", "Approximation_Error");
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

        public void FillData()
        {
            DataTable tempData = null;
            DataTable dailyCloseSMA = null;
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataRow[] filteredRows = null;
            string expression = "";

            StockManager stockManager = new StockManager();

            //string folderPath = Session["DATAFOLDER"].ToString();

            string fromDate = null;

            int smallperiod = Int32.Parse(textboxSMASmallPeriod.Text.ToString());
            int longperiod = Int32.Parse(textboxSMALongPeriod.Text.ToString());
            int buySpan = Int32.Parse(textboxBuySpan.Text.ToString());
            int sellSpan = Int32.Parse(textboxSellSpan.Text.ToString());
            string symbol = Request.QueryString["symbol"].ToString();
            string exchange = Request.QueryString["exchange"].ToString();
            double simulationQty = double.Parse(textboxSimulationQty.Text.ToString());

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
                dailyCloseSMA = stockManager.GetBacktestFromSMA(symbol, exchange, "CLOSE", "Full", "1d",
                                fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate,
                                smallPeriod: smallperiod, longPeriod: longperiod, buySpan: buySpan, sellSpan: sellSpan, simulationQty: simulationQty);
                if (dailyCloseSMA != null)
                {
                    ViewState["FetchedData"] = dailyCloseSMA;
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

        /// <summary>
        ///try and do forward prediction for each of the series using Forecasting Formula
        ///The forecasting formula attempts to fit the historical data to a regression function and forecast future values of the data best on the best fit.
        ///
        ///Chart.DataManipulator.FinancialFormula(FinancialFormula.Forecasting,"RegressionType,Period,ApproxError,ForecastError","Historical","Forecast,UpperError,LowerError")
        ///
        /// Parameters - This formula takes four optional parameters.
        ///     RegressionType: Regression type. Use a numeral to indicate a polynomial regression of a specific degree, or 
        ///         one of the following to specify a different regression type: 
        ///             Linear, Exponential, Logarithmic, Power.The default value is 2, which is the same as Linear.
        ///     Period: Forecasting period.The formula predicts data for this period of days into the future. The default value is half of the series' length.
        ///     ApproxError: Whether to output the approximation error. If set to false, output error series contain no data for the corresponding historical data. 
        ///                 The default value is true.
        ///     ForecastError: Whether to output the forecasting error. If set to false, output error series contain the approximation error for all predicted data points 
        ///                 if ApproxError is set to true. The default value is true.
        ///Input Values - This formula takes one input Y value.
        ///     Historical: Historical data for forecasting.
        ///Output Value - This formula outputs three Y values.
        ///     Forecast: Forecasted values.
        ///     UpperError: Upper bound error.
        ///     LowerError: Lower bound error.
        ///
        ///Remarks - The Line chart type is a convenient chart type to display the forecasted values, and the Range chart type is a convenient chart type to 
        ///         display the error bounds.
        ///         
        /// Example - The following example takes input from Series1(Series1:Y) and outputs the forecast on Series2(Series2:Y) and 
        ///             error bounds on Series3(Series3:Y, Series3:Y2). It uses a second degree polynomial regression and a forecasting period of 40 days.
        ///             
        /// Chart1.DataManipulator.FinancialFormula (FinancialFormula.Forecasting, "2,40,true,true", "Series1:Y", "Series2:Y,Series3:Y,Series3:Y2")
        /// </summary>
        public void ForeCastPrice()
        {
            //We will use the Daily series to forecast the daily close price

            chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, ddlRegressionType.SelectedValue + "," + textboxForecastPeriod.Text + ",true,true", "Daily:Y",
                                                            "Future_Price:Y,Approximation_Error:Y,Approximation_Error:Y2");
            chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, ddlRegressionType.SelectedValue + "," + textboxForecastPeriod.Text + ",false,false", "SMA_SMALL:Y",
                                                            "SMA_SMALL_Future:Y");
            chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, ddlRegressionType.SelectedValue + "," + textboxForecastPeriod.Text + ",false,false", "SMA_LONG:Y",
                                                            "SMA_LONG_Future:Y");
            //chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, ddlRegressionType.SelectedValue + "," + textboxForecastPeriod.Text + ",false,false", "Daily:Y",
            //                                        "Future_Price:Y");
            //chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, ddlRegressionType.SelectedValue + "," + textboxForecastPeriod.Text + ",false,false", "SMA_SMALL:Y",
            //                                                "SMA_SMALL_Future:Y");
            //chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, ddlRegressionType.SelectedValue + "," + textboxForecastPeriod.Text + ",false,false", "SMA_LONG:Y",
            //                                                "SMA_LONG_Future:Y");

        }
        public void ShowGraph()
        {
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataTable valuationTable = null;
            DataTable dailysmaTable = null;
            Series tempSeries = null;

            int portfolioTxnNumber = 1;

            string symbol = Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();
            string exchange = Request.QueryString["exchange"].ToString();
            int smallperiod = Int32.Parse(textboxSMASmallPeriod.Text.ToString());
            int longperiod = Int32.Parse(textboxSMALongPeriod.Text.ToString());

            FillData();

            dailysmaTable = (DataTable)ViewState["FetchedData"];
            valuationTable = (DataTable)ViewState["VALUATION_TABLE"];
            sensexTable = (DataTable)ViewState["SENSEX"];
            niftyTable = (DataTable)ViewState["NIFTY50"];


            gridviewBackTest.DataSource = dailysmaTable;
            gridviewBackTest.DataBind();

            if (chartBackTest.Annotations.Count > 0)
                chartBackTest.Annotations.Clear();

            if ((dailysmaTable != null) && (dailysmaTable.Rows.Count > 0))
            {
                if (smallperiod > 0)
                {
                    if (chartBackTest.Series.FindByName("SMA_SMALL") == null)
                    {
                        chartBackTest.Series.Add("SMA_SMALL");

                        chartBackTest.Series["SMA_SMALL"].Name = "SMA_SMALL";
                        (chartBackTest.Series["SMA_SMALL"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTest.Series["SMA_SMALL"]).ChartArea = chartBackTest.ChartAreas[0].Name;

                        chartBackTest.Series["SMA_SMALL"].Legend = chartBackTest.Legends[0].Name;
                        chartBackTest.Series["SMA_SMALL"].XAxisType = AxisType.Secondary;
                        chartBackTest.Series["SMA_SMALL"].YAxisType = AxisType.Primary;
                    }
                    chartBackTest.Series["SMA_SMALL"].LegendText = "SMA Period: " + smallperiod;//Request.QueryString["smasmall"].ToString();
                    chartBackTest.Series["SMA_SMALL"].LegendToolTip = "SMA Period: " + smallperiod;//Request.QueryString["smasmall"].ToString();
                    chartBackTest.Series["SMA_SMALL"].ToolTip = "SMA Period: " + smallperiod + " : Date:#VALX; SMA:#VALY (Click to see details)";
                    chartBackTest.Series["SMA_SMALL"].PostBackValue = "SMA_SMALL," + smallperiod + ",#VALX,#VALY";

                    (chartBackTest.Series["SMA_SMALL"]).Points.Clear();

                    if (longperiod > 0)
                    {
                        if (chartBackTest.Series.FindByName("SMA_LONG") == null)
                        {
                            chartBackTest.Series.Add("SMA_LONG");

                            chartBackTest.Series["SMA_LONG"].Name = "SMA_LONG";
                            (chartBackTest.Series["SMA_LONG"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                            (chartBackTest.Series["SMA_LONG"]).ChartArea = chartBackTest.ChartAreas[0].Name;

                            chartBackTest.Series["SMA_LONG"].Legend = chartBackTest.Legends[0].Name;
                            chartBackTest.Series["SMA_LONG"].XAxisType = AxisType.Secondary;
                            chartBackTest.Series["SMA_LONG"].YAxisType = AxisType.Primary;

                        }
                        chartBackTest.Series["SMA_LONG"].LegendText = "SMA Period: " + longperiod; //Request.QueryString["smalong"].ToString();
                        chartBackTest.Series["SMA_LONG"].LegendToolTip = "SMA Period: " + longperiod; //Request.QueryString["smalong"].ToString();
                        chartBackTest.Series["SMA_LONG"].ToolTip = "SMA Period: " + longperiod + " : Date:#VALX; SMA:#VALY (Click to see details)";
                        chartBackTest.Series["SMA_LONG"].PostBackValue = "SMA_LONG," + longperiod + ",#VALX,#VALY";

                        (chartBackTest.Series["SMA_LONG"]).Points.Clear();
                    }

                    if (chartBackTest.Series.FindByName("Daily") == null)
                    {
                        chartBackTest.Series.Add("Daily");

                        chartBackTest.Series["Daily"].Name = "Daily";
                        (chartBackTest.Series["Daily"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTest.Series["Daily"]).ChartArea = chartBackTest.ChartAreas[0].Name;

                        chartBackTest.Series["Daily"].Legend = chartBackTest.Legends[0].Name;
                        chartBackTest.Series["Daily"].XAxisType = AxisType.Secondary;
                        chartBackTest.Series["Daily"].YAxisType = AxisType.Primary;
                    }
                    chartBackTest.Series["Daily"].LegendText = "Daily CLOSE";
                    chartBackTest.Series["Daily"].LegendToolTip = "Daily CLOSE";
                    chartBackTest.Series["Daily"].ToolTip = "Daily price: " + " : Date:#VALX; Close:#VALY (Click to see details)";
                    chartBackTest.Series["Daily"].PostBackValue = "Daily" + ",#VALX,#VALY";

                    (chartBackTest.Series["Daily"]).Points.Clear();

                    for (int rownum = 0; rownum < dailysmaTable.Rows.Count; rownum++)
                    {
                        if (smallperiod > 0)
                        {
                            (chartBackTest.Series["SMA_SMALL"]).Points.AddXY(dailysmaTable.Rows[rownum]["TIMESTAMP"], dailysmaTable.Rows[rownum]["SMA_SMALL"]);
                            (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].PostBackValue =
                                                "SMA_SMALL," +
                                                dailysmaTable.Rows[rownum]["SYMBOL"] + "," + dailysmaTable.Rows[rownum]["TIMESTAMP"] + "," +
                                                dailysmaTable.Rows[rownum]["CLOSE"] + "," +
                                                "SMA Period: " + smallperiod + "," +
                                                "SMA: ," + dailysmaTable.Rows[rownum]["SMA_SMALL"];
                        }
                        if (longperiod > 0)
                        {
                            (chartBackTest.Series["SMA_LONG"]).Points.AddXY(dailysmaTable.Rows[rownum]["TIMESTAMP"], dailysmaTable.Rows[rownum]["SMA_LONG"]);
                            (chartBackTest.Series["SMA_LONG"]).Points[(chartBackTest.Series["SMA_LONG"]).Points.Count - 1].PostBackValue =
                                                "SMA_LONG," +
                                                dailysmaTable.Rows[rownum]["SYMBOL"] + "," + dailysmaTable.Rows[rownum]["TIMESTAMP"] + "," +
                                                dailysmaTable.Rows[rownum]["CLOSE"] + "," +
                                                "SMA Period: " + longperiod + "," +
                                                "SMA: ," + dailysmaTable.Rows[rownum]["SMA_LONG"];
                        }
                        (chartBackTest.Series["Daily"]).Points.AddXY(dailysmaTable.Rows[rownum]["TIMESTAMP"], dailysmaTable.Rows[rownum]["CLOSE"]);
                        (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].PostBackValue =
                                            "Daily," +
                                            dailysmaTable.Rows[rownum]["SYMBOL"] + "," + dailysmaTable.Rows[rownum]["TIMESTAMP"] + "," +
                                            dailysmaTable.Rows[rownum]["CLOSE"] + "," +
                                            dailysmaTable.Rows[rownum]["BUY_FLAG"] + "," +
                                            dailysmaTable.Rows[rownum]["SELL_FLAG"] + "," +
                                            dailysmaTable.Rows[rownum]["QUANTITY"] + "," +
                                            dailysmaTable.Rows[rownum]["BUY_COST"] + "," +
                                            dailysmaTable.Rows[rownum]["SELL_VALUE"] + "," +
                                            dailysmaTable.Rows[rownum]["PROFIT_LOSS"] + "," +
                                            dailysmaTable.Rows[rownum]["RESULT"];
                        //if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["BUY_FLAG"]) == true)
                        if (dailysmaTable.Rows[rownum]["CROSSOVER_FLAG"].ToString().Equals("X") == true)
                        {
                            //we just mark this point as cross over
                            (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].MarkerSize = 11;
                            (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Cross;
                            (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].MarkerColor = Color.Blue;
                            (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].ToolTip = "Backtest Initiated on: " + "#VALX";
                        }

                        if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["BUY_FLAG"]) == true)
                        {
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerSize = 12;
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerColor = Color.Yellow;
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].ToolTip = "Buy Signal on: " + "#VALX, Buy Price: #VALY";
                        }
                        if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["SELL_FLAG"]) == true)
                        {
                            //we just mark this point as cross over
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerSize = 12;
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerColor = Color.Green;
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].ToolTip = "Sell Signal: " + dailysmaTable.Rows[rownum]["RESULT"].ToString();
                        }
                    }
                    ForeCastPrice();
                    chartBackTest.Series["Future_Price"].Points[chartBackTest.Series["Daily"].Points.Count - 1].MarkerStyle = MarkerStyle.Triangle;
                    chartBackTest.Series["Future_Price"].Points[chartBackTest.Series["Daily"].Points.Count - 1].MarkerSize = 12;
                    chartBackTest.Series["Future_Price"].Points[chartBackTest.Series["Daily"].Points.Count - 1].ToolTip = "Last actual close";
                    chartBackTest.Series["Future_Price"].PostBackValue = "Future_Price," + symbol + ",#VALX,#VALY{0.##}";
                    chartBackTest.Series["Future_Price"].ToolTip = "Date: #VALX; Future Price: #VALY{0.##}";
                    chartBackTest.Series["SMA_SMALL_Future"].PostBackValue = "SMA_SMALL_Future," + symbol + ",#VALX,#VALY{0.##}";
                    chartBackTest.Series["SMA_SMALL_Future"].ToolTip = "Date: #VALX; SMA_SMALL_Future: #VALY{0.##}";
                    chartBackTest.Series["SMA_LONG_Future"].PostBackValue = "SMA_LONG_Future," + symbol + ",#VALX,#VALY{0.##}";
                    chartBackTest.Series["SMA_LONG_Future"].ToolTip = "Date: #VALX; SMA_LONG_Future: #VALY{0.##}";
                    chartBackTest.Series["Approximation_Error"].PostBackValue = "Approximation_Error," + symbol + ",#VALX,#VALY1{0.##},#VALY2{0.##}";
                    chartBackTest.Series["Approximation_Error"].ToolTip = "Date: #VALX; Approximation_Error: #VALY1{0.##}, Forecast error: #VALY2{0.##}";


                }
                else
                {
                    tempSeries = chartBackTest.Series.FindByName("Daily");
                    if (tempSeries != null)
                        chartBackTest.Series.Remove(tempSeries);
                    tempSeries = chartBackTest.Series.FindByName("SMA_SMALL");
                    if (tempSeries != null)
                        chartBackTest.Series.Remove(tempSeries);
                    tempSeries = chartBackTest.Series.FindByName("SMA_LONG");
                    if (tempSeries != null)
                        chartBackTest.Series.Remove(tempSeries);
                }

                if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    if (chartBackTest.Series.FindByName(symbol) == null)
                    {
                        chartBackTest.Series.Add(symbol);

                        chartBackTest.Series[symbol].Name = symbol; // "Portfolio";
                        (chartBackTest.Series[symbol]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTest.Series[symbol]).ChartArea = chartBackTest.ChartAreas[0].Name;

                        chartBackTest.Series[symbol].Legend = chartBackTest.Legends[0].Name;

                        chartBackTest.Series[symbol].XAxisType = AxisType.Secondary;
                        chartBackTest.Series[symbol].YAxisType = AxisType.Primary;

                        (chartBackTest.Series[symbol]).XValueType = ChartValueType.Date;
                        (chartBackTest.Series[symbol]).YValueType = ChartValueType.Double;
                    }
                    chartBackTest.Series[symbol].LegendText = valuationTable.Rows[0]["SYMBOL"].ToString();
                    chartBackTest.Series[symbol].LegendToolTip = valuationTable.Rows[0]["SYMBOL"].ToString();
                    chartBackTest.Series[symbol].ToolTip = "Portfolio: " + "Date:#VALX; Close:#VALY (Click to see details)";
                    chartBackTest.Series[symbol].PostBackValue = "Portfolio:" + ",#VALX,#VALY";

                    (chartBackTest.Series[symbol]).Points.Clear();

                    for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                    {
                        //(chartBackTest.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                        (chartBackTest.Series[symbol]).Points.AddXY(valuationTable.Rows[rownum]["TIMESTAMP"], valuationTable.Rows[rownum]["CLOSE"]);
                        (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].PostBackValue =
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
                            (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].MarkerSize = 11;
                            (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                            (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                        }
                    }
                    (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].MarkerSize = 10;
                    (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                    (chartBackTest.Series[symbol]).Points[(chartBackTest.Series[symbol]).Points.Count - 1].ToolTip = "Click to see latest valuation";

                }
                else
                {
                    tempSeries = chartBackTest.Series.FindByName(symbol);
                    if (tempSeries != null)
                        chartBackTest.Series.Remove(tempSeries);
                }

                if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
                {
                    if (chartBackTest.Series.FindByName("^BSESN") == null)
                    {
                        chartBackTest.Series.Add("^BSESN");

                        chartBackTest.Series["^BSESN"].Name = "^BSESN";
                        (chartBackTest.Series["^BSESN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTest.Series["^BSESN"]).ChartArea = chartBackTest.ChartAreas[0].Name;

                        chartBackTest.Series["^BSESN"].Legend = chartBackTest.Legends[0].Name;
                        chartBackTest.Series["^BSESN"].LegendText = "BSE SENSEX";
                        chartBackTest.Series["^BSESN"].LegendToolTip = "BSE SENSEX";

                        chartBackTest.Series["^BSESN"].XAxisType = AxisType.Secondary;
                        chartBackTest.Series["^BSESN"].YAxisType = AxisType.Primary;

                        (chartBackTest.Series["^BSESN"]).YValuesPerPoint = 4;
                        chartBackTest.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartBackTest.Series["^BSESN"].PostBackValue = "^BSESN" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    (chartBackTest.Series["^BSESN"]).Points.Clear();
                    (chartBackTest.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartBackTest.Series.FindByName("^BSESN");
                    if (tempSeries != null)
                        chartBackTest.Series.Remove(tempSeries);
                }

                if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
                {
                    if (chartBackTest.Series.FindByName("^NSEI") == null)
                    {
                        chartBackTest.Series.Add("^NSEI");

                        chartBackTest.Series["^NSEI"].Name = "^NSEI";
                        (chartBackTest.Series["^NSEI"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTest.Series["^NSEI"]).ChartArea = chartBackTest.ChartAreas[0].Name;

                        chartBackTest.Series["^NSEI"].Legend = chartBackTest.Legends[0].Name;
                        chartBackTest.Series["^NSEI"].LegendText = "NIFTY 50";
                        chartBackTest.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                        chartBackTest.Series["^NSEI"].XAxisType = AxisType.Secondary;
                        chartBackTest.Series["^NSEI"].YAxisType = AxisType.Primary;

                        (chartBackTest.Series["^NSEI"]).YValuesPerPoint = 4;
                        chartBackTest.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartBackTest.Series["^NSEI"].PostBackValue = "^NSEI" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    (chartBackTest.Series["^NSEI"]).Points.Clear();
                    (chartBackTest.Series["^NSEI"]).Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartBackTest.Series.FindByName("^NSEI");
                    if (tempSeries != null)
                        chartBackTest.Series.Remove(tempSeries);
                }

                foreach (ListItem item in Master.checkboxlistLines.Items)
                {
                    if (chartBackTest.Series.FindByName(item.Value) != null)
                    {
                        chartBackTest.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartBackTest.Annotations.FindByName(item.Value) != null)
                                chartBackTest.Annotations.Clear();
                        }
                    }
                }
            }
        }
        protected void chartBackTest_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth = 0;
            double lineHeight = 0;

            string seriesName;
            StringBuilder raString = new StringBuilder();

            try
            {
                if (chartBackTest.Annotations.Count > 0)
                    chartBackTest.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                seriesName = postBackValues[0];


                if (seriesName.Equals("Daily"))
                {
                    //NAV
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;

                    raString.Clear();
                    raString.AppendLine("Backtest simulation for: " + postBackValues[1]);
                    raString.AppendLine("Current Price Date: " + postBackValues[2]);
                    raString.AppendLine("Current Price: " + postBackValues[3]);
                    raString.AppendLine("Buy flag: " + postBackValues[4]);
                    raString.AppendLine("Sell Flag: " + postBackValues[5]);
                    raString.AppendLine("Simulation QTY: " + postBackValues[6]);
                    raString.AppendLine("Buy Cost: " + postBackValues[7]);
                    raString.AppendLine("Sell Value: " + postBackValues[8]);
                    raString.AppendLine("Profit/Loss: " + postBackValues[9]);
                    raString.AppendLine("Backtest Result : " + postBackValues[10]);

                    ra.Text = raString.ToString();

                    HA.ToolTip = "Close: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("Portfolio"))
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;

                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase Price:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];

                    HA.ToolTip = "Close: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("SMA_LONG") || seriesName.Equals("SMA_SMALL"))
                {
                    //SMA
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[6]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;

                    ra.Text = postBackValues[1] + "\nClose Date:" + postBackValues[2] + "\nCLOSE:" + postBackValues[3] + "\n" + postBackValues[4] + "\n" + postBackValues[5] + postBackValues[6];

                    HA.ToolTip = "SMA: " + postBackValues[6];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Contains("Future"))//forecasted series
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[1].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[1].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[1].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[1].AxisX;
                    VA.AxisX = chartBackTest.ChartAreas[1].AxisX;
                    ra.AxisX = chartBackTest.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[1].Name;

                    ra.Text = postBackValues[0] + "\n" + "\nDate:" + postBackValues[2] + "\n" + seriesName + postBackValues[3];

                    HA.ToolTip = seriesName + postBackValues[3];
                    VA.ToolTip = postBackValues[2];

                }
                else if (seriesName.Equals("^BSESN") || seriesName.Equals("^NSEI"))
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;
                    ra.Text = seriesName + "\n" + "Date:" + postBackValues[2] + "\n" + "Close:" + postBackValues[3] + "\n" + "Open:" + postBackValues[4] + "\n" +
                        "High:" + postBackValues[5] + "\n" + "Low:" + postBackValues[6];
                }
                else if (seriesName.Contains("Error"))//forecasted series
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[2].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[2].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[2].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[2].AxisX;
                    VA.AxisX = chartBackTest.ChartAreas[2].AxisX;
                    ra.AxisX = chartBackTest.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[2].Name;

                    ra.Text = postBackValues[0] + "\n" + "\nDate:" + postBackValues[2] + "\n" + "Approximation Error: " + postBackValues[3] + "\n" + "Forecast Error: " + postBackValues[4];

                    HA.ToolTip = seriesName + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }


                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartBackTest.Annotations.Add(HA);

                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartBackTest.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                chartBackTest.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }

        protected void buttonShowGraph_Click()
        {
            //ViewState["FromDate"] = Master.textboxFromDate.Text;
            //ViewState["ToDate"] = Master.textboxToDate.Text;
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
        protected void buttonShowGrid_Click()
        {
            if (gridviewBackTest.Visible)
            {
                gridviewBackTest.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                gridviewBackTest.Visible = true;
                Master.buttonShowGrid.Text = "Hide Raw Data";
            }
        }

        protected void gridviewBackTest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewBackTest.PageIndex = e.NewPageIndex;
            gridviewBackTest.DataSource = (DataTable)ViewState["FetchedData"];
            gridviewBackTest.DataBind();
            //ShowGraph();
        }
        protected void gridviewPortfolioValuation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewPortfolioValuation.PageIndex = e.NewPageIndex;
            gridviewPortfolioValuation.DataSource = (DataTable)ViewState["VALUATION_TABLE"];
            gridviewPortfolioValuation.DataBind();
            //ShowGraph();
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