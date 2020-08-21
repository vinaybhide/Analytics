<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Analytics.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center">Features - Portfolio Analytics</h3>
    <div class="row">
        <div class="col-md-4">
            <h4>Portfolio Manager- Add, delete, research your stock investments</h4>
            <ul>
                <li>Allows to search & add stock from any global stock market</li>
                <li>Maintains date-wise stock purchases</li>
                <li>Get real-time quotes</li>
                <li>Consolidated purchase datewise portfolio valuation graph</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Standard graphs- Uses standard indicator data to show following graphs</h4>
            <ul>
                <li>Daily (Open/High/Low/Close/Volume)</li>
                <li>Intra-day (Open/High/Low/Close/Volume)</li>
                <li>Simple moving average-SMA</li>
                <li>Exponential moving average-EMA</li>
                <li>Volume Weighted Avg Price-VWAP</li>
                <li>Relative strength index-RSI</li>
                <li>Stochastic oscillator-STOCH</li>
                <li>Moving average convergence/divergence-MACD</li>
                <li>AROON</li>
                <li>Average directional movement index-ADX</li>
                <li>Bollinger Bands</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Advance graphs - uses data from multiple indicators to show comparative graphs</h4>
            <ul>
                <li>Intra-day Indicator</li>
                <li>Crossover (Buy/Sell Signal) </li>
                <li>Trend Reversal Indicator</li>
                <li>Momentum Indicator</li>
                <li>Gauge Trends</li>
                <li>Buy-Sell Indicator</li>
                <li>Trend Direction</li>
                <li>Price Direction & Strength</li>
            </ul>
        </div>
    </div>
    <div class="row">

        <div class="col-md-4">
            <h4>On-line and Off-line mode</h4>
            <ul>
                <li>On-line mode works with real time data accessed using Alpha Vantage API</li>
                <li>Off-line mode works with downloaded stock data</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Download data for off-line operation</h4>
            <ul>
                <li>Configure & download data for all indicators as well as daily and intra-day pricing</li>
                <li>Allows for optimized analysis without any delay in accessing real-time data</li>
                <li>As Tech Indicators work off historical data, this is a key feature for deciding your portfolio strategy prior to trading session</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Integration with Alpha Vantage</h4>
            <ul>
                <li>Default operation is with Alpha Vantage free API service, with limitation of 5 calls per minut & 500 calls in a day</li>
                <li>You can use a your own Alpha Vantage key to remove this restriction</li>
                <li>Get your <a href="https://www.alphavantage.co/premium/">Alpha Vantage API access key</a></li>
            </ul>
        </div>
    </div>

    <%--<div>
        <ul>
            <li>Portfolio Manager- Add, delete, research your stock investments
                <ul>
                    <li>Allows to search & add stock from any global stock market</li>
                    <li>Maintains date-wise stock purchases</li>
                    <li>Get real-time quotes</li>
                    <li>Consolidated purchase datewise portfolio valuation graph</li>
                </ul>
            </li>
            <li>Standard graphs- Shows following graphs and allows you to select appropriate parameters to narrow down your analysis
                <ul>
                    <li>Daily (Open/High/Low/Close/Volume)</li>
                    <li>Intra-day (Open/High/Low/Close/Volume)</li>
                    <li>Simple moving average-SMA</li>
                    <li>Exponential moving average-EMA</li>
                    <li>Volume Weighted Avg Price-VWAP</li>
                    <li>Relative strength index-RSI</li>
                    <li>Stochastic oscillator-STOCH</li>
                    <li>Moving average convergence/divergence-MACD</li>
                    <li>AROON</li>
                    <li>Average directional movement index-ADX</li>
                    <li>Bollinger Bands</li>
                </ul>
            </li>
            <li>Advance graphs - Shows customizable combination of tech indicators graphs for data-driven comparative analysis. Following is sample list of graphs
                <ul>
                    <li>Intra-day Indicator</li>
                    <li>Crossover (Buy/Sell Signal) </li>
                    <li>Trend Reversal Indicator</li>
                    <li>Momentum Indicator</li>
                    <li>Gauge Trends</li>
                    <li>Buy-Sell Indicator</li>
                    <li>Trend Direction</li>
                    <li>Price Direction & Strength</li>
                </ul>
            </li>
            <li>Real time and Off-line mode
                <ul>
                    <li>Real-time mode works with current data which is accessed using Alpha Vantage gateway</li>
                    <li> Off-line mode works with downloaded stock data</li>
                </ul>
            </li>
            <li>Download data for off-line operation
                <ul>
                    <li>Configure & download data for all indicators as well as daily and intra-day procing</li>
                    <li>Allows for optimized analysis without any delay in accessing real-time data</li>
                    <li>As Tech Indicators work off historical data, this is a key feature for deciding your portfolio strategy prior to trading session</li>
                </ul>
            </li>
            <li>Integration with Alpha Vantage
                <ul>
                    <li>Default operation is with Alpha Vantage free API service, with limitation on number of request per day</li>
                    <li>You can use a your paid Alpha Vantage key to remove this restriction</li>
                    <li>Get your Alpha Vantage key from <a href="https://www.alphavantage.co/">here&raquo;</a> </li>
                </ul>
            </li>
        </ul>
    </div>--%>
</asp:Content>
