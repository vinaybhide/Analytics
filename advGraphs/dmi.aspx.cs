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
    public partial class dmi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Price direction & strength";
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataDaily"] = null;
                    ViewState["FetchedDataMINUSDM"] = null;
                    ViewState["FetchedDataPLUSDM"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Price Direction & strength: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());
                    
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartDMIDaily.Visible = true;
                        chartDMIDaily.Width = int.Parse(Master.panelWidth.Value);
                        chartDMIDaily.Height = int.Parse(Master.panelHeight.Value);
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
            ListItem li;

            li = new ListItem("-ve Directinal Movement Indicator(-DMI)", "MINUS_DM");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("+ve Directinal Movement Indicator(+DMI)", "PLUS_DM");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Open", "Open");
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
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("The directional movement indicator(also known as the directional movement index or DMI) is a valuable tool for assessing price direction and strength.");
            Master.bulletedlistDesc.Items.Add("The DMI is especially useful for trend trading strategies because it differentiates between strong and weak trends, allowing the trader to enter only the ones with real momentum.");
            Master.bulletedlistDesc.Items.Add("DMI tells you when to be long or short.");
            Master.bulletedlistDesc.Items.Add("DMI comprises of two lines, +DMI & -DMI.The line which is on top is referred as dominant DMI.The dominant DMI is stronger and more likely to predict the direction of price.For the buyers and sellers to change dominance, the lines must cross over.");
            Master.bulletedlistDesc.Items.Add("The + DMI generally moves in sync with price, which means the + DMI rises when price rises, and it falls when price falls. It is important to note that the - DMI behaves in the opposite manner and moves counter - directional to price. The - DMI rises when price falls, and it falls when price rises.");
            Master.bulletedlistDesc.Items.Add("Reading directional signals is easy.");
            Master.bulletedlistDesc.Items.Add("When the + DMI is dominant and rising, price direction is up.");
            Master.bulletedlistDesc.Items.Add("When the - DMI is dominant and rising, price direction is down.");
            Master.bulletedlistDesc.Items.Add("But the strength of price must also be considered.DMI strength ranges from a low of 0 to a high of 100. The higher the DMI value, the stronger the prices swing.");
            Master.bulletedlistDesc.Items.Add("DMI values over 25 mean price is directionally strong. DMI values under 25 mean price is directionally weak.");
            Master.bulletedlistDesc.Items.Add("When the buyers are stronger than the sellers, the + DMI peaks will be above 25 and the - DMI peaks will be below 25. This is seen in a strong uptrend.But when the sellers are stronger than the buyers, the - DMI peaks will be above 25 and the + DMI peaks will be below 25.In this case, the trend will be down.");
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable dailyData = null;
            DataTable minusdmData = null;
            DataTable plusdmData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize;
            string interval_minusdm;
            string period_minusdm;
            string interval_plusdm;
            string period_plusdm;

            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
                if (((ViewState["FetchedDataDaily"] == null) || (ViewState["FetchedDataMINUSDM"] == null)
                        || (ViewState["FetchedDataPLUSDM"] == null))
                    || ((((DataTable)ViewState["FetchedDataDaily"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataMINUSDM"]).Rows.Count == 0) ||
                     (((DataTable)ViewState["FetchedDataPLUSDM"]).Rows.Count == 0))
                     )
                {
                    if (Session["IsTestOn"] != null)
                    {
                        bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                    }

                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }
                    if ((Request.QueryString["size"] != null)
                        && (Request.QueryString["intervalminusdm"] != null) && (Request.QueryString["periodminusdm"] != null)
                        && (Request.QueryString["intervalplusdm"] != null) && (Request.QueryString["periodplusdm"] != null)
                        )
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval_minusdm = Request.QueryString["intervalminusdm"];
                        period_minusdm = Request.QueryString["periodminusdm"];
                        interval_plusdm = Request.QueryString["intervalplusdm"];
                        period_plusdm = Request.QueryString["periodplusdm"];

                        dailyData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        if (dailyData== null)
                        {
                            //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                            dailyData = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputSize,
                                                    bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        }

                        ViewState["FetchedDataDaily"] = dailyData;

                        minusdmData = StockApi.getMinusDM(folderPath, scriptName, day_interval: interval_minusdm, period: period_minusdm,
                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataMINUSDM"] = minusdmData;

                        plusdmData = StockApi.getPlusDM(folderPath, scriptName, day_interval: interval_plusdm, period: period_plusdm,
                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataPLUSDM"] = plusdmData;

                    }
                    else
                    {
                        ViewState["FetchedDataDaily"] = null;
                        dailyData = null;
                        ViewState["FetchedDataMINUSDM"] = null;
                        minusdmData = null;
                        ViewState["FetchedDataPLUSDM"] = null;
                        plusdmData = null;
                    }
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
                    GridViewDaily.DataBind();
                    GridViewMINUSDM.DataSource = (DataTable)ViewState["FetchedDataPLUSDM"];
                    GridViewMINUSDM.DataBind();
                    GridViewPLUSDM.DataSource = (DataTable)ViewState["FetchedDataPLUSDM"];
                    GridViewPLUSDM.DataBind();
                }
                //else
                //{
                if (ViewState["FromDate"] != null)
                    fromDate = ViewState["FromDate"].ToString();
                if (ViewState["ToDate"] != null)
                    toDate = ViewState["ToDate"].ToString();

                if ((fromDate.Length > 0) && (toDate.Length > 0))
                {
                    tempData = (DataTable)ViewState["FetchedDataDaily"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        dailyData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataMINUSDM"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        minusdmData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataPLUSDM"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        plusdmData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;
                }
                else
                {
                    dailyData = (DataTable)ViewState["FetchedDataDaily"];
                    minusdmData = (DataTable)ViewState["FetchedDataMINUSDM"];
                    plusdmData = (DataTable)ViewState["FetchedDataPLUSDM"];
                }
                //}

                if ((dailyData != null) && (minusdmData != null) && (plusdmData != null))
                {
                    chartDMIDaily.Series["Open"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open", "");
                    chartDMIDaily.Series["High"].Points.DataBind(dailyData.AsEnumerable(), "Date", "High", "");
                    chartDMIDaily.Series["Low"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Low", "");
                    chartDMIDaily.Series["Close"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Close", "");
                    chartDMIDaily.Series["OHLC"].Points.DataBind(dailyData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartDMIDaily.Series["MINUS_DM"].Points.DataBind(minusdmData.AsEnumerable(), "Date", "MINUS_DM", "");
                    chartDMIDaily.Series["PLUS_DM"].Points.DataBind(plusdmData.AsEnumerable(), "Date", "PLUS_DM", "");

                    chartDMIDaily.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartDMIDaily.ChartAreas[1].AxisX.IsStartedFromZero = true;

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartDMIDaily.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartDMIDaily.Annotations.FindByName(item.Value) != null)
                                chartDMIDaily.Annotations.Clear();
                        }
                    }
                    Master.headingtext.Text = "Price Direction & strength: " + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Price Direction & strength-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Price Direction & strength-" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }
        protected void chartDMIDaily_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            int chartindex;
            //string legendName;

            //DataPoint p;
            //double lineHeight = -35;

            try
            {
                postBackValues = e.PostBackValue.Split(',');

                if (chartDMIDaily.Annotations.Count > 0)
                    chartDMIDaily.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //HA.Name = seriesName;
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();
                if (seriesName.Equals("MINUS_DM") || seriesName.Equals("PLUS_DM"))
                {
                    HA.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    HA.AxisY = chartDMIDaily.ChartAreas[1].AxisY;

                    VA.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    VA.AxisY = chartDMIDaily.ChartAreas[1].AxisY;

                    ra.AxisX = chartDMIDaily.ChartAreas[1].AxisX;
                    ra.AxisY = chartDMIDaily.ChartAreas[1].AxisY;
                    chartindex = 1;
                }
                else
                {
                    HA.AxisX = chartDMIDaily.ChartAreas[0].AxisX;
                    HA.AxisY = chartDMIDaily.ChartAreas[0].AxisY;

                    VA.AxisX = chartDMIDaily.ChartAreas[0].AxisX;
                    VA.AxisY = chartDMIDaily.ChartAreas[0].AxisY;

                    ra.AxisX = chartDMIDaily.ChartAreas[0].AxisX;
                    ra.AxisY = chartDMIDaily.ChartAreas[0].AxisY;
                    chartindex = 0;
                }
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartDMIDaily.ChartAreas[chartindex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartDMIDaily.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartDMIDaily.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartDMIDaily.Annotations.Add(VA);

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

                chartDMIDaily.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
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

        //protected void buttonDesc_Click(object sender, EventArgs e)
        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }


        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
            GridViewDaily.DataBind();
        }

        protected void GridViewMINUSDM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMINUSDM.PageIndex = e.NewPageIndex;
            GridViewMINUSDM.DataSource = (DataTable)ViewState["FetchedDataMINUSDM"];
            GridViewMINUSDM.DataBind();
        }
        protected void GridViewPLUSDM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPLUSDM.PageIndex = e.NewPageIndex;
            GridViewPLUSDM.DataSource = (DataTable)ViewState["FetchedDataPLUSDM"];
            GridViewPLUSDM.DataBind();
        }
        void buttonShowGrid_Click()
        {
            if ((GridViewDaily.Visible) || (GridViewMINUSDM.Visible) || (GridViewPLUSDM.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewMINUSDM.Visible = false;
                GridViewPLUSDM.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                //if (ViewState["FetchedDataDaily"] != null)
                //{
                    GridViewDaily.Visible = true;
                 //   GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataDaily"];
                 //   GridViewDaily.DataBind();
                //}
                //if (ViewState["FetchedDataMINUSDM"] != null)
                //{
                    GridViewMINUSDM.Visible = true;
                 //   GridViewMINUSDM.DataSource = (DataTable)ViewState["FetchedDataPLUSDM"];
                 //   GridViewMINUSDM.DataBind();
                //}
                //if (ViewState["FetchedDataPLUSDM"] != null)
                //{
                    GridViewPLUSDM.Visible = true;
                 //   GridViewPLUSDM.DataSource = (DataTable)ViewState["FetchedDataPLUSDM"];
                 //   GridViewPLUSDM.DataBind();
                //}
            }
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}