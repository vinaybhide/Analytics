<%@ Page Title="Standard Graphs" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="showgraph.aspx.cs" Inherits="Analytics.showgraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px; margin-top: 2%;">
        <tr>
            <td colspan="3" style="text-align: center; border: solid; border-width: 1px; border-style: solid;">
                <asp:Label ID="Label48" runat="server" Font-Size="Large" Text="Standard Graphs"></asp:Label>
            </td>
        </tr>
        <%--<tr>
            <td colspan="3" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>--%>
    </table>
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="Label2" runat="server" Style="text-align: right" Text="Select Exchange:"></asp:Label>
            </td>
            <td style="width: 10%;">
                <asp:DropDownList ID="ddlExchange" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlExchange_SelectedIndexChanged" TabIndex="1">
                    <asp:ListItem Text="NSE" Selected="True" Value="NS"></asp:ListItem>
                    <asp:ListItem Text="BSE" Value="BO"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="ButtonGetAllForExchange" runat="server" Text="Get Company list from exchange" TabIndex="2" OnClick="ButtonGetAllForExchange_Click" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="label3" Text="Select Portfolio :" runat="server"></asp:Label>
            </td>
            <td style="width: 10%;">
                <asp:DropDownList ID="ddlPortfolios" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlPortfolios_SelectedIndexChanged" TabIndex="3"></asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="ButtonSearchPortfolio" runat="server" Text="Get company list from portfolio" TabIndex="4" OnClick="ButtonSearchPortfolio_Click" />
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">

        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="Label1" runat="server" Style="text-align: right" Text="Filter by Company name :"></asp:Label>
            </td>
            <td style="width: 10%;">
                <asp:TextBox ID="TextBoxSearch" AutoPostBack="true" runat="server" TabIndex="5" ToolTip="Enter initial few characters of company name" OnTextChanged="ButtonSearch_Click"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="ButtonSearch" runat="server" Text="Search" TabIndex="6" OnClick="ButtonSearch_Click" />
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
        <tr>
            <td style="text-align: right; width: 40%;">
                <asp:Label ID="label49" Text="Select stock or company :" runat="server"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListStock" runat="server" AutoPostBack="True" TabIndex="7 " OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">

        <tr>
            <td style="text-align: right; width: 40%;"></td>
            <td colspan="2" style="width: 100%; text-align: left;">
                <%--<asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>--%>
                <asp:TextBox ID="textboxSelectedSymbol" Width="100" runat="server" TabIndex="8"></asp:TextBox>
            </td>
        </tr>
    </table>

    <table style="width: 100%; margin-top: 1%;">
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
            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label4" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="9">
                    <asp:ListItem Value="Compact" Enabled="false">Compact</asp:ListItem>
                    <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonIntraday" Text="Show Intra-day" runat="server" OnClick="buttonIntraday_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label5" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_Interval" runat="server" TabIndex="10">
                    <asp:ListItem Value="1m">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Selected="True">5 min</asp:ListItem>
                    <asp:ListItem Value="15m">15 min</asp:ListItem>
                    <asp:ListItem Value="30m">30 min</asp:ListItem>
                    <asp:ListItem Value="60m">60 min</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label6" runat="server" Text="Output size:"></asp:Label>
                <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="11">
                    <asp:ListItem Value="Compact" Selected="True">Compact</asp:ListItem>
                    <asp:ListItem Value="Full"  Enabled="false">Full</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonSMA" Text="Show SMA" runat="server" OnClick="buttonSMA_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label7" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlSMA_Interval" runat="server" TabIndex="12">
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label8" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxSMA_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="13"></asp:TextBox><br />
                <asp:Label ID="Label10" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlSMA_Series" runat="server" TabIndex="14">
                    <asp:ListItem Value="OPEN">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH">High</asp:ListItem>
                    <asp:ListItem Value="LOW">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonEMA" Text="Show EMA" runat="server" OnClick="buttonEMA_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label11" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlEMA_Interval" runat="server" TabIndex="15">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label12" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxEMA_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="16"></asp:TextBox><br />
                <asp:Label ID="Label13" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlEMA_Series" runat="server" TabIndex="17">
                    <asp:ListItem Value="OPEN">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH">High</asp:ListItem>
                    <asp:ListItem Value="LOW">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonVWAPrice" runat="server" Text="Show VWAP" OnClick="buttonVWAPrice_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label14" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlVWAP_Interval" runat="server" TabIndex="18">
                    <asp:ListItem Value="1m">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Selected="True">5 min</asp:ListItem>
                    <asp:ListItem Value="15m">15 min</asp:ListItem>
                    <asp:ListItem Value="30m">30 min</asp:ListItem>
                    <asp:ListItem Value="60m">60 min</asp:ListItem>
                    <asp:ListItem Value="1d">Daily</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonRSI" Text="Show RSI" runat="server" OnClick="buttonRSI_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label15" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlRSI_Interval" runat="server" TabIndex="19">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label16" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxRSI_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="20"></asp:TextBox><br />
                <asp:Label ID="Label17" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlRSI_Series" runat="server" TabIndex="21">
                    <asp:ListItem Value="OPEN">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH">High</asp:ListItem>
                    <asp:ListItem Value="LOW">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonSTOCH" Text="Show Stochastics" runat="server" OnClick="buttonSTOCH_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label18" runat="server" Text="Interval: "></asp:Label>
                <asp:DropDownList ID="ddlSTOCH_Interval" runat="server" TabIndex="22">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label19" runat="server" Text="FastK Period: "></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Fastkperiod" runat="server" TextMode="Number" Width="40" Text="5" TabIndex="23"></asp:TextBox><br />
                <asp:Label ID="Label21" runat="server" Text="SlowD Period: "></asp:Label>
                <asp:TextBox ID="textboxSTOCH_Slowdperiod" runat="server" TextMode="Number" Width="40" Text="3" TabIndex="25"></asp:TextBox><br />
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonMACD" Text="Show MACD" runat="server" OnClick="buttonMACD_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label24" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlMACD_Interval" runat="server" TabIndex="28">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label25" runat="server" Text="Series Type:"></asp:Label>
                <asp:DropDownList ID="ddlMACD_Series" runat="server" TabIndex="29">
                    <asp:ListItem Value="OPEN">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH">High</asp:ListItem>
                    <asp:ListItem Value="LOW">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label26" runat="server" Text="Fast Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_FastPeriod" runat="server" TextMode="Number" Width="40" Text="12" TabIndex="30"></asp:TextBox><br />
                <asp:Label ID="Label27" runat="server" Text="Slow Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_SlowPeriod" runat="server" TextMode="Number" Width="40" Text="26" TabIndex="31"></asp:TextBox><br />
                <asp:Label ID="Label28" runat="server" Text="Signal Period:"></asp:Label>
                <asp:TextBox ID="textboxMACD_SignalPeriod" runat="server" TextMode="Number" Width="40" Text="9" TabIndex="32"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonAroon" Text="Show AROON" runat="server" OnClick="buttonAroon_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label29" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlAroon_Interval" runat="server" TabIndex="33">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label30" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxAroon_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="34"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-right-color: black; border-right-style: solid; border-right-width: 1px;">
                <asp:Button ID="buttonAdx" Text="Show ADX" runat="server" OnClick="buttonAdx_Click" />
            </td>

            <td style="width: 80%; padding-left: 1%;">
                <asp:Label ID="Label31" runat="server" Text="Interval:"></asp:Label>
                <asp:DropDownList ID="ddlAdx_Interval" runat="server" TabIndex="35">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label32" runat="server" Text="Period:"></asp:Label>
                <asp:TextBox ID="textboxAdx_Period" runat="server" TextMode="Number" Width="40" Text="14" TabIndex="36"></asp:TextBox>
            </td>
        </tr>
        <tr style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
            <td style="width: 20%; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                <asp:Button ID="buttonBBands" Text="Show Bollinger Bands" runat="server" OnClick="buttonBBands_Click" Font-Size="Small" />
            </td>

            <td style="width: 80%; padding-left: 1%; border-color: black; border-style: solid; border-width: 1px;">
                <asp:Label ID="Label33" runat="server" Text="Interval: "></asp:Label>
                <asp:DropDownList ID="ddlBBands_Interval" runat="server" TabIndex="37">
                    <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                    <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                    <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                    <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                    <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                    <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                    <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                    <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label34" runat="server" Text="Period: "></asp:Label>
                <asp:TextBox ID="textboxBBands_Period" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="38"></asp:TextBox><br />
                <asp:Label ID="Label35" runat="server" Text="Series Type: "></asp:Label>
                <asp:DropDownList ID="ddlBBands_Series" runat="server" TabIndex="39">
                    <asp:ListItem Value="OPEN">Open</asp:ListItem>
                    <asp:ListItem Value="HIGH">High</asp:ListItem>
                    <asp:ListItem Value="LOW">Low</asp:ListItem>
                    <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label36" runat="server" Text="Std Deviation: " ></asp:Label>
                <asp:TextBox ID="textboxBBands_StdDev" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="40"></asp:TextBox><br />
            </td>
        </tr>
    </table>
</asp:Content>
