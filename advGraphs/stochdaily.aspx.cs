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
            if (Session["EmailId"] != null)
            {
                Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
                Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
                Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
                this.Title = "Buy & Sell Indicator";
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
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Buy & Sell Indicator: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "Stochastics Vs Daily Price Vs RSI: " + Request.QueryString["script"].ToString();
                    
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartSTOCHDaily.Visible = true;
                        chartSTOCHDaily.Width = int.Parse(Master.panelWidth.Value);
                        chartSTOCHDaily.Height = int.Parse(Master.panelHeight.Value);
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

            li = new ListItem("Slow K", "SlowK");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Slow D", "SlowD");
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

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable dailyData = null;
            DataTable stochData = null;
            DataTable rsiData = null;
            DataTable tempData = null;
            string expression = "";
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
                        if (dailyData == null)
                        {
                            //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                            dailyData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        }

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
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
                    GridViewDaily.DataBind();
                    GridViewData.DataSource = (DataTable)ViewState["FetchedDataSTOCH"];
                    GridViewData.DataBind();
                    GridViewRSI.DataSource = (DataTable)ViewState["FetchedDataRSI"];
                    GridViewRSI.DataBind();
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

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartSTOCHDaily.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartSTOCHDaily.Annotations.FindByName(item.Value) != null)
                                chartSTOCHDaily.Annotations.Clear();
                        }
                    }
                    Master.headingtext.Text = "Buy & Sell Indicator: " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Buy & Sell Indicatory-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Buy & Sell Indicatory-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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
            GridViewData.DataSource = (DataTable)ViewState["FetchedDataSTOCH"];
            GridViewData.DataBind();
        }
        protected void GridViewRSI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRSI.PageIndex = e.NewPageIndex;
            GridViewRSI.DataSource = (DataTable)ViewState["FetchedDataRSI"];
            GridViewRSI.DataBind();
        }

        void buttonShowGrid_Click()
        {
            if ((GridViewDaily.Visible) || (GridViewData.Visible) || (GridViewRSI.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewData.Visible = false;
                GridViewRSI.Visible = false;
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
               // }
                //if (ViewState["FetchedDataSTOCH"] != null)
                //{
                    GridViewData.Visible = true;
                //    GridViewData.DataSource = (DataTable)ViewState["FetchedDataSTOCH"];
                //    GridViewData.DataBind();
                //}
                //if (ViewState["FetchedDataRSI"] != null)
                //{
                    GridViewRSI.Visible = true;
                //    GridViewRSI.DataSource = (DataTable)ViewState["FetchedDataRSI"];
                //    GridViewRSI.DataBind();
                //}
            }
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}