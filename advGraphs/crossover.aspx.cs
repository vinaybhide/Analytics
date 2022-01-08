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
    public partial class crossover : System.Web.UI.Page
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
                if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null) &&
                    (Request.QueryString["smallperiod"] != null) && (Request.QueryString["longperiod"] != null) &&
                    (Request.QueryString["seriestype"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["outputsize"] != null))
                {
                    this.Title = "Crossover: " + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();
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
                        ddlDaily_Outputsize.SelectedValue = Request.QueryString["outputsize"].ToString();
                        ddlDaily_Interval.SelectedValue = Request.QueryString["interval"].ToString();
                        textboxSMASmallPeriod.Text = Request.QueryString["smallperiod"].ToString();
                        textboxSMALongPeriod.Text = Request.QueryString["longperiod"].ToString();
                        ddlDaily_SeriesType.SelectedValue = Request.QueryString["seriestype"].ToString();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph();

                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartCrossover.Visible = true;
                        chartCrossover.Width = int.Parse(Master.panelWidth.Value);
                        chartCrossover.Height = int.Parse(Master.panelHeight.Value);
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

            li = new ListItem("SMA Small", "SMA1");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("SMA Long", "SMA2");
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
            li = new ListItem("Volume", "Volume");
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
            Master.bulletedlistDesc.Items.Add("The crossover is a point on the trading chart in which a security's price and a technical indicator " +
                "line intersect, or when two indicators themselves cross. Crossovers are used to estimate the performance of a " +
                "financial instrument and to predict coming changes in trend, such as reversals or breakouts.");
            Master.bulletedlistDesc.Items.Add("A golden cross and a death cross are exact opposites.A golden cross indicates a long-term bull market " +
                "going forward, while a death cross signals a long-term bear market.");
            Master.bulletedlistDesc.Items.Add("The golden cross occurs when a short-term moving average crosses over a major long-term moving average " +
                "to the upside and is interpreted as signaling a definitive upward turn in a market. There are three stages to a golden cross.");
            Master.bulletedlistDesc.Items.Add("Stage 1 - A downtrend that eventually ends as selling is depleted");
            Master.bulletedlistDesc.Items.Add("Stage 2 - A second stage where the shorter moving average crosses up through the longer moving average");
            Master.bulletedlistDesc.Items.Add("Stage 3 - Finally, the continuing uptrend, hopefully leading to higher prices");
            Master.bulletedlistDesc.Items.Add("Conversely, a similar downside moving average crossover constitutes the death cross and is " +
                "understood to signal a decisive downturn in a market. The death cross occurs when the short term average trends down and crosses " +
                "the long-term average, basically going in the opposite direction of the golden cross.");
            Master.bulletedlistDesc.Items.Add("A golden cross indicates a long-term bull market going forward, while death cross signals a long-term " +
                "bear market. Both refer to the solid confirmation of a long-term trend by the occurrence of a short-term moving average crossing over " +
                "a major long-term moving average.");

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
            string symbol = Request.QueryString["symbol"].ToString();
            string exchange = Request.QueryString["exchange"].ToString();
            string outputsize = ddlDaily_Outputsize.SelectedValue;
            string seriestype = ddlDaily_SeriesType.SelectedValue;
            string time_interval = ddlDaily_Interval.SelectedValue;

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
                //dailyCloseSMA = stockManager.GetSMATable(symbol, exchange, seriestype, outputsize, time_interval,
                //                fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate,
                //                smallperiod, longperiod);

                dailyCloseSMA = stockManager.GetBacktestFromSMA(symbol, exchange, seriestype, outputsize, time_interval,
                                fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate,
                                smallperiod, longperiod); //buySpan: 0, sellSpan: 20, simulationQty: 100);
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

        public void ShowGraph()
        {
            string expression = "";
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataTable valuationTable = null;
            DataTable dailysmaTable = null;

            try
            {
                string symbol = Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();

                FillData();

                dailysmaTable = (DataTable)ViewState["FetchedData"];
                valuationTable = (DataTable)ViewState["VALUATION_TABLE"];
                sensexTable = (DataTable)ViewState["SENSEX"];
                niftyTable = (DataTable)ViewState["NIFTY50"];

                GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
                GridViewDaily.DataBind();
                GridViewSMA1.DataSource = (DataTable)ViewState["FetchedData"];
                GridViewSMA1.DataBind();
                GridViewSMA2.DataSource = (DataTable)ViewState["FetchedData"];
                GridViewSMA2.DataBind();

                if ((dailysmaTable != null) && (dailysmaTable.Rows.Count > 0))
                {
                    //showCandleStickGraph(ohlcData);
                    //showSMA(sma1Data, "SMA1");
                    //showSMA(sma2Data, "SMA2");
                    chartCrossover.Series["Open"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "OPEN", "");
                    chartCrossover.Series["High"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "HIGH", "");
                    chartCrossover.Series["Low"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "LOW", "");
                    chartCrossover.Series["Close"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "CLOSE", "");
                    chartCrossover.Series["Volume"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "VOLUME", "");
                    chartCrossover.Series["OHLC"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "HIGH,LOW,OPEN,CLOSE", "");
                    chartCrossover.Series["SMA1"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "SMA_SMALL,BUY_FLAG,SELL_FLAG", "");
                    chartCrossover.Series["SMA2"].Points.DataBind(dailysmaTable.AsEnumerable(), "TIMESTAMP", "SMA_LONG,BUY_FLAG,SELL_FLAG", "");

                    chartCrossover.ChartAreas[0].AxisX2.IsStartedFromZero = true;
                    chartCrossover.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartCrossover.ChartAreas[1].AxisX.IsStartedFromZero = true;


                    findGoldenCross(dailysmaTable, dailysmaTable);
                    //findGoldenCross();

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartCrossover.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartCrossover.Annotations.FindByName(item.Value) != null)
                                chartCrossover.Annotations.Clear();
                        }
                    }
                    //Master.headingtext.Text = "Crossover (Buy-Sell Signal)/(Golden-Death Cross): " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Crossover (Buy-Sell Signal)/(Golden-Death Cross): " + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Crossover (Buy-Sell Signal)/(Golden-Death Cross): " + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }
        //public void ShowGraph(string scriptName)
        //{
        //    string folderPath = Server.MapPath("~/scriptdata/");
        //    bool bIsTestOn = true;
        //    DataTable ohlcData = null;
        //    DataTable sma1Data = null;
        //    DataTable sma2Data = null;
        //    DataTable tempData = null;
        //    string expression = "";
        //    string outputSize = "";
        //    string interval1 = "";
        //    string period1 = "";
        //    string seriestype1 = "";
        //    string interval2 = "";
        //    string period2 = "";
        //    string seriestype2 = "";
        //    string fromDate = "", toDate = "";
        //    DataRow[] filteredRows = null;

        //    try
        //    {
        //        if (((ViewState["FetchedDataOHLC"] == null) || (ViewState["FetchedDataSMA1"] == null) || (ViewState["FetchedDataSMA2"] == null))
        //        || ((((DataTable)ViewState["FetchedDataOHLC"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataSMA1"]).Rows.Count == 0) ||
        //             (((DataTable)ViewState["FetchedDataSMA2"]).Rows.Count == 0)))
        //        {
        //            if (Session["IsTestOn"] != null)
        //            {
        //                bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
        //            }

        //            if (Session["DATAFOLDER"] != null)
        //            {
        //                folderPath = Session["DATAFOLDER"].ToString();
        //            }
        //            if ((Request.QueryString["size"] != null) &&
        //                 (Request.QueryString["period1"] != null) && (Request.QueryString["interval1"] != null) &&
        //                 (Request.QueryString["seriestype1"] != null) && (Request.QueryString["period2"] != null) &&
        //                 (Request.QueryString["interval2"] != null) && (Request.QueryString["seriestype2"] != null))
        //            {
        //                outputSize = Request.QueryString["size"].ToString();
        //                //ohlcData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
        //                //if (ohlcData == null)
        //                //{
        //                //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
        //                ohlcData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
        //                                        bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
        //                //}
        //                ViewState["FetchedDataOHLC"] = ohlcData;

        //                interval1 = Request.QueryString["interval1"].ToString();
        //                period1 = Request.QueryString["period1"].ToString();
        //                seriestype1 = Request.QueryString["seriestype1"].ToString();

        //                //sma1Data = StockApi.getSMA(folderPath, scriptName, day_interval: interval1, period: period1,
        //                //    seriestype: seriestype1, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());

        //                sma1Data = StockApi.getSMAAlternate(folderPath, scriptName, day_interval: interval1, period: period1,
        //                    seriestype: seriestype1, outputsize: outputSize, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(),
        //                    dailyTable: ohlcData);
        //                ViewState["FetchedDataSMA1"] = sma1Data;

        //                interval2 = Request.QueryString["interval2"].ToString();
        //                period2 = Request.QueryString["period2"].ToString();
        //                seriestype2 = Request.QueryString["seriestype2"].ToString();

        //                //sma2Data = StockApi.getSMA(folderPath, scriptName, day_interval: interval2, period: period2,
        //                //    seriestype: seriestype2, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
        //                sma2Data = StockApi.getSMAAlternate(folderPath, scriptName, day_interval: interval2, period: period2,
        //                    seriestype: seriestype2, outputsize: outputSize, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(),
        //                    dailyTable: ohlcData);
        //                ViewState["FetchedDataSMA2"] = sma2Data;

        //            }
        //            else
        //            {
        //                ViewState["FetchedDataOHLC"] = null;
        //                ohlcData = null;

        //                ViewState["FetchedDataSMA1"] = null;
        //                sma1Data = null;

        //                ViewState["FetchedDataSMA2"] = null;
        //                sma2Data = null;
        //            }

        //            GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataOHLC"];
        //            GridViewDaily.DataBind();
        //            GridViewSMA1.DataSource = (DataTable)ViewState["FetchedDataSMA1"];
        //            GridViewSMA1.DataBind();
        //            GridViewSMA2.DataSource = (DataTable)ViewState["FetchedDataSMA2"];
        //            GridViewSMA2.DataBind();
        //        }

        //        //else
        //        //{
        //        if (ViewState["FromDate"] != null)
        //            fromDate = ViewState["FromDate"].ToString();
        //        if (ViewState["ToDate"] != null)
        //            toDate = ViewState["ToDate"].ToString();

        //        if ((fromDate.Length > 0) && (toDate.Length > 0))
        //        {
        //            tempData = (DataTable)ViewState["FetchedDataOHLC"];
        //            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
        //            filteredRows = tempData.Select(expression);
        //            if ((filteredRows != null) && (filteredRows.Length > 0))
        //                ohlcData = filteredRows.CopyToDataTable();

        //            tempData.Clear();
        //            tempData = null;

        //            tempData = (DataTable)ViewState["FetchedDataSMA1"];
        //            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
        //            filteredRows = tempData.Select(expression);
        //            if ((filteredRows != null) && (filteredRows.Length > 0))
        //                sma1Data = filteredRows.CopyToDataTable();

        //            tempData.Clear();
        //            tempData = null;

        //            tempData = (DataTable)ViewState["FetchedDataSMA2"];
        //            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
        //            filteredRows = tempData.Select(expression);
        //            if ((filteredRows != null) && (filteredRows.Length > 0))
        //                sma2Data = filteredRows.CopyToDataTable();
        //        }
        //        else
        //        {
        //            ohlcData = (DataTable)ViewState["FetchedDataOHLC"];
        //            sma1Data = (DataTable)ViewState["FetchedDataSMA1"];
        //            sma2Data = (DataTable)ViewState["FetchedDataSMA2"];
        //        }
        //        //}

        //        if ((ohlcData != null) && (sma1Data != null) && (sma2Data != null))
        //        {
        //            //showCandleStickGraph(ohlcData);
        //            //showSMA(sma1Data, "SMA1");
        //            //showSMA(sma2Data, "SMA2");
        //            chartCrossover.Series["Open"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Open", "");
        //            chartCrossover.Series["High"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "High", "");
        //            chartCrossover.Series["Low"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Low", "");
        //            chartCrossover.Series["Close"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Close", "");
        //            chartCrossover.Series["Volume"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Volume", "");
        //            chartCrossover.Series["OHLC"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "High,Low,Open,Close", "");
        //            chartCrossover.Series["SMA1"].Points.DataBind(sma1Data.AsEnumerable(), "Date", "SMA", "");
        //            chartCrossover.Series["SMA2"].Points.DataBind(sma2Data.AsEnumerable(), "Date", "SMA", "");

        //            chartCrossover.ChartAreas[0].AxisX2.IsStartedFromZero = true;
        //            chartCrossover.ChartAreas[0].AxisX.IsStartedFromZero = true;
        //            chartCrossover.ChartAreas[1].AxisX.IsStartedFromZero = true;


        //            findGoldenCross(sma1Data, sma2Data);

        //            foreach (ListItem item in Master.checkboxlistLines.Items)
        //            {
        //                chartCrossover.Series[item.Value].Enabled = item.Selected;
        //                if (item.Selected == false)
        //                {
        //                    if (chartCrossover.Annotations.FindByName(item.Value) != null)
        //                        chartCrossover.Annotations.Clear();
        //                }
        //            }
        //            //Master.headingtext.Text = "Crossover (Buy-Sell Signal)/(Golden-Death Cross): " + Request.QueryString["script"].ToString();
        //            Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
        //        }
        //        else
        //        {
        //            if (expression.Length == 0)
        //            {
        //                Master.headingtext.Text = "Crossover (Buy-Sell Signal)/(Golden-Death Cross): " + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
        //            }
        //            else
        //            {
        //                Master.headingtext.Text = "Crossover (Buy-Sell Signal)/(Golden-Death Cross): " + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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


        public void findGoldenCross()
        {
            if ((chartCrossover.Series["SMA1"].Enabled) && (chartCrossover.Series["SMA2"].Enabled))
            {
                string buy_flag, sell_flag;
                DataPointCollection sma1Points = chartCrossover.Series["SMA1"].Points;
                DataPointCollection sma2Points = chartCrossover.Series["SMA2"].Points;
                foreach (DataPoint sma1Point in sma1Points.AsEnumerable())
                {
                    buy_flag = sma1Point.YValues[1].ToString();
                    sell_flag = sma1Point.YValues[2].ToString();
                    if (buy_flag.Equals("1"))
                    {
                        StripLine stripLine1 = new StripLine();
                        stripLine1.StripWidth = 0;
                        stripLine1.BorderColor = System.Drawing.Color.RoyalBlue;
                        stripLine1.BorderWidth = 2;
                        stripLine1.BorderDashStyle = ChartDashStyle.Dot;
                        stripLine1.Interval = sma1Point.YValues[0];
                        stripLine1.BackColor = System.Drawing.Color.RosyBrown;
                        stripLine1.BackSecondaryColor = System.Drawing.Color.Purple;
                        stripLine1.BackGradientStyle = GradientStyle.TopBottom;
                        stripLine1.Text = "Crossover";
                        stripLine1.TextAlignment = StringAlignment.Near;
                        // Add the strip line to the chart
                        chartCrossover.ChartAreas[0].AxisY.StripLines.Add(stripLine1);
                    }
                }
            }

        }

        public void findGoldenCross(DataTable sma1Data, DataTable sma2Data)
        {
            if ((chartCrossover.Series["SMA1"].Enabled) && (chartCrossover.Series["SMA2"].Enabled))
            {
                DataPointCollection sma1Points = chartCrossover.Series["SMA1"].Points;
                DataPointCollection sma2Points = chartCrossover.Series["SMA2"].Points;
                foreach (DataPoint sma1Point in sma1Points.AsEnumerable())
                {
                    //DataPoint pt = sma2Points.FindByValue(sma1Point.XValue, "X");

                    //if( (pt != null) && (pt.YValues[0] == sma1Point.YValues[0]))
                    if (chartCrossover.Series["SMA2"].Points.Contains(sma1Point))
                    {
                        StripLine stripLine1 = new StripLine();
                        stripLine1.StripWidth = 0;
                        stripLine1.BorderColor = System.Drawing.Color.RoyalBlue;
                        stripLine1.BorderWidth = 2;
                        stripLine1.BorderDashStyle = ChartDashStyle.Dot;
                        stripLine1.Interval = sma1Point.YValues[0];
                        stripLine1.BackColor = System.Drawing.Color.RosyBrown;
                        stripLine1.BackSecondaryColor = System.Drawing.Color.Purple;
                        stripLine1.BackGradientStyle = GradientStyle.TopBottom;
                        stripLine1.Text = "Crossover";
                        stripLine1.TextAlignment = StringAlignment.Near;
                        // Add the strip line to the chart
                        //chartCrossover.ChartAreas[0].AxisX.StripLines.Add(stripLine1);
                        chartCrossover.ChartAreas[0].AxisY.StripLines.Add(stripLine1);
                    }
                }
            }

        }
        public void showCandleStickGraph(DataTable scriptData)
        {
            //chartVWAP_Intra.DataSource = scriptData;
            chartCrossover.Series["OHLC"].Points.DataBind(scriptData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
            //chartVWAP_Intra.DataBind();
            chartCrossover.Series["OHLC"].XValueMember = "Date";
            chartCrossover.Series["OHLC"].XValueType = ChartValueType.Date;
            chartCrossover.Series["OHLC"].YValueMembers = "Open,High,Low,Close";

            chartCrossover.Series["OHLC"].BorderColor = System.Drawing.Color.Black;
            chartCrossover.Series["OHLC"].Color = System.Drawing.Color.Black;
            chartCrossover.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red";
            chartCrossover.Series["OHLC"].XValueType = ChartValueType.DateTime;
            chartCrossover.Series["OHLC"]["OpenCloseStyle"] = "Triangle";
            chartCrossover.Series["OHLC"]["ShowOpenClose"] = "Both";
            //chartCrossover.Series["OHLC"]["PriceDownColor"] = "Triangle";
            //chartCrossover.Series["OHLC"]["PriceUpColor"] = "Both";

            chartCrossover.ChartAreas["chartareaCrossover"].AxisX.MajorGrid.LineWidth = 1;
            chartCrossover.ChartAreas["chartareaCrossover"].AxisY.MajorGrid.LineWidth = 1;
            chartCrossover.ChartAreas["chartareaCrossover"].AxisY.Minimum = 0;
            //chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Maximum = chartdailyGraph.Series["OHLC"].Points.FindMaxByValue("Y1", 0).YValues[0];
            chartCrossover.DataManipulator.IsStartFromFirst = true;

            chartCrossover.ChartAreas["chartareaCrossover"].AxisX.Title = "Date-" + "OHLC";
            chartCrossover.ChartAreas["chartareaCrossover"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartCrossover.ChartAreas["chartareaCrossover"].AxisY.Title = "OHLC";
            chartCrossover.ChartAreas["chartareaCrossover"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            //chartCrossover.ChartAreas["chartareaCrossover"].AxisX.LabelStyle.Format = "g";

            chartCrossover.Series["OHLC"].Enabled = true;

            if (chartCrossover.Annotations.Count > 0)
                chartCrossover.Annotations.Clear();
        }

        public void showSMA(DataTable scriptData, string seriesName)
        {
            chartCrossover.Series[seriesName].Points.DataBind(scriptData.AsEnumerable(), "Date", "SMA", "");
            (chartCrossover.Series[seriesName]).XValueMember = "Date";
            (chartCrossover.Series[seriesName]).XValueType = ChartValueType.Date;
            (chartCrossover.Series[seriesName]).YValueMembers = "SMA";
            //(chartVWAP_Intra.Series["VWAP"]).ToolTip = "SMA: Date:#VALX;   Value:#VALY";


            chartCrossover.ChartAreas["chartareaCrossover"].AxisX2.Title = "Date-" + "SMA";
            chartCrossover.ChartAreas["chartareaCrossover"].AxisX2.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartCrossover.ChartAreas["chartareaCrossover"].AxisY2.Title = "SMA";
            chartCrossover.ChartAreas["chartareaCrossover"].AxisY2.TitleAlignment = System.Drawing.StringAlignment.Center;
            //chartCrossover.ChartAreas["chartareaCrossover"].AxisX2.LabelStyle.Format = "g";

            chartCrossover.Series[seriesName].Enabled = true;

            //chartVWAP.Titles["titleVWAP"].Text = $"{"Volume Weighted Average Price - "}{scriptName}";
            if (chartCrossover.Annotations.Count > 0)
                chartCrossover.Annotations.Clear();
        }

        protected void chartCrossover_Click(object sender, ImageMapEventArgs e)
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

                if (chartCrossover.Annotations.Count > 0)
                    chartCrossover.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("SMA1") || seriesName.Equals("SMA2"))
                {
                    HA.AxisX = chartCrossover.ChartAreas[0].AxisX2;
                    HA.AxisY = chartCrossover.ChartAreas[0].AxisY2;

                    VA.AxisX = chartCrossover.ChartAreas[0].AxisX2;
                    VA.AxisY = chartCrossover.ChartAreas[0].AxisY2;

                    ra.AxisX = chartCrossover.ChartAreas[0].AxisX2;
                    ra.AxisY = chartCrossover.ChartAreas[0].AxisY2;
                    chartIndex = 0;
                }
                else if (seriesName.Equals("Volume"))
                {
                    HA.AxisX = chartCrossover.ChartAreas[1].AxisX;
                    HA.AxisY = chartCrossover.ChartAreas[1].AxisY;

                    VA.AxisX = chartCrossover.ChartAreas[1].AxisX;
                    VA.AxisY = chartCrossover.ChartAreas[1].AxisY;

                    ra.AxisX = chartCrossover.ChartAreas[1].AxisX;
                    ra.AxisY = chartCrossover.ChartAreas[1].AxisY;

                    chartIndex = 1;
                }
                else
                {
                    HA.AxisX = chartCrossover.ChartAreas[0].AxisX;
                    HA.AxisY = chartCrossover.ChartAreas[0].AxisY;

                    VA.AxisX = chartCrossover.ChartAreas[0].AxisX;
                    VA.AxisY = chartCrossover.ChartAreas[0].AxisY;

                    ra.AxisX = chartCrossover.ChartAreas[0].AxisX;
                    ra.AxisY = chartCrossover.ChartAreas[0].AxisY;
                    chartIndex = 0;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartCrossover.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartCrossover.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartCrossover.ChartAreas[chartIndex].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartCrossover.Annotations.Add(VA);

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
                    ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[4] + "\n" + "High:" + postBackValues[2] + "\n" +
                                "Low:" + postBackValues[3] + "\n" + "Close:" + postBackValues[5];
                }
                else
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                }
                //ra.SmartLabelStyle = sl;

                chartCrossover.Annotations.Add(ra);
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
            if ((GridViewDaily.Visible) || (GridViewSMA1.Visible) || (GridViewSMA2.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewSMA1.Visible = false;
                GridViewSMA2.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                GridViewDaily.Visible = true;
                GridViewSMA1.Visible = true;
                GridViewSMA2.Visible = true;
            }
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewDaily.DataBind();
        }

        protected void GridViewSMA1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewSMA1.PageIndex = e.NewPageIndex;
            GridViewSMA1.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewSMA1.DataBind();
        }

        protected void GridViewSMA2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewSMA2.PageIndex = e.NewPageIndex;
            GridViewSMA2.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewSMA2.DataBind();
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