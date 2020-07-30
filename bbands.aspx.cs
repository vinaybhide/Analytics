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
    public partial class bbands : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["FromDate"] = null;
                ViewState["ToDate"] = null;
                ViewState["FetchedData"] = null;
            }

            if (Request.QueryString["script"] != null)
            {
                ShowGraph(Request.QueryString["script"].ToString());
                headingtext.InnerText = "Bollinger Bands:" + Request.QueryString["script"].ToString();
                if (panelWidth.Value != "" && panelHeight.Value != "")
                {
                    chartBollingerBands.Visible = true;
                    chartBollingerBands.Width = int.Parse(panelWidth.Value);
                    chartBollingerBands.Height = int.Parse(panelHeight.Value);
                }
            }
            else
            {
                Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
            }
        }

        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable scriptData = null;
            DataTable tempData = null;
            string expression = "";
            string interval = "";
            string period = "";
            string seriestype = "";
            string nbdevup = "";
            string nbdevdn = ""; 
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;


            if (ViewState["FetchedData"] == null)
            {
                if (Session["IsTestOn"] != null)
                {
                    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                }

                if (Session["TestDataFolder"] != null)
                {
                    folderPath = Session["TestDataFolder"].ToString();
                }
                if ((Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null) && (Request.QueryString["seriestype"] != null) && 
                    (Request.QueryString["nbdevup"] != null) && (Request.QueryString["nbdevdn"] != null))
                {
                    interval = Request.QueryString["interval"];
                    period = Request.QueryString["period"];
                    seriestype = Request.QueryString["seriestype"];
                    nbdevup = Request.QueryString["nbdevup"];
                    nbdevdn = Request.QueryString["nbdevdn"];
                    scriptData = StockApi.getADX(folderPath, scriptName, day_interval: interval, period: period,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false);
                    scriptData = StockApi.getBbands(folderPath, scriptName, day_interval: interval, period: period,
                        seriestype: seriestype, nbdevup: nbdevup, nbdevdn: nbdevdn, bIsTestModeOn: bIsTestOn, bSaveData: false);
                }
                ViewState["FetchedData"] = scriptData;
            }
            else
            {
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
                }
                else
                {
                    scriptData = (DataTable)ViewState["FetchedData"];
                }
            }

            if (scriptData != null)
            {
                ////time,Real Lower Band,Real Middle Band,Real Upper Band
                ///
                (chartBollingerBands.Series["Real Lower Band"]).XValueMember = "Date";
                (chartBollingerBands.Series["Real Lower Band"]).XValueType = ChartValueType.Date;
                (chartBollingerBands.Series["Real Lower Band"]).YValueMembers = "Real Lower Band";
                //(chartBollingerBands.Series["Real Lower Band"]).ToolTip = "Lower Band: Date:#VALX;   Value:#VALY";

                (chartBollingerBands.Series["Real Middle Band"]).XValueMember = "Date";
                (chartBollingerBands.Series["Real Middle Band"]).XValueType = ChartValueType.Date;
                (chartBollingerBands.Series["Real Middle Band"]).YValueMembers = "Real Middle Band";
                //(chartBollingerBands.Series["Real Middle Band"]).ToolTip = "Middle Band: Date:#VALX;   Value:#VALY";

                (chartBollingerBands.Series["Real Upper Band"]).XValueMember = "Date";
                (chartBollingerBands.Series["Real Upper Band"]).XValueType = ChartValueType.Date;
                (chartBollingerBands.Series["Real Upper Band"]).YValueMembers = "Real Upper Band";
                //(chartBollingerBands.Series["Real Upper Band"]).ToolTip = "Upper Band: Date:#VALX;   Value:#VALY";

                //chartBollingerBands.Legends.Add("Real Lower Band");
                //chartBollingerBands.Legends["Real Lower Band"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                //chartBollingerBands.Legends["Real Lower Band"].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                //chartBollingerBands.Legends["Real Lower Band"].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                //chartBollingerBands.Legends["Real Lower Band"].BorderColor = System.Drawing.Color.Black;

                //chartBollingerBands.Legends.Add("Real Middle Band");
                //chartBollingerBands.Legends["Real Middle Band"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                //chartBollingerBands.Legends["Real Middle Band"].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                //chartBollingerBands.Legends["Real Middle Band"].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                //chartBollingerBands.Legends["Real Middle Band"].BorderColor = System.Drawing.Color.Black;

                //chartBollingerBands.Legends.Add("Real Upper Band");
                //chartBollingerBands.Legends["Real Upper Band"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                //chartBollingerBands.Legends["Real Upper Band"].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                //chartBollingerBands.Legends["Real Upper Band"].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                //chartBollingerBands.Legends["Real Upper Band"].BorderColor = System.Drawing.Color.Black;

                chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisX.Title = "Date";
                chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisY.Title = "Value";
                chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                //chartBollingerBands.Titles["titleBbands"].Text = $"{"Bollinger Bands - "}{scriptName}";

                //VerticalLineAnnotation VA = new VerticalLineAnnotation();
                //VA.AxisX = chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisX;
                //VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartBollingerBands.ChartAreas["chartareaBollingerBands"].Name;
                //VA.Name = "myLine";
                //VA.LineColor = System.Drawing.Color.Red;
                //VA.LineWidth = 10;         // use your numbers!

                //HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //VA.AxisY = chartBollingerBands.ChartAreas["chartareaBollingerBands"].AxisY;
                //VA.IsInfinitive = true;
                //VA.ClipToChartArea = chartBollingerBands.ChartAreas["chartareaBollingerBands"].Name;
                //VA.Name = "myLine2";
                //VA.LineColor = System.Drawing.Color.Red;
                //VA.LineWidth = 10;         // use your numbers!

                //chartBollingerBands.Annotations.Add(VA);
                //chartBollingerBands.Annotations.Add(HA);

                if (chartBollingerBands.Annotations.Count > 0)
                    chartBollingerBands.Annotations.Clear();

                chartBollingerBands.DataSource = scriptData;
                chartBollingerBands.DataBind();

                //VA.X = chartBollingerBands.Series[0].Points.FindMinByValue("X", 0).XValue;
                //VA.Y = chartBollingerBands.Series[0].Points.FindMinByValue("Y1", 0).YValues[0]; ;
                //chartBollingerBands.ChartAreas[0].AxisX.Minimum = chartBollingerBands.Series[0].Points.FindMinByValue("X", 0).XValue;
                //chartBollingerBands.ChartAreas[0].AxisX.Maximum = chartBollingerBands.Series[0].Points.FindMaxByValue("X", 0).XValue; // + 1;


            }
        }

        protected void chartBollingerBands_Click(object sender, ImageMapEventArgs e)
        {
            DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

            //double lineHeight = -35;

            if (chartBollingerBands.Annotations.Count > 0)
                chartBollingerBands.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA);

        }

        protected void drawLine2(string e)
        {
            DateTime xDate = System.Convert.ToDateTime(e.Split(',')[0]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.Split(',')[1]);

            //double lineHeight = -35;

            if (chartBollingerBands.Annotations.Count > 0)
                chartBollingerBands.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA);

        }
        public void drawLine(string eventargs)
        {
            string[] sargs = eventargs.Split(',');

            double lineWidthClient = System.Convert.ToDouble(sargs[0]);

            double lineHeightClient = System.Convert.ToDouble(sargs[1]);
            double lineWidthScreen = System.Convert.ToDouble(sargs[2]);

            double lineHeightScreen = System.Convert.ToDouble(sargs[3]);

            //double lineHeight = -35;

            if (chartBollingerBands.Annotations.Count > 0)
                chartBollingerBands.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeightClient;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidthClient;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA);

            HorizontalLineAnnotation HA2 = new HorizontalLineAnnotation();
            HA2.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            HA2.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            HA2.IsSizeAlwaysRelative = false;
            HA2.AnchorY = lineHeightScreen;
            HA2.IsInfinitive = true;
            HA2.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            HA2.LineDashStyle = ChartDashStyle.Dash;
            HA2.LineColor = Color.Blue;
            HA2.LineWidth = 1;
            chartBollingerBands.Annotations.Add(HA2);

            VerticalLineAnnotation VA2 = new VerticalLineAnnotation();
            VA2.AxisX = chartBollingerBands.ChartAreas[0].AxisX;
            VA2.AxisY = chartBollingerBands.ChartAreas[0].AxisY;
            VA2.IsSizeAlwaysRelative = false;
            VA2.AnchorX = lineWidthScreen;
            VA2.IsInfinitive = true;
            VA2.ClipToChartArea = chartBollingerBands.ChartAreas[0].Name;
            VA2.LineDashStyle = ChartDashStyle.Dash;
            VA2.LineColor = Color.Blue;
            VA2.LineWidth = 1;
            chartBollingerBands.Annotations.Add(VA2);
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
    }
}