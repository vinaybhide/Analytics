<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="meditmftrans.aspx.cs" Inherits="Analytics.meditmftrans" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black;margin-top:2%;">
        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px;">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Edit MF Transaction"></asp:Label>
            </td>
        </tr>
    </table>
    <%--<hr />--%>
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black;margin-top:1%;">
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Fund House:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxFundHouse" runat="server" ReadOnly="true" ToolTip="Fund House" ></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label9" runat="server" Text="Fund Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxFundName" runat="server" ReadOnly="true" ToolTip="Fund Name" TabIndex="1" ></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>

        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="Select New Fund Name:"></asp:Label>
            </td>
            <td>
                <%--<asp:TextBox ID="textboxFundName" runat="server" ReadOnly="false" ToolTip="Fund Name" TabIndex="1"></asp:TextBox>--%>
                <asp:DropDownList ID="ddlFundName" Width="90%" runat="server" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlFundName_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="width:2%;"></td>
        </tr>
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label8" runat="server" Text="Scheme Code:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxSchemeCode" runat="server" ReadOnly="true" ToolTip="Scheme Code"></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Purchase Date:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxPurchaseDate" runat="server" ReadOnly="false" TextMode="Date" ToolTip="Date of purchase" TabIndex="2"></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label4" runat="server" Text="Purchase NAV:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxPurchaseNAV" runat="server" ReadOnly="false" ToolTip="Purchase NAV" TabIndex="3" 
                    OnTextChanged="textboxPurchaseNAV_TextChanged" AutoPostBack="True"></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label6" runat="server" Text="Units: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxUnits" runat="server" ReadOnly="false" ToolTip="Units" TabIndex="4" OnTextChanged="textboxUnits_TextChanged" 
                    AutoPostBack="True"></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>
        <tr>
            <td style="width: 45%; text-align: right;">
                <asp:Label ID="Label7" runat="server" Text="Value at cost: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxValueAtCost" runat="server" ReadOnly="true" ToolTip="Value at cost"></asp:TextBox>
            </td>
            <td style="width:2%;"></td>
        </tr>
    </table>
    <%--<hr />--%>
    <table style="width: 100%;margin-top:1%;">
        <tr>
            <td style="width: 35%; text-align: right;">
                <asp:Button ID="buttonSave" runat="server" Text="Save Changes" TabIndex="4" OnClick="buttonSave_Click"/>
            </td>
            <td style="width: 2%; "></td>
            <td style="width: 40%;">
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="5" OnClick="buttonBack_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
