<%@ Page Title="About" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="aroon.aspx.cs" Inherits="Analytics.aroon" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--<ul>
                            <li>The Aroon indicator is a technical indicator that is used to identify trend changes in the price of an asset, 
                                as well as the strength of that trend.</li>
                            <li>In essence, the indicator measures the time between highs and the time between lows over a time period. 
                                The idea is that strong uptrends will regularly see new highs, and strong downtrends will regularly see new lows. 
                                The indicator signals when this is happening, and when it isn't.
                            </li>
                            <li>The indicator consists of the "Aroon up" line, which measures the strength of the uptrend, and the "Aroon down" line, 
                                which measures the strength of the downtrend.
                            <ul>
                                <li>The Arron indicator is composed of two lines. An up line which measures the number of periods since a High, 
                                    and a down line which measures the number of periods since a Low.</li>
                                <li>When the Aroon Up is above the Aroon Down, it indicates bullish price behavior.</li>
                                <li>When the Aroon Down is above the Aroon Up, it signals bearish price behavior.</li>
                                <li>Crossovers of the two lines can signal trend changes. For example, when Aroon Up crosses above Aroon Down it 
                                    may mean a new uptrend is starting.</li>
                                <li>The indicator moves between zero and 100. A reading above 50 means that a high/low (whichever line is above 50) 
                                    was seen within the last 12 periods.</li>
                                <li>A reading below 50 means that the high/low was seen within the 13 periods.</li>
                            </ul>--%>
    <asp:Chart ID="chartAROON" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True"
        OnClick="chartAROON_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleAROON" Text="AROON" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <Legends>
            <asp:Legend Name="legendAROON" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesAROON_Down" Legend="legendAROON" LegendText="AROON Down" ChartType="Line" ChartArea="chartareaAROON"
                XValueMember="Date" XValueType="Date" YValueMembers="Aroon Down" YValueType="Double"
                PostBackValue="AROON Down,#VALX,#VALY" ToolTip="Date:#VALX; AROON Down:#VALY" LegendToolTip="ARRON Down">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="seriesAROON_Up" Legend="legendAROON" LegendText="AROON Up" ChartType="Line" ChartArea="chartareaAROON"
                XValueMember="Date" XValueType="Date" YValueMembers="Aroon Up" YValueType="Double"
                PostBackValue="AROON Up,#VALX,#VALY" ToolTip="Date:#VALX; AROON Up:#VALY" LegendToolTip="ARRON Up">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaAROON" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="AROON" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
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
                <asp:BoundField HeaderText="Aroon Down" DataField="Aroon Down" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Aroon Up" DataField="Aroon Up" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" />
        </asp:GridView>
    </div>
</asp:Content>
