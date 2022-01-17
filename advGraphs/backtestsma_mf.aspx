<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="backtestsma_mf.aspx.cs" Inherits="Analytics.advGraphs.backtestsma_mf" %>

<%@ MasterType VirtualPath="~/advGraphs/advancegraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartBackTest" runat="server" CssClass="auto-style1" Visible="false" BorderlineColor="Black"
        BorderlineDashStyle="Solid" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        EnableViewState="True" OnClick="chartBackTest_Click"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendBackTest" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Near" LegendStyle="Row" IsTextAutoFit="true" AutoFitMinFontSize="15"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <%--<Series>--%>
        <%--<asp:Series Name="Future_Price" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendBackTest" LegendText="Daily_Future"
                PostBackValue="Future_Price,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; Future_Price:#VALY{0.##}" LegendToolTip="Future_Price">
            </asp:Series>--%>
        <%--<asp:Series Name="SMA_SMALL_Future" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendBackTest" LegendText="SMA_SMALL_Future"
                PostBackValue="SMA_SMALL_Future,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; SMA_SMALL_Future:#VALY{0.##}" LegendToolTip="SMA_SMALL_Future">
            </asp:Series>--%>
        <%--<asp:Series Name="SMA_LONG_Future" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendBackTest" LegendText="SMA_LONG_Future"
                PostBackValue="SMA_LONG_Future,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; SMA_LONG_Future:#VALY{0.##}" LegendToolTip="SMA_LONG_Future">
            </asp:Series>--%>

        <%--<asp:Series Name="Approximation_Error" XAxisType="Primary" YAxisType="Primary" ChartType="Range" ChartArea="chartarea3"
                Legend="legendBackTest" LegendText="Approximation_Error"
                YValuesPerPoint="2"
                PostBackValue="Approximation_Error,#VALX,#VALY1{0.##},#VALY2{0.##}" 
                ToolTip="Date:#VALX; Approx error: #VALY1{0.##}; Forecast error: #VALY2{0.##}" LegendToolTip="Approximation_Error">
            </asp:Series>--%>

        <%--</Series>--%>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="true" />
                <AxisY Title="Daily Price/SMA Small/SMA Long" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="true" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Forecast" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="true" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Approximation Error" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##" />
                </AxisY>
            </asp:ChartArea>

        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%; font-size: small; text-align: center;">
            <tr>
                <td style="width: 100%; text-align: center;">
                    <asp:GridView ID="gridviewBackTest" Enabled="false" Visible="false" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewBackTest_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" Caption="Daily NAV with SMA" CaptionAlign="Top">
                        <Columns>
                            <%--<asp:BoundField HeaderText="Fund House" DataField="MF_COMP_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:BoundField HeaderText="SCHEME CODE" DataField="SCHEMECODE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SCHEME NAME" DataField="SCHEMENAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="NAV" DataField="NET_ASSET_VALUE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="NAV Date" DataField="NAVDATE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SMA SMALL" DataField="SMA_SMALL" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SMA LONG" DataField="SMA_LONG" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="CROSSOVER FLAG" DataField="CROSSOVER_FLAG" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="BUY FLAG" DataField="BUY_FLAG" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SELL FLAG" DataField="SELL_FLAG" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SIMULATION QTY" DataField="QUANTITY" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="BUY COST" DataField="BUY_COST" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SELL VALUE" DataField="SELL_VALUE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="PROFIT_LOSS" DataField="PROFIT_LOSS" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="RESULT" DataField="RESULT" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>

                        <%--<HeaderStyle HorizontalAlign="Center" />--%>

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                    </asp:GridView>
                </td>
                <td>
                    <asp:GridView ID="gridviewPortfolioValuation" Enabled="false" Visible="false" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewPortfolioValuation_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" Caption="Valuation" CaptionAlign="Top">
                        <Columns>
                            <%--<asp:BoundField HeaderText="Fund House" DataField="MF_COMP_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:BoundField HeaderText="SCHEME CODE" DataField="SCHEME_CODE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="SCHEME NAME" DataField="SCHEME_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="NAV" DataField="NET_ASSET_VALUE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="NAV Date" DataField="DATE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Date" DataField="PurchaseDate" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase NAV" DataField="PurchaseNAV" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Units" DataField="PurchaseUnits" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Cost" DataField="ValueAtCost" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Units" DataField="CumulativeUnits" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Cost" DataField="CumulativeCost" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Value" DataField="CurrentValue" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>

                        <%--<HeaderStyle HorizontalAlign="Center" />--%>

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
