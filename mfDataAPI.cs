using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Text;

namespace Analytics
{
    public static class MfDataAPI
    {

        //webservice_url = "https://www.amfiindia.com/spages/NAVAll.txt?t=27092020012930"; //string.Format(mfCurrentNAVALL_URL);
        //webservice_url = "https://www.amfiindia.com/spages/NAVAll.txt?t=27-09-2020"; //string.Format(mfCurrentNAVALL_URL);
        // string urlMF_MASTER_CURRENT = "https://www.amfiindia.com/spages/NAVAll.txt?t={0}";


        //Following URL will fetch latest NAV for ALL MF in following format
        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
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


        static string dbFile = ".\\portfolio\\MFData\\mfdata.db";
        //static void Main(string[] args)
        //{
        //    dbFile = args[0];
        //    //getAllMFNAVToday();
        //    TestLoadFromTo();
        //    //Console.WriteLine("Hello World!");
        //    //SQLiteConnection sqlite_conn;
        //    //sqlite_conn = CreateConnection();
        //    ////CreateTable(sqlite_conn);
        //    ////InsertData(sqlite_conn);
        //    //ReadData(sqlite_conn);
        //}

        static public SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn = null;
            string sCurrentDir = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDir, dbFile);
            string sFilePath = Path.GetFullPath(sFile);
            // Create a new database connection:
            //sqlite_conn = new SQLiteConnection(@"Data Source= E:\MSFT_SampleWork\Analytics\portfolio\MFData\mfdata.db; " +
            //    "   Version = 3; FailIfMissing=True; Foreign Keys=True; New = True; Compress = True; ");

            sqlite_conn = new SQLiteConnection("Data Source=" + sFilePath +
                ";   Version = 3; FailIfMissing=True; Foreign Keys=True; New = True; Compress = True; ");

            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (ex);
            }
            return sqlite_conn;
        }

        static public void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM FUNDHOUSE";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                int fundhousecode = Int32.Parse(sqlite_datareader[0].ToString());
                string name = sqlite_datareader[1].ToString();
                Console.WriteLine("code = " + fundhousecode + ", name = " + name);
            }
            conn.Close();
        }

        #region fetch from web methods
        /// <summary>
        /// method that inserts data in DB
        /// </summary>
        /// <param name="sourceFile">A string builder object containing all records read from respective URL</param>
        /// <returns>Number of recrods processed</returns>
        static public long insertRecordInDB(StringBuilder sourceFile)
        {
            string[] fields;
            string record;
            //DataRow r;
            string mfType = "", tmp1 = "";
            string mfCompName = "";
            int fundhousecode = -1;
            int schemetypeid = -1;
            int schemecode = -1;
            int recCounter = 0;
            string[] sourceLines;
            double nav;
            string schemeName, ISINDivPayoutISINGrowth, ISINDivReinvestment, netAssetValue, navDate;
            string recFormat1 = "Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date";
            string recFormat2 = "Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date";
            try
            {
                //No data found on the basis of selected parameters for this report
                sourceLines = sourceFile.ToString().Split('\n');
                sourceFile.Clear();

                if ((sourceLines[0].Contains(recFormat1)) || (sourceLines[0].Contains(recFormat2)))
                {
                    //get first line where fields are mentioned
                    //record = reader.ReadLine();
                    record = sourceLines[recCounter++];

                    //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                    //while (!reader.EndOfStream)
                    while (recCounter < sourceLines.Length)
                    {
                        //record = reader.ReadLine();
                        record = sourceLines[recCounter++];

                        record = record.Trim();

                        if (record.Length == 0)
                        {
                            continue;
                        }
                        else if (record.Contains(";") == false)
                        {
                            //case of either MF type or MF House

                            tmp1 = record;
                            //lets read next few lines till we find a line with either ; or no ;
                            //if we find a line with ; then it's continuation of same MF Type but
                            //while (!reader.EndOfStream)
                            while (recCounter < sourceLines.Length)
                            {
                                //record = reader.ReadLine();
                                record = sourceLines[recCounter++];

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

                                    schemetypeid = getSchemeTypeId(mfType);
                                    if (schemetypeid == -1)
                                    {
                                        schemetypeid = insertSchemeType(mfType);
                                    }
                                    Console.WriteLine("Schemetype= " + mfType);
                                }
                                else if (record.Contains(";") == true)
                                {
                                    //we continue with same MF type
                                    mfCompName = tmp1;

                                    //First check if MF COMP NAME exist in fundhouse table
                                    fundhousecode = getFundHouseCode(mfCompName);
                                    if (fundhousecode == -1)
                                    {
                                        //this should never happen as the fundhouse table is manually maintained
                                        fundhousecode = insertFundHouse(mfCompName);
                                    }
                                    Console.WriteLine("Fund House= " + mfCompName);

                                    break;
                                }
                            }
                        }

                        fields = record.Split(';');

                        //record can be one of following
                        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                        //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date
                        //Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date

                        if ((fields.Length == 6) || (fields.Length == 8))
                        {
                            //first get the schemecode
                            schemecode = int.Parse(fields[0]);
                            ISINDivPayoutISINGrowth = fields[1];
                            ISINDivReinvestment = fields[2];
                            schemeName = fields[3];
                            netAssetValue = fields[4];
                            navDate = fields[5];

                            if (fields.Length == 8)
                            {
                                schemeName = fields[1];
                                ISINDivPayoutISINGrowth = fields[2];
                                ISINDivReinvestment = fields[3];
                                navDate = fields[7];
                            }


                            //Now check if scheme exists in SCHEMES table
                            if (isSchemeExists(schemecode).ToUpper().Equals(schemeName.ToUpper()) == false)
                            {
                                //insert new scheme in schemes tables
                                insertScheme(fundhousecode, schemetypeid, schemecode, schemeName);
                            }

                            Console.WriteLine("Scheme code= " + schemecode + "--Scheme name= " + schemeName);

                            try
                            {
                                nav = System.Convert.ToDouble(netAssetValue);
                            }
                            catch (Exception)
                            {
                                nav = 0.00;
                            }

                            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                            insertTransaction(fundhousecode, schemetypeid, schemecode, ISINDivPayoutISINGrowth, ISINDivReinvestment, schemeName,
                                        string.Format("{0:0.0000}", nav), System.Convert.ToDateTime(navDate).ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertRecordInDB exception: " + ex.Message);
                throw ex;
            }
            return recCounter;
        }

        /// <summary>
        /// Method to fetch LATEST NAV for all MF's for all MF companies and all MF types
        /// </summary>
        /// <returns>true if data is fetched & processed successfully else false</returns>
        static public bool getAllMFNAVToday()
        {
            string webservice_url;
            Uri url;
            WebResponse wr;
            Stream receiveStream = null;
            StreamReader reader = null;
            StringBuilder sourceFile;
            long recCounter = 0;
            //string[] fields;
            //string record;
            ////DataRow r;
            //string mfType = "", tmp1 = "";
            //string mfCompName = "";
            //int fundhousecode = -1;
            //int schemetypeid = -1;
            //string schemecode = string.Empty;
            //string[] sourceLines;
            //double nav;
            try
            {
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
                    sourceFile = new StringBuilder(reader.ReadToEnd());

                    if (reader != null)
                        reader.Close();
                    if (receiveStream != null)
                        receiveStream.Close();

                    recCounter = insertRecordInDB(sourceFile);

                    //    sourceLines = sourceFile.ToString().Split('\n');

                    //    //get first line where fields are mentioned
                    //    //record = reader.ReadLine();
                    //    record = sourceLines[recCounter++];

                    //    fields = record.Split(';');

                    //    //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                    //    //while (!reader.EndOfStream)
                    //    while (recCounter < sourceLines.Length)
                    //    {
                    //        //record = reader.ReadLine();
                    //        record = sourceLines[recCounter++];

                    //        record = record.Trim();

                    //        if (record.Length == 0)
                    //        {
                    //            continue;
                    //        }
                    //        else if (record.Contains(";") == false)
                    //        {
                    //            //case of either MF type or MF House

                    //            tmp1 = record;
                    //            //lets read next few lines till we find a line with either ; or no ;
                    //            //if we find a line with ; then it's continuation of same MF Type but
                    //            //while (!reader.EndOfStream)
                    //            while (recCounter < sourceLines.Length)
                    //            {
                    //                //record = reader.ReadLine();
                    //                record = sourceLines[recCounter++];

                    //                record = record.Trim();

                    //                if (record.Length == 0)
                    //                {
                    //                    continue;
                    //                }
                    //                else if (record.Contains(";") == false)
                    //                {
                    //                    //we found a MF company name
                    //                    mfType = tmp1;
                    //                    mfCompName = record;
                    //                    tmp1 = record;

                    //                    schemetypeid = getSchemeTypeId(mfType);
                    //                    if (schemetypeid == -1)
                    //                    {
                    //                        schemetypeid = insertSchemeType(mfType);
                    //                    }
                    //                    Console.WriteLine("Schemetype= " + mfType);
                    //                }
                    //                else if (record.Contains(";") == true)
                    //                {
                    //                    //we continue with same MF type
                    //                    mfCompName = tmp1;

                    //                    //First check if MF COMP NAME exist in fundhouse table
                    //                    fundhousecode = getFundHouseCode(mfCompName);
                    //                    if (fundhousecode == -1)
                    //                    {
                    //                        //this should never happen as the fundhouse table is manually maintained
                    //                        fundhousecode = insertFundHouse(mfCompName);
                    //                    }
                    //                    Console.WriteLine("Fund House= " + mfCompName);

                    //                    break;
                    //                }
                    //            }
                    //        }

                    //        fields = record.Split(';');

                    //        //Check if we have values for - Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                    //        if (fields.Length == 6)
                    //        {
                    //            //first get the schemecode
                    //            schemecode = fields[0];

                    //            //Now check if scheme exists in SCHEMES table
                    //            if (isSchemeExists(schemecode) == string.Empty)
                    //            {
                    //                //insert new scheme in schemes tables
                    //                insertScheme(fundhousecode, schemetypeid, schemecode, fields[3]);
                    //            }

                    //            Console.WriteLine("Scheme code= " + schemecode + "--Scheme name= " + fields[3]);

                    //            try
                    //            {
                    //                nav = System.Convert.ToDouble(fields[4]);
                    //            }
                    //            catch (Exception)
                    //            {

                    //                nav = 0.00;
                    //            }

                    //            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                    //            insertTransaction(fundhousecode, schemetypeid, schemecode, fields[1], fields[2], fields[3], string.Format("{0:0.0000}", nav), System.Convert.ToDateTime(fields[5]).ToString("yyyy-MM-dd"));
                    //        }
                    //    }
                }
                //if (reader != null)
                //    reader.Close();
                //if (receiveStream != null)
                //    receiveStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Method - getAllMFNAVToday: " + "record processing failed with following error");
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine("Method - getAllMFNAVToday: " + recCounter + "records processed successfully");
            return true;
        }

        /// <summary>
        /// //This method will fetch MF data for specific date with following URL.
        ///http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt=01-Jan-2020
        ///The output is in different format than NAVALL for the current NAV
        ///The out put of this URL is as below
        ///Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date

        ///output of the method is table in following format
        ///MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        /// </summary>
        /// <param name="fetchDate"></param>
        /// <returns></returns>
        static public bool getAllMFNAVForDate(string fetchDate)
        {
            string webservice_url;
            Uri url;
            WebResponse wr;
            Stream receiveStream = null;
            StreamReader reader = null;
            StringBuilder sourceFile;
            long recCounter = 0;
            string dateFetch;

            //string[] fields;
            //string record;
            ////DataRow r;
            //string mfType = "", tmp1 = "";
            //string mfCompName = "";
            //int fundhousecode = -1;
            //int schemetypeid = -1;
            //string schemecode = string.Empty;
            //string[] sourceLines;
            //double nav;

            try
            {
                dateFetch = System.Convert.ToDateTime(fetchDate).ToString("yyyy-MM-dd");
                //http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?frmdt=01-Jan-2020
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
                    sourceFile = new StringBuilder(reader.ReadToEnd());

                    if (reader != null)
                        reader.Close();
                    if (receiveStream != null)
                        receiveStream.Close();

                    recCounter = insertRecordInDB(sourceFile);

                    //    sourceLines = sourceFile.ToString().Split('\n');

                    //    //get first line where fields are mentioned
                    //    //record = reader.ReadLine();
                    //    record = sourceLines[recCounter++];

                    //    fields = record.Split(';');

                    //    //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                    //    //while (!reader.EndOfStream)
                    //    while (recCounter < sourceLines.Length)
                    //    {
                    //        //record = reader.ReadLine();
                    //        record = sourceLines[recCounter++];

                    //        record = record.Trim();

                    //        if (record.Length == 0)
                    //        {
                    //            continue;
                    //        }
                    //        else if (record.Contains(";") == false)
                    //        {
                    //            //case of either MF type or MF House

                    //            tmp1 = record;
                    //            //lets read next few lines till we find a line with either ; or no ;
                    //            //if we find a line with ; then it's continuation of same MF Type but
                    //            //while (!reader.EndOfStream)
                    //            while (recCounter < sourceLines.Length)
                    //            {
                    //                //record = reader.ReadLine();
                    //                record = sourceLines[recCounter++];

                    //                record = record.Trim();

                    //                if (record.Length == 0)
                    //                {
                    //                    continue;
                    //                }
                    //                else if (record.Contains(";") == false)
                    //                {
                    //                    //we found a MF company name
                    //                    mfType = tmp1;
                    //                    mfCompName = record;
                    //                    tmp1 = record;

                    //                    schemetypeid = getSchemeTypeId(mfType);
                    //                    if (schemetypeid == -1)
                    //                    {
                    //                        schemetypeid = insertSchemeType(mfType);
                    //                    }
                    //                    Console.WriteLine("Schemetype= " + mfType);
                    //                }
                    //                else if (record.Contains(";") == true)
                    //                {
                    //                    //we continue with same MF type
                    //                    mfCompName = tmp1;

                    //                    //First check if MF COMP NAME exist in fundhouse table
                    //                    fundhousecode = getFundHouseCode(mfCompName);
                    //                    if (fundhousecode == -1)
                    //                    {
                    //                        //this should never happen as the fundhouse table is manually maintained
                    //                        fundhousecode = insertFundHouse(mfCompName);
                    //                    }
                    //                    Console.WriteLine("Fund House= " + mfCompName);

                    //                    break;
                    //                }
                    //            }
                    //        }

                    //        fields = record.Split(';');

                    //        //Check if we have values for - Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                    //        if (fields.Length == 6)
                    //        {
                    //            //first get the schemecode
                    //            schemecode = fields[0];

                    //            //Now check if scheme exists in SCHEMES table
                    //            if (isSchemeExists(schemecode) == string.Empty)
                    //            {
                    //                //insert new scheme in schemes tables
                    //                insertScheme(fundhousecode, schemetypeid, schemecode, fields[3]);
                    //            }

                    //            Console.WriteLine("Scheme code= " + schemecode + "--Scheme name= " + fields[3]);

                    //            try
                    //            {
                    //                nav = System.Convert.ToDouble(fields[4]);
                    //            }
                    //            catch (Exception)
                    //            {

                    //                nav = 0.00;
                    //            }

                    //            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                    //            insertTransaction(fundhousecode, schemetypeid, schemecode, fields[1], fields[2], fields[3], string.Format("{0:0.0000}", nav), System.Convert.ToDateTime(fields[5]).ToString("yyyy-MM-dd"));
                    //        }
                    //    }
                }
                //if (reader != null)
                //    reader.Close();
                //if (receiveStream != null)
                //    receiveStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Method - getAllMFNAVForDate: " + "record processing failed with following error");
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine("Method - getAllMFNAVForDate: " + recCounter + "records processed successfully");
            return true;
        }

        /// <summary>
        /// //This method will fetch MF NAV history data for specific MF Code between from date = fromDt & To date < to date
        ///http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf=27&frmdt=2020-09-01&todt=2020-09-04
        ///The output is in different format than NAVALL for the current NAV
        ///The out put of this URL is as below
        ///Scheme Code;Scheme Name;ISIN Div Payout/ISIN Growth;ISIN Div Reinvestment;Net Asset Value;Repurchase Price;Sale Price;Date
        ///output of the method is table in following format
        ///MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
        /// </summary>
        /// <param name="mfCode">Code of MF company</param>
        /// <param name="fromdt">From date string in yyyy-MM-dd format</param>
        /// <param name="todt">optional TO date string in yyyy-MM-dd format</param>
        /// <returns>true if ALL records processed successfully else false even if partial success</returns>
        static public bool getHistoryNAVForMFCode(string mfCode, string fromdt, string todt = null)
        {
            string webservice_url;
            Uri url;
            WebResponse wr;
            Stream receiveStream = null;
            StreamReader reader = null;
            StringBuilder sourceFile;
            long recCounter = 0;
            string dateFrom;
            string dateTo = null;

            //string[] fields;
            //string record;
            ////DataRow r;
            //string mfType = "", tmp1 = "";
            //string mfCompName = "";
            //int fundhousecode = -1;
            //int schemetypeid = -1;
            //string schemecode = string.Empty;
            //string[] sourceLines;
            //double nav;


            try
            {
                dateFrom = System.Convert.ToDateTime(fromdt).ToString("yyyy-MM-dd");

                if (todt != null)
                {
                    dateTo = System.Convert.ToDateTime(todt).ToString("yyyy-MM-dd");
                    webservice_url = string.Format(urlMF_NAV_HISTORY_FROM_TO, mfCode, dateFrom, dateTo);
                }
                else
                {
                    webservice_url = string.Format(urlMF_NAV_HISTORY_FROM, mfCode, dateFrom);
                }
                url = new Uri(webservice_url);
                var webRequest = WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.File.DownloadFile;
                //webRequest.ContentType = "application/json";
                wr = webRequest.GetResponseAsync().Result;
                receiveStream = wr.GetResponseStream();
                reader = new StreamReader(receiveStream);
                if (reader != null)
                {
                    sourceFile = new StringBuilder(reader.ReadToEnd());

                    if (reader != null)
                        reader.Close();
                    if (receiveStream != null)
                        receiveStream.Close();

                    recCounter = insertRecordInDB(sourceFile);
                    //    sourceLines = sourceFile.ToString().Split('\n');

                    //    //get first line where fields are mentioned
                    //    //record = reader.ReadLine();
                    //    record = sourceLines[recCounter++];

                    //    fields = record.Split(';');

                    //    //Now read each line and fill the data in table. We have to skip lines which do not have ';' and hence fields will be empty
                    //    //while (!reader.EndOfStream)
                    //    while (recCounter < sourceLines.Length)
                    //    {
                    //        //record = reader.ReadLine();
                    //        record = sourceLines[recCounter++];

                    //        record = record.Trim();

                    //        if (record.Length == 0)
                    //        {
                    //            continue;
                    //        }
                    //        else if (record.Contains(";") == false)
                    //        {
                    //            //case of either MF type or MF House

                    //            tmp1 = record;
                    //            //lets read next few lines till we find a line with either ; or no ;
                    //            //if we find a line with ; then it's continuation of same MF Type but
                    //            //while (!reader.EndOfStream)
                    //            while (recCounter < sourceLines.Length)
                    //            {
                    //                //record = reader.ReadLine();
                    //                record = sourceLines[recCounter++];

                    //                record = record.Trim();

                    //                if (record.Length == 0)
                    //                {
                    //                    continue;
                    //                }
                    //                else if (record.Contains(";") == false)
                    //                {
                    //                    //we found a MF company name
                    //                    mfType = tmp1;
                    //                    mfCompName = record;
                    //                    tmp1 = record;

                    //                    schemetypeid = getSchemeTypeId(mfType);
                    //                    if (schemetypeid == -1)
                    //                    {
                    //                        schemetypeid = insertSchemeType(mfType);
                    //                    }
                    //                    Console.WriteLine("Schemetype= " + mfType);
                    //                }
                    //                else if (record.Contains(";") == true)
                    //                {
                    //                    //we continue with same MF type
                    //                    mfCompName = tmp1;

                    //                    //First check if MF COMP NAME exist in fundhouse table
                    //                    fundhousecode = getFundHouseCode(mfCompName);
                    //                    if (fundhousecode == -1)
                    //                    {
                    //                        //this should never happen as the fundhouse table is manually maintained
                    //                        fundhousecode = insertFundHouse(mfCompName);
                    //                    }
                    //                    Console.WriteLine("Fund House= " + mfCompName);

                    //                    break;
                    //                }
                    //            }
                    //        }

                    //        fields = record.Split(';');

                    //        //Check if we have values for - Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
                    //        if (fields.Length == 6)
                    //        {
                    //            //first get the schemecode
                    //            schemecode = fields[0];

                    //            //Now check if scheme exists in SCHEMES table
                    //            if (isSchemeExists(schemecode) == string.Empty)
                    //            {
                    //                //insert new scheme in schemes tables
                    //                insertScheme(fundhousecode, schemetypeid, schemecode, fields[3]);
                    //            }

                    //            Console.WriteLine("Scheme code= " + schemecode + "--Scheme name= " + fields[3]);

                    //            try
                    //            {
                    //                nav = System.Convert.ToDouble(fields[4]);
                    //            }
                    //            catch (Exception)
                    //            {

                    //                nav = 0.00;
                    //            }

                    //            //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                    //            insertTransaction(fundhousecode, schemetypeid, schemecode, fields[1], fields[2], fields[3], string.Format("{0:0.0000}", nav), System.Convert.ToDateTime(fields[5]).ToString("yyyy-MM-dd"));
                    //        }
                    //    }
                }
                //if (reader != null)
                //    reader.Close();
                //if (receiveStream != null)
                //    receiveStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Method - getHistoryNAVForMFCode: " + "record processing failed with following error");
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine("Method - getHistoryNAVForMFCode: " + recCounter + "records processed successfully");
            return true;
        }
        #endregion

        #region insert methods
        static public int insertSchemeType(string schemeType)
        {
            int schemetypeid = -1;
            SQLiteConnection sqlite_conn = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "REPLACE INTO SCHEME_TYPE(TYPE) VALUES (@TYPE)";
                sqlite_cmd.Prepare();
                sqlite_cmd.Parameters.AddWithValue("@TYPE", schemeType);

                try
                {
                    schemetypeid = sqlite_cmd.ExecuteNonQuery();
                    if (schemetypeid > 0)
                    {
                        schemetypeid = getSchemeTypeId(schemeType);
                    }
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("insertSchemeType: [" + schemeType + "] " + exSQL.Message);
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertSchemeType: [" + schemeType + "] " + ex.Message);
            }
            sqlite_conn = null;
            sqlite_cmd = null;
            return schemetypeid;
        }

        static public int insertFundHouse(string fundHouse)
        {
            int fundhousecode = -1;
            SQLiteConnection sqlite_conn = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                //first get max(fundhousecode)
                fundhousecode = getMaxFundHouseID(fundHouse);

                if (fundhousecode != -1)
                {
                    sqlite_conn = CreateConnection();
                    sqlite_cmd = sqlite_conn.CreateCommand();

                    try
                    {
                        sqlite_cmd.CommandText = "INSERT INTO FUNDHOUSE(FUNDHOUSECODE, NAME) VALUES (@CODE, @NAME)";
                        sqlite_cmd.Prepare();
                        sqlite_cmd.Parameters.AddWithValue("@CODE", fundhousecode);
                        sqlite_cmd.Parameters.AddWithValue("@NAME", fundHouse);
                        sqlite_cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException exSQL)
                    {
                        Console.WriteLine("insertFundHouse: [" + fundHouse + "] " + exSQL.Message);
                        fundhousecode = -1;
                    }
                    if (sqlite_cmd != null)
                    {
                        sqlite_cmd.Dispose();
                    }

                    if (sqlite_conn != null)
                    {
                        sqlite_conn.Close();
                        sqlite_conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertFundHouse: [" + fundHouse + "] " + ex.Message);
            }
            sqlite_conn = null;
            sqlite_cmd = null;
            return fundhousecode;
        }

        static public int insertScheme(int fundHouseCode, int schemeTypeId, int schemeCode, string schemeName)
        {
            int numOfRowsInserted = 0;
            SQLiteConnection sqlite_conn = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "REPLACE INTO SCHEMES(SCHEMECODE, SCHEMENAME, FUNDHOUSECODE, SCHEMETYPEID) VALUES (@SCHEMECODE, @SCHEMENAME, @FUNDHOUSECODE, @SCHEMETYPEID)";
                sqlite_cmd.Prepare();
                sqlite_cmd.Parameters.AddWithValue("@SCHEMECODE", schemeCode);
                sqlite_cmd.Parameters.AddWithValue("@SCHEMENAME", schemeName);
                sqlite_cmd.Parameters.AddWithValue("@FUNDHOUSECODE", fundHouseCode);
                sqlite_cmd.Parameters.AddWithValue("@SCHEMETYPEID", schemeTypeId);

                //sqlite_cmd.CommandText = "INSERT INTO SCHEMES(SCHEMECODE, SCHEMENAME, FUNDHOUSECODE, SCHEMETYPEID) VALUES ('" +
                //    schemeCode + "','" + schemeName + "'," +  fundHouseCode.ToString() + "," + schemeTypeId.ToString() + ")";

                try
                {
                    numOfRowsInserted = sqlite_cmd.ExecuteNonQuery();
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("insertScheme: [" + fundHouseCode + "," + schemeTypeId + "," + schemeCode + "," + schemeName + "] " + exSQL.Message);
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertScheme: [" + fundHouseCode + "," + schemeTypeId + "," + schemeCode + "," + schemeName + "] " + ex.Message);
            }
            sqlite_conn = null;
            sqlite_cmd = null;
            return numOfRowsInserted;
        }

        //Scheme Code;ISIN Div Payout/ ISIN Growth;ISIN Div Reinvestment;Scheme Name;Net Asset Value;Date
        //insertTransaction(fundhousecode, schemetypeid, schemecode, fields[1], fields[2], fields[3], string.Format("{0:0.0000}", nav), System.Convert.ToDateTime(fields[5]).ToString("yyyy-MM-dd"));
        static public int insertTransaction(int fundHouseCode, int schemeTypeId, int schemeCode, string ISINDivPayout_ISINGrowth, string ISINDivReinvestment,
                                           string schemeName, string netAssetValue, string navDate)
        {
            int numOfRowsInserted = 0;
            SQLiteConnection sqlite_conn = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "REPLACE INTO NAVRECORDS(FUNDHOUSECODE, SCHEMECODE, SCHEMETYPEID, ISIN_Div_Payout_ISIN_Growth, ISIN_Div_Reinvestment, " +
                    "NET_ASSET_VALUE, NAVDATE) VALUES (@FUNDHOUSECODE, @SCHEMECODE, @SCHEMETYPEID, @ISIN_Div_Payout_ISIN_Growth, @ISIN_Div_Reinvestment, " +
                    "@NET_ASSET_VALUE, @NAVDATE)";
                sqlite_cmd.Prepare();
                sqlite_cmd.Parameters.AddWithValue("@FUNDHOUSECODE", fundHouseCode);
                sqlite_cmd.Parameters.AddWithValue("@SCHEMECODE", schemeCode);
                sqlite_cmd.Parameters.AddWithValue("@SCHEMETYPEID", schemeTypeId);

                sqlite_cmd.Parameters.AddWithValue("@ISIN_Div_Payout_ISIN_Growth", ISINDivPayout_ISINGrowth);
                sqlite_cmd.Parameters.AddWithValue("@ISIN_Div_Reinvestment", ISINDivReinvestment);
                sqlite_cmd.Parameters.AddWithValue("@NET_ASSET_VALUE", netAssetValue);
                sqlite_cmd.Parameters.AddWithValue("@NAVDATE", navDate);

                try
                {
                    numOfRowsInserted = sqlite_cmd.ExecuteNonQuery();
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("insertTransaction: [" + fundHouseCode + "," + schemeTypeId + "," + schemeCode + "," + ISINDivPayout_ISINGrowth + ","
                        + ISINDivReinvestment + "," + schemeName + "," + netAssetValue + "," + navDate + "] " + exSQL.Message);
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertTransaction: [" + fundHouseCode + "," + schemeTypeId + "," + schemeCode + "," + ISINDivPayout_ISINGrowth + ","
                        + ISINDivReinvestment + "," + schemeName + "," + netAssetValue + "," + navDate + "] " + ex.Message);
            }
            sqlite_conn = null;
            sqlite_cmd = null;
            return numOfRowsInserted;
        }
        #endregion

        #region get_methods
        /// <summary>
        /// Method to get predefined fundhouse code for given fundhouse/mf company value
        /// </summary>
        /// <param name="fundHouse">name of the fund house</param>
        /// <returns>matchng fund house code</returns>
        static public int getFundHouseCode(string fundHouse)
        {
            int fundhousecode = -1;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT FUNDHOUSECODE FROM FUNDHOUSE WHERE NAME = '" + fundHouse + "'";
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    //if (sqlite_datareader.HasRows)
                    if (sqlite_datareader.Read())
                    {
                        //sqlite_datareader.Read();
                        fundhousecode = Int32.Parse(sqlite_datareader["FUNDHOUSECODE"].ToString());
                    }
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getFundHouseCode: [" + fundHouse + "] :" + exSQL.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getFundHouseCode: [" + fundHouse + "] :" + ex.Message);
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
                sqlite_conn = null;
                sqlite_datareader = null;
                sqlite_cmd = null;
            }
            return fundhousecode;
        }

        /// <summary>
        /// Returns all data from fundhouse table. 
        /// Columns - FUNDHOUSECODE, NAME
        /// </summary>
        /// <returns></returns>
        static public DataTable getFundHouseTable()
        {
            DataTable returnTable = null;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null; ;
            SQLiteCommand sqlite_cmd = null;

            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT FUNDHOUSECODE, NAME FROM FUNDHOUSE";
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    returnTable = new DataTable();
                    returnTable.Load(sqlite_datareader);
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getFundHouseTable: " + exSQL.Message);
                    if (returnTable != null)
                    {
                        returnTable.Clear();
                        returnTable.Dispose();
                        returnTable = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getFundHouseTable: " + ex.Message);
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
                sqlite_conn = null;
                sqlite_datareader = null;
                sqlite_cmd = null;
            }
            return returnTable;
        }
        /// <summary>
        /// Gets ID of scheme type given scheme type
        /// </summary>
        /// <param name="schemeType"></param>
        /// <returns>ID of matching scheme type</returns>
        static public int getSchemeTypeId(string schemeType)
        {
            int schemetypeid = -1;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null; ;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT ID FROM SCHEME_TYPE WHERE TYPE = '" + schemeType + "'";
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    //if (sqlite_datareader.HasRows)
                    if (sqlite_datareader.Read())
                    {
                        //sqlite_datareader.Read();
                        schemetypeid = Int32.Parse(sqlite_datareader["ID"].ToString());
                    }
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getSchemeTypeId: [" + schemeType + "]" + exSQL.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getSchemeTypeId: [" + schemeType + "]" + ex.Message);
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }

                sqlite_conn = null;
                sqlite_datareader = null; ;
                sqlite_cmd = null;
            }
            return schemetypeid;
        }

        /// <summary>
        /// Return all data from scheme_type table
        /// columns - ID, TYPE
        /// </summary>
        /// <returns></returns>
        static public DataTable getSchemeTypeTable()
        {
            DataTable returnTable = null;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null; ;
            SQLiteCommand sqlite_cmd = null;

            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT ID, TYPE FROM SCHEME_TYPE";
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    returnTable = new DataTable();
                    returnTable.Load(sqlite_datareader);
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getSchemeTypeTable: " + exSQL.Message);
                    if (returnTable != null)
                    {
                        returnTable.Clear();
                        returnTable.Dispose();
                        returnTable = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getSchemeTypeTable: " + ex.Message);
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
                sqlite_conn = null;
                sqlite_datareader = null;
                sqlite_cmd = null;
            }
            return returnTable;
        }

        /// <summary>
        /// Finds if given scheme code exists in Schemes table
        /// </summary>
        /// <param name="schemeCode"></param>
        /// <returns>matching scheme name if found else empty string </returns>
        static public string isSchemeExists(int schemeCode)
        {
            string schemeName = string.Empty;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT SCHEMENAME FROM SCHEMES WHERE SCHEMECODE = @CODE";
                sqlite_cmd.Prepare();
                sqlite_cmd.Parameters.AddWithValue("@CODE", schemeCode);
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    //if (sqlite_datareader.HasRows)
                    if (sqlite_datareader.Read())
                    {
                        //sqlite_datareader.Read();
                        schemeName = sqlite_datareader["SCHEMENAME"].ToString();
                    }
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("isSchemeExists: [" + schemeCode + "] " + exSQL.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("isSchemeExists: [" + schemeCode + "] " + ex.Message);
                schemeName = string.Empty;
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }

                sqlite_conn = null;
                sqlite_datareader = null;
                sqlite_cmd = null;
            }
            return schemeName;
        }

        /// <summary>
        /// returns data from schemes table matching given fundhouse code and scheme type id. The filters are ignored if value passed to method = -1
        /// </summary>
        /// <param name="fundhousecode"></param>
        /// <param name="schemetypeid"></param>
        /// <returns>Data Table matching criterion provided in fundhousecode and schemetypeid</returns>
        static public DataTable getSchemesTable(int fundhousecode = -1, int schemetypeid = -1)
        {
            DataTable returnTable = null;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null; ;
            SQLiteCommand sqlite_cmd = null;
            string statement = "SELECT SCHEME_TYPE.ID AS SCHEMETYPEID , SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME FROM SCHEMES " +
                "INNER JOIN SCHEME_TYPE ON SCHEME_TYPE.ID = SCHEMES.SCHEMETYPEID " +
                "INNER JOIN FUNDHOUSE ON FUNDHOUSE.FUNDHOUSECODE = SCHEMES.FUNDHOUSECODE ";

            try
            {
                if ((fundhousecode != -1) && (schemetypeid != -1))
                {
                    statement += "WHERE FUNDHOUSE.FUNDHOUSECODE = " + fundhousecode.ToString() + "AND SCHEME_TYPE.ID = " + schemetypeid.ToString();
                }
                else
                {
                    if (fundhousecode != -1)
                    {
                        statement += "WHERE FUNDHOUSE.FUNDHOUSECODE = " + fundhousecode.ToString();
                    }
                    else if (schemetypeid != -1)
                    {
                        if (fundhousecode != -1)
                        {
                            statement += " AND SCHEME_TYPE.ID = " + schemetypeid.ToString();
                        }
                        else
                        {
                            statement += "WHERE SCHEME_TYPE.ID = " + schemetypeid.ToString();
                        }
                    }
                }
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = statement;
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    returnTable = new DataTable();
                    returnTable.Load(sqlite_datareader);
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getSchemesTable: " + exSQL.Message);
                    if (returnTable != null)
                    {
                        returnTable.Clear();
                        returnTable.Dispose();
                        returnTable = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getSchemesTable: " + ex.Message);
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
                sqlite_conn = null;
                sqlite_datareader = null;
                sqlite_cmd = null;
            }
            return returnTable;
        }

        static public int getMaxFundHouseID(string fundHouse)
        {
            int fundhousecode = -1;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null;
            SQLiteCommand sqlite_cmd = null;
            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();

                //first get max(fundhousecode)
                sqlite_cmd.CommandText = "SELECT MAX(FUNDHOUSECODE) FROM FUNDHOUSE";
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    //if (sqlite_datareader.HasRows)
                    if (sqlite_datareader.Read())
                    {
                        //sqlite_datareader.Read();
                        fundhousecode = Int32.Parse(sqlite_datareader[0].ToString());
                        fundhousecode++;
                    }
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getMaxFundHouseID: [" + fundHouse + "] " + exSQL.Message);
                    fundhousecode = -1;
                }
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getMaxFundHouseID: [" + fundHouse + "] " + ex.Message);
            }
            sqlite_conn = null;
            sqlite_datareader = null;
            sqlite_cmd = null;
            return fundhousecode;
        }

        static public DataTable getNAVRecordsTable(int schemetypeid = -1, int fundhousecode = -1, int schemecode = -1, string fromDate = null, string toDate = null)
        {
            DataTable returnTable = null;
            SQLiteConnection sqlite_conn = null;
            SQLiteDataReader sqlite_datareader = null; ;
            SQLiteCommand sqlite_cmd = null;
            string statement = "SELECT NAVRECORDS.ID, SCHEME_TYPE.ID AS SCHEMETYPEID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, " +
                                "SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME, NAVRECORDS.NET_ASSET_VALUE, strftime('%d-%m-%Y', NAVRECORDS.NAVDATE) as NAVDATE FROM NAVRECORDS ";
            statement += "INNER JOIN SCHEME_TYPE ON SCHEME_TYPE.ID = NAVRECORDS.SCHEMETYPEID ";
            statement += "INNER JOIN FUNDHOUSE ON FUNDHOUSE.FUNDHOUSECODE = NAVRECORDS.FUNDHOUSECODE ";
            statement += "INNER JOIN SCHEMES ON SCHEMES.SCHEMECODE = NAVRECORDS.SCHEMECODE ";
            try
            {
                if ((fundhousecode != -1) && (schemetypeid != -1) && (schemecode != -1))
                {
                    statement += "WHERE SCHEME_TYPE.ID  = " + schemetypeid.ToString() + " AND FUNDHOUSE.FUNDHOUSECODE = " + fundhousecode.ToString() +
                                " AND SCHEMES.SCHEMECODE = " + schemecode.ToString() + " ";
                }
                else
                {
                    if (schemetypeid != -1)
                    {
                        statement += "WHERE SCHEME_TYPE.ID = " + schemetypeid.ToString() + " ";
                    }
                    if (fundhousecode != -1)
                    {
                        if (schemetypeid != -1)
                        {
                            statement += "AND FUNDHOUSE.FUNDHOUSECODE = " + fundhousecode.ToString() + " ";
                        }
                        else
                        {
                            statement += "WHERE FUNDHOUSE.FUNDHOUSECODE = " + fundhousecode.ToString() + " ";
                        }
                    }
                    if (schemecode != -1)
                    {
                        if ((schemetypeid != -1) || (fundhousecode != -1))
                        {
                            statement += "AND SCHEMES.SCHEMECODE = " + schemecode.ToString() + " ";
                        }
                        else
                        {
                            statement += "WHERE SCHEMES.SCHEMECODE = " + schemecode.ToString() + " ";
                        }
                    }
                }
                if (fromDate != null)
                {
                    if ((fundhousecode != -1) || (schemetypeid != -1) || (schemecode != -1))
                    {
                        statement += "AND date(NAVRECORDS.NAVDATE) >= date(\"" + System.Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + "\") ";
                    }
                    else
                    {
                        statement += "WHERE date(NAVRECORDS.NAVDATE) >= date(\"" + System.Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd") + "\") ";
                    }
                }
                if (toDate != null)
                {
                    if ((fundhousecode != -1) || (schemetypeid != -1) || (schemecode != -1))
                    {
                        statement += "AND date(NAVRECORDS.NAVDATE) <= date(\"" + System.Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") + "\") ";
                    }
                    else
                    {
                        statement += "WHERE date(NAVRECORDS.NAVDATE) <= date(\"" + System.Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") + "\") ";
                    }
                }
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = statement;
                try
                {
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    returnTable = new DataTable();
                    returnTable.Columns.Add("ID", typeof(long));
                    returnTable.Columns.Add("SCHEMETYPEID", typeof(long));
                    returnTable.Columns.Add("TYPE", typeof(string));
                    returnTable.Columns.Add("FUNDHOUSECODE", typeof(int));
                    returnTable.Columns.Add("NAME", typeof(string));
                    returnTable.Columns.Add("SCHEMECODE", typeof(long));
                    returnTable.Columns.Add("SCHEMENAME", typeof(string));
                    returnTable.Columns.Add("NET_ASSET_VALUE", typeof(decimal));
                    returnTable.Columns.Add("NAVDATE", typeof(DateTime));
                    returnTable.Load(sqlite_datareader);
                    //NAVRECORDS.ID, SCHEME_TYPE.ID AS SCHEMETYPEID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, " +
                    //"SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME, NAVRECORDS.NET_ASSET_VALUE, strftime('%d-%m-%Y', NAVRECORDS.NAVDATE)  as NAVDATE
                    //returnTable.Columns["NAVDATE"].DataType = typeof(DateTime);
                    //returnTable.Columns["NET_ASSET_VALUE"].DataType = typeof(decimal);
                }
                catch (SQLiteException exSQL)
                {
                    Console.WriteLine("getNAVRecordsTable: " + exSQL.Message);
                    if (returnTable != null)
                    {
                        returnTable.Clear();
                        returnTable.Dispose();
                        returnTable = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getNAVRecordsTable: " + ex.Message);
            }
            finally
            {
                if (sqlite_datareader != null)
                {
                    sqlite_datareader.Close();
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
                sqlite_conn = null;
                sqlite_datareader = null;
                sqlite_cmd = null;
            }
            return returnTable;
        }


        static public DataTable getRSIDataTableFromDailyNAV(int schemetypeid = -1, int fundhousecode = -1, int schemecode = -1, string fromDate = null, string toDate = null,
                                                            string period = "20")
        {
            DataTable dailyTable = null;
            DataTable rsiDataTable = null;
            int iPeriod;
            double change, gain, loss, avgGain = 0.00, avgLoss = 0.00, rs, rsi;
            double sumOfGain = 0.00, sumOfLoss = 0.00;
            DateTime dateCurrentRow = DateTime.Today;

            try
            {
                dailyTable = getNAVRecordsTable(schemetypeid, fundhousecode, schemecode, fromDate, toDate);
                if ((dailyTable != null) && (dailyTable.Rows.Count > 0))
                {
                    iPeriod = System.Convert.ToInt32(period);
                    rsiDataTable = new DataTable();

                    rsiDataTable.Columns.Add("SCHEMECODE", typeof(long));
                    rsiDataTable.Columns.Add("SCHEMENAME", typeof(string));
                    rsiDataTable.Columns.Add("Date", typeof(DateTime));
                    rsiDataTable.Columns.Add("RSI", typeof(decimal));

                    //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                    //SMA = divide the sum by "period"
                    //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                    for (int rownum = 1; rownum < dailyTable.Rows.Count; rownum++)
                    {
                        //current - prev
                        change = System.Convert.ToDouble(dailyTable.Rows[rownum]["NET_ASSET_VALUE"]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["NET_ASSET_VALUE"]);
                        dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["NAVDATE"]);

                        if (change < 0)
                        {
                            loss = change;
                            gain = 0.00;
                        }
                        else
                        {
                            gain = change;
                            loss = 0.00;
                        }

                        //for the first iPeriod keep adding loss & gain
                        if (rownum < iPeriod)
                        {
                            sumOfGain += gain;
                            sumOfLoss += loss;
                        }
                        else if (rownum == iPeriod)
                        {
                            sumOfGain += gain;
                            sumOfLoss += loss;
                            //we also find  other fields and SAVE
                            avgGain = sumOfGain / iPeriod;
                            avgLoss = sumOfLoss / iPeriod;
                            rs = avgGain / avgLoss;
                            rsi = 100 - (100 / (1 - rs));
                            rsiDataTable.Rows.Add(new object[] {
                                                                    schemecode,
                                                                    dailyTable.Rows[rownum]["SCHEMENAME"].ToString(),
                                                                    dateCurrentRow.ToString("dd-MM-yyyy"),
                                                                    Math.Round(rsi, 4)
                                                                });
                        }
                        else
                        {
                            avgGain = ((avgGain * (iPeriod - 1)) + gain) / iPeriod;
                            avgLoss = ((avgLoss * (iPeriod - 1)) + loss) / iPeriod;
                            rs = avgGain / avgLoss;
                            rsi = 100 - (100 / (1 - rs));
                            rsiDataTable.Rows.Add(new object[] {
                                                                    schemecode,
                                                                    dailyTable.Rows[rownum]["SCHEMENAME"].ToString(),
                                                                    dateCurrentRow.ToString("dd-MM-yyyy"),
                                                                    Math.Round(rsi, 4)
                                                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (rsiDataTable != null)
                {
                    rsiDataTable.Clear();
                    rsiDataTable.Dispose();
                }
                rsiDataTable = null;
            }
            if(dailyTable != null)
            {
                dailyTable.Clear();
                dailyTable.Dispose();
            }
            dailyTable = null;
            return rsiDataTable;
        }

        #endregion


        #region portfolio
        static public long createnewMFPortfolio(string userid, string portfolioname)
        {
            long portfolio_id = -1;
            SQLiteConnection sqlite_conn = null;
            SQLiteCommand sqlite_cmd = null;

            try
            {
                sqlite_conn = CreateConnection();
                sqlite_cmd = sqlite_conn.CreateCommand();
                try
                {
                    sqlite_cmd.CommandText = "INSERT INTO PORTFOLIO_MASTER(USERID, PORTFOLIO_NAME) VALUES (@USERID, @NAME)";
                    sqlite_cmd.Prepare();
                    sqlite_cmd.Parameters.AddWithValue("@USERID", userid);
                    sqlite_cmd.Parameters.AddWithValue("@NAME", portfolioname);
                    if(sqlite_cmd.ExecuteNonQuery() > 0)
                    {
                        sqlite_cmd.CommandText = "SELECT seq from sqlite_sequence WHERE name = \"PORTFOLIO\"";
                        portfolio_id = Convert.ToInt64(sqlite_cmd.ExecuteScalar());
                    }
                }
                catch (SQLiteException exSQL)
                {
                   Console.WriteLine("CreateNewPortfolio: " + userid + ", " + portfolioname  + "\n" + exSQL.Message);
                    portfolio_id = -1;
                }
                if (sqlite_cmd != null)
                {
                    sqlite_cmd.Dispose();
                }

                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                    sqlite_conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                portfolio_id = -1;
            }
            return portfolio_id;
        }

        #endregion
        static void TestLoadFromTo()
        {
            string fromDt = "2008-01-01";
            string toDt = DateTime.Today.ToString("yyyy-MM-dd");
            DataTable fundhouseTable = getFundHouseTable();
            foreach (DataRow row in fundhouseTable.Rows)
            {
                //if (row["FUNDHOUSECODE"].ToString().Equals("-1") == false)
                {
                    getHistoryNAVForMFCode(row["FUNDHOUSECODE"].ToString(), fromdt: fromDt, todt: toDt);
                }
            }
        }
    }
}