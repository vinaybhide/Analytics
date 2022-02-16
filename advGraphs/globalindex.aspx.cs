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
            try
            {
                dailyData = stockManager.GetStockPriceData(selectedindex, fromDate: fromDate);
                if (chartAdvGraph.Annotations.Count > 0)
                    chartAdvGraph.Annotations.Clear();
                ViewState["MAIN_DATA"] = null;

                if ((dailyData != null) && (dailyData.Rows.Count > 0))
                {
                    GridViewData.DataSource = dailyData;
                    GridViewData.DataBind();

                    ViewState["MAIN_DATA"] = dailyData;

                    chartAdvGraph.DataSource = dailyData;
                    chartAdvGraph.DataBind();

                    if (chartAdvGraph.Series.FindByName("Open") != null)
                    {
                        chartAdvGraph.Series["Open"].PostBackValue = "Open," + selectedindexname + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("High") != null)
                    {
                        chartAdvGraph.Series["High"].PostBackValue = "High," + selectedindexname + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("Low") != null)
                    {
                        chartAdvGraph.Series["Low"].PostBackValue = "Low," + selectedindexname + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("Close") != null)
                    {
                        chartAdvGraph.Series["Close"].PostBackValue = "Close," + selectedindexname + "," + "#VALX,#VALY";
                    }
                    if (chartAdvGraph.Series.FindByName("OHLC") != null)
                    {
                        chartAdvGraph.Series.FindByName("OHLC").Enabled = true;
                        chartAdvGraph.Series["OHLC"].PostBackValue = "OHLC," + selectedindexname + "," + "#VALX,#VALY1,#VALY2,#VALY3,#VALY4";
                    }
                    if (chartAdvGraph.Series.FindByName("Volume") != null)
                    {
                        chartAdvGraph.Series.FindByName("Volume").Enabled = true;
                        chartAdvGraph.Series["Volume"].PostBackValue = "Volume," + selectedindexname + "," + "#VALX,#VALY";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void buttonShowSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false;

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
                        bArea1 = true;
                    }
                    else
                    {
                        //all other graphs are on area 0
                        bArea0 = true;
                    }
                }
            }
            chartAdvGraph.ChartAreas[0].Visible = bArea0;
            chartAdvGraph.ChartAreas[1].Visible = bArea1;
        }
        public void buttonRemoveSelectedIndicatorGraph_Click()
        {
            string graphName = Master.dropdownGraphList.SelectedValue;
            bool bArea0 = false, bArea1 = false;

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
                        bArea1 = true;
                    }
                    else
                    {
                        bArea0 = true;
                    }
                }
            }
            chartAdvGraph.ChartAreas[0].Visible = bArea0;
            chartAdvGraph.ChartAreas[1].Visible = bArea1;
        }
        public void buttonShowGraph_Click()
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);

            ShowIndexGraph();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);

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