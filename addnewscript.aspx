<%@ Page Title="Add New Script" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="addnewscript.aspx.cs" Inherits="Analytics.addnewscript" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .label {
            text-align: right;
        }
    </style>
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black; margin-top:2%">
        <%--<tr>
            <td colspan="3" style="width: 100%;">
            </td>
        </tr>--%>

        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px;">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Add Stock transaction"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Search Stock to add:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="TextBoxSearch" runat="server" Width="90%" TabIndex="1"></asp:TextBox>
            </td>
            <td style="width: 10%;">
                <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" TabIndex="2" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: center; width:100%;">
                <asp:DropDownList ID="DropDownListStock" runat="server" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label9" runat="server" Text="Company Name:"></asp:Label>
            </td>
            <td colspan="2" style="text-align: left;">
                <asp:Label ID="LabelCompanyName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label8" runat="server" Text="Symbol:"></asp:Label>
            </td>
            <td colspan="2" style="text-align: left;">
                <asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <%--<hr />--%>
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black; margin-top:1%;">
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="Exchange Code:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxExch" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label10" runat="server" Text="Exchange Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxExchDisp" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label11" runat="server" Text="Type:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxType" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label12" runat="server" Text="Type Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxTypeDisp" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Purchase Date:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPurchaseDate" runat="server" TextMode="Date" ToolTip="Enter date of purchase" TabIndex="5"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label4" runat="server" Text="Price per stock:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPurchasePrice" runat="server" ToolTip="Enter your unit purchase price " TabIndex="4"></asp:TextBox>
            </td>
            <td style="width: 20%;">
                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                    ControlToValidate="textboxPurchasePrice"
                    ErrorMessage="Please enter valid decimal number with at least 1 decimal place."
                    ToolTip="Please enter valid decimal number with at least 1 decimal place."
                    ValidationExpression="((\d+)+(\.\d+))$"
                    Text="*" />--%>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label6" runat="server" Text="Quantity: "></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxQuantity" runat="server" TextMode="Number" ToolTip="Enter quantity you purchased" TabIndex="6"></asp:TextBox><br />
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label7" runat="server" Text="Commission: "></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxCommission" runat="server" Text="0.00" ToolTip="Enter commission you paid to the broker" TabIndex="7"></asp:TextBox>
            </td>
            <td style="width: 20%;">
                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                    ControlToValidate="textboxCommission"
                    ErrorMessage="Please enter valid decimal number with at least 1 decimal place."
                    ToolTip="Please enter valid decimal number with at least 1 decimal place."
                    ValidationExpression="((\d+)+(\.\d+))$"
                    Text="*" />--%>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Button ID="buttonCalCost" runat="server" Text="Calculate Total Cost" OnClick="buttonCalCost_Click" TabIndex="8" />
            </td>
            <td style="width: 20%;">
                <asp:Label ID="labelTotalCost" runat="server" Style="text-align: left" Text="0.00"></asp:Label>
            </td>
            <td style="width: 20%;"></td>
        </tr>
    </table>
    <hr />
    <table style="width: 100%;">
        <tr>
            <td style="width: 25%; text-align: right;"></td>
            <td style="width: 30%; text-align: center;">
                <asp:Button ID="buttonAddStock" runat="server" Text="Add to Portfolio" OnClick="buttonAddStock_Click" TabIndex="9" />
            </td>
            <td style="width: 50%;">
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="10" OnClick="buttonBack_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
