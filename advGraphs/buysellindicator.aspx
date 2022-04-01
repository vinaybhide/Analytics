<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="buysellindicator.aspx.cs" Inherits="Analytics.advGraphs.buysellindicator" %>

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
                Legend="legendAdvGraph" LegendText="Open" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="High" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Low" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Close" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="OHLC"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="High,Low,Open,Close" YValueType="Double"
                PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; High:#VALY1; Low:#VALY2; Open:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
            </asp:Series>

            <asp:Series Name="K-FastLine" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="K-FastLine"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="SlowK" YValueType="Double"
                PostBackValue="K-FastLine,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; K-FastLine:#VALY{0.##}" LegendToolTip="K-FastLine">
            </asp:Series>

            <asp:Series Name="D-SlowLine" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="D-SlowLine"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="SlowD" YValueType="Double"
                PostBackValue="D-SlowLine,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; D-SlowLine:#VALY{0.##}" LegendToolTip="D-SlowLine">
            </asp:Series>

            <asp:Series Name="RSI" Enabled="false" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea3"
                Legend="legendAdvGraph" LegendText="RSI"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="RSI_CLOSE" YValueType="Double"
                PostBackValue="RSI,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; RSI:#VALY{0.##}">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>

                <AxisY Title="OHLC Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>

                <AxisY Title="K-FastLine/D-SlowLine" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Adjusted overbought level at 50" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="80"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Overbought > 80%" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="20"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Oversold < 20%" TextAlignment="Near" />
                    </StripLines>

                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="RSI Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Adjusted overbought level at 50" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="RoyalBlue"
                            BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="30" IntervalOffsetType="Number"
                            BackColor="RosyBrown" BackSecondaryColor="Purple"
                            BackGradientStyle="LeftRight" Text="Oversold < 30%" TextAlignment="Near" TextLineAlignment="Far" />
                        <asp:StripLine StripWidth="0" BorderColor="RoyalBlue"
                            BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="70" IntervalOffsetType="Number"
                            BackColor="RosyBrown" BackSecondaryColor="Purple"
                            BackGradientStyle="LeftRight" Text="Overbought > 70%" TextAlignment="Near" TextLineAlignment="Far" />
                    </StripLines>
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##" />
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
                            <asp:BoundField HeaderText="RSI" DataField="RSI_CLOSE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SlowD Line" DataField="SlowD" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SlowK Line" DataField="SlowK" ItemStyle-HorizontalAlign="Center">
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
