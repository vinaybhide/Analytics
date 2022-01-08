using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Xml;

namespace Analytics
{
    public partial class portfolioValuation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedData"] = null;
                    ViewState["FetchedIndexData"] = null;
                    ViewState["SelectedIndex"] = "0";
                    listboxScripts.Items.Clear();
                    ListItem li = new ListItem("Show All", "All");
                    listboxScripts.Items.Add(li);
                    listboxScripts.Items[0].Selected = true;
                }
                if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
                {
                    ShowGraph();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    //string fileName = Session["STOCKPORTFOLIONAME"].ToString();
                    //ShowGraph(fileName);
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        chartPortfolioValuation.Visible = true;
                        chartPortfolioValuation.Width = int.Parse(panelWidth.Value);
                        chartPortfolioValuation.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    //Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
                    //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioNameToOpen + "');", true);
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

        /// <summary>
        /// Method to get portfolio valuation data and show graph for each symbol and index
        /// The valuation table has following columns
        /// [0]	{rowid}	object {System.Data.DataColumn}
        /// [1]	{SYMBOL}	object {System.Data.DataColumn}
        /// [2]	{EXCHANGENAME}	object {System.Data.DataColumn}
        /// [3]	{TYPE}	object {System.Data.DataColumn}
        /// [4]	{DATA_GRANULARITY}	object {System.Data.DataColumn}
        /// [5]	{OPEN}	object {System.Data.DataColumn}
        /// [6]	{HIGH}	object {System.Data.DataColumn}
        /// [7]	{LOW}	object {System.Data.DataColumn}
        /// [8]	{CLOSE}	object {System.Data.DataColumn}
        /// [9]	{ADJ_CLOSE}	object {System.Data.DataColumn}
        /// [10]	{VOLUME}	object {System.Data.DataColumn}
        /// [11]	{TIMESTAMP}	object {System.Data.DataColumn}
        /// [12]	{PURCHASE_DATE}	object {System.Data.DataColumn}
        /// [13]	{PURCHASE_PRICE}	object {System.Data.DataColumn}
        /// [14]	{PURCHASE_QTY}	object {System.Data.DataColumn}
        /// [15]	{INVESTMENT_COST}	object {System.Data.DataColumn}
        /// [16]	{CURRENTVALUE}	object {System.Data.DataColumn}
        /// [17]	{CumulativeQty}	object {System.Data.DataColumn}
        /// [18]	{CumulativeCost}	object {System.Data.DataColumn}
        /// [19]	{CumulativeValue}	object {System.Data.DataColumn}
        /// </summary>
        //public void ShowGraph(string fileName)
        public void ShowGraph()
        {
            DataTable valuationTable = null;
            DataTable indexTable = null;
            DataTable filteredValuationTable = null;

            DataTable tempData = null;
            string fromDate = "", toDate = "";
            string expression = "";
            DataRow[] filteredRows = null;

            string scriptName;
            StockManager stockManager = new StockManager();

            try
            {
                if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                {
                    valuationTable = stockManager.GetPortfolio_ValuationLineGraph(Session["STOCKPORTFOLIOMASTERROWID"].ToString());
                    ViewState["FetchedData"] = valuationTable;

                    gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
                    gridviewPortfolioValuation.DataBind();
                }
                else
                {
                    valuationTable = (DataTable)ViewState["FetchedData"];
                }

                //now get the index table
                if ((System.Convert.ToInt32((ViewState["SelectedIndex"].ToString())) != ddlIndex.SelectedIndex) &&
                        (ddlIndex.SelectedIndex > 0))
                {
                    indexTable = stockManager.GetStockPriceData(ddlIndex.SelectedValue, "", outputsize: "Full", time_interval: "1d");

                    ViewState["FetchedIndexData"] = indexTable;
                    ViewState["SelectedIndex"] = ddlIndex.SelectedIndex;
                }
                else
                {
                    indexTable = (DataTable)ViewState["FetchedIndexData"];
                }

                //check if date filter is applied by user
                if (ViewState["FromDate"] != null)
                { 
                    fromDate = System.Convert.ToDateTime(ViewState["FromDate"].ToString()).ToString("dd-MM-yyyy");
                }
                else if((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    //set from date to Min TIMESTAMP Date n valuation table
                    textboxFromDate.Text = System.Convert.ToDateTime(valuationTable.Compute("MIN([TIMESTAMP])", "")).ToString("yyyy-MM-dd"); 
                    fromDate = System.Convert.ToDateTime(textboxFromDate.Text).ToString("dd-MM-yyyy");
                    ViewState["FromDate"] = fromDate;
                }
                if (ViewState["ToDate"] != null)
                { 
                    toDate = System.Convert.ToDateTime(ViewState["ToDate"].ToString()).ToString("dd-MM-yyyy");
                }
                else
                {
                    textboxToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    toDate = DateTime.Today.ToString("dd-MM-yyyy");
                    ViewState["ToDate"] = toDate;
                }

                if ((fromDate.Length > 0) && (toDate.Length > 0))
                {
                    tempData = (DataTable)ViewState["FetchedData"];
                    expression = "TIMESTAMP >= '" + fromDate + "' and TIMESTAMP <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        valuationTable = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    if (ViewState["FetchedIndexData"] != null)
                    {
                        tempData = (DataTable)ViewState["FetchedIndexData"];
                        expression = "TIMESTAMP >= '" + fromDate + "' and TIMESTAMP <= '" + toDate + "'";
                        filteredRows = tempData.Select(expression);
                        if ((filteredRows != null) && (filteredRows.Length > 0))
                            indexTable = filteredRows.CopyToDataTable();
                    }
                }
                //else
                //{
                //    valuationTable = (DataTable)ViewState["FetchedData"];
                //    indexTable = (DataTable)ViewState["FetchedIndexData"];
                //}

                //Now setup valuation graph for each symbol
                if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    DataTable symbolTable = valuationTable.DefaultView.ToTable(true, "SYMBOL", "EXCHANGE");
                    foreach (DataRow rowSymbolTable in symbolTable.Rows)
                    {
                        scriptName = rowSymbolTable["SYMBOL"].ToString() + "." + rowSymbolTable["EXCHANGE"].ToString();
                        if (listboxScripts.Items.FindByValue(scriptName) == null)
                        {
                            ListItem li = new ListItem(scriptName, scriptName);
                            listboxScripts.Items.Add(li);
                        }
                        if (chartPortfolioValuation.Series.FindByName(scriptName) == null)
                        {
                            chartPortfolioValuation.Series.Add(scriptName);
                            chartPortfolioValuation.Series[scriptName].Name = scriptName;
                            (chartPortfolioValuation.Series[scriptName]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                            (chartPortfolioValuation.Series[scriptName]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;
                            
                            //chartPortfolioValuation.Series[scriptName].Sort(PointSortOrder.Descending, "X");

                            chartPortfolioValuation.Series[scriptName].Legend = chartPortfolioValuation.Legends[0].Name;
                            chartPortfolioValuation.Series[scriptName].LegendText = scriptName;
                            chartPortfolioValuation.Series[scriptName].LegendToolTip = scriptName;
                            chartPortfolioValuation.Series[scriptName].ToolTip = scriptName + ": Date:#VALX; Value:#VALY (Click to see details)";
                            chartPortfolioValuation.Series[scriptName].PostBackValue = scriptName + ",#VALX,#VALY1,#VAL2,#VAL3,#VAL4";
                        }

                        //now get the data specific to the current script
                        valuationTable.DefaultView.RowFilter = "SYMBOL = '" + rowSymbolTable["SYMBOL"].ToString() + "' AND EXCHANGE = '" + rowSymbolTable["EXCHANGE"].ToString() + "'";
                        filteredValuationTable = valuationTable.DefaultView.ToTable();
                        //filteredValuationTable.DefaultView.Sort = "TIMESTAMP DESC";
                        //filteredValuationTable = filteredValuationTable.DefaultView.ToTable();
                        (chartPortfolioValuation.Series[scriptName]).Points.Clear();
                        foreach (DataRow rowfilteredValuationTable in filteredValuationTable.Rows)
                        {
                            (chartPortfolioValuation.Series[scriptName]).Points.AddXY(rowfilteredValuationTable["TIMESTAMP"], rowfilteredValuationTable["CumulativeValue"]);

                            //if TIMESTAMP (which is from stockdata) == PURCHASE_DATE then we need to highlight this with diamond to show purchase/sale activity
                            //if (rowfilteredValuationTable["TIMESTAMP"].ToString().Equals(rowfilteredValuationTable["PURCHASE_DATE"].ToString()))
                            if(rowfilteredValuationTable["PORTFOLIO_FLAG"].ToString().Equals("True"))
                            {
                                //this is the row where we have purchase/sale activity
                                (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 10;
                                (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            }
                            (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].PostBackValue =
                                        scriptName + "," + rowfilteredValuationTable["TIMESTAMP"] + "," + rowfilteredValuationTable["CumulativeValue"] + "," +
                                        rowfilteredValuationTable["CumulativeQty"] + "," + rowfilteredValuationTable["CumulativeCost"];

                        }
                        (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 10;
                        (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    }
                }

                //now setup index graph
                if (indexTable != null)
                {
                    if (chartPortfolioValuation.Series.FindByName(ddlIndex.SelectedValue) == null)
                    {
                        chartPortfolioValuation.Series.Add(ddlIndex.SelectedValue);

                        chartPortfolioValuation.Series[ddlIndex.SelectedValue].Name = ddlIndex.SelectedValue;
                        (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

                        chartPortfolioValuation.Series[ddlIndex.SelectedValue].Legend = chartPortfolioValuation.Legends[0].Name;
                        chartPortfolioValuation.Series[ddlIndex.SelectedValue].LegendText = ddlIndex.SelectedValue;
                        chartPortfolioValuation.Series[ddlIndex.SelectedValue].LegendToolTip = ddlIndex.SelectedValue;

                        (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).YValuesPerPoint = 4;
                        chartPortfolioValuation.Series[ddlIndex.SelectedValue].ToolTip = ddlIndex.SelectedValue + ": Date:#VALX; Close:#VALY4 (Click to see details)";
                        chartPortfolioValuation.Series[ddlIndex.SelectedValue].PostBackValue = ddlIndex.SelectedValue + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).Points.DataBindXY(indexTable.Rows, "TIMESTAMP", indexTable.Rows, "OPEN,HIGH,LOW,CLOSE");
                    for (int i = 1; i < ddlIndex.Items.Count; i++)
                    {
                        Series tempSeries = chartPortfolioValuation.Series.FindByName(ddlIndex.Items[i].Value);
                        if (tempSeries != null)
                        {
                            if (ddlIndex.SelectedValue != ddlIndex.Items[i].Value)
                            {
                                chartPortfolioValuation.Series.Remove(tempSeries);
                            }
                        }
                    }
                }

                //show hide valuation graphs for symbols
                foreach (ListItem item in listboxScripts.Items)
                {
                    if (item.Value.Equals("All") && item.Selected)
                    {
                        foreach (Series itemSeries in chartPortfolioValuation.Series)
                        {
                            itemSeries.Enabled = true;
                        }
                        break;
                    }
                    else if (!item.Value.Equals("All") && (item.Selected))
                    {
                        chartPortfolioValuation.Series[item.Value].Enabled = true;
                    }
                    else if (!item.Value.Equals("All") && (!item.Selected))
                    {
                        chartPortfolioValuation.Series[item.Value].Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
            }
            
            //string fileName = "";
            //bool bIsTestOn = true;
            //DataTable portfolioTable = null;
            //DataTable indexTable = null;
            //DataTable tempData = null;
            //string folderPath = Server.MapPath("~/scriptdata/");
            //XmlDocument xmlPortfolio;
            //XmlNode root;
            //XmlNodeList scriptNodeList;
            //string scriptName;
            //string searchPath;
            ////int markerInterval = 10;
            ////int i = 0;
            //int tempQty = 0;
            //double tempCost;
            //string fromDate = "", toDate = "";
            //string expression = "";
            //DataRow[] filteredRows = null;
            //DataRow[] scriptRows;

            //try
            //{
            //    if (File.Exists(fileName))
            //    {
            //        //portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestOn);

            //        if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
            //        {
            //            if (Session["IsTestOn"] != null)
            //            {
            //                bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
            //            }

            //            if (Session["DATAFOLDER"] != null)
            //            {
            //                folderPath = Session["DATAFOLDER"].ToString();
            //            }

            //            portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestModeOn: false, apiKey: Session["ApiKey"].ToString());
            //            ViewState["FetchedData"] = portfolioTable;
            //            gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
            //            gridviewPortfolioValuation.DataBind();
            //        }

            //        if ((System.Convert.ToInt32((ViewState["SelectedIndex"].ToString())) != ddlIndex.SelectedIndex) &&
            //                (ddlIndex.SelectedIndex > 0))
            //        {
            //            if (Session["IsTestOn"] != null)
            //            {
            //                bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
            //            }

            //            if (Session["DATAFOLDER"] != null)
            //            {
            //                folderPath = Session["DATAFOLDER"].ToString();
            //            }

            //            //Some index is selected by user
            //            indexTable = StockApi.getDailyAlternate(folderPath, ddlIndex.SelectedValue, bIsTestModeOn: false, bSaveData: false,
            //                                                    apiKey: Session["ApiKey"].ToString());
            //            ViewState["FetchedIndexData"] = indexTable;
            //            ViewState["SelectedIndex"] = ddlIndex.SelectedIndex;
            //        }
            //        //else
            //        //{
            //        if (ViewState["FromDate"] != null)
            //            fromDate = ViewState["FromDate"].ToString();
            //        if (ViewState["ToDate"] != null)
            //            toDate = ViewState["ToDate"].ToString();

            //        if ((fromDate.Length > 0) && (toDate.Length > 0))
            //        {
            //            tempData = (DataTable)ViewState["FetchedData"];
            //            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
            //            filteredRows = tempData.Select(expression);
            //            if ((filteredRows != null) && (filteredRows.Length > 0))
            //                portfolioTable = filteredRows.CopyToDataTable();

            //            tempData.Clear();
            //            tempData = null;

            //            if (ViewState["FetchedIndexData"] != null)
            //            {
            //                tempData = (DataTable)ViewState["FetchedIndexData"];
            //                expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
            //                filteredRows = tempData.Select(expression);
            //                if ((filteredRows != null) && (filteredRows.Length > 0))
            //                    indexTable = filteredRows.CopyToDataTable();
            //            }
            //        }
            //        else
            //        {
            //            portfolioTable = (DataTable)ViewState["FetchedData"];
            //            indexTable = (DataTable)ViewState["FetchedIndexData"];
            //        }
            //        //}

            //        if (portfolioTable != null)
            //        {
            //            if (chartPortfolioValuation.Annotations.Count > 0)
            //                chartPortfolioValuation.Annotations.Clear();

            //            xmlPortfolio = new XmlDocument();
            //            xmlPortfolio.Load(fileName);
            //            root = xmlPortfolio.DocumentElement;
            //            searchPath = "/portfolio/script/name";
            //            scriptNodeList = root.SelectNodes(searchPath);
            //            foreach (XmlNode scriptNameNode in scriptNodeList)
            //            {
            //                scriptName = scriptNameNode.InnerText;

            //                scriptRows = portfolioTable.Select("Symbol='" + scriptName + "'");

            //                if (scriptRows.Length > 0)
            //                {
            //                    if (listboxScripts.Items.FindByValue(scriptName) == null)
            //                    {
            //                        ListItem li = new ListItem(scriptName, scriptName);
            //                        listboxScripts.Items.Add(li);
            //                    }

            //                    if (chartPortfolioValuation.Series.FindByName(scriptName) == null)
            //                    {
            //                        chartPortfolioValuation.Series.Add(scriptName);
            //                        chartPortfolioValuation.Series[scriptName].Name = scriptName;
            //                        (chartPortfolioValuation.Series[scriptName]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            //                        (chartPortfolioValuation.Series[scriptName]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

            //                        chartPortfolioValuation.Series[scriptName].Legend = chartPortfolioValuation.Legends[0].Name;
            //                        chartPortfolioValuation.Series[scriptName].LegendText = scriptName;
            //                        chartPortfolioValuation.Series[scriptName].LegendToolTip = scriptName;
            //                        chartPortfolioValuation.Series[scriptName].ToolTip = scriptName + ": Date:#VALX; Value:#VALY (Click to see details)";
            //                        chartPortfolioValuation.Series[scriptName].PostBackValue = scriptName + ",#VALX,#VALY1,#VAL2,#VAL3,#VAL4";
            //                    }

            //                    tempQty = 0;
            //                    tempCost = 0.00;
            //                    (chartPortfolioValuation.Series[scriptName]).Points.Clear();
            //                    foreach (DataRow itemRow in scriptRows)
            //                    {
            //                        (chartPortfolioValuation.Series[scriptName]).Points.AddXY(itemRow["Date"], itemRow["ValueOnDate"]);

            //                        if ((itemRow["CumulativeQuantity"] != System.DBNull.Value) && ((tempQty == 0) || (tempQty != System.Convert.ToInt32(itemRow["CumulativeQuantity"]))))
            //                        {
            //                            tempQty = System.Convert.ToInt32(itemRow["CumulativeQuantity"]);
            //                            (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 10;
            //                            (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
            //                            //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].Label = 
            //                            //    scriptName + "\n" + "Purchase date:" + itemRow["PurchaseDate"].ToString() + 
            //                            //    "\nCumulative Qty=" + tempQty.ToString() + "\nCost of Investment: " + itemRow["CostofInvestment"].ToString() + 
            //                            //    "\nValuation: " + itemRow["ValueOnDate"].ToString();
            //                            //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBackColor = Color.Transparent;
            //                            //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;
            //                            //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderColor = System.Drawing.Color.Transparent;
            //                            //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].IsValueShownAsLabel = true;
            //                        }
            //                        (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].PostBackValue =
            //                            scriptName + "," + itemRow["Date"] + "," + itemRow["ValueOnDate"] + "," + itemRow["CumulativeQuantity"] + "," + itemRow["CostofInvestment"];
            //                    }
            //                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 10;
            //                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;

            //                    //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].Label =
            //                    //    scriptName + "\n" + "Purchase date:" + scriptRows[scriptRows.Length - 1]["PurchaseDate"].ToString() +
            //                    //    "\nValuation: " + scriptRows[scriptRows.Length - 1]["ValueOnDate"].ToString();
            //                    //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBackColor = Color.RoyalBlue;
            //                    //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;
            //                    //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderColor = System.Drawing.Color.Black;
            //                    //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].IsValueShownAsLabel = false;
            //                }
            //            }

            //            if (indexTable != null)
            //            {
            //                if (chartPortfolioValuation.Series.FindByName(ddlIndex.SelectedValue) == null)
            //                {
            //                    chartPortfolioValuation.Series.Add(ddlIndex.SelectedValue);

            //                    chartPortfolioValuation.Series[ddlIndex.SelectedValue].Name = ddlIndex.SelectedValue;
            //                    (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            //                    (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

            //                    chartPortfolioValuation.Series[ddlIndex.SelectedValue].Legend = chartPortfolioValuation.Legends[0].Name;
            //                    chartPortfolioValuation.Series[ddlIndex.SelectedValue].LegendText = ddlIndex.SelectedValue;
            //                    chartPortfolioValuation.Series[ddlIndex.SelectedValue].LegendToolTip = ddlIndex.SelectedValue;

            //                    (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).YValuesPerPoint = 4;
            //                    chartPortfolioValuation.Series[ddlIndex.SelectedValue].ToolTip = ddlIndex.SelectedValue + ": Date:#VALX; Close:#VALY4 (Click to see details)";
            //                    chartPortfolioValuation.Series[ddlIndex.SelectedValue].PostBackValue = ddlIndex.SelectedValue + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
            //                }
            //                (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).Points.DataBindXY(indexTable.Rows, "Date", indexTable.Rows, "Open,High,Low,Close");

            //                for (int i = 1; i < ddlIndex.Items.Count; i++)
            //                {
            //                    Series tempSeries = chartPortfolioValuation.Series.FindByName(ddlIndex.Items[i].Value);
            //                    if (tempSeries != null)
            //                    {
            //                        if (ddlIndex.SelectedValue != ddlIndex.Items[i].Value)
            //                        {
            //                            chartPortfolioValuation.Series.Remove(tempSeries);
            //                        }
            //                    }
            //                }
            //            }

            //            foreach (ListItem item in listboxScripts.Items)
            //            {
            //                if (item.Value.Equals("All") && item.Selected)
            //                {
            //                    foreach (Series itemSeries in chartPortfolioValuation.Series)
            //                    {
            //                        itemSeries.Enabled = true;
            //                    }
            //                    break;
            //                }
            //                else if (!item.Value.Equals("All") && (item.Selected))
            //                {
            //                    chartPortfolioValuation.Series[item.Value].Enabled = true;
            //                }
            //                else if (!item.Value.Equals("All") && (!item.Selected))
            //                {
            //                    chartPortfolioValuation.Series[item.Value].Enabled = false;
            //                }
            //            }


            //            //chartPortfolioValuation.ChartAreas[0].AxisX.MaximumAutoSize = false;
            //            //scriptRows = portfolioTable.Select("[Date] = MAX([Date])");
            //            //chartPortfolioValuation.ChartAreas[0].AxisX.Maximum = System.Convert.ToDateTime(scriptRows[0][1]).AddDays(100).ToOADate();

            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
            //}
        }

        protected void gridviewPortfolioValuation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewPortfolioValuation.PageIndex = e.NewPageIndex;
            gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
            gridviewPortfolioValuation.DataBind();
        }

        //protected void checkboxlistScripts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string fileName = Session["STOCKPORTFOLIONAME"].ToString();
        //    ShowGraph(fileName);
        //}

        protected void chartPortfolioValuation_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            int quantity;
            double cost;
            string seriesName;

            double open, high, low, close;

            try
            {
                if (chartPortfolioValuation.Annotations.Count > 0)
                    chartPortfolioValuation.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                seriesName = postBackValues[0];
                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);

                if (seriesName.Contains("^"))
                {
                    ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nOpen:" + postBackValues[2] + "\nHigh:" + postBackValues[3] + "\nLow:" + postBackValues[4] + "\nClose:" + postBackValues[5];
                }
                else
                {
                    quantity = System.Convert.ToInt32(postBackValues[3]);
                    cost = System.Convert.ToDouble(postBackValues[4]);
                    ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nValuation:" + postBackValues[2] + "\nCum Qty:" + quantity + "\nCost:" + cost;
                }


                HA.AxisY = chartPortfolioValuation.ChartAreas[0].AxisY;
                VA.AxisY = chartPortfolioValuation.ChartAreas[0].AxisY;
                ra.AxisY = chartPortfolioValuation.ChartAreas[0].AxisY;

                //HA.Name = seriesName;
                HA.AxisX = chartPortfolioValuation.ChartAreas[0].AxisX;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartPortfolioValuation.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                HA.ToolTip = "Script: " + seriesName + ", Valuation: " + postBackValues[2];
                chartPortfolioValuation.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.AxisX = chartPortfolioValuation.ChartAreas[0].AxisX;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartPortfolioValuation.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartPortfolioValuation.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.AxisX = chartPortfolioValuation.ChartAreas[0].AxisX;
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

                chartPortfolioValuation.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            //ShowGraph(fileName);
            ShowGraph();
        }

        protected void buttonShowGrid_Click(object sender, EventArgs e)
        {
            if (gridviewPortfolioValuation.Visible)
            {
                gridviewPortfolioValuation.Visible = false;
                buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                //if (ViewState["FetchedData"] != null)
                //{
                gridviewPortfolioValuation.Visible = true;
                buttonShowGrid.Text = "Hide Raw Data";
                //gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
                //gridviewPortfolioValuation.DataBind();
                //}
            }
        }

        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}