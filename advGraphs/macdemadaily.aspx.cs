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
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "MACD Vs EMA Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();
                    headingtext.Text = "MACD Vs EMA Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartMACDEMADaily.Visible = true;
                        chartMACDEMADaily.Width = int.Parse(panelWidth.Value);
                        chartMACDEMADaily.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Response.Redirect("~/Default.aspx");
            }
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
                    if (Request.QueryString["size"] != null)
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        ohlcData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, 
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataOHLC"] = ohlcData;
                    }
                    else
                    {
                        ViewState["FetchedDataOHLC"] = null;
                        ohlcData = null;
                    }

                    if ((Request.QueryString["interval"] != null) && (Request.QueryString["seriestype"] != null) && (Request.QueryString["fastperiod"] != null) &&
                        (Request.QueryString["slowperiod"] != null) && (Request.QueryString["signalperiod"] != null))
                    {
                        interval = Request.QueryString["interval"].ToString();
                        seriestype = Request.QueryString["seriestype"].ToString();
                        fastperiod = Request.QueryString["fastperiod"].ToString();
                        slowperiod = Request.QueryString["slowperiod"].ToString();
                        signalperiod = Request.QueryString["signalperiod"].ToString();

                        ema12Data = StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: fastperiod,
                            seriestype: seriestype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataEMA12"] = ema12Data;

                        ema26Data = StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: slowperiod,
                            seriestype: seriestype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataEMA26"] = ema26Data;

                        macdData = StockApi.getMACD(folderPath, scriptName, day_interval: interval, seriestype: seriestype, fastperiod: fastperiod,
                                                    slowperiod: slowperiod, signalperiod: signalperiod, 
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataMACD"] = macdData;
                    }
                    else
                    {
                        ViewState["FetchedDataEMA12"] = null;
                        ema12Data = null;
                        ViewState["FetchedDataEMA26"] = null;
                        ema26Data = null;
                        ViewState["FetchedDataMACD"] = null;
                        macdData = null;
                    }
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


                    if (checkBoxOpen.Checked)
                        //showOpenLine(scriptData);
                        chartMACDEMADaily.Series["Open"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["Open"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("Open") != null)
                            chartMACDEMADaily.Annotations.Clear();
                    }

                    if (checkBoxHigh.Checked)
                        //showHighLine(scriptData);
                        chartMACDEMADaily.Series["High"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["High"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("High") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }
                    if (checkBoxLow.Checked)
                        //showLowLine(scriptData);
                        chartMACDEMADaily.Series["Low"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["Low"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("Low") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }

                    if (checkBoxClose.Checked)
                        //showCloseLine(scriptData);
                        chartMACDEMADaily.Series["Close"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["Close"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("Close") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }

                    if (checkBoxCandle.Checked)
                        //showCandleStickGraph(scriptData);
                        chartMACDEMADaily.Series["OHLC"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["OHLC"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("OHLC") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }

                    //if (checkBoxVolume.Checked)
                    //    //showVolumeGraph(scriptData);
                    //    chartMACDEMADaily.Series["Volume"].Enabled = true;
                    //else
                    //{
                    //    chartMACDEMADaily.Series["Volume"].Enabled = false;
                    //    if (chartMACDEMADaily.Annotations.FindByName("Volume") != null)
                    //        chartMACDEMADaily.Annotations.Clear();

                    //}
                    if (checkBoxEMA12.Checked)
                        //showVolumeGraph(scriptData);
                        chartMACDEMADaily.Series["EMA12"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["EMA12"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("EMA12") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }
                    if (checkBoxEMA26.Checked)
                        //showVolumeGraph(scriptData);
                        chartMACDEMADaily.Series["EMA26"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["EMA26"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("EMA26") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }
                    if (checkBoxMACD.Checked)
                        //showVolumeGraph(scriptData);
                        chartMACDEMADaily.Series["MACD"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["MACD"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("MACD") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }
                    if (checkBoxMACD_Signal.Checked)
                        //showVolumeGraph(scriptData);
                        chartMACDEMADaily.Series["MACD_Signal"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["MACD_Signal"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("MACD_Signal") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }
                    if (checkBoxMACD_Hist.Checked)
                        //showVolumeGraph(scriptData);
                        chartMACDEMADaily.Series["MACD_Hist"].Enabled = true;
                    else
                    {
                        chartMACDEMADaily.Series["MACD_Hist"].Enabled = false;
                        if (chartMACDEMADaily.Annotations.FindByName("MACD_Hist") != null)
                            chartMACDEMADaily.Annotations.Clear();

                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = ohlcData;
                        GridViewDaily.DataBind();

                        GridViewEMA12.Visible = true;
                        GridViewEMA12.DataSource = ema12Data;
                        GridViewEMA12.DataBind();

                        GridViewEMA26.Visible = true;
                        GridViewEMA26.DataSource = ema26Data;
                        GridViewEMA26.DataBind();

                        GridViewMACD.Visible = true;
                        GridViewMACD.DataSource = macdData;
                        GridViewMACD.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                        GridViewEMA12.Visible = false;
                        GridViewEMA26.Visible = false;
                        GridViewMACD.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
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

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            ShowGraph(scriptName);
        }
        protected void buttonDesc_Click(object sender, EventArgs e)
        {
            if (trid.Visible)
                trid.Visible = false;
            else
                trid.Visible = true;
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
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
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
    }
}