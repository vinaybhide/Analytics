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
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedData"] = null;
                    listboxScripts.Items.Clear();
                    ListItem li = new ListItem("Show All", "All");
                    listboxScripts.Items.Add(li);
                    listboxScripts.Items[0].Selected = true;
                }
                if (Session["PortfolioName"] != null)
                {
                    string fileName = Session["PortfolioName"].ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(fileName);
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
            DataTable tempData = null;
            string folderPath = Server.MapPath("~/scriptdata/");
            XmlDocument xmlPortfolio;
            XmlNode root;
            XmlNodeList scriptNodeList;
            string scriptName;
            string searchPath;
            //int markerInterval = 10;
            //int i = 0;
            int tempQty = 0;
            string fromDate = "", toDate = "";
            string expression = "";
            DataRow[] filteredRows = null;
            DataRow[] scriptRows;

            try
            {
                if (File.Exists(fileName))
                {
                    //portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestOn);

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

                        portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestOn, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedData"] = portfolioTable;
                        gridviewPortfolioValuation.DataSource = (DataTable)ViewState["FetchedData"];
                        gridviewPortfolioValuation.DataBind();
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
                            portfolioTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        portfolioTable = (DataTable)ViewState["FetchedData"];
                    }
                    //}

                    if (portfolioTable != null)
                    {
                        if (chartPortfolioValuation.Annotations.Count > 0)
                            chartPortfolioValuation.Annotations.Clear();

                        xmlPortfolio = new XmlDocument();
                        xmlPortfolio.Load(fileName);
                        root = xmlPortfolio.DocumentElement;
                        searchPath = "/portfolio/script/name";
                        scriptNodeList = root.SelectNodes(searchPath);
                        foreach (XmlNode scriptNameNode in scriptNodeList)
                        {
                            scriptName = scriptNameNode.InnerText;

                            scriptRows = portfolioTable.Select("Symbol='" + scriptName + "'");

                            if (scriptRows.Length > 0)
                            {
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

                                    chartPortfolioValuation.Series[scriptName].Legend = chartPortfolioValuation.Legends[0].Name;
                                    chartPortfolioValuation.Series[scriptName].LegendText = scriptName;
                                    chartPortfolioValuation.Series[scriptName].LegendToolTip = scriptName;
                                    chartPortfolioValuation.Series[scriptName].ToolTip = scriptName + ": Date:#VALX; Value:#VALY (Click to see details)";
                                    chartPortfolioValuation.Series[scriptName].PostBackValue = scriptName + ",#VALX,#VALY1,#VAL2,#VAL3,#VAL4";
                                }

                                tempQty = 0;
                                (chartPortfolioValuation.Series[scriptName]).Points.Clear();
                                foreach (DataRow itemRow in scriptRows)
                                {
                                    (chartPortfolioValuation.Series[scriptName]).Points.AddXY(itemRow["Date"], itemRow["ValueOnDate"]);

                                    if ((itemRow["CumulativeQuantity"] != System.DBNull.Value) && ((tempQty == 0) || (tempQty != System.Convert.ToInt32(itemRow["CumulativeQuantity"]))))
                                    {
                                        tempQty = System.Convert.ToInt32(itemRow["CumulativeQuantity"]);
                                        (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 10;
                                        (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                                        //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].Label = 
                                        //    scriptName + "\n" + "Purchase date:" + itemRow["PurchaseDate"].ToString() + 
                                        //    "\nCumulative Qty=" + tempQty.ToString() + "\nCost of Investment: " + itemRow["CostofInvestment"].ToString() + 
                                        //    "\nValuation: " + itemRow["ValueOnDate"].ToString();
                                        //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBackColor = Color.Transparent;
                                        //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;
                                        //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderColor = System.Drawing.Color.Transparent;
                                        //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].IsValueShownAsLabel = true;
                                    }
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].PostBackValue =
                                        scriptName + "," + itemRow["Date"] + "," + itemRow["ValueOnDate"] + "," + itemRow["CumulativeQuantity"] + "," + itemRow["CostofInvestment"];
                                }
                                (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 10;
                                (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;

                                //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].Label =
                                //    scriptName + "\n" + "Purchase date:" + scriptRows[scriptRows.Length - 1]["PurchaseDate"].ToString() +
                                //    "\nValuation: " + scriptRows[scriptRows.Length - 1]["ValueOnDate"].ToString();
                                //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBackColor = Color.RoyalBlue;
                                //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;
                                //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderColor = System.Drawing.Color.Black;
                                //(chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].IsValueShownAsLabel = false;
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

                        scriptRows = portfolioTable.Select("[Date] = MAX([Date])");

                        //chartPortfolioValuation.ChartAreas[0].AxisX.MaximumAutoSize = false;
                        chartPortfolioValuation.ChartAreas[0].AxisX.Maximum = System.Convert.ToDateTime(scriptRows[0][1]).AddDays(100).ToOADate();

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

        //protected void checkboxlistScripts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string fileName = Session["PortfolioName"].ToString();
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

            try
            {
                if (chartPortfolioValuation.Annotations.Count > 0)
                    chartPortfolioValuation.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                quantity = System.Convert.ToInt32(postBackValues[3]);
                cost = System.Convert.ToDouble(postBackValues[4]);
                seriesName = postBackValues[0];

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

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
                ra.Text = seriesName + "\nDate:" + postBackValues[1] + "\nValuation:" + postBackValues[2] + "\nCum Qty:" + quantity + "\nCost:" + cost;
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
            string fileName = Session["PortfolioName"].ToString();
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