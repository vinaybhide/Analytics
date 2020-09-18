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
    public partial class rsi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "RSI: " + Request.QueryString["script"].ToString(); 
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
                        //Master.headingtext.Text = "Relative strength Index:" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    
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
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph+ "');", true);
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
            Master.checkboxlistLines.Visible = false;
            return;
            //Master.checkboxlistLines.Visible = true;
            //ListItem li = new ListItem("ADX", "ADX");
            //li.Selected = true;
            //Master.checkboxlistLines.Items.Add(li);
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("The relative strength index(RSI) is a momentum indicator used in technical analysis that measures the magnitude of recent price changes to evaluate overbought or oversold conditions in the price of a stock or other asset.");
            Master.bulletedlistDesc.Items.Add("Traditional interpretation and usage of the RSI are that values of 70 or above indicate that a security is becoming overbought or overvalued and may be primed for a trend reversal or corrective pullback in price.An RSI reading of 30 or below indicates an oversold or undervalued condition.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string period = "";
            string seriestype = "";
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
                        seriestype = Request.QueryString["seriestype"].ToString();

                        //scriptData = StockApi.getRSI(folderPath, scriptName, day_interval: interval, period: period,
                        //    seriestype: seriestype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        scriptData = StockApi.getRSIalternate(folderPath, scriptName, day_interval: interval, period: period,
                            seriestype: seriestype, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                    }
                    ViewState["FetchedData"] = scriptData;
                    GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                    GridViewData.DataBind();
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
                    //Adding below code to aspx
                    //(chartRSI.Series["seriesRSI"]).XValueMember = "Date";
                    //(chartRSI.Series["seriesRSI"]).XValueType = ChartValueType.Date;
                    //(chartRSI.Series["seriesRSI"]).YValueMembers = "RSI";
                    ////(chartRSI.Series["seriesRSI"]).ToolTip = "RSI: Date:#VALX;   Value:#VALY";

                    //chartRSI.ChartAreas["chartareaRSI"].AxisX.Title = "Date";
                    //chartRSI.ChartAreas["chartareaRSI"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartRSI.ChartAreas["chartareaRSI"].AxisY.Title = "Value";
                    //chartRSI.ChartAreas["chartareaRSI"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    ////chartRSI.Titles["titleRSI"].Text = $"{"Relative Strength Index- "}{scriptName}";

                    //if (chartRSI.Annotations.Count > 0)
                    //    chartRSI.Annotations.Clear();

                    chartRSI.DataSource = scriptData;
                    chartRSI.DataBind();
                    //Master.headingtext.Text = "Relative strength Index:" + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Relative strength Index:" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Relative strength Index:" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message+ "');", true);

            }
        }

        protected void chartRSI_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (chartRSI.Annotations.Count > 0)
                    chartRSI.Annotations.Clear();
                if (e.PostBackValue.Split(',')[0].Equals("AnnotationClicked"))
                    return;

                DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
                double lineWidth = xDate.ToOADate();

                double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

                //double lineHeight = -35;


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartRSI.ChartAreas[0].AxisX;
                HA.AxisY = chartRSI.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartRSI.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartRSI.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartRSI.ChartAreas[0].AxisX;
                VA.AxisY = chartRSI.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartRSI.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartRSI.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartRSI.ChartAreas[0].AxisX;
                ra.AxisY = chartRSI.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\nRSI: " + e.PostBackValue.Split(',')[1];
                ra.PostBackValue = "AnnotationClicked";
                //ra.SmartLabelStyle = sl;

                chartRSI.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
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
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }

    }
}