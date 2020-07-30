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
    public partial class stoch : System.Web.UI.Page
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
                headingtext.InnerText = "Stochastic Oscillator:" + Request.QueryString["script"].ToString();
                if (panelWidth.Value != "" && panelHeight.Value != "")
                {
                    chartSTOCH.Visible = true;
                    chartSTOCH.Width = int.Parse(panelWidth.Value);
                    chartSTOCH.Height = int.Parse(panelHeight.Value);
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
            string fastkperiod = "";
            string slowkperiod = "";
            string slowdperiod = "";
            string slowkmatype = "";
            string slowdmatype = "";
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
                if ((Request.QueryString["interval"] != null) && (Request.QueryString["fastkperiod"] != null) &&
                    (Request.QueryString["slowkperiod"] != null) && (Request.QueryString["slowdperiod"] != null) &&
                    (Request.QueryString["slowkmatype"] != null) && (Request.QueryString["slowdmatype"] != null))
                {
                    interval = Request.QueryString["interval"];
                    fastkperiod = Request.QueryString["fastkperiod"].ToString();
                    slowkperiod = Request.QueryString["slowkperiod"].ToString();
                    slowdperiod = Request.QueryString["slowdperiod"].ToString();
                    slowkmatype = Request.QueryString["slowkmatype"].ToString();
                    slowdmatype = Request.QueryString["slowdmatype"].ToString();

                    scriptData = StockApi.getSTOCH(folderPath, scriptName, day_interval: interval, fastkperiod: fastkperiod,
                        slowkperiod: slowkperiod, slowdperiod: slowdperiod, slowkmatype: slowkmatype,
                        slowdmatype: slowdmatype, bIsTestModeOn: bIsTestOn, bSaveData: false);
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
                (chartSTOCH.Series["seriesSlowK"]).XValueMember = "Date";
                (chartSTOCH.Series["seriesSlowK"]).XValueType = ChartValueType.Date;
                (chartSTOCH.Series["seriesSlowK"]).YValueMembers = "SlowK";
                //(chartSTOCH.Series["seriesSlowK"]).ToolTip = "SlowK: Date:#VALX;   Value:#VALY";

                chartSTOCH.ChartAreas["chartareaSlowK"].AxisX.Title = "Date";
                chartSTOCH.ChartAreas["chartareaSlowK"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartSTOCH.ChartAreas["chartareaSlowK"].AxisY.Title = "SlowK Value";
                chartSTOCH.ChartAreas["chartareaSlowK"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                (chartSTOCH.Series["seriesSlowD"]).XValueMember = "Date";
                (chartSTOCH.Series["seriesSlowD"]).XValueType = ChartValueType.Date;
                (chartSTOCH.Series["seriesSlowD"]).YValueMembers = "SlowD";
                //(chartSTOCH.Series["seriesSlowD"]).ToolTip = "SlowD: Date:#VALX;   Value:#VALY";

                chartSTOCH.ChartAreas["chartareaSlowD"].AxisX.Title = "Date";
                chartSTOCH.ChartAreas["chartareaSlowD"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartSTOCH.ChartAreas["chartareaSlowD"].AxisY.Title = "SlowD Value";
                chartSTOCH.ChartAreas["chartareaSlowD"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                //chartSTOCH.Titles["titleSTOCH"].Text = $"{"Stochastic Oscillator- "}{scriptName}";

                if (chartSTOCH.Annotations.Count > 0)
                    chartSTOCH.Annotations.Clear();

                chartSTOCH.DataSource = scriptData;
                chartSTOCH.DataBind();
            }
        }

        protected void chartSTOCH_Click(object sender, ImageMapEventArgs e)
        {
            int chartIndex = System.Convert.ToInt32(e.PostBackValue.Split(',')[0]);
            DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[1]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[2]);

            //double lineHeight = -35;

            if (chartSTOCH.Annotations.Count > 0)
                chartSTOCH.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartSTOCH.ChartAreas[chartIndex].AxisX;
            HA.AxisY = chartSTOCH.ChartAreas[chartIndex].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartSTOCH.ChartAreas[chartIndex].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartSTOCH.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartSTOCH.ChartAreas[chartIndex].AxisX;
            VA.AxisY = chartSTOCH.ChartAreas[chartIndex].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartSTOCH.ChartAreas[chartIndex].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartSTOCH.Annotations.Add(VA);
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