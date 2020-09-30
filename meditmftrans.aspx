<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="meditmftrans.aspx.cs" Inherits="Analytics.meditmftrans" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black;">
        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px;">
                <asp:Label ID="Label1" runat="server" Text="Edit MF Transaction"></asp:Label>
            </td>
        </tr>
    </table>
    <hr />
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black;">
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Fund House:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxFundHouse" runat="server" ReadOnly="true" ToolTip="Fund House"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label9" runat="server" Text="Fund Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxFundName" runat="server" ReadOnly="true" ToolTip="Fund Name" TabIndex="1"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>

        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="Select Fund Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <%--<asp:TextBox ID="textboxFundName" runat="server" ReadOnly="false" ToolTip="Fund Name" TabIndex="1"></asp:TextBox>--%>
                <asp:DropDownList ID="ddlFundName" runat="server" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlFundName_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label8" runat="server" Text="Scheme Code:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxSchemeCode" runat="server" ReadOnly="true" ToolTip="Scheme Code"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Purchase Date:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPurchaseDate" runat="server" ReadOnly="false" TextMode="Date" ToolTip="Date of purchase" TabIndex="2"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label4" runat="server" Text="Purchase NAV:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPurchaseNAV" runat="server" ReadOnly="false" ToolTip="Purchase NAV" TabIndex="3" 
                    OnTextChanged="textboxPurchaseNAV_TextChanged" AutoPostBack="True"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label6" runat="server" Text="Units: "></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxUnits" runat="server" ReadOnly="false" ToolTip="Units" TabIndex="4" OnTextChanged="textboxUnits_TextChanged" 
                    AutoPostBack="True"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label7" runat="server" Text="Value at cost: "></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxValueAtCost" runat="server" ReadOnly="true" ToolTip="Value at cost"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
    </table>
    <hr />
    <table style="width: 100%;">
        <tr>
            <td style="width: 25%; text-align: right;"></td>
            <td style="width: 30%; text-align: center;">
                <asp:Button ID="buttonSave" runat="server" Text="Save Changes" TabIndex="4" OnClick="buttonSave_Click"/>
            </td>
            <td style="width: 50%;">
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="5" OnClick="buttonBack_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
