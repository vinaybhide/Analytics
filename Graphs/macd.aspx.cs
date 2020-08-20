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
    public partial class macd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedData"] = null;
                }

                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    headingtext.Text = "Moving average convergence/divergence:" + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        chartMACD.Visible = true;
                        chartMACD.Width = int.Parse(panelWidth.Value);
                        chartMACD.Height = int.Parse(panelHeight.Value);
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
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string interval = "";
            string seriestype = "";
            string fastperiod = "";
            string slowperiod = "";
            string signalperiod = "";
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                {
                    if (Session["IsTestOn"] != null)
                    {
                        bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                    }

                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }
                    if ((Request.QueryString["interval"] != null) && (Request.QueryString["seriestype"] != null) &&
                        (Request.QueryString["fastperiod"] != null) && (Request.QueryString["slowperiod"] != null) &&
                        (Request.QueryString["signalperiod"] != null))
                    {
                        interval = Request.QueryString["interval"];
                        seriestype = Request.QueryString["seriestype"].ToString();
                        fastperiod = Request.QueryString["fastperiod"].ToString();
                        slowperiod = Request.QueryString["slowperiod"].ToString();
                        signalperiod = Request.QueryString["signalperiod"].ToString();

                        scriptData = StockApi.getMACD(folderPath, scriptName, day_interval: interval, seriestype: seriestype,
                                                    fastperiod: fastperiod, slowperiod: slowperiod, signalperiod: signalperiod,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                    }
                    ViewState["FetchedData"] = scriptData;
                }
                //else
                //{
                if (ViewState["FromDate"] != null)
                    fromDate = ViewState["FromDate"].ToString();
                if (ViewState["ToDate"] != null)
                    toDate = ViewState["ToDate"].ToString();

                if ((fromDate.Length > 0) && (toDate.Length > 0))
                {
                    tempData = (DataTable)ViewState["FetchedData"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        scriptData = filteredRows.CopyToDataTable();
                }
                else
                {
                    scriptData = (DataTable)ViewState["FetchedData"];
                }
                //}

                if (scriptData != null)
                {
                    //FOllowing code moved to aspx
                    //(chartMACD.Series["seriesMACD"]).XValueMember = "Date";
                    //(chartMACD.Series["seriesMACD"]).XValueType = ChartValueType.Date;
                    //(chartMACD.Series["seriesMACD"]).YValueMembers = "MACD";
                    ////(chartMACD.Series["seriesMACD"]).ToolTip = "MACD: Date:#VALX;   Value:#VALY";

                    //(chartMACD.Series["seriesMACD_Hist"]).XValueMember = "Date";
                    //(chartMACD.Series["seriesMACD_Hist"]).XValueType = ChartValueType.Date;
                    //(chartMACD.Series["seriesMACD_Hist"]).YValueMembers = "MACD_Hist";
                    ////(chartMACD.Series["seriesMACD_Hist"]).ToolTip = "MACD_Hist: Date:#VALX;   Value:#VALY";

                    //(chartMACD.Series["seriesMACD_Signal"]).XValueMember = "Date";
                    //(chartMACD.Series["seriesMACD_Signal"]).XValueType = ChartValueType.Date;
                    //(chartMACD.Series["seriesMACD_Signal"]).YValueMembers = "MACD_Signal";
                    ////(chartMACD.Series["seriesMACD_Signal"]).ToolTip = "MACD_Signal: Date:#VALX;   Value:#VALY";

                    //chartMACD.ChartAreas["chartareaMACD"].AxisX.Title = "Date";
                    //chartMACD.ChartAreas["chartareaMACD"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartMACD.ChartAreas["chartareaMACD"].AxisY.Title = "Value";
                    //chartMACD.ChartAreas["chartareaMACD"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    ////chartMACD.Titles["titleMACD"].Text = $"{"Moving Average Convergence Divergence- "}{scriptName}";

                    //if (chartMACD.Annotations.Count > 0)
                    //    chartMACD.Annotations.Clear();

                    chartMACD.DataSource = scriptData;
                    chartMACD.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }

        protected void chartMACD_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            try
            {
                if (chartMACD.Annotations.Count > 0)
                    chartMACD.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationCliecked"))
                    return;

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();

                lineHeight = System.Convert.ToDouble(postBackValues[2]);

                seriesName = postBackValues[0];

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("MACD_Hist"))
                {
                    HA.AxisX = chartMACD.ChartAreas[0].AxisX;
                    HA.AxisY = chartMACD.ChartAreas[0].AxisY2;
                    VA.AxisX = chartMACD.ChartAreas[0].AxisX;
                    VA.AxisY = chartMACD.ChartAreas[0].AxisY2;
                    ra.AxisX = chartMACD.ChartAreas[0].AxisX;
                    ra.AxisY = chartMACD.ChartAreas[0].AxisY2;
                }
                else
                {
                    HA.AxisX = chartMACD.ChartAreas[0].AxisX2;
                    HA.AxisY = chartMACD.ChartAreas[0].AxisY;
                    VA.AxisX = chartMACD.ChartAreas[0].AxisX2;
                    VA.AxisY = chartMACD.ChartAreas[0].AxisY;
                    ra.AxisX = chartMACD.ChartAreas[0].AxisX2;
                    ra.AxisY = chartMACD.ChartAreas[0].AxisY;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartMACD.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartMACD.Annotations.Add(HA);


                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartMACD.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartMACD.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                ra.PostBackValue = "AnnotationCliecked";
                //ra.SmartLabelStyle = sl;

                chartMACD.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
            }
        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            string fromDate = textboxFromDate.Text;
            string toDate = textboxToDate.Text;
            string scriptName = Request.QueryString["script"].ToString();
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            ShowGraph(scriptName);
        }

        protected void buttonShowGrid_Click(object sender, EventArgs e)
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                if (ViewState["FetchedData"] != null)
                {
                    GridViewData.Visible = true;
                    buttonShowGrid.Text = "Hide Raw Data";
                    GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                    GridViewData.DataBind();
                }
            }
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewData.DataBind();
        }

        protected void buttonDesc_Click(object sender, EventArgs e)
        {
            if (trid.Visible)
                trid.Visible = false;
            else
                trid.Visible = true;
        }
    }
}