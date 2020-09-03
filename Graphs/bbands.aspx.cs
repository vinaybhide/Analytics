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
    public partial class bbands : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Bollinger Band Graph";
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
                        Master.headingtext.Text = "Bollinger Bands: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    
                    ShowGraph(Request.QueryString["script"].ToString());
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartBollingerBands.Visible = true;
                        chartBollingerBands.Width = int.Parse(Master.panelWidth.Value);
                        chartBollingerBands.Height = int.Parse(Master.panelHeight.Value);
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
            Master.bulletedlistDesc.Items.Add("A Bollinger Band is a technical analysis tool defined by a set of trendlines plotted two standard deviations (positively and negatively) away from a simple moving average(SMA) of a security's price, but which can be adjusted to user preferences.");
            Master.bulletedlistDesc.Items.Add("There are three lines that compose Bollinger Bands: A simple moving average(middle band) and an upper and lower band.");
            Master.bulletedlistDesc.Items.Add("The upper and lower bands are typically 2 standard deviations +/ -from a 20 - day simple moving average, but can be modified.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string interval;
            string period;
            string seriestype;
            string nbdevup;
            string nbdevdn;
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
                    if ((Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null) && (Request.QueryString["seriestype"] != null) &&
                        (Request.QueryString["nbdevup"] != null) && (Request.QueryString["nbdevdn"] != null))
                    {
                        interval = Request.QueryString["interval"];
                        period = Request.QueryString["period"];
                        seriestype = Request.QueryString["seriestype"];
                        nbdevup = Request.QueryString["nbdevup"];
                        nbdevdn = Request.QueryString["nbdevdn"];
                        scriptData = StockApi.getBbands(folderPath, scriptName, day_interval: interval, period: period,
                            seriestype: seriestype, nbdevup: nbdevup, nbdevdn: nbdevdn, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
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
                    ///Moved below code toaspx
                    //(chartBollingerBands.Series["Real Lower Band"]).XValueMember = "Date";
                    //(chartBollingerBands.Series["Real Lower Band"]).XValueType = ChartValueType.Date;
                    //(chartBollingerBands.Series["Real Lower Band"]).YValueMembers = "Real Lower Band";
                    ////(chartBollingerBands.Series["Real Lower Band"]).ToolTip = "Lower Band: Date:#VALX;   Value:#VALY";

                    //(chartBollingerBands.Series["Real Middle Band"]).XValueMember = "Date";
                    //(chartBollingerBands.Series["Real Middle Band"]).XValueType = ChartValueType.Date;
                    //(chartBollingerBands.Series["Real Middle Band"]).YValueMembers = "Real Middle Band";
                    ////(chartBollingerBands.Series["Real Middle Band"]).ToolTip = "Middle Band: Date:#VALX;   Value:#VALY";

                    //(chartBollingerBands.Series["Real Upper Band"]).XValueMember = "Date";
                    //(chartBollingerBands.Series["Real Upper Band"]).XValueType = ChartValueType.Date;
                    //(chartBollingerBands.Series["Real Upper Band"]).YValueMembers = "Real Upper Band";

                    ////(chartBollingerBands.Series["Real Upper Band"]).ToolTip = "Upper Band: Date:#VALX;   Value:#VALY";

                    ////chartBollingerBands.Legends.Add("Real Lower Band");
                    ////chartBollingerBands.Legends["Real Lower Band"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                    ////chartBollingerBands.Legends["Real Lower Band"].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                    ////chartBollingerBands.Legends["Real Lower Band"].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                    ////chartBollingerBands.Legends["Real Lower Band"].BorderColor = System.Drawing.Color.Black;

                    ////chartBollingerBands.Legends.Add("Real Middle Band");
                    ////chartBollingerBands.Legends["Real Middle Band"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                    ////chartBollingerBands.Legends["Real Middle Band"].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                    ////chartBollingerBands.Legends["Real Middle Band"].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                    ////chartBollingerBands.Legends["Real Middle Band"].BorderColor = System.Drawing.Color.Black;

                    ////chartBollingerBands.Legends.Add("Real Upper Band");
                    ////chartBollingerBands.Legends["Real Upper Band"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                    ////chartBollingerBands.Legends["Real Upper Band"].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                    ////chartBollingerBands.Legends["Real Upper Band"].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                    ////chartBollingerBands.Legends["Real Upper Band"].BorderColor = System.Drawing.Color.Black;

                    //chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisX.Title = "Date";
                    //chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisY.Title = "Value";
                    //chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    //chartBollingerBands.Titles["titleBbands"].Text = $"{"Bollinger Bands - "}{scriptName}";

                    //VerticalLineAnnotation VA = new VerticalLineAnnotation();
                    //VA.AxisX = chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisX;
                    //VA.IsInfinitive = true;
                    //VA.ClipToChartArea = chartBollingerBands.ChartAreas["chartareaBollingerBands"].Name;
                    //VA.Name = "myLine";
                    //VA.LineColor = System.Drawing.Color.Red;
                    //VA.LineWidth = 10;         // use your numbers!

                    //HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                    //VA.AxisY = chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisY;
                    //VA.IsInfinitive = true;
                    //VA.ClipToChartArea = chartBollingerBands.ChartAreas["chartareaBollingerBands"].Name;
                    //VA.Name = "myLine2";
                    //VA.LineColor = System.Drawing.Color.Red;
                    //VA.LineWidth = 10;         // use your numbers!

                    //chartBollingerBands.Annotations.Add(VA);
                    //chartBollingerBands.Annotations.Add(HA);

                    //if (chartBollingerBands.Annotations.Count > 0)
                    //    chartBollingerBands.Annotations.Clear();

                    chartBollingerBands.DataSource = scriptData;
                    chartBollingerBands.DataBind();

                    //VA.X = chartBollingerBands.Series[0].Points.FindMinByValue("X", 0).XValue;
                    //VA.Y = chartBollingerBands.Series[0].Points.FindMinByValue("Y1", 0).YValues[0]; ;
                    //chartBollingerBands.ChartAreas[0].AxisX.Minimum = chartBollingerBands.Series[0].Points.FindMinByValue("X", 0).XValue;
                    //chartBollingerBands.ChartAreas[0].AxisX.Maximum = chartBollingerBands.Series[0].Points.FindMaxByValue("X", 0).XValue; // + 1;

                    Master.headingtext.Text = "Bollinger Bands: " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Bollinger Bands: " + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Bollinger Bands: " + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }

        }

        protected void chartBollingerBands_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartBollingerBands.Annotations.Count > 0)
                    chartBollingerBands.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];

                //double lineHeight = -35;


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
                HA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartBollingerBands.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
                VA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartBollingerBands.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
                ra.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date:" + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                ra.PostBackValue = "AnnotationClicked";

                //ra.SmartLabelStyle = sl;

                chartBollingerBands.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
            }
        }

        protected void drawLine2(string e)
        {
            DateTime xDate = System.Convert.ToDateTime(e.Split(',')[0]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.Split(',')[1]);

            //double lineHeight = -35;

            if (chartBollingerBands.Annotations.Count > 0)
                chartBollingerBands.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA);

        }
        public void drawLine(string eventargs)
        {
            string[] sargs = eventargs.Split(',');

            double lineWidthClient = System.Convert.ToDouble(sargs[0]);

            double lineHeightClient = System.Convert.ToDouble(sargs[1]);
            double lineWidthScreen = System.Convert.ToDouble(sargs[2]);

            double lineHeightScreen = System.Convert.ToDouble(sargs[3]);

            //double lineHeight = -35;

            if (chartBollingerBands.Annotations.Count > 0)
                chartBollingerBands.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeightClient;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidthClient;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA);

            HorizontalLineAnnotation HA2 = new HorizontalLineAnnotation();
            HA2.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA2.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA2.IsSizeAlwaysRelative = false;
            HA2.AnchorY = lineHeightScreen;
            HA2.IsInfinitive = true;
            HA2.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA2.LineDashStyle = ChartDashStyle.Dash;
            HA2.LineColor = Color.Blue;
            HA2.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA2);

            VerticalLineAnnotation VA2 = new VerticalLineAnnotation();
            VA2.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA2.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA2.IsSizeAlwaysRelative = false;
            VA2.AnchorX = lineWidthScreen;
            VA2.IsInfinitive = true;
            VA2.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA2.LineDashStyle = ChartDashStyle.Dash;
            VA2.LineColor = Color.Blue;
            VA2.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA2);
        }

        //protected void buttonShowGraph_Click(object sender, EventArgs e)
        public void buttonShowGraph_Click()
        {
            //string fromDate = textboxFromDate.Text;
            //string toDate = textboxToDate.Text;
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
                //    GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                //    GridViewData.DataBind();
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