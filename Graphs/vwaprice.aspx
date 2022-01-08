<%@ Page Title="VWAP" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="vwaprice.aspx.cs" Inherits="Analytics.vwaprice" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Panel ID="panelParam" runat="server" Visible="true">
        <div style="width: 100%; border: groove;">
            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label1" runat="server" Text="Output size: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Outputsize" runat="server" TabIndex="26">
                            <asp:ListItem Value="Full" >Full</asp:ListItem>
                            <asp:ListItem Value="Compact" Selected="True">Compact</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label2" runat="server" Text="Interval: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Interval" runat="server">
                            <asp:ListItem Value="1d">Daily</asp:ListItem>
                            <asp:ListItem Value="1m">1 min</asp:ListItem>
                            <asp:ListItem Value="5m" Selected="True">5 min</asp:ListItem>
                            <asp:ListItem Value="15m">15 min</asp:ListItem>
                            <asp:ListItem Value="30m">30 min</asp:ListItem>
                            <asp:ListItem Value="60m">60 min</asp:ListItem>
                            <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                            <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label6" runat="server" Text="Series Type: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_SeriesType" runat="server" TabIndex="29">
                            <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                            <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                            <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                            <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>

        </div>
    </asp:Panel>

    <asp:Chart ID="chartStdGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True"
        OnClick="chartStdGraph_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendStdGraph" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="Open"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="OPEN" YValueType="Double"
                PostBackValue="Open,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Open:#VALY" LegendToolTip="Open Price Line">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="High"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="HIGH" YValueType="Double"
                PostBackValue="High,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; High:#VALY" LegendToolTip="High Price Line">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="Low"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="LOW" YValueType="Double"
                PostBackValue="Low,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Low:#VALY" LegendToolTip="Low Price Line">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="Close"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="Close,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Close:#VALY" LegendToolTip="Close Price Line">
            </asp:Series>
            <asp:Series Name="OHLC" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="OHLC"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="HIGH,LOW,OPEN,CLOSE" YValueType="Double"
                BorderColor="Black" Color="Black"
                PostBackValue="OHLC,#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4" ToolTip="Date:#VALX{g}; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4"
                LegendToolTip="OHLC Candlestick"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Traingle">
            </asp:Series>
            <asp:Series Name="Volume" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartarea2" Legend="legendStdGraph"
                LegendText="Volume"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="VOLUME" YValueType="Auto"
                PostBackValue="Volume,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Volume:#VALY" LegendToolTip="Daily Volume">
            </asp:Series>

            <asp:Series Name="VWAP" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea3" Legend="legendStdGraph" LegendText="VWAP"
                XValueMember="TIMESTAMP" XValueType="DateTime" YValueMembers="VWAP" YValueType="Double"
                PostBackValue="#VALX{g},#VALY" ToolTip="Date:#VALX{g}; VWAP:#VALY">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="3" Height="30" Width="99" />
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Format="g" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>

                <AxisY Title="Open/High/Low/Close Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="33" Height="30" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Format="g" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Volume" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="66" Height="30" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Format="g" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="VWAP" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center"
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
    </div>
</asp:Content>
