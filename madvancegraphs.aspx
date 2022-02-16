<%@ Page Title="Advance Graphs" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="madvancegraphs.aspx.cs" Inherits="Analytics.madvancegraphs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table style="width: 100%; border-color: black; border-style: solid; border-width: thin; margin-top: 2%;">
        <tr>
            <td style="width: 100%; text-align: center;">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Advance Indicator Graphs"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-left-color: black; border-left-style: solid; border-left-width: thin; border-right-color: black; border-right-style: solid; 
            border-right-width: thin; border-bottom-color: black; border-bottom-style: solid; border-bottom-width: thin;">
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="label71" Text="Investments: " runat="server"></asp:Label>
            </td>
            <td style="width: 60%;">
                <asp:DropDownList ID="DropDownListStock" runat="server" AutoPostBack="True" TabIndex="1 " OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged"></asp:DropDownList>
                <asp:Button ID="ButtonGetAllForExchange" runat="server" Text="Reset Investments" TabIndex="2" OnClick="ButtonGetAllForExchange_Click" />
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="60" OnClick="buttonBack_Click" />
            </td>
        </tr>
    </table>

    <table style="width: 100%; border-left-color: black; border-left-style: solid; border-left-width: thin; border-right-color: black; border-right-style: solid; 
            border-right-width: thin; border-bottom-color: black; border-bottom-style: solid; border-bottom-width: thin;">
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="Label40" runat="server" Style="text-align: right" Text="Filter by Exchange:"></asp:Label>
            </td>
            <td style="width: 60%;">
                <asp:DropDownList ID="ddlExchange" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlExchange_SelectedIndexChanged" TabIndex="3">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="Label12" runat="server" Style="text-align: right" Text="Filter by Investment Type:"></asp:Label>
            </td>
            <td style="width: 60%;">
                <asp:DropDownList ID="ddlInvestmentType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlInvestmentType_SelectedIndexChanged" TabIndex="4">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="label69" Text="Filter by Portfolio :" runat="server"></asp:Label>
            </td>
            <td style="width: 60%;">
                <asp:DropDownList ID="ddlPortfolios" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlPortfolios_SelectedIndexChanged" TabIndex="5"></asp:DropDownList>
                <asp:Button ID="ButtonSearchPortfolio" runat="server" Text="Populate investments from portfolio" TabIndex="6" OnClick="ButtonSearchPortfolio_Click" />
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-left-color: black; border-left-style: solid; border-left-width: thin; border-right-color: black; border-right-style: solid; 
            border-right-width: thin; border-bottom-color: black; border-bottom-style: solid; border-bottom-width: thin;">
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="Label70" runat="server" Style="text-align: right" Text="Search Investment :"></asp:Label>
            </td>
            <td style="width: 60%;">
                <asp:TextBox ID="TextBoxSearch" AutoPostBack="true" runat="server" TabIndex="7" ToolTip="Enter first few letters of company or investment. For example to search Vanguard type: Vang" OnTextChanged="ButtonSearch_Click"></asp:TextBox>
                <asp:Button ID="ButtonSearch" runat="server" Text="Search" TabIndex="8" OnClick="ButtonSearch_Click" />
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-left-color: black; border-left-style: solid; border-left-width: thin; border-right-color: black; border-right-style: solid; 
            border-right-width: thin; border-bottom-color: black; border-bottom-style: solid; border-bottom-width: thin;">
        <tr>
            <td style="text-align: right; width: 40%;"></td>
            <td style="width: 60%;">
                <asp:TextBox ID="textboxSelectedSymbol" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%; margin-top: 1%;">
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                <asp:Label ID="Label38" runat="server" Text="Graph"></asp:Label></td>
            <td style="width: 80%; background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
        </tr>

        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonVWAPIntra" Text="Price Validator" runat="server" Font-Size="Small" TabIndex="8" OnClick="buttonVWAPIntra_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label4" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_Interval" runat="server" TabIndex="9">
                    <asp:ListItem Value="1m">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Selected="True">5 min</asp:ListItem>
                    <asp:ListItem Value="15m">15 min</asp:ListItem>
                    <asp:ListItem Value="30m">30 min</asp:ListItem>
                    <asp:ListItem Value="60m">60 min</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label7" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="10">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <asp:ListItem Value="Compact">Compact</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonCrossover" Text="Crossover (Back test + Forecast)" Font-Size="Small" runat="server" TabIndex="12" OnClick="buttonCrossover_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label11" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="13">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <asp:ListItem Value="Compact" Enabled="false">Compact</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label13" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlSMA1_Interval" runat="server" TabIndex="13">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1m" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label16" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlSMA1_Series" runat="server" TabIndex="15">
                    <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                    <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
                <br />

                <asp:Label ID="Label8" runat="server" Text="SMAL Small Period:"></asp:Label>
                <asp:TextBox ID="textboxSMA1_Period" runat="server" TextMode="Number" Width="50" Text="50" TabIndex="14"></asp:TextBox>
                <br />
                <asp:Label ID="Label14" runat="server" Text="SMA Long Period:"></asp:Label>
                <asp:TextBox ID="textboxSMA2_Period" runat="server" TextMode="Number" Width="50" Text="100" TabIndex="17"></asp:TextBox>
                <br />
                <asp:Label ID="Label18" runat="server" Text="Buy span in days: "></asp:Label>
                <asp:TextBox ID="textboxBuySpan" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label20" runat="server" Text="Sell span in days: "></asp:Label>
                <asp:TextBox ID="textboxSellSpan" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label37" runat="server" Text="Simulation Quantity: "></asp:Label>
                <asp:TextBox ID="textboxSimulationQty" runat="server" TextMode="Number" Width="60" Text="100" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label47" runat="server" Text="Forecast Regression Type:"></asp:Label>
                <asp:DropDownList ID="ddlRegressionType" runat="server">
                    <asp:ListItem Text="Linear" Value="2" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Exponential" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Logarithmic" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Power" Value="5"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label52" runat="server" Text="Forecasting period: "></asp:Label>
                <asp:TextBox ID="textboxForecastPeriod" runat="server" TextMode="Number" Width="60" Text="40" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonMACD_EMA_Daily" Text="Trend Identifier" Font-Size="Small" runat="server" TabIndex="19" OnClick="buttonMACD_EMA_Daily_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label22" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlMACDEMADaily_outputsize" runat="server" TabIndex="16">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <asp:ListItem Value="Compact">Compact</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label24" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlMACDEMADail_interval1" runat="server" TabIndex="20">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label26" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlMACDEMADail_seriestype1" runat="server" TabIndex="21">
                    <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                    <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label25" runat="server" Text="Fast Period:"></asp:Label>
                <asp:TextBox ID="textboxMACDEMADaily_fastperiod" runat="server" TextMode="Number" ReadOnly="true" Width="50" Text="12" TabIndex="22"></asp:TextBox>
                <br />
                <asp:Label ID="Label29" runat="server" Text="Slow Period:"></asp:Label>
                <asp:TextBox ID="textboxMACDEMADaily_slowperiod" runat="server" TextMode="Number" ReadOnly="true" Width="50" Text="26" TabIndex="23"></asp:TextBox>
                <br />
                <asp:Label ID="Label27" runat="server" Text="Signal Period:"></asp:Label>
                <asp:TextBox ID="textboxMACDEMADaily_signalperiod" runat="server" TextMode="Number" ReadOnly="true" Width="50" Text="9" TabIndex="24"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonRSIDaily" Text="Momentum Identifier" runat="server" Font-Size="Small" TabIndex="25" OnClick="buttonRSIDaily_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label31" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlRSIDaily_Outputsize" runat="server" TabIndex="26">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <%--<asp:ListItem Value="Compact">Compact</asp:ListItem>--%>
                    <asp:ListItem Value="Full">Compact</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label32" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlRSIDaily_Interval" runat="server" TabIndex="27">
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1m" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label33" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxRSIDaily_Period" runat="server" TextMode="Number" Width="50" Text="14" TabIndex="28"></asp:TextBox>
                <br />
                <asp:Label ID="Label34" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlRSIDaily_SeriesType" runat="server" TabIndex="29">
                    <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                    <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="button1" Text="Gauge Trends" runat="server" Font-Size="Small" TabIndex="30" OnClick="buttonBBandsDaily_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label28" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlBBandsDaily_Outputsize" runat="server" TabIndex="31">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <asp:ListItem Value="Compact">Compact</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label35" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlBBandsDaily_Interval" runat="server" TabIndex="32">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label48" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxBBandsDaily_Period" runat="server" TextMode="Number" Width="50" Text="20" TabIndex="33"></asp:TextBox>
                <br />
                <asp:Label ID="Label36" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlBBandsDaily_SeriesType" runat="server" TabIndex="34">
                    <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                    <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label42" runat="server" Text="Std Deviation: "></asp:Label>
                <asp:TextBox ID="textboxBBandsDaily_StdDev" runat="server" TextMode="Number" Width="50" Text="2" TabIndex="35"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonStochDaily" Text="Buy-Sell Indicator" Font-Size="Small" runat="server" TabIndex="37" OnClick="buttonStochDaily_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label41" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlStochDaily_OutuputSize" runat="server" TabIndex="38">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <%--<asp:ListItem Value="Compact">Compact</asp:ListItem>--%>
                    <asp:ListItem Value="Full">Compact</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label44" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlStochDaily_Interval" runat="server" TabIndex="39">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label49" runat="server" Text="FastK Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCHDaily_Fastkperiod" runat="server" TextMode="Number" Width="50" Text="5" TabIndex="40"></asp:TextBox>
                <br />
                <asp:Label ID="Label51" runat="server" Text="SlowD Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCHDaily_Slowdperiod" runat="server" TextMode="Number" Width="50" Text="3" TabIndex="42"></asp:TextBox>
                <br />
                <asp:Label ID="Label46" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlStochDailyRSI_Interval" runat="server" TabIndex="45">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label56" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxStochDailyRSI_Period" runat="server" TextMode="Number" Width="50" Text="14" TabIndex="46"></asp:TextBox>
                <br />
                <asp:Label ID="Label57" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlStochDailyRSI_SeriesType" runat="server" TabIndex="47">
                    <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                    <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonDMI" Text="Trend Direction" runat="server" Font-Size="Small" TabIndex="48" OnClick="buttonDMI_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label59" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlDMIDaily_Outputsize" runat="server" TabIndex="49">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <%--<asp:ListItem Value="Compact">Compact</asp:ListItem>--%>
                    <asp:ListItem Value="Full">Compact</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label64" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlDMIMINUSDI_Interval" runat="server" TabIndex="50">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label65" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxDMIMINUSDI_Period" runat="server" TextMode="Number" Width="50" Text="14" TabIndex="51"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonPrice" Text="Price Direction" runat="server" Font-Size="Small" TabIndex="56" OnClick="buttonPrice_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label73" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlPrice_Outputsize" runat="server" TabIndex="57">
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                    <%--<asp:ListItem Value="Compact">Compact</asp:ListItem>--%>
                    <asp:ListItem Value="Full">Compact</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label61" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlDMIDX_Interval" runat="server" TabIndex="58">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label62" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxDMIDX_Period" runat="server" TextMode="Number" Width="50" Text="14" TabIndex="59"></asp:TextBox>
            </td>
        </tr>

    </table>
</asp:Content>
