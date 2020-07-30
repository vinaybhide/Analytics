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
        <table style="border: solid; border-width: thin; width: 100%">
            <tr style="border-color: black; border-top-style: solid; border-width: 1px;">
                <td style="background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label38" runat="server" Text="Graph"></asp:Label></td>
                <td style="background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label5" runat="server" Text="Sub-Graphs"></asp:Label></td>
                <td style="background-color: gray; text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label39" runat="server" Text="Parameters"></asp:Label></td>
            </tr>
            <tr>
                <td rowspan="3" style="text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonVWAPIntra" Text="VWAP + Intra-day" runat="server" TabIndex="4" OnClick="buttonVWAPIntra_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label6" runat="server" Text="Intra-day"></asp:Label>
                </td>
                <td style="padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
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
                    </asp:DropDownList> <br />
                    <asp:Label ID="Label7" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlIntraday_outputsize" runat="server" TabIndex="6">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label8" runat="server" Text="VWAP"></asp:Label>
                </td>
                <td style="padding-left: 1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label14" runat="server" Text="Interval:"></asp:Label>
                    <asp:DropDownList ID="ddlVWAP_Interval" runat="server" TabIndex="7">
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
            <tr>
                <td rowspan="4" style="text-align: center; border-color: black; border-style: solid; border-width: 1px;">
                    <asp:Button ID="buttonCrossover" Text="Crossover" runat="server" TabIndex="8" OnClick="buttonCrossover_Click"/>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label10" runat="server" Text="Daily OHLC"></asp:Label>
                </td>
                <td style="padding-left:1%; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label11" runat="server" Text="Output size:"></asp:Label>
                    <asp:DropDownList ID="ddlDaily_OutputSize" runat="server" TabIndex="9">
                        <asp:ListItem Value="compact" Selected="True">Compact</asp:ListItem>
                        <asp:ListItem Value="full">Full</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label12" runat="server" Text="SMA 1"></asp:Label>
                </td>
                <td style="padding-left:1%; border: black; border-style: solid; border-width: 1px;">
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
                    </asp:DropDownList><br />
                    <asp:Label ID="Label15" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxSMA1_Period" runat="server" TextMode="Number" Text="50" TabIndex="11"></asp:TextBox><br />
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
                <td style="text-align: center; border: black; border-style: solid; border-width: 1px;">
                    <asp:Label ID="Label17" runat="server" Text="SMA 2"></asp:Label>
                </td>
                <td style="padding-left:1%; border: black; border-style: solid; border-width: 1px;">
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
                    </asp:DropDownList><br />
                    <asp:Label ID="Label19" runat="server" Text="Period:"></asp:Label>
                    <asp:TextBox ID="textboxSMA2_Period" runat="server" TextMode="Number" Text="100" TabIndex="14"></asp:TextBox><br />
                    <asp:Label ID="Label20" runat="server" Text="Series Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSMA2_Series" runat="server" TabIndex="15">
                        <asp:ListItem Value="open" Enabled="false">Open</asp:ListItem>
                        <asp:ListItem Value="high" Enabled="false">High</asp:ListItem>
                        <asp:ListItem Value="low" Enabled="false">Low</asp:ListItem>
                        <asp:ListItem Value="close" Selected="True">Close</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

        </table>
    </div>

</asp:Content>
