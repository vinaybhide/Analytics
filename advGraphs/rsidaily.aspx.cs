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
            this.Title = "Momentum Indicator";
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
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Momentum Indicator: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "RSI Vs Daily Price: " + Request.QueryString["script"].ToString();
                    
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartRSIDaily.Visible = true;
                        chartRSIDaily.Width = int.Parse(Master.panelWidth.Value);
                        chartRSIDaily.Height = int.Parse(Master.panelHeight.Value);
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
            Master.bulletedlistDesc.Items.Add("The relative strength index(RSI) is a momentum indicator used in technical analysis that measures the magnitude of recent price changes to evaluate overbought or oversold conditions in the price of a stock or other asset. The RSI is displayed as an oscillator (a line graph that moves between two extremes) and can have a reading from 0 to 100.");
            Master.bulletedlistDesc.Items.Add("RSI of 70 or above indicate that a security is becoming overbought or overvalued and may be primed for a trend reversal or corrective pullback in price.");
            Master.bulletedlistDesc.Items.Add("An RSI reading of 30 or below indicates an oversold or undervalued condition.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable dailyData = null;
            DataTable rsiData = null;
            DataTable tempData = null;
            string expression = "";
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

                        //dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        //if (dailyData == null)
                        //{
                            //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                            dailyData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        //}

                        ViewState["FetchedDataDaily"] = dailyData;

                        //rsiData = StockApi.getRSI(folderPath, scriptName, day_interval: interval, period: period, seriestype: series_type,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        rsiData = StockApi.getRSIalternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: series_type, 
                                                    outputsize: outputSize, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(), 
                                                    dailyTable:dailyData);
                        ViewState["FetchedDataRSI"] = rsiData;

                    }
                    else
                    {
                        ViewState["FetchedDataDaily"] = null;
                        dailyData = null;
                        ViewState["FetchedDataRSI"] = null;
                        rsiData = null;
                    }
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
                    GridViewDaily.DataBind();
                    GridViewData.DataSource = (DataTable)ViewState["FetchedDataRSI"];
                    GridViewData.DataBind();
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
                    chartRSIDaily.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High,Low,Open,Close", "");
                    chartRSIDaily.Series["RSI"].Points.DataBind(rsiData.AsEnumerable(), "Date", "RSI", "");

                    //chartRSIDaily.ChartAreas[1].AlignWithChartArea = chartRSIDaily.ChartAreas[0].Name;
                    chartRSIDaily.ChartAreas[1].AxisX.IsStartedFromZero = true;
                    chartRSIDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartRSIDaily.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartRSIDaily.Annotations.FindByName(item.Value) != null)
                                chartRSIDaily.Annotations.Clear();
                        }
                    }
                    Master.headingtext.Text = "Momentum Indicator: " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Momentum Indicator-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Momentum Indicator-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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
                    //0-OHLC,1-Date,2-High,3-Low,4-Open,5-Close
                    ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[4] + "\n" + "High:" + postBackValues[2] + "\n" +
                                "Low:" + postBackValues[3] + "\n" + "Close:" + postBackValues[5];
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
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
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
                //if (ViewState["FetchedDataDaily"] != null)
                //{
                    GridViewDaily.Visible = true;
                //    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
                //    GridViewDaily.DataBind();
                //}
                //if (ViewState["FetchedDataRSI"] != null)
                //{
                    GridViewData.Visible = true;
                //    GridViewData.DataSource = (DataTable)ViewState["FetchedDataRSI"];
                //    GridViewData.DataBind();
                //}
            }
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}