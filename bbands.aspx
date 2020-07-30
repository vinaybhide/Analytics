<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bbands.aspx.cs" Inherits="Analytics.bbands" %>

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
            <h3 id="headingtext" runat="server" style="text-align:center">Bollinger Bands:</h3>
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
                <asp:Chart ID="chartBollingerBands" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid" EnableViewState="True"
                    OnClick="chartBollingerBands_Click" ImageType="Png">
                    <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
                    <%--<Titles>
                        <asp:Title Name="titleBbands" Text="Bollinger Bands" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendBBands" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" BorderDashStyle="Dash" BorderColor="Black"></asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="Real Lower Band" Legend="legendBBands" LegendText="Lower Band" ChartType="Line" 
                            ChartArea="chartareaBollingerBands" PostBackValue="#VALX, #VALY" ToolTip = "Lower Band: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="Real Middle Band" Legend="legendBBands" LegendText="Middle Band" ChartType="Line" 
                            ChartArea="chartareaBollingerBands" PostBackValue="#VALX, #VALY" ToolTip = "Middle Band: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="Real Upper Band" Legend="legendBBands" LegendText="Upper Band" ChartType="Line" 
                            ChartArea="chartareaBollingerBands" PostBackValue="#VALX, #VALY" ToolTip = "Upper Band: Date:#VALX; Value:#VALY"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaBollingerBands" AlignmentOrientation="Vertical"></asp:ChartArea>
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

        //Sys.UI.DomEvent.addHandler(document, 'mouseover', OnMouseOver);
        //function OnMouseOver(evt) {
        //    var message = evt.clientX + ',' + evt.clientY + ',' + evt.screenX + ',' + evt.screenY;
        //    __doPostBack('DRAW', message);
        //}
        //function drawline(event) {
        //    var cX = event.clientX;
        //    var sX = event.screenX;
        //    var cY = event.clientY;
        //    var sY = event.screenY;
        //    var coords1 = "client - X: " + cX + ", Y coords: " + cY;
        //    var coords2 = "screen - X: " + sX + ", Y coords: " + sY;
        //    __doPostBack('DRAW', coords1);
        //}

        //function clearline(x) {
        //    __doPostBack('CLEAR', "#VALX, #VALY");
        //}
    </script>

</body>
</html>
