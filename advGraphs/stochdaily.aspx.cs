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
    public partial class stochdaily : System.Web.UI.Page
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
                    ViewState["FetchedDataSTOCH"] = null;
                    ViewState["FetchedDataRSI"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "Stochastics Vs Daily Price Vs RSI: " + Request.QueryString["script"].ToString();
                    headingtext.Text = "Stochastics Vs Daily Price Vs RSI: " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartSTOCHDaily.Visible = true;
                        chartSTOCHDaily.Width = int.Parse(panelWidth.Value);
                        chartSTOCHDaily.Height = int.Parse(panelHeight.Value);
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
            DataTable stochData = null;
            DataTable rsiData = null;
            DataTable tempData = null;
            string expression;
            string outputSize;
            string interval;
            string fastkperiod;
            string slowkperiod;
            string slowdperiod;
            string slowkmatype;
            string slowdmatype;
            string rsi_interval;
            string rsi_period;
            string rsi_seriestype;
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataDaily"] == null) || (ViewState["FetchedDataSTOCH"] == null))
                    ||
                    ((((DataTable)ViewState["FetchedDataDaily"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataSTOCH"]).Rows.Count == 0))
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
                    if ((Request.QueryString["size"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["fastkperiod"] != null)
                        && (Request.QueryString["slowkperiod"] != null) && (Request.QueryString["slowdperiod"] != null)
                        && (Request.QueryString["slowkmatype"] != null) && (Request.QueryString["slowdmatype"] != null)
                        && (Request.QueryString["rsiinterval"] != null) && (Request.QueryString["rsiperiod"] != null)
                        && (Request.QueryString["rsiseriestype"] != null))
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval = Request.QueryString["interval"];
                        fastkperiod = Request.QueryString["fastkperiod"];
                        slowkperiod = Request.QueryString["slowkperiod"];
                        slowdperiod = Request.QueryString["slowdperiod"];
                        slowkmatype = Request.QueryString["slowkmatype"];
                        slowdmatype = Request.QueryString["slowdmatype"];
                        rsi_interval = Request.QueryString["rsiinterval"];
                        rsi_period = Request.QueryString["rsiperiod"];
                        rsi_seriestype = Request.QueryString["rsiseriestype"];

                        dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataDaily"] = dailyData;

                        stochData = StockApi.getSTOCH(folderPath, scriptName, day_interval: interval, fastkperiod: fastkperiod, slowkperiod: slowkperiod,
                            slowdperiod: slowdperiod, slowkmatype: slowkmatype, slowdmatype: slowdmatype,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataSTOCH"] = stochData;

                        rsiData = StockApi.getRSI(folderPath, scriptName, day_interval: rsi_interval, period: rsi_period, seriestype: rsi_seriestype,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataRSI"] = rsiData;
                    }
                    else
                    {
                        ViewState["FetchedDataDaily"] = null;
                        dailyData = null;
                        ViewState["FetchedDataSTOCH"] = null;
                        stochData = null;
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

                    tempData = (DataTable)ViewState["FetchedDataSTOCH"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        stochData = filteredRows.CopyToDataTable();

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
                    stochData = (DataTable)ViewState["FetchedDataSTOCH"];
                    rsiData = (DataTable)ViewState["FetchedDataRSI"];
                }
                //}

                if ((dailyData != null) && (stochData != null))
                {
                    chartSTOCHDaily.Series["Open"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open", "");
                    chartSTOCHDaily.Series["High"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High", "");
                    chartSTOCHDaily.Series["Low"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Low", "");
                    chartSTOCHDaily.Series["Close"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Close", "");
                    chartSTOCHDaily.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartSTOCHDaily.Series["SlowK"].Points.DataBind(stochData.AsEnumerable(), "Date", "SlowK", "");
                    chartSTOCHDaily.Series["SlowD"].Points.DataBind(stochData.AsEnumerable(), "Date", "SlowD", "");
                    chartSTOCHDaily.Series["RSI"].Points.DataBind(rsiData.AsEnumerable(), "Date", "RSI", "");

                    chartSTOCHDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartSTOCHDaily.ChartAreas[1].AxisX.IsStartedFromZero = true;
                    chartSTOCHDaily.ChartAreas[2].AxisX.IsStartedFromZero = true;

                    //if (chartSTOCHDaily.ChartAreas[1].AxisY.StripLines.Count == 0)
                    //{
                    //    StripLine stripLine1 = new StripLine();
                    //    stripLine1.StripWidth = 0;
                    //    stripLine1.BorderColor = System.Drawing.Color.RoyalBlue;
                    //    stripLine1.BorderWidth = 2;
                    //    stripLine1.BorderDashStyle = ChartDashStyle.Dot;
                    //    stripLine1.Interval = 80;
                    //    stripLine1.BackColor = System.Drawing.Color.RosyBrown;
                    //    stripLine1.BackSecondaryColor = System.Drawing.Color.Purple;
                    //    stripLine1.BackGradientStyle = GradientStyle.LeftRight;
                    //    stripLine1.Text = "80";
                    //    stripLine1.TextAlignment = StringAlignment.Near;
                    //    // Add the strip line to the chart
                    //    chartSTOCHDaily.ChartAreas[1].AxisY.StripLines.Add(stripLine1);

                    //    StripLine stripLine2 = new StripLine();
                    //    stripLine2.StripWidth = 0;
                    //    stripLine2.BorderColor = System.Drawing.Color.RoyalBlue;
                    //    stripLine2.BorderWidth = 2;
                    //    stripLine2.BorderDashStyle = ChartDashStyle.Dot;
                    //    stripLine2.Interval = 30;
                    //    stripLine2.BackColor = System.Drawing.Color.RosyBrown;
                    //    stripLine2.BackSecondaryColor = System.Drawing.Color.Purple;
                    //    stripLine2.BackGradientStyle = GradientStyle.LeftRight;
                    //    stripLine2.Text = "30";
                    //    stripLine2.TextAlignment = StringAlignment.Near;
                    //    // Add the strip line to the chart
                    //    chartSTOCHDaily.ChartAreas[2].AxisY.StripLines.Add(stripLine2);

                    //    StripLine stripLine3 = new StripLine();
                    //    stripLine3.StripWidth = 0;
                    //    stripLine3.BorderColor = System.Drawing.Color.RoyalBlue;
                    //    stripLine3.BorderWidth = 2;
                    //    stripLine3.BorderDashStyle = ChartDashStyle.Dot;
                    //    stripLine3.Interval = 70;
                    //    stripLine3.BackColor = System.Drawing.Color.RosyBrown;
                    //    stripLine3.BackSecondaryColor = System.Drawing.Color.Purple;
                    //    stripLine3.BackGradientStyle = GradientStyle.LeftRight;
                    //    stripLine3.Text = "70";
                    //    stripLine3.TextAlignment = StringAlignment.Near;
                    //    // Add the strip line to the chart
                    //    chartSTOCHDaily.ChartAreas[2].AxisY.StripLines.Add(stripLine3);

                    //}

                    if (checkBoxOpen.Checked)
                        chartSTOCHDaily.Series["Open"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["Open"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("Open") != null)
                            chartSTOCHDaily.Annotations.Clear();
                    }

                    if (checkBoxHigh.Checked)
                        chartSTOCHDaily.Series["High"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["High"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("High") != null)
                            chartSTOCHDaily.Annotations.Clear();

                    }
                    if (checkBoxLow.Checked)
                        chartSTOCHDaily.Series["Low"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["Low"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("Low") != null)
                            chartSTOCHDaily.Annotations.Clear();

                    }

                    if (checkBoxClose.Checked)
                        chartSTOCHDaily.Series["Close"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["Close"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("Close") != null)
                            chartSTOCHDaily.Annotations.Clear();

                    }

                    if (checkBoxCandle.Checked)
                        chartSTOCHDaily.Series["OHLC"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["OHLC"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("OHLC") != null)
                            chartSTOCHDaily.Annotations.Clear();

                    }

                    if (checkBoxSlowK.Checked)
                        chartSTOCHDaily.Series["SlowK"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["SlowK"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("SlowK") != null)
                            chartSTOCHDaily.Annotations.Clear();
                    }

                    if (checkBoxSlowD.Checked)
                        chartSTOCHDaily.Series["SlowD"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["SlowD"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("SlowD") != null)
                            chartSTOCHDaily.Annotations.Clear();
                    }
                    if (checkBoxRSI.Checked)
                        chartSTOCHDaily.Series["RSI"].Enabled = true;
                    else
                    {
                        chartSTOCHDaily.Series["RSI"].Enabled = false;
                        if (chartSTOCHDaily.Annotations.FindByName("RSI") != null)
                            chartSTOCHDaily.Annotations.Clear();
                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = dailyData;
                        GridViewDaily.DataBind();

                        GridViewData.Visible = true;
                        GridViewData.DataSource = stochData;
                        GridViewData.DataBind();

                        GridViewRSI.Visible = true;
                        GridViewRSI.DataSource = rsiData;
                        GridViewRSI.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                        GridViewData.Visible = false;
                        GridViewRSI.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }
        protected void chartSTOCHDaily_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            int chartindex;
            //string legendName;

            //DataPoint p;
            //double lineHeight = -35;

            try
            {
                postBackValues = e.PostBackValue.Split(',');

                if (chartSTOCHDaily.Annotations.Count > 0)
                    chartSTOCHDaily.Annotations.Clear();

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
                if ((seriesName.Equals("SlowK")) || seriesName.Equals("SlowD"))
                {
                    HA.AxisX = chartSTOCHDaily.ChartAreas[1].AxisX;
                    HA.AxisY = chartSTOCHDaily.ChartAreas[1].AxisY;

                    VA.AxisX = chartSTOCHDaily.ChartAreas[1].AxisX;
                    VA.AxisY = chartSTOCHDaily.ChartAreas[1].AxisY;

                    ra.AxisX = chartSTOCHDaily.ChartAreas[1].AxisX;
                    ra.AxisY = chartSTOCHDaily.ChartAreas[1].AxisY;
                    chartindex = 1;
                }
                else if (seriesName.Equals("RSI"))
                {
                    HA.AxisX = chartSTOCHDaily.ChartAreas[2].AxisX;
                    HA.AxisY = chartSTOCHDaily.ChartAreas[2].AxisY;

                    VA.AxisX = chartSTOCHDaily.ChartAreas[2].AxisX;
                    VA.AxisY = chartSTOCHDaily.ChartAreas[2].AxisY;

                    ra.AxisX = chartSTOCHDaily.ChartAreas[2].AxisX;
                    ra.AxisY = chartSTOCHDaily.ChartAreas[2].AxisY;
                    chartindex = 2;
                }
                else
                {
                    HA.AxisX = chartSTOCHDaily.ChartAreas[0].AxisX;
                    HA.AxisY = chartSTOCHDaily.ChartAreas[0].AxisY;

                    VA.AxisX = chartSTOCHDaily.ChartAreas[0].AxisX;
                    VA.AxisY = chartSTOCHDaily.ChartAreas[0].AxisY;

                    ra.AxisX = chartSTOCHDaily.ChartAreas[0].AxisX;
                    ra.AxisY = chartSTOCHDaily.ChartAreas[0].AxisY;
                    chartindex = 0;
                }
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartSTOCHDaily.ChartAreas[chartindex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartSTOCHDaily.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartSTOCHDaily.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartSTOCHDaily.Annotations.Add(VA);

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

                chartSTOCHDaily.Annotations.Add(ra);
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
            GridViewData.DataSource = (DataTable)ViewState["FetchedDataSTOCH"];
            GridViewData.DataBind();
        }
        protected void GridViewRSI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRSI.PageIndex = e.NewPageIndex;
            GridViewRSI.DataSource = (DataTable)ViewState["FetchedDataRSI"];
            GridViewRSI.DataBind();
        }
    }
}