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
            Master.OnDoEventShowGraph += new complexgraphs.DoEventShowGraph(buttonShowGraph_Click);
            Master.OnDoEventShowGrid += new complexgraphs.DoEventShowGrid(buttonShowGrid_Click);
            Master.OnDoEventToggleDesc += new complexgraphs.DoEventToggleDesc(buttonDesc_Click);
            this.Title = "Intra-day Indicator";
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
                    if (!IsPostBack)
                    {
                        Master.headingtext.Text = "Intra-day Indicator: " + Request.QueryString["script"].ToString();
                        fillLinesCheckBoxes();
                        fillDesc();
                    }
                    ShowGraph(Request.QueryString["script"].ToString());
                    if (Master.panelWidth.Value != "" && Master.panelHeight.Value != "")
                    {
                        //GetDaily(scriptName);
                        chartVWAP_Intra.Visible = true;
                        chartVWAP_Intra.Width = int.Parse(Master.panelWidth.Value);
                        chartVWAP_Intra.Height = int.Parse(Master.panelHeight.Value);
                    }
                }
                else
                {
                    Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                    Server.Transfer("~/" + Request.QueryString["parent"].ToString());
                    //Response.Redirect("~/" + Request.QueryString["parent"].ToString());
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Server.Transfer("~/Default.aspx");
                //Response.Redirect("~/Default.aspx");
            }
        }

        public void fillLinesCheckBoxes()
        {
            //Master.checkboxlistLines.Visible = false;
            //return;
            Master.checkboxlistLines.Visible = true;
            ListItem li;

            li = new ListItem("VWAP", "VWAP");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Candlestick", "OHLC");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Open", "Open");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("High", "High");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Low", "Low");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);
            li = new ListItem("Close", "Close");
            li.Selected = false;
            Master.checkboxlistLines.Items.Add(li);

            li = new ListItem("Volume", "Volume");
            li.Selected = true;
            Master.checkboxlistLines.Items.Add(li);

        }

        public void fillDesc()
        {
            Master.bulletedlistDesc.Items.Add("The directional movement indicator(also known as the directional movement index or DMI) is a valuable tool for assessing price direction and strength.");
            Master.bulletedlistDesc.Items.Add("The DMI is especially useful for trend trading strategies because it differentiates between strong and weak trends, allowing the trader to enter only the ones with real momentum.");
            Master.bulletedlistDesc.Items.Add("DMI tells you when to be long or short.");
            Master.bulletedlistDesc.Items.Add("DMI comprises of two lines, +DMI & -DMI.The line which is on top is referred as dominant DMI.The dominant DMI is stronger and more likely to predict the direction of price.For the buyers and sellers to change dominance, the lines must cross over.");
            Master.bulletedlistDesc.Items.Add("The + DMI generally moves in sync with price, which means the + DMI rises when price rises, and it falls when price falls. It is important to note that the - DMI behaves in the opposite manner and moves counter - directional to price. The - DMI rises when price falls, and it falls when price rises.");
            Master.bulletedlistDesc.Items.Add("Reading directional signals is easy.");
            Master.bulletedlistDesc.Items.Add("When the + DMI is dominant and rising, price direction is up.");
            Master.bulletedlistDesc.Items.Add("When the - DMI is dominant and rising, price direction is down.");
            Master.bulletedlistDesc.Items.Add("But the strength of price must also be considered.DMI strength ranges from a low of 0 to a high of 100. The higher the DMI value, the stronger the prices swing.");
            Master.bulletedlistDesc.Items.Add("DMI values over 25 mean price is directionally strong. DMI values under 25 mean price is directionally weak.");
            Master.bulletedlistDesc.Items.Add("When the buyers are stronger than the sellers, the + DMI peaks will be above 25 and the - DMI peaks will be below 25. This is seen in a strong uptrend.But when the sellers are stronger than the buyers, the - DMI peaks will be above 25 and the + DMI peaks will be below 25.In this case, the trend will be down.");

            Master.bulletedlistDesc.Items.Add("The volume weighted average price(VWAP) is a trading benchmark that gives the average price a security has traded at throughout the day, based on both volume and price.It is important because it provides you with insight into both the trend and value of a security.");
            Master.bulletedlistDesc.Items.Add("Large institutional buyers will try to buy below the VWAP, or sell above it. This way their actions push the price back toward the average, instead of away from it.");
            Master.bulletedlistDesc.Items.Add("Traders may use VWAP as a trend confirmation tool, and build trading rules around it.");
            Master.bulletedlistDesc.Items.Add("For example, when the price is above VWAP they may prefer to initiate long positions.  When the price is below VWAP they may prefer to initiate short positions.");
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

            try
            {
                if (((ViewState["FetchedDataIntra"] == null) || (ViewState["FetchedDataVWAP"] == null))
                    ||
                    ((((DataTable)ViewState["FetchedDataIntra"]).Rows.Count == 0) || (((DataTable)ViewState["FetchedDataVWAP"]).Rows.Count == 0))
                    )
                {
                    if (Session["IsTestOn"] != null)
                    {
                        bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                    }

                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }
                    if ((Request.QueryString["size"] != null) && (Request.QueryString["interval_intra"] != null)
                        && (Request.QueryString["interval_vwap"] != null))
                    {
                        outputSize = Request.QueryString["size"].ToString();
                        interval_intra = Request.QueryString["interval_intra"];
                        intraData = StockApi.getIntraday(folderPath, scriptName, time_interval: interval_intra, outputsize: outputSize,
                                                        bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataIntra"] = intraData;

                        interval_vwap = Request.QueryString["interval_vwap"];

                        vwapData = StockApi.getVWAP(folderPath, scriptName, day_interval: interval_vwap,
                                                    bIsTestModeOn: bIsTestOn, bSaveData: false, apiKey: Session["ApiKey"].ToString());
                        ViewState["FetchedDataVWAP"] = vwapData;

                    }
                    else
                    {
                        ViewState["FetchedDataIntra"] = null;
                        intraData = null;

                        ViewState["FetchedDataVWAP"] = null;
                        vwapData = null;
                    }

                }
                //else
                //{
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
                //}

                if ((intraData != null) && (vwapData != null))
                {
                    //showCandleStickGraph(intraData);
                    //showVWAP(vwapData);
                    chartVWAP_Intra.Series["Open"].Points.DataBind(intraData.AsEnumerable(), "Date", "Open", "");
                    chartVWAP_Intra.Series["High"].Points.DataBind(intraData.AsEnumerable(), "Date", "High", "");
                    chartVWAP_Intra.Series["Low"].Points.DataBind(intraData.AsEnumerable(), "Date", "Low", "");
                    chartVWAP_Intra.Series["Close"].Points.DataBind(intraData.AsEnumerable(), "Date", "Close", "");
                    chartVWAP_Intra.Series["Volume"].Points.DataBind(intraData.AsEnumerable(), "Date", "Volume", "");
                    chartVWAP_Intra.Series["OHLC"].Points.DataBind(intraData.AsEnumerable(), "Date", "Open,High,Low,Close", "");
                    chartVWAP_Intra.Series["VWAP"].Points.DataBind(vwapData.AsEnumerable(), "Date", "VWAP", "");
                    chartVWAP_Intra.ChartAreas[0].AxisX.LabelStyle.Format = "g";
                    chartVWAP_Intra.ChartAreas[0].AxisX2.LabelStyle.Format = "g";
                    chartVWAP_Intra.ChartAreas[1].AxisX.LabelStyle.Format = "g";

                    chartVWAP_Intra.ChartAreas[0].AxisX.IsStartedFromZero = true;
                    chartVWAP_Intra.ChartAreas[0].AxisX2.IsStartedFromZero = true;
                    chartVWAP_Intra.ChartAreas[1].AxisX.IsStartedFromZero = true;

                    foreach (ListItem item in Master.checkboxlistLines.Items)
                    {
                        chartVWAP_Intra.Series[item.Value].Enabled = item.Selected;
                        if (item.Selected == false)
                        {
                            if (chartVWAP_Intra.Annotations.FindByName(item.Value) != null)
                                chartVWAP_Intra.Annotations.Clear();
                        }
                    }
                }
                else
                {
                    Master.headingtext.Text = "Intra-day Indicator-" + Request.QueryString["script"].ToString() + "---DATA NOT AVAILABLE. Please try again later.";
                    Master.headingtext.BackColor = Color.Red;
                    Master.headingtext.CssClass = "blinking blinkingText";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while generating graph: " + ex.Message + "')</script>");
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
            //string legendName;
            int chartIndex;

            //DataPoint p;
            //double lineHeight = -35;

            try
            {
                postBackValues = e.PostBackValue.Split(',');

                if (chartVWAP_Intra.Annotations.Count > 0)
                    chartVWAP_Intra.Annotations.Clear();

                if (postBackValues[0].Equals("AnnotationClicked"))
                {
                    return;
                }

                xDate = System.Convert.ToDateTime(postBackValues[1]);
                lineWidth = xDate.ToOADate();
                lineHeight = System.Convert.ToDouble(postBackValues[2]);

                seriesName = postBackValues[0];


                HorizontalLineAnnotation HA = new HorizontalLineAnnotation();
                //HA.Name = seriesName;
                VerticalLineAnnotation VA = new VerticalLineAnnotation();
                RectangleAnnotation ra = new RectangleAnnotation();

                if (seriesName.Equals("VWAP"))
                {
                    HA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX2;
                    HA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY2;

                    VA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX2;
                    VA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY2;

                    ra.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX2;
                    ra.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY2;
                    chartIndex = 0;
                }
                else if (seriesName.Equals("Volume"))
                {
                    HA.AxisX = chartVWAP_Intra.ChartAreas[1].AxisX;
                    HA.AxisY = chartVWAP_Intra.ChartAreas[1].AxisY;

                    VA.AxisX = chartVWAP_Intra.ChartAreas[1].AxisX;
                    VA.AxisY = chartVWAP_Intra.ChartAreas[1].AxisY;

                    ra.AxisX = chartVWAP_Intra.ChartAreas[1].AxisX;
                    ra.AxisY = chartVWAP_Intra.ChartAreas[1].AxisY;

                    chartIndex = 1;
                }
                else
                {
                    HA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX;
                    HA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY;

                    VA.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX;
                    VA.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY;

                    ra.AxisX = chartVWAP_Intra.ChartAreas[0].AxisX;
                    ra.AxisY = chartVWAP_Intra.ChartAreas[0].AxisY;
                    chartIndex = 0;
                }

                HA.IsSizeAlwaysRelative = false;
                HA.AnchorY = lineHeight;
                HA.IsInfinitive = true;
                HA.ClipToChartArea = chartVWAP_Intra.ChartAreas[chartIndex].Name;
                HA.LineDashStyle = ChartDashStyle.Dash;
                HA.LineColor = Color.Red;
                HA.LineWidth = 1;
                chartVWAP_Intra.Annotations.Add(HA);

                //VA.Name = seriesName;
                VA.IsSizeAlwaysRelative = false;
                VA.AnchorX = lineWidth;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = chartVWAP_Intra.ChartAreas[chartIndex].Name;
                VA.LineDashStyle = ChartDashStyle.Dash;
                VA.LineColor = Color.Red;
                VA.LineWidth = 1;
                chartVWAP_Intra.Annotations.Add(VA);

                ra.Name = seriesName;
                ra.IsSizeAlwaysRelative = true;
                ra.AnchorX = lineWidth;
                ra.AnchorY = lineHeight;
                ra.IsMultiline = true;
                //ra.ClipToChartArea = chartADX.ChartAreas[0].Name;
                ra.LineDashStyle = ChartDashStyle.Solid;
                ra.LineColor = Color.Blue;
                ra.LineWidth = 1;
                ra.PostBackValue = "AnnotationClicked";

                if (seriesName.Equals("OHLC"))
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + "Open:" + postBackValues[2] + "\n" + "High:" + postBackValues[3] + "\n" +
                                "Low:" + postBackValues[4] + "\n" + "Close:" + postBackValues[5];
                }
                else
                {
                    ra.Text = "Date:" + postBackValues[1] + "\n" + seriesName + ":" + postBackValues[2];
                }
                //ra.SmartLabelStyle = sl;

                chartVWAP_Intra.Annotations.Add(ra);


                //p = (chartVWAP_Intra.Series[seriesName]).Points.FindByValue(lineHeight, "Y");

                //if (p != null)
                //{
                //    p.MarkerSize = 8;
                //    p.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Diamond;
                //    p.Label = e.PostBackValue.Split(',')[2] + "\n" + e.PostBackValue.Split(',')[0] + "\n" + e.PostBackValue.Split(',')[1];
                //    p.LabelBackColor = System.Drawing.Color.Transparent;
                //    p.LabelBorderDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
                //    p.LabelBorderColor = System.Drawing.Color.Black;
                //    p.IsValueShownAsLabel = true;
                //}
                //}
                //else
                //{
                //    legendName = e.PostBackValue;
                //    if (legendName.ToUpper().Equals("OHLC"))
                //    {
                //        chartVWAP_Intra.Series["OHLC"].Enabled = !(chartVWAP_Intra.Series["OHLC"].Enabled);
                //    }
                //    else
                //    {
                //        chartVWAP_Intra.Series["VWAP"].Enabled = !(chartVWAP_Intra.Series["VWAP"].Enabled);
                //    }
                //}
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while ploting lines: " + ex.Message + "')</script>");
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

        //protected void buttonDesc_Click(object sender, EventArgs e)
        public void buttonDesc_Click()
        {
            if (Master.bulletedlistDesc.Visible)
                Master.bulletedlistDesc.Visible = false;
            else
                Master.bulletedlistDesc.Visible = true;
        }


        protected void GridViewDaily_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDaily.PageIndex = e.NewPageIndex;
            GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataIntra"];
            GridViewDaily.DataBind();
        }

        protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewData.PageIndex = e.NewPageIndex;
            GridViewData.DataSource = (DataTable)ViewState["FetchedDataVWAP"];
            GridViewData.DataBind();
        }
        void buttonShowGrid_Click()
        {
            if ((GridViewDaily.Visible) || (GridViewData.Visible))
            {
                GridViewDaily.Visible = false;
                GridViewData.Visible = false;
                Master.buttonShowGrid.Text = "Show Raw Data";
            }
            else
            {
                Master.buttonShowGrid.Text = "Hide Raw Data";
                if (ViewState["FetchedDataIntra"] != null)
                {
                    GridViewDaily.Visible = true;
                    GridViewDaily.DataSource = (DataTable)ViewState["FetchedDataIntra"];
                    GridViewDaily.DataBind();
                }
                if (ViewState["FetchedDataVWAP"] != null)
                {
                    GridViewData.Visible = true;
                    GridViewData.DataSource = (DataTable)ViewState["FetchedDataVWAP"];
                    GridViewData.DataBind();
                }
            }
        }

    }
}