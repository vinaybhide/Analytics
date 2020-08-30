<%@ Page Title="STOCH" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="stoch.aspx.cs" Inherits="Analytics.stoch" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--<li>A stochastic oscillator is a momentum indicator comparing a particular closing price of a security to a range of its 
                                prices over a certain period of time.</li>
                            <li>It is used to generate overbought and oversold trading signals, utilizing a 0-100 bounded range of values.</li>
                            <li>Traditionally, readings over 80 are considered in the overbought range, and readings under 20 are considered oversold. </li>
                            <li>Stochastic oscillator charting generally consists of two lines: one reflecting the actual value of the oscillator for 
                                each session, and one reflecting its three-day simple moving average. Because price is thought to follow momentum, 
                                intersection of these two lines is considered to be a signal that a reversal may be in the works, as it indicates a 
                                large shift in momentum from day to day.</li>--%>
    <asp:Chart ID="chartSTOCH" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartSTOCH_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleSTOCH" Text="Stochastic Oscillator" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <Legends>
            <asp:Legend Name="legendSTOCH" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesSlowK" Legend="legendSTOCH" LegendText="SlowK" ChartType="Line" ChartArea="chartareaSlowK"
                XValueMember="Date" XValueType="Date" YValueMembers="SlowK" YValueType="Double"
                PostBackValue="0,SlowK,#VALX,#VALY" ToolTip="Date:#VALX; SlowK:#VALY" LegendToolTip="SlowK line">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="seriesSlowD" Legend="legendSTOCH" LegendText="SlowD" ChartType="Line" ChartArea="chartareaSlowD"
                XValueMember="Date" XValueType="Date" YValueMembers="SlowD" YValueType="Double"
                PostBackValue="1,SlowD,#VALX,#VALY" ToolTip="Date:#VALX; SlowD:#VALY" LegendToolTip="SlowD line">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaSlowK" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="3" Height="45" Width="100" />
                <AxisX>
                    <LabelStyle Enabled="false" />
                </AxisX>
                <AxisY Title="SlowK" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartareaSlowD" AlignWithChartArea="chartareaSlowK" AlignmentOrientation="Vertical"
                AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="48" Height="52" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="SlowD" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
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
                <asp:BoundField HeaderText="SlowK" DataField="SlowK" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SlowD" DataField="SlowD" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
</asp:Content>
