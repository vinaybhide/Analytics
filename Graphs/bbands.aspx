<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bbands.aspx.cs" Inherits="Analytics.bbands" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Bollinger Bands</title>
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
                        <asp:Label ID="headingtext" runat="server" Text="Average directional movement index (ADX)"></asp:Label>
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
                            <li>A Bollinger Band is a technical analysis tool defined by a set of trendlines plotted two standard deviations 
                                (positively and negatively) away from a simple moving average (SMA) of a security's price, but which can be adjusted 
                                to user preferences.
                                <ul>
                                    <li>There are three lines that compose Bollinger Bands: A simple moving average (middle band) and an upper and lower band.</li>
                                    <li>The upper and lower bands are typically 2 standard deviations +/- from a 20-day simple moving average, but can be modified.</li>
                                </ul>
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
                <asp:Chart ID="chartBollingerBands" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True" OnClick="chartBollingerBands_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
                    <%--<Titles>
                        <asp:Title Name="titleBbands" Text="Bollinger Bands" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendBBands" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="Real Lower Band" Legend="legendBBands" LegendText="Lower Band" ChartType="Line" ChartArea="chartareaBollingerBands"
                            XValueMember="Date" XValueType="Date" YValueMembers="Real Lower Band" YValueType="Double"
                            PostBackValue="Real Lower Band,#VALX,#VALY" ToolTip="Date:#VALX; Lower Band:#VALY" LegendToolTip="Lower Band">
                        </asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="Real Middle Band" Legend="legendBBands" LegendText="Middle Band" ChartType="Line" ChartArea="chartareaBollingerBands"
                            XValueMember="Date" XValueType="Date" YValueMembers="Real Middle Band" YValueType="Double"
                            PostBackValue="Real Middle Band,#VALX,#VALY" ToolTip="Date:#VALX; Middle Band:#VALY" LegendToolTip="Middle Band">
                        </asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="Real Upper Band" Legend="legendBBands" LegendText="Upper Band" ChartType="Line" ChartArea="chartareaBollingerBands"
                            XValueMember="Date" XValueType="Date" YValueMembers="Real Upper Band" YValueType="Double"
                            PostBackValue="Real Upper Band,#VALX,#VALY" ToolTip="Date:#VALX; Upper Band:#VALY" LegendToolTip="Upper Band">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaBollingerBands" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="Bollinger Bands" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY>
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
                            <asp:BoundField HeaderText="Real Lower Band" DataField="Real Lower Band" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Real Middle Band" DataField="Real Middle Band" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Real Upper Band" DataField="Real Upper Band" ItemStyle-HorizontalAlign="Center">
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
