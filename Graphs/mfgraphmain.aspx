<%@ Page Title="" Language="C#" MasterPageFile="~/graphs/mfstandardindicators.Master" AutoEventWireup="true" CodeBehind="mfgraphmain.aspx.cs" Inherits="Analytics.mfGraphMain" %>

<%@ MasterType VirtualPath="~/Graphs/mfstandardindicators.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartMF" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartMF_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendMF" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
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

        <%--<Series>--%>
        <%--<asp:Series Name="DAILY_NAV" Enabled="false" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendMF" LegendText="Daily NAV"
                XValueMember="NAVDate" XValueType="Date" YValueMembers="NET_ASSET_VALUE" YValueType="Double"
                PostBackValue="#VALX,#VALY{#.00}" ToolTip="Date:#VALX; NAV:#VALY{#.00}" LegendToolTip="NAV Price Line">
            </asp:Series>--%>
        <%--<asp:Series Name="Indicator" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2" Legend="legendMF" LegendText="Indicator"
                PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; Indicator:#VALY" LegendToolTip="Indicator">
            </asp:Series>--%>
        <%--</Series>--%>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <%--<Position Auto="false" X="0" Y="3" Height="47" Width="99" />--%>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>

                <AxisY Title="Daily NAV" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="N2" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" Visible="false" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <%--<Position Auto="false" X="0" Y="55" Height="45" Width="99" />--%>
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Indicator" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="true">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="N2" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea3" Visible="false" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <%--<Position Auto="false" X="0" Y="55" Height="45" Width="99" />--%>
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Indicator" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="true">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="N2" />
                </AxisY>
            </asp:ChartArea>

        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%; font-size: small; text-align: center;">
            <tr>
                <td style="width: 100%; text-align: center;">
                    <asp:GridView ID="GridViewDaily" Enabled="false" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center"
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
