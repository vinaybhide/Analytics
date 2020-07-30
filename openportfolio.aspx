<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="openportfolio.aspx.cs" Inherits="Analytics.openportfolio" %>

<%@ MasterType VirtualPath="./Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gridheader {
            text-align: center;
        }
    </style>
    <div>
        <asp:Button ID="ButtonAddNew" runat="server" Text="Add New Stock" OnClick="ButtonAddNew_Click" />
        <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete Stock" OnClick="buttonDeleteSelectedScript_Click" />
        <asp:Button ID="buttonGetQuote" runat="server" Text="Get Quote & Add" OnClick="buttonGetQuote_Click" />
        <asp:Button ID="buttonValuation" runat="server" Text="Portfolio Valuation" OnClick="buttonValuation_Click" />
    </div>
    <div class="row" >
        <div class="col-lg-12 ">
            <div class="table-responsive">
                <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged" Width="100%" ShowHeaderWhenEmpty="True"
                    CssClass="table table-striped table-bordered table-hover">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Script Name" HeaderStyle-CssClass="gridheader" />
                        <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" SortExpression="PurchaseDate"
                            HeaderStyle-CssClass="gridheader"/>
                        <asp:BoundField DataField="PurchasePrice" HeaderText="Purchase Price" SortExpression="PurchasePrice"
                            HeaderStyle-CssClass="gridheader"/>
                        <asp:BoundField DataField="PurchaseQty" HeaderText="Purchase Quantity" SortExpression="PurchaseQty"
                            HeaderStyle-CssClass="gridheader" />
                        <asp:BoundField DataField="CommissionPaid" HeaderText="Commission Paid" SortExpression="CommissionPaid"
                            HeaderStyle-CssClass="gridheader" />
                        <asp:BoundField DataField="CostofInvestment" HeaderText="Cost of Investment" SortExpression="CostofInvestment"
                            HeaderStyle-CssClass="gridheader" />
                        <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price"
                            HeaderStyle-CssClass="gridheader" />
                        <asp:BoundField DataField="CurrentValue" HeaderText="Value at Close" SortExpression="CurrentValue"
                            HeaderStyle-CssClass="gridheader" />
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                    <RowStyle HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#CCCCCC" />

                </asp:GridView>
            </div>
        </div>
    </div>
    <%--<div>
        <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged" Width="100%" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Script Name" HeaderStyle-CssClass="gridheader" />
                <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" SortExpression="PurchaseDate"
                    HeaderStyle-CssClass="gridheader visible-lg" ItemStyle-CssClass="visible-lg" />
                <asp:BoundField DataField="PurchasePrice" HeaderText="Purchase Price" SortExpression="PurchasePrice"
                    HeaderStyle-CssClass="gridheader visible-xs" ItemStyle-CssClass="visible-xs" />
                <asp:BoundField DataField="PurchaseQty" HeaderText="Purchase Quantity" SortExpression="PurchaseQty"
                    HeaderStyle-CssClass="gridheader visible-md" ItemStyle-CssClass="visible-md" />
                <asp:BoundField DataField="CommissionPaid" HeaderText="Commission Paid" SortExpression="CommissionPaid"
                    HeaderStyle-CssClass="gridheader hidden-xs" ItemStyle-CssClass="hidden-xs" />
                <asp:BoundField DataField="CostofInvestment" HeaderText="Cost of Investment" SortExpression="CostofInvestment"
                    HeaderStyle-CssClass="gridheader visible-xs" ItemStyle-CssClass="visible-xs" />
                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price"
                    HeaderStyle-CssClass="gridheader hidden-xs" ItemStyle-CssClass="hidden-xs" />
                <asp:BoundField DataField="CurrentValue" HeaderText="Value at Close" SortExpression="CurrentValue"
                    HeaderStyle-CssClass="gridheader visible-xs" ItemStyle-CssClass="visible-xs" />
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
            <RowStyle HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
    </div>--%>
    <div>
        <asp:TreeView ID="TreeViewPortfolio" runat="server" ExpandDepth="1" NodeWrap="True" ShowLines="True"></asp:TreeView>
    </div>
</asp:Content>
