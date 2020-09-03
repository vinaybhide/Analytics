<%@ Page Title="VWAP" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="vwaprice.aspx.cs" Inherits="Analytics.vwaprice" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--The volume weighted average price (VWAP) is a trading benchmark used by traders that gives the average price a security 
                                has traded at throughout the day, based on both volume and price.</li>
                            <li>It is important because it provides traders with insight into both the trend and value of a security.</li>--%>
    <asp:Chart ID="chartVWAP" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True"
        OnClick="chartVWAP_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <%--onmouseover="drawline(this)" onmouseout="clearline(this)"--%>
        <%--<Titles>
                        <asp:Title Name="titleVWAP" Text="Volume Weighted Average Price" Alignment="TopCenter" Font="Microsoft Sans Serif, 10pt"></asp:Title>
                    </Titles>--%>
        <%--<Legends>
                        <asp:Legend Name="legendVWAP" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" BorderDashStyle="Dash" BorderColor="Black"></asp:Legend>
                    </Legends>--%>
        <Series>
            <asp:Series Name="seriesVWAP" ChartType="Line" ChartArea="chartareaVWAP"
                XValueMember="Date" XValueType="DateTime" YValueMembers="VWAP" YValueType="Double"
                PostBackValue="#VALX{g},#VALY" ToolTip="Date:#VALX{g}; VWAP:#VALY">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaVWAP" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="0" Height="100" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Format="g" Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="VWAP" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
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
                <asp:BoundField HeaderText="VWAP" DataField="VWAP" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
</asp:Content>
