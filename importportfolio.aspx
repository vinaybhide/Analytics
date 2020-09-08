<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="importportfolio.aspx.cs" Inherits="Analytics.importportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .content {
            width: 100%;
            min-width: 50%;
        }
    </style>

    <%--<h3 style="text-align: center; margin-top: 1%;">Download data for off-line mode</h3>--%>
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
        <tr>
            <td colspan="3" style="text-align: center; border: solid; border-width: 1px; border-style: solid;">
                <asp:Label ID="Label48" runat="server" Text="Import Portfolio"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>

        <tr>
            <td style="text-align: right; width: 50%;">
                <asp:Label ID="Label2" runat="server" Style="text-align: right" Text="Select file:"></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="FileUploadCSV" runat="server" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%; text-align: right;">
                <asp:Button ID="ButtonUpload" runat="server" Text="Import" TabIndex="2" OnClick="ButtonUpload_Click" />
            </td>
            <td style="text-align: left;">
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="3" OnClick="buttonBack_Click" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: center;">
                <asp:Label ID="labelMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <hr />
    <table style="width: 100%; border-color: black; border-style: solid; border-width: 1px;">
        <tr>
            <td style="width: 50%; text-align: right;">
                <asp:Label ID="label4" runat="server" Text="Select Country"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True">
                    <asp:ListItem Text="India-BSE" Value=".BSE"></asp:ListItem>
                    <asp:ListItem Text="India-NSE" Value=".NSE"></asp:ListItem>
                    <asp:ListItem Text="USA" Value=""></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>

        <tr>
            <td style="width: 50%; text-align: right;">
                <asp:Label ID="label3" runat="server" Text="Select Source Column"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSourceCols" runat="server" AutoPostBack="True"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>

        <tr>
            <td style="width: 50%; text-align: right;">
                <asp:Label ID="label1" runat="server" Text="Select Target Column"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlTargetCols" runat="server" AutoPostBack="True">
                    <asp:ListItem Text="Company Name" Value="companyname"></asp:ListItem>
                    <asp:ListItem Text="Purchase Date" Value="PurchaseDate"></asp:ListItem>
                    <asp:ListItem Text="Purchase Price" Value="PurchasePrice"></asp:ListItem>
                    <asp:ListItem Text="Purchase Qty" Value="PurchaseQty"></asp:ListItem>
                    <asp:ListItem Text="Commission Paid" Value="CommissionPaid"></asp:ListItem>
                    <asp:ListItem Text="Total Buy Value" Value="CostofInvestment"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>

        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="buttonMapSelected" runat="server" Text="Map Selected" OnClick="buttonMapSelected_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%;">
                <asp:GridView ID="GridViewMapping" class="gridheader" Visible="true" Caption="Mapped Columns" runat="server" Height="10%" AutoGenerateColumns="False"
                    HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                    PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewMapping_PageIndexChanging"
                    PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:BoundField HeaderText="Source Col" DataField="SourceCol" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Target Col" DataField="TargetCol" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"></PagerSettings>
                    <SelectedRowStyle BackColor="#999999" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="buttonConvert" runat="server" Text="Convert Data to Portfolio" OnClick="buttonConvert_Click" />
            </td>
        </tr>
    </table>
    <div style="overflow: scroll:">
        <%--<td colspan="2" style="width: 100%;">--%>
        <asp:GridView ID="GridViewMapped" class="gridheader" Visible="true" Caption="Mapped Data" runat="server" AutoGenerateColumns="False"
            HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
            PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewMapped_PageIndexChanging"
            PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
            <HeaderStyle HorizontalAlign="Center" />
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"></PagerSettings>
            <SelectedRowStyle BackColor="#999999" />
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
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>
