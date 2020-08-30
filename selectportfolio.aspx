<%@ Page Title="Select Portfolio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="selectportfolio.aspx.cs" Inherits="Analytics.selectportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Select Portfolio</h3>
    <div style="width: 100%; align-content: space-evenly; border: solid">
        <p style="width: 100%; padding: 150px 0px 50px 50px;">
            <asp:Label ID="label3" Text="Select portfolio to open:" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlPortfolios" runat="server" Width="50%" AutoPostBack="True" OnSelectedIndexChanged="ddlPortfolios_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <br />
            <br />
        </p>
        <p style="width: 100%; text-align: center;">
            <asp:Label ID="labelSelectedFile" runat="server"></asp:Label><br /><br />
            <asp:Button ID="buttonLoad" runat="server" Text="Open Portfolio" OnClick="buttonLoad_Click" />
        </p>
    </div>
</asp:Content>
