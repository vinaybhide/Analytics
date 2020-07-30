<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="crossover.aspx.cs" Inherits="Analytics.crossover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crossover</title>
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
            <h3 id="headingtext" runat="server" style="text-align: center">Crossover</h3>
            <table style="width: 100%">
                <tr style="text-align: center;">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="From date:"></asp:Label>
                        <asp:TextBox ID="textboxFromDate" runat="server" TextMode="Date"></asp:TextBox><br />
                        <asp:Label ID="Label1" runat="server" Text="To date:"></asp:Label>
                        <asp:TextBox ID="textboxToDate" runat="server" TextMode="Date"></asp:TextBox><br />
                        <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph to dates" TabIndex="2" OnClick="buttonShowGraph_Click"/>
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
                    EnableViewState="True" OnClick="chartCrossover_Click" >
                    <Legends>
                        <asp:Legend Name="legendCrossover" LegendItemOrder="ReversedSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="chartareaCrossover" IsDockedInsideChartArea="false">
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="OHLC" YAxisType="Primary" XAxisType="Primary"  ChartType="Candlestick" ChartArea="chartareaCrossover"
                            Legend="legendCrossover" LegendText="OHLC" PostBackValue="#VALX,#VALY4,OHLC"
                            ToolTip="Date:#VALX; Open:#VALY1; High:#VALY2; Low:#VALY3; Close:#VALY4" LegendPostBackValue="OHLC">
                        </asp:Series>
                        <asp:Series Name="SMA1" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaCrossover" Legend="legendCrossover" LegendText="SMA 1"
                            PostBackValue="#VALX,#VALY,SMA1" ToolTip="SMA1: Date:#VALX; Value:#VALY" MarkerSize="8" MarkerStep="10" MarkerStyle="Cross" LegendPostBackValue="SMA1">
                        </asp:Series>
                        <asp:Series Name="SMA2" YAxisType="Secondary" XAxisType="Secondary" ChartType="Line" ChartArea="chartareaCrossover" Legend="legendCrossover" LegendText="SMA 2"
                            PostBackValue="#VALX,#VALY,SMA2" ToolTip="SMA2: Date:#VALX; Value:#VALY" MarkerSize="8" MarkerStep="10" MarkerStyle="Cross" LegendPostBackValue="SMA2">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaCrossover"></asp:ChartArea>
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
