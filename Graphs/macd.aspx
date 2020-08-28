﻿<%@ Page Title="About" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="macd.aspx.cs" Inherits="Analytics.macd" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">


    <%--<li>Moving Average Convergence Divergence (MACD) is a trend-following momentum indicator that shows the relationship between 
                                two moving averages of a security’s price. The MACD is calculated by subtracting the 26-period Exponential Moving Average 
                                (EMA) from the 12-period EMA.</li>
                            <li>MACD triggers technical signals when it crosses above (to buy) or below (to sell) its signal line.</li>
                            <li>The speed of crossovers is also taken as a signal of a market is overbought or oversold.--%>
    <asp:Chart ID="chartMACD" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartMACD_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleMACD" Text="Moving Average Convergence Divergence" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <Legends>
            <asp:Legend Name="legendMACD" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesMACD" XAxisType="Secondary" YAxisType="Primary" Legend="legendMACD" LegendText="MACD" ChartType="Line" ChartArea="chartareaMACD"
                XValueMember="Date" XValueType="Date" YValueMembers="MACD" YValueType="Double"
                PostBackValue="MACD,#VALX,#VALY" ToolTip="Date:#VALX; MACD:#VALY" LegendToolTip="MACD Line">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="seriesMACD_Signal" XAxisType="Secondary" YAxisType="Primary" Legend="legendMACD" LegendText="MACD Signal" ChartType="Line" ChartArea="chartareaMACD"
                XValueMember="Date" XValueType="Date" YValueMembers="MACD_Signal" YValueType="Double"
                PostBackValue="MACD_Signal,#VALX,#VALY" ToolTip="Date:#VALX; MACD_Signal:#VALY" LegendToolTip="Signal Line">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="seriesMACD_Hist" XAxisType="Primary" YAxisType="Secondary" Legend="legendMACD" LegendText="MACD Historical"
                ChartType="Column" ChartArea="chartareaMACD"
                XValueMember="Date" XValueType="Date" YValueMembers="MACD_Hist" YValueType="Double"
                PostBackValue="MACD_Hist,#VALX,#VALY" ToolTip="Date:#VALX; MACD_Hist:#VALY" LegendToolTip="MACD Histogram">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaMACD" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="MACD & Signal" IsStartedFromZero="true" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX2>

                <AxisY2 Title="MACD History" IsStartedFromZero="true" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY2>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
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
    </div>
</asp:Content>
