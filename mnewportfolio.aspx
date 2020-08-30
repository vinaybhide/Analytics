<%@ Page Title="New Portfolio" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="newportfolio.aspx.cs" Inherits="Analytics.newportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black; margin-top:50px; padding-top:150px;">
        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px;">
                <asp:Label ID="Label1" runat="server" Text="Create new portfolio"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Enter Portfolio Name:" Style="text-align: right;"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPortfolioName" runat="server" MaxLength="10"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                    ControlToValidate="textboxPortfolioName"
                    ErrorMessage="No space allowed in portfolio name"
                    ToolTip="No space allowed in portfolio name"
                    ValidationExpression="\w+" Text="*" />
            </td>
            </tr>
        <tr>
            <td colspan="2" style="width: 20%; text-align:center;">
                <asp:Button ID="buttonNewPortfolio" runat="server" Text="Create New Portfolio" OnClick="buttonNewPortfolio_Click" />
                
            </td>
            
        </tr>
    </table>
    <%--<div style="padding: 10% 5% 10% 5%;">
        <asp:Label ID="Label1" runat="server" Text="Enter Portfolio Name:" Style="text-align: right;" ></asp:Label><br />
        <asp:TextBox ID="textboxPortfolioName" runat="server" MaxLength="10"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
            ControlToValidate="textboxPortfolioName"
            ErrorMessage="No space allowed in portfolio name"
            ToolTip="No space allowed in portfolio name"
            ValidationExpression="\w+" Text="*" />
        <br />
        <br />
        <div>
            <asp:Button ID="buttonNewPortfolio" runat="server" Text="Create New Portfolio" OnClick="buttonNewPortfolio_Click" />
        </div>
    </div>--%>
</asp:Content>
