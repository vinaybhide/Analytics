<%@ Page Title="Trend Reversal Indicator" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="macdemadaily.aspx.cs" Inherits="Analytics.macdemadaily" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

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
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label19" runat="server" Text="Fast Period:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxMACD_Fastperiod" runat="server" TextMode="Number" Width="40" Text="12" TabIndex="23"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label20" runat="server" Text="Slow Period:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxMACD_Slowperiod" runat="server" TextMode="Number" Width="40" Text="26" TabIndex="24"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label21" runat="server" Text="Signal Period:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxMACD_Signalperiod" runat="server" TextMode="Number" Width="40" Text="9" TabIndex="25"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <asp:Chart ID="chartAdvGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartAdvGraph_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendAdvGraph" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Open"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="OPEN" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="High"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Low"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="LOW" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Close"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="CLOSE" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="HIGH,LOW,OPEN,CLOSE" YValueType="Double"
                Legend="legendAdvGraph" LegendText="OHLC" PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; High:#VALY1; Low:#VALY2; Open:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
            </asp:Series>
            <asp:Series Name="EMA Small" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="EMA 12"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="EMA_SMALL" YValueType="Double"
                PostBackValue="EMA12,#VALX,#VALY" ToolTip="Date:#VALX; EMA12:#VALY" LegendToolTip="EMA 12">
            </asp:Series>
            <asp:Series Name="EMA Long" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="EMA 26"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="EMA_LONG" YValueType="Double"
                PostBackValue="EMA26,#VALX,#VALY" ToolTip="Date:#VALX; EMA26:#VALY" LegendToolTip="EMA 26">
            </asp:Series>

            <asp:Series Name="MACD" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="MACD"
                XValueMember="TIMESTAMP" XValueType="Date" YValuesPerPoint="3" YValueMembers="MACD,MACD_Signal,MACD_Hist" YValueType="Double"
                PostBackValue="MACD,#VALX,#VALY" ToolTip="Date:#VALX; MACD:#VALY" LegendToolTip="MACD">
            </asp:Series>
            <asp:Series Name="MACD Signal" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="Signal Line"
                XValueMember="TIMESTAMP" XValueType="Date" YValuesPerPoint="3" YValueMembers="MACD_Signal,MACD,MACD_Hist" YValueType="Double"
                PostBackValue="MACD_Signal,#VALX,#VALY" ToolTip="Date:#VALX; Signal:#VALY" LegendToolTip="MACD Signal">
            </asp:Series>
            <asp:Series Name="MACD Histogram" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="MACD Histogram"
                XValueMember="TIMESTAMP" XValueType="Date" YValuesPerPoint="3" YValueMembers="MACD_Hist,MACD,MACD_Signal" YValueType="Double"
                PostBackValue="MACD_Hist,#VALX,#VALY" ToolTip="Date:#VALX; History:#VALY" LegendToolTip="MACD Histogram">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="3" Height="50" Width="99" />
                <AxisX Enabled="false">
                    <LabelStyle Enabled="false" />
                </AxisX>
                <AxisY Title="Daily-Open/High/Low/close, EMA" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
                <AxisX2 IsMarginVisible="false"  IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisX2>
                <AxisY2 Title="EMA12/EMA26 Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY2>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="53" Height="47" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisX>
                <AxisY Title="MACD/Signal/Histogram" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="0"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="0" TextAlignment="Near" ToolTip="0 line" />
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
                <asp:BoundField HeaderText="SMA Small" DataField="SMA_SMALL" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SMA Long" DataField="SMA_LONG" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="EMA SMALL" DataField="EMA_SMALL" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="EMA LONG" DataField="EMA_LONG" ItemStyle-HorizontalAlign="Center">
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
