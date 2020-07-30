<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Analytics.login" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align:center; margin-top:2%;">Login or Register</h3>
    <div style="padding-top:10%; text-align:right; padding-right:50%; padding-bottom:10%; border: solid"">
        <div>
            <asp:Label ID="Label_Email" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="textboxEmail" runat="server" TextMode="Email" TabIndex="1"></asp:TextBox>
            <br />
            <asp:Label ID="Label_Pwd" runat="server" Text="Password:"></asp:Label>
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
        </div>
        <br />
    </div>
</asp:Content>
