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
    public partial class dmi1 : System.Web.UI.Page
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
                    ViewState["FetchedDataMINUSDM"] = null;
                    ViewState["FetchedDataPLUSDM"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    headingtext.Text = "Price Direction & strength: " + Request.QueryString["script"].ToString();
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
            DataTable minusdmData = null;
            DataTable plusdmData = null;
            DataTable tempData = null;
            string expression;
            string outputSize;
            string interval_minusdm;
            string period_minusdm;
            string interval_plusdm;
            string period_plusdm;

            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataDaily"] == null) || (ViewState["FetchedDataMINUSDM"] == null)
                        || (ViewState["FetchedDataPLUSDM"] == null))
                    || ((((DataTable)ViewState["FetchedDataDaily"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataMINUSDM"]).Rows.Count == 0) ||
                     (((DataTable)ViewState["FetchedDataPLUSDM"]).Rows.Count == 0))
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
                    if ((Request.QueryString["size"] != null)
                        && (Request.QueryString["intervalminusdm"] != null) && (Request.QueryString["periodminusdm"] != null)
                        && (Request.QueryString["intervalplusdm"] != null) && (Request.QueryString["periodplusdm"] != null)
                        )
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval_minusdm = Request.QueryString["intervalminusdm"];
                        period_minusdm = Request.QueryString["periodminusdm"];
                        interval_plusdm = Request.QueryString["intervalplusdm"];
                        period_plusdm = Request.QueryString["periodplusdm"];

                        dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataDaily"] = dailyData;

                        minusdmData = StockApi.getMinusDM(folderPath, scriptName, day_interval: interval_minusdm, period: period_minusdm,
                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataMINUSDM"] = minusdmData;

                        plusdmData = StockApi.getPlusDM(folderPath, scriptName, day_interval: interval_plusdm, period: period_plusdm,
                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataPLUSDM"] = plusdmData;

                    }
                    else
                    {
                        ViewState["FetchedDataDaily"] = null;
                        dailyData = null;
                        ViewState["FetchedDataMINUSDM"] = null;
                        minusdmData = null;
                        ViewState["FetchedDataPLUSDM"] = null;
                        plusdmData = null;
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

                    tempData = (DataTable)ViewState["FetchedDataMINUSDM"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        minusdmData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataPLUSDM"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        plusdmData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;
                }
                else
                {
                    dailyData = (DataTable)ViewState["FetchedDataDaily"];
                    minusdmData = (DataTable)ViewState["FetchedDataMINUSDM"];
                    plusdmData = (DataTable)ViewState["FetchedDataPLUSDM"];
                }
                //}

                if ((dailyData != null) && (minusdmData != null) && (plusdmData != null))
                {
                    chartDMIDaily.Series["Open"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open", "");
                    chartDMIDaily.Series["High"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High", "");
                    chartDMIDaily.Series["Low"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Low", "");
                    chartDMIDaily.Series["Close"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Close", "");
                    chartDMIDaily.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartDMIDaily.Series["MINUS_DM"].Points.DataBind(minusdmData.AsEnumerable(), "Date", "MINUS_DM", "");
                    chartDMIDaily.Series["PLUS_DM"].Points.DataBind(plusdmData.AsEnumerable(), "Date", "PLUS_DM", "");

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

                    if (checkBoxMINUS_DM.Checked)
                        chartDMIDaily.Series["MINUS_DM"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["MINUS_DM"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("MINUS_DM") != null)
                            chartDMIDaily.Annotations.Clear();
                    }
                    if (checkBoxPLUS_DM.Checked)
                        chartDMIDaily.Series["PLUS_DM"].Enabled = true;
                    else
                    {
                        chartDMIDaily.Series["PLUS_DM"].Enabled = false;
                        if (chartDMIDaily.Annotations.FindByName("PLUS_DM") != null)
                            chartDMIDaily.Annotations.Clear();
                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = dailyData;
                        GridViewDaily.DataBind();

                        GridViewMINUSDM.Visible = true;
                        GridViewMINUSDM.DataSource = minusdmData;
                        GridViewMINUSDM.DataBind();

                        GridViewPLUSDM.Visible = true;
                        GridViewPLUSDM.DataSource = plusdmData;
                        GridViewPLUSDM.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                        GridViewMINUSDM.Visible = false;
                        GridViewPLUSDM.Visible = false;
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
                if (seriesName.Equals("MINUS_DM") || seriesName.Equals("PLUS_DM"))
                {
                    HA.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    HA.AxisY = chartDMIDaily.ChartAreas[1].AxisY;

                    VA.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    VA.AxisY = chartDMIDaily.ChartAreas[1].AxisY;

                    ra.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    ra.AxisY = chartDMIDaily.ChartAreas[1].AxisY;
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

        protected void GridViewMINUSDM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMINUSDM.PageIndex = e.NewPageIndex;
            GridViewMINUSDM.DataSource = (DataTable)ViewState["FetchedDataMINUSDM"];
            GridViewMINUSDM.DataBind();
        }
        protected void GridViewPLUSDM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPLUSDM.PageIndex = e.NewPageIndex;
            GridViewPLUSDM.DataSource = (DataTable)ViewState["FetchedDataPLUSDM"];
            GridViewPLUSDM.DataBind();
        }

    }
}