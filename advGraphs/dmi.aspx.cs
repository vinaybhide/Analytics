using Antlr.Runtime.Misc;
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

namespace Analytics
{
    public partial class dmi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            Master.OnDoEventToggleParameters += new complexgraphs.DoEventToggleParameters(buttonShowHideParam_Click);
            Master.buttonShowHideParam.Visible = true;
            //this.Title = "Daily Price Graph";
            if (Session["EMAILID"] != null)
            {
                if ((Request.QueryString["symbol"] != null) && (Request.QueryString["exchange"] != null) &&
                    (Request.QueryString["seriestype"] != null) && (Request.QueryString["outputsize"] != null) &&
                    (Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null))
                {
                    this.Title = "Trend Direction graph : " + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();

                    if (!IsPostBack)
                    {
                        ViewState["FromDate"] = null;
                        ViewState["ToDate"] = null;
                        ViewState["FetchedData"] = null;
                        ViewState["VALUATION_TABLE"] = null;
                        ViewState["SENSEX"] = null;
                        ViewState["NIFTY50"] = null;

                        fillLinesCheckBoxes();
                        fillDesc();

                        ddl_Outputsize.SelectedValue = Request.QueryString["outputsize"].ToString();
                        ddl_SeriesType.SelectedValue = Request.QueryString["seriestype"].ToString();
                        ddl_Interval.SelectedValue = Request.QueryString["interval"].ToString();
                        textboxPeriod.Text = Request.QueryString["period"];
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);

                    ShowGraph();
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartAdvGraph.Visible = true;
                        chartAdvGraph.Width = int.Parse(Master.panelWidth.Value);
                        chartAdvGraph.Height = int.Parse(Master.panelHeight.Value);
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
            //Master.checkboxlistLines.Visible = false;
            //return;
            Master.checkboxlistLines.Visible = true;
            ListItem li = new ListItem("ADX", "ADX");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("DX", "DX");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("-DMI", "MINUS_DM");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("+DMI", "PLUS_DM");
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
            if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            {
                if (Request.QueryString["symbol"] != null)
                {
                    li = new ListItem("Valuation :" + Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString(),
                        Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString());
                    li.Selected = true;
                    Master.checkboxlistLines.Items.Add(li);
                }
            }

            li = new ListItem("BSE SENSEX", "^BSESN");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("NIFTY 50", "^NSEI");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);

        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("The directional movement indicator(DMI) is a valuable tool for assessing price direction and strength.");
            Master.bulletedlistDesc.Items.Add("The DMI is especially useful for trend trading strategies because it differentiates between strong and " +
                "weak trends, allowing the trader to enter only the ones with real momentum.");
            Master.bulletedlistDesc.Items.Add("DMI tells you when to be long or short.");
            Master.bulletedlistDesc.Items.Add("DMI comprises of two lines, +DMI & -DMI.The line which is on top is referred as dominant DMI." +
                "The dominant DMI is stronger and more likely to predict the direction of price. For the buyers and sellers to change dominance, " +
                "the lines must cross over.");
            Master.bulletedlistDesc.Items.Add("The + DMI generally moves in sync with price, which means the + DMI rises when price rises, " +
                "and it falls when price falls. It is important to note that the - DMI behaves in the opposite manner and moves counter - directional " +
                "to price. The - DMI rises when price falls, and it falls when price rises.");
            Master.bulletedlistDesc.Items.Add("Reading directional signals is easy.");
            Master.bulletedlistDesc.Items.Add("When the + DMI is dominant and rising, price direction is up.");
            Master.bulletedlistDesc.Items.Add("When the - DMI is dominant and rising, price direction is down.");
            Master.bulletedlistDesc.Items.Add("But the strength of price must also be considered. DMI strength ranges from a low of 0 to a high of 100. " +
                "The higher the DMI value, the stronger the prices swing.");
            Master.bulletedlistDesc.Items.Add("DMI values over 25 mean price is directionally strong. DMI values under 25 mean price is directionally weak.");
            Master.bulletedlistDesc.Items.Add("When the buyers are stronger than the sellers, the + DMI peaks will be above 25 and the - DMI peaks will be below 25. This is seen in a strong uptrend.But when the sellers are stronger than the buyers, the - DMI peaks will be above 25 and the + DMI peaks will be below 25.In this case, the trend will be down.");
            Master.bulletedlistDesc.Items.Add("\n");
            Master.bulletedlistDesc.Items.Add("ADX: The Trend Strength Indicator. The average directional index (ADX) is used to determine when the price is trending strongly");
            Master.bulletedlistDesc.Items.Add("ADX values help traders identify the strongest and most profitable trends to trade. The values are also important for distinguishing between trending and non - trending conditions");
            Master.bulletedlistDesc.Items.Add("Many traders will use ADX readings above 25 to suggest that the trend is strong enough for trend.Conversely, when ADX is below 25, many will avoid trend");
            Master.bulletedlistDesc.Items.Add("0 - 25-- > Absent or Weak Trend");
            Master.bulletedlistDesc.Items.Add("25 - 50-- > Strong Trend");
            Master.bulletedlistDesc.Items.Add("50 - 75-- > Very Strong Trend");
            Master.bulletedlistDesc.Items.Add("75 - 100-- > Extremely Strong Trend");
            Master.bulletedlistDesc.Items.Add("The direction of the ADX line is important for reading trend strength.When the ADX line is rising, trend strength is " +
                                   "increasing, and the price moves in the direction of the trend.When the line is falling, trend strength is decreasing," +
                                   "and the price enters a period of retracement or consolidation");

        }

        public void FillData()
        {
            DataTable tempData = null;
            DataTable dailyData = null;
            DataTable sensexTable = null;
            DataTable niftyTable = null;
            DataRow[] filteredRows = null;
            string expression = "";

            StockManager stockManager = new StockManager();
            string fromDate = null;

            string symbol = Request.QueryString["symbol"].ToString();
            string exchange = Request.QueryString["exchange"].ToString();

            string seriestype = ddl_SeriesType.SelectedValue;
            string outputsize = ddl_Outputsize.SelectedValue;
            int period = Int32.Parse(textboxPeriod.Text);
            string interval = ddl_Interval.SelectedValue;

            ViewState["FromDate"] = Master.textboxFromDate.Text;

            if (ViewState["FromDate"] != null)
                fromDate = ViewState["FromDate"].ToString();

            if ((fromDate != null) && (fromDate.Equals(string.Empty) == false))
            {
                expression = "TIMESTAMP >= '" + fromDate + "'";
            }

            //if we were called from portfolio page then get the portfolio data for selected scheme
            //if (Request.QueryString["schemecode"] != null)
            if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
            {
                //if ((ddlShowHidePortfolio.SelectedIndex == 0) && ((ViewState["VALUATION_TABLE"] == null) || (((DataTable)ViewState["VALUATION_TABLE"]).Rows.Count == 0)))
                if ((ViewState["VALUATION_TABLE"] == null) || (((DataTable)ViewState["VALUATION_TABLE"]).Rows.Count == 0))
                {
                    tempData = stockManager.GetPortfolio_ValuationLineGraph(Session["STOCKPORTFOLIOMASTERROWID"].ToString());
                    if (expression == string.Empty)
                    {
                        expression = "SYMBOL = '" + symbol + "'";
                    }
                    else
                    {
                        expression += " and SYMBOL = '" + symbol + "'";
                    }
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                    {
                        ViewState["VALUATION_TABLE"] = (DataTable)filteredRows.CopyToDataTable();
                    }
                }
            }

            if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
            {
                dailyData = stockManager.getADX_DX_DM_DI_DataTableFromDaily(symbol, exchange, seriestype, outputsize, time_interval: interval,
                                fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate, period: period.ToString());
                if (dailyData != null)
                {
                    ViewState["FetchedData"] = dailyData;
                }
            }

            if ((Master.checkboxlistLines.Items.FindByValue("^BSESN") != null) && (Master.checkboxlistLines.Items.FindByValue("^BSESN").Selected))
            {
                if ((ViewState["SENSEX"] == null) || (((DataTable)ViewState["SENSEX"]).Rows.Count <= 0))
                {
                    sensexTable = stockManager.GetStockPriceData("^BSESN",
                          fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                    ViewState["SENSEX"] = sensexTable;
                }
            }
            if ((Master.checkboxlistLines.Items.FindByValue("^NSEI") != null) && (Master.checkboxlistLines.Items.FindByValue("^NSEI").Selected))
            {
                if ((ViewState["NIFTY50"] == null) || (((DataTable)ViewState["NIFTY50"]).Rows.Count <= 0))
                {
                    niftyTable = stockManager.GetStockPriceData("^NSEI",
                          fromDate: ((fromDate == null) || (fromDate.Equals(""))) ? null : fromDate);
                    ViewState["NIFTY50"] = niftyTable;
                }
            }
        }
        public void ShowGraph()
        {
            DataTable scriptData = null, valuationTable = null, sensexTable = null, niftyTable = null;
            int portfolioTxnNumber = 1;
            Series tempSeries = null;

            string symbol = Request.QueryString["symbol"].ToString() + "." + Request.QueryString["exchange"].ToString();

            try
            {
                FillData();

                scriptData = (DataTable)ViewState["FetchedData"];
                valuationTable = (DataTable)ViewState["VALUATION_TABLE"];
                sensexTable = (DataTable)ViewState["SENSEX"];
                niftyTable = (DataTable)ViewState["NIFTY50"];

                GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                GridViewData.DataBind();

                if (scriptData != null)
                {
                    chartAdvGraph.DataSource = scriptData;
                    chartAdvGraph.DataBind();
                    if (chartAdvGraph.Series.FindByName("Open") != null)
                    {
                        chartAdvGraph.Series["Open"].PostBackValue = "Open," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("High") != null)
                    {
                        chartAdvGraph.Series["High"].PostBackValue = "High," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("Low") != null)
                    {
                        chartAdvGraph.Series["Low"].PostBackValue = "Low," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("Close") != null)
                    {
                        chartAdvGraph.Series["Close"].PostBackValue = "Close," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("OHLC") != null)
                    {
                        chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + symbol + "," + "#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    if (chartAdvGraph.Series.FindByName("ADX") != null)
                    {
                        chartAdvGraph.Series["ADX"].PostBackValue = "ADX-" + textboxPeriod.Text + "," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("MINUS_DM") != null)
                    {
                        chartAdvGraph.Series["MINUS_DM"].PostBackValue = "MINUS_DM-" + textboxPeriod.Text + "," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("PLUS_DM") != null)
                    {
                        chartAdvGraph.Series["PLUS_DM"].PostBackValue = "PLUS_DM-" + textboxPeriod.Text + "," + symbol + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("DX") != null)
                    {
                        chartAdvGraph.Series["DX"].PostBackValue = "DX-" + textboxPeriod.Text + "," + symbol + "," + "#VALX,#VALY";
                    }
                }
                if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    if (chartAdvGraph.Series.FindByName(symbol) == null)
                    {
                        chartAdvGraph.Series.Add(symbol);

                        chartAdvGraph.Series[symbol].Name = symbol;
                        (chartAdvGraph.Series[symbol]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartAdvGraph.Series[symbol]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                        chartAdvGraph.Series[symbol].Legend = chartAdvGraph.Legends[0].Name;

                        (chartAdvGraph.Series[symbol]).XAxisType = AxisType.Secondary;
                        (chartAdvGraph.Series[symbol]).YAxisType = AxisType.Primary;

                        (chartAdvGraph.Series[symbol]).XValueMember = "TIMESTAMP";
                        (chartAdvGraph.Series[symbol]).XValueType = ChartValueType.Date;
                        (chartAdvGraph.Series[symbol]).YValueMembers = "CLOSE";
                        (chartAdvGraph.Series[symbol]).YValueType = ChartValueType.Double;

                        chartAdvGraph.Series[symbol].LegendText = symbol;
                        chartAdvGraph.Series[symbol].LegendToolTip = symbol;
                        chartAdvGraph.Series[symbol].ToolTip = symbol + ":  Date:#VALX; CLOSE:#VALY (Click to see details)";
                        chartAdvGraph.Series[symbol].PostBackValue = symbol + "," + "#VALX,#VALY";
                    }
                    (chartAdvGraph.Series[symbol]).Points.Clear();
                    for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                    {
                        //(chartAdvGraph.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                        (chartAdvGraph.Series[symbol]).Points.AddXY(valuationTable.Rows[rownum]["TIMESTAMP"], valuationTable.Rows[rownum]["CLOSE"]);
                        (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].PostBackValue =
                                            "Portfolio," +
                                            valuationTable.Rows[rownum]["SYMBOL"] + "," + valuationTable.Rows[rownum]["TIMESTAMP"] + "," +
                                            valuationTable.Rows[rownum]["CLOSE"] + "," +
                                            valuationTable.Rows[rownum]["PURCHASE_DATE"] + "," + valuationTable.Rows[rownum]["PURCHASE_PRICE"] + "," +
                                            valuationTable.Rows[rownum]["PURCHASE_QTY"] + "," +
                                            valuationTable.Rows[rownum]["INVESTMENT_COST"] + "," + valuationTable.Rows[rownum]["CumulativeQty"] + "," +
                                            valuationTable.Rows[rownum]["CumulativeCost"] + "," + valuationTable.Rows[rownum]["CumulativeValue"];

                        //if (valuationTable.Rows[rownum]["PURCHASE_DATE"].ToString().Equals(valuationTable.Rows[rownum]["TIMESTAMP"].ToString())) // || ((rownum + 1) == valuationTable.Rows.Count))
                        if ((valuationTable.Rows[rownum]["PORTFOLIO_FLAG"].Equals("True")) || ((rownum + 1) == valuationTable.Rows.Count))
                        {
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerSize = 11;
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                            (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                        }
                    }
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerSize = 10;
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].MarkerColor = Color.Black;
                    (chartAdvGraph.Series[symbol]).Points[(chartAdvGraph.Series[symbol]).Points.Count - 1].ToolTip = "Click to see latest valuation";

                }
                else
                {
                    tempSeries = chartAdvGraph.Series.FindByName(symbol);
                    if (tempSeries != null)
                        chartAdvGraph.Series.Remove(tempSeries);
                }

                if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
                {
                    if (chartAdvGraph.Series.FindByName("^BSESN") == null)
                    {
                        chartAdvGraph.Series.Add("^BSESN");

                        chartAdvGraph.Series["^BSESN"].Name = "^BSESN";
                        (chartAdvGraph.Series["^BSESN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartAdvGraph.Series["^BSESN"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                        chartAdvGraph.Series["^BSESN"].Legend = chartAdvGraph.Legends[0].Name;
                        chartAdvGraph.Series["^BSESN"].LegendText = "BSE SENSEX";
                        chartAdvGraph.Series["^BSESN"].LegendToolTip = "BSE SENSEX";

                        (chartAdvGraph.Series["^BSESN"]).YValuesPerPoint = 4;

                        chartAdvGraph.Series["^BSESN"].XAxisType = AxisType.Secondary;
                        chartAdvGraph.Series["^BSESN"].YAxisType = AxisType.Primary;

                        chartAdvGraph.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^BSESN"].PostBackValue = "^BSESN," + "SENSEX" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    chartAdvGraph.Series["^BSESN"].Points.Clear();
                    (chartAdvGraph.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                }
                else
                {
                    tempSeries = chartAdvGraph.Series.FindByName("^BSESN");
                    if (tempSeries != null)
                        chartAdvGraph.Series.Remove(tempSeries);
                }

                if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
                {
                    if (chartAdvGraph.Series.FindByName("^NSEI") == null)
                    {
                        chartAdvGraph.Series.Add("^NSEI");

                        chartAdvGraph.Series["^NSEI"].Name = "^NSEI";
                        (chartAdvGraph.Series["^NSEI"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartAdvGraph.Series["^NSEI"]).ChartArea = chartAdvGraph.ChartAreas[0].Name;

                        chartAdvGraph.Series["^NSEI"].Legend = chartAdvGraph.Legends[0].Name;
                        chartAdvGraph.Series["^NSEI"].LegendText = "NIFTY 50";
                        chartAdvGraph.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                        (chartAdvGraph.Series["^NSEI"]).YValuesPerPoint = 4;

                        chartAdvGraph.Series["^NSEI"].XAxisType = AxisType.Secondary;
                        chartAdvGraph.Series["^NSEI"].YAxisType = AxisType.Primary;

                        chartAdvGraph.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                        chartAdvGraph.Series["^NSEI"].PostBackValue = "^NSEI," + "NIFTY50" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    (chartAdvGraph.Series["^NSEI"]).Points.Clear();
                    (chartAdvGraph.Series["^NSEI"]).Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,HIGH,LOW,CLOSE");
                }
                else
                {
                    tempSeries = chartAdvGraph.Series.FindByName("^NSEI");
                    if (tempSeries != null)
                        chartAdvGraph.Series.Remove(tempSeries);
                }

                foreach (ListItem item in Master.checkboxlistLines.Items)
                {
                    if (chartAdvGraph.Series.FindByName(item.Value) != null)
                    {
                        chartAdvGraph.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartAdvGraph.Annotations.FindByName(item.Value) != null)
                                chartAdvGraph.Annotations.Clear();
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

                if (seriesName.Contains("DX"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[2].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[2].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[2].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[2].Name;
                }
                else if (seriesName.Contains("DM"))
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[1].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[1].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    VA.AxisX = chartAdvGraph.ChartAreas[1].AxisX;
                    ra.AxisX = chartAdvGraph.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[1].Name;
                }
                else
                {
                    HA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    VA.AxisY = chartAdvGraph.ChartAreas[0].AxisY;
                    ra.AxisY = chartAdvGraph.ChartAreas[0].AxisY;

                    HA.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    VA.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    ra.AxisX = chartAdvGraph.ChartAreas[0].AxisX2;
                    HA.ClipToChartArea = chartAdvGraph.ChartAreas[0].Name;
                }

                //HA.Name = seriesName;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                HA.ToolTip = postBackValues[3];
                chartAdvGraph.Annotations.Add(HA);

                //VA.Name = seriesName;
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
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                if (seriesName.Equals("OHLC"))
                {   //high,low,open,close
                    //"OHLC," + symbol + "," + "#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + "Open:" + postBackValues[5] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[6];
                }
                else if (seriesName.Equals("Portfolio"))
                {
                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase Price:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];

                    HA.ToolTip = "Close Price: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("^BSESN") || seriesName.Equals("^NSEI"))
                {
                    ra.Text = seriesName + "\n" + "Date:" + postBackValues[2] + "\n" + "Close:" + postBackValues[3] + "\n" + "Open:" + postBackValues[4] + "\n" +
                                "High:" + postBackValues[5] + "\n" + "Low:" + postBackValues[6];
                }
                else
                {
                    //0-Volume, 1-Date, 2-Volume/Open/High/Low/Close
                    ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + seriesName + ":" + postBackValues[3];
                }
                //ra.SmartLabelStyle = sl;

                chartAdvGraph.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }
        void buttonShowGraph_Click()
        {
            ViewState["FetchedData"] = null;
            ViewState["SENSEX"] = null;
            ViewState["NIFTY50"] = null;
            ViewState["VALUATION_TABLE"] = null;
            ShowGraph();
        }
        protected void buttonShowHideParam_Click()
        {
            panelParam.Visible = !panelParam.Visible;
        }

        void buttonShowGrid_Click()
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                GridViewData.Visible = true;
                Master.buttonShowGrid.Text = "Hide Raw Data";
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