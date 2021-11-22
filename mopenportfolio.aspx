<%@ Page Title="Open Portfolio" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="openportfolio.aspx.cs" Inherits="Analytics.openportfolio" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gridheader {
            text-align: center;
        }

        .FixedHeader {
            position: absolute;
            font-weight: normal;
        }

        .grid-sltrow {
            background: #d7d5d5;
            font-weight: bold;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            /*background-color: #D8D8D8;*/
            font-weight: bold;
        }

        .GrandTotalRowStyle {
            border: solid 1px Gray;
            background-color: #000000;
            color: #ffffff;
            font-weight: bold;
        }

        .GroupHeaderStyle {
            border: solid 1px Black;
            /*background-color: #4682B4;
            color: #ffffff;*/
            background-color: #e1b94a;
            color: #000000;
            font-weight: bold;
        }

        .serh-grid {
            width: 85%;
            border: 1px solid #6AB5FF;
            background: #fff;
            line-height: 14px;
            font-size: 11px;
            font-family: Verdana;
        }
    </style>
    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive;">
                <div class="container table-responsive" style="padding-top: 1%; text-align: center; position: fixed; background-color: #c2c2c2;">
                    <asp:Button ID="ButtonAddNew" runat="server" Text="Add New" OnClick="ButtonAddNew_Click" />
                    <asp:Button ID="ButtonEdit" runat="server" Text="Edit" OnClick="ButtonEdit_Click" />
                    <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete" OnClick="buttonDeleteSelectedScript_Click" />
                    <asp:Button ID="buttonGetQuote" runat="server" Text="Get Quote & Add" OnClick="buttonGetQuote_Click" />
                    <asp:Button ID="buttonValuation" runat="server" Text="Portfolio Valuation" OnClick="buttonValuation_Click" />
                    <div style="padding-top: 2px; padding-bottom: 2px;">
                        <%--<asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Selected Script:" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                        <asp:Label ID="lblScript" CssClass="text-left" runat="server" Text="Selected script name will appear here" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <%--<asp:Label ID="Label4" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>--%>
                        <br />
                        <asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Exchange:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblExchange" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="true"></asp:Label>

                        <br />
                        <asp:Label ID="Label3" CssClass="text-right" runat="server" Text="Purchase Date:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblDate" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div class="container">
                    <%--OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged"--%>
                    <%--CssClass="table table-striped table-bordered table-hover serh-grid"--%>
                    <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover serh-grid"
                        Width="100%" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
                        OnRowDataBound="grdViewOrders_RowDataBound"
                        OnRowCreated="grdViewOrders_RowCreated" OnRowCommand="grdViewOrders_RowCommand">
                        <Columns>
                            <%--<asp:BoundField DataField="CompanyName" HeaderText="Comp Name" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Name" HeaderText="Symbol" ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:BoundField DataField="PURCHASE_DATE" HeaderText="Txn Date" SortExpression="PURCHASE_DATE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PURCHASE_PRICE" HeaderText="Txn Price" SortExpression="PURCHASE_PRICE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PURCHASE_QTY" HeaderText="Txn Quantity" SortExpression="PURCHASE_QTY"
                                ItemStyle-HorizontalAlign="Center" />
                            <%--<asp:BoundField DataField="CommissionPaid" HeaderText="Commission" SortExpression="CommissionPaid"
                                ItemStyle-HorizontalAlign="Center" />--%>

                            <asp:TemplateField HeaderText="Commission+Taxes" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("COMMISSION_TAXES","{0}") != "0.00") ? Eval("COMMISSION_TAXES","{0:0.00}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:BoundField DataField="INVESTMENT_COST" HeaderText="Cost" SortExpression="INVESTMENT_COST"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CURRENTDATE" HeaderText="Quote Date" SortExpression="CURRENTDATE"
                                ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="CURRENTPRICE" HeaderText="Quote" SortExpression="CURRENTPRICE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CURRENTVALUE" HeaderText="Value Now" SortExpression="CURRENTVALUE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="YearsInvested" HeaderText="Years Invested" SortExpression="YearsInvested"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ARR" HeaderText="ARR" SortExpression="ARR"
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
        <asp:TreeView ID="TreeViewPortfolio" runat="server" NodeWrap="True" ExpandDepth="1" ShowLines="True"></asp:TreeView>
    </div>--%>
</asp:Content>
