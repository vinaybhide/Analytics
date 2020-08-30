<%@ Page Title="ADX" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="adx.aspx.cs" Inherits="Analytics.adx" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--<div class="row">
        <asp:Panel ID="trid" runat="server" Width="100%">
            <div class="col-md-15">
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
            </div>
        </asp:Panel>

    </div>--%>

    <%--OnPreRender="chart_PreRender"--%>
    <asp:Chart ID="chartADX" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartADX_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation">
        <Legends>
            <asp:Legend Name="legendADX" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesADX" ChartType="Line" ChartArea="chartareaADX"
                XValueMember="Date" XValueType="Date" YValueMembers="ADX" YValueType="Double"
                PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; ADX:#VALY">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaADX">
                <Position Auto="false" X="0" Y="4" Height="96" Width="100" />
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
    <hr />
    <div>
        <asp:GridView ID="GridViewData" class="gridheader" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="ADX" DataField="ADX" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />

            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"></PagerSettings>
        </asp:GridView>
    </div>
</asp:Content>
