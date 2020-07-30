<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="portfolioValuation.aspx.cs" Inherits="Analytics.portfolioValuation" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
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
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%">
            <ContentTemplate>
                <h3 style="text-align:center;">Portfolio Valuation</h3>
                <asp:Chart ID="chartPortfolioValuation" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid">
                    <%--<Titles>
                        <asp:Title Text="Portfolio Valuation" Alignment="TopCenter" Font="Microsoft Sans Serif, 20pt"></asp:Title>
                    </Titles>--%>
                    <Legends>
                        <asp:Legend Name="legendValuation" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" BorderDashStyle="Dash" BorderColor="Black"></asp:Legend>
                    </Legends>
                    <%--<Legends>
                        <asp:Legend BorderDashStyle="Solid"
                    </Legends>--%>
                    <%--<Series>
                        <asp:Series Name="Series1" ChartType="Line">
                            <SmartLabelStyle AllowOutsidePlotArea="Yes" Enabled="true"/>
                        </asp:Series>
                    </Series>--%>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaPortfolioValuation" AlignmentOrientation="Vertical"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <div></div>
                <asp:GridView ID="gridviewPortfolioValuation" runat="server" Width="100%" AutoGenerateColumns="False" HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField HeaderText="Symbol" DataField="Symbol" />
                        <asp:BoundField HeaderText="Date" DataField="Date" />
                        <asp:BoundField HeaderText="Open" DataField="Open" />
                        <asp:BoundField HeaderText="High" DataField="High" />
                        <asp:BoundField HeaderText="Low" DataField="Low" />
                        <asp:BoundField HeaderText="Close" DataField="Close" />
                        <asp:BoundField HeaderText="Volume" DataField="Volume" />
                        <asp:BoundField HeaderText="Purchase Date" DataField="PurchaseDate" />
                        <asp:BoundField HeaderText="Cumulative Quantity" DataField="CumulativeQuantity" />
                        <asp:BoundField HeaderText="Cost of Investment" DataField="CostofInvestment" />
                        <asp:BoundField HeaderText="Value On Date" DataField="ValueOnDate" />
                    </Columns>

                    <HeaderStyle HorizontalAlign="Center" />

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
