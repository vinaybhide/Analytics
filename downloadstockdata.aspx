<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="downloadstockdata.aspx.cs" Inherits="Analytics.downloadstockdata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Download data for off-line mode</h3>
    <div class="container" style="margin-top: 2%;">
        <div>
            <asp:Label ID="Label1" runat="server" Style="text-align: right" Text="Search Stock:"></asp:Label>
            <asp:TextBox ID="TextBoxSearch" runat="server" TabIndex="1"></asp:TextBox>
            <asp:Label ID="Label2" runat="server"></asp:Label>
            <asp:Button ID="ButtonSearch" runat="server" Text="Search" TabIndex="2" OnClick="ButtonSearch_Click" />
            <asp:Label ID="Label3" runat="server"></asp:Label>
            <asp:DropDownList ID="DropDownListStock" runat="server" AutoPostBack="True" TabIndex="3" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged"></asp:DropDownList>
            <asp:Label ID="Label9" runat="server"></asp:Label>
            <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label><br />
            <asp:Button ID="buttonDownloadAll" runat="server" Text="Download All Functions" OnClick="buttonDownloadAll_Click" />
            <asp:Button ID="buttonDownloadSelected" runat="server" Text="Download Selected Functions" OnClick="buttonDownloadSelected_Click" />
        </div>
    </div>
    <hr />

    <div>
        <table style="width: 100%; border:thin;">
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%; background-color: gray; text-align:center;">
                    <asp:Label ID="Label38" runat="server" Text="Function Name"></asp:Label></td>
                <td style="width: 80%; background-color: gray; text-align:center;">
                    <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxQuote" Text="Get Quote" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">&nbsp
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxDaily" Text="Daily (Open/High/Low/Close/Volume)" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label4" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="1">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxIntraday" Text="Intra-day (Open/High/Low/Close/Volume)" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label5" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlIntraday_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min" Selected="True">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <%--<asp:ListItem Value="daily">Daily</asp:ListItem>
                            <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                            <asp:ListItem Value="monthly">Monthly</asp:ListItem>--%>
                    </asp:DropDownList>
                    <asp:Label ID="Label6" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="1">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxSMA" Text="Simple moving average-SMA" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label7" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label8" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxSMA_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="Label10" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxEMA" Text="Exponential moving average-EMA" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label11" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlEMA_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label12" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxEMA_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="Label13" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlEMA_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxVWAP" Text="Volume weighted average price-VWAP" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label14" runat="server" Text="Interval:"></asp:Label>
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
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxRSI" Text="Relative strength index-RSI" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label15" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlRSI_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label16" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxRSI_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="Label17" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlRSI_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxSTOCH" Text="Stochastic oscillator-STOCH" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label18" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlSTOCH_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label19" runat="server" Text="FastK Period:"></asp:Label>
                    <asp:TextBox ID="textboxSTOCH_Fastkperiod" runat="server" TextMode="Number" Text="5" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="Label20" runat="server" Text="SlowK Period:"></asp:Label>
                    <asp:TextBox ID="textboxSTOCH_Slowkperiod" runat="server" TextMode="Number" Text="3" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="Label21" runat="server" Text="SlowD Period:"></asp:Label>
                    <asp:TextBox ID="textboxSTOCH_Slowdperiod" runat="server" TextMode="Number" Text="3" TabIndex="2"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label22" runat="server" Text="SlowK MA Type:"></asp:Label>
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
                    </asp:DropDownList>
                    <asp:Label ID="Label23" runat="server" Text="SlowD MA Type:"></asp:Label>
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
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxMACD" Text="Moving average convergence / divergence-MACD" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label24" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlMACD_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label25" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlMACD_Series" runat="server" TabIndex="2">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label26" runat="server" Text="Fast Period:"></asp:Label>
                    <asp:TextBox ID="textboxMACD_FastPeriod" runat="server" TextMode="Number" Text="12" TabIndex="3"></asp:TextBox>
                    <asp:Label ID="Label27" runat="server" Text="Slow Period:"></asp:Label>
                    <asp:TextBox ID="textboxMACD_SlowPeriod" runat="server" TextMode="Number" Text="26" TabIndex="4"></asp:TextBox>
                    <asp:Label ID="Label28" runat="server" Text="Signal Period:"></asp:Label>
                    <asp:TextBox ID="textboxMACD_SignalPeriod" runat="server" TextMode="Number" Text="9" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxAroon" Text="AROON" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label29" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlAroon_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label30" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxAroon_Period" runat="server" TextMode="Number" Text="20" TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxAdx" Text="Average directional movement index-ADX" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label31" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlAdx_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label32" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxAdx_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="width: 20%;">
                    <asp:CheckBox ID="checkboxBBands" Text="Bollinger Bands" runat="server" Font-Size="Smaller" />
                </td>
                <td style="width: 80%">
                    <asp:Label ID="Label33" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlBBands_Interval" runat="server" TabIndex="1">
                        <asp:ListItem Value="1min">1 min</asp:ListItem>
                        <asp:ListItem Value="5min">5 min</asp:ListItem>
                        <asp:ListItem Value="15min">15 min</asp:ListItem>
                        <asp:ListItem Value="30min">30 min</asp:ListItem>
                        <asp:ListItem Value="60min">60 min</asp:ListItem>
                        <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                        <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                        <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label34" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxBBands_Period" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="Label35" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlBBands_Series" runat="server" TabIndex="3">
                        <asp:ListItem Value="open">Open</asp:ListItem>
                        <asp:ListItem Value="high">High</asp:ListItem>
                        <asp:ListItem Value="low">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Label ID="Label36" runat="server" Text="Deviation multiplier for upper band(nbDevUp):"></asp:Label>
                    <asp:TextBox ID="textboxBBands_NbdevUp" runat="server" TextMode="Number" Text="2" TabIndex="4"></asp:TextBox>
                    <asp:Label ID="Label37" runat="server" Text="Deviation multiplier for lower band(nbDevDn):"></asp:Label>
                    <asp:TextBox ID="textboxBBands_NbdevDn" runat="server" TextMode="Number" Text="2" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr style="width: 100%; border-color: black; border-top-style: double; border-bottom-style:solid; border-width: 1px;">
                <td colspan="2" style="width: 100%;">
                    <asp:Label ID="textboxMessage" runat="server" Text="Message:"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
