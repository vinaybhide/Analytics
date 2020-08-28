<%@ Page Title="About" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="intraday.aspx.cs" Inherits="Analytics.intraday" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">

    <%--<li>A daily chart is a graph of data points, where each point represents the security's price action for a specific day of trading.
                            </li>
                            <li>Daily charts are one of the main tools used by technical traders seeking to profit from intraday price movements and 
                                longer-term trends. A daily chart may focus on the price action of a security for a single day or it can also, comprehensively, 
                                show the daily price movements of a security over a specified time frame.
                            </li>
                            <li>Candlestick charts are popular mainly due to the ease with which they convey the basic information, such as the opening and closing price, 
                                as well as the trading range for the selected period of time. --%>
    <asp:Chart ID="chartintraGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation" EnableViewState="True" OnClick="chartintraGraph_Click">
        <Legends>
            <asp:Legend Name="legendIntra" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaIntra" Legend="legendIntra"
                LegendText="Open"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Open:#VALY" LegendToolTip="Intra-day Open Price Line">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaIntra" Legend="legendIntra"
                LegendText="High"
                XValueMember="Date" XValueType="DateTime" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; High:#VALY" LegendToolTip="Intra-day High Price Line">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaIntra" Legend="legendIntra"
                LegendText="Low"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Low:#VALY" LegendToolTip="Intra-day Low Price Line">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaIntra" Legend="legendIntra"
                LegendText="Close"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Close:#VALY" LegendToolTip="Intra-day Close Price Line">
            </asp:Series>
            <asp:Series Name="OHLC" XAxisType="Primary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaIntra" Legend="legendIntra"
                LegendText="OHLC"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Open,High,Low,Close" YValueType="Double"
                BorderColor="Black" Color="Black"
                PostBackValue="OHLC,#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4" ToolTip="Date:#VALX{g}; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="Intra-day OHlC">
            </asp:Series>
            <asp:Series Name="Volume" XAxisType="Primary" YAxisType="Secondary" ChartType="Column" ChartArea="chartareaIntra" Legend="legendIntra"
                LegendText="Volume"
                XValueMember="Date" XValueType="Date" YValueMembers="Volume" YValueType="Auto"
                PostBackValue="Volume,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Volume:#VALY" LegendToolTip="Intra-day Volume">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaIntra">
                <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Format="g" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Open/High/Low/Close Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
                <AxisY2 Title="Volume" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY2>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center" OnPageIndexChanging="GridViewDaily_PageIndexChanging" AllowPaging="True">
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
    </div>
</asp:Content>
