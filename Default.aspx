<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="margin-top: 1px; padding-top: 5px; padding-bottom: 1px; margin-bottom: 5px;">
        <div class="label-info " style="text-align: center;">
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
        <p class="lead" style="margin-top: 10px; margin-bottom: 1px; padding-bottom: 1px; margin-right:50px;">
            <span style="font-size: 20px; font-weight:bold;">Portfolio Analytics</span> 
        
            <span style="font-size: 13px; padding-right:0px;">A free, open source platform to manage, evaluate, research & analyze your global stocks & mutual fund investments</span>
            <a id="loginlink" href="mlogin.aspx" class="btn btn-primary btn-sm" runat="server">Login&raquo;</a>
            <a id="registerlink" href="mlogin.aspx" class="btn btn-primary btn-sm" runat="server">Register&raquo;</a>
        </p>
    </div>
    <div class="row">
        <asp:Timer ID="TimerGraphSet" runat="server" Interval="60000" OnTick="RedrawGraphs" />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="TimerGraphSet" />
            </Triggers>
            <ContentTemplate>
                <table style="width:100%; border:1px solid black;">
                    <tr>
                        <td style="width: 50%; text-align: center;">

                            <asp:Chart ID="chart1" Width="500" runat="server" CssClass="chart" Visible="true"
                                EnableViewState="True"
                                ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseHttpHandler">
                                <Series>
                                    <asp:Series Name="SENSEX" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                                        XValueMember="TIMESTAMP" XValueType="DateTime" YValuesPerPoint="4" YValueMembers="CLOSE,OPEN,HIGH,LOW"
                                        PostBackValue="SENSEX,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; SENSEX:#VALY">
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
                        </td>
                        <td style="width: 100%; text-align: center;">
                            <asp:Chart ID="chart2" Width="500" runat="server" CssClass="chart" Visible="true"
                                EnableViewState="True"
                                ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseHttpHandler">
                                <Series>
                                    <asp:Series Name="NIFTY" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                                        XValueMember="TIMESTAMP" XValueType="DateTime" YValuesPerPoint="4" YValueMembers="CLOSE,OPEN,HIGH,LOW"
                                        PostBackValue="NIFTY,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; NIFTY:#VALY">
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
    <div class="row" style="border:1px solid black; margin-top:1px;">
        <div class="col-md-2">
            <h4>Key Features</h4>
            <div>
                <ul style="margin-left:-20px;">
                    <li>Manage Stocks & MF</li>
                    <li>MF SIP transaction</li>
                    <li>Real-time market data</li>
                    <li>Get live quotes</li>
                    <li>Compare global indices</li>
                    <li>Entry & exit strategy</li>
                    <li>Investment performance</li>
                    <li>Customizable graphs</li>
                </ul>
            </div>
        </div>
        <div class="col-md-2">
            <h4>Differentiating Features</h4>
            <div>
                <ul style="margin-left:-20px;">
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
        <div class="col-md-2">
            <h4>How to use</h4>
            <div>
                <ul style="margin-left:-20px;">
                    <li>Create your account by registering</li>
                    <li>Use your valid email id as user id</li>
                    <li>Enter a password that you will remember</li>
                    <li>Use the same email id & password combination to login</li>
                </ul>
            </div>
        </div>
        <div class="col-md-3">
            <h4>How to manage stock investment</h4>
            <div>
                <ul style="margin-left:-20px;">
                    <li>Stocks->New: Create portfolio</li>
                    <li>Stocks->Open: Open portfolio</li>
                    <li>Stocks->Delete: Delete portfolio </li>
                    <li>Stocks->Portfolio Valuation: Consolidated Valuation graph</li>
                    <li>Stocks->Get Quote: Get quote</li>
                    <li>Stocks->Graphs->Standard Indicators: Standard indicator graphs</li>
                    <li>Stocks->Graphs->Advance Indicators: Advance/Combination indicator graphs</li>
                    <li>Stocks->Graphs->Global Indices: Global index graphs</li>
                </ul>
            </div>
        </div>
        <div class="col-md-3">
            <h4>How to manage MF investment</h4>
            <div>
                <ul style="margin-left:-20px;">
                    <li>MF->New: Create new portfolio</li>
                    <li>MF->Open: Open an existing portfolio</li>
                    <li>MF->Delete: Delete an existing portfolio </li>
                    <li>MF->Portfolio Fund Valuation: Consolidated valuation of all funds</li>
                    <li>MF->Portfolio Cost Vs Value: Cost Vs Today's Value for each fund</li>
                    <li>MF->Graphs->Standard Indicator: See standard indicator graphs</li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
