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
    public partial class vwap_intra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["FromDate"] = null;
                    ViewState["ToDate"] = null;
                    ViewState["FetchedDataIntra"] = null;
                    ViewState["FetchedDataVWAP"] = null;
                }
                if (Request.QueryString["script"] != null)
                {
                    ShowGraph(Request.QueryString["script"].ToString());
                    headingtext.InnerText = "VWAP Vs Intra-day : " + Request.QueryString["script"].ToString();
                    if (panelWidth.Value != "" && panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartVWAP_Intra.Visible = true;
                        chartVWAP_Intra.Width = int.Parse(panelWidth.Value);
                        chartVWAP_Intra.Height = int.Parse(panelHeight.Value);
                    }
                }
                else
                {
                    Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                Response.Redirect(".\\Default.aspx");
            }
        }
        public void ShowGraph(string scriptName)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            bool bIsTestOn = true;
            DataTable intraData = null;
            DataTable vwapData = null;
            DataTable tempData = null;
            string expression = "";
            string outputSize = "";
            string interval_intra = "";
            string interval_vwap = "";
            string fromDate = "", toDate = "";
            DataRow[] filteredRows = null;


            if ((ViewState["FetchedDataIntra"] == null) || (ViewState["FetchedDataVWAP"] == null))
            {
                if (Session["IsTestOn"] != null)
                {
                    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                }

                if (Session["TestDataFolder"] != null)
                {
                    folderPath = Session["TestDataFolder"].ToString();
                }
                if ((Request.QueryString["size"] != null) && (Request.QueryString["interval_intra"] != null))
                {
                    outputSize = Request.QueryString["size"].ToString();
                    interval_intra = Request.QueryString["interval_intra"];
                    intraData = StockApi.getIntraday(folderPath, scriptName, time_interval: interval_intra, outputsize: outputSize, bIsTestModeOn: bIsTestOn, bSaveData: false);
                    ViewState["FetchedDataIntra"] = intraData;
                }
                else
                {
                    ViewState["FetchedDataIntra"] = null;
                    intraData = null;
                }
                if (Request.QueryString["interval_vwap"] != null)
                {
                    interval_vwap = Request.QueryString["interval_vwap"];

                    vwapData = StockApi.getVWAP(folderPath, scriptName, day_interval: interval_vwap, bIsTestModeOn: bIsTestOn, bSaveData: false);
                    ViewState["FetchedDataVWAP"] = vwapData;
                }
                else
                {
                    ViewState["FetchedDataVWAP"] = null;
                    vwapData = null;
                }

            }
            else
            {
                if (ViewState["FromDate"] != null)
                    fromDate = ViewState["FromDate"].ToString();
                if (ViewState["ToDate"] != null)
                    toDate = ViewState["ToDate"].ToString();

                if ((fromDate.Length > 0) && (toDate.Length > 0))
                {
                    tempData = (DataTable)ViewState["FetchedDataIntra"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        intraData = filteredRows.CopyToDataTable();

                    tempData.Clear();
                    tempData = null;

                    tempData = (DataTable)ViewState["FetchedDataVWAP"];
                    expression = "Date >= '" + fromDate + "' and Date <= '" + toDate + "'";
                    filteredRows = tempData.Select(expression);
                    if ((filteredRows != null) && (filteredRows.Length > 0))
                        vwapData = filteredRows.CopyToDataTable();
                }
                else
                {
                    intraData = (DataTable)ViewState["FetchedDataIntra"];
                    vwapData = (DataTable)ViewState["FetchedDataVWAP"];
                }
            }

            if ((intraData != null) && (vwapData != null))
            {
                showCandleStickGraph(intraData);
                showVWAP(vwapData);
            }
        }

        public void showCandleStickGraph(DataTable scriptData)
        {
            //chartVWAP_Intra.DataSource = scriptData;
            chartVWAP_Intra.Series["OHLC"].Points.DataBind(scriptData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
            //chartVWAP_Intra.DataBind();
            chartVWAP_Intra.Series["OHLC"].XValueMember = "Date";
            chartVWAP_Intra.Series["OHLC"].XValueType = ChartValueType.DateTime;
            chartVWAP_Intra.Series["OHLC"].YValueMembers = "Open,High,Low,Close";

            chartVWAP_Intra.Series["OHLC"].BorderColor = System.Drawing.Color.Black;
            chartVWAP_Intra.Series["OHLC"].Color = System.Drawing.Color.Black;
            chartVWAP_Intra.Series["OHLC"].CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red";
            chartVWAP_Intra.Series["OHLC"].XValueType = ChartValueType.DateTime;
            chartVWAP_Intra.Series["OHLC"]["OpenCloseStyle"] = "Triangle";
            chartVWAP_Intra.Series["OHLC"]["ShowOpenClose"] = "Both";
            //chartVWAP_Intra.Series["OHLC"]["PriceDownColor"] = "Triangle";
            //chartVWAP_Intra.Series["OHLC"]["PriceUpColor"] = "Both";

            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.MajorGrid.LineWidth = 1;
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.MajorGrid.LineWidth = 1;
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Minimum = 0;
            //chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Maximum = chartdailyGraph.Series["OHLC"].Points.FindMaxByValue("Y1", 0).YValues[0];
            chartVWAP_Intra.DataManipulator.IsStartFromFirst = true;

            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.Title = "Date";
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.Title = "OHLC";
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX.LabelStyle.Format = "g";

            chartVWAP_Intra.Series["OHLC"].Enabled = true;

            if (chartVWAP_Intra.Annotations.Count > 0)
                chartVWAP_Intra.Annotations.Clear();
        }

        public void showVWAP(DataTable scriptData)
        {
            chartVWAP_Intra.Series["VWAP"].Points.DataBind(scriptData.AsEnumerable(), "Date", "VWAP", "");
            (chartVWAP_Intra.Series["VWAP"]).XValueMember = "Date";
            (chartVWAP_Intra.Series["VWAP"]).XValueType = ChartValueType.DateTime;
            (chartVWAP_Intra.Series["VWAP"]).YValueMembers = "VWAP";
            //(chartVWAP_Intra.Series["VWAP"]).ToolTip = "VWAP: Date:#VALX;   Value:#VALY";

            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX2.Title = "Date";
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX2.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY2.Title = "VWAP";
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisY2.TitleAlignment = System.Drawing.StringAlignment.Center;
            chartVWAP_Intra.ChartAreas["chartareaVWAP_Intra"].AxisX2.LabelStyle.Format = "g";

            chartVWAP_Intra.Series["VWAP"].Enabled = true;

            //chartVWAP.Titles["titleVWAP"].Text = $"{"Volume Weighted Average Price - "}{scriptName}";
            if (chartVWAP_Intra.Annotations.Count > 0)
                chartVWAP_Intra.Annotations.Clear();
        }

        protected void chartVWAP_Intra_Click(object sender, ImageMapEventArgs e)
        {
            string[] postBackValues;

            DateTime xDate;
            double lineWidth;
            double lineHeight;
            string seriesName;
            string legendName;

            DataPoint p;
            //double lineHeight = -35;

            if (e.PostBackValue.Contains(','))
            {
                postBackValues = e.PostBackValue.Split(',');
                xDate = System.Convert.ToDateTime(postBackValues[0]);
                lineWidth = xDate.ToOADate();

                lineHeight = System.Convert.ToDouble(postBackValues[1]);
                seriesName = postBackValues[2];

                if (chartVWAP_Intra.Annotations.Count > 0)
                    chartVWAP_Intra.Annotations.Clear();

                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                if (seriesName.Equals("OHLC"))
                {
                    HA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX;
                    HA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY;
                }
                else
                {
                    HA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX2;
                    HA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY2;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartVWAP_Intra.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartVWAP_Intra.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                if (seriesName.Equals("OHLC"))
                {
                    VA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX;
                    VA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY;
                }
                else
                {
                    VA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX2;
                    VA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY2;
                }
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartVWAP_Intra.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartVWAP_Intra.Annotations.Add(VA);

                p = (chartVWAP_Intra.Series[seriesName]).Points.FindByValue(lineHeight, "Y");

                if (p != null)
                {
                    p.MarkerSize = 8;
                    p.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                    p.Label = e.PostBackValue.Split(',')[2] + "\n" + e.PostBackValue.Split(',')[0] + "\n" + e.PostBackValue.Split(',')[1];
                    p.LabelBackColor = System.Drawing.Color.Transparent;
                    p.LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
                    p.LabelBorderColor = System.Drawing.Color.Black;
                    p.IsValueShownAsLabel = true;
                }
            }
            else
            {
                legendName = e.PostBackValue;
                if(legendName.ToUpper().Equals("OHLC"))
                {
                    chartVWAP_Intra.Series["OHLC"].Enabled = !(chartVWAP_Intra.Series["OHLC"].Enabled);
                }
                else
                {
                    chartVWAP_Intra.Series["VWAP"].Enabled = !(chartVWAP_Intra.Series["VWAP"].Enabled);
                }
            }

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