<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="getquoteadd.aspx.cs" Inherits="Analytics.getquoteadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .label {
            text-align: right;
        }
    </style>
    <h3 style="text-align:center; margin-top:2%;">Get Quote & Add to portfolio</h3>
    <div style="width: 100%; height: 100%; align-content: space-evenly; border: solid;">
        <p style="width: 100%; padding: 50px 0px 10px 50px;">
            <asp:Label ID="Label1" runat="server" Text="Search Stock:" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="TextBoxSearch" runat="server" TabIndex="1"></asp:TextBox>
            <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" TabIndex="2" />
            <asp:DropDownList ID="DropDownListStock" runat="server" Width="50%" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
        </p>
        <p style="width: 100%; padding: 0px 0px 0px 50px;">
            <asp:Label ID="labelSelectedSymbol" runat="server"></asp:Label>
            <asp:Button ID="ButtonGetQuote" runat="server" Text="Get Quote" OnClick="ButtonGetQuote_Click" />
        </p>
        <hr />
        <p style="width: 100%; padding: 10px 0px 10px 50px">
            <asp:Label ID="Label3" runat="server" Text="Open: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxOpen" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="High: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxHigh" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="Low: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxLow" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label5" runat="server" Text="Price: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxPrice" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label6" runat="server" Text="Volume: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxVolume" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label7" runat="server" Text="Latest Day: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxLatestDay" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
        </p>
        <p style="width: 100%; padding: 10px 0px 10px 50px;">
            <asp:Label ID="Label8" runat="server" Text="Prev Close: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxPrevClose" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label9" runat="server" Text="Change: " Width="7%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxChange" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
            <asp:Label ID="Label10" runat="server" Text="Change Percent: " Width="10%" Style="text-align: right;"></asp:Label>
            <asp:TextBox ID="textboxChangePercent" runat="server" ReadOnly="true" Width="7%"></asp:TextBox>
        </p>
        <hr />
        <p style="width: 100%; text-align: center; padding: 10px 0px 10px 50px;">
            <asp:Button ID="buttonAddStock" runat="server" Text="Add to Portfolio" OnClick="buttonAddStock_Click" />
            <asp:Button ID="buttonGoBack" runat="server" Text="Back" OnClick="buttonGoBack_Click" />
        </p>
    </div>

</asp:Content>
