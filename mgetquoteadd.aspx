<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="getquoteadd.aspx.cs" Inherits="Analytics.getquoteadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align: center; margin-top: 2%;">Get Quote & Add to portfolio</h3>

    <div style="padding-left: 5%; border:thin;">
        <asp:Label ID="Label1" runat="server" Text="Search Stock:"></asp:Label>
        <asp:TextBox ID="TextBoxSearch" runat="server" TabIndex="1"></asp:TextBox>
        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" TabIndex="2" />
        <asp:DropDownList ID="DropDownListStock" runat="server" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
        <br />
        <asp:Label ID="labelSelectedSymbol" runat="server"></asp:Label>
        <br />
        <asp:Button ID="ButtonGetQuote" runat="server" Text="Get Quote" OnClick="ButtonGetQuote_Click" />
        <hr />
    </div>
    <div style="padding-left: 5%;border:thin; ">
        <asp:Label ID="Label3" runat="server" Text="Open:"></asp:Label>
        <asp:TextBox ID="textboxOpen" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label2" runat="server" Text="High:"></asp:Label>
        <asp:TextBox ID="textboxHigh" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label4" runat="server" Text="Low:"></asp:Label>
        <asp:TextBox ID="textboxLow" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label5" runat="server" Text="Price:"></asp:Label>
        <asp:TextBox ID="textboxPrice" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label6" runat="server" Text="Volume:"></asp:Label>
        <asp:TextBox ID="textboxVolume" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label7" runat="server" Text="Latest Day:"></asp:Label>
        <asp:TextBox ID="textboxLatestDay" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label8" runat="server" Text="Prev Close:"></asp:Label>
        <asp:TextBox ID="textboxPrevClose" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label9" runat="server" Text="Change:"></asp:Label>
        <asp:TextBox ID="textboxChange" runat="server" ReadOnly="true"></asp:TextBox><br />
        <asp:Label ID="Label10" runat="server" Text="Change Percent:"></asp:Label>
        <asp:TextBox ID="textboxChangePercent" runat="server" ReadOnly="true"></asp:TextBox><br />
        <br />
        <asp:Button ID="buttonAddStock" runat="server" Text="Add to Portfolio" OnClick="buttonAddStock_Click" />
        <asp:Button ID="buttonGoBack" runat="server" Text="Back" OnClick="buttonGoBack_Click" />
    </div>
</asp:Content>
