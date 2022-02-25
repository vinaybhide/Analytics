<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="globalindex.aspx.cs" Inherits="Analytics.advGraphs.globalindex" %>
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
        <%--<Series>
            <asp:Series Name="PctChange" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendAdvGraph"
                LegendText="Pct Change"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="PctChange,#VALX,#VALY" ToolTip="Date:#VALX; % Change:#VALY" LegendToolTip="Percent Change">
            </asp:Series>
            <asp:Series Name="Open" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2" Legend="legendAdvGraph"
                LegendText="Open"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="OPEN" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open Price Line">
            </asp:Series>
            <asp:Series Name="High" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2" Legend="legendAdvGraph"
                LegendText="High"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High Price Line">
            </asp:Series>
            <asp:Series Name="Low" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2" Legend="legendAdvGraph"
                LegendText="Low"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="LOW" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low Price Line">
            </asp:Series>
            <asp:Series Name="Close" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2" Legend="legendAdvGraph"
                LegendText="Close"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close Price Line">
            </asp:Series>
            <asp:Series Name="OHLC" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea2" Legend="legendAdvGraph"
                LegendText="OHLC"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH,LOW,OPEN,CLOSE" YValueType="Double"
                BorderColor="Black" Color="Black"
                PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4" ToolTip="Date:#VALX; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4"
                LegendToolTip="OHLC Candlestick"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Traingle">
            </asp:Series>

            <asp:Series Name="Volume" Enabled="false" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartarea3" Legend="legendAdvGraph"
                LegendText="Volume"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="VOLUME" YValueType="Auto"
                PostBackValue="Volume,#VALX,#VALY" ToolTip="Date:#VALX; Volume:#VALY" LegendToolTip="Daily Volume">
            </asp:Series>
        </Series>--%>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY Title="Pct Change" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##"/>
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY Title="Daily Index Value" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##"/>
                </AxisY>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
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
                    <asp:GridView ID="GridViewData" Enabled="false" Visible="False" runat="server" Width="100%" AutoGenerateColumns="true"
                        AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                        <%--<Columns>
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
                        </Columns>--%>
                        <HeaderStyle HorizontalAlign="Center" />
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>


</asp:Content>
