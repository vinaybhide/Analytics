<%@ Page Title="New Portfolio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="newportfolio.aspx.cs" Inherits="Analytics.newportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black; margin-top: 50px; padding-top: 150px;">
        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px;">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Create new Stock portfolio"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Enter Portfolio Name:" Style="text-align: right;"></asp:Label>
            </td>
            <td style="width: 20%; border-right: solid; border-right-color: black; border-right-width: 1px;">
                <asp:TextBox ID="textboxPortfolioName" runat="server" MaxLength="10"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                    ControlToValidate="textboxPortfolioName"
                    ErrorMessage="No space allowed in portfolio name"
                    ToolTip="No space allowed in portfolio name"
                    ValidationExpression="\w+" Text="*" />
            </td>
        </tr>
            <tr>
                <td colspan="3" style="width: 100%; text-align: center; ">
                    <asp:Label runat="server">&nbsp</asp:Label>
                </td>
            </tr>

        <tr>
            <td colspan="2" style="width: 20%; text-align: center; border-right: solid; border-right-color: black; border-right-width: 1px;">
                <asp:Button ID="buttonNewPortfolio" runat="server" Text="Create New Portfolio" OnClick="buttonNewPortfolio_Click" />

            </td>

        </tr>
    </table>
</asp:Content>
