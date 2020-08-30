<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Portfolio Analytics</h1>
        <p class="lead">
            Portfolio Analytics is a free web based portfolio management tool, allowing real-time graph based and comparative stock analysis. 
            In off-line mode you can research your stock based on downloaded data or you can choose real-time mode to do online real time research
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
                    <li>Use advance graphs to define your strategy for entry, exit or  long, short</li>
                    <li>Use parameters to customize graphs specific to your requirements</li>
                    <li>Use application in off-line or on-line mode</li>
                    <li>On-line mode get real time data from Alpha Vantage </li>
                    <li>Off-line mode works off data you have downloaded</li>
                </ul>
            </div>
            <p>
                <a class="btn btn-default" href="Default.aspx">Register for free and start using this software&raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Graphs</h2>
            <div>
                <ul>
                    <li>Application has built in graphs, that cover wide range of analytics & trading strategy paradigms. 
                    </li>
                    <li>Each graph can be configured via parameters to suite your specific requirement. If you have specific needs, please write to me. </li>
                    <li>
                    Following is partial list of graphs currently supported:
                <li>Daily, Intra-day SMA, EMA, ADX, AROON, Bollinger Bands, MACD, STOCH, VWAP</li>
                    <li>Crossover, Trend reversal Indicator, Momentum Indicator, Gauge Trends, Buy & Sell Indicator, Trend Direction, Price Direction & Strength</li>
                    <li>And many more....</li>
                </ul>
                <p>
                    <a class="btn btn-default" href="Default.aspx">Register for free and start using this software&raquo;</a>
                </p>
            </div>
        </div>
        <div class="col-md-4">
            <h2>Sponsors welcome!</h2>
            <ul>
                <li>This application is hosted on AWS free tier server which has limitation in terms of bandwidth, storage and overall performance.
                </li>
                <li>Stock marked data is accessed using Alpha Vantage free api accessm which has limitation of 5 calls per minute and 500 calls in a day.
                </li>
                <li>Please write to me at <a href="mailto:vinaybhide@hotmail.com">vinaybhide@hotmail.com</a>, if you wish to support for 
                <ul>
                    <li>Getting a paid unlimited API access from Alpha Vantage </li>
                    <li>Improved performance by hosting the application on high performance server from AWS hosting service.</li>
                </ul>
                </li>
            </ul>
            <p>
                <a class="btn btn-default" href="contact.aspx">Contact Us &raquo;</a>
            </p>
        </div>
    </div>

    <%--    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>--%>
</asp:Content>
