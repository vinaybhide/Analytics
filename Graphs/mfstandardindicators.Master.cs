using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class mfstandardindicators : System.Web.UI.MasterPage
    {
        public HiddenField panelWidth
        {
            get
            {
                // Return the textbox on the master page
                return this.panelWidthM;
            }
        }

        public HiddenField panelHeight
        {
            get
            {
                // Return the textbox on the master page
                return this.panelHeightM;
            }
        }

        public Label headingtext
        {
            get
            {
                // Return the textbox on the master page
                return this.headingtextM;
            }
        }

        public TextBox textboxFromDate
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxFromDateM;
            }
        }

        public TextBox textboxToDate
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxToDateM;
            }
        }
        //public BulletedList bulletedlistDesc
        //{
        //    get
        //    {
        //        // Return the textbox on the master page
        //        return this.bulletedlistDescM;
        //    }
        //}
        //public CheckBoxList checkboxlistLines
        //{
        //    get
        //    {
        //        // Return the textbox on the master page
        //        return this.checkboxlistLinesM;
        //    }
        //}
        public Button buttonDesc
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonDesc;
            }
        }
        public Button buttonShowGrid
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonShowGridM;
            }
        }

        public DropDownList fundHouseList
        {
            get
            {
                return ddlFundHouse;
            }
        }

        public DropDownList fundNameList
        {
            get
            {
                return ddlFundName;
            }
        }

        public TextBox selectedFundName
        {
            get
            {
                return textboxSelectedFundName;
            }
        }

        public TextBox selectedSchemeCode
        {
            get
            {
                return textboxSchemeCode;
            }
        }

        public DropDownList indicatorList
        {
            get
            {
                return ddlIndicator;
            }
        }

        public TextBox textboxFastPeriod
        {
            get
            {
                return textboxFastPeriodM;
            }
        }

        public TextBox textboxSlowPeriod
        {
            get
            {
                return textboxSlowPeriodM;
            }
        }
        public TextBox textboxRSIPeriod
        {
            get
            {
                return textboxRSIPeriodM;
            }
        }
        public TextBox textboxStdDev
        {
            get
            {
                return textboxStdDevM;
            }
        }
        public TextBox textboxSignal
        {
            get
            {
                return textboxSignalM;
            }
        }
        public delegate void DoEventShowGraph();
        public event DoEventShowGraph OnDoEventShowGraph;

        public delegate void DoEventShowGrid();
        public event DoEventShowGrid OnDoEventShowGrid;

        public delegate void DoEventToggleDesc();
        public event DoEventToggleDesc OnDoEventToggleDesc;

        public delegate void DoEventRemoveSelectedIndicatorGraph();
        public event DoEventRemoveSelectedIndicatorGraph OnDoEventRemoveSelectedIndicatorGraph;

        public delegate void DoEventShowSelectedIndicatorGraph();
        public event DoEventShowSelectedIndicatorGraph OnDoEventShowSelectedIndicatorGraph;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    GetIndexValues(null, null);
                    //ViewState["MFMasterTable"] = null;
                    //ViewState["MFSchemeTable"] = null;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Server.Transfer("~/Default.aspx");
            }

        }

        public void LoadFundHouseList()
        {
            ViewState["MFSchemeTable"] = null;
            DataManager dataMgr = new DataManager();
            DataTable fundHouseTable = dataMgr.getFundHouseTable();
            if ((fundHouseTable != null) && (fundHouseTable.Rows.Count > 0))
            {
                // Columns - FUNDHOUSECODE, NAME
                ddlFundHouse.DataTextField = "NAME";
                ddlFundHouse.DataValueField = "FUNDHOUSECODE";
                ddlFundHouse.DataSource = fundHouseTable;
                ddlFundHouse.DataBind();
            }
        }

        public void LoadFundNameList()
        {
            ViewState["MFSchemeTable"] = null;

            ddlFundName.Enabled = true;
            ddlFundName.Items.Clear();
            textboxSchemeCode.Text = "";

            DataManager dataMgr = new DataManager();
            DataTable mfSchemeTable = dataMgr.getSchemesTable(fundhousecode: System.Convert.ToInt32(ddlFundHouse.SelectedValue));
            if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
            {
                //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
                ddlFundName.DataTextField = "SCHEMENAME";
                ddlFundName.DataValueField = "SCHEMECODE";
                ddlFundName.DataSource = mfSchemeTable;
                ddlFundName.DataBind();

                ListItem li = new ListItem("-- Select Fund Name --", "-1");
                ddlFundName.Items.Insert(0, li);
                ViewState["MFSchemeTable"] = mfSchemeTable;
            }
        }
        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedValue != "-1")
            {
                LoadFundNameList();

                //ViewState["MFSchemeTable"] = null;

                //ddlFundName.Enabled = true;
                //ddlFundName.Items.Clear();
                //textboxSchemeCode.Text = "";

                //DataManager dataMgr = new DataManager();
                //DataTable mfSchemeTable = dataMgr.getSchemesTable(fundhousecode: System.Convert.ToInt32(ddlFundHouse.SelectedValue));
                //if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
                //{
                //    //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
                //    ddlFundName.DataTextField = "SCHEMENAME";
                //    ddlFundName.DataValueField = "SCHEMECODE";
                //    ddlFundName.DataSource = mfSchemeTable;
                //    ddlFundName.DataBind();

                //    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                //    ddlFundName.Items.Insert(0, li);
                //    ViewState["MFSchemeTable"] = mfSchemeTable;
                //}
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund data not found for selected fund house. Please select another fund house.');", true);
            }
        }

        protected void buttonSearchFUndName_Click(object sender, EventArgs e)
        {
            if (ViewState["MFSchemeTable"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund house and then search for fund name');", true);
            }
            else
            {
                DataTable fundNameTable = (DataTable)ViewState["MFSchemeTable"];
                StringBuilder filter = new StringBuilder();
                if (!(string.IsNullOrEmpty(textboxSelectedFundName.Text)))
                    filter.Append("SCHEMENAME Like '%" + textboxSelectedFundName.Text + "%'");
                DataView dv = fundNameTable.DefaultView;
                dv.RowFilter = filter.ToString();
                if (dv.Count > 0)
                {
                    ddlFundName.Items.Clear();
                    ddlFundName.DataTextField = "SCHEMENAME";
                    ddlFundName.DataValueField = "SCHEMECODE";
                    ddlFundName.DataSource = dv;//mfFundList.DefaultView;
                    ddlFundName.DataBind();
                    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                    ddlFundName.Items.Insert(0, li);
                }
            }
        }

        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundName.SelectedValue != "-1")
            {
                textboxSchemeCode.Text = ddlFundName.SelectedValue;
                textboxSelectedFundName.Text = ddlFundName.Items[ddlFundName.SelectedIndex].Text;
            }
        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowGraph != null)
            {
                OnDoEventShowGraph();
            }
        }

        protected void buttonShowGrid_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowGrid != null)
            {
                OnDoEventShowGrid();
            }
        }

        protected void buttonDescM_Click(object sender, EventArgs e)
        {
            if (OnDoEventToggleDesc != null)
            {
                OnDoEventToggleDesc();
            }
        }

        protected void buttonRemoveSelectedIndicatorGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventRemoveSelectedIndicatorGraph != null)
            {
                OnDoEventRemoveSelectedIndicatorGraph();
            }
        }

        protected void buttonShowSelectedIndicatorGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowSelectedIndicatorGraph != null)
            {
                OnDoEventShowSelectedIndicatorGraph();
            }
        }

        protected void GetIndexValues(object sender, EventArgs e)
        {
            //Use myQuote.close.Last() - myMeta.chartPreviousClose to show difference
            //(myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100 to show percentage diff
            StockManager stockManager = new StockManager();
            StringBuilder indexString = new StringBuilder();

            DataAccessLayer.Chart myChart;
            DataAccessLayer.Result myResult;
            DataAccessLayer.Meta myMeta;
            DataAccessLayer.Indicators myIndicators;
            DataAccessLayer.Quote myQuote;
            DateTime myDate;
            DataAccessLayer.Root myDeserializedClass = stockManager.getIndexIntraDayAlternate("^BSESN", time_interval: "1min", outputsize: "compact");

            if (myDeserializedClass != null)
            {
                myChart = myDeserializedClass.chart;

                myResult = myChart.result[0];

                myMeta = myResult.meta;

                myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                myQuote = myIndicators.quote[0];

                ////this will be typically only 1 row and adjClose will have list of adjClose
                //Adjclose myAdjClose = null;
                //myAdjClose = myIndicators.adjclose[0];

                //DateTime myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp.Last()).ToLocalTime();
                myDate = stockManager.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);

                indexString.Append(string.Format("SENSEX@{0:HH:mm}: ", myDate));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last()));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last() - myMeta.chartPreviousClose));
                indexString.Append(string.Format("{0:0.00}% ", (myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100));
            }

            myDeserializedClass = stockManager.getIndexIntraDayAlternate("^NSEI", time_interval: "1min", outputsize: "compact");

            if (myDeserializedClass != null)
            {
                myChart = myDeserializedClass.chart;

                myResult = myChart.result[0];

                myMeta = myResult.meta;

                myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                myQuote = myIndicators.quote[0];

                //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp.Last()).ToLocalTime();
                //myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);
                myDate = stockManager.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);

                indexString.Append(string.Format("| NIFTY@{0:HH:mm}: ", myDate));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last()));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last() - myMeta.chartPreviousClose));
                indexString.Append(string.Format("{0:0.00}%", (myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100));
            }
            headingtext.Text = indexString.ToString();
            //headingtext.CssClass = headingtext.CssClass.Replace("blinking blinkingText", "");
            headingtext.CssClass = headingtext.CssClass.Replace("fade", "");
        }
        protected void ClearHeading(object sender, EventArgs e)
        {
            headingtext.CssClass = "fade";
            //headingtext.Text = "";
        }
    }
}