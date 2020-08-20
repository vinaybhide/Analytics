<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stochdaily.aspx.cs" Inherits="Analytics.advGraphs.stochdaily" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Stochastics_Daily</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        html, body, form {
            height: 100%;
        }

        .chart {
            width: 100% !important;
            height: 100% !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="font-size: small;">
        <div style="width: 100%; border: groove;">
            <table style="width: 100%">
                <tr>
                    <td colspan="3" style="text-align: center; font-size: medium;">
                        <asp:Label ID="headingtext" runat="server" Text="Stochastics Vs Daily"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: auto">
                        <asp:Label ID="Label2" runat="server" Text="From date:"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="textboxFromDate" runat="server" TextMode="Date" TabIndex="1"></asp:TextBox>
                    </td>
                    <td rowspan="2" style="width: auto;">
                        <asp:CheckBox ID="checkBoxSlowD" runat="server" Checked="true" Text="Slow D" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxSlowK" runat="server" Checked="true" Text="Slow K" AutoPostBack="True" TabIndex="5" />
                        <asp:CheckBox ID="checkBoxOpen" runat="server" Text="Open" AutoPostBack="True" TabIndex="6" />
                        <asp:CheckBox ID="checkBoxHigh" runat="server" Text="High" AutoPostBack="True" TabIndex="7" />
                        <asp:CheckBox ID="checkBoxLow" runat="server" Text="Low" AutoPostBack="True" TabIndex="8" />
                        <asp:CheckBox ID="checkBoxClose" runat="server" Text="Close" AutoPostBack="True" TabIndex="9" />
                        <asp:CheckBox ID="checkBoxCandle" runat="server" Checked="true" Text="Candlestick" AutoPostBack="True" TabIndex="10" />
                        <asp:CheckBox ID="checkBoxRSI" runat="server" Checked="true" Text="RSI" AutoPostBack="True" TabIndex="11" />
                        <asp:CheckBox ID="checkBoxGrid" runat="server" Text="Raw data" AutoPostBack="True" TabIndex="12" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: auto">
                        <asp:Label ID="Label1" runat="server" Text="To date:"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="textboxToDate" runat="server" TextMode="Date" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: auto"></td>
                    <td style="width: auto;">
                        <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph" OnClick="buttonShowGraph_Click" TabIndex="3" />
                    </td>
                    <td style="width: auto;">
                        <asp:Button ID="buttonDesc" runat="server" Text="Toggle Description" OnClick="buttonDesc_Click" TabIndex="13" />
                    </td>
                </tr>
                <tr id="trid" runat="server">
                    <td colspan="3" style="width: auto;">
                        <ul>
                            <li>Stochastics are used to show when a stock has moved into an overbought or oversold position.
                            </li>
                            <li>The premise of stochastics is that when a stock trends upwards, its closing price tends to trade at the high end of the 
                                day's range or price action. Price action refers to the range of prices at which a stock trades throughout the daily session.
                            </li>
                            <li>The K line is faster than the D line; the D line is the slower of the two. 
                            </li>
                            <li>The investor needs to watch asThe investor needs to watch as the D line and the price of the issue begin to change and move into either the overbought 
                                (over the 80 line) or the oversold (under the 20 line) positions. 
                                The investor needs to consider selling the stock when the indicator moves above the 80 levels. 
                                Conversely, the investor needs to consider buying an issue that is below the 20 line and is starting to move up with 
                                increased volume.
                            </li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%;">
            <ContentTemplate>
                <asp:Chart ID="chartSTOCHDaily" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True" OnClick="chartSTOCHDaily_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <Legends>
                        <asp:Legend Name="legendSTOCHDaily" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily1"
                            Legend="legendSTOCHDaily" LegendText="Open" XValueMember="Date" XValueType="Date" YValueMembers="Open" YValueType="Double"
                            PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
                        </asp:Series>
                        <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily1"
                            Legend="legendSTOCHDaily" LegendText="High" XValueMember="Date" XValueType="Date" YValueMembers="High" YValueType="Double"
                            PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
                        </asp:Series>
                        <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily1"
                            Legend="legendSTOCHDaily" LegendText="Low" XValueMember="Date" XValueType="Date" YValueMembers="Low" YValueType="Double"
                            PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
                        </asp:Series>
                        <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily1"
                            Legend="legendSTOCHDaily" LegendText="Close" XValueMember="Date" XValueType="Date" YValueMembers="Close" YValueType="Double"
                            PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
                        </asp:Series>
                        <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaSTOCHDaily1"
                            Legend="legendSTOCHDaily" LegendText="OHLC"
                            XValueMember="Date" XValueType="Date" YValueMembers="Open,High,Low,Close" YValueType="Double"
                            PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                            ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                            BorderColor="Black" Color="Black"
                            CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
                            <%--LegendPostBackValue="OHLC"--%>
                        </asp:Series>
                        <asp:Series Name="SlowK" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily2"
                            Legend="legendSTOCHDaily" LegendText="Slow K"
                            XValueMember="Date" XValueType="Date" YValueMembers="SlowK" YValueType="Double"
                            PostBackValue="SlowK,#VALX,#VALY" ToolTip="Date:#VALX; SlowK:#VALY" LegendToolTip="Slow K">
                            <%--LegendPostBackValue="VWAP"--%>
                        </asp:Series>
                        <asp:Series Name="SlowD" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily2"
                            Legend="legendSTOCHDaily" LegendText="Slow D"
                            XValueMember="Date" XValueType="Date" YValueMembers="SlowD" YValueType="Double"
                            PostBackValue="SlowD,#VALX,#VALY" ToolTip="Date:#VALX; SlowD:#VALY" LegendToolTip="Slow D">
                            <%--LegendPostBackValue="VWAP"--%>
                        </asp:Series>
                        <asp:Series Name="RSI" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaSTOCHDaily3"
                            Legend="legendSTOCHDaily" LegendText="RSI"
                            XValueMember="Date" XValueType="Date" YValueMembers="RSI" YValueType="Double"
                            PostBackValue="RSI,#VALX,#VALY" ToolTip="Date:#VALX; RSI:#VALY">
                            <%--LegendPostBackValue="VWAP"--%>
                        </asp:Series>

                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaSTOCHDaily1">
                            <Position Auto="false" X="0" Y="3" Height="30" Width="100" />
                            <AxisX>
                                <LabelStyle Enabled="false" />
                            </AxisX>
                            <AxisY Title="OHLC Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap" 
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY>
                        </asp:ChartArea>
                        <asp:ChartArea Name="chartareaSTOCHDaily2" AlignWithChartArea="chartareaSTOCHDaily1" AlignmentOrientation="Vertical" 
                            AlignmentStyle="PlotPosition">
                            <Position Auto="false" X="0" Y="33" Height="30" Width="100" />
                            <AxisX>
                                <LabelStyle Enabled="false" />
                            </AxisX>
                            <AxisY Title="Stochastics Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" 
                                LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt">
                                <StripLines>
                                    <asp:StripLine StripWidth="0" BorderColor="RoyalBlue"
                                        BorderWidth="2" BorderDashStyle="Dot" Interval="80"
                                        BackColor="RosyBrown" BackSecondaryColor="Purple"
                                        BackGradientStyle="LeftRight" Text="80" TextAlignment="Near" />
                                </StripLines>
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY>
                        </asp:ChartArea>
                        <asp:ChartArea Name="chartareaSTOCHDaily3" AlignWithChartArea="chartareaSTOCHDaily1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                            <Position Auto="false" X="0" Y="63" Height="35" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="RSI Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap" 
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <StripLines>
                                    <asp:StripLine StripWidth="0" BorderColor="RoyalBlue"
                                        BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="30" IntervalOffsetType="Number"
                                        BackColor="RosyBrown" BackSecondaryColor="Purple"
                                        BackGradientStyle="LeftRight" Text="30" TextAlignment="Near" TextLineAlignment="Far" />
                                    <asp:StripLine StripWidth="0" BorderColor="RoyalBlue"
                                        BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="70" IntervalOffsetType="Number"
                                        BackColor="RosyBrown" BackSecondaryColor="Purple"
                                        BackGradientStyle="LeftRight" Text="70" TextAlignment="Near" TextLineAlignment="Far" />
                                </StripLines>
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />

                            </AxisY>

                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <div></div>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;">
                                <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center"
                                    AllowPaging="True" OnPageIndexChanging="GridViewDaily_PageIndexChanging" Caption="Daily Price Data" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Open" DataField="Open" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="High" DataField="High" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Low" DataField="Low" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Close" DataField="Close" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Volume" DataField="Volume" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </td>
                            <td style="width: 30%;">
                                <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging" Caption="Stochastics Data" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Slow K" DataField="SlowK" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Slow D" DataField="SlowD" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                            <td style="width: 20%;">
                                <asp:GridView ID="GridViewRSI" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewRSI_PageIndexChanging" Caption="RSI Data" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="RSI" DataField="RSI" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        (function () {
            var panel = document.getElementById('<%= UpdatePanel1.ClientID %>');
            var panelWidth = document.getElementById('<%= panelWidth.ClientID %>');
            var panelHeight = document.getElementById('<%= panelHeight.ClientID %>');
            var initialWidth = panel.offsetWidth;
            var initialHeight = panel.offsetHeight;

            function getChangeRatio(val1, val2) {
                return Math.abs(val2 - val1) / val1;
            };

            function redrawChart() {
                setTimeout(function () {
                    initialWidth = panel.offsetWidth;
                    initialHeight = panel.offsetHeight;
                    document.getElementById('<%= btnPostBack.ClientID %>').click();
                }, 0);
            };

            function savePanelSize() {
                var isFirstDisplay = panelWidth.value == '';
                panelWidth.value = panel.offsetWidth;
                panelHeight.value = panel.offsetHeight;
                var widthChange = getChangeRatio(initialWidth, panel.offsetWidth);
                var heightChange = getChangeRatio(initialHeight, panel.offsetHeight);
                if (isFirstDisplay || widthChange > 0.2 || heightChange > 0.2) {
                    redrawChart();
                }
            };

            savePanelSize();
            window.addEventListener('resize', savePanelSize, false);
        })();
    </script>
</body>
</html>
