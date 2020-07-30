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
    public partial class adx : System.Web.UI.Page
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
                headingtext.InnerText = "Average directional movement index:" + Request.QueryString["script"].ToString();
                if (panelWidth.Value != "" && panelHeight.Value != "")
                {
                    chartADX.Visible = true;
                    chartADX.Width = int.Parse(panelWidth.Value);
                    chartADX.Height = int.Parse(panelHeight.Value);
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
                if ((Request.QueryString["interval"] != null) && (Request.QueryString["period"] != null))
                {
                    interval = Request.QueryString["interval"];
                    period = Request.QueryString["period"];
                    scriptData = StockApi.getADX(folderPath, scriptName, day_interval: interval, period: period,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false);
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
                (chartADX.Series["seriesADX"]).XValueMember = "Date";
                (chartADX.Series["seriesADX"]).XValueType = ChartValueType.Date;
                (chartADX.Series["seriesADX"]).YValueMembers = "ADX";
                //(chartADX.Series["seriesADX"]).ToolTip = "ADX: Date:#VALX;   Value:#VALY";

                chartADX.ChartAreas["chartareaADX"].AxisX.Title = "Date";
                chartADX.ChartAreas["chartareaADX"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartADX.ChartAreas["chartareaADX"].AxisY.Title = "Value";
                chartADX.ChartAreas["chartareaADX"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                //chartADX.Titles["titleADX"].Text = $"{"Average Directional Movement Index- "}{scriptName}";

                if (chartADX.Annotations.Count > 0)
                    chartADX.Annotations.Clear();

                chartADX.DataSource = scriptData;
                chartADX.DataBind();
            }
        }

        protected void chartADX_Click(object sender, ImageMapEventArgs e)
        {
            DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

            //double lineHeight = -35;

            if (chartADX.Annotations.Count > 0)
                chartADX.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartADX.ChartAreas[0].AxisX;
            HA.AxisY = chartADX.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartADX.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartADX.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartADX.ChartAreas[0].AxisX;
            VA.AxisY = chartADX.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartADX.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartADX.Annotations.Add(VA);

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