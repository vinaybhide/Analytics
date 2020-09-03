<%@ Page Title="RSI" Language="C#" MasterPageFile="~/graphs/standardgraphs.Master" AutoEventWireup="true" CodeBehind="rsi.aspx.cs" Inherits="Analytics.rsi" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <%--<li>The relative strength index (RSI) is a momentum indicator used in technical analysis that measures the magnitude of 
                                recent price changes to evaluate overbought or oversold conditions in the price of a stock or other asset. </li>
                            <li>Traditional interpretation and usage of the RSI are that values of 70 or above indicate that a security is becoming 
                                overbought or overvalued and may be primed for a trend reversal or corrective pullback in price. An RSI reading of 30 
                                or below indicates an oversold or undervalued condition.--%>
    <asp:Chart ID="chartRSI" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        EnableViewState="True" OnClick="chartRSI_Click" ImageType="Png" ImageLocation="~/chartimg/" ImageStorageMode="UseImageLocation"
        OnPreRender="chart_PreRender">
        <Series>
            <asp:Series Name="seriesRSI" ChartType="Line" ChartArea="chartareaRSI"
                XValueMember="Date" XValueType="Date" YValueMembers="RSI" YValueType="Double"
                PostBackValue="#VALX,#VALY" ToolTip="Date:#VALX; RSI:#VALY">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="chartareaRSI" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="0" Height="100" Width="100" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 8pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY Title="RSI" TitleAlignment="Center" IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <hr />
    <div>
        <asp:GridView ID="GridViewData" Visible="false" runat="server" Width="50%" Height="50%" AutoGenerateColumns="False"
            HorizontalAlign="Center" AllowPaging="True" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging"
            PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="RSI" DataField="RSI" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" />
        </asp:GridView>
    </div>
</asp:Content>
