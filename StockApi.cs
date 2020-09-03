using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        //public static string apiKey = "&apikey=XXXX";
        //public static string apiKey = "XXXX"; instead added tpo each method signature--> string apiKey = "UV6KQA6735QZKBTV")
        public static string dataType = "csv";

        public static string indicators = "quote";
        public static string includeTimestamps = "true";

        //https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=BA&apikey=demo&datatype=csv
        public static string urlSymbolSearch = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={0}&apikey={1}&datatype={2}";

        //'https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={}&apikey={}&datatype=csv'
        public static string urlGlobalQuote = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey={1}&datatype={2}";

        //https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={}&apikey={}&outputsize={}&datatype=csv
        public static string urlGetDaily = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}&outputsize={2}&datatype={3}";

        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=2yr&interval=1d&indicators=quote&includeTimestamps=true
        public static string urlGetDaily_alternate = "https://query1.finance.yahoo.com/v7/finance/chart/{0}?range={1}&interval={2}&indicators={3}&includeTimestamps={4}";

        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=2yr&interval=1d&indicators=quote&includeTimestamps=true
        public static string urlGetIntra_alternate = "https://query1.finance.yahoo.com/v7/finance/chart/{0}?range={1}&interval={2}&indicators={3}&includeTimestamps={4}";

        //https://query1.finance.yahoo.com/v7/finance/chart/HDFC.BO?range=1d&interval=1d&indicators=quote&timestamp=true
        public static string urlGlobalQuote_alternate = "https://query1.finance.yahoo.com/v7/finance/chart/{0}?range=1d&interval=1d&indicators=quote&timestamp=true";

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
                            File.WriteAllText(folderPath + scriptName + "_" + "SMA_" + day_interval + "_" + period + "_" + seriestype + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "SMA_" + day_interval + "_" + period + "_" + seriestype + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "SMA_" + day_interval + "_" + period + "_" + seriestype + ".csv");
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
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable emaDataTable = null;

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
                            File.WriteAllText(folderPath + scriptName + "_" + "EMA_" + day_interval + "_" + period + "_" + seriestype + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "EMA_" + day_interval + "_" + period + "_" + seriestype + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "EMA_" + day_interval + "_" + period + "_" + seriestype + ".csv");
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
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable rsiDataTable = null;

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
                            File.WriteAllText(folderPath + scriptName + "_" + "RSI_" + day_interval + "_" + period + "_" + seriestype + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "RSI_" + day_interval + "_" + period + "_" + seriestype + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "RSI_" + day_interval + "_" + period + "_" + seriestype + ".csv");
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

                        return rsiDataTable;
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
                            File.WriteAllText(folderPath + scriptName + "_" + "STOCH_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "STOCH_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "STOCH_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv");
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
                            File.WriteAllText(folderPath + scriptName + "_" + "MACD_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "MACD_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "MACD_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv");
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
                            File.WriteAllText(folderPath + scriptName + "_" + "AROON_" + day_interval + "_" + period + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "AROON_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "AROON_" + day_interval + "_" + period + ".csv");
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
                            File.WriteAllText(folderPath + scriptName + "_" + "ADX_" + day_interval + "_" + period + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "ADX_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "ADX_" + day_interval + "_" + period + ".csv");
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
                            File.WriteAllText(folderPath + scriptName + "_" + "BBANDS_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv", fileData);
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
                    if (File.Exists(folderPath + scriptName + "_" + "BBANDS_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "BBANDS_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv");
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
                DataTable allDataTable = new DataTable();
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
                                tempTable = getDailyRows.CopyToDataTable();

                                allDataTable.Merge(tempTable);
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
        public static DataTable getPortfolioTable(string folderPath, string portfolioFileName, bool bCurrent = true, bool bIsTestModeOn = true, string apiKey = "UV6KQA6735QZKBTV")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ScriptID", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("PurchaseDate", typeof(string));
            dt.Columns.Add("PurchasePrice", typeof(decimal));
            dt.Columns.Add("PurchaseQty", typeof(int));
            dt.Columns.Add("CommissionPaid", typeof(decimal));
            dt.Columns.Add("CostofInvestment", typeof(decimal));
            dt.Columns.Add("CumulativeQty", typeof(int));

            if (bCurrent)
            {
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("CurrentValue", typeof(decimal));
            }


            //XDocument doc = XDocument.Load(Server.MapPath(".\\data\\demo_portfolio.xml"));
            XDocument doc = XDocument.Load(portfolioFileName);
            DataTable quoteTable;
            string symbol = "";
            string price = "0.00";
            string currValue = "";
            foreach (XElement script in doc.Descendants("script"))
            {
                symbol = (string)script.Element("name");
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
                            symbol,
                            //(DateTime)DateTime.ParseExact(row.Element("PurchaseDate").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ((DateTime)(row.Element("PurchaseDate"))).ToString("yyyy-MM-dd"),
                            (decimal)row.Element("PurchasePrice"),
                            (int)row.Element("PurchaseQty"),
                            (decimal)row.Element("CommissionPaid"),
                            (decimal)row.Element("CostofInvestment"),
                            0,
                            (decimal)System.Convert.ToDecimal(price),
                            (decimal)System.Convert.ToDecimal(currValue)
                        });
                    }
                    else
                    {
                        dt.Rows.Add(new object[] {
                            symbol,
                            symbol,
                            //(DateTime)DateTime.ParseExact(row.Element("PurchaseDate").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ((DateTime)(row.Element("PurchaseDate"))).ToString("yyyy-MM-dd"),
                            (decimal)row.Element("PurchasePrice"),
                            (int)row.Element("PurchaseQty"),
                            (decimal)row.Element("CommissionPaid"),
                            (decimal)row.Element("CostofInvestment"),
                            0
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
        static public void insertNode(string filename, string symbol, string price, string date, string qty, string commission, string cost)
        {
            //string filename = "E:\\MSFT_SampleWork\\PortfolioAnalytics\\portfolio\\demo.xml";

            XmlDocument xmlPortfolio = new XmlDocument();
            XmlNode root;
            XmlNode scriptNode;
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

                scriptNode = xmlPortfolio.CreateElement("script");
                scriptNode.AppendChild(elemName);
                scriptNode.AppendChild(elemRow);

                root.AppendChild(scriptNode);
            }
            xmlPortfolio.Save(filename);

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

        static public void deleteNode(string filename, string symbol, string price, string date, string qty, string commission, string cost)
        {
            //string filename = "E:\\MSFT_SampleWork\\PortfolioAnalytics\\portfolio\\demo.xml";

            XmlDocument xmlPortfolio = new XmlDocument();
            XmlNode root;
            XmlNode scriptNode;
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
                    }
                }
            }
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

        #region yahoo_finance_api
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
        
            *******WARNING: In case of Intra, we do not get adjustedclose
            * 
            * How to use: provide range & interval in the url. Rest of the url should stay as is
            * a data class is already generated based on the json output using jsontocsharp StockDailyJson.cs & StockIntraJson.cs
            * deserialize the output of web call using Newtonsoft's DeserializeObject, this will populate appropriate classes with data
        */
        static public DataTable getDailyAlternate(string folderPath, string scriptName, string outputsize = "compact", bool bIsTestModeOn = true,
                                        bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable dailyDataTable = null;
                string convertedScriptName;
                string range, interval = "1d";
                if (outputsize.Equals("compact"))
                {
                    range = "3mo";
                }
                else
                {
                    range = "max";
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
                            File.WriteAllText(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv", fileData);
                            dailyDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                    else
                    {
                        dailyDataTable = getDailyIntraDataTableFromJSON(reader.ReadToEnd(), scriptName, bIsDaily: true);
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                }
                else
                {
                    if (File.Exists(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "Daily_" + outputsize + ".csv");
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
                return dailyDataTable;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        static public DataTable getIntradayAlternate(string folderPath, string scriptName, string time_interval = "5min", string outputsize = "compact",
                                            bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable intraDataTable = null;
                string convertedScriptName;
                string range, interval;
                if (outputsize.Equals("compact"))
                {
                    range = "60d";
                }
                else
                {
                    range = "max";
                }

                if (time_interval == "60min")
                {
                    interval = "60m";
                }
                else if (time_interval == "1min")
                {
                    interval = "1m";
                }
                else if (time_interval == "15min")
                {
                    interval = "15m";
                }
                else if (time_interval == "30min")
                {
                    interval = "30m";
                }
                else //if(time_interval == "60min")
                {
                    interval = "5m";
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
                            File.WriteAllText(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv", fileData);
                            intraDataTable = new DataTable();
                        }
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                    else
                    {
                        intraDataTable = getDailyIntraDataTableFromJSON(reader.ReadToEnd(), scriptName, bIsDaily: false);
                        reader.Close();
                        if (receiveStream != null)
                            receiveStream.Close();
                    }
                }
                else
                {
                    if (File.Exists(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "Intraday_" + time_interval + "_" + outputsize + ".csv");
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
                return intraDataTable;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        static public DataTable getVWAPAlternate(string folderPath, string scriptName, string time_interval = "5min", string outputsize = "compact",
                                    bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV", DataTable intraDataTable=null)
        {
            try
            {
                //DataTable intraDataTable;
                DataTable vwapDataTable = null;
                StreamReader reader = null;

                if (bIsTestModeOn == false)
                {
                    //we will override the savedata flag as we need the online data for intra, test mode is false
                    if (intraDataTable == null)
                    {
                        intraDataTable = StockApi.getIntradayAlternate(folderPath, scriptName, time_interval: time_interval, outputsize: outputsize,
                                            bIsTestModeOn: bIsTestModeOn, bSaveData: false, apiKey: apiKey);
                    }
                    if (intraDataTable == null)
                    {
                        return null;
                    }
                    else if (intraDataTable.Rows.Count > 0)
                    {
                        if (bSaveData)
                        {
                            string fileData = getVWAPDataFileFromJSON(intraDataTable, scriptName);
                            //if (fileData.StartsWith("{") == false)
                            if (fileData != null)
                            {
                                //(folderPath + scriptName + "_" + "VWAP_" + day_interval + ".csv")
                                File.WriteAllText(folderPath + scriptName + "_" + "VWAP_" + time_interval + ".csv", fileData);
                                intraDataTable.Clear();
                                vwapDataTable = new DataTable();
                            }
                        }
                        else
                        {
                            vwapDataTable = getVWAPDataTableFromJSON(intraDataTable, scriptName);
                        }
                    }
                }
                else
                {
                    //VWAP_LT.BSE.csv
                    if (File.Exists(folderPath + scriptName + "_" + "VWAP_" + time_interval + ".csv"))
                        reader = new StreamReader(folderPath + scriptName + "_" + "VWAP_" + time_interval + ".csv");
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
                        }
                        else
                        {
                            reader.Close();
                        }
                    }
                }
                return vwapDataTable;
            }
            catch (Exception ex)
            {
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

        public static DataTable getVWAPDataTableFromJSON(DataTable intraDataTable, string scriptName)
        {
            double high, low, close, avgprice = 0.00, cumavgpricevol = 0.00, vwap = 0.00, prev_cumavgpricevol = 0.00;
            DateTime transDate;
            long volume, cumvol = 0, prev_cumvol = 0;

            DataTable vwapDataTable;

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
                                                        vwap
                                                    });
                }
                return vwapDataTable;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        static public DataTable globalQuoteAlternate(string folderPath, string symbol, bool bIsTestModeOn = true, bool bSaveData = false, string apiKey = "UV6KQA6735QZKBTV")
        {
            try
            {
                string webservice_url = "";
                WebResponse wr;
                Stream receiveStream = null;
                StreamReader reader = null;
                DataTable resultDataTable = null;
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
                return resultDataTable;
            }
            catch (Exception ex)
            {
            }
            return null;
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

                    myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
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
                        open,
                        high,
                        low,
                        close,
                        volume,
                        myDate,
                        prevclose,
                        change,
                        changepercent
                        //adjusetedClose
                    });
                }
                return resultDataTable;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        //used for daily & intra
        static public DataTable getDailyIntraDataTableFromJSON(string record, string symbol, bool bIsDaily = true)
        {
            //Root myDeserializedClass = (Root)JsonConvert.DeserializeObject(record);

            //first try
            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(record);

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

                    myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
                    //string formatedDate = myDate.ToString("dd-MM-yyyy");

                    //if (bIsDaily)
                    //{
                    //    formatedDate = myDate.ToString("yyyy-dd-MM");
                    //}
                    //else
                    //{
                    //    formatedDate = myDate.ToString("yyyy-dd-MM HH:mm");
                    //}

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
                        volume = (long)myQuote.volume[i];
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
                        open,
                        high,
                        low,
                        close,
                        volume
                        //adjusetedClose
                    });
                }
                return dt;
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

                    myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
                    //string formatedDate = myDate.ToString("dd-MM-yyyy");
                    //string formatedDate = myDate.ToString("yyyy-dd-MM");
                    //rowToWrite += (myDate.ToString("yyyy-MM-dd") + ",");
                    returnString.Append(myDate.ToString("yyyy-MM-dd") + ",");

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
                return returnString.ToString();
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        //used by daily & intra
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

                for (int i = 0; i < myResult.timestamp.Count; i++)
                {
                    if ((myQuote.close[i] == null) && (myQuote.high[i] == null) && (myQuote.low[i] == null) && (myQuote.open[i] == null)
                        && (myQuote.volume[i] == null))
                    {
                        continue;
                    }

                    //rowToWrite = "";

                    myDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(myResult.timestamp[i]).ToLocalTime();
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
                return returnString.ToString();
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        #endregion

    }
}