using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
//using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;

namespace Analytics
{
    public static class StockApi
    {
        //public static bool isTestMode = true;
        public static string testDataFolder = ".\\scriptdata\\";
        public static bool ALPHAVANTAGE = false;

        //public static string apiKey = "&apikey=XXXX";
        //public static string apiKey = "XXXX"; instead added tpo each method signature--> string apiKey = "UV6KQA6735QZKBTV")
        public static string dataType = "csv";

        public static string indicators = "quote";
        public static string includeTimestamps = "true";

        //https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=BA&apikey=demo&datatype=csv
        public static string urlSymbolSearch = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={0}&apikey={1}&datatype={2}";

        //https://autoc.finance.yahoo.com/autoc?query=larsen&lang=en-US
        public static string urlSymbolSearch_altername = "https://autoc.finance.yahoo.com/autoc?query={0}&lang=en-US";

        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=2yr&interval=1d&indicators=quote&includeTimestamps=true
        public static string urlGetDaily_alternate = "https://query1.finance.yahoo.com/v7/finance/chart/{0}?range={1}&interval={2}&indicators={3}&includeTimestamps={4}";

        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=2yr&interval=1d&indicators=quote&includeTimestamps=true
        public static string urlGetIntra_alternate = "https://query1.finance.yahoo.com/v7/finance/chart/{0}?range={1}&interval={2}&indicators={3}&includeTimestamps={4}";

        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=1d&interval=1d&indicators=quote&timestamp=true
        public static string urlGlobalQuote_alternate = "https://query1.finance.yahoo.com/v7/finance/chart/{0}?range=1d&interval=1d&indicators=quote&timestamp=true";

#if ALPHAVANTAGE == true

        //'https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={}&apikey={}&datatype=csv'
        public static string urlGlobalQuote = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey={1}&datatype={2}";

        //https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={}&apikey={}&outputsize={}&datatype=csv
        public static string urlGetDaily = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}&outputsize={2}&datatype={3}";

        //'https://www.alphavantage.co/query?function=BBANDS&symbol={}&interval=daily&time_period=20&series_type=close&nbdevup=2&nbdevdn=2&apikey={}&datatype=csv'
        public static string urlGetBBands = "https://www.alphavantage.co/query?function=BBANDS&symbol={0}&interval={1}&time_period={2}&series_type={3}&nbdevup={4}&nbdevdn={5}&apikey={6}&datatype={7}";

        //'https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={}&interval=5min&apikey={}&outputsize={}&datatype=csv'
        public static string urlIntra = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={0}&interval={1}&apikey={2}&outputsize={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=SMA&symbol={}&interval=daily&time_period=20&series_type=close&apikey={}&datatype=csv
        public static string urlSMA = "https://www.alphavantage.co/query?function=SMA&symbol={0}&interval={1}&time_period={2}&series_type={3}&apikey={4}&datatype={5}";

        //https://www.alphavantage.co/query?function=EMA&symbol={}&interval=daily&time_period=20&series_type=close&apikey={}&datatype=csv
        public static string urlEMA = "https://www.alphavantage.co/query?function=EMA&symbol={0}&interval={1}&time_period={2}&series_type={3}&apikey={4}&datatype={5}";

        //https://www.alphavantage.co/query?function=VWAP&symbol={}&interval=5min&apikey={}&datatype=csv
        public static string urlVWAP = "https://www.alphavantage.co/query?function=VWAP&symbol={0}&interval={1}&apikey={2}&datatype={3}";

        //https://www.alphavantage.co/query?function=RSI&symbol={}&interval=daily&time_period=20&series_type=close&apikey={}&datatype=csv
        public static string urlRSI = "https://www.alphavantage.co/query?function=RSI&symbol={0}&interval={1}&time_period={2}&series_type={3}&apikey={4}&datatype={5}";

        //https://www.alphavantage.co/query?function=STOCH&symbol={}&interval=daily&apikey={}&datatype=csv
        public static string urlSTOCH = "https://www.alphavantage.co/query?function=STOCH&symbol={0}&interval={1}&fastkperiod={2}&slowkperiod={3}&slowdperiod={4}&slowkmatype={5}&slowdmatype={6}&apikey={7}&datatype={8}";

        //https://www.alphavantage.co/query?function=MACD&symbol={}&interval=daily&series_type=close&apikey={}&datatype=csv
        public static string urlMACD = "https://www.alphavantage.co/query?function=MACD&symbol={0}&interval={1}&series_type={2}&fastperiod={3}&slowperiod={4}&signalperiod={5}&apikey={6}&datatype={7}";

        //https://www.alphavantage.co/query?function=AROON&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
        public static string urlAROON = "https://www.alphavantage.co/query?function=AROON&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
        public static string urlADX = "https://www.alphavantage.co/query?function=ADX&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=DX&symbol=IBM&interval=daily&time_period=10&apikey=demo
        public static string urlDX = "https://www.alphavantage.co/query?function=DX&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=MINUS_DI&symbol=IBM&interval=weekly&time_period=10&apikey=demo
        public static string urlMinusDI = "https://www.alphavantage.co/query?function=MINUS_DI&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=PLUS_DI&symbol=IBM&interval=daily&time_period=10&apikey=demo
        public static string urlPlusDI = "https://www.alphavantage.co/query?function=PLUS_DI&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=MINUS_DM&symbol=IBM&interval=daily&time_period=10&apikey=demo
        public static string urlMinusDM = "https://www.alphavantage.co/query?function=MINUS_DM&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        //https://www.alphavantage.co/query?function=PLUS_DM&symbol=IBM&interval=daily&time_period=10&apikey=demo
        public static string urlPlusDM = "https://www.alphavantage.co/query?function=PLUS_DM&symbol={0}&interval={1}&time_period={2}&apikey={3}&datatype={4}";

        /* return CSV is in following format
    symbol,name,type,region,marketOpen,marketClose,timezone,currency,matchScore
    BA,The Boeing Company,Equity,United States,09:30,16:00,UTC-05,USD,1.0000
    BAC,Bank of America Corporation,Equity,United States,09:30,16:00,UTC-05,USD,0.8000
    BABA,Alibaba Group Holding Limited,Equity,United States,09:30,16:00,UTC-05,USD,0.6667
    GOLD,Barrick Gold Corporation,Equity,United States,09:30,16:00,UTC-05,USD,0.5714
    BIDU,Baidu Inc.,Equity,United States,09:30,16:00,UTC-05,USD,0.5000
    BAYRY,Bayer Aktiengesellschaft,Equity,United States,09:30,16:00,UTC-05,USD,0.4000
    BLDP,Ballard Power Systems Inc.,Equity,United States,09:30,16:00,UTC-05,USD,0.3333
    BHC,Bausch Health Companies Inc.,Equity,United States,09:30,16:00,UTC-05,USD,0.3333
    BK,The Bank of New York Mellon Corporation,Equity,United States,09:30,16:00,UTC-05,USD,0.1538
*/
        public static DataTable symbolSearch(string searchKeyword, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                //https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=BA&apikey=demo&datatype=csv
                //string webservice_url = $"{urlSymbolSearch} + {searchKeyword} + {apiKey} + {dataType}";
                string webservice_url = string.Format(StockApi.urlSymbolSearch, searchKeyword, apiKey, StockApi.dataType);
                Uri url = new Uri(webservice_url);
                var webRequest = WebRequest.Create(url);
                if (webRequest != null)
                {
                    DataTable resultDataTable = new DataTable();

                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";

                    //Get the response 
                    WebResponse wr = webRequest.GetResponseAsync().Result;
                    Stream receiveStream = wr.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream);

                    //DataTable dt = new DataTable();
                    resultDataTable.Columns.Add("Symbol", typeof(string));
                    resultDataTable.Columns.Add("Name", typeof(string));

                    //First line indicates the fields
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        string[] field;

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            resultDataTable.Rows.Add(new object[] {
                        field[0],
                        field[0]+ ": " + field[1]
                        });

                        }
                        reader.Close();
                        receiveStream.Close();

                        return resultDataTable;
                    }
                    else
                    {
                        reader.Close();
                        receiveStream.Close();

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="outputsize">[default=compact or full]</param>
        /// <param name="bIsTestModeOn">default=true or false</param>
        /// <param name="bSaveData">default=false. To save data to file & return set it to true and set bIsTestModeOn = false</param>
        /// <returns></returns>
        public static DataTable getDaily(string folderPath, string scriptName, string outputsize = "compact", bool bIsTestModeOn = true,
                                        bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dailyDataTable = null;
                if (bIsTestModeOn == false)
                {
                    //"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}&outputsize={2}&datatype={3}";
                    webservice_url = string.Format(StockApi.urlGetDaily, scriptName, apiKey, outputsize, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv", fileData);
                            dailyDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return dailyDataTable;
                    }
                }
                else
                {
                    if (File.Exists(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        dailyDataTable = new DataTable();
                        string[] field;
                        //DataTable dt = new DataTable();
                        dailyDataTable.Columns.Add("Symbol", typeof(string));
                        dailyDataTable.Columns.Add("Date", typeof(DateTime));
                        dailyDataTable.Columns.Add("Open", typeof(decimal));
                        dailyDataTable.Columns.Add("High", typeof(decimal));
                        dailyDataTable.Columns.Add("Low", typeof(decimal));
                        dailyDataTable.Columns.Add("Close", typeof(decimal));
                        dailyDataTable.Columns.Add("Volume", typeof(int));
                        dailyDataTable.Columns.Add("PurchaseDate", typeof(string));
                        dailyDataTable.Columns.Add("CumulativeQuantity", typeof(int));
                        dailyDataTable.Columns.Add("CostofInvestment", typeof(decimal));
                        dailyDataTable.Columns.Add("ValueOnDate", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dailyDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3],
                                                                    field[4],
                                                                    field[5]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return dailyDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="time_interval">['1min', *'5min', '15min', '30min', '60min', 'daily', 'weekly', 'monthly']</param>
        /// <param name="outputsize">[*compact, full</param>
        /// <param name="bIsTestModeOn"></param>
        /// <returns></returns>
        public static DataTable getIntraday(string folderPath, string scriptName, string time_interval = "5min", string outputsize = "compact",
                                            bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable intradayDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //'https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={}&interval=5min&apikey={}&outputsize={}&datatype=csv'
                    webservice_url = string.Format(StockApi.urlIntra, scriptName, time_interval, apiKey, outputsize, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv", fileData);
                            intradayDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return intradayDataTable;
                    }

                }
                else
                {
                    //intraday_5min_compact_LT.BSE.csv OR intraday_5min_full_LT.BSE.csv
                    if (File.Exists(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //timestamp,open,high,low,close,volume

                        intradayDataTable = new DataTable();

                        string[] field = record.Split(',');

                        intradayDataTable.Columns.Add("Symbol", typeof(string));
                        intradayDataTable.Columns.Add("Date", typeof(DateTime));
                        //intradayDataTable.Columns.Add("Date", typeof(Double));
                        intradayDataTable.Columns.Add("Open", typeof(decimal));
                        intradayDataTable.Columns.Add("High", typeof(decimal));
                        intradayDataTable.Columns.Add("Low", typeof(decimal));
                        intradayDataTable.Columns.Add("Close", typeof(decimal));
                        intradayDataTable.Columns.Add("Volume", typeof(int));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            intradayDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    //System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    System.Convert.ToDateTime(field[0]),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3],
                                                                    field[4],
                                                                    field[5]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return intradayDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="period">[10, *20]</param>
        /// <param name="seriestype">['open', 'high', 'low', *'close']</param>
        /// <param name="bIsTestModeOn"></param>
        /// <returns></returns>
        public static DataTable getSMA(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                    string seriestype = "close", bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable smaDataTable = null;
                string filename = folderPath + scriptName + "_" + "SMA_" + day_interval + "_" + period + "_" + seriestype + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=SMA&symbol={}&interval=daily&time_period=20&series_type=close&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlSMA, scriptName, day_interval, period, seriestype, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            smaDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return smaDataTable;
                    }

                }
                else
                {
                    //SMA_10_LT.BSE.csv or SMA_20_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SMA

                        smaDataTable = new DataTable();

                        string[] field = record.Split(',');

                        smaDataTable.Columns.Add("Symbol", typeof(string));
                        smaDataTable.Columns.Add("Date", typeof(DateTime));
                        smaDataTable.Columns.Add("SMA", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            smaDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return smaDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="period">[10, *20]</param>
        /// <param name="seriestype">['open', 'high', 'low', *'close']</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getEMA(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        string seriestype = "close", bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable emaDataTable = null;
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                string filename = folderPath + scriptName + "_" + "EMA_" + day_interval + "_" + period + "_" + seriestype + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=EMA&symbol={}&interval=daily&time_period=20&series_type=close&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlEMA, scriptName, day_interval, period, seriestype, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            emaDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return emaDataTable;
                    }

                }
                else
                {
                    //EMA_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,EMA

                        emaDataTable = new DataTable();

                        string[] field = record.Split(',');

                        emaDataTable.Columns.Add("Symbol", typeof(string));
                        emaDataTable.Columns.Add("Date", typeof(DateTime));
                        emaDataTable.Columns.Add("EMA", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            emaDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return emaDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', *'5min', '15min', '30min', '60min']</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getVWAP(string folderPath, string scriptName, string day_interval = "5min", bool bIsTestModeOn = true,
                                        bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable vwapDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=VWAP&symbol={}&interval=5min&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlVWAP, scriptName, day_interval, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "VWAP_" + day_interval + ".csv", fileData);
                            vwapDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return vwapDataTable;
                    }

                }
                else
                {
                    //VWAP_LT.BSE.csv
                    if (File.Exists(folderPath + scriptName + "_" + "VWAP_" + day_interval + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "VWAP_" + day_interval + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,VWAP

                        vwapDataTable = new DataTable();

                        string[] field = record.Split(',');

                        vwapDataTable.Columns.Add("Symbol", typeof(string));
                        vwapDataTable.Columns.Add("Date", typeof(DateTime));
                        vwapDataTable.Columns.Add("VWAP", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');


                            vwapDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    //System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    System.Convert.ToDateTime(field[0]),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return vwapDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="period">[10, *20]</param>
        /// <param name="seriestype">['open', 'high', 'low', *'close']</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getRSI(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        string seriestype = "close", bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable rsiDataTable = null;
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                string filename = folderPath + scriptName + "_" + "RSI_" + day_interval + "_" + period + "_" + seriestype + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=RSI&symbol={}&interval=daily&time_period=20&series_type=close&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlRSI, scriptName, day_interval, period, seriestype, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            rsiDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return rsiDataTable;
                    }

                }
                else
                {
                    //RSI_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,RSI

                        rsiDataTable = new DataTable();

                        string[] field = record.Split(',');

                        rsiDataTable.Columns.Add("Symbol", typeof(string));
                        rsiDataTable.Columns.Add("Date", typeof(DateTime));
                        rsiDataTable.Columns.Add("RSI", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            rsiDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
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
            return rsiDataTable;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="fastkperiod">Positive int, default = 5</param>
        /// <param name="slowkperiod">Positive int, default = 3</param>
        /// <param name="slowdperiod">Positive int, default = 3</param>
        /// <param name="slowkmatype">Moving average type for the slowk moving average. By default, slowkmatype=0. 
        /// Integers 0 - 8 are accepted with the following mappings. 
        /// 0 = Simple Moving Average (SMA), 1 = Exponential Moving Average (EMA), 2 = Weighted Moving Average (WMA), 
        /// 3 = Double Exponential Moving Average (DEMA), 4 = Triple Exponential Moving Average (TEMA), 5 = Triangular Moving Average (TRIMA), 
        /// 6 = T3 Moving Average, 7 = Kaufman Adaptive Moving Average (KAMA), 8 = MESA Adaptive Moving Average (MAMA).</param>
        /// <param name="slowdmatype">Moving average type for the slowd moving average. By default, slowkmatype=0. 
        /// Integers 0 - 8 are accepted with the following mappings. 
        /// 0 = Simple Moving Average (SMA), 1 = Exponential Moving Average (EMA), 2 = Weighted Moving Average (WMA), 
        /// 3 = Double Exponential Moving Average (DEMA), 4 = Triple Exponential Moving Average (TEMA), 5 = Triangular Moving Average (TRIMA), 
        /// 6 = T3 Moving Average, 7 = Kaufman Adaptive Moving Average (KAMA), 8 = MESA Adaptive Moving Average (MAMA).</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getSTOCH(string folderPath, string scriptName, string day_interval = "daily", string fastkperiod = "5",
                                        string slowkperiod = "3", string slowdperiod = "3", string slowkmatype = "0", string slowdmatype = "0",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable stochDataTable = null;
                string filename = folderPath + scriptName + "_" + "STOCH_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=STOCH&symbol={}&interval=daily&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlSTOCH, scriptName, day_interval, fastkperiod, slowkperiod, slowdperiod,
                                                    slowkmatype, slowdmatype, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            stochDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return stochDataTable;
                    }

                }
                else
                {
                    //STOCH_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SlowD,SlowK

                        stochDataTable = new DataTable();

                        string[] field = record.Split(',');

                        stochDataTable.Columns.Add("Symbol", typeof(string));
                        stochDataTable.Columns.Add("Date", typeof(DateTime));
                        stochDataTable.Columns.Add("SlowD", typeof(decimal));
                        stochDataTable.Columns.Add("SlowK", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            stochDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return stochDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="seriestype">['open', 'high', 'low', *'close']</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getMACD(string folderPath, string scriptName, string day_interval = "daily", string seriestype = "close",
                                        string fastperiod = "12", string slowperiod = "26", string signalperiod = "9", bool bIsTestModeOn = true,
                                        bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable macdDataTable = null;
                string filename = folderPath + scriptName + "_" + "MACD_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=MACD&symbol={}&interval=daily&series_type=close&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlMACD, scriptName, day_interval, seriestype, fastperiod, slowperiod, signalperiod,
                                                    apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            macdDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return macdDataTable;
                    }

                }
                else
                {
                    //MACD_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,MACD,MACD_Hist,MACD_Signal

                        macdDataTable = new DataTable();

                        string[] field = record.Split(',');

                        macdDataTable.Columns.Add("Symbol", typeof(string));
                        macdDataTable.Columns.Add("Date", typeof(DateTime));
                        macdDataTable.Columns.Add("MACD", typeof(decimal));
                        macdDataTable.Columns.Add("MACD_Hist", typeof(decimal));
                        macdDataTable.Columns.Add("MACD_Signal", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            macdDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return macdDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="period">[10, *20]</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getAROON(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable aroonDataTable = null;
                string filename = folderPath + scriptName + "_" + "AROON_" + day_interval + "_" + period + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=AROON&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlAROON, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            aroonDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return aroonDataTable;
                    }

                }
                else
                {
                    //AROON_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,Aroon Down,Aroon Up

                        aroonDataTable = new DataTable();

                        string[] field = record.Split(',');

                        aroonDataTable.Columns.Add("Symbol", typeof(string));
                        aroonDataTable.Columns.Add("Date", typeof(DateTime));
                        aroonDataTable.Columns.Add("Aroon Down", typeof(decimal));
                        aroonDataTable.Columns.Add("Aroon Up", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            aroonDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return aroonDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="period">[10, *20]</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getADX(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable adxDataTable = null;
                string filename = folderPath + scriptName + "_" + "ADX_" + day_interval + "_" + period + ".csv";
                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlADX, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            adxDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return adxDataTable;
                    }

                }
                else
                {
                    //ADX_LT.BSE.csv
                    //reader = new StreamReader(folderPath + "ADX_" + scriptName + ".csv");
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,ADX

                        adxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        adxDataTable.Columns.Add("Symbol", typeof(string));
                        adxDataTable.Columns.Add("Date", typeof(DateTime));
                        adxDataTable.Columns.Add("ADX", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return adxDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static DataTable getDX(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dxDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlDX, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "DX_" + day_interval + "_" + period + ".csv", fileData);
                            dxDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return dxDataTable;
                    }

                }
                else
                {
                    //ADX_LT.BSE.csv
                    //reader = new StreamReader(folderPath + "ADX_" + scriptName + ".csv");
                    if (File.Exists(folderPath + scriptName + "_" + "DX_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "DX_" + day_interval + "_" + period + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,ADX

                        dxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        dxDataTable.Columns.Add("Symbol", typeof(string));
                        dxDataTable.Columns.Add("Date", typeof(DateTime));
                        dxDataTable.Columns.Add("DX", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return dxDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static DataTable getMinusDI(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dxDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlMinusDI, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "MINUSDI_" + day_interval + "_" + period + ".csv", fileData);
                            dxDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return dxDataTable;
                    }

                }
                else
                {
                    //ADX_LT.BSE.csv
                    //reader = new StreamReader(folderPath + "ADX_" + scriptName + ".csv");
                    if (File.Exists(folderPath + scriptName + "_" + "MINUSDI_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "MINUSDI_" + day_interval + "_" + period + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,ADX

                        dxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        dxDataTable.Columns.Add("Symbol", typeof(string));
                        dxDataTable.Columns.Add("Date", typeof(DateTime));
                        dxDataTable.Columns.Add("MINUS_DI", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return dxDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static DataTable getPlusDI(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dxDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlPlusDI, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "PLUSDI_" + day_interval + "_" + period + ".csv", fileData);
                            dxDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return dxDataTable;
                    }

                }
                else
                {
                    //ADX_LT.BSE.csv
                    //reader = new StreamReader(folderPath + "ADX_" + scriptName + ".csv");
                    if (File.Exists(folderPath + scriptName + "_" + "PLUSDI_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "PLUSDI_" + day_interval + "_" + period + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,ADX

                        dxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        dxDataTable.Columns.Add("Symbol", typeof(string));
                        dxDataTable.Columns.Add("Date", typeof(DateTime));
                        dxDataTable.Columns.Add("PLUS_DI", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return dxDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static DataTable getMinusDM(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dxDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlMinusDM, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "MINUSDM_" + day_interval + "_" + period + ".csv", fileData);
                            dxDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return dxDataTable;
                    }

                }
                else
                {
                    //ADX_LT.BSE.csv
                    //reader = new StreamReader(folderPath + "ADX_" + scriptName + ".csv");
                    if (File.Exists(folderPath + scriptName + "_" + "MINUSDM_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "MINUSDM_" + day_interval + "_" + period + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,ADX

                        dxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        dxDataTable.Columns.Add("Symbol", typeof(string));
                        dxDataTable.Columns.Add("Date", typeof(DateTime));
                        dxDataTable.Columns.Add("MINUS_DM", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return dxDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static DataTable getPlusDM(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dxDataTable = null;

                if (bIsTestModeOn == false)
                {
                    //https://www.alphavantage.co/query?function=ADX&symbol={}&interval=daily&time_period=20&apikey={}&datatype=csv
                    webservice_url = string.Format(StockApi.urlPlusDM, scriptName, day_interval, period, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + scriptName + "_" + "PLUSDM_" + day_interval + "_" + period + ".csv", fileData);
                            dxDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return dxDataTable;
                    }

                }
                else
                {
                    //ADX_LT.BSE.csv
                    //reader = new StreamReader(folderPath + "ADX_" + scriptName + ".csv");
                    if (File.Exists(folderPath + scriptName + "_" + "PLUSDM_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "PLUSDM_" + day_interval + "_" + period + ".csv");
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,ADX

                        dxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        dxDataTable.Columns.Add("Symbol", typeof(string));
                        dxDataTable.Columns.Add("Date", typeof(DateTime));
                        dxDataTable.Columns.Add("PLUS_DM", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return dxDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval">['1min', '5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="period">[10, *20]</param>
        /// <param name="seriestype">['open', 'high', 'low', *'close']</param>
        /// <param name="nbdevup">positive int (default 2)</param>
        /// <param name="nbdevdn">positive int (default 2)</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getBbands(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        string seriestype = "close", string nbdevup = "2", string nbdevdn = "2", bool bIsTestModeOn = true,
                                        bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable bbandsDataTable = null;
                string filename = folderPath + scriptName + "_" + "BBANDS_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv";
                if (bIsTestModeOn == false)
                {
                    //'https://www.alphavantage.co/query?function=BBANDS&symbol={}&interval=daily&time_period=20&series_type=close&nbdevup=2&nbdevdn=2&apikey={}&datatype=csv'
                    webservice_url = string.Format(StockApi.urlGetBBands, scriptName, day_interval, period, seriestype, nbdevup, nbdevdn, apiKey, StockApi.dataType);
                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(filename, fileData);
                            bbandsDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return bbandsDataTable;
                    }

                }
                else
                {
                    //BBANDS_LT.BSE.csv
                    if (File.Exists(filename))
                        reader = new StreamReader(filename);
                }

                //Get the response 

                //First line indicates the fields
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,Real Lower Band,Real Middle Band,Real Upper Band

                        bbandsDataTable = new DataTable();

                        string[] field = record.Split(',');

                        bbandsDataTable.Columns.Add("Symbol", typeof(string));
                        bbandsDataTable.Columns.Add("Date", typeof(DateTime));
                        bbandsDataTable.Columns.Add("Real Lower Band", typeof(decimal));
                        bbandsDataTable.Columns.Add("Real Middle Band", typeof(decimal));
                        bbandsDataTable.Columns.Add("Real Upper Band", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            bbandsDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3]
                                                                });

                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();

                        return bbandsDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 
        /// Following is the sample date from csv file: 
        /// 
        /// symbol,open,high,low,price,volume,latestDay,previousClose,change,changePercent
        /// IBM,119.6900,121.4200,119.2600,119.7000,3732768,2020-07-02,118.5400,1.1600,0.9786%
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="symbol"></param>
        /// <param name="bIsTestModeOn"></param>
        /// <returns></returns>

        public static DataTable globalQuote(string folderPath, string symbol, bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url;
                Uri url;
                WebResponse wr;
                Stream receiveStream = null;
                DataTable resultDataTable = null;
                StreamReader reader = null;
                DataRow r;

                if (bIsTestModeOn == false)
                {
                    //'https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={}&apikey={}&datatype=csv'
                    webservice_url = string.Format(StockApi.urlGlobalQuote, symbol, apiKey, StockApi.dataType);
                    url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = reader.ReadToEnd();
                        if (fileData.StartsWith("{") == false)
                        {
                            File.WriteAllText(folderPath + symbol + "global_quote" + ".csv", fileData);
                            resultDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return resultDataTable;
                    }
                }
                else
                {
                    //global_quote_HDFC.BSE.csv
                    if (File.Exists(folderPath + symbol + "global_quote" + ".csv"))
                        reader = new StreamReader(folderPath + symbol + "global_quote" + ".csv");
                }

                if (reader != null)
                {
                    string record = reader.ReadLine();

                    if (record.StartsWith("{") == false)
                    {
                        resultDataTable = new DataTable();

                        string[] field = record.Split(',');

                        foreach (string fieldname in field)
                        {
                            resultDataTable.Columns.Add(fieldname, typeof(string));
                        }

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            r = resultDataTable.NewRow();

                            r.ItemArray = field;

                            resultDataTable.Rows.Add(r);
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return resultDataTable;
                    }
                    else
                    {
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                        return null;
                    }
                }
                else
                {
                    if (receiveStream != null)
                        receiveStream.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

#endif

        /// <summary>
        /// Method used by portfolio valuatin graph
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <param name="bIsTestModeOn"></param>
        /// <returns></returns>
        public static DataTable GetValuation(string folderPath, string fileName, bool bIsTestModeOn = true, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                DataTable portfolioTable = null;
                //DataTable orderedPortfolioTable;
                DataTable tempTable;
                DataTable getDailyScriptTable;
                DataTable allDataTable = null;
                DataRow[] scriptRows;
                DataRow[] getDailyRows;
                //Get names of symbols from the file
                XmlDocument xmlPortfolio = new XmlDocument();
                XmlNode root;
                XmlNodeList scriptNodeList;
                string scriptName;
                int cumulativeQty;
                string expression;
                if (File.Exists(fileName))
                {
                    portfolioTable = getPortfolioTable(folderPath, fileName, bCurrent: false, bIsTestModeOn: bIsTestModeOn, apiKey: apiKey);
                    //orderedPortfolioTable = portfolioTable.Clone();

                    //allDataTable.Columns.Add("Symbol", typeof(string));
                    //allDataTable.Columns.Add("Date", typeof(string));
                    //allDataTable.Columns.Add("Open", typeof(decimal));
                    //allDataTable.Columns.Add("High", typeof(decimal));
                    //allDataTable.Columns.Add("Low", typeof(decimal));
                    //allDataTable.Columns.Add("Close", typeof(decimal));
                    //allDataTable.Columns.Add("Volume", typeof(int));
                    //allDataTable.Columns.Add("PurchaseDate", typeof(string));
                    //allDataTable.Columns.Add("CumulativeQuantity", typeof(int));
                    //allDataTable.Columns.Add("CostofInvestment", typeof(decimal));
                    //allDataTable.Columns.Add("ValueOnDate", typeof(decimal));

                    xmlPortfolio.Load(fileName);
                    root = xmlPortfolio.DocumentElement;
                    string searchPath = "/portfolio/script/name";
                    scriptNodeList = root.SelectNodes(searchPath);
                    foreach (XmlNode scriptNameNode in scriptNodeList)
                    {
                        scriptName = scriptNameNode.InnerText;
                        scriptRows = portfolioTable.Select("Name='" + scriptName + "'", "PurchaseDate ASC");
                        cumulativeQty = 0;
                        //getDailyScriptTable = getDaily(folderPath, scriptName, outputsize: "full", bIsTestModeOn: bIsTestModeOn, apiKey: apiKey);
                        //if (getDailyScriptTable == null)
                        //{
                        getDailyScriptTable = getDailyAlternate(folderPath, scriptName, outputsize: "full", bIsTestModeOn: false, apiKey: apiKey);
                        //}
                        if (getDailyScriptTable == null)
                            continue;
                        for (int i = 0; i < scriptRows.Length; i++)
                        {
                            scriptRows[i]["CumulativeQty"] = cumulativeQty + System.Convert.ToInt16(scriptRows[i]["PurchaseQty"]);
                            cumulativeQty = System.Convert.ToInt32(scriptRows[i]["CumulativeQty"]);
                            if ((i + 1) == scriptRows.Length) //last row
                            {
                                expression = "Date >= '" + scriptRows[i]["PurchaseDate"].ToString() + "'";
                            }
                            else
                            {
                                expression = "Date >= '" + scriptRows[i]["PurchaseDate"].ToString() + "' and Date <= '" + scriptRows[i + 1]["PurchaseDate"].ToString() + "'";
                            }
                            getDailyRows = getDailyScriptTable.Select(expression);
                            for (int j = 0; j < getDailyRows.Length; j++)
                            {
                                getDailyRows[j]["PurchaseDate"] = scriptRows[i]["PurchaseDate"];
                                getDailyRows[j]["CumulativeQuantity"] = scriptRows[i]["CumulativeQty"];
                                getDailyRows[j]["CostofInvestment"] = scriptRows[i]["CostofInvestment"];
                                getDailyRows[j]["ValueOnDate"] = System.Convert.ToDecimal(getDailyRows[j]["Close"]) * System.Convert.ToInt32(scriptRows[i]["CumulativeQty"]);
                            }
                            getDailyScriptTable.AcceptChanges();
                            portfolioTable.AcceptChanges();

                            if (getDailyRows.Length > 0)
                            {
                                tempTable = getDailyScriptTable.Clone();
                                tempTable = getDailyRows.CopyToDataTable<DataRow>();

                                if(allDataTable == null)
                                {
                                    allDataTable = getDailyScriptTable.Clone();
                                }
                                try
                                {
                                    allDataTable.Merge(tempTable, true, MissingSchemaAction.Ignore);
                                }
                                catch(Exception ex)
                                {
                                    int error = 0;
                                }
                                tempTable.Clear();
                                tempTable = null;
                            }
                            //recTable.Select(string.Format("[code] = '{0}'", someName)).ToList<DataRow>().ForEach(r => r["Color"] = colorValue);
                            //getDailyScriptTable.Select(expression).ToList<DataRow>().ForEach(r => r["PurchaseDate"] = scriptRows[i]["PurchaseDate"].ToString());
                            //getDailyScriptTable.Select(expression).ToList<DataRow>().ForEach(r => r["CumulativeQuantity"] = scriptRows[i]["CumulativeQty"].ToString());

                            //getDailyRows.ToList<DataRow>().ForEach(readDaily => readDaily["PurchaseDate"] = scriptRows[i]["PurchaseDate"].ToString());
                        }
                        //allDataTable.Merge(getDailyScriptTable);

                        //tempTable = scriptRows.CopyToDataTable();
                        //orderedPortfolioTable.Merge(tempTable);
                    }
                }
                return allDataTable;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// Method used by OpenPortfolio page by sending bCurrent = true
        /// Method used by StockApi.GetValuation by sending bCurrent = false
        /// </summary>
        /// <param name="portfolioFileName"></param>
        /// <param name="bCurrent"></param>
        /// <returns></returns>
        public static DataTable getPortfolioTable(string folderPath, string portfolioFileName, bool bCurrent = true, bool bIsTestModeOn = true, 
            string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ScriptID", typeof(string));
            dt.Columns.Add("CompanyName", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("PurchaseDate", typeof(string));
            dt.Columns.Add("PurchasePrice", typeof(decimal));
            dt.Columns.Add("PurchaseQty", typeof(int));
            dt.Columns.Add("CommissionPaid", typeof(decimal));
            dt.Columns.Add("CostofInvestment", typeof(decimal));
            dt.Columns.Add("CumulativeQty", typeof(int));
            dt.Columns.Add("exch", typeof(string));
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("exchDisp", typeof(string));
            dt.Columns.Add("typeDisp", typeof(string));

            if (bCurrent)
            {
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("CurrentValue", typeof(decimal));
            }


            //XDocument doc = XDocument.Load(Server.MapPath(".\\data\\demo_portfolio.xml"));
            XDocument doc = XDocument.Load(portfolioFileName);
            DataTable quoteTable;
            string symbol = "", companyname="", exch="", type="", exchDisp="", typeDisp="";
            string price = "0.00";
            string currValue = "";
            foreach (XElement script in doc.Descendants("script"))
            {
                symbol = (string)script.Element("name");

                companyname = ""; exch = ""; type = ""; exchDisp = ""; typeDisp = "";
                if ((script.Element("name")).HasAttributes)
                {
                    if ((script.Element("name")).Attribute("companyname") != null)
                    {
                        companyname = (script.Element("name")).Attribute("companyname").Value;
                    }
                    if ((script.Element("name")).Attribute("exch") != null)
                    {
                        exch = (script.Element("name")).Attribute("exch").Value;
                    }
                    if ((script.Element("name")).Attribute("type") != null)
                    {
                        type = (script.Element("name")).Attribute("type").Value;
                    }
                    if ((script.Element("name")).Attribute("exchDisp") != null)
                    {
                        exchDisp = (script.Element("name")).Attribute("exchDisp").Value;
                    }
                    if ((script.Element("name")).Attribute("typeDisp") != null)
                    {
                        typeDisp = (script.Element("name")).Attribute("typeDisp").Value;
                    }
                }

                if (bCurrent)
                {
                    price = "0.00";
                    //quoteTable = StockApi.globalQuote(folderPath, symbol, bIsTestModeOn: bIsTestModeOn, apiKey: apiKey);

                    //quoteTable = StockApi.globalQuoteAlternate(folderPath, symbol, bIsTestModeOn: bIsTestModeOn, apiKey: apiKey);
                    //will try to ALWAYS get quote from market
                    quoteTable = StockApi.globalQuoteAlternate(folderPath, symbol, bIsTestModeOn: false, apiKey: apiKey);
                    if (quoteTable != null)
                    {
                        price = quoteTable.Rows[0]["price"].ToString();
                    }
                }
                foreach (XElement row in script.Elements("row"))
                {
                    if (bCurrent)
                    {
                        currValue = System.Convert.ToString((int)row.Element("PurchaseQty") * System.Convert.ToDecimal(price));
                        dt.Rows.Add(new object[] {
                            symbol,
                            companyname,
                            symbol,
                            //(DateTime)DateTime.ParseExact(row.Element("PurchaseDate").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ((DateTime)(row.Element("PurchaseDate"))).ToString("yyyy-MM-dd"),
                            (decimal)row.Element("PurchasePrice"),
                            (int)row.Element("PurchaseQty"),
                            (decimal)row.Element("CommissionPaid"),
                            (decimal)row.Element("CostofInvestment"),
                            0,
                            exch, type,exchDisp,typeDisp,
                            (decimal)System.Convert.ToDecimal(price),
                            (decimal)System.Convert.ToDecimal(currValue)
                        });
                    }
                    else
                    {
                        dt.Rows.Add(new object[] {
                            symbol,
                            companyname,
                            symbol,
                            //(DateTime)DateTime.ParseExact(row.Element("PurchaseDate").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ((DateTime)(row.Element("PurchaseDate"))).ToString("yyyy-MM-dd"),
                            (decimal)row.Element("PurchasePrice"),
                            (int)row.Element("PurchaseQty"),
                            (decimal)row.Element("CommissionPaid"),
                            (decimal)row.Element("CostofInvestment"),
                            0, exch, type,exchDisp,typeDisp
                        });
                    }
                }
            }
            return dt;
        }

        static public string[] getScriptFromPortfolioFile(string portfolioFileName)
        {
            try
            {
                XDocument doc = XDocument.Load(portfolioFileName);
                string symbol = "";
                int i = 0;
                string[] scriptList = new string[doc.Descendants("script").Count()];
                foreach (XElement script in doc.Descendants("script"))
                {
                    symbol = (string)script.Element("name");
                    if (scriptList.Contains(symbol) == false)
                    {
                        scriptList[i++] = symbol;
                    }
                }
                return scriptList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        static public bool updateNode(string filename, string symbol, string price, string date, string qty, string commission, string cost,
                                    string companyname, string exch, string type, string exchDisp, string typeDisp,
                                    string newsymbol, string newprice, string newdate, string newqty, string newcommission, string newcost,
                                    string newcompanyname, string newexch, string newtype, string newexchDisp, string newtypeDisp)
        {
            bool breturn = false;

            try
            {
                breturn = StockApi.deleteNode(filename, symbol, price, date, qty, commission, cost);
                if (breturn)
                {
                    breturn = StockApi.insertNode(filename, newsymbol, newprice, newdate, newqty, newcommission, newcost, newcompanyname, newexch, newtype, newexchDisp, newtypeDisp);
                }

                /*
                //if (!File.Exists(filename))
                //{
                //    breturn = false;
                //}
                //else
                //{
                //    xmlPortfolio = new XmlDocument();
                //    xmlPortfolio.Load(filename);
                //    root = xmlPortfolio.DocumentElement;
                //    //    /portfolio/script[name='AAPL'] gives
                //    ///<script>
                //    //  < name > AAPL </ name >
                //    //  < row >
                //    //    < PurchaseDate > 2019 - 06 - 10 </ PurchaseDate >
                //    //    < PurchasePrice > 192.5800 </ PurchasePrice >
                //    //    < PurchaseQty > 3 </ PurchaseQty >
                //    //    < CommissionPaid > 0.00 </ CommissionPaid >
                //    //    < CostofInvestment > 577.74 </ CostofInvestment >
                //    //  </ row >
                //    //  < row >
                //    //    < PurchaseDate > 2020 - 05 - 06 </ PurchaseDate >
                //    //    < PurchasePrice > 300.6300 </ PurchasePrice >
                //    //    < PurchaseQty > 5 </ PurchaseQty >
                //    //    < CommissionPaid > 0.00 </ CommissionPaid >
                //    //    < CostofInvestment > 1503.15 </ CostofInvestment >
                //    //  </ row >
                //    //</ script > 

                //    searchPath = $"{"script[name='"}{symbol}{"']"}";
                //    scriptNode = root.SelectSingleNode(searchPath);
                //    if (scriptNode == null)
                //    {
                //        breturn = false;
                //    }
                //    else
                //    {
                //        //we found matching symbol entry in file. Now search row node matching supplied values
                //        //
                //        //  /script/row[PurchaseDate='2019-06-10' and PurchasePrice='192.5800'] will give matching row
                //        ///<row>
                //        //  < PurchaseDate > 2019 - 06 - 10 </ PurchaseDate >
                //        //  < PurchasePrice > 192.5800 </ PurchasePrice >
                //        //  < PurchaseQty > 3 </ PurchaseQty >
                //        //  < CommissionPaid > 0.00 </ CommissionPaid >
                //        //  < CostofInvestment > 577.74 </ CostofInvestment >
                //        //</ row > 
                //        searchPath = $"{"row[PurchaseDate='"}{date}{"' and PurchasePrice='"}{price}{"' and PurchaseQty='"}{qty}" +
                //                    $"{"' and CommissionPaid='"}{commission}{"' and CostofInvestment='"}{cost}{"'"}";

                //        txnNode = scriptNode.SelectSingleNode(searchPath);
                //        if (txnNode == null)
                //        {
                //            breturn = false;
                //        }
                //        else
                //        {
                //            scriptNode.RemoveChild(txnNode);
                //            //xmlPortfolio.Save(filename);

                //            breturn = StockApi.insertNode(filename, newsymbol, newprice, newdate, newqty, newcommission, newcost, newcompanyname, xmlPortfolio);

                //            //txnNode["PurchaseDate"].InnerText = newdate;
                //            //txnNode["PurchasePrice"].InnerText = newprice;
                //            //txnNode["PurchaseQty"].InnerText = newqty;
                //            //txnNode["CommissionPaid"].InnerText = newcommission;
                //            //txnNode["CostofInvestment"].InnerText = newcost;

                //            //scriptNode["name"].InnerText = newsymbol;
                //            //scriptNode["name"].Attributes["companyname"].Value = newcompanyname;
                //            //xmlPortfolio.Save(filename);
                //        }
                //    }
                //}*/
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        static public bool insertNode(string filename, string symbol, string price, string date, string qty, string commission, string cost, 
            string companyname = "", string exch="", string type="", string exchDisp="", string typeDisp="")
        {
            //string filename = "E:\\MSFT_SampleWork\\PortfolioAnalytics\\portfolio\\demo.xml";
            bool breturn = false;
            XmlDocument xmlPortfolio;
            XmlNode root;
            XmlNode scriptNode;
            try
            {
                xmlPortfolio = new XmlDocument();
                if (!File.Exists(filename))
                {
                    (File.Create(filename)).Close();
                    root = xmlPortfolio.CreateElement("portfolio");
                    xmlPortfolio.AppendChild(root);
                }
                else
                {
                    xmlPortfolio.Load(filename);
                    root = xmlPortfolio.DocumentElement;
                }

                string searchPath = $"{"script[name='"}{symbol}{"']"}";
                scriptNode = root.SelectSingleNode(searchPath);
                if (scriptNode != null)
                {
                    XmlElement elemRow = xmlPortfolio.CreateElement("row");

                    XmlElement elemDate = xmlPortfolio.CreateElement("PurchaseDate");
                    elemDate.InnerText = date;
                    elemRow.AppendChild(elemDate);

                    XmlElement elemPrice = xmlPortfolio.CreateElement("PurchasePrice");
                    elemPrice.InnerText = price;
                    elemRow.AppendChild(elemPrice);

                    XmlElement elemQty = xmlPortfolio.CreateElement("PurchaseQty");
                    elemQty.InnerText = qty;
                    elemRow.AppendChild(elemQty);

                    XmlElement elemCommission = xmlPortfolio.CreateElement("CommissionPaid");
                    elemCommission.InnerText = commission;
                    elemRow.AppendChild(elemCommission);

                    XmlElement elemCost = xmlPortfolio.CreateElement("CostofInvestment");
                    elemCost.InnerText = cost;
                    elemRow.AppendChild(elemCost);

                    scriptNode.AppendChild(elemRow);
                    XmlAttribute xmlattribCompanyName;
                    if (scriptNode["name"].HasAttribute("companyname") == true)
                    {
                        scriptNode["name"].SetAttribute("companyname", companyname);
                    }
                    else
                    {
                        xmlattribCompanyName = xmlPortfolio.CreateAttribute("companyname");
                        xmlattribCompanyName.Value = companyname;

                        scriptNode["name"].Attributes.Append(xmlattribCompanyName);
                    }
                    if (scriptNode["name"].HasAttribute("exch") == true)
                    {
                        scriptNode["name"].SetAttribute("exch", exch);
                    }
                    else
                    {
                        xmlattribCompanyName = xmlPortfolio.CreateAttribute("exch");
                        xmlattribCompanyName.Value = exch;

                        scriptNode["name"].Attributes.Append(xmlattribCompanyName);
                    }
                    if (scriptNode["name"].HasAttribute("type") == true)
                    {
                        scriptNode["name"].SetAttribute("type", type);
                    }
                    else
                    {
                        xmlattribCompanyName = xmlPortfolio.CreateAttribute("type");
                        xmlattribCompanyName.Value = type;

                        scriptNode["name"].Attributes.Append(xmlattribCompanyName);
                    }
                    if (scriptNode["name"].HasAttribute("exchDisp") == true)
                    {
                        scriptNode["name"].SetAttribute("exchDisp", exchDisp);
                    }
                    else
                    {
                        xmlattribCompanyName = xmlPortfolio.CreateAttribute("exchDisp");
                        xmlattribCompanyName.Value = exchDisp;

                        scriptNode["name"].Attributes.Append(xmlattribCompanyName);
                    }
                    if (scriptNode["name"].HasAttribute("typeDisp") == true)
                    {
                        scriptNode["name"].SetAttribute("typeDisp", typeDisp);
                    }
                    else
                    {
                        xmlattribCompanyName = xmlPortfolio.CreateAttribute("typeDisp");
                        xmlattribCompanyName.Value = typeDisp;

                        scriptNode["name"].Attributes.Append(xmlattribCompanyName);
                    }
                }
                else
                {
                    XmlElement elemRow = xmlPortfolio.CreateElement("row");

                    XmlElement elemDate = xmlPortfolio.CreateElement("PurchaseDate");
                    elemDate.InnerText = date;
                    elemRow.AppendChild(elemDate);

                    XmlElement elemPrice = xmlPortfolio.CreateElement("PurchasePrice");
                    elemPrice.InnerText = price;
                    elemRow.AppendChild(elemPrice);

                    XmlElement elemQty = xmlPortfolio.CreateElement("PurchaseQty");
                    elemQty.InnerText = qty;
                    elemRow.AppendChild(elemQty);

                    XmlElement elemCommission = xmlPortfolio.CreateElement("CommissionPaid");
                    elemCommission.InnerText = commission;
                    elemRow.AppendChild(elemCommission);

                    XmlElement elemCost = xmlPortfolio.CreateElement("CostofInvestment");
                    elemCost.InnerText = cost;
                    elemRow.AppendChild(elemCost);

                    XmlElement elemName = xmlPortfolio.CreateElement("name");
                    elemName.InnerText = symbol;

                    XmlAttribute xmlattribCompanyName = xmlPortfolio.CreateAttribute("companyname");
                    xmlattribCompanyName.Value = companyname;

                    elemName.Attributes.Append(xmlattribCompanyName);

                    xmlattribCompanyName = xmlPortfolio.CreateAttribute("exch");
                    xmlattribCompanyName.Value = exch;

                    elemName.Attributes.Append(xmlattribCompanyName);

                    xmlattribCompanyName = xmlPortfolio.CreateAttribute("type");
                    xmlattribCompanyName.Value = type;

                    elemName.Attributes.Append(xmlattribCompanyName);

                    xmlattribCompanyName = xmlPortfolio.CreateAttribute("exchDisp");
                    xmlattribCompanyName.Value = exchDisp;

                    elemName.Attributes.Append(xmlattribCompanyName);

                    xmlattribCompanyName = xmlPortfolio.CreateAttribute("typeDisp");
                    xmlattribCompanyName.Value = typeDisp;

                    elemName.Attributes.Append(xmlattribCompanyName);

                    scriptNode = xmlPortfolio.CreateElement("script");
                    scriptNode.AppendChild(elemName);
                    scriptNode.AppendChild(elemRow);

                    root.AppendChild(scriptNode);
                }
                xmlPortfolio.Save(filename);
                breturn = true;
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        static public void createNewPortfolio(string filename)
        {
            //string filename = "E:\\MSFT_SampleWork\\PortfolioAnalytics\\portfolio\\demo.xml";

            XmlDocument xmlPortfolio = new XmlDocument();
            XmlNode root;
            if (!File.Exists(filename))
            {
                (File.Create(filename)).Close();
                root = xmlPortfolio.CreateElement("portfolio");
                xmlPortfolio.AppendChild(root);
            }
            xmlPortfolio.Save(filename);

        }

        static public bool deleteNode(string filename, string symbol, string price, string date, string qty, string commission, string cost)
        {
            //string filename = "E:\\MSFT_SampleWork\\PortfolioAnalytics\\portfolio\\demo.xml";
            bool breturn = false;

            XmlDocument xmlPortfolio = new XmlDocument();
            XmlNode root;
            XmlNode scriptNode;

            try
            {
                if (File.Exists(filename))
                {
                    xmlPortfolio.Load(filename);
                    root = xmlPortfolio.DocumentElement;

                    string searchPath = $"{"script[name='"}{symbol}{"']"}";
                    scriptNode = root.SelectSingleNode(searchPath);
                    if (scriptNode != null)
                    {
                        XmlNodeList rowNodesList = scriptNode.SelectNodes("row");
                        XmlNode nodeDelete = null;
                        foreach (XmlNode rowChild in rowNodesList)
                        {
                            if ((rowChild["PurchasePrice"].InnerText.Equals(price)) && (rowChild["PurchaseDate"].InnerText.Equals(date)) &&
                                (rowChild["PurchaseQty"].InnerText.Equals(qty)) && (rowChild["CommissionPaid"].InnerText.Equals(commission)) &&
                                (rowChild["CostofInvestment"].InnerText.Equals(cost)))
                            {
                                nodeDelete = rowChild;
                                break;
                            }
                        }

                        if (nodeDelete != null)
                        {
                            if (rowNodesList.Count == 1)
                            {
                                root.RemoveChild(scriptNode);
                            }
                            else
                            {
                                scriptNode.RemoveChild(nodeDelete);
                            }
                            xmlPortfolio.Save(filename);
                            breturn = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }

        static public void createKey(string filename, string key)
        {
            File.WriteAllBytes(filename, Encoding.UTF8.GetBytes(key));
        }
        static public string readKey(string filename)
        {
            string key = null;
            try
            {
                if (File.Exists(filename))
                {
                    key = Encoding.UTF8.GetString(File.ReadAllBytes(filename));
                }
            }
            catch (Exception ex)
            {
                key = null;
            }
            return key;
        }
        static public void deleteKey(string filename, string key)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

        }

        static public DataTable readCSV(StreamReader reader)
        {
            DataTable returnDT = null;
            try
            {
                if (reader != null)
                {
                    string record = reader.ReadLine();

                    //time,Real Lower Band,Real Middle Band,Real Upper Band

                    returnDT = new DataTable();

                    string[] field = record.Split(',');

                    for (int i = 0; i < field.Length; i++)
                    {
                        returnDT.Columns.Add(field[i], typeof(string));
                    }
                    while (!reader.EndOfStream)
                    {
                        record = reader.ReadLine();
                        field = record.Split(',');
                        returnDT.Rows.Add(field);
                    }
                }
            }
            catch (Exception ex)
            {
                if (returnDT != null)
                {
                    returnDT.Clear();
                    returnDT.Dispose();
                }
                returnDT = null;
            }
            return returnDT;
        }

        static public DataTable readColumnsFromCSVTable(DataTable inputDT)
        {
            DataTable returnDT = null;
            try
            {
                returnDT = new DataTable();
                returnDT.Columns.Add("SourceCol", typeof(string));
                returnDT.Columns.Add("TargetCol", typeof(string));
                foreach (DataColumn col in inputDT.Columns)
                {
                    returnDT.Rows.Add(new object[] {
                            col.ColumnName,
                            ""
                    });
                }

            }
            catch (Exception ex)
            {
                if (returnDT != null)
                {
                    returnDT.Clear();
                    returnDT.Dispose();
                }
                returnDT = null;
            }
            return returnDT;
        }

        static public bool convertTableToPortfolio(string filename, DataTable csvDT, DataTable colDT, string exchangeCode, string apiKey = "UV6KQA6735QZKBTV")
        {
            bool breturn = false;
            string symbol, price, date, qty, commission, cost, companyname;
            string sourceCol;
            try
            {

                foreach (DataRow csvDTrow in csvDT.Rows)
                {
                    symbol = ""; price = "0.00"; date = ""; qty = "0"; commission = "0.00"; cost = "0.00"; companyname = "";

                    foreach (DataRow colDTrow in colDT.Rows)
                    {
                        //
                        switch (colDTrow[1].ToString())
                        {
                            case "companyname":
                                sourceCol = colDTrow[0].ToString();
                                //get the data from csvDT
                                if (csvDTrow[sourceCol].ToString().Length > 0)
                                {
                                    companyname = csvDTrow[sourceCol].ToString();
                                    symbol = companyname + exchangeCode;
                                    //DataTable srchDT = StockApi.symbolSearch(companyname, apiKey: apiKey);
                                    //if ((srchDT != null) && (srchDT.Rows.Count > 0))
                                    //{
                                    //    symbol = srchDT.Rows[0]["Symbol"].ToString();
                                    //}
                                }
                                break;
                            case "PurchaseDate":
                                sourceCol = colDTrow[0].ToString();
                                if (csvDTrow[sourceCol].ToString().Length > 0)
                                {
                                    date = DateTime.ParseExact(csvDTrow[sourceCol].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                                    //date = (System.Convert.ToDateTime(csvDTrow[sourceCol].ToString())).ToString("yyyy-MM-dd"); ;
                                }
                                break;
                            case "PurchasePrice":
                                sourceCol = colDTrow[0].ToString();
                                if (csvDTrow[sourceCol].ToString().Length > 0)
                                {
                                    //returnString.Append(string.Format("{0:0.0000}", vwap));
                                    price = string.Format("{0:0.0000}", System.Convert.ToDouble(csvDTrow[sourceCol].ToString()));
                                }
                                break;
                            case "PurchaseQty":
                                sourceCol = colDTrow[0].ToString();
                                if (csvDTrow[sourceCol].ToString().Length > 0)
                                {
                                    qty = string.Format("{0:0}", System.Convert.ToInt64(csvDTrow[sourceCol].ToString()));
                                }
                                break;
                            case "CommissionPaid":
                                sourceCol = colDTrow[0].ToString();
                                if (csvDTrow[sourceCol].ToString().Length > 0)
                                {
                                    commission = string.Format("{0:0.0000}", System.Convert.ToDouble(csvDTrow[sourceCol].ToString()));
                                }
                                break;
                            case "CostofInvestment":
                                sourceCol = colDTrow[0].ToString();
                                if (csvDTrow[sourceCol].ToString().Length > 0)
                                {
                                    cost = string.Format("{0:0.0000}", System.Convert.ToDouble(csvDTrow[sourceCol].ToString()));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    StockApi.insertNode(filename, symbol, price, date, qty, commission, cost, companyname);
                }
                breturn = true;
            }
            catch (Exception ex)
            {
                breturn = false;
            }
            return breturn;
        }


#region yahoo_finance_api

#region helper methods

        public static string findTimeZoneId(string zoneId)
        {
            string returnTimeZoneId = "";
            switch (zoneId)
            {
                case "IST":
                    returnTimeZoneId = "India Standard Time";
                    break;
                default:
                    returnTimeZoneId = "India Standard Time";
                    break;
            }
            return returnTimeZoneId;
        }
        public static DateTime convertUnixEpochToLocalDateTime(long dateEpoch, string zoneId)
        {
            DateTime localDateTime;

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(dateEpoch);
            string timeZoneId = StockApi.findTimeZoneId(zoneId);
            TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, currentTimeZone);

            return localDateTime;
        }
        /// <summary>
        /// Checks if file write time equals today. If yes then returns true else returns false
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>
        /// true : if file write time = today
        /// false : if file write time != today
        /// </returns>
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

        /// <summary>
        /// Given a list of values the method will return Standard Deviation
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public static double StandardDeviation(List<double> valueList)
        {
            double M = 0.0;
            double S = 0.0;
            int k = 1;
            foreach (double value in valueList)
            {
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }
            return Math.Sqrt(S / (k - 2));
        }

        public static double CalculateAroonUp(int rownum, int period, DataTable dailyTable)
        {
            var maxIndex = FindMaxAroonIndex(rownum - period, rownum, dailyTable);

            var up = CalcAroon(rownum - maxIndex, period);

            return Math.Round(up, 4);
        }

        public static double CalculateAroonDown(int rownum, int period, DataTable dailyTable)
        {
            var minIndex = FindMinAroonIndex(rownum - period, rownum, dailyTable);

            var down = CalcAroon(rownum - minIndex, period);

            return Math.Round(down, 4);
        }

        public static double CalcAroon(int numOfDays, int period)
        {
            var result = ((period - numOfDays)) * ((double)100 / period);
            return result;
        }

        public static int FindMinAroonIndex(int startIndex, int endIndex, DataTable dailyTable)
        {
            var min = double.MaxValue;
            var index = startIndex;
            for (var i = startIndex; i <= endIndex; i++)
            {
                if (min < System.Convert.ToDouble(dailyTable.Rows[i]["Low"]))
                    continue;

                min = System.Convert.ToDouble(dailyTable.Rows[i]["Low"]);
                index = i;
            }
            return index;
        }

        public static int FindMaxAroonIndex(int startIndex, int endIndex, DataTable dailyTable)
        {
            var max = double.MinValue;
            var index = startIndex;
            for (var i = startIndex; i <= endIndex; i++)
            {
                if (max > System.Convert.ToDouble(dailyTable.Rows[i]["High"]))
                    continue;

                max = System.Convert.ToDouble(dailyTable.Rows[i]["High"]);
                index = i;
            }
            return index;
        }

        public static double FindTR1(int rownum, DataTable dailyTable)
        {
            //MAX(Current High- Current Low,ABS(Current High- Previous Close),ABS(Current Low - Previous Close))
            double diffHighLow = System.Convert.ToDouble(dailyTable.Rows[rownum]["High"]) - System.Convert.ToDouble(dailyTable.Rows[rownum]["Low"]);
            double diffCurrHighPrevClose = Math.Abs(System.Convert.ToDouble(dailyTable.Rows[rownum]["High"]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["Close"]));
            double diffCurrLowPrevClose = Math.Abs(System.Convert.ToDouble(dailyTable.Rows[rownum]["Low"]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["Close"]));

            double maxTR = Math.Max(diffHighLow, Math.Max(diffCurrHighPrevClose, diffCurrLowPrevClose));

            return maxTR;
        }

        public static double FindPositiveDM1(int rownum, DataTable dailyTable)
        {
            //IF((Current High- Previous High)>(Previous Low - Current Low)
            //    MAX((Current High-Previous High),0)
            //ELSE 
            //    0
            double diffCurrHighPrevHigh = System.Convert.ToDouble(dailyTable.Rows[rownum]["High"]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["High"]);
            double diffPrevLowCurrLow = System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["Low"]) - System.Convert.ToDouble(dailyTable.Rows[rownum]["Low"]);
            double positiveDM1 = 0.00;

            if (diffCurrHighPrevHigh > diffPrevLowCurrLow)
            {
                positiveDM1 = Math.Max(diffCurrHighPrevHigh, 0);
            }

            return positiveDM1;
        }

        public static double FindNegativeDM1(int rownum, DataTable dailyTable)
        {
            //IF((Previous Low - Current Low) > (Current High- Previous High))
            //    MAX((Previous Low - Current Low),0)
            //ELSE 
            //    0
            double diffCurrHighPrevHigh = System.Convert.ToDouble(dailyTable.Rows[rownum]["High"]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["High"]);
            double diffPrevLowCurrLow = System.Convert.ToDouble(dailyTable.Rows[rownum - 1]["Low"]) - System.Convert.ToDouble(dailyTable.Rows[rownum]["Low"]);
            double negativeDM1 = 0.00;

            if (diffPrevLowCurrLow > diffCurrHighPrevHigh)
            {
                negativeDM1 = Math.Max(diffPrevLowCurrLow, 0);
            }

            return negativeDM1;
        }

        /// <summary>
        /// This method shold be called only for rows from "period" onwords
        /// </summary>
        /// <param name="rownum"></param>
        /// <param name="period"></param>
        /// <param name="dailyTable"></param>
        /// <param name="TR1"></param>
        /// <param name="TRPeriod"></param>
        /// <returns></returns>
        public static double FindTR_Period(int rownum, int period, List<double> TR1, List<double> TRPeriod)
        {
            double valueTR = 0.00;
            if (rownum == period)
            {
                //SUM of TR1[1] to TR1[14]
                valueTR = TR1.GetRange(0, period).Sum();
            }
            else if (rownum > period)
            {
                //I17-(I17/14)+F18
                //TRPeriod[rownum - 1] - (TRPeriod[rownum - 1]/period) + TR1[rownum]
                valueTR = TRPeriod[rownum - period - 1] - (TRPeriod[rownum - period - 1] / period) + TR1[rownum - 1];
            }
            return valueTR;
        }

        public static double FindPositveDM_Period(int rownum, int period, List<double> positiveDM1, List<double> positiveDMPeriod)
        {
            double valueDM = 0.00;
            if (rownum == period)
            {
                //SUM of TR1[1] to TR1[14]
                valueDM = positiveDM1.GetRange(0, period).Sum();
            }
            else if (rownum > period)
            {
                //I17-(I17/14)+F18
                //TRPeriod[rownum - 1] - (TRPeriod[rownum - 1]/period) + TR1[rownum]
                valueDM = positiveDMPeriod[rownum - period - 1] - (positiveDMPeriod[rownum - period - 1] / period) + positiveDM1[rownum - 1];
            }
            return Math.Round(valueDM, 4);
        }

        public static double FindNegativeDM_Period(int rownum, int period, List<double> negativeDM1, List<double> negativeDMPeriod)
        {
            double valueDM = 0.00;
            if (rownum == period)
            {
                //SUM of TR1[1] to TR1[14]
                valueDM = negativeDM1.GetRange(0, period).Sum();
            }
            else if (rownum > period)
            {
                //I17-(I17/14)+F18
                //TRPeriod[rownum - 1] - (TRPeriod[rownum - 1]/period) + TR1[rownum]
                valueDM = negativeDMPeriod[rownum - period - 1] - (negativeDMPeriod[rownum - period - 1] / period) + negativeDM1[rownum - 1];
            }
            return Math.Round(valueDM, 4);
        }

        public static double FindPositveDI_Period(int rownum, int period, List<double> TRPeriod, List<double> positiveDMPeriod)
        {
            double valueDI;
            //I17-(I17/14)+F18
            //TRPeriod[rownum - 1] - (TRPeriod[rownum - 1]/period) + TR1[rownum]
            valueDI = (100 * ((positiveDMPeriod[rownum - period]) / (TRPeriod[rownum - period])));
            return Math.Round(valueDI, 4);
        }

        public static double FindNegativeDI_Period(int rownum, int period, List<double> TRPeriod, List<double> negativeDMPeriod)
        {
            double valueDI;
            //I17-(I17/14)+F18
            //TRPeriod[rownum - 1] - (TRPeriod[rownum - 1]/period) + TR1[rownum]
            valueDI = (100 * ((negativeDMPeriod[rownum - period]) / (TRPeriod[rownum - period])));
            return Math.Round(valueDI, 4);
        }

        public static double FindDX(int rownum, int period, List<double> positiveDI, List<double> negativeDI)
        {
            double valueDX;
            double diffDI, sumDI;
            diffDI = Math.Abs(positiveDI[rownum - period] - negativeDI[rownum - period]);
            sumDI = positiveDI[rownum - period] + negativeDI[rownum - period];
            valueDX = 100 * (diffDI / sumDI);
            return Math.Round(valueDX, 4);
        }

        public static double FindADX(int rownum, int period, List<double> listDX, List<double> listADX)
        {
            double valueADX = 0.00;

            if (rownum == ((period * 2) - 1))
            {
                valueADX = listDX.GetRange(0, period).Average();
            }
            else if (rownum > ((period * 2) - 1))
            {
                valueADX = ((listADX[rownum - (period * 2)] * (period - 1)) + listDX[rownum - period]) / period;
            }

            return Math.Round(valueADX, 4);
        }

        public static double FindEMA(int rownum, int period, string seriestype, DataTable dailyTable, double emaPrev)
        {
            double multiplier = 2 / ((double)period + 1);
            double ema = 0.00;
            double sumoffirstperiod = 0.00;
            if ((rownum + 1) == period)
            {
                for (int i = 0; i <= rownum; i++)
                {
                    sumoffirstperiod += System.Convert.ToDouble(dailyTable.Rows[i][seriestype]);
                }
                if (sumoffirstperiod > 0)
                {
                    ema = sumoffirstperiod / period;
                }
            }
            else
            {
                ema = ((System.Convert.ToDouble(dailyTable.Rows[rownum][seriestype]) - emaPrev) * multiplier) + emaPrev;
            }
            return Math.Round(ema, 4);
        }

        public static double FindSignal(int rownum, int signalperiod, List<double> listMACD, double signalPrev)
        {
            double multiplier = (2 / ((double)signalperiod + 1));
            double signal = 0.00;
            if (rownum == (signalperiod - 1))
            {
                signal = (listMACD.GetRange(0, signalperiod)).Average();
            }
            else
            {
                signal = ((listMACD.Last() - signalPrev) * multiplier) + signalPrev;
            }
            return Math.Round(signal, 4);
        }

        public static double FindHighestHigh(List<double> listHigh, int start, int count)
        {
            double highestHigh = (listHigh.GetRange(start, count)).Max();
            return highestHigh;
        }
        public static double FindLowestLow(List<double> listLow, int start, int count)
        {
            double lowestLow = (listLow.GetRange(start, count)).Min();
            return lowestLow;
        }

        public static double FindSlowK(List<double> listClose, List<double> listHighestHigh, List<double> listLowestLow)
        {
            double slowK = ((listClose.Last() - listLowestLow.Last()) / (listHighestHigh.Last() - listLowestLow.Last())) * 100;
            return Math.Round(slowK, 4);
        }

        public static double FindSlowD(List<double> listSlowK, int start, int count)
        {
            double slowD = listSlowK.GetRange(start, count).Average();
            return Math.Round(slowD, 4);
        }
#endregion helper methods

#region >> Methods to Get data from today's downloaded file
        static public DataTable getDailyAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable dailyDataTable = null;

            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        dailyDataTable = new DataTable();
                        string[] field;
                        //DataTable dt = new DataTable();
                        dailyDataTable.Columns.Add("Symbol", typeof(string));
                        dailyDataTable.Columns.Add("Date", typeof(DateTime));
                        dailyDataTable.Columns.Add("Open", typeof(decimal));
                        dailyDataTable.Columns.Add("High", typeof(decimal));
                        dailyDataTable.Columns.Add("Low", typeof(decimal));
                        dailyDataTable.Columns.Add("Close", typeof(decimal));
                        dailyDataTable.Columns.Add("Volume", typeof(int));
                        dailyDataTable.Columns.Add("PurchaseDate", typeof(string));
                        dailyDataTable.Columns.Add("CumulativeQuantity", typeof(int));
                        dailyDataTable.Columns.Add("CostofInvestment", typeof(decimal));
                        dailyDataTable.Columns.Add("ValueOnDate", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dailyDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3],
                                                                    field[4],
                                                                    field[5]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dailyDataTable != null)
                {
                    dailyDataTable.Clear();
                    dailyDataTable.Dispose();
                }
                dailyDataTable = null;
            }
            return dailyDataTable;
        }

        static public DataTable getIntraAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable intraDataTable = null;

            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        intraDataTable = new DataTable();
                        string[] field;
                        //DataTable dt = new DataTable();
                        intraDataTable.Columns.Add("Symbol", typeof(string));
                        intraDataTable.Columns.Add("Date", typeof(DateTime));
                        intraDataTable.Columns.Add("Open", typeof(decimal));
                        intraDataTable.Columns.Add("High", typeof(decimal));
                        intraDataTable.Columns.Add("Low", typeof(decimal));
                        intraDataTable.Columns.Add("Close", typeof(decimal));
                        intraDataTable.Columns.Add("Volume", typeof(int));
                        intraDataTable.Columns.Add("PurchaseDate", typeof(string));
                        intraDataTable.Columns.Add("CumulativeQuantity", typeof(int));
                        intraDataTable.Columns.Add("CostofInvestment", typeof(decimal));
                        intraDataTable.Columns.Add("ValueOnDate", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            intraDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3],
                                                                    field[4],
                                                                    field[5]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (intraDataTable != null)
                {
                    intraDataTable.Clear();
                    intraDataTable.Dispose();
                }
                intraDataTable = null;
            }
            return intraDataTable;
        }

        static public DataTable getVWAPAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable vwapDataTable = null;

            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,VWAP
                        vwapDataTable = new DataTable();
                        string[] field = record.Split(',');

                        vwapDataTable.Columns.Add("Symbol", typeof(string));
                        vwapDataTable.Columns.Add("Date", typeof(DateTime));
                        vwapDataTable.Columns.Add("VWAP", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            vwapDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    //System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    System.Convert.ToDateTime(field[0]),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (vwapDataTable != null)
                {
                    vwapDataTable.Clear();
                    vwapDataTable.Dispose();
                }
                vwapDataTable = null;
            }
            return vwapDataTable;
        }

        static public DataTable getSMAAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable smaDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SMA

                        smaDataTable = new DataTable();

                        string[] field = record.Split(',');

                        smaDataTable.Columns.Add("Symbol", typeof(string));
                        smaDataTable.Columns.Add("Date", typeof(DateTime));
                        smaDataTable.Columns.Add("SMA", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            smaDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();

                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (smaDataTable != null)
                {
                    smaDataTable.Clear();
                    smaDataTable.Dispose();
                }
                smaDataTable = null;
            }
            return smaDataTable;
        }

        static public DataTable getEMAAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable emaDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SMA

                        emaDataTable = new DataTable();

                        string[] field = record.Split(',');

                        emaDataTable.Columns.Add("Symbol", typeof(string));
                        emaDataTable.Columns.Add("Date", typeof(DateTime));
                        emaDataTable.Columns.Add("EMA", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            emaDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();

                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (emaDataTable != null)
                {
                    emaDataTable.Clear();
                    emaDataTable.Dispose();
                }
                emaDataTable = null;
            }
            return emaDataTable;
        }

        static public DataTable getRSIAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable rsiDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,RSI

                        rsiDataTable = new DataTable();

                        string[] field = record.Split(',');

                        rsiDataTable.Columns.Add("Symbol", typeof(string));
                        rsiDataTable.Columns.Add("Date", typeof(DateTime));
                        rsiDataTable.Columns.Add("RSI", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            rsiDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();

                    }
                    else
                    {
                        reader.Close();
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
            return rsiDataTable;
        }

        static public DataTable getBBandsAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable bbandsDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SMA

                        bbandsDataTable = new DataTable();

                        string[] field = record.Split(',');

                        bbandsDataTable.Columns.Add("Symbol", typeof(string));
                        bbandsDataTable.Columns.Add("Date", typeof(DateTime));
                        bbandsDataTable.Columns.Add("Real Lower Band", typeof(decimal));
                        bbandsDataTable.Columns.Add("Real Middle Band", typeof(decimal));
                        bbandsDataTable.Columns.Add("Real Upper Band", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            bbandsDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3]
                                                                });

                        }
                        reader.Close();

                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (bbandsDataTable != null)
                {
                    bbandsDataTable.Clear();
                    bbandsDataTable.Dispose();
                }
                bbandsDataTable = null;
            }
            return bbandsDataTable;
        }

        static public DataTable getAROONAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable aroonDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SMA

                        aroonDataTable = new DataTable();

                        string[] field = record.Split(',');

                        aroonDataTable.Columns.Add("Symbol", typeof(string));
                        aroonDataTable.Columns.Add("Date", typeof(DateTime));
                        aroonDataTable.Columns.Add("Aroon Down", typeof(decimal));
                        aroonDataTable.Columns.Add("Aroon Up", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            aroonDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2]
                                                                });

                        }

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (aroonDataTable != null)
                {
                    aroonDataTable.Clear();
                    aroonDataTable.Dispose();
                }
                aroonDataTable = null;
            }
            return aroonDataTable;
        }

        //returnType must be either ADX DX PLUS_DM MINUS_DM PLUS_DI MINUS_DI
        static public DataTable getADXAlternateFromTodayFile(string filename, string scriptName, string returnType = "ADX")
        {
            StreamReader reader = null;
            DataTable adxDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        adxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        adxDataTable.Columns.Add("Symbol", typeof(string));
                        adxDataTable.Columns.Add("Date", typeof(DateTime));
                        adxDataTable.Columns.Add(returnType, typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (adxDataTable != null)
                {
                    adxDataTable.Clear();
                    adxDataTable.Dispose();
                }
                adxDataTable = null;
            }
            return adxDataTable;
        }

        static public DataTable getMACDAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable macdDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        //time,SMA

                        macdDataTable = new DataTable();

                        string[] field = record.Split(',');

                        macdDataTable.Columns.Add("Symbol", typeof(string));
                        macdDataTable.Columns.Add("Date", typeof(DateTime));
                        macdDataTable.Columns.Add("MACD", typeof(decimal));
                        macdDataTable.Columns.Add("MACD_Hist", typeof(decimal));
                        macdDataTable.Columns.Add("MACD_Signal", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            macdDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2],
                                                                    field[3]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (macdDataTable != null)
                {
                    macdDataTable.Clear();
                    macdDataTable.Dispose();
                }
                macdDataTable = null;
            }
            return macdDataTable;
        }

        static public DataTable getSTOCHAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable stochDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        stochDataTable = new DataTable();

                        string[] field = record.Split(',');

                        stochDataTable.Columns.Add("Symbol", typeof(string));
                        stochDataTable.Columns.Add("Date", typeof(DateTime));
                        stochDataTable.Columns.Add("SlowD", typeof(decimal));
                        stochDataTable.Columns.Add("SlowK", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            stochDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1],
                                                                    field[2]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (stochDataTable != null)
                {
                    stochDataTable.Clear();
                    stochDataTable.Dispose();
                }
                stochDataTable = null;
            }
            return stochDataTable;
        }

#region >> Methods not used
        static public DataTable getDXAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable dxDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        dxDataTable = new DataTable();

                        string[] field = record.Split(',');

                        dxDataTable.Columns.Add("Symbol", typeof(string));
                        dxDataTable.Columns.Add("Date", typeof(DateTime));
                        dxDataTable.Columns.Add("DX", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            dxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });
                        }

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dxDataTable != null)
                {
                    dxDataTable.Clear();
                    dxDataTable.Dispose();
                }
                dxDataTable = null;
            }
            return dxDataTable;
        }

        static public DataTable getPositiveDIAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable plusDIDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        plusDIDataTable = new DataTable();

                        string[] field = record.Split(',');

                        plusDIDataTable.Columns.Add("Symbol", typeof(string));
                        plusDIDataTable.Columns.Add("Date", typeof(DateTime));
                        plusDIDataTable.Columns.Add("PLUS_DI", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            plusDIDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (plusDIDataTable != null)
                {
                    plusDIDataTable.Clear();
                    plusDIDataTable.Dispose();
                }
                plusDIDataTable = null;
            }
            return plusDIDataTable;
        }

        static public DataTable getNegativeDIAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable minusDIDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        minusDIDataTable = new DataTable();

                        string[] field = record.Split(',');

                        minusDIDataTable.Columns.Add("Symbol", typeof(string));
                        minusDIDataTable.Columns.Add("Date", typeof(DateTime));
                        minusDIDataTable.Columns.Add("MINUS_DI", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            minusDIDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (minusDIDataTable != null)
                {
                    minusDIDataTable.Clear();
                    minusDIDataTable.Dispose();
                }
                minusDIDataTable = null;
            }
            return minusDIDataTable;
        }

        static public DataTable getPositiveDMAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable plusDMDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        plusDMDataTable = new DataTable();

                        string[] field = record.Split(',');

                        plusDMDataTable.Columns.Add("Symbol", typeof(string));
                        plusDMDataTable.Columns.Add("Date", typeof(DateTime));
                        plusDMDataTable.Columns.Add("PLUS_DM", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            plusDMDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (plusDMDataTable != null)
                {
                    plusDMDataTable.Clear();
                    plusDMDataTable.Dispose();
                }
                plusDMDataTable = null;
            }
            return plusDMDataTable;
        }

        static public DataTable getNegativeDMAlternateFromTodayFile(string filename, string scriptName)
        {
            StreamReader reader = null;
            DataTable negativeDMDataTable = null;
            try
            {
                if (File.Exists(filename))
                    reader = new StreamReader(filename);
                if (reader != null)
                {
                    string record = reader.ReadLine();
                    if (record.StartsWith("{") == false)
                    {
                        negativeDMDataTable = new DataTable();

                        string[] field = record.Split(',');

                        negativeDMDataTable.Columns.Add("Symbol", typeof(string));
                        negativeDMDataTable.Columns.Add("Date", typeof(DateTime));
                        negativeDMDataTable.Columns.Add("MINUS_DM", typeof(decimal));

                        while (!reader.EndOfStream)
                        {
                            record = reader.ReadLine();
                            field = record.Split(',');

                            negativeDMDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                                    field[1]
                                                                });

                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (negativeDMDataTable != null)
                {
                    negativeDMDataTable.Clear();
                    negativeDMDataTable.Dispose();
                }
                negativeDMDataTable = null;
            }
            return negativeDMDataTable;
        }
        #endregion >> Methods not used

        #endregion >> Methods to Get data from today's downloaded file


        #region >> Methods to get data from yahoo finance - called from client

        /// <summary>
        /// https://autoc.finance.yahoo.com/autoc?query=larsen&lang=en-US
        /// 
        /// using above URL you get a json which has list of matching stocks
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static DataTable symbolSearchAltername(string searchKeyword, string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable resultDataTable = null;

            try
            {
                //https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=BA&apikey=demo&datatype=csv
                //string webservice_url = $"{urlSymbolSearch} + {searchKeyword} + {apiKey} + {dataType}";
                string webservice_url = string.Format(StockApi.urlSymbolSearch_altername, searchKeyword);
                Uri url = new Uri(webservice_url);
                var webRequest = WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";

                //Get the response 
                WebResponse wr = webRequest.GetResponseAsync().Result;
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream);
                string record = reader.ReadToEnd();

                reader.Close();
                wr.Close();
                receiveStream.Close();

                var errors = new List<string>();

                SearchRoot myDeserializedClass = JsonConvert.DeserializeObject<SearchRoot>(record, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                        //args.ErrorContext.Handled = false;
                    }
                    //Converters = { new IsoDateTimeConverter() }

                });

                ResultSet searchResultsSet = myDeserializedClass.ResultSet;

                if ((searchResultsSet != null) && (searchResultsSet.Result.Count > 0))
                {
                    resultDataTable = new DataTable();
                    resultDataTable.Columns.Add("Symbol", typeof(string));
                    resultDataTable.Columns.Add("Name", typeof(string));
                    resultDataTable.Columns.Add("Exchange", typeof(string));
                    resultDataTable.Columns.Add("Type", typeof(string));
                    resultDataTable.Columns.Add("ExchangeDisplay", typeof(string));
                    resultDataTable.Columns.Add("TypeDisplay", typeof(string));

                    foreach (SearchResult item in searchResultsSet.Result)
                    {
                        resultDataTable.Rows.Add(new object[] {
                                                                item.symbol,
                                                                item.symbol+ ": " + item.name + " :" + item.exchDisp,
                                                                item.exch,
                                                                item.type,
                                                                item.exchDisp,
                                                                item.typeDisp
                        });

                    }
                }

            }
            catch (Exception ex)
            {
                if(resultDataTable != null)
                {
                    resultDataTable.Clear();
                    resultDataTable.Dispose();
                    resultDataTable = null;
                }
            }
            return resultDataTable;
        }


        static public Root getIndexIntraDayAlternate(string scriptName, string time_interval="5min", string outputsize="full")
        {
            Root myDeserializedClass = null;
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                string convertedScriptName;
                string range, interval;
                var errors = new List<string>();

                if (time_interval == "60min")
                {
                    interval = "60m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "2y";
                    }

                }
                else if (time_interval == "1min")
                {
                    interval = "1m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "7d";
                    }

                }
                else if (time_interval == "15min")
                {
                    interval = "15m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "60d";
                    }

                }
                else if (time_interval == "30min")
                {
                    interval = "30m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "60d";
                    }

                }
                else //if(time_interval == "60min")
                {
                    interval = "5m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "60d";
                    }
                }

                webservice_url = string.Format(StockApi.urlGetIntra_alternate, scriptName, range, interval, indicators, includeTimestamps);

                Uri url = new Uri(webservice_url);
                var webRequest = WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";
                wr = webRequest.GetResponseAsync().Result;
                receiveStream = wr.GetResponseStream();
                reader = new StreamReader(receiveStream);

                myDeserializedClass = JsonConvert.DeserializeObject<Root>(reader.ReadToEnd(), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                        //args.ErrorContext.Handled = false;
                    }
                    //Converters = { new IsoDateTimeConverter() }

                });

                //Chart myChart = myDeserializedClass.chart;

                //Result myResult = myChart.result[0];

                //Meta myMeta = myResult.meta;

                //Indicators myIndicators = myResult.indicators;

                ////this will be typically only 1 row and quote will have list of close, high, low, open, volume
                //Quote myQuote = myIndicators.quote[0];

                ////this will be typically only 1 row and adjClose will have list of adjClose
                //Adjclose myAdjClose = null;
                //if (bIsDaily)
                //{
                //    myAdjClose = myIndicators.adjclose[0];
                //}

                reader.Close();
                if (receiveStream != null)
                    receiveStream.Close();
            }
            catch (Exception ex)
            {
                myDeserializedClass = null;
            }
            return myDeserializedClass;
        }

        /*
         *To get daily historical prices "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=2y&interval=1d&indicators=quote&includeTimestamps=true"
         * "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=3mo&interval=1wk&indicators=quote&includeTimestamps=true"
         *To get weekly historical prices "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=5y&interval=1wk&indicators=quote&includeTimestamps=true"
         * To get monthly historical prices "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=max&interval=1mo&indicators=quote&includeTimestamps=true"
         * To get 1 minute intra-day proces "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=1d&interval=1m&indicators=quote&includeTimestamps=true"
         * To get 5 minute intra-day prices "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=5d&interval=5m&indicators=quote&includeTimestamps=true"
         * TO get 15 minute intra-day prices "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=5d&interval=15m&indicators=quote&includeTimestamps=true"
         * To get 60 min intra-day prices "https://query1.finance.yahoo.com/v7/finance/chart/AAPL?range=1mo&interval=60m&indicators=quote&includeTimestamps=true"
         *Larsen BSE https://query1.finance.yahoo.com/v7/finance/chart/LT.BO?range=1d&interval=1d&indicators=quote&includeTimestamps=true
         *Larse National Stock exchange https://query1.finance.yahoo.com/v7/finance/chart/LT.NS?range=1d&interval=1d&indicators=quote&includeTimestamps=true
         * TO get specific indicator https://query1.finance.yahoo.com/v7/finance/chart/LT.NS?range=1y&interval=1mo&indicators=close&includeTimestamps=true
        
            Valid intervals: [1m, 2m, 5m, 15m, 30m, 60m, 90m, 1h, 1d, 5d, 1wk, 1mo, 3mo]

            *******WARNING: In case of Intra, we do not get adjustedclose
            * 
            * How to use: provide range & interval in the url. Rest of the url should stay as is
            * a data class is already generated based on the json output using jsontocsharp StockDailyJson.cs & StockIntraJson.cs
            * deserialize the output of web call using Newtonsoft's DeserializeObject, this will populate appropriate classes with data
        */
        static public DataTable getDailyAlternate(string folderPath, string scriptName, string outputsize = "full", bool bIsTestModeOn = true,
                                        bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable dailyDataTable = null;
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                string convertedScriptName;
                string range, interval = "1d";
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv");
                if (outputsize.Equals("compact"))
                {
                    range = "3mo";
                }
                else
                {
                    range = "10y";
                }

                if (scriptName.Contains(".BSE"))
                {
                    convertedScriptName = scriptName.Replace(".BSE", ".BO");
                }
                else if (scriptName.Contains(".NSE"))
                {
                    convertedScriptName = scriptName.Replace(".NSE", ".NS");
                }
                else
                {
                    convertedScriptName = scriptName;
                }

                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=3mo&interval=1wk&indicators=quote&includeTimestamps=true
                        webservice_url = string.Format(StockApi.urlGetDaily_alternate, convertedScriptName, range, interval, indicators, includeTimestamps);

                        Uri url = new Uri(webservice_url);
                        var webRequest = WebRequest.Create(url);
                        webRequest.Method = "GET";
                        webRequest.ContentType = "application/json";
                        wr = webRequest.GetResponseAsync().Result;
                        receiveStream = wr.GetResponseStream();
                        reader = new StreamReader(receiveStream);
                        if (bSaveData)
                        {
                            string fileData = getDailyIntraDataFileFromJSON(reader.ReadToEnd(), scriptName, bIsDaily: true);
                            //if (fileData.StartsWith("{") == false)
                            if (fileData != null)
                            {
                                File.WriteAllText(filename.ToString(), fileData);
                                dailyDataTable = new DataTable();
                            }
                            reader.Close();
                            if (receiveStream != null)
                                receiveStream.Close();
                        }
                        else
                        {
                            dailyDataTable = getDailyIntraDataTableFromJSON(filename.ToString(), reader.ReadToEnd(), scriptName, bIsDaily: true);
                            reader.Close();
                            if (receiveStream != null)
                                receiveStream.Close();
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            dailyDataTable = StockApi.getDailyAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            dailyDataTable = new DataTable();
                        }
                    }
                }
                else
                {
                    dailyDataTable = StockApi.getDailyAlternateFromTodayFile(filename.ToString(), scriptName);
                    //if (File.Exists(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv"))
                    //    reader = new StreamReader(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv");
                    //if (reader != null)
                    //{
                    //    string record = reader.ReadLine();
                    //    if (record.StartsWith("{") == false)
                    //    {
                    //        dailyDataTable = new DataTable();
                    //        string[] field;
                    //        //DataTable dt = new DataTable();
                    //        dailyDataTable.Columns.Add("Symbol", typeof(string));
                    //        dailyDataTable.Columns.Add("Date", typeof(DateTime));
                    //        dailyDataTable.Columns.Add("Open", typeof(decimal));
                    //        dailyDataTable.Columns.Add("High", typeof(decimal));
                    //        dailyDataTable.Columns.Add("Low", typeof(decimal));
                    //        dailyDataTable.Columns.Add("Close", typeof(decimal));
                    //        dailyDataTable.Columns.Add("Volume", typeof(int));
                    //        dailyDataTable.Columns.Add("PurchaseDate", typeof(string));
                    //        dailyDataTable.Columns.Add("CumulativeQuantity", typeof(int));
                    //        dailyDataTable.Columns.Add("CostofInvestment", typeof(decimal));
                    //        dailyDataTable.Columns.Add("ValueOnDate", typeof(decimal));

                    //        while (!reader.EndOfStream)
                    //        {
                    //            record = reader.ReadLine();
                    //            field = record.Split(',');

                    //            dailyDataTable.Rows.Add(new object[] {
                    //                                                scriptName,
                    //                                                System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                    //                                                field[1],
                    //                                                field[2],
                    //                                                field[3],
                    //                                                field[4],
                    //                                                field[5]
                    //                                            });

                    //        }
                    //        reader.Close();
                    //        if (receiveStream != null)
                    //            receiveStream.Close();
                    //    }
                    //    else
                    //    {
                    //        reader.Close();
                    //        if (receiveStream != null)
                    //            receiveStream.Close();
                    //    }
                    //}
                    //else
                    //{
                    //    if (receiveStream != null)
                    //        receiveStream.Close();
                    //}
                }
            }
            catch (Exception ex)
            {
                if (dailyDataTable != null)
                {
                    dailyDataTable.Clear();
                    dailyDataTable.Dispose();
                }
                dailyDataTable = null;
            }
            return dailyDataTable;
        }

        //"validRanges":["1d","5d","1mo","3mo","6mo","1y","2y","5y","10y","ytd","max"]
        static public DataTable getIntradayAlternate(string folderPath, string scriptName, string time_interval = "5min", string outputsize = "full",
                                            bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable intraDataTable = null;
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                string convertedScriptName;
                string range, interval;
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv");

                if (outputsize.Equals("compact"))
                {
                    range = "10d";
                }
                else
                {
                    range = "60d";
                }

                if (time_interval == "60min")
                {
                    interval = "60m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "2y";
                    }

                }
                else if (time_interval == "1min")
                {
                    interval = "1m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "7d";
                    }

                }
                else if (time_interval == "15min")
                {
                    interval = "15m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "60d";
                    }

                }
                else if (time_interval == "30min")
                {
                    interval = "30m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "60d";
                    }

                }
                else //if(time_interval == "60min")
                {
                    interval = "5m";
                    if (outputsize.Equals("compact"))
                    {
                        range = "1d";
                    }
                    else
                    {
                        range = "60d";
                    }

                }

                if (scriptName.Contains(".BSE"))
                {
                    convertedScriptName = scriptName.Replace(".BSE", ".BO");
                }
                else if (scriptName.Contains(".NSE"))
                {
                    convertedScriptName = scriptName.Replace(".NSE", ".NS");
                }
                else
                {
                    convertedScriptName = scriptName;
                }

                if (bIsTestModeOn == false)
                {
                    //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=3mo&interval=5m&indicators=quote&includeTimestamps=true
                    webservice_url = string.Format(StockApi.urlGetIntra_alternate, convertedScriptName, range, interval, indicators, includeTimestamps);

                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = getDailyIntraDataFileFromJSON(reader.ReadToEnd(), scriptName, bIsDaily: false);
                        //if (fileData.StartsWith("{") == false)
                        if (fileData != null)
                        {
                            File.WriteAllText(filename.ToString(), fileData);
                            intraDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                    else
                    {
                        intraDataTable = getDailyIntraDataTableFromJSON(filename.ToString(), reader.ReadToEnd(), scriptName, bIsDaily: false);
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                }
                else
                {
                    intraDataTable = StockApi.getIntraAlternateFromTodayFile(filename.ToString(), scriptName);
                    //if (File.Exists(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv"))
                    //    reader = new StreamReader(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv");
                    //if (reader != null)
                    //{
                    //    string record = reader.ReadLine();
                    //    if (record.StartsWith("{") == false)
                    //    {
                    //        intraDataTable = new DataTable();
                    //        string[] field;
                    //        //DataTable dt = new DataTable();
                    //        intraDataTable.Columns.Add("Symbol", typeof(string));
                    //        intraDataTable.Columns.Add("Date", typeof(DateTime));
                    //        intraDataTable.Columns.Add("Open", typeof(decimal));
                    //        intraDataTable.Columns.Add("High", typeof(decimal));
                    //        intraDataTable.Columns.Add("Low", typeof(decimal));
                    //        intraDataTable.Columns.Add("Close", typeof(decimal));
                    //        intraDataTable.Columns.Add("Volume", typeof(int));
                    //        intraDataTable.Columns.Add("PurchaseDate", typeof(string));
                    //        intraDataTable.Columns.Add("CumulativeQuantity", typeof(int));
                    //        intraDataTable.Columns.Add("CostofInvestment", typeof(decimal));
                    //        intraDataTable.Columns.Add("ValueOnDate", typeof(decimal));

                    //        while (!reader.EndOfStream)
                    //        {
                    //            record = reader.ReadLine();
                    //            field = record.Split(',');

                    //            intraDataTable.Rows.Add(new object[] {
                    //                                                scriptName,
                    //                                                System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                    //                                                field[1],
                    //                                                field[2],
                    //                                                field[3],
                    //                                                field[4],
                    //                                                field[5]
                    //                                            });

                    //        }
                    //        reader.Close();
                    //        if (receiveStream != null)
                    //            receiveStream.Close();
                    //    }
                    //    else
                    //    {
                    //        reader.Close();
                    //        if (receiveStream != null)
                    //            receiveStream.Close();
                    //    }
                    //}
                    //else
                    //{
                    //    if (receiveStream != null)
                    //        receiveStream.Close();
                    //}
                }
            }
            catch (Exception ex)
            {
                if (intraDataTable != null)
                {
                    intraDataTable.Clear();
                    intraDataTable.Dispose();
                }
                intraDataTable = null;
            }
            return intraDataTable;
        }

        static public DataTable globalQuoteAlternate(string folderPath, string symbol, bool bIsTestModeOn = true, bool bSaveData = false,
            string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable resultDataTable = null;
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataRow r;

                string convertedScriptName;

                if (symbol.Contains(".BSE"))
                {
                    convertedScriptName = symbol.Replace(".BSE", ".BO");
                }
                else if (symbol.Contains(".NSE"))
                {
                    convertedScriptName = symbol.Replace(".NSE", ".NS");
                }
                else
                {
                    convertedScriptName = symbol;
                }

                if (bIsTestModeOn == false)
                {
                    //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=1d&interval=1d&indicators=quote&timestamp=true
                    webservice_url = string.Format(StockApi.urlGlobalQuote_alternate, convertedScriptName);

                    Uri url = new Uri(webservice_url);
                    var webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    wr = webRequest.GetResponseAsync().Result;
                    receiveStream = wr.GetResponseStream();
                    reader = new StreamReader(receiveStream);
                    if (bSaveData)
                    {
                        string fileData = getQuoteFileFromJSON(reader.ReadToEnd(), symbol, bIsDaily: true);
                        //if (fileData.StartsWith("{") == false)
                        if (fileData != null)
                        {
                            File.WriteAllText(folderPath + symbol + "global_quote" + ".csv", fileData);
                            resultDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                    else
                    {
                        resultDataTable = getQuoteTableFromJSON(reader.ReadToEnd(), symbol, bIsDaily: true);
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                }
                else
                {
                    if (File.Exists(folderPath + symbol + "global_quote" + ".csv"))
                        reader = new StreamReader(folderPath + symbol + "global_quote" + ".csv");
                    if (reader != null)
                    {
                        string record = reader.ReadLine();

                        if (record.StartsWith("{") == false)
                        {
                            resultDataTable = new DataTable();

                            string[] field = record.Split(',');

                            foreach (string fieldname in field)
                            {
                                resultDataTable.Columns.Add(fieldname, typeof(string));
                            }

                            while (!reader.EndOfStream)
                            {
                                record = reader.ReadLine();
                                field = record.Split(',');

                                r = resultDataTable.NewRow();

                                r.ItemArray = field;

                                resultDataTable.Rows.Add(r);
                            }
                            reader.Close();
                            if (receiveStream != null)
                                receiveStream.Close();
                        }
                        else
                        {
                            reader.Close();
                            if (receiveStream != null)
                                receiveStream.Close();
                        }
                    }
                    else
                    {
                        if (receiveStream != null)
                            receiveStream.Close();
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

        static public DataTable getVWAPAlternate(string folderPath, string scriptName, string time_interval = "5min", string outputsize = "full",
                                    bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV",
                                    DataTable intraDataTable = null)
        {
            DataTable vwapDataTable = null;
            try
            {
                //DataTable intraDataTable;
                //StreamReader reader = null;
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "VWAP_" + time_interval + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    //we will override the savedata flag as we need the online data for intra, test mode is false
                    if (intraDataTable == null)
                    {
                        intraDataTable = StockApi.getIntradayAlternate(folderPath, scriptName, time_interval: time_interval, outputsize: outputsize,
                                            bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey);
                    }
                    if ((intraDataTable != null) && (intraDataTable.Rows.Count > 0))
                    {
                        if (bSaveData)
                        {
                            string fileData = getVWAPDataFileFromJSON(intraDataTable, scriptName);
                            //if (fileData.StartsWith("{") == false)
                            if (fileData != null)
                            {
                                //(folderPath + scriptName + "_" + "VWAP_" + day_interval + ".csv")
                                File.WriteAllText(filename.ToString(), fileData);
                                intraDataTable.Clear();
                                vwapDataTable = new DataTable();
                            }
                        }
                        else
                        {
                            vwapDataTable = getVWAPDataTableFromJSON(filename.ToString(), intraDataTable, scriptName);
                        }
                    }
                }
                else
                {
                    vwapDataTable = StockApi.getVWAPAlternateFromTodayFile(filename.ToString(), scriptName);
                    //VWAP_LT.BSE.csv
                    //if (File.Exists(folderPath + scriptName + "_" + "VWAP_" + time_interval + ".csv"))
                    //    reader = new StreamReader(folderPath + scriptName + "_" + "VWAP_" + time_interval + ".csv");
                    ////Get the response 

                    ////First line indicates the fields
                    //if (reader != null)
                    //{
                    //    string record = reader.ReadLine();
                    //    if (record.StartsWith("{") == false)
                    //    {
                    //        //time,VWAP

                    //        vwapDataTable = new DataTable();

                    //        string[] field = record.Split(',');

                    //        vwapDataTable.Columns.Add("Symbol", typeof(string));
                    //        vwapDataTable.Columns.Add("Date", typeof(DateTime));
                    //        vwapDataTable.Columns.Add("VWAP", typeof(decimal));

                    //        while (!reader.EndOfStream)
                    //        {
                    //            record = reader.ReadLine();
                    //            field = record.Split(',');

                    //            vwapDataTable.Rows.Add(new object[] {
                    //                                                scriptName,
                    //                                                //System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                    //                                                System.Convert.ToDateTime(field[0]),
                    //                                                field[1]
                    //                                            });

                    //        }
                    //        reader.Close();
                    //    }
                    //    else
                    //    {
                    //        reader.Close();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                if (vwapDataTable != null)
                {
                    vwapDataTable.Clear();
                    vwapDataTable.Dispose();
                }
                vwapDataTable = null;
            }
            return vwapDataTable;
        }

        public static DataTable getSMAAlternate(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                            string seriestype = "close", string outputsize = "full", bool bIsTestModeOn = true, bool bSaveData = false,
                            string apiKey = "UV6KQA6735QZKBTV", DataTable dailyTable = null)
        {
            DataTable smaTable = null;

            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "SMA_" + day_interval + "_" + period + "_" + seriestype + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyTable == null)
                        {
                            dailyTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey);
                        }
                        if ((dailyTable != null) && (dailyTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getSMADataFromDailyForSaveFile(dailyTable, scriptName, day_interval: day_interval, period: period, seriestype: seriestype);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    smaTable = new DataTable();
                                }
                            }
                            else
                            {
                                smaTable = getSMADataTableFromDailyForTable(filename.ToString(), dailyTable, scriptName, day_interval: day_interval, period: period, seriestype: seriestype);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            smaTable = getSMAAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            smaTable = new DataTable();
                        }
                    }
                }
                else
                {
                    smaTable = StockApi.getSMAAlternateFromTodayFile(filename.ToString(), scriptName);

                }
            }
            catch (Exception ex)
            {
                if (smaTable != null)
                {
                    smaTable.Clear();
                    smaTable.Dispose();
                }
                smaTable = null;
            }
            return smaTable;
        }

        public static DataTable getEMAalternate(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                        string seriestype = "close", string outputsize = "full", bool bIsTestModeOn = true, bool bSaveData = false,
                        string apiKey = "UV6KQA6735QZKBTV", DataTable dailyDataTable = null)
        {
            DataTable emaTable = null;
            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "EMA_" + day_interval + "_" + period + "_" + seriestype + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyDataTable == null)
                        {
                            dailyDataTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn,
                                bSaveData: false, apiKey: apiKey);
                        }

                        if ((dailyDataTable != null) && (dailyDataTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getEMADataFromDailyForSaveFile(dailyDataTable, scriptName, day_interval: day_interval,
                                                                                period: period, seriestype: seriestype);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    emaTable = new DataTable();
                                }
                            }
                            else
                            {
                                emaTable = getEMADataTableFromDailyForTable(filename.ToString(), dailyDataTable, scriptName,
                                    day_interval: day_interval, period: period, seriestype: seriestype);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            emaTable = getEMAAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            emaTable = new DataTable();
                        }
                    }
                }
                else
                {
                    emaTable = StockApi.getEMAAlternateFromTodayFile(filename.ToString(), scriptName);

                }
            }
            catch (Exception ex)
            {
                if (emaTable != null)
                {
                    emaTable.Clear();
                    emaTable.Dispose();
                }
                emaTable = null;
            }
            return emaTable;
        }

        public static DataTable getRSIalternate(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                string seriestype = "close", string outputsize = "full", bool bIsTestModeOn = true, bool bSaveData = false,
                                string apiKey = "UV6KQA6735QZKBTV", DataTable dailyTable = null)
        {
            DataTable rsiTable = null;
            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "RSI_" + day_interval + "_" + period + "_" + seriestype + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyTable == null)
                        {
                            dailyTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey);
                        }
                        if ((dailyTable != null) && (dailyTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getRSIDataFromDailyForSaveFile(dailyTable, scriptName, day_interval: day_interval, period: period, seriestype: seriestype);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    rsiTable = new DataTable();
                                }
                            }
                            else
                            {
                                rsiTable = getRSIDataTableFromDailyForTable(filename.ToString(), dailyTable, scriptName, day_interval: day_interval, period: period, seriestype: seriestype);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            rsiTable = getRSIAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            rsiTable = new DataTable();
                        }
                    }
                }
                else
                {
                    rsiTable = StockApi.getRSIAlternateFromTodayFile(filename.ToString(), scriptName);
                }
            }
            catch (Exception ex)
            {
                if (rsiTable != null)
                {
                    rsiTable.Clear();
                    rsiTable.Dispose();
                }
                rsiTable = null;
            }
            return rsiTable;
        }

        public static DataTable getBbandsAlternate(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                                        string seriestype = "close", string nbdevup = "2", string nbdevdn = "2", string outputsize = "full",
                                        bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV",
                                        DataTable dailyDataTable = null, DataTable smaDataTable = null)
        {
            DataTable bbandsDataTable = null;
            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "BBANDS_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyDataTable == null)
                        {
                            dailyDataTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn,
                                bSaveData: false, apiKey: apiKey);
                        }
                        if (smaDataTable == null)
                        {
                            smaDataTable = StockApi.getSMAAlternate(folderPath, scriptName, day_interval: day_interval, period: period,
                                seriestype: seriestype, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey, dailyTable: dailyDataTable);
                        }
                        if ((dailyDataTable != null) && (dailyDataTable.Rows.Count > 0) && (smaDataTable != null) && (smaDataTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getBBandsDataFromDailySMAForSaveFile(dailyDataTable, smaDataTable, scriptName, day_interval: day_interval,
                                                                          period: period, seriestype: seriestype, nbdevup: nbdevup, nbdevdn: nbdevdn);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    bbandsDataTable = new DataTable();
                                }
                            }
                            else
                            {
                                bbandsDataTable = getBBandsDataTableFromDailySMAForTable(filename.ToString(), dailyDataTable, smaDataTable, scriptName,
                                    day_interval: day_interval, period: period, seriestype: seriestype, nbdevup: nbdevup, nbdevdn: nbdevdn);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            bbandsDataTable = getBBandsAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            bbandsDataTable = new DataTable();
                        }
                    }
                }
                else
                {
                    bbandsDataTable = StockApi.getBBandsAlternateFromTodayFile(filename.ToString(), scriptName);

                }
            }
            catch (Exception ex)
            {
                if (bbandsDataTable != null)
                {
                    bbandsDataTable.Clear();
                    bbandsDataTable.Dispose();
                }
                bbandsDataTable = null;
            }
            return bbandsDataTable;
        }


        public static DataTable getAroonAlternate(string folderPath, string scriptName, string day_interval = "daily", string period = "20",
                    string outputsize = "full", bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV",
                    DataTable dailyTable = null)
        {
            DataTable aroonTable = null;

            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "AROON_" + day_interval + "_" + period + "_" + outputsize + ".csv");

                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyTable == null)
                        {
                            dailyTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey);
                        }
                        if ((dailyTable != null) && (dailyTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getAROONDataFromDailyForSaveFile(dailyTable, scriptName, day_interval: day_interval, period: period);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    aroonTable = new DataTable();
                                }
                            }
                            else
                            {
                                aroonTable = getAROONDataTableFromDailyForTable(filename.ToString(), dailyTable, scriptName, day_interval: day_interval, period: period);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            aroonTable = getAROONAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            aroonTable = new DataTable();
                        }
                    }
                }
                else
                {
                    aroonTable = StockApi.getAROONAlternateFromTodayFile(filename.ToString(), scriptName);

                }
            }
            catch (Exception ex)
            {
                if (aroonTable != null)
                {
                    aroonTable.Clear();
                    aroonTable.Dispose();
                }
                aroonTable = null;
            }
            return aroonTable;
        }


        //returnType must be either ADX DX PLUS_DM MINUS_DM PLUS_DI MINUS_DI
        public static DataTable getADXAlternate(string folderPath, string scriptName, string day_interval = "daily", string period = "14", string outputsize = "full",
                                       bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV",
                                       DataTable dailyTable = null, string returnType = "ADX")
        {
            DataTable adxDataTable = null;
            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + returnType + "_" + day_interval + "_" + period + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyTable == null)
                        {
                            dailyTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey);
                        }
                        if ((dailyTable != null) && (dailyTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getADXDataFromDailyForSaveFile(folderPath, dailyTable, scriptName, day_interval: day_interval, period: period,
                                    outputsize: outputsize, returnType: returnType);
                                if (fileData != null)
                                {
                                    //File.WriteAllText(filename.ToString(), fileData);
                                    adxDataTable = new DataTable();
                                }
                            }
                            else
                            {
                                adxDataTable = getADXDataTableFromDailyForTable(folderPath, dailyTable, scriptName, day_interval: day_interval, period: period,
                                    outputsize: outputsize, returnType: returnType);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            adxDataTable = getADXAlternateFromTodayFile(filename.ToString(), scriptName, returnType: returnType);
                        }
                        else
                        {
                            adxDataTable = new DataTable();
                        }
                    }
                }
                else
                {
                    adxDataTable = StockApi.getADXAlternateFromTodayFile(filename.ToString(), scriptName, returnType: returnType);
                }
            }
            catch (Exception ex)
            {
                if (adxDataTable != null)
                {
                    adxDataTable.Clear();
                    adxDataTable.Dispose();
                }
                adxDataTable = null;
            }
            return adxDataTable;
        }

        public static DataTable getMACDAlternate(string folderPath, string scriptName, string day_interval = "daily", string seriestype = "close",
                                string fastperiod = "12", string slowperiod = "26", string signalperiod = "9", string outputsize = "full",
                                bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV", DataTable dailyDataTable = null,
                                DataTable emaFastTable = null, DataTable emaSlowTable = null)
        {
            DataTable macdTable = null;
            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "MACD_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" +
                                    signalperiod + "_" + outputsize + ".csv");
                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyDataTable == null)
                        {
                            dailyDataTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn,
                                bSaveData: false, apiKey: apiKey);
                        }

                        if (emaFastTable == null)
                        {
                            emaFastTable = StockApi.getEMAalternate(folderPath, scriptName, day_interval: day_interval, period: fastperiod,
                                            seriestype: seriestype, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn,
                                            bSaveData: false, apiKey: apiKey, dailyDataTable: dailyDataTable);
                        }

                        if (emaSlowTable == null)
                        {
                            emaSlowTable = StockApi.getEMAalternate(folderPath, scriptName, day_interval: day_interval, period: slowperiod,
                                            seriestype: seriestype, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn,
                                            bSaveData: false, apiKey: apiKey, dailyDataTable: dailyDataTable);

                        }
                        if ((dailyDataTable != null) && (dailyDataTable.Rows.Count > 0) && (emaFastTable != null) && (emaFastTable.Rows.Count > 0) &&
                            (emaSlowTable != null) && (emaSlowTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getMACDDataForSaveFile(emaFastTable, emaSlowTable, scriptName, day_interval: day_interval,
                                                  fastperiod, slowperiod, signalperiod);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    macdTable = new DataTable();
                                }
                            }
                            else
                            {
                                macdTable = getMACDDataForTable(filename.ToString(), emaFastTable, emaSlowTable, scriptName, day_interval: day_interval,
                                                  fastperiod, slowperiod, signalperiod);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            macdTable = getMACDAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            macdTable = new DataTable();
                        }
                    }
                }
                else
                {
                    macdTable = StockApi.getMACDAlternateFromTodayFile(filename.ToString(), scriptName);
                }
            }
            catch (Exception ex)
            {
                if (macdTable != null)
                {
                    macdTable.Clear();
                    macdTable.Dispose();
                }
                macdTable = null;
            }
            return macdTable;
        }

        public static DataTable getSTOCHAlternate(string folderPath, string scriptName, string day_interval = "daily",
                                string fastkperiod = "5", string slowkperiod = "3", string slowdperiod = "3", string slowkmatype = "0",
                                string slowdmatype = "0", string outputsize = "full",
                                bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV", DataTable dailyDataTable = null)
        {
            DataTable stochTable = null;
            try
            {
                StringBuilder filename = new StringBuilder(folderPath + scriptName + "_" + "STOCH_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" +
                                    slowdperiod + "_" + slowkmatype + "_" + slowdmatype + "_" + outputsize + ".csv");

                if (bIsTestModeOn == false)
                {
                    if (StockApi.isFileWriteDateEqualsToday(filename.ToString()) == false)
                    {
                        //we will override the savedata flag as we need the online data for intra, test mode is false
                        if (dailyDataTable == null)
                        {
                            dailyDataTable = StockApi.getDailyAlternate(folderPath, scriptName, outputsize: outputsize, bIsTestModeOn: bIsTestModeOn,
                                bSaveData: false, apiKey: apiKey);
                        }

                        if ((dailyDataTable != null) && (dailyDataTable.Rows.Count > 0))
                        {
                            if (bSaveData)
                            {
                                string fileData = getSTOCHDataForSaveFile(dailyDataTable, scriptName, day_interval: day_interval,
                                        fastkperiod: fastkperiod, slowkperiod: slowkperiod, slowdperiod: slowdperiod, slowkmatype: slowkmatype,
                                        slowdmatype: slowdmatype);
                                if (fileData != null)
                                {
                                    File.WriteAllText(filename.ToString(), fileData);
                                    stochTable = new DataTable();
                                }
                            }
                            else
                            {
                                stochTable = getSTOCHDataForTable(filename.ToString(), dailyDataTable, scriptName, day_interval: day_interval,
                                                    fastkperiod: fastkperiod, slowkperiod: slowkperiod, slowdperiod: slowdperiod, slowkmatype: slowkmatype,
                                                    slowdmatype: slowdmatype);
                            }
                        }
                    }
                    else
                    {
                        if (bSaveData == false)
                        {
                            stochTable = getSTOCHAlternateFromTodayFile(filename.ToString(), scriptName);
                        }
                        else
                        {
                            stochTable = new DataTable();
                        }
                    }
                }
                else
                {
                    stochTable = StockApi.getSTOCHAlternateFromTodayFile(filename.ToString(), scriptName);
                }
            }
            catch (Exception ex)
            {
                if (stochTable != null)
                {
                    stochTable.Clear();
                    stochTable.Dispose();
                }
                stochTable = null;
            }
            return stochTable;
        }

#endregion >> Methods to get data from yahoo finance - called from client


#region >> Methods that return string with data to be written to file. They get data from JSON or input data table
        static public string getDailyIntraDataFileFromJSON(string record, string symbol, bool bIsDaily = true)
        {
            //Root myDeserializedClass = (Root)JsonConvert.DeserializeObject(record);

            //first try
            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record);

            StringBuilder returnString = new StringBuilder("timestamp,open,high,low,close,volume");
            returnString.AppendLine();
            //string rowToWrite;
            DateTime myDate;
            //double close;
            //double high;
            //double low;
            //double open;
            //int volume;
            //double adjusetedClose = 0.00;
            var errors = new List<string>();
            try
            {
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                        //args.ErrorContext.Handled = false;
                    }
                    //Converters = { new IsoDateTimeConverter() }

                });

                Chart myChart = myDeserializedClass.chart;

                Result myResult = myChart.result[0];

                Meta myMeta = myResult.meta;

                Indicators myIndicators = myResult.indicators;

                //this will be typically only 1 row and quote will have list of close, high, low, open, volume
                Quote myQuote = myIndicators.quote[0];

                //this will be typically only 1 row and adjClose will have list of adjClose
                Adjclose myAdjClose = null;
                if (bIsDaily)
                {
                    myAdjClose = myIndicators.adjclose[0];
                }
                if (myResult.timestamp != null)
                {
                    for (int i = 0; i < myResult.timestamp.Count; i++)
                    {
                        if ((myQuote.close[i] == null) && (myQuote.high[i] == null) && (myQuote.low[i] == null) && (myQuote.open[i] == null)
                            && (myQuote.volume[i] == null))
                        {
                            continue;
                        }

                        //rowToWrite = "";

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
                        myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp[i], myMeta.timezone);

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]);
                        //string formatedDate = myDate.ToString("dd-MM-yyyy");
                        //string formatedDate = myDate.ToString("yyyy-dd-MM");

                        if (bIsDaily)
                        {
                            //rowToWrite += (myDate.ToString("yyyy-MM-dd") + ",");
                            returnString.Append(myDate.ToString("yyyy-MM-dd") + ",");
                        }
                        else
                        {
                            //rowToWrite += (myDate.ToString("yyyy-MM-dd HH:mm") + ",");
                            returnString.Append(myDate.ToString("yyyy-MM-dd HH:mm") + ",");
                        }

                        //myDate = System.Convert.ToDateTime(myResult.timestamp[i]);

                        //if all are null do not enter this row

                        if (myQuote.open[i] == null)
                        {
                            //open = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //open = (double)myQuote.open[i];
                            //open = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.open[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.open[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.open[i]) + ",");
                        }

                        if (myQuote.high[i] == null)
                        {
                            //high = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //high = (double)myQuote.high[i];
                            //high = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.high[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.high[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.high[i]) + ",");
                        }

                        if (myQuote.low[i] == null)
                        {
                            //low = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //low = (double)myQuote.low[i];
                            //low = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.low[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.low[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.low[i]) + ",");

                        }

                        if (myQuote.close[i] == null)
                        {
                            //close = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //close = (double)myQuote.close[i];
                            //close = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.close[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.close[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.close[i]) + ",");
                        }

                        if (myQuote.volume[i] == null)
                        {
                            //volume = 0;
                            //rowToWrite += "0,";
                            returnString.Append("0,");

                        }
                        else
                        {
                            //volume = (int)myQuote.volume[i];
                            //rowToWrite += (string.Format("{0:0}", myQuote.volume[i]));
                            returnString.Append(string.Format("{0:0}", myQuote.volume[i]));
                        }

                        //if (bIsDaily)
                        //{
                        //    if (myAdjClose.adjclose[i] == null)
                        //    {
                        //        //adjusetedClose = 0.00;
                        //        rowToWrite += "0,";
                        //    }
                        //    else
                        //    {
                        //        //adjusetedClose = (double)myAdjClose.adjclose[i];
                        //        //adjusetedClose = System.Convert.ToDouble(string.Format("{0:0.00}", myAdjClose.adjclose[i]));
                        //        rowToWrite += (string.Format("{0:0.00}", myAdjClose.adjclose[i]) + ",");
                        //    }
                        //}

                        if ((i + 1) < myResult.timestamp.Count)
                        {
                            returnString.AppendLine();
                        }
                    }
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }


        public static string getVWAPDataFileFromJSON(DataTable intraDataTable, string scriptName)
        {
            StringBuilder returnString = new StringBuilder("time,VWAP");
            returnString.AppendLine();
            double high, low, close, avgprice = 0.00, cumavgpricevol = 0.00, vwap = 0.00, prev_cumavgpricevol = 0.00;
            DateTime transDate;
            long volume, cumvol = 0, prev_cumvol = 0;

            try
            {
                for (int i = 0; i < intraDataTable.Rows.Count; i++)
                {
                    //find all the values
                    transDate = System.Convert.ToDateTime(intraDataTable.Rows[i]["Date"]);
                    high = System.Convert.ToDouble(intraDataTable.Rows[i]["High"]);
                    low = System.Convert.ToDouble(intraDataTable.Rows[i]["Low"]);
                    close = System.Convert.ToDouble(intraDataTable.Rows[i]["Close"]);
                    volume = System.Convert.ToInt32(intraDataTable.Rows[i]["Volume"]);
                    if (volume == 0)
                        continue;
                    avgprice = (high + low + close) / 3;

                    cumavgpricevol = (avgprice * volume) + prev_cumavgpricevol;
                    prev_cumavgpricevol = cumavgpricevol;

                    cumvol = volume + prev_cumvol;
                    prev_cumvol = cumvol;

                    vwap = cumavgpricevol / cumvol;

                    returnString.Append((transDate.ToString("yyyy-MM-dd HH:mm") + ","));
                    returnString.Append(string.Format("{0:0.0000}", vwap));
                    if ((i + 1) < intraDataTable.Rows.Count)
                    {
                        returnString.AppendLine();
                    }
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        static public string getQuoteFileFromJSON(string record, string symbol, bool bIsDaily = true)
        {
            //Root myDeserializedClass = (Root)JsonConvert.DeserializeObject(record);

            //first try
            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record);

            StringBuilder returnString = new StringBuilder("symbol,open,high,low,price,volume,latestDay,previousClose,change,changePercent");
            returnString.AppendLine();
            //string rowToWrite;
            DateTime myDate;
            double change;
            double changepercent;
            double prevclose, close;
            var errors = new List<string>();
            try
            {
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                        //args.ErrorContext.Handled = false;
                    }
                    //Converters = { new IsoDateTimeConverter() }

                });

                Chart myChart = myDeserializedClass.chart;

                Result myResult = myChart.result[0];

                Meta myMeta = myResult.meta;

                Indicators myIndicators = myResult.indicators;

                //this will be typically only 1 row and quote will have list of close, high, low, open, volume
                Quote myQuote = myIndicators.quote[0];

                //this will be typically only 1 row and adjClose will have list of adjClose
                Adjclose myAdjClose = null;
                if (bIsDaily)
                {
                    myAdjClose = myIndicators.adjclose[0];
                }

                if (myResult.timestamp != null)
                {
                    for (int i = 0; i < myResult.timestamp.Count; i++)
                    {
                        if ((myQuote.close[i] == null) && (myQuote.high[i] == null) && (myQuote.low[i] == null) && (myQuote.open[i] == null)
                            && (myQuote.volume[i] == null))
                        {
                            continue;
                        }

                        //rowToWrite = symbol + ",";
                        returnString.Append(symbol + ",");

                        if (myQuote.open[i] == null)
                        {
                            //open = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //open = (double)myQuote.open[i];
                            //open = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.open[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.open[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.open[i]) + ",");
                        }

                        if (myQuote.high[i] == null)
                        {
                            //high = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //high = (double)myQuote.high[i];
                            //high = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.high[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.high[i]) + ",");
                            returnString.Append(string.Format("{0:0.00}", myQuote.high[i]) + ",");
                        }

                        if (myQuote.low[i] == null)
                        {
                            //low = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //low = (double)myQuote.low[i];
                            //low = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.low[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.low[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.low[i]) + ",");

                        }
                        if (myQuote.close[i] == null)
                        {
                            close = 0.00;
                            //rowToWrite += "0.00,";
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //close = (double)myQuote.close[i];
                            close = System.Convert.ToDouble(string.Format("{0:0.0000}", myQuote.close[i]));
                            //rowToWrite += (string.Format("{0:0.00}", myQuote.close[i]) + ",");
                            returnString.Append(string.Format("{0:0.0000}", myQuote.close[i]) + ",");
                        }

                        if (myQuote.volume[i] == null)
                        {
                            //volume = 0;
                            //rowToWrite += "0,";
                            returnString.Append("0,");

                        }
                        else
                        {
                            //volume = (int)myQuote.volume[i];
                            //rowToWrite += (string.Format("{0:0}", myQuote.volume[i]) + ",");
                            returnString.Append(string.Format("{0:0}", myQuote.volume[i]) + ",");
                        }

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
                        myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp[i], myMeta.timezone);

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]);
                        //string formatedDate = myDate.ToString("dd-MM-yyyy");
                        //string formatedDate = myDate.ToString("yyyy-dd-MM");
                        //rowToWrite += (myDate.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(myDate.ToString("yyyy-MM-dd HH:mm") + ",");

                        prevclose = System.Convert.ToDouble(string.Format("{0:0.0000}", myMeta.chartPreviousClose));
                        change = close - prevclose;

                        if (change > 0 && prevclose > 0)
                        {
                            changepercent = (change / prevclose) * 100;
                        }
                        else
                        {
                            changepercent = 0.00;
                        }


                        //rowToWrite += string.Format("{0:0.00}", myMeta.chartPreviousClose) + ",";
                        returnString.Append(string.Format("{0:0.0000}", myMeta.chartPreviousClose) + ",");
                        //rowToWrite += string.Format("{0:0.00}", change) + ",";
                        returnString.Append(string.Format("{0:0.0000}", change) + ",");
                        //rowToWrite += string.Format("{0:0.00}", changepercent);
                        returnString.Append(string.Format("{0:0.0000}", changepercent));
                        //if (bIsDaily)
                        //{
                        //    if (myAdjClose.adjclose[i] == null)
                        //    {
                        //        //adjusetedClose = 0.00;
                        //        rowToWrite += "0,";
                        //    }
                        //    else
                        //    {
                        //        //adjusetedClose = (double)myAdjClose.adjclose[i];
                        //        //adjusetedClose = System.Convert.ToDouble(string.Format("{0:0.00}", myAdjClose.adjclose[i]));
                        //        rowToWrite += (string.Format("{0:0.00}", myAdjClose.adjclose[i]) + ",");
                        //    }
                        //}

                        if ((i + 1) < myResult.timestamp.Count)
                        {
                            returnString.AppendLine();
                        }
                    }
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }

        public static string getSMADataFromDailyForSaveFile(DataTable dailyTable, string scriptName, string day_interval = "daily", string period = "20",
                                    string seriestype = "close")
        {
            StringBuilder returnString = new StringBuilder("time,SMA");
            returnString.AppendLine();
            int iPeriod;

            double sumOfSeriesType;
            double columnValue;
            double sma;
            DateTime dateLastRow = DateTime.Today;
            int subrownum;
            try
            {
                iPeriod = System.Convert.ToInt32(period);

                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = 0; rownum < dailyTable.Rows.Count; rownum++)
                {
                    sumOfSeriesType = 0.00;
                    //add the seriestype column values from dailytable from next "iPeriod" number of rows
                    for (subrownum = rownum; ((subrownum < (rownum + iPeriod)) && (subrownum < dailyTable.Rows.Count)); subrownum++)
                    {
                        columnValue = System.Convert.ToDouble(dailyTable.Rows[subrownum][seriestype]);
                        dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[subrownum]["Date"]);
                        sumOfSeriesType += columnValue;
                    }
                    //Find average
                    sma = sumOfSeriesType / iPeriod;
                    //add to sma table
                    returnString.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", sma));
                    //if we have reached last row then break from main for
                    if (subrownum >= dailyTable.Rows.Count)
                        break;
                    else
                    {
                        returnString.AppendLine();
                    }

                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }

        public static string getEMADataFromDailyForSaveFile(DataTable dailyTable, string scriptName, string day_interval = "daily",
                                                            string period = "20", string seriestype = "close")
        {
            StringBuilder returnString = new StringBuilder("time,EMA");
            returnString.AppendLine();
            double ema = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            int iPeriod;
            int rownum;
            try
            {
                //Strat from 1st row in smaTable and get the row from smaTable where dailyTable's Date matches current Date row from smaTable
                //            Multiplier: (2 / (Time periods + 1) ) 
                //EMA: { Close - EMA(previous day)} x multiplier +EMA(previous day)
                //Here Time period is the number of days you want to look back.
                //For the 1st value = average of close. 
                //since we don’t have EMA for the first time, we just take simple moving average on the 10th day. 
                //From 11th day onwards we start calculating EMA
                iPeriod = System.Convert.ToInt32(period);
                rownum = iPeriod - 1;
                ema = StockApi.FindEMA(rownum, iPeriod, seriestype, dailyTable, ema);

                dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);
                returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                returnString.Append(string.Format("{0:0.0000}", ema));
                returnString.AppendLine();

                for (rownum = iPeriod; rownum < dailyTable.Rows.Count; rownum++)
                {
                    dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

                    ema = StockApi.FindEMA(rownum, iPeriod, seriestype, dailyTable, ema);

                    returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", ema));
                    returnString.AppendLine();
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }

        /// <summary>
        /// Step 1: Closing Price
        ///We will take the closing price of the stock for 30 days.The closing price is mentioned in column(1).
        ///Step 2: Changes in Closing Price
        ///We then compare the closing price of the current day with the previous day’s closing price and note them down.
        ///Thus, from the table, for 25-04, we get the change in price as (280.69 - 283.46) = -2.77.
        ///Similarly, for 26-04, Change in price = (Current closing price - Previous closing price) = (285.48 - 280.6) = 4.79. 
        ///We will then tabulate the results in the column mentioned as “Change(2)”. In this manner, we calculated the change in price.
        ///Step 3: Gain and Loss
        ///We will now create two sections depending on the fact the price increased or decreased, with respect to the previous day’s closing price.
        ///If the price has increased, we note down the difference in the “Gain” column and if it’s a loss, then we note it down in the “Loss” column.
        ///For example, on 26-04, the price had increased by 4.79. Thus, this value would be noted in the “Gain” column.
        ///If you look at the data for 25-04, there was a decrease in the price by 2.77. Now, while the value is written as negative in the 
        ///“change” column, we do not mention the negative sign in the “Loss” column.And only write it as 2.77. 
        ///In this manner, the table for the columns “Gain (3)” and “Loss (4)” is updated.
        ///Step 4: Average Gain and Loss
        ///In the RSI indicator, to smoothen the price movement, we take an average of the gains(and losses) for a certain period.
        ///While we call it an average, a little explanation would be needed.For the first 14 periods, it is a simple average of the values.
        ///To explain it, we will look at the average gain column.
        ///Thus, in the table, the first 14 values would be from (25-04) to(14-05) which is, 
        ///(0.00 + 4.79 + 8.60 + 0.00 + 6.02 + 1.23 + 0.00 + 9.64 + 8.68 + 0.00 + 4.88 + 0.00 + 0.00 + 0.00)/14 = 3.13.
        ///Now, since we are placing more emphasis on the recent values, for the next set of values, we use the following formula,
        ////[(Previous avg.gain)*13)+ current gain)]/14.
        ///Thus, for (15-05), we will calculate the average gain as [(3.13 * 13)+0.00]/14 = 2.91.
        ///Similarly, we will calculate the average Loss too.
        ///Based on these formulae, the table is updated for the columns “Avg Gain(5)” and “Avg Loss(6)”.
        ///Step 5: Calculate RS
        ///Now, to make matters simple, we add a column called “RS” which is simply, 
        ////(Avg Gain)/(Avg Loss). 
        ///Thus, for 14-05, RS = (Avg Gain)/(Avg Loss) = 3.13/2.52 = 1.24.
        ///In this manner, the table for the column “RS (7)” is updated. In the next step, we finally work out the RSI values.
        ///Step 6: Calculation of RSI
        ////RSI = [100 - (100/{1+ RS})].
        ///For example, for (14-05),
        ///RSI = [100 - (100/{1+ RS})] = [100 - (100/{1 - 1.24})] = 55.37.
        /// </summary>
        /// <param name="dailyTable"></param>
        /// <param name="scriptName"></param>
        /// <param name="day_interval"></param>
        /// <param name="period"></param>
        /// <param name="seriestype"></param>
        /// <returns></returns>
        public static string getRSIDataFromDailyForSaveFile(DataTable dailyTable, string scriptName, string day_interval = "daily", string period = "20",
                                    string seriestype = "close")
        {
            StringBuilder returnString = new StringBuilder("time,RSI");
            returnString.AppendLine();
            int iPeriod;
            double change, gain, loss, avgGain = 0.00, avgLoss = 0.00, rs, rsi;
            double sumOfGain = 0.00, sumOfLoss = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            try
            {
                iPeriod = System.Convert.ToInt32(period);

                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = 1; rownum < dailyTable.Rows.Count; rownum++)
                {
                    //current - prev
                    change = System.Convert.ToDouble(dailyTable.Rows[rownum][seriestype]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1][seriestype]);
                    dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

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
                        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(string.Format("{0:0.0000}", rsi));
                        returnString.AppendLine();
                    }
                    else
                    {
                        avgGain = ((avgGain * (iPeriod - 1)) + gain) / iPeriod;
                        avgLoss = ((avgLoss * (iPeriod - 1)) + loss) / iPeriod;
                        rs = avgGain / avgLoss;
                        rsi = 100 - (100 / (1 - rs));
                        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(string.Format("{0:0.0000}", rsi));
                        returnString.AppendLine();
                    }
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }

        public static string getBBandsDataFromDailySMAForSaveFile(DataTable dailyTable, DataTable smaTable, string scriptName,
                        string day_interval = "daily", string period = "20", string seriestype = "close", string nbdevup = "2", string nbdevdn = "2")
        {
            StringBuilder returnString = new StringBuilder("time,Real Lower Band,Real Middle Band,Real Upper Band");
            returnString.AppendLine();
            double sma, upperBand, lowerBand;
            DataRow[] smaRows;
            int subrownum;
            int iPeriod;
            double pricecolumnValue;
            DateTime dateLastRow = DateTime.Today;

            double standardDevUpper, standardDevLower;
            double M;
            double S;
            int k;
            double tmpM;
            int upFactor = System.Convert.ToInt32(nbdevup);
            int dnFactor = System.Convert.ToInt32(nbdevdn);
            try
            {
                iPeriod = System.Convert.ToInt32(period);
                for (int rownum = 0; (rownum + iPeriod) < dailyTable.Rows.Count; rownum++)
                {
                    M = 0.0;
                    S = 0.0;
                    k = 1;
                    //find the standard deviation of price
                    for (subrownum = rownum; ((subrownum < (rownum + iPeriod)) && (subrownum < dailyTable.Rows.Count)); subrownum++)
                    {
                        pricecolumnValue = System.Convert.ToDouble(dailyTable.Rows[subrownum][seriestype]);
                        dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[subrownum]["Date"]);

                        tmpM = M;
                        M += (pricecolumnValue - tmpM) / k;
                        S += (pricecolumnValue - tmpM) * (pricecolumnValue - M);
                        k++;
                    }
                    standardDevUpper = Math.Sqrt(S / (k - upFactor));
                    standardDevLower = Math.Sqrt(S / (k - dnFactor));

                    //get the SMA for the last row date
                    smaRows = smaTable.Select("Date = '" + dateLastRow.Date.ToString() + "'");
                    sma = 0.00;
                    if ((smaRows != null) && (smaRows.Length > 0))
                    {
                        sma = System.Convert.ToDouble(smaRows[0]["SMA"]);
                    }

                    //Find upper & lower bands
                    upperBand = sma + (standardDevUpper * upFactor);
                    lowerBand = sma - (standardDevLower * dnFactor);

                    returnString.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", lowerBand) + ",");
                    returnString.Append(string.Format("{0:0.0000}", sma) + ",");
                    returnString.Append(string.Format("{0:0.0000}", upperBand));
                    returnString.AppendLine();
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }

        public static string getAROONDataFromDailyForSaveFile(DataTable dailyTable, string scriptName, string day_interval = "daily", string period = "20")
        {
            StringBuilder returnString = new StringBuilder("time,Aroon Down,Aroon Up");
            returnString.AppendLine();
            int iPeriod;

            double aroonUp, aroonDown;
            DateTime dateLastRow = DateTime.Today;

            try
            {
                iPeriod = System.Convert.ToInt32(period);

                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = iPeriod; rownum < dailyTable.Rows.Count; rownum++)
                {
                    aroonUp = CalculateAroonUp(rownum, iPeriod, dailyTable);
                    aroonDown = CalculateAroonDown(rownum, iPeriod, dailyTable);
                    dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

                    returnString.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", aroonDown) + ",");
                    returnString.Append(string.Format("{0:0.0000}", aroonUp));
                    returnString.AppendLine();
                }
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
            }
            return null;
        }

        //returnType must be either ADX DX PLUS_DM MINUS_DM PLUS_DI MINUS_DI
        public static string getADXDataFromDailyForSaveFile(string folderPath, DataTable dailyTable, string scriptName, string day_interval = "daily",
                                                            string period = "20", string outputsize = "full", string returnType = "ADX")
        {
            string returnString = null;

            StringBuilder returnStringADX = new StringBuilder("time,ADX");
            returnStringADX.AppendLine();

            StringBuilder returnStringDX = new StringBuilder("time,DX");
            returnStringDX.AppendLine();

            StringBuilder returnStringPlusDM = new StringBuilder("time,PLUS_DM");
            returnStringPlusDM.AppendLine();

            StringBuilder returnStringMinusDM = new StringBuilder("time,MINUS_DM");
            returnStringMinusDM.AppendLine();

            StringBuilder returnStringPlusDI = new StringBuilder("time,PLUS_DI");
            returnStringPlusDI.AppendLine();

            StringBuilder returnStringMinusDI = new StringBuilder("time,MINUS_DI");
            returnStringMinusDI.AppendLine();

            int iPeriod;
            DateTime dateLastRow = DateTime.Today;

            double tr, minusDM, plusDM, trPeriod, minusDMPeriod, plusDMPeriod, minusDIPeriod, plusDIPeriod, dx, adx;
            List<double> listTR = new List<double>();
            List<double> listPlusDM1 = new List<double>();
            List<double> listMinusDM1 = new List<double>();
            List<double> listTRPeriod = new List<double>();
            List<double> listPlusDMPeriod = new List<double>();
            List<double> listMinusDMPeriod = new List<double>();
            List<double> listPlusDIPeriod = new List<double>();
            List<double> listMinusDIPeriod = new List<double>();
            List<double> listDX = new List<double>();
            List<double> listADX = new List<double>();

            StringBuilder filenameADX = new StringBuilder(folderPath + scriptName + "_" + "ADX_" + day_interval + "_" + period + "_" + outputsize + ".csv");
            StringBuilder filenameDX = new StringBuilder(folderPath + scriptName + "_" + "DX_" + day_interval + "_" + period + "_" + outputsize + ".csv");
            StringBuilder filenamePlusDM = new StringBuilder(folderPath + scriptName + "_" + "PLUS_DM_" + day_interval + "_" + period + "_" + outputsize + ".csv");
            StringBuilder filenameMinusDM = new StringBuilder(folderPath + scriptName + "_" + "MINUS_DM_" + day_interval + "_" + period + "_" + outputsize + ".csv");
            StringBuilder filenamePlusDI = new StringBuilder(folderPath + scriptName + "_" + "PLUS_DI_" + day_interval + "_" + period + "_" + outputsize + ".csv");
            StringBuilder filenameMinusDI = new StringBuilder(folderPath + scriptName + "_" + "MINUS_DI_" + day_interval + "_" + period + "_" + outputsize + ".csv");

            try
            {


                iPeriod = System.Convert.ToInt32(period);

                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = 1; rownum < dailyTable.Rows.Count; rownum++)
                {
                    dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);
                    tr = StockApi.FindTR1(rownum, dailyTable);
                    listTR.Add(tr);

                    plusDM = StockApi.FindPositiveDM1(rownum, dailyTable);
                    listPlusDM1.Add(plusDM);

                    minusDM = StockApi.FindNegativeDM1(rownum, dailyTable);
                    listMinusDM1.Add(minusDM);

                    if (rownum >= iPeriod)
                    {
                        trPeriod = StockApi.FindTR_Period(rownum, iPeriod, listTR, listTRPeriod);
                        listTRPeriod.Add(trPeriod);

                        plusDMPeriod = StockApi.FindPositveDM_Period(rownum, iPeriod, listPlusDM1, listPlusDMPeriod);
                        listPlusDMPeriod.Add(plusDMPeriod);
                        returnStringPlusDM.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringPlusDM.Append(string.Format("{0:0.0000}", plusDMPeriod));
                        returnStringPlusDM.AppendLine();

                        minusDMPeriod = StockApi.FindNegativeDM_Period(rownum, iPeriod, listMinusDM1, listMinusDMPeriod);
                        listMinusDMPeriod.Add(minusDMPeriod);
                        returnStringMinusDM.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringMinusDM.Append(string.Format("{0:0.0000}", minusDMPeriod));
                        returnStringMinusDM.AppendLine();

                        plusDIPeriod = StockApi.FindPositveDI_Period(rownum, iPeriod, listTRPeriod, listPlusDMPeriod);
                        listPlusDIPeriod.Add(plusDIPeriod);
                        returnStringPlusDI.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringPlusDI.Append(string.Format("{0:0.0000}", plusDIPeriod));
                        returnStringPlusDI.AppendLine();

                        minusDIPeriod = StockApi.FindNegativeDI_Period(rownum, iPeriod, listTRPeriod, listMinusDMPeriod);
                        listMinusDIPeriod.Add(minusDIPeriod);
                        returnStringMinusDI.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringMinusDI.Append(string.Format("{0:0.0000}", minusDIPeriod));
                        returnStringMinusDI.AppendLine();

                        dx = StockApi.FindDX(rownum, iPeriod, listPlusDIPeriod, listMinusDIPeriod);
                        listDX.Add(dx);
                        returnStringDX.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringDX.Append(string.Format("{0:0.0000}", dx));
                        returnStringDX.AppendLine();

                        if ((rownum + 1) >= (iPeriod * 2))
                        {
                            adx = StockApi.FindADX(rownum, iPeriod, listDX, listADX);
                            listADX.Add(adx);
                            returnStringADX.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                            returnStringADX.Append(string.Format("{0:0.0000}", adx));
                            returnStringADX.AppendLine();
                        }
                    }
                }

                //Save all strings
                File.WriteAllText(filenameADX.ToString(), returnStringADX.ToString());
                File.WriteAllText(filenameDX.ToString(), returnStringDX.ToString());
                File.WriteAllText(filenamePlusDM.ToString(), returnStringPlusDM.ToString());
                File.WriteAllText(filenameMinusDM.ToString(), returnStringMinusDM.ToString());
                File.WriteAllText(filenamePlusDI.ToString(), returnStringPlusDI.ToString());
                File.WriteAllText(filenameMinusDI.ToString(), returnStringMinusDI.ToString());

                returnString = "";
            }
            catch (Exception ex)
            {
                returnString = null;
            }
            listTR.Clear();
            listTR = null;
            listPlusDM1.Clear();
            listPlusDM1 = null;
            listMinusDM1.Clear();
            listMinusDM1 = null;
            listTRPeriod.Clear();
            listTRPeriod = null;
            listPlusDMPeriod.Clear();
            listPlusDMPeriod = null;
            listMinusDMPeriod.Clear();
            listMinusDMPeriod = null;
            listPlusDIPeriod.Clear();
            listPlusDIPeriod = null;
            listMinusDIPeriod.Clear();
            listMinusDIPeriod = null;
            listDX.Clear();
            listDX = null;
            listADX.Clear();
            listADX = null;

            returnStringADX.Clear();
            returnStringADX = null;
            returnStringDX.Clear();
            returnStringDX = null;
            returnStringPlusDM.Clear();
            returnStringPlusDM = null;
            returnStringMinusDM.Clear();
            returnStringMinusDM = null;
            returnStringPlusDI.Clear();
            returnStringPlusDI = null;
            returnStringMinusDI.Clear();
            returnStringMinusDI = null;


            return returnString;
        }

        public static string getMACDDataForSaveFile(DataTable emaFastTable, DataTable emaSlowTable, string scriptName,
                        string day_interval = "daily", string fastperiod = "12", string slowperiod = "26", string signalperiod = "9")
        {
            StringBuilder returnString = new StringBuilder("time,MACD,MACD_Hist,MACD_Signal");
            returnString.AppendLine();
            int iSlowPeriod, iSignalPeriod, iFastPeriod;
            double macd = 0.00, signal = 0.00, histogram = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            List<double> listMACD = new List<double>();
            int emaFastIndex;
            int rownum;
            try
            {
                iSlowPeriod = System.Convert.ToInt32(slowperiod);
                iSignalPeriod = System.Convert.ToInt32(signalperiod);
                iFastPeriod = System.Convert.ToInt32(fastperiod);

                emaFastIndex = iSlowPeriod - iFastPeriod;

                for (rownum = 0; rownum < emaSlowTable.Rows.Count; rownum++)
                {

                    dateCurrentRow = System.Convert.ToDateTime(emaFastTable.Rows[(emaFastIndex + rownum)]["Date"]);
                    macd = System.Convert.ToDouble(emaFastTable.Rows[(emaFastIndex + rownum)]["EMA"]) - System.Convert.ToDouble(emaSlowTable.Rows[rownum]["EMA"]);
                    listMACD.Add(macd);

                    if (rownum >= (iSignalPeriod - 1))
                    {
                        signal = StockApi.FindSignal(rownum, iSignalPeriod, listMACD, signal);
                        histogram = macd - signal;

                        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(string.Format("{0:0.0000}", macd) + ",");
                        returnString.Append(string.Format("{0:0.0000}", histogram) + ",");
                        returnString.Append(string.Format("{0:0.0000}", signal));
                        returnString.AppendLine();
                    }
                }


                //for (int rownum = iSlowPeriod - 1; ((rownum < emaFastTable.Rows.Count) && ((rownum - iSlowPeriod + 1) < emaSlowTable.Rows.Count)); rownum++)
                //{
                //    dateCurrentRow = System.Convert.ToDateTime(emaFastTable.Rows[rownum]["Date"]);
                //    macd = System.Convert.ToDouble(emaFastTable.Rows[rownum]["EMA"]) - System.Convert.ToDouble(emaSlowTable.Rows[rownum - iSlowPeriod + 1]["EMA"]);
                //    listMACD.Add(macd);

                //    if(rownum >= iSignalPeriodStart)
                //    {
                //        signal = StockApi.FindSignal(rownum, iSignalPeriodStart, iSignalPeriod, listMACD, signal);
                //        histogram = macd - signal;

                //        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                //        returnString.Append(string.Format("{0:0.0000}", macd) + ",");
                //        returnString.Append(string.Format("{0:0.0000}", histogram) + ",");
                //        returnString.Append(string.Format("{0:0.0000}", signal));
                //        returnString.AppendLine();
                //    }
                //}
                listMACD.Clear();
                listMACD = null;
                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                returnString = null;
                if (listMACD != null)
                {
                    listMACD.Clear();
                }
                listMACD = null;
            }
            return null;
        }


        /*
         * %K = (Current Close - Lowest Low)/(Highest High - Lowest Low) * 100
           %D = 3-day SMA of %K

           Lowest Low = lowest low for the look-back period
           Highest High = highest high for the look-back period
           %K is multiplied by 100 to move the decimal point two places

           The default setting for the Stochastic Oscillator is 14 periods.

         %K = The current value of the stochastic indicator
         %K is referred to sometimes as the slow stochastic indicator.
         The "fast" stochastic indicator is taken as %D = 3 - period moving average of %K.
         */

        public static string getSTOCHDataForSaveFile(DataTable dailyDataTable, string scriptName, string day_interval = "daily",
                            string fastkperiod = "5", string slowkperiod = "3", string slowdperiod = "3", string slowkmatype = "0",
                            string slowdmatype = "0")
        {
            StringBuilder returnString = new StringBuilder("time,SlowD,SlowK");
            returnString.AppendLine();

            int iFastKPeriod, iSlowKPeriod, iSlowDPeriod;
            double slowK = 0.00, slowD = 0.00, highestHigh = 0.00, lowestLow = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            List<double> listHigh = new List<double>();
            List<double> listClose = new List<double>();
            List<double> listLow = new List<double>();
            List<double> listHighestHigh = new List<double>();
            List<double> listLowestLow = new List<double>();
            List<double> listSlowK = new List<double>();

            int rownum, startSlowK, startSlowD;
            try
            {
                iFastKPeriod = System.Convert.ToInt32(fastkperiod);
                iSlowKPeriod = System.Convert.ToInt32(slowkperiod);
                iSlowDPeriod = System.Convert.ToInt32(slowdperiod);

                startSlowK = 0; startSlowD = 0;

                for (rownum = 0; rownum < dailyDataTable.Rows.Count; rownum++)
                {
                    listClose.Add(System.Convert.ToDouble(dailyDataTable.Rows[rownum]["Close"]));
                    listHigh.Add(System.Convert.ToDouble(dailyDataTable.Rows[rownum]["High"]));
                    listLow.Add(System.Convert.ToDouble(dailyDataTable.Rows[rownum]["Low"]));
                    
                    if ((rownum + 1) >= iFastKPeriod) //CASE of iFastKPeriod = 5: rownum = 4, 5th or higher row
                    {
                        highestHigh = StockApi.FindHighestHigh(listHigh, startSlowK, iFastKPeriod);
                        listHighestHigh.Add(highestHigh);

                        lowestLow = StockApi.FindLowestLow(listLow, startSlowK, iFastKPeriod);
                        listLowestLow.Add(lowestLow);

                        startSlowK++;

                        slowK = StockApi.FindSlowK(listClose, listHighestHigh, listLowestLow);
                        listSlowK.Add(slowK);

                        if((rownum+1) >= (iFastKPeriod + iSlowDPeriod)) //CASE of iSlowDPeriod = 3: rownum = 7, 8th or higher row
                        {
                            slowD = StockApi.FindSlowD(listSlowK, startSlowD, iSlowDPeriod);
                            startSlowD++;

                            //now save the datat
                            dateCurrentRow = System.Convert.ToDateTime(dailyDataTable.Rows[rownum]["Date"]);
                            returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                            returnString.Append(string.Format("{0:0.0000}", slowD) + ",");
                            returnString.Append(string.Format("{0:0.0000}", slowK));
                            returnString.AppendLine();
                        }
                    }
                }
                listHigh.Clear();
                listHigh = null;
                listClose.Clear();
                listClose = null;
                listLow.Clear();
                listLow = null;
                listHighestHigh.Clear();
                listHighestHigh = null;
                listLowestLow.Clear();
                listLowestLow = null;
                listSlowK.Clear();
                listSlowK = null;

                return returnString.ToString();
            }
            catch (Exception ex)
            {
                returnString.Clear();
                listHigh.Clear();
                listClose.Clear();
                listLow.Clear();
                listHighestHigh.Clear();
                listLowestLow.Clear();
                listSlowK.Clear();
            }
            return null;
        }

#endregion >> Methods that return string with data to be written to file. They get data from JSON or input data table

#region >>Methods that return DataTable filled with data from JSON or DataTable
        public static DataTable getVWAPDataTableFromJSON(string filename, DataTable intraDataTable, string scriptName)
        {
            double high, low, close, avgprice = 0.00, cumavgpricevol = 0.00, vwap = 0.00, prev_cumavgpricevol = 0.00;
            DateTime transDate;
            long volume, cumvol = 0, prev_cumvol = 0;
            StringBuilder returnString = new StringBuilder("time,VWAP");
            returnString.AppendLine();

            DataTable vwapDataTable = null;

            try
            {
                vwapDataTable = new DataTable();

                vwapDataTable.Columns.Add("Symbol", typeof(string));
                vwapDataTable.Columns.Add("Date", typeof(DateTime));
                vwapDataTable.Columns.Add("VWAP", typeof(decimal));


                for (int i = 0; i < intraDataTable.Rows.Count; i++)
                {
                    //find all the values
                    transDate = System.Convert.ToDateTime(intraDataTable.Rows[i]["Date"]);
                    high = System.Convert.ToDouble(intraDataTable.Rows[i]["High"]);
                    low = System.Convert.ToDouble(intraDataTable.Rows[i]["Low"]);
                    close = System.Convert.ToDouble(intraDataTable.Rows[i]["Close"]);
                    volume = System.Convert.ToInt64(intraDataTable.Rows[i]["Volume"]);
                    if (volume == 0)
                        continue;

                    avgprice = (high + low + close) / 3;

                    cumavgpricevol = (avgprice * volume) + prev_cumavgpricevol;
                    prev_cumavgpricevol = cumavgpricevol;

                    cumvol = volume + prev_cumvol;
                    prev_cumvol = cumvol;
                    vwap = cumavgpricevol / cumvol;

                    vwapDataTable.Rows.Add(new object[] {
                                                        scriptName,
                                                        //System.Convert.ToDateTime(field[0]).ToString("yyyy-MM-dd"),
                                                        transDate,
                                                        Math.Round(vwap, 4)
                                                    });
                    returnString.Append((transDate.ToString("yyyy-MM-dd HH:mm") + ","));
                    returnString.Append(string.Format("{0:0.0000}", vwap));
                    if ((i + 1) < intraDataTable.Rows.Count)
                    {
                        returnString.AppendLine();
                    }
                }
                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }
            }
            catch (Exception ex)
            {
                if (vwapDataTable != null)
                {
                    vwapDataTable.Clear();
                    vwapDataTable.Dispose();
                }
                vwapDataTable = null;
            }
            returnString.Clear();
            returnString = null;
            return vwapDataTable;
        }

        //used by global quote
        static public DataTable getQuoteTableFromJSON(string record, string symbol, bool bIsDaily = true)
        {
            DataTable resultDataTable = null;
            DateTime myDate;
            double close;
            double high;
            double low;
            double open;
            int volume;
            double change;
            double changepercent;
            double prevclose;
            //double adjusetedClose = 0.00;
            //string formatedDate;
            var errors = new List<string>();
            try
            {
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                        //args.ErrorContext.Handled = false;
                    }
                    //Converters = { new IsoDateTimeConverter() }

                });

                Chart myChart = myDeserializedClass.chart;

                Result myResult = myChart.result[0];

                Meta myMeta = myResult.meta;

                Indicators myIndicators = myResult.indicators;

                //this will be typically only 1 row and quote will have list of close, high, low, open, volume
                Quote myQuote = myIndicators.quote[0];

                //this will be typically only 1 row and adjClose will have list of adjClose
                Adjclose myAdjClose = null;
                if (bIsDaily)
                {
                    myAdjClose = myIndicators.adjclose[0];
                }

                if (myResult.timestamp != null)
                {
                    resultDataTable = new DataTable();

                    resultDataTable.Columns.Add("Symbol", typeof(string));
                    resultDataTable.Columns.Add("Open", typeof(decimal));
                    resultDataTable.Columns.Add("High", typeof(decimal));
                    resultDataTable.Columns.Add("Low", typeof(decimal));
                    resultDataTable.Columns.Add("Price", typeof(decimal));
                    resultDataTable.Columns.Add("Volume", typeof(int));
                    resultDataTable.Columns.Add("latestDay", typeof(DateTime));
                    resultDataTable.Columns.Add("previousClose", typeof(decimal));
                    resultDataTable.Columns.Add("change", typeof(decimal));
                    resultDataTable.Columns.Add("changePercent", typeof(decimal));

                    for (int i = 0; i < myResult.timestamp.Count; i++)
                    {
                        if ((myQuote.close[i] == null) && (myQuote.high[i] == null) && (myQuote.low[i] == null) && (myQuote.open[i] == null)
                            && (myQuote.volume[i] == null))
                        {
                            continue;
                        }

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
                        myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp[i], myMeta.timezone);

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]);
                        //string formatedDate = myDate.ToString("dd-MM-yyyy");
                        //formatedDate = myDate.ToString("yyyy-dd-MM");

                        //myDate = System.Convert.ToDateTime(myResult.timestamp[i]);

                        //if all are null do not enter this row

                        if (myQuote.close[i] == null)
                        {
                            close = 0.00;
                        }
                        else
                        {
                            //close = (double)myQuote.close[i];
                            close = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.close[i]));
                        }

                        if (myQuote.high[i] == null)
                        {
                            high = 0.00;
                        }
                        else
                        {
                            //high = (double)myQuote.high[i];
                            high = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.high[i]));
                        }

                        if (myQuote.low[i] == null)
                        {
                            low = 0.00;
                        }
                        else
                        {
                            //low = (double)myQuote.low[i];
                            low = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.low[i]));
                        }

                        if (myQuote.open[i] == null)
                        {
                            open = 0.00;
                        }
                        else
                        {
                            //open = (double)myQuote.open[i];
                            open = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.open[i]));
                        }
                        if (myQuote.volume[i] == null)
                        {
                            volume = 0;
                        }
                        else
                        {
                            volume = (int)myQuote.volume[i];
                        }
                        prevclose = System.Convert.ToDouble(string.Format("{0:0.00}", myMeta.chartPreviousClose));
                        change = close - prevclose;
                        changepercent = (change / prevclose) * 100;
                        change = System.Convert.ToDouble(string.Format("{0:0.00}", change));
                        changepercent = System.Convert.ToDouble(string.Format("{0:0.00}", changepercent));

                        //if (bIsDaily)
                        //{
                        //    if (myAdjClose.adjclose[i] == null)
                        //    {
                        //        adjusetedClose = 0.00;
                        //    }
                        //    else
                        //    {
                        //        //adjusetedClose = (double)myAdjClose.adjclose[i];
                        //        adjusetedClose = System.Convert.ToDouble(string.Format("{0:0.00}", myAdjClose.adjclose[i]));
                        //    }
                        //}

                        resultDataTable.Rows.Add(new object[] {
                                                                    symbol,
                                                                    Math.Round(open, 4),
                                                                    Math.Round(high, 4),
                                                                    Math.Round(low, 4),
                                                                    Math.Round(close, 4),
                                                                    volume,
                                                                    myDate,
                                                                    Math.Round(prevclose, 4),
                                                                    Math.Round(change, 4),
                                                                    Math.Round(changepercent, 4)
                                                                    //adjusetedClose
                                                                });
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


        //used for daily & intra
        static public DataTable getDailyIntraDataTableFromJSON(string filename, string record, string symbol, bool bIsDaily = true)
        {
            //Root myDeserializedClass = (Root)JsonConvert.DeserializeObject(record);

            //first try
            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record);
            StringBuilder returnString = new StringBuilder("timestamp,open,high,low,close,volume");
            returnString.AppendLine();

            DataTable dt = null;
            DateTime myDate;
            double close;
            double high;
            double low;
            double open;
            long volume;
            //double adjusetedClose = 0.00;
            //string formatedDate;
            var errors = new List<string>();

            try
            {
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                        //args.ErrorContext.Handled = false;
                    }
                    //Converters = { new IsoDateTimeConverter() }

                });

                Chart myChart = myDeserializedClass.chart;

                Result myResult = myChart.result[0];

                Meta myMeta = myResult.meta;

                Indicators myIndicators = myResult.indicators;

                //this will be typically only 1 row and quote will have list of close, high, low, open, volume
                Quote myQuote = myIndicators.quote[0];

                //this will be typically only 1 row and adjClose will have list of adjClose
                Adjclose myAdjClose = null;
                if (bIsDaily)
                {
                    myAdjClose = myIndicators.adjclose[0];
                }

                if (myResult.timestamp != null)
                {
                    dt = new DataTable();

                    dt.Columns.Add("Symbol", typeof(string));
                    dt.Columns.Add("Date", typeof(DateTime));
                    dt.Columns.Add("Open", typeof(decimal));
                    dt.Columns.Add("High", typeof(decimal));
                    dt.Columns.Add("Low", typeof(decimal));
                    dt.Columns.Add("Close", typeof(decimal));
                    dt.Columns.Add("Volume", typeof(long));
                    dt.Columns.Add("PurchaseDate", typeof(string));
                    dt.Columns.Add("CumulativeQuantity", typeof(int));
                    dt.Columns.Add("CostofInvestment", typeof(decimal));
                    dt.Columns.Add("ValueOnDate", typeof(decimal));

                    //if (bIsDaily)
                    //{
                    //    dt.Columns.Add("AdjClose", typeof(decimal));
                    //}


                    for (int i = 0; i < myResult.timestamp.Count; i++)
                    {
                        if ((myQuote.close[i] == null) && (myQuote.high[i] == null) && (myQuote.low[i] == null) && (myQuote.open[i] == null)
                            && (myQuote.volume[i] == null))
                        {
                            continue;
                        }

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
                        myDate = StockApi.convertUnixEpochToLocalDateTime(myResult.timestamp[i], myMeta.timezone);

                        //myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]);

                        if (bIsDaily)
                        {
                            //rowToWrite += (myDate.ToString("yyyy-MM-dd") + ",");
                            returnString.Append(myDate.ToString("yyyy-MM-dd") + ",");
                        }
                        else
                        {
                            //rowToWrite += (myDate.ToString("yyyy-MM-dd HH:mm") + ",");
                            returnString.Append(myDate.ToString("yyyy-MM-dd HH:mm") + ",");
                        }

                        if (myQuote.open[i] == null)
                        {
                            open = 0.00;
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //open = (double)myQuote.open[i];
                            open = System.Convert.ToDouble(string.Format("{0:0.0000}", myQuote.open[i]));
                            returnString.Append(string.Format("{0:0.0000}", myQuote.open[i]) + ",");
                        }

                        if (myQuote.high[i] == null)
                        {
                            high = 0.00;
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //high = (double)myQuote.high[i];
                            high = System.Convert.ToDouble(string.Format("{0:0.0000}", myQuote.high[i]));
                            returnString.Append(string.Format("{0:0.0000}", myQuote.high[i]) + ",");
                        }

                        if (myQuote.low[i] == null)
                        {
                            low = 0.00;
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //low = (double)myQuote.low[i];
                            low = System.Convert.ToDouble(string.Format("{0:0.0000}", myQuote.low[i]));
                            returnString.Append(string.Format("{0:0.0000}", myQuote.low[i]) + ",");
                        }

                        if (myQuote.close[i] == null)
                        {
                            close = 0.00;
                            returnString.Append("0.00,");
                        }
                        else
                        {
                            //close = (double)myQuote.close[i];
                            close = System.Convert.ToDouble(string.Format("{0:0.00}", myQuote.close[i]));
                            returnString.Append(string.Format("{0:0.0000}", myQuote.close[i]) + ",");
                        }

                        if (myQuote.volume[i] == null)
                        {
                            volume = 0;
                            returnString.Append("0,");
                        }
                        else
                        {
                            volume = (long)myQuote.volume[i];
                            returnString.Append(string.Format("{0:0}", myQuote.volume[i]));
                        }

                        //if (bIsDaily)
                        //{
                        //    if (myAdjClose.adjclose[i] == null)
                        //    {
                        //        adjusetedClose = 0.00;
                        //    }
                        //    else
                        //    {
                        //        //adjusetedClose = (double)myAdjClose.adjclose[i];
                        //        adjusetedClose = System.Convert.ToDouble(string.Format("{0:0.00}", myAdjClose.adjclose[i]));
                        //    }
                        //}

                        dt.Rows.Add(new object[] {
                                symbol,
                                myDate,
                                Math.Round(open, 4),
                                Math.Round(high, 4),
                                Math.Round(low, 4),
                                Math.Round(close, 4),
                                volume
                                //adjusetedClose
                            });

                        if ((i + 1) < myResult.timestamp.Count)
                        {
                            returnString.AppendLine();
                        }
                    }

                    if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                    {
                        File.WriteAllText(filename, returnString.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt.Dispose();
                }
                dt = null;
            }
            returnString.Clear();
            returnString = null;
            return dt;
        }

        //used by daily & intra

        public static DataTable getSMADataTableFromDailyForTable(string filename, DataTable dailyTable, string scriptName, string day_interval = "daily", 
            string period = "20", string seriestype = "close")
        {
            DataTable smaDataTable = null;
            int iPeriod;
            double sumOfSeriesType;
            double columnValue;
            double sma;
            DateTime dateLastRow = DateTime.Today;
            int subrownum;

            StringBuilder returnString = new StringBuilder("time,SMA");
            returnString.AppendLine();

            try
            {
                iPeriod = System.Convert.ToInt32(period);
                smaDataTable = new DataTable();

                smaDataTable.Columns.Add("Symbol", typeof(string));
                smaDataTable.Columns.Add("Date", typeof(DateTime));
                smaDataTable.Columns.Add("SMA", typeof(decimal));
                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = 0; rownum < dailyTable.Rows.Count; rownum++)
                {
                    sumOfSeriesType = 0.00;
                    //add the seriestype column values from dailytable from next "iPeriod" number of rows
                    for (subrownum = rownum; ((subrownum < (rownum + iPeriod)) && (subrownum < dailyTable.Rows.Count)); subrownum++)
                    {
                        columnValue = System.Convert.ToDouble(dailyTable.Rows[subrownum][seriestype]);
                        dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[subrownum]["Date"]);
                        sumOfSeriesType += columnValue;
                    }
                    //Find average
                    sma = sumOfSeriesType / iPeriod;
                    //add to sma table
                    smaDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(sma, 4)
                                                                });

                    returnString.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", sma));

                    //if we have reached last row then break from main for
                    if (subrownum >= dailyTable.Rows.Count)
                        break;
                    else
                    {
                        returnString.AppendLine();
                    }

                }
                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }

                //we have our sma table
            }
            catch (Exception ex)
            {
                if (smaDataTable != null)
                {
                    smaDataTable.Clear();
                    smaDataTable.Dispose();
                }
                smaDataTable = null;
            }
            returnString.Clear();
            returnString = null;
            return smaDataTable;
        }

        public static DataTable getEMADataTableFromDailyForTable(string filename, DataTable dailyTable, string scriptName, string day_interval = "daily",
                                                                string period = "20", string seriestype = "close")
        {
            StringBuilder returnString = new StringBuilder("time,EMA");
            returnString.AppendLine();
            double ema = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            DataTable emaDataTable = null;
            int iPeriod, rownum;
            try
            {
                emaDataTable = new DataTable();

                emaDataTable.Columns.Add("Symbol", typeof(string));
                emaDataTable.Columns.Add("Date", typeof(DateTime));
                emaDataTable.Columns.Add("EMA", typeof(decimal));

                //Strat from 1st row in smaTable and get the row from smaTable where dailyTable's Date matches current Date row from smaTable
                //            Multiplier: (2 / (Time periods + 1) ) 
                //EMA: { Close - EMA(previous day)} x multiplier +EMA(previous day)
                //Here Time period is the number of days you want to look back.
                //For the 1st value = average of close. 
                //since we don’t have EMA for the first time, we just take simple moving average on the 10th day. 
                //From 11th day onwards we start calculating EMA
                iPeriod = System.Convert.ToInt32(period);
                rownum = iPeriod - 1;
                ema = StockApi.FindEMA(rownum, iPeriod, seriestype, dailyTable, ema);
                dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

                emaDataTable.Rows.Add(new object[] {
                                                        scriptName,
                                                        dateCurrentRow.ToString("yyyy-MM-dd"),
                                                        Math.Round(ema, 4)
                                                    });

                returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                returnString.Append(string.Format("{0:0.0000}", ema));
                returnString.AppendLine();

                for (rownum = iPeriod; rownum < dailyTable.Rows.Count; rownum++)
                {
                    dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

                    ema = StockApi.FindEMA(rownum, iPeriod, seriestype, dailyTable, ema);

                    emaDataTable.Rows.Add(new object[] {
                                                        scriptName,
                                                        dateCurrentRow.ToString("yyyy-MM-dd"),
                                                        Math.Round(ema, 4)
                                                    });

                    returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", ema));
                    returnString.AppendLine();
                }
                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }
            }
            catch (Exception ex)
            {
                if (emaDataTable != null)
                {
                    emaDataTable.Clear();
                    emaDataTable.Dispose();
                }
                emaDataTable = null;
            }
            returnString.Clear();
            returnString = null;
            return emaDataTable;
        }

        public static DataTable getRSIDataTableFromDailyForTable(string filename, DataTable dailyTable, string scriptName, string day_interval = "daily", 
            string period = "20", string seriestype = "close")
        {
            DataTable rsiDataTable = null;
            StringBuilder returnString = new StringBuilder("time,RSI");
            returnString.AppendLine();
            int iPeriod;
            double change, gain, loss, avgGain = 0.00, avgLoss = 0.00, rs, rsi;
            double sumOfGain = 0.00, sumOfLoss = 0.00;
            DateTime dateCurrentRow = DateTime.Today;

            try
            {
                iPeriod = System.Convert.ToInt32(period);
                rsiDataTable = new DataTable();

                rsiDataTable.Columns.Add("Symbol", typeof(string));
                rsiDataTable.Columns.Add("Date", typeof(DateTime));
                rsiDataTable.Columns.Add("RSI", typeof(decimal));
                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = 1; rownum < dailyTable.Rows.Count; rownum++)
                {
                    //current - prev
                    change = System.Convert.ToDouble(dailyTable.Rows[rownum][seriestype]) - System.Convert.ToDouble(dailyTable.Rows[rownum - 1][seriestype]);
                    dateCurrentRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

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
                        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(string.Format("{0:0.0000}", rsi));
                        returnString.AppendLine();
                        rsiDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateCurrentRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(rsi, 4)
                                                                });
                    }
                    else
                    {
                        avgGain = ((avgGain * (iPeriod - 1)) + gain) / iPeriod;
                        avgLoss = ((avgLoss * (iPeriod - 1)) + loss) / iPeriod;
                        rs = avgGain / avgLoss;
                        rsi = 100 - (100 / (1 - rs));
                        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(string.Format("{0:0.0000}", rsi));
                        returnString.AppendLine();
                        rsiDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateCurrentRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(rsi, 4)
                                                                });
                    }
                }
                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
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
            returnString.Clear();
            returnString = null;
            return rsiDataTable;
        }

        public static DataTable getBBandsDataTableFromDailySMAForTable(string filename, DataTable dailyTable, DataTable smaTable, string scriptName,
                                        string day_interval = "daily", string period = "20", string seriestype = "close",
                                        string nbdevup = "2", string nbdevdn = "2")
        {
            StringBuilder returnString = new StringBuilder("time,Real Lower Band,Real Middle Band,Real Upper Band");
            returnString.AppendLine();
            double sma, upperBand, lowerBand;
            DataRow[] smaRows;
            int subrownum;
            int iPeriod;
            double pricecolumnValue;
            DateTime dateLastRow = DateTime.Today;

            double standardDevUpper, standardDevLower;
            double M;
            double S;
            int k;
            double tmpM;
            int upFactor = System.Convert.ToInt32(nbdevup);
            int dnFactor = System.Convert.ToInt32(nbdevdn);
            DataTable bbandsDataTable = null;
            try
            {
                iPeriod = System.Convert.ToInt32(period);

                bbandsDataTable = new DataTable();

                bbandsDataTable.Columns.Add("Symbol", typeof(string));
                bbandsDataTable.Columns.Add("Date", typeof(DateTime));
                bbandsDataTable.Columns.Add("Real Lower Band", typeof(decimal));
                bbandsDataTable.Columns.Add("Real Middle Band", typeof(decimal));
                bbandsDataTable.Columns.Add("Real Upper Band", typeof(decimal));


                for (int rownum = 0; (rownum + 1) < dailyTable.Rows.Count; rownum++)
                {
                    M = 0.0;
                    S = 0.0;
                    k = 1;
                    //find the standard deviation of price
                    for (subrownum = rownum; ((subrownum < (rownum + iPeriod)) && (subrownum < dailyTable.Rows.Count)); subrownum++)
                    {
                        pricecolumnValue = System.Convert.ToDouble(dailyTable.Rows[subrownum][seriestype]);
                        dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[subrownum]["Date"]);

                        tmpM = M;
                        M += (pricecolumnValue - tmpM) / k;
                        S += (pricecolumnValue - tmpM) * (pricecolumnValue - M);
                        k++;
                    }
                    standardDevUpper = Math.Sqrt(S / (k - upFactor));
                    standardDevLower = Math.Sqrt(S / (k - dnFactor));

                    //get the SMA for the last row date
                    smaRows = smaTable.Select("Date = '" + dateLastRow.Date.ToString() + "'");
                    sma = 0.00;
                    if ((smaRows != null) && (smaRows.Length > 0))
                    {
                        sma = System.Convert.ToDouble(smaRows[0]["SMA"]);
                    }

                    //Find upper & lower bands
                    upperBand = sma + (standardDevUpper * upFactor);
                    lowerBand = sma - (standardDevLower * dnFactor);

                    bbandsDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(lowerBand, 4),
                                                                    Math.Round(sma, 4),
                                                                    Math.Round(upperBand, 4)
                                                                });

                    returnString.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", lowerBand) + ",");
                    returnString.Append(string.Format("{0:0.0000}", sma) + ",");
                    returnString.Append(string.Format("{0:0.0000}", upperBand));
                    returnString.AppendLine();
                }

                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }

                //we have our sma table
            }
            catch (Exception ex)
            {
                if (bbandsDataTable != null)
                {
                    bbandsDataTable.Clear();
                    bbandsDataTable.Dispose();
                }
                bbandsDataTable = null;
            }
            returnString.Clear();
            returnString = null;
            return bbandsDataTable;
        }

        public static DataTable getAROONDataTableFromDailyForTable(string filename, DataTable dailyTable, string scriptName, string day_interval = "daily",
                                string period = "20")
        {
            DataTable aroonDataTable = null;
            StringBuilder returnString = new StringBuilder("time,Aroon Down,Aroon Up");
            returnString.AppendLine();
            int iPeriod;

            double aroonUp, aroonDown;
            DateTime dateLastRow = DateTime.Today;

            try
            {
                iPeriod = System.Convert.ToInt32(period);
                aroonDataTable = new DataTable();

                aroonDataTable.Columns.Add("Symbol", typeof(string));
                aroonDataTable.Columns.Add("Date", typeof(DateTime));
                aroonDataTable.Columns.Add("Aroon Down", typeof(decimal));
                aroonDataTable.Columns.Add("Aroon Up", typeof(decimal));

                for (int rownum = iPeriod; rownum < dailyTable.Rows.Count; rownum++)
                {
                    aroonUp = CalculateAroonUp(rownum, iPeriod, dailyTable);
                    aroonDown = CalculateAroonDown(rownum, iPeriod, dailyTable);
                    dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);

                    aroonDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(aroonDown, 4),
                                                                    Math.Round(aroonUp, 4)
                                                                });

                    returnString.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(string.Format("{0:0.0000}", aroonDown) + ",");
                    returnString.Append(string.Format("{0:0.0000}", aroonUp));
                    returnString.AppendLine();
                }

                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }
            }
            catch (Exception ex)
            {
                if (aroonDataTable != null)
                {
                    aroonDataTable.Clear();
                    aroonDataTable.Dispose();
                }
                aroonDataTable = null;
            }
            returnString.Clear();
            returnString = null;
            return aroonDataTable;
        }

        //returnType must be either ADX DX PLUS_DM MINUS_DM PLUS_DI MINUS_DI
        public static DataTable getADXDataTableFromDailyForTable(string folderPath, DataTable dailyTable, string scriptName, string day_interval = "daily",
                                                            string period = "20", string outputsize = "full", string returnType = "ADX")
        {
            DataTable adxDataTable = null;


            StringBuilder returnStringADX = new StringBuilder("time,ADX");
            returnStringADX.AppendLine();

            StringBuilder returnStringDX = new StringBuilder("time,DX");
            returnStringDX.AppendLine();

            StringBuilder returnStringPlusDM = new StringBuilder("time,PLUS_DM");
            returnStringPlusDM.AppendLine();

            StringBuilder returnStringMinusDM = new StringBuilder("time,MINUS_DM");
            returnStringMinusDM.AppendLine();

            StringBuilder returnStringPlusDI = new StringBuilder("time,PLUSDI");
            returnStringPlusDI.AppendLine();

            StringBuilder returnStringMinusDI = new StringBuilder("time,MINUSDI");
            returnStringMinusDI.AppendLine();

            int iPeriod;
            DateTime dateLastRow = DateTime.Today;

            double tr, minusDM, plusDM, trPeriod, minusDMPeriod, plusDMPeriod, minusDIPeriod, plusDIPeriod, dx, adx;
            List<double> listTR = new List<double>();
            List<double> listPlusDM1 = new List<double>();
            List<double> listMinusDM1 = new List<double>();
            List<double> listTRPeriod = new List<double>();
            List<double> listPlusDMPeriod = new List<double>();
            List<double> listMinusDMPeriod = new List<double>();
            List<double> listPlusDIPeriod = new List<double>();
            List<double> listMinusDIPeriod = new List<double>();
            List<double> listDX = new List<double>();
            List<double> listADX = new List<double>();

            try
            {
                string filenameADX = folderPath + scriptName + "_" + "ADX_" + day_interval + "_" + period + "_" + outputsize + ".csv";
                string filenameDX = folderPath + scriptName + "_" + "DX_" + day_interval + "_" + period + "_" + outputsize + ".csv";
                string filenamePlusDM = folderPath + scriptName + "_" + "PLUS_DM_" + day_interval + "_" + period + "_" + outputsize + ".csv";
                string filenameMinusDM = folderPath + scriptName + "_" + "MINUS_DM_" + day_interval + "_" + period + "_" + outputsize + ".csv";
                string filenamePlusDI = folderPath + scriptName + "_" + "PLUS_DI_" + day_interval + "_" + period + "_" + outputsize + ".csv";
                string filenameMinusDI = folderPath + scriptName + "_" + "MINUS_DI_" + day_interval + "_" + period + "_" + outputsize + ".csv";

                adxDataTable = new DataTable();

                adxDataTable.Columns.Add("Symbol", typeof(string));
                adxDataTable.Columns.Add("Date", typeof(DateTime));
                adxDataTable.Columns.Add(returnType, typeof(decimal));


                iPeriod = System.Convert.ToInt32(period);

                //Strat from 1st row in dailyTable and sum all the "seriestype" column upto "period"
                //SMA = divide the sum by "period"
                //Store the symbol, Date from the last row of the current set and SMA in the smaDataTable

                for (int rownum = 1; rownum < dailyTable.Rows.Count; rownum++)
                {
                    dateLastRow = System.Convert.ToDateTime(dailyTable.Rows[rownum]["Date"]);
                    tr = StockApi.FindTR1(rownum, dailyTable);
                    listTR.Add(tr);

                    plusDM = StockApi.FindPositiveDM1(rownum, dailyTable);
                    listPlusDM1.Add(plusDM);

                    minusDM = StockApi.FindNegativeDM1(rownum, dailyTable);
                    listMinusDM1.Add(minusDM);

                    if (rownum >= iPeriod)
                    {
                        trPeriod = StockApi.FindTR_Period(rownum, iPeriod, listTR, listTRPeriod);
                        listTRPeriod.Add(trPeriod);

                        plusDMPeriod = StockApi.FindPositveDM_Period(rownum, iPeriod, listPlusDM1, listPlusDMPeriod);
                        listPlusDMPeriod.Add(plusDMPeriod);
                        returnStringPlusDM.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringPlusDM.Append(string.Format("{0:0.0000}", plusDMPeriod));
                        returnStringPlusDM.AppendLine();

                        minusDMPeriod = StockApi.FindNegativeDM_Period(rownum, iPeriod, listMinusDM1, listMinusDMPeriod);
                        listMinusDMPeriod.Add(minusDMPeriod);
                        returnStringMinusDM.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringMinusDM.Append(string.Format("{0:0.0000}", minusDMPeriod));
                        returnStringMinusDM.AppendLine();

                        plusDIPeriod = StockApi.FindPositveDI_Period(rownum, iPeriod, listTRPeriod, listPlusDMPeriod);
                        listPlusDIPeriod.Add(plusDIPeriod);
                        returnStringPlusDI.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringPlusDI.Append(string.Format("{0:0.0000}", plusDIPeriod));
                        returnStringPlusDI.AppendLine();

                        minusDIPeriod = StockApi.FindNegativeDI_Period(rownum, iPeriod, listTRPeriod, listMinusDMPeriod);
                        listMinusDIPeriod.Add(minusDIPeriod);
                        returnStringMinusDI.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringMinusDI.Append(string.Format("{0:0.0000}", minusDIPeriod));
                        returnStringMinusDI.AppendLine();

                        dx = StockApi.FindDX(rownum, iPeriod, listPlusDIPeriod, listMinusDIPeriod);
                        listDX.Add(dx);
                        returnStringDX.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                        returnStringDX.Append(string.Format("{0:0.0000}", dx));
                        returnStringDX.AppendLine();

                        if ((rownum + 1) >= (iPeriod * 2))
                        {
                            adx = StockApi.FindADX(rownum, iPeriod, listDX, listADX);
                            listADX.Add(adx);
                            returnStringADX.Append(dateLastRow.ToString("yyyy-MM-dd") + ",");
                            returnStringADX.Append(string.Format("{0:0.0000}", adx));
                            returnStringADX.AppendLine();
                            if (returnType.Equals("ADX"))
                            {
                                adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(adx, 4)
                                                                });
                            }
                        }

                        //returnType must be either ADX DX PLUS_DM MINUS_DM PLUS_DI MINUS_DI
                        if (returnType.Equals("DX"))
                        {
                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(dx, 4)
                                                                });
                        }
                        else if (returnType.Equals("PLUS_DM"))
                        {
                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(plusDMPeriod, 4)
                                                                });
                        }
                        else if (returnType.Equals("MINUS_DM"))
                        {
                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(minusDMPeriod, 4)
                                                                });
                        }
                        else if (returnType.Equals("PLUS_DI"))
                        {
                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(plusDIPeriod, 4)
                                                                });
                        }
                        else if (returnType.Equals("MINUS_DI"))
                        {
                            adxDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateLastRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(minusDIPeriod, 4)
                                                                });
                        }

                    }
                }

                //Save all strings
                File.WriteAllText(filenameADX, returnStringADX.ToString());
                File.WriteAllText(filenameDX, returnStringDX.ToString());
                File.WriteAllText(filenamePlusDM, returnStringPlusDM.ToString());
                File.WriteAllText(filenameMinusDM, returnStringMinusDM.ToString());
                File.WriteAllText(filenamePlusDI, returnStringPlusDI.ToString());
                File.WriteAllText(filenameMinusDI, returnStringMinusDI.ToString());

            }
            catch (Exception ex)
            {
                if (adxDataTable != null)
                {
                    adxDataTable.Clear();
                    adxDataTable.Dispose();
                }
                adxDataTable = null;
            }
            listTR.Clear();
            listPlusDM1.Clear();
            listMinusDM1.Clear();
            listTRPeriod.Clear();
            listPlusDMPeriod.Clear();
            listMinusDMPeriod.Clear();
            listPlusDIPeriod.Clear();
            listMinusDIPeriod.Clear();
            listDX.Clear();
            listADX.Clear();

            returnStringADX.Clear();
            returnStringDX.Clear();
            returnStringPlusDM.Clear();
            returnStringMinusDM.Clear();
            returnStringPlusDI.Clear();
            returnStringMinusDI.Clear();


            return adxDataTable;
        }

        public static DataTable getMACDDataForTable(string filename, DataTable emaFastTable, DataTable emaSlowTable, string scriptName,
                        string day_interval = "daily", string fastperiod = "12", string slowperiod = "26", string signalperiod = "9")
        {
            DataTable macdDataTable = null;
            StringBuilder returnString = new StringBuilder("time,MACD,MACD_Hist,MACD_Signal");
            returnString.AppendLine();
            int iSlowPeriod, iSignalPeriod, iFastPeriod;
            double macd = 0.00, signal = 0.00, histogram = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            List<double> listMACD = new List<double>();
            int emaFastIndex;

            try
            {
                iSlowPeriod = System.Convert.ToInt32(slowperiod);
                iSignalPeriod = System.Convert.ToInt32(signalperiod);
                iFastPeriod = System.Convert.ToInt32(fastperiod);

                emaFastIndex = iSlowPeriod - iFastPeriod;

                macdDataTable = new DataTable();
                macdDataTable.Columns.Add("Symbol", typeof(string));
                macdDataTable.Columns.Add("Date", typeof(DateTime));
                macdDataTable.Columns.Add("MACD", typeof(decimal));
                macdDataTable.Columns.Add("MACD_Hist", typeof(decimal));
                macdDataTable.Columns.Add("MACD_Signal", typeof(decimal));

                for (int rownum = 0; rownum < emaSlowTable.Rows.Count; rownum++)
                {
                    dateCurrentRow = System.Convert.ToDateTime(emaFastTable.Rows[(emaFastIndex + rownum)]["Date"]);
                    macd = System.Convert.ToDouble(emaFastTable.Rows[(emaFastIndex + rownum)]["EMA"]) - System.Convert.ToDouble(emaSlowTable.Rows[rownum]["EMA"]);
                    listMACD.Add(macd);

                    if (rownum >= (iSignalPeriod - 1))
                    {
                        signal = StockApi.FindSignal(rownum, iSignalPeriod, listMACD, signal);
                        histogram = macd - signal;

                        macdDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateCurrentRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(macd, 4),
                                                                    Math.Round(histogram, 4),
                                                                    Math.Round(signal, 4)
                                                                });

                        returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                        returnString.Append(string.Format("{0:0.0000}", macd) + ",");
                        returnString.Append(string.Format("{0:0.0000}", histogram) + ",");
                        returnString.Append(string.Format("{0:0.0000}", signal));
                        returnString.AppendLine();
                    }
                }
                listMACD.Clear();
                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }
            }
            catch (Exception ex)
            {
                if (macdDataTable != null)
                {
                    macdDataTable.Clear();
                    macdDataTable.Dispose();
                }
                macdDataTable = null;
                listMACD.Clear();
            }
            returnString.Clear();
            returnString = null;
            return macdDataTable;
        }


        public static DataTable getSTOCHDataForTable(string filename, DataTable dailyDataTable, string scriptName,
                        string day_interval = "daily", string fastkperiod = "5", string slowkperiod = "3", string slowdperiod = "3", 
                        string slowkmatype = "0", string slowdmatype = "0")
        {
            DataTable stochDataTable = null;
            StringBuilder returnString = new StringBuilder("time,SlowD,SlowK");
            returnString.AppendLine();

            int iFastKPeriod, iSlowKPeriod, iSlowDPeriod;
            double slowK = 0.00, slowD = 0.00, highestHigh = 0.00, lowestLow = 0.00;
            DateTime dateCurrentRow = DateTime.Today;
            List<double> listHigh = new List<double>();
            List<double> listClose = new List<double>();
            List<double> listLow = new List<double>();
            List<double> listHighestHigh = new List<double>();
            List<double> listLowestLow = new List<double>();
            List<double> listSlowK = new List<double>();

            int rownum, startSlowK, startSlowD;

            try
            {
                iFastKPeriod = System.Convert.ToInt32(fastkperiod);
                iSlowKPeriod = System.Convert.ToInt32(slowkperiod);
                iSlowDPeriod = System.Convert.ToInt32(slowdperiod);

                startSlowK = 0; startSlowD = 0;

                stochDataTable = new DataTable();
                stochDataTable.Columns.Add("Symbol", typeof(string));
                stochDataTable.Columns.Add("Date", typeof(DateTime));
                stochDataTable.Columns.Add("SlowD", typeof(decimal));
                stochDataTable.Columns.Add("SlowK", typeof(decimal));

                for (rownum = 0; rownum < dailyDataTable.Rows.Count; rownum++)
                {
                    listClose.Add(System.Convert.ToDouble(dailyDataTable.Rows[rownum]["Close"]));
                    listHigh.Add(System.Convert.ToDouble(dailyDataTable.Rows[rownum]["High"]));
                    listLow.Add(System.Convert.ToDouble(dailyDataTable.Rows[rownum]["Low"]));

                    if ((rownum + 1) >= iFastKPeriod) //CASE of iFastKPeriod = 5: rownum = 4, 5th or higher row
                    {
                        highestHigh = StockApi.FindHighestHigh(listHigh, startSlowK, iFastKPeriod);
                        listHighestHigh.Add(highestHigh);

                        lowestLow = StockApi.FindLowestLow(listLow, startSlowK, iFastKPeriod);
                        listLowestLow.Add(lowestLow);

                        startSlowK++;

                        slowK = StockApi.FindSlowK(listClose, listHighestHigh, listLowestLow);
                        listSlowK.Add(slowK);

                        if ((rownum + 1) >= (iFastKPeriod + iSlowDPeriod)) //CASE of iSlowDPeriod = 3: rownum = 7, 8th or higher row
                        {
                            slowD = StockApi.FindSlowD(listSlowK, startSlowD, iSlowDPeriod);
                            startSlowD++;

                            //now save the datat
                            dateCurrentRow = System.Convert.ToDateTime(dailyDataTable.Rows[rownum]["Date"]);

                            stochDataTable.Rows.Add(new object[] {
                                                                    scriptName,
                                                                    dateCurrentRow.ToString("yyyy-MM-dd"),
                                                                    Math.Round(slowD, 4),
                                                                    Math.Round(slowK, 4)
                                                                });

                            returnString.Append(dateCurrentRow.ToString("yyyy-MM-dd") + ",");
                            returnString.Append(string.Format("{0:0.0000}", slowD) + ",");
                            returnString.Append(string.Format("{0:0.0000}", slowK));
                            returnString.AppendLine();
                        }
                    }
                }
                if (StockApi.isFileWriteDateEqualsToday(filename) == false)
                {
                    File.WriteAllText(filename, returnString.ToString());
                }
            }
            catch (Exception ex)
            {
                if (stochDataTable != null)
                {
                    stochDataTable.Clear();
                    stochDataTable.Dispose();
                }
                stochDataTable = null;
            }
            returnString.Clear();
            listHigh.Clear();
            listClose.Clear();
            listLow.Clear();
            listHighestHigh.Clear();
            listLowestLow.Clear();
            listSlowK.Clear();
            return stochDataTable;
        }

#endregion >>Methods that return DataTable filled with data from JSON or DataTable
#endregion

    }
}