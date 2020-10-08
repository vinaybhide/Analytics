<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Analytics.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align: center; margin-top: 2%;">Login or Register</h3>
    <div style="padding: 5% 5% 10% 0%; border: solid;">
        <table style="width: 100%;">
            <tr>
                <td>
                    <p></p>
                </td>
                <td>
                    <p></p>
                </td>
            </tr>
            <tr>
                <td>
                    <p></p>
                </td>
                <td>
                    <p></p>
                </td>
            </tr>

            <tr>
                <td style="width: 40%; text-align: right;">
                    <asp:Label ID="Label_Email" CssClass="text-right" runat="server" Text="Email:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="textboxEmail" runat="server" TextMode="Email" Text="demo@demo.com" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 40%; text-align: right;">
                    <asp:Label ID="Label_Pwd" runat="server" CssClass="text-right" Text="Password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="textboxPwd" runat="server" TextMode="Password" Text="demo" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <asp:CheckBox ID="checkboxTestMode" Visible="false" Checked="true" Text="Off-line mode? <sup>+</sup>" runat="server" TabIndex="3" ToolTip="Select this check box to run the application in offline mode using data you have downloaded" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Label ID="Label1" Visible="false" runat="server" Text="<sup>+</sup>(In off-line mode application will use data you have previously downloaded)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr >
                <td style="width: 40%; text-align: right; padding-top:1%;padding-right:1%;">
                    <asp:Button ID="mbuttonLogin" runat="server" Text="Login" TabIndex="4" OnClick="mbuttonLogin_Click" />
                </td>
                <td style="padding-top:1%;">
                    <asp:Button ID="mbuttonRegister" runat="server" Text="Register" TabIndex="5" OnClick="mbuttonRegister_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <p></p>
                </td>
                <td>
                    <p></p>
                </td>
            </tr>
            <tr>
                <td>
                    <p></p>
                </td>
                <td>
                    <p></p>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Label ID="Label2" runat="server" Text="To register provide your email id & enter a password that you will remember. We will only use your email id for authenticated login to application."></asp:Label>
                </td>
            </tr>
            <%--<tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Label ID="Label4" runat="server" Text="Once you register we will create <b>demo_stock</b> and <b>demo_mf</b> portfolio's so that you can explore all features"></asp:Label>
                </td>
            </tr>--%>

<%--            <tr>
                <td colspan="2" style="text-align: left;">
                    <p style="font-size: small;">
                        **By default application uses Alpha Vantage free api kei to access online stock data. It has limitation of 5 calls per minute and 500 calls per day. 
            We recommend to get your own <a href="https://www.alphavantage.co/premium/">Alpha Vantage key</a> and use application <a href="~/addkey.aspx">Admin-->Add Key</a> menu to add your own key.
                    </p>
                </td>
            </tr>--%>

        </table>
    </div>
</asp:Content>
