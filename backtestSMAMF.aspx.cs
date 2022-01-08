using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class backtestSMAMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                if (Request.QueryString["schemecode"] != null)
                {
                    if (!IsPostBack)
                    {
                        ViewState["FromDate"] = null;
                        ViewState["ToDate"] = null;
                        ViewState["FetchedData"] = null;
                        ViewState["FetchedIndexData"] = null;
                        ViewState["VALUATION_TABLE"] = null;
                        ViewState["SelectedIndex"] = "0";
                        if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
                        {
                            ddlShowHidePortfolio.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlShowHidePortfolio.SelectedIndex = 1;
                            ddlShowHidePortfolio.Enabled = false;
                        }
                        if (Request.QueryString["smasmall"] != null)
                        {
                            textboxSMASmallPeriod.Text = Request.QueryString["smasmall"].ToString();
                        }
                        if (Request.QueryString["smalong"] != null)
                        {
                            textboxSMALongPeriod.Text = Request.QueryString["smalong"].ToString();
                        }
                        if (Request.QueryString["buyspan"] != null)
                        {
                            textboxBuySpan.Text = Request.QueryString["buyspan"].ToString();
                        }
                        if (Request.QueryString["sellspan"] != null)
                        {
                            textboxSellSpan.Text = Request.QueryString["sellspan"].ToString();
                        }
                        if (Request.QueryString["simulationqty"] != null)
                        {
                            textboxSimulationQty.Text = Request.QueryString["simulationqty"].ToString();
                        }
                    }
                    ShowGraph();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        chartBackTestMF.Visible = true;
                        chartBackTestMF.Width = int.Parse(panelWidth.Value);
                        chartBackTestMF.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    //Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
                    //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select MF scheme');", true);
                    Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
        }

        public void FillData()
        {
            DataTable tempData = null;
            DataTable dailyNAVSMA = null;
            DataTable indexTable = null;
            DataRow[] filteredRows = null;
            string expression = "";
            DataManager dataMgr = new DataManager();
            string folderPath = Session["DATAFOLDER"].ToString();

            int smallperiod = Int32.Parse(textboxSMASmallPeriod.Text.ToString());
            int longperiod = Int32.Parse(textboxSMALongPeriod.Text.ToString());
            int buySpan = Int32.Parse(textboxBuySpan.Text.ToString());
            int sellSpan = Int32.Parse(textboxSellSpan.Text.ToString());
            string schemeCode = Request.QueryString["schemecode"].ToString();
            double simulationQty = double.Parse(textboxSimulationQty.Text.ToString());
            //if we were called from portfolio page then get the portfolio data for selected scheme
            //if (Request.QueryString["schemecode"] != null)
            if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
            {
                if ((ddlShowHidePortfolio.SelectedIndex == 0) && ((ViewState["VALUATION_TABLE"] == null) || (((DataTable)ViewState["VALUATION_TABLE"]).Rows.Count == 0)))
                {
                    tempData = dataMgr.GetValuationLineGraph(Session["MFPORTFOLIOMASTERROWID"].ToString(), Session["EMAILID"].ToString(),
                              Session["MFPORTFOLIONAME"].ToString());

                    expression = "SCHEME_CODE = '" + schemeCode + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                    {
                        ViewState["VALUATION_TABLE"] = (DataTable)filteredRows.CopyToDataTable();

                        //gridviewPortfolioValuation.DataSource = (DataTable)ViewState["VALUATION_TABLE"];
                        //gridviewPortfolioValuation.DataBind();
                    }
                }
            }
            if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
            {
                dailyNAVSMA = dataMgr.getBacktestFromSMA(Int32.Parse(schemeCode), 
                                fromDate: ((ViewState["FromDate"] == null) || (ViewState["FromDate"].ToString().Equals(""))) ? null : ViewState["FromDate"].ToString(),
                                toDate: ((ViewState["ToDate"] == null) || (ViewState["ToDate"].ToString().Equals(""))) ? null : ViewState["ToDate"].ToString(),
                                smallPeriod: smallperiod, longPeriod: longperiod, buySpan: buySpan, sellSpan: sellSpan, simulationQty: simulationQty);
                if (dailyNAVSMA != null)
                {
                    ViewState["FetchedData"] = dailyNAVSMA;

                    //gridviewBackTestMF.DataSource = (DataTable)ViewState["FetchedData"];
                    //gridviewBackTestMF.DataBind();
                }
            }

            if ((System.Convert.ToInt32((ViewState["SelectedIndex"].ToString())) != ddlIndex.SelectedIndex) && (ddlIndex.SelectedIndex > 0))
            {
                //Some index is selected by user
                indexTable = StockApi.getDailyAlternate(folderPath, ddlIndex.SelectedValue, bIsTestModeOn: false, bSaveData: false,
                                                        apiKey: Session["ApiKey"].ToString());
                ViewState["FetchedIndexData"] = indexTable;
                ViewState["SelectedIndex"] = ddlIndex.SelectedIndex;
            }
        }
        public void ShowGraph()
        {
            DataTable indexTable = null;
            DataTable valuationTable = null;
            DataTable dailysmaTable = null;
            DataTable tempData = null;
            DataRow[] filteredRows = null;
            Series tempSeries = null;

            string fromDate = "", toDate = "";
            string expression = "";
            int portfolioTxnNumber = 1;

            string schemeCode = Request.QueryString["schemecode"].ToString();
            int smallperiod = Int32.Parse(textboxSMASmallPeriod.Text.ToString());
            int longperiod = Int32.Parse(textboxSMALongPeriod.Text.ToString());

            FillData();

            if (ViewState["FromDate"] != null)
                fromDate = ViewState["FromDate"].ToString();
            if (ViewState["ToDate"] != null)
                toDate = ViewState["ToDate"].ToString();

            if ((fromDate.Length > 0) && (toDate.Length > 0))
            {
                if (ViewState["FetchedData"] != null)
                {
                    tempData = (DataTable)ViewState["FetchedData"];
                    expression = "NAVDATE >= '" + fromDate + "' and NAVDATE <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    //if ((filteredRows != null) && (filteredRows.Length > 0))
                    //    dailysmaTable = filteredRows.CopyToDataTable();
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                    {
                        dailysmaTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        if (dailysmaTable != null)
                        {
                            dailysmaTable.Clear();
                            dailysmaTable.Dispose();
                            dailysmaTable = null;
                        }
                    }
                    tempData.Clear();
                    tempData.Dispose();
                    tempData = null;
                }
                if (ViewState["FetchedIndexData"] != null)
                {
                    tempData = (DataTable)ViewState["FetchedIndexData"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    //if ((filteredRows != null) && (filteredRows.Length > 0))
                    //    indexTable = filteredRows.CopyToDataTable();

                    if ((filteredRows != null) && (filteredRows.Length > 0))
                    {
                        indexTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        if (indexTable != null)
                        {
                            indexTable.Clear();
                            indexTable.Dispose();
                            indexTable = null;
                        }
                    }
                    tempData.Clear();
                    tempData.Dispose();
                    tempData = null;
                }


                if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
                {
                    if (ViewState["VALUATION_TABLE"] != null)
                    {
                        tempData = (DataTable)ViewState["VALUATION_TABLE"];
                        //expression = "PurchaseDate >= '" + fromDate + "' and PurchaseDate <= '" + toDate + "'";
                        expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                        filteredRows = tempData.Select(expression);
                        //if ((filteredRows != null) && (filteredRows.Length > 0))
                        //    valuationTable = filteredRows.CopyToDataTable();

                        if ((filteredRows != null) && (filteredRows.Length > 0))
                        {
                            valuationTable = filteredRows.CopyToDataTable();
                        }
                        else
                        {
                            if (valuationTable != null)
                            {
                                valuationTable.Clear();
                                valuationTable.Dispose();
                                valuationTable = null;
                            }
                        }
                        tempData.Clear();
                        tempData.Dispose();
                        tempData = null;
                    }
                }
            }
            else
            {
                dailysmaTable = (DataTable)ViewState["FetchedData"];
                indexTable = (DataTable)ViewState["FetchedIndexData"];
                valuationTable = (DataTable)ViewState["VALUATION_TABLE"];
            }


            //gridviewPortfolioValuation.DataSource = valuationTable;
            //gridviewPortfolioValuation.DataBind();


            gridviewBackTestMF.DataSource = dailysmaTable;
            gridviewBackTestMF.DataBind();

            if (chartBackTestMF.Annotations.Count > 0)
                chartBackTestMF.Annotations.Clear();

            if ((dailysmaTable != null) && (dailysmaTable.Rows.Count > 0))
            {
                if (smallperiod > 0)
                {
                    if (chartBackTestMF.Series.FindByName("SMA_SMALL") == null)
                    {
                        chartBackTestMF.Series.Add("SMA_SMALL");

                        chartBackTestMF.Series["SMA_SMALL"].Name = "SMA_SMALL";
                        (chartBackTestMF.Series["SMA_SMALL"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTestMF.Series["SMA_SMALL"]).ChartArea = chartBackTestMF.ChartAreas[0].Name;

                        chartBackTestMF.Series["SMA_SMALL"].Legend = chartBackTestMF.Legends[0].Name;
                        chartBackTestMF.Series["SMA_SMALL"].LegendText = "SMA Period: " + smallperiod;//Request.QueryString["smasmall"].ToString();
                        chartBackTestMF.Series["SMA_SMALL"].LegendToolTip = "SMA Period: " + smallperiod;//Request.QueryString["smasmall"].ToString();

                        (chartBackTestMF.Series["SMA_SMALL"]).YValuesPerPoint = 2;
                        chartBackTestMF.Series["SMA_SMALL"].ToolTip = "SMA Period: " + smallperiod + " : Date:#VALX; SMA:#VALY (Click to see details)";
                        chartBackTestMF.Series["SMA_SMALL"].PostBackValue = "SMA_SMALL," + smallperiod + ",#VALX,#VALY";
                    }
                    (chartBackTestMF.Series["SMA_SMALL"]).Points.Clear();
                }
                //(chartBackTestMF.Series["SMA_SMALL"]).Points.DataBind(dailysmaTable.AsEnumerable(), "NAVDATE", "SMA_SMALL", "");
                if (longperiod > 0)
                {
                    if (chartBackTestMF.Series.FindByName("SMA_LONG") == null)
                    {
                        chartBackTestMF.Series.Add("SMA_LONG");

                        chartBackTestMF.Series["SMA_LONG"].Name = "SMA_LONG";
                        (chartBackTestMF.Series["SMA_LONG"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartBackTestMF.Series["SMA_LONG"]).ChartArea = chartBackTestMF.ChartAreas[0].Name;

                        chartBackTestMF.Series["SMA_LONG"].Legend = chartBackTestMF.Legends[0].Name;
                        chartBackTestMF.Series["SMA_LONG"].LegendText = "SMA Period: " + longperiod; //Request.QueryString["smalong"].ToString();
                        chartBackTestMF.Series["SMA_LONG"].LegendToolTip = "SMA Period: " + longperiod; //Request.QueryString["smalong"].ToString();

                        (chartBackTestMF.Series["SMA_LONG"]).YValuesPerPoint = 2;
                        chartBackTestMF.Series["SMA_LONG"].ToolTip = "SMA Period: " + longperiod + " : Date:#VALX; SMA:#VALY (Click to see details)";
                        chartBackTestMF.Series["SMA_LONG"].PostBackValue = "SMA_LONG," + longperiod + ",#VALX,#VALY";
                    }
                    (chartBackTestMF.Series["SMA_LONG"]).Points.Clear();
                    //(chartBackTestMF.Series["SMA_LONG"]).Points.DataBind(dailysmaTable.AsEnumerable(), "NAVDATE", "SMA_LONG", "");
                }

                if (chartBackTestMF.Series.FindByName("Daily") == null)
                {
                    chartBackTestMF.Series.Add("Daily");

                    chartBackTestMF.Series["Daily"].Name = "Daily";
                    (chartBackTestMF.Series["Daily"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartBackTestMF.Series["Daily"]).ChartArea = chartBackTestMF.ChartAreas[0].Name;

                    chartBackTestMF.Series["Daily"].Legend = chartBackTestMF.Legends[0].Name;
                    chartBackTestMF.Series["Daily"].LegendText = "Daily NAV";
                    chartBackTestMF.Series["Daily"].LegendToolTip = "Daily NAV";

                    (chartBackTestMF.Series["Daily"]).YValuesPerPoint = 2;
                    chartBackTestMF.Series["Daily"].ToolTip = "Daily NAV: " + " : Date:#VALX; NAV:#VALY (Click to see details)";
                    chartBackTestMF.Series["Daily"].PostBackValue = "Daily" + ",#VALX,#VALY";
                }
                //(chartBackTestMF.Series["Daily"]).Points.DataBind(dailysmaTable.AsEnumerable(), "NAVDATE", "NET_ASSET_VALUE", "");
                (chartBackTestMF.Series["Daily"]).Points.Clear();

                for (int rownum = 0; rownum < dailysmaTable.Rows.Count; rownum++)
                {
                    if (smallperiod > 0)
                    {
                        (chartBackTestMF.Series["SMA_SMALL"]).Points.AddXY(dailysmaTable.Rows[rownum]["NAVDATE"], dailysmaTable.Rows[rownum]["SMA_SMALL"]);
                        (chartBackTestMF.Series["SMA_SMALL"]).Points[(chartBackTestMF.Series["SMA_SMALL"]).Points.Count - 1].PostBackValue =
                                            "SMA_SMALL," +
                                            dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," + dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
                                            dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                            "SMA Period: " + smallperiod + "," +
                                            "SMA: ," + dailysmaTable.Rows[rownum]["SMA_SMALL"];
                    }
                    if (longperiod > 0)
                    {
                        (chartBackTestMF.Series["SMA_LONG"]).Points.AddXY(dailysmaTable.Rows[rownum]["NAVDATE"], dailysmaTable.Rows[rownum]["SMA_LONG"]);
                        (chartBackTestMF.Series["SMA_LONG"]).Points[(chartBackTestMF.Series["SMA_LONG"]).Points.Count - 1].PostBackValue =
                                            "SMA_LONG," +
                                            dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," + dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
                                            dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                            "SMA Period: " + longperiod + "," +
                                            "SMA: ," + dailysmaTable.Rows[rownum]["SMA_LONG"];
                    }
                    (chartBackTestMF.Series["Daily"]).Points.AddXY(dailysmaTable.Rows[rownum]["NAVDATE"], dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"]);
                    (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].PostBackValue =
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
                    //if ((dailysmaTable.Rows[rownum]["CROSSOVER_FLAG"]).ToString().Equals("X") == true)
                    //{
                    //}
                    if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["BUY_FLAG"]) == true)
                    {
                        //we just mark this point as cross over
                        (chartBackTestMF.Series["SMA_SMALL"]).Points[(chartBackTestMF.Series["SMA_SMALL"]).Points.Count - 1].MarkerSize = 11;
                        (chartBackTestMF.Series["SMA_SMALL"]).Points[(chartBackTestMF.Series["SMA_SMALL"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Cross;
                        (chartBackTestMF.Series["SMA_SMALL"]).Points[(chartBackTestMF.Series["SMA_SMALL"]).Points.Count - 1].MarkerColor = Color.Blue;
                        (chartBackTestMF.Series["SMA_SMALL"]).Points[(chartBackTestMF.Series["SMA_SMALL"]).Points.Count - 1].ToolTip = "Backtest Initiator";


                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].MarkerSize = 12;
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].MarkerColor = Color.Yellow;
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].ToolTip = "Buy Signal";
                    }
                    if (System.Convert.ToBoolean(dailysmaTable.Rows[rownum]["SELL_FLAG"]) == true)
                    {
                        //we just mark this point as cross over
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].MarkerSize = 12;
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].MarkerColor = Color.Green;
                        (chartBackTestMF.Series["Daily"]).Points[(chartBackTestMF.Series["Daily"]).Points.Count - 1].ToolTip = "Sell Signal: " + dailysmaTable.Rows[rownum]["RESULT"].ToString();
                    }
                }
            }
            else
            {
                tempSeries = chartBackTestMF.Series.FindByName("Daily");
                if (tempSeries != null)
                    chartBackTestMF.Series.Remove(tempSeries);
                tempSeries = chartBackTestMF.Series.FindByName("SMA_SMALL");
                if (tempSeries != null)
                    chartBackTestMF.Series.Remove(tempSeries);
                tempSeries = chartBackTestMF.Series.FindByName("SMA_LONG");
                if (tempSeries != null)
                    chartBackTestMF.Series.Remove(tempSeries);
            }
            if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
            {
                if (chartBackTestMF.Series.FindByName(schemeCode) == null)
                {
                    chartBackTestMF.Series.Add(schemeCode);

                    chartBackTestMF.Series[schemeCode].Name = schemeCode; // "Portfolio";
                    (chartBackTestMF.Series[schemeCode]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartBackTestMF.Series[schemeCode]).ChartArea = chartBackTestMF.ChartAreas[0].Name;

                    chartBackTestMF.Series[schemeCode].Legend = chartBackTestMF.Legends[0].Name;
                    chartBackTestMF.Series[schemeCode].LegendText = valuationTable.Rows[0]["SCHEME_NAME"].ToString();
                    chartBackTestMF.Series[schemeCode].LegendToolTip = valuationTable.Rows[0]["SCHEME_NAME"].ToString();

                    (chartBackTestMF.Series[schemeCode]).YValuesPerPoint = 2;
                    chartBackTestMF.Series[schemeCode].ToolTip = "Portfolio: " + "Purchase Date:#VALX; Purchase NAV:#VALY (Click to see details)";
                    chartBackTestMF.Series[schemeCode].PostBackValue = "Portfolio:" + ",#VALX,#VALY";
                }
                //(chartBackTestMF.Series["Daily"]).Points.DataBind(dailysmaTable.AsEnumerable(), "NAVDATE", "NET_ASSET_VALUE", "");
                (chartBackTestMF.Series[schemeCode]).Points.Clear();
                for (int rownum = 0; rownum < valuationTable.Rows.Count; rownum++)
                {
                    //(chartBackTestMF.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["PurchaseDate"], valuationTable.Rows[rownum]["PurchaseNAV"]);
                    (chartBackTestMF.Series[schemeCode]).Points.AddXY(valuationTable.Rows[rownum]["DATE"], valuationTable.Rows[rownum]["NET_ASSET_VALUE"]);
                    (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].PostBackValue =
                                        "Portfolio," +
                                        valuationTable.Rows[rownum]["SCHEME_NAME"] + "," + valuationTable.Rows[rownum]["DATE"] + "," +
                                        valuationTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                                        valuationTable.Rows[rownum]["PurchaseDate"] + "," + valuationTable.Rows[rownum]["PurchaseNAV"] + "," +
                                        valuationTable.Rows[rownum]["PurchaseUnits"] + "," +
                                        valuationTable.Rows[rownum]["ValueAtCost"] + "," + valuationTable.Rows[rownum]["CumulativeUnits"] + "," +
                                        valuationTable.Rows[rownum]["CumulativeCost"] + "," + valuationTable.Rows[rownum]["CurrentValue"];

                    //if( (rownum == 0)  || (valuationTable.Rows[rownum - 1]["PurchaseDate"] != valuationTable.Rows[rownum]["PurchaseDate"])) // || ((rownum + 1) == valuationTable.Rows.Count))
                    if (valuationTable.Rows[rownum]["PurchaseDate"].ToString().Equals(valuationTable.Rows[rownum]["DATE"].ToString())) // || ((rownum + 1) == valuationTable.Rows.Count))
                    {
                        (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].MarkerSize = 11;
                        (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                        (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].MarkerColor = Color.Black;
                        (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].ToolTip = "Transaction: " + portfolioTxnNumber++;
                    }
                }
                (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].MarkerSize = 10;
                (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].MarkerColor = Color.Black;
                (chartBackTestMF.Series[schemeCode]).Points[(chartBackTestMF.Series[schemeCode]).Points.Count - 1].ToolTip = "Click to see latest valuation";

            }
            else
            {
                tempSeries = chartBackTestMF.Series.FindByName(schemeCode);
                if (tempSeries != null)
                    chartBackTestMF.Series.Remove(tempSeries);
            }
            if ((indexTable != null) && (indexTable.Rows.Count > 0))
            {
                if (chartBackTestMF.Series.FindByName(ddlIndex.SelectedValue) == null)
                {
                    chartBackTestMF.Series.Add(ddlIndex.SelectedValue);

                    chartBackTestMF.Series[ddlIndex.SelectedValue].Name = ddlIndex.SelectedValue;
                    (chartBackTestMF.Series[ddlIndex.SelectedValue]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    (chartBackTestMF.Series[ddlIndex.SelectedValue]).ChartArea = chartBackTestMF.ChartAreas[0].Name;

                    chartBackTestMF.Series[ddlIndex.SelectedValue].Legend = chartBackTestMF.Legends[0].Name;
                    chartBackTestMF.Series[ddlIndex.SelectedValue].LegendText = ddlIndex.SelectedValue;
                    chartBackTestMF.Series[ddlIndex.SelectedValue].LegendToolTip = ddlIndex.SelectedValue;

                    (chartBackTestMF.Series[ddlIndex.SelectedValue]).YValuesPerPoint = 4;
                    chartBackTestMF.Series[ddlIndex.SelectedValue].ToolTip = ddlIndex.SelectedValue + ": Date:#VALX; Close:#VALY4 (Click to see details)";
                    chartBackTestMF.Series[ddlIndex.SelectedValue].PostBackValue = ddlIndex.SelectedValue + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                }
                (chartBackTestMF.Series[ddlIndex.SelectedValue]).Points.DataBindXY(indexTable.Rows, "Date", indexTable.Rows, "Open,High,Low,Close");

            }
            for (int i = 1; i < ddlIndex.Items.Count; i++)
            {
                tempSeries = chartBackTestMF.Series.FindByName(ddlIndex.Items[i].Value);
                if (tempSeries != null)
                {
                    if (ddlIndex.SelectedValue != ddlIndex.Items[i].Value)
                    {
                        chartBackTestMF.Series.Remove(tempSeries);
                    }
                }
            }

        }

        //"SMA_LONG," +
        //dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," +
        //dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
        //dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," + 
        //"SMA Period: " + Request.QueryString["smalong"].ToString() + "," +
        //"SMA: ," +
        //dailysmaTable.Rows[rownum]["SMA_LONG"];

        //"Portfolio," +
        //                                valuationTable.Rows[rownum]["SCHEME_NAME"] + "," + valuationTable.Rows[rownum]["DATE"] + "," +
        //                                valuationTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
        //                                valuationTable.Rows[rownum]["PurchaseDate"] + "," + valuationTable.Rows[rownum]["PurchaseNAV"] + "," +
        //                                valuationTable.Rows[rownum]["PurchaseUnits"] + "," +
        //                                valuationTable.Rows[rownum]["ValueAtCost"] + "," + valuationTable.Rows[rownum]["CumulativeUnits"] + "," +
        //                                valuationTable.Rows[rownum]["CumulativeCost"] + "," + portfolioTabvinayb68le.Rows[rownum]["CurrentValue"];
        protected void chartBackTestMF_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth = 0;
            double lineHeight = 0;

            string seriesName;
            StringBuilder raString = new StringBuilder();

            try
            {
                if (chartBackTestMF.Annotations.Count > 0)
                    chartBackTestMF.Annotations.Clear();

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
                    // "Daily," +
                    //dailysmaTable.Rows[rownum]["SCHEMENAME"] + "," +
                    //dailysmaTable.Rows[rownum]["NAVDATE"] + "," +
                    //dailysmaTable.Rows[rownum]["NET_ASSET_VALUE"] + "," +
                    //dailysmaTable.Rows[rownum]["BUY_FLAG"] + "," +
                    //dailysmaTable.Rows[rownum]["SELL_FLAG"] + "," +
                    //dailysmaTable.Rows[rownum]["QUANTITY"] + "," +
                    //dailysmaTable.Rows[rownum]["BUY_COST"] + "," +
                    //dailysmaTable.Rows[rownum]["SELL_VALUE"] + "," +
                    //dailysmaTable.Rows[rownum]["PROFIT_LOSS"] + "," +
                    //dailysmaTable.Rows[rownum]["RESULT"];

                    raString.Clear();
                    raString.AppendLine("Backtest simulation for: " + postBackValues[1]);
                    raString.AppendLine("Current NAV Date: " + postBackValues[2]);
                    raString.AppendLine("Current NAV: " + postBackValues[3]);
                    raString.AppendLine("Buy flag: " + postBackValues[4]);
                    raString.AppendLine("Sell Flag: " + postBackValues[5]);
                    raString.AppendLine("Simulation QTY: " + postBackValues[6]);
                    raString.AppendLine("Buy Cost: " + postBackValues[7]);
                    raString.AppendLine("Sell Value: " + postBackValues[8]);
                    raString.AppendLine("Profit/Loss: " + postBackValues[9]);
                    raString.AppendLine("Backtest Result : " + postBackValues[10]);

                    //ra.Text = "Backtest simulation for: " + postBackValues[1] + "\nNAV Date:" + postBackValues[2] + "\nNAV:" + postBackValues[3];
                    ra.Text = raString.ToString();

                    HA.ToolTip = "NAV: " + postBackValues[3];
                    VA.ToolTip = postBackValues[2];
                }
                else if (seriesName.Equals("Portfolio"))
                {
                    //purchase NAV
                    //xDate = System.Convert.ToDateTime(postBackValues[4]);
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[5]);
                    ra.Text = postBackValues[1] + "\nPurchase Date:" + postBackValues[4] + "\nPurchase NAV:" + postBackValues[5] + "\nPurchased Units: " + postBackValues[6] +
                        "\nPurchase Cost: " + postBackValues[7] + "\nCumulative Units: " + postBackValues[8] + "\nCumulative Cost: " + postBackValues[9] +
                        "\nValue as of date: " + postBackValues[10];

                    HA.ToolTip = "Purchase NAV: " + postBackValues[5];
                    VA.ToolTip = postBackValues[4];
                }
                else
                {
                    //SMA
                    xDate = System.Convert.ToDateTime(postBackValues[2]);
                    lineWidth = xDate.ToOADate();
                    lineHeight = System.Convert.ToDouble(postBackValues[6]);

                    ra.Text = postBackValues[1] + "\nNAV Date:" + postBackValues[2] + "\nNAV:" + postBackValues[3] + "\n" + postBackValues[4] + "\n" + postBackValues[5] + postBackValues[6];

                    HA.ToolTip = "SMA: " + postBackValues[6];
                    VA.ToolTip = postBackValues[2];
                }



                HA.AxisY = chartBackTestMF.ChartAreas[0].AxisY;
                VA.AxisY = chartBackTestMF.ChartAreas[0].AxisY;
                ra.AxisY = chartBackTestMF.ChartAreas[0].AxisY;

                HA.AxisX = chartBackTestMF.ChartAreas[0].AxisX;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartBackTestMF.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartBackTestMF.Annotations.Add(HA);

                VA.AxisX = chartBackTestMF.ChartAreas[0].AxisX;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartBackTestMF.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartBackTestMF.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.AxisX = chartBackTestMF.ChartAreas[0].AxisX;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                //ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nValuation:" + postBackValues[2] + "\nCum Qty:" + quantity + "\nCost:" + cost;
                //ra.SmartLabelStyle = sl;
                ra.PostBackValue = "AnnotationClicked";

                chartBackTestMF.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }
        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            string fromDate = textboxFromDate.Text;
            string toDate = textboxToDate.Text;
            //string fileName = Session["PortfolioNameMF"].ToString();
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            ViewState["FetchedData"] = null;
            ViewState["FetchedIndexData"] = null;
            ViewState["VALUATION_TABLE"] = null;
            ShowGraph();
        }

        protected void buttonShowGrid_Click(object sender, EventArgs e)
        {
            //if ((gridviewBackTestMF.Visible) || (gridviewPortfolioValuation.Visible))
            if (gridviewBackTestMF.Visible)
            {
                gridviewBackTestMF.Visible = false;
                //gridviewPortfolioValuation.Visible = false;
                buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                //if (ViewState["FetchedData"] != null)
                //{
                gridviewBackTestMF.Visible = true;
                //gridviewPortfolioValuation.Visible = true;
                buttonShowGrid.Text = "Hide Raw Data";
                //gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
                //gridviewPortfolioValuation.DataBind();
                //}
            }
        }

        protected void gridviewBackTestMF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewBackTestMF.PageIndex = e.NewPageIndex;
            //gridviewBackTestMF.DataSource = (DataTable)ViewState["FetchedData"];
            //gridviewBackTestMF.DataBind();
            ShowGraph();
        }
        protected void gridviewPortfolioValuation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewPortfolioValuation.PageIndex = e.NewPageIndex;
            //gridviewPortfolioValuation.DataSource = (DataTable)ViewState["VALUATION_TABLE"];
            //gridviewPortfolioValuation.DataBind();
            ShowGraph();
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}