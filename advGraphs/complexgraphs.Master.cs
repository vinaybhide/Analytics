using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class complexgraphs : System.Web.UI.MasterPage
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
        public BulletedList bulletedlistDesc
        {
            get
            {
                // Return the textbox on the master page
                return this.bulletedlistDescM;
            }
        }
        public CheckBoxList checkboxlistLines
        {
            get
            {
                // Return the textbox on the master page
                return this.checkboxlistLinesM;
            }
        }
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

        public delegate void DoEventShowGraph();
        public event DoEventShowGraph OnDoEventShowGraph;

        public delegate void DoEventShowGrid();
        public event DoEventShowGrid OnDoEventShowGrid;

        public delegate void DoEventToggleDesc();
        public event DoEventToggleDesc OnDoEventToggleDesc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetIndexValues(null, null);
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
        protected void GetIndexValues(object sender, EventArgs e)
        {
            //Use myQuote.close.Last() - myMeta.chartPreviousClose to show difference
            //(myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100 to show percentage diff

            Root myDeserializedClass = StockApi.getIndexIntraDayAlternate("^BSESN", time_interval: "1min", outputsize: "compact");

            if (myDeserializedClass != null)
            {
                Chart myChart = myDeserializedClass.chart;

                Result myResult = myChart.result[0];

                Meta myMeta = myResult.meta;

                Indicators myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                Quote myQuote = myIndicators.quote[0];

                ////this will be typically only 1 row and adjClose will have list of adjClose
                //Adjclose myAdjClose = null;
                //myAdjClose = myIndicators.adjclose[0];

                //DateTime myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp.Last()).ToLocalTime();
                DateTime myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);

                StringBuilder indexString = new StringBuilder();
                indexString.Append(string.Format("SENSEX@{0:HH:mm}--", myDate));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last()));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last() - myMeta.chartPreviousClose));
                indexString.Append(string.Format("{0:0.00}% ", (myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100));

                myDeserializedClass = StockApi.getIndexIntraDayAlternate("^NSEI", time_interval: "1min", outputsize: "compact");

                myChart = myDeserializedClass.chart;

                myResult = myChart.result[0];

                myMeta = myResult.meta;

                myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                myQuote = myIndicators.quote[0];

                //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp.Last()).ToLocalTime();
                myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp.Last(), myMeta.timezone);

                indexString.Append(string.Format("| NIFTY@{0:HH:mm}--", myDate));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last()));
                indexString.Append(string.Format("{0:0.00}|", myQuote.close.Last() - myMeta.chartPreviousClose));
                indexString.Append(string.Format("{0:0.00}%", (myQuote.close.Last() - myMeta.chartPreviousClose) / myQuote.close.Last() * 100));

                headingtext.Text = indexString.ToString();
                headingtext.CssClass = headingtext.CssClass.Replace("blinking blinkingText", "");
            }
        }
    }
}