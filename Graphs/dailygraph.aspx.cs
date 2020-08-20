using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class dailygraph : System.Web.UI.Page
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
                }
                if (Request.QueryString["script"] != null)
                {
                    //labelTitle.Text = Request.QueryString["script"].ToString();
                    //if (Request.QueryString["size"] != null)
                    //    ddlOutputSize.SelectedValue = Request.QueryString["size"];

                    //if(!IsPostBack)

                    ShowGraph(Request.QueryString["script"].ToString());
                    headingtext.Text = "Daily Price - " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //ShowGraph(scriptName);
                        chartdailyGraph.Visible = true;
                        chartdailyGraph.Width = int.Parse(panelWidth.Value);
                        chartdailyGraph.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Response.Redirect("~/Default.aspx");
            }
        }
        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize = "";
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;

            try
            {
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
                    if (Request.QueryString["size"] != null)
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        scriptData = StockApi.getDaily(folderPath, scriptName, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                    }
                    ViewState["FetchedData"] = scriptData;
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
                        scriptData = filteredRows.CopyToDataTable();

                    if (chartdailyGraph.Annotations.Count > 0)
                        chartdailyGraph.Annotations.Clear();
                }
                else
                {
                    scriptData = (DataTable)ViewState["FetchedData"];
                }
                //}
                if (scriptData != null)
                {
                    chartdailyGraph.DataSource = scriptData;
                    chartdailyGraph.DataBind();
                    if (checkBoxOpen.Checked)
                        //showOpenLine(scriptData);
                        chartdailyGraph.Series["Open"].Enabled = true;
                    else
                    {
                        chartdailyGraph.Series["Open"].Enabled = false;
                        if (chartdailyGraph.Annotations.FindByName("Open") != null)
                            chartdailyGraph.Annotations.Clear();
                    }

                    if (checkBoxHigh.Checked)
                        //showHighLine(scriptData);
                        chartdailyGraph.Series["High"].Enabled = true;
                    else
                    {
                        chartdailyGraph.Series["High"].Enabled = false;
                        if (chartdailyGraph.Annotations.FindByName("High") != null)
                            chartdailyGraph.Annotations.Clear();

                    }
                    if (checkBoxLow.Checked)
                        //showLowLine(scriptData);
                        chartdailyGraph.Series["Low"].Enabled = true;
                    else
                    {
                        chartdailyGraph.Series["Low"].Enabled = false;
                        if (chartdailyGraph.Annotations.FindByName("Low") != null)
                            chartdailyGraph.Annotations.Clear();

                    }

                    if (checkBoxClose.Checked)
                        //showCloseLine(scriptData);
                        chartdailyGraph.Series["Close"].Enabled = true;
                    else
                    {
                        chartdailyGraph.Series["Close"].Enabled = false;
                        if (chartdailyGraph.Annotations.FindByName("Close") != null)
                            chartdailyGraph.Annotations.Clear();

                    }

                    if (checkBoxCandle.Checked)
                        //showCandleStickGraph(scriptData);
                        chartdailyGraph.Series["OHLC"].Enabled = true;
                    else
                    {
                        chartdailyGraph.Series["OHLC"].Enabled = false;
                        if (chartdailyGraph.Annotations.FindByName("OHLC") != null)
                            chartdailyGraph.Annotations.Clear();

                    }

                    if (checkBoxVolume.Checked)
                        //showVolumeGraph(scriptData);
                        chartdailyGraph.Series["Volume"].Enabled = true;
                    else
                    {
                        chartdailyGraph.Series["Volume"].Enabled = false;
                        if (chartdailyGraph.Annotations.FindByName("Volume") != null)
                            chartdailyGraph.Annotations.Clear();

                    }

                    if (checkBoxGrid.Checked)
                    {
                        GridViewDaily.Visible = true;
                        GridViewDaily.DataSource = scriptData;
                        GridViewDaily.DataBind();
                    }
                    else
                    {
                        GridViewDaily.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
            }
        }

        public void showCloseLine(DataTable scriptData)
        {
            (chartdailyGraph.Series["Close"]).XValueMember = "Date";
            (chartdailyGraph.Series["Close"]).XValueType = ChartValueType.Date;
            (chartdailyGraph.Series["Close"]).YValueMembers = "Close";
            //(chartdailyGraph.Series["Close"]).ToolTip = "Close:   Date:#VALX;   Value:#VALY";
        }

        public void showOpenLine(DataTable scriptData)
        {
            chartdailyGraph.Series["Open"].Enabled = true;
            (chartdailyGraph.Series["Open"]).XValueMember = "Date";
            (chartdailyGraph.Series["Open"]).XValueType = ChartValueType.Date;
            (chartdailyGraph.Series["Open"]).YValueMembers = "Open";
            //(chartdailyGraph.Series["Close"]).ToolTip = "Close:   Date:#VALX;   Value:#VALY";
        }
        public void showHighLine(DataTable scriptData)
        {
            (chartdailyGraph.Series["High"]).XValueMember = "Date";
            (chartdailyGraph.Series["High"]).XValueType = ChartValueType.Date;
            (chartdailyGraph.Series["High"]).YValueMembers = "High";
            //(chartdailyGraph.Series["Close"]).ToolTip = "Close:   Date:#VALX;   Value:#VALY";
        }
        public void showLowLine(DataTable scriptData)
        {
            (chartdailyGraph.Series["Low"]).XValueMember = "Date";
            (chartdailyGraph.Series["Low"]).XValueType = ChartValueType.Date;
            (chartdailyGraph.Series["Low"]).YValueMembers = "Low";
            //(chartdailyGraph.Series["Close"]).ToolTip = "Close:   Date:#VALX;   Value:#VALY";
        }

        public void showCandleStickGraph(DataTable scriptData)
        {
            //chartdailyGraph.DataSource = scriptData;
            chartdailyGraph.Series["OHLC"].XValueMember = "Date";
            chartdailyGraph.Series["OHLC"].YValueMembers = "Open, High, Low, Close";
            //chartdailyGraph.DataBind();

            chartdailyGraph.Series["OHLC"].BorderColor = System.Drawing.Color.Black;
            chartdailyGraph.Series["OHLC"].Color = System.Drawing.Color.Black;
            chartdailyGraph.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red, OpenCloseStyle=Triangle, ShowOpenClose=Both";
            chartdailyGraph.Series["OHLC"].XValueType = ChartValueType.Date;
            chartdailyGraph.Series["OHLC"]["OpenCloseStyle"] = "Triangle";
            chartdailyGraph.Series["OHLC"]["ShowOpenClose"] = "Both";

            //chartdailyGraph.Series["OHLC"]["PriceDownColor"] = "Triangle";
            //chartdailyGraph.Series["OHLC"]["PriceUpColor"] = "Both";

            //chartdailyGraph.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            //chartdailyGraph.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chartdailyGraph.ChartAreas[0].AxisY.Minimum = 0;
            chartdailyGraph.DataManipulator.IsStartFromFirst = true;

            //chartdailyGraph.ChartAreas[0].AxisY.Maximum = chartdailyGraph.Series["OHLC"].Points.FindMaxByValue("Y1", 0).YValues[0];

        }
        public void showVolumeGraph(DataTable scriptData)
        {
            (chartdailyGraph.Series["Volume"]).XValueMember = "Date";
            (chartdailyGraph.Series["Volume"]).XValueType = ChartValueType.Date;
            (chartdailyGraph.Series["Volume"]).YValueMembers = "Volume";
            //(chartdailyGraph.Series["Volume"]).ToolTip = "Date:#VALX;   Volume:#VALY";
        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            string fromDate = textboxFromDate.Text;
            string toDate = textboxToDate.Text;
            string scriptName = Request.QueryString["script"].ToString();
            ViewState["FromDate"] = textboxFromDate.Text;
            ViewState["ToDate"] = textboxToDate.Text;
            ShowGraph(scriptName);
        }

        protected void checkBoxOpen_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void checkBoxHigh_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void checkBoxLow_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void checkBoxClose_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void checkBoxCandle_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void checkBoxGrid_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void checkBoxVolume_CheckedChanged(object sender, EventArgs e)
        {
            string scriptName = Request.QueryString["script"].ToString();
            ShowGraph(scriptName);
        }

        protected void chartdailyGraph_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;
            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;

            try
            {
                if (chartdailyGraph.Annotations.Count > 0)
                    chartdailyGraph.Annotations.Clear();

                postBackValues = e.PostBackValue.Split(',');

                if (postBackValues[0].Equals("AnnotationClicked"))
                    return;
                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);
                seriesName = postBackValues[0];

                //if (postBackValues.Length > 3) //OHLC
                //{
                //    seriesName = postBackValues[5];

                //}
                //else
                //{
                //    seriesName = postBackValues[2];
                //}

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("Volume"))
                {
                    HA.AxisY = chartdailyGraph.ChartAreas[0].AxisY2;
                    VA.AxisY = chartdailyGraph.ChartAreas[0].AxisY2;
                    ra.AxisY = chartdailyGraph.ChartAreas[0].AxisY2;
                }
                else
                {
                    HA.AxisY = chartdailyGraph.ChartAreas[0].AxisY;
                    VA.AxisY = chartdailyGraph.ChartAreas[0].AxisY;
                    ra.AxisY = chartdailyGraph.ChartAreas[0].AxisY;
                }

                //HA.Name = seriesName;
                HA.AxisX = chartdailyGraph.ChartAreas[0].AxisX;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartdailyGraph.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartdailyGraph.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.AxisX = chartdailyGraph.ChartAreas[0].AxisX;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartdailyGraph.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartdailyGraph.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.AxisX = chartdailyGraph.ChartAreas[0].AxisX;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                if (seriesName.Equals("OHLC"))
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[2] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[5];
                }
                else
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                }
                //ra.SmartLabelStyle = sl;

                chartdailyGraph.Annotations.Add(ra);

            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
            }
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
            GridViewDaily.DataBind();
        }

        protected void buttonDesc_Click(object sender, EventArgs e)
        {
            if (trid.Visible)
                trid.Visible = false;
            else
                trid.Visible = true;
        }
    }
}