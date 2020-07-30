<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="showgraph.aspx.cs" Inherits="Analytics.showgraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Standard Analytical Graphs</h3>
    <div style="margin-top: 2%; text-align: center; border:thin;">
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
        <table style="border:thin;">
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="background-color: gray; text-align: center;">
                    <asp:Label ID="Label38" runat="server" Text="Graph"></asp:Label></td>
                <td style="background-color: gray; text-align: center;">
                    <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonDaily" Text="Daily (Open/High/Low/Close/Volume)" runat="server" OnClick="buttonDaily_Click" />
                </td>
                <td style="padding-left:4%;">
                    <asp:Label ID="Label4" runat="server" Text="Output size:"></asp:Label><br />
                    <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="1">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonIntraday" Text="Intra-day (Open/High/Low/Close/Volume)" runat="server" OnClick="buttonIntraday_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label5" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlIntraday_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Selected="True">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label6" runat="server" Text="Output size:"></asp:Label><br />
                    <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="1">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonSMA" Text="Simple moving average-SMA" runat="server" OnClick="buttonSMA_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label7" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlSMA_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label8" runat="server" Text="Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxSMA_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label10" runat="server" Text="Series Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlSMA_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonEMA" Text="Exponential moving average-EMA" runat="server" OnClick="buttonEMA_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label11" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlEMA_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label12" runat="server" Text="Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxEMA_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label13" runat="server" Text="Series Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlEMA_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonVWAPrice" runat="server" Text="Volume Weighted Avg Price-VWAP" OnClick="buttonVWAPrice_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label14" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlVWAP_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonRSI" Text="Relative strength index-RSI" runat="server" OnClick="buttonRSI_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label15" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlRSI_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label16" runat="server" Text="Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxRSI_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label17" runat="server" Text="Series Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlRSI_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonSTOCH" Text="Stochastic oscillator-STOCH" runat="server" OnClick="buttonSTOCH_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label18" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlSTOCH_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label19" runat="server" Text="FastK Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxSTOCH_Fastkperiod" runat="server" TextMode="Number" Text="5" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label20" runat="server" Text="SlowK Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxSTOCH_Slowkperiod" runat="server" TextMode="Number" Text="3" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label21" runat="server" Text="SlowD Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxSTOCH_Slowdperiod" runat="server" TextMode="Number" Text="3" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label22" runat="server" Text="SlowK MA Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlSTOCH_Slowkmatype" runat="server" TabIndex="3">
                        <asp:ListItem Value="0" Selected="True">Simple Moving Average (SMA)</asp:ListItem>
                        <asp:ListItem Value="1">Exponential Moving Average (EMA)</asp:ListItem>
                        <asp:ListItem Value="2">Weighted Moving Average (WMA)</asp:ListItem>
                        <asp:ListItem Value="3">Double Exponential Moving Average (DEMA)</asp:ListItem>
                        <asp:ListItem Value="4">Triple Exponential Moving Average (TEMA)</asp:ListItem>
                        <asp:ListItem Value="5">Triangular Moving Average (TRIMA)</asp:ListItem>
                        <asp:ListItem Value="6">T3 Moving Average</asp:ListItem>
                        <asp:ListItem Value="7">Kaufman Adaptive Moving Average (KAMA)</asp:ListItem>
                        <asp:ListItem Value="8">MESA Adaptive Moving Average (MAMA)</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label23" runat="server" Text="SlowD MA Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlSTOCH_Slowdmatype" runat="server" TabIndex="3">
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
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonMACD" Text="Moving average convergence / divergence-MACD" runat="server" OnClick="buttonMACD_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label24" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlMACD_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label25" runat="server" Text="Series Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlMACD_Series" runat="server" TabIndex="2">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label26" runat="server" Text="Fast Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxMACD_FastPeriod" runat="server" TextMode="Number" Text="12" TabIndex="3"></asp:TextBox><br />
                    <asp:Label ID="Label27" runat="server" Text="Slow Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxMACD_SlowPeriod" runat="server" TextMode="Number" Text="26" TabIndex="4"></asp:TextBox><br />
                    <asp:Label ID="Label28" runat="server" Text="Signal Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxMACD_SignalPeriod" runat="server" TextMode="Number" Text="9" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonAroon" Text="AROON" runat="server" OnClick="buttonAroon_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label29" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlAroon_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label30" runat="server" Text="Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxAroon_Period" runat="server" TextMode="Number" Text="20" TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonAdx" Text="Average directional movement index-ADX" runat="server" OnClick="buttonAdx_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label31" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlAdx_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label32" runat="server" Text="Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxAdx_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                    <asp:Button ID="buttonBBands" Text="Bollinger Bands" runat="server" OnClick="buttonBBands_Click" />
                </td>

                <td style="padding-left:4%;">
                    <asp:Label ID="Label33" runat="server" Text="Interval:"></asp:Label><br />
                    <asp:DropDownList ID="ddlBBands_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label34" runat="server" Text="Period:"></asp:Label><br />
                    <asp:TextBox ID="textboxBBands_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox><br />
                    <asp:Label ID="Label35" runat="server" Text="Series Type:"></asp:Label><br />
                    <asp:DropDownList ID="ddlBBands_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Label ID="Label36" runat="server" Text="Deviation multiplier for upper band(nbDevUp):"></asp:Label><br />
                    <asp:TextBox ID="textboxBBands_NbdevUp" runat="server" TextMode="Number" Text="2" TabIndex="4"></asp:TextBox><br />
                    <asp:Label ID="Label37" runat="server" Text="Deviation multiplier for lower band(nbDevDn):"></asp:Label><br />
                    <asp:TextBox ID="textboxBBands_NbdevDn" runat="server" TextMode="Number" Text="2" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
