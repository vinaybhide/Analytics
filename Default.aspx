<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Analytics._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="margin-top: 1px; padding-top: 5px; padding-bottom: 1px; margin-bottom: 5px;">
        <div class="label-info " style="text-align: center;">
            <asp:Timer ID="TimerIndices" runat="server" Interval="60000" OnTick="GetIndexValues" />
            <asp:Timer ID="TimerClear" runat="server" Interval="59900" OnTick="ClearHeading" />

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TimerIndices" />
                    <asp:AsyncPostBackTrigger ControlID="TimerClear" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="headingtext" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <h3>Portfolio Analytics</h3>
        <p class="lead">
            Anywhere-anytime portfolio manager for stocks & mutual funds and research & analytics tool for stocks & mutual funds using real time market data.
        </p>
        <p>
            <a id="loginlink" href="mlogin.aspx" class="btn btn-primary btn-lg" runat="server">Login&raquo;</a>
            <label></label>
            <a id="registerlink" href="mlogin.aspx" class="btn btn-primary btn-lg" runat="server">Register&raquo;</a>
        </p>
    </div>
    <div class="row">
        <table>
            <tr>
                <td style="width:50%; text-align: center;">
                    <asp:Chart ID="chart1" Width="500" runat="server" CssClass="chart" Visible="true" 
                        EnableViewState="True"
                        ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation">
                        <Series>
                            <asp:Series Name="SENSEX" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="CLOSE" YValueType="Double"
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
                <td style="width:100%; text-align: center;">
                    <asp:Chart ID="chart2" Width="500" runat="server" CssClass="chart" Visible="true" 
                        EnableViewState="True"
                        ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation">
                        <Series>
                            <asp:Series Name="NIFTY" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="CLOSE" YValueType="Double"
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
    </div>
    <div class="row">
        <div class="col-md-6">
            <h4>Key Features</h4>
            <div>
                <ul>
                    <li>Manage multiple portfolios for your stocks & mutual fund</li>
                    <li>Add, modify or delete portfolio transactions including SIP</li>
                    <li>Real-time market data integration</li>
                    <li>Get live quotes</li>
                    <li>Portfolio Valuation Graph</li>
                    <li>Comparison with global indices</li>
                    <li>Advance Indicator graphs - define entry, exit, long or short strategy</li>
                    <li>Standard Indicator grphs - understand perfomance of your investment</li>
                    <li>All graphs are customizable</li>
                    <li>Anywhere-Anytime Application</li>
                </ul>
            </div>
        </div>
        <div class="col-md-6">
            <h4>Differentiating Features</h4>
            <div>
                <ul>
                    <li>Valuation Graph of your portfolio</li>
                    <li>Backtest graph & price forecast</li>
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
    </div>
</asp:Content>
