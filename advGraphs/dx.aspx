<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dx.aspx.cs" Inherits="Analytics.advGraphs.dmi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>DMI_Daily</title>
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
            <%--<h3 id="headingtext" runat="server" style="text-align: center">RSI Vs Daily</h3>--%>
            <table style="width: 100%">
                <tr>
                    <td colspan="3" style="text-align: center; font-size: medium;">
                        <asp:Label ID="headingtext" runat="server" Text="Trend Direction"></asp:Label>
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
                        <asp:CheckBox ID="checkBoxOpen" runat="server" Text="Open" AutoPostBack="True" TabIndex="5" />
                        <asp:CheckBox ID="checkBoxHigh" runat="server" Text="High" AutoPostBack="True" TabIndex="6" />
                        <asp:CheckBox ID="checkBoxLow" runat="server" Text="Low" AutoPostBack="True" TabIndex="7" />
                        <asp:CheckBox ID="checkBoxClose" runat="server" Text="Close" AutoPostBack="True" TabIndex="8" />
                        <asp:CheckBox ID="checkBoxCandle" runat="server" Checked="true" Text="Candlestick" AutoPostBack="True" TabIndex="9" />
                        <asp:CheckBox ID="checkBoxDX" runat="server" Checked="true" Text="Directional Movement (DX)" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxMINUS_DI" runat="server" Checked="true" Text="-ve Directinal Movement" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxPLUS_DI" runat="server" Checked="true" Text="+ve Directinal Movement" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxADX" runat="server" Checked="true" Text="Avg Directinal Movement Index(ADX)" AutoPostBack="True" TabIndex="4" />
                        <asp:CheckBox ID="checkBoxGrid" runat="server" Text="Raw data" AutoPostBack="True" TabIndex="10" />
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
                        <asp:Button ID="buttonDesc" runat="server" Text="Toggle Description" TabIndex="11" OnClick="buttonDesc_Click" />
                    </td>
                </tr>
                <tr id="trid" runat="server">
                    <td colspan="3" style="width: auto;">
                        <ul>
                            <li>The average directional index (ADX) is a technical analysis indicator used by some traders to determine the strength 
                                of a trend. The trend can be either up or down, and this is shown by two accompanying indicators, the Negative 
                                Directional Indicator (-DI) and the Positive Directional Indicator (+DI). </li>
                            <li>The indicator does this by comparing prior highs and lows and drawing two lines: a positive directional movement 
                                line (+DI) and a negative directional movement line (-DI). An optional third line, called directional 
                                movement (DX) shows the difference between the lines.</li>
                            <li>When +DI is above -DI, there is more upward pressure than downward pressure in the price. If -DI is above +DI, 
                                then there is more downward pressure in the price. This indicator may help traders assess the trend direction. </li>
                            <li>Crossovers between the lines are also sometimes used as trade signals to buy or sell.</li>
                            <li>The Average Directional Index (ADX) along with the Negative Directional Indicator (-DI) and 
                                the Positive Directional Indicator (+DI) are momentum indicators. The ADX helps investors determine trend strength while -DI 
                                and +DI help determine trend direction.</li>
                            <li>The ADX identifies a strong trend when the ADX is over 25 and a weak trend when the ADX is below 20.</li>
                            <li>Crossovers of the -DI and +DI lines can be used to generate trade signals. For example, if the +DI line crosses above 
                                the -DI line and the ADX is above 20, or ideally above 25, then that is a potential signal to buy.</li>
                            <li>If the -DI crosses above the +DI, and ADX is above 20 or 25, then that is an opportunity to enter a potential short trade.</li>
                            <li>Crosses can also be used to exit current trades. For example, if long, exit when the -DI crosses above the +DI.</li>
                            <li>When ADX is below 20 the indicator is signaling that the price is trendless, and therefore may not be an ideal time to enter a trade.</li>
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
                <asp:Chart ID="chartDMIDaily" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True" OnClick="chartDMIDaily_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <Legends>
                        <asp:Legend Name="legendDMIDaily" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="Open" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                            Legend="legendDMIDaily" LegendText="Open" XValueMember="Date" XValueType="Date" YValueMembers="Open" YValueType="Double"
                            PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
                        </asp:Series>
                        <asp:Series Name="High" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                            Legend="legendDMIDaily" LegendText="High" XValueMember="Date" XValueType="Date" YValueMembers="High" YValueType="Double"
                            PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
                        </asp:Series>
                        <asp:Series Name="Low" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                            Legend="legendDMIDaily" LegendText="Low" XValueMember="Date" XValueType="Date" YValueMembers="Low" YValueType="Double"
                            PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
                        </asp:Series>
                        <asp:Series Name="Close" XAxisType="Primary" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily1"
                            Legend="legendDMIDaily" LegendText="Close" XValueMember="Date" XValueType="Date" YValueMembers="Close" YValueType="Double"
                            PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
                        </asp:Series>
                        <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaDMIDaily1"
                            Legend="legendDMIDaily" LegendText="OHLC"
                            XValueMember="Date" XValueType="Date" YValueMembers="Open,High,Low,Close" YValueType="Double"
                            PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                            ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"
                            BorderColor="Black" Color="Black"
                            CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
                            <%--LegendPostBackValue="OHLC"--%>
                        </asp:Series>
                        <asp:Series Name="DX" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily2"
                            Legend="legendDMIDaily" LegendText="DX"
                            XValueMember="Date" XValueType="Date" YValueMembers="DX" YValueType="Double"
                            PostBackValue="DX,#VALX,#VALY" ToolTip="Date:#VALX; DX:#VALY" LegendToolTip="Directional Movement">
                        </asp:Series>
                        <asp:Series Name="MINUS_DI" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily2"
                            Legend="legendDMIDaily" LegendText="MINUS DI"
                            XValueMember="Date" XValueType="Date" YValueMembers="MINUS_DI" YValueType="Double"
                            PostBackValue="MINUS_DI,#VALX,#VALY" ToolTip="Date:#VALX; MINUS_DI:#VALY" LegendToolTip="Negative Directional Movement">
                        </asp:Series>
                        <asp:Series Name="PLUS_DI" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily2"
                            Legend="legendDMIDaily" LegendText="PLUS DI"
                            XValueMember="Date" XValueType="Date" YValueMembers="PLUS_DI" YValueType="Double"
                            PostBackValue="PLUS_DI,#VALX,#VALY" ToolTip="Date:#VALX; PLUS_DI:#VALY" LegendToolTip="Positive Directional Movement">
                        </asp:Series>
                        <asp:Series Name="ADX" YAxisType="Secondary" XAxisType="Primary" ChartType="Line" ChartArea="chartareaDMIDaily2"
                            Legend="legendDMIDaily" LegendText="ADX"
                            XValueMember="Date" XValueType="Date" YValueMembers="ADX" YValueType="Double"
                            PostBackValue="ADX,#VALX,#VALY" ToolTip="Date:#VALX; ADX:#VALY" LegendToolTip="Average Directional Movement Index">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaDMIDaily1">
                            <Position Auto="false" X="0" Y="3" Height="50" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="Daily Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                                LabelAutoFitStyle="WordWrap" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY>
                        </asp:ChartArea>
                        <asp:ChartArea Name="chartareaDMIDaily2" AlignWithChartArea="chartareaDMIDaily1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                            <Position Auto="false" X="0" Y="53" Height="47" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="Directional Movement Index" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                                <%--<StripLines>
                                    <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                                        BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Adjusted overbought level at 50" TextAlignment="Near" />
                                    <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="70"
                                        BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Overbought > 70%" TextAlignment="Near" />
                                    <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="30"
                                        BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="Oversold < 30%" TextAlignment="Near" />
                                </StripLines>--%>
                            </AxisY>
                            <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX2>
                            <AxisY2 Title="ADX" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisY2>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <div></div>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 40%;">
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
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                            <td style="width: 15%;">
                                <asp:GridView ID="GridViewDX" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewDX_PageIndexChanging" Caption="Directional Movement" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="DX" DataField="DX" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                            <td style="width: 15%;">
                                <asp:GridView ID="GridViewMINUSDI" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewMINUSDI_PageIndexChanging" Caption="Negative Directional Movement" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="MINUS_DI" DataField="MINUS_DI" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                            <td style="width: 15%;">
                                <asp:GridView ID="GridViewPLUSDI" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewPLUSDI_PageIndexChanging" Caption="Positive Directional Movement" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="PLUS_DI" DataField="PLUS_DI" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </td>
                            <td style="width: 15%;">
                                <asp:GridView ID="GridViewADX" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewADX_PageIndexChanging" Caption="Avg Directional Movement Index" CaptionAlign="Top">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="ADX" DataField="ADX" ItemStyle-HorizontalAlign="Center">
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
