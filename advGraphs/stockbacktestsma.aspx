<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="stockbacktestsma.aspx.cs" Inherits="Analytics.advGraphs.stockbacktestsma" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Panel ID="panelParam" runat="server" Visible="true">
        <div style="width: 100%; border: groove;">
            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label4" runat="server" Text="SMA Small Period: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxSMASmallPeriod" runat="server" TextMode="Number" Text="10" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label5" runat="server" Text="SMA Long Period: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxSMALongPeriod" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label6" runat="server" Text="Buy span in days: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxBuySpan" runat="server" TextMode="Number" Width="40" Text="2" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label7" runat="server" Text="Sell span in days: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxSellSpan" runat="server" TextMode="Number" Width="40" Text="20" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label9" runat="server" Text="Simulation Quantity: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxSimulationQty" runat="server" TextMode="Number" Width="60" Text="100" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="width: 10%; text-align: right;">
                        <asp:Label ID="Label1" runat="server" Text="Forecast Regression Type:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRegressionType" runat="server">
                            <asp:ListItem Text="Linear" Value="2" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Exponential" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Logarithmic" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Power" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label2" runat="server" Text="Forecasting period: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxForecastPeriod" runat="server" TextMode="Number" Width="60" Text="40" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>

        </div>
    </asp:Panel>
    <asp:Chart ID="chartBackTest" runat="server" CssClass="auto-style1" Visible="false" BorderlineColor="Black"
        BorderlineDashStyle="Solid" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        EnableViewState="True" OnClick="chartBackTest_Click"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendBackTest" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Near" LegendStyle="Row" IsTextAutoFit="true" AutoFitMinFontSize="15"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false">
                <Position X="0" Y="0" Height="5" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <%--XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Close" YValueType="Double"--%>
            <asp:Series Name="Future_Price" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendBackTest" LegendText="Daily_Future"
                PostBackValue="Future_Price,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; Future_Price:#VALY{0.##}" LegendToolTip="Future_Price">
            </asp:Series>
            <asp:Series Name="SMA_SMALL_Future" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendBackTest" LegendText="SMA_SMALL_Future"
                PostBackValue="SMA_SMALL_Future,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; SMA_SMALL_Future:#VALY{0.##}" LegendToolTip="SMA_SMALL_Future">
            </asp:Series>
            <asp:Series Name="SMA_LONG_Future" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendBackTest" LegendText="SMA_LONG_Future"
                PostBackValue="SMA_LONG_Future,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; SMA_LONG_Future:#VALY{0.##}" LegendToolTip="SMA_LONG_Future">
            </asp:Series>

            <asp:Series Name="Approximation_Error" XAxisType="Primary" YAxisType="Primary" ChartType="Range" ChartArea="chartarea3"
                Legend="legendBackTest" LegendText="Approximation_Error"
                YValuesPerPoint="2"
                PostBackValue="Approximation_Error,#VALX,#VALY1{0.##},#VALY2{0.##}" 
                ToolTip="Date:#VALX; Approx error: #VALY1{0.##}; Forecast error: #VALY2{0.##}" LegendToolTip="Approximation_Error">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="5" Height="30" Width="99" />
                <AxisY Title="Daily Price/SMA Small/SMA Long" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="35" Height="30" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Forecast" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="0.##" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="67" Height="30" Width="99" />
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
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:GridView ID="gridviewBackTest" Visible="false" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewBackTest_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" Caption="Daily Price with SMA" CaptionAlign="Top">
                        <Columns>
                            <%--<asp:BoundField HeaderText="Fund House" DataField="MF_COMP_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:BoundField HeaderText="SYMBOL" DataField="SYMBOL" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="CLOSE" DataField="CLOSE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Date" DataField="TIMESTAMP" ItemStyle-HorizontalAlign="Center">
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
                    <asp:GridView ID="gridviewPortfolioValuation" Visible="false" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewPortfolioValuation_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" Caption="Valuation" CaptionAlign="Top">
                        <Columns>
                            <%--<asp:BoundField HeaderText="Fund House" DataField="MF_COMP_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:BoundField HeaderText="SYMBOL" DataField="SYMBOL" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="CLOSE" DataField="CLOSE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="TIMESTAMP" DataField="TIMESTAMP" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Date" DataField="PURCHASE_DATE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Price" DataField="PURCHASE_PRICE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Qty" DataField="PURCHASE_QTY" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Cost" DataField="INVESTMENT_COST" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Qty" DataField="CumulativeQty" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Cost" DataField="CumulativeCost" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Value" DataField="CumulativeValue" ItemStyle-HorizontalAlign="Center">
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
