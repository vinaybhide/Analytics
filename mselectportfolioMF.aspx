<%@ Page Title="Select MF Portfolio" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mselectportfolioMF.aspx.cs" Inherits="Analytics.mselectportfolioMF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Select MF Portfolio</h3>
    <div style="text-align:center; padding: 10% 5% 10% 5%; border: solid"">
        <asp:Label ID="label3" Text="Select portfolio to open:" runat="server"></asp:Label><br />
        <asp:DropDownList ID="ddlPortfolios" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPortfolios_SelectedIndexChanged"></asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="labelSelectedFile" runat="server" Text="Selected File: Please select portfolio to open" ></asp:Label>
        <br />
        <br />
        <asp:Button ID="buttonLoad" runat="server" Text="Open Portfolio" OnClick="buttonLoad_Click" />
    </div>
</asp:Content>
