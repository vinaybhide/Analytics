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
    public partial class dailygraphMF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            //this.Title = "Daily NAV Price: " + Request.QueryString["fundname"].ToString();
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    //ViewState["FetchedData"] = null;
                    ViewState["SchemeData"] = null;
                    ViewState["SchemeName"] = null;

                    if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null) &&
                        (Request.QueryString["fromdate"] != null) && (Request.QueryString["todate"] != null))
                    {
                        Master.textboxFromDate.Text = Convert.ToDateTime(Request.QueryString["fromdate"].ToString()).ToString("yyyy-MM-dd");  //DateTime.Today.AddDays(-90).ToString("dd-MM-yyyy");
                        ViewState["FromDate"] = Request.QueryString["fromdate"].ToString();

                        Master.textboxToDate.Text = Convert.ToDateTime(Request.QueryString["todate"].ToString()).ToString("yyyy-MM-dd"); // DateTime.Today.ToString("dd-MM-yyyy");
                        ViewState["ToDate"] = Request.QueryString["todate"].ToString();



                        DataTable tempTable = DataManager.getNAVRecordsTable(Int32.Parse(Request.QueryString["schemecode"]), fromDate: Request.QueryString["fromdate"].ToString(), 
                                                                                toDate: Request.QueryString["todate"].ToString());
                        if ((tempTable != null) && (tempTable.Rows.Count > 0))
                        {
                            ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
                            this.Title = "Daily NAV Price: " + ViewState["SchemeName"].ToString();

                            fillLinesCheckBoxes();
                            fillDesc();

                            ViewState["SchemeData"] = tempTable;
                        }
                    }
                }
                if (ViewState["SchemeName"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph();
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //ShowGraph(scriptName);
                        chartdailyGraphMF.Visible = true;
                        chartdailyGraphMF.Width = int.Parse(Master.panelWidth.Value);
                        chartdailyGraphMF.Height = int.Parse(Master.panelHeight.Value);
                    }
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noFundSelected + "');", true);
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
        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("You can change the date range to see different range of NAV for selected Scheme. Click the Reset Graph button to refresh the graph.");
        }

        public void ShowGraph()
        {
            if (ViewState["SchemeData"] != null)
            {
                GridViewDaily.DataSource = (DataTable)ViewState["SchemeData"];
                GridViewDaily.DataBind();
                chartdailyGraphMF.DataSource = (DataTable)ViewState["SchemeData"];
                chartdailyGraphMF.DataBind();

                Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
            }
        }
        protected void chartdailyGraphMF_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (chartdailyGraphMF.Annotations.Count > 0)
                    chartdailyGraphMF.Annotations.Clear();
                if (e.PostBackValue.Split(',')[0].Equals("AnnotationClicked"))
                    return;

                DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
                double lineWidth = xDate.ToOADate();

                double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                HA.AxisX = chartdailyGraphMF.ChartAreas[0].AxisX;
                HA.AxisY = chartdailyGraphMF.ChartAreas[0].AxisY;
                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartdailyGraphMF.ChartAreas[0].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartdailyGraphMF.Annotations.Add(HA);

                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                VA.AxisX = chartdailyGraphMF.ChartAreas[0].AxisX;
                VA.AxisY = chartdailyGraphMF.ChartAreas[0].AxisY;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartdailyGraphMF.ChartAreas[0].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartdailyGraphMF.Annotations.Add(VA);

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartdailyGraphMF.ChartAreas[0].AxisX;
                ra.AxisY = chartdailyGraphMF.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartdailyGraphMF.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\nNAV: " + e.PostBackValue.Split(',')[1];
                ra.PostBackValue = "AnnotationClicked";
                //ra.SmartLabelStyle = sl;

                chartdailyGraphMF.Annotations.Add(ra);
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

            ViewState["SchemeData"] = null;
            ViewState["SchemeName"] = null;

            if ((Request.QueryString["schemetypeid"] != null) && (Request.QueryString["fundhousecode"] != null) && (Request.QueryString["schemecode"] != null))
            {
                DataTable tempTable = DataManager.getNAVRecordsTable(Int32.Parse(Request.QueryString["schemecode"]),
                                                                        fromDate: Master.textboxFromDate.Text, toDate: Master.textboxToDate.Text);
                if ((tempTable != null) && (tempTable.Rows.Count > 0))
                {
                    ViewState["SchemeName"] = tempTable.Rows[0]["SCHEMENAME"].ToString();
                    ViewState["SchemeData"] = tempTable;
                }
            }
            if (ViewState["SchemeData"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                ShowGraph();

                if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                {
                    chartdailyGraphMF.Visible = true;
                    chartdailyGraphMF.Width = int.Parse(Master.panelWidth.Value);
                    chartdailyGraphMF.Height = int.Parse(Master.panelHeight.Value);
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


        void buttonShowGrid_Click()
        {
            if (GridViewDaily.Visible)
            {
                GridViewDaily.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                //if (ViewState["FetchedData"] != null)
                //{
                GridViewDaily.Visible = true;
                Master.buttonShowGrid.Text = "Hide Raw Data";
                //GridViewDaily.DataSource = (DataTable)ViewState["FetchedData"];
                //GridViewDaily.DataBind();
                //}
            }
        }

        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["SchemeData"];
            GridViewDaily.DataBind();
        }

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