<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dailygraph.aspx.cs" Inherits="Analytics.dailygraph" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily Graph</title>
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
    <form id="form1" runat="server">
        <div style="width: 100%; border: groove; text-align: left">
            <h3 id="headingtext" runat="server" style="text-align:center">Daily</h3>
            <table>
                <tr>
                    <td style="border-right:dashed;border-right-width:thin;">
                        <asp:Label ID="Label2" runat="server" Text="From date:"></asp:Label>
                        <asp:TextBox ID="textboxFromDate" runat="server" TextMode="Date"></asp:TextBox><br />
                        <asp:Label ID="Label1" runat="server" Text="To date:"></asp:Label>
                        <asp:TextBox ID="textboxToDate" runat="server" TextMode="Date"></asp:TextBox><br />
                        <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph to dates" OnClick="buttonShowGraph_Click" TabIndex="2" />
                    </td>
                    <td>
                        <asp:CheckBox ID="checkBoxOpen" runat="server" Text="Open" AutoPostBack="True" OnCheckedChanged="checkBoxOpen_CheckedChanged" /><br />
                        <asp:CheckBox ID="checkBoxHigh" runat="server" Text="High" AutoPostBack="True" OnCheckedChanged="checkBoxHigh_CheckedChanged" /><br />
                        <asp:CheckBox ID="checkBoxLow" runat="server" Text="Low" AutoPostBack="True" OnCheckedChanged="checkBoxLow_CheckedChanged" /><br />
                        <asp:CheckBox ID="checkBoxClose" runat="server" Text="Close" AutoPostBack="True" OnCheckedChanged="checkBoxClose_CheckedChanged" /><br />
                        <asp:CheckBox ID="checkBoxCandle" runat="server" Text="Candlestick" AutoPostBack="True" OnCheckedChanged="checkBoxCandle_CheckedChanged" /><br />
                        <asp:CheckBox ID="checkBoxVolume" runat="server" Text="Volume" AutoPostBack="True" OnCheckedChanged="checkBoxVolume_CheckedChanged" /><br />
                        <asp:CheckBox ID="checkBoxGrid" runat="server" Text="Raw data" AutoPostBack="True" OnCheckedChanged="checkBoxGrid_CheckedChanged" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%">
            <ContentTemplate>
                <asp:Chart ID="chartdailyGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid">
                    <%--                    <Titles>
                        <asp:Title Name="titleDaily" Text="Daily Open/High/Low/Close - " Alignment="TopCenter" Font="Microsoft Sans Serif, 8pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendDaily" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" BorderDashStyle="Dash" BorderColor="Black"></asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="Open" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="Open" ToolTip="Open: Date:#VALX; Value:#VALY"></asp:Series>
                        <asp:Series Name="High" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="High" ToolTip="High: Date:#VALX; Value:#VALY"></asp:Series>
                        <asp:Series Name="Low" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="Low" ToolTip="Low: Date:#VALX; Value:#VALY"></asp:Series>
                        <asp:Series Name="Close" YAxisType="Primary" ChartType="Line" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="Close" ToolTip="Close: Date:#VALX; Value:#VALY"></asp:Series>
                        <asp:Series Name="OHLC" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="OHLC" ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4"></asp:Series>
                        <asp:Series Name="Volume" YAxisType="Secondary" ChartType="Column" ChartArea="chartareaDaily" Legend="legendDaily" LegendText="Volume" ToolTip="Volume: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaDaily"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <div></div>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <div></div>
                <asp:GridView ID="GridViewDaily" Visible="false" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField HeaderText="Date" DataField="Date" />
                        <asp:BoundField HeaderText="Open" DataField="Open" />
                        <asp:BoundField HeaderText="High" DataField="High" />
                        <asp:BoundField HeaderText="Low" DataField="Low" />
                        <asp:BoundField HeaderText="Close" DataField="Close" />
                        <asp:BoundField HeaderText="Volume" DataField="Volume" />
                    </Columns>
                </asp:GridView>
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
