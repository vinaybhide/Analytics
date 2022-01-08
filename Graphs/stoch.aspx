<%@ Page Title="STOCH" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="stoch.aspx.cs" Inherits="Analytics.stoch" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Panel ID="panelParam" runat="server" Visible="true">
        <div style="width: 100%; border: groove;">
            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label1" runat="server" Text="Output size: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Outputsize" runat="server" TabIndex="26">
                            <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                            <asp:ListItem Value="Compact">Compact</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label2" runat="server" Text="Interval: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Interval" runat="server">
                            <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                            <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                            <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                            <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                            <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                            <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                            <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                            <asp:ListItem Value="1mo" Enabled="false">Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label19" runat="server" Text="FastK Period:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxSTOCH_Fastkperiod" runat="server" TextMode="Number" Width="40" Text="5" TabIndex="23"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label21" runat="server" Text="SlowD Period:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxSTOCH_Slowdperiod" runat="server" TextMode="Number" Width="40" Text="3" TabIndex="25"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label6" runat="server" Text="Series Type: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_SeriesType" runat="server" TabIndex="29">
                            <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                            <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                            <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                            <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Chart ID="chartStdGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartStdGraph_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendStdGraph" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="Open"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="OPEN" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open Price Line">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="High"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High Price Line">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="Low"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="LOW" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low Price Line">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="Close"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close Price Line">
            </asp:Series>
            <asp:Series Name="OHLC" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1" Legend="legendStdGraph"
                LegendText="OHLC"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH,LOW,OPEN,CLOSE" YValueType="Double"
                BorderColor="Black" Color="Black"
                PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4" ToolTip="Date:#VALX; Open:#VALY3; High:#VALY1; Low:#VALY2; Close:#VALY4"
                LegendToolTip="OHLC Candlestick"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Traingle">
            </asp:Series>
            <asp:Series Name="Volume" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartarea3" Legend="legendStdGraph"
                LegendText="Volume"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="VOLUME" YValueType="Auto"
                PostBackValue="Volume,#VALX,#VALY" ToolTip="Date:#VALX; Volume:#VALY" LegendToolTip="Daily Volume">
            </asp:Series>

            <asp:Series Name="K-FastLine" XAxisType="Primary" YAxisType="Primary" Legend="legendStdGraph" LegendText="K-FastLine" ChartType="Line" ChartArea="chartarea2"
                
                PostBackValue="0,K-FastLine,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; K-FastLine:#VALY{0.##}" LegendToolTip="K-FastLine">
            </asp:Series>
            <asp:Series Name="D-SlowLine" XAxisType="Primary" YAxisType="Primary" Legend="legendStdGraph" LegendText="D-SlowLine" ChartType="Line" ChartArea="chartarea2"
                
                PostBackValue="1,D-SlowLine,#VALX,#VALY{0.##}" ToolTip="Date:#VALX; D-SlowLine:#VALY{0.##}" LegendToolTip="D-SlowLine">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="3" Height="30" Width="99" />
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>

                <AxisY Title="Open/High/Low/Close Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>

            </asp:ChartArea>

            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="33" Height="30" Width="99" />

                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 8pt"  IsEndLabelVisible="true" />
                </AxisX>

                <AxisY Title="K-FastLine/D-SlowLine" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="true">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" Format="N2" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Adjusted overbought level at 50" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="80"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Overbought > 80%" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="20"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Oversold < 20%" TextAlignment="Near" />
                    </StripLines>
                </AxisY>
            </asp:ChartArea>

            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="66" Height="30" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Volume" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
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
                <asp:BoundField HeaderText="Date" DataField="TIMESTAMP" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Open" DataField="OPEN" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="High" DataField="HIGH" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Low" DataField="LOW" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Close" DataField="CLOSE" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Volume" DataField="VOLUME" ItemStyle-HorizontalAlign="Center">
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
