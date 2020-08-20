<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="advancegraphs.aspx.cs" Inherits="Analytics.advancegraphs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align: center; margin-top: 2%;">Advanced Analytical Graphs</h3>
    <div style="margin-top: 2%; text-align: center; border: thin;">
        <div>
            <asp:Label ID="Label1" runat="server" Style="text-align: right" Text="Search Stock:"></asp:Label>
            <asp:TextBox ID="TextBoxSearch" runat="server" TabIndex="1"></asp:TextBox><br />
            <asp:Button ID="ButtonSearch" runat="server" Text="Search" TabIndex="2" OnClick="ButtonSearch_Click" /><br />
            <asp:DropDownList ID="DropDownListStock" runat="server" AutoPostBack="True" TabIndex="3" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <hr />
    <div>
        <table style="border: solid; border-width: thin; width: 100%; font-size: x-small;">
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 10%; background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label38" runat="server" Text="Graph"></asp:Label></td>
                <td style="width: 10%; background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label5" runat="server" Text="Sub-Graphs"></asp:Label></td>
                <td style="width: 80%; background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
            </tr>
            <tr>
                <td rowspan="4" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonVWAPIntra" Text="Intra-day Indicator" runat="server" TabIndex="4" OnClick="buttonVWAPIntra_Click" />
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label6" runat="server" Text="Intra-day"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label4" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlIntraday_Interval" runat="server" TabIndex="5">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Selected="True">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <%--<asp:ListItem Value="daily">Daily</asp:ListItem>
                            <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                            <asp:ListItem Value="monthly">Monthly</asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label7" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="6">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label8" runat="server" Text="VWAP"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label14" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlVWAP_Interval" runat="server" TabIndex="7">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Selected="True">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="10" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonCrossover" Text="Crossover (Buy/Sell Signal)" runat="server" TabIndex="8" OnClick="buttonCrossover_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label10" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label11" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="9">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="4" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label12" runat="server" Text="SMA 1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%;">
                    <asp:Label ID="Label13" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA1_Interval" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label15" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxSMA1_Period" runat="server" TextMode="Number" Text="50" TabIndex="11"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label16" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA1_Series" runat="server" TabIndex="12">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="4" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label17" runat="server" Text="SMA 2"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%;">
                    <asp:Label ID="Label18" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA2_Interval" runat="server" TabIndex="13">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label19" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxSMA2_Period" runat="server" TextMode="Number" Text="100" TabIndex="14"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label20" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA2_Series" runat="server" TabIndex="15">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="8" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonMACD_EMA_Daily" Text="Trend Reversal Indicator" runat="server" TabIndex="16" OnClick="buttonMACD_EMA_Daily_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label21" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label22" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlMACDEMADaily_outputsize" runat="server" TabIndex="17">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="6" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label23" runat="server" Text="MACD"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%;">
                    <asp:Label ID="Label24" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlMACDEMADail_interval1" runat="server" TabIndex="18">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label26" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlMACDEMADail_seriestype1" runat="server" TabIndex="20">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label25" runat="server" Text="Fast Period:"></asp:Label>
                    <asp:TextBox ID="textboxMACDEMADaily_fastperiod" runat="server" TextMode="Number" ReadOnly="true" Text="12" TabIndex="19"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label29" runat="server" Text="Slow Period:"></asp:Label>
                    <asp:TextBox ID="textboxMACDEMADaily_slowperiod" runat="server" TextMode="Number" ReadOnly="true" Text="26" TabIndex="20"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label27" runat="server" Text="Signal Period:"></asp:Label>
                    <asp:TextBox ID="textboxMACDEMADaily_signalperiod" runat="server" TextMode="Number" ReadOnly="true" Text="9" TabIndex="21"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td rowspan="6" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonRSIDaily" Text="Momentum Indicator" runat="server" TabIndex="22" OnClick="buttonRSIDaily_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label2" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label31" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlRSIDaily_Outputsize" runat="server" TabIndex="23">
                        <asp:ListItem Value="compact">Compact</asp:ListItem>
                        <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="4" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label3" runat="server" Text="RSI"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%;">
                    <asp:Label ID="Label32" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlRSIDaily_Interval" runat="server" TabIndex="24">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label33" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxRSIDaily_Period" runat="server" TextMode="Number" Text="20" TabIndex="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label34" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlRSIDaily_SeriesType" runat="server" TabIndex="26">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="5" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="button1" Text="Gauge Trends" runat="server" TabIndex="27" OnClick="buttonBBandsDaily_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label9" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label28" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlBBandsDaily_Outputsize" runat="server" TabIndex="28">
                        <asp:ListItem Value="compact">Compact</asp:ListItem>
                        <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label30" runat="server" Text="Bollinger Bands"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label35" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlBBandsDaily_Interval" runat="server" TabIndex="29">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label48" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxBBandsDaily_Period" runat="server" TextMode="Number" Width="5%" Text="20" TabIndex="30"></asp:TextBox>
                    <asp:Label ID="Label36" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlBBandsDaily_SeriesType" runat="server" TabIndex="31">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label42" runat="server" Text="Deviation multiplier for upper band(nbDevUp):"></asp:Label>
                    <asp:TextBox ID="textboxBBandsDaily_NbdevUp" runat="server" TextMode="Number" Width="5%" Text="2" TabIndex="32"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label37" runat="server" Text="Deviation multiplier for upper band(nbDevUp):"></asp:Label>
                    <asp:TextBox ID="textboxBBandsDaily_NbdevDn" runat="server" TextMode="Number" Width="5%" Text="2" TabIndex="33"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td rowspan="9" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonStochDaily" Text="Buy & Sell Indicator" runat="server" TabIndex="34" OnClick="buttonStochDaily_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label40" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label41" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlStochDaily_OutuputSize" runat="server" TabIndex="35">
                        <asp:ListItem Value="compact">Compact</asp:ListItem>
                        <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="4" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label43" runat="server" Text="Stochastics"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label44" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlStochDaily_Interval" runat="server" TabIndex="36">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label49" runat="server" Text="FastK Period:"></asp:Label>
                    <asp:TextBox ID="textboxSTOCHDaily_Fastkperiod" runat="server" TextMode="Number" Width="5%" Text="5" TabIndex="37"></asp:TextBox>
                    <asp:Label ID="Label50" runat="server" Text="SlowK Period:"></asp:Label>
                    <asp:TextBox ID="textboxSTOCHDaily_Slowkperiod" runat="server" TextMode="Number" Width="5%" Text="3" TabIndex="38"></asp:TextBox>
                    <asp:Label ID="Label51" runat="server" Text="SlowD Period:"></asp:Label>
                    <asp:TextBox ID="textboxSTOCHDaily_Slowdperiod" runat="server" TextMode="Number" Width="5%" Text="3" TabIndex="39"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label52" runat="server" Text="SlowK MA Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSTOCHDaily_Slowkmatype" runat="server" TabIndex="40">
                        <asp:ListItem Value="0" Selected="True">Simple Moving Average (SMA)</asp:ListItem>
                        <asp:ListItem Value="1">Exponential Moving Average (EMA)</asp:ListItem>
                        <asp:ListItem Value="2">Weighted Moving Average (WMA)</asp:ListItem>
                        <asp:ListItem Value="3">Double Exponential Moving Average (DEMA)</asp:ListItem>
                        <asp:ListItem Value="4">Triple Exponential Moving Average (TEMA)</asp:ListItem>
                        <asp:ListItem Value="5">Triangular Moving Average (TRIMA)</asp:ListItem>
                        <asp:ListItem Value="6">T3 Moving Average</asp:ListItem>
                        <asp:ListItem Value="7">Kaufman Adaptive Moving Average (KAMA)</asp:ListItem>
                        <asp:ListItem Value="8">MESA Adaptive Moving Average (MAMA)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label53" runat="server" Text="SlowD MA Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSTOCHDaily_Slowdmatype" runat="server" TabIndex="41">
                        <asp:ListItem Value="0" Selected="True">Simple Moving Average (SMA)</asp:ListItem>
                        <asp:ListItem Value="1">Exponential Moving Average (EMA)</asp:ListItem>
                        <asp:ListItem Value="2">Weighted Moving Average (WMA)</asp:ListItem>
                        <asp:ListItem Value="3">Double Exponential Moving Average (DEMA)</asp:ListItem>
                        <asp:ListItem Value="4">Triple Exponential Moving Average (TEMA)</asp:ListItem>
                        <asp:ListItem Value="5">Triangular Moving Average (TRIMA)</asp:ListItem>
                        <asp:ListItem Value="6">T3 Moving Average</asp:ListItem>
                        <asp:ListItem Value="7">Kaufman Adaptive Moving Average (KAMA)</asp:ListItem>
                        <asp:ListItem Value="8">MESA Adaptive Moving Average (MAMA)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label45" runat="server" Text="RSI"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label46" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="DropDownList1" runat="server" TabIndex="36">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label56" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxStochDailyRSI_Period" runat="server" TextMode="Number" Text="20" TabIndex="26"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label57" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlStochDailyRSI_SeriesType" runat="server" TabIndex="27">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="14" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonDMI" Text="Trend Direction" runat="server" TabIndex="34" OnClick="buttonDMI_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label58" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label59" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlDMIDaily_Outputsize" runat="server" TabIndex="35">
                        <asp:ListItem Value="compact">Compact</asp:ListItem>
                        <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label60" runat="server" Text="Directional Movement (DX)"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label61" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlDMIDX_Interval" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label62" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxDMIDX_Period" runat="server" TextMode="Number" Text="14" TabIndex="11"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label63" runat="server" Text="-ve Directional Movement(-DI)"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label64" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlDMIMINUSDI_Interval" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label65" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxDMIMINUSDI_Period" runat="server" TextMode="Number" Text="14" TabIndex="11"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label66" runat="server" Text="+ve Directional Movement(+DI)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label67" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlDMIPLUSDI_Interval" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label68" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxDMIPLUSDI_Interval" runat="server" TextMode="Number" Text="14" TabIndex="11"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label47" runat="server" Text="Avg Directional Movement Index(ADX)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label54" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlDMIADX_Interval" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label55" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxDMIADX_Period" runat="server" TextMode="Number" Text="14" TabIndex="11"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td rowspan="12" style="width: 10%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonPrice" Text="Price Direction & Strength" runat="server" TabIndex="34" OnClick="buttonPrice_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label72" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label73" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlPrice_Outputsize" runat="server" TabIndex="35">
                        <asp:ListItem Value="compact">Compact</asp:ListItem>
                        <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label77" runat="server" Text="Minus DMI"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label78" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlPriceMINUSDMI_Interval" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label79" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxPriceMINUSDMI" runat="server" TextMode="Number" Text="14" TabIndex="11"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 10%; text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label80" runat="server" Text="Plus DMI"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label81" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlPricePLUSDMI" runat="server" TabIndex="10">
                        <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                        <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                        <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                        <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 80%; padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label82" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxPricePlusDMI" runat="server" TextMode="Number" Text="14" TabIndex="11"></asp:TextBox>
                </td>
            </tr>

        </table>
    </div>

</asp:Content>
