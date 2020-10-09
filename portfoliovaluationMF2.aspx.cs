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

namespace Analytics
{
    public partial class portfoliovaluationMF2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmailId"] != null)
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
                if (Session["PortfolioNameMF"] != null)
                {
                    string fileName = Session["PortfolioNameMF"].ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(fileName);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'default';", true);
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
        public void ShowGraph(string fileName)
        {
            bool bIsTestOn = true;
            DataTable portfolioTable = null;
            DataTable valuationTable = null;
            DataTable indexTable = null;
            DataTable tempData = null;
            string folderPath = Server.MapPath("~/scriptdata/");
            string currentFundName;
            string searchPath;
            //int markerInterval = 10;
            //int i = 0;
            double tempQty = 0;
            double tempCost;
            string fromDate = "", toDate = "";
            string expression = "";
            DataRow[] filteredRows = null;
            DataRow[] scriptRows;

            try
            {
                if (File.Exists(fileName))
                {
                    if (Session["TestDataFolderMF"] != null)
                    {
                        folderPath = Session["TestDataFolderMF"].ToString();
                    }

                    //portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestOn);
                    portfolioTable = MFAPI.openMFPortfolio(folderPath, fileName);

                    if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                    {
                        if (Session["IsTestOn"] != null)
                        {
                            bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                        }

                        if (Session["TestDataFolder"] != null)
                        {
                            folderPath = Session["TestDataFolder"].ToString();
                        }

                        valuationTable = MFAPI.GetMFValuationLine(folderPath, fileName, portfolioTable:portfolioTable);
                        ViewState["FetchedData"] = valuationTable;
                        gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
                        gridviewPortfolioValuation.DataBind();
                    }

                    if ((System.Convert.ToInt32((ViewState["SelectedIndex"].ToString())) != ddlIndex.SelectedIndex) &&
                            (ddlIndex.SelectedIndex > 0))
                    {
                        if (Session["IsTestOn"] != null)
                        {
                            bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                        }

                        if (Session["TestDataFolder"] != null)
                        {
                            folderPath = Session["TestDataFolder"].ToString();
                        }

                        //Some index is selected by user
                        indexTable = StockApi.getDailyAlternate(folderPath, ddlIndex.SelectedValue, bIsTestModeOn: false, bSaveData: false,
                                                                apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedIndexData"] = indexTable;
                        ViewState["SelectedIndex"] = ddlIndex.SelectedIndex;
                    }
                    //else
                    //{
                    if (ViewState["FromDate"] != null)
                        fromDate = ViewState["FromDate"].ToString();
                    if (ViewState["ToDate"] != null)
                        toDate = ViewState["ToDate"].ToString();

                    if ((fromDate.Length > 0) && (toDate.Length > 0))
                    {
                        tempData = (DataTable)ViewState["FetchedData"];
                        expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                        filteredRows = tempData.Select(expression);
                        if ((filteredRows != null) && (filteredRows.Length > 0))
                            valuationTable = filteredRows.CopyToDataTable();

                        tempData.Clear();
                        tempData = null;

                        if (ViewState["FetchedIndexData"] != null)
                        {
                            tempData = (DataTable)ViewState["FetchedIndexData"];
                            expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                            filteredRows = tempData.Select(expression);
                            if ((filteredRows != null) && (filteredRows.Length > 0))
                                indexTable = filteredRows.CopyToDataTable();
                        }
                    }
                    else
                    {
                        valuationTable = (DataTable)ViewState["FetchedData"];
                        indexTable = (DataTable)ViewState["FetchedIndexData"];
                    }
                    //}

                    if (valuationTable != null)
                    {
                        if (chartPortfolioValuation.Annotations.Count > 0)
                            chartPortfolioValuation.Annotations.Clear();

                        DataTable fundNameTable = portfolioTable.DefaultView.ToTable(true, "FundName");

                        foreach (DataRow fundNameRow in fundNameTable.Rows)
                        {
                            currentFundName = fundNameRow["FundName"].ToString();

                            scriptRows = valuationTable.Select("SCHEME_NAME='" + currentFundName + "'");

                            if (scriptRows.Length > 0)
                            {
                                if (listboxScripts.Items.FindByValue(currentFundName) == null)
                                {
                                    ListItem li = new ListItem(currentFundName, currentFundName);
                                    listboxScripts.Items.Add(li);
                                }

                                if (chartPortfolioValuation.Series.FindByName(currentFundName) == null)
                                {
                                    chartPortfolioValuation.Series.Add(currentFundName);
                                    chartPortfolioValuation.Series[currentFundName].Name = currentFundName;
                                    (chartPortfolioValuation.Series[currentFundName]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                                    (chartPortfolioValuation.Series[currentFundName]).ChartArea = chartPortfolioValuation.ChartAreas[0].Name;

                                    chartPortfolioValuation.Series[currentFundName].Legend = chartPortfolioValuation.Legends[0].Name;
                                    chartPortfolioValuation.Series[currentFundName].LegendText = currentFundName;
                                    chartPortfolioValuation.Series[currentFundName].LegendToolTip = currentFundName;
                                    chartPortfolioValuation.Series[currentFundName].ToolTip = currentFundName + ": Date:#VALX; Cumulative Units:#VALY3; Value:#VALY (Click to see details)";
                                    chartPortfolioValuation.Series[currentFundName].PostBackValue = currentFundName + ",#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                                }

                                tempQty = 0;
                                tempCost = 0.00;
                                (chartPortfolioValuation.Series[currentFundName]).Points.Clear();
                                foreach (DataRow itemRow in scriptRows)
                                {
                                    (chartPortfolioValuation.Series[currentFundName]).Points.AddXY(itemRow["DATE"], itemRow["CurrentValue"]);

                                    if ((itemRow["CumulativeUnits"] != System.DBNull.Value) && ((tempQty == 0) || (tempQty != System.Convert.ToDouble(itemRow["CumulativeUnits"]))))
                                    {
                                        tempQty = System.Convert.ToDouble(itemRow["CumulativeUnits"]);
                                        (chartPortfolioValuation.Series[currentFundName]).Points[(chartPortfolioValuation.Series[currentFundName]).Points.Count - 1].MarkerSize = 10;
                                        (chartPortfolioValuation.Series[currentFundName]).Points[(chartPortfolioValuation.Series[currentFundName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                                        //(chartPortfolioValuation.Series[currentFundName]).Points[(chartPortfolioValuation.Series[currentFundName]).Points.Count - 1].Label = itemRow["CumulativeUnits"].ToString();
                                    }
                                    (chartPortfolioValuation.Series[currentFundName]).Points[(chartPortfolioValuation.Series[currentFundName]).Points.Count - 1].PostBackValue =
                                        itemRow["SCHEME_NAME"] + "," + itemRow["DATE"] + "," + itemRow["CurrentValue"] + "," + itemRow["CumulativeUnits"] + "," + itemRow["CumulativeCost"];
                                }
                                (chartPortfolioValuation.Series[currentFundName]).Points[(chartPortfolioValuation.Series[currentFundName]).Points.Count - 1].MarkerSize = 10;
                                (chartPortfolioValuation.Series[currentFundName]).Points[(chartPortfolioValuation.Series[currentFundName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                            }
                        }

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
                            (chartPortfolioValuation.Series[ddlIndex.SelectedValue]).Points.DataBindXY(indexTable.Rows, "Date", indexTable.Rows, "Open,High,Low,Close");

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

                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message + "');", true);
            }
        }
        protected void gridviewPortfolioValuation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridviewPortfolioValuation.PageIndex = e.NewPageIndex;
            gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
            gridviewPortfolioValuation.DataBind();
        }
        protected void chartPortfolioValuation_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            double quantity;
            double cost;
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
                    quantity = System.Convert.ToDouble(postBackValues[3]);
                    cost = System.Convert.ToDouble(postBackValues[4]);
                    ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nValuation:" + postBackValues[2] + "\nCumulative Units:" + quantity + "\nCumulative Cost:" + cost;
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
                HA.ToolTip = "Fund name: " + seriesName + ", Valuation: " + postBackValues[2];
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
            string fromDate = textboxFromDate.Text;
            string toDate = textboxToDate.Text;
            string fileName = Session["PortfolioNameMF"].ToString();
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            ShowGraph(fileName);
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