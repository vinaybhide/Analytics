<%@ Page Title="Add New Script" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="addnewscript.aspx.cs" Inherits="Analytics.addnewscript" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .label {
            text-align: right;
        }
    </style>
    <h3 style="text-align:center; margin-top:2%;">Add new script to portfolio</h3>
    <div style="width: 100%; align-content:space-evenly; border:solid;">
        <p style="width: 100%; padding: 50px 0px 0px 10px;">
            <asp:Label ID="Label1" runat="server" Style="text-align: right" Text="Search Stock to add:"></asp:Label>
            <asp:TextBox ID="TextBoxSearch" runat="server" Width="10%" TabIndex="1"></asp:TextBox>
            <asp:Label ID="Label2" runat="server"></asp:Label>
            <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" TabIndex="2" />
            <asp:Label ID="Label3" runat="server"></asp:Label>
            <asp:DropDownList ID="DropDownListStock" runat="server" Width="20%" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
        </p>
        <p style="width: 100%; padding: 0px 0px 0px 50px;">
            <asp:Label ID="Label9" runat="server" Text="Company Name:"></asp:Label>
            <asp:Label ID="LabelCompanyName" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Label10" runat="server" Text="Symbol: "></asp:Label>
            <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>
        </p>
            <hr />
        <p style="width: 100%; padding: 50px 0px 50px 50px;">
            <asp:Label ID="Label5" runat="server" Text="Purchase Date: "></asp:Label>
            <asp:TextBox ID="textboxPurchaseDate" runat="server" Width="12%" TextMode="Date" ToolTip="Enter date of purchase" TabIndex="5"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="Price per stock: "></asp:Label>
            <asp:TextBox ID="textboxPurchasePrice" runat="server" Width="8%" ToolTip="Enter your unit purchase price " TabIndex="4"></asp:TextBox>
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ControlToValidate="textboxPurchasePrice" 
                ErrorMessage="Please enter valid decimal number with at least 1 decimal place." 
                ToolTip="Please enter valid decimal number with at least 1 decimal place." 
                ValidationExpression="((\d+)+(\.\d+))$" 
                Text="*" 
                />--%>
            <asp:Label ID="Label6" runat="server" Text="Purchase Quantity: "></asp:Label>
            <asp:TextBox ID="textboxQuantity" runat="server" Width="5%" TextMode="Number" ToolTip="Enter quantity you purchased" TabIndex="6" ></asp:TextBox>
            <asp:Label ID="Label7" runat="server" Text="Commission Paid: "></asp:Label>
            <asp:TextBox ID="textboxCommission" runat="server" Text="0.00" Width="5%" ToolTip="Enter commission you paid to the broker" TabIndex="7"></asp:TextBox>
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                ControlToValidate="textboxCommission" 
                ErrorMessage="Please enter valid decimal number with at least 1 decimal place." 
                ToolTip="Please enter valid decimal number with at least 1 decimal place." 
                ValidationExpression="((\d+)+(\.\d+))$" 
                Text="*" 
                />--%>
            <br />
            <br />
            <asp:Button ID="buttonCalCost" runat="server" Text="Calculate Total Cost" OnClick="buttonCalCost_Click" TabIndex="8" />
            <asp:Label ID="labelTotalCost" runat="server" Style="text-align: left" Width="10%" Text=""></asp:Label>
        </p>
        <p style="width: 100%; text-align: center;">
            <asp:Button ID="buttonAddStock" runat="server" Text="Add to Portfolio" OnClick="buttonAddStock_Click" TabIndex="9"/>
            <asp:Label ID="Label8" runat="server" Width="10%"></asp:Label>
            <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="10" OnClick="buttonBack_Click"/>
        </p>
        <br />
    </div>

</asp:Content>
