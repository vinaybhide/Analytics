using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics.advGraphs
{
    public partial class globalindex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "doHourglass", "doHourglass();", true);
            Master.OnDoEventShowGraph += new advancegraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new advancegraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventRemoveSelectedIndicatorGraph += new advancegraphs.DoEventRemoveSelectedIndicatorGraph(buttonRemoveSelectedIndicatorGraph_Click);
            Master.OnDoEventShowSelectedIndicatorGraph += new advancegraphs.DoEventShowSelectedIndicatorGraph(buttonShowSelectedIndicatorGraph_Click);
            if (!IsPostBack)
            {
                ViewState["MAIN_DATA"] = null;

                ManagePanels();
                fillGraphList();
                Master.FillExchangeForIndexList();
                Master.FillIndexList();

                if (Request.QueryString["fromdate"] != null)
                {
                    Master.textboxFromDate.Text = System.Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");
                }
                else
                {
                    Master.textboxFromDate.Text = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
                }

                if (Request.QueryString["todate"] != null)
                {
                    //we do not need todate from caller, but still...
                    Master.textboxToDate.Text = System.Convert.ToDateTime(Request.QueryString["todate"].ToString()).ToString("yyyy-MM-dd");
                }
                else
                {
                    Master.textboxToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    Master.textboxToDate.Enabled = false;
                }

            }
            this.Title = "Global Indexes Graph";

            if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
            {
                //ShowGraph(scriptName);
                chartAdvGraph.Visible = true;
                chartAdvGraph.Width = int.Parse(Master.panelWidth.Value);
                chartAdvGraph.Height = int.Parse(Master.panelHeight.Value);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "resetCursor", "resetCursor();", true);
        }

        public void ManagePanels()
        {
            Master.panelTopMost.Enabled = true;
            Master.panelTopMost.Visible = true;

            Master.panelStocksMain.Enabled = false;
            Master.panelStocksMain.Visible = false;

            Master.panelMFMain.Enabled = false;
            Master.panelMFMain.Visible = false;

            Master.panelCommonControls.Enabled = true;
            Master.panelCommonControls.Visible = true;

            Master.panelGraphList.Enabled = true;
            Master.panelGraphList.Visible = true;

            Master.panelFromTo.Enabled = true;
            Master.panelFromTo.Visible = true;

            Master.panelMainParam.Enabled = true;
            Master.panelMainParam.Visible = true;

            Master.panelGlobalIndex.Enabled = true;
            Master.panelGlobalIndex.Visible = true;

            Master.panelDescription.Enabled = false;
            Master.panelDescription.Visible = false;

            Master.buttonDesc.Enabled = false;
            Master.buttonDesc.Visible = false;
        }
        public void fillGraphList()
        {
            Master.dropdownGraphList.Visible = true;

            Master.dropdownGraphList.Items.Clear();

            ListItem li;

            li = new ListItem("% Change", "PctChange");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("Open", "Open");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("High", "High");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("Low", "Low");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("Close", "Close");
            Master.dropdownGraphList.Items.Add(li);
            li = new ListItem("Volume", "Volume");
            Master.dropdownGraphList.Items.Add(li);
        }
        public void ShowIndexGraph()
        {
            DataTable dailyData = null;
            StockManager stockManager = new StockManager();

            string fromDate = Master.textboxFromDate.Text;

            string selectedindex = Master.ddlIndexList.SelectedValue;
            string selectedindexname = Master.ddlIndexList.SelectedValue;
            bool bIntraday = false;
            try
            {
                DateTime dateFrom = System.Convert.ToDateTime(fromDate);
                if (DateTime.Today.ToShortDateString().Equals(dateFrom.ToShortDateString()))
                {
                    bIntraday = true;
                    //in case of today we will need to make sure we set time interval to min. If caller did not send interval in mins then we will default to 5min
                    //dailyData = stockManager.GetStockPriceData(selectedindex, time_interval: "5m", fromDate: fromDate);
                    //dailyData = stockManager.GetIntraWithPctChange(selectedindex, time_interval: "1min", outputsize: "compact");
                    dailyData = stockManager.GetStockPriceData(selectedindex, outputsize: "Compact", time_interval: "1m", fromDate: fromDate);
                    //Master.dropdownGraphList.Items[0].Enabled = true;
                    //Master.dropdownGraphList.Items[6].Enabled = false;
                }
                else
                {
                    dailyData = stockManager.GetStockPriceData(selectedindex, fromDate: fromDate);
                    //Master.dropdownGraphList.Items[0].Enabled = false;
                    //Master.dropdownGraphList.Items[6].Enabled = true;
                }

                if (chartAdvGraph.Annotations.Count > 0)
                    chartAdvGraph.Annotations.Clear();
                ViewState["MAIN_DATA"] = null;

                if ((dailyData != null) && (dailyData.Rows.Count > 0))
                {
                   GridViewData.DataSource = dailyData;
                    GridViewData.DataBind();

                    ViewState["MAIN_DATA"] = dailyData;

                    if (bIntraday)
                    {
                        chartAdvGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "g";
                        chartAdvGraph.ChartAreas[1].AxisX2.LabelStyle.Format = "g";
                        chartAdvGraph.ChartAreas[2].AxisX.LabelStyle.Format = "g";
                    }
                    else
                    {
                        chartAdvGraph.ChartAreas[0].AxisX2.LabelStyle.Format = "dd-MM-yyyy";
                        chartAdvGraph.ChartAreas[1].AxisX2.LabelStyle.Format = "dd-MM-yyyy";
                        chartAdvGraph.ChartAreas[2].AxisX.LabelStyle.Format = "dd-MM-yyyy";
                    }


                    if (chartAdvGraph.Series.FindByName("Open") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("Open"));
                    }

                    chartAdvGraph.Series.Add("Open");

                    if (chartAdvGraph.Series.FindByName("High") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("High"));
                    }
                    chartAdvGraph.Series.Add("High");

                    if (chartAdvGraph.Series.FindByName("Low") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("Low"));
                    }
                    chartAdvGraph.Series.Add("Low");

                    if (chartAdvGraph.Series.FindByName("Close") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("Close"));
                    }
                    chartAdvGraph.Series.Add("Close");

                    if (chartAdvGraph.Series.FindByName("OHLC") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("OHLC"));
                    }
                    chartAdvGraph.Series.Add("OHLC");

                    if (chartAdvGraph.Series.FindByName("Volume") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("Volume"));
                    }
                    chartAdvGraph.Series.Add("Volume");


                    chartAdvGraph.Series["Open"].Enabled = false;
                    chartAdvGraph.Series["Open"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["Open"].YAxisType = AxisType.Primary;
                    chartAdvGraph.Series["Open"].ChartType = SeriesChartType.Line;
                    chartAdvGraph.Series["Open"].ChartArea = "chartarea2";
                    chartAdvGraph.Series["Open"].Legend = "legendAdvGraph";
                    chartAdvGraph.Series["Open"].LegendText = "Open";

                    chartAdvGraph.Series["High"].Enabled = false;
                    chartAdvGraph.Series["High"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["High"].YAxisType = AxisType.Primary;
                    chartAdvGraph.Series["High"].ChartType = SeriesChartType.Line;
                    chartAdvGraph.Series["High"].ChartArea = "chartarea2";
                    chartAdvGraph.Series["High"].Legend = "legendAdvGraph";
                    chartAdvGraph.Series["High"].LegendText = "High";

                    chartAdvGraph.Series["Low"].Enabled = false;
                    chartAdvGraph.Series["Low"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["Low"].YAxisType = AxisType.Primary;
                    chartAdvGraph.Series["Low"].ChartType = SeriesChartType.Line;
                    chartAdvGraph.Series["Low"].ChartArea = "chartarea2";
                    chartAdvGraph.Series["Low"].Legend = "legendAdvGraph";
                    chartAdvGraph.Series["Low"].LegendText = "Low";

                    chartAdvGraph.Series["Close"].Enabled = false;
                    chartAdvGraph.Series["Close"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["Close"].YAxisType = AxisType.Primary;
                    chartAdvGraph.Series["Close"].ChartType = SeriesChartType.Line;
                    chartAdvGraph.Series["Close"].ChartArea = "chartarea2";
                    chartAdvGraph.Series["Close"].Legend = "legendAdvGraph";
                    chartAdvGraph.Series["Close"].LegendText = "Close";

                    chartAdvGraph.Series["OHLC"].Enabled = true;
                    chartAdvGraph.Series["OHLC"].XAxisType = AxisType.Secondary;
                    chartAdvGraph.Series["OHLC"].YAxisType = AxisType.Primary;
                    chartAdvGraph.Series["OHLC"].ChartType = SeriesChartType.Candlestick;
                    chartAdvGraph.Series["OHLC"].ChartArea = "chartarea2";
                    chartAdvGraph.Series["OHLC"].Legend = "legendAdvGraph";
                    chartAdvGraph.Series["OHLC"].LegendText = "Candlestick";
                    chartAdvGraph.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Traingle";

                    chartAdvGraph.Series["Volume"].Enabled = true;
                    chartAdvGraph.Series["Volume"].XAxisType = AxisType.Primary;
                    chartAdvGraph.Series["Volume"].YAxisType = AxisType.Primary;
                    chartAdvGraph.Series["Volume"].ChartType = SeriesChartType.Column;
                    chartAdvGraph.Series["Volume"].ChartArea = "chartarea3";
                    chartAdvGraph.Series["Volume"].Legend = "legendAdvGraph";
                    chartAdvGraph.Series["Volume"].LegendText = "Volume";

                    if (bIntraday)
                    {
                        chartAdvGraph.Series["Open"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["Open"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["Open"].YValueMembers = "Open";
                        chartAdvGraph.Series["Open"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Open"].ToolTip = "Date:#VALX{g}; Open:#VALY";
                        chartAdvGraph.Series["Open"].PostBackValue = "Open," + selectedindexname + ",#VALX{g},#VALY";

                        chartAdvGraph.Series["High"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["High"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["High"].YValueMembers = "High";
                        chartAdvGraph.Series["High"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["High"].ToolTip = "Date:#VALX{g}; High:#VALY";
                        chartAdvGraph.Series["High"].PostBackValue = "High," + selectedindexname + ",#VALX{g},#VALY";

                        chartAdvGraph.Series["Low"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["Low"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["Low"].YValueMembers = "Low";
                        chartAdvGraph.Series["Low"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Low"].ToolTip = "Date:#VALX{g}; Low:#VALY";
                        chartAdvGraph.Series["Low"].PostBackValue = "Low," + selectedindexname + ",#VALX{g},#VALY";

                        chartAdvGraph.Series["Close"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["Close"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["Close"].YValueMembers = "CLOSE"; // "Price";
                        chartAdvGraph.Series["Close"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Close"].ToolTip = "Date:#VALX{g}; Close:#VALY";
                        chartAdvGraph.Series["Close"].PostBackValue = "Close," + selectedindexname + ",#VALX{g},#VALY";

                        chartAdvGraph.Series["OHLC"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["OHLC"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["OHLC"].YValueMembers = "High,Low,Open,Close";// "High,Low,Open,Price";
                        chartAdvGraph.Series["OHLC"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["OHLC"].ToolTip = "Date:#VALX{g}; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4";
                        chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + selectedindexname + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";

                        chartAdvGraph.Series["Volume"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["Volume"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["Volume"].YValueMembers = "Volume";
                        chartAdvGraph.Series["Volume"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Volume"].ToolTip = "Date:#VALX{g}; Volume:#VALY";
                        chartAdvGraph.Series["Volume"].PostBackValue = "Volume," + selectedindexname + ",#VALX{g},#VALY";
                    }
                    else
                    {
                        chartAdvGraph.Series["Open"].XValueMember = "TIMESTAMP";
                        chartAdvGraph.Series["Open"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["Open"].YValueMembers = "OPEN";
                        chartAdvGraph.Series["Open"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Open"].ToolTip = "Date:#VALX; Open:#VALY";
                        chartAdvGraph.Series["Open"].PostBackValue = "Open," + selectedindexname + ",#VALX,#VALY";

                        chartAdvGraph.Series["High"].XValueMember = "TIMESTAMP";
                        chartAdvGraph.Series["High"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["High"].YValueMembers = "HIGH";
                        chartAdvGraph.Series["High"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["High"].ToolTip = "Date:#VALX; High:#VALY";
                        chartAdvGraph.Series["High"].PostBackValue = "High," + selectedindexname + ",#VALX,#VALY";

                        chartAdvGraph.Series["Low"].XValueMember = "TIMESTAMP";
                        chartAdvGraph.Series["Low"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["Low"].YValueMembers = "LOW";
                        chartAdvGraph.Series["Low"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Low"].ToolTip = "Date:#VALX; Low:#VALY";
                        chartAdvGraph.Series["Low"].PostBackValue = "Low," + selectedindexname + ",#VALX,#VALY";

                        chartAdvGraph.Series["Close"].XValueMember = "TIMESTAMP";
                        chartAdvGraph.Series["Close"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["Close"].YValueMembers = "CLOSE";
                        chartAdvGraph.Series["Close"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Close"].ToolTip = "Date:#VALX; Close:#VALY";
                        chartAdvGraph.Series["Close"].PostBackValue = "Close," + selectedindexname + ",#VALX,#VALY";

                        chartAdvGraph.Series["OHLC"].XValueMember = "TIMESTAMP";
                        chartAdvGraph.Series["OHLC"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["OHLC"].YValueMembers = "HIGH,LOW,OPEN,CLOSE";
                        chartAdvGraph.Series["OHLC"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["OHLC"].ToolTip = "Date:#VALX; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4";
                        chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + selectedindexname + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";

                        chartAdvGraph.Series["Volume"].XValueMember = "TIMESTAMP";
                        chartAdvGraph.Series["Volume"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["Volume"].YValueMembers = "VOLUME";
                        chartAdvGraph.Series["Volume"].YValueType = ChartValueType.Double;
                        chartAdvGraph.Series["Volume"].ToolTip = "Date:#VALX; Volume:#VALY";
                        chartAdvGraph.Series["Volume"].PostBackValue = "Volume," + selectedindexname + ",#VALX,#VALY";
                    }

                    if (chartAdvGraph.Series.FindByName("PctChange") != null)
                    {
                        chartAdvGraph.Series.Remove(chartAdvGraph.Series.FindByName("PctChange"));
                    }
                    //chartAdvGraph.ChartAreas[0].Visible = false;
                    chartAdvGraph.ChartAreas[0].Visible = true;
                    chartAdvGraph.Series.Add("PctChange");
                    if (bIntraday)
                    {
                        //chartAdvGraph.ChartAreas[0].Visible = true;
                        //chartAdvGraph.Series.Add("PctChange");
                        chartAdvGraph.Series["PctChange"].Enabled = true;
                        chartAdvGraph.Series["PctChange"].XAxisType = AxisType.Secondary;
                        chartAdvGraph.Series["PctChange"].YAxisType = AxisType.Primary;
                        chartAdvGraph.Series["PctChange"].ChartType = SeriesChartType.Line;
                        chartAdvGraph.Series["PctChange"].ChartArea = "chartarea1";
                        chartAdvGraph.Series["PctChange"].Legend = "legendAdvGraph";
                        chartAdvGraph.Series["PctChange"].LegendText = "Change";

                        chartAdvGraph.Series.FindByName("PctChange").Enabled = true;
                        chartAdvGraph.Series["PctChange"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["PctChange"].XValueType = ChartValueType.DateTime;
                        chartAdvGraph.Series["PctChange"].YValuesPerPoint = 3;
                        chartAdvGraph.Series["PctChange"].YValueMembers = "CHANGE_PCT,CHANGE,PREV_CLOSE"; // "changePercent";
                        chartAdvGraph.Series["PctChange"].YValueType = ChartValueType.Auto;
                        chartAdvGraph.Series["PctChange"].ToolTip = "Date:#VALX{g}; Prev Close:#VALY3, Change%:#VALY1, Change:#VALY2";
                        chartAdvGraph.Series["PctChange"].PostBackValue = "PctChange," + selectedindexname + ",#VALX{g},#VALY1,#VALY2,#VALY3";

                        chartAdvGraph.Series["Volume"].Enabled = false;
                        chartAdvGraph.ChartAreas[2].Visible = false;
                    }
                    else
                    {
                        chartAdvGraph.Series["PctChange"].Enabled = true;
                        chartAdvGraph.Series["PctChange"].XAxisType = AxisType.Secondary;
                        chartAdvGraph.Series["PctChange"].YAxisType = AxisType.Primary;
                        chartAdvGraph.Series["PctChange"].ChartType = SeriesChartType.Line;
                        chartAdvGraph.Series["PctChange"].ChartArea = "chartarea1";
                        chartAdvGraph.Series["PctChange"].Legend = "legendAdvGraph";
                        chartAdvGraph.Series["PctChange"].LegendText = "Change";

                        chartAdvGraph.Series.FindByName("PctChange").Enabled = true;
                        chartAdvGraph.Series["PctChange"].XValueMember = "TIMESTAMP";// "latestDay";
                        chartAdvGraph.Series["PctChange"].XValueType = ChartValueType.Date;
                        chartAdvGraph.Series["PctChange"].YValuesPerPoint = 3;
                        chartAdvGraph.Series["PctChange"].YValueMembers = "CHANGE_PCT,CHANGE,PREV_CLOSE"; // "changePercent";
                        chartAdvGraph.Series["PctChange"].YValueType = ChartValueType.Auto;
                        chartAdvGraph.Series["PctChange"].ToolTip = "Date:#VALX; Prev Close:#VALY3, Change%:#VALY1, Change:#VALY2";
                        chartAdvGraph.Series["PctChange"].PostBackValue = "PctChange," + selectedindexname + ",#VALX,#VALY1,#VALY2,#VALY3";


                        chartAdvGraph.Series["Volume"].Enabled = true;
                        chartAdvGraph.ChartAreas[2].Visible = true;
                    }

                    chartAdvGraph.DataSource = dailyData;
                    chartAdvGraph.DataBind();
                }
            }

            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }

        public void buttonShowSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false, bArea2 = false;

            if (chartAdvGraph.Series.FindByName(graphName) != null)
            {
                chartAdvGraph.Series[graphName].Enabled = true;
            }

            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                if ((chartAdvGraph.Series.FindByName(Master.dropdownGraphList.Items[i].Value) != null) &&
                    (chartAdvGraph.Series[Master.dropdownGraphList.Items[i].Value].Enabled))
                {
                    if (Master.dropdownGraphList.Items[i].Value.Equals("Volume"))
                    {
                        bArea2 = true;
                    }
                    else if (Master.dropdownGraphList.Items[i].Value.Equals("PctChange"))
                    {
                        bArea0 = true;
                    }
                    else
                    {
                        bArea1 = true;
                    }
                }
            }
            chartAdvGraph.ChartAreas[0].Visible = bArea0;
            chartAdvGraph.ChartAreas[1].Visible = bArea1;
            chartAdvGraph.ChartAreas[2].Visible = bArea2;
        }
        public void buttonRemoveSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false, bArea2 = false;

            if (chartAdvGraph.Series.FindByName(graphName) != null)
            {
                chartAdvGraph.Series[graphName].Enabled = false;
            }

            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                if ((chartAdvGraph.Series.FindByName(Master.dropdownGraphList.Items[i].Value) != null) &&
                    (chartAdvGraph.Series[Master.dropdownGraphList.Items[i].Value].Enabled))
                {
                    if (Master.dropdownGraphList.Items[i].Value.Equals("Volume"))
                    {
                        bArea2 = true;
                    }
                    else if (Master.dropdownGraphList.Items[i].Value.Equals("PctChange"))
                    {
                        bArea0 = true;
                    }
                    else
                    {
                        bArea1 = true;
                    }
                }
            }
            chartAdvGraph.ChartAreas[0].Visible = bArea0;
            chartAdvGraph.ChartAreas[1].Visible = bArea1;
            chartAdvGraph.ChartAreas[2].Visible = bArea2;
        }
        public void buttonShowGraph_Click()
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);

            ShowIndexGraph();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);

        }
        protected void buttonShowGrid_Click()
        {
            GridViewData.Enabled = !GridViewData.Enabled;
            GridViewData.Visible = !GridViewData.Visible;
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["MAIN_DATA"];
            GridViewData.DataBind();
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
        protected void chartAdvGraph_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartAdvGraph.Annotations.Count > 0)
                    chartAdvGraph.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                seriesName = postBackValues[0];

                xDate = System.Convert.ToDateTime(postBackValues[2]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[3]);

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("Volume"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[2].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[2].Name;
                }
                else if (seriesName.Equals("PctChange"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[0].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    VA.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    ra.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[0].Name;
                }
                else
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX2;
                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX2;
                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX2;
                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                HA.ToolTip = postBackValues[3];
                chartAdvGraph.Annotations.Add(HA);

                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                VA.ToolTip = postBackValues[2];
                chartAdvGraph.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                if (seriesName.Equals("OHLC"))
                {   //high,low,open,close
                    //"OHLC," + symbol + "," + "#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Open:" + postBackValues[5] + "\n" + "Close:" + postBackValues[6];
                }
                else if (seriesName.Equals("PctChange"))
                {
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + "Prev Close:" + postBackValues[5] + "\n" + "Change %:" + postBackValues[3] + "\n" +
                                 "Change:" + postBackValues[4];

                }
                else if (seriesName.Equals("Portfolio"))
                {
                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase Price:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];
                    HA.ToolTip = "Close Price: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else
                {
                    //0-Volume, 1-Date, 2-Volume/Open/High/Low/Close
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + seriesName + ":" + postBackValues[3];
                }

                chartAdvGraph.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }
    }
}