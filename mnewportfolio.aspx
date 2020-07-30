<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="newportfolio.aspx.cs" Inherits="Analytics.newportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 10% 5% 10% 5%;">
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
    </div>

</asp:Content>
