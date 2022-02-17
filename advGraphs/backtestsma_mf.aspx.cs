using DataAccessLayer;
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

namespace Analytics.advGraphs
{
    public partial class backtestsma_mf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "doHourglass", "doHourglass();", true);

                Master.OnDoEventShowGraph += new advancegraphs.DoEventShowGraph(buttonShowGraph_Click);
                Master.OnDoEventShowGrid += new advancegraphs.DoEventShowGrid(buttonShowGrid_Click);
                //Master.OnDoEventToggleDesc += new advancegraphs.DoEventToggleDesc(buttonDesc_Click);
                //Master.OnDoEventToggleParameters += new advancegraphs.DoEventToggleParameters(buttonShowHideParam_Click);
                Master.OnDoEventRemoveSelectedIndicatorGraph += new advancegraphs.DoEventRemoveSelectedIndicatorGraph(buttonRemoveSelectedIndicatorGraph_Click);
                Master.OnDoEventShowSelectedIndicatorGraph += new advancegraphs.DoEventShowSelectedIndicatorGraph(buttonShowSelectedIndicatorGraph_Click);
                if (!IsPostBack)
                {
                    ViewState["BACKTEST_DATA"] = null;
                    ViewState["VALUATION_DATA"] = null;

                    ManagePanels();

                    fillDesc();

                    //if caller to this page may send us stock in querystring for which back test needs to be done, else user will select stock on this page
                    //first load stock & portfolio list
                    Master.LoadFundHouseList();
                    Master.LoadPortfolioMF();

                    if (Request.QueryString["smasmall"] != null)
                    {
                        Master.textboxSMASmallPeriod.Text = Request.QueryString["smasmall"].ToString();
                    }
                    if (Request.QueryString["smalong"] != null)
                    {
                        Master.textboxSMALongPeriod.Text = Request.QueryString["smalong"].ToString();
                    }
                    if (Request.QueryString["buyspan"] != null)
                    {
                        Master.textboxBuySpan.Text = Request.QueryString["buyspan"].ToString();
                    }
                    if (Request.QueryString["sellspan"] != null)
                    {
                        Master.textboxSellSpan.Text = Request.QueryString["sellspan"].ToString();
                    }
                    if (Request.QueryString["simulationqty"] != null)
                    {
                        Master.textboxSimulationQty.Text = Request.QueryString["simulationqty"].ToString();
                    }
                    if (Request.QueryString["regressiontype"] != null)
                    {
                        Master.ddlRegressionType.SelectedValue = Request.QueryString["regressiontype"];
                    }
                    if (Request.QueryString["forecastperiod"] != null)
                    {
                        Master.textboxForecastPeriod.Text = Request.QueryString["forecastperiod"];
                    }

                    if (Request.QueryString["fromdate"] != null)
                    {
                        Master.textboxFromDate.Text = System.Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Master.textboxFromDate.Text = DateTime.Today.AddYears(-3).ToString("yyyy-MM-dd");
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


                    if ((Request.QueryString["schemecode"] != null) && (Request.QueryString["fundhousecode"] != null))
                    {
                        Master.dropdownFundhouseList.SelectedValue = Request.QueryString["fundhousecode"].ToString();
                        if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
                        {
                            //if we were called frm portfolio page then select current portfolio
                            Master.dropdownPortfolioMF.SelectedValue = Session["MFPORTFOLIOMASTERROWID"].ToString();
                            Master.LoadPortfolioFundNameList();
                        }
                        else
                        {
                            Master.LoadFundNameList();
                        }
                        Master.textbox_selectedSchemeCode.Text = Request.QueryString["schemecode"].ToString();


                        Master.dropdownFundNameList.SelectedValue = Master.textbox_selectedSchemeCode.Text;

                        Master.textbox_selectedFundName.Text = Master.dropdownFundNameList.Items[Master.dropdownFundNameList.SelectedIndex].Text;

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);

                        //now show the backtest graph
                        ShowBackTestGraph();
                        ShowForecast();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);

                    }

                    //since we need to handle case of portfolio we are calling it after master page selected fundname is populated & selected
                    fillGraphList();

                }
                this.Title = "BackTest Graph for: " + Master.textbox_selectedFundName.Text;

                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    //ShowGraph(scriptName);
                    chartBackTest.Visible = true;
                    chartBackTest.Width = int.Parse(Master.panelWidth.Value);
                    chartBackTest.Height = int.Parse(Master.panelHeight.Value);
                }
                ClientScript.RegisterStartupScript(this.GetType(), "resetCursor", "resetCursor();", true);
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Server.Transfer("~/Default.aspx");
                //Response.Redirect("~/Default.aspx");
            }
        }

        public void ManagePanels()
        {
            Master.InitializePanels(bStocksGraph: false);
            //now handle this graph specific panels
            Master.panelMainParam.Enabled = true;
            Master.panelParamStockBackTest.Enabled = true;
            Master.panelMainParam.Visible = true;
            Master.panelParamStockBackTest.Visible = true;
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("Back test example: Go long on 100 stocks (i.e. buy 100 stocks), when the short term moving average crosses above the long term moving average. This is known as golden cross.");
            Master.bulletedlistDesc.Items.Add("Sell the stock a few days later.For instance, we will keep the stock 20 days and then sell them. Compute the profit");
        }

        public void fillGraphList()
        {
            Master.dropdownGraphList.Visible = true;

            Master.dropdownGraphList.Items.Clear();

            ListItem li;

            li = new ListItem("Daily Price", "Daily");
            li.Selected = true;
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("SMA Small", "SMA_SMALL");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("SMA Long", "SMA_LONG");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("Future_Price", "Future_Price");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("SMA_SMALL_Future", "SMA_SMALL_Future");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("SMA_LONG_Future", "SMA_LONG_Future");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("Approximation_Error", "Approximation_Error");
            Master.dropdownGraphList.Items.Add(li);

            if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
            {
                if ((Request.QueryString["schemecode"] != null) && (Request.QueryString["fundhousecode"] != null))
                {
                    li = new ListItem("Valuation :" + Master.textbox_selectedFundName.Text, Request.QueryString["schemecode"].ToString());
                    //Request.QueryString["symbol"].ToString());
                    Master.dropdownGraphList.Items.Add(li);
                }
            }

            li = new ListItem("BSE SENSEX", "^BSESN");
            Master.dropdownGraphList.Items.Add(li);

            li = new ListItem("NIFTY 50", "^NSEI");
            Master.dropdownGraphList.Items.Add(li);
        }

        public void ShowBackTestGraph()
        {
            DataManager dataMgr = new DataManager();
            int smallperiod = Int32.Parse(Master.textboxSMASmallPeriod.Text.ToString());
            int longperiod = Int32.Parse(Master.textboxSMALongPeriod.Text.ToString());
            int buySpan = Int32.Parse(Master.textboxBuySpan.Text.ToString());
            int sellSpan = Int32.Parse(Master.textboxSellSpan.Text.ToString());
            string schemecode = Master.textbox_selectedSchemeCode.Text;
            double simulationQty = double.Parse(Master.textboxSimulationQty.Text.ToString());

            string fromDate = Master.textboxFromDate.Text;
            string toDate = Master.textboxToDate.Text;
            try
            {
                DataTable dailysmaTable = dataMgr.getBacktestFromSMA(Int32.Parse(schemecode),
                                fromDate: fromDate, toDate: toDate,
                                smallPeriod: smallperiod, longPeriod: longperiod, buySpan: buySpan, sellSpan: sellSpan, simulationQty: simulationQty);
                if (chartBackTest.Annotations.Count > 0)
                    chartBackTest.Annotations.Clear();

                ViewState["BACKTEST_DATA"] = null;

                if ((dailysmaTable != null) && (dailysmaTable.Rows.Count > 0))
                {
                    gridviewBackTest.DataSource = dailysmaTable;
                    gridviewBackTest.DataBind();

                    ViewState["BACKTEST_DATA"] = dailysmaTable;

                    if (smallperiod > 0)
                    {
                        if (chartBackTest.Series.FindByName("SMA_SMALL") == null)
                        {
                            chartBackTest.Series.Add("SMA_SMALL");

                            chartBackTest.Series["SMA_SMALL"].Name = "SMA_SMALL";
                            chartBackTest.Series["SMA_SMALL"].ChartType = SeriesChartType.Line;
                            chartBackTest.Series["SMA_SMALL"].ChartArea = chartBackTest.ChartAreas[0].Name;
                            chartBackTest.Series["SMA_SMALL"].Legend = chartBackTest.Legends[0].Name;

                            chartBackTest.Series["SMA_SMALL"].XAxisType = AxisType.Secondary;
                            chartBackTest.Series["SMA_SMALL"].YAxisType = AxisType.Primary;
                        }

                        chartBackTest.Series["SMA_SMALL"].LegendText = "SMA Period: " + smallperiod;//Request.QueryString["smasmall"].ToString();
                        chartBackTest.Series["SMA_SMALL"].LegendToolTip = "SMA Period: " + smallperiod;//Request.QueryString["smasmall"].ToString();
                        chartBackTest.Series["SMA_SMALL"].ToolTip = "SMA Period: " + smallperiod + " : Date:#VALX; SMA:#VALY (Click to see details)";
                        chartBackTest.Series["SMA_SMALL"].PostBackValue = "SMA_SMALL," + smallperiod + ",#VALX,#VALY";

                        (chartBackTest.Series["SMA_SMALL"]).Points.Clear();

                        if (longperiod > 0)
                        {
                            if (chartBackTest.Series.FindByName("SMA_LONG") == null)
                            {
                                chartBackTest.Series.Add("SMA_LONG");

                                chartBackTest.Series["SMA_LONG"].Name = "SMA_LONG";
                                chartBackTest.Series["SMA_LONG"].ChartType = SeriesChartType.Line;
                                chartBackTest.Series["SMA_LONG"].ChartArea = chartBackTest.ChartAreas[0].Name;
                                chartBackTest.Series["SMA_LONG"].Legend = chartBackTest.Legends[0].Name;

                                chartBackTest.Series["SMA_LONG"].XAxisType = AxisType.Secondary;
                                chartBackTest.Series["SMA_LONG"].YAxisType = AxisType.Primary;
                            }
                            chartBackTest.Series["SMA_LONG"].LegendText = "SMA Period: " + longperiod; //Request.QueryString["smalong"].ToString();
                            chartBackTest.Series["SMA_LONG"].LegendToolTip = "SMA Period: " + longperiod; //Request.QueryString["smalong"].ToString();
                            chartBackTest.Series["SMA_LONG"].ToolTip = "SMA Period: " + longperiod + " : Date:#VALX; SMA:#VALY (Click to see details)";
                            chartBackTest.Series["SMA_LONG"].PostBackValue = "SMA_LONG," + longperiod + ",#VALX,#VALY";

                            (chartBackTest.Series["SMA_LONG"]).Points.Clear();
                        }

                        if (chartBackTest.Series.FindByName("Daily") == null)
                        {
                            chartBackTest.Series.Add("Daily");

                            chartBackTest.Series["Daily"].Name = "Daily";
                            (chartBackTest.Series["Daily"]).ChartType = SeriesChartType.Line;
                            (chartBackTest.Series["Daily"]).ChartArea = chartBackTest.ChartAreas[0].Name;
                            chartBackTest.Series["Daily"].Legend = chartBackTest.Legends[0].Name;

                            chartBackTest.Series["Daily"].XAxisType = AxisType.Secondary;
                            chartBackTest.Series["Daily"].YAxisType = AxisType.Primary;
                        }
                        chartBackTest.Series["Daily"].LegendText = "Daily CLOSE";
                        chartBackTest.Series["Daily"].LegendToolTip = "Daily CLOSE";
                        chartBackTest.Series["Daily"].ToolTip = "Daily price: " + " : Date:#VALX; Close:#VALY (Click to see details)";
                        chartBackTest.Series["Daily"].PostBackValue = "Daily" + ",#VALX,#VALY";

                        (chartBackTest.Series["Daily"]).Points.Clear();

                        for (int rownum = 0; rownum < dailysmaTable.Rows.Count; rownum++)
                        {
                            if (smallperiod > 0)
                            {
                                (chartBackTest.Series["SMA_SMALL"]).Points.AddXY(dailysmaTable.Rows[rownum]["NAVDATE"], dailysmaTable.Rows[rownum]["SMA_SMALL"]);
                                (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].PostBackValue =
                                                    "SMA_SMALL," +
                                                    dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," + dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
                                                    dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                                    "SMA Period: " + smallperiod + "," +
                                                    "SMA: ," + dailysmaTable.Rows[rownum]["SMA_SMALL"];
                            }
                            if (longperiod > 0)
                            {
                                (chartBackTest.Series["SMA_LONG"]).Points.AddXY(dailysmaTable.Rows[rownum]["NAVDATE"], dailysmaTable.Rows[rownum]["SMA_LONG"]);
                                (chartBackTest.Series["SMA_LONG"]).Points[(chartBackTest.Series["SMA_LONG"]).Points.Count - 1].PostBackValue =
                                                    "SMA_LONG," +
                                                    dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," + dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
                                                    dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                                    "SMA Period: " + longperiod + "," +
                                                    "SMA: ," + dailysmaTable.Rows[rownum]["SMA_LONG"];
                            }
                            (chartBackTest.Series["Daily"]).Points.AddXY(dailysmaTable.Rows[rownum]["NAVDATE"], dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"]);
                            (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].PostBackValue =
                                                "Daily," +
                                                dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," + dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
                                                dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                                dailysmaTable.Rows[rownum]["BUY_FLAG"] + "," +
                                                dailysmaTable.Rows[rownum]["SELL_FLAG"] + "," +
                                                dailysmaTable.Rows[rownum]["QUANTITY"] + "," +
                                                dailysmaTable.Rows[rownum]["BUY_COST"] + "," +
                                                dailysmaTable.Rows[rownum]["SELL_VALUE"] + "," +
                                                dailysmaTable.Rows[rownum]["PROFIT_LOSS"] + "," +
                                                dailysmaTable.Rows[rownum]["RESULT"];
                            //if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["BUY_FLAG"]) == true)
                            if (dailysmaTable.Rows[rownum]["CROSSOVER_FLAG"].ToString().Equals("X") == true)
                            {
                                //we just mark this point as cross over
                                (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].MarkerSize = 11;
                                (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Cross;
                                (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].MarkerColor = Color.Blue;
                                (chartBackTest.Series["SMA_SMALL"]).Points[(chartBackTest.Series["SMA_SMALL"]).Points.Count - 1].ToolTip = "Backtest Initiated on: " + "#VALX";
                            }

                            if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["BUY_FLAG"]) == true)
                            {
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerSize = 12;
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerColor = Color.Yellow;
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].ToolTip = "Buy Signal on: " + "#VALX, Buy Price: #VALY";
                            }
                            if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["SELL_FLAG"]) == true)
                            {
                                //we just mark this point as cross over
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerSize = 12;
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].MarkerColor = Color.Green;
                                (chartBackTest.Series["Daily"]).Points[(chartBackTest.Series["Daily"]).Points.Count - 1].ToolTip = "Sell Signal: " + dailysmaTable.Rows[rownum]["RESULT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }
        public void ShowForecast()
        {
            string symbol = Master.textbox_SelectedSymbol.Text;

            if ((chartBackTest.Series.FindByName("Daily") == null) || (chartBackTest.Series.FindByName("SMA_SMALL") == null) ||
                (chartBackTest.Series.FindByName("SMA_LONG") == null))
            {
                ShowBackTestGraph();
            }

            if (chartBackTest.Series.FindByName("Future_Price") == null)
            {
                chartBackTest.Series.Add("Future_Price");

                chartBackTest.Series["Future_Price"].Name = "Future_Price"; // "Portfolio";
                (chartBackTest.Series["Future_Price"]).ChartType = SeriesChartType.Line;
                (chartBackTest.Series["Future_Price"]).ChartArea = chartBackTest.ChartAreas[1].Name;
                chartBackTest.Series["Future_Price"].Legend = chartBackTest.Legends[0].Name;
                chartBackTest.Series["Future_Price"].LegendText = "Future_Price";

                chartBackTest.Series["Future_Price"].XAxisType = AxisType.Primary;
                chartBackTest.Series["Future_Price"].YAxisType = AxisType.Primary;

                //(chartBackTest.Series[symbol]).XValueType = ChartValueType.Date;
                //(chartBackTest.Series[symbol]).YValueType = ChartValueType.Double;
            }
            else
            {
                chartBackTest.Series["Future_Price"].Enabled = true;
            }

            if (chartBackTest.Series.FindByName("SMA_SMALL_Future") == null)
            {
                chartBackTest.Series.Add("SMA_SMALL_Future");

                chartBackTest.Series["SMA_SMALL_Future"].Name = "SMA_SMALL_Future"; // "Portfolio";
                (chartBackTest.Series["SMA_SMALL_Future"]).ChartType = SeriesChartType.Line;
                (chartBackTest.Series["SMA_SMALL_Future"]).ChartArea = chartBackTest.ChartAreas[1].Name;
                chartBackTest.Series["SMA_SMALL_Future"].Legend = chartBackTest.Legends[0].Name;
                chartBackTest.Series["SMA_SMALL_Future"].LegendText = "SMA_SMALL_Future";

                chartBackTest.Series["SMA_SMALL_Future"].XAxisType = AxisType.Primary;
                chartBackTest.Series["SMA_SMALL_Future"].YAxisType = AxisType.Primary;

                //(chartBackTest.Series["SMA_SMALL_Future"]).XValueType = ChartValueType.Date;
                //(chartBackTest.Series["SMA_SMALL_Future"]).YValueType = ChartValueType.Double;
            }
            else
            {
                chartBackTest.Series["SMA_SMALL_Future"].Enabled = true;
            }

            if (chartBackTest.Series.FindByName("SMA_LONG_Future") == null)
            {
                chartBackTest.Series.Add("SMA_LONG_Future");

                chartBackTest.Series["SMA_LONG_Future"].Name = "SMA_LONG_Future"; // "Portfolio";
                (chartBackTest.Series["SMA_LONG_Future"]).ChartType = SeriesChartType.Line;
                (chartBackTest.Series["SMA_LONG_Future"]).ChartArea = chartBackTest.ChartAreas[1].Name;
                chartBackTest.Series["SMA_LONG_Future"].Legend = chartBackTest.Legends[0].Name;
                chartBackTest.Series["SMA_LONG_Future"].LegendText = "SMA_LONG_Future";

                chartBackTest.Series["SMA_LONG_Future"].XAxisType = AxisType.Primary;
                chartBackTest.Series["SMA_LONG_Future"].YAxisType = AxisType.Primary;

                //(chartBackTest.Series["SMA_LONG_Future"]).XValueType = ChartValueType.Date;
                //(chartBackTest.Series["SMA_LONG_Future"]).YValueType = ChartValueType.Double;
            }
            else
            {
                chartBackTest.Series["SMA_SMALL_Future"].Enabled = true;
            }

            if (chartBackTest.Series.FindByName("Approximation_Error") == null)
            {
                chartBackTest.Series.Add("Approximation_Error");

                chartBackTest.Series["Approximation_Error"].Name = "Approximation_Error"; // "Portfolio";
                (chartBackTest.Series["Approximation_Error"]).ChartType = SeriesChartType.Range;
                (chartBackTest.Series["Approximation_Error"]).ChartArea = chartBackTest.ChartAreas[2].Name;
                chartBackTest.Series["Approximation_Error"].Legend = chartBackTest.Legends[0].Name;
                chartBackTest.Series["Approximation_Error"].LegendText = "Approximation_Error";

                chartBackTest.Series["Approximation_Error"].XAxisType = AxisType.Primary;
                chartBackTest.Series["Approximation_Error"].YAxisType = AxisType.Primary;

                //(chartBackTest.Series["Approximation_Error"]).XValueType = ChartValueType.Date;
                //(chartBackTest.Series["Approximation_Error"]).YValueType = ChartValueType.Double;
            }
            else
            {
                chartBackTest.Series["SMA_SMALL_Future"].Enabled = true;
            }

            chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, Master.ddlRegressionType.SelectedValue + "," + Master.textboxForecastPeriod.Text + ",true,true", "Daily:Y",
                                                            "Future_Price:Y,Approximation_Error:Y,Approximation_Error:Y2");
            chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, Master.ddlRegressionType.SelectedValue + "," + Master.textboxForecastPeriod.Text + ",false,false", "SMA_SMALL:Y",
                                                            "SMA_SMALL_Future:Y");
            chartBackTest.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, Master.ddlRegressionType.SelectedValue + "," + Master.textboxForecastPeriod.Text + ",false,false", "SMA_LONG:Y",
                                                            "SMA_LONG_Future:Y");
            chartBackTest.Series["Future_Price"].Points[chartBackTest.Series["Daily"].Points.Count - 1].MarkerStyle = MarkerStyle.Triangle;
            chartBackTest.Series["Future_Price"].Points[chartBackTest.Series["Daily"].Points.Count - 1].MarkerSize = 12;
            chartBackTest.Series["Future_Price"].Points[chartBackTest.Series["Daily"].Points.Count - 1].ToolTip = "Last actual close";
            chartBackTest.Series["Future_Price"].PostBackValue = "Future_Price," + symbol + ",#VALX,#VALY{0.##}";
            chartBackTest.Series["Future_Price"].ToolTip = "Date: #VALX; Future Price: #VALY{0.##}";
            chartBackTest.Series["SMA_SMALL_Future"].PostBackValue = "SMA_SMALL_Future," + symbol + ",#VALX,#VALY{0.##}";
            chartBackTest.Series["SMA_SMALL_Future"].ToolTip = "Date: #VALX; SMA_SMALL_Future: #VALY{0.##}";
            chartBackTest.Series["SMA_LONG_Future"].PostBackValue = "SMA_LONG_Future," + symbol + ",#VALX,#VALY{0.##}";
            chartBackTest.Series["SMA_LONG_Future"].ToolTip = "Date: #VALX; SMA_LONG_Future: #VALY{0.##}";
            chartBackTest.Series["Approximation_Error"].PostBackValue = "Approximation_Error," + symbol + ",#VALX,#VALY1{0.##},#VALY2{0.##}";
            chartBackTest.Series["Approximation_Error"].ToolTip = "Date: #VALX; Approximation_Error: #VALY1{0.##}, Forecast error: #VALY2{0.##}";
        }
        public void ShowSymbolValuation()
        {
            string schemecode = Master.textbox_selectedSchemeCode.Text;
            string expression = "";
            DataTable tempData = null, valuationTable = null;
            DataRow[] filteredRows = null;
            int portfolioTxnNumber = 1;

            DataManager dataMgr = new DataManager();
            ViewState["VALUATION_DATA"] = null;

            //if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
            if(Master.dropdownPortfolioMF.SelectedIndex > 0)
            {
                //tempData = dataMgr.GetValuationLineGraph(Session["MFPORTFOLIOMASTERROWID"].ToString());
                tempData = dataMgr.GetValuationLineGraph(Master.dropdownPortfolioMF.SelectedValue);

                expression = "SCHEME_CODE = '" + schemecode + "'";

                filteredRows = tempData.Select(expression);
                if ((filteredRows != null) && (filteredRows.Length > 0))
                {
                    valuationTable = (DataTable)filteredRows.CopyToDataTable();
                }
            }

            if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
            {
                gridviewPortfolioValuation.DataSource = valuationTable;
                gridviewPortfolioValuation.DataBind();

                ViewState["VALUATION_DATA"] = valuationTable;

                if (chartBackTest.Series.FindByName(schemecode) == null)
                {
                    chartBackTest.Series.Add(schemecode);

                    chartBackTest.Series[schemecode].Name = schemecode; // "Portfolio";
                    chartBackTest.Series[schemecode].ChartType = SeriesChartType.Line;
                    chartBackTest.Series[schemecode].ChartArea = chartBackTest.ChartAreas[0].Name;
                    chartBackTest.Series[schemecode].Legend = chartBackTest.Legends[0].Name;

                    chartBackTest.Series[schemecode].XAxisType = AxisType.Secondary;
                    chartBackTest.Series[schemecode].YAxisType = AxisType.Primary;

                    chartBackTest.Series[schemecode].XValueType = ChartValueType.Date;
                    chartBackTest.Series[schemecode].YValueType = ChartValueType.Double;
                }
                else
                {
                    chartBackTest.Series[schemecode].Enabled = true;
                }
                chartBackTest.Series[schemecode].LegendText = valuationTable.Rows[0]["SCHEME_NAME"].ToString();
                chartBackTest.Series[schemecode].LegendToolTip = valuationTable.Rows[0]["SCHEME_NAME"].ToString();
                chartBackTest.Series[schemecode].ToolTip = "Portfolio: " + "Date:#VALX; Close:#VALY (Click to see details)";
                chartBackTest.Series[schemecode].PostBackValue = "Portfolio:" + ",#VALX,#VALY";

                chartBackTest.Series[schemecode].Points.Clear();

                for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                {
                    //(chartBackTest.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                    chartBackTest.Series[schemecode].Points.AddXY(valuationTable.Rows[rownum]["DATE"], valuationTable.Rows[rownum]["NET_ASSET_VALUE"]);
                    chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].PostBackValue =
                                        "Portfolio," +
                                        valuationTable.Rows[rownum]["SCHEME_NAME"] + "," + valuationTable.Rows[rownum]["DATE"] + "," +
                                        valuationTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                        valuationTable.Rows[rownum]["PurchaseDate"] + "," + valuationTable.Rows[rownum]["PurchaseNAV"] + "," +
                                        valuationTable.Rows[rownum]["PurchaseUnits"] + "," +
                                        valuationTable.Rows[rownum]["ValueAtCost"] + "," + valuationTable.Rows[rownum]["CumulativeUnits"] + "," +
                                        valuationTable.Rows[rownum]["CumulativeCost"] + "," + valuationTable.Rows[rownum]["CurrentValue"];

                    //if (valuationTable.Rows[rownum]["PURCHASE_DATE"].ToString().Equals(valuationTable.Rows[rownum]["NAVDATE"].ToString())) // || ((rownum + 1) == valuationTable.Rows.Count))
                    if (valuationTable.Rows[rownum]["PurchaseDate"].ToString().Equals(valuationTable.Rows[rownum]["DATE"].ToString())) // || ((rownum + 1) == valuationTable.Rows.Count))
                    {
                        chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].MarkerSize = 11;
                        chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].MarkerStyle = MarkerStyle.Diamond;
                        chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].MarkerColor = Color.Black;
                        chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                    }
                }
                chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].MarkerSize = 10;
                chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].MarkerStyle = MarkerStyle.Diamond;
                chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].MarkerColor = Color.Black;
                chartBackTest.Series[schemecode].Points[chartBackTest.Series[schemecode].Points.Count - 1].ToolTip = "Click to see latest valuation";
            }
        }
        public void ShowBSE()
        {
            StockManager stockManager = new StockManager();
            string fromDate = Master.textboxFromDate.Text;

            DataTable sensexTable = stockManager.GetStockPriceData("^BSESN", fromDate: fromDate);
            if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
            {
                if (chartBackTest.Series.FindByName("^BSESN") == null)
                {
                    chartBackTest.Series.Add("^BSESN");

                    chartBackTest.Series["^BSESN"].Name = "^BSESN";
                    (chartBackTest.Series["^BSESN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartBackTest.Series["^BSESN"]).ChartArea = chartBackTest.ChartAreas[0].Name;

                    chartBackTest.Series["^BSESN"].Legend = chartBackTest.Legends[0].Name;
                    chartBackTest.Series["^BSESN"].LegendText = "BSE SENSEX";
                    chartBackTest.Series["^BSESN"].LegendToolTip = "BSE SENSEX";

                    chartBackTest.Series["^BSESN"].XAxisType = AxisType.Secondary;
                    chartBackTest.Series["^BSESN"].YAxisType = AxisType.Primary;

                    (chartBackTest.Series["^BSESN"]).YValuesPerPoint = 4;
                    chartBackTest.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                    chartBackTest.Series["^BSESN"].PostBackValue = "^BSESN" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                }
                else
                {
                    chartBackTest.Series["^BSESN"].Enabled = true;
                }
                (chartBackTest.Series["^BSESN"]).Points.Clear();
                (chartBackTest.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
            }
        }

        public void ShowNSE()
        {
            StockManager stockManager = new StockManager();
            string fromDate = Master.textboxFromDate.Text;
            DataTable niftyTable = stockManager.GetStockPriceData("^NSEI", fromDate: fromDate);

            if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
            {
                if (chartBackTest.Series.FindByName("^NSEI") == null)
                {
                    chartBackTest.Series.Add("^NSEI");

                    chartBackTest.Series["^NSEI"].Name = "^NSEI";
                    chartBackTest.Series["^NSEI"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    chartBackTest.Series["^NSEI"].ChartArea = chartBackTest.ChartAreas[0].Name;

                    chartBackTest.Series["^NSEI"].Legend = chartBackTest.Legends[0].Name;
                    chartBackTest.Series["^NSEI"].LegendText = "NIFTY 50";
                    chartBackTest.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                    chartBackTest.Series["^NSEI"].XAxisType = AxisType.Secondary;
                    chartBackTest.Series["^NSEI"].YAxisType = AxisType.Primary;

                    (chartBackTest.Series["^NSEI"]).YValuesPerPoint = 4;
                    chartBackTest.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                    chartBackTest.Series["^NSEI"].PostBackValue = "^NSEI" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                }
                else
                {
                    chartBackTest.Series["^NSEI"].Enabled = true;
                }
                chartBackTest.Series["^NSEI"].Points.Clear();
                chartBackTest.Series["^NSEI"].Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
            }
        }
        public void buttonShowSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false, bArea2 = false;
            string schemecode = Master.textbox_selectedSchemeCode.Text;

            if (chartBackTest.Series.FindByName(graphName) != null)
            {
                chartBackTest.Series[graphName].Enabled = true;
            }
            else if (graphName.Equals("^BSESN"))
            {
                ShowBSE();
                chartBackTest.Series["^BSESN"].Enabled = true;
            }
            else if (graphName.Equals("^NSEI"))
            {
                ShowNSE();
                chartBackTest.Series["^NSEI"].Enabled = true;
            }
            else if (graphName.Equals(schemecode))
            {
                ShowSymbolValuation();
                chartBackTest.Series[schemecode].Enabled = true;
            }

            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                if ((chartBackTest.Series.FindByName(Master.dropdownGraphList.Items[i].Value) != null) &&
                    (chartBackTest.Series[Master.dropdownGraphList.Items[i].Value].Enabled))
                {
                    if (Master.dropdownGraphList.Items[i].Value.Equals("Daily") || Master.dropdownGraphList.Items[i].Value.Equals("SMA_SMALL") ||
                        Master.dropdownGraphList.Items[i].Value.Equals("SMA_LONG") ||
                        Master.dropdownGraphList.Items[i].Value.Equals("^BSESN") || Master.dropdownGraphList.Items[i].Value.Equals("^NSEI") ||
                        Master.dropdownGraphList.Items[i].Value.Equals(schemecode))
                    {
                        bArea0 = true;
                    }
                    else if (Master.dropdownGraphList.Items[i].Value.Equals("Future_Price") || Master.dropdownGraphList.Items[i].Value.Equals("SMA_SMALL_Future") ||
                        Master.dropdownGraphList.Items[i].Value.Equals("SMA_LONG_Future"))
                    {
                        bArea1 = true;
                    }
                    else if (Master.dropdownGraphList.Items[i].Value.Equals("Approximation_Error"))
                    {
                        bArea2 = true;
                    }
                }
            }
            chartBackTest.ChartAreas[0].Visible = bArea0;
            chartBackTest.ChartAreas[1].Visible = bArea1;
            chartBackTest.ChartAreas[2].Visible = bArea2;
        }
        public void buttonRemoveSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false, bArea2 = false;
            string schemecode = Master.textbox_selectedSchemeCode.Text;

            if (chartBackTest.Series.FindByName(graphName) != null)
            {
                chartBackTest.Series[graphName].Enabled = false;
            }

            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                if ((chartBackTest.Series.FindByName(Master.dropdownGraphList.Items[i].Value) != null) &&
                    (chartBackTest.Series[Master.dropdownGraphList.Items[i].Value].Enabled))
                {
                    if (Master.dropdownGraphList.Items[i].Value.Equals("Daily") || Master.dropdownGraphList.Items[i].Value.Equals("SMA_SMALL") ||
                        Master.dropdownGraphList.Items[i].Value.Equals("SMA_LONG") ||
                        Master.dropdownGraphList.Items[i].Value.Equals("^BSESN") || Master.dropdownGraphList.Items[i].Value.Equals("^NSEI") ||
                        Master.dropdownGraphList.Items[i].Value.Equals(schemecode))
                    {
                        bArea0 = true;
                    }
                    else if (Master.dropdownGraphList.Items[i].Value.Equals("Future_Price") || Master.dropdownGraphList.Items[i].Value.Equals("SMA_SMALL_Future") ||
                        Master.dropdownGraphList.Items[i].Value.Equals("SMA_LONG_Future"))
                    {
                        bArea1 = true;
                    }
                    else if (Master.dropdownGraphList.Items[i].Value.Equals("Approximation_Error"))
                    {
                        bArea2 = true;
                    }
                }
            }
            chartBackTest.ChartAreas[0].Visible = bArea0;
            chartBackTest.ChartAreas[1].Visible = bArea1;
            chartBackTest.ChartAreas[2].Visible = bArea2;
        }
        public void buttonShowGraph_Click()
        {
            ShowBackTestGraph();
            ShowForecast();

            if (chartBackTest.Series.FindByName("^BSESN") != null)// && (chartBackTest.Series["^BSESN"].Enabled))
            {
                ShowBSE();
            }
            if (chartBackTest.Series.FindByName("^NSEI") != null)// && (chartBackTest.Series["^NSEI"].Enabled))
            {
                ShowNSE();
            }

            //handle valuation graph
            string portfolioentry = "";
            //if we were called from portfolio page then we need to remove the current valuation graph
            for (int i = 0; i < Master.dropdownGraphList.Items.Count; i++)
            {
                //find list item with valuation text
                if (Master.dropdownGraphList.Items[i].Text.Contains("Valuation :"))
                {
                    //if found save the schemecode
                    portfolioentry = Master.dropdownGraphList.Items[i].Value;
                    //we need to handle case where user may use fund house list to select fundname, where portfolio is unselected (done at fundhouse selection change)
                    if (Master.dropdownPortfolioMF.SelectedIndex > 0)
                    {
                        //if portfolio is selected then we need to replace text & value of the same entry with Valuation text
                        Master.dropdownGraphList.Items[i].Text = "Valuation :" + Master.textbox_selectedFundName.Text;
                        Master.dropdownGraphList.Items[i].Value = Master.textbox_selectedSchemeCode.Text;
                    }
                    break;
                }
            }
            if (portfolioentry.Equals(string.Empty) == false)
            {
                //means we had a valuation entry in the graph list
                //check if older schemecode valuation entry is shown in graph
                if (chartBackTest.Series.FindByName(portfolioentry) != null)
                {
                    //remove the older schemecode valuation graph
                    chartBackTest.Series.Remove(chartBackTest.Series.FindByName(portfolioentry));
                    //now add the new one only if portfolio is selected
                    if (Master.dropdownPortfolioMF.SelectedIndex > 0)
                    {
                        ShowSymbolValuation();
                    }
                }
            }

        }

        protected void buttonShowGrid_Click()
        {
            gridviewBackTest.Enabled = !gridviewBackTest.Enabled;
            gridviewBackTest.Visible = !gridviewBackTest.Visible;

            gridviewPortfolioValuation.Enabled = !gridviewPortfolioValuation.Enabled;
            gridviewPortfolioValuation.Visible = !gridviewPortfolioValuation.Visible;
        }
        protected void gridviewBackTest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewBackTest.PageIndex = e.NewPageIndex;
            gridviewBackTest.DataSource = (DataTable)ViewState["BACKTEST_DATA"];
            gridviewBackTest.DataBind();
            //ShowGraph();
        }
        protected void gridviewPortfolioValuation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewPortfolioValuation.PageIndex = e.NewPageIndex;
            gridviewPortfolioValuation.DataSource = (DataTable)ViewState["VALUATION_DATA"];
            gridviewPortfolioValuation.DataBind();
            //ShowGraph();
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
        protected void chartBackTest_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth = 0;
            double lineHeight = 0;

            string seriesName;
            StringBuilder raString = new StringBuilder();

            try
            {
                if (chartBackTest.Annotations.Count > 0)
                    chartBackTest.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                seriesName = postBackValues[0];


                if (seriesName.Equals("Daily"))
                {
                    //NAV
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;

                    raString.Clear();
                    raString.AppendLine("Backtest simulation for: " + postBackValues[1]);
                    raString.AppendLine("Current Price Date: " + postBackValues[2]);
                    raString.AppendLine("Current Price: " + postBackValues[3]);
                    raString.AppendLine("Buy flag: " + postBackValues[4]);
                    raString.AppendLine("Sell Flag: " + postBackValues[5]);
                    raString.AppendLine("Simulation QTY: " + postBackValues[6]);
                    raString.AppendLine("Buy Cost: " + postBackValues[7]);
                    raString.AppendLine("Sell Value: " + postBackValues[8]);
                    raString.AppendLine("Profit/Loss: " + postBackValues[9]);
                    raString.AppendLine("Backtest Result : " + postBackValues[10]);

                    ra.Text = raString.ToString();

                    HA.ToolTip = "Close: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("Portfolio"))
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;

                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase NAV:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];

                    HA.ToolTip = "NAV: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("SMA_LONG") || seriesName.Equals("SMA_SMALL"))
                {
                    //SMA
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[6]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;

                    ra.Text = postBackValues[1] + "\nClose Date:" + postBackValues[2] + "\nCLOSE:" + postBackValues[3] + "\n" + postBackValues[4] + "\n" + postBackValues[5] + postBackValues[6];

                    HA.ToolTip = "SMA: " + postBackValues[6];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Contains("Future"))//forecasted series
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[1].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[1].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[1].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[1].AxisX;
                    VA.AxisX = chartBackTest.ChartAreas[1].AxisX;
                    ra.AxisX = chartBackTest.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[1].Name;

                    ra.Text = postBackValues[0] + "\n" + "\nDate:" + postBackValues[2] + "\n" + seriesName + postBackValues[3];

                    HA.ToolTip = seriesName + postBackValues[3];
                    VA.ToolTip = postBackValues[2];

                }
                else if (seriesName.Equals("^BSESN") || seriesName.Equals("^NSEI"))
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[0].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[0].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    VA.AxisX = chartBackTest.ChartAreas[0].AxisX2;
                    ra.AxisX = chartBackTest.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[0].Name;
                    ra.Text = seriesName + "\n" + "Date:" + postBackValues[2] + "\n" + "Close:" + postBackValues[3] + "\n" + "Open:" + postBackValues[4] + "\n" +
                        "High:" + postBackValues[5] + "\n" + "Low:" + postBackValues[6];
                }
                else if (seriesName.Contains("Error"))//forecasted series
                {
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[3]);

                    HA.AxisY = chartBackTest.ChartAreas[2].AxisY;
                    VA.AxisY = chartBackTest.ChartAreas[2].AxisY;
                    ra.AxisY = chartBackTest.ChartAreas[2].AxisY;

                    HA.AxisX = chartBackTest.ChartAreas[2].AxisX;
                    VA.AxisX = chartBackTest.ChartAreas[2].AxisX;
                    ra.AxisX = chartBackTest.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartBackTest.ChartAreas[2].Name;

                    ra.Text = postBackValues[0] + "\n" + "\nDate:" + postBackValues[2] + "\n" + "Approximation Error: " + postBackValues[3] + "\n" + "Forecast Error: " + postBackValues[4];

                    HA.ToolTip = seriesName + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }


                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartBackTest.Annotations.Add(HA);

                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartBackTest.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                chartBackTest.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }
    }
}