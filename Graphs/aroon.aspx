<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aroon.aspx.cs" Inherits="Analytics.aroon" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>AROON</title>
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
                        <asp:Label ID="headingtext" runat="server" Text="AROON"></asp:Label>
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
                            <li>The Aroon indicator is a technical indicator that is used to identify trend changes in the price of an asset, 
                                as well as the strength of that trend.</li>
                            <li>In essence, the indicator measures the time between highs and the time between lows over a time period. 
                                The idea is that strong uptrends will regularly see new highs, and strong downtrends will regularly see new lows. 
                                The indicator signals when this is happening, and when it isn't.
                            </li>
                            <li>The indicator consists of the "Aroon up" line, which measures the strength of the uptrend, and the "Aroon down" line, 
                                which measures the strength of the downtrend.
                            <ul>
                                <li>The Arron indicator is composed of two lines. An up line which measures the number of periods since a High, 
                                    and a down line which measures the number of periods since a Low.</li>
                                <li>When the Aroon Up is above the Aroon Down, it indicates bullish price behavior.</li>
                                <li>When the Aroon Down is above the Aroon Up, it signals bearish price behavior.</li>
                                <li>Crossovers of the two lines can signal trend changes. For example, when Aroon Up crosses above Aroon Down it 
                                    may mean a new uptrend is starting.</li>
                                <li>The indicator moves between zero and 100. A reading above 50 means that a high/low (whichever line is above 50) 
                                    was seen within the last 12 periods.</li>
                                <li>A reading below 50 means that the high/low was seen within the 13 periods.</li>
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
                <asp:Chart ID="chartAROON" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True"
                    OnClick="chartAROON_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
                    <%--<Titles>
                        <asp:Title Name="titleAROON" Text="AROON" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendAROON" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <Series>
                        <asp:Series Name="seriesAROON_Down" Legend="legendAROON" LegendText="AROON Down" ChartType="Line" ChartArea="chartareaAROON"
                            XValueMember="Date" XValueType="Date" YValueMembers="Aroon Down" YValueType="Double"
                            PostBackValue="AROON Down,#VALX,#VALY" ToolTip="Date:#VALX; AROON Down:#VALY" LegendToolTip="ARRON Down">
                        </asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="seriesAROON_Up" Legend="legendAROON" LegendText="AROON Up" ChartType="Line" ChartArea="chartareaAROON"
                            XValueMember="Date" XValueType="Date" YValueMembers="Aroon Up" YValueType="Double"
                            PostBackValue="AROON Up,#VALX,#VALY" ToolTip="Date:#VALX; AROON Up:#VALY" LegendToolTip="ARRON Up">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaAROON" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="AROON" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Aroon Down" DataField="Aroon Down" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Aroon Up" DataField="Aroon Up" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" />
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
