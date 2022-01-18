<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="trendidentifier.aspx.cs" Inherits="Analytics.advGraphs.trendidentifier" %>

<%@ MasterType VirtualPath="~/advGraphs/advancegraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartAdvGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartAdvGraph_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseHttpHandler"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendAdvGraph" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Open"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="OPEN" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="High"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Low"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="LOW" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Close"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH,LOW,OPEN,CLOSE" YValueType="Double"
                Legend="legendAdvGraph" LegendText="OHLC" PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; High:#VALY1; Low:#VALY2; Open:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
            </asp:Series>
            <asp:Series Name="EMA Small" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="EMA 12"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="EMA_SMALL" YValueType="Double"
                PostBackValue="EMA12,#VALX,#VALY" ToolTip="Date:#VALX; EMA12:#VALY" LegendToolTip="EMA 12">
            </asp:Series>
            <asp:Series Name="EMA Long" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="EMA 26"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="EMA_LONG" YValueType="Double"
                PostBackValue="EMA26,#VALX,#VALY" ToolTip="Date:#VALX; EMA26:#VALY" LegendToolTip="EMA 26">
            </asp:Series>

            <asp:Series Name="MACD" Enabled="false" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="MACD"
                XValueMember="TIMESTAMP" XValueType="Date" YValuesPerPoint="3" YValueMembers="MACD,MACD_Signal,MACD_Hist" YValueType="Double"
                PostBackValue="MACD,#VALX,#VALY" ToolTip="Date:#VALX; MACD:#VALY" LegendToolTip="MACD">
            </asp:Series>
            <asp:Series Name="MACD Signal" Enabled="false" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="Signal Line"
                XValueMember="TIMESTAMP" XValueType="Date" YValuesPerPoint="3" YValueMembers="MACD_Signal,MACD,MACD_Hist" YValueType="Double"
                PostBackValue="MACD_Signal,#VALX,#VALY" ToolTip="Date:#VALX; Signal:#VALY" LegendToolTip="MACD Signal">
            </asp:Series>
            <asp:Series Name="MACD Histogram" Enabled="false" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="MACD Histogram"
                XValueMember="TIMESTAMP" XValueType="Date" YValuesPerPoint="3" YValueMembers="MACD_Hist,MACD,MACD_Signal" YValueType="Double"
                PostBackValue="MACD_Hist,#VALX,#VALY" ToolTip="Date:#VALX; History:#VALY" LegendToolTip="MACD Histogram">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX Enabled="false">
                    <LabelStyle Enabled="false" />
                </AxisX>
                <AxisY Title="Daily-Open/High/Low/close, EMA" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisX2>
                <AxisY2 Title="EMA12/EMA26 Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY2>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisX>
                <AxisY Title="MACD/Signal/Histogram" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="0"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="0" TextAlignment="Near" ToolTip="0 line" />
                    </StripLines>
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%; font-size: small; text-align: center;">
            <tr>
                <td style="width: 100%; text-align: center;">
                    <asp:GridView ID="GridViewData" Enabled="false" Visible="false" runat="server" Width="100%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center"
                        AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="TIMESTAMP" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Open" DataField="OPEN" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="High" DataField="HIGH" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Low" DataField="LOW" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Close" DataField="CLOSE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Volume" DataField="VOLUME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SMA Small" DataField="SMA_SMALL" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SMA Long" DataField="SMA_LONG" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="EMA SMALL" DataField="EMA_SMALL" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="EMA LONG" DataField="EMA_LONG" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="MACD" DataField="MACD" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="MACD_Hist" DataField="MACD_Hist" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="MACD_Signal" DataField="MACD_Signal" ItemStyle-HorizontalAlign="Center">
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
