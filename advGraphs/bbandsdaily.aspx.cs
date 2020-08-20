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
    public partial class bbandsdaily : System.Web.UI.Page
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
                    ViewState["FetchedDataBBands"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    //headingtext.InnerText = "MACD Vs EMA Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();
                    headingtext.Text = "Bollinger Bands Vs Daily(OHLC)-" + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartBBandsDaily.Visible = true;
                        chartBBandsDaily.Width = int.Parse(panelWidth.Value);
                        chartBBandsDaily.Height = int.Parse(panelHeight.Value);
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
            DataTable bbandsData = null;
            DataTable tempData = null;
            string expression;
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

                        ohlcData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataOHLC"] = ohlcData;

                        bbandsData = StockApi.getBbands(folderPath, scriptName, day_interval: interval, period: period, seriestype: seriestype, 
                                                    nbdevup: nbdevup, nbdevdn: nbdevdn,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataBBands"] = bbandsData;
                    }
                    else
                    {
                        ViewState["FetchedDataOHLC"] = null;
                        ohlcData = null;
                        ViewState["FetchedDataBBands"] = null;
                        bbandsData = null;
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
                    chartBBandsDaily.Series["OHLC"].Points.DataBind(ohlcData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartBBandsDaily.Series["LowerBand"].Points.DataBind(bbandsData.AsEnumerable(), "Date", "Real Lower Band", "");
                    chartBBandsDaily.Series["MiddleBand"].Points.DataBind(bbandsData.AsEnumerable(), "Date", "Real Middle Band", "");
                    chartBBandsDaily.Series["UpperBand"].Points.DataBind(bbandsData.AsEnumerable(), "Date", "Real Upper Band", "");

                    chartBBandsDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartBBandsDaily.ChartAreas[0].AxisX2.IsStartedFromZero = true;

                    if (checkBoxOpen.Checked)
                        chartBBandsDaily.Series["Open"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["Open"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("Open") != null)
                            chartBBandsDaily.Annotations.Clear();
                    }

                    if (checkBoxHigh.Checked)
                        chartBBandsDaily.Series["High"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["High"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("High") != null)
                            chartBBandsDaily.Annotations.Clear();

                    }
                    if (checkBoxLow.Checked)
                        chartBBandsDaily.Series["Low"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["Low"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("Low") != null)
                            chartBBandsDaily.Annotations.Clear();

                    }

                    if (checkBoxClose.Checked)
                        chartBBandsDaily.Series["Close"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["Close"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("Close") != null)
                            chartBBandsDaily.Annotations.Clear();

                    }

                    if (checkBoxCandle.Checked)
                        chartBBandsDaily.Series["OHLC"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["OHLC"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("OHLC") != null)
                            chartBBandsDaily.Annotations.Clear();
                    }

                    if (checkBoxLowerBand.Checked)
                        //showVolumeGraph(scriptData);
                        chartBBandsDaily.Series["LowerBand"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["LowerBand"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("Lower Band") != null)
                            chartBBandsDaily.Annotations.Clear();

                    }
                    if (checkBoxMiddleBand.Checked)
                        //showVolumeGraph(scriptData);
                        chartBBandsDaily.Series["MiddleBand"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["MiddleBand"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("Middle Band") != null)
                            chartBBandsDaily.Annotations.Clear();

                    }
                    if (checkBoxUpperBand.Checked)
                        //showVolumeGraph(scriptData);
                        chartBBandsDaily.Series["UpperBand"].Enabled = true;
                    else
                    {
                        chartBBandsDaily.Series["UpperBand"].Enabled = false;
                        if (chartBBandsDaily.Annotations.FindByName("Upper Band") != null)
                            chartBBandsDaily.Annotations.Clear();
                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = ohlcData;
                        GridViewDaily.DataBind();

                        GridViewBBands.Visible = true;
                        GridViewBBands.DataSource = bbandsData;
                        GridViewBBands.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                        GridViewBBands.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
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
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
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

    }
}