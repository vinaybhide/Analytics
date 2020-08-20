using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class adx : System.Web.UI.Page
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
                    //headingtext.InnerText = "Average directional movement index: " + Request.QueryString["script"].ToString();
                    headingtext.Text = "Average directional movement index: " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        chartADX.Visible = true;
                        chartADX.Width = int.Parse(panelWidth.Value);
                        chartADX.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Response.Redirect("~/Default.aspx");
            }
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression;
            string interval = "";
            string period = "";
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
                    if ((Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null))
                    {
                        interval = Request.QueryString["interval"];
                        period = Request.QueryString["period"];
                        scriptData = StockApi.getADX(folderPath, scriptName, day_interval: interval, period: period,
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
                    //All below is in aspx file
                    //(chartADX.Series["seriesADX"]).XValueMember = "Date";
                    //(chartADX.Series["seriesADX"]).XValueType = ChartValueType.Date;
                    //(chartADX.Series["seriesADX"]).YValueMembers = "ADX";
                    //(chartADX.Series["seriesADX"]).ToolTip = "ADX: Date:#VALX;   Value:#VALY";

                    //chartADX.ChartAreas["chartareaADX"].AxisX.Title = "Date";
                    //chartADX.ChartAreas["chartareaADX"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartADX.ChartAreas["chartareaADX"].AxisY.Title = "Value";
                    //chartADX.ChartAreas["chartareaADX"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    //chartADX.Titles["titleADX"].Text = $"{"Average Directional Movement Index- "}{scriptName}";

                    //if (chartADX.Annotations.Count > 0)
                    //    chartADX.Annotations.Clear();

                    chartADX.DataSource = scriptData;
                    chartADX.DataBind();
                    //chartADX.ChartAreas["chartareaADX"].AxisX.Maximum = chartADX.Series["seriesADX"].Points.FindMaxByValue("X", 0).XValue;
                    chartADX.ChartAreas["chartareaADX"].AxisX.Minimum = chartADX.Series["seriesADX"].Points.FindMinByValue().XValue;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }

        protected void chartADX_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (chartADX.Annotations.Count > 0)
                    chartADX.Annotations.Clear();

                if (e.PostBackValue.Split(',')[0].Equals("AnnotationClicked"))
                    return;

                DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
                double lineWidth = xDate.ToOADate();

                double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);
                //DataPoint p;

                //double lineHeight = -35;

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartADX.ChartAreas[0].AxisX;
                HA.AxisY = chartADX.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartADX.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartADX.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartADX.ChartAreas[0].AxisX;
                VA.AxisY = chartADX.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartADX.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartADX.Annotations.Add(VA);


                //AnnotationSmartLabelStyle sl = new AnnotationSmartLabelStyle();
                //sl.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                //sl.CalloutLineColor = Color.Blue;
                //sl.CalloutLineDashStyle = ChartDashStyle.Solid;
                //sl.CalloutLineWidth = 1;
                //sl.CalloutStyle = LabelCalloutStyle.Box;
                //sl.IsMarkerOverlappingAllowed = false;
                //sl.IsOverlappedHidden = false;
                //sl.MaxMovingDistance = 20;
                //sl.MinMovingDistance = 10;
                //sl.MovingDirection = LabelAlignmentStyles.TopRight;

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartADX.ChartAreas[0].AxisX;
                ra.AxisY = chartADX.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\nADX: " + e.PostBackValue.Split(',')[1];
                //ra.SmartLabelStyle = sl;
                ra.PostBackValue = "AnnotationClicked";

                chartADX.Annotations.Add(ra);

                //p = (chartADX.Series[0]).Points.FindByValue(lineHeight, "Y");


                //if (p != null)
                //{
                //    p.MarkerSize = 8;
                //    p.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                //    p.Label = "Date: " + e.PostBackValue.Split(',')[0] + "\nADX: " + e.PostBackValue.Split(',')[1];
                //    p.LabelBackColor = System.Drawing.Color.Transparent;
                //    p.LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
                //    p.LabelBorderColor = System.Drawing.Color.Black;
                //    p.IsValueShownAsLabel = true;
                //}

            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
            }
        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            //string fromDate = textboxFromDate.Text;
            //string toDate = textboxToDate.Text;
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