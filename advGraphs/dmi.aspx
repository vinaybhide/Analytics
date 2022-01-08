<%@ Page Title="Price direction & strength" Language="C#" MasterPageFile="~/advGraphs/complexgraphs.Master" AutoEventWireup="true" CodeBehind="dmi.aspx.cs" Inherits="Analytics.dmi" %>

<%@ MasterType VirtualPath="~/advGraphs/complexgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Panel ID="panelParam" runat="server" Visible="true">
        <div style="width: 100%; border: groove;">
            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label1" runat="server" Text="Output size: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Outputsize" runat="server" TabIndex="26">
                            <asp:ListItem Value="Full" Selected="True">Full</asp:ListItem>
                            <asp:ListItem Value="Compact">Compact</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>

            <table style="width: 100%">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label2" runat="server" Text="Interval: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Interval" runat="server">
                            <asp:ListItem Value="1d" Selected="True">Daily</asp:ListItem>
                            <asp:ListItem Value="1m" Enabled="false">1 min</asp:ListItem>
                            <asp:ListItem Value="5m" Enabled="false">5 min</asp:ListItem>
                            <asp:ListItem Value="15m" Enabled="false">15 min</asp:ListItem>
                            <asp:ListItem Value="30m" Enabled="false">30 min</asp:ListItem>
                            <asp:ListItem Value="60m" Enabled="false">60 min</asp:ListItem>
                            <asp:ListItem Value="1w" Enabled="false">Weekly</asp:ListItem>
                            <asp:ListItem Value="1m" Enabled="false">Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label5" runat="server" Text="Period: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxPeriod" runat="server" TextMode="Number" Text="14" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr style="width: 100%;">
                    <td style="width: 20%;"></td>
                    <td style="text-align: right; width: 10%;">
                        <asp:Label ID="Label6" runat="server" Text="Series Type: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_SeriesType" runat="server" TabIndex="29">
                            <asp:ListItem Value="OPEN" Enabled="false">Open</asp:ListItem>
                            <asp:ListItem Value="HIGH" Enabled="false">High</asp:ListItem>
                            <asp:ListItem Value="LOW" Enabled="false">Low</asp:ListItem>
                            <asp:ListItem Value="CLOSE" Selected="True">Close</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <asp:Chart ID="chartAdvGraph" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartAdvGraph_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendAdvGraph" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Open" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Open" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Open" YValueType="Double"
                PostBackValue="Open,#VALX,#VALY" ToolTip="Date:#VALX; Open:#VALY" LegendToolTip="Open">
            </asp:Series>
            <asp:Series Name="High" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="High" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="High" YValueType="Double"
                PostBackValue="High,#VALX,#VALY" ToolTip="Date:#VALX; High:#VALY" LegendToolTip="High">
            </asp:Series>
            <asp:Series Name="Low" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Low" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Low" YValueType="Double"
                PostBackValue="Low,#VALX,#VALY" ToolTip="Date:#VALX; Low:#VALY" LegendToolTip="Low">
            </asp:Series>
            <asp:Series Name="Close" XAxisType="Secondary" YAxisType="Primary" ChartType="Line" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="Close" XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="Close" YValueType="Double"
                PostBackValue="Close,#VALX,#VALY" ToolTip="Date:#VALX; Close:#VALY" LegendToolTip="Close">
            </asp:Series>
            <asp:Series Name="OHLC" XAxisType="Secondary" YAxisType="Primary" ChartType="Candlestick" ChartArea="chartarea1"
                Legend="legendAdvGraph" LegendText="OHLC"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="High,Low,Open,Close" YValueType="Double"
                PostBackValue="OHLC,#VALX,#VALY1,#VALY2,#VALY3,#VALY4"
                ToolTip="Date:#VALX; High:#VALY1; Low:#VALY2; Open:#VALY3; Close:#VALY4"
                BorderColor="Black" Color="Black"
                CustomProperties="PriceDownColor=Blue, ShowOpenClose=Both, PriceUpColor=Red, OpenCloseStyle=Triangle" LegendToolTip="OHLC">
            </asp:Series>
            <asp:Series Name="MINUS_DM" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="-DMI"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="MINUS_DM" YValueType="Double"
                PostBackValue="MINUS_DM,#VALX,#VALY" ToolTip="Date:#VALX; MINUS_DM:#VALY" LegendToolTip="-DMI">
            </asp:Series>
            <asp:Series Name="PLUS_DM" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea2"
                Legend="legendAdvGraph" LegendText="+DMI"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="PLUS_DM" YValueType="Double"
                PostBackValue="PLUS_DM,#VALX,#VALY" ToolTip="Date:#VALX; PLUS_DM:#VALY" LegendToolTip="+DMI">
            </asp:Series>
            <asp:Series Name="DX" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea3"
                Legend="legendAdvGraph" LegendText="DX"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="DX" YValueType="Double"
                PostBackValue="DX,#VALX,#VALY" ToolTip="Date:#VALX; DX:#VALY" LegendToolTip="Directional Movement">
            </asp:Series>
            <asp:Series Name="ADX" YAxisType="Primary" XAxisType="Primary" ChartType="Line" ChartArea="chartarea3" Legend="legendAdvGraph" LegendText="ADX"
                XValueMember="TIMESTAMP" XValueType="Date" YValueMembers="ADX" YValueType="Double"
                PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; ADX:#VALY">
            </asp:Series>

        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="3" Height="30" Width="99" />
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="true" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>
                <AxisY Title="Daily Open/High/Low/close" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true"
                    LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea2" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="33" Height="30" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Enabled="false" Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="DMI" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="true">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="25"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="25 Level" TextAlignment="Near" />
                    </StripLines>
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartarea3" AlignWithChartArea="chartarea1" AlignmentOrientation="Vertical" AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="66" Height="30" Width="99" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="ADX" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="25"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="25 Level" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="50 Level" TextAlignment="Near" />
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="75"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="75 Level" TextAlignment="Near" />
                    </StripLines>
                </AxisY>
            </asp:ChartArea>

        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewData" class="gridheader" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False"
            HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
            PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging"
            PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
            <Columns>
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
                <asp:BoundField HeaderText="ADX" DataField="ADX" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="DX" DataField="DX" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="+DM" DataField="PLUS_DM" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="-DM" DataField="MINUS_DM" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="+DI" DataField="PLUS_DI" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="-DI" DataField="MINUS_DI" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />

            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"></PagerSettings>
        </asp:GridView>
    </div>
</asp:Content>
