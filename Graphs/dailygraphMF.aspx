<%@ Page Title="" Language="C#" MasterPageFile="~/Graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="dailygraphMF.aspx.cs" Inherits="Analytics.Graphs.dailygraphMF" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartdailyGraphMF" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartdailyGraphMF_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendDailyNAV" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>

        <%--<asp:Series Name="NAV" ChartType="Line" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="NAV Daily"
                <%--XAxisType="Primary" YAxisType="Primary"
                XValueMember="NAVDATE" XValueType="Auto" YValueMembers="NET_ASSET_VALUE" YValueType="Double"
                PostBackValue="#VALX,#VALY" ToolTip="NAV Date:#VALX; NAV:#VALY"
                <%--LegendToolTip="NAV Price Line">
            </asp:Series>--%>

        <Series>
            <asp:Series Name="NAV" ChartType="Line" ChartArea="chartareaDaily" Legend="legendDailyNAV" LegendText="NAV"
                XValueMember="NAVDate" XValueType="Date" YValueMembers="NET_ASSET_VALUE" YValueType="Double"
                PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; NAV:#VALY">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaDaily" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="3" Height="97" Width="98" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <%--Title="NAV Date" TitleAlignment="Center" IsStartedFromZero="false"--%>
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true"/>
                </AxisX>

                <AxisY Title="NAV" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center"
            AllowPaging="True" OnPageIndexChanging="GridViewDaily_PageIndexChanging"
            PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField HeaderText="NAV Date" DataField="NAVDATE" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Scheme Code" DataField="SCHEMECODE" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fund Name" DataField="SCHEMENAME" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="NAV" DataField="NET_ASSET_VALUE" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
</asp:Content>
