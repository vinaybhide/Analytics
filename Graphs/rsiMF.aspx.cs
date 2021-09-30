using Analytics.Graphs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics.Graphs
{
    public partial class rsiMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {

                    ViewState["RSIData"] = null;
                    ViewState["SchemeName"] = null;

                    if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null) &&
                        (Request.QueryString["fromdate"] != null) && (Request.QueryString["todate"] != null))
                    {
                        Master.textboxFromDate.Text = Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");  //DateTime.Today.AddDays(-90).ToString("dd-MM-yyyy");
                        ViewState["FromDate"] = Request.QueryString["fromdate"].ToString();

                        Master.textboxToDate.Text = Convert.ToDateTime(Request.QueryString["todate"].ToString()).ToString("yyyy-MM-dd"); // DateTime.Today.ToString("dd-MM-yyyy");
                        ViewState["ToDate"] = Request.QueryString["todate"].ToString();

                        DataTable tempTable = DataManager.getRSIDataTableFromDailyNAV(Int32.Parse(Request.QueryString["schemecode"]), fromDate: Request.QueryString["fromdate"].ToString(),
                                                                                toDate: Request.QueryString["todate"].ToString());
                        if ((tempTable != null) && (tempTable.Rows.Count > 0))
                        {
                            ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
                            this.Title = "RSI: " + ViewState["SchemeName"].ToString();

                            fillLinesCheckBoxes();
                            fillDesc();

                            ViewState["RSIData"] = tempTable;
                        }
                    }
                }
                if (ViewState["SchemeName"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph();

                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartRSI.Visible = true;
                        chartRSI.Width = int.Parse(Master.panelWidth.Value);
                        chartRSI.Height = int.Parse(Master.panelHeight.Value);
                    }
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
                    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
                    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Server.Transfer("~/Default.aspx");
                //Response.Redirect("~/Default.aspx");
            }
        }
        public void fillLinesCheckBoxes()
        {
            Master.checkboxlistLines.Visible = false;
            return;
            //Master.checkboxlistLines.Visible = true;
            //ListItem li = new ListItem("ADX", "ADX");
            //li.Selected = true;
            //Master.checkboxlistLines.Items.Add(li);
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("You can change the date range to see different range of NAV for selected Scheme. Click the Reset Graph button to refresh the graph.");
            Master.bulletedlistDesc.Items.Add("The relative strength index(RSI) is a momentum indicator used in technical analysis that measures the magnitude of recent price changes to evaluate overbought or oversold conditions in the price of a stock or other asset.");
            Master.bulletedlistDesc.Items.Add("Traditional interpretation and usage of the RSI are that values of 70 or above indicate that a security is becoming overbought or overvalued and may be primed for a trend reversal or corrective pullback in price.An RSI reading of 30 or below indicates an oversold or undervalued condition.");
        }

        public void ShowGraph()
        {
            if (ViewState["RSIData"] != null)
            {
                GridViewData.DataSource = (DataTable)ViewState["RSIData"];
                GridViewData.DataBind();
                chartRSI.DataSource = (DataTable)ViewState["RSIData"];
                chartRSI.DataBind();

                Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
            }
        }

        protected void chartRSI_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (chartRSI.Annotations.Count > 0)
                    chartRSI.Annotations.Clear();
                if (e.PostBackValue.Split(',')[0].Equals("AnnotationClicked"))
                    return;

                DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
                double lineWidth = xDate.ToOADate();

                double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

                //double lineHeight = -35;


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartRSI.ChartAreas[0].AxisX;
                HA.AxisY = chartRSI.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartRSI.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartRSI.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartRSI.ChartAreas[0].AxisX;
                VA.AxisY = chartRSI.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartRSI.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartRSI.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartRSI.ChartAreas[0].AxisX;
                ra.AxisY = chartRSI.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\nRSI: " + e.PostBackValue.Split(',')[1];
                ra.PostBackValue = "AnnotationClicked";
                //ra.SmartLabelStyle = sl;

                chartRSI.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }
        public void buttonShowGraph_Click()
        {
            ViewState["FromDate"] = Master.textboxFromDate.Text;
            ViewState["ToDate"] = Master.textboxToDate.Text;

            ViewState["RSIData"] = null;
            ViewState["SchemeName"] = null;

            if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null))
            {
                DataTable tempTable = MfDataAPI.getRSIDataTableFromDailyNAV(schemetypeid: Int32.Parse(Request.QueryString["schemetypeid"]),
                                                                        fundhousecode: Int32.Parse(Request.QueryString["fundhousecode"]),
                                                                        Int32.Parse(Request.QueryString["schemecode"]), 
                                                                        fromDate: Master.textboxFromDate.Text, toDate: Master.textboxToDate.Text);
                if ((tempTable != null) && (tempTable.Rows.Count > 0))
                {
                    ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
                    ViewState["RSIData"] = tempTable;
                }
            }

            if (ViewState["RSIData"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                ShowGraph();

                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    chartRSI.Visible = true;
                    chartRSI.Width = int.Parse(Master.panelWidth.Value);
                    chartRSI.Height = int.Parse(Master.panelHeight.Value);
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
                Server.Transfer("~/" + Request.QueryString["parent"].ToString());
                //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
            }
        }

        protected void buttonShowGrid_Click()
        {
            if (GridViewData.Visible)
            {
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                //if (ViewState["FetchedData"] != null)
                //{
                GridViewData.Visible = true;
                Master.buttonShowGrid.Text = "Hide Raw Data";
                //GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                //GridViewData.DataBind();
                //}
            }
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["RSIData"];
            GridViewData.DataBind();
        }

        //protected void buttonDesc_Click(object sender, EventArgs e)
        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }
        protected void chart_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetCursor1", "document.body.style.cursor = 'default';", true);
        }
    }
}