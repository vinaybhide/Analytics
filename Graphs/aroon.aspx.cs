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
    public partial class aroon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "AROON Graph";

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
                        Master.headingtext.Text = "AROON: " + Request.QueryString["script"].ToString();
                        //headingtext.InnerText = "AROON: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    
                    ShowGraph(Request.QueryString["script"].ToString());
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartAROON.Visible = true;
                        chartAROON.Width = int.Parse(Master.panelWidth.Value);
                        chartAROON.Height = int.Parse(Master.panelHeight.Value);
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
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
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
            Master.bulletedlistDesc.Items.Add("The Aroon indicator is a technical indicator that is used to identify trend changes in the price of an asset, as well as the strength of that trend");
            Master.bulletedlistDesc.Items.Add("In essence, the indicator measures the time between highs and the time between lows over a time period");
            Master.bulletedlistDesc.Items.Add("The idea is that strong uptrends will regularly see new highs, and strong downtrends will regularly see new lows. The indicator signals when this is happening, and when it isn't.");
            Master.bulletedlistDesc.Items.Add("The indicator consists of the \"Aroon up\" line, which measures the strength of the uptrend, and the \"Aroon down\" line, which measures the strength of the downtrend.");
            Master.bulletedlistDesc.Items.Add("Aroon Up measures the number of periods since a High, and Aroon Down line measures the number of periods since a Low.");
            Master.bulletedlistDesc.Items.Add("When the Aroon Up is above the Aroon Down, it indicates bullish price behavior.");
            Master.bulletedlistDesc.Items.Add("When the Aroon Down is above the Aroon Up, it signals bearish price behavior.");
            Master.bulletedlistDesc.Items.Add("Crossovers of the two lines can signal trend changes. For example, when Aroon Up crosses above Aroon Down it may mean a new uptrend is starting.");
            Master.bulletedlistDesc.Items.Add("The indicator moves between zero and 100. A reading above 50 means that a high/ low(whichever line is above 50) was seen within the last 12 periods.");
            Master.bulletedlistDesc.Items.Add("A reading below 50 means that the high/ low was seen within the 13 periods.");
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
                        //scriptData = StockApi.getAROON(folderPath, scriptName, day_interval: interval, period: period,
                        //                                bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        scriptData = StockApi.getAroonAlternate(folderPath, scriptName, day_interval: interval, period: period,
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
                    //moved below code to aspx
                    //(chartAROON.Series["seriesAROON_Down"]).XValueMember = "Date";
                    //(chartAROON.Series["seriesAROON_Down"]).XValueType = ChartValueType.Date;
                    //(chartAROON.Series["seriesAROON_Down"]).YValueMembers = "Aroon Down";
                    ////(chartAROON.Series["seriesAROON_Down"]).ToolTip = "AROON Down: Date:#VALX;   Value:#VALY";

                    //(chartAROON.Series["seriesAROON_Up"]).XValueMember = "Date";
                    //(chartAROON.Series["seriesAROON_Up"]).XValueType = ChartValueType.Date;
                    //(chartAROON.Series["seriesAROON_Up"]).YValueMembers = "Aroon Up";
                    ////(chartAROON.Series["seriesAROON_Up"]).ToolTip = "AROON Up: Date:#VALX;   Value:#VALY";

                    //chartAROON.ChartAreas["chartareaAROON"].AxisX.Title = "Date";
                    //chartAROON.ChartAreas["chartareaAROON"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartAROON.ChartAreas["chartareaAROON"].AxisY.Title = "Value";
                    //chartAROON.ChartAreas["chartareaAROON"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    ////chartAROON.Titles["titleAROON"].Text = $"{"AROON- "}{scriptName}";

                    //if (chartAROON.Annotations.Count > 0)
                    //    chartAROON.Annotations.Clear();

                    chartAROON.DataSource = scriptData;
                    chartAROON.DataBind();

                    Master.headingtext.Text = "AROON: " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "AROON: " + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "AROON: " + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
            }
        }

        protected void chartAROON_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            try
            {
                postBackValues = e.PostBackValue.Split(',');

                if (chartAROON.Annotations.Count > 0)
                    chartAROON.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }
                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartAROON.ChartAreas[0].AxisX;
                HA.AxisY = chartAROON.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartAROON.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartAROON.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartAROON.ChartAreas[0].AxisX;
                VA.AxisY = chartAROON.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartAROON.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartAROON.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartAROON.ChartAreas[0].AxisX;
                ra.AxisY = chartAROON.ChartAreas[0].AxisY;
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
                chartAROON.Annotations.Add(ra);
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