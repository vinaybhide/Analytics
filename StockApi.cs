using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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
        public static string apiKey = "XXXX";
        public static string dataType = "csv";

        //https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=BA&apikey=demo&datatype=csv
        public static string urlSymbolSearch = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={0}&apikey={1}&datatype={2}";

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="outputsize">[default=compact or full]</param>
        /// <param name="bIsTestModeOn">default=true or false</param>
        /// <param name="bSaveData">default=false. To save data to file & return set it to true and set bIsTestModeOn = false</param>
        /// <returns></returns>
        public static DataTable getDaily(string folderPath, string scriptName, string outputsize = "compact", bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlGetDaily, scriptName, StockApi.apiKey, outputsize, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "Daily_" + scriptName + "_" + outputsize + ".csv", fileData);
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
                    if(File.Exists(folderPath + "Daily_" + scriptName + "_" + outputsize + ".csv"))
                        reader = new StreamReader(folderPath + "Daily_" + scriptName + "_" + outputsize + ".csv");
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
                                            bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlIntra, scriptName, time_interval, StockApi.apiKey, outputsize, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "Intraday_" + scriptName + "_" + time_interval + "_" + outputsize + ".csv", fileData);
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
                    if(File.Exists(folderPath + "Intraday_" + scriptName + "_" + time_interval + "_" + outputsize + ".csv"))
                        reader = new StreamReader(folderPath + "Intraday_" + scriptName + "_" + time_interval + "_" + outputsize + ".csv");
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
                                    string seriestype = "close", bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlSMA, scriptName, day_interval, period, seriestype, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "SMA_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv", fileData);
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
                    if(File.Exists(folderPath + "SMA_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv"))
                        reader = new StreamReader(folderPath + "SMA_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv");
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
                                        string seriestype = "close", bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlEMA, scriptName, day_interval, period, seriestype, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "EMA_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv", fileData);
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
                    if(File.Exists(folderPath + "EMA_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv"))
                        reader = new StreamReader(folderPath + "EMA_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv");
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
        /// <param name="day_interval">['1min', *'5min', '15min', '30min', '60min', *'daily', 'weekly', 'monthly']</param>
        /// <param name="bIsTestModeOn">default true, false</param>
        /// <returns></returns>
        public static DataTable getVWAP(string folderPath, string scriptName, string day_interval = "daily", bool bIsTestModeOn = true,
                                        bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlVWAP, scriptName, day_interval, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "VWAP_" + scriptName + "_" + day_interval + ".csv", fileData);
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
                    if(File.Exists(folderPath + "VWAP_" + scriptName + "_" + day_interval + ".csv"))
                        reader = new StreamReader(folderPath + "VWAP_" + scriptName + "_" + day_interval + ".csv");
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
                                        string seriestype = "close", bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlRSI, scriptName, day_interval, period, seriestype, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "RSI_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv", fileData);
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
                    if(File.Exists(folderPath + "RSI_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv"))
                        reader = new StreamReader(folderPath + "RSI_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + ".csv");
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
                                        bool bIsTestModeOn = true, bool bSaveData = false)
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
                                                    slowkmatype, slowdmatype, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "STOCH_" + scriptName + "_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv", fileData);
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
                    if(File.Exists(folderPath + "STOCH_" + scriptName + "_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv"))
                        reader = new StreamReader(folderPath + "STOCH_" + scriptName + "_" + day_interval + "_" + fastkperiod + "_" + slowkperiod + "_" + slowdperiod + "_" + slowkmatype + "_" + slowdmatype + ".csv");
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
                                        bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlMACD, scriptName, day_interval, seriestype, fastperiod, slowperiod, signalperiod, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "MACD_" + scriptName + "_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv", fileData);
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
                    if(File.Exists(folderPath + "MACD_" + scriptName + "_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv"))
                        reader = new StreamReader(folderPath + "MACD_" + scriptName + "_" + day_interval + "_" + seriestype + "_" + fastperiod + "_" + slowperiod + "_" + signalperiod + ".csv");
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
                                        bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlAROON, scriptName, day_interval, period, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "AROON_" + scriptName + "_" + day_interval + "_" + period + ".csv", fileData);
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
                    if(File.Exists(folderPath + "AROON_" + scriptName + "_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + "AROON_" + scriptName + "_" + day_interval + "_" + period + ".csv");
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
                                        bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlADX, scriptName, day_interval, period, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "ADX_" + scriptName + "_" + day_interval + "_" + period + ".csv", fileData);
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
                    if(File.Exists(folderPath + "ADX_" + scriptName + "_" + day_interval + "_" + period + ".csv"))
                        reader = new StreamReader(folderPath + "ADX_" + scriptName + "_" + day_interval + "_" + period + ".csv");
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
                                        bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlGetBBands, scriptName, day_interval, period, seriestype, nbdevup, nbdevdn, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "BBANDS_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv", fileData);
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
                    if (File.Exists(folderPath + "BBANDS_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv"))
                        reader = new StreamReader(folderPath + "BBANDS_" + scriptName + "_" + day_interval + "_" + period + "_" + seriestype + "_" + nbdevup + "_" + nbdevdn + ".csv");
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
        public static DataTable symbolSearch(string searchKeyword)
        {
            try
            {
                //https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=BA&apikey=demo&datatype=csv
                //string webservice_url = $"{urlSymbolSearch} + {searchKeyword} + {apiKey} + {dataType}";
                string webservice_url = string.Format(StockApi.urlSymbolSearch, searchKeyword, StockApi.apiKey, StockApi.dataType);
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
        public static DataTable globalQuote(string folderPath, string symbol, bool bIsTestModeOn = true, bool bSaveData = false)
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
                    webservice_url = string.Format(StockApi.urlGlobalQuote, symbol, StockApi.apiKey, StockApi.dataType);
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
                            File.WriteAllText(folderPath + "global_quote_" + symbol + ".csv", fileData);
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
                    if(File.Exists(folderPath + "global_quote_" + symbol + ".csv"))
                        reader = new StreamReader(folderPath + "global_quote_" + symbol + ".csv");
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
        public static DataTable GetValuation(string folderPath, string fileName, bool bIsTestModeOn = true)
        {
            try
            {
                DataTable portfolioTable = null;
                //DataTable orderedPortfolioTable;
                //DataTable tempTable;
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
                    portfolioTable = getPortfolioTable(folderPath, fileName, bCurrent: false, bIsTestModeOn);
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
                        getDailyScriptTable = getDaily(folderPath, scriptName, bIsTestModeOn: bIsTestModeOn);
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
                                expression = "Date >= '" + scriptRows[i]["PurchaseDate"].ToString() + "' and Date < '" + scriptRows[i + 1]["PurchaseDate"].ToString() + "'";
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
                            //recTable.Select(string.Format("[code] = '{0}'", someName)).ToList<DataRow>().ForEach(r => r["Color"] = colorValue);
                            //getDailyScriptTable.Select(expression).ToList<DataRow>().ForEach(r => r["PurchaseDate"] = scriptRows[i]["PurchaseDate"].ToString());
                            //getDailyScriptTable.Select(expression).ToList<DataRow>().ForEach(r => r["CumulativeQuantity"] = scriptRows[i]["CumulativeQty"].ToString());

                            //getDailyRows.ToList<DataRow>().ForEach(readDaily => readDaily["PurchaseDate"] = scriptRows[i]["PurchaseDate"].ToString());
                        }
                        allDataTable.Merge(getDailyScriptTable);
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
        public static DataTable getPortfolioTable(string folderPath, string portfolioFileName, bool bCurrent = true, bool bIsTestModeOn = true)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("PurchasePrice", typeof(decimal));
            dt.Columns.Add("PurchaseDate", typeof(string));
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
                    quoteTable = StockApi.globalQuote(folderPath, symbol, bIsTestModeOn);
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
                            (decimal)row.Element("PurchasePrice"),
                            //(DateTime)DateTime.ParseExact(row.Element("PurchaseDate").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ((DateTime)(row.Element("PurchaseDate"))).ToString("yyyy-MM-dd"),
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
                            (decimal)row.Element("PurchasePrice"),
                            //(DateTime)DateTime.ParseExact(row.Element("PurchaseDate").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ((DateTime)(row.Element("PurchaseDate"))).ToString("yyyy-MM-dd"),
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

                XmlElement elemPrice = xmlPortfolio.CreateElement("PurchasePrice");
                elemPrice.InnerText = price;
                elemRow.AppendChild(elemPrice);

                XmlElement elemDate = xmlPortfolio.CreateElement("PurchaseDate");
                elemDate.InnerText = date;
                elemRow.AppendChild(elemDate);

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

                XmlElement elemPrice = xmlPortfolio.CreateElement("PurchasePrice");
                elemPrice.InnerText = price;
                elemRow.AppendChild(elemPrice);

                XmlElement elemDate = xmlPortfolio.CreateElement("PurchaseDate");
                elemDate.InnerText = date;
                elemRow.AppendChild(elemDate);

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
    }
}