﻿using System;
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
    public partial class ema : System.Web.UI.Page
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
                headingtext.InnerText = "Exponential moving average:" + Request.QueryString["script"].ToString();
                if (panelWidth.Value != "" && panelHeight.Value != "")
                {
                    chartEMA.Visible = true;
                    chartEMA.Width = int.Parse(panelWidth.Value);
                    chartEMA.Height = int.Parse(panelHeight.Value);
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
            string period = "";
            string seriestype = "";
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
                if ((Request.QueryString["period"] != null) && (Request.QueryString["interval"] != null) && (Request.QueryString["seriestype"] != null))
                {
                    interval = Request.QueryString["interval"];
                    period = Request.QueryString["period"].ToString();
                    seriestype = Request.QueryString["seriestype"].ToString();

                    scriptData = StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: period,
                        seriestype: seriestype, bIsTestModeOn: bIsTestOn, bSaveData: false);
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
                (chartEMA.Series["seriesEMA"]).XValueMember = "Date";
                (chartEMA.Series["seriesEMA"]).XValueType = ChartValueType.Date;
                (chartEMA.Series["seriesEMA"]).YValueMembers = "EMA";
                //(chartEMA.Series["seriesEMA"]).ToolTip = "EMA: Date:#VALX;   Value:#VALY";

                chartEMA.ChartAreas["chartareaEMA"].AxisX.Title = "Date";
                chartEMA.ChartAreas["chartareaEMA"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                chartEMA.ChartAreas["chartareaEMA"].AxisY.Title = "Value";
                chartEMA.ChartAreas["chartareaEMA"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                //chartEMA.Titles["titleEMA"].Text = $"{"Exponential Moving Average- "}{scriptName}";

                if (chartEMA.Annotations.Count > 0)
                    chartEMA.Annotations.Clear();

                chartEMA.DataSource = scriptData;
                chartEMA.DataBind();
            }
        }

        protected void chartEMA_Click(object sender, ImageMapEventArgs e)
        {
            DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
            double lineWidth = xDate.ToOADate();

            double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

            //double lineHeight = -35;

            if (chartEMA.Annotations.Count > 0)
                chartEMA.Annotations.Clear();

            HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
            HA.AxisX = chartEMA.ChartAreas[0].AxisX;
            HA.AxisY = chartEMA.ChartAreas[0].AxisY;
            HA.IsSizeAlwaysRelative = false;
            HA.AnchorY = lineHeight;
            HA.IsInfinitive = true;
            HA.ClipToChartArea = chartEMA.ChartAreas[0].Name;
            HA.LineDashStyle = ChartDashStyle.Dash;
            HA.LineColor = Color.Red;
            HA.LineWidth = 1;
            chartEMA.Annotations.Add(HA);

            VerticalLineAnnotation VA = new VerticalLineAnnotation();
            VA.AxisX = chartEMA.ChartAreas[0].AxisX;
            VA.AxisY = chartEMA.ChartAreas[0].AxisY;
            VA.IsSizeAlwaysRelative = false;
            VA.AnchorX = lineWidth;
            VA.IsInfinitive = true;
            VA.ClipToChartArea = chartEMA.ChartAreas[0].Name;
            VA.LineDashStyle = ChartDashStyle.Dash;
            VA.LineColor = Color.Red;
            VA.LineWidth = 1;
            chartEMA.Annotations.Add(VA);

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