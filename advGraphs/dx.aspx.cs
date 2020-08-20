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
    public partial class dmi : System.Web.UI.Page
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
                    ViewState["FetchedDataDX"] = null;
                    ViewState["FetchedDataMINUSDI"] = null;
                    ViewState["FetchedDataPLUSDI"] = null;
                    ViewState["FetchedDataADX"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    headingtext.Text = "Trend Direction: " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartDMIDaily.Visible = true;
                        chartDMIDaily.Width = int.Parse(panelWidth.Value);
                        chartDMIDaily.Height = int.Parse(panelHeight.Value);
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
            DataTable dxData = null;
            DataTable minusdiData = null;
            DataTable plusdiData = null;
            DataTable adxData = null;
            DataTable tempData = null;
            string expression;
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

                    if (checkBoxOpen.Checked)
                        chartDMIDaily.Series["Open"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["Open"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("Open") != null)
                            chartDMIDaily.Annotations.Clear();
                    }

                    if (checkBoxHigh.Checked)
                        chartDMIDaily.Series["High"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["High"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("High") != null)
                            chartDMIDaily.Annotations.Clear();

                    }
                    if (checkBoxLow.Checked)
                        chartDMIDaily.Series["Low"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["Low"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("Low") != null)
                            chartDMIDaily.Annotations.Clear();

                    }

                    if (checkBoxClose.Checked)
                        chartDMIDaily.Series["Close"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["Close"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("Close") != null)
                            chartDMIDaily.Annotations.Clear();

                    }

                    if (checkBoxCandle.Checked)
                        chartDMIDaily.Series["OHLC"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["OHLC"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("OHLC") != null)
                            chartDMIDaily.Annotations.Clear();

                    }

                    if (checkBoxDX.Checked)
                        chartDMIDaily.Series["DX"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["DX"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("DX") != null)
                            chartDMIDaily.Annotations.Clear();
                    }

                    if (checkBoxMINUS_DI.Checked)
                        chartDMIDaily.Series["MINUS_DI"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["MINUS_DI"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("MINUS_DI") != null)
                            chartDMIDaily.Annotations.Clear();
                    }
                    if (checkBoxPLUS_DI.Checked)
                        chartDMIDaily.Series["PLUS_DI"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["PLUS_DI"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("PLUS_DI") != null)
                            chartDMIDaily.Annotations.Clear();
                    }
                    if (checkBoxADX.Checked)
                        chartDMIDaily.Series["ADX"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["ADX"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("ADX") != null)
                            chartDMIDaily.Annotations.Clear();
                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = dailyData;
                        GridViewDaily.DataBind();

                        GridViewDX.Visible = true;
                        GridViewDX.DataSource = dxData;
                        GridViewDX.DataBind();

                        GridViewMINUSDI.Visible = true;
                        GridViewMINUSDI.DataSource = minusdiData;
                        GridViewMINUSDI.DataBind();

                        GridViewPLUSDI.Visible = true;
                        GridViewPLUSDI.DataSource = plusdiData;
                        GridViewPLUSDI.DataBind();

                        GridViewADX.Visible = true;
                        GridViewADX.DataSource = adxData;
                        GridViewADX.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                        GridViewDX.Visible = false;
                        GridViewMINUSDI.Visible = false;
                        GridViewPLUSDI.Visible = false;
                        GridViewADX.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
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

    }
}