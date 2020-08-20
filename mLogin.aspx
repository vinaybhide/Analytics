<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Analytics.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align: center; margin-top: 2%;">Login or Register</h3>
    <div style="padding: 5% 5% 10% 0%; border: solid;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 40%; text-align: right;">
                    <asp:Label ID="Label_Email" CssClass="text-right" runat="server" Text="Email:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="textboxEmail" runat="server" TextMode="Email" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 40%; text-align: right;">
                    <asp:Label ID="Label_Pwd" runat="server" CssClass="text-right" Text="Password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="textboxPwd" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <asp:CheckBox ID="checkboxTestMode" Checked="true" Text="Off-line mode? <sup>+</sup>" runat="server" TabIndex="3" ToolTip="Select this check box to run the application in offline mode using data you have downloaded" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Label ID="Label1" runat="server" Text="<sup>+</sup>(Application will use data you have previously downloaded)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 40%; text-align: right;">
                    <asp:Button ID="mbuttonLogin" runat="server" Text="Login" TabIndex="4" OnClick="mbuttonLogin_Click" />
                </td>
                <td>
                    <asp:Button ID="mbuttonRegister" runat="server" Text="Register" TabIndex="5" OnClick="mbuttonRegister_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: left;">
                    <p style="font-size:small;">
                        **By default we use Alpha Vantage free api kei to access online stock data. It has limitation of 5 calls per minute and 500 calls per day. 
            We recommend to get your own <a href="https://www.alphavantage.co/premium/">Alpha Vantage key</a> and use application <a href="~/addkey.aspx">Admin-->Add Key</a> menu to add your own key.
                    </p>
                </td>
            </tr>

        </table>

<%--        <div>
            <asp:Label ID="Label_Email" Style="text-align: right;" runat="server" Text="Email:"></asp:Label><br />
            <asp:TextBox ID="textboxEmail" runat="server" TextMode="Email" TabIndex="1"></asp:TextBox>
            <br />
            <asp:Label ID="Label_Pwd" Style="text-align: right;" runat="server" Text="Password:"></asp:Label><br />
            <asp:TextBox ID="textboxPwd" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
        </div>
        <br />
        <div>
            <asp:CheckBox ID="checkboxTestMode" Text="Is Test Mode?" runat="server" TabIndex="3" ToolTip="Select this check box to run the application based in offline mode" />
        </div>
        <br />
        <div>
            <asp:Button ID="mbuttonLogin" runat="server" Text="Login" TabIndex="4" OnClick="mbuttonLogin_Click" />
            <asp:Button ID="mbuttonRegister" runat="server" Text="Register" TabIndex="5" OnClick="mbuttonRegister_Click" />
        </div>--%>
    </div>
    <%--<p style="font-size:xx-small;">
        **By default we use free demo api kei to access online stock data. It has limitation of 5 calls per minute and 500 calls in a day. 
            We recommend to use <a href="~/maddkey.aspx">Admin-->Add Key</a> menu to add your own key.
    </p>--%>

</asp:Content>
