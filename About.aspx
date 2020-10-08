﻿<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Analytics.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <p class="lead">
            Portfolio Analytics is a solution I developed to help me do research & define strategies for my own investments. 
            It helps in maintaining my stock and mutual fund portfolio, as well as in defining strategy for algorithmic trading.
            The differentiating feature that I use daily is the single view graph showing consolidated valuation of my investments. 
            After using it successfully I decided to host it on cloud so other can make use of it.
        </p>
    </div>
    <h3 class="text-center">Features - Portfolio Analytics</h3>
    <div class="row">
        <div class="col-md-6">
            <h3>Portfolio Manager- Add, edit, delete or research</h3>
            <ul>
                <li>Search & add stocks or mutual funds</li>
                <li>Maintain date-wise purchase transactions including SIP</li>
                <li>Edit or delete selected transaction</li>
                <li>Get real-time quotes</li>
                <li>Consolidated portfolio valuation graph</li>
                <li>Import existing portfolios</li>
                <li>Create as many portfolios as you want</li>
            </ul>
        </div>
        <div class="col-md-6">
            <h3>Portfolio Valuation graph</h3>
            <ul>
                <li>Shows performance for each of your investment in a single view</li>
                <li>Highlights cumulative purchase transactions, as on date cost Vs valuation</li>
                <li>Compare portfolio performance against market indices</li>
                <li>Choose specific investments for comparative graph</li>
            </ul>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <h3>Standard stock indicator graphs</h3>
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
        <div class="col-md-6">
            <h3>Advance combination graphs for stocks</h3>
            <ul>
                <li>Price Validator - VWAP Vs Intra-day</li>
                <li>Crossover - SMA100 Vs SMA50 Vs Daily </li>
                <li>Trend Reversal - MACD Vs EMA12 Vs EMA26 Vs Daily</li>
                <li>Momentum - RSI Vs Daily</li>
                <li>Gauge Trends - Bollinger Bands Vs Daily</li>
                <li>Buy-Sell - Stochastics Vs RSI Vs Daily</li>
                <li>Trend Direction - ADX Vs DX Vs +DI Vs -DI Vs Daily</li>
                <li>Price Direction & Strength: Daily Vs -DMI Vs +DMI</li>
            </ul>
        </div>
    </div>
    <%--<div class="row">

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
            <h4>Integration with Alpha Vantage API & Yahoo Finance API</h4>
            <ul>
                <li>Default operation is with Alpha Vantage free API service and/or using Yahoo finance API</li>
                <li>Free API provided by Alpha Vantage has limitation of 5 calls per minut & 500 calls in a day</li>
                <li>You can use a your own Alpha Vantage key to remove this restriction</li>
                <li>Get your <a href="https://www.alphavantage.co/premium/">Alpha Vantage API access key</a></li>
            </ul>
        </div>
    </div>--%>
</asp:Content>
