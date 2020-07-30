<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="addnewscript.aspx.cs" Inherits="Analytics.addnewscript" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .label {
            text-align: right;
        }
    </style>
    <h3 style="text-align:center; margin-top:2%;">Add new script to portfolio</h3>
    <div style="padding-left:5%; border:solid;">
        <p>
            <asp:Label ID="Label1" runat="server" Text="Search Stock to add:"></asp:Label>
            <asp:TextBox ID="TextBoxSearch" runat="server" TabIndex="1"></asp:TextBox>
            <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" TabIndex="2" />
            <asp:DropDownList ID="DropDownListStock" runat="server" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
            <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>
        </p>
        <p>
            <asp:Label ID="Label4" runat="server" Text="Purchase Price: "></asp:Label>
            <asp:TextBox ID="textboxPurchasePrice" runat="server" ToolTip="Enter your unit purchase price " TabIndex="4"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                ControlToValidate="textboxPurchasePrice"
                ErrorMessage="Please enter valid decimal number with at least 1 decimal place."
                ToolTip="Please enter valid decimal number with at least 1 decimal place."
                ValidationExpression="((\d+)+(\.\d+))$"
                Text="*" /><br />
            <asp:Label ID="Label5" runat="server" Text="Purchase Date: "></asp:Label>
            <asp:TextBox ID="textboxPurchaseDate" runat="server" TextMode="Date" ToolTip="Enter date of purchase" TabIndex="5"></asp:TextBox><br />
            <asp:Label ID="Label6" runat="server" Text="Purchase Quantity: "></asp:Label>
            <asp:TextBox ID="textboxQuantity" runat="server" TextMode="Number" ToolTip="Enter quantity you purchased" TabIndex="6"></asp:TextBox><br />
            <asp:Label ID="Label7" runat="server" Text="Commission Paid: "></asp:Label>
            <asp:TextBox ID="textboxCommission" runat="server" ToolTip="Enter commission you paid to the broker" TabIndex="7"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                ControlToValidate="textboxCommission"
                ErrorMessage="Please enter valid decimal number with at least 1 decimal place."
                ToolTip="Please enter valid decimal number with at least 1 decimal place."
                ValidationExpression="((\d+)+(\.\d+))$"
                Text="*" />
            <br />
            <asp:Button ID="buttonCalCost" runat="server" Text="Calculate Total Cost" OnClick="buttonCalCost_Click" TabIndex="8" />
            <asp:Label ID="labelTotalCost" runat="server" style="text-align: left" Text=""></asp:Label>
        </p>
        <div >
            <asp:Button ID="buttonAddStock" runat="server" Text="Add to Portfolio" OnClick="buttonAddStock_Click" TabIndex="9" />
            <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="10" OnClick="buttonBack_Click" />
        </div>
        <br />
    </div>

</asp:Content>
