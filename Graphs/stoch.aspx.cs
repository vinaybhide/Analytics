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
    public partial class stoch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "STOCH Graph";
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
                        Master.headingtext.Text = "Stochastics Oscillator:" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartSTOCH.Visible = true;
                        chartSTOCH.Width = int.Parse(Master.panelWidth.Value);
                        chartSTOCH.Height = int.Parse(Master.panelHeight.Value);
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
            Master.bulletedlistDesc.Items.Add("A stochastic oscillator is a momentum indicator comparing a particular closing price of a security to a range of its prices over a certain period of time.");
            Master.bulletedlistDesc.Items.Add("It is used to generate overbought and oversold trading signals, utilizing a 0 - 100 bounded range of values.");
            Master.bulletedlistDesc.Items.Add("Traditionally, readings over 80 are considered in the overbought range, and readings under 20 are considered oversold.");
            Master.bulletedlistDesc.Items.Add("Stochastic oscillator charting generally consists of two lines: one reflecting the actual value of " +
                "the oscillator for each session, and one reflecting its three - day simple moving average.Because price is thought to follow momentum, " +
                "intersection of these two lines is considered to be a signal that a reversal may be in the works, as it indicates a large shift in " +
                "momentum from day to day.");
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

                        //scriptData = StockApi.getSTOCH(folderPath, scriptName, day_interval: interval, fastkperiod: fastkperiod,
                        //    slowkperiod: slowkperiod, slowdperiod: slowdperiod, slowkmatype: slowkmatype,
                        //    slowdmatype: slowdmatype, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        scriptData = StockApi.getSTOCHAlternate(folderPath, scriptName, day_interval: interval, fastkperiod: fastkperiod,
                            slowkperiod: slowkperiod, slowdperiod: slowdperiod, slowkmatype: slowkmatype,
                            slowdmatype: slowdmatype, bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
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
                    Master.headingtext.Text = "Stochastics Oscillator:" + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "STOCH - " + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "STOCH - " + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
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
        public void buttonShowGrid_Click()
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
           ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}