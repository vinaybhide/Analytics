﻿<%@ Page Title="Intra-day Indicator" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="vwap_intra.aspx.cs" Inherits="Analytics.vwap_intra" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartVWAP_Intra" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartVWAP_Intra_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendVWAP_Intra" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaVWAP_Intra" Legend="legendVWAP_Intra"
                LegendText="Open"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaVWAP_Intra" Legend="legendVWAP_Intra"
                LegendText="High"
                XValueMember="Date" XValueType="DateTime" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaVWAP_Intra" Legend="legendVWAP_Intra"
                LegendText="Low"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaVWAP_Intra" Legend="legendVWAP_Intra"
                LegendText="Close"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" XAxisType="Secondary" YAxisType="Primary"  ChartType="Candlestick" ChartArea="chartareaVWAP_Intra"
                Legend="legendVWAP_Intra" LegendText="OHLC"
                XValueMember="Date" XValueType="DateTime" YValueMembers="High,Low,Open,Close" YValueType="Double"
                PostBackValue="OHLC,#VALX{g},#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX{g}; High:#VALY1; Low:#VALY2; Open:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
                <%--LegendPostBackValue="OHLC"--%>
            </asp:Series>
            <asp:Series Name="VWAP" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaVWAP_Intra"
                Legend="legendVWAP_Intra" LegendText="VWAP"
                XValueMember="Date" XValueType="DateTime" YValueMembers="VWAP" YValueType="Double"
                PostBackValue="VWAP,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; VWAP:#VALY" LegendToolTip="VWAP">
                <%--LegendPostBackValue="VWAP"--%>
            </asp:Series>

            <asp:Series Name="Volume" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartareaVolume"
                XValueMember="Date" XValueType="DateTime" YValueMembers="Volume" YValueType="Auto"
                PostBackValue="Volume,#VALX{g},#VALY" ToolTip="Date:#VALX{g}; Volume:#VALY" LegendToolTip="Volume">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaVWAP_Intra">
                <Position Auto="false" X="0" Y="3" Height="50" Width="99" />
                <AxisX>
                    <LabelStyle Enabled="false" />
                    <%--Title="Intra-day DateTime" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 5pt">--%>
                </AxisX>
                <AxisY Title="Intra-day Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY2 Title="VWAP Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisY2>
            </asp:ChartArea>
            <asp:ChartArea Name="chartareaVolume" AlignWithChartArea="chartareaVWAP_Intra" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="53" Height="47" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Intra-day Volume" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%">
            <tr>
                <td style="width: 70%;">
                    <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center"
                        AllowPaging="True" OnPageIndexChanging="GridViewDaily_PageIndexChanging" Caption="Intra-Day Data" CaptionAlign="Top" 
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
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
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" Position="TopAndBottom" />
                    </asp:GridView>
                </td>
                <td style="width: 30%;">
                    <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging" Caption="VWAP Data" 
                        CaptionAlign="Top" PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="VWAP" DataField="VWAP" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" Position="TopAndBottom" />
                    </asp:GridView>

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
