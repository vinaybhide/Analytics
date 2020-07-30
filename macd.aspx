<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="macd.aspx.cs" Inherits="Analytics.macd" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            <h3 id="headingtext" runat="server" style="text-align:center">Moving average convergence/divergence</h3>
            <table>
                <tr>
                    <td style="border-right: dashed; border-right-width: thin;">
                        <asp:Label ID="Label2" runat="server" Text="From date:"></asp:Label>
                        <asp:TextBox ID="textboxFromDate" runat="server" TextMode="Date"></asp:TextBox><br />
                        <asp:Label ID="Label1" runat="server" Text="To date:"></asp:Label>
                        <asp:TextBox ID="textboxToDate" runat="server" TextMode="Date"></asp:TextBox><br />
                        <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph to dates" OnClick="buttonShowGraph_Click" TabIndex="2" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%">
            <ContentTemplate>
                <asp:Chart ID="chartMACD" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid" EnableViewState="True"
                    OnClick="chartMACD_Click" ImageType="Png">
                    <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
                    <%--<Titles>
                        <asp:Title Name="titleMACD" Text="Moving Average Convergence Divergence" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendMACD" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" BorderDashStyle="Dash" BorderColor="Black"></asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="seriesMACD" Legend="legendMACD" LegendText="MACD" ChartType="Line" ChartArea="chartareaMACD" PostBackValue="#VALX, #VALY" ToolTip="MACD: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="seriesMACD_Hist" Legend="legendMACD" LegendText="MACD Historical" ChartType="Line" ChartArea="chartareaMACD" PostBackValue="#VALX, #VALY" ToolTip = "MACD_Hist: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="seriesMACD_Signal" Legend="legendMACD" LegendText="MACD Historical" ChartType="Line" ChartArea="chartareaMACD" PostBackValue="#VALX, #VALY" ToolTip = "MACD_Signal: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaMACD" AlignmentOrientation="Vertical"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
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
