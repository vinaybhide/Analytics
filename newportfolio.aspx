<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="newportfolio.aspx.cs" Inherits="Analytics.newportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-top:10%; text-align:right; padding-right:50%;">
        <asp:Label ID="Label1" runat="server" Text="Enter Portfolio Name: "></asp:Label>
        <asp:TextBox ID="textboxPortfolioName" runat="server" MaxLength="10"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
            ControlToValidate="textboxPortfolioName"
            ErrorMessage="No space allowed in portfolio name"
            ToolTip="No space allowed in portfolio name"
            ValidationExpression="\w+" Text="*" />
        <br />
        <br />
        <asp:Button ID="buttonNewPortfolio" runat="server" Text="Create New Portfolio" OnClick="buttonNewPortfolio_Click" />
    </div>

</asp:Content>
