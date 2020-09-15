<%@ Page Title="Standard Graphs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="showgraph.aspx.cs" Inherits="Analytics.showgraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
        <tr>
            <td colspan="3" style="text-align: center;border:solid; border-width:1px; border-style:solid;">
                <asp:Label ID="Label48" runat="server" Text="Standard Graphs"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
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
            <td></td>
            <td style="width: 10%; text-align: center;">
                <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>
            </td>
            <td></td>
        </tr>
    </table>

    <table style="width: 100%;">
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; background-color: gray; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Label ID="Label38" runat="server" Text="Graph"></asp:Label></td>
            <td style="width: 80%; background-color: gray; text-align: center;">
                <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonDaily" Text="Show Daily" runat="server" OnClick="buttonDaily_Click" />
            </td>
            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label4" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="1">
                    <asp:ListItem Value="compact" Enabled="false">Compact</asp:ListItem>
                    <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonIntraday" Text="Show Intra-day" runat="server" OnClick="buttonIntraday_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label5" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Selected="True">5 min</asp:ListItem>
                    <asp:ListItem Value="15min">15 min</asp:ListItem>
                    <asp:ListItem Value="30min">30 min</asp:ListItem>
                    <asp:ListItem Value="60min">60 min</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label6" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="1">
                    <asp:ListItem Value="compact" Enabled="false">Compact</asp:ListItem>
                    <asp:ListItem Value="full" Selected="True">Full</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonSMA" Text="Show SMA" runat="server" OnClick="buttonSMA_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label7" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlSMA_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label8" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxSMA_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox><br />
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
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonEMA" Text="Show EMA" runat="server" OnClick="buttonEMA_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label11" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlEMA_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label12" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxEMA_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox><br />
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
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonVWAPrice" runat="server" Text="Show VWAP" OnClick="buttonVWAPrice_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
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
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonRSI" Text="Show RSI" runat="server" OnClick="buttonRSI_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label15" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlRSI_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label16" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxRSI_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox><br />
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
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonSTOCH" Text="Show Stochastics" runat="server" OnClick="buttonSTOCH_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label18" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlSTOCH_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label19" runat="server" Text="FastK Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Fastkperiod" runat="server" TextMode="Number" Width="40" Text="5" TabIndex="2"></asp:TextBox><br />
                <asp:Label ID="Label20" runat="server" Text="SlowK Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Slowkperiod" runat="server" TextMode="Number" Width="40" Text="3" TabIndex="2"></asp:TextBox><br />
                <asp:Label ID="Label21" runat="server" Text="SlowD Period:"></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Slowdperiod" runat="server" TextMode="Number" Width="40" Text="3" TabIndex="2"></asp:TextBox><br />
                <asp:Label ID="Label22" runat="server" Text="SlowK MA Type:"></asp:Label>
                <asp:DropDownList ID="ddlSTOCH_Slowkmatype" runat="server" TabIndex="3">
                    <asp:ListItem Value="0" Selected="True">SMA</asp:ListItem>
                        <asp:ListItem Value="1" Enabled="false">EMA</asp:ListItem>
                        <asp:ListItem Value="2" Enabled="false">WMA</asp:ListItem>
                        <asp:ListItem Value="3" Enabled="false">DEMA</asp:ListItem>
                        <asp:ListItem Value="4" Enabled="false">TEMA</asp:ListItem>
                        <asp:ListItem Value="5" Enabled="false">TRIMA</asp:ListItem>
                        <asp:ListItem Value="6" Enabled="false">T3 MA</asp:ListItem>
                        <asp:ListItem Value="7" Enabled="false">KAMA</asp:ListItem>
                        <asp:ListItem Value="8" Enabled="false">MAMA</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label23" runat="server" Text="SlowD MA Type:"></asp:Label>
                <asp:DropDownList ID="ddlSTOCH_Slowdmatype" runat="server" TabIndex="3">
                    <asp:ListItem Value="0" Selected="True">SMA</asp:ListItem>
                        <asp:ListItem Value="1" Enabled="false">EMA</asp:ListItem>
                        <asp:ListItem Value="2" Enabled="false">WMA</asp:ListItem>
                        <asp:ListItem Value="3" Enabled="false">DEMA</asp:ListItem>
                        <asp:ListItem Value="4" Enabled="false">TEMA</asp:ListItem>
                        <asp:ListItem Value="5" Enabled="false">TRIMA</asp:ListItem>
                        <asp:ListItem Value="6" Enabled="false">T3 MA</asp:ListItem>
                        <asp:ListItem Value="7" Enabled="false">KAMA</asp:ListItem>
                        <asp:ListItem Value="8" Enabled="false">MAMA</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonMACD" Text="Show MACD" runat="server" OnClick="buttonMACD_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label24" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlMACD_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false" >1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false" >5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false" >15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false" >30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false" >60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false" >Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false" >Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label25" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlMACD_Series" runat="server" TabIndex="2">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label26" runat="server" Text="Fast Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_FastPeriod" runat="server" TextMode="Number" Width="40" Text="12" TabIndex="3"></asp:TextBox><br />
                <asp:Label ID="Label27" runat="server" Text="Slow Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_SlowPeriod" runat="server" TextMode="Number" Width="40" Text="26" TabIndex="4"></asp:TextBox><br />
                <asp:Label ID="Label28" runat="server" Text="Signal Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_SignalPeriod" runat="server" TextMode="Number" Width="40" Text="9" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonAroon" Text="Show AROON" runat="server" OnClick="buttonAroon_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label29" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlAroon_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label30" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxAroon_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="3"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonAdx" Text="Show ADX" runat="server" OnClick="buttonAdx_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%; ">
                <asp:Label ID="Label31" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlAdx_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label32" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxAdx_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                <asp:Button ID="buttonBBands" Text="Show Bollinger Bands" runat="server" OnClick="buttonBBands_Click" Font-Size="Small" />
            </td>

            <td style="width: 80%; padding-left: 1%;  border-color: black; border-style: solid; border-width: 1px;">
                <asp:Label ID="Label33" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlBBands_Interval" runat="server" TabIndex="1">
                    <asp:ListItem Value="1min" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5min" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15min" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30min" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60min" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="daily" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="weekly" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="monthly" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label34" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxBBands_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox><br />
                <asp:Label ID="Label35" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlBBands_Series" runat="server" TabIndex="3">
                    <asp:ListItem Value="open">Open</asp:ListItem>
                    <asp:ListItem Value="high">High</asp:ListItem>
                    <asp:ListItem Value="low">Low</asp:ListItem>
                    <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label36" runat="server" Text="Deviation multiplier for upper band:" Font-Size="Small"></asp:Label>
                <asp:TextBox ID="textboxBBands_NbdevUp" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="4"></asp:TextBox><br />
                <asp:Label ID="Label37" runat="server" Text="Deviation multiplier for lower band:"  Font-Size="Small"></asp:Label>
                <asp:TextBox ID="textboxBBands_NbdevDn" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
