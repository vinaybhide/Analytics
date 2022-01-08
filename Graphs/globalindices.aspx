<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="globalindices.aspx.cs" Inherits="Analytics.globalindices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Global Indices</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
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
    </style>

</head>
<body onbeforeunload="doHourglass();" onunload="resetCursor();">
    <form id="form1" runat="server" style="font-size: small;">
        <asp:ScriptManager ID="scriptManager1" runat="server" />
        <asp:HiddenField ID="panelWidth" runat="server" Value="" />
        <asp:HiddenField ID="panelHeight" runat="server" Value="" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 100%; height: 100%">
            <ContentTemplate>
                <div style="width: 100%; border: groove;">
                    <table style="width: 100%">
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
                                <asp:ListBox ID="checkboxlistLines" Width="100%" SelectionMode="Multiple" runat="server" TabIndex="3"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;"></td>
                            <td>
                                <asp:Button ID="buttonShowGraph" runat="server" Text="Reset Graph" TabIndex="4" OnClick="buttonShowGraph_Click" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <asp:Chart ID="chartdailyIndices" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
                    ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation" EnableViewState="True" OnClick="chartdailyIndices_Click"
                    OnPreRender="chart_PreRender">
                    <Legends>
                        <asp:Legend Name="legendIndices" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                            BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                            <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
                        </asp:Legend>
                    </Legends>
                    <ChartAreas>
                        <asp:ChartArea Name="chartareaIndices" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="3" Height="97" Width="99" />
                            <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" 
                                TitleFont="Microsoft Sans Serif, 5pt" IsStartedFromZero="false">
                                <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                            </AxisX>
                            <AxisY IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                                TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                                <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:Button ID="btnPostBack" runat="server" Style="display: none" />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div id="Background"></div>
                        <div id="Progress">
                            <img src="../WaitImage/pageloader.gif" width="100" height="100" style="vertical-align: central" />
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
