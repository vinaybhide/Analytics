using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetIndexValues(null, null);
                RedrawGraphs(null, null);
            }
            Session["EMAILID"] = null;
            Session["USERROWID"] = null;
            Session["DATAFOLDER"] = UserManager.GetDataFolder();

            //Stock Portfolio related
            Session["STOCKPORTFOLIONAME"] = null;
            Session["STOCKPORTFOLIOMASTERROWID"] = null;
            Session["STOCKPORTFOLIOROWID"] = null;
            Session["STOCKPORTFOLIOEXCHANGE"] = null;
            Session["STOCKPORTFOLIOSCRIPTNAME"] = null;
            Session["STOCKPORTFOLIOSCRIPTID"] = null;
            Session["STOCKPORTFOLIOCOMPNAME"] = null;
            Session["STOCKSELECTEDINDEXPORTFOLIO"] = null;

            //Session["STOCKMASTERROWID"] = null;

            //MF Portfolio related
            Session["MFPORTFOLIONAME"] = null;
            Session["MFPORTFOLIOMASTERROWID"] = null;

            Session["MFPORTFOLIOROWID"] = null;
            Session["MFSELECTEDINDEXPORTFOLIO"] = null;

            Session["MFPORTFOLIOFUNDNAME"] = null;
            Session["MFPORTFOLIOFUNDHOUSECODE"] = null;
            Session["MFPORTFOLIOFUNDHOUSE"] = null;
            Session["MFPORTFOLIOSCHEMECODE"] = null;

            if (this.MasterPageFile.Contains("Site.Master"))
            {
                loginlink.HRef = "mlogin.aspx";
                registerlink.HRef = "mlogin.aspx";
            }
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            {
                loginlink.HRef = "mlogin.aspx";
                registerlink.HRef = "mlogin.aspx";
            }
            else
            {
                loginlink.HRef = "mlogin.aspx";
                registerlink.HRef = "mlogin.aspx";
            }
        }

        /// <summary>
        /// method gets called every 60seconds from timer. First time gets called from Onpageload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        protected void RedrawGraphs(object sender, EventArgs e)
        {
            StockManager stockManager = new StockManager();
            DataTable sensexTable = stockManager.GetStockPriceData("^BSESN", time_interval: "1m", fromDate: DateTime.Today.ToShortDateString());
            if ((sensexTable != null) && (sensexTable.Rows.Count > 0))
            {
                chart1.Series["SENSEX"].ToolTip = "SENSEX" + ": Date:#VALX{g}; Close:#VALY1";
                chart1.Series["SENSEX"].PostBackValue = "SENSEX," + "SENSEX" + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                chart1.Series["SENSEX"].Points.Clear();
                chart1.Series["SENSEX"].Points.DataBindXY(sensexTable.Rows, "TIMESTAMP", sensexTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                chart1.Series["SENSEX"].Points[chart1.Series["SENSEX"].Points.Count - 1].MarkerSize = 5;
                chart1.Series["SENSEX"].Points[chart1.Series["SENSEX"].Points.Count - 1].MarkerStyle = MarkerStyle.Cross;
                chart1.Series["SENSEX"].Points[chart1.Series["SENSEX"].Points.Count - 1].Label = sensexTable.Rows[sensexTable.Rows.Count - 1]["TIMESTAMP"].ToString() + "\n" +
                                                                                                 sensexTable.Rows[sensexTable.Rows.Count - 1]["CLOSE"].ToString();
            }

            DataTable niftyTable = stockManager.GetStockPriceData("^NSEI", time_interval: "1m", fromDate: DateTime.Today.ToShortDateString());

            if ((niftyTable != null) && (niftyTable.Rows.Count > 0))
            {
                chart2.Series["NIFTY"].ToolTip = "NIFTY" + ": Date:#VALX{g}; Close:#VALY1";
                chart2.Series["NIFTY"].PostBackValue = "SENSEX," + "SENSEX" + ",#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4";
                chart2.Series["NIFTY"].Points.Clear();
                chart2.Series["NIFTY"].Points.DataBindXY(niftyTable.Rows, "TIMESTAMP", niftyTable.Rows, "CLOSE,OPEN,HIGH,LOW");
                chart2.Series["NIFTY"].Points[chart2.Series["NIFTY"].Points.Count - 1].MarkerSize = 5;
                chart2.Series["NIFTY"].Points[chart2.Series["NIFTY"].Points.Count - 1].MarkerStyle = MarkerStyle.Cross;
                chart2.Series["NIFTY"].Points[chart2.Series["NIFTY"].Points.Count - 1].Label = niftyTable.Rows[niftyTable.Rows.Count - 1]["TIMESTAMP"].ToString() + "\n" +
                                                                                                 niftyTable.Rows[niftyTable.Rows.Count - 1]["CLOSE"].ToString();
            }
        }
        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    ViewState["counter"] = Int32.Parse(ViewState["counter"].ToString()) + 1;
        //    timertest.Text = ViewState["counter"].ToString();

        //    StockManager stockManager = new StockManager();
        //    stockManager.GetQuote()
        //}

    }
}