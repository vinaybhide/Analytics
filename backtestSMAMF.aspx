<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="backtestSMAMF.aspx.cs" Inherits="Analytics.backtestSMAMF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>MF Back Test using SMA</title>
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
                                <asp:Label ID="headingtext" runat="server" Text="MF Back Test using SMA"></asp:Label>
                                <br />
                                <asp:Label ID="Label8" runat="server" Text="Back test example: Go long on 100 stocks (i.e. buy 100 stocks), when the short term moving average crosses above the long term moving average. This is known as golden cross.
Sell the stock a few days later. For instance, we will keep the stock 20 days and then sell them.
Compute the profit"></asp:Label>
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
                            <td style="text-align: right; width: 5%;">
                                <asp:Label ID="Label4" runat="server" Text="SMA Small Period: "></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxSMASmallPeriod" runat="server" TextMode="Number" Text="10" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="text-align: right; width: 5%;">
                                <asp:Label ID="Label5" runat="server" Text="SMA Long Period: "></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxSMALongPeriod" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="text-align: right; width: 5%;">
                                <asp:Label ID="Label6" runat="server" Text="Buy span in days: "></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxBuySpan" runat="server" TextMode="Number" Text="2" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="text-align: right; width: 5%;">
                                <asp:Label ID="Label7" runat="server" Text="Sell span in days: "></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxSellSpan" runat="server" TextMode="Number" Text="20" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="text-align: right; width: 5%;">
                                <asp:Label ID="Label9" runat="server" Text="Simulation Quantity: "></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:TextBox ID="textboxSimulationQty" runat="server" TextMode="Number" Text="100" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 20%;"></td>
                            <td style="width: 5%; text-align: right;">
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
                            <td style="width: 5%; text-align: right;">
                                <asp:Label ID="Label10" runat="server" Text="Portfolio:"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlShowHidePortfolio" runat="server">
                                    <asp:ListItem Text="Show Portfolio Purchase" Selected="True" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Hide Portfolio Purchase" Value="1"></asp:ListItem>
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

                <asp:Chart ID="chartBackTestMF" runat="server" CssClass="auto-style1" Visible="false" BorderlineColor="Black"
                    BorderlineDashStyle="Solid" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
                    EnableViewState="True" OnClick="chartBackTestMF_Click"
                    OnPreRender="chart_PreRender">
                    <Legends>
                        <asp:Legend Name="legendBackTestMF" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Near" LegendStyle="Row" IsTextAutoFit="true" AutoFitMinFontSize="15"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false">
                            <Position X="0" Y="0" Height="5" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaBackTestMF" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="10" Height="90" Width="95" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                                <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY Title="NAV Values(Daily-SMA-Purchase)" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                                TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                                <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <hr />
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:GridView ID="gridviewBackTestMF" Visible="false" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                    OnPageIndexChanging="gridviewBackTestMF_PageIndexChanging"
                                    PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" Caption="Daily NAV with SMA" CaptionAlign="Top">
                                    <Columns>
                                        <%--<asp:BoundField HeaderText="Fund House" DataField="MF_COMP_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                                        <asp:BoundField HeaderText="SCHEME CODE" DataField="SCHEMECODE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SCHEME NAME" DataField="SCHEMENAME" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="NAV" DataField="NET_ASSET_VALUE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="NAV Date" DataField="NAVDATE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SMA SMALL" DataField="SMA_SMALL" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SMA LONG" DataField="SMA_LONG" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="CROSSOVER FLAG" DataField="CROSSOVER_FLAG" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="BUY FLAG" DataField="BUY_FLAG" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SELL FLAG" DataField="SELL_FLAG" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SIMULATION QTY" DataField="QUANTITY" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="BUY COST" DataField="BUY_COST" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SELL VALUE" DataField="SELL_VALUE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="PROFIT_LOSS" DataField="PROFIT_LOSS" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="RESULT" DataField="RESULT" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>

                                    <%--<HeaderStyle HorizontalAlign="Center" />--%>

                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast" Position="TopAndBottom" NextPageText=" &gt;" PreviousPageText=" &lt;" />

                                </asp:GridView>
                            </td>
                            <td>
                                <asp:GridView ID="gridviewPortfolioValuation" Visible="false" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                    OnPageIndexChanging="gridviewPortfolioValuation_PageIndexChanging"
                                    PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True" Caption="Valuation" CaptionAlign="Top">
                                    <Columns>
                                        <%--<asp:BoundField HeaderText="Fund House" DataField="MF_COMP_NAME" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                                        <asp:BoundField HeaderText="SCHEME CODE" DataField="SCHEME_CODE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="SCHEME NAME" DataField="SCHEME_NAME" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="NAV" DataField="NET_ASSET_VALUE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="NAV Date" DataField="DATE" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Purchase Date" DataField="PurchaseDate" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Purchase NAV" DataField="PurchaseNAV" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Purchase Units" DataField="PurchaseUnits" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Purchase Cost" DataField="ValueAtCost" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Cumulative Units" DataField="CumulativeUnits" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Cumulative Cost" DataField="CumulativeCost" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Cumulative Value" DataField="CurrentValue" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>

                                    <%--<HeaderStyle HorizontalAlign="Center" />--%>

                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast" Position="TopAndBottom" />

                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
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
