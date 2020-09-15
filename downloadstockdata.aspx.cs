using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class downloadstockdata : System.Web.UI.Page
    {
        public string defaultOutputsize = "full";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                textboxMessage.Text = "";
                if (Session["EmailId"] == null)
                {
                    textboxMessage.Text = common.noLogin;
                    textboxMessage.BackColor = System.Drawing.Color.Red;
                    ButtonSearch.Enabled = false;
                    ButtonSearchPortfolio.Enabled = false;
                    buttonDownloadAll.Enabled = false;
                    buttonDownloadSelected.Enabled = false;
                    //Response.Redirect("~/Default.aspx");
                    //common.ShowMessageBox(this.Page, common.noLogin);
                    //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin+ "');", true);

                    //Response.Flush();
                    //Response.Redirect("~/Default.aspx");
                    Server.Transfer("~/Default.aspx",false);

                }
                else
                {
                    //if (Session["IsTestOn"] != null)
                    //{
                        //bool bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                        //if (bIsTestOn)
                        //{
                        //    textboxMessage.Text = common.testFlagTrue;
                        //    textboxMessage.BackColor = System.Drawing.Color.Red;
                        //    ButtonSearch.Enabled = false;
                        //    ButtonSearchPortfolio.Enabled = false;
                        //    buttonDownloadAll.Enabled = false;
                        //    buttonDownloadSelected.Enabled = false;
                        //    //Response.Redirect("~/Default.aspx");
                        //}
                        //else
                        //{
                            if (Session["PortfolioFolder"] != null)
                            {
                                string folder = Session["PortfolioFolder"].ToString();
                                string[] filelist = Directory.GetFiles(folder, "*.xml");

                                //int lstwidth = 0;
                                if (filelist.Length > 0)
                                {
                                    ddlPortfolios.Items.Clear();
                                    ListItem li = new ListItem("Select Portfolio", "-1");
                                    ddlPortfolios.Items.Insert(0, li);

                                    foreach (string filename in filelist)
                                    {
                                        string portfolioName = filename.Remove(0, filename.LastIndexOf('\\') + 1);
                                        ListItem filenameItem = new ListItem(portfolioName, filename);
                                        ddlPortfolios.Items.Add(filenameItem);
                                    }
                                }
                                else
                                {
                                    ButtonSearchPortfolio.Enabled = false;
                                    ddlPortfolios.Enabled = false;
                                }
                            }
                        //}

                    //}
                }
            }
        }

        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelSelectedSymbol.Text = "Selected stock: " + DropDownListStock.SelectedValue;
            textboxMessage.Text = "";
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextBoxSearch.Text.Length > 0)
            {
                //DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());
                DataTable resultTable = StockApi.symbolSearchAltername(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());
                if (resultTable != null)
                {
                    DropDownListStock.Items.Clear();
                    DropDownListStock.DataSource = null;
                    DropDownListStock.DataTextField = "Name";
                    DropDownListStock.DataValueField = "Symbol";
                    DropDownListStock.DataSource = resultTable;
                    DropDownListStock.DataBind();
                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                    //ddlPortfolios.Items.Clear();
                    labelSelectedSymbol.Text = "Selected stock: ";
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('" + common.noSymbolFound + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noTextSearchSymbol + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTextSearchSymbol + "');", true);
            }
        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            DropDownListStock.Items.Clear();
            DropDownListStock.DataSource = null;
            ListItem li = new ListItem("Select Stock", "-1");
            DropDownListStock.Items.Insert(0, li);

            //Session["PortfolioName"] = ddlPortfolios.SelectedValue;
            //Session["ShortPortfolioName"] = ddlPortfolios.SelectedItem.Text;

            string[] scriptList = StockApi.getScriptFromPortfolioFile(ddlPortfolios.SelectedValue);
            if (scriptList != null)
            {
                foreach (string script in scriptList)
                {
                    li = new ListItem(script, script);
                    DropDownListStock.Items.Add(li);
                }
                labelSelectedSymbol.Text = "Selected stock: ";
            }

        }

        protected void buttonDownloadAll_Click(object sender, EventArgs e)
        {
            textboxMessage.Text = "";
            string folderPath = Server.MapPath("~/scriptdata/");
            string scriptName;
            //bool bIsTestOn = true;
            bool bIsTestOn = false;
            bool bSaveData = true;

            //In either mode we will always allow download
            //if (Session["IsTestOn"] != null)
            //{
            //    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
            //}

            if (DropDownListStock.SelectedIndex > 0)
            {
                //if (bIsTestOn == false)
                //{
                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }
                    scriptName = DropDownListStock.SelectedValue;

                    textboxMessage.Text = "Download progress...." + Environment.NewLine;
                    if (downloadGetQuote(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Get Quote: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "Get Quote: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadDaily(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Daily: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "Daily: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadIntraday(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Intra-day: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "Intra-day: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadSMA(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "SMA: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "SMA: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadEMA(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "EMA: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "EMA: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadVWAP(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "VWAP: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "VWAP: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadRSI(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "RSI: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "RSI: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadSTOCH(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "STOCH: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "STOCH: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadMACD(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "MACD: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "MACD: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadAroon(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "AROON: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "AROON: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadAdx(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "ADX: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "ADX: Could not retrieve data from server" + Environment.NewLine;

                    if (downloadBBands(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Bollinger Bands: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "Bollinger Bands: Could not retrieve data from server" + Environment.NewLine;
                    if (downloadDX(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "DX: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "DX: Could not retrieve data from server" + Environment.NewLine;
                    if (downloadMINUS_DM(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "MINUS_DM: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "MINUS_DM: Could not retrieve data from server" + Environment.NewLine;
                    if (downloadPLUS_DM(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "PLUS_DM: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "PLUS_DM: Could not retrieve data from server" + Environment.NewLine;
                    if (downloadMINUS_DI(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "MINUS_DI: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "MINUS_DI: Could not retrieve data from server" + Environment.NewLine;
                    if (downloadPLUS_DI(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "PLUS_DI: Successful" + Environment.NewLine;
                    else
                        textboxMessage.Text += "PLUS_DI: Could not retrieve data from server" + Environment.NewLine;
                //}
                //else
                //{
                //    textboxMessage.Text = Environment.NewLine + common.testFlagTrue;
                //}
            }
            else
            {
                textboxMessage.Text = Environment.NewLine + common.noStockSelectedToDownload;
            }
        }

        protected void buttonDownloadSelected_Click(object sender, EventArgs e)
        {
            textboxMessage.Text = "";
            string folderPath = Server.MapPath("~/scriptdata/");
            string scriptName;
            //bool bIsTestOn = true;
            bool bIsTestOn = false;
            bool bSaveData = true;

            //we will allow download data in either mode
            //if (Session["IsTestOn"] != null)
            //{
            //    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
            //}

            if (DropDownListStock.SelectedIndex > 0)
            {
                //if (bIsTestOn == false)
                //{
                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }
                    scriptName = DropDownListStock.SelectedValue;

                    if (checkboxQuote.Checked)
                        downloadGetQuote(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxDaily.Checked)
                        downloadDaily(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxIntraday.Checked)
                        downloadIntraday(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxSMA.Checked)
                        downloadSMA(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxEMA.Checked)
                        downloadEMA(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxVWAP.Checked)
                        downloadVWAP(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxRSI.Checked)
                        downloadRSI(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxSTOCH.Checked)
                        downloadSTOCH(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxMACD.Checked)
                        downloadMACD(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxAroon.Checked)
                        downloadAroon(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxAdx.Checked)
                        downloadAdx(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxBBands.Checked)
                        downloadBBands(folderPath, scriptName, bIsTestOn, bSaveData);
                    if (checkboxDX.Checked)
                        downloadDX(folderPath, scriptName, bIsTestOn, bSaveData);
                    if(checkboxMinusDM.Checked)
                        downloadMINUS_DM(folderPath, scriptName, bIsTestOn, bSaveData);
                    if(checkboxPlusDM.Checked)
                        downloadPLUS_DM(folderPath, scriptName, bIsTestOn, bSaveData);
                    if(checkboxMinusDI.Checked)
                        downloadMINUS_DI(folderPath, scriptName, bIsTestOn, bSaveData);
                    if(checkboxPlusDI.Checked)
                        downloadPLUS_DI(folderPath, scriptName, bIsTestOn, bSaveData);
                //}
                //else
                //{
                  //  textboxMessage.Text = Environment.NewLine + common.testFlagTrue;
                //}
            }
            else
            {
                textboxMessage.Text = Environment.NewLine + common.noStockSelectedToDownload;
            }
        }

        public bool downloadGetQuote(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            //if (StockApi.globalQuote(folderPath, scriptName, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            if (StockApi.globalQuoteAlternate(folderPath, scriptName, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                //Response.Write("<script language=javascript>alert('Quote data not available for selected script.')</script>");
                textboxMessage.Text = Environment.NewLine + "Quote data not available for selected script";
                return false;
            }
            return true;
        }

        public bool downloadDaily(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string outputsize = ddlDaily_OutputSize.SelectedValue;

            //if (StockApi.getDaily(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestOn, 
            //                        bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
                //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                if (StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestOn,
                                    bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
                {
                    textboxMessage.Text = Environment.NewLine + "Daily data not available for selected script.";
                    return false;
                }
            //}
            return true;
        }

        public bool downloadIntraday(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlIntraday_Interval.SelectedValue;
            string outputsize = ddlIntraday_outputsize.SelectedValue;

            //if (StockApi.getIntraday(folderPath, scriptName, time_interval: interval, outputsize: outputsize,
            //                        bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
                //if we failed to get data from alphavantage we will try to get it from yahoo online with test flag = false
                if (StockApi.getIntradayAlternate(folderPath, scriptName, time_interval: interval, outputsize: outputsize,
                                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
                {
                    textboxMessage.Text = Environment.NewLine + "Intraday data not available for selected script.";
                    return false;
                }
            //}
            return true;
        }

        public bool downloadSMA(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlSMA_Interval.SelectedValue;
            string period = textboxSMA_Period.Text;
            string series = ddlSMA_Series.SelectedValue;

            if(StockApi.getSMAAlternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, 
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "SMA data not available for selected script.";
                return false;
            }

            //if (StockApi.getSMA(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "SMA data not available for selected script.";
            //    return false;
            //}
            return true;
        }


        public bool downloadEMA(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlEMA_Interval.SelectedValue;
            string period = textboxEMA_Period.Text;
            string series = ddlEMA_Series.SelectedValue;

            if (StockApi.getEMAalternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, 
                                            bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "EMA data not available for selected script.";
                return false;
            }
            //if (StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "EMA data not available for selected script.";
            //    return false;
            //}
            return true;
        }

        public bool downloadVWAP(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlVWAP_Interval.SelectedValue;

            //if (StockApi.getVWAP(folderPath, scriptName, day_interval: interval, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
                if (StockApi.getVWAPAlternate(folderPath, scriptName, time_interval: interval,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
                {
                    textboxMessage.Text = Environment.NewLine + "VWAP data not available for selected script.";
                    return false;
                }
            //}
            return true;
        }

        public bool downloadRSI(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlRSI_Interval.SelectedValue;
            string period = textboxRSI_Period.Text;
            string series = ddlRSI_Series.SelectedValue;

            if (StockApi.getRSIalternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: series,
                                            bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "RSI data not available for selected script.";
                return false;
            }
            //if (StockApi.getRSI(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "RSI data not available for selected script.";
            //    return false;
            //}
            return true;
        }

        public bool downloadSTOCH(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlSTOCH_Interval.SelectedValue;
            string Fastkperiod = textboxSTOCH_Fastkperiod.Text;
            string Slowkperiod = textboxSTOCH_Slowkperiod.Text;
            string Slowdperiod = textboxSTOCH_Slowdperiod.Text;
            string Slowkmatype = ddlSTOCH_Slowkmatype.SelectedValue;
            string Slowdmatype = ddlSTOCH_Slowdmatype.SelectedValue;

            if (StockApi.getSTOCHAlternate(folderPath, scriptName, day_interval: interval, fastkperiod: Fastkperiod, slowkperiod: Slowkperiod,
                                  slowdperiod: Slowdperiod, slowkmatype: Slowkmatype, slowdmatype: Slowdmatype,
                                  bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "STOCH data not available for selected script.";
                return false;
            }

            //if (StockApi.getSTOCH(folderPath, scriptName, day_interval: interval, fastkperiod: Fastkperiod, slowkperiod: Slowkperiod, 
            //                      slowdperiod: Slowdperiod, slowkmatype: Slowkmatype, slowdmatype: Slowdmatype, 
            //                      bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "STOCH data not available for selected script.";
            //    return false;
            //}
            return true;
        }

        public bool downloadMACD(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlMACD_Interval.SelectedValue;
            string series = ddlMACD_Series.SelectedValue;
            string FastPeriod = textboxMACD_FastPeriod.Text;
            string Slowperiod = textboxMACD_SlowPeriod.Text;
            string SignalPeriod = textboxMACD_SignalPeriod.Text;

            if (StockApi.getMACDAlternate(folderPath, scriptName, day_interval: interval, seriestype: series, fastperiod: FastPeriod,
                                            slowperiod: Slowperiod, signalperiod: SignalPeriod,
                                            bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "MACD data not available for selected script.";
                return false;
            }

            //if (StockApi.getMACD(folderPath, scriptName, day_interval: interval, seriestype: series, fastperiod: FastPeriod, 
            //                    slowperiod: Slowperiod, signalperiod: SignalPeriod, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "MACD data not available for selected script.";
            //    return false;
            //}
            return true;
        }
        public bool downloadAroon(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlAroon_Interval.SelectedValue;
            string period = textboxAroon_Period.Text;

            if (StockApi.getAroonAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "AROON data not available for selected script.";
                return false;
            }

            //if (StockApi.getAROON(folderPath, scriptName, day_interval: interval, period: period, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "AROON data not available for selected script.";
            //    return false;
            //}
            return true;
        }

        public bool downloadAdx(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlAdx_Interval.SelectedValue;
            string period = textboxAdx_Period.Text;

            if (StockApi.getADXAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString(), returnType:"ADX") == null)
            {
                textboxMessage.Text = Environment.NewLine + "ADX data not available for selected script.";
                return false;
            }
            //if (StockApi.getADX(folderPath, scriptName, day_interval: interval, period: period, 
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "ADX data not available for selected script.";
            //    return false;
            //}
            return true;

        }
        public bool downloadBBands(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlBBands_Interval.SelectedValue;
            string period = textboxBBands_Period.Text;
            string series = ddlBBands_Series.SelectedValue;
            string nbdevUp = textboxBBands_NbdevUp.Text;
            string nbdevDn = textboxBBands_NbdevDn.Text;

            if (StockApi.getBbandsAlternate(folderPath, scriptName, day_interval: interval, period: period, seriestype: series,
                                    nbdevup: nbdevUp, nbdevdn: nbdevDn,
                                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            {
                textboxMessage.Text = Environment.NewLine + "Bollinger Bands data not available for selected script.";
                return false;
            }
            //if (StockApi.getBbands(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, 
            //                        nbdevup: nbdevUp, nbdevdn: nbdevDn,
            //                        bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "Bollinger Bands data not available for selected script.";
            //    return false;
            //}
            return true;
        }
        public bool downloadDX(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlDX_Interval.SelectedValue;
            string period = textboxDX_Period.Text;

            if (StockApi.getADXAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString(), returnType:"DX") == null)
            {
                textboxMessage.Text = Environment.NewLine + "DX data not available for selected script.";
                return false;
            }
            //if (StockApi.getDX(folderPath, scriptName, day_interval: interval, period: period,
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "DX data not available for selected script.";
            //    return false;
            //}
            return true;
        }

        public bool downloadMINUS_DM(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlMinusDM_Interval.SelectedValue;
            string period = textboxMinusDM_Period.Text;

            if (StockApi.getADXAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString(), returnType: "MINUS_DM") == null)
            {
                textboxMessage.Text = Environment.NewLine + "MINUS_DM data not available for selected script.";
                return false;
            }
            //if (StockApi.getMinusDM(folderPath, scriptName, day_interval: interval, period: period,
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "MINUS_DM data not available for selected script.";
            //    return false;
            //}
            return true;
        }
        public bool downloadPLUS_DM(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlPlusDM_Interval.SelectedValue;
            string period = textboxPlusDM_Period.Text;

            if (StockApi.getADXAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString(), returnType:"PLUS_DM") == null)
            {
                textboxMessage.Text = Environment.NewLine + "PLUS_DM data not available for selected script.";
                return false;
            }
            //if (StockApi.getPlusDM(folderPath, scriptName, day_interval: interval, period: period,
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "PLUS_DM data not available for selected script.";
            //    return false;
            //}
            return true;
        }
        public bool downloadMINUS_DI(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlMinusDI_Interval.SelectedValue;
            string period = textboxMinusDI_Period.Text;

            if (StockApi.getADXAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString(), returnType:"MINUS_DI") == null)
            {
                textboxMessage.Text = Environment.NewLine + "MINUS_DI data not available for selected script.";
                return false;
            }
            //if (StockApi.getMinusDI(folderPath, scriptName, day_interval: interval, period: period,
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "MINUS_DI data not available for selected script.";
            //    return false;
            //}
            return true;
        }
        public bool downloadPLUS_DI(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlPlusDI_Interval.SelectedValue;
            string period = textboxPlusDI_Period.Text;

            if (StockApi.getADXAlternate(folderPath, scriptName, day_interval: interval, period: period,
                                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString(), returnType:"PLUS_DI") == null)
            {
                textboxMessage.Text = Environment.NewLine + "PLUS_DI data not available for selected script.";
                return false;
            }
            //if (StockApi.getPlusDI(folderPath, scriptName, day_interval: interval, period: period,
            //                    bIsTestModeOn: bIsTestOn, bSaveData: bSaveData, apiKey: Session["ApiKey"].ToString()) == null)
            //{
            //    textboxMessage.Text = Environment.NewLine + "PLUS_DI data not available for selected script.";
            //    return false;
            //}
            return true;
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            if (Session["PortfolioFolder"] != null)
            {
                string folder = Session["PortfolioFolder"].ToString();
                if ((Directory.GetFiles(folder, "*")).Length > 0)
                {
                    //Server.Transfer("~/openportfolio.aspx");
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/selectportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Response.Redirect("~/mselectportfolio.aspx");
                    else
                        //Response.Redirect("~/selectportfolio.aspx");
                        Response.Redirect("~/Default.aspx");
                }
                else
                {
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/newportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/mnewportfolio.aspx");
                    else
                        //Response.Redirect("~/newportfolio.aspx");
                        Response.Redirect("~/Default.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }

    }
}