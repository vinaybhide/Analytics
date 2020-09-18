<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2>Portfolio Analytics</h2>
        <%--<p class="lead">
            Portfolio Analytics is a web based portfolio management tool, allowing real-time graph based and comparative stock analysis. 
            In off-line mode you can research your stock based on downloaded data or you can choose real-time mode to do online real time research
        </p>--%>
        <p class="lead">
            Portfolio Analytics is a Anywhere-Anytime portfolio management & stock market research application. It helps users manage stock investments & 
            make buy, sell, long or short decisions based on real-time data. It gives users an ability to weigh strengths and weaknesses, 
            opportunities and threats across the full spectrum of investments
        </p>
        <p><a id="loginlink" href="mlogin.aspx" class="btn btn-primary btn-lg" runat="server">Login&raquo;</a></p>
        <p><a id="registerlink" href="mlogin.aspx" class="btn btn-primary btn-lg" runat="server">Register&raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <div>
                <ul>
                    <li>Manage multiple portfolios</li>
                    <li>Add stocks from local market or from any other country. Application is country agnostic!</li>
                    <li>Get live quotes and do research using graphs, see valuation of your entire portfolio</li>
                    <li>Advance graphs are built using combination of indicators to help you define strategy for entry and/or, exit from specific stock</li>
                    <li>Standard grphs are built using standard indicators showing perfomance of stock</li>
                    <li>All graphs are customizable, and you can choose various parameters to suite to your specific requirements</li>
                </ul>
            </div>
            <p>
                <a class="btn btn-default" href="Default.aspx">Register Now&raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Real time graphs</h2>
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
                    <a class="btn btn-default" href="Default.aspx">Register Now&raquo;</a>
                </p>
            </div>
        </div>
        <div class="col-md-4">
            <h2>Key Features</h2>
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
        </div>
    </div>
</asp:Content>
