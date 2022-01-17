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
    public partial class stockvaluationline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                Master.OnDoEventShowGraph += new advancegraphs.DoEventShowGraph(buttonShowGraph_Click);
                Master.OnDoEventShowGrid += new advancegraphs.DoEventShowGrid(buttonShowGrid_Click);
                Master.OnDoEventRemoveSelectedIndicatorGraph += new advancegraphs.DoEventRemoveSelectedIndicatorGraph(buttonRemoveSelectedIndicatorGraph_Click);
                Master.OnDoEventShowSelectedIndicatorGraph += new advancegraphs.DoEventShowSelectedIndicatorGraph(buttonShowSelectedIndicatorGraph_Click);
                if (!IsPostBack)
                {
                    ViewState["VALUATION_DATA"] = null;

                    ManagePanels();

                    Master.textboxFromDate.Text = "";
                    Master.textboxToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    Master.textboxToDate.Enabled = false;

                    Master.LoadPortfolioList();

                    ViewState["CURRENT_PORTFOLIO_ROW_ID"] = null;
                    if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
                    {
                        ViewState["CURRENT_PORTFOLIO_ROW_ID"] = Session["STOCKPORTFOLIOMASTERROWID"].ToString();
                        Master.dropdownPortfolioList.SelectedValue = Session["STOCKPORTFOLIOMASTERROWID"].ToString();
                        Master.LoadPortfolioStockList();
                        Master.textbox_SelectedSymbol.Text = "";
                        Master.textbox_SelectedExchange.Text = "";


                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                        //now show the backtest graph
                        ShowLineValuation();
                    }
                    //fillGraphList();

                    this.Title = "Porfolio valuation graph";
                }

                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    //ShowGraph(scriptName);
                    chartPortfolioValuation.Visible = true;
                    chartPortfolioValuation.Width = int.Parse(Master.panelWidth.Value);
                    chartPortfolioValuation.Height = int.Parse(Master.panelHeight.Value);
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
        public void ManagePanels()
        {
            Master.InitializePanels(bStocksGraph: true);

            Master.panelOnlineStocks.Enabled = false;
            Master.panelOnlineStocks.Visible = false;

            Master.buttonSearchStockPortfolio.Enabled = false;
            Master.buttonSearchStockPortfolio.Visible = false;

            Master.panelSearchListStocks.Enabled = false;
            Master.panelSearchListStocks.Visible = false;

            Master.panelMainParam.Enabled = false;
            Master.panelMainParam.Visible = false;


            Master.panelDescription.Enabled = false;
            Master.panelDescription.Visible = false;


            Master.buttonDesc.Enabled = false;
            Master.buttonDesc.Visible = false;

            Master.buttonShowHideParam.Enabled = false;
            Master.buttonShowHideParam.Visible = false;
        }
        public void ShowLineValuation()
        {
            DataTable tempTable = null, valuationTable = null;
            string expression = string.Empty;
            DataRow[] filteredRows;
            DataRow[] scriptRows;

            StockManager dataMgr = new StockManager();
            ViewState["VALUATION_DATA"] = null;

            if (Master.dropdownPortfolioList.SelectedIndex > 0)
            {
                tempTable = dataMgr.GetPortfolio_ValuationLineGraph(Master.dropdownPortfolioList.SelectedValue);
                if ((tempTable != null) && (tempTable.Rows.Count > 0))
                {
                    if (Master.textboxFromDate.Text.Equals(string.Empty))
                    {
                        object ofromDate = tempTable.Compute("MIN(TIMESTAMP)", null);
                        Master.textboxFromDate.Text = System.Convert.ToDateTime(ofromDate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        expression = "TIMESTAMP >= '" + Master.textboxFromDate.Text + "'";
                    }
                    if (expression.Equals(string.Empty) == false)
                    {
                        filteredRows = tempTable.Select(expression);
                        if ((filteredRows != null) && (filteredRows.Length > 0))
                            valuationTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        valuationTable = tempTable.Copy();
                    }
                    if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                    {
                        if (chartPortfolioValuation.Annotations.Count > 0)
                            chartPortfolioValuation.Annotations.Clear();

                        gridviewPortfolioValuation.DataSource = valuationTable;
                        gridviewPortfolioValuation.DataBind();

                        ViewState["VALUATION_DATA"] = valuationTable;
                        Master.dropdownGraphList.Visible = true;

                        Master.dropdownGraphList.Items.Clear();
                        ListItem li;
                        foreach (ListItem itemStockName in Master.dropdownStockList.Items)
                        {
                            if (itemStockName.Value.Equals("-1"))
                            {
                                continue;
                            }
                            li = new ListItem(itemStockName.Text, itemStockName.Value);
                            Master.dropdownGraphList.Items.Add(li);

                            scriptRows = valuationTable.Select("SYMBOL='" + itemStockName.Value + "'");
                            if (scriptRows.Length > 0)
                            {
                                if (chartPortfolioValuation.Series.FindByName(itemStockName.Value) == null)
                                {
                                    chartPortfolioValuation.Series.Add(itemStockName.Value);
                                    chartPortfolioValuation.Series[itemStockName.Value].Name = itemStockName.Value;
                                    chartPortfolioValuation.Series[itemStockName.Value].ChartType = SeriesChartType.Line;
                                    chartPortfolioValuation.Series[itemStockName.Value].ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

                                    chartPortfolioValuation.Series[itemStockName.Value].Legend = chartPortfolioValuation.Legends[0].Name;
                                    chartPortfolioValuation.Series[itemStockName.Value].LegendText = itemStockName.Value;
                                    chartPortfolioValuation.Series[itemStockName.Value].LegendToolTip = itemStockName.Text;
                                    chartPortfolioValuation.Series[itemStockName.Value].ToolTip = itemStockName.Text + ": Date:#VALX; Value:#VALY (Click to see details)";
                                    chartPortfolioValuation.Series[itemStockName.Value].PostBackValue = itemStockName.Value + ",#VALX,#VALY1,#VAL2,#VAL3,#VAL4";
                                }
                                chartPortfolioValuation.Series[itemStockName.Value].Points.Clear();
                                foreach (DataRow itemRow in scriptRows)
                                {
                                    chartPortfolioValuation.Series[itemStockName.Value].Points.AddXY(itemRow["TIMESTAMP"], itemRow["CumulativeValue"]);

                                    //if TIMESTAMP (which is from stockdata) == PURCHASE_DATE then we need to highlight this with diamond to show purchase/sale activity
                                    //if (rowfilteredValuationTable["TIMESTAMP"].ToString().Equals(rowfilteredValuationTable["PURCHASE_DATE"].ToString()))
                                    if (itemRow["PORTFOLIO_FLAG"].ToString().Equals("True"))
                                    {
                                        //this is the row where we have purchase/sale activity
                                        chartPortfolioValuation.Series[itemStockName.Value].Points[chartPortfolioValuation.Series[itemStockName.Value].Points.Count - 1].MarkerSize = 10;
                                        chartPortfolioValuation.Series[itemStockName.Value].Points[chartPortfolioValuation.Series[itemStockName.Value].Points.Count - 1].MarkerStyle = MarkerStyle.Diamond;
                                    }
                                    chartPortfolioValuation.Series[itemStockName.Value].Points[chartPortfolioValuation.Series[itemStockName.Value].Points.Count - 1].PostBackValue =
                                                itemStockName.Value + "," + itemRow["TIMESTAMP"] + "," + itemRow["CumulativeValue"] + "," +
                                                itemRow["CumulativeQty"] + "," + itemRow["CumulativeCost"];

                                }
                                chartPortfolioValuation.Series[itemStockName.Value].Points[chartPortfolioValuation.Series[itemStockName.Value].Points.Count - 1].MarkerSize = 10;
                                chartPortfolioValuation.Series[itemStockName.Value].Points[chartPortfolioValuation.Series[itemStockName.Value].Points.Count - 1].MarkerStyle = MarkerStyle.Diamond;
                            }
                        }
                        li = new ListItem("BSE SENSEX", "^BSESN");
                        Master.dropdownGraphList.Items.Add(li);

                        li = new ListItem("NIFTY 50", "^NSEI");
                        Master.dropdownGraphList.Items.Add(li);
                    }
                }
            }
        }
        public void ShowBSE()
        {
            StockManager stockManager = new StockManager();
            string fromDate = Master.textboxFromDate.Text;

            DataTable sensexTable = stockManager.GetStockPriceData("^BSESN", fromDate: fromDate);
            if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
            {
                if (chartPortfolioValuation.Series.FindByName("^BSESN") == null)
                {
                    chartPortfolioValuation.Series.Add("^BSESN");

                    chartPortfolioValuation.Series["^BSESN"].Name = "^BSESN";
                    (chartPortfolioValuation.Series["^BSESN"]).ChartType = SeriesChartType.Line;
                    (chartPortfolioValuation.Series["^BSESN"]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

                    chartPortfolioValuation.Series["^BSESN"].Legend = chartPortfolioValuation.Legends[0].Name;
                    chartPortfolioValuation.Series["^BSESN"].LegendText = "BSE SENSEX";
                    chartPortfolioValuation.Series["^BSESN"].LegendToolTip = "BSE SENSEX";

                    chartPortfolioValuation.Series["^BSESN"].XAxisType = AxisType.Secondary;
                    chartPortfolioValuation.Series["^BSESN"].YAxisType = AxisType.Primary;

                    (chartPortfolioValuation.Series["^BSESN"]).YValuesPerPoint = 4;
                    chartPortfolioValuation.Series["^BSESN"].ToolTip = "^BSESN" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                    chartPortfolioValuation.Series["^BSESN"].PostBackValue = "^BSESN" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                }
                else
                {
                    chartPortfolioValuation.Series["^BSESN"].Enabled = true;
                }
                (chartPortfolioValuation.Series["^BSESN"]).Points.Clear();
                (chartPortfolioValuation.Series["^BSESN"]).Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
            }
        }

        public void ShowNSE()
        {
            StockManager stockManager = new StockManager();
            string fromDate = Master.textboxFromDate.Text;
            DataTable niftyTable = stockManager.GetStockPriceData("^NSEI", fromDate: fromDate);

            if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
            {
                if (chartPortfolioValuation.Series.FindByName("^NSEI") == null)
                {
                    chartPortfolioValuation.Series.Add("^NSEI");

                    chartPortfolioValuation.Series["^NSEI"].Name = "^NSEI";
                    chartPortfolioValuation.Series["^NSEI"].ChartType = SeriesChartType.Line;
                    chartPortfolioValuation.Series["^NSEI"].ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

                    chartPortfolioValuation.Series["^NSEI"].Legend = chartPortfolioValuation.Legends[0].Name;
                    chartPortfolioValuation.Series["^NSEI"].LegendText = "NIFTY 50";
                    chartPortfolioValuation.Series["^NSEI"].LegendToolTip = "NIFTY 50";

                    chartPortfolioValuation.Series["^NSEI"].XAxisType = AxisType.Secondary;
                    chartPortfolioValuation.Series["^NSEI"].YAxisType = AxisType.Primary;

                    (chartPortfolioValuation.Series["^NSEI"]).YValuesPerPoint = 4;
                    chartPortfolioValuation.Series["^NSEI"].ToolTip = "^NSEI" + ": Date:#VALX; Close:#VALY1 (Click to see details)";
                    chartPortfolioValuation.Series["^NSEI"].PostBackValue = "^NSEI" + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                }
                else
                {
                    chartPortfolioValuation.Series["^NSEI"].Enabled = true;
                }
                chartPortfolioValuation.Series["^NSEI"].Points.Clear();
                chartPortfolioValuation.Series["^NSEI"].Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
            }
        }
        public void buttonShowSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;

            if (chartPortfolioValuation.Series.FindByName(graphName) != null)
            {
                chartPortfolioValuation.Series[graphName].Enabled = true;
            }
            else if (graphName.Equals("^BSESN"))
            {
                ShowBSE();
                chartPortfolioValuation.Series["^BSESN"].Enabled = true;
            }
            else if (graphName.Equals("^NSEI"))
            {
                ShowNSE();
                chartPortfolioValuation.Series["^NSEI"].Enabled = true;
            }
        }
        public void buttonRemoveSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;

            if (chartPortfolioValuation.Series.FindByName(graphName) != null)
            {
                chartPortfolioValuation.Series[graphName].Enabled = false;
            }
        }
        public void buttonShowGraph_Click()
        {
            if (Master.dropdownPortfolioList.SelectedValue.Equals(ViewState["CURRENT_PORTFOLIO_ROW_ID"].ToString()) == false)
            {
                Master.textboxFromDate.Text = "";
                ViewState["CURRENT_PORTFOLIO_ROW_ID"] = Master.dropdownPortfolioList.SelectedValue;
            }
            ShowLineValuation();

            if (chartPortfolioValuation.Series.FindByName("^BSESN") != null)// && (chartPortfolioValuation.Series["^BSESN"].Enabled))
            {
                ShowBSE();
            }
            if (chartPortfolioValuation.Series.FindByName("^NSEI") != null)// && (chartPortfolioValuation.Series["^NSEI"].Enabled))
            {
                ShowNSE();
            }
        }
        protected void gridviewPortfolioValuation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewPortfolioValuation.PageIndex = e.NewPageIndex;
            gridviewPortfolioValuation.DataSource = (DataTable)ViewState["VALUATION_DATA"];
            gridviewPortfolioValuation.DataBind();
        }

        protected void buttonShowGrid_Click()
        {
            gridviewPortfolioValuation.Enabled = !gridviewPortfolioValuation.Enabled;
            gridviewPortfolioValuation.Visible = !gridviewPortfolioValuation.Visible;
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
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
    }
}