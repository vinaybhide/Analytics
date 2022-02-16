using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccessLayer;

namespace Analytics
{
    public partial class mshowgraph : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIONAME"] = null;

            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["GraphScript"] = null;
                    ViewState["STOCKMASTER"] = null;
                    FillExchangeList();
                    FillInvestmentTypeList();
                    FillSymbolList();
                    LoadPortfolioList();

                    textboxSelectedSymbol.Text = "";
                }
                else
                {
                    if (ViewState["GraphScript"] != null)
                        //labelSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                        textboxSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                //Response.Flush();
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                //Response.Redirect("~/Default.aspx");
                Server.Transfer("~/Default.aspx");
            }
        }
        public void FillExchangeList()
        {
            StockManager stockManager = new StockManager();
            DataTable tableExchange = stockManager.GetExchangeList();
            if ((tableExchange != null) && (tableExchange.Rows.Count > 0))
            {
                ddlExchange.Items.Clear();
                ddlExchange.DataTextField = "EXCHANGE";
                ddlExchange.DataValueField = "EXCHANGE";
                ddlExchange.DataSource = tableExchange;
                ddlExchange.DataBind();

                ListItem li = new ListItem("Filter By Exchange", "-1");
                ddlExchange.Items.Insert(0, li);
                ddlExchange.SelectedIndex = 0;
            }
        }

        public void FillInvestmentTypeList()
        {
            StockManager stockManager = new StockManager();
            DataTable tableInvestmentType = stockManager.GetInvestmentTypeList();
            if ((tableInvestmentType != null) && (tableInvestmentType.Rows.Count > 0))
            {
                ddlInvestmentType.Items.Clear();
                ddlInvestmentType.DataTextField = "SERIES";
                ddlInvestmentType.DataValueField = "SERIES";
                ddlInvestmentType.DataSource = tableInvestmentType;
                ddlInvestmentType.DataBind();
                ListItem li = new ListItem("Filter By Investment Type", "-1");
                ddlInvestmentType.Items.Insert(0, li);
                ddlInvestmentType.SelectedIndex = 0;
            }
        }

        public void FillSymbolList()
        {
            DropDownListStock.Items.Clear();
            textboxSelectedSymbol.Text = "";
            ViewState["GraphScript"] = "";
            ddlExchange.SelectedIndex = 0;
            ddlInvestmentType.SelectedIndex = 0;

            StockManager stockManager = new StockManager();
            DataTable tableStockMaster;

            tableStockMaster = stockManager.getStockMaster();

            if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
            {
                ViewState["STOCKMASTER"] = tableStockMaster;
                DropDownListStock.Items.Clear();
                DropDownListStock.DataTextField = "COMP_NAME";
                DropDownListStock.DataValueField = "SYMBOL";
                DropDownListStock.DataSource = tableStockMaster;
                DropDownListStock.DataBind();
                ListItem li = new ListItem("Select Investment", "-1");
                DropDownListStock.Items.Insert(0, li);
            }
        }

        public void LoadPortfolioList()
        {
            try
            {
                StockManager stockManager = new StockManager();
                DataTable tablePortfolioList = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());
                if ((tablePortfolioList != null) && (tablePortfolioList.Rows.Count > 0))
                {
                    ddlPortfolios.Enabled = true;
                    ButtonSearchPortfolio.Enabled = true;
                    ddlPortfolios.Items.Clear();
                    ListItem li = new ListItem("Filter By Portfolio", "-1");
                    ddlPortfolios.Items.Insert(0, li);
                    foreach (DataRow rowPortfolio in tablePortfolioList.Rows)
                    {
                        li = new ListItem(rowPortfolio["PORTFOLIO_NAME"].ToString(), rowPortfolio["ROWID"].ToString());
                        ddlPortfolios.Items.Add(li);
                    }
                }
                else
                {
                    //ButtonSearchPortfolio.Enabled = false;
                    ddlPortfolios.Enabled = false;
                    ButtonSearchPortfolio.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error: " + ex.Message + "');", true);
            }
        }

        public void LoadPortfolioStockList()
        {
            if (ddlPortfolios.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlExchange.SelectedIndex = 0;
                ddlInvestmentType.SelectedIndex = 0;
                //ViewState["STOCKMASTER"] = null;

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfolios.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    //ViewState["STOCKMASTER"] = symbolTable;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = symbolTable;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Investment", "-1");
                    DropDownListStock.Items.Insert(0, li);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
        }

        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;
                ddlInvestmentType.SelectedIndex = 0;

                if (ViewState["STOCKMASTER"] != null)
                {
                    DataTable stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();
                        filter.Append("EXCHANGE = '" + ddlExchange.SelectedValue + "'");

                        DataView dv = stockMaster.DefaultView;
                        dv.RowFilter = filter.ToString();
                        if (dv.Count > 0)
                        {
                            DropDownListStock.Items.Clear();
                            ListItem li = new ListItem("Select Investment", "-1");
                            DropDownListStock.Items.Add(li);
                            foreach (DataRow rowitem in dv.ToTable().Rows)
                            {
                                li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                                DropDownListStock.Items.Add(li);
                            }
                        }
                    }
                }
            }
        }
        protected void ddlInvestmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInvestmentType.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;
                ddlExchange.SelectedIndex = 0;

                if (ViewState["STOCKMASTER"] != null)
                {
                    DataTable stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();
                        filter.Append("SERIES = '" + ddlInvestmentType.SelectedValue + "'");

                        DataView dv = stockMaster.DefaultView;
                        dv.RowFilter = filter.ToString();
                        if (dv.Count > 0)
                        {
                            DropDownListStock.Items.Clear();
                            ListItem li = new ListItem("Select Investment", "-1");
                            DropDownListStock.Items.Add(li);
                            foreach (DataRow rowitem in dv.ToTable().Rows)
                            {
                                li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                                DropDownListStock.Items.Add(li);
                            }
                        }
                    }
                }
            }
        }
        protected void ButtonGetAllForExchange_Click(object sender, EventArgs e)
        {
            FillExchangeList();
            FillInvestmentTypeList();
            FillSymbolList();
            LoadPortfolioList();
        }

        public bool SearchPopulateStocksDropDown(string searchStr)
        {
            bool breturn = false;
            DataTable stockMaster;
            try
            {
                if (ViewState["STOCKMASTER"] != null)
                {
                    stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();
                        if (!(string.IsNullOrEmpty(TextBoxSearch.Text.ToUpper())))
                            //filter.Append("COMP_NAME Like '%" + searchStr.ToUpper() + "%'");
                            filter.Append("COMP_NAME Like '" + searchStr + "%'");
                        DataView dv = stockMaster.DefaultView;
                        dv.RowFilter = filter.ToString();
                        if (dv.Count > 0)
                        {
                            DropDownListStock.Items.Clear();
                            ListItem li = new ListItem("Select Investment", "-1");
                            DropDownListStock.Items.Add(li);
                            foreach (DataRow rowitem in dv.ToTable().Rows)
                            {
                                li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                                DropDownListStock.Items.Add(li);
                            }
                            breturn = true;
                            ddlExchange.SelectedIndex = 0;
                            ddlInvestmentType.SelectedIndex = 0;
                            if (ddlPortfolios.Items.Count > 0)
                            {
                                ddlPortfolios.SelectedIndex = 0;
                            }
                            textboxSelectedSymbol.Text = "";
                            ViewState["GraphScript"] = "";
                        }
                        else
                        {
                            breturn = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
                breturn = false;
            }
            return breturn;
        }

        /// <summary>
        /// search from the company list. use the filter entered in the search text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            //first try & find the user given string in currently loaded stock drop down
            bool bfound = SearchPopulateStocksDropDown(TextBoxSearch.Text);

            if (bfound == false)
            {
                //if not found in current drop down then try and search online and insert new result in db and then re-load the exchange & stock dropdown
                StockManager stockManager = new StockManager();
                bfound = stockManager.SearchOnlineInsertInDB(TextBoxSearch.Text);
                if (bfound)
                {
                    FillExchangeList();
                    FillInvestmentTypeList();
                    FillSymbolList();

                    bfound = SearchPopulateStocksDropDown(TextBoxSearch.Text);
                }
            }

            if (bfound == false)
            {
                //if we still do not find the user given search string then show error message
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }
        }
        protected void ddlPortfolios_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPortfolioStockList();
        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            LoadPortfolioStockList();
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedIndex > 0)
            {
                ViewState["GraphScript"] = DropDownListStock.SelectedValue;
                //labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
                StockManager stockManager = new StockManager();
                DataTable stockTable = stockManager.SearchStock(DropDownListStock.SelectedValue);
                if ((stockTable != null) && (stockTable.Rows.Count > 0))
                {
                    ddlExchange.SelectedValue = stockTable.Rows[0]["EXCHANGE"].ToString();
                    ddlInvestmentType.SelectedValue = stockTable.Rows[0]["SERIES"].ToString();
                }
                else
                {
                    ddlExchange.SelectedIndex = 0;
                    ddlInvestmentType.SelectedIndex = 0;
                }
            }
            else
            {
                ViewState["GraphScript"] = null;
                //labelSelectedSymbol.Text = "Search & Select script";
                textboxSelectedSymbol.Text = "Search & Select investment";
            }

        }

        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockSelectedAction();
        }

        #region graph button methods
        protected void buttonDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlDaily_OutputSize.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;
            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockdaily.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue +
                    "&outputsize=" + outputSize + "&seriestype=" + "CLOSE" + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonIntraday_Click(object sender, EventArgs e)
        {
            string outputSize = ddlIntraday_outputsize.SelectedValue;
            string interval = ddlIntraday_Interval.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockintra.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                    "&interval=" + ddlIntraday_Interval.SelectedValue + "&seriestype=" + "CLOSE" + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonEMA_Click(object sender, EventArgs e)
        {
            string interval = ddlEMA_Interval.SelectedValue;
            string period = textboxEMA_Period.Text;
            string seriesType = ddlEMA_Series.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockema.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                        "&interval=" + ddlSMA_Interval.SelectedValue + "&seriestype=" + ddlSMA_Series.SelectedValue + "&smallperiod=" + textboxSMA_Period.Text + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonSMA_Click(object sender, EventArgs e)
        {
            string interval = ddlSMA_Interval.SelectedValue;
            string period = textboxSMA_Period.Text;
            string seriesType = ddlSMA_Series.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stocksma.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + ddlSMA_Interval.SelectedValue + "&seriestype=" + ddlSMA_Series.SelectedValue + "&smallperiod=" + textboxSMA_Period.Text + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonAdx_Click(object sender, EventArgs e)
        {
            string interval = ddlAdx_Interval.SelectedValue;
            string period = textboxAdx_Period.Text;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockadx.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                        "&interval=" + ddlRSI_Interval.SelectedValue + "&seriestype=" + ddlRSI_Series.SelectedValue + "&period=" + textboxRSI_Period.Text + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonRSI_Click(object sender, EventArgs e)
        {
            string interval = ddlRSI_Interval.SelectedValue;
            string period = textboxRSI_Period.Text;
            string seriestype = ddlRSI_Series.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockrsi.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + ddlRSI_Interval.SelectedValue + "&seriestype=" + ddlRSI_Series.SelectedValue + "&period=" + textboxRSI_Period.Text + "&type=" + ddlInvestmentType.SelectedValue;

                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonSTOCH_Click(object sender, EventArgs e)
        {
            string interval = ddlSTOCH_Interval.SelectedValue;
            string fastkperiod = textboxSTOCH_Fastkperiod.Text;
            string slowdperiod = textboxSTOCH_Slowdperiod.Text;

            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockstoch.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=Full" +
                    "&interval=" + ddlSTOCH_Interval.SelectedValue + "&seriestype=CLOSE" + "&fastkperiod=" + fastkperiod +
                                    "&slowdperiod=" + slowdperiod + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonMACD_Click(object sender, EventArgs e)
        {
            string interval = ddlMACD_Interval.SelectedValue;
            string seriestype = ddlMACD_Series.SelectedValue;
            string fastperiod = textboxMACD_FastPeriod.Text;
            string slowperiod = textboxMACD_SlowPeriod.Text;
            string signalperiod = textboxMACD_SignalPeriod.Text;

            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockmacd.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=Full" +
                    "&interval=" + interval + "&seriestype=" + seriestype + "&fastperiod=" + fastperiod +
                                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonAroon_Click(object sender, EventArgs e)
        {
            string interval = ddlAroon_Interval.SelectedValue;
            string period = textboxAroon_Period.Text;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockaroon.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + ddlAroon_Interval.SelectedValue + "&seriestype=CLOSE" + "&period=" + textboxAroon_Period.Text + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonBBands_Click(object sender, EventArgs e)
        {
            string interval = ddlBBands_Interval.SelectedValue;
            string period = textboxBBands_Period.Text;
            string seriestype = ddlBBands_Series.SelectedValue;
            string stddev = textboxBBands_StdDev.Text;

            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockbbands.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" + "&interval=" + interval +
                    "&period=" + period + "&seriestype=" + seriestype + "&stddev=" + stddev + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonVWAPrice_Click(object sender, EventArgs e)
        {
            string interval = ddlVWAP_Interval.SelectedValue;
            //string scriptName = labelSelectedSymbol.Text;
            string scriptName = textboxSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/advGraphs/stockvwap.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Compact" +
                    "&interval=" + ddlIntraday_Interval.SelectedValue + "&seriestype=" + "CLOSE" + "&type=" + ddlInvestmentType.SelectedValue;
                url += "&parent=mshowgraph.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }
        #endregion

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/mselectportfolio.aspx");
        }
    }
}