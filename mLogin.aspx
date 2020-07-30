<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Analytics.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Login or Register</h3>
    <div style="padding:10% 5% 10% 5%; border:solid;">
        <div>
            <asp:Label ID="Label_Email" style="text-align:right;"  runat="server" Text="Email:"></asp:Label><br />
            <asp:TextBox ID="textboxEmail" runat="server" TextMode="Email" TabIndex="1"></asp:TextBox>
            <br />
            <asp:Label ID="Label_Pwd" style="text-align:right;" runat="server" Text="Password:"></asp:Label><br />
            <asp:TextBox ID="textboxPwd" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
        </div>
        <br />
        <div>
            <asp:CheckBox ID="checkboxTestMode" Text="Is Test Mode?"  runat="server" TabIndex="3" ToolTip="Select this check box to run the application based in offline mode" />
        </div>
        <br />
        <div >
            <asp:Button ID="mbuttonLogin" runat="server" Text="Login" TabIndex="4" OnClick="mbuttonLogin_Click" />
            <asp:Button ID="mbuttonRegister" runat="server" Text="Register" TabIndex="5" OnClick="mbuttonRegister_Click" />
        </div>
    </div>
</asp:Content>
