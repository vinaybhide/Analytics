<%@ Page Title="SMA" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="sma.aspx.cs" Inherits="Analytics.sma" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--<li>A simple moving average (SMA) calculates the average of a selected range of prices, usually closing prices, by the 
                                number of periods in that range.</li>
                            <li>The SMA is a technical indicator that can aid in determining if an asset price will continue or reverse a 
                                bull or bear trend.</li>
                            <li>A simple moving average smooths out volatility, and makes it easier to view the price trend of a security. </li>
                            <li>If the simple moving average points up, this means that the security's price is increasing. If it is pointing down 
                                it means that the security's price is decreasing. </li>
                            <li>The longer the time frame for the moving average, the smoother the simple moving average. A shorter-term moving average is 
                                more volatile, but its reading is closer to the source data.
                            </li>
                            <li>Two popular trading patterns that use simple moving averages include the death cross and a golden cross. 
                                <ul>
                                    <li>A death cross occurs when the 50-day SMA crosses below the 200-day SMA. This is considered a bearish signal, that further losses are in store. </li>
                                    <li>The golden cross occurs when a short-term SMA breaks above a long-term SMA. Reinforced by high trading volumes, this can signal further gains 
                                     are in store.</li>--%>
    <asp:Chart ID="chartSMA" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartSMA_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleSMA" Text="Simple Moving Average" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <%--<Legends>
                        <asp:Legend Name="legendSMA" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" BorderDashStyle="Dash" BorderColor="Black"></asp:Legend>
                    </Legends>--%>
        <Series>
            <asp:Series Name="seriesSMA" ChartType="Line" ChartArea="chartareaSMA"
                XValueMember="Date" XValueType="Date" YValueMembers="SMA" YValueType="Double"
                PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; SMA:#VALY">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaSMA" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="0" Height="100" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="SMA" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="True" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SMA" DataField="SMA" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" />
        </asp:GridView>
    </div>
</asp:Content>
