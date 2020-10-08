<%@ Page Title="STOCH" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="stoch.aspx.cs" Inherits="Analytics.stoch" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartSTOCH" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartSTOCH_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleSTOCH" Text="Stochastic Oscillator" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <Legends>
            <asp:Legend Name="legendSTOCH" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesSlowK" XAxisType="Secondary" YAxisType="Primary" Legend="legendSTOCH" LegendText="SlowK" ChartType="Line" ChartArea="chartareaSlowK"
                XValueMember="Date" XValueType="Date" YValueMembers="SlowK" YValueType="Double"
                PostBackValue="0,SlowK,#VALX,#VALY" ToolTip="Date:#VALX; SlowK:#VALY" LegendToolTip="SlowK line">
            </asp:Series>
        </Series>
        <Series>
            <asp:Series Name="seriesSlowD" XAxisType="Primary" YAxisType="Primary" Legend="legendSTOCH" LegendText="SlowD" ChartType="Line" ChartArea="chartareaSlowD"
                XValueMember="Date" XValueType="Date" YValueMembers="SlowD" YValueType="Double"
                PostBackValue="1,SlowD,#VALX,#VALY" ToolTip="Date:#VALX; SlowD:#VALY" LegendToolTip="SlowD line">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaSlowK" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="3" Height="50" Width="98" />
                <%--<AxisX>
                    <LabelStyle Enabled="false" />
                </AxisX>--%>
                <AxisX2 IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX2>

                <AxisY Title="SlowK" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
            <asp:ChartArea Name="chartareaSlowD" AlignWithChartArea="chartareaSlowK" AlignmentOrientation="Vertical"
                AlignmentStyle="PlotPosition">
                <Position Auto="false" X="0" Y="53" Height="47" Width="98" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="SlowD" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False" HorizontalAlign="Center" 
            AllowPaging="True" OnPageIndexChanging="GridViewData_PageIndexChanging"
            PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SlowK" DataField="SlowK" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SlowD" DataField="SlowD" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
</asp:Content>
