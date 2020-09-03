<%@ Page Title="Gauge Trends - Bollinger Band Vs Daily" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="bbandsdaily.aspx.cs" Inherits="Analytics.bbandsdaily" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartBBandsDaily" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartBBandsDaily_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendBBandsDaily" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="Open"
                XValueMember="Date" XValueType="Date" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="High"
                XValueMember="Date" XValueType="Date" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="Low"
                XValueMember="Date" XValueType="Date" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="Close"
                XValueMember="Date" XValueType="Date" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaBBandsDaily1"
                XValueMember="Date" XValueType="Date" YValueMembers="Open,High,Low,Close" YValueType="Double"
                Legend="legendBBandsDaily" LegendText="OHLC" PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
                <%--LegendPostBackValue="OHLC"--%>
            </asp:Series>
            <asp:Series Name="LowerBand" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="Lower Band"
                XValueMember="Date" XValueType="Date" YValueMembers="Real Lower Band" YValueType="Double"
                PostBackValue="LowerBand,#VALX,#VALY" ToolTip="Date:#VALX; Lower Band:#VALY" LegendToolTip="Lower Band">
                <%--LegendPostBackValue="EMA12"--%>
            </asp:Series>
            <asp:Series Name="MiddleBand" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="Middle Band"
                XValueMember="Date" XValueType="Date" YValueMembers="Real Middle Band" YValueType="Double"
                PostBackValue="MiddleBand,#VALX,#VALY" ToolTip="Date:#VALX; Middle Band:#VALY" LegendToolTip="Middle Band">
                <%--LegendPostBackValue="EMA26"--%>
            </asp:Series>
            <asp:Series Name="UpperBand" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaBBandsDaily1"
                Legend="legendBBandsDaily" LegendText="Upper Band"
                XValueMember="Date" XValueType="Date" YValueMembers="Real Upper Band" YValueType="Double"
                PostBackValue="UpperBand,#VALX,#VALY" ToolTip="Date:#VALX; Upper Band:#VALY" LegendToolTip="Upper Band">
                <%--LegendPostBackValue="EMA26"--%>
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaBBandsDaily1">
                <Position Auto="false" X="0" Y="3" Height="97" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="LabelsAngleStep90">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Daily Price(OHLC)" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY2 Title="Bollinger Band Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisY2>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%; font-size: small;text-align:center;">
            <tr>
                <td style="width: 60%; text-align:center;">
                    <asp:GridView ID="GridViewDaily" Visible="False" runat="server" Width="100%" AutoGenerateColumns="False"
                        AllowPaging="True" Caption="Daily Data" CaptionAlign="Top" OnPageIndexChanging="GridViewDaily_PageIndexChanging" 
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" >
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Open" DataField="Open" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="High" DataField="High" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Low" DataField="Low" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Close" DataField="Close" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Volume" DataField="Volume" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>

                <td style="width: 40%;">
                    <asp:GridView ID="GridViewBBands" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Left" AllowPaging="True" Caption="Bollinger Bands" CaptionAlign="Top" 
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True"
                        OnPageIndexChanging="GridViewBBands_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Lower Band" DataField="Real Lower Band" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Middle Band" DataField="Real Middle Band" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Upper Band" DataField="Real Upper Band" ItemStyle-HorizontalAlign="Center">
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
