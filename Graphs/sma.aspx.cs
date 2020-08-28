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

namespace Analytics
{
    public partial class sma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "SMA Graph";
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
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Simple moving average:" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ShowGraph(Request.QueryString["script"].ToString());
                    
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartSMA.Visible = true;
                        chartSMA.Width = int.Parse(Master.panelWidth.Value);
                        chartSMA.Height = int.Parse(Master.panelHeight.Value);
                    }
                }
                else
                {
                    Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
                    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Server.Transfer("~/Default.aspx");
                //Response.Redirect("~/Default.aspx");
            }
        }

        public void fillLinesCheckBoxes()
        {
            Master.checkboxlistLines.Visible = false;
            return;
            //Master.checkboxlistLines.Visible = true;
            //ListItem li = new ListItem("ADX", "ADX");
            //li.Selected = true;
            //Master.checkboxlistLines.Items.Add(li);
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("A simple moving average(SMA) calculates the average of a selected range of prices, usually closing prices, by thenumber of periods in that range.");
            Master.bulletedlistDesc.Items.Add("The SMA is a technical indicator that can aid in determining if an asset price will continue or reverse a bull or bear trend.");
            Master.bulletedlistDesc.Items.Add("A simple moving average smooths out volatility, and makes it easier to view the price trend of a security.");
            Master.bulletedlistDesc.Items.Add("If the simple moving average points up, this means that the security's price is increasing. If it is pointing down it means that the security's price is decreasing.");
            Master.bulletedlistDesc.Items.Add("The longer the time frame for the moving average, the smoother the simple moving average.A shorter - term moving average is more volatile, but its reading is closer to the source data.");
            Master.bulletedlistDesc.Items.Add("Two popular trading patterns that use simple moving averages include the death cross and a golden cross.");
            Master.bulletedlistDesc.Items.Add("A death cross occurs when the 50 - day SMA crosses below the 200 - day SMA.This is considered a bearish signal, that further losses are in store.");
            Master.bulletedlistDesc.Items.Add("The golden cross occurs when a short-term SMA breaks above a long-term SMA.Reinforced by high trading volumes, this can signal further gains are in store.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string period = "";
            string seriesType = "";
            string interval = "";
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
                    if ((Request.QueryString["period"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["seriestype"] != null))
                    {
                        interval = Request.QueryString["interval"];
                        period = Request.QueryString["period"].ToString();
                        seriesType = Request.QueryString["seriestype"].ToString();

                        scriptData = StockApi.getSMA(folderPath, scriptName, day_interval: interval, period: period, seriestype: seriesType,
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
                    //moving code below to aspx
                    //(chartSMA.Series["seriesSMA"]).XValueMember = "Date";
                    //(chartSMA.Series["seriesSMA"]).XValueType = ChartValueType.Date;
                    //(chartSMA.Series["seriesSMA"]).YValueMembers = "SMA";
                    ////(chartSMA.Series["seriesSMA"]).ToolTip = "SMA: Date:#VALX;   Value:#VALY";

                    //chartSMA.ChartAreas["chartareaSMA"].AxisX.Title = "Date";
                    //chartSMA.ChartAreas["chartareaSMA"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartSMA.ChartAreas["chartareaSMA"].AxisY.Title = "Value";
                    //chartSMA.ChartAreas["chartareaSMA"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    ////chartSMA.Titles["titleSMA"].Text = $"{"Simple Moving Average - "}{scriptName}";

                    //if (chartSMA.Annotations.Count > 0)
                    //    chartSMA.Annotations.Clear();

                    chartSMA.DataSource = scriptData;
                    chartSMA.DataBind();
                }
                else
                {
                    Master.headingtext.Text = "Simple moving average:" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }

        protected void chartSMA_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (chartSMA.Annotations.Count > 0)
                    chartSMA.Annotations.Clear();

                if (e.PostBackValue.Split(',')[0].Equals("AnnotationClicked"))
                    return;

                DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
                double lineWidth = xDate.ToOADate();

                double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

                //double lineHeight = -35;


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartSMA.ChartAreas[0].AxisX;
                HA.AxisY = chartSMA.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartSMA.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartSMA.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartSMA.ChartAreas[0].AxisX;
                VA.AxisY = chartSMA.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartSMA.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartSMA.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartSMA.ChartAreas[0].AxisX;
                ra.AxisY = chartSMA.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\nSMA: " + e.PostBackValue.Split(',')[1];
                ra.PostBackValue = "AnnotationClicked";
                //ra.SmartLabelStyle = sl;

                chartSMA.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
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

        //protected void buttonShowGrid_Click(object sender, EventArgs e)
        public void buttonShowGrid_Click()
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                if (ViewState["FetchedData"] != null)
                {
                    GridViewData.Visible = true;
                    Master.buttonShowGrid.Text = "Hide Raw Data";
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

        //protected void buttonDesc_Click(object sender, EventArgs e)
        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }

    }
}