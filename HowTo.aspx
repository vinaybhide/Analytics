<%@ Page Title="How To" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HowTo.aspx.cs" Inherits="Analytics.HowTo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center">How to use Portfolio Analytics</h3>
    <div class="row">
        <div class="col-md-4">
            <h4>Getting started - Register</h4>
            <ul>
                <li>To register, provide your email id & choose a password you can remember</li>
                <li>You need to register only once</li>
                <li>We will only use your email id to authenticate during login</li>
            </ul>
            <h4>Getting started - Login</h4>
            <ul>
                <li>Use the same email id & password you provided during registration to login to Portfolio Analytics</li>
                <li>Check or un-check the Off-line mode flag, to enter the application in Off-Line or On-Line mode</li>
            </ul>
            <h4>Getting started - On-line mode</h4>
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
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Portfolio Menu</h4>
            <div>
                <ul>
                    <li>You start off by creating a new portfolio using menu
                        <br />
                        "Portfolio->New Portfolio"</li>
                    <li>Open any existign portfolio using menu<br />
                        "Portfolio->Open Portfolio"</li>
                    <li>Delete any existing portfolio permanantly using menu<br />
                        "Portfolio->Delete Portfolio"</li>
                    <li>Once you open a portfolio you can:
                        <br />
                        <b>Add new stock:</b> search stock, input purchase information & add to current portfolio
                        <br />
                        <b>Delete stock:</b> Select a specific row to permanantly delete it from current portfolio
                        <br />
                        <b>Get Quote & Add:</b> gets quote for stock, and allows user to add the stock to current portfolio
                        <br />
                        <b>Portfolio Valuation:</b> graph showing cumulative day wise performance of stocks in current portfolio
                    </li>
                </ul>
            </div>
            <h4>Graphs Menu</h4>
            <div>
                <ul>
                    <li><b>Standard Graphs:</b>
                        <br />
                        Daily, Intra-day SMA, EMA, ADX, AROON, Bollinger Bands, MACD, STOCH, VWAP</li>
                    <li><b>Advance Graphs:</b>
                        <br />
                        Crossover, Trend reversal Indicator, Momentum Indicator, Gauge Trends, Buy & Sell Indicator, Trend Direction, Price Direction & Strength</li>
                    <li>Graph's can be customized by selecting parameters to suite your specific requirement</li>
                    <li>Each graph is shown in a separate window to allow comparative analysis</li>
                    <li>We will continue to add more graphs, do let us know if you have any specific requirement</li>
                </ul>
            </div>
        </div>
        <div class="col-md-4">
            <h4>Admin Menu</h4>
            <ul>
                <li>Download data
                    <ul>
                        <li>Search & select specific stock</li>
                        <li>Select specific item you want to download by checking the box against that item</li>
                        <li>You can customize what you want to download by changing parameter value for that item</li>
                        <li>If you leave all parameters unchanged then the data will be downloaded as per standard parameter values on this page</li>
                        <li>Click "Download Selected" button to start your download</li>
                        <li>Due to limitation off Alpha Vantage free api, you should download one item at a time</li>
                        <li>To overcome this limitation, please get your <a href="https://www.alphavantage.co/premium/">Alpha Vantage Api Key</a></li>
                    </ul>
                </li>
                <li>Add Key
                    <ul>
                        <li>Get your own <a href="https://www.alphavantage.co/premium/">Alpha Vantage Api Key</a></li>
                        <li>This page allows you to add your Alpha Vantage key</li>
                        <li>The entered key will be assigned only to your Portfolio Analytics user id</li>
                        <li>Portfolio Analytics will use assigned key to access stock data</li>
                    </ul>
                </li>
            </ul>
        </div>
        <div class="col-md-4">
            <h4>Contact US</h4>
            <ul>
            <li>If you have any suggestions, feedback or if you wish to provide financial support please write to me at <a href="mailto:vinaybhide@hotmail.com">vinaybhide@hotmail.com</a></li> 
               <li>Financial assistance is needed for
                <ul>
                    <li>Getting a unlimited API access from Alpha Vantage </li>
                    <li>Improved performance by hosting the application on high performance server from AWS hosting service.</li>
                </ul>
            </li>
            </ul>
        </div>
    </div>
</asp:Content>
