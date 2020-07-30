using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Analytics
{
    public partial class portfolioValuation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["FetchedData"] = null;
            }
            if (Session["PortfolioName"] != null)
            {
                string fileName = Session["PortfolioName"].ToString();
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
                Response.Redirect(".\\" + Request.QueryString["parent"].ToString());
            }
        }

        public void ShowGraph(string fileName)
        {
            bool bIsTestOn = true;
            DataTable portfolioTable = null;
            string folderPath = Server.MapPath("~/scriptdata/");
            XmlDocument xmlPortfolio;
            XmlNode root;
            XmlNodeList scriptNodeList;
            string scriptName;
            string searchPath;
            int markerInterval = 10;
            int i = 0;
            int tempQty = 0;

            if (File.Exists(fileName))
            {
                //portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestOn);

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

                    portfolioTable = StockApi.GetValuation(folderPath, fileName, bIsTestOn);
                    ViewState["FetchedData"] = portfolioTable;
                }
                else
                {
                    portfolioTable = (DataTable)ViewState["FetchedData"];
                }

                if (portfolioTable != null)
                {
                    xmlPortfolio = new XmlDocument();
                    xmlPortfolio.Load(fileName);
                    root = xmlPortfolio.DocumentElement;
                    searchPath = "/portfolio/script/name";
                    scriptNodeList = root.SelectNodes(searchPath);
                    foreach (XmlNode scriptNameNode in scriptNodeList)
                    {
                        scriptName = scriptNameNode.InnerText;
                        DataRow[] scriptRows = portfolioTable.Select("Symbol='" + scriptName + "'");

                        if (scriptRows.Length > 0)
                        {
                            chartPortfolioValuation.Series.Add(scriptName);
                            (chartPortfolioValuation.Series[scriptName]).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                            (chartPortfolioValuation.Series[scriptName]).ChartArea = "chartareaPortfolioValuation";

                            //(chartPortfolioValuation.Series[scriptName]).SmartLabelStyle.Enabled = true;
                            //(chartPortfolioValuation.Series[scriptName]).SmartLabelStyle.AllowOutsidePlotArea = System.Web.UI.DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes;
                            //(chartPortfolioValuation.Series[scriptName]).SmartLabelStyle.CalloutStyle = System.Web.UI.DataVisualization.Charting.LabelCalloutStyle.Box;
                            //i = 0;
                            tempQty = 0;
                            foreach (DataRow itemRow in scriptRows)
                            {
                                (chartPortfolioValuation.Series[scriptName]).Points.AddXY(itemRow["Date"], itemRow["ValueOnDate"]);
                                //if (i == 10)
                                //{
                                //    (chartPortfolioValuation.Series[scriptName]).Label = itemRow["ValueOnDate"].ToString();
                                //    i = 0;
                                //}
                                //i++;

                                if ((itemRow["CumulativeQuantity"] != System.DBNull.Value) && ((tempQty == 0) || (tempQty != System.Convert.ToInt32(itemRow["CumulativeQuantity"]))))
                                {
                                    tempQty = System.Convert.ToInt32(itemRow["CumulativeQuantity"]);
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerSize = 5;
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].Label = "Purchase date:" + itemRow["PurchaseDate"] + "\n Cumulative Qty=" + tempQty.ToString();
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].LabelBorderColor = System.Drawing.Color.Black;
                                    (chartPortfolioValuation.Series[scriptName]).Points[(chartPortfolioValuation.Series[scriptName]).Points.Count - 1].IsValueShownAsLabel = true;
                                }
                            }

                            //(chartPortfolioValuation.Series[scriptName]).IsValueShownAsLabel = true;
                            //(chartPortfolioValuation.Series[scriptName]).IsVisibleInLegend = true;
                            //FOllowing line shows X & Y value of point while mouse over
                            //(chartPortfolioValuation.Series[scriptName]).ToolTip = "Value of X:#VALX;   Value of Y:#VALY";
                            (chartPortfolioValuation.Series[scriptName]).ToolTip = "Date:#VALX; Value:#VALY";
                            (chartPortfolioValuation.Series[scriptName]).Legend = chartPortfolioValuation.Legends["legendValuation"].Name;
                            (chartPortfolioValuation.Series[scriptName]).LegendText = scriptName;
                            //chartPortfolioValuation.Legends.Add(scriptName);
                            //chartPortfolioValuation.Legends[scriptName].Docking = System.Web.UI.DataVisualization.Charting.Docking.Top;
                            //chartPortfolioValuation.Legends[scriptName].LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Row;
                            //chartPortfolioValuation.Legends[scriptName].BorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dash;
                            //chartPortfolioValuation.Legends[scriptName].BorderColor = System.Drawing.Color.Black;
                        }
                    }

                    chartPortfolioValuation.ChartAreas[0].AxisX.Title = "Date";
                    chartPortfolioValuation.ChartAreas[0].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    chartPortfolioValuation.ChartAreas[0].AxisY.Title = "Value";
                    chartPortfolioValuation.ChartAreas[0].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
                }

            }


        }

    }
}