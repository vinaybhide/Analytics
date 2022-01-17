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
                    FillExchangeList();

                    textboxSelectedSymbol.Text = "";
                    StockManager stockManager = new StockManager();

                    DataTable tableStockMaster;
                    if (ddlExchange.SelectedIndex == 0)
                    {
                        tableStockMaster = stockManager.getStockMaster();
                    }
                    else
                    {
                        tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());
                    }

                    if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
                    {
                        ViewState["STOCKMASTER"] = tableStockMaster;
                        DropDownListStock.Items.Clear();
                        DropDownListStock.DataTextField = "COMP_NAME";
                        DropDownListStock.DataValueField = "SYMBOL";
                        DropDownListStock.DataSource = tableStockMaster;
                        DropDownListStock.DataBind();

                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Insert(0, li);
                    }

                    DataTable tablePortfolioList = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());
                    if ((tablePortfolioList != null) && (tablePortfolioList.Rows.Count > 0))
                    {
                        ddlPortfolios.Items.Clear();
                        ListItem li = new ListItem("Select Portfolio", "-1");
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
                    }
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

        protected void ddlExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;

                StockManager stockManager = new StockManager();

                DataTable tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());

                if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
                {
                    ViewState["STOCKMASTER"] = tableStockMaster;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = tableStockMaster;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                }
                else
                {
                    ViewState["STOCKMASTER"] = null;
                    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
        }

        protected void ButtonGetAllForExchange_Click(object sender, EventArgs e)
        {
            if (ddlExchange.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";
                ddlPortfolios.SelectedIndex = 0;

                StockManager stockManager = new StockManager();

                DataTable tableStockMaster = stockManager.getStockMaster(ddlExchange.SelectedValue.ToString());

                if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
                {
                    ViewState["STOCKMASTER"] = tableStockMaster;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = tableStockMaster;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                }
                else
                {
                    ViewState["STOCKMASTER"] = null;
                    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }
            }
        }

        /// <summary>
        /// search from the company list. use the filter entered in the search text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            bool bfound = false;
            DataTable stockMaster = null;
            if (ViewState["STOCKMASTER"] != null)
            {
                stockMaster = (DataTable)ViewState["STOCKMASTER"];
                if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                {
                    StringBuilder filter = new StringBuilder();
                    if (!(string.IsNullOrEmpty(TextBoxSearch.Text.ToUpper())))
                        filter.Append("COMP_NAME Like '%" + TextBoxSearch.Text.ToUpper() + "%'");
                    DataView dv = stockMaster.DefaultView;
                    dv.RowFilter = filter.ToString();
                    if (dv.Count > 0)
                    {
                        DropDownListStock.Items.Clear();
                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Add(li);
                        foreach (DataRow rowitem in dv.ToTable().Rows)
                        {
                            li = new ListItem(rowitem["COMP_NAME"].ToString(), rowitem["SYMBOL"].ToString());//rowitem["EXCHANGE"].ToString());
                            DropDownListStock.Items.Add(li);
                        }
                        bfound = true;
                    }
                    else
                    {
                        bfound = false;
                        //Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                    }

                }
            }
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            //}

            if (bfound == false)
            {
                StockManager stockManager = new StockManager();
                //try to see if this is new stock that we do not have in DB
                DataTable tableSearch = stockManager.SearchStock(TextBoxSearch.Text.ToUpper());
                if ((tableSearch == null) || (tableSearch.Rows.Count <= 0))
                {
                    //try to add new stock
                    tableSearch = stockManager.InsertNewStockIfNotFoundInDB(TextBoxSearch.Text.ToUpper());
                    if ((tableSearch != null) && (tableSearch.Rows.Count > 0))
                    {
                        stockMaster = stockManager.getStockMaster(tableSearch.Rows[0]["EXCHANGE"].ToString());
                        ViewState["STOCKMASTER"] = stockMaster;
                        DropDownListStock.Items.Clear();
                        DropDownListStock.DataTextField = "COMP_NAME";
                        DropDownListStock.DataValueField = "SYMBOL";
                        DropDownListStock.DataSource = stockMaster;
                        DropDownListStock.DataBind();

                        ListItem li = new ListItem("Select Stock", "-1");
                        DropDownListStock.Items.Insert(0, li);
                        DropDownListStock.SelectedValue = TextBoxSearch.Text.ToUpper();
                        FillExchangeList();

                        ddlExchange.SelectedValue = tableSearch.Rows[0]["EXCHANGE"].ToString();
                        StockSelectedAction();
                        bfound = true;
                    }
                }
                else
                {
                    //we found stock
                    stockMaster = stockManager.getStockMaster(tableSearch.Rows[0]["EXCHANGE"].ToString());
                    ViewState["STOCKMASTER"] = stockMaster;
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataTextField = "COMP_NAME";
                    DropDownListStock.DataValueField = "SYMBOL";
                    DropDownListStock.DataSource = stockMaster;
                    DropDownListStock.DataBind();

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                    DropDownListStock.SelectedValue = TextBoxSearch.Text.ToUpper();
                    FillExchangeList();

                    ddlExchange.SelectedValue = tableSearch.Rows[0]["EXCHANGE"].ToString();
                    StockSelectedAction();
                    bfound = true;

                }
            }

            if (bfound == false)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            }

            //replacing below code as yahoo url is not working with NSE code and new DataAccessLayer.StockManager
            //DataTable resultTable = StockApi.symbolSearchAltername(TextBoxSearch.Text.ToUpper(), apiKey: Session["ApiKey"].ToString());
            //if (resultTable != null)
            //{
            //    DropDownListStock.DataTextField = "Name";
            //    DropDownListStock.DataValueField = "Symbol";
            //    DropDownListStock.DataSource = resultTable;
            //    DropDownListStock.DataBind();
            //    ListItem li = new ListItem("Select Stock", "-1");
            //    DropDownListStock.Items.Insert(0, li);
            //    ViewState["GraphScript"] = "Search & Select script";
            //    labelSelectedSymbol.Text = "Search & Select script";
            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
            //}

            //}
            //else
            //{
            //    //Response.Write("<script language=javascript>alert('"+ common.noTextSearchSymbol +"')</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTextSearchSymbol + "');", true);
            //}

        }
        protected void ddlPortfolios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPortfolios.SelectedIndex > 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfolios.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataSource = null;

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);

                    foreach (DataRow rowSymbols in symbolTable.Rows)
                    {
                        li = new ListItem(rowSymbols["COMP_NAME"].ToString(), rowSymbols["SYMBOL"].ToString());
                        DropDownListStock.Items.Add(li);
                    }
                    ViewState["STOCKMASTER"] = symbolTable;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelected + "');", true);
            }
        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            if (ddlPortfolios.SelectedIndex >= 0)
            {
                DropDownListStock.Items.Clear();
                TextBoxSearch.Text = "";
                textboxSelectedSymbol.Text = "";
                ViewState["GraphScript"] = "";

                StockManager stockManager = new StockManager();
                DataTable symbolTable = stockManager.getSymbolListFromPortfolio(ddlPortfolios.SelectedValue);
                if ((symbolTable != null) && (symbolTable.Rows.Count > 0))
                {
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataSource = null;

                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);

                    foreach (DataRow rowSymbols in symbolTable.Rows)
                    {
                        li = new ListItem(rowSymbols["COMP_NAME"].ToString(), rowSymbols["SYMBOL"].ToString());
                        DropDownListStock.Items.Add(li);
                    }
                    ViewState["STOCKMASTER"] = symbolTable;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelected + "');", true);
            }
        }

        public void StockSelectedAction()
        {
            if (DropDownListStock.SelectedValue != "-1")
            {
                ViewState["GraphScript"] = DropDownListStock.SelectedValue;
                //labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
                textboxSelectedSymbol.Text = DropDownListStock.SelectedValue;
            }
            else
            {
                ViewState["GraphScript"] = "Search & Select script";
                //labelSelectedSymbol.Text = "Search & Select script";
                textboxSelectedSymbol.Text = "Search & Select script";
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
                //url = "~/graphs/dailygraph.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize + "&seriestype=" + "CLOSE";
                url = "~/advGraphs/stockdaily.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize + "&seriestype=" + "CLOSE";

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/intraday.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize + 
                //    "&interval=" + ddlIntraday_Interval.SelectedValue + "&seriestype=" + "CLOSE";
                url = "~/advGraphs/stockintra.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + outputSize +
                    "&interval=" + ddlIntraday_Interval.SelectedValue + "&seriestype=" + "CLOSE";

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/ema.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                //    "&interval=" + ddlSMA_Interval.SelectedValue + "&seriestype=" + ddlSMA_Series.SelectedValue + "&smallperiod=" + textboxSMA_Period.Text;
                url = "~/advGraphs/stockema.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                        "&interval=" + ddlSMA_Interval.SelectedValue + "&seriestype=" + ddlSMA_Series.SelectedValue + "&smallperiod=" + textboxSMA_Period.Text;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/sma.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                //    "&interval=" + ddlSMA_Interval.SelectedValue + "&seriestype=" + ddlSMA_Series.SelectedValue + "&smallperiod=" + textboxSMA_Period.Text;
                url = "~/advGraphs/stocksma.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + ddlSMA_Interval.SelectedValue + "&seriestype=" + ddlSMA_Series.SelectedValue + "&smallperiod=" + textboxSMA_Period.Text;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/adx.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                //    "&interval=" + ddlRSI_Interval.SelectedValue + "&seriestype=" + ddlRSI_Series.SelectedValue + "&period=" + textboxRSI_Period.Text;
                url = "~/advGraphs/stockadx.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                        "&interval=" + ddlRSI_Interval.SelectedValue + "&seriestype=" + ddlRSI_Series.SelectedValue + "&period=" + textboxRSI_Period.Text;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/rsi.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                //    "&interval=" + ddlRSI_Interval.SelectedValue + "&seriestype=" + ddlRSI_Series.SelectedValue + "&period=" + textboxRSI_Period.Text;
                url = "~/advGraphs/stockrsi.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + ddlRSI_Interval.SelectedValue + "&seriestype=" + ddlRSI_Series.SelectedValue + "&period=" + textboxRSI_Period.Text;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/stoch.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=Full" +
                //    "&interval=" + ddlSTOCH_Interval.SelectedValue + "&seriestype=CLOSE" + "&fastkperiod=" + fastkperiod + 
                //                    "&slowdperiod=" + slowdperiod;
                url = "~/advGraphs/stockstoch.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=Full" +
                    "&interval=" + ddlSTOCH_Interval.SelectedValue + "&seriestype=CLOSE" + "&fastkperiod=" + fastkperiod +
                                    "&slowdperiod=" + slowdperiod;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/macd.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=Full" +
                //    "&interval=" + interval + "&seriestype=" + seriestype + "&fastperiod=" + fastperiod +
                //                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod;
                url = "~/advGraphs/stockmacd.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=Full" +
                    "&interval=" + interval + "&seriestype=" + seriestype + "&fastperiod=" + fastperiod +
                                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/aroon.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                //    "&interval=" + ddlAroon_Interval.SelectedValue + "&seriestype=CLOSE" + "&period=" + textboxAroon_Period.Text;
                url = "~/advGraphs/stockaroon.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" +
                    "&interval=" + ddlAroon_Interval.SelectedValue + "&seriestype=CLOSE" + "&period=" + textboxAroon_Period.Text;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/bbands.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" + "&interval=" + interval + 
                //    "&period=" + period + "&seriestype=" + seriestype + "&stddev=" + stddev;
                url = "~/advGraphs/stockbbands.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Full" + "&interval=" + interval +
                    "&period=" + period + "&seriestype=" + seriestype + "&stddev=" + stddev;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
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
                //url = "~/graphs/vwaprice.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Compact" +
                //    "&interval=" + ddlIntraday_Interval.SelectedValue + "&seriestype=" + "CLOSE";
                url = "~/advGraphs/stockvwap.aspx" + "?symbol=" + scriptName + "&exchange=" + ddlExchange.SelectedValue + "&outputsize=" + "Compact" +
                    "&interval=" + ddlIntraday_Interval.SelectedValue + "&seriestype=" + "CLOSE";
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }
        #endregion
    }
}