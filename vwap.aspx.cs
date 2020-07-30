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
    public partial class vwap : System.Web.UI.Page
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
                headingtext.InnerText = "Volume weighted average price:" + Request.QueryString["script"].ToString();
                if (panelWidth.Value != "" && panelHeight.Value != "")
                {
                    chartVWAP.Visible = true;
                    chartVWAP.Width = int.Parse(panelWidth.Value);
                    chartVWAP.Height = int.Parse(panelHeight.Value);
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
                if (Request.QueryString["interval"] != null)
                {
                    interval = Request.QueryString["interval"];

                    scriptData = StockApi.getVWAP(folderPath, scriptName, day_interval: interval, bIsTestModeOn: bIsTestOn, bSaveData: false);
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
                (chartVWAP.Series["seriesVWAP"]).XValueMember = "Date";
                (chartVWAP.Series["seriesVWAP"]).XValueType = ChartValueType.Date;
                (chartVWAP.Series["seriesVWAP"]).YValueMembers = "VWAP";
                //(chartVWAP.Series["seriesVWAP"]).ToolTip = "VWAP: Date:#VALX;   Value:#VALY";

                chartVWAP.ChartAreas["chartareaVWAP"].AxisX.Title = "Date";
                chartVWAP.ChartAreas["chartareaVWAP"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartVWAP.ChartAreas["chartareaVWAP"].AxisY.Title = "Value";
                chartVWAP.ChartAreas["chartareaVWAP"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartVWAP.ChartAreas["chartareaVWAP"].AxisX.LabelStyle.Format = "g";

                //chartVWAP.Titles["titleVWAP"].Text = $"{"Volume Weighted Average Price - "}{scriptName}";

                if (chartVWAP.Annotations.Count > 0)
                    chartVWAP.Annotations.Clear();

                chartVWAP.DataSource = scriptData;
                chartVWAP.DataBind();
            }
        }

        protected void chartVWAP_Click(object sender, ImageMapEventArgs e)
        {
            DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

            //double lineHeight = -35;

            if (chartVWAP.Annotations.Count > 0)
                chartVWAP.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartVWAP.ChartAreas[0].AxisX;
            HA.AxisY = chartVWAP.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartVWAP.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartVWAP.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartVWAP.ChartAreas[0].AxisX;
            VA.AxisY = chartVWAP.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartVWAP.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartVWAP.Annotations.Add(VA);

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