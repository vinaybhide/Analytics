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
    public partial class rsidaily : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataDaily"] = null;
                    ViewState["FetchedDataRSI"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "RSI Vs Daily Price: " + Request.QueryString["script"].ToString();
                    headingtext.Text = "RSI Vs Daily Price: " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartRSIDaily.Visible = true;
                        chartRSIDaily.Width = int.Parse(panelWidth.Value);
                        chartRSIDaily.Height = int.Parse(panelHeight.Value);
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
            DataTable dailyData = null;
            DataTable rsiData = null;
            DataTable tempData = null;
            string expression;
            string outputSize;
            string interval;
            string period;
            string series_type;
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataDaily"] == null) || (ViewState["FetchedDataRSI"] == null))
                     ||
                    ((((DataTable)ViewState["FetchedDataDaily"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataRSI"]).Rows.Count == 0))
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
                    if ((Request.QueryString["size"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null)
                        && (Request.QueryString["seriestype"] != null))
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval = Request.QueryString["interval"];
                        period = Request.QueryString["period"];
                        series_type = Request.QueryString["seriestype"];

                        dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataDaily"] = dailyData;

                        rsiData = StockApi.getRSI(folderPath, scriptName, day_interval: interval, period: period, seriestype: series_type,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataRSI"] = rsiData;

                    }
                    else
                    {
                        ViewState["FetchedDataDaily"] = null;
                        dailyData = null;
                        ViewState["FetchedDataRSI"] = null;
                        rsiData = null;
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
                    tempData = (DataTable)ViewState["FetchedDataDaily"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        dailyData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataRSI"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        rsiData = filteredRows.CopyToDataTable();
                }
                else
                {
                    dailyData = (DataTable)ViewState["FetchedDataDaily"];
                    rsiData = (DataTable)ViewState["FetchedDataRSI"];
                }
                //}

                if ((dailyData != null) && (rsiData != null))
                {
                    //showCandleStickGraph(intraData);
                    //showVWAP(vwapData);
                    chartRSIDaily.Series["Open"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open", "");
                    chartRSIDaily.Series["High"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High", "");
                    chartRSIDaily.Series["Low"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Low", "");
                    chartRSIDaily.Series["Close"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Close", "");
                    chartRSIDaily.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartRSIDaily.Series["RSI"].Points.DataBind(rsiData.AsEnumerable(), "Date", "RSI", "");

                    //chartRSIDaily.ChartAreas[1].AlignWithChartArea = chartRSIDaily.ChartAreas[0].Name;
                    chartRSIDaily.ChartAreas[1].AxisX.IsStartedFromZero = true;
                    chartRSIDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;

                    //if (chartRSIDaily.ChartAreas[1].AxisY.StripLines.Count == 0)
                    //{
                    //    StripLine stripLine1 = new StripLine();
                    //    stripLine1.StripWidth = 0;
                    //    stripLine1.BorderColor = System.Drawing.Color.RoyalBlue;
                    //    stripLine1.BorderWidth = 2;
                    //    stripLine1.BorderDashStyle = ChartDashStyle.Dot;
                    //    stripLine1.Interval = 50;
                    //    stripLine1.BackColor = System.Drawing.Color.RosyBrown;
                    //    stripLine1.BackSecondaryColor = System.Drawing.Color.Purple;
                    //    stripLine1.BackGradientStyle = GradientStyle.LeftRight;
                    //    stripLine1.Text = "50";
                    //    stripLine1.TextAlignment = StringAlignment.Near;
                    //    // Add the strip line to the chart
                    //    chartRSIDaily.ChartAreas[1].AxisY.StripLines.Add(stripLine1);

                    //    StripLine stripLine2 = new StripLine();
                    //    stripLine2.StripWidth = 0;
                    //    stripLine2.BorderColor = System.Drawing.Color.RoyalBlue;
                    //    stripLine2.BorderWidth = 2;
                    //    stripLine2.BorderDashStyle = ChartDashStyle.Dot;
                    //    stripLine2.Interval = 70;
                    //    stripLine2.BackColor = System.Drawing.Color.RosyBrown;
                    //    stripLine2.BackSecondaryColor = System.Drawing.Color.Purple;
                    //    stripLine2.BackGradientStyle = GradientStyle.LeftRight;
                    //    stripLine2.Text = "70";
                    //    stripLine2.TextAlignment = StringAlignment.Near;
                    //    // Add the strip line to the chart
                    //    chartRSIDaily.ChartAreas[1].AxisY.StripLines.Add(stripLine2);
                    //}

                    if (checkBoxOpen.Checked)
                        //showOpenLine(scriptData);
                        chartRSIDaily.Series["Open"].Enabled = true;
                    else
                    {
                        chartRSIDaily.Series["Open"].Enabled = false;
                        if (chartRSIDaily.Annotations.FindByName("Open") != null)
                            chartRSIDaily.Annotations.Clear();
                    }

                    if (checkBoxHigh.Checked)
                        //showHighLine(scriptData);
                        chartRSIDaily.Series["High"].Enabled = true;
                    else
                    {
                        chartRSIDaily.Series["High"].Enabled = false;
                        if (chartRSIDaily.Annotations.FindByName("High") != null)
                            chartRSIDaily.Annotations.Clear();

                    }
                    if (checkBoxLow.Checked)
                        //showLowLine(scriptData);
                        chartRSIDaily.Series["Low"].Enabled = true;
                    else
                    {
                        chartRSIDaily.Series["Low"].Enabled = false;
                        if (chartRSIDaily.Annotations.FindByName("Low") != null)
                            chartRSIDaily.Annotations.Clear();

                    }

                    if (checkBoxClose.Checked)
                        //showCloseLine(scriptData);
                        chartRSIDaily.Series["Close"].Enabled = true;
                    else
                    {
                        chartRSIDaily.Series["Close"].Enabled = false;
                        if (chartRSIDaily.Annotations.FindByName("Close") != null)
                            chartRSIDaily.Annotations.Clear();

                    }

                    if (checkBoxCandle.Checked)
                        //showCandleStickGraph(scriptData);
                        chartRSIDaily.Series["OHLC"].Enabled = true;
                    else
                    {
                        chartRSIDaily.Series["OHLC"].Enabled = false;
                        if (chartRSIDaily.Annotations.FindByName("OHLC") != null)
                            chartRSIDaily.Annotations.Clear();

                    }

                    if (checkBoxRSI.Checked)
                        //showVolumeGraph(scriptData);
                        chartRSIDaily.Series["RSI"].Enabled = true;
                    else
                    {
                        chartRSIDaily.Series["RSI"].Enabled = false;
                        if (chartRSIDaily.Annotations.FindByName("RSI") != null)
                            chartRSIDaily.Annotations.Clear();

                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = dailyData;
                        GridViewDaily.DataBind();

                        GridViewData.Visible = true;
                        GridViewData.DataSource = rsiData;
                        GridViewData.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                        GridViewData.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }

        protected void chartRSIDaily_Click(object sender, ImageMapEventArgs e)
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

                if (chartRSIDaily.Annotations.Count > 0)
                    chartRSIDaily.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);

                seriesName = postBackValues[0];


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //HA.Name = seriesName;
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("RSI"))
                {
                    HA.AxisX = chartRSIDaily.ChartAreas[1].AxisX;
                    HA.AxisY = chartRSIDaily.ChartAreas[1].AxisY;

                    VA.AxisX = chartRSIDaily.ChartAreas[1].AxisX;
                    VA.AxisY = chartRSIDaily.ChartAreas[1].AxisY;

                    ra.AxisX = chartRSIDaily.ChartAreas[1].AxisX;
                    ra.AxisY = chartRSIDaily.ChartAreas[1].AxisY;
                    chartIndex = 1;

                }
                else
                {
                    HA.AxisX = chartRSIDaily.ChartAreas[0].AxisX;
                    HA.AxisY = chartRSIDaily.ChartAreas[0].AxisY;

                    VA.AxisX = chartRSIDaily.ChartAreas[0].AxisX;
                    VA.AxisY = chartRSIDaily.ChartAreas[0].AxisY;

                    ra.AxisX = chartRSIDaily.ChartAreas[0].AxisX;
                    ra.AxisY = chartRSIDaily.ChartAreas[0].AxisY;
                    chartIndex = 0;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartRSIDaily.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartRSIDaily.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartRSIDaily.ChartAreas[chartIndex].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartRSIDaily.Annotations.Add(VA);

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

                chartRSIDaily.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
            }
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
        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
            GridViewDaily.DataBind();
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["FetchedDataRSI"];
            GridViewData.DataBind();
        }

    }
}