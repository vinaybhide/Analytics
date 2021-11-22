<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="editscript.aspx.cs" Inherits="Analytics.editscript" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black; margin-top:2%;">
        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px;">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Edit Stock Transaction"></asp:Label>
            </td>
        </tr>
    </table>
    <%--<hr />--%>
    <table style="width: 100%; border: solid; border-width: 1px; border-color: black;margin-top:1%;">
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Company Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxCompaName" Enabled="false" runat="server" ToolTip="Enter company name" TabIndex="1"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="Symbol:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxSymbol" Enabled="false" runat="server" ToolTip="Enter stock Symbol:" TabIndex="2"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <%--<tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label8" runat="server" Text="Exchange Code:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxExch" runat="server" ReadOnly="false"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>--%>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label10" runat="server" Text="Exchange Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxExchDisp" Enabled="false" runat="server" ReadOnly="false"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <%--<tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label11" runat="server" Text="Type:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxType" runat="server" ReadOnly="false"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>--%>
        <%--<tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label12" runat="server" Text="Type Name:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxTypeDisp" runat="server" ReadOnly="false"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>--%>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Purchase Date:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPurchaseDate" runat="server" TextMode="Date" ToolTip="Enter date of purchase" TabIndex="3"></asp:TextBox>
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label4" runat="server" Text="Price per stock:"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxPurchasePrice" runat="server" ToolTip="Purchase price " TabIndex="4"></asp:TextBox>
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
                <asp:TextBox ID="textboxQuantity" runat="server" TextMode="Number" ToolTip="Enter quantity you purchased" TabIndex="5"></asp:TextBox><br />
            </td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label7" runat="server" Text="Commission: "></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="textboxCommission" runat="server" Text="0.00" ToolTip="Enter commission you paid to the broker" TabIndex="6"></asp:TextBox>
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
                <asp:Button ID="buttonCalCost" runat="server" Text="Calculate Total Cost" OnClick="buttonCalCost_Click" TabIndex="7" />
            </td>
            <td style="width: 20%;">
                <asp:Label ID="labelTotalCost" runat="server" Style="text-align: left" Text="0.00"></asp:Label>
            </td>
            <td style="width: 20%;"></td>
        </tr>
    </table>
    <%--<hr />--%>
    <table style="width: 100%;margin-top:1%;">
        <tr>
            <td style="width: 25%; text-align: right;"></td>
            <td style="width: 30%; text-align: center;">
                <asp:Button ID="buttonSave" runat="server" Text="Save Changes" TabIndex="8" OnClick="buttonSave_Click"/>
            </td>
            <td style="width: 50%;">
                <asp:Button ID="buttonBack" runat="server" Text="Back" TabIndex="10" OnClick="buttonBack_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
