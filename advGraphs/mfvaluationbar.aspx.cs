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
    public partial class mfvaluationbar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                Master.OnDoEventShowGraph += new advancegraphs.DoEventShowGraph(ShowBarValuation);
                Master.OnDoEventShowGrid += new advancegraphs.DoEventShowGrid(buttonShowGrid_Click);
                if (!IsPostBack)
                {
                    ViewState["VALUATION_DATA"] = null;

                    ManagePanels();

                    Master.LoadPortfolioMF();

                    if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
                    {
                        Master.dropdownPortfolioMF.SelectedValue = Session["MFPORTFOLIOMASTERROWID"].ToString();
                        //Master.LoadPortfolioFundNameList();
                        //Master.dropdownFundNameList.Items[0].Text = "Show All";
                        //Master.dropdownFundNameList.SelectedValue = "-1";
                        //Master.textbox_selectedFundName.Text = "Show All";
                        //Master.textbox_selectedSchemeCode.Text = "Show All";

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);
                        //now show the backtest graph
                        ShowBarValuation();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);

                    }
                    this.Title = "Porfolio valuation Cost Vs Value graph";
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
            Master.InitializePanels(bStocksGraph: false);

            Master.panelOnlineFundHouse.Enabled = false;
            Master.panelOnlineFundHouse.Visible = false;

            Master.panelSearchListFundName.Enabled = false;
            Master.panelSearchListFundName.Visible = false;

            Master.panelCommonControls.Enabled = false;
            Master.panelCommonControls.Visible = false;

            Master.panelMainParam.Enabled = false;
            Master.panelMainParam.Visible = false;

            Master.panelDescription.Enabled = false;
            Master.panelDescription.Visible = false;


            Master.buttonDesc.Enabled = false;
            Master.buttonDesc.Visible = false;

            Master.buttonShowHideParam.Enabled = false;
            Master.buttonShowHideParam.Visible = false;

            Master.buttonTopSecion.Enabled = false;
            Master.buttonTopSecion.Visible = false;
        }

        public void ShowBarValuation()
        {
            DataTable valuationTable = null;

            DataManager dataMgr = new DataManager();

            if (Master.dropdownPortfolioMF.SelectedIndex > 0)
            {
                ViewState["VALUATION_DATA"] = null;
                valuationTable = dataMgr.GetMFValuationBarGraph(Master.dropdownPortfolioMF.SelectedValue);

                if ((valuationTable != null) && (valuationTable.Rows.Count > 0))
                {
                    if (chartPortfolioValuation.Annotations.Count > 0)
                        chartPortfolioValuation.Annotations.Clear();

                    gridviewPortfolioValuation.DataSource = valuationTable;
                    gridviewPortfolioValuation.DataBind();

                    ViewState["VALUATION_DATA"] = valuationTable;

                    //chartPortfolioValuation.DataSource = valuationTable;
                    //chartPortfolioValuation.DataBind();
                    //if (chartPortfolioValuation.Series.FindByName("Cost") == null)
                    //{
                    //    chartPortfolioValuation.Series.Add("Cost");
                    //    chartPortfolioValuation.Series["Cost"].Name = "Cost";
                    //    (chartPortfolioValuation.Series["Cost"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    //    (chartPortfolioValuation.Series["Cost"]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;
                    //    chartPortfolioValuation.Series["Cost"].Legend = chartPortfolioValuation.Legends[0].Name;
                    //    chartPortfolioValuation.Series["Cost"].LegendText = "Cost";
                    //    chartPortfolioValuation.Series["Cost"].LegendToolTip = "Cost";
                    //    chartPortfolioValuation.Series["Cost"].IsValueShownAsLabel = true;


                    //    (chartPortfolioValuation.Series["Cost"]).YValuesPerPoint = 10;
                    //    //(chartPortfolioValuation.Series["Cost"]).YValueType = ChartValueType.String;
                    //    (chartPortfolioValuation.Series["Cost"]).ToolTip = "#VALX; Total Cost:#VALY (Click to see details)";
                    //    //chartPortfolioValuation.Series["Cost"].PostBackValue = "Cost" + ",#VALX,#VALY,#VAL2,#VAL3,#VAL4,#VAL5,#VAL6,#VAL7,#VAL8,#VAL9,#VAL10";
                    //    chartPortfolioValuation.Series["Cost"].PostBackValue = "Cost" + ",#VALX,#VALY,#VALY2,#VALY3,#VALY4,#VALY5,#VALY6,#VALY7,#VALY8,#VALY9,#VALY10";

                    //}
                    (chartPortfolioValuation.Series["Cost"]).Points.DataBindXY(valuationTable.Rows, "FundName", valuationTable.Rows,
                         "CumulativeCost,FundHouse,SCHEME_CODE,FirstPurchaseDate,CumulativeUnits,CurrentNAV,NAVDate,CumulativeValue,TotalYearsInvested,TotalARR");
                    //if (chartPortfolioValuation.Series.FindByName("Value") == null)
                    //{
                    //    chartPortfolioValuation.Series.Add("Value");
                    //    chartPortfolioValuation.Series["Value"].Name = "Value";
                    //    (chartPortfolioValuation.Series["Value"]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    //    (chartPortfolioValuation.Series["Value"]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;
                    //    chartPortfolioValuation.Series["Value"].Legend = chartPortfolioValuation.Legends[0].Name;
                    //    chartPortfolioValuation.Series["Value"].LegendText = "Value";
                    //    chartPortfolioValuation.Series["Value"].LegendToolTip = "Value";
                    //    chartPortfolioValuation.Series["Value"].IsValueShownAsLabel = true;
                    //    (chartPortfolioValuation.Series["Value"]).YValuesPerPoint = 10;
                    //    //(chartPortfolioValuation.Series["Value"]).YValueType = ChartValueType.String;
                    //    (chartPortfolioValuation.Series["Value"]).ToolTip = "#VALX; Total Value:#VALY (Click to see details)";
                    //    chartPortfolioValuation.Series["Value"].PostBackValue = "Value" + ",#VALX,#VALY,#VALY2,#VALY3,#VALY4,#VALY5,#VALY6,#VALY7,#VALY8,#VALY9,#VALY10";

                    //}
                    (chartPortfolioValuation.Series["Value"]).Points.DataBindXY(valuationTable.Rows, "FundName", valuationTable.Rows,
                         "CumulativeValue,FundHouse,SCHEME_CODE,FirstPurchaseDate,CumulativeUnits,CumulativeCost,CurrentNAV,NAVDate,TotalYearsInvested,TotalARR");
                }
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

            if (chartPortfolioValuation.Annotations.Count > 0)
                chartPortfolioValuation.Annotations.Clear();

            //"Cost, #VALX, #VALY, Fund Name=#VALX,Cumulative Cost=#VALY,Fund House=#VALY2,Scheme Code=#VALY3,First Purchase Date=#VALY4,Current NAV=#VALY5,NAV Date=#VALY6,Cumulative Units=#VALY7,Cumulative Value=#VALY8, Total Years Invested=#VALY9, Total ARR=#VALY10">
            //OR
            //AnnotationClicked

            postBackValues = e.PostBackValue.Split(',');

            if (postBackValues[0].Equals("AnnotationClicked"))
                return;


            if (ViewState["VALUATION_DATA"] != null)
            {
                DataTable valuationTable = (DataTable)ViewState["VALUATION_DATA"];

                valuationTable.DefaultView.RowFilter = "FundName = '" + postBackValues[1].Replace("'", "''") + "'";
                if (valuationTable.DefaultView.Count > 0)
                {

                    RectangleAnnotation ra = new RectangleAnnotation();


                    ra.Name = postBackValues[0];
                    ra.AxisY = chartPortfolioValuation.ChartAreas[0].AxisY;
                    ra.AxisX = chartPortfolioValuation.ChartAreas[0].AxisX;
                    //ra.ClipToChartArea = chartPortfolioValuation.ChartAreas[0].Name;
                    ra.IsSizeAlwaysRelative = true;

                    DataPoint dp = null;

                    if (postBackValues[0].Equals("Cost"))
                    {
                        //ra.Text = "Fund House:" + postBackValues[3] + "\nFund Name:" + postBackValues[1] + "\nScheme Code:" + postBackValues[4] +
                        //    "\nFirst Purchase Date:" + postBackValues[5] + "\nCurrent NAV:" + postBackValues[6] + "\nNAV Date:" +
                        //    postBackValues[7] + "\nTotal Units:" + postBackValues[8] + "\nTotal Cost:" + postBackValues[2] + "\ncurrent Value:" + postBackValues[9] +
                        //    "\nTotal Years Invested:" + postBackValues[10] + "\nTotal ARR:" + postBackValues[11];

                        ra.Text = "Fund House:" + valuationTable.DefaultView[0]["FundHouse"] +
                            "\nFund Name: " + valuationTable.DefaultView[0]["FundName"] +
                            "\nScheme Code:" + valuationTable.DefaultView[0]["SCHEME_CODE"] +
                            "\nFirst Purchase Date:" + System.Convert.ToDateTime(valuationTable.DefaultView[0]["FirstPurchaseDate"]).ToShortDateString() +
                            "\nCurrent NAV:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["CurrentNAV"]) +
                            "\nNAV Date:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : System.Convert.ToDateTime(valuationTable.DefaultView[0]["NAVDate"]).ToShortDateString()) +
                            "\nTotal Units:" + valuationTable.DefaultView[0]["CumulativeUnits"] +
                            "\nTotal Cost:" + valuationTable.DefaultView[0]["CumulativeCost"] +
                            "\nCurrent Value:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["CumulativeValue"]) +
                            "\nTotal Years Invested:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["TotalYearsInvested"]) +
                            "\nTotal ARR:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["TotalARR"] + "%") +
                            "\n\n(Click the border to clear details)";


                        foreach (DataPoint item in chartPortfolioValuation.Series["Cost"].Points.FindAllByValue(System.Convert.ToDouble(postBackValues[2])))
                        {
                            if (item.AxisLabel.Equals(postBackValues[1]))
                            {
                                dp = item;
                                //dp = new DataPoint(item.XValue, item.YValues);
                                break;
                            }
                        }
                        //dp = chartPortfolioValuation.Series["Cost"].Points.FindByValue(System.Convert.ToDouble(postBackValues[2]));
                    }
                    else
                    {
                        //ra.Text = "Fund House:" + postBackValues[3] + "\nFund Name:" + postBackValues[1] + "\nScheme Code:" + postBackValues[4] +
                        //    "\nFirst Purchase Date:" + postBackValues[5] + "\nCurrent NAV:" + postBackValues[6] + "\nNAV Date:" +
                        //    postBackValues[7] + "\nTotal Units:" + postBackValues[8] + "\nTotal Cost:" + postBackValues[9] + "\ncurrent Value:" + postBackValues[2] +
                        //    "\nTotal Years Invested:" + postBackValues[10] + "\nTotal ARR:" + postBackValues[11];

                        ra.Text = "Fund House:" + valuationTable.DefaultView[0]["FundHouse"] +
                            "\nFund Name: " + valuationTable.DefaultView[0]["FundName"] +
                            "\nScheme Code:" + valuationTable.DefaultView[0]["SCHEME_CODE"] +
                            "\nFirst Purchase Date:" + System.Convert.ToDateTime(valuationTable.DefaultView[0]["FirstPurchaseDate"]).ToShortDateString() +
                            "\nCurrent NAV:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["CurrentNAV"]) +
                            "\nNAV Date:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : System.Convert.ToDateTime(valuationTable.DefaultView[0]["NAVDate"]).ToShortDateString()) +
                            "\nTotal Units:" + valuationTable.DefaultView[0]["CumulativeUnits"] +
                            "\nTotal Cost:" + valuationTable.DefaultView[0]["CumulativeCost"] +
                            "\nCurrent Value:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["CumulativeValue"]) +
                            "\nTotal Years Invested:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["TotalYearsInvested"]) +
                            "\nTotal ARR:" + (System.Convert.ToDouble(valuationTable.DefaultView[0]["CurrentNAV"]) == 0 ? "NA" : valuationTable.DefaultView[0]["TotalARR"] + "%") +
                            "\n\n(Click the border to clear details)";

                        foreach (DataPoint item in chartPortfolioValuation.Series["Value"].Points.FindAllByValue(System.Convert.ToDouble(postBackValues[2])))
                        {
                            if (item.AxisLabel.Equals(postBackValues[1]))
                            {
                                dp = item;
                                //dp = item.Clone();
                                //dp = new DataPoint(item.XValue, item.YValues);
                                break;
                            }
                        }
                        //dp = chartPortfolioValuation.Series["Value"].Points.FindByValue(System.Convert.ToDouble(postBackValues[2]));
                    }

                    if (dp != null)
                    {
                        //ra.Alignment = ContentAlignment.MiddleLeft;
                        ra.SetAnchor(dp);
                        ra.IsMultiline = true;
                        ra.LineDashStyle = ChartDashStyle.Solid;
                        ra.LineColor = Color.Black;//.Blue;
                        ra.ForeColor = Color.Black;
                        ra.LineWidth = 1;


                        //ra.AnchorAlignment = ContentAlignment.TopRight;
                        //ra.IsSizeAlwaysRelative = true;

                        ra.PostBackValue = "AnnotationClicked";
                        chartPortfolioValuation.Annotations.Add(ra);
                    }
                    //ra.AnchorX = System.Convert.ToDouble(postBackValues[2]);
                    //ra.AnchorY = 0;
                    //ra.AnchorOffsetY = 3;
                    //ra.AnchorOffsetX = 90;
                }
            }
        }
    }
}