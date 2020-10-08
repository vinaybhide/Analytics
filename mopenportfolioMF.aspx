<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mopenportfolioMF.aspx.cs" Inherits="Analytics.mopenportfolioMF" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gridheader {
            text-align: center;
        }

        .FixedHeader {
            position: absolute;
            font-weight: normal;
        }

        .grid-sltrow {
            background: Gray; /*#ddd;*/
            font-weight: bold;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            background-color: cadetblue; /*#D8D8D8;*/
            font-weight: bold;
        }

        .GrandTotalRowStyle {
            border: solid 1px Gray;
            background-color: #000000;
            color: #ffffff;
            font-weight: bold;
        }

        .GroupHeaderStyle {
            border: solid 1px Black;
            background-color: #4682B4;
            color: #ffffff;
            font-weight: bold;
        }

        .serh-grid {
            width: 85%;
            border: 1px solid #6AB5FF;
            background: #fff;
            line-height: 14px;
            font-size: 11px;
            font-family: Verdana;
        }
    </style>
    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive;">
                <div style="padding-top: 1%; width: 100%; text-align: center; position: fixed; border: solid; border-color: black; border-width: 1px;">
                    <asp:Button ID="ButtonAddNew" runat="server" Text="Add New" OnClick="ButtonAddNew_Click" />
                    <asp:Button ID="ButtonEdit" runat="server" Text="Edit" OnClick="ButtonEdit_Click" />
                    <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete" OnClick="buttonDeleteSelectedScript_Click" />
                    <asp:Button ID="buttonValuationLine" runat="server" Text="Valuation (Line Graph)" OnClick="buttonValuationLine_Click" />
                    <asp:Button ID="buttonValuation" runat="server" Text="Valuation (Bar Graph)" OnClick="buttonValuation_Click" />
                    <br />
                    <asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Selected MF:" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblScript" CssClass="text-left" runat="server" Text="None" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:Label ID="Label4" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>
                    <asp:Label ID="Label3" CssClass="text-right" runat="server" Text="Purchase Date:" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblDate" CssClass="text-left" runat="server" Text="None" BackColor="Gray" ForeColor="Black" Font-Bold="true"></asp:Label>
                </div>
                <br />
                <br />
                <br />
                <div>

                    <%--OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged"--%>
                    <%--CssClass="table table-striped table-bordered table-hover serh-grid"--%>
                    <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover serh-grid"
                        Width="100%" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
                        OnRowDataBound="grdViewOrders_RowDataBound"
                        OnRowCreated="grdViewOrders_RowCreated" OnRowCommand="grdViewOrders_RowCommand" ShowHeader="true">
                        <Columns>
                            <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" SortExpression="PurchaseDate"
                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:BoundField DataField="PurchaseNAV" HeaderText="Purchase NAV" SortExpression="PurchaseNAV"
                                ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="PurchaseUnits" HeaderText="Units" SortExpression="PurchaseUnits"
                                ItemStyle-HorizontalAlign="Center" />

                            <%--<asp:BoundField DataField="CurrentNAV" HeaderText="Current NAV" SortExpression="CurrentNAV"
                                ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:TemplateField HeaderText="Current NAV" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("CurrentNAV","{0}") != "0") ? Eval("CurrentNAV","{0:0.0000}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="NAVdate" HeaderText="NAV Date" SortExpression="NAVdate"
                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />--%>
                            <asp:TemplateField HeaderText="NAV Date" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("NAVdate","{0}") != DateTime.MinValue.ToString() ) ? Eval("NAVdate","{0:dd-MM-yyyy}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ValueAtCost" HeaderText="Value at Cost" SortExpression="ValueAtCost"
                                ItemStyle-HorizontalAlign="Center" />
                            <%--<asp:TemplateField HeaderText="Value At Cost" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("ValueAtCost","{0}") != "0") ? Eval("ValueAtCost","{0:0.0000}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Value at Cost" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("ValueAtCost","{0}") != "0") ? Eval("ValueAtCost","{0:0.0000}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <%--<asp:BoundField DataField="CurrentValue" HeaderText="Value now" SortExpression="CurrentValue"
                                ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:TemplateField HeaderText="Value now" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("CurrentValue","{0}") != "0") ? Eval("CurrentValue","{0:0.0000}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="YearsInvested" HeaderText="Years Invested" SortExpression="YearsInvested"
                                ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:TemplateField HeaderText="Years Invested" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("YearsInvested","{0}") != "0") ? Eval("YearsInvested","{0:0.0000}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="ARR" HeaderText="ARR" SortExpression="ARR"
                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:0.00%}" />--%>

                            <asp:TemplateField HeaderText="ARR" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("ARR","{0}") != "0") ? Eval("ARR","{0:0.0000%}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <%--<SelectedRowStyle BackColor="#CCCCCC" />--%>
                        <SelectedRowStyle CssClass="grid-sltrow" />
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderStyle="Solid"
                            BorderWidth="1px" BorderColor="Black" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
