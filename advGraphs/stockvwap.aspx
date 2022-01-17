<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="stockvwap.aspx.cs" Inherits="Analytics.advGraphs.stockvwap" %>
<%@ MasterType VirtualPath="~/advGraphs/advancegraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartAdvGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True"
        OnClick="chartAdvGraph_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendAdvGraph" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendAdvGraph"
                LegendText="Open"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="OPEN" YValueType="Double"
                PostBackValue="Open,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Open:#VALY" LegendToolTip="Open Price Line">
            </asp:Series>
            <asp:Series Name="High" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendAdvGraph"
                LegendText="High"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="HIGH" YValueType="Double"
                PostBackValue="High,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; High:#VALY" LegendToolTip="High Price Line">
            </asp:Series>
            <asp:Series Name="Low" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendAdvGraph"
                LegendText="Low"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="LOW" YValueType="Double"
                PostBackValue="Low,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Low:#VALY" LegendToolTip="Low Price Line">
            </asp:Series>
            <asp:Series Name="Close" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendAdvGraph"
                LegendText="Close"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="Close,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Close:#VALY" LegendToolTip="Close Price Line">
            </asp:Series>
            <asp:Series Name="OHLC" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1" Legend="legendAdvGraph"
                LegendText="OHLC"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="HIGH,LOW,OPEN,CLOSE" YValueType="Double"
                BorderColor="Black" Color="Black"
                PostBackValue="OHLC,#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4" ToolTip="Date:#VALX{g}; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4"
                LegendToolTip="OHLC Candlestick"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Traingle">
            </asp:Series>
            <asp:Series Name="VWAP" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendAdvGraph" LegendText="VWAP"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="VWAP" YValueType="Double"
                PostBackValue="#VALX{g},#VALY" ToolTip="Date:#VALX{g}; VWAP:#VALY">
            </asp:Series>

            <asp:Series Name="Volume" Enabled="false" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartarea2" Legend="legendAdvGraph"
                LegendText="Volume"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="VOLUME" YValueType="Auto"
                PostBackValue="Volume,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Volume:#VALY" LegendToolTip="Daily Volume">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical">
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Format="g" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY Title="Open/High/Low/Close Vs VWAP" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Format="g" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Volume" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
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
                            <asp:BoundField HeaderText="VWAP" DataField="VWAP" ItemStyle-HorizontalAlign="Center">
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
