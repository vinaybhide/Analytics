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
    public partial class stoch : System.Web.UI.Page
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
                    headingtext.Text = "Stochastics Oscillator:" + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        chartSTOCH.Visible = true;
                        chartSTOCH.Width = int.Parse(panelWidth.Value);
                        chartSTOCH.Height = int.Parse(panelHeight.Value);
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
            string fastkperiod = "";
            string slowkperiod = "";
            string slowdperiod = "";
            string slowkmatype = "";
            string slowdmatype = "";
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
                    if ((Request.QueryString["interval"] != null) && (Request.QueryString["fastkperiod"] != null) &&
                        (Request.QueryString["slowkperiod"] != null) && (Request.QueryString["slowdperiod"] != null) &&
                        (Request.QueryString["slowkmatype"] != null) && (Request.QueryString["slowdmatype"] != null))
                    {
                        interval = Request.QueryString["interval"];
                        fastkperiod = Request.QueryString["fastkperiod"].ToString();
                        slowkperiod = Request.QueryString["slowkperiod"].ToString();
                        slowdperiod = Request.QueryString["slowdperiod"].ToString();
                        slowkmatype = Request.QueryString["slowkmatype"].ToString();
                        slowdmatype = Request.QueryString["slowdmatype"].ToString();

                        scriptData = StockApi.getSTOCH(folderPath, scriptName, day_interval: interval, fastkperiod: fastkperiod,
                            slowkperiod: slowkperiod, slowdperiod: slowdperiod, slowkmatype: slowkmatype,
                            slowdmatype: slowdmatype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
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
                    //Moving code below to aspx
                    //(chartSTOCH.Series["seriesSlowK"]).XValueMember = "Date";
                    //(chartSTOCH.Series["seriesSlowK"]).XValueType = ChartValueType.Date;
                    //(chartSTOCH.Series["seriesSlowK"]).YValueMembers = "SlowK";
                    ////(chartSTOCH.Series["seriesSlowK"]).ToolTip = "SlowK: Date:#VALX;   Value:#VALY";

                    //chartSTOCH.ChartAreas["chartareaSlowK"].AxisX.Title = "Date";
                    //chartSTOCH.ChartAreas["chartareaSlowK"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartSTOCH.ChartAreas["chartareaSlowK"].AxisY.Title = "SlowK Value";
                    //chartSTOCH.ChartAreas["chartareaSlowK"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    //(chartSTOCH.Series["seriesSlowD"]).XValueMember = "Date";
                    //(chartSTOCH.Series["seriesSlowD"]).XValueType = ChartValueType.Date;
                    //(chartSTOCH.Series["seriesSlowD"]).YValueMembers = "SlowD";
                    ////(chartSTOCH.Series["seriesSlowD"]).ToolTip = "SlowD: Date:#VALX;   Value:#VALY";

                    //chartSTOCH.ChartAreas["chartareaSlowD"].AxisX.Title = "Date";
                    //chartSTOCH.ChartAreas["chartareaSlowD"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartSTOCH.ChartAreas["chartareaSlowD"].AxisY.Title = "SlowD Value";
                    //chartSTOCH.ChartAreas["chartareaSlowD"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    ////chartSTOCH.Titles["titleSTOCH"].Text = $"{"Stochastic Oscillator- "}{scriptName}";

                    //if (chartSTOCH.Annotations.Count > 0)
                    //    chartSTOCH.Annotations.Clear();

                    chartSTOCH.DataSource = scriptData;
                    chartSTOCH.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }

        protected void chartSTOCH_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            int chartIndex;
            try
            {
                if (chartSTOCH.Annotations.Count > 0)
                    chartSTOCH.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                chartIndex = System.Convert.ToInt32(postBackValues[0]);
                xDate = System.Convert.ToDateTime(postBackValues[2]);
                lineWidth = xDate.ToOADate();

                lineHeight = System.Convert.ToDouble(postBackValues[3]);

                seriesName = postBackValues[1];

                //double lineHeight = -35;


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartSTOCH.ChartAreas[chartIndex].AxisX;
                HA.AxisY = chartSTOCH.ChartAreas[chartIndex].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartSTOCH.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartSTOCH.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartSTOCH.ChartAreas[chartIndex].AxisX;
                VA.AxisY = chartSTOCH.ChartAreas[chartIndex].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartSTOCH.ChartAreas[chartIndex].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartSTOCH.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.Name = seriesName;
                ra.AxisX = chartSTOCH.ChartAreas[chartIndex].AxisX;
                ra.AxisY = chartSTOCH.ChartAreas[chartIndex].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date:" + postBackValues[2] + "\n" + seriesName + ":" + postBackValues[3];
                //ra.SmartLabelSty                
                ra.PostBackValue = "AnnotationClicked";

                chartSTOCH.Annotations.Add(ra);

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