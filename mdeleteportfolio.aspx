<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="deleteportfolio.aspx.cs" Inherits="Analytics.deleteportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Delete Portfolio</h3>
    <div style="text-align:center; padding: 10% 5% 10% 5%; border:solid;">
        <asp:Label ID="label2" Text="Select portfolio to delete:" runat="server"></asp:Label><br />
        <asp:DropDownList ID="ddlFiles" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiles_SelectedIndexChanged"></asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="labelSelectedFile" Text="Selected File: Please select portfolio to delete" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="buttonDelete" runat="server" Text="Delete Portfolio" OnClick="buttonDelete_Click" />
    </div>

</asp:Content>
