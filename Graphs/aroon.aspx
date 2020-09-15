<%@ Page Title="AROON" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="aroon.aspx.cs" Inherits="Analytics.aroon" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartAROON" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True"
        OnClick="chartAROON_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
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
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                    <StripLines>
                        <asp:StripLine StripWidth="0" BorderColor="Red" BorderWidth="2" BorderDashStyle="Dot" IntervalOffset="50"
                            BackColor="RosyBrown" BackSecondaryColor="Purple" BackGradientStyle="LeftRight" Text="50" TextAlignment="Near" />
                    </StripLines>

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
</asp:Content>
