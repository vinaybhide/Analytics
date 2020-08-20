<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Analytics.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Portfolio Analytics - Free global stock portfolio management & Analytics software</h2>
    <h3>Features</h3>
    <div>
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
    </div>
</asp:Content>
