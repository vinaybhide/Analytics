<%@ Page Title="Bollinger Bands" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="bbands.aspx.cs" Inherits="Analytics.bbands" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartBollingerBands" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartBollingerBands_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleBbands" Text="Bollinger Bands" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <Legends>
            <asp:Legend Name="legendBBands" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Real Lower Band" Legend="legendBBands" LegendText="Lower Band" ChartType="Line" ChartArea="chartareaBollingerBands"
                XValueMember="Date" XValueType="Date" YValueMembers="Real Lower Band" YValueType="Double"
                PostBackValue="Real Lower Band,#VALX,#VALY" ToolTip="Date:#VALX; Lower Band:#VALY" LegendToolTip="Lower Band">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="Real Middle Band" Legend="legendBBands" LegendText="Middle Band" ChartType="Line" ChartArea="chartareaBollingerBands"
                XValueMember="Date" XValueType="Date" YValueMembers="Real Middle Band" YValueType="Double"
                PostBackValue="Real Middle Band,#VALX,#VALY" ToolTip="Date:#VALX; Middle Band:#VALY" LegendToolTip="Middle Band">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="Real Upper Band" Legend="legendBBands" LegendText="Upper Band" ChartType="Line" ChartArea="chartareaBollingerBands"
                XValueMember="Date" XValueType="Date" YValueMembers="Real Upper Band" YValueType="Double"
                PostBackValue="Real Upper Band,#VALX,#VALY" ToolTip="Date:#VALX; Upper Band:#VALY" LegendToolTip="Upper Band">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaBollingerBands" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="3" Height="97" Width="98" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Bollinger Bands" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="0"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="0" TextAlignment="Near" />
                    </StripLines>
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
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Real Lower Band" DataField="Real Lower Band" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Real Middle Band" DataField="Real Middle Band" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Real Upper Band" DataField="Real Upper Band" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
</asp:Content>
