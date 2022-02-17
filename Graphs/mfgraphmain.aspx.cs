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
using DataAccessLayer;

namespace Analytics
{
    public partial class mfGraphMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                Master.OnDoEventShowGraph += new mfstandardindicators.DoEventShowGraph(buttonShowGraph_Click);
                Master.OnDoEventShowGrid += new mfstandardindicators.DoEventShowGrid(buttonShowGrid_Click);
                //Master.OnDoEventToggleDesc += new mfstandardindicators.DoEventToggleDesc(buttonDesc_Click);
                Master.OnDoEventRemoveSelectedIndicatorGraph += new mfstandardindicators.DoEventRemoveSelectedIndicatorGraph(buttonRemoveSelectedIndicatorGraph_Click);
                Master.OnDoEventShowSelectedIndicatorGraph += new mfstandardindicators.DoEventShowSelectedIndicatorGraph(buttonShowSelectedIndicatorGraph_Click);
                this.Title = "Graph for: " + "No fund name selected";

                if (!IsPostBack)
                {
                    //Initialize
                    ViewState["SchemeData"] = null;
                    ViewState["SchemeName"] = null;

                    Master.fundHouseList.Enabled = true;
                    Master.fundHouseList.Items.Clear();

                    Master.LoadFundHouseList();
                    Master.fundNameList.Items.Clear();
                    Master.fundNameList.Enabled = false;
                    Master.selectedSchemeCode.Text = "";
                    Master.selectedFundName.Text = "";

                    Master.textboxFromDate.Text = DateTime.Today.AddDays(-90).ToString("yyyy-MM-dd");  //DateTime.Today.AddDays(-90).ToString("dd-MM-yyyy");

                    Master.textboxToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                    //fillLinesCheckBoxes();
                    //fillDesc();

                    //if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null) &&
                    //    (Request.QueryString["fromdate"] != null) && (Request.QueryString["todate"] != null))
                    if ((Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null) &&
                        (Request.QueryString["fromdate"] != null) && (Request.QueryString["todate"] != null))
                    {
                        //means we were called by some other page
                        Master.fundHouseList.SelectedValue = Request.QueryString["fundhousecode"].ToString();
                        Master.fundNameList.Enabled = true;
                        Master.LoadFundNameList();
                        Master.fundNameList.SelectedValue = Request.QueryString["schemecode"].ToString();
                        Master.selectedSchemeCode.Text = Master.fundNameList.SelectedValue;
                        Master.selectedFundName.Text = Master.fundNameList.Items[Master.fundNameList.SelectedIndex].Text;
                        Master.textboxFromDate.Text = Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");  //DateTime.Today.AddDays(-90).ToString("dd-MM-yyyy");
                        Master.textboxToDate.Text = Convert.ToDateTime(Request.QueryString["todate"].ToString()).ToString("yyyy-MM-dd"); // DateTime.Today.ToString("dd-MM-yyyy");

                        this.Title = "Graph for: " + Master.selectedFundName.Text;

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);
                        if (Request.QueryString["graphtype"] != null)
                        {
                            ShowDailyGraph("DAILY_NAV");
                            chartMF.Series["DAILY_NAV"].Enabled = false;
                            Master.indicatorList.SelectedValue = Request.QueryString["graphtype"].ToString();
                            buttonShowSelectedIndicatorGraph_Click();
                            if (Master.indicatorList.SelectedValue.Equals("DAILY_NAV") ||
                                Master.indicatorList.SelectedValue.Equals("SMA Fast") || Master.indicatorList.SelectedValue.Equals("SMA Slow") ||
                                Master.indicatorList.SelectedValue.Equals("EMA Fast") || Master.indicatorList.SelectedValue.Equals("EMA Slow") ||
                                  Master.indicatorList.SelectedValue.Equals("WMA Fast") || Master.indicatorList.SelectedValue.Equals("WMA Slow") ||
                                  Master.indicatorList.SelectedValue.Equals("Upper Band") || Master.indicatorList.SelectedValue.Equals("Middle Band") ||
                                  Master.indicatorList.SelectedValue.Equals("Lower Band"))
                            {
                                chartMF.ChartAreas[0].Visible = true;
                            }
                            else
                            {
                                chartMF.ChartAreas[0].Visible = false;
                            }
                        }
                        else
                        {
                            ShowDailyGraph("DAILY_NAV");
                        }
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);
                    }
                }

                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    //ShowGraph(scriptName);
                    chartMF.Visible = true;
                    chartMF.Width = int.Parse(Master.panelWidth.Value);
                    chartMF.Height = int.Parse(Master.panelHeight.Value);
                }
                //else
                //{
                //    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noFundSelected + "');", true);
                //    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
                //    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                //}
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Server.Transfer("~/Default.aspx");
                //Response.Redirect("~/Default.aspx");
            }
        }

        //public void fillLinesCheckBoxes()
        //{
        //}

        //public void fillDesc()
        //{
        //    Master.bulletedlistDesc.Visible = true;
        //    Master.bulletedlistDesc.Items.Add("You can change the date range to see different range of NAV for selected Scheme. Click the Reset Graph button to refresh the graph.");
        //}

        /// <summary>
        /// Parses the parameter string
        /// Format of the parameter string in master page : parametername=value,parametername=value
        /// </summary>
        /// <returns></returns>
        public List<string> GetParameters()
        {
            List<string> returnParamList = new List<string>();
            //string allParams = Master.textboxParameters.Text;
            //returnParamList.AddRange(allParams.Split(','));
            return returnParamList;
        }

        public string FindParamValue(string paramToSearch, List<string> sourceList)
        {
            //int index = myList.FindIndex(a => a.Contains("Tennis"));
            int paramIndex = sourceList.FindIndex(a => a.StartsWith(paramToSearch));
            string value = string.Empty;

            if (paramIndex != -1)
            {
                value = sourceList[paramIndex].Split('=')[1];
            }
            return value;
        }
        public void buttonRemoveSelectedIndicatorGraph_Click()
        {
            //if (Master.indicatorList.SelectedValue.Equals("DAILY_NAV"))
            //{
            //    //do nothing - we need to make sure Daily NAV graph is always shown
            //    return;
            //}
            bool bHideMainGraphArea = false;

            if (chartMF.Series.FindByName(Master.indicatorList.SelectedValue) != null)
            {
                //chartMF.Series.Remove(chartMF.Series[Master.indicatorList.SelectedValue]);
                chartMF.Series[Master.indicatorList.SelectedValue].Enabled = false;
                if (Master.indicatorList.SelectedValue.Equals("Upper Band"))
                {
                    //we have removed Upped Band above, but we need to remove middle and lower band
                    //chartMF.Series.Remove(chartMF.Series["Middle Band"]);
                    chartMF.Series["Middle Band"].Enabled = false;
                    //chartMF.Series.Remove(chartMF.Series["Lower Band"]);
                    chartMF.Series["Lower Band"].Enabled = false;
                }
                else if (Master.indicatorList.SelectedValue.Equals("MACD"))
                {
                    //we have removed Upped Band above, but we need to remove middle and lower band
                    //chartMF.Series.Remove(chartMF.Series["Histogram"]);
                    chartMF.Series["Histogram"].Enabled = false;
                    //chartMF.Series.Remove(chartMF.Series["Signal"]);
                    chartMF.Series["Signal"].Enabled = false;
                    chartMF.ChartAreas[2].Visible = false;
                }
                else if (Master.indicatorList.SelectedValue.Equals("RSI"))
                {
                    chartMF.ChartAreas[1].Visible = false;
                }
            }

            for (int i = 0; i < Master.indicatorList.Items.Count; i++)
            {
                //if (Master.indicatorList.Items[i].Value.Equals("DAILY_NAV"))
                //{
                //    continue;
                //}
                if ((chartMF.Series.FindByName(Master.indicatorList.Items[i].Value) != null) &&
                        (chartMF.Series[Master.indicatorList.Items[i].Value].Enabled))
                {
                    if (Master.indicatorList.Items[i].Value.Equals("DAILY_NAV") || Master.indicatorList.Items[i].Value.Equals("SMA Fast") ||
                        Master.indicatorList.Items[i].Value.Equals("SMA Slow") || Master.indicatorList.Items[i].Value.Equals("EMA Fast") ||
                         Master.indicatorList.Items[i].Value.Equals("EMA Slow") ||
                         Master.indicatorList.Items[i].Value.Equals("WMA Fast") || Master.indicatorList.Items[i].Value.Equals("WMA Slow") ||
                         Master.indicatorList.Items[i].Value.Equals("Upper Band") || Master.indicatorList.Items[i].Value.Equals("Middle Band") ||
                         Master.indicatorList.Items[i].Value.Equals("Lower Band"))
                    {
                        bHideMainGraphArea = true;
                        break;
                    }
                }
            }
            chartMF.ChartAreas[0].Visible = bHideMainGraphArea;
        }


        public void buttonShowSelectedIndicatorGraph_Click()
        {
            string graphName = Master.indicatorList.SelectedValue;

            switch (graphName)
            {
                case "DAILY_NAV":
                    ShowDailyGraph(graphName);
                    break;
                case "RSI":
                    ShowRSIGraph(graphName);
                    break;
                case "SMA Fast":
                case "SMA Slow":
                    ShowSMAGraph(graphName);
                    break;
                case "EMA Fast":
                case "EMA Slow":
                    ShowEMAGraph(graphName);
                    break;
                case "WMA Fast":
                case "WMA Slow":
                    ShowWMAGraph(graphName);
                    break;
                case "MACD":
                    ShowMACDGraph(graphName);
                    break;
                case "Upper Band":
                    ShowBBANDSGraph(graphName);
                    break;
                default:
                    break;
            }
        }

        public void ShowDailyGraph(string graphName)
        {

            //List<string> paramList;
            DataManager dataManager = new DataManager();
            DataTable resultTable = dataManager.getNAVRecordsTable(Int32.Parse(Master.selectedSchemeCode.Text), Master.textboxFromDate.Text,
                                            Master.textboxToDate.Text);
            if ((resultTable != null) && (resultTable.Rows.Count > 0))
            {
                GridViewDaily.DataSource = resultTable;
                GridViewDaily.DataBind();
                ViewState["SchemeData"] = resultTable;

                chartMF.ChartAreas[0].Visible = true;
                if (chartMF.Series.FindByName(graphName) == null)
                {
                    chartMF.Series.Add(graphName);

                    chartMF.Series[graphName].Name = graphName;
                    chartMF.Series[graphName].ChartType = SeriesChartType.Line;
                    chartMF.Series[graphName].ChartArea = chartMF.ChartAreas[0].Name;
                    chartMF.Series[graphName].Legend = chartMF.Legends[0].Name;
                    chartMF.Series[graphName].LegendText = graphName;

                    chartMF.Series[graphName].XAxisType = AxisType.Secondary;
                    chartMF.Series[graphName].YAxisType = AxisType.Primary;

                    chartMF.Series[graphName].ToolTip = "Date: #VALX; " + graphName + ": #VALY{#.00}";

                    chartMF.Series[graphName].PostBackValue = graphName + "," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                    chartMF.Series[graphName].XValueMember = "NAVDATE";
                    chartMF.Series[graphName].XValueType = ChartValueType.Date;
                    chartMF.Series[graphName].YValueMembers = "NET_ASSET_VALUE";
                    chartMF.Series[graphName].YValueType = ChartValueType.Double;
                    //indicatorSeries.Points.DataBind(resultTable.Rows, "NAVDATE", "RSI", "");
                }
                else
                {
                    chartMF.Series[graphName].Enabled = true;
                }
                chartMF.Series[graphName].Points.Clear();
                chartMF.Series[graphName].Points.DataBind(resultTable.AsEnumerable(), "NAVDATE", "NET_ASSET_VALUE", "");
            }
        }
        public void ShowRSIGraph(string graphName)
        {

            //List<string> paramList;
            DataManager dataManager = new DataManager();
            string period = Master.textboxRSIPeriod.Text;

            //paramList = GetParameters();
            //period = FindParamValue("Period", paramList);
            period = (period == string.Empty) ? "14" : period;

            DataTable resultTable = dataManager.getRSIDataTableFromDailyNAV(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                                            Master.textboxToDate.Text, period);
            if ((resultTable != null) && (resultTable.Rows.Count > 0))
            {
                chartMF.ChartAreas[1].Visible = true;
                if (chartMF.Series.FindByName(graphName) == null)
                {
                    chartMF.Series.Add(graphName);

                    chartMF.Series[graphName].Name = graphName;
                    chartMF.Series[graphName].ChartType = SeriesChartType.Line;
                    chartMF.Series[graphName].ChartArea = chartMF.ChartAreas[1].Name;
                    chartMF.Series[graphName].Legend = chartMF.Legends[0].Name;
                    chartMF.Series[graphName].LegendText = graphName;

                    chartMF.Series[graphName].XAxisType = AxisType.Primary;
                    chartMF.Series[graphName].YAxisType = AxisType.Primary;

                    chartMF.Series[graphName].ToolTip = "Date: #VALX; " + graphName + ": #VALY{#.00}";

                    chartMF.Series[graphName].PostBackValue = graphName + "," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                    chartMF.Series[graphName].XValueMember = "NAVDATE";
                    chartMF.Series[graphName].XValueType = ChartValueType.Date;
                    chartMF.Series[graphName].YValueMembers = "RSI";
                    chartMF.Series[graphName].YValueType = ChartValueType.Double;
                    //indicatorSeries.Points.DataBind(resultTable.Rows, "NAVDATE", "RSI", "");
                }
                else
                {
                    chartMF.Series[graphName].Enabled = true;
                }
                chartMF.Series[graphName].Points.Clear();
                chartMF.Series[graphName].Points.DataBind(resultTable.AsEnumerable(), "NAVDATE", "RSI", "");
                //now create the graph
                //chartMF.DataManipulator.FinancialFormula(FinancialFormula.RelativeStrengthIndex, period, "DAILY_NAV:Y", graphName + ":Y");
                chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                    chartMF.Series[graphName].Points[0].XValue, graphName);
            }

        }

        public void ShowSMAGraph(string graphName)
        {
            //List<string> paramList;
            DataTable resultTable = null;
            DataManager dataManager = new DataManager();

            //paramList = GetParameters();
            //string period = FindParamValue("Period", paramList);
            string period = string.Empty;

            if (graphName.Equals("SMA Fast"))
            {
                period = (Master.textboxFastPeriod.Text == string.Empty) ? "14" : Master.textboxFastPeriod.Text;
                resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                              Master.textboxToDate.Text, small_fast_Period: Int32.Parse(period), long_slow_Period: -1, emaRequired: false, macdRequired: true, bbandsRequired: false);
            }
            else if (graphName.Equals("SMA Slow"))
            {
                period = (Master.textboxSlowPeriod.Text == string.Empty) ? "50" : Master.textboxSlowPeriod.Text;
                resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                              Master.textboxToDate.Text, small_fast_Period: -1, long_slow_Period: Int32.Parse(period), emaRequired: false, macdRequired: true, bbandsRequired: false);
            }

            if ((resultTable != null) && (resultTable.Rows.Count > 0))
            {
                chartMF.ChartAreas[0].Visible = true;
                if (chartMF.Series.FindByName(graphName) == null)
                {
                    chartMF.Series.Add(graphName);
                    chartMF.Series[graphName].Name = graphName;
                    chartMF.Series[graphName].ChartType = SeriesChartType.Line;
                    chartMF.Series[graphName].ChartArea = chartMF.ChartAreas[0].Name;
                    chartMF.Series[graphName].XAxisType = AxisType.Secondary;
                    chartMF.Series[graphName].YAxisType = AxisType.Primary;
                    chartMF.Series[graphName].Legend = chartMF.Legends[0].Name;

                    chartMF.Series[graphName].LegendText = graphName;
                    chartMF.Series[graphName].ToolTip = "Date: #VALX; " + graphName + ": #VALY{#.00}";

                    chartMF.Series[graphName].PostBackValue = graphName + "," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                    chartMF.Series[graphName].XValueMember = "NAVDATE";
                    chartMF.Series[graphName].XValueType = ChartValueType.Date;
                    chartMF.Series[graphName].YValueMembers = graphName.Equals("SMA Fast") ? "SMA_SMALL" : "SMA_LONG";
                    chartMF.Series[graphName].YValueType = ChartValueType.Double;
                }
                else
                {
                    chartMF.Series[graphName].Enabled = true;
                }

                chartMF.Series[graphName].Points.DataBind(resultTable.AsEnumerable(), "NAVDATE", graphName.Equals("SMA Fast") ? "SMA_SMALL" : "SMA_LONG", "");
                //now create the graph
                //chartMF.DataManipulator.FinancialFormula(FinancialFormula.RelativeStrengthIndex, period, "DAILY_NAV:Y", graphName + ":Y");
                //chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                //    indicatorSeries.Points[0].XValue, indicatorSeries);
                chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                    chartMF.Series[graphName].Points[0].XValue, graphName);
            }

        }

        public void ShowEMAGraph(string graphName)
        {
            //List<string> paramList;
            DataTable resultTable = null;
            DataManager dataManager = new DataManager();

            //paramList = GetParameters();
            //string period = FindParamValue("Period", paramList);
            string period = string.Empty;

            if (graphName.Equals("EMA Fast"))
            {
                period = (Master.textboxFastPeriod.Text == string.Empty) ? "14" : Master.textboxFastPeriod.Text;

                resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                              Master.textboxToDate.Text, small_fast_Period: Int32.Parse(period), long_slow_Period: -1, emaRequired: true, macdRequired: true, bbandsRequired: false);
            }
            else if (graphName.Equals("EMA Slow"))
            {
                period = (Master.textboxSlowPeriod.Text == string.Empty) ? "50" : Master.textboxSlowPeriod.Text;

                resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                              Master.textboxToDate.Text, small_fast_Period: -1, long_slow_Period: Int32.Parse(period), emaRequired: true, macdRequired: true, bbandsRequired: false);
            }

            if ((resultTable != null) && (resultTable.Rows.Count > 0))
            {
                chartMF.ChartAreas[0].Visible = true;
                if (chartMF.Series.FindByName(graphName) == null)
                {
                    chartMF.Series.Add(graphName);
                    chartMF.Series[graphName].ChartArea = chartMF.ChartAreas[0].Name;
                    chartMF.Series[graphName].XAxisType = AxisType.Secondary;
                    chartMF.Series[graphName].YAxisType = AxisType.Primary;
                    chartMF.Series[graphName].ChartType = SeriesChartType.Line;
                    chartMF.Series[graphName].Legend = chartMF.Legends[0].Name;

                    chartMF.Series[graphName].LegendText = graphName;
                    chartMF.Series[graphName].ToolTip = "Date: #VALX; " + graphName + ": #VALY{#.00}";

                    chartMF.Series[graphName].PostBackValue = graphName + "," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                    chartMF.Series[graphName].XValueMember = "NAVDATE";
                    chartMF.Series[graphName].XValueType = ChartValueType.Date;
                    chartMF.Series[graphName].YValueMembers = graphName.Equals("EMA Fast") ? "EMA_SMALL" : "EMA_LONG";
                    chartMF.Series[graphName].YValueType = ChartValueType.Double;
                }
                else
                {
                    chartMF.Series[graphName].Enabled = true;
                }

                chartMF.Series[graphName].Points.DataBind(resultTable.Rows, "NAVDATE", graphName.Equals("EMA Fast") ? "EMA_SMALL" : "EMA_LONG", "");
                //now create the graph
                //chartMF.DataManipulator.FinancialFormula(FinancialFormula.RelativeStrengthIndex, period, "DAILY_NAV:Y", graphName + ":Y");
                //chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                //    indicatorSeries.Points[0].XValue, indicatorSeries);
                chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                    chartMF.Series[graphName].Points[0].XValue, graphName);
            }

        }

        public void ShowWMAGraph(string graphName)
        {
            //List<string> paramList;
            //DataTable resultTable = null;
            //DataManager dataManager = new DataManager();

            //paramList = GetParameters();
            //string period = FindParamValue("Period", paramList);
            string period = string.Empty;

            if (graphName.Equals("WMA Fast"))
            {
                period = (Master.textboxFastPeriod.Text == string.Empty) ? "14" : Master.textboxFastPeriod.Text;


                //resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                //              Master.textboxToDate.Text, small_fast_Period: Int32.Parse(period), long_slow_Period: -1, emaRequired: true, macdRequired: true, bbandsRequired: false);
            }
            else if (graphName.Equals("WMA Slow"))
            {
                period = (Master.textboxSlowPeriod.Text == string.Empty) ? "50" : Master.textboxSlowPeriod.Text;


                //resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                //              Master.textboxToDate.Text, small_fast_Period: -1, long_slow_Period: Int32.Parse(period), emaRequired: true, macdRequired: true, bbandsRequired: false);
            }
            chartMF.ChartAreas[0].Visible = true;
            if (chartMF.Series.FindByName(graphName) == null)
            {
                chartMF.Series.Add(graphName);
                chartMF.Series[graphName].ChartArea = chartMF.ChartAreas[0].Name;
                chartMF.Series[graphName].XAxisType = AxisType.Secondary;
                chartMF.Series[graphName].YAxisType = AxisType.Primary;
                chartMF.Series[graphName].ChartType = SeriesChartType.Line;
                chartMF.Series[graphName].Legend = chartMF.Legends[0].Name;

                chartMF.Series[graphName].LegendText = graphName;
                chartMF.Series[graphName].ToolTip = "Date: #VALX; " + graphName + ": #VALY{#.00}";

                chartMF.Series[graphName].PostBackValue = graphName + "," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                //indicatorSeries.XValueMember = "NAVDATE";
                //indicatorSeries.XValueType = ChartValueType.Date;
                //indicatorSeries.YValueMembers = "SMA_SMALL";
                //indicatorSeries.YValueType = ChartValueType.Double;
                //indicatorSeries.Points.DataBind(resultTable.Rows, "NAVDATE", "SMA_SMALL", "");
                //now create the graph
            }
            else
            {
                chartMF.Series[graphName].Enabled = true;
            }

            chartMF.DataManipulator.IsStartFromFirst = true;
            chartMF.DataManipulator.FinancialFormula(FinancialFormula.WeightedMovingAverage, period, "DAILY_NAV:Y", graphName + ":Y");
            //chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
            //    indicatorSeries.Points[0].XValue, indicatorSeries);
            chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                chartMF.Series[graphName].Points[0].XValue, graphName);
        }

        public void ShowBBANDSGraph(string graphName)
        {
            //List<string> paramList;
            //paramList = GetParameters();

            //string period = FindParamValue("Period", paramList);
            string period = (Master.textboxFastPeriod.Text == string.Empty) ? "20" : Master.textboxFastPeriod.Text;

            //string stddev = FindParamValue("Standardd Deviation", paramList);
            string stddev = (Master.textboxStdDev.Text == string.Empty) ? "2" : Master.textboxStdDev.Text;

            chartMF.ChartAreas[0].Visible = true;
            //now create the graph
            if (chartMF.Series.FindByName(graphName) == null)
            {
                Series indicatorSeries = chartMF.Series.Add(graphName);
                indicatorSeries.ChartArea = chartMF.ChartAreas[0].Name;
                indicatorSeries.XAxisType = AxisType.Secondary;
                indicatorSeries.YAxisType = AxisType.Primary;
                indicatorSeries.ChartType = SeriesChartType.Line;
                indicatorSeries.LegendText = "Upper Band";
                indicatorSeries.ToolTip = "Date: #VALX; Upper Band: #VALY{#.00}";
                indicatorSeries.PostBackValue = "Upper Band," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                //we also need to create one more series to show lower band.
                Series lowerBandSeries = chartMF.Series.Add("Lower Band");
                lowerBandSeries.ChartArea = chartMF.ChartAreas[0].Name;
                lowerBandSeries.XAxisType = AxisType.Secondary;
                lowerBandSeries.YAxisType = AxisType.Primary;
                lowerBandSeries.ChartType = SeriesChartType.Line;
                lowerBandSeries.Legend = chartMF.Legends[0].Name;
                lowerBandSeries.LegendText = "Lower Band";
                lowerBandSeries.ToolTip = "Date: #VALX; Lower Band: #VALY{#.00}";
                lowerBandSeries.PostBackValue = "Lower Band," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                //we also need to create one more series to show middle band.
                Series middleBandSeries = chartMF.Series.Add("Middle Band");
                middleBandSeries.ChartArea = chartMF.ChartAreas[0].Name;
                middleBandSeries.XAxisType = AxisType.Secondary;
                middleBandSeries.YAxisType = AxisType.Primary;
                middleBandSeries.ChartType = SeriesChartType.Line;
                middleBandSeries.Legend = chartMF.Legends[0].Name;
                middleBandSeries.LegendText = "Middle Band";
                middleBandSeries.ToolTip = "Date: #VALX; Middle Band: #VALY{#.00}";
                middleBandSeries.PostBackValue = "Middle Band," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";
            }
            else
            {
                chartMF.Series[graphName].Enabled = true;
                chartMF.Series["Lower Band"].Enabled = true;
                chartMF.Series["Middle Band"].Enabled = true;
            }


            chartMF.DataManipulator.IsStartFromFirst = true;
            chartMF.DataManipulator.FinancialFormula(FinancialFormula.BollingerBands, period + "," + stddev, "DAILY_NAV:Y", "Upper Band:Y,Lower Band:Y");
            chartMF.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, period, "DAILY_NAV:Y", "Middle Band:Y");

            chartMF.DataManipulator.InsertEmptyPoints(double.Parse(period), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                chartMF.Series[graphName].Points[0].XValue, "Upper Band, Middle Band, Lower Band");
        }

        public void ShowMACDGraph(string graphName)
        {
            //List<string> paramList;
            //string fastPeriod, slowPeriod;
            DataManager dataManager = new DataManager();

            //paramList = GetParameters();

            //fastPeriod = FindParamValue("Fast Period", paramList);
            string fastPeriod = (Master.textboxFastPeriod.Text == string.Empty) ? "12" : Master.textboxFastPeriod.Text;
            //slowPeriod = FindParamValue("Slow Period", paramList);
            string slowPeriod = (Master.textboxSlowPeriod.Text == string.Empty) ? "26" : Master.textboxSlowPeriod.Text;

            DataTable resultTable = dataManager.GetSMA_EMA_MACD_BBANDS_Table(Int32.Parse(Master.selectedSchemeCode.Text), "NET_ASSET_VALUE", Master.textboxFromDate.Text,
                            Master.textboxToDate.Text, Int32.Parse(fastPeriod), Int32.Parse(slowPeriod), emaRequired: true, macdRequired: true, bbandsRequired: false);

            if (resultTable != null)
            {
                //graphName = MACD
                chartMF.ChartAreas[2].Visible = true;
                if (chartMF.Series.FindByName(graphName) == null)
                {
                    Series macdSeries = chartMF.Series.Add(graphName);
                    macdSeries.ChartArea = chartMF.ChartAreas[2].Name;
                    macdSeries.XAxisType = AxisType.Primary;
                    macdSeries.YAxisType = AxisType.Primary;
                    macdSeries.ChartType = SeriesChartType.Line;
                    macdSeries.Legend = chartMF.Legends[0].Name;
                    macdSeries.LegendText = graphName;
                    macdSeries.XValueMember = "NAVDATE";
                    macdSeries.XValueType = ChartValueType.Date;
                    macdSeries.YValueMembers = "MACD";
                    macdSeries.YValueType = ChartValueType.Double;
                    macdSeries.ToolTip = "Date: #VALX; " + graphName + ": #VALY{#.00}";
                    macdSeries.PostBackValue = graphName + "," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                    //first get EMA1 for 12 & EMA2 for 26
                    Series historySeries = chartMF.Series.Add("Histogram");
                    historySeries.ChartArea = chartMF.ChartAreas[2].Name;
                    historySeries.XAxisType = AxisType.Primary;
                    historySeries.YAxisType = AxisType.Primary;
                    historySeries.ChartType = SeriesChartType.Column;
                    historySeries.Legend = chartMF.Legends[0].Name;
                    historySeries.LegendText = "MACD Histogram";
                    historySeries.XValueMember = "NAVDATE";
                    historySeries.XValueType = ChartValueType.Date;
                    historySeries.YValueMembers = "MACD_Hist";
                    historySeries.YValueType = ChartValueType.Double;
                    historySeries.ToolTip = "Date: #VALX; Histogram: #VALY{#.00}";
                    historySeries.PostBackValue = "Histogram," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";

                    //we also need to create one more series to show middle band.
                    Series signalSeries = chartMF.Series.Add("Signal");
                    signalSeries.ChartArea = chartMF.ChartAreas[2].Name;
                    signalSeries.XAxisType = AxisType.Primary;
                    signalSeries.YAxisType = AxisType.Primary;
                    signalSeries.ChartType = SeriesChartType.Line;
                    signalSeries.Legend = chartMF.Legends[0].Name;
                    signalSeries.LegendText = "MACD Signal";
                    signalSeries.XValueMember = "NAVDATE";
                    signalSeries.XValueType = ChartValueType.Date;
                    signalSeries.YValueMembers = "MACD_Signal";
                    signalSeries.YValueType = ChartValueType.Double;
                    signalSeries.ToolTip = "Date: #VALX; Signal: #VALY{#.00}";
                    signalSeries.PostBackValue = "Signal," + Master.selectedFundName.Text + "," + "#VALX,#VALY{#.00}";
                }
                else
                {
                    chartMF.Series[graphName].Enabled = true;
                    chartMF.Series["Histogram"].Enabled = true;
                    chartMF.Series["Signal"].Enabled = true;
                }

                chartMF.Series[graphName].Points.DataBind(resultTable.Rows, "NAVDATE", "MACD", "");
                chartMF.Series["Signal"].Points.DataBind(resultTable.Rows, "NAVDATE", "MACD_Signal", "");
                chartMF.Series["Histogram"].Points.DataBind(resultTable.Rows, "NAVDATE", "MACD_Hist", "");
                chartMF.DataManipulator.InsertEmptyPoints(double.Parse(slowPeriod), IntervalType.Days, 0, IntervalType.Days, chartMF.Series["DAILY_NAV"].Points[0].XValue,
                    chartMF.Series[graphName].Points[0].XValue, "MACD, Signal, Histogram");
            }
        }

        protected void chartMF_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartMF.Annotations.Count > 0)
                    chartMF.Annotations.Clear();

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

                if (seriesName.Equals("DAILY_NAV") || seriesName.Equals("SMA Fast") || seriesName.Equals("SMA Slow") || seriesName.Equals("EMA Fast") ||
                    seriesName.Equals("EMA Slow") ||
                    seriesName.Equals("WMA Fast") || seriesName.Equals("WMA Slow") || seriesName.Equals("Upper Band") || seriesName.Equals("Middle Band") ||
                    seriesName.Equals("Lower Band"))
                {
                    HA.AxisY = chartMF.ChartAreas[0].AxisY;
                    VA.AxisY = chartMF.ChartAreas[0].AxisY;
                    ra.AxisY = chartMF.ChartAreas[0].AxisY;

                    HA.AxisX = chartMF.ChartAreas[0].AxisX2;
                    VA.AxisX = chartMF.ChartAreas[0].AxisX2;
                    ra.AxisX = chartMF.ChartAreas[0].AxisX2;

                    HA.ClipToChartArea = chartMF.ChartAreas[0].Name;
                }
                else if (seriesName.Equals("RSI"))
                {
                    HA.AxisY = chartMF.ChartAreas[1].AxisY;
                    VA.AxisY = chartMF.ChartAreas[1].AxisY;
                    ra.AxisY = chartMF.ChartAreas[1].AxisY;

                    HA.AxisX = chartMF.ChartAreas[1].AxisX;
                    VA.AxisX = chartMF.ChartAreas[1].AxisX;
                    ra.AxisX = chartMF.ChartAreas[1].AxisX;

                    HA.ClipToChartArea = chartMF.ChartAreas[1].Name;
                }
                else if (seriesName.Equals("MACD") || seriesName.Equals("Histogram") || seriesName.Equals("Signal"))
                {
                    HA.AxisY = chartMF.ChartAreas[2].AxisY;
                    VA.AxisY = chartMF.ChartAreas[2].AxisY;
                    ra.AxisY = chartMF.ChartAreas[2].AxisY;

                    HA.AxisX = chartMF.ChartAreas[2].AxisX;
                    VA.AxisX = chartMF.ChartAreas[2].AxisX;
                    ra.AxisX = chartMF.ChartAreas[2].AxisX;

                    HA.ClipToChartArea = chartMF.ChartAreas[2].Name;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                HA.ToolTip = postBackValues[3];
                chartMF.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                VA.ToolTip = postBackValues[2];
                chartMF.Annotations.Add(VA);

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
                //ra.SmartLabelStyle = sl;
                ra.Text = postBackValues[1] + "\n" + "Date:" + postBackValues[2] + "\n" + seriesName + ":" + postBackValues[3];

                chartMF.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }

        public void ResetAllIndicatorGraphs()
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);

            for (int i = 0; i < Master.indicatorList.Items.Count; i++)
            {
                //if (Master.indicatorList.Items[i].Value.Equals("DAILY_NAV"))
                //{
                //    continue;
                //}
                if ((chartMF.Series.FindByName(Master.indicatorList.Items[i].Value) != null) &&
                        (chartMF.Series[Master.indicatorList.Items[i].Value].Enabled))
                {
                    if (Master.indicatorList.Items[i].Value.Equals("DAILY_NAV"))
                    {
                        ShowDailyGraph(Master.indicatorList.Items[i].Value);
                    }
                    else if (Master.indicatorList.Items[i].Value.Equals("RSI"))
                    {
                        ShowRSIGraph(Master.indicatorList.Items[i].Value);
                    }
                    else if (Master.indicatorList.Items[i].Value.Equals("SMA Fast") || Master.indicatorList.Items[i].Value.Equals("SMA Slow"))
                    {
                        ShowSMAGraph(Master.indicatorList.Items[i].Value);
                    }
                    else if (Master.indicatorList.Items[i].Value.Equals("EMA Fast") || Master.indicatorList.Items[i].Value.Equals("EMA Slow"))
                    {
                        ShowEMAGraph(Master.indicatorList.Items[i].Value);
                    }
                    else if (Master.indicatorList.Items[i].Value.Equals("WMA Fast") || Master.indicatorList.Items[i].Value.Equals("WMA Slow"))
                    {
                        ShowWMAGraph(Master.indicatorList.Items[i].Value);
                    }
                    else if (Master.indicatorList.Items[i].Value.Equals("Upper Band"))
                    {
                        ShowBBANDSGraph(Master.indicatorList.Items[i].Value);
                    }
                    else if (Master.indicatorList.Items[i].Value.Equals("MACD"))
                    {
                        ShowMACDGraph(Master.indicatorList.Items[i].Value);
                    }
                }
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);

        }
        public void buttonShowGraph_Click()
        {
            ViewState["SchemeData"] = null;
            //ShowGraph();
            ResetAllIndicatorGraphs();
        }


        void buttonShowGrid_Click()
        {
            GridViewDaily.Enabled = !GridViewDaily.Enabled;
            GridViewDaily.Visible = !GridViewDaily.Visible;
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["SchemeData"];
            GridViewDaily.DataBind();
        }

        //public void buttonDesc_Click()
        //{
        //    if (Master.bulletedlistDesc.Visible)
        //        Master.bulletedlistDesc.Visible = false;
        //    else
        //        Master.bulletedlistDesc.Visible = true;
        //}

        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }

    }
}