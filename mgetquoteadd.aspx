<%@ Page Title="Get Quote" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mgetquoteadd.aspx.cs" Inherits="Analytics.mgetquoteadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: solid; border-width: 1px; margin-top: 2%;">
        <tr style="border: solid; border-width: 1px;">
            <td colspan="3" style="width: 100%; text-align: center; border: solid; border-width: 1px; border-color: black;">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Add Stock to portfolio"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="width: 100%; text-align: center;">
                <asp:Label runat="server">&nbsp</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; ">
                <asp:Label ID="Label17" runat="server" Style="text-align: right" Text="Select Exchange:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlExchange" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlExchange_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="Filter by Company name :"></asp:Label>
            </td>
            <td style="width: 20%;">
                <asp:TextBox ID="TextBoxSearch" runat="server" Width="90%" TabIndex="1"></asp:TextBox><br />
            </td>
            <td style="width: 20%;">
                <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" TabIndex="2" /><br />
            </td>
        </tr>
        <tr>
            <td style="text-align: right; ">
                <asp:Label ID="label49" Text="Select stock or company :" runat="server"></asp:Label>
            </td>
            <td colspan="2">
                <asp:DropDownList ID="DropDownListStock" runat="server" OnSelectedIndexChanged="DropDownListStock_SelectedIndexChanged" AutoPostBack="True" TabIndex="3"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="Label8" runat="server" Text="Selected Stock:"></asp:Label>
            </td>
            <td colspan="2" style="text-align: left;">
                <%--<asp:Label ID="labelSelectedSymbol" runat="server" Text=""></asp:Label>--%>
                <asp:TextBox ID="textboxSelectedSymbol" Width="100" runat="server" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 25%; text-align: right;"></td>

            <td colspan="2">
                <asp:Button ID="ButtonGetQuote" runat="server" Text="Get Quote" OnClick="ButtonGetQuote_Click" />

            </td>
        </tr>
    </table>
    <%--<hr />--%>
    <table style="width: 100%; border: solid; border-width: 1px; margin-top: 1%;">
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="Open:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxOpen" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label4" runat="server" Text="High:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxHigh" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Low:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxLow" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label6" runat="server" Text="Price:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxPrice" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label7" runat="server" Text="Volume:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxVolume" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label9" runat="server" Text="Latest Day:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxLatestDay" TextMode="DateTimeLocal" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label10" runat="server" Text="Prev Close:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxPrevClose" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label11" runat="server" Text="Change:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxChange" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label12" runat="server" Text="Change Percent:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxChangePercent" runat="server" ReadOnly="true"></asp:TextBox><br />
            </td>

        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label13" runat="server" Text="Exchange Code:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxExch" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label14" runat="server" Text="Exchange Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxExchDisp" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label15" runat="server" Text="Type:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxType" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 38%; text-align: right;">
                <asp:Label ID="Label16" runat="server" Text="Type Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textboxTypeDisp" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <%--        <tr>
            <td></td>
            <td></td>
        </tr>--%>
    </table>
    <table style="width: 100%; margin-top: 1%;">
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Button ID="buttonAddStock" runat="server" Text="Add to Portfolio" OnClick="buttonAddStock_Click" />
            </td>
            <td style="width: 2%;"></td>
            <td style="width: 8%;">
                <asp:Button ID="buttonGoBack" runat="server" Text="Back" OnClick="buttonGoBack_Click" />
            </td>
            <td style="width: 20%;"></td>
        </tr>

    </table>
</asp:Content>
