﻿<%@ Page Title="About" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="dmi.aspx.cs" Inherits="Analytics.dmi" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">

    <%--<asp:CheckBox ID="checkBoxOpen" runat="server" Text="Open" AutoPostBack="True" TabIndex="5" />
                        <asp:CheckBox ID="checkBoxHigh" runat="server" Text="High" AutoPostBack="True" TabIndex="6" />
                        <asp:CheckBox ID="checkBoxLow" runat="server" Text="Low" AutoPostBack="True" TabIndex="7" />
                        <asp:CheckBox ID="checkBoxClose" runat="server" Text="Close" AutoPostBack="True" TabIndex="8" />
                        <asp:CheckBox ID="checkBoxCandle" runat="server" Checked="true" Text="Candlestick" AutoPostBack="True" TabIndex="9" />
                        <asp:CheckBox ID="checkBoxMINUS_DM" runat="server" Checked="true" Text="-ve Directinal Movement Indicator(-DMI)" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxPLUS_DM" runat="server" Checked="true" Text="+ve Directinal Movement Indicator(+DMI)" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxGrid" runat="server" Text="Raw data" AutoPostBack="True" TabIndex="10" />--%>
    <%--The directional movement indicator (also known as the directional movement index or DMI) is a valuable tool for assessing 
                                price direction and strength.</li>
                            <li>The DMI is especially useful for trend trading strategies because it differentiates between strong and weak trends, 
                                allowing the trader to enter only the ones with real momentum. </li>
                            <li>DMI tells you when to be long or short.</li>
                            <li>DMI comprises of two lines, +DMI & -DMI. The line which is on top is referred as dominant DMI. The dominant DMI is stronger 
                                and more likely to predict the direction of price. For the buyers and sellers to change dominance, 
                                the lines must cross over.</li>

                            <li>The +DMI generally moves in sync with price, which means the +DMI rises when price rises, and it falls when price falls. 
                                It is important to note that the -DMI behaves in the opposite manner and moves counter-directional to price. 
                                The -DMI rises when price falls, and it falls when price rises. </li>
                            <li>TReading directional signals is easy. 
                                <ul>
                                    <li>When the +DMI is dominant and rising, price direction is up. </li>
                                    <li>When the -DMI is dominant and rising, price direction is down. </li>
                                    <li>But the strength of price must also be considered. DMI strength ranges from a low of 0 to a high of 100. 
                                        The higher the DMI value, the stronger the prices swing. </li>
                                    <li>DMI values over 25 mean price is directionally strong. DMI values under 25 mean price is directionally weak.</li>
                                </ul>
                            </li>
                            <li>When the buyers are stronger than the sellers, the +DMI peaks will be above 25 and the -DMI peaks will be below 25. 
                                This is seen in a strong uptrend. But when the sellers are stronger than the buyers, the -DMI peaks will be above 25 and 
                                the +DMI peaks will be below 25. In this case, the trend will be down.</li>--%>
    <asp:Chart ID="chartDMIDaily" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartDMIDaily_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <Legends>
            <asp:Legend Name="legendDMIDaily" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                Legend="legendDMIDaily" LegendText="Open" XValueMember="Date" XValueType="Date" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                Legend="legendDMIDaily" LegendText="High" XValueMember="Date" XValueType="Date" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                Legend="legendDMIDaily" LegendText="Low" XValueMember="Date" XValueType="Date" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                Legend="legendDMIDaily" LegendText="Close" XValueMember="Date" XValueType="Date" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaDMIDaily1"
                Legend="legendDMIDaily" LegendText="OHLC"
                XValueMember="Date" XValueType="Date" YValueMembers="Open,High,Low,Close" YValueType="Double"
                PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
            </asp:Series>
            <asp:Series Name="MINUS_DM" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily2"
                Legend="legendDMIDaily" LegendText="-DMI"
                XValueMember="Date" XValueType="Date" YValueMembers="MINUS_DM" YValueType="Double"
                PostBackValue="MINUS_DM,#VALX,#VALY" ToolTip="Date:#VALX; MINUS_DM:#VALY" LegendToolTip="-DMI">
            </asp:Series>
            <asp:Series Name="PLUS_DM" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily2"
                Legend="legendDMIDaily" LegendText="+DMI"
                XValueMember="Date" XValueType="Date" YValueMembers="PLUS_DM" YValueType="Double"
                PostBackValue="PLUS_DM,#VALX,#VALY" ToolTip="Date:#VALX; PLUS_DM:#VALY" LegendToolTip="+DMI">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaDMIDaily1">
                <Position Auto="false" X="0" Y="3" Height="50" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Daily Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartareaDMIDaily2" AlignWithChartArea="chartareaDMIDaily1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="53" Height="47" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="DMI" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="25"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="25 Level" TextAlignment="Near" />
                    </StripLines>
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%">
            <tr>
                <td style="width: 40%;">
                    <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center"
                        AllowPaging="True" OnPageIndexChanging="GridViewDaily_PageIndexChanging" Caption="Daily Price Data" CaptionAlign="Top">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Open" DataField="Open" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="High" DataField="High" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Low" DataField="Low" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Close" DataField="Close" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Volume" DataField="Volume" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
                <td style="width: 30%;">
                    <asp:GridView ID="GridViewMINUSDM" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewMINUSDM_PageIndexChanging" Caption="-DMI" CaptionAlign="Top">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Negative DMI" DataField="MINUS_DM" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
                <td style="width: 30%;">
                    <asp:GridView ID="GridViewPLUSDM" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewPLUSDM_PageIndexChanging" Caption="+DMI" CaptionAlign="Top">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Positive DMI" DataField="PLUS_DM" ItemStyle-HorizontalAlign="Center">
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
