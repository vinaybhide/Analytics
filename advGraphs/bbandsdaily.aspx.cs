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
    public partial class bbandsdaily : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Gauge Trends: " + Request.QueryString["script"].ToString();

            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataOHLC"] = null;
                    ViewState["FetchedDataBBands"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    if (!IsPostBack)
                    {
                        //Master.headingtext.Text = "Gauge Trends:Bollinger Bands Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartBBandsDaily.Visible = true;
                        chartBBandsDaily.Width = int.Parse(Master.panelWidth.Value);
                        chartBBandsDaily.Height = int.Parse(Master.panelHeight.Value);
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

            li = new ListItem("Lower Band", "LowerBand");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Middle Band", "MiddleBand");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Upper Band", "UpperBand");
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
            Master.bulletedlistDesc.Items.Add("A Bollinger Band® is a technical analysis tool defined by a set of trendlines plotted two standard deviations(positively and negatively) away from a simple moving average(SMA) of a security's price, but which can be adjusted to user preferences.");
            Master.bulletedlistDesc.Items.Add("It gives investors a higher probability of properly identifying when an asset is oversold or overbought.");
            Master.bulletedlistDesc.Items.Add("There are three lines that compose Bollinger Bands: A simple moving average(middle band) and an upper and lower band.");
            Master.bulletedlistDesc.Items.Add("Closer the prices move to the upper band, the more overbought the market, and the closer the prices move to the lower band, the more oversold the market.");
            Master.bulletedlistDesc.Items.Add("The squeeze: When the bands come close together, constricting the moving average, it is called a squeeze.");
            Master.bulletedlistDesc.Items.Add("A squeeze signals a period of low volatility and is considered by traders to be a potential sign of future increased volatility and possible trading opportunities.Conversely, the wider apart the bands move, the more likely the chance of a decrease in volatility and the greater the possibility of exiting a trade.");
            Master.bulletedlistDesc.Items.Add("Breakout: Approximately 90 % of price action occurs between the two bands.Any breakout above or below the bands is a major event.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable ohlcData = null;
            DataTable bbandsData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize;
            string interval;
            string period;
            string seriestype;
            string nbdevup;
            string nbdevdn;
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataOHLC"] == null) || (ViewState["FetchedDataBBands"] == null))
                ||
                ((((DataTable)ViewState["FetchedDataOHLC"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataBBands"]).Rows.Count == 0))
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
                    if ((Request.QueryString["size"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null) && (Request.QueryString["seriestype"] != null) &&
                        (Request.QueryString["nbdevup"] != null) && (Request.QueryString["nbdevdn"] != null))
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval = Request.QueryString["interval"].ToString();
                        seriestype = Request.QueryString["seriestype"].ToString();
                        period = Request.QueryString["period"].ToString();
                        nbdevup = Request.QueryString["nbdevup"].ToString();
                        nbdevdn = Request.QueryString["nbdevdn"].ToString();

                        //ohlcData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        //if(ohlcData == null)
                        //{
                            //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                            ohlcData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        //}
                        ViewState["FetchedDataOHLC"] = ohlcData;

                        //bbandsData = StockApi.getBbands(folderPath, scriptName, day_interval: interval, period: period, seriestype: seriestype, 
                        //                            nbdevup: nbdevup, nbdevdn: nbdevdn,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        bbandsData = StockApi.getBbandsAlternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: seriestype,
                                                    nbdevup: nbdevup, nbdevdn: nbdevdn, outputsize:outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(), dailyDataTable:ohlcData);
                        ViewState["FetchedDataBBands"] = bbandsData;
                    }
                    else
                    {
                        ViewState["FetchedDataOHLC"] = null;
                        ohlcData = null;
                        ViewState["FetchedDataBBands"] = null;
                        bbandsData = null;
                    }
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataOHLC"];
                    GridViewDaily.DataBind();

                    GridViewBBands.DataSource = (DataTable)ViewState["FetchedDataBBands"];
                    GridViewBBands.DataBind();
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

                    tempData = (DataTable)ViewState["FetchedDataBBands"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        bbandsData = filteredRows.CopyToDataTable();
                }
                else
                {
                    ohlcData = (DataTable)ViewState["FetchedDataOHLC"];
                    bbandsData = (DataTable)ViewState["FetchedDataBBands"];
                }
                //}

                if ((ohlcData != null) && (bbandsData != null))
                {
                    chartBBandsDaily.Series["Open"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Open", "");
                    chartBBandsDaily.Series["High"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "High", "");
                    chartBBandsDaily.Series["Low"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Low", "");
                    chartBBandsDaily.Series["Close"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Close", "");
                    chartBBandsDaily.Series["OHLC"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "High,Low,Open,Close", "");
                    chartBBandsDaily.Series["LowerBand"].Points.DataBind(bbandsData.AsEnumerable(), "Date", "Real Lower Band", "");
                    chartBBandsDaily.Series["MiddleBand"].Points.DataBind(bbandsData.AsEnumerable(), "Date", "Real Middle Band", "");
                    chartBBandsDaily.Series["UpperBand"].Points.DataBind(bbandsData.AsEnumerable(), "Date", "Real Upper Band", "");

                    chartBBandsDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartBBandsDaily.ChartAreas[0].AxisX2.IsStartedFromZero = true;

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartBBandsDaily.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartBBandsDaily.Annotations.FindByName(item.Value) != null)
                                chartBBandsDaily.Annotations.Clear();
                        }
                    }
                    //Master.headingtext.Text = "Gauge Trends:Bollinger Bands Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Gauge Trends:Bollinger Bands Vs Daily(OHLC)-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Gauge Trends:Bollinger Bands Vs Daily(OHLC)-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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

         protected void chartBBandsDaily_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            //DataPoint p;
            //double lineHeight = -35;

            try
            {
                //if (e.PostBackValue.Contains(','))
                //{
                postBackValues = e.PostBackValue.Split(',');

                if (chartBBandsDaily.Annotations.Count > 0)
                    chartBBandsDaily.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("Lower Band") || seriesName.Equals("Middle Band") || seriesName.Equals("Upper Band"))
                {
                    HA.AxisX = chartBBandsDaily.ChartAreas[0].AxisX2;
                    HA.AxisY = chartBBandsDaily.ChartAreas[0].AxisY2;

                    VA.AxisX = chartBBandsDaily.ChartAreas[0].AxisX2;
                    VA.AxisY = chartBBandsDaily.ChartAreas[0].AxisY2;

                    ra.AxisX = chartBBandsDaily.ChartAreas[0].AxisX2;
                    ra.AxisY = chartBBandsDaily.ChartAreas[0].AxisY2;
                }
                else
                {
                    HA.AxisX = chartBBandsDaily.ChartAreas[0].AxisX;
                    HA.AxisY = chartBBandsDaily.ChartAreas[0].AxisY;

                    VA.AxisX = chartBBandsDaily.ChartAreas[0].AxisX;
                    VA.AxisY = chartBBandsDaily.ChartAreas[0].AxisY;

                    ra.AxisX = chartBBandsDaily.ChartAreas[0].AxisX;
                    ra.AxisY = chartBBandsDaily.ChartAreas[0].AxisY;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartBBandsDaily.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartBBandsDaily.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartBBandsDaily.ChartAreas[0].Name;

                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartBBandsDaily.Annotations.Add(VA);

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

                chartBBandsDaily.Annotations.Add(ra);
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
        protected void GridViewBBands_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewBBands.PageIndex = e.NewPageIndex;
            GridViewBBands.DataSource = (DataTable)ViewState["FetchedDataBBands"];
            GridViewBBands.DataBind();
        }

        void buttonShowGrid_Click()
        {
            if ( (GridViewDaily.Visible) || (GridViewBBands.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewBBands.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                //if (ViewState["FetchedDataOHLC"] != null)
                //{
                    GridViewDaily.Visible = true;
                 //   GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataOHLC"];
                  //  GridViewDaily.DataBind();
                //}
                //if (ViewState["FetchedDataBBands"] != null)
                //{
                    GridViewBBands.Visible = true;
                 //   GridViewBBands.DataSource = (DataTable)ViewState["FetchedDataBBands"];
                  //  GridViewBBands.DataBind();
                //}
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
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }

    }
}