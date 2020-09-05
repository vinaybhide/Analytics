<%@ Page Title="Download Data" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="downloadstockdata.aspx.cs" Inherits="Analytics.downloadstockdata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .content {
            width: 100%;
            min-width: 50%;
        }
    </style>

    <%--<h3 style="text-align: center; margin-top: 1%;">Download data for off-line mode</h3>--%>
   
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
        <tr>
            <td colspan="3" style="text-align: center;border:solid; border-width:1px; border-style:solid;">
                <asp:Label ID="Label48" runat="server" Text="Download data for off-line mode"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 50%;">
                <asp:Label ID="Label1" runat="server" Style="text-align: right" Text="Search Stock:"></asp:Label>
            </td>
            <td >
                <asp:TextBox ID="TextBoxSearch" Width="100" runat="server" TabIndex="1"></asp:TextBox>
            </td>
            <td >
                <asp:Button ID="ButtonSearch" runat="server" Text="Search Online" TabIndex="2" OnClick="ButtonSearch_Click" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 50%;">
                <asp:Label ID="label3" Text="Portfolio:" runat="server"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlPortfolios" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="ButtonSearchPortfolio" runat="server" Text="Search Portfolio" Font-Size="Small" TabIndex="2" OnClick="ButtonSearchPortfolio_Click" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 50%;">
                <asp:Label ID="label49" Text="Select:" runat="server"></asp:Label>
            </td>
            <td colspan="2">
                <asp:DropDownList ID="DropDownListStock" runat="server" AutoPostBack="True" TabIndex="3" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td> </td>
            <td style="width: 10%; text-align: left;">
                <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="text-align: right; width: 50%;">
                <asp:Button ID="buttonDownloadAll" Visible="false" runat="server" Text="Download All" OnClick="buttonDownloadAll_Click" Enabled="False" />
            </td>
            <td style="text-align: center; ">
                <asp:Button ID="buttonDownloadSelected" runat="server" Text="Download Selected" OnClick="buttonDownloadSelected_Click" />
            </td>
            <td style="text-align: left; ">
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="3" OnClick="buttonBack_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: center;">
                <asp:TextBox ID="textboxMessage" runat="server" CssClass="content" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <%--<hr />--%>

    <%--<div style="padding-left: 1%;">--%>
    <table style="width: 100%; border: thin;">
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; text-align: center; background-color: gray; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Label ID="Label38" runat="server" Text="Function Name"></asp:Label></td>
            <td style="width: 70%; text-align: center; background-color: gray; border: solid; border-width: 1px; border-color: black;">
                <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:CheckBox ID="checkboxQuote" Text="Get Quote" runat="server" Font-Bold="false" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">&nbsp
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:CheckBox ID="checkboxDaily" Text="Daily" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
                <asp:Label ID="Label4" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="1">
                    <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                    <asp:ListItem Value="full">Full</asp:ListItem>
                </asp:DropDownList><br />
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxIntraday" Text="Intra-day" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                </asp:DropDownList><br />
                <asp:Label ID="Label6" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="1">
                    <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                    <asp:ListItem Value="full">Full</asp:ListItem>
                </asp:DropDownList><br />
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxSMA" Text="SMA" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                </asp:DropDownList><br />
                <asp:Label ID="Label8" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxSMA_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label10" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlSMA_Series" runat="server" TabIndex="3">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxEMA" Text="EMA" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                <br />
                <asp:Label ID="Label12" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxEMA_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label13" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlEMA_Series" runat="server" TabIndex="3">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxVWAP" Text="VWAP" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
                <asp:Label ID="Label14" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlVWAP_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Selected="True">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxRSI" Text="RSI" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%; border: solid; border-width: 1px; border-color: black;">
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
                <br />
                <asp:Label ID="Label16" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxRSI_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label17" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlRSI_Series" runat="server" TabIndex="3">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxSTOCH" Text="Stochastics" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                <br />
                <asp:Label ID="Label19" runat="server" Text="FastK Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Fastkperiod" runat="server" TextMode="Number" Width="40" Text="5" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label20" runat="server" Text="SlowK Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Slowkperiod" runat="server" TextMode="Number" Width="40" Text="3" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label21" runat="server" Text="SlowD Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Slowdperiod" runat="server" TextMode="Number" Width="40" Text="3" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label22" runat="server" Text="SlowK MA Type:"></asp:Label>
                <asp:DropDownList ID="ddlSTOCH_Slowkmatype" runat="server" TabIndex="3">
                    <asp:ListItem Value="0" Selected="True">SMA</asp:ListItem>
                    <asp:ListItem Value="1">EMA</asp:ListItem>
                    <asp:ListItem Value="2">WMA</asp:ListItem>
                    <asp:ListItem Value="3">DEMA</asp:ListItem>
                    <asp:ListItem Value="4">TEMA</asp:ListItem>
                    <asp:ListItem Value="5">TRIMA</asp:ListItem>
                    <asp:ListItem Value="6">T3 MA</asp:ListItem>
                    <asp:ListItem Value="7">KAMA</asp:ListItem>
                    <asp:ListItem Value="8">MAMA</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label23" runat="server" Text="SlowD MA Type:"></asp:Label>
                <asp:DropDownList ID="ddlSTOCH_Slowdmatype" runat="server" TabIndex="3">
                    <asp:ListItem Value="0" Selected="True">SMA</asp:ListItem>
                    <asp:ListItem Value="1">EMA</asp:ListItem>
                    <asp:ListItem Value="2">WMA</asp:ListItem>
                    <asp:ListItem Value="3">DEMA</asp:ListItem>
                    <asp:ListItem Value="4">TEMA</asp:ListItem>
                    <asp:ListItem Value="5">TRIMA</asp:ListItem>
                    <asp:ListItem Value="6">T3 MA</asp:ListItem>
                    <asp:ListItem Value="7">KAMA</asp:ListItem>
                    <asp:ListItem Value="8">MAMA</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxMACD" Text="MACD" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                <br />
                <asp:Label ID="Label25" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlMACD_Series" runat="server" TabIndex="2">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label26" runat="server" Text="Fast Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_FastPeriod" runat="server" TextMode="Number" Width="40" Text="12" TabIndex="3"></asp:TextBox>
                <br />
                <asp:Label ID="Label27" runat="server" Text="Slow Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_SlowPeriod" runat="server" TextMode="Number" Width="40" Text="26" TabIndex="4"></asp:TextBox>
                <br />
                <asp:Label ID="Label28" runat="server" Text="Signal Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_SignalPeriod" runat="server" TextMode="Number" Width="40" Text="9" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxAroon" Text="AROON" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                <br />
                <asp:Label ID="Label30" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxAroon_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="3"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxAdx" Text="ADX" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                <br />
                <asp:Label ID="Label32" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxAdx_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxBBands" Text="Bollinger Bands" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
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
                <br />
                <asp:Label ID="Label34" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxBBands_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                <br />
                <asp:Label ID="Label35" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlBBands_Series" runat="server" TabIndex="3">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label36" runat="server" Text="Deviation multiplier for upper band(nbDevUp):"></asp:Label>
                <asp:TextBox ID="textboxBBands_NbdevUp" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="4"></asp:TextBox>
                <br />
                <asp:Label ID="Label37" runat="server" Text="Deviation multiplier for lower band(nbDevDn):"></asp:Label>
                <asp:TextBox ID="textboxBBands_NbdevDn" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxDX" Text="DX" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
                <asp:Label ID="Label2" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlDX_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label9" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxDX_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxMinusDI" Text="MINUS DI" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
                <asp:Label ID="Label40" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlMinusDI_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label41" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxMinusDI_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxPlusDI" Text="PLUS DI" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
                <asp:Label ID="Label42" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlPlusDI_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label43" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxPlusDI_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="2"></asp:TextBox>
            </td>
        </tr>

        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxMinusDM" Text="MINUS DM" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%;">
                <asp:Label ID="Label44" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlMinusDM_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label45" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxMinusDM_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 30%; border: solid; border-width: 1px; border-color: black;">
                <asp:CheckBox ID="checkboxPlusDM" Text="Plus DM" runat="server" Font-Size="Smaller" />
            </td>
            <td style="width: 70%; border-color: black; border-style: solid; border-width: 1px;">
                <asp:Label ID="Label46" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlPlusDM_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly">Monthly</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label47" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxPlusDM_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
    </table>
    <%--</div>--%>
</asp:Content>
