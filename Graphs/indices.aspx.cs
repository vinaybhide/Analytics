using Analytics.Graphs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class indices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            //Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            //this.Title = "Daily Price Graph";
            this.Title = "Global Indices";
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                }
                if (!IsPostBack)
                {
                    //Master.headingtext.Text = "Daily Price: " + Request.QueryString["script"].ToString();
                    fillLinesCheckBoxes();
                    fillDesc();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                ShowGraph();
                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    chartdailyIndices.Visible = true;
                    chartdailyIndices.Width = int.Parse(Master.panelWidth.Value);
                    chartdailyIndices.Height = int.Parse(Master.panelHeight.Value);
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
            //Master.checkboxlistLines.Visible = false;
            //return;
            Master.checkboxlistLines.Visible = true;
            ListItem li = new ListItem("BSE SENSEX", "^BSESN");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Nifty 50", "^NSEI");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Dow", "^DJI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Nasdaq", "^IXIC");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Nikkei", "^N225");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("HANG SENG INDEX", "^HSI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("S&P/ASX 200", "^AXJO");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("TSEC weighted index", "^TWII");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("STI Index", "^STI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("SSE Composite Index", "000001.SS");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Shenzhen Component", "399001.SZ");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Jakarta Composite Index", "^JKSE");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("KOSPI Composite Index", "^KS11");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("S&P 500", "^GSPC");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("ALL ORDINARIES", "^AORD");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("FTSE Bursa Malaysia KLCI", "^KLSE");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("NYSE AMEX COMPOSITE INDEX", "^XAX");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Russell 2000", "^RUT");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("CBOE Volatility Index", "^VIX");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("S&P/TSX Composite index", "^GSPTSE");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("FTSE Bursa Malaysia KLCI", "^KLSE");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("FTSE 100", "^FTSE");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("DAX PERFORMANCE-INDEX", "^GDAXI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("CAC 40", "^FCHI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("ESTX 50 PR.EUR", "^STOXX50E");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("EURONEXT 100", "^N100");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("BEL 20", "^BFX");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("MOEX Russia Index", "IMOEX.ME");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("IBOVESPA", "^BVSP");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("IPC MEXICO", "^MXX");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("S&P/CLX IPSA", "^IPSA");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("MERVAL", "^MERV");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("TA-125", "^TA125.TA");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("EGX 30 Price Return Index", "^CASE30");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Top 40 USD Net TRI Index", "^JN0U.JO");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("S&P/NZX 50 INDEX GROSS", "^NZ50");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
        }
        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("Global indices - select one or many from the list above to see graph");
        }

        public void ShowGraph()
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "full";
            string outputSize = "";
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (Session["IsTestOn"] != null)
                {
                    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                }

                if (Session["TestDataFolder"] != null)
                {
                    folderPath = Session["TestDataFolder"].ToString();
                }

                if (chartdailyIndices.Annotations.Count > 0)
                    chartdailyIndices.Annotations.Clear();

                foreach (ListItem item in Master.checkboxlistLines.Items)
                {
                    if (item.Selected == true)
                    {
                        if (chartdailyIndices.Series.FindByName(item.Value) == null)
                        {
                            chartdailyIndices.Series.Add(item.Value);
                            chartdailyIndices.Series[item.Value].Name = item.Value;
                            (chartdailyIndices.Series[item.Value]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Candlestick;
                            (chartdailyIndices.Series[item.Value]).ChartArea = chartdailyIndices.ChartAreas[0].Name;

                            chartdailyIndices.Series[item.Value].Legend = chartdailyIndices.Legends[0].Name;
                            chartdailyIndices.Series[item.Value].LegendText = item.Value;
                            chartdailyIndices.Series[item.Value].LegendToolTip = item.Text;
                        }
                        scriptData = StockApi.getDailyAlternate(folderPath, item.Value, outputsize: outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        if (scriptData != null)
                        {
                            if (ViewState["FromDate"] != null)
                                fromDate = ViewState["FromDate"].ToString();
                            if (ViewState["ToDate"] != null)
                                toDate = ViewState["ToDate"].ToString();

                            if ((fromDate.Length > 0) && (toDate.Length > 0))
                            {
                                tempData = scriptData.Copy();
                                expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                                filteredRows = tempData.Select(expression);
                                if ((filteredRows != null) && (filteredRows.Length > 0))
                                {
                                    scriptData.Clear();
                                    scriptData = filteredRows.CopyToDataTable();
                                }
                            }

                            (chartdailyIndices.Series[item.Value]).Points.DataBindXY(scriptData.Rows, "Date", scriptData.Rows, "High,Low,Open,Close");

                            (chartdailyIndices.Series[item.Value]).XValueMember = "Date";
                            (chartdailyIndices.Series[item.Value]).XValueType = ChartValueType.Date;
                            (chartdailyIndices.Series[item.Value]).YValueMembers = "High,Low,Open,Close";
                            (chartdailyIndices.Series[item.Value]).YValueType = ChartValueType.Double;

                            chartdailyIndices.Series[item.Value].ToolTip = item.Value + ": Date:#VALX; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4 (Click to see details)";
                            chartdailyIndices.Series[item.Value].PostBackValue = item.Value + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                            chartdailyIndices.Series[item.Value].Enabled = item.Selected;
                        }
                        else
                        {
                            item.Selected = false;
                            Master.headingtext.Text = item.Text + "---DATA NOT AVAILABLE. Please try again later.";
                            Master.headingtext.CssClass = "blinking blinkingText";
                        }
                    }
                    else
                    {
                        if (chartdailyIndices.Series.FindByName(item.Value) != null)
                        {
                            chartdailyIndices.Series[item.Value].Enabled = item.Selected;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
            }
        }
        protected void chartdailyIndices_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartdailyIndices.Annotations.Count > 0)
                    chartdailyIndices.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;
                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                HA.AxisY = chartdailyIndices.ChartAreas[0].AxisY;
                VA.AxisY = chartdailyIndices.ChartAreas[0].AxisY;
                ra.AxisY = chartdailyIndices.ChartAreas[0].AxisY;

                HA.AxisX = chartdailyIndices.ChartAreas[0].AxisX;
                VA.AxisX = chartdailyIndices.ChartAreas[0].AxisX;
                ra.AxisX = chartdailyIndices.ChartAreas[0].AxisX;
                HA.ClipToChartArea = chartdailyIndices.ChartAreas[0].Name;
                VA.ClipToChartArea = chartdailyIndices.ChartAreas[0].Name;

                //HA.Name = seriesName;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartdailyIndices.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartdailyIndices.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                //0-OHLC,1-Date,2-High,3-Low,4-Open,5-Close
                ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[4] + "\n" + "High:" + postBackValues[2] + "\n" +
                            "Low:" + postBackValues[3] + "\n" + "Close:" + postBackValues[5];

                chartdailyIndices.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }

        public void buttonShowGraph_Click()
        {
            ViewState["FromDate"] = Master.textboxFromDate.Text;
            ViewState["ToDate"] = Master.textboxToDate.Text;
            ShowGraph();
        }

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