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

namespace Analytics
{
    public partial class vwaprice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.OnDoEventShowGraph += new standardgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new standardgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new standardgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "VWAP: " + Request.QueryString["script"].ToString();
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
                    if (!IsPostBack)
                    {
                        //Master.headingtext.Text = "Volume weighted average price:" + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
                    ShowGraph(Request.QueryString["script"].ToString());

                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        chartVWAP.Visible = true;
                        chartVWAP.Width = int.Parse(Master.panelWidth.Value);
                        chartVWAP.Height = int.Parse(Master.panelHeight.Value);
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
            Master.bulletedlistDesc.Items.Add("The volume weighted average price(VWAP) is a trading benchmark used by traders that gives the average price a security has traded at throughout the day, based on both volume and price.");
            Master.bulletedlistDesc.Items.Add("Let’s face it, at its fundamental level, if we had to compare two seemingly good securities, more often than not, we would check its price trend and the trading volume. Price is obvious, but why the volume? Volume is important as we don’t want to get stuck with a stock which has few takers, even if you think it is priced attractively. Thus, the VWAP was created to take into account both volume as well as Price so that the potential investor would make the trading decision or not.");
            Master.bulletedlistDesc.Items.Add("It is important because it provides traders with insight into both the trend and value of a security.");
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
                    if (Request.QueryString["interval"] != null)
                    {
                        interval = Request.QueryString["interval"];

                        //scriptData = StockApi.getVWAP(folderPath, scriptName, day_interval: interval,
                        //                            bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());

                        //if(scriptData == null)
                        //{
                            scriptData = StockApi.getVWAPAlternate(folderPath, scriptName, time_interval: interval,
                                                        bIsTestModeOn: false, bSaveData: false, apiKey: Session["ApiKey"].ToString(), intraDataTable: null);
                        //}
                    }
                    ViewState["FetchedData"] = scriptData;
                    GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
                    GridViewData.DataBind();
                }
                //else
                //{
                if (ViewState["FromDate"] != null)
                    //fromDate = System.Convert.ToDateTime(ViewState["FromDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                    fromDate = ViewState["FromDate"].ToString();
                if (ViewState["ToDate"] != null)
                    //toDate = System.Convert.ToDateTime(ViewState["ToDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
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
                //}

                if (scriptData != null)
                {
                    //Moving below code toaspx
                    //(chartVWAP.Series["seriesVWAP"]).XValueMember = "Date";
                    //(chartVWAP.Series["seriesVWAP"]).XValueType = ChartValueType.Date;
                    //(chartVWAP.Series["seriesVWAP"]).YValueMembers = "VWAP";
                    ////(chartVWAP.Series["seriesVWAP"]).ToolTip = "VWAP: Date:#VALX;   Value:#VALY";

                    //chartVWAP.ChartAreas["chartareaVWAP"].AxisX.Title = "Date";
                    //chartVWAP.ChartAreas["chartareaVWAP"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
                    //chartVWAP.ChartAreas["chartareaVWAP"].AxisY.Title = "Value";
                    //chartVWAP.ChartAreas["chartareaVWAP"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;

                    //moved below line to aspx
                    //chartVWAP.ChartAreas["chartareaVWAP"].AxisX.LabelStyle.Format = "g";

                    //chartVWAP.Titles["titleVWAP"].Text = $"{"Volume Weighted Average Price - "}{scriptName}";

                    //if (chartVWAP.Annotations.Count > 0)
                    //    chartVWAP.Annotations.Clear();

                    chartVWAP.DataSource = scriptData;
                    chartVWAP.DataBind();
                    //Master.headingtext.Text = "Volume weighted average price:" + Request.QueryString["script"].ToString();
                    Master.headingtext.CssClass = Master.headingtext.CssClass.Replace("blinking blinkingText", "");
                }
                else
                {
                    if (expression.Length == 0)
                    {
                        Master.headingtext.Text = "Volume Weighted Avg Price:" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    }
                    else
                    {
                        Master.headingtext.Text = "Volume Weighted Avg Price:" + Request.QueryString["script"].ToString() + "---Invalid filter. Please correct filter & retry.";
                    }
                    //Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while generating graph:" + ex.Message+ "');", true);
            }
        }

        protected void chartVWAP_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (chartVWAP.Annotations.Count > 0)
                    chartVWAP.Annotations.Clear();

                if (e.PostBackValue.Split(',')[0].Equals("AnnotationClicked"))
                    return;

                DateTime xDate = System.Convert.ToDateTime(e.PostBackValue.Split(',')[0]);
                double lineWidth = xDate.ToOADate();

                double lineHeight = System.Convert.ToDouble(e.PostBackValue.Split(',')[1]);

                //double lineHeight = -35;


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

                RectangleAnnotation ra = new RectangleAnnotation();
                ra.AxisX = chartVWAP.ChartAreas[0].AxisX;
                ra.AxisY = chartVWAP.ChartAreas[0].AxisY;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.Text = "Date: " + e.PostBackValue.Split(',')[0] + "\nVWAP: " + e.PostBackValue.Split(',')[1];
                ra.PostBackValue = "AnnotationClicked";
                //ra.SmartLabelStyle = sl;

                chartVWAP.Annotations.Add(ra);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while plotting lines:" + ex.Message + "');", true);
            }
        }

        //protected void buttonShowGraph_Click(object sender, EventArgs e)
        public void buttonShowGraph_Click()
        {
            string scriptName = Request.QueryString["script"].ToString();
            ViewState["FromDate"] = Master.textboxFromDate.Text;
            ViewState["ToDate"] = Master.textboxToDate.Text;
            ShowGraph(scriptName);
        }

        //protected void buttonShowGrid_Click(object sender, EventArgs e)
        public void buttonShowGrid_Click()
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
            GridViewData.DataSource = (DataTable)ViewState["FetchedData"];
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