using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class downloaddata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EmailId"] != null)
            //{
            //    Master.UserID = Session["EmailId"].ToString();
            //}

            //if (Session["PortfolioName"] != null)
            //{
            //    Master.Portfolio = Session["PortfolioName"].ToString();
            //}
            if (Session["EmailId"] == null)
            {
                Response.Redirect(".\\Default.aspx");
            }
        }

        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextBoxSearch.Text.Length > 0)
            {
                DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text);

                if (resultTable != null)
                {
                    DropDownListStock.DataTextField = "Name";
                    DropDownListStock.DataValueField = "Symbol";
                    DropDownListStock.DataSource = resultTable;
                    DropDownListStock.DataBind();
                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                }
                else
                {
                    Response.Write("<script language=javascript>alert('No matching symbols found')</script>");
                }

            }
            else
            {
                Response.Write("<script language=javascript>alert('Enter text in Search Stock to search stock symbol')</script>");
            }

        }

        protected void buttonDownloadAll_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            string scriptName;
            bool bIsTestOn = true;
            bool bSaveData = true;

            if (Session["IsTestOn"] != null)
            {
                bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
            }

            if (DropDownListStock.SelectedIndex > 0)
            {
                if (bIsTestOn == false)
                {
                    if (Session["TestDataFolder"] != null)
                    {
                        folderPath = Session["TestDataFolder"].ToString();
                    }
                    scriptName = DropDownListStock.SelectedValue;

                    textboxMessage.Text = "Download in progress....\n";
                    if (downloadGetQuote(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Get Quote: Successful\n";
                    else
                        textboxMessage.Text += "Get Quote: Could not retrieve data from server\n";

                    if (downloadDaily(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Daily: Successful\n";
                    else
                        textboxMessage.Text += "Daily: Could not retrieve data from server\n";

                    if (downloadIntraday(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Intra-day: Successful\n";
                    else
                        textboxMessage.Text += "Intra-day: Could not retrieve data from server\n";

                    if (downloadSMA(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "SMA: Successful\n";
                    else
                        textboxMessage.Text += "SMA: Could not retrieve data from server\n";

                    if (downloadEMA(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "EMA: Successful\n";
                    else
                        textboxMessage.Text += "EMA: Could not retrieve data from server\n";

                    if (downloadVWAP(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "VWAP: Successful\n";
                    else
                        textboxMessage.Text += "VWAP: Could not retrieve data from server\n";

                    if (downloadRSI(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "RSI: Successful\n";
                    else
                        textboxMessage.Text += "RSI: Could not retrieve data from server\n";

                    if (downloadSTOCH(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "STOCH: Successful\n";
                    else
                        textboxMessage.Text += "STOCH: Could not retrieve data from server\n";

                    if (downloadMACD(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "MACD: Successful\n";
                    else
                        textboxMessage.Text += "MACD: Could not retrieve data from server\n";

                    if (downloadAroon(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "AROON: Successful\n";
                    else
                        textboxMessage.Text += "AROON: Could not retrieve data from server\n";

                    if (downloadAdx(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "ADX: Successful\n";
                    else
                        textboxMessage.Text += "ADX: Could not retrieve data from server\n";

                    if (downloadBBands(folderPath, scriptName, bIsTestOn, bSaveData))
                        textboxMessage.Text += "Bollinger Bands: Successful\n";
                    else
                        textboxMessage.Text += "Bollinger Bands: Could not retrieve data from server\n";
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Please un-check the \'Is Test Mode\' option during login to download script data.')</script>");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('Please search & select stock before download operation.')</script>");
            }
        }

        protected void buttonDownloadSelected_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/scriptdata/");
            string scriptName;
            bool bIsTestOn = true;
            bool bSaveData = true;

            if (Session["IsTestOn"] != null)
            {
                bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
            }

            if (DropDownListStock.SelectedIndex > 0)
            {
                if (bIsTestOn == false)
                {
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
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Please un-check the \'Is Test Mode\' option during login to download script data.')</script>");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('Please search & select stock before download operation.')</script>");
            }
        }

        public bool downloadGetQuote(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            if (StockApi.globalQuote(folderPath, scriptName, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('Quote data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadDaily(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string outputsize = ddlDaily_OutputSize.SelectedValue;

            if (StockApi.getDaily(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('Daily data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadIntraday(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlIntraday_Interval.SelectedValue;
            string outputsize = ddlIntraday_outputsize.SelectedValue;

            if (StockApi.getIntraday(folderPath, scriptName, time_interval: interval, outputsize: outputsize, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('Intraday data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadSMA(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlSMA_Interval.SelectedValue;
            string period = textboxSMA_Period.Text;
            string series = ddlSMA_Series.SelectedValue;

            if (StockApi.getSMA(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('SMA data not available for selected script.')</script>");
                return false;
            }
            return true;
        }


        public bool downloadEMA(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlEMA_Interval.SelectedValue;
            string period = textboxEMA_Period.Text;
            string series = ddlEMA_Series.SelectedValue;

            if (StockApi.getEMA(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('EMA data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadVWAP(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlVWAP_Interval.SelectedValue;

            if (StockApi.getVWAP(folderPath, scriptName, day_interval: interval, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('VWAP data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadRSI(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlRSI_Interval.SelectedValue;
            string period = textboxRSI_Period.Text;
            string series = ddlRSI_Series.SelectedValue;

            if (StockApi.getRSI(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('RSI data not available for selected script.')</script>");
                return false;
            }
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

            if (StockApi.getSTOCH(folderPath, scriptName, day_interval: interval, fastkperiod: Fastkperiod, slowkperiod: Slowkperiod, slowdperiod: Slowdperiod, slowkmatype: Slowkmatype, slowdmatype: Slowdmatype, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('STOCH data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadMACD(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlMACD_Interval.SelectedValue;
            string series = ddlMACD_Series.SelectedValue;
            string FastPeriod = textboxMACD_FastPeriod.Text;
            string Slowperiod = textboxMACD_SlowPeriod.Text;
            string SignalPeriod = textboxMACD_SignalPeriod.Text;

            if (StockApi.getMACD(folderPath, scriptName, day_interval: interval, seriestype: series, fastperiod: FastPeriod, slowperiod: Slowperiod, signalperiod: SignalPeriod, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('MACD data not available for selected script.')</script>");
                return false;
            }
            return true;
        }
        public bool downloadAroon(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlAroon_Interval.SelectedValue;
            string period = textboxAroon_Period.Text;
            if (StockApi.getAROON(folderPath, scriptName, day_interval: interval, period: period, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('AROON data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

        public bool downloadAdx(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlAdx_Interval.SelectedValue;
            string period = textboxAdx_Period.Text;
            if (StockApi.getADX(folderPath, scriptName, day_interval: interval, period: period, bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('ADX data not available for selected script.')</script>");
                return false;
            }
            return true;

        }
        public bool downloadBBands(string folderPath, string scriptName, bool bIsTestOn, bool bSaveData)
        {
            string interval = ddlBBands_Interval.SelectedValue;
            string period = textboxBBands_Period.Text;
            string series = ddlBBands_Series.SelectedValue;
            string nbdevUp = textboxBBands_NbdevUp.Text;
            string nbdevDn = textboxBBands_NbdevDn.Text;

            if (StockApi.getBbands(folderPath, scriptName, day_interval: interval, period: period, seriestype: series, nbdevup: nbdevUp, nbdevdn: nbdevDn,
                bIsTestModeOn: bIsTestOn, bSaveData: bSaveData) == null)
            {
                Response.Write("<script language=javascript>alert('Bollinger Bands data not available for selected script.')</script>");
                return false;
            }
            return true;
        }

    }
}