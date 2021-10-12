using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

//following gets all data for mf=53 from start to to date, if you dont give mf code then it will download data for all MF
//http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=53&frmdt=01-Jul-2020&todt=25-Sep-2020
//http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=53&frmdt=2020-07-01&todt=2020-09-25
//static string urlMFCompCodeHistoryURL = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&frmdt={1}&todt={2}";

namespace Analytics
{
    public static class MFAPI
    {

        public static ListItem[] listFundHouseMaster = new[]
        {
            //Following list is created based on http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?
            // use document.getElementById("ctl00_amfiHomeContent_cmbMutualFund")[0] gives following
                //<option value="0">-- Select Mutual Fund --</option>
            //document.getElementById("ctl00_amfiHomeContent_cmbMutualFund")[0].value gives following
                //"0"
            //ocument.getElementById("ctl00_amfiHomeContent_cmbMutualFund")[1].innerHTML gives following
                //"All"

            //DropDownSubCategories.Items.Clear();
            //DropDownSubCategories.Items.AddRange(Electronics);
            // OR 
            //DropDownSubCategories.Items.Add(new ListItem{ Value = "2", Text = "Home Audio"}); .......

            new ListItem{ Value = "-1" , Text = "--Select Mutual Fund --"},
                //new ListItem{ Value = "all" , Text = "All </ option >

                new ListItem{ Value ="39" , Text = "ABN AMRO Mutual Fund"},

                new ListItem{ Value = "3" , Text = "Aditya Birla Sun Life Mutual Fund"},
                new ListItem{ Value = "50" , Text = "AEGON Mutual Fund"},
                new ListItem{ Value = "1" , Text = "Alliance Capital Mutual Fund"},
                new ListItem{ Value = "53" , Text = "Axis Mutual Fund"},
                new ListItem{ Value = "4" , Text = "Baroda Mutual Fund"},
                new ListItem{ Value = "36" , Text = "Benchmark Mutual Fund"},
                new ListItem{ Value = "59" , Text = "BNP Paribas Mutual Fund"},
                new ListItem{ Value = "46" , Text = "BOI AXA Mutual Fund"},
                new ListItem{ Value = "32" , Text = "Canara Robeco Mutual Fund"},
                new ListItem{ Value = "60" , Text = "Daiwa Mutual Fund"},
                new ListItem{ Value = "31" , Text = "DBS Chola Mutual Fund"},
                new ListItem{ Value = "38" , Text = "Deutsche Mutual Fund"},
                new ListItem{ Value = "6" , Text = "DSP Mutual Fund"},
                new ListItem{ Value = "47" , Text = "Edelweiss Mutual Fund"},
                new ListItem{ Value = "54" , Text = "Essel Mutual Fund"},
                new ListItem{ Value = "40" , Text = "Fidelity Mutual Fund"},
                new ListItem{ Value = "51" , Text = "Fortis Mutual Fund"},
                new ListItem{ Value = "27" , Text = "Franklin Templeton Mutual Fund"},
                new ListItem{ Value = "8" , Text = "GIC Mutual Fund"},
                new ListItem{ Value = "49" , Text = "Goldman Sachs Mutual Fund"},
                new ListItem{ Value = "9" , Text = "HDFC Mutual Fund"},
                new ListItem{ Value = "37" , Text = "HSBC Mutual Fund"},
                new ListItem{ Value = "20" , Text = "ICICI Prudential Mutual Fund"},
                new ListItem{ Value = "57" , Text = "IDBI Mutual Fund"},
                new ListItem{ Value = "48" , Text = "IDFC Mutual Fund"},
                new ListItem{ Value = "68" , Text = "IIFCL Mutual Fund (IDF)"},
                new ListItem{ Value = "62" , Text = "IIFL Mutual Fund"},
                ////new ListItem{ Value = "11" , Text = "IL & amp; F S Mutual Fund"},
                //new ListItem{ Value = "11" , Text = "IL&F S Mutual Fund"},
                ////new ListItem{ Value = "65" , Text = "IL & amp; FS Mutual Fund(IDF)"},
                //new ListItem{ Value = "65" , Text = "IL&FS Mutual Fund(IDF)"},
                new ListItem{ Value = "63" , Text = "Indiabulls Mutual Fund"},
                new ListItem{ Value = "14" , Text = "ING Mutual Fund"},
                new ListItem{ Value = "42" , Text = "Invesco Mutual Fund"},
                new ListItem{ Value = "70" , Text = "ITI Mutual Fund"},
                new ListItem{ Value = "16" , Text = "JM Financial Mutual Fund"},
                    new ListItem{ Value = "43" , Text = "JPMorgan Mutual Fund"},
                new ListItem{ Value = "17" , Text = "Kotak Mahindra Mutual Fund"},
                    //new ListItem{ Value = "56" , Text = "L & amp; T Mutual Fund"},
                    new ListItem{ Value = "56" , Text = "L&T Mutual Fund"},
                new ListItem{ Value = "18" , Text = "LIC Mutual Fund"},
                new ListItem{ Value = "69" , Text = "Mahindra Manulife Mutual Fund"},
                    new ListItem{ Value = "45" , Text = "Mirae Asset Mutual Fund"},
                    new ListItem{ Value = "19" , Text = "Morgan Stanley Mutual Fund"},
                    new ListItem{ Value = "55" , Text = "Motilal Oswal Mutual Fund"},
                    new ListItem{ Value = "21" , Text = "Nippon India Mutual Fund"},
                    new ListItem{ Value = "58" , Text = "PGIM India Mutual Fund"},
                    new ListItem{ Value = "44" , Text = "PineBridge Mutual Fund"},
                new ListItem{ Value = "34" , Text = "PNB Mutual Fund"},
                new ListItem{ Value = "64" , Text = "PPFAS Mutual Fund"},
                new ListItem{ Value = "10" , Text = "Principal Mutual Fund"},
                new ListItem{ Value = "13" , Text = "quant Mutual Fund"},
                new ListItem{ Value = "41" , Text = "Quantum Mutual Fund"},
                new ListItem{ Value = "35" , Text = "Sahara Mutual Fund"},
                new ListItem{ Value = "22" , Text = "SBI Mutual Fund"},
                new ListItem{ Value = "52" , Text = "Shinsei Mutual Fund"},
                new ListItem{ Value = "67" , Text = "Shriram Mutual Fund"},
                new ListItem{ Value = "66" , Text = "SREI Mutual Fund (IDF)"},
                new ListItem{ Value = "2" , Text = "Standard Chartered Mutual Fund"},
                    //new ListItem{ Value = "24" , Text = "SUN F&C Mutual Fund"},
                new ListItem{ Value = "33" , Text = "Sundaram Mutual Fund"},
                new ListItem{ Value = "25" , Text = "Tata Mutual Fund"},
                new ListItem{ Value = "26" , Text = "Taurus Mutual Fund"},
                new ListItem{ Value = "72" , Text = "Trust Mutual Fund"},
                new ListItem{ Value = "61" , Text = "Union Mutual Fund"},
                new ListItem{ Value = "28" , Text = "UTI Mutual Fund"},
                new ListItem{ Value = "71" , Text = "YES Mutual Fund"},
                new ListItem{ Value = "29" , Text = "Zurich India Mutual Fund"},
                };


        static string mfMasterFile = "MF_MASTER_CURRENT_NAV.txt";



        //Following URL will fetch latest NAV for ALL MF
        //webservice_url = "https://www.amfiindia.com/spages/NAVAll.txt?t=27092020012930"; //string.Format(mfCurrentNAVALL_URL);
        //webservice_url = "https://www.amfiindia.com/spages/NAVAll.txt?t=27-09-2020"; //string.Format(mfCurrentNAVALL_URL);
        //static string urlMF_MASTER_CURRENT = "https://www.amfiindia.com/spages/NAVAll.txt?t={0}";
        static string urlMF_MASTER_CURRENT = "https://www.amfiindia.com/spages/NAVAll.txt";

        //Use following URL to get specific date NAV for ALL MF. The format is same as urlMF_MASTER_CURRENT
        //Output is:
        //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt=01-Jan-2020
        static string urlMF_NAV_FOR_DATE = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt={0}";

        //Use following URL to get NAV history between from dt & to dt for specific MF code. 
        //Output is :
        //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=27&frmdt=27-Sep-2020&todt=05-Oct-2020
        static string urlMF_NAV_HISTORY_FROM_TO = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&frmdt={1}&todt={2}";
        static string urlMF_NAV_HISTORY_FROM = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&frmdt={1}";

        //
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt=27-Sep-2020&todt=05-Oct-2020&mf=3&scm=119551
        static string urlSCHEME_NAV_HISTORY_FROM_TO = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&frmdt={1}&todt={2}";
        static string urlSCHEME_NAV_HISTORY_FROM = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&frmdt={1}";

        //public static string readMFList()
        //{
        //    //HTMLAgilityPack
        //    WebClient webClient = new WebClient();
        //    string html = webClient.DownloadString("http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx");
        //    HTMLDocument 
        //}
        public static bool isFileWriteDateEqualsToday(string filename)
        {
            bool breturn = false;
            try
            {
                if (File.Exists(filename))
                {
                    if (filename.Contains("MF_MASTER_CURRENT_NAV.txt"))
                    {
                        DateTime dtFileWriteTime = File.GetLastWriteTime(filename);
                        DateTime dtToday = DateTime.Today;

                        if (dtFileWriteTime.Date == DateTime.Today)
                        {
                            breturn = true;
                        }
                        else
                        {
                            breturn = false;
                        }
                    }
                    else
                    {
                        breturn = true;
                    }
                }
                else
                {
                    breturn = false;
                }
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        //Function to get ALL MF;s and latest available NAV
        //"https://www.amfiindia.com/spages/NAVAll.txt"
        //It will check if file was fetched today, if yes then it will not fetch it from AMFIINDIA.COM, but return DataTable from the existing file
        //Else it will fetch file, save it locally and then return DataTable
        //Format of the MF Master file & return table is as below
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable loadMFMasterWithCurrentNAV(string folderPath)
        {
            DataTable resultDataTable = null;
            StringBuilder returnString = new StringBuilder("MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE");
            string webservice_url;
            Uri url;
            WebResponse wr;
            Stream receiveStream = null;
            StreamReader reader = null;
            string[] fields;
            string record;
            DataRow r;
            string mfType = "", tmp1 = "";
            string mfCompName = "";
            double nav;
            StringBuilder filename = new StringBuilder(folderPath + mfMasterFile); // "MF_MASTER_CURRENT_NAV.txt");
            try
            {
                if (isFileWriteDateEqualsToday(filename.ToString()) == false)
                {
                    //webservice_url = string.Format(mfCurrentNAVALL_URL);
                    //using (var webClient = new System.Net.WebClient())
                    //{
                    //    record = webClient.DownloadString("https://www.amfiindia.com/spages/NAVAll.txt?t=27092020012930");
                    //    webClient.Dispose();
                    //}

                    //https://www.amfiindia.com/spages/NAVAll.txt;


                    //webservice_url = string.Format(urlMF_MASTER_CURRENT, DateTime.Today.Date.ToShortDateString());
                    webservice_url = urlMF_MASTER_CURRENT;
                    url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = WebRequestMethods.File.DownloadFile;
                    //webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (reader != null)
                    {
                        //get first line where fields are mentioned
                        record = reader.ReadLine();
                        fields = record.Split(';');
                        resultDataTable = new DataTable();
                        resultDataTable.Columns.Add("MF_TYPE", typeof(string));
                        resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        resultDataTable.Columns.Add("SCHEME_CODE", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Payout_ISIN_Growth", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Reinvestment", typeof(string));
                        resultDataTable.Columns.Add("SCHEME_NAME", typeof(string));
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                        resultDataTable.Columns.Add("DATE", typeof(DateTime));

                        //Now we have table with following fields
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date

                        //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();

                            record = record.Trim();

                            if (record.Length == 0)
                            {
                                continue;
                            }
                            else if (record.Contains(";") == false) //case of either MF type or MF House
                            {
                                tmp1 = record;
                                //lets read next few lines till we find a line with either ; or no ;
                                //if we find a line with ; then it's continuation of same MF Type but
                                while (!reader.EndOfStream)
                                {
                                    record = reader.ReadLine();
                                    record = record.Trim();

                                    if (record.Length == 0)
                                    {
                                        continue;
                                    }
                                    else if (record.Contains(";") == false)
                                    {
                                        //we found a MF company name
                                        mfType = tmp1;
                                        mfCompName = record;
                                        tmp1 = record;
                                    }
                                    else if (record.Contains(";") == true)
                                    {
                                        //we continue with same MF type
                                        mfCompName = tmp1;
                                        break;
                                    }
                                }
                            }

                            fields = record.Split(';');

                            //Check if we have values for - Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                            if (fields.Length == 6)
                            {
                                try
                                {
                                    nav = System.Convert.ToDouble(fields[4]);
                                }
                                catch (Exception)
                                {

                                    nav = 0.00;
                                }

                                //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                                returnString.AppendLine();
                                returnString.Append(mfType);
                                returnString.Append(";");
                                returnString.Append(mfCompName);
                                returnString.Append(";");
                                returnString.Append(fields[0]);
                                returnString.Append(";");
                                returnString.Append(fields[1]);
                                returnString.Append(";");
                                returnString.Append(fields[2]);
                                returnString.Append(";");
                                returnString.Append(fields[3]);
                                returnString.Append(";");

                                returnString.Append(string.Format("{0:0.0000}", nav));
                                //returnString.Append(fields[4]);

                                returnString.Append(";");
                                returnString.Append(System.Convert.ToDateTime(fields[5]).ToString("yyyy-MM-dd"));
                                resultDataTable.Rows.Add(new object[] {
                                                                    mfType,
                                                                    mfCompName,
                                                                    fields[0],
                                                                    fields[1],
                                                                    fields[2],
                                                                    fields[3],
                                                                    System.Convert.ToDouble(string.Format("{0:0.0000}", nav)),
                                                                    //fields[4],
                                                                    System.Convert.ToDateTime(fields[5]).ToString("yyyy-MM-dd")
                                                                });
                            }
                        }
                        File.WriteAllText(filename.ToString(), returnString.ToString());
                    }
                }
                else
                {
                    if (File.Exists(filename.ToString()))
                        reader = new StreamReader(filename.ToString());
                    if (reader != null)
                    {
                        record = reader.ReadLine();

                        fields = record.Split(';');
                        resultDataTable = new DataTable();
                        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                        //foreach (string fieldname in fields)
                        //{
                        //    resultDataTable.Columns.Add(fieldname, typeof(string));
                        //}
                        resultDataTable.Columns.Add("MF_TYPE", typeof(string));
                        resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        resultDataTable.Columns.Add("SCHEME_CODE", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Payout_ISIN_Growth", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Reinvestment", typeof(string));
                        resultDataTable.Columns.Add("SCHEME_NAME", typeof(string));
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                        resultDataTable.Columns.Add("DATE", typeof(DateTime));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            fields = record.Split(';');

                            //r = resultDataTable.NewRow();
                            //r.ItemArray = fields;
                            //resultDataTable.Rows.Add(r);

                            //Fields in file
                            //0MF_TYPE;1MF_COMP_NAME;2SCHEME_CODE;3ISIN_Div_Payout_ISIN_Growth;4ISIN_Div_Reinvestment;5SCHEME_NAME;6NET_ASSET_VALUE;7DATE
                            resultDataTable.Rows.Add(new object[] {
                                                                    fields[0],
                                                                    fields[1],
                                                                    fields[2],
                                                                    fields[3],
                                                                    fields[4],
                                                                    fields[5],
                                                                    System.Convert.ToDouble(string.Format("{0:0.0000}", fields[6])),
                                                                    //fields[6],
                                                                    System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd")
                                                                });

                        }
                    }
                }
                if (reader != null)
                    reader.Close();
                if (receiveStream != null)
                    receiveStream.Close();
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }


        //This method will fetch MF data for specific date with following URL.
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt=01-Jan-2020
        //The output is in different format than NAVALL for the current NAV
        //The out put of this URL is as below
        //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date

        //output of the method is table in following format
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable getMFNAVForDate(string folderPath, string fetchDate)
        {
            DataTable resultDataTable = null;
            StringBuilder returnString = new StringBuilder("MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE");
            string webservice_url;
            Uri url;
            WebResponse wr;
            Stream receiveStream = null;
            StreamReader reader = null;
            string[] fields;
            string record;
            DataRow r;
            string mfType = "", tmp1 = "";
            string mfCompName = "";
            string dateFetch = System.Convert.ToDateTime(fetchDate).ToString("yyyy-MM-dd");

            StringBuilder filename = new StringBuilder(folderPath + dateFetch + ".txt"); // "MF_MASTER_CURRENT_NAV.txt");
            try
            {
                //if (isFileWriteDateEqualsToday(filename.ToString()) == false)
                if (File.Exists(filename.ToString()) == false)
                {
                    //webservice_url = string.Format(urlMF_MASTER_CURRENT, DateTime.Today.Date.ToShortDateString());
                    webservice_url = string.Format(urlMF_NAV_FOR_DATE, dateFetch);
                    url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = WebRequestMethods.File.DownloadFile;
                    //webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (reader != null)
                    {
                        //get first line where fields are mentioned
                        record = reader.ReadLine();
                        fields = record.Split(';');
                        resultDataTable = new DataTable();
                        resultDataTable.Columns.Add("MF_TYPE", typeof(string));
                        resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        resultDataTable.Columns.Add("SCHEME_CODE", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Payout_ISIN_Growth", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Reinvestment", typeof(string));
                        resultDataTable.Columns.Add("SCHEME_NAME", typeof(string));
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                        resultDataTable.Columns.Add("DATE", typeof(DateTime));

                        //Now we have table with following fields
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date

                        //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();

                            record = record.Trim();

                            if (record.Length == 0)
                            {
                                continue;
                            }
                            else if (record.Contains(";") == false) //case of either MF type or MF House
                            {
                                tmp1 = record;
                                //lets read next few lines till we find a line with either ; or no ;
                                //if we find a line with ; then it's continuation of same MF Type but
                                while (!reader.EndOfStream)
                                {
                                    record = reader.ReadLine();
                                    record = record.Trim();

                                    if (record.Length == 0)
                                    {
                                        continue;
                                    }
                                    else if (record.Contains(";") == false)
                                    {
                                        //we found a MF company name
                                        mfType = tmp1;
                                        mfCompName = record;
                                        tmp1 = record;
                                    }
                                    else if (record.Contains(";") == true)
                                    {
                                        //we continue with same MF type
                                        mfCompName = tmp1;
                                        break;
                                    }
                                }
                            }

                            fields = record.Split(';');
                            //Following fields are in a record for this URL
                            //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date

                            //Check if we have values for - Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                            if (fields.Length >= 6)
                            {
                                //Our table is in following format
                                //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                                returnString.AppendLine();
                                returnString.Append(mfType);
                                returnString.Append(";");
                                returnString.Append(mfCompName);
                                returnString.Append(";");
                                returnString.Append(fields[0]); //Scheme Code
                                returnString.Append(";");
                                returnString.Append(fields[2]); //ISIN_Div_Payout_ISIN_Growth
                                returnString.Append(";");
                                returnString.Append(fields[3]); //ISIN_Div_Reinvestment
                                returnString.Append(";");
                                returnString.Append(fields[1]); //SCHEME_NAME
                                returnString.Append(";");

                                returnString.Append(string.Format("{0:0.0000}", System.Convert.ToDouble(fields[4])));
                                //returnString.Append(fields[4]); //NET_ASSET_VALUE

                                returnString.Append(";");
                                returnString.Append(System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd"));  //DATE
                                resultDataTable.Rows.Add(new object[] {
                                                                    mfType,
                                                                    mfCompName,
                                                                    fields[0],
                                                                    fields[2],
                                                                    fields[3],
                                                                    fields[1],
                                                                    string.Format("{0:0.0000}", System.Convert.ToDouble(fields[4])),
                                                                    //fields[4],
                                                                    System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd")
                                                                });
                            }
                        }
                        File.WriteAllText(filename.ToString(), returnString.ToString());
                    }
                }
                else
                {
                    //if (File.Exists(filename.ToString()))
                    reader = new StreamReader(filename.ToString());
                    if (reader != null)
                    {
                        record = reader.ReadLine();

                        fields = record.Split(';');
                        resultDataTable = new DataTable();
                        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                        //foreach (string fieldname in fields)
                        //{
                        //    resultDataTable.Columns.Add(fieldname, typeof(string));
                        //}
                        resultDataTable.Columns.Add("MF_TYPE", typeof(string));
                        resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        resultDataTable.Columns.Add("SCHEME_CODE", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Payout_ISIN_Growth", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Reinvestment", typeof(string));
                        resultDataTable.Columns.Add("SCHEME_NAME", typeof(string));
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                        resultDataTable.Columns.Add("DATE", typeof(DateTime));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            fields = record.Split(';');

                            //r = resultDataTable.NewRow();
                            //r.ItemArray = fields;
                            //resultDataTable.Rows.Add(r);
                            //FIle is in following format
                            //0MF_TYPE;1MF_COMP_NAME;2SCHEME_CODE;3ISIN_Div_Payout_ISIN_Growth;4ISIN_Div_Reinvestment;5SCHEME_NAME;6NET_ASSET_VALUE;7DATE
                            resultDataTable.Rows.Add(new object[] {
                                                                    fields[0],
                                                                    fields[1],
                                                                    fields[2],
                                                                    fields[3],
                                                                    fields[4],
                                                                    fields[5],
                                                                    string.Format("{0:0.0000}", System.Convert.ToDouble(fields[6])),
                                                                    //fields[4],
                                                                    System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd")
                                                                });
                        }
                    }
                }
                if (reader != null)
                    reader.Close();
                if (receiveStream != null)
                    receiveStream.Close();
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }

        //This method will fetch MF NAV history data for specific MF Code between from date = fromDt & To date < to date
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=27&frmdt=2020-09-01&todt=2020-09-04
        //The output is in different format than NAVALL for the current NAV
        //The out put of this URL is as below
        //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date
        //output of the method is table in following format
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable getHistoryNAV(string folderPath, string mfCode, string fromdt, string todt = null)
        {
            DataTable resultDataTable = null;
            StringBuilder returnString = new StringBuilder("MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE");
            string webservice_url;
            Uri url;
            WebResponse wr;
            Stream receiveStream = null;
            StreamReader reader = null;
            string[] fields;
            string record;
            DataRow r;
            string mfType = "", tmp1 = "";
            string mfCompName = "";
            string dateFrom = System.Convert.ToDateTime(fromdt).ToString("yyyy-MM-dd");
            string dateTo = null;

            StringBuilder filename;
            if (todt != null)
            {
                dateTo = System.Convert.ToDateTime(todt).ToString("yyyy-MM-dd");
                filename = new StringBuilder(folderPath + mfCode + "_" + dateFrom + "_" + dateTo + ".txt");
                webservice_url = string.Format(urlMF_NAV_HISTORY_FROM_TO, mfCode, dateFrom, dateTo);
            }
            else
            {
                filename = new StringBuilder(folderPath + mfCode + "_" + dateFrom + ".txt");
                webservice_url = string.Format(urlMF_NAV_HISTORY_FROM, mfCode, dateFrom);
            }
            try
            {
                //if (isFileWriteDateEqualsToday(filename.ToString()) == false)
                if (File.Exists(filename.ToString()) == false)
                {
                    //webservice_url = string.Format(urlMF_NAV_HISTORY, mfCode, dateFrom, dateTo);
                    url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = WebRequestMethods.File.DownloadFile;
                    //webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (reader != null)
                    {
                        //get first line where fields are mentioned
                        record = reader.ReadLine();
                        if (record.Length <= 0)
                        {
                            throw new Exception("No records found.");
                        }
                        fields = record.Split(';');
                        resultDataTable = new DataTable();
                        resultDataTable.Columns.Add("MF_TYPE", typeof(string));
                        resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        resultDataTable.Columns.Add("SCHEME_CODE", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Payout_ISIN_Growth", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Reinvestment", typeof(string));
                        resultDataTable.Columns.Add("SCHEME_NAME", typeof(string));
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                        //resultDataTable.Columns.Add("DATE", typeof(string));
                        resultDataTable.Columns.Add("DATE", typeof(DateTime));

                        //Now we have table with following fields
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date

                        //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();

                            record = record.Trim();

                            if (record.Length == 0)
                            {
                                continue;
                            }
                            else if (record.Contains(";") == false) //case of either MF type or MF House
                            {
                                tmp1 = record;
                                //lets read next few lines till we find a line with either ; or no ;
                                //if we find a line with ; then it's continuation of same MF Type but
                                while (!reader.EndOfStream)
                                {
                                    record = reader.ReadLine();
                                    record = record.Trim();

                                    if (record.Length == 0)
                                    {
                                        continue;
                                    }
                                    else if (record.Contains(";") == false)
                                    {
                                        //we found a MF company name
                                        mfType = tmp1;
                                        mfCompName = record;
                                        tmp1 = record;
                                    }
                                    else if (record.Contains(";") == true)
                                    {
                                        //we continue with same MF type
                                        mfCompName = tmp1;
                                        break;
                                    }
                                }
                            }

                            fields = record.Split(';');
                            //Following fields are in a record for this URL
                            //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date

                            //Check if we have values for - Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                            if (fields.Length >= 6)
                            {
                                //Our table is in following format
                                //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                                returnString.AppendLine();
                                returnString.Append(mfType);
                                returnString.Append(";");
                                returnString.Append(mfCompName);
                                returnString.Append(";");
                                returnString.Append(fields[0]); //Scheme Code
                                returnString.Append(";");
                                returnString.Append(fields[2]); //ISIN_Div_Payout_ISIN_Growth
                                returnString.Append(";");
                                returnString.Append(fields[3]); //ISIN_Div_Reinvestment
                                returnString.Append(";");
                                returnString.Append(fields[1]); //SCHEME_NAME
                                returnString.Append(";");

                                returnString.Append(string.Format("{0:0.0000}", System.Convert.ToDouble(fields[4])));
                                //returnString.Append(fields[4]); //NET_ASSET_VALUE

                                returnString.Append(";");
                                returnString.Append(System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd"));  //DATE
                                resultDataTable.Rows.Add(new object[] {
                                                                    mfType,
                                                                    mfCompName,
                                                                    fields[0],
                                                                    fields[2],
                                                                    fields[3],
                                                                    fields[1],
                                                                    System.Convert.ToDouble(string.Format("{0:0.0000}", System.Convert.ToDouble(fields[4]))),
                                                                    //fields[4],
                                                                    System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd")
                                                                });
                            }
                        }
                        File.WriteAllText(filename.ToString(), returnString.ToString());
                    }
                }
                else
                {
                    //if (File.Exists(filename.ToString()))
                    reader = new StreamReader(filename.ToString());
                    if (reader != null)
                    {
                        record = reader.ReadLine();

                        fields = record.Split(';');
                        resultDataTable = new DataTable();
                        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                        //foreach (string fieldname in fields)
                        //{
                        //    resultDataTable.Columns.Add(fieldname, typeof(string));
                        //}
                        resultDataTable.Columns.Add("MF_TYPE", typeof(string));
                        resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        resultDataTable.Columns.Add("SCHEME_CODE", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Payout_ISIN_Growth", typeof(string));
                        resultDataTable.Columns.Add("ISIN_Div_Reinvestment", typeof(string));
                        resultDataTable.Columns.Add("SCHEME_NAME", typeof(string));
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                        //resultDataTable.Columns.Add("DATE", typeof(string));
                        resultDataTable.Columns.Add("DATE", typeof(DateTime));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            fields = record.Split(';');

                            //r = resultDataTable.NewRow();
                            //r.ItemArray = fields;
                            //resultDataTable.Rows.Add(r);
                            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                            resultDataTable.Rows.Add(new object[] {
                                                                    fields[0],
                                                                    fields[1],
                                                                    fields[2],
                                                                    fields[3],
                                                                    fields[4],
                                                                    fields[5],
                                                                    System.Convert.ToDouble(string.Format("{0:0.0000}", System.Convert.ToDouble(fields[6]))),
                                                                    //fields[4],
                                                                    System.Convert.ToDateTime(fields[7]).ToString("yyyy-MM-dd")
                                                                });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            if (reader != null)
                reader.Close();
            if (receiveStream != null)
                receiveStream.Close();
            return resultDataTable;
        }

        //Function that will search a string in SCHEME_NAME column and return all rows from MF Master table that matches search string
        //Format of the MF Master file & return table is as below
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable searchMFMaster(string folderPath, string searchString = null, bool bExactMatch = false,
                                            DataTable mfMasterTable = null, int retryDays = 0, string searchDate = null)
        {
            DataTable resultDataTable = null;
            DataTable localMFMaster = null;
            int retryCount = 0;
            string localSearchDate;
            try
            {
                if (mfMasterTable == null)
                {
                    //mfMasterTable = loadMFMasterWithCurrentNAV(folderPath);
                    localMFMaster = loadMFMasterWithCurrentNAV(folderPath);
                }
                else
                {
                    localMFMaster = mfMasterTable.Copy();
                }

                if ((searchString == null) || (searchString.Length == 0))
                {
                    resultDataTable = mfMasterTable.Copy();
                }
                else
                {
                    do
                    {
                        if (bExactMatch == false)
                        {
                            localMFMaster.DefaultView.RowFilter = "SCHEME_NAME like '%" + searchString + "%'";
                        }
                        else
                        {
                            localMFMaster.DefaultView.RowFilter = "SCHEME_NAME = '" + searchString + "'";
                        }
                        resultDataTable = localMFMaster.DefaultView.ToTable();
                        if ((resultDataTable != null) && (resultDataTable.Rows.Count > 0))
                        {
                            break;
                        }
                        if (retryDays == 0)
                        {
                            break;
                        }
                        if (searchDate == null)
                        {
                            break;
                        }
                        retryCount++;

                        localSearchDate = System.Convert.ToDateTime(searchDate).AddDays(retryCount).ToShortDateString();

                        localMFMaster = getMFNAVForDate(folderPath, localSearchDate);
                    } while ((retryCount <= retryDays) && (searchDate != null) && (System.Convert.ToDateTime(searchDate) <= DateTime.Today));
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }

        //Function that will search a string in SCHEME_NAME column and return all rows from MF History table that matches search string
        //Format of the MF Master file & return table is as below
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable searchMFHistoryForSchemeName(string folderPath, string mfCode, string fromDate,
                                    string searchString = null, bool bExactMatch = false,
                                    DataTable mfHistoryTable = null, string toDate = null)
        {
            DataTable resultDataTable = null;
            DataTable localMFHistoryTable = null;
            string dateFrom = System.Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            string dateTo = null;

            try
            {
                if (mfHistoryTable == null)
                {
                    //mfMasterTable = loadMFMasterWithCurrentNAV(folderPath);
                    if (toDate != null)
                    {
                        dateTo = System.Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                    }
                    localMFHistoryTable = getHistoryNAV(folderPath, mfCode, dateFrom, todt: dateTo);
                }
                else
                {
                    localMFHistoryTable = mfHistoryTable.Copy();
                }

                if ((searchString == null) || (searchString.Length == 0))
                {
                    resultDataTable = mfHistoryTable.Copy();
                }
                else
                {
                    if (bExactMatch == false)
                    {
                        localMFHistoryTable.DefaultView.RowFilter = "SCHEME_NAME like '%" + searchString + "%'";
                    }
                    else
                    {
                        localMFHistoryTable.DefaultView.RowFilter = "SCHEME_NAME = '" + searchString + "'";
                    }
                    if (localMFHistoryTable.DefaultView.Count > 0)
                    {
                        resultDataTable = localMFHistoryTable.DefaultView.ToTable();
                    }
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }

        //Function that will search a string in MF_COMP_NAME column and return all rows from MF Master table that matches search string
        //Format of the MF Master file & return table is as below
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable getALLMFforFundHouse(string folderPath, string searchString = null, bool bExactMatch = false, DataTable mfMasterTable = null)
        {
            DataTable resultDataTable = null;
            try
            {
                if (mfMasterTable == null)
                {
                    mfMasterTable = loadMFMasterWithCurrentNAV(folderPath);
                }


                if ((searchString == null) || (searchString.Length == 0))
                {
                    resultDataTable = mfMasterTable.Copy();
                }
                else
                {
                    if (bExactMatch == false)
                    {
                        mfMasterTable.DefaultView.RowFilter = "MF_COMP_NAME like '%" + searchString + "%'";
                    }
                    else
                    {
                        mfMasterTable.DefaultView.RowFilter = "MF_COMP_NAME = '" + searchString + "'";
                    }
                    resultDataTable = mfMasterTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }

        //Function that will either return ALL Fund Houses or search a string in MF_COMP_NAME column and return all rows from MF Master table that matches search string
        //Format of the MF Master file & return table is as below
        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        static public DataTable getFundHouses(string folderPath, string searchString = null, bool bExactMatch = false, DataTable mfMasterTable = null)
        {
            DataTable resultDataTable = null;
            try
            {
                if (mfMasterTable == null)
                {
                    mfMasterTable = loadMFMasterWithCurrentNAV(folderPath);
                }

                if ((searchString == null) || (searchString.Length == 0))
                {
                    //Get all unique MF_COMP_NAME/Fund House rows
                    resultDataTable = mfMasterTable.DefaultView.ToTable(true, "MF_COMP_NAME");
                }
                else
                {
                    if (bExactMatch == false)
                    {
                        mfMasterTable.DefaultView.RowFilter = "MF_COMP_NAME like '%" + searchString + "%'";
                    }
                    else
                    {
                        mfMasterTable.DefaultView.RowFilter = "MF_COMP_NAME = '" + searchString + "'";
                    }
                    resultDataTable = mfMasterTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }

        static public DataTable getFundHouseMaster()
        {
            DataTable resultDataTable = null;

            try
            {
                resultDataTable = new DataTable();
                resultDataTable.Columns.Add("MF_CODE", typeof(string));
                resultDataTable.Columns.Add("MF_COMP_NAME", typeof(string));

                foreach (ListItem item in listFundHouseMaster)
                {
                    resultDataTable.Rows.Add(new object[] {
                                                            item.Value,
                                                            item.Text
                                                            });
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            return resultDataTable;
        }

        static public string getMFCodefromFundHouseMaster(string mfCompName)
        {
            string mfCode = null;

            try
            {
                foreach (ListItem item in listFundHouseMaster)
                {
                    if (item.Text.Equals(mfCompName))
                    {
                        mfCode = item.Value;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                mfCode = null;
            }
            return mfCode;
        }

        static public string getMFCompNamefromFundHouseMaster(string mfCodeName)
        {
            string mfCompName = null;

            try
            {
                foreach (ListItem item in listFundHouseMaster)
                {
                    if (item.Value.Equals(mfCodeName))
                    {
                        mfCompName = item.Text;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                mfCompName = null;
            }
            return mfCompName;
        }

        //Creates a new blank portfolio file with first row indicating fields
        //FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
        //filename must be with full path
        static public bool createnewMFPortfolio(string filename)
        {
            bool breturn = false;
            string line = "FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost";
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    writer.WriteLine(line);
                    writer.Close();
                    writer.Dispose();
                }
                breturn = true;
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        static public bool addNewTransaction(string filename, string fundHouse, string mfName, string schemeCode, string purchaseDate,
            string purchaseNAV, string purchaseUnits, string valueAtCost)
        {
            bool breturn = false;
            string lineAdd = fundHouse + ";" + mfName + ";" + schemeCode + ";" + System.Convert.ToDateTime(purchaseDate).ToString("yyyy-MM-dd") +
                ";" + string.Format("{0:0:0000}", purchaseNAV) + ";" + string.Format("{0:0.0000}", purchaseUnits) + ";" + string.Format("{0:0.0000}", valueAtCost);

            try
            {
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    writer.WriteLine(lineAdd);
                    writer.Close();
                    writer.Dispose();
                }
                breturn = true;
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        //static public bool addNewSIPTransactionSet(string folderPath, string filename, string fundHouse, string mfName, string schemeCode,
        //    string startDate, string endDate, string monthlyContribution, string sipFrequency)
        //{
        //    bool breturn = true;
        //    DataTable datewiseData = null;
        //    DateTime fromDt = System.Convert.ToDateTime(startDate);
        //    DateTime toDt = System.Convert.ToDateTime(endDate);
        //    double purchaseUnits;
        //    double valueAtCost = System.Convert.ToDouble(monthlyContribution);
        //    double purchaseNAV;
        //    DateTime workingDt;
        //    string mfCode;
        //    try
        //    {
        //        for (DateTime dt = fromDt; dt < toDt;)
        //        {
        //            workingDt = dt;
        //            while ((workingDt.DayOfWeek == DayOfWeek.Saturday) || (workingDt.DayOfWeek == DayOfWeek.Sunday))
        //            {
        //                workingDt = workingDt.AddDays(1);
        //            }


        //            //Following is the format of the data table
        //            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE

        //            //first get the NAV for specific date
        //            datewiseData = getMFNAVForDate(folderPath, workingDt.ToShortDateString());
        //            if (datewiseData != null)
        //            {
        //                datewiseData = searchMFMaster(folderPath, searchString: mfName, bExactMatch: true, mfMasterTable: datewiseData,
        //                                                retryDays: 5, searchDate: workingDt.ToShortDateString());
        //                if ((datewiseData != null) && (datewiseData.Rows.Count > 0))
        //                {
        //                    try
        //                    {
        //                        purchaseNAV = System.Convert.ToDouble(datewiseData.Rows[0]["NET_ASSET_VALUE"]);
        //                        purchaseUnits = Math.Round((valueAtCost / purchaseNAV), 4);

        //                        //string.Format("{0:0.0000}", fields[4])
        //                        addNewTransaction(filename, fundHouse, mfName, schemeCode,
        //                            System.Convert.ToDateTime(datewiseData.Rows[0]["Date"].ToString()).ToShortDateString(),
        //                            string.Format("{0:0.0000}", purchaseNAV),
        //                            string.Format("{0:0.0000}", purchaseUnits), string.Format("{0:0.0000}", valueAtCost));
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //go for next date
        //                        breturn = false;
        //                    }
        //                }
        //            }

        //            if (sipFrequency == "Daily")
        //            {
        //                dt = dt.AddDays(1);
        //            }
        //            else if (sipFrequency == "Weekly")
        //            {
        //                dt = dt.AddDays(7);
        //            }
        //            else if (sipFrequency == "Monthly")
        //            {
        //                dt = dt.AddMonths(1);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        breturn = false;
        //    }
        //    return breturn;
        //}


        static public DateTime getNextSIPDate(DateTime sourcedt, int dayofmonth, string frequency)
        {
            DateTime returnDt = new DateTime(sourcedt.AddMonths(1).Year, sourcedt.AddMonths(1).Month, 1);

            if((frequency.Equals("Daily")) || (frequency.Equals("Weekly")))
            {
                returnDt = sourcedt.AddDays(dayofmonth);
            }
            else
            {
                returnDt = new DateTime(sourcedt.AddMonths(1).Year, sourcedt.AddMonths(1).Month, 1);
                returnDt = returnDt.AddDays(dayofmonth);
            }

            while ((returnDt.DayOfWeek == DayOfWeek.Saturday) || (returnDt.DayOfWeek == DayOfWeek.Sunday))
            {
                returnDt = returnDt.AddDays(1);
            }
            return returnDt;
        }

        static public bool addNewSIP(string folderPath, string filename, string fundHouse, string mfCode, string mfName, string schemeCode,
            string startDate, string endDate, string monthlyContribution, string sipFrequency = null, string monthday = null, DataTable historyNAVTable = null)
        {
            bool breturn = true;
            DataTable datewiseData = null;
            DateTime fromDt;
            DateTime toDt;
            double purchaseUnits;
            double valueAtCost = System.Convert.ToDouble(monthlyContribution);
            double purchaseNAV;
            //int increment;
            int dayofmonth;
            try
            {
                fromDt = System.Convert.ToDateTime(startDate);
                toDt = System.Convert.ToDateTime(endDate);

                if (historyNAVTable == null)
                {
                    datewiseData = MFAPI.getHistoryNAV(folderPath, mfCode, fromDt.ToString("yyyy-MM-dd"), toDt.ToString("yyyy-MM-dd"));
                }
                else
                {
                    //We were given specific fund's NAV history
                    datewiseData = historyNAVTable.Copy();
                }

                datewiseData.DefaultView.RowFilter = "SCHEME_NAME = '" + mfName + "'";
                datewiseData = datewiseData.DefaultView.ToTable();


                if (sipFrequency == "Monthly")
                {
                    dayofmonth = System.Convert.ToInt32(monthday) - 1;
                }
                else
                {
                    dayofmonth = System.Convert.ToInt32(monthday);
                }

                //dayofmonth = System.Convert.ToInt32(sipFrequency) - 1;

                for (DateTime dt = fromDt; dt <= System.Convert.ToDateTime(datewiseData.Rows[datewiseData.Rows.Count - 1]["Date"]); dt = getNextSIPDate(dt, dayofmonth, sipFrequency) )
                {
                    DateTime transDt = dt;
                    do
                    {
                        datewiseData.DefaultView.RowFilter = "Date = '" + transDt.ToShortDateString() + "'";
                        if (datewiseData.DefaultView.Count > 0)
                        {
                            break;
                        }
                        transDt = transDt.AddDays(1);
                    } while (transDt <= System.Convert.ToDateTime(datewiseData.Rows[datewiseData.Rows.Count - 1]["Date"]));

                    if (datewiseData.DefaultView.Count > 0)
                    {
                        purchaseNAV = System.Convert.ToDouble(datewiseData.DefaultView[0]["NET_ASSET_VALUE"]);

                        //purchaseNAV = System.Convert.ToDouble(datewiseData.Rows[rownum]["NET_ASSET_VALUE"]);
                        purchaseUnits = Math.Round((valueAtCost / purchaseNAV), 4);

                        //string.Format("{0:0.0000}", fields[4])
                        addNewTransaction(filename, fundHouse, mfName, schemeCode,
                            System.Convert.ToDateTime(datewiseData.DefaultView[0]["Date"]).ToString("yyyy-MM-dd"),
                            string.Format("{0:0.0000}", purchaseNAV),
                            string.Format("{0:0.0000}", purchaseUnits), string.Format("{0:0.0000}", valueAtCost));
                    }
                }
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            if (datewiseData != null)
            {
                datewiseData.Clear();
                datewiseData.Dispose();
                datewiseData = null;
            }
            return breturn;
        }

        // Delete line FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
        static public bool deletePortfolioRow(string filename, string fundHouse, string mfName, string schemeCode, string purchaseDate,
            string purchaseNAV, string purchaseUnits, string valueAtCost)
        {
            bool breturn = false;
            try
            {
                string lineDelete = fundHouse + ";" + mfName + ";" + schemeCode + ";" + System.Convert.ToDateTime(purchaseDate).ToString("yyyy-MM-dd") +
                    ";" + string.Format("{0:0.0000}", purchaseNAV) + ";" + string.Format("{0:0.0000}", purchaseUnits) +
                    ";" + string.Format("{0:0.0000}", valueAtCost);
                string line = null;

                using (StreamReader reader = new StreamReader(filename))
                {
                    using (StreamWriter writer = new StreamWriter(filename + ".tmp"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if ((breturn == false) && (String.Compare(line, lineDelete) == 0))
                            {
                                breturn = true;
                                line = reader.ReadToEnd();
                                continue;
                            }

                            writer.WriteLine(line);
                        }
                        writer.Close();
                        writer.Dispose();
                    }
                    reader.Close();
                    reader.Dispose();
                }
                File.Delete(filename);
                System.IO.File.Move(filename + ".tmp", filename);
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        //(Session["PortfolioNameMF"].ToString(), Request.QueryString["fundhouse"].ToString(),
        //                Request.QueryString["fundname"].ToString(), Request.QueryString["schemecode"].ToString(),
        //                Request.QueryString["purchasedate"].ToString(), Request.QueryString["purchasenav"].ToString(), 
        //                Request.QueryString["units"].ToString(), Request.QueryString["valueatcost"].ToString(),
        //                PurchaseDate, PurchaseNAV, PurchaseUnits, ValueAtCost)
        static public bool updateTransaction(string filename, string oldfundHouse, string oldFundName, string schemeCode,
            string oldpurchaseDate, string oldpurchaseNAV, string oldpurchaseUnits, string oldvalueAtCost,
            string fundHouse, string fundName, string purchaseDate, string purchaseNAV, string purchaseUnits, string valueAtCost)
        {
            bool breturn = false;
            try
            {
                string oldTransaction = oldfundHouse + ";" + oldFundName + ";" + schemeCode + ";" +
                    System.Convert.ToDateTime(oldpurchaseDate).ToString("yyyy-MM-dd") + ";" + string.Format("{0:0.0000}", oldpurchaseNAV) +
                    ";" + string.Format("{0:0.0000}", oldpurchaseUnits) + ";" + string.Format("{0:0.0000}", oldvalueAtCost);
                string newTransaction = fundHouse + ";" + fundName + ";" + schemeCode + ";" + System.Convert.ToDateTime(purchaseDate).ToString("yyyy-MM-dd") +
                    ";" + string.Format("{0:0.0000}", purchaseNAV) + ";" + string.Format("{0:0.0000}", purchaseUnits) + ";" + string.Format("{0:0.0000}", valueAtCost);
                string line = null;

                using (StreamReader reader = new StreamReader(filename))
                {
                    using (StreamWriter writer = new StreamWriter(filename + ".tmp"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if ((breturn == false) && (String.Compare(line, oldTransaction) == 0))
                            {
                                breturn = true;
                                writer.WriteLine(newTransaction);
                            }
                            else
                            {
                                writer.WriteLine(line);
                            }
                        }
                        writer.Close();
                        writer.Dispose();
                    }
                    reader.Close();
                    reader.Dispose();
                }
                File.Delete(filename);
                System.IO.File.Move(filename + ".tmp", filename);
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }


        static public DateTime getPastWorkingDate(DateTime sourceDt)
        {
            DateTime pastWorkingDt;

            if(sourceDt == DateTime.Today)
            {
                pastWorkingDt = sourceDt.AddDays(-1);
            }
            else
            {
                pastWorkingDt = sourceDt;
            }
            
            while ((pastWorkingDt.DayOfWeek == DayOfWeek.Saturday) || (pastWorkingDt.DayOfWeek == DayOfWeek.Sunday))
            {
                pastWorkingDt = pastWorkingDt.AddDays(-1);
            }

            return pastWorkingDt;
        }

        /// <summary>
        /// Opens a MF portfolio ; separated text file which is in following format:
        /// FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
        /// </summary>
        /// <param name="folderPath">path of the folder from where to pick MF master data file</param>
        /// <param name="portfolioFileName">Portfolio filename including full path</param>
        /// <param name="bCurrent">true when called by Open Portfolio, false when called by Get Valuation Graph</param>
        /// <returns>DataTable containing portfolio with following columns
        /// FundHouse;FundName;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
        /// </returns>
        static public DataTable openMFPortfolio(string folderPath, string portfolioFileName, bool bCurrent = true, bool bValuation = false)
        {
            DataTable latestNAVTable;
            DataTable resultDataTable = null;
            StreamReader reader = null;
            string[] fields;
            string record;
            StringBuilder filename = new StringBuilder(folderPath + mfMasterFile); // "MF_MASTER_CURRENT_NAV.txt");
            DataTable quoteTable = null;
            double currentNAV, valueAtCost, currentValue, yearsInvested, arr;
            DateTime currentNAVdt, purchaseNAVDt;
            DateTime latestNAVDt = getPastWorkingDate(DateTime.Today);
            try
            {
                if (File.Exists(portfolioFileName))
                    reader = new StreamReader(portfolioFileName);
                if (reader != null)
                {
                    record = reader.ReadLine();

                    fields = record.Split(';');

                    resultDataTable = new DataTable();
                    //FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
                    resultDataTable.Columns.Add(fields[0], typeof(string)); //FundHouse
                    resultDataTable.Columns.Add(fields[1], typeof(string)); //FundName
                    resultDataTable.Columns.Add(fields[2], typeof(string)); //SCHEME_CODE
                    resultDataTable.Columns.Add(fields[3], typeof(DateTime)); //PurchaseDate
                    resultDataTable.Columns.Add(fields[4], typeof(decimal)); //PurchaseNAV
                    resultDataTable.Columns.Add(fields[5], typeof(decimal)); //PurchaseUnits
                    resultDataTable.Columns.Add(fields[6], typeof(decimal)); //ValueAtCost

                    resultDataTable.Columns.Add("CurrentNAV", typeof(decimal));
                    resultDataTable.Columns.Add("NAVDate", typeof(DateTime));
                    resultDataTable.Columns.Add("CurrentValue", typeof(decimal));
                    resultDataTable.Columns.Add("YearsInvested", typeof(decimal));
                    resultDataTable.Columns.Add("ARR", typeof(decimal));

                    latestNAVTable = loadMFMasterWithCurrentNAV(folderPath);

                    while (!reader.EndOfStream)
                    {
                        record = reader.ReadLine();
                        if (record.Length <= 0)
                        {
                            continue;
                        }
                        fields = record.Split(';');

                        currentNAV = 0.00; currentValue = 0.00; yearsInvested = 0.00; arr = 0.00; valueAtCost = 0.00;
                        //currentNAVdt = DateTime.Today;
                        currentNAVdt = DateTime.MinValue;


                        //get current NAV & find other details
                        //Format of the MF Master file & return table is as below
                        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE

                        //Format of the portfolio file
                        //FundHouse;FundName;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
                        //quoteTable = searchMFHistoryForSchemeName(folderPath, getMFCodefromFundHouseMaster(fields[0]), 
                        //    latestNAVDt.ToString("yyyy-MM-dd"), searchString: fields[1],
                        //    bExactMatch: true);

                        quoteTable = searchMFMaster(folderPath, searchString: fields[1], bExactMatch: true, mfMasterTable: latestNAVTable);

                        if ((quoteTable != null) && (quoteTable.Rows.Count > 0))
                        {
                            try
                            {
                                currentNAV = System.Convert.ToDouble(string.Format("{0:0.0000}", quoteTable.Rows[0]["NET_ASSET_VALUE"]));
                                //current value = current NAV * Purchase Units
                                currentValue = Math.Round(currentNAV * System.Convert.ToDouble(string.Format("{0:0.0000}", fields[5])), 4);

                                currentNAVdt = System.Convert.ToDateTime(quoteTable.Rows[0]["DATE"].ToString());
                                purchaseNAVDt = System.Convert.ToDateTime(fields[3].ToString());

                                //find years invested = (current NAV date - Purchase NAV date) / 365.25
                                yearsInvested = Math.Round(((currentNAVdt - purchaseNAVDt).TotalDays) / 365.25, 4);

                                valueAtCost = System.Convert.ToDouble(string.Format("{0:0.0000}", fields[6]));

                                //find ARR = ((current value / value at cost) ^ (1 / years invested)) - 1
                                arr = Math.Round(Math.Pow((currentValue / valueAtCost), (1 / yearsInvested)) - 1, 4);
                            }
                            catch (Exception ex)
                            {
                                currentNAV = 0.00; currentValue = 0.00; yearsInvested = 0.00; arr = 0.00;
                            }
                        }

                        //now save row - FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                        resultDataTable.Rows.Add(new object[] {
                            fields[0],  //FundHouse
                            fields[1],  //FundName
                            fields[2],  //SCHEME_CODE
                            System.Convert.ToDateTime(fields[3]).ToString("yyyy-MM-dd"),  //PurchaseDate
                            string.Format("{0:0.0000}", System.Convert.ToDouble(fields[4])),  //Math.Round(System.Convert.ToDouble(fields[4]), 4),  //PurchaseNAV
                            string.Format("{0:0.0000}", System.Convert.ToDouble(fields[5])),  //Math.Round(System.Convert.ToDouble(fields[5]), 4),  //PurchaseUnits
                            string.Format("{0:0.0000}", System.Convert.ToDouble(fields[6])),  //Math.Round(System.Convert.ToDouble(fields[6]), 4),  //ValueAtCost
                            
                            string.Format("{0:0.0000}", currentNAV),    //Math.Round(currentNAV, 4),    //CurrentNAV
                            currentNAVdt.ToString("yyyy-MM-dd"),           //NAVDate
                            string.Format("{0:0.0000}", currentValue), //Math.Round(currentValue, 4),  //CurrentValue
                            string.Format("{0:0.0000}", yearsInvested),  //Math.Round(yearsInvested, 4), //YearsInvested
                            string.Format("{0:0.0000}", arr)            //Math.Round(arr,4)
                        });
                    }
                    resultDataTable.DefaultView.Sort = "FundHouse, FundName, PurchaseDate";
                    resultDataTable = resultDataTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            if (reader != null)
                reader.Close();

            return resultDataTable;
        }
        static public DataTable _openMFPortfolio(string folderPath, string portfolioFileName, bool bCurrent = true, bool bValuation = false)
        {
            DataTable resultDataTable = null;
            StreamReader reader = null;
            string[] fields;
            string record;
            StringBuilder filename = new StringBuilder(folderPath + mfMasterFile); // "MF_MASTER_CURRENT_NAV.txt");
            DataTable quoteTable = null;
            double currentNAV, valueAtCost, currentValue, yearsInvested, arr;
            DateTime currentNAVdt, purchaseNAVDt;

            try
            {
                if (File.Exists(portfolioFileName))
                    reader = new StreamReader(portfolioFileName);
                if (reader != null)
                {
                    record = reader.ReadLine();

                    fields = record.Split(';');


                    resultDataTable = new DataTable();
                    //FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
                    resultDataTable.Columns.Add(fields[0], typeof(string)); //FundHouse
                    resultDataTable.Columns.Add(fields[1], typeof(string)); //FundName
                    resultDataTable.Columns.Add(fields[2], typeof(string)); //SCHEME_CODE
                    resultDataTable.Columns.Add(fields[3], typeof(DateTime)); //PurchaseDate
                    resultDataTable.Columns.Add(fields[4], typeof(double)); //PurchaseNAV
                    resultDataTable.Columns.Add(fields[5], typeof(double)); //PurchaseUnits
                    resultDataTable.Columns.Add(fields[6], typeof(double)); //ValueAtCost

                    resultDataTable.Columns.Add("CurrentNAV", typeof(double));
                    resultDataTable.Columns.Add("NAVDate", typeof(DateTime));
                    resultDataTable.Columns.Add("CurrentValue", typeof(double));
                    resultDataTable.Columns.Add("YearsInvested", typeof(double));
                    resultDataTable.Columns.Add("ARR", typeof(double));

                    while (!reader.EndOfStream)
                    {
                        record = reader.ReadLine();
                        if (record.Length <= 0)
                        {
                            continue;
                        }
                        fields = record.Split(';');

                        currentNAV = 0.00; currentValue = 0.00; yearsInvested = 0.00; arr = 0.00; valueAtCost = 0.00;
                        //currentNAVdt = DateTime.Today;
                        currentNAVdt = DateTime.MinValue;


                        //get current NAV & find other details
                        //Format of the MF Master file & return table is as below
                        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE

                        //Format of the portfolio file
                        //FundHouse;FundName;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost


                        quoteTable = searchMFMaster(folderPath, fields[1], bExactMatch: true);
                        //quoteTable = searchMFHistoryForSchemeName(folderPath, getMFCodefromFundHouseMaster(fields[0]), DateTime.Today.ToShortDateString(), fields[1],
                        //    bExactMatch: true);

                        if ((quoteTable != null) && (quoteTable.Rows.Count > 0))
                        {
                            try
                            {
                                currentNAV = System.Convert.ToDouble(string.Format("{0:0.0000}", quoteTable.Rows[0]["NET_ASSET_VALUE"]));
                                //current value = current NAV * Purchase Units
                                currentValue = Math.Round(currentNAV * System.Convert.ToDouble(string.Format("{0:0.0000}", fields[5])), 4);

                                currentNAVdt = System.Convert.ToDateTime(quoteTable.Rows[0]["DATE"].ToString());
                                purchaseNAVDt = System.Convert.ToDateTime(fields[3].ToString());

                                //find years invested = (current NAV date - Purchase NAV date) / 365.25
                                yearsInvested = Math.Round(((currentNAVdt - purchaseNAVDt).TotalDays) / 365.25, 4);

                                valueAtCost = System.Convert.ToDouble(string.Format("{0:0.0000}", fields[6]));

                                //find ARR = ((current value / value at cost) ^ (1 / years invested)) - 1
                                arr = Math.Round(Math.Pow((currentValue / valueAtCost), (1 / yearsInvested)) - 1, 4);
                            }
                            catch (Exception ex)
                            {
                                currentNAV = 0.00; currentValue = 0.00; yearsInvested = 0.00; arr = 0.00;
                            }
                        }

                        //now save row - FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                        resultDataTable.Rows.Add(new object[] {
                            fields[0],  //FundHouse
                            fields[1],  //FundName
                            fields[2],  //SCHEME_CODE
                            System.Convert.ToDateTime(fields[3]).ToShortDateString(),  //PurchaseDate
                            string.Format("{0:0.0000}", System.Convert.ToDouble(fields[4])),  //Math.Round(System.Convert.ToDouble(fields[4]), 4),  //PurchaseNAV
                            string.Format("{0:0.0000}", System.Convert.ToDouble(fields[5])),  //Math.Round(System.Convert.ToDouble(fields[5]), 4),  //PurchaseUnits
                            string.Format("{0:0.0000}", System.Convert.ToDouble(fields[6])),  //Math.Round(System.Convert.ToDouble(fields[6]), 4),  //ValueAtCost
                            string.Format("{0:0.0000}", currentNAV),    //Math.Round(currentNAV, 4),    //CurrentNAV
                            currentNAVdt.ToShortDateString(),           //NAVDate
                            string.Format("{0:0.0000}", currentValue), //Math.Round(currentValue, 4),  //CurrentValue
                            string.Format("{0:0.0000}", yearsInvested),  //Math.Round(yearsInvested, 4), //YearsInvested
                            string.Format("{0:0.0000}", arr)            //Math.Round(arr,4)
                        });
                    }

                    resultDataTable.DefaultView.Sort = "FundHouse, FundName, PurchaseDate";
                    resultDataTable = resultDataTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            if (reader != null)
                reader.Close();

            return resultDataTable;
        }

        public static DataTable GetMFValuation(string folderPath, string fileName, DataTable portfolioTable = null)
        {
            DataTable resultDataTable = null;

            List<string> listFundHouse = new List<string>();
            List<string> listFundName = new List<string>();
            List<string> listSchemeCode = new List<string>();
            List<DateTime> listFirstPurchaseDate = new List<DateTime>();
            List<double> listCurrentNAV = new List<double>();
            List<DateTime> listNAVDate = new List<DateTime>();
            List<double> listCumulativeUnits = new List<double>();
            List<double> listCumulativeCost = new List<double>();
            List<double> listCumulativeValue = new List<double>();
            List<double> listTotalYearsInvested = new List<double>();
            List<double> listTotalARR = new List<double>();
            int indexForList;
            try
            {
                if (portfolioTable == null)
                {
                    portfolioTable = openMFPortfolio(folderPath, fileName);
                }
                //var last = tbl.AsEnumerable().Max(r => r.Field<DateTime>(col.ColumnName));
                //var first = tbl.AsEnumerable().Min(r => r.Field<DateTime>(col.ColumnName));

                if ((portfolioTable != null) && (portfolioTable.Rows.Count > 0))
                {
                    resultDataTable = new DataTable();

                    resultDataTable.Columns.Add("FundHouse", typeof(string)); //FundHouse
                    resultDataTable.Columns.Add("FundName", typeof(string)); //FundName
                    resultDataTable.Columns.Add("SCHEME_CODE", typeof(string)); //SCHEME_CODE
                    resultDataTable.Columns.Add("PurchaseDate", typeof(DateTime)); //PurchaseDate

                    resultDataTable.Columns.Add("CurrentNAV", typeof(decimal));
                    resultDataTable.Columns.Add("NAVDate", typeof(DateTime));

                    //for valuation
                    resultDataTable.Columns.Add("CumulativeUnits", typeof(decimal)); //Sum of PurchaseUnits
                    resultDataTable.Columns.Add("CumulativeCost", typeof(decimal)); //Sum ValueAtCost
                    resultDataTable.Columns.Add("CumulativeValue", typeof(decimal)); //Sum Cumulative Value
                    resultDataTable.Columns.Add("TotalYearsInvested", typeof(decimal));   // from date & Today's date duration
                    resultDataTable.Columns.Add("TotalARR", typeof(decimal));             //Annualized Rate of Return for years invested

                    foreach (DataRow transaction in portfolioTable.Rows)
                    {

                        //find the fundname in list
                        indexForList = listFundName.IndexOf(transaction[1].ToString());

                        if (indexForList < 0)
                        {
                            //add this fund name to list. If this is signle transaction for this fund then this will serve as final values
                            listFundName.Add(transaction[1].ToString());

                            //Only insert first time
                            indexForList = listFundName.IndexOf(transaction[1].ToString());

                            listFundHouse.Insert(indexForList, transaction[0].ToString());
                            listFirstPurchaseDate.Insert(indexForList, System.Convert.ToDateTime(transaction[3]));
                            listSchemeCode.Insert(indexForList, transaction[2].ToString());
                            listCurrentNAV.Insert(indexForList, Math.Round(System.Convert.ToDouble(transaction[7]), 4));
                            listNAVDate.Insert(indexForList, System.Convert.ToDateTime(transaction[8]));

                            //insert first time and then update if there are more transactions of the same fundname
                            listCumulativeUnits.Insert(indexForList, Math.Round(System.Convert.ToDouble(transaction[5]), 4));
                            listCumulativeCost.Insert(indexForList, Math.Round(System.Convert.ToDouble(transaction[6]), 4));
                            listCumulativeValue.Insert(indexForList, Math.Round(System.Convert.ToDouble(transaction[9]), 4));
                            listTotalYearsInvested.Insert(indexForList, Math.Round(System.Convert.ToDouble(transaction[10]), 4));
                            listTotalARR.Insert(indexForList, Math.Round(System.Convert.ToDouble(transaction[11]), 4));
                        }
                        else
                        {
                            //if there are multiple transactions for this fund then we need to add totals and find yearsinvested & arr from start to end
                            //add
                            listCumulativeUnits[indexForList] = listCumulativeUnits[indexForList] + Math.Round(System.Convert.ToDouble(transaction[5]), 4);
                            listCumulativeCost[indexForList] = listCumulativeCost[indexForList] + Math.Round(System.Convert.ToDouble(transaction[6]), 4);
                            listCumulativeValue[indexForList] = listCumulativeValue[indexForList] + Math.Round(System.Convert.ToDouble(transaction[9]), 4);
                            if (System.Convert.ToDouble(transaction[7]) > 0) //currentNAV
                            {
                                listTotalYearsInvested[indexForList] = Math.Round(((System.Convert.ToDateTime(transaction[8]) - listFirstPurchaseDate[indexForList]).TotalDays) / 365.25, 2);
                                listTotalARR[indexForList] = Math.Round(Math.Pow((listCumulativeValue[indexForList] / listCumulativeCost[indexForList]), (1 / listTotalYearsInvested[indexForList])) - 1, 2);
                            }
                        }
                    }

                    //now insert the list into table

                    for (int i = 0; i < listFundName.Count; i++)
                    {
                        resultDataTable.Rows.Add(new object[] {
                            listFundHouse[i],  //FundHouse
                            listFundName[i],  //FundName
                            listSchemeCode[i],  //SCHEME_CODE
                            listFirstPurchaseDate[i],  //PurchaseDate
                            listCurrentNAV[i],    //CurrentNAV
                            listNAVDate[i],           //NAVDate
                            listCumulativeUnits[i],
                            listCumulativeCost[i],
                            listCumulativeValue[i],
                            listTotalYearsInvested[i],
                            listTotalARR[i]
                        });
                    }

                    resultDataTable.DefaultView.Sort = "FundHouse, FundName, PurchaseDate";
                    resultDataTable = resultDataTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                if (resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                }
                resultDataTable = null;
            }
            listFundName.Clear();
            listFirstPurchaseDate.Clear();
            listCumulativeUnits.Clear();
            listCumulativeCost.Clear();
            listCumulativeValue.Clear();
            listTotalYearsInvested.Clear();
            listTotalARR.Clear();

            return resultDataTable;
        }

        //Find min max
        //int minAccountLevel = int.MaxValue;
        //int maxAccountLevel = int.MinValue;
        //foreach (DataRow dr in table.Rows)
        //{
        //    int accountLevel = dr.Field<int>("AccountLevel");
        //        minAccountLevel = Math.Min(minAccountLevel, accountLevel);
        //    maxAccountLevel = Math.Max(maxAccountLevel, accountLevel);
        //}
        //There is another way using following
        //following gives min max values
        //var last = tbl.AsEnumerable().Max(r => r.Field<DateTime>(col.ColumnName));
        //following gives min date
        //var first = tbl.AsEnumerable().Min(r => r.Field<DateTime>(col.ColumnName));

        public static DateTime findMinDate(DataTable sourceDataTable, string colToSearch)
        {
            DateTime minDate = DateTime.MaxValue.Date;
            //DateTime maxDate = DateTime.MinValue.Date;
            int compare;
            foreach (DataRow dr in sourceDataTable.Rows)
            {
                DateTime rowDt = dr.Field<DateTime>(colToSearch);
                compare = DateTime.Compare(minDate, rowDt.Date);
                //compare returns < 0 if dt1 earlier than dt2, =0 if dt1 = dt2, > 0 dt1 later than dt2
                if (compare > 0)
                {
                    minDate = rowDt.Date;
                }
            }
            return minDate;
        }

        public static DateTime findMaxDate(DataTable sourceDataTable, string colToSearch)
        {
            //DateTime minDate = DateTime.MaxValue.Date;
            DateTime maxDate = DateTime.MinValue.Date;
            int compare;
            foreach (DataRow dr in sourceDataTable.Rows)
            {
                DateTime rowDt = dr.Field<DateTime>(colToSearch);
                compare = DateTime.Compare(maxDate, rowDt.Date);
                //compare returns < 0 if dt1 earlier than dt2, =0 if dt1 = dt2, > 0 dt1 later than dt2
                if (compare <= 0)
                {
                    maxDate = rowDt.Date;
                }
            }
            return maxDate;
        }

        public static DataTable GetMFValuationLine(string folderPath, string fileName, DataTable portfolioTable = null)
        {
            DataTable fundHouseTable = null, fundNameTable = null, valuationTable = null;
            DataTable matchingFundHouseTable = null;
            DataTable matchingFundNameTable = null;
            DataTable historyTable = null, historyFundNameNAVTable = null;
            double cumulativeQty;
            double cumulativeCost;
            string currentFundHouse, currentFundName;
            DateTime minPurchaseDate, maxPurchaseDate, currentPurchaseDt, nextPurchaseDt;
            string mfCode;
            double historyRowNAV, currentVal;
            DataRow portfolioRow;
            DataRow[] filteredRows;
            try
            {
                if (portfolioTable == null)
                {
                    // FundHouse;FundName;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                    portfolioTable = openMFPortfolio(folderPath, fileName);
                }

                valuationTable = new DataTable();
                //following comes from history table
                valuationTable.Columns.Add("MF_COMP_NAME", typeof(string));
                valuationTable.Columns.Add("SCHEME_CODE", typeof(string));
                valuationTable.Columns.Add("SCHEME_NAME", typeof(string));
                valuationTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                valuationTable.Columns.Add("DATE", typeof(DateTime));
                //following comes from portfolio table
                valuationTable.Columns.Add("PurchaseDate", typeof(DateTime)); //PurchaseDate
                valuationTable.Columns.Add("PurchaseNAV", typeof(decimal)); //PurchaseNAV
                valuationTable.Columns.Add("PurchaseUnits", typeof(decimal)); //PurchaseUnits
                valuationTable.Columns.Add("ValueAtCost", typeof(decimal)); //ValueAtCost
                                                                           //following are calculated
                valuationTable.Columns.Add("CumulativeUnits", typeof(decimal)); //CumulativeUnits
                valuationTable.Columns.Add("CumulativeCost", typeof(decimal)); //CumulativeCost
                valuationTable.Columns.Add("CurrentValue", typeof(decimal));

                //select unique fund house + fund name combination from portfolio table
                fundHouseTable = portfolioTable.DefaultView.ToTable(true, "FundHouse");
                foreach (DataRow fundHouseRow in fundHouseTable.Rows)
                {
                    currentFundHouse = fundHouseRow["FundHouse"].ToString();

                    //get all rows for current fund house from portfolio table
                    portfolioTable.DefaultView.RowFilter = "FundHouse = '" + currentFundHouse + "'";
                    matchingFundHouseTable = portfolioTable.DefaultView.ToTable();

                    //find min & max purchase date
                    minPurchaseDate = MFAPI.findMinDate(matchingFundHouseTable, "PurchaseDate");
                    //maxPurchaseDate = MFAPI.findMaxDate(matchingFundHouseTable, "PurchaseDate");
                    maxPurchaseDate = getPastWorkingDate(DateTime.Today);

                    //now get the history data for the fund
                    mfCode = MFAPI.getMFCodefromFundHouseMaster(currentFundHouse);
                    if (mfCode != null)
                    {
                        historyTable = MFAPI.getHistoryNAV(folderPath, mfCode, minPurchaseDate.ToString("yyyy-MM-dd"), todt: maxPurchaseDate.ToString("yyyy-MM-dd"));

                        //for each fund name within fund house in the matching table calculate & fill the current value
                        fundNameTable = matchingFundHouseTable.DefaultView.ToTable(true, "FundName");
                        foreach (DataRow fundNameRow in fundNameTable.Rows)
                        {
                            currentFundName = fundNameRow["FundName"].ToString();

                            //now find all history NAV for the current Fund Name frm history table
                            historyFundNameNAVTable = MFAPI.searchMFHistoryForSchemeName(folderPath, mfCode, minPurchaseDate.ToString("yyyy-MM-dd"),
                                searchString: currentFundName, bExactMatch: true, mfHistoryTable: historyTable,
                                toDate: maxPurchaseDate.ToString("yyyy-MM-dd"));

                            //if the current fund name is not found in history nav table then go for the next fund. May be the fund is renamed or no longer exists
                            if( (historyFundNameNAVTable == null) || (historyFundNameNAVTable.Rows.Count == 0) )
                            {
                                continue;
                            }

                            //Now we have to get the matching fundname rows from portfolio
                            matchingFundHouseTable.DefaultView.RowFilter = "FundName = '" + currentFundName + "'";
                            matchingFundNameTable = matchingFundHouseTable.DefaultView.ToTable();
                            cumulativeQty = 0;
                            cumulativeCost = 0.0;

                            for (int i = 0; i < matchingFundNameTable.Rows.Count; i++)
                            {
                                portfolioRow = matchingFundNameTable.Rows[i];
                                //FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost

                                cumulativeQty += System.Convert.ToDouble(portfolioRow["PurchaseUnits"]);
                                cumulativeCost += System.Convert.ToDouble(portfolioRow["ValueAtCost"]);
                                currentPurchaseDt = System.Convert.ToDateTime(portfolioRow["PurchaseDate"]);
                                //Now find range of rows from NAV history table
                                //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                                if ((i + 1) == matchingFundNameTable.Rows.Count)
                                {
                                    //historyFundNameNAVTable.DefaultView.RowFilter = "Date >= '" + currentPurchaseDt.ToString("dd-MM-yyyy") + "'";
                                    filteredRows = historyFundNameNAVTable.Select("Date >= '" + currentPurchaseDt.ToString("yyyy-MM-dd") + "'");
                                }
                                else
                                {
                                    nextPurchaseDt = System.Convert.ToDateTime(matchingFundNameTable.Rows[i + 1]["PurchaseDate"]);
                                    //string.Format(CultureInfo.InvariantCulture.DateTimeFormat, "Column >= #{0:d}#", dateTimeValue);
                                    //historyFundNameNAVTable.DefaultView.RowFilter = 
                                    //    string.Format(CultureInfo.InvariantCulture.DateTimeFormat, "DATE >= #{0:dd-MM-yyyy}# AND DATE <=  #{1:dd-MM-yyyy}#", currentPurchaseDt.Date, nextPurchaseDt.Date);

                                    //historyFundNameNAVTable.DefaultView.RowFilter = "Date >= #" + currentPurchaseDt.ToString("dd-MM-yyyy") + "# and Date <= #" + nextPurchaseDt.ToString("dd-MM-yyyy") + "#";
                                    filteredRows = historyFundNameNAVTable.Select("DATE >= '" + currentPurchaseDt.ToString("yyyy-MM-dd") + "' AND DATE <= '" + nextPurchaseDt.ToString("yyyy-MM-dd") + "'");
                                }

                                //for (int j = 0; j < historyFundNameNAVTable.DefaultView.Count; j++)
                                for (int j = 0; j < filteredRows.Length; j++)
                                {
                                    //historyRowNAV = System.Convert.ToDouble(historyFundNameNAVTable.DefaultView[j]["NET_ASSET_VALUE"]);
                                    historyRowNAV = System.Convert.ToDouble(filteredRows[j]["NET_ASSET_VALUE"]);
                                    currentVal = historyRowNAV * cumulativeQty;

                                    //Now insert into valuation table
                                    //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                                    //FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost

                                    valuationTable.Rows.Add(new object[] {
                                                        filteredRows[j]["MF_COMP_NAME"],  //MF_COMP_NAME
                                                        filteredRows[j]["SCHEME_CODE"],  //SCHEME_CODE
                                                        filteredRows[j]["SCHEME_NAME"],  //SCHEME_NAME
                                                        string.Format("{0:0.0000}", historyRowNAV),  //NET_ASSET_VALUE
                                                        System.Convert.ToDateTime(filteredRows[j]["DATE"]).ToString("yyyy-MM-dd"),  //DATE

                                                        currentPurchaseDt.ToString("yyyy-MM-dd"), //PurchaseDate
                                                        string.Format("{0:0.0000}", System.Convert.ToDouble(portfolioRow["PurchaseNAV"])), //PurchaseNAV
                                                        string.Format("{0:0.0000}", System.Convert.ToDouble(portfolioRow["PurchaseUnits"])), //PurchaseUnits
                                                        string.Format("{0:0.0000}", System.Convert.ToDouble(portfolioRow["ValueAtCost"])), //ValueAtCost

                                                        string.Format("{0:0.0000}", cumulativeQty),
                                                        string.Format("{0:0.0000}", cumulativeCost),
                                                        string.Format("{0:0.0000}", currentVal)
                                                        });

                                }
                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                if (valuationTable != null)
                {
                    valuationTable.Clear();
                    valuationTable.Dispose();
                }
                valuationTable = null;
                //throw;
            }

            if (fundHouseTable != null)
            {
                fundHouseTable.Clear();
                fundHouseTable.Dispose();
            }
            if (fundNameTable != null)
            {
                fundNameTable.Clear();
                fundNameTable.Dispose();
            }
            if (matchingFundHouseTable != null)
            {
                matchingFundHouseTable.Clear();
                matchingFundHouseTable.Dispose();
            }
            if (matchingFundNameTable != null)
            {
                matchingFundNameTable.Clear();
                matchingFundNameTable.Dispose();
            }
            if (historyTable != null)
            {
                historyTable.Clear();
                historyTable.Dispose();
            }

            if (historyFundNameNAVTable != null)
            {
                historyFundNameNAVTable.Clear();
                historyFundNameNAVTable.Dispose();
            }
            return valuationTable;
        }
    }
}
