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
    public partial class macdemadaily : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Trend Reversal Indicator";
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataOHLC"] = null;
                    ViewState["FetchedDataEMA12"] = null;
                    ViewState["FetchedDataEMA26"] = null;
                    ViewState["FetchedDataMACD"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Trend Reversal Indicator-" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "MACD Vs EMA Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();

                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartMACDEMADaily.Visible = true;
                        chartMACDEMADaily.Width = int.Parse(Master.panelWidth.Value);
                        chartMACDEMADaily.Height = int.Parse(Master.panelHeight.Value);
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

            li = new ListItem("EMA12", "EMA12");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("EMA26", "EMA26");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("MACD", "MACD");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("MACD Signal", "MACD_Signal");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("MACD History", "MACD_Hist");
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
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("MACD is a trend - following momentum indicator that shows the relationship between two moving averages of a security’s price. The MACD is calculated by subtracting the 26 - period Exponential Moving Average(EMA) from the 12 - period EMA.");
            Master.bulletedlistDesc.Items.Add("You may buy the security when the MACD crosses above its signal line and sell -or short -the security when the MACD crosses below the signal line.");
            Master.bulletedlistDesc.Items.Add("The MACD generates a bullish signal when it moves above its own signal line, and it sends a sell sign when it moves below its signal line.");
            Master.bulletedlistDesc.Items.Add("The histogram is positive when the MACD is above its signal line and negative when the MACD is below its signal line.");
            Master.bulletedlistDesc.Items.Add("If prices are rising, the histogram grows larger as the speed of the price movement accelerates, and contracts as price movement decelerates.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable ohlcData = null;
            DataTable ema12Data = null;
            DataTable ema26Data = null;
            DataTable macdData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize = "";
            string interval = "";
            string seriestype = "";
            string fastperiod = "";
            string slowperiod = "";
            string signalperiod = "";
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataOHLC"] == null) || (ViewState["FetchedDataEMA12"] == null) || (ViewState["FetchedDataEMA26"] == null) || (ViewState["FetchedDataMACD"] == null))
                ||
                (
                (((DataTable)ViewState["FetchedDataOHLC"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataEMA12"]).Rows.Count == 0) ||
                     (((DataTable)ViewState["FetchedDataEMA26"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataMACD"]).Rows.Count == 0))
                     )
                {
                    if (Session["IsTestOn"] != null)
                    {
                        bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                    }

                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }

                    if ((Request.QueryString["interval"] != null) && (Request.QueryString["seriestype"] != null) && (Request.QueryString["fastperiod"] != null) &&
                        (Request.QueryString["slowperiod"] != null) && (Request.QueryString["signalperiod"] != null) &&
                        (Request.QueryString["size"] != null)
                        )
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval = Request.QueryString["interval"].ToString();
                        seriestype = Request.QueryString["seriestype"].ToString();
                        fastperiod = Request.QueryString["fastperiod"].ToString();
                        slowperiod = Request.QueryString["slowperiod"].ToString();
                        signalperiod = Request.QueryString["signalperiod"].ToString();

                        //ohlcData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        //if (ohlcData == null)
                        //{
                        //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                        ohlcData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
                                                bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataOHLC"] = ohlcData;


                        //ema12Data = StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: fastperiod,
                        //    seriestype: seriestype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ema12Data = StockApi.getEMAalternate(folderPath, scriptName, day_interval: interval, period: fastperiod,
                            seriestype: seriestype, outputsize:outputSize, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(), dailyDataTable:ohlcData);
                        ViewState["FetchedDataEMA12"] = ema12Data;

                        //ema26Data = StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: slowperiod,
                        //    seriestype: seriestype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ema26Data = StockApi.getEMAalternate(folderPath, scriptName, day_interval: interval, period: slowperiod,
                            seriestype: seriestype, outputsize: outputSize, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(), dailyDataTable:ohlcData);
                        ViewState["FetchedDataEMA26"] = ema26Data;

                        //macdData = StockApi.getMACD(folderPath, scriptName, day_interval: interval, seriestype: seriestype, fastperiod: fastperiod,
                        //                            slowperiod: slowperiod, signalperiod: signalperiod,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        macdData = StockApi.getMACDAlternate(folderPath, scriptName, day_interval: interval, seriestype: seriestype, fastperiod: fastperiod,
                                                    slowperiod: slowperiod, signalperiod: signalperiod,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(), dailyDataTable: ohlcData,
                                                    emaFastTable: ema12Data, emaSlowTable: ema26Data);
                                                    
                        ViewState["FetchedDataMACD"] = macdData;
                    }
                    else
                    {
                        ViewState["FetchedDataOHLC"] = null;
                        ohlcData = null;
                        ViewState["FetchedDataEMA12"] = null;
                        ema12Data = null;
                        ViewState["FetchedDataEMA26"] = null;
                        ema26Data = null;
                        ViewState["FetchedDataMACD"] = null;
                        macdData = null;
                    }
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataOHLC"];
                    GridViewDaily.DataBind();
                    GridViewEMA12.DataSource = (DataTable)ViewState["FetchedDataEMA12"];
                    GridViewEMA12.DataBind();
                    GridViewEMA26.DataSource = (DataTable)ViewState["FetchedDataEMA26"];
                    GridViewEMA26.DataBind();
                    GridViewMACD.DataSource = (DataTable)ViewState["FetchedDataMACD"];
                    GridViewMACD.DataBind();
                }

                //else
                //{
                if (ViewState["FromDate"] != null)
                    fromDate = ViewState["FromDate"].ToString();
                if (ViewState["ToDate"] != null)
                    toDate = ViewState["ToDate"].ToString();

                if ((fromDate.Length > 0) && (toDate.Length > 0))
                {
                    tempData = (DataTable)ViewState["FetchedDataOHLC"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        ohlcData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataEMA12"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        ema12Data = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataEMA26"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        ema26Data = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataMACD"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        macdData = filteredRows.CopyToDataTable();
                }
                else
                {
                    ohlcData = (DataTable)ViewState["FetchedDataOHLC"];
                    ema12Data = (DataTable)ViewState["FetchedDataEMA12"];
                    ema26Data = (DataTable)ViewState["FetchedDataEMA26"];
                    macdData = (DataTable)ViewState["FetchedDataMACD"];
                }
                //}

                if ((ohlcData != null) && (ema12Data != null) && (ema26Data != null) && (macdData != null))
                {
                    chartMACDEMADaily.Series["Open"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Open", "");
                    chartMACDEMADaily.Series["High"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "High", "");
                    chartMACDEMADaily.Series["Low"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Low", "");
                    chartMACDEMADaily.Series["Close"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Close", "");
                    //chartMACDEMADaily.Series["Volume"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Volume", "");
                    chartMACDEMADaily.Series["OHLC"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartMACDEMADaily.Series["EMA12"].Points.DataBind(ema12Data.AsEnumerable(), "Date", "EMA", "");
                    chartMACDEMADaily.Series["EMA26"].Points.DataBind(ema26Data.AsEnumerable(), "Date", "EMA", "");
                    chartMACDEMADaily.Series["MACD"].Points.DataBind(macdData.AsEnumerable(), "Date", "MACD", "");
                    chartMACDEMADaily.Series["MACD_Signal"].Points.DataBind(macdData.AsEnumerable(), "Date", "MACD_Signal", "");
                    chartMACDEMADaily.Series["MACD_Hist"].Points.DataBind(macdData.AsEnumerable(), "Date", "MACD_Hist", "");

                    chartMACDEMADaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartMACDEMADaily.ChartAreas[0].AxisX2.IsStartedFromZero = true;

                    chartMACDEMADaily.ChartAreas[1].AxisX.IsStartedFromZero = true;
                    chartMACDEMADaily.ChartAreas[1].AxisX2.IsStartedFromZero = true;

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartMACDEMADaily.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartMACDEMADaily.Annotations.FindByName(item.Value) != null)
                                chartMACDEMADaily.Annotations.Clear();
                        }
                    }
                    Master.headingtext.Text = "Trend Reversal Indicator-" + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Trend Reversal Indicator-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Trend Reversal Indicator-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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
        public void showCandleStickGraph(DataTable scriptData)
        {
            //chartVWAP_Intra.DataSource = scriptData;
            chartMACDEMADaily.Series["OHLC"].Points.DataBind(scriptData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
            //chartVWAP_Intra.DataBind();
            chartMACDEMADaily.Series["OHLC"].XValueMember = "Date";
            chartMACDEMADaily.Series["OHLC"].XValueType = ChartValueType.Date;
            chartMACDEMADaily.Series["OHLC"].YValueMembers = "Open,High,Low,Close";

            chartMACDEMADaily.Series["OHLC"].BorderColor = System.Drawing.Color.Black;
            chartMACDEMADaily.Series["OHLC"].Color = System.Drawing.Color.Black;
            chartMACDEMADaily.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red";
            chartMACDEMADaily.Series["OHLC"].XValueType = ChartValueType.DateTime;
            chartMACDEMADaily.Series["OHLC"]["OpenCloseStyle"] = "Triangle";
            chartMACDEMADaily.Series["OHLC"]["ShowOpenClose"] = "Both";
            //chartMACDEMADaily.Series["OHLC"]["PriceDownColor"] = "Triangle";
            //chartMACDEMADaily.Series["OHLC"]["PriceUpColor"] = "Both";

            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisX.MajorGrid.LineWidth = 1;
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY.MajorGrid.LineWidth = 1;
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY.Minimum = 0;
            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY.Maximum = chartdailyGraph.Series["OHLC"].Points.FindMaxByValue("Y1", 0).YValues[0];
            chartMACDEMADaily.DataManipulator.IsStartFromFirst = true;

            chartMACDEMADaily.Series["OHLC"].Enabled = true;

            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisX.Title = "Date-" + "OHLC";
            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY.Title = "OHLC";
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            //chartCrossover.ChartAreas["chartareaMACDEMADaily1"].AxisX.LabelStyle.Format = "g";

            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisX.Enabled = AxisEnabled.False;

            if (chartMACDEMADaily.Annotations.Count > 0)
                chartMACDEMADaily.Annotations.Clear();
        }
        public void showEMA(DataTable scriptData, string seriesName)
        {
            chartMACDEMADaily.Series[seriesName].Points.DataBind(scriptData.AsEnumerable(), "Date", "EMA", "");
            (chartMACDEMADaily.Series[seriesName]).XValueMember = "Date";
            (chartMACDEMADaily.Series[seriesName]).XValueType = ChartValueType.Date;
            (chartMACDEMADaily.Series[seriesName]).YValueMembers = "EMA";
            //(chartMACDEMADaily.Series[seriesName]).ToolTip = "SMA: Date:#VALX;   Value:#VALY";


            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisX2.Title = "Date";
            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisX2.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY2.Title = "EMA";
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AxisY2.TitleAlignment = System.Drawing.StringAlignment.Center;
            //chartCrossover.ChartAreas["chartareaMACDEMADaily1"].AxisX2.LabelStyle.Format = "g";

            chartMACDEMADaily.Series[seriesName].Enabled = true;

            //chartMACDEMADaily.Titles["titleVWAP"].Text = $"{"Volume Weighted Average Price - "}{scriptName}";
            if (chartMACDEMADaily.Annotations.Count > 0)
                chartMACDEMADaily.Annotations.Clear();
        }
        public void showMACD(DataTable scriptData)
        {
            chartMACDEMADaily.Series["MACD"].Points.DataBind(scriptData.AsEnumerable(), "Date", "MACD", "");
            (chartMACDEMADaily.Series["MACD"]).XValueMember = "Date";
            (chartMACDEMADaily.Series["MACD"]).XValueType = ChartValueType.Date;
            (chartMACDEMADaily.Series["MACD"]).YValueMembers = "MACD";

            chartMACDEMADaily.Series["MACD"].Enabled = true;

            chartMACDEMADaily.Series["MACD_Signal"].Points.DataBind(scriptData.AsEnumerable(), "Date", "MACD_Signal", "");
            (chartMACDEMADaily.Series["MACD_Signal"]).XValueMember = "Date";
            (chartMACDEMADaily.Series["MACD_Signal"]).XValueType = ChartValueType.Date;
            (chartMACDEMADaily.Series["MACD_Signal"]).YValueMembers = "MACD_Signal";

            chartMACDEMADaily.Series["MACD_Signal"].Enabled = true;

            chartMACDEMADaily.Series["MACD_Hist"].Points.DataBind(scriptData.AsEnumerable(), "Date", "MACD_Hist", "");
            (chartMACDEMADaily.Series["MACD_Hist"]).XValueMember = "Date";
            (chartMACDEMADaily.Series["MACD_Hist"]).XValueType = ChartValueType.Date;
            (chartMACDEMADaily.Series["MACD_Hist"]).YValueMembers = "MACD_Hist";

            chartMACDEMADaily.Series["MACD_Hist"].Enabled = true;

            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisX.Title = "Date";
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisY.Title = "MACD";
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisY2.Title = "MACD_Hist";
            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;


            chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].AxisX2.Enabled = AxisEnabled.False;

            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].AlignWithChartArea = chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].Name;


            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].Position.Height = 43;
            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].Position.Width = 100;

            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].Position.Y = chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].Position.Bottom;
            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].Position.Height = chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].Position.Height;
            //chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily2"].Position.Width = chartMACDEMADaily.ChartAreas["chartareaMACDEMADaily1"].Position.Width;

            //to position chart areas
            //MyChart.Series[0].ChartArea = chartArea1.Name;
            //MyChart.Series[5].ChartArea = chartArea5.Name;
            //chartArea1.AlignWithChartArea = chartArea5.Name;
            //chartArea2.AlignWithChartArea = chartArea5.Name;
            //chartArea1.Position.Y = 0;
            //chartArea1.Position.Height = 43;
            //chartArea1.Position.Width = 100;
            //chartArea2.Position.Y = chartArea1.Position.Bottom + 1;
            //chartArea2.Position.Height = chartArea1.Position.Height;
            //chartArea2.Position.Width = chartArea1.Position.Width;

        }

        //protected void buttonShowGraph_Click(object sender, EventArgs e)
        public void buttonShowGraph_Click()
        {
            string scriptName = Request.QueryString["script"].ToString();
            ViewState["FromDate"] = Master.textboxFromDate.Text;
            ViewState["ToDate"] = Master.textboxToDate.Text;
            ShowGraph(scriptName);
        }
        //protected void buttonDesc_Click(object sender, EventArgs e)
        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }


        protected void chartMACDEMADaily_Click(object sender, ImageMapEventArgs e)
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

                if (chartMACDEMADaily.Annotations.Count > 0)
                    chartMACDEMADaily.Annotations.Clear();

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

                if (seriesName.Equals("EMA12") || seriesName.Equals("EMA26"))
                {
                    HA.AxisX = chartMACDEMADaily.ChartAreas[0].AxisX2;
                    HA.AxisY = chartMACDEMADaily.ChartAreas[0].AxisY2;

                    VA.AxisX = chartMACDEMADaily.ChartAreas[0].AxisX2;
                    VA.AxisY = chartMACDEMADaily.ChartAreas[0].AxisY2;

                    ra.AxisX = chartMACDEMADaily.ChartAreas[0].AxisX2;
                    ra.AxisY = chartMACDEMADaily.ChartAreas[0].AxisY2;
                    chartIndex = 0;
                }
                //else if (seriesName.Equals("Volume"))
                //{
                //    HA.AxisX = chartMACDEMADaily.ChartAreas[2].AxisX;
                //    HA.AxisY = chartMACDEMADaily.ChartAreas[2].AxisY;

                //    VA.AxisX = chartMACDEMADaily.ChartAreas[2].AxisX;
                //    VA.AxisY = chartMACDEMADaily.ChartAreas[2].AxisY;

                //    ra.AxisX = chartMACDEMADaily.ChartAreas[2].AxisX;
                //    ra.AxisY = chartMACDEMADaily.ChartAreas[2].AxisY;

                //    chartIndex = 2;
                //}
                else if (seriesName.Equals("MACD") || seriesName.Equals("MACD_Signal"))
                {
                    HA.AxisX = chartMACDEMADaily.ChartAreas[1].AxisX;
                    HA.AxisY = chartMACDEMADaily.ChartAreas[1].AxisY;

                    VA.AxisX = chartMACDEMADaily.ChartAreas[1].AxisX;
                    VA.AxisY = chartMACDEMADaily.ChartAreas[1].AxisY;

                    ra.AxisX = chartMACDEMADaily.ChartAreas[1].AxisX;
                    ra.AxisY = chartMACDEMADaily.ChartAreas[1].AxisY;

                    chartIndex = 1;
                }
                else if (seriesName.Equals("MACD_Hist"))
                {
                    HA.AxisX = chartMACDEMADaily.ChartAreas[1].AxisX2;
                    HA.AxisY = chartMACDEMADaily.ChartAreas[1].AxisY2;

                    VA.AxisX = chartMACDEMADaily.ChartAreas[1].AxisX2;
                    VA.AxisY = chartMACDEMADaily.ChartAreas[1].AxisY2;

                    ra.AxisX = chartMACDEMADaily.ChartAreas[1].AxisX2;
                    ra.AxisY = chartMACDEMADaily.ChartAreas[1].AxisY2;
                    chartIndex = 1;
                }
                else
                {
                    HA.AxisX = chartMACDEMADaily.ChartAreas[0].AxisX;
                    HA.AxisY = chartMACDEMADaily.ChartAreas[0].AxisY;

                    VA.AxisX = chartMACDEMADaily.ChartAreas[0].AxisX;
                    VA.AxisY = chartMACDEMADaily.ChartAreas[0].AxisY;

                    ra.AxisX = chartMACDEMADaily.ChartAreas[0].AxisX;
                    ra.AxisY = chartMACDEMADaily.ChartAreas[0].AxisY;
                    chartIndex = 0;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartMACDEMADaily.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartMACDEMADaily.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartMACDEMADaily.ChartAreas[chartIndex].Name;

                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartMACDEMADaily.Annotations.Add(VA);

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
                    ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[2] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[5];
                }
                else
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                }
                //ra.SmartLabelStyle = sl;

                chartMACDEMADaily.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataOHLC"];
            GridViewDaily.DataBind();
        }

        protected void GridViewEMA12_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewEMA12.PageIndex = e.NewPageIndex;
            GridViewEMA12.DataSource = (DataTable)ViewState["FetchedDataEMA12"];
            GridViewEMA12.DataBind();
        }

        protected void GridViewEMA26_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewEMA26.PageIndex = e.NewPageIndex;
            GridViewEMA26.DataSource = (DataTable)ViewState["FetchedDataEMA26"];
            GridViewEMA26.DataBind();
        }

        protected void GridViewMACD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMACD.PageIndex = e.NewPageIndex;
            GridViewMACD.DataSource = (DataTable)ViewState["FetchedDataMACD"];
            GridViewMACD.DataBind();
        }

        void buttonShowGrid_Click()
        {
            if ((GridViewDaily.Visible) || (GridViewEMA12.Visible) || (GridViewEMA26.Visible) || (GridViewMACD.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewEMA12.Visible = false;
                GridViewEMA26.Visible = false;
                GridViewMACD.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                //if (ViewState["FetchedDataOHLC"] != null)
                //{
                GridViewDaily.Visible = true;
                //    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataOHLC"];
                //    GridViewDaily.DataBind();
                //}
                //if (ViewState["FetchedDataEMA12"] != null)
                //{
                GridViewEMA12.Visible = true;
                //    GridViewEMA12.DataSource = (DataTable)ViewState["FetchedDataEMA12"];
                //    GridViewEMA12.DataBind();
                //}
                //if (ViewState["FetchedDataEMA26"] != null)
                //{
                GridViewEMA26.Visible = true;
                //   GridViewEMA26.DataSource = (DataTable)ViewState["FetchedDataEMA26"];
                //   GridViewEMA26.DataBind();
                //}
                //if (ViewState["FetchedDataMACD"] != null)
                //{
                GridViewMACD.Visible = true;
                //   GridViewMACD.DataSource = (DataTable)ViewState["FetchedDataMACD"];
                //   GridViewMACD.DataBind();
                //}

            }
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}