<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="indices.aspx.cs" Inherits="Analytics.indices" %>

<%@ MasterType VirtualPath="~/Graphs/standardgraphs.Master" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

</head>





<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGraphs" runat="server">
    <asp:Chart ID="chartdailyIndices" runat="server" CssClass="chart" Visible="false" BorderlineColor="Black" BorderlineDashStyle="Solid"
        ImageType="Png" ImageLocation="~/chartimg/ChartPic_#SEQ(300,3)" ImageStorageMode="UseImageLocation" EnableViewState="True" OnClick="chartdailyIndices_Click"
        OnPreRender="chart_PreRender">
        <Legends>
            <asp:Legend Name="legendIndices" LegendItemOrder="SameAsSeriesOrder" Docking="Top" Alignment="Center" LegendStyle="Row"
                BorderDashStyle="Dash" BorderColor="Black" DockedToChartArea="NotSet" IsDockedInsideChartArea="false" Font="Microsoft Sans Serif, 8pt">
                <Position X="0" Y="0" Height="10" Width="100" Auto="false" />
            </asp:Legend>
        </Legends>
        <ChartAreas>
            <asp:ChartArea Name="chartareaIndices" AlignmentOrientation="Vertical">
                <Position Auto="false" X="0" Y="10" Height="90" Width="98" />
                <AxisX IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="LabelsAngleStep90" TitleFont="Microsoft Sans Serif, 5pt">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" IsEndLabelVisible="true" />
                </AxisX>
                <AxisY IsMarginVisible="false" IsLabelAutoFit="true" LabelAutoFitStyle="WordWrap"
                    TitleFont="Microsoft Sans Serif, 8pt" IsStartedFromZero="false">
                    <LabelStyle Font="Microsoft Sans Serif, 5pt" />
                </AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
</asp:Content>
