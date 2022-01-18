<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="trenddirection.aspx.cs" Inherits="Analytics.advGraphs.trenddirection" %>

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
                <%--LegendPostBackValue="OHLC"--%>
            </asp:Series>
            <asp:Series Name="MINUS_DI" Enabled="false" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="MINUS DI"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="MINUS_DI" YValueType="Double"
                PostBackValue="MINUS_DI,#VALX,#VALY" ToolTip="Date:#VALX; MINUS_DI:#VALY" LegendToolTip="Negative Directional Movement">
            </asp:Series>
            <asp:Series Name="PLUS_DI" Enabled="false" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="PLUS DI"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="PLUS_DI" YValueType="Double"
                PostBackValue="PLUS_DI,#VALX,#VALY" ToolTip="Date:#VALX; PLUS_DI:#VALY" LegendToolTip="Positive Directional Movement">
            </asp:Series>
            <asp:Series Name="ADX" Enabled="false" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="ADX"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="ADX" YValueType="Double"
                PostBackValue="ADX,#VALX,#VALY" ToolTip="Date:#VALX; ADX:#VALY" LegendToolTip="Average Directional Movement Index">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Enabled="true" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY Title="Daily Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="AD Vs +DI Vs -DI" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Adjusted overbought level at 50" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="75"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Overbought > 75%" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="25"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Oversold < 25%" TextAlignment="Near" />
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

                    <asp:GridView ID="GridViewData" class="gridheader" Enabled="false" Visible="false" runat="server" Width="100%" Height="50%" AutoGenerateColumns="False"
                        HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                        PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging"
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
                            <asp:BoundField HeaderText="ADX" DataField="ADX" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="+DI" DataField="PLUS_DI" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="-DI" DataField="MINUS_DI" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"></PagerSettings>
                    </asp:GridView>
                </td>
            </tr>
        </table>

    </div>

</asp:Content>
