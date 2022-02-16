<%@ Page Title="Select Portfolio" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mselectportfolio.aspx.cs" Inherits="Analytics.mselectportfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gridheader {
            text-align: center;
        }

        .fixedHeader {
            /*font-weight: bold;*/
            position: absolute; /*absolute;*/
            background-color: #006699;
            color: #ffffff;
            /*height: 37px;
            top: expression(Sys.UI.DomElement.getBounds(document.getElementById("panelContainer")).y-37);*/
        }

        .grid-sltrow {
            background: #d7d5d5;
            font-weight: bold;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            /*background-color: #D8D8D8;*/
            font-weight: bold;
        }

        .TableTitleRowStyle {
            border: solid 1px Gray;
            background-color: chocolate;
            color: #ffffff;
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
            /*background-color: #4682B4;
            color: #ffffff;*/
            background-color: #e1b94a;
            color: #000000;
            font-weight: bold;
        }

        .serh-grid {
            width: 100%;
            border: 1px solid #6AB5FF;
            background: #fff;
            line-height: 14px;
            font-size: 11px;
            font-family: Verdana;
        }

        /*table.grdCamera {*/
        /* table width */
        /*min-width: 600px;
            width: 50%;
        }*/

        /****TABLE HEADER (headerCamera) *******/
        /*table.grdCamera tr.headerCamera {
                position: fixed;
                overflow: hidden;
                white-space: nowrap;
                width: 80%;
                margin: 0;
                z-index: 100;
            }*/

        /*padding content of 2nd row (it's the 1st data row)*/
        /*table.grdCamera tr:nth-child(2) td {
                padding-top: 40px;
            }*/
    </style>
    <script>
        window.addEventListener("beforeunload", function (e) {
            sessionStorage.setItem('stockportscrollpos', "-1");
        });

    </script>

    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive">
                <div class="container table-responsive" style="padding-top: 1%; text-align: center; position: fixed; background-color: #c2c2c2;">
                    <table style="width: 100%;">
                        <tr style="width: 100%;">
                            <td style="width: 100%; text-align: center; margin-top: 2px;">
                                <%--<asp:Timer ID="TimerIndices" runat="server" Interval="60000" OnTick="GetIndexValues" />
                                <asp:Timer ID="TimerClear" runat="server" Interval="59900" OnTick="ClearHeading" />
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TimerIndices" />
                                        <asp:AsyncPostBackTrigger ControlID="TimerClear" />
                                    </Triggers>
                                    <ContentTemplate>--%>
                                <asp:Label ID="lblDashboard" runat="server" Font-Bold="true" Font-Size="Medium" Text="Portfolio Manager for: "></asp:Label>
                                <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                    </table>
                    <div style="padding-top: 2px; padding-bottom: 2px;">
                        <table style="width: 100%;">
                            <tr style="width: 100%;">
                                <td style="text-align: center; margin-top: 2px;">
                                    <asp:Button ID="buttonNew" runat="server" Text="Create New Portfolio" OnClick="buttonNew_Click" />
                                    <asp:Button ID="buttonImport" runat="server" Text="Import Portfolio" OnClick="buttonImport_Click" />
                                    <asp:Button ID="buttonGetQuote" runat="server" Text="Get Quote" OnClick="buttonGetQuote_Click" />
                                    <asp:Button ID="buttonStdIndicators" runat="server" Text="Standard Indicators" OnClick="buttonStdIndicators_Click" />
                                    <asp:Button ID="buttonAdvIndicators" runat="server" Text="Advance Indicators" OnClick="buttonAdvIndicators_Click" />
                                    <asp:Button ID="buttonGlobalIndices" runat="server" Text="Global Indices" OnClick="buttonGlobalIndices_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <br />
                <div class="container">
                    <%--OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged"--%>
                    <%--CssClass="table table-striped table-bordered table-hover serh-grid"--%>
                    <asp:Panel ID="panelPortfolioMaster" runat="server" Width="100%" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1" BorderColor="Black">
                        <asp:GridView ID="gvPortfolioMaster" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover serh-grid"
                            Width="100%" ShowHeaderWhenEmpty="true" HorizontalAlign="Center" EmptyDataText="Please create a portfolio!"
                            OnRowCancelingEdit="gvPortfolioMaster_RowCancelingEdit"
                            OnRowEditing="gvPortfolioMaster_RowEditing" OnRowUpdating="gvPortfolioMaster_RowUpdating">
                            <%--OnRowUpdating="gvPortfolioMaster_RowUpdating" OnRowEditing="gvPortfolioMaster_RowEditing" OnRowDeleting="gvPortfolioMaster_RowDeleting"--%>
                            <%--Caption="Portfolio Summary" CaptionAlign="Top">--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Portfolio Name" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblPortfolioName" runat="server" OnClick="btnOpen_Click" class="btn-link" Font-Underline="true" Text='<%# Eval("PORTFOLIO_NAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="textboxPortfolioName" runat="server" Text='<%# Eval("PORTFOLIO_NAME")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CumulativeCost" HeaderText="Total Investment" ReadOnly="true" SortExpression="CumulativeCost" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CumulativeValue" HeaderText="Valuation" ReadOnly="true" SortExpression="CumulativeValue" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Create New" Visible="false" ShowHeader="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnedit" runat="server" Visible="false" Enabled="false" CommandName="Edit" class="btn btn-primary btn-sm" Text="Edit&raquo;"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" class="btn btn-primary btn-sm" Text="Save&raquo;"></asp:LinkButton>
                                        <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" class="btn btn-primary btn-sm" Text="Cancel&raquo;"></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" Visible="true" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnOpen" runat="server" OnClick="btnOpen_Click" class="btn btn-primary btn-sm" Text="Open&raquo;"></asp:LinkButton>
                                        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" class="btn btn-primary btn-sm" Text="Delete&raquo;"></asp:LinkButton>
                                        <asp:LinkButton ID="btnValuation" runat="server" OnClick="btnValuation_Click" class="btn btn-primary btn-sm" Text="Valuation&raquo;"></asp:LinkButton>

                                        <%--<asp:LinkButton ID="btnopen" runat="server" CommandName="Update" class="btn btn-primary btn-sm" Text="Open&raquo;"></asp:LinkButton>
                                        <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" class="btn btn-primary btn-sm" Text="Valuation&raquo;"></asp:LinkButton>
                                        <asp:LinkButton ID="btndelete" runat="server" CommandName="Delete" class="btn btn-primary btn-sm" Text="Delete&raquo;"></asp:LinkButton>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <SelectedRowStyle CssClass="grid-sltrow" />
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" ForeColor="White" BorderStyle="Solid"
                                BorderWidth="1px" BorderColor="Black" />

                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
