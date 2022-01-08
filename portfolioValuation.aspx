<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="portfolioValuation.aspx.cs" Inherits="Analytics.portfolioValuation" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Portfolio Valuation</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <%--<script src="http://code.jquery.com/jquery-1.8.2.js"></script> --%>

    <style>
        #Background {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            background-color: Gray;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        #Progress {
            position: fixed;
            top: 50%;
            left: 0%;
            width: 100%;
            height: 100%;
            text-align: center;
            /*background-color: White;
            border: solid 3px black;*/
        }

        html, body, form {
            height: 100%;
        }

        .chart {
            width: 100% !important;
            height: 100% !important;
        }
        /*.wait, .wait * { cursor: wait !important; }*/
        /*#loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('/images/pageloader.gif') 50% 50% no-repeat rgb(249,249,249);
        }*/
    </style>
</head>
<body onbeforeunload="doHourglass();" onunload="resetCursor();">
    <form id="form1" runat="server" style="font-size: small;">
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%">
            <ContentTemplate>
                <%--<div id="loader"></div> --%>
                <div style="width: 100%; border: groove;">
                    <%--<h3 id="headingtext" runat="server" style="text-align: center">Portfolio Valuation</h3>--%>
                    <table style="width: 100%">
                        <tr>
                            <td colspan="5" style="text-align: center; font-size: medium; width: 100%;">
                                <asp:Label ID="headingtext" runat="server" Text="Portfolio Valuation"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="text-align: right; width: 10%;">
                                <asp:Label ID="Label2" runat="server" Text="From date:"></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxFromDate" runat="server" TextMode="Date" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="text-align: right; width: 5%;">
                                <asp:Label ID="Label3" runat="server" Text="To date:"></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxToDate" runat="server" TextMode="Date" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td colspan="2">
                                <asp:ListBox ID="listboxScripts" Width="100%" SelectionMode="Multiple" runat="server" TabIndex="3"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Label1" runat="server" Text="Select Index to Show:"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlIndex" runat="server">
                                    <asp:ListItem Text="Select Index" Value="HideIndex" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="BSE SENSEX" Value="^BSESN"></asp:ListItem>
                                    <asp:ListItem Text="Nifty 50" Value="^NSEI"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td>
                                <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph" OnClick="buttonShowGraph_Click" TabIndex="4" />
                            </td>
                            <td>
                                <asp:Button ID="buttonShowGrid" runat="server" Text="Show Raw Data" TabIndex="5" OnClick="buttonShowGrid_Click" />

                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>

                <asp:Chart ID="chartPortfolioValuation" runat="server" CssClass="auto-style1" Visible="false" BorderlineColor="Black"
                    BorderlineDashStyle="Solid" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
                    EnableViewState="True" OnClick="chartPortfolioValuation_Click"
                    OnPreRender="chart_PreRender">
                    <Legends>
                        <asp:Legend Name="legendValuation" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Near" LegendStyle="Row" IsTextAutoFit="true" AutoFitMinFontSize="5"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false">
                            <Position X="0" Y="0" Height="5" Width="100" Auto="false" />
                        </asp:Legend>
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
                        <asp:ChartArea Name="chartareaPortfolioValuation" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="10" Height="90" Width="95" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="Portfolio Valuation" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                                TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <asp:GridView ID="gridviewPortfolioValuation" Visible="false" runat="server" Width="100%" Height="50%" AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewPortfolioValuation_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Symbol" DataField="SYMBOL" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Date" DataField="TIMESTAMP" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Open" DataField="OPEN" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="High" DataField="HIGH" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Low" DataField="LOW" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Close" DataField="CLOSE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Volume" DataField="VOLUME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Purchase Date" DataField="PURCHASE_DATE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cumulative Quantity" DataField="CumulativeQty" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cost of Investment" DataField="CumulativeCost" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Value On Date" DataField="CumulativeValue" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>

                        <HeaderStyle HorizontalAlign="Center" />

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" Position="TopAndBottom" />

                    </asp:GridView>
                </div>

                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div id="Background"></div>
                        <div id="Progress">
                            <img src="WaitImage/pageloader.gif" width="100" height="100" style="vertical-align: central" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
    <script type="text/javascript">
        //$(window).load(function () {
        //    $("#loader").fadeOut(1000);
        //});

        function doHourglass() {
            document.body.style.cursor = 'wait';
        };

        function resetCursor() {
            document.body.style.cursor = 'default';
        };

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
                //if (isFirstDisplay || widthChange > 0.2 || heightChange > 0.2) {
                //    redrawChart();
                //}
                if (isFirstDisplay || widthChange > 0 || heightChange > 0) {
                    redrawChart();
                }
            };

            savePanelSize();
            window.addEventListener('resize', savePanelSize, false);
            //body.addEventListener('onbeforeunload', doHourglass, false);
            //body.addEventListener('onunload', resetCursor, false);
        })();
    </script>
</body>
</html>
