<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="mfvaluationbar.aspx.cs" Inherits="Analytics.advGraphs.mfvaluationbar" %>

<%@ MasterType VirtualPath="~/advGraphs/advancegraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartPortfolioValuation" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartPortfolioValuation_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseHttpHandler"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendValuation" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row" 
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Cost" ChartType="Column" ChartArea="chartareaPortfolioValuation" Legend="legendValuation" LegendText="Cost"
                LegendToolTip="Shows total Cost" IsValueShownAsLabel="true" ToolTip="#VALX; Total Cost:#VALY (Click to see details)"
                XValueMember="FundName" YValuesPerPoint="10" 
                YValueMembers="CumulativeCost,FundHouse,SCHEME_CODE,FirstPurchaseDate,CumulativeUnits,CurrentNAV,NAVDate,CumulativeValue,TotalYearsInvested,TotalARR"
                PostBackValue="Cost,#VALX,#VALY,#VALY2,#VALY3,#VALY4,#VALY5,#VALY6,#VALY7,#VALY8,#VALY9,#VALY10">
            </asp:Series>

            <asp:Series Name="Value" ChartType="Column" ChartArea="chartareaPortfolioValuation" Legend="legendValuation" LegendText="Value"
                LegendToolTip="Shows total Value" IsValueShownAsLabel="true" ToolTip="#VALX; Total Value:#VALY (Click to see details)"
                XValueMember="FundName" YValuesPerPoint="10" 
                YValueMembers="CumulativeValue,FundHouse,SCHEME_CODE,FirstPurchaseDate,CumulativeUnits,CumulativeCost,CurrentNAV,NAVDate,TotalYearsInvested,TotalARR"
                PostBackValue="Value,#VALX,#VALY,#VALY2,#VALY3,#VALY4,#VALY5,#VALY6,#VALY7,#VALY8,#VALY9,#VALY10">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaPortfolioValuation" AlignmentOrientation="Vertical">
                            <Position Auto="false" X="0" Y="10" Height="90" Width="95" />

                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="Portfolio Valuation" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="true">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <table style="width: 100%; font-size: small; text-align: center;">
            <tr>
                <td style="width: 100%; text-align: center;">

                    <asp:GridView ID="gridviewPortfolioValuation" Enabled="false" Visible="false" runat="server" Width="100%" Height="50%" 
                        AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewPortfolioValuation_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Fund House" DataField="FundHouse" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Scheme Code" DataField="SCHEME_CODE" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Fund Name" DataField="FundName" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="First Purchase Date" DataField="FirstPurchaseDate" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cum Units" DataField="CumulativeUnits" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cum Cost" DataField="CumulativeCost" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="NAV Date" DataField="NAVDate" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="NAV" DataField="CurrentNAV" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cum Value" DataField="CumulativeValue" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Years Invested" DataField="TotalYearsInvested" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ARR" DataField="TotalARR" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
