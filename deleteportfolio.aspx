<%@ Page Title="Delete Portfolio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="deleteportfolio.aspx.cs" Inherits="Analytics.deleteportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Delete Stock Portfolio</h3>
    <div style="width: 100%; align-content: space-evenly; border: solid">
        <p style="width: 100%; padding: 150px 0px 50px 0px; text-align:center;">
            <asp:Label ID="label2" Text="Select portfolio to delete:" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlFiles" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiles_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
            <br />
            <br />
            <br />
        </p>
        <p style="width: 100%; text-align: center;">
            <asp:Label ID="labelSelectedFile" Text="Selected File: " runat="server"></asp:Label><br /><br />
            <asp:Button ID="buttonDelete" runat="server" Text="Delete Portfolio" OnClick="buttonDelete_Click" TabIndex="2" />
            <asp:Label ID="Label8" runat="server" Width="10%"></asp:Label>
            <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="3" OnClick="buttonBack_Click"/>
        </p>
    </div>

</asp:Content>
