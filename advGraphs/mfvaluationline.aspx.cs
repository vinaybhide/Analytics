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
    public partial class mfvaluationline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "doHourglass", "doHourglass();", true);
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

                    Master.LoadPortfolioMF();

                    ViewState["CURRENT_PORTFOLIO_ROW_ID"] = null;
                    if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
                    {
                        ViewState["CURRENT_PORTFOLIO_ROW_ID"] = Session["MFPORTFOLIOMASTERROWID"].ToString();
                        Master.dropdownPortfolioMF.SelectedValue = Session["MFPORTFOLIOMASTERROWID"].ToString();
                        Master.LoadPortfolioFundNameList();
                        //Master.dropdownFundNameList.SelectedIndex = 0;
                        Master.textbox_selectedFundName.Text = "";
                        Master.textbox_selectedSchemeCode.Text = "";


                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                        //ClientScript.RegisterStartupScript(this.GetType(), "doHourglass", "doHourglass();", true);
                        //now show the backtest graph
                        ShowLineValuation();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
//                        ClientScript.RegisterStartupScript(this.GetType(), "resetCursor", "resetCursor();", true);
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

            Master.panelOnlineFundHouse.Enabled = false;
            Master.panelOnlineFundHouse.Visible = false;

            Master.panelSearchListFundName.Enabled = false;
            Master.panelSearchListFundName.Visible = false;

            //Master.panelCommonControls.Enabled = false;
            //Master.panelCommonControls.Visible = false;

            Master.panelMainParam.Enabled = false;
            Master.panelMainParam.Visible = false;

            Master.panelDescription.Enabled = false;
            Master.panelDescription.Visible = false;


            Master.buttonDesc.Enabled = false;
            Master.buttonDesc.Visible = false;

            Master.buttonShowHideParam.Enabled = false;
            Master.buttonShowHideParam.Visible = false;

            //Master.buttonTopSecion.Enabled = false;
            //Master.buttonTopSecion.Visible = false;
        }

        public void ShowLineValuation()
        {
            DataTable tempTable = null, valuationTable = null;
            string expression = string.Empty;
            DataRow[] filteredRows;
            DataRow[] scriptRows;

            double tempQty = 0;

            DataManager dataMgr = new DataManager();
            ViewState["VALUATION_DATA"] = null;

            if (Master.dropdownPortfolioMF.SelectedIndex > 0)
            {
                tempTable = dataMgr.GetValuationLineGraph(Master.dropdownPortfolioMF.SelectedValue);
                if( (tempTable!= null) && (tempTable.Rows.Count > 0))
                {
                    //first apply date filter

                    if(Master.textboxFromDate.Text.Equals(string.Empty))
                    {
                        object ofromDate = tempTable.Compute("MIN(DATE)", null);
                        Master.textboxFromDate.Text = System.Convert.ToDateTime(ofromDate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        expression = "DATE >= '" + Master.textboxFromDate.Text + "'";
                    }

                    if(expression.Equals(string.Empty) == false)
                    {
                        filteredRows = tempTable.Select(expression);
                        if ((filteredRows != null) && (filteredRows.Length > 0))
                            valuationTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        valuationTable = tempTable.Copy();
                    }
                    if((valuationTable != null) && (valuationTable.Rows.Count > 0))
                    {
                        if (chartPortfolioValuation.Annotations.Count > 0)
                            chartPortfolioValuation.Annotations.Clear();

                        gridviewPortfolioValuation.DataSource = valuationTable;
                        gridviewPortfolioValuation.DataBind();

                        ViewState["VALUATION_DATA"] = valuationTable;

                        Master.dropdownGraphList.Visible = true;

                        Master.dropdownGraphList.Items.Clear();
                        ListItem li;


                        foreach (ListItem itemFundName in Master.dropdownFundNameList.Items)
                        {
                            if (itemFundName.Value.Equals("-1"))
                            {
                                continue;
                            }
                            
                            li = new ListItem(itemFundName.Text, itemFundName.Value);
                            Master.dropdownGraphList.Items.Add(li);

                            scriptRows = valuationTable.Select("SCHEME_CODE='" + itemFundName.Value + "'");

                            if (scriptRows.Length > 0)
                            {
                                if (chartPortfolioValuation.Series.FindByName(itemFundName.Value) == null)
                                {
                                    chartPortfolioValuation.Series.Add(itemFundName.Value);
                                    chartPortfolioValuation.Series[itemFundName.Value].Name = itemFundName.Value;
                                    chartPortfolioValuation.Series[itemFundName.Value].ChartType = SeriesChartType.Line;
                                    chartPortfolioValuation.Series[itemFundName.Value].ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

                                    chartPortfolioValuation.Series[itemFundName.Value].Legend = chartPortfolioValuation.Legends[0].Name;
                                    chartPortfolioValuation.Series[itemFundName.Value].LegendText = itemFundName.Value;
                                    chartPortfolioValuation.Series[itemFundName.Value].LegendToolTip = itemFundName.Text;
                                    chartPortfolioValuation.Series[itemFundName.Value].ToolTip = itemFundName.Text + ": Date:#VALX; Current Value:#VALY (Click to see details)";
                                    chartPortfolioValuation.Series[itemFundName.Value].PostBackValue = itemFundName.Value + ":" + itemFundName.Text + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4,#VALY5,#VALY6";
                                }
                                tempQty = 0;
                                chartPortfolioValuation.Series[itemFundName.Value].Points.Clear();
                                foreach (DataRow itemRow in scriptRows)
                                {
                                    chartPortfolioValuation.Series[itemFundName.Value].Points.AddXY(itemRow["DATE"], itemRow["CurrentValue"]);

                                    if ((itemRow["CumulativeUnits"] != System.DBNull.Value) && ((tempQty == 0) || (tempQty != System.Convert.ToDouble(itemRow["CumulativeUnits"]))))
                                    {
                                        tempQty = System.Convert.ToDouble(itemRow["CumulativeUnits"]);
                                        chartPortfolioValuation.Series[itemFundName.Value].Points[chartPortfolioValuation.Series[itemFundName.Value].Points.Count - 1].MarkerSize = 10;
                                        chartPortfolioValuation.Series[itemFundName.Value].Points[chartPortfolioValuation.Series[itemFundName.Value].Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                                    }

                                    chartPortfolioValuation.Series[itemFundName.Value].Points[chartPortfolioValuation.Series[itemFundName.Value].Points.Count - 1].PostBackValue =
                                        itemRow["SCHEME_NAME"] + "," + itemRow["DATE"] + "," + itemRow["NET_ASSET_VALUE"] + "," + itemRow["CurrentValue"] + "," + itemRow["CumulativeUnits"] + "," + itemRow["CumulativeCost"];
                                }
                                chartPortfolioValuation.Series[itemFundName.Value].Points[chartPortfolioValuation.Series[itemFundName.Value].Points.Count - 1].MarkerSize = 10;
                                chartPortfolioValuation.Series[itemFundName.Value].Points[chartPortfolioValuation.Series[itemFundName.Value].Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
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
                    (chartPortfolioValuation.Series["^BSESN"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
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
                    chartPortfolioValuation.Series["^NSEI"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
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
            if(Master.dropdownPortfolioMF.SelectedValue.Equals(ViewState["CURRENT_PORTFOLIO_ROW_ID"].ToString()) == false)
            {
                Master.textboxFromDate.Text = "";
                ViewState["CURRENT_PORTFOLIO_ROW_ID"] = Master.dropdownPortfolioMF.SelectedValue;
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
            double quantity;
            double cost;
            double nav;

            string seriesName;
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

                //itemRow["SCHEME_NAME"] + "," + itemRow["DATE"] + 
                //itemRow["CurrentValue"] + "," + itemRow["CumulativeUnits"] + "," + itemRow["CumulativeCost"] + "," + itemRow["NET_ASSET_VALUE"];

                seriesName = postBackValues[0];
                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[3]);

                if (seriesName.Contains("^"))
                {
                    ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nOpen:" + postBackValues[2] + "\nHigh:" + postBackValues[3] + "\nLow:" + postBackValues[4] + "\nClose:" + postBackValues[5];
                }
                else
                {
                    nav = System.Convert.ToDouble(postBackValues[2]);
                    quantity = System.Convert.ToDouble(postBackValues[4]);
                    cost = System.Convert.ToDouble(postBackValues[5]);
                    ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nCurrent NAV:" + nav + "\nCumulative Units:" + quantity + "\nValuation:" + postBackValues[3] + "\nCumulative Cost:" + cost;
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
                HA.ToolTip = postBackValues[3];
                HA.ToolTip = "Fund name: " + seriesName + ", Valuation: " + postBackValues[3];
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
                VA.ToolTip = postBackValues[1];
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