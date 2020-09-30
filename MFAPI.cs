using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Analytics
{
    public static class MFAPI
    {
        static string mfMasterFile = "MF_MASTER_CURRENT_NAV.txt";

        //Following URL will fetch latest NAV for ALL MF
        //webservice_url = "https://www.amfiindia.com/spages/NAVAll.txt?t=27092020012930"; //string.Format(mfCurrentNAVALL_URL);
        //webservice_url = "https://www.amfiindia.com/spages/NAVAll.txt?t=27-09-2020"; //string.Format(mfCurrentNAVALL_URL);
        //static string urlMF_MASTER_CURRENT = "https://www.amfiindia.com/spages/NAVAll.txt?t={0}";
        static string urlMF_MASTER_CURRENT = "https://www.amfiindia.com/spages/NAVAll.txt";

        //Use following URL to get specific date NAV for ALL MF. The format is same as urlMF_MASTER_CURRENT
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt=01-Jan-2020
        static string urlMF_NAV_FOR_DATE = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt={0}";

        //following gets all data for mf=53 from start to to date, if you dont give mf code then it will download data for all MF
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=53&frmdt=01-Jul-2020&todt=25-Sep-2020
        //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=53&frmdt=2020-07-01&todt=2020-09-25
        static string urlMFCompCodeHistoryURL = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&frmdt={1}&todt={2}";




        public static bool isFileWriteDateEqualsToday(string filename)
        {
            bool breturn = false;
            try
            {
                if (File.Exists(filename))
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
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(string));
                        resultDataTable.Columns.Add("DATE", typeof(string));

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

                                //returnString.Append(string.Format("{0:0.0000}", fields[4]));
                                returnString.Append(fields[4]);

                                returnString.Append(";");
                                returnString.Append(System.Convert.ToDateTime(fields[5]).ToShortDateString());
                                resultDataTable.Rows.Add(new object[] {
                                                                    mfType,
                                                                    mfCompName,
                                                                    fields[0],
                                                                    fields[1],
                                                                    fields[2],
                                                                    fields[3],
                                                                    //System.Convert.ToDouble(string.Format("{0:0.00}", fields[4])),
                                                                    fields[4],
                                                                    System.Convert.ToDateTime(fields[5]).ToShortDateString()
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
                        foreach (string fieldname in fields)
                        {
                            resultDataTable.Columns.Add(fieldname, typeof(string));
                        }
                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            fields = record.Split(';');

                            r = resultDataTable.NewRow();

                            r.ItemArray = fields;

                            resultDataTable.Rows.Add(r);
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
                        resultDataTable.Columns.Add("NET_ASSET_VALUE", typeof(string));
                        resultDataTable.Columns.Add("DATE", typeof(string));

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

                                //returnString.Append(string.Format("{0:0.0000}", fields[4]));
                                returnString.Append(fields[4]); //NET_ASSET_VALUE

                                returnString.Append(";");
                                returnString.Append(System.Convert.ToDateTime(fields[7]).ToShortDateString());  //DATE
                                resultDataTable.Rows.Add(new object[] {
                                                                    mfType,
                                                                    mfCompName,
                                                                    fields[0],
                                                                    fields[2],
                                                                    fields[3],
                                                                    fields[1],
                                                                    //System.Convert.ToDouble(string.Format("{0:0.00}", fields[4])),
                                                                    fields[4],
                                                                    System.Convert.ToDateTime(fields[7]).ToShortDateString()
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
                        foreach (string fieldname in fields)
                        {
                            resultDataTable.Columns.Add(fieldname, typeof(string));
                        }
                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            fields = record.Split(';');

                            r = resultDataTable.NewRow();

                            r.ItemArray = fields;

                            resultDataTable.Rows.Add(r);
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
                        if((resultDataTable != null) && (resultDataTable.Rows.Count > 0))
                        {
                            break;
                        }
                        if(retryDays == 0)
                        {
                            break;
                        }
                        if(searchDate == null)
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
            string lineAdd = fundHouse + ";" + mfName + ";" + schemeCode + ";" + purchaseDate + ";" + purchaseNAV + ";" + purchaseUnits + ";" + valueAtCost;
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

        static public bool addNewSIPTransactionSet(string folderPath, string filename, string fundHouse, string mfName, string schemeCode,
            string startDate, string endDate, string monthlyContribution, string sipFrequency)
        {
            bool breturn = true;
            DataTable datewiseData = null;
            DateTime fromDt = System.Convert.ToDateTime(startDate);
            DateTime toDt = System.Convert.ToDateTime(endDate);
            double purchaseUnits;
            double valueAtCost = System.Convert.ToDouble(monthlyContribution);
            double purchaseNAV;
            DateTime workingDt;

            try
            {
                for (DateTime dt = fromDt; dt < toDt;)
                {
                    workingDt = dt;
                    while ((workingDt.DayOfWeek == DayOfWeek.Saturday) || (workingDt.DayOfWeek == DayOfWeek.Sunday))
                    {
                        workingDt = workingDt.AddDays(1);
                    }


                    //Following is the format of the data table
                    //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE

                    //first get the NAV for specific date
                    datewiseData = getMFNAVForDate(folderPath, workingDt.ToShortDateString());
                    if (datewiseData != null)
                    {
                        datewiseData = searchMFMaster(folderPath, searchString: mfName, bExactMatch: true, mfMasterTable: datewiseData, 
                                                        retryDays:5, searchDate:workingDt.ToShortDateString());
                        if ((datewiseData != null) && (datewiseData.Rows.Count > 0))
                        {
                            try
                            {
                                purchaseNAV = System.Convert.ToDouble(datewiseData.Rows[0]["NET_ASSET_VALUE"]);
                                purchaseUnits = Math.Round((valueAtCost / purchaseNAV), 4);

                                //string.Format("{0:0.0000}", fields[4])
                                addNewTransaction(filename, fundHouse, mfName, schemeCode, 
                                    System.Convert.ToDateTime(datewiseData.Rows[0]["Date"].ToString()).ToShortDateString(), 
                                    string.Format("{0:0.0000}", purchaseNAV),
                                    string.Format("{0:0.0000}", purchaseUnits), string.Format("{0:0.0000}", valueAtCost));
                            }
                            catch (Exception ex)
                            {
                                //go for next date
                                breturn = false;
                            }
                        }
                    }

                    if (sipFrequency == "Daily")
                    {
                        dt = dt.AddDays(1);
                    }
                    else if (sipFrequency == "Weekly")
                    {
                        dt = dt.AddDays(7);
                    }
                    else if (sipFrequency == "Monthly")
                    {
                        dt = dt.AddMonths(1);
                    }
                }
            }
            catch (Exception ex)
            {
                breturn = false;
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
                string lineDelete = fundHouse + ";" + mfName + ";" + schemeCode + ";" + purchaseDate + ";" + purchaseNAV + ";" + purchaseUnits + ";" + valueAtCost;
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
        static public bool updateTransaction(string filename, string fundHouse, string oldFundName, string schemeCode,
            string oldpurchaseDate, string oldpurchaseNAV, string oldpurchaseUnits, string oldvalueAtCost,
            string fundName, string purchaseDate, string purchaseNAV, string purchaseUnits, string valueAtCost)
        {
            bool breturn = false;
            try
            {
                string oldTransaction = fundHouse + ";" + oldFundName + ";" + schemeCode + ";" + oldpurchaseDate + ";" + oldpurchaseNAV + ";" + oldpurchaseUnits + ";" + oldvalueAtCost;
                string newTransaction = fundHouse + ";" + fundName + ";" + schemeCode + ";" + purchaseDate + ";" + purchaseNAV + ";" + purchaseUnits + ";" + valueAtCost;
                string line = null;

                using (StreamReader reader = new StreamReader(filename))
                {
                    using (StreamWriter writer = new StreamWriter(filename + ".tmp"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if((breturn == false) && (String.Compare(line, oldTransaction) == 0))
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
        static public DataTable openMFPortfolio(string folderPath, string portfolioFileName, bool bCurrent = true)
        {
            DataTable resultDataTable = null;
            StreamReader reader = null;
            string[] fields;
            string record;
            DataRow r;
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

                    //foreach (string fieldname in fields)
                    //{
                    //    resultDataTable.Columns.Add(fieldname, typeof(string));
                    //}
                    if (bCurrent == true)
                    {
                        resultDataTable.Columns.Add("CurrentNAV", typeof(double));
                        resultDataTable.Columns.Add("NAVDate", typeof(DateTime));
                        resultDataTable.Columns.Add("CurrentValue", typeof(double));
                        resultDataTable.Columns.Add("YearsInvested", typeof(double));
                        resultDataTable.Columns.Add("ARR", typeof(double));
                    }
                    while (!reader.EndOfStream)
                    {
                        record = reader.ReadLine();
                        fields = record.Split(';');

                        currentNAV = 0.00; currentValue = 0.00; yearsInvested = 0.00; arr = 0.00; valueAtCost = 0.00;
                        //currentNAVdt = DateTime.Today;
                        currentNAVdt = DateTime.MinValue;
                        if (bCurrent == true)
                        {
                            //get current NAV & find other details
                            //Format of the MF Master file & return table is as below
                            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE

                            //Format of the portfolio file
                            //FundHouse;FundName;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost
                            quoteTable = searchMFMaster(folderPath, fields[1], bExactMatch: true);
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
                                    yearsInvested = Math.Round(((currentNAVdt - purchaseNAVDt).TotalDays) / 365.25, 2);

                                    valueAtCost = System.Convert.ToDouble(string.Format("{0:0.0000}", fields[6]));

                                    //find ARR = ((current value / value at cost) ^ (1 / years invested)) - 1
                                    arr = Math.Round(Math.Pow((currentValue / valueAtCost), (1 / yearsInvested)) - 1, 2);
                                }
                                catch (Exception ex)
                                {
                                    currentNAV = 0.00; currentValue = 0.00; yearsInvested = 0.00; arr = 0.00;
                                }
                            }
                        }

                        //now save row - FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                        resultDataTable.Rows.Add(new object[] {
                            fields[0],  //FundHouse
                            fields[1],  //FundName
                            fields[2],  //SCHEME_CODE
                            System.Convert.ToDateTime(fields[3]).ToShortDateString(),  //PurchaseDate
                            Math.Round(System.Convert.ToDouble(fields[4]), 4),  //PurchaseNAV
                            Math.Round(System.Convert.ToDouble(fields[5]), 4),  //PurchaseUnits
                            Math.Round(System.Convert.ToDouble(fields[6]), 4),  //ValueAtCost
                            Math.Round(currentNAV, 4),    //CurrentNAV
                            currentNAVdt.ToShortDateString(),           //NAVDate
                            Math.Round(currentValue, 4),  //CurrentValue
                            Math.Round(yearsInvested, 2), //YearsInvested
                            Math.Round(arr,2)            //ARR
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

    }
}