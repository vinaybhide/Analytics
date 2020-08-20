<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adx.aspx.cs" Inherits="Analytics.adx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>ADX</title>
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

        .label {
            text-align: right;
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
                            <li>ADX: The Trend Strength Indicator. The average directional index (ADX) is used to determine when the price is trending strongly.</li>
                            <li>ADX values help traders identify the strongest and most profitable trends to trade. The values are also important 
                                for distinguishing between trending and non-trending conditions. Many traders will use ADX readings above 25 to 
                                suggest that the trend is strong enough for trend-trading strategies. Conversely, when ADX is below 25, many will 
                                avoid trend-trading strategies.
                                <ul>
                                    <li>0-25-->     Absent or Weak Trend</li>
                                    <li>25-50-->	Strong Trend</li>
                                    <li>50-75-->    Very Strong Trend</li>
                                    <li>75-100-->	Extremely Strong Trend</li>
                                </ul>
                            </li>
                            <li>The direction of the ADX line is important for reading trend strength. When the ADX line is rising, trend strength is 
                                increasing, and the price moves in the direction of the trend. When the line is falling, trend strength is decreasing, 
                                and the price enters a period of retracement or consolidation.
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
                <asp:Chart ID="chartADX" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    EnableViewState="True" OnClick="chartADX_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
                    <Series>
                        <asp:Series Name="seriesADX" ChartType="Line" ChartArea="chartareaADX"
                            XValueMember="Date" XValueType="Date" YValueMembers="ADX" YValueType="Double"
                            PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; ADX:#VALY">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaADX">
                            <Position Auto="false" X="0" Y="0" Height="100" Width="100" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="ADX" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap" 
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
                    <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="ADX" DataField="ADX" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
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
