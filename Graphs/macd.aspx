<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="macd.aspx.cs" Inherits="Analytics.macd" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>MACD</title>
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
            <table id="tableID" style="width: 100%;">
                <tr>
                    <td colspan="6" style="text-align: center; font-size: medium;">
                        <asp:Label ID="headingtext" runat="server" Text="MACD"></asp:Label>
                    </td>
                </tr>
                <tr style="width: 100%;">
                    <td style="width: 8%; text-align: right;">
                        <asp:Label ID="Label2" runat="server" Text="From date:"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:TextBox ID="textboxFromDate" runat="server" Width="100%" TextMode="Date" TabIndex="1"></asp:TextBox>
                    </td>
                    <td style="width: 8%; text-align: right;">
                        <asp:Label ID="Label1" runat="server" Text="To date:"></asp:Label>
                    </td>
                    <td style="width: 15%;">
                        <asp:TextBox ID="textboxToDate" runat="server" Width="90%" TextMode="Date" TabIndex="2"></asp:TextBox>
                    </td>
                    <td style="width: 60%;">
                        <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph" OnClick="buttonShowGraph_Click" TabIndex="3" />
                        <asp:Button ID="buttonShowGrid" runat="server" Text="Show Raw Data" TabIndex="4" OnClick="buttonShowGrid_Click" />
                        <asp:Button ID="buttonDesc" runat="server" Text="Toggle Description" OnClick="buttonDesc_Click" TabIndex="5" />
                    </td>
                </tr>
                <tr id="trid" runat="server">
                    <td colspan="6" style="width: 100%;">
                        <ul>
                            <li>Moving Average Convergence Divergence (MACD) is a trend-following momentum indicator that shows the relationship between 
                                two moving averages of a security’s price. The MACD is calculated by subtracting the 26-period Exponential Moving Average 
                                (EMA) from the 12-period EMA.</li>
                            <li>MACD triggers technical signals when it crosses above (to buy) or below (to sell) its signal line.</li>
                            <li>The speed of crossovers is also taken as a signal of a market is overbought or oversold.
                            </li>
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
                <asp:Chart ID="chartMACD" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True" OnClick="chartMACD_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
                    <%--<Titles>
                        <asp:Title Name="titleMACD" Text="Moving Average Convergence Divergence" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendMACD" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="seriesMACD" XAxisType="Secondary" YAxisType="Primary" Legend="legendMACD" LegendText="MACD" ChartType="Line" ChartArea="chartareaMACD"
                            XValueMember="Date" XValueType="Date" YValueMembers="MACD" YValueType="Double"
                            PostBackValue="MACD,#VALX,#VALY" ToolTip="Date:#VALX; MACD:#VALY" LegendToolTip="MACD Line">
                        </asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="seriesMACD_Signal" XAxisType="Secondary" YAxisType="Primary" Legend="legendMACD" LegendText="MACD Signal" ChartType="Line" ChartArea="chartareaMACD"
                            XValueMember="Date" XValueType="Date" YValueMembers="MACD_Signal" YValueType="Double"
                            PostBackValue="MACD_Signal,#VALX,#VALY" ToolTip="Date:#VALX; MACD_Signal:#VALY" LegendToolTip="Signal Line">
                        </asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="seriesMACD_Hist" XAxisType="Primary" YAxisType="Secondary" Legend="legendMACD" LegendText="MACD Historical"
                            ChartType="Column" ChartArea="chartareaMACD"
                            XValueMember="Date" XValueType="Date" YValueMembers="MACD_Hist" YValueType="Double"
                            PostBackValue="MACD_Hist,#VALX,#VALY" ToolTip="Date:#VALX; MACD_Hist:#VALY" LegendToolTip="MACD Histogram">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaMACD" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="MACD & Signal" IsStartedFromZero="true" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY>
                            <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX2>

                            <AxisY2 Title="MACD History" IsStartedFromZero="true" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY2>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <div></div>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
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
