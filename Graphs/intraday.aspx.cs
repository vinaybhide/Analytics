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
    public partial class intraday : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Intra-day Price Graph";
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
                        Master.headingtext.Text = "Intra-day Prices: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    
                    ShowGraph(Request.QueryString["script"].ToString());
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartintraGraph.Visible = true;
                        chartintraGraph.Width = int.Parse(Master.panelWidth.Value);
                        chartintraGraph.Height = int.Parse(Master.panelHeight.Value);
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
            //Master.checkboxlistLines.Visible = false;
            //return;
            Master.checkboxlistLines.Visible = true;
            ListItem li = new ListItem("Open", "Open");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("High", "High");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Low", "Low");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Close", "Close");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Volume", "Volume");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("A daily price chart is a graph of data points, where each point represents the security's price action for a specific day of trading.");
            Master.bulletedlistDesc.Items.Add("Daily charts are one of the main tools used by technical traders seeking to profit from intraday price movements and longer - term trends.");
            Master.bulletedlistDesc.Items.Add("A daily chart may focus on the price action of a security for a single day or it can also, comprehensively,show the daily price movements of a security over a specified time frame.");
            Master.bulletedlistDesc.Items.Add("Candlestick charts are popular mainly due to the ease with which they convey the basic information, such as the opening and closing price, as well as the trading range for the selected period of time.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize = "";
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
                    if ((Request.QueryString["size"] != null) && (Request.QueryString["interval"] != null))
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval = Request.QueryString["interval"];
                        scriptData = StockApi.getIntraday(folderPath, scriptName, time_interval: interval, outputsize: outputSize,
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

                    if (chartintraGraph.Annotations.Count > 0)
                        chartintraGraph.Annotations.Clear();
                }
                else
                {
                    scriptData = (DataTable)ViewState["FetchedData"];
                }
                //}
                if (scriptData != null)
                {
                    chartintraGraph.DataSource = scriptData;
                    chartintraGraph.DataBind();
                    //moved following line to aspx
                    //chartintraGraph.ChartAreas[0].AxisX.LabelStyle.Format = "g";
                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartintraGraph.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartintraGraph.Annotations.FindByName(item.Value) != null)
                                chartintraGraph.Annotations.Clear();
                        }
                    }
                }
                else
                {
                    Master.headingtext.Text = "Intra-day Prices: " + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
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

        protected void chartintraGraph_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartintraGraph.Annotations.Count > 0)
                    chartintraGraph.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);

                seriesName = postBackValues[0];

                //if (postBackValues.Length > 3) //OHLC
                //{
                //    seriesName = postBackValues[5];

                //}
                //else
                //{
                //    seriesName = postBackValues[2];
                //}

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //HA.Name = seriesName;
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("Volume"))
                {
                    HA.AxisY = chartintraGraph.ChartAreas[0].AxisY2;
                    VA.AxisY = chartintraGraph.ChartAreas[0].AxisY2;
                    ra.AxisY = chartintraGraph.ChartAreas[0].AxisY2;
                }
                else
                {
                    HA.AxisY = chartintraGraph.ChartAreas[0].AxisY;
                    VA.AxisY = chartintraGraph.ChartAreas[0].AxisY;
                    ra.AxisY = chartintraGraph.ChartAreas[0].AxisY;
                }

                HA.AxisX = chartintraGraph.ChartAreas[0].AxisX;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartintraGraph.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartintraGraph.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.AxisX = chartintraGraph.ChartAreas[0].AxisX;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartintraGraph.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartintraGraph.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.AxisX = chartintraGraph.ChartAreas[0].AxisX;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                if (seriesName.Equals("OHLC"))
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[2] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[5];
                }
                else
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                }
                //ra.SmartLabelStyle = sl;

                chartintraGraph.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
            }
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewDaily.DataBind();
        }

        void buttonShowGrid_Click()
        {
            if (GridViewDaily.Visible)
            {
                GridViewDaily.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                if (ViewState["FetchedData"] != null)
                {
                    GridViewDaily.Visible = true;
                    Master.buttonShowGrid.Text = "Hide Raw Data";
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
                    GridViewDaily.DataBind();
                }
            }
        }

        //protected void buttonDesc_Click(object sender, EventArgs e)
        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }

        //protected void chart_PreRender(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        //}

    }
}