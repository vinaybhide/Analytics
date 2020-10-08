<%@ Page Title="How To" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HowTo.aspx.cs" Inherits="Analytics.HowTo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center">How to use Portfolio Analytics</h3>
    <div class="row">
        <div class="col-md-4">
            <h4>Getting started - Register & Login</h4>
            <ul>
                <li>To register, enter email id & password</li>
                <li>You need to register only once</li>
                <li>Login using email id & password you provided during registration</li>
                <%--<li>Check or un-check the Off-line mode flag, to enter the application in Off-Line or On-Line mode</li>--%>
            </ul>
            <%--<h4>Getting started - On-line mode</h4>
            <ul>
                <li>Uncheck the Off-Line mode check box to login to application in On-line mode</li>
                <li>Data required for graphs, quote, portfolio valuation is fetched in real-time using Alpha Vantage API and/or Yahoo Finance API</li>
                <li>There is a limitation on calls in On-line mode - 5 api calls per minute, 500 api calls in a day</li>
                <li>To enjoy uninturrupted access to real-time stock data, get your <a href="https://www.alphavantage.co/premium/">Alpha Vantage API access key</a></li>
                <li>Use the Add Key menu to save your own key, which will be used for real time access to Alpha Vantage </li>
            </ul>
            <h4>Getting started - Off-line mode</h4>
            <ul>
                <li>Make sure you have checked the Off-Line mode check box during login</li>
                <li>This mode uses data you donloaded previously to generate graphs</li>
                <li>Overall application performance is superior in Off-line mode</li>
            </ul>--%>
            <h4>Portfolio Menu (Stock & MF)</h4>
            <ul>
                <li><b>New Portfolio-></b> Create a new portfolio</li>
                <li><b>Open Portfolio-></b> Select portfolio to work with</li>
                <li><b>Delete Portfolio-></b> Select portfolios to delete</li>
                <li><b>Import stock Portfolio (CSV File)-></b> Import any external file in CSV format</li>
                <li><b>Get Stock Quote-></b> Search any stock and get market quote</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Portfolio Page (Stock & MF)</h4>
            <ul>
                <li>Select any transaction of any stock from the table to
                        <ul>
                            <li><b>Edit-></b> Edit transaction</li>
                            <li><b>Delete-></b> transaction</li>
                            <li><b>Get Stock Quote & Add-></b> Get quote & add new</li>
                        </ul>
                </li>
                <li><b>Add new stock or MF-></b> Search stock or MF & add the transaction to currently open portfolio.</li>
                <li><b>Add MF SIP-></b>Just Provide start & end date of SIP and the application will add individual SIP transaction</li>
                <li><b>Get Quote & Add-></b> Gets quote for any stock, and add a new transaction to currently open portfolio</li>
                <li><b>Portfolio Valuation-></b> Shows valuation graph for all stocks from currently open portfolio</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Import external stock portfolio(CSV file)</h4>
            <ul>
                <li>Select & Import a CSV File from your local machine</li>
                <li>Select Country/stock exchange</li>
                <li>Use Map Selected button to map each source column to one of the target column</li>
                <li>Do this for all relevant columns</li>
                <li>Click Convert Data to Portfolio button to convert imported file to Portfolio Analytics format</li>
            </ul>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <h4>Research & Graphs Menu->Standard Graphs</h4>
            <ul>
                <li><b>Show Daily-></b>Daily prices graph</li>
                <li><b>Intra-day-></b>Intra-day prices graph.</li>
                <li><b>Simple Moving Average(SMA)-></b> Shows SMA using given period as average factor</li>
                <li><b>Exponential Moving Average(EMA)-></b> Shows EMA using given period for selected series</li>
                <li><b>Volume Weighted Average Price(VWAP)-></b> Shows VWAP for the selected interval</li>
                <li><b>Relative Strength Index(RSI)-></b> Shows RSI using given period for selected series</li>
                <li><b>Stochastic Oscillator(STOCH)-></b> Shows SlowK & SlowD graphs using given periods and SMA</li>
                <li><b>Moving Average Convergence Divergence(MACD)-></b> Shows MACD, Signal & histogram using given period and series type</li>
                <li><b>Aroon Indicator(AROON)-></b> Shows AROON UP & AROON Down using given period</li>
                <li><b>Average Directional Index(ADX)-></b>Shows SMA using given period</li>
                <li><b>Bollinger Bands(BBANDS)-></b> Shows Upper & Lower Bollinger Bands using given period, series type and standard deviation multiplier </li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Research & Graphs Menu->Advance Graphs</h4>
            <ul>
                <li><b>Price Validator-></b> Intra-day Vs VWAP graph, used to validate the stock price</li>
                <li><b>Crossover-></b>  Daily Vs SMA100 Vs SMA50 graph, used to estimate stock performance & predict coming changes in trend, such as reversals or breakouts</li>
                <li><b>Trend Identifier-></b> Daily Vs EMA12 Vs EMA26 and MACD Vs Signal Vs Histogram graph. Used to identify when bullish or bearish 
                    momentum is high in order to identify entry and exit points for trades</li>
                <li><b>Momentum Identifier-></b> Daily Vs RSI graph, used to identify bullish and bearish price momentum</li>
                <li><b>Gauge Trends-></b> Daily Vs Upper band Vs Lower  band graph, used to determine entry and exit points as well 
                    as overbought and oversold conditions.</li>
                <li><b>Buy-Sell Indicator-></b> Daily prices, SlowK, SlowD And RSI graphs that helps to identify when a stock has moved 
                    into an overbought or oversold position and make good entry and exit decisions.</li>
                <li><b>Trend Direction-></b> Daily prices, DX, +DI, -DI and ADX graph, that helps to determine trend strength and trend direction.</li>
                <li><b>Price Direction-></b> Daily, +DMI, -DMI graph that helps in identifying in which direction price is moving</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Portfolio Valuation Graph (Stock and MF)</h4>
            <ul>
                <li><b>Stock Portfolio Valuation-></b> Shows date wise performance graph for all stocks. You can select combination of stocks from your portfolio. You can also compare against market indices graphs. </li>
                <li><b>MF Portfolio Valuation (Bar Graph)-></b> Cost Vs Value comparison bar graph for each of your MF within the portfolio</li>
                <li><b>MF Portfolio Valuation (Line Graph)-></b> Shows date wise, unit wise performance graph for all funds. You can select combination of funds from your portfolio. You can also compare against market indices graphs. </li>
            </ul>
            <h4>Contact US</h4>
            <ul>
                <li>If you have any suggestions, feedback or if you wish to provide financial support please write to me at <a href="mailto:vinaybhide@hotmail.com">vinaybhide@hotmail.com</a></li>
                <li>Financial assistance is appreciated that will keep the application running on AWS</li>
            </ul>
        </div>
    </div>
</asp:Content>
