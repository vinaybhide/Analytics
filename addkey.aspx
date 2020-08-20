<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="addkey.aspx.cs" Inherits="Analytics.addkey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-align: center; margin-top: 2%;">Alpha Vantage Key</h3>
    <div style="padding-top: 10%; padding-bottom: 10%; border: solid;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 10%; text-align: right;">
                    <asp:Label ID="Label1" runat="server" Text="Enter Alpha Vantage key:"></asp:Label>
                </td>
                <td style="width: 20%;">
                    <asp:TextBox ID="textboxKey" Width="90%" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="width: 20%;">
                    <br />
                    <asp:Button ID="buttonAddKey" runat="server" Text="Save Key" OnClick="buttonAddKey_Click" />
                    <asp:Button ID="buttonBack" runat="server" Text="Back" OnClick="buttonBack_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: left;">
                    <p style="font-size: small;">
                        **By default we use Alpha Vantage free api kei to access online stock data. It has limitation of 5 calls per minute and 500 calls per day. 
                        <br />
                        We recommend to get your own <a href="https://www.alphavantage.co/premium/">Alpha Vantage key</a> and add that key for better uninterrupted experience.
                    </p>
                </td>
            </tr>
        </table>
    </div>
    <p style="font-size: x-small;">
        **By default we use free demo api kei to access online stock data. It has limitation of 5 calls per minute and 500 calls in a day. 
            We recommend you get your own key from <a href="https://www.alphavantage.co/support/#api-key">Alpha Vantage</a> and add your own key.
    </p>

</asp:Content>
