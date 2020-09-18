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
    public partial class macd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "MACD: " + Request.QueryString["script"].ToString(); ;
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
                        //Master.headingtext.Text = "Moving average convergence/divergence:" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartMACD.Visible = true;
                        chartMACD.Width = int.Parse(Master.panelWidth.Value);
                        chartMACD.Height = int.Parse(Master.panelHeight.Value);
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
            Master.checkboxlistLines.Visible = false;
            return;
            //Master.checkboxlistLines.Visible = true;
            //ListItem li = new ListItem("ADX", "ADX");
            //li.Selected = true;
            //Master.checkboxlistLines.Items.Add(li);
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("Moving Average Convergence Divergence(MACD) is a trend - following momentum indicator that shows the relationship between two moving averages of a security’s price.");
            Master.bulletedlistDesc.Items.Add("The MACD is calculated by subtracting the 26 - period Exponential Moving Average (EMA) from the 12 - period EMA.");
            Master.bulletedlistDesc.Items.Add("MACD triggers technical signals when it crosses above(to buy) or below(to sell) its signal line.");
            Master.bulletedlistDesc.Items.Add("The speed of crossovers is also taken as a signal of a market is overbought or oversold.");
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

                        //scriptData = StockApi.getMACD(folderPath, scriptName, day_interval: interval, seriestype: seriestype,
                        //                            fastperiod: fastperiod, slowperiod: slowperiod, signalperiod: signalperiod,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());

                        scriptData = StockApi.getMACDAlternate(folderPath, scriptName, day_interval: interval, seriestype: seriestype,
                                                    fastperiod: fastperiod, slowperiod: slowperiod, signalperiod: signalperiod,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
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
                    //Master.headingtext.Text = "Moving average convergence/divergence:" + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Moving average convergence/divergence:" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Moving average convergence/divergence:" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
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
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message+ "');", true);
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