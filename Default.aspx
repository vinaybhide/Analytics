<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Portfolio Analytics</h1>
        <p class="lead">Portfolio Analytics is a free web based portfolio management tool, allowing real-time graph based and comparative stock analysis. 
            In off-line mode you can research your stock based on downloaded data or you can choose real-time mode to do online real time research </p>
        <p><a id="loginlink" href="login.aspx" class="btn btn-primary btn-lg" runat="server">Login&raquo;</a></p>
        <p><a id="registerlink" href="login.aspx" class="btn btn-primary btn-lg" runat="server">Register&raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                Portfolio Analytics help you manage multiple portfolios. You can create as many portfolios as you want. 
                <br />Get quote, do research, add stocks to your portfolio.
                <br />See consolidated valuation of your stocks within selected portfolio.
                <br />Analyze selected stocks by leveraging graphs based on key Tech Indicator's
                <br />Download stock specific data for off-line analysis
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more from Alpha-Vantage</h2>
            <p>
                Portfolio Analytics uses <a href="https://www.alphavantage.co/">Alpha Vantage</a> to get real time stock data. 
                If you need unlimited access to real time stock data get your paid Alpha Vantage key from <a href="https://www.alphavantage.co/support/#api-key">here</a>.
                Then add that key to your registered users profile using <a href="AddKey.aspx">Add Key</a> menu option.
            </p>
            <p>
                <a class="btn btn-default" href="https://www.alphavantage.co/">Learn more about Alpha Vantage &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Tech Indicator graphs</h2>
            <p>
                Please write to us if you want application to support additional Tech Indicators. 
                <br />Current Portfolio analytics shows following graphs:
            </p>
                <ul>
                    <li>Daily, Intra-day, SMA, EMA</li>
                    <li>ADX, AROON, Bollinger Bands</li>
                    <li>MACD, STOCH, VWAP</li>
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
