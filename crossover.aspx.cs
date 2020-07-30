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
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataOHLC"] = null;
                    ViewState["FetchedDataSMA1"] = null;
                    ViewState["FetchedDataSMA2"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    headingtext.InnerText = "Crossover : " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartCrossover.Visible = true;
                        chartCrossover.Width = int.Parse(panelWidth.Value);
                        chartCrossover.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                Response.Redirect(".\\Default.aspx");
            }
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable ohlcData = null;
            DataTable sma1Data = null;
            DataTable sma2Data = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize = "";
            string interval1 = "";
            string period1 = "";
            string seriestype1 = "";
            string interval2 = "";
            string period2 = "";
            string seriestype2 = "";
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;


            if( ((ViewState["FetchedDataOHLC"] == null) || (ViewState["FetchedDataSMA1"] == null) || (ViewState["FetchedDataSMA2"] == null))
                || ( (((DataTable)ViewState["FetchedDataOHLC"]).Rows.Count ==0)  || (((DataTable)ViewState["FetchedDataSMA1"]).Rows.Count == 0) ||
                     (((DataTable)ViewState["FetchedDataSMA2"]).Rows.Count == 0) ) )
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
                    ohlcData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false);
                    ViewState["FetchedDataOHLC"] = ohlcData;
                }
                else
                {
                    ViewState["FetchedDataOHLC"] = null;
                    ohlcData = null;
                }

                if ((Request.QueryString["period1"] != null) && (Request.QueryString["interval1"] != null) && (Request.QueryString["seriestype1"] != null))
                {
                    interval1 = Request.QueryString["interval1"].ToString();
                    period1 = Request.QueryString["period1"].ToString();
                    seriestype1 = Request.QueryString["seriestype1"].ToString();

                    sma1Data = StockApi.getSMA(folderPath, scriptName, day_interval: interval1, period: period1,
                        seriestype: seriestype1, bIsTestModeOn: bIsTestOn, bSaveData: false);
                    ViewState["FetchedDataSMA1"] = sma1Data;
                }
                else
                {
                    ViewState["FetchedDataSMA1"] = null;
                    sma1Data = null;
                }

                if ((Request.QueryString["period2"] != null) && (Request.QueryString["interval2"] != null) && (Request.QueryString["seriestype2"] != null))
                {
                    interval2 = Request.QueryString["interval2"].ToString();
                    period2 = Request.QueryString["period2"].ToString();
                    seriestype2 = Request.QueryString["seriestype2"].ToString();

                    sma2Data = StockApi.getSMA(folderPath, scriptName, day_interval: interval2, period: period2,
                        seriestype: seriestype2, bIsTestModeOn: bIsTestOn, bSaveData: false);
                    ViewState["FetchedDataSMA2"] = sma2Data;
                }
                else
                {
                    ViewState["FetchedDataSMA2"] = null;
                    sma2Data = null;
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

                    tempData = (DataTable)ViewState["FetchedDataSMA1"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        sma1Data = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataSMA2"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        sma2Data = filteredRows.CopyToDataTable();
                }
                else
                {
                    ohlcData = (DataTable)ViewState["FetchedDataOHLC"];
                    sma1Data = (DataTable)ViewState["FetchedDataSMA1"];
                    sma2Data = (DataTable)ViewState["FetchedDataSMA2"];
                }
            //}

            if ((ohlcData != null) && (sma1Data != null) && (sma2Data != null))
            {
                showCandleStickGraph(ohlcData);
                showSMA(sma1Data, "SMA1");
                showSMA(sma2Data, "SMA2");
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

            chartCrossover.ChartAreas["chartareaCrossover"].AxisX.Title = "Date";
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


            chartCrossover.ChartAreas["chartareaCrossover"].AxisX2.Title = "Date";
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
            string legendName;

            DataPoint p;
            //double lineHeight = -35;

            if (e.PostBackValue.Contains(','))
            {
                postBackValues = e.PostBackValue.Split(',');
                xDate = System.Convert.ToDateTime(postBackValues[0]);
                lineWidth = xDate.ToOADate();

                lineHeight = System.Convert.ToDouble(postBackValues[1]);
                seriesName = postBackValues[2];

                if (chartCrossover.Annotations.Count > 0)
                    chartCrossover.Annotations.Clear();

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                if (seriesName.Equals("OHLC"))
                {
                    HA.AxisX = chartCrossover.ChartAreas[0].AxisX;
                    HA.AxisY = chartCrossover.ChartAreas[0].AxisY;
                }
                else
                {
                    HA.AxisX = chartCrossover.ChartAreas[0].AxisX2;
                    HA.AxisY = chartCrossover.ChartAreas[0].AxisY2;
                }
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartCrossover.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartCrossover.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                if (seriesName.Equals("OHLC"))
                {
                    VA.AxisX = chartCrossover.ChartAreas[0].AxisX;
                    VA.AxisY = chartCrossover.ChartAreas[0].AxisY;
                }
                else
                {
                    VA.AxisX = chartCrossover.ChartAreas[0].AxisX2;
                    VA.AxisY = chartCrossover.ChartAreas[0].AxisY2;
                }
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartCrossover.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartCrossover.Annotations.Add(VA);

                //p = (chartCrossover.Series[seriesName]).Points.FindByValue(lineHeight, "Y4");
                p = (chartCrossover.Series[seriesName]).Points.FindByValue(lineWidth, "X");
                if (p != null)
                {
                    p.MarkerSize = 8;
                    p.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    //p.Label = postBackValues[2] + "\n" + postBackValues[0] + "\n" + "Close:" + postBackValues[1];
                    p.Label = postBackValues[2] + "\n" + postBackValues[0] + "\n" + "Close:" + p.YValues[3].ToString();
                    p.LabelBackColor = System.Drawing.Color.Transparent;
                    p.LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
                    p.LabelBorderColor = System.Drawing.Color.Black;
                    p.IsValueShownAsLabel = true;
                }
            }
            else
            {
                legendName = e.PostBackValue;
                if (legendName.ToUpper().Equals("OHLC"))
                {
                    chartCrossover.Series["OHLC"].Enabled = !(chartCrossover.Series["OHLC"].Enabled);
                }
                else if (legendName.ToUpper().Equals("SMA1"))
                {
                    chartCrossover.Series["SMA1"].Enabled = !(chartCrossover.Series["SMA1"].Enabled);
                }
                else if (legendName.ToUpper().Equals("SMA2"))
                {
                    chartCrossover.Series["SMA2"].Enabled = !(chartCrossover.Series["SMA2"].Enabled);
                }
            }

        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            string fromDate = textboxFromDate.Text;
            string toDate = textboxToDate.Text;
            string scriptName = Request.QueryString["script"].ToString();
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            ShowGraph(scriptName);
        }
    }
}