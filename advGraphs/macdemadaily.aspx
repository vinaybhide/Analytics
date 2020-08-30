﻿<%@ Page Title="Trend Reversal Indicator" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="macdemadaily.aspx.cs" Inherits="Analytics.macdemadaily" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--<asp:CheckBox ID="checkBoxEMA12" runat="server" Checked="true" Text="EMA12" AutoPostBack="True" TabIndex="4"/>
                        <asp:CheckBox ID="checkBoxEMA26" runat="server" Checked="true" Text="EMA26" AutoPostBack="True" TabIndex="5"/>
                        <asp:CheckBox ID="checkBoxCandle" runat="server" Checked="true" Text="Candlestick" AutoPostBack="True" TabIndex="6"/>
                        <asp:CheckBox ID="checkBoxMACD" runat="server" Checked="true" Text="MACD" AutoPostBack="True" TabIndex="7"/>
                        <asp:CheckBox ID="checkBoxMACD_Signal" runat="server" Checked="true" Text="MACD Signal" AutoPostBack="True" TabIndex="8"/>
                        <asp:CheckBox ID="checkBoxMACD_Hist" runat="server" Checked="true" Text="MACD History" AutoPostBack="True" TabIndex="9"/>
                        <asp:CheckBox ID="checkBoxOpen" runat="server" Text="Open" AutoPostBack="True" TabIndex="10"/>
                        <asp:CheckBox ID="checkBoxHigh" runat="server" Text="High" AutoPostBack="True" TabIndex="11"/>
                        <asp:CheckBox ID="checkBoxLow" runat="server" Text="Low" AutoPostBack="True" TabIndex="12"/>
                        <asp:CheckBox ID="checkBoxClose" runat="server" Text="Close" AutoPostBack="True" TabIndex="13"/>
                        <asp:CheckBox ID="checkBoxVolume" Visible="false" runat="server" Text="Volume" AutoPostBack="True" TabIndex="14"/>
                        <asp:CheckBox ID="checkBoxGrid" runat="server" Text="Raw data" AutoPostBack="True" TabIndex="15"/>--%>
    <%--MACD is a trend-following momentum indicator that shows the relationship between two moving averages of a security’s price. 
                                The MACD is calculated by subtracting the 26-period Exponential Moving Average (EMA) from the 12-period EMA.
                            </li>
                            <li>You may buy the security when the MACD crosses above its signal line and sell - or short - the security when the MACD crosses below the signal line.
                            </li>
                            <li>The MACD generates a bullish signal when it moves above its own signal line, and it sends a sell sign when it moves 
                                below its signal line.
                            </li>
                            <li>The histogram is positive when the MACD is above its signal line and negative when the MACD is below its signal line. 
                            </li>
                            <li>If prices are rising, the histogram grows larger as the speed of the price movement accelerates, 
                                and contracts as price movement decelerates. --%>
    <asp:Chart ID="chartMACDEMADaily" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartMACDEMADaily_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <Legends>
            <asp:Legend Name="legendMACDEMADaily" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaMACDEMADaily1"
                Legend="legendMACDEMADaily" LegendText="Open"
                XValueMember="Date" XValueType="Date" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaMACDEMADaily1"
                Legend="legendMACDEMADaily" LegendText="High"
                XValueMember="Date" XValueType="Date" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaMACDEMADaily1"
                Legend="legendMACDEMADaily" LegendText="Low"
                XValueMember="Date" XValueType="Date" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaMACDEMADaily1"
                Legend="legendMACDEMADaily" LegendText="Close"
                XValueMember="Date" XValueType="Date" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaMACDEMADaily1"
                XValueMember="Date" XValueType="Date" YValueMembers="Open,High,Low,Close" YValueType="Double"
                Legend="legendMACDEMADaily" LegendText="OHLC" PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
                <%--LegendPostBackValue="OHLC"--%>
            </asp:Series>
            <asp:Series Name="EMA12" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaMACDEMADaily1"
                Legend="legendMACDEMADaily" LegendText="EMA 12"
                XValueMember="Date" XValueType="Date" YValueMembers="EMA" YValueType="Double"
                PostBackValue="EMA12,#VALX,#VALY" ToolTip="Date:#VALX; EMA12:#VALY" LegendToolTip="EMA 12">
                <%--LegendPostBackValue="EMA12"--%>
            </asp:Series>
            <asp:Series Name="EMA26" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaMACDEMADaily1"
                Legend="legendMACDEMADaily" LegendText="EMA 26"
                XValueMember="Date" XValueType="Date" YValueMembers="EMA" YValueType="Double"
                PostBackValue="EMA26,#VALX,#VALY" ToolTip="Date:#VALX; EMA26:#VALY" LegendToolTip="EMA 26">
                <%--LegendPostBackValue="EMA26"--%>
            </asp:Series>

            <asp:Series Name="MACD" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaMACDEMADaily2"
                Legend="legendMACDEMADaily" LegendText="MACD"
                XValueMember="Date" XValueType="Date" YValueMembers="MACD" YValueType="Double"
                PostBackValue="MACD,#VALX,#VALY" ToolTip="Date:#VALX; MACD:#VALY" LegendToolTip="MACD">
                <%--LegendPostBackValue="MACD"--%>
            </asp:Series>
            <asp:Series Name="MACD_Signal" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaMACDEMADaily2"
                Legend="legendMACDEMADaily" LegendText="Signal Line"
                XValueMember="Date" XValueType="Date" YValueMembers="MACD_Signal" YValueType="Double"
                PostBackValue="MACD_Signal,#VALX,#VALY" ToolTip="Date:#VALX; Signal:#VALY" LegendToolTip="MACD Signal">
                <%--LegendPostBackValue="MACD_Signal"--%>
            </asp:Series>
            <asp:Series Name="MACD_Hist" XAxisType="Secondary" YAxisType="Secondary" ChartType="Column" ChartArea="chartareaMACDEMADaily2"
                Legend="legendMACDEMADaily" LegendText="MACD Histogram"
                XValueMember="Date" XValueType="Date" YValueMembers="MACD_Hist" YValueType="Double"
                PostBackValue="MACD_Hist,#VALX,#VALY" ToolTip="Date:#VALX; History:#VALY" LegendToolTip="MACD Histogram">
                <%--LegendPostBackValue="MACD_Hist"--%>
            </asp:Series>
            <%--MarkerSize="8" MarkerStep="10" MarkerStyle="Cross"--%>
            <%--<asp:Series Name="Volume" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartareaMACDEMADaily3"
                            Legend="legendMACDEMADaily" LegendText="Volume"
                            XValueMember="Date" XValueType="Date" YValueMembers="Volume" YValueType="Auto"
                            PostBackValue="Volume,#VALX,#VALY" ToolTip="Date:#VALX; Volume:#VALY">
                        </asp:Series>--%>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaMACDEMADaily1">
                <Position Auto="false" X="0" Y="3" Height="50" Width="95" />
                <AxisX Enabled="false">
                    <LabelStyle Enabled="false" />
                </AxisX>
                <AxisY Title="Daily Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisX2>
                <AxisY2 Title="EMA12/EMA26 Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY2>
            </asp:ChartArea>
            <asp:ChartArea Name="chartareaMACDEMADaily2" AlignWithChartArea="chartareaMACDEMADaily1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="53" Height="47" Width="95" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisX>
                <AxisY Title="MACD/Signal" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
                <AxisX2 Enabled="False">
                    <LabelStyle Enabled="false" />
                </AxisX2>
                <AxisY2 Title="History" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY2>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%; font-size: small;">
            <tr>
                <td style="width: 40%;">
                    <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Left" AllowPaging="True" Caption="Daily Data" CaptionAlign="Top" OnPageIndexChanging="GridViewDaily_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Open" DataField="Open" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="High" DataField="High" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Low" DataField="Low" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Close" DataField="Close" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Volume" DataField="Volume" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
                <td style="width: 15%;">
                    <asp:GridView ID="GridViewEMA12" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Left" AllowPaging="True" Caption="EMA12" CaptionAlign="Top" OnPageIndexChanging="GridViewEMA12_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="EMA12" DataField="EMA" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>

                </td>
                <td style="width: 15%;">
                    <asp:GridView ID="GridViewEMA26" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Left" AllowPaging="True" Caption="EMA26" CaptionAlign="Top" OnPageIndexChanging="GridViewEMA26_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="EMA26" DataField="EMA" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
                <td style="width: 30%;">
                    <asp:GridView ID="GridViewMACD" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Left" AllowPaging="True" Caption="MACD" CaptionAlign="Top" OnPageIndexChanging="GridViewMACD_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="MACD" DataField="MACD" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Signal" DataField="MACD_Signal" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="History" DataField="MACD_Hist" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
