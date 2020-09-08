using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class dx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Trend direction";
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataDaily"] = null;
                    ViewState["FetchedDataDX"] = null;
                    ViewState["FetchedDataMINUSDI"] = null;
                    ViewState["FetchedDataPLUSDI"] = null;
                    ViewState["FetchedDataADX"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Trend Direction: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartDMIDaily.Visible = true;
                        chartDMIDaily.Width = int.Parse(Master.panelWidth.Value);
                        chartDMIDaily.Height = int.Parse(Master.panelHeight.Value);
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

            li = new ListItem("Directional Movement (DX)", "DX");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("-ve Directinal Movement", "MINUS_DI");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("+ve Directinal Movement", "PLUS_DI");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Avg Directinal Movement Index(ADX)", "ADX");
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
            Master.bulletedlistDesc.Items.Add("The average directional index(ADX) is a technical analysis indicator used by some traders to determine the strength of a trend.The trend can be either up or down, and this is shown by two accompanying indicators, the Negative Directional Indicator(-DI) and the Positive Directional Indicator(+DI)");
            Master.bulletedlistDesc.Items.Add("The indicator does this by comparing prior highs and lows and drawing two lines: a positive directional movement line(+DI) and a negative directional movement line(-DI). An optional third line, called directional movement(DX) shows the difference between the lines.");
            Master.bulletedlistDesc.Items.Add("When + DI is above - DI, there is more upward pressure than downward pressure in the price. If - DI is above + DI,  then there is more downward pressure in the price. This indicator may help traders assess the trend direction.");
            Master.bulletedlistDesc.Items.Add("Crossovers between the lines are also sometimes used as trade signals to buy or sell.");
            Master.bulletedlistDesc.Items.Add("The Average Directional Index(ADX) along with the Negative Directional Indicator(-DI) and the Positive Directional Indicator(+DI) are momentum indicators.The ADX helps investors determine trend strength while -DI and + DI help determine trend direction.");
            Master.bulletedlistDesc.Items.Add("The ADX identifies a strong trend when the ADX is over 25 and a weak trend when the ADX is below 20.");
            Master.bulletedlistDesc.Items.Add("Crossovers of the -DI and + DI lines can be used to generate trade signals. For example, if the + DI line crosses above the - DI line and the ADX is above 20, or ideally above 25, then that is a potential signal to buy.");
            Master.bulletedlistDesc.Items.Add("If the - DI crosses above the + DI, and ADX is above 20 or 25, then that is an opportunity to enter a potential short trade.");
            Master.bulletedlistDesc.Items.Add("Crosses can also be used to exit current trades. For example, if long, exit when the - DI crosses above the + DI.");
            Master.bulletedlistDesc.Items.Add("When ADX is below 20 the indicator is signaling that the price is trendless, and therefore may not be an ideal time to enter a trade.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable dailyData = null;
            DataTable dxData = null;
            DataTable minusdiData = null;
            DataTable plusdiData = null;
            DataTable adxData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize;
            string interval_dx;
            string period_dx;
            string interval_minusdi;
            string period_minusdi;
            string interval_plusdi;
            string period_plusdi;
            string interval_adx;
            string period_adx;

            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataDaily"] == null) || (ViewState["FetchedDataDX"] == null) || (ViewState["FetchedDataMINUSDI"] == null)
                        || (ViewState["FetchedDataPLUSDI"] == null) || (ViewState["FetchedDataADX"] == null))
                    || ((((DataTable)ViewState["FetchedDataDaily"]).Rows.Count == 0) || (ViewState["FetchedDataDX"] == null)
                        || (((DataTable)ViewState["FetchedDataMINUSDI"]).Rows.Count == 0)
                        || (((DataTable)ViewState["FetchedDataPLUSDI"]).Rows.Count == 0))

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
                    if ((Request.QueryString["size"] != null) && (Request.QueryString["intervaldx"] != null) && (Request.QueryString["perioddx"] != null)
                        && (Request.QueryString["intervalminusdi"] != null) && (Request.QueryString["periodminusdi"] != null)
                        && (Request.QueryString["intervalplusdi"] != null) && (Request.QueryString["periodplusdi"] != null)
                        && (Request.QueryString["intervaladx"] != null) && (Request.QueryString["intervaladx"] != null)
                        )
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval_dx = Request.QueryString["intervaldx"];
                        period_dx = Request.QueryString["perioddx"];
                        interval_minusdi = Request.QueryString["intervalminusdi"];
                        period_minusdi = Request.QueryString["periodminusdi"];
                        interval_plusdi = Request.QueryString["intervalplusdi"];
                        period_plusdi = Request.QueryString["periodplusdi"];
                        interval_adx = Request.QueryString["intervaladx"];
                        period_adx = Request.QueryString["periodadx"];

                        dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        if (dailyData == null)
                        {
                            //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                            dailyData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        }

                        ViewState["FetchedDataDaily"] = dailyData;

                        dxData = StockApi.getDX(folderPath, scriptName, day_interval: interval_dx, period: period_dx,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataDX"] = dxData;

                        minusdiData = StockApi.getMinusDI(folderPath, scriptName, day_interval: interval_minusdi, period: period_minusdi,
                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataMINUSDI"] = minusdiData;

                        plusdiData = StockApi.getPlusDI(folderPath, scriptName, day_interval: interval_plusdi, period: period_plusdi,
                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataPLUSDI"] = plusdiData;

                        adxData = StockApi.getADX(folderPath, scriptName, day_interval: interval_adx, period: period_adx,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataADX"] = adxData;
                    }
                    else
                    {
                        ViewState["FetchedDataDaily"] = null;
                        dailyData = null;
                        ViewState["FetchedDataDX"] = null;
                        dxData = null;
                        ViewState["FetchedDataMINUSDI"] = null;
                        minusdiData = null;
                        ViewState["FetchedDataPLUSDI"] = null;
                        plusdiData = null;
                        ViewState["FetchedDataADX"] = null;
                        adxData = null;
                    }
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
                    GridViewDaily.DataBind();
                    GridViewDX.DataSource = (DataTable)ViewState["FetchedDataDX"];
                    GridViewDX.DataBind();
                    GridViewMINUSDI.DataSource = (DataTable)ViewState["FetchedDataMINUSDI"];
                    GridViewMINUSDI.DataBind();
                    GridViewPLUSDI.DataSource = (DataTable)ViewState["FetchedDataPLUSDI"];
                    GridViewPLUSDI.DataBind();
                    GridViewADX.DataSource = (DataTable)ViewState["FetchedDataADX"];
                    GridViewADX.DataBind();
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

                    tempData = (DataTable)ViewState["FetchedDataDX"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        dxData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataMINUSDI"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        minusdiData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataPLUSDI"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        plusdiData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataADX"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        adxData = filteredRows.CopyToDataTable();
                }
                else
                {
                    dailyData = (DataTable)ViewState["FetchedDataDaily"];
                    dxData = (DataTable)ViewState["FetchedDataDX"];
                    minusdiData = (DataTable)ViewState["FetchedDataMINUSDI"];
                    plusdiData = (DataTable)ViewState["FetchedDataPLUSDI"];
                    adxData = (DataTable)ViewState["FetchedDataADX"];
                }
                //}

                if ((dailyData != null) && (dxData != null) && (minusdiData != null) && (plusdiData != null) && (adxData != null))
                {
                    chartDMIDaily.Series["Open"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open", "");
                    chartDMIDaily.Series["High"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High", "");
                    chartDMIDaily.Series["Low"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Low", "");
                    chartDMIDaily.Series["Close"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Close", "");
                    chartDMIDaily.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartDMIDaily.Series["DX"].Points.DataBind(dxData.AsEnumerable(), "Date", "DX", "");
                    chartDMIDaily.Series["MINUS_DI"].Points.DataBind(minusdiData.AsEnumerable(), "Date", "MINUS_DI", "");
                    chartDMIDaily.Series["PLUS_DI"].Points.DataBind(plusdiData.AsEnumerable(), "Date", "PLUS_DI", "");
                    chartDMIDaily.Series["ADX"].Points.DataBind(adxData.AsEnumerable(), "Date", "ADX", "");

                    chartDMIDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartDMIDaily.ChartAreas[1].AxisX.IsStartedFromZero = true;

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartDMIDaily.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartDMIDaily.Annotations.FindByName(item.Value) != null)
                                chartDMIDaily.Annotations.Clear();
                        }
                    }
                    Master.headingtext.Text = "Trend Direction: " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Trend direction-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Trend direction-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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
        protected void chartDMIDaily_Click(object sender, ImageMapEventArgs e)
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

                if (chartDMIDaily.Annotations.Count > 0)
                    chartDMIDaily.Annotations.Clear();

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
                if ((seriesName.Equals("DX")) || seriesName.Equals("MINUS_DI") || seriesName.Equals("PLUS_DI"))
                {
                    HA.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    HA.AxisY = chartDMIDaily.ChartAreas[1].AxisY;

                    VA.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    VA.AxisY = chartDMIDaily.ChartAreas[1].AxisY;

                    ra.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    ra.AxisY = chartDMIDaily.ChartAreas[1].AxisY;
                    chartindex = 1;
                }
                else if (seriesName.Equals("ADX"))
                {
                    HA.AxisX = chartDMIDaily.ChartAreas[1].AxisX2;
                    HA.AxisY = chartDMIDaily.ChartAreas[1].AxisY2;

                    VA.AxisX = chartDMIDaily.ChartAreas[1].AxisX2;
                    VA.AxisY = chartDMIDaily.ChartAreas[1].AxisY2;

                    ra.AxisX = chartDMIDaily.ChartAreas[1].AxisX2;
                    ra.AxisY = chartDMIDaily.ChartAreas[1].AxisY2;
                    chartindex = 1;
                }
                else
                {
                    HA.AxisX = chartDMIDaily.ChartAreas[0].AxisX;
                    HA.AxisY = chartDMIDaily.ChartAreas[0].AxisY;

                    VA.AxisX = chartDMIDaily.ChartAreas[0].AxisX;
                    VA.AxisY = chartDMIDaily.ChartAreas[0].AxisY;

                    ra.AxisX = chartDMIDaily.ChartAreas[0].AxisX;
                    ra.AxisY = chartDMIDaily.ChartAreas[0].AxisY;
                    chartindex = 0;
                }
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartDMIDaily.ChartAreas[chartindex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartDMIDaily.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartDMIDaily.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartDMIDaily.Annotations.Add(VA);

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

                chartDMIDaily.Annotations.Add(ra);
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

        protected void GridViewDX_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDX.PageIndex = e.NewPageIndex;
            GridViewDX.DataSource = (DataTable)ViewState["FetchedDataDX"];
            GridViewDX.DataBind();
        }
        protected void GridViewMINUSDI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMINUSDI.PageIndex = e.NewPageIndex;
            GridViewMINUSDI.DataSource = (DataTable)ViewState["FetchedDataMINUSDI"];
            GridViewMINUSDI.DataBind();
        }
        protected void GridViewPLUSDI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPLUSDI.PageIndex = e.NewPageIndex;
            GridViewPLUSDI.DataSource = (DataTable)ViewState["FetchedDataPLUSDI"];
            GridViewPLUSDI.DataBind();
        }
        protected void GridViewADX_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewADX.PageIndex = e.NewPageIndex;
            GridViewADX.DataSource = (DataTable)ViewState["FetchedDataADX"];
            GridViewADX.DataBind();
        }

        void buttonShowGrid_Click()
        {
            if ((GridViewDaily.Visible) || (GridViewDX.Visible) || (GridViewMINUSDI.Visible) || (GridViewPLUSDI.Visible) || (GridViewADX.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewDX.Visible = false;
                GridViewMINUSDI.Visible = false;
                GridViewPLUSDI.Visible = false;
                GridViewADX.Visible = false;
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
                //if (ViewState["FetchedDataDX"] != null)
                //{
                    GridViewDX.Visible = true;
                 //   GridViewDX.DataSource = (DataTable)ViewState["FetchedDataDX"];
                 //   GridViewDX.DataBind();
                //}
                //if (ViewState["FetchedDataMINUSDI"] != null)
                //{
                    GridViewMINUSDI.Visible = true;
                //    GridViewMINUSDI.DataSource = (DataTable)ViewState["FetchedDataMINUSDI"];
                //    GridViewMINUSDI.DataBind();
                //}
                //if (ViewState["FetchedDataPLUSDI"] != null)
                //{
                    GridViewPLUSDI.Visible = true;
                //    GridViewPLUSDI.DataSource = (DataTable)ViewState["FetchedDataPLUSDI"];
                //    GridViewPLUSDI.DataBind();
                //}
                //if (ViewState["FetchedDataADX"] != null)
                //{
                    GridViewADX.Visible = true;
                //    GridViewADX.DataSource = (DataTable)ViewState["FetchedDataADX"];
                //    GridViewADX.DataBind();
                //}
            }
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}