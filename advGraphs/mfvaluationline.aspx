<%@ Page Title="" Language="C#" MasterPageFile="~/advGraphs/advancegraphs.Master" AutoEventWireup="true" CodeBehind="mfvaluationline.aspx.cs" Inherits="Analytics.advGraphs.mfvaluationline" %>

<%@ MasterType VirtualPath="~/advGraphs/advancegraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartPortfolioValuation" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartPortfolioValuation_Click" ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendValuation" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="3" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <ChartAreas>
            <asp:ChartArea Name="chartareaPortfolioValuation" AlignmentOrientation="Vertical">
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 8pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="MF Portfolio Valuation" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="DecreaseFont"
                    TitleFont="Microsoft Sans Serif, 8pt">
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
                    <asp:GridView ID="gridviewPortfolioValuation" Enabled="false" Visible="false" runat="server" Width="100%" Height="50%" AutoGenerateColumns="False" AllowPaging="True"
                        HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gridviewPortfolioValuation_PageIndexChanging"
                        PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
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

                        <HeaderStyle HorizontalAlign="Center" />

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
