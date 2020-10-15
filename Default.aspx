<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h3>Portfolio Analytics</h3>
        <%--<p class="lead">
            Portfolio Analytics is a web based portfolio management tool, allowing real-time graph based and comparative stock analysis. 
            In off-line mode you can research your stock based on downloaded data or you can choose real-time mode to do online real time research
        </p>--%>
        <p class="lead">
            Portfolio Analytics is a anywhere-anytime portfolio manager application, as well as real-time data enabled research & analytics tool for stocks & mutual funds.
            It gives users an ability to weigh strengths and weaknesses, opportunities and threats across the full spectrum of investments.
        </p>
        <p><a id="loginlink" href="mlogin.aspx" class="btn btn-primary btn-lg" runat="server">Login&raquo;</a></p>
        <p><a id="registerlink" href="mlogin.aspx" class="btn btn-primary btn-lg" runat="server">Register&raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-6">
            <h4>Key Features</h4>
            <div>
                <ul>
                    <li>Manage multiple portfolios for your stocks & mutual fund</li>
                    <li>Maintain separate portfolios depending one your requirement</li>
                    <li>Add, modify or delete portfolio transactions</li>
                    <li>Maintain & manage SIP transactions</li>
                    <li>Real-time data integration</li>
                    <li>Get live quotes and add new stocks & mutial funds to your portfolio</li>
                    <li>Portfolio Valuation Graph showing performance of your investments in a single view</li>
                    <li>Compare your investments with global indices</li>
                    <li>Advance graphs - to help you define strategy for entry, exit, long or short</li>
                    <li>Standard grphs - to help you understand perfomance of your investment over time</li>
                    <li>Customizable graphs to help you data driven investment decisions</li>
                    <li>Anywhere-Anytime access - use any browser on any device to access the application</li>
                </ul>
            </div>
            <%--<p>
                <a class="btn btn-default" href="mlogin.aspx">Register Now&raquo;</a>
            </p>--%>
        </div>
        <div class="col-md-6">
            <h4>Differentiating Features</h4>
            <div>
                <ul>
                    <li>One application to manage your stocks & mutual fund investments</li>
                    <li>Create multiple porfolios, financial advisors can maintain separate client portfolios</li>
                    <li>Valuation graph that shows ALL investments on a single page</li>
                    <li>Parameterized graphs allowing users to customize output based on their requirements</li>
                    <li>Each graph has its own view window, allowing users to compare graphs</li>
                    <li>Interactive graphs - on mouse click shows details related to specific data point</li>
                    <li>Raw data view for all graphs</li>
                    <li>Import any external stock portfolio that is in CSV file cormat</li>
                    <li>Responsive application design allows application to run on any device</li>
                </ul>
            </div>
        </div>
        <%--<div class="col-md-6">
            <h2>Analytics using Graphs</h2>
            <div>
                <ul>
                    <li>Standard graphs:
                        <ul>
                            <li>Daily and Intra-day price</li>
                            <li>SMA, EMA, ADX, AROON, Bollinger Bands, MACD, STOCH, VWAP</li>
                        </ul>
                    </li>
                    <li>Advance graphs
                        <ul>
                            <li>Price validator</li>
                            <li>Crossover</li>
                            <li>Trend reversal Indicator</li>
                            <li>Momentum Indicator</li>
                            <li>Trends gauger</li>
                            <li>Buy & Sell Indicator</li>
                            <li>Trend Direction</li>
                            <li>Price Direction & Strength</li>
                        </ul>
                    </li>
                </ul>
                <p>
                    <a class="btn btn-default" href="mlogin.aspx">Register Now&raquo;</a>
                </p>
            </div>
        </div>--%>
        <%--<div class="col-md-4">
            <h2>Real-time data</h2>
            <ul>
                <li>Real-time data</li>
                <li>Create or import as many stock portfolio as you want</li>
                <li>Add, modify or delete portfolio transactions</li>
                <li>Get live quotes for any stock</li>
                <li>Portfolio valuation graph that shows your investment performance and compare the same with market indices</li>
                <li>Customizable graphs to help you make data drives investment decisions</li>
                <li>Anywhere-Anytime access - application is hosted on AWS free tier server(with limited bandwidth, storage and performance)</li>
                <li>If you would like to see additional graphs or support us financially, please write to me at <a href="mailto:vinaybhide@hotmail.com">vinaybhide@hotmail.com</a></li>
            </ul>
            <p>
                <a class="btn btn-default" href="contact.aspx">Contact Us &raquo;</a>
            </p>
        </div>--%>
    </div>
</asp:Content>
