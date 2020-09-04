<%@ Page Title="Open Portfolio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="openportfolio.aspx.cs" Inherits="Analytics.openportfolio" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="./Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gridheader {
            text-align: center;
        }

        .FixedHeader {
            position: absolute;
            font-weight: normal;
        }
        .grid-sltrow
        {
            background: Gray; /*#ddd;*/
            font-weight: bold;
        }
        .SubTotalRowStyle
        {
            border: solid 1px Black;
            background-color: #D8D8D8;
            font-weight: bold;
        }
        .GrandTotalRowStyle
        {
            border: solid 1px Gray;
            background-color: #000000;
            color: #ffffff;
            font-weight: bold;
        }
        .GroupHeaderStyle
        {
            border: solid 1px Black;
            background-color: #4682B4;
            color: #ffffff;
            font-weight: bold;
        }
        .serh-grid
        {
            width: 85%;
            border: 1px solid #6AB5FF;
            background: #fff;
            line-height: 14px;
            font-size: 11px;
            font-family: Verdana;
        }

        }
    </style>
    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive;">
                <div style="padding-top: 1%; text-align: center; position: fixed;">
                    <asp:Button ID="ButtonAddNew" runat="server" Text="Add New Stock" OnClick="ButtonAddNew_Click" />
                    <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete Stock" OnClick="buttonDeleteSelectedScript_Click" />
                    <asp:Button ID="buttonGetQuote" runat="server" Text="Get Quote & Add" OnClick="buttonGetQuote_Click" />
                    <asp:Button ID="buttonValuation" runat="server" Text="Portfolio Valuation" OnClick="buttonValuation_Click" />
                    <asp:Label ID="Label2" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>
                    <asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Selected Script:" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblScript" CssClass="text-left" runat="server" Text="None" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:Label ID="Label4" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>
                    <asp:Label ID="Label3" CssClass="text-right" runat="server" Text="Purchase Date:" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblDate" CssClass="text-left" runat="server" Text="None" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                </div>
                <br />
                <br />

                <div>
                        
                    
                    <%--OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged"--%>
                    <%--CssClass="table table-striped table-bordered table-hover serh-grid"--%>
                    <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover serh-grid"
                        Width="100%" ShowHeaderWhenEmpty="True" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        OnRowDataBound="grdViewOrders_RowDataBound" 
                        OnRowCreated="grdViewOrders_RowCreated" OnRowCommand="grdViewOrders_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CompanyName" HeaderText="Comp Name" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Name" HeaderText="Symbol" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PurchaseDate" HeaderText="Txn Date" SortExpression="PurchaseDate"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PurchasePrice" HeaderText="Txn Price" SortExpression="PurchasePrice"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PurchaseQty" HeaderText="Txn Quantity" SortExpression="PurchaseQty"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CommissionPaid" HeaderText="Commission" SortExpression="CommissionPaid"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CostofInvestment" HeaderText="Txn Cost" SortExpression="CostofInvestment"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Price" HeaderText="Close" SortExpression="Price"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CurrentValue" HeaderText="Value at Close" SortExpression="CurrentValue"
                                ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <%--<SelectedRowStyle BackColor="#CCCCCC" />--%>
                        <SelectedRowStyle CssClass="grid-sltrow" />
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderStyle="Solid"
                            BorderWidth="1px" BorderColor="Black" />
                        
                    </asp:GridView>
                </div>
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
