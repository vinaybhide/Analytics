<%@ Page Title="Open Portfolio" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mopenportfolio.aspx.cs" Inherits="Analytics.mopenportfolio" EnableEventValidation="false" MaintainScrollPositionOnPostback="false" %>

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
        //document.addEventListener("DOMContentLoaded", function (event) {
        //    var scrollpos = localStorage.getItem('scrollpos');
        //    if (scrollpos) window.scrollTo(0, scrollpos);
        //});

        //window.onbeforeunload = function (e) {
        //    localStorage.setItem('scrollpos', window.scrollY);
        //};
        document.addEventListener("DOMContentLoaded", function (event) {
            var scrollpos = sessionStorage.getItem('stockportscrollpos');
            var name = ' <%= Session["STOCKPORTFOLIOMASTERROWID"] %>'
            var stockrowid = sessionStorage.getItem('stockportfoliomasterrowid');
            if (stockrowid) {
                if (stockrowid == name) {
                    if (scrollpos) {
                        window.scrollTo(0, scrollpos);
                        sessionStorage.removeItem('stockportscrollpos');
                        sessionStorage.removeItem('stockportfoliomasterrowid');
                    }
                }
            }
        });

        window.addEventListener("beforeunload", function (e) {
            sessionStorage.setItem('stockportscrollpos', window.scrollY);
            var name = ' <%= Session["STOCKPORTFOLIOMASTERROWID"] %>'
            sessionStorage.setItem('stockportfoliomasterrowid', name);
        });

        function setscrollportfolio(actualindex) {
            var gridsummary = document.getElementsByTagName("table")[0];
            var numofsummaryrows = gridsummary.getElementsByTagName("tr");
            //first find height of summary
            var htsummary = gridsummary.offsetHeight;

            var gridportfolio = document.getElementsByTagName("table")[1];
            var numoftransactionrows = gridportfolio.getElementsByTagName("tr");
            var htportfolio = gridportfolio.offsetHeight;

            window.scroll(0, (htsummary + numoftransactionrows[actualindex].offsetTop));
            sessionStorage.setItem('stockportscrollpos', (htsummary + numoftransactionrows[actualindex].offsetTop));

            //if (actualindex < numoftransactionrows.length) {
            //    alert('before scroll');
            //    window.scroll(0, mygridcol[actualindex].offsetTop);
            //    alert('before sessionstorage');
            //    sessionStorage.setItem('mfportscrollpos', mygridcol[actualindex].offsetTop);
            //    alert('after sessionstorage');
            //}
        }

        //function not used - sample
        //function setscroll() {
        //    alert('in on load');
        //    var gridportfolio = document.getElementsByTagName("table")[1];
        //    alert('after gridviewportfolio');
        //    var mygridcol = gridportfolio.getElementsByTagName("tr");
        //    alert('after mygrid col len' + mygridcol.length);
        //    if (mygridcol.length > 0) {
        //        var mycount = 0;
        //        while (mycount < mygridcol.length) {
        //            if (mygridcol[mycount].style.backgroundColor == "orange") {
        //                alert(mygridcol[mycount].style.backgroundColor);
        //                window.scroll(0, mygridcol[mycount].offsetTop);
        //                break;
        //            }
        //            mycount += 1;
        //        }
        //    }


        //var gridCol = document.getElementById("GridViewPortfolio").getElementsByTagName("ID");
        //if (gridCol.length > 0) {
        //    var rowCOunt = 0;
        //    while (rowCOunt < gridCol.length) {
        //        alert(gridCol[rowCOunt].style.backgroundColor);
        //        if (gridCol[rowCOunt].style.backgroundColor == "Light Gray") {
        //            window.scrollTo(0, gridCol[rowCOunt].offsetTop);
        //            break;
        //        }
        //        rowCOunt += 1;
        //    }
        //}

        //};

        function adjustheaderwidths(tablenum) {

            var dataGridObj = document.getElementsByTagName("table")[tablenum];
            //get all tr elements in array
            var allRows = dataGridObj.getElementsByTagName("tr");

            //0th row is title & 1st row is header
            var headerRow = allRows[1].getElementsByTagName("th");

            var dataRow = allRows[2].getElementsByTagName("td");

            allRows[1].style.width = allRows[2].clientWidth + 'px';
            allRows[1].style.border = 'none';

            for (var iCntr = 0; iCntr < headerRow.length; iCntr++) {
                headerRow[iCntr].style.width = dataRow[iCntr].clientWidth + 'px';
                dataRow[iCntr].style.width = headerRow[iCntr].clientWidth + 'px';
            }
        }
        function adjustgrids(e) {
            adjustheaderwidths(0);
        }

        //window.onresize = adjustgrids;



    </script>
    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive">
                <div class="container table-responsive" style="width=100%; padding-top: 1%; text-align: center; position: fixed; background-color: #c2c2c2;">
                    <asp:Button ID="ButtonAddNew" runat="server" Text="Add New" OnClick="ButtonAddNew_Click" />
                    <asp:Button ID="ButtonEdit" runat="server" Text="Edit" OnClick="ButtonEdit_Click" />
                    <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete" OnClick="buttonDeleteSelectedScript_Click" />
                    <asp:Button ID="buttonGetQuote" runat="server" Text="Get Quote & Add" OnClick="buttonGetQuote_Click" />
                    <asp:Button ID="buttonValuation" runat="server" Text="Portfolio Valuation" OnClick="buttonValuation_Click" />
                    <asp:Button ID="buttonBack" runat="server" Text="Back" OnClick="buttonBack_Click" />
                    <div style="width=100%; padding-top: 2px; padding-bottom: 2px;">
                        <asp:Label ID="Label4" CssClass="text-right" runat="server" Text="Company Name :" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblCompName" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <br />
                        <%--<asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Selected Script:" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                        <asp:Label ID="Label5" CssClass="text-right" runat="server" Text="Symbol:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblScript" CssClass="text-left" runat="server" Text="Selected script name will appear here" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <%--<asp:Label ID="Label4" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>--%>
                        <br />
                        <asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Exchange:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblExchange" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <br />
                        <asp:Label ID="Label7" CssClass="text-right" runat="server" Text="Investment Type:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblInvestmentType" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="false"></asp:Label>

                        <br />
                        <asp:Label ID="Label3" CssClass="text-right" runat="server" Text="Purchase Date:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblDate" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="false"></asp:Label>
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
                            <asp:ListItem Value="VALUATION_LINE">Portfolio - Valuation</asp:ListItem>
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
                <br />
                <br />


                <div class="container">
                    <%--OnSelectedIndexChanged="GridViewPortfolio_SelectedIndexChanged"--%>
                    <%--CssClass="table table-striped table-bordered table-hover serh-grid"--%>
                    <asp:Panel ID="panelSummary" runat="server" Width="100%" ScrollBars="Auto">
                        <asp:GridView ID="GridViewSummary" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover serh-grid"
                            Width="100%" ShowHeaderWhenEmpty="true" HorizontalAlign="Center" OnRowCommand="GridViewSummary_RowCommand">
                            <%--Caption="Portfolio Summary" CaptionAlign="Top">--%>
                            <Columns>
                                <%--<asp:BoundField DataField="FundHouse" SortExpression="FundHouse" ItemStyle-HorizontalAlign="Center" />--%>

                                <asp:BoundField DataField="COMP_NAME" HeaderText="Company Name" SortExpression="COMP_NAME" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="SYMBOL" HeaderText="Symbol" SortExpression="SYMBOL" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="EXCHANGE" HeaderText="Exchange" SortExpression="EXCHANGE" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="CumQty" HeaderText="Total Qty" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="CumCost" HeaderText="Investment Cost" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="QuoteDt" HeaderText="Quote Date" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="Quote" HeaderText="Quote" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="CurrVal" HeaderText="Valuation" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="CumYearsInvested" HeaderText="Years" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="CumARR" HeaderText="ARR" ItemStyle-HorizontalAlign="Center" />
                            </Columns>

                            <SelectedRowStyle CssClass="grid-sltrow" />
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" ForeColor="White" BorderStyle="Solid"
                                BorderWidth="1px" BorderColor="Black" />

                        </asp:GridView>

                    </asp:Panel>

                    <asp:Panel ID="panel1" runat="server" Width="100%" ScrollBars="Auto">
                        <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover serh-grid"
                            Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No transactions!" HorizontalAlign="Center"
                            OnRowDataBound="grdViewOrders_RowDataBound"
                            OnRowCreated="grdViewOrders_RowCreated" OnRowCommand="grdViewOrders_RowCommand">
                            <Columns>
                                <%--<asp:BoundField DataField="CompanyName" HeaderText="Comp Name" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Name" HeaderText="Symbol" ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:BoundField DataField="PURCHASE_DATE" HeaderText="Purchase Date" SortExpression="PURCHASE_DATE"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PURCHASE_PRICE" HeaderText="Purchase Price" SortExpression="PURCHASE_PRICE"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PURCHASE_QTY" HeaderText="Purchase Qty" SortExpression="PURCHASE_QTY"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="COMMISSION_TAXES" HeaderText="Comm & Taxes" ConvertEmptyStringToNull="true" NullDisplayText="0.00" SortExpression="CommissionPaid"
                                    ItemStyle-HorizontalAlign="Center" />
                                <%--<asp:TemplateField HeaderText="Commission+Taxes"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("COMMISSION_TAXES","{0}") != "0.00") ? Eval("COMMISSION_TAXES","{0:0.00}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>


                                <asp:BoundField DataField="INVESTMENT_COST" HeaderText="Cost" SortExpression="INVESTMENT_COST"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CURRENTDATE" HeaderText="Current Date" SortExpression="CURRENTDATE"
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
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <%--<div>
        <asp:TreeView ID="TreeViewPortfolio" runat="server" NodeWrap="True" ExpandDepth="1" ShowLines="True"></asp:TreeView>
    </div>--%>
</asp:Content>
