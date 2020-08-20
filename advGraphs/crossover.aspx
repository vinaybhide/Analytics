﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="crossover.aspx.cs" Inherits="Analytics.crossover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Crossover</title>
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
    <form id="form1" runat="server" style="font-size: xx-small;">
        <div style="width: 100%; border: groove; text-align: left">
            <%--<h3 id="headingtext" runat="server" style="text-align: center">Crossover (Golden/Death Cross)</h3>--%>
            <table style="width: 100%">
                <tr>
                    <td colspan="3" style="text-align: center; font-size: medium;">
                        <asp:Label ID="headingtext" runat="server" Text="Crossover (Golden/Death Cross)"></asp:Label>
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
                        <asp:CheckBox ID="checkBoxSMA1" runat="server" Checked="true" Text="SMA 50" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxSMA2" runat="server" Checked="true" Text="SMA 100" AutoPostBack="True" TabIndex="5" />
                        <asp:CheckBox ID="checkBoxCandle" runat="server" Checked="true" Text="Candlestick" AutoPostBack="True" TabIndex="6" />
                        <asp:CheckBox ID="checkBoxOpen" runat="server" Text="Open" AutoPostBack="True" TabIndex="7" />
                        <asp:CheckBox ID="checkBoxHigh" runat="server" Text="High" AutoPostBack="True" TabIndex="8" />
                        <asp:CheckBox ID="checkBoxLow" runat="server" Text="Low" AutoPostBack="True" TabIndex="9" />
                        <asp:CheckBox ID="checkBoxClose" runat="server" Text="Close" AutoPostBack="True" TabIndex="10" />
                        <asp:CheckBox ID="checkBoxVolume" runat="server" Checked="true" Text="Volume" AutoPostBack="True" TabIndex="11" />
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
                    <td colspan="6" style="width: auto;">
                        <ul>
                            <li>The crossover is a point on the trading chart in which a security's price and a technical indicator line intersect, 
                                or when two indicators themselves cross. Crossovers are used to estimate the performance of a financial instrument and 
                                to predict coming changes in trend, such as reversals or breakouts.
                            </li>
                            <li>A golden cross and a death cross are exact opposites. A golden cross indicates a long-term bull market going forward, 
                            while a death cross signals a long-term bear market.</li>
                            <li>The golden cross occurs when a short-term moving average crosses over a major long-term moving average to the upside and 
                                is interpreted as signaling a definitive upward turn in a market. There are three stages to a golden cross:
                            <ul>
                                <li>A downtrend that eventually ends as selling is depleted</li>
                                <li>A second stage where the shorter moving average crosses up through the longer moving average</li>
                                <li>Finally, the continuing uptrend, hopefully leading to higher prices.</li>
                            </ul>
                            </li>
                            <li>Conversely, a similar downside moving average crossover constitutes the death cross and is understood to signal a 
                                decisive downturn in a market. The death cross occurs when the short term average trends down and crosses the 
                                long-term average, basically going in the opposite direction of the golden cross.</li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%">
            <ContentTemplate>
                <asp:Chart ID="chartCrossover" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True" OnClick="chartCrossover_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <Legends>
                        <asp:Legend Name="legendCrossover" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaCrossover"
                            Legend="legendCrossover" LegendText="Open"
                            XValueMember="Date" XValueType="Date" YValueMembers="Open" YValueType="Double"
                            PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
                        </asp:Series>
                        <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaCrossover"
                            Legend="legendCrossover" LegendText="High"
                            XValueMember="Date" XValueType="Date" YValueMembers="High" YValueType="Double"
                            PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
                        </asp:Series>
                        <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaCrossover"
                            Legend="legendCrossover" LegendText="Low"
                            XValueMember="Date" XValueType="Date" YValueMembers="Low" YValueType="Double"
                            PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
                        </asp:Series>
                        <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaCrossover"
                            Legend="legendCrossover" LegendText="Close"
                            XValueMember="Date" XValueType="Date" YValueMembers="Close" YValueType="Double"
                            PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
                        </asp:Series>
                        <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaCrossover"
                            XValueMember="Date" XValueType="Date" YValueMembers="Open,High,Low,Close" YValueType="Double"
                            Legend="legendCrossover" LegendText="OHLC" PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                            ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                            BorderColor="Black" Color="Black"
                            CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
                            <%--LegendPostBackValue="OHLC"--%>
                        </asp:Series>
                        <asp:Series Name="SMA1" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaCrossover" Legend="legendCrossover"
                            LegendText="SMA 50"
                            XValueMember="Date" XValueType="Date" YValueMembers="SMA" YValueType="Double"
                            PostBackValue="SMA1,#VALX,#VALY" ToolTip="Date:#VALX; SMA1:#VALY" LegendToolTip="SMA1">
                            <%--MarkerSize="8" MarkerStep="10" MarkerStyle="Cross" --%>
                            <%--LegendPostBackValue="SMA1"--%>
                        </asp:Series>
                        <asp:Series Name="SMA2" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaCrossover" Legend="legendCrossover"
                            LegendText="SMA 100"
                            XValueMember="Date" XValueType="Date" YValueMembers="SMA" YValueType="Double"
                            PostBackValue="SMA2,#VALX,#VALY" ToolTip="Date:#VALX; SMA2:#VALY" LegendToolTip="SMA2">
                            <%--MarkerSize="8" MarkerStep="10" MarkerStyle="Cross" --%>
                            <%--LegendPostBackValue="SMA2"--%>
                        </asp:Series>
                        <asp:Series Name="Volume" XAxisType="Primary" YAxisType="Primary" ChartType="Column" ChartArea="chartareaVolume"
                            Legend="legendCrossover" LegendText="Volume"
                            XValueMember="Date" XValueType="Date" YValueMembers="Volume" YValueType="Auto"
                            PostBackValue="Volume,#VALX,#VALY" ToolTip="Date:#VALX; Volume:#VALY" LegendToolTip="Volume">
                        </asp:Series>

                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaCrossover">
                            <Position Auto="false" X="0" Y="3" Height="50" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90"
                                TitleFont="Microsoft Sans Serif, 5pt">
                                <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="Daily Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                                LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisY>
                            <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90"
                                TitleFont="Microsoft Sans Serif, 5pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX2>
                            <AxisY2 Title="SMA1/SMA2 Values" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisY2>
                        </asp:ChartArea>
                        <asp:ChartArea Name="chartareaVolume" AlignWithChartArea="chartareaCrossover" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                            <Position Auto="false" X="0" Y="53" Height="47" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="Daily Volume" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <div></div>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 50%;">
                                <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="true" Caption="Daily Data" CaptionAlign="Top" OnPageIndexChanging="GridViewDaily_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Open" DataField="Open" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="High" DataField="High" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Low" DataField="Low" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Close" DataField="Close" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Volume" DataField="Volume" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                            <td style="width: 25%;">
                                <asp:GridView ID="GridViewSMA1" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" Caption="SMA1" CaptionAlign="Top" OnPageIndexChanging="GridViewSMA1_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SMA1" DataField="SMA" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>

                            </td>
                            <td style="width: 25%;">
                                <asp:GridView ID="GridViewSMA2" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" Caption="SMA2" CaptionAlign="Top" OnPageIndexChanging="GridViewSMA2_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SMA2" DataField="SMA" ItemStyle-HorizontalAlign="Center">
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
