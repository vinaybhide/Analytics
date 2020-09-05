<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="importportfolio.aspx.cs" Inherits="Analytics.importportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .content {
            width: 100%;
            min-width: 50%;
        }
    </style>

    <%--<h3 style="text-align: center; margin-top: 1%;">Download data for off-line mode</h3>--%>
    <table style="width: 100%; margin-top: 2%;">
        <tr>
            <td colspan="3" style="text-align: center;">
                <asp:Label ID="Label48" runat="server" Text="Import Portfolio"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 50%;">
                <asp:Label ID="Label2" runat="server" Style="text-align: right" Text="Selected File:"></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="FileUploadCSV" runat="server" />
            </td>
            <td>
                <asp:Button ID="ButtonUpload" runat="server" Text="Upload" TabIndex="2" OnClick="ButtonUpload_Click" />
            </td>

        </tr>

        <tr>
            <td></td>
            <td style="text-align: left;">
                <asp:Label ID="labelMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 10%;">
                <asp:Button ID="buttonDownloadAll" runat="server" Text="Download All" Enabled="False" />
            </td>
            <td style="text-align: center;">
                <asp:Button ID="buttonDownloadSelected" runat="server" Text="Download Selected" />
            </td>
            <td>
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="3" OnClick="buttonBack_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView ID="GridViewData" class="gridheader" Visible="true" Caption="Imported Data" runat="server" Height="50%" AutoGenerateColumns="False"
                    HorizontalAlign="Center" AllowPaging="True" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                    PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="GridViewData_PageIndexChanging"
                    PagerSettings-Position="TopAndBottom" ShowHeaderWhenEmpty="True">
                    <%--            <Columns>
                <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="ADX" DataField="ADX" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>--%>
                    <HeaderStyle HorizontalAlign="Center" />

                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"></PagerSettings>
                    <SelectedRowStyle BackColor="#999999" />
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr style="text-align: center;">
            <td>
                <asp:Label ID="label3" runat="server" Text="Source Column"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlSourceCols" runat="server" AutoPostBack="True" ></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="label1" runat="server" Text="Target Column"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlTargetCols" runat="server" AutoPostBack="True" >
                    <asp:ListItem Text="Company Name" Value="companyname"></asp:ListItem>
                    <asp:ListItem Text="Purchase Date" Value="PurchaseDate"></asp:ListItem>
                    <asp:ListItem Text="Purchase Price" Value="PurchasePrice"></asp:ListItem>
                    <asp:ListItem Text="Purchase Qty" Value="PurchaseQty"></asp:ListItem>
                    <asp:ListItem Text="Commission Paid" Value="CommissionPaid"></asp:ListItem>
                    <asp:ListItem Text="Total Buy Value" Value="CostofInvestment"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="buttonMapSelected" runat="server" Text="Map Selected Column" OnClick="buttonMapSelected_Click" />
            </td>
            <td>
                <asp:GridView ID="GridViewMapping" class="gridheader" Visible="true" Caption="Mapped Columns" runat="server" Height="50%" AutoGenerateColumns="False"
                    HorizontalAlign="Center" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:BoundField HeaderText="Source Col" DataField="SourceCol" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Target Col" DataField="TargetCol" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#999999" />
                </asp:GridView>
            </td>
        </tr>
    </table>


</asp:Content>
