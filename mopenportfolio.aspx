<%@ Page Title="Open Portfolio" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="openportfolio.aspx.cs" Inherits="Analytics.openportfolio" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

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
            background: #d7d5d5;
            font-weight: bold;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            /*background-color: #D8D8D8;*/
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
            width: 85%;
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
    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive;">
                <div class="container table-responsive" style="padding-top: 1%; text-align: center; position: fixed; background-color: #c2c2c2;">
                    <asp:Button ID="ButtonAddNew" runat="server" Text="Add New" OnClick="ButtonAddNew_Click" />
                    <asp:Button ID="ButtonEdit" runat="server" Text="Edit" OnClick="ButtonEdit_Click" />
                    <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete" OnClick="buttonDeleteSelectedScript_Click" />
                    <asp:Button ID="buttonGetQuote" runat="server" Text="Get Quote & Add" OnClick="buttonGetQuote_Click" />
                    <asp:Button ID="buttonValuation" runat="server" Text="Portfolio Valuation" OnClick="buttonValuation_Click" />
                    <div style="padding-top: 2px; padding-bottom: 2px;">
                        <asp:Label ID="Label4" CssClass="text-right" runat="server" Text="Company Name :" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblCompName" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <br />
                        <%--<asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Selected Script:" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                        <asp:Label ID="Label5" CssClass="text-right" runat="server" Text="Symbol:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblScript" CssClass="text-left" runat="server" Text="Selected script name will appear here" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <%--<asp:Label ID="Label4" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>--%>
                        <br />
                        <asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Exchange:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblExchange" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="true"></asp:Label>

                        <br />
                        <asp:Label ID="Label3" CssClass="text-right" runat="server" Text="Purchase Date:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblDate" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <br />
                        <asp:Label ID="Label6" CssClass="text-left" runat="server" Text="Standard Indicators:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="ddlStdGrphType" runat="server" OnSelectedIndexChanged="ddlStdGrphType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="-1">-- Select Graph to Show --</asp:ListItem>
                            <asp:ListItem Value="Daily">Daily Price</asp:ListItem>
                            <asp:ListItem Value="Intra">Intra-day Price</asp:ListItem>
                            <asp:ListItem Value="SMA">SMA</asp:ListItem>
                            <asp:ListItem Value="EMA">EMA</asp:ListItem>
                            <asp:ListItem Value="VWAP">VWAP</asp:ListItem>
                            <asp:ListItem Value="RSI">RSI</asp:ListItem>
                            <asp:ListItem Value="ADX">ADX</asp:ListItem>
                            <asp:ListItem Value="STOCH">Stochastics</asp:ListItem>
                            <asp:ListItem Value="MACD">MACD</asp:ListItem>
                            <asp:ListItem Value="AROON">AROON</asp:ListItem>
                            <asp:ListItem Value="BBANDS">BBANDS</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="Label2" CssClass="text-left" runat="server" Text="Advance Indicators:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="ddlAdvGrphType" runat="server" OnSelectedIndexChanged="ddlAdvGrphType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="-1">-- Select Graph to Show --</asp:ListItem>
                            <asp:ListItem Value="INTRA_VWAP">Price Validator</asp:ListItem>
                            <asp:ListItem Value="DAILY_MACD">Trend Identifier</asp:ListItem>
                            <asp:ListItem Value="DAILY_RSI">Price momentum with RSI</asp:ListItem>
                            <asp:ListItem Value="DAILY_BBANDS">Trend Gauger</asp:ListItem>
                            <asp:ListItem Value="DAILY_STOCH_RSI">Buy Sell Indicator</asp:ListItem>
                            <asp:ListItem Value="DAILY_DX_DM_ADX">Price Direction</asp:ListItem>
                            <asp:ListItem Value="DAILY_DI_ADX">Trend Direction</asp:ListItem>
                            <asp:ListItem Value="BACKTEST">Back Test and Forecast</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div class="container">
                    <%--OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged"--%>
                    <%--CssClass="table table-striped table-bordered table-hover serh-grid"--%>
                    <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover serh-grid"
                        Width="100%" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
                        OnRowDataBound="grdViewOrders_RowDataBound"
                        OnRowCreated="grdViewOrders_RowCreated" OnRowCommand="grdViewOrders_RowCommand">
                        <Columns>
                            <%--<asp:BoundField DataField="CompanyName" HeaderText="Comp Name" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Name" HeaderText="Symbol" ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:BoundField DataField="PURCHASE_DATE" HeaderText="Txn Date" SortExpression="PURCHASE_DATE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PURCHASE_PRICE" HeaderText="Txn Price" SortExpression="PURCHASE_PRICE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PURCHASE_QTY" HeaderText="Txn Quantity" SortExpression="PURCHASE_QTY"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="COMMISSION_TAXES" HeaderText="Commission Taxes" ConvertEmptyStringToNull="true" NullDisplayText="0.00" SortExpression="CommissionPaid"
                                ItemStyle-HorizontalAlign="Center" />
                            <%--<asp:TemplateField HeaderText="Commission+Taxes"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("COMMISSION_TAXES","{0}") != "0.00") ? Eval("COMMISSION_TAXES","{0:0.00}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>


                            <asp:BoundField DataField="INVESTMENT_COST" HeaderText="Cost" SortExpression="INVESTMENT_COST"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CURRENTDATE" HeaderText="Quote Date" SortExpression="CURRENTDATE"
                                ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="CURRENTPRICE" HeaderText="Quote" SortExpression="CURRENTPRICE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CURRENTVALUE" HeaderText="Value Now" SortExpression="CURRENTVALUE"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="YearsInvested" HeaderText="Years Invested" SortExpression="YearsInvested"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ARR" HeaderText="ARR" SortExpression="ARR"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CumulativeQty" HeaderText="Cum Qty" SortExpression="CumulativeQty"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CumulativeCost" HeaderText="Cum Cost" SortExpression="CumulativeCost"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CumulativeValue" HeaderText="Cum Value" SortExpression="CumulativeValue"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CumulativeYearsInvested" HeaderText="Cum Years Invested" SortExpression="CumulativeYearsInvested"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CumulativeARR" HeaderText="Cum ARR" SortExpression="CumulativeARR"
                                ItemStyle-HorizontalAlign="Center" />

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
    <%--<div>
        <asp:TreeView ID="TreeViewPortfolio" runat="server" NodeWrap="True" ExpandDepth="1" ShowLines="True"></asp:TreeView>
    </div>--%>
</asp:Content>
