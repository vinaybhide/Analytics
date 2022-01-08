using Analytics.Graphs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics.Graphs
{
    public partial class rsiMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["RSIData"] = null;
                    ViewState["SchemeName"] = null;
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;

                    if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null) &&
                        (Request.QueryString["fromdate"] != null) && (Request.QueryString["todate"] != null) && (Request.QueryString["period"] != null))
                    {
                        Master.textboxFromDate.Text = Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");  //DateTime.Today.AddDays(-90).ToString("dd-MM-yyyy");
                        ViewState["FromDate"] = Request.QueryString["fromdate"].ToString();

                        Master.textboxToDate.Text = Convert.ToDateTime(Request.QueryString["todate"].ToString()).ToString("yyyy-MM-dd"); // DateTime.Today.ToString("dd-MM-yyyy");
                        ViewState["ToDate"] = Request.QueryString["todate"].ToString();

                        fillLinesCheckBoxes();
                        fillDesc();

                        //DataManager dataMgr = new DataManager();
                        //DataTable tempTable = dataMgr.getRSIDataTableFromDailyNAV(Int32.Parse(Request.QueryString["schemecode"]), fromDate: Request.QueryString["fromdate"].ToString(),
                        //                                                        toDate: Request.QueryString["todate"].ToString());
                        //if ((tempTable != null) && (tempTable.Rows.Count > 0))
                        //{
                        //    ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
                        //    this.Title = "RSI: " + ViewState["SchemeName"].ToString();

                        //    fillLinesCheckBoxes();
                        //    fillDesc();

                        //    ViewState["RSIData"] = tempTable;
                        //}
                    }
                }
                
                if (Request.QueryString["schemecode"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph();

                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartRSI.Visible = true;
                        chartRSI.Width = int.Parse(Master.panelWidth.Value);
                        chartRSI.Height = int.Parse(Master.panelHeight.Value);
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
            Master.checkboxlistLines.Visible = true;
            ListItem li;

            li = new ListItem("RSI", "RSI");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Daily", "Daily");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("You can change the date range to see different range of NAV for selected Scheme. Click the Reset Graph button to refresh the graph.");
            Master.bulletedlistDesc.Items.Add("The relative strength index(RSI) is a momentum indicator used in technical analysis that measures the magnitude of recent price changes to evaluate overbought or oversold conditions in the price of a stock or other asset.");
            Master.bulletedlistDesc.Items.Add("Traditional interpretation and usage of the RSI are that values of 70 or above indicate that a security is becoming overbought or overvalued and may be primed for a trend reversal or corrective pullback in price.An RSI reading of 30 or below indicates an oversold or undervalued condition.");
        }

        public void FillData()
        {
            if ((ViewState["RSIData"] == null) || (((DataTable)ViewState["RSIData"]).Rows.Count == 0))
            {
                DataManager dataMgr = new DataManager();
                DataTable tempTable = dataMgr.getRSIDataTableFromDailyNAV(Int32.Parse(Request.QueryString["schemecode"]),
                    fromDate: ((ViewState["FromDate"] == null) || (ViewState["FromDate"].ToString().Equals(""))) ? null : ViewState["FromDate"].ToString(),
                    toDate: ((ViewState["ToDate"] == null) || (ViewState["ToDate"].ToString().Equals(""))) ? null : ViewState["ToDate"].ToString(),
                    period: ((Request.QueryString["period"] == null) || (Request.QueryString["period"].ToString().Equals(""))) ? null : Request.QueryString["period"].ToString());
                if ((tempTable != null) && (tempTable.Rows.Count > 0))
                {
                    ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
                    this.Title = "RSI: " + ViewState["SchemeName"].ToString();

                    ViewState["RSIData"] = tempTable;
                }
            }
        }
        public void ShowGraph()
        {
            DataTable dailyrsiTable = null;
            DataTable tempData = null;
            DataRow[] filteredRows = null;
            Series tempSeries = null;

            string fromDate = "", toDate = "";
            string expression = "";

            FillData();

            if (ViewState["FromDate"] != null)
                fromDate = ViewState["FromDate"].ToString();
            if (ViewState["ToDate"] != null)
                toDate = ViewState["ToDate"].ToString();

            if ((fromDate.Length > 0) && (toDate.Length > 0))
            {
                if (ViewState["RSIData"] != null)
                {
                    tempData = (DataTable)ViewState["RSIData"];
                    expression = "NAVDATE >= '" + fromDate + "' and NAVDATE <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    //if ((filteredRows != null) && (filteredRows.Length > 0))
                    //    dailysmaTable = filteredRows.CopyToDataTable();
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                    {
                        dailyrsiTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        if (dailyrsiTable != null)
                        {
                            dailyrsiTable.Clear();
                            dailyrsiTable.Dispose();
                            dailyrsiTable = null;
                        }
                    }
                    tempData.Clear();
                    tempData.Dispose();
                    tempData = null;
                }
            }
            else
            {
                dailyrsiTable = (DataTable)ViewState["RSIData"];
            }

            GridViewData.DataSource = dailyrsiTable;
            GridViewData.DataBind();

            if (dailyrsiTable != null)
            {
                chartRSI.Series["Daily"].Points.DataBind(dailyrsiTable.AsEnumerable(), "NAVDATE", "NET_ASSET_VALUE", "");
                chartRSI.Series["RSI"].Points.DataBind(dailyrsiTable.AsEnumerable(), "NAVDATE", "RSI", "");

                chartRSI.ChartAreas[1].AxisX.IsStartedFromZero = true;
                chartRSI.ChartAreas[0].AxisX.IsStartedFromZero = true;
                foreach (ListItem item in Master.checkboxlistLines.Items)
                {
                    chartRSI.Series[item.Value].Enabled = item.Selected;
                    if (item.Selected == false)
                    {
                        if (chartRSI.Annotations.FindByName(item.Value) != null)
                            chartRSI.Annotations.Clear();
                    }
                }
                //Master.headingtext.Text = "Momentum Indicator: " + Request.QueryString["script"].ToString();
                Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
            }
            else
            {
                if (expression.Length == 0)
                {
                    Master.headingtext.Text = "Momentum Indicator for scheme code:" + Request.QueryString["schemecode"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                }
                else
                {
                    Master.headingtext.Text = "Momentum Indicator for scheme code: " + Request.QueryString["schemecode"].ToString() + "---Invalid filter. Please correct filter & retry.";
                }
                //Master.headingtext.BackColor = Color.Red;
                Master.headingtext.CssClass = "blinking blinkingText";
            }

            //if (ViewState["RSIData"] != null)
            //{
            //    GridViewData.DataSource = (DataTable)ViewState["RSIData"];
            //    GridViewData.DataBind();

            //    chartRSI.DataSource = (DataTable)ViewState["RSIData"];
            //    chartRSI.DataBind();

            //    if (chartRSI.Annotations.Count > 0)
            //        chartRSI.Annotations.Clear();

            //    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            //    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
            //    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
            //}

        }

        protected void chartRSI_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            //string legendName;
            int chartIndex;

            try
            {
                postBackValues = e.PostBackValue.Split(',');

                if (chartRSI.Annotations.Count > 0)
                    chartRSI.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);

                seriesName = postBackValues[0];

                //double lineHeight = -35;


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //HA.Name = seriesName;
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("RSI"))
                {
                    HA.AxisX = chartRSI.ChartAreas[1].AxisX;
                    HA.AxisY = chartRSI.ChartAreas[1].AxisY;

                    VA.AxisX = chartRSI.ChartAreas[1].AxisX;
                    VA.AxisY = chartRSI.ChartAreas[1].AxisY;

                    ra.AxisX = chartRSI.ChartAreas[1].AxisX;
                    ra.AxisY = chartRSI.ChartAreas[1].AxisY;
                    chartIndex = 1;

                }
                else
                {
                    HA.AxisX = chartRSI.ChartAreas[0].AxisX;
                    HA.AxisY = chartRSI.ChartAreas[0].AxisY;

                    VA.AxisX = chartRSI.ChartAreas[0].AxisX;
                    VA.AxisY = chartRSI.ChartAreas[0].AxisY;

                    ra.AxisX = chartRSI.ChartAreas[0].AxisX;
                    ra.AxisY = chartRSI.ChartAreas[0].AxisY;
                    chartIndex = 0;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartRSI.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartRSI.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartRSIDaily.ChartAreas[chartIndex].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartRSI.Annotations.Add(VA);

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


                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\n" + seriesName + ": " + e.PostBackValue.Split(',')[1];
                //ra.SmartLabelStyle = sl;

                chartRSI.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }
        public void buttonShowGraph_Click()
        {
            ViewState["FromDate"] = Master.textboxFromDate.Text;
            ViewState["ToDate"] = Master.textboxToDate.Text;

            //ViewState["RSIData"] = null;
            //ViewState["SchemeName"] = null;
            ShowGraph();

            //if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null))
            //{
            //    DataTable tempTable = MfDataAPI.getRSIDataTableFromDailyNAV(schemetypeid: Int32.Parse(Request.QueryString["schemetypeid"]),
            //                                                            fundhousecode: Int32.Parse(Request.QueryString["fundhousecode"]),
            //                                                            Int32.Parse(Request.QueryString["schemecode"]), 
            //                                                            fromDate: Master.textboxFromDate.Text, toDate: Master.textboxToDate.Text);
            //    if ((tempTable != null) && (tempTable.Rows.Count > 0))
            //    {
            //        ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
            //        ViewState["RSIData"] = tempTable;
            //        ShowGraph();
            //    }
            //}

            //if (ViewState["RSIData"] != null)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
            //    ShowGraph();

            //    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
            //    {
            //        chartRSI.Visible = true;
            //        chartRSI.Width = int.Parse(Master.panelWidth.Value);
            //        chartRSI.Height = int.Parse(Master.panelHeight.Value);
            //    }
            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            //    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
            //    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
            //}
        }

        protected void buttonShowGrid_Click()
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                //if (ViewState["FetchedData"] != null)
                //{
                GridViewData.Visible = true;
                Master.buttonShowGrid.Text = "Hide Raw Data";
                //GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                //GridViewData.DataBind();
                //}
            }
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;

            ShowGraph();
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