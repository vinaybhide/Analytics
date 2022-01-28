<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="border: 1px solid; ">
        <table style="width: 100%; height: 100%;">
            <tr>
                <td style="width: 100%; text-align: center; background-color:lightblue;">
                    <asp:Timer ID="TimerHeadingSet" runat="server" Interval="60000" OnTick="GetIndexValues" />
                    <asp:Timer ID="TimerHeadingClear" runat="server" Interval="59900" OnTick="ClearHeading" />

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TimerHeadingSet" />
                            <asp:AsyncPostBackTrigger ControlID="TimerHeadingClear" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="headingtext" runat="server" Text=""></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <table style="width: 100%; height: 100%;">

            <tr>
                <td class="h4" style="width: 80%; text-align:center;">
                    <asp:Label Text="A free, open source platform to manage, evaluate, research & analyze global investments - Stocks, MF, ETF, Futures & more" runat="server" />
                </td>
                <td style="width:5%;">
                    <a id="loginlink" href="mlogin.aspx" class="btn btn-primary btn-sm" runat="server">Login&raquo;</a>
                </td>
                <td style="width:5%;">
                    <a id="registerlink" href="mlogin.aspx" class="btn btn-primary btn-sm" runat="server">Register&raquo;</a>
                </td>
            </tr>
        </table>
    </div>

    <%--<div class="jumbotron" style="margin-top: 1px; padding-top: 5px; padding-bottom: 1px; margin-bottom: 5px;">
        <div class="label-info" style="text-align: center;">
            <asp:Timer ID="TimerHeadingSet" runat="server" Interval="60000" OnTick="GetIndexValues" />
            <asp:Timer ID="TimerHeadingClear" runat="server" Interval="59900" OnTick="ClearHeading" />

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TimerHeadingSet" />
                    <asp:AsyncPostBackTrigger ControlID="TimerHeadingClear" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="headingtext" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <p class="lead" style="margin-top: 10px; margin-bottom: 1px; padding-bottom: 1px; margin-right: 50px;">
            <span style="font-size: 20px; font-weight: bold;">Portfolio Analytics</span>

            <span style="font-size: 13px; padding-right: 0px;">A free, open source platform to manage, evaluate, research & analyze global investments - Stocks, MF, ETF, Futures & more</span>
            <a id="loginlink" href="mlogin.aspx" class="btn btn-primary btn-sm" runat="server">Login&raquo;</a>
            <a id="registerlink" href="mlogin.aspx" class="btn btn-primary btn-sm" runat="server">Register&raquo;</a>
        </p>
    </div>--%>
    <div class="row">
        <asp:Timer ID="TimerGraphSet" runat="server" Interval="60000" OnTick="RedrawGraphs" />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="TimerGraphSet" />
            </Triggers>
            <ContentTemplate>
                <table style="width: 100%; border: 1px solid black;">
                    <tr>
                        <td style="width: 100%; text-align: center;">

                            <asp:Chart ID="chart1" runat="server" CssClass="chart" Visible="true"
                                EnableViewState="true"
                                ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseHttpHandler">
                                <Series>
                                    <asp:Series Name="SENSEX" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                                        XValueMember="TIMESTAMP" XValueType="DateTime" YValuesPerPoint="4" YValueMembers="CLOSE,OPEN,HIGH,LOW">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="chartarea1">
                                        <Position Auto="true" />
                                        <AxisX2 Title="SENSEX TODAY" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                                            <LabelStyle Format="hh:mm" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                                        </AxisX2>
                                        <AxisY Title="" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                                            TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                                            <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                                        </AxisY>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        <%--</td>
                        <td style="width: 50%; text-align: center;">--%>
                            <asp:Chart ID="chart2" runat="server" CssClass="chart" Visible="true"
                                EnableViewState="true"
                                ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseHttpHandler">
                                <Series>
                                    <asp:Series Name="NIFTY" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                                        XValueMember="TIMESTAMP" XValueType="DateTime" YValuesPerPoint="4" YValueMembers="CLOSE,OPEN,HIGH,LOW">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="chartarea2">
                                        <Position Auto="true" />
                                        <AxisX2 Title="NIFTY TODAY" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                                            <LabelStyle Format="hh:mm" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                                        </AxisX2>
                                        <AxisY Title="" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                                            TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                                            <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                                        </AxisY>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row" style="border: 1px solid black; margin-top: 1px;">
        <div class="col-sm-3">
            <h4>Differentiating Features</h4>
            <div>
                <%--<ul style="margin-left:-20px;">--%>
                <ul>
                    <li>Manage global investments</li>
                    <li>Supports SIP transaction</li>
                    <li>Real-time global market data</li>
                    <li>Valuation Graph</li>
                    <li>Backtest & forecast</li>
                    <li>Buy-Sell Indicator</li>
                    <li>Momentum Identifier</li>
                    <li>Price Direction</li>
                    <li>Price Validator</li>
                    <li>Trend Direction</li>
                    <li>Trend Gauger</li>
                    <li>Trend Identifier</li>
                </ul>
            </div>
        </div>
        <div class="col-sm-3">
            <h4>How to use</h4>
            <div>
                <%--<ul style="margin-left:-20px;">--%>
                <ul>
                    <li>Create your account by registering</li>
                    <li>Use your valid email id as user id</li>
                    <li>Enter a password that you will remember</li>
                    <li>Use the same email id & password combination to login</li>
                </ul>
            </div>
        </div>
        <div class="col-sm-3">
            <h4>How to Use Manage Investments</h4>
            <div>
                <%--<ul style="margin-left:-20px;">--%>
                <ul>
                    <li>New: Create portfolio</li>
                    <li>Open: Open portfolio</li>
                    <li>Delete: Delete portfolio </li>
                    <li>Portfolio Valuation Graph</li>
                    <li>Get Quot</li>
                    <li>Graphs->Standard Indicator Graphs</li>
                    <li>Graphs->Advance Indicator Graphs</li>
                    <li>Graphs->Global Indices Graphs</li>
                </ul>
            </div>
        </div>
        <div class="col-sm-3">
            <h4>How to use Manage Mutual Funds</h4>
            <div>
                <%--<ul style="margin-left:-20px;">--%>
                <ul>
                    <li>New: Create portfolio</li>
                    <li>Open: Open portfolio</li>
                    <li>Delete: Delete an existing portfolio </li>
                    <li>Portfolio Fund Valuation Graph</li>
                    <li>Portfolio Cost Vs Value Graph</li>
                    <li>Standard Indicator Graphs</li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
