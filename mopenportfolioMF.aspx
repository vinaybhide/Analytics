<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mopenportfolioMF.aspx.cs" Inherits="Analytics.mopenportfolioMF" EnableEventValidation="false" MaintainScrollPositionOnPostback="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gridheader {
            text-align: center;
        }

        .FixedHeader {
            position: fixed;
            font-weight: normal;
            width: 100%;
        }

        .grid-sltrow {
            /*background: Gray;*/ /*#ddd;*/
            background: #d7d5d5;
            /*background: Orange;*/
            font-weight: bold;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            /*background-color: cadetblue;*/ /*#D8D8D8;*/
            /*background-color:  #019AC1;*/ /*#238AA4;*/
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
            /*background-color: #4682B4;*/
            /*color: #ffffff;*/
            background-color: #e1b94a;
            color: #000000;
            font-weight: bold;
        }

        .FundNameHeaderStyle {
            border: solid 1px Black;
            /*background-color: #4682B4;*/
            /*color: #ffffff;*/
            background-color: #f2dc9b;
            color: #000000;
            font-weight: bold;
        }

        .FundHouseSubTotalStyle {
            border: solid 1px Black;
            /*background-color: cadetblue;*/ /*background-color: lightblue;*/ /*#4682B4;*/
            /*background-color: #019AC1;*/ /*#238AA4;*/
            /*color: #ffffff;*/
            color: black;
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

    <script type="text/javascript">
        //document.addEventListener("DOMContentLoaded", function (event) {
        //    var scrollpos = localStorage.getItem('scrollpos');
        //    if (scrollpos) window.scrollTo(0, scrollpos);
        //});

        //window.onbeforeunload = function (e) {
        //    localStorage.setItem('scrollpos', window.scrollY);
        //};


        document.addEventListener("DOMContentLoaded", function (event) {
            var scrollpos = sessionStorage.getItem('mfportscrollpos');
            var name = ' <%= Session["MFPORTFOLIOMASTERROWID"] %>'
            var mfrowid = sessionStorage.getItem('mfportfoliomasterrowid');
            if (mfrowid) {
                if (mfrowid == name) {
                    if (scrollpos) {
                        window.scrollTo(0, scrollpos);
                        sessionStorage.removeItem('mfportscrollpos');
                        sessionStorage.removeItem('mfportfoliomasterrowid');
                    }
                }
            }

        });

        //function not used - trial for calling it from page load
        function setpageposition() {
            var scrollpos = sessionStorage.getItem('mfportscrollpos');
            var name = ' <%= Session["MFPORTFOLIOMASTERROWID"] %>'
            var mfrowid = sessionStorage.getItem('mfportfoliomasterrowid');
            if (mfrowid) {
                if (mfrowid == name) {
                    if (scrollpos) {
                        window.scrollTo(0, scrollpos);
                        sessionStorage.removeItem('mfportscrollpos');
                        sessionStorage.removeItem('mfportfoliomasterrowid');
                    }
                }
            }
        }

        window.addEventListener("beforeunload", function (e) {
            sessionStorage.setItem('mfportscrollpos', window.scrollY);
            var name = ' <%= Session["MFPORTFOLIOMASTERROWID"] %>'
            sessionStorage.setItem('mfportfoliomasterrowid', name);
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
            sessionStorage.setItem('mfportscrollpos', (htsummary + numoftransactionrows[actualindex].offsetTop));

            //if (actualindex < numoftransactionrows.length) {
            //    alert('before scroll');
            //    window.scroll(0, mygridcol[actualindex].offsetTop);
            //    alert('before sessionstorage');
            //    sessionStorage.setItem('mfportscrollpos', mygridcol[actualindex].offsetTop);
            //    alert('after sessionstorage');
            //}
        }

        //function not used - sample
        function setscroll() {
            alert('in on load');
            var gridportfolio = document.getElementsByTagName("table")[1];
            alert('after gridviewportfolio');
            var mygridcol = gridportfolio.getElementsByTagName("tr");
            alert('after mygrid col len' + mygridcol.length);
            if (mygridcol.length > 0) {
                var mycount = 0;
                while (mycount < mygridcol.length) {
                    if (mygridcol[mycount].style.backgroundColor == "orange") {
                        alert(mygridcol[mycount].style.backgroundColor);
                        window.scroll(0, mygridcol[mycount].offsetTop);
                        break;
                    }
                    mycount += 1;
                }
            }


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

        };

    </script>

    <div class="row;">
        <div class="col-lg-12; ">
            <div class="table-responsive">
                <div class="container table-responsive" style="padding-top: 1%; text-align: center; position: fixed; background-color: #c2c2c2;">
                    <%--border: solid; border-color: black; border-width: 1px;">--%>
                    <asp:Button ID="ButtonAddNew" runat="server" Text="Add New" OnClick="ButtonAddNew_Click" />
                    <asp:Button ID="ButtonEdit" runat="server" Text="Edit" OnClick="ButtonEdit_Click" />
                    <asp:Button ID="buttonDeleteSelectedScript" runat="server" Text="Delete" OnClick="buttonDeleteSelectedScript_Click" />
                    <asp:Button ID="buttonBack" runat="server" Text="Back" OnClick="buttonBack_Click" />
                    <%--<asp:Button ID="buttonValuationLine" runat="server" Text="Valuation (Line Graph)" OnClick="buttonValuationLine_Click" />
                    <asp:Button ID="buttonValuation" runat="server" Text="Valuation (Bar Graph)" OnClick="buttonValuation_Click" />--%>
                    <div style="padding-top: 2px; padding-bottom: 2px;">
                        <%--<asp:Label ID="Label1" CssClass="text-right" runat="server" Text="Selected MF:" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                        <asp:Label ID="Label2" CssClass="text-center" runat="server" Text="Fund House: " ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblFundHouse" CssClass="text-center" runat="server" Text="" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <asp:Label ID="lblFundHouseCode" Visible="false" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="Label4" CssClass="text-center" runat="server" Text="Fund Name: " ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblScript" CssClass="text-center" runat="server" Text="" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <%--<asp:Label ID="Label4" CssClass="text-right" runat="server" Text="&nbsp&nbsp&nbsp"></asp:Label>--%>
                        <br />
                        <asp:Label ID="Label5" CssClass="text-center" runat="server" Text="Scheme Code: " ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblSchemeCode" CssClass="text-center" runat="server" Text="" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <br />
                        <asp:Label ID="Label3" CssClass="text-right" runat="server" Text="Purchase Date:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblDate" CssClass="text-left" runat="server" Text="None" ForeColor="Black" Font-Bold="false"></asp:Label>
                        <br />
                        <asp:Label ID="Label1" CssClass="text-left" runat="server" Text="Standard Indicators:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="ddlGrphType" runat="server" OnSelectedIndexChanged="ddlGrphType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="-1">-- Select graph --</asp:ListItem>
                            <asp:ListItem Value="DAILY_NAV">Daily NAV</asp:ListItem>
                            <asp:ListItem Value="SMA Fast">SMA Fast</asp:ListItem>
                            <asp:ListItem Value="SMA Slow">SMA Slow</asp:ListItem>
                            <asp:ListItem Value="EMA Fast">EMA Fast</asp:ListItem>
                            <asp:ListItem Value="EMA Slow">EMA Slow</asp:ListItem>
                            <asp:ListItem Value="WMA Fast">WMA Fast</asp:ListItem>
                            <asp:ListItem Value="WMA Slow">WMA Slow</asp:ListItem>
                            <asp:ListItem Value="Upper Band">BBANDS</asp:ListItem>
                            <asp:ListItem Value="MACD">MACD</asp:ListItem>
                            <asp:ListItem Value="RSI">RSI</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label6" CssClass="text-left" runat="server" Text="Advance Indicators:" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="ddlAdvGrphType" runat="server" OnSelectedIndexChanged="ddlAdvGrphType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="-1">-- Select graph --</asp:ListItem>
                            <asp:ListItem Value="BACKTEST">Back Test and Forecast</asp:ListItem>
                            <asp:ListItem Value="BAR Graph">Portfolio: Cost Vs Value</asp:ListItem>
                            <asp:ListItem Value="VALUE Graph">Portfolio: Valuation</asp:ListItem>
                        </asp:DropDownList>

                        <%--<asp:Button ID="buttonShowGraph" runat="server" Text="Show Graph" OnClick="buttonShowGraph_Click"/>--%>
                    </div>
                </div>
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
                    <asp:GridView ID="GridViewSummary" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover serh-grid"
                        Width="100%" ShowHeaderWhenEmpty="true" HorizontalAlign="Center" OnRowCommand="GridViewSummary_RowCommand">
                        <%--Caption="Portfolio Summary" CaptionAlign="Top">--%>
                        <Columns>
                            <%--<asp:BoundField DataField="FundHouse" SortExpression="FundHouse" ItemStyle-HorizontalAlign="Center" />--%>

                            <asp:BoundField DataField="FundName" SortExpression="FundName" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="CumUnits" HeaderText="Cumulative Units" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="CumCost" HeaderText="Cumulative Cost" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="CurrVal" HeaderText="Valuation" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="YearsInvested" HeaderText="Years Invested" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="ARR" HeaderText="ARR" ItemStyle-HorizontalAlign="Center" />
                        </Columns>

                        <SelectedRowStyle CssClass="grid-sltrow" />
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderStyle="Solid"
                            BorderWidth="1px" BorderColor="Black" />

                    </asp:GridView>

                    <asp:GridView ID="GridViewPortfolio" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover serh-grid"
                        Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No Transactions!" HorizontalAlign="Center"
                        OnRowDataBound="grdViewOrders_RowDataBound"
                        OnRowCreated="grdViewOrders_RowCreated" OnRowCommand="grdViewOrders_RowCommand">
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
                                    <%# (Eval("CurrentNAV","{0}") != "0") ? Eval("CurrentNAV","{0:0.00}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="NAVdate" HeaderText="NAV Date" SortExpression="NAVdate"
                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />--%>
                            <asp:TemplateField HeaderText="NAV Date" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("NAVdate","{0}") != DateTime.MinValue.ToString() ) ? Eval("NAVdate","{0:dd-MM-yyyy}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ValueAtCost" HeaderText="Cost" SortExpression="ValueAtCost"
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
                                    <%# (Eval("CurrentValue","{0}") != "0") ? Eval("CurrentValue","{0:0.00}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="YearsInvested" HeaderText="Years Invested" SortExpression="YearsInvested"
                                ItemStyle-HorizontalAlign="Center" />--%>
                            <asp:TemplateField HeaderText="Years Invested" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("YearsInvested","{0}") != "0") ? Eval("YearsInvested","{0:0.00}") : "NA" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="ARR" HeaderText="ARR" SortExpression="ARR"
                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:0.00%}" />--%>

                            <asp:TemplateField HeaderText="ARR" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# (Eval("ARR","{0}") != "0") ? Eval("ARR","{0:0.00%}") : "NA" %>
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
    <%--<div>
        <asp:TreeView ID="TreeViewPortfolio" runat="server" NodeWrap="True" ExpandDepth="1" ShowLines="True"></asp:TreeView>
    </div>--%>
</asp:Content>
