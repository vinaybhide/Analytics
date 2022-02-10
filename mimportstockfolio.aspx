<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="mimportstockfolio.aspx.cs" Inherits="Analytics.mimportstockfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .info-label {
            background-color: #5bc0de
        }
    </style>

    <table style="width: 100%; margin-top: 2%;">
        <tr>
            <td colspan="3" style="text-align: center; border: solid 1px black;" class="info-label">
                <asp:Label ID="Label48" runat="server" Font-Size="Large" Text="Import Stock Portfolio"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px black; margin-bottom: 1px; margin-top: 5px;">
        <tr style="width: 100%;">
            <td colspan="8" style="width: 100%">
                <asp:Label runat="server" Text="Import Process" Font-Size="Large"></asp:Label>
            </td>
        </tr>
        <tr style="width: 100%;">
            <td style="text-align: left;">
                <asp:Label ID="Label4" runat="server" Font-Italic="true" Text="1. Select file properties"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label9" runat="server" Font-Italic="true" Text="2. Select file"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label5" runat="server" Font-Italic="true" Text="3. Upload file"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label6" runat="server" Font-Italic="true" Text="4. Map columns"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label10" runat="server" Font-Italic="true" Text="5. Merge data"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label7" runat="server" Font-Italic="true" Text="6. Verify & Map mandatory fields"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label12" runat="server" Font-Italic="true" Text="7. Portfolio Name"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:Label ID="Label13" runat="server" Font-Italic="true" Text="8. Import data"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px black;">
        <tr>
            <td style="padding-left: 5px; width: 40%; border-right: solid 1px black;">
                <asp:Panel ID="panelFileProperties" runat="server">
                    <asp:Label ID="Label20" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 1: File Properties"></asp:Label>
                    <br />
                    <asp:Label ID="Label1" runat="server" CssClass="text-right" Text="Select file tyle:"></asp:Label>
                    <asp:DropDownList ID="ddlfiletype" runat="server">
                        <asp:ListItem Value="-1" Selected="True">Select file type</asp:ListItem>
                        <asp:ListItem Value="TEXT">Text file</asp:ListItem>
                        <asp:ListItem Value="EXCEL">MS-Excel file</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Label ID="Label3" runat="server" Style="text-align: right" Text="Does first line indicates column names?"></asp:Label>
                    <asp:DropDownList ID="ddlFirstLine" runat="server">
                        <asp:ListItem Value="-1" Selected="True">Select 1st row type</asp:ListItem>
                        <asp:ListItem Value="true">Yes</asp:ListItem>
                        <asp:ListItem Value="false">NO</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Label ID="Label8" runat="server" Style="text-align: right" Text="Field separator:"></asp:Label>
                    <asp:DropDownList ID="ddlFieldSeparator" runat="server">
                        <asp:ListItem Value="-1" Selected="True">Select field separator for text file</asp:ListItem>
                        <asp:ListItem Value="0">Comma (,)</asp:ListItem>
                        <asp:ListItem Value="1">Pipe (|)</asp:ListItem>
                        <asp:ListItem Value="2">Tab (\t)</asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
            </td>
            <td></td>
            <td style="padding-left: 5px; width: 30%; border-right: solid 1px black;">
                <asp:Panel ID="panelFileName" runat="server">
                    <asp:Label ID="Label14" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 2: File Selection"></asp:Label>
                    <br />
                    <asp:Label ID="Label2" runat="server" Style="text-align: left" Text="Select file:"></asp:Label>
                    <asp:FileUpload ID="FileUploadCSV" runat="server" />
                    <br />
                    <asp:Label runat="server"></asp:Label>
                </asp:Panel>
            </td>
            <td></td>
            <td style="padding-left: 5px; width: 30%;">
                <asp:Panel ID="panelReadSource" runat="server">
                    <asp:Label ID="Label15" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 3: Read Source File"></asp:Label>
                    <br />
                    <asp:Label runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Button ID="buttonReadSourceFile" runat="server" Text="Read File" OnClick="buttonReadSourceFile_Click" />
                    <asp:Label runat="server" Text=""></asp:Label>
                    <asp:Button ID="buttonBack" runat="server" Text="Back" OnClick="buttonBack_Click" />
                    <br />
                    <br />
                    <asp:Label runat="server" Text=""></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table style="width: 100%; margin-top: 5px; margin-bottom: 5px;">
        <tr>
            <td style="margin-right: 5px;">
                <asp:Panel ID="panelSourceFileData" runat="server" Enabled="false" Visible="false">
                    <asp:Label ID="Label16" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Source File Data"></asp:Label>
                    <br />
                    <asp:GridView CssClass="text-center" HeaderStyle-Font-Size="Small" RowStyle-Font-Size="Small" ID="gvSource" runat="server"
                        HeaderStyle-HorizontalAlign="Center" AlternatingRowStyle-BackColor="LightGray">
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                        <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                        <RowStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    </asp:GridView>
                </asp:Panel>
            </td>
            <td style="padding-left: 5px;">
                <asp:Panel ID="panelColumnMap" runat="server" Enabled="false" Visible="false">
                    <asp:Label ID="Label17" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 4: Map Columns (Click Target column to edit)"></asp:Label>
                    <br />

                    <asp:GridView CssClass="text-center" ID="gvMappedColumns" runat="server" AutoGenerateColumns="False"
                        OnRowCancelingEdit="gvMappedColumns_RowCancelingEdit"
                        OnRowEditing="gvMappedColumns_RowEditing" OnRowUpdating="gvMappedColumns_RowUpdating" OnRowDataBound="gvMappedColumns_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Target Column" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblTargetColName" runat="server" Text='<%# Eval("TARGET_COLUMN") %>'>
                                    </asp:Label>--%>
                                    <asp:LinkButton ID="lblTargetColName" runat="server" CommandName="Edit" class="btn-link" Font-Underline="true" Text='<%# Eval("TARGET_COLUMN") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Source Column">
                                <ItemTemplate>
                                    <asp:Label ID="lblSourceColName" runat="server" Text='<%# Eval("SOURCE_COLUMN") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlgvSourceColNames" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlgvSourceColNames_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" Visible="false" ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" class="btn btn-primary btn-sm" Text="Edit&raquo;"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" class="btn btn-primary btn-sm" Text="Map&raquo;"></asp:LinkButton>
                                    <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" class="btn btn-primary btn-sm" Text="Un-Map&raquo;"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                        <RowStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <table style="margin-top: 0px; padding-top: -10px; width: 100%; border: solid 1px black;">
        <tr>
            <asp:Panel ID="panelValidateSymbols" runat="server" Enabled="false" Visible="false">

                <td style="width: 100%; text-align: center;">
                    <asp:Label ID="Label18" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 5: Merge Data"></asp:Label>
                    <br />
                    <asp:Button ID="buttonValidateSymbols" runat="server" Text="Merge Data" OnClick="buttonValidateSymbols_Click" />
                </td>
            </asp:Panel>
        </tr>
    </table>

    <table style="margin-top: 5px; width: 100%;">
        <tr>
            <asp:Panel ID="panelMergedData" runat="server" Enabled="false" Visible="false">
                <td style="width: 100%;">
                    <asp:Label ID="Label19" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 6: Verification & correction for mandatory fields. Each row status should be MAPPED. Click Symbol field to edit/map or delete"></asp:Label>
                    <br />

                    <asp:GridView ID="gvMergedData" runat="server" AutoGenerateColumns="False"
                        OnRowCancelingEdit="gvMergedData_RowCancelingEdit" OnRowEditing="gvMergedData_RowEditing" OnRowDeleting="gvMergedData_RowDeleting"
                        OnRowUpdating="gvMergedData_RowUpdating" OnRowDataBound="gvMergedData_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSuccess" runat="server" Text='<%# Eval("STATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SYMBOL (Source)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSourceSymbol" runat="server" Text='<%# Eval("SOURCE_SYMBOL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SYMBOL (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblMappedSymbol" runat="server" Text='<%# Eval("MAPPED_SYMBOL") %>'></asp:Label>--%>
                                    <asp:LinkButton ID="lblMappedSymbol" runat="server" CommandName="Edit" class="btn-link" Font-Underline="true" Text='<%# Eval("MAPPED_SYMBOL") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlMappedSymbol" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMappedSymbol_SelectedIndexChanged"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="COMP NAME (Source)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSourceCompName" runat="server" Text='<%# Eval("SOURCE_COMP_NAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="COMP NAME (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblMappedCompName" runat="server" Text='<%# Eval("MAPPED_COMP_NAME") %>'></asp:Label>--%>
                                    <asp:LinkButton ID="lblMappedCompName" runat="server" CommandName="Edit" class="btn-link" Font-Underline="true" Text='<%# Eval("MAPPED_COMP_NAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlMappedCompName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMappedCompName_SelectedIndexChanged"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="EXCHANGE (Source)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSourceExchange" runat="server" Text='<%# Eval("SOURCE_EXCHANGE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="EXCHANGE (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblMappedExchange" runat="server" Text='<%# Eval("MAPPED_EXCHANGE") %>'></asp:Label>--%>
                                    <asp:LinkButton ID="lblMappedExchange" runat="server" CommandName="Edit" class="btn-link" Font-Underline="true" Text='<%# Eval("MAPPED_EXCHANGE") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlMappedExchange" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMappedExchange_SelectedIndexChanged"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PURCHASE DATE (Source)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSourcePurchaseDate" runat="server" Text='<%# Eval("SOURCE_PURCHASE_DATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PURCHASE DATE (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblMappedPurchaseDate" runat="server" Text='<%# Eval("MAPPED_PURCHASE_DATE") %>'></asp:Label>--%>
                                    <asp:LinkButton ID="lblMappedPurchaseDate" runat="server" CommandName="Edit" class="btn-link" Font-Underline="true" Text='<%# Eval("MAPPED_PURCHASE_DATE") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="textboxMappedPurchaseDate" runat="server" TextMode="Date" Text='<%# Eval("MAPPED_PURCHASE_DATE").ToString().Equals("NOT MAPPED") ? "NOT MAPPED" : System.Convert.ToDateTime(Eval("MAPPED_PURCHASE_DATE").ToString()).ToString("yyyy-MM-dd")%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PURCHASE QTY (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMappedPurchaseQty" runat="server" Text='<%# Eval("MAPPED_PURCHASE_QTY") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:TextBox ID="textboxMappedPurchaseQty" runat="server" Text='<%# Eval("MAPPED_PURCHASE_QTY")%>'></asp:TextBox>
                                </EditItemTemplate>--%>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PURCHASE PRICE (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMappedPurchasePrice" runat="server" Text='<%# Eval("MAPPED_PURCHASE_PRICE") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:TextBox ID="textboxMappedPurchasePrice" runat="server" Text='<%# Eval("MAPPED_PURCHASE_PRICE")%>'></asp:TextBox>
                                </EditItemTemplate>--%>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="HOLDING COST (Mapped)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMappedCommissionTaxes" runat="server" Text='<%# Eval("MAPPED_HOLDING_VALUE") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:TextBox ID="textboxMappedCommissionTaxes" runat="server" Text='<%# Eval("MAPPED_HOLDING_COST")%>'></asp:TextBox>
                                </EditItemTemplate>--%>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Edit" ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn-sm" Text="Edit&raquo;"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnupdatemerge" runat="server" CommandName="Update" CssClass="btn btn-primary btn-sm" Text="Update&raquo;"></asp:LinkButton>
                                    <asp:LinkButton ID="btncancelmerge" runat="server" CommandName="Cancel" CssClass="btn btn-primary btn-sm" Text="Cancel&raquo;"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btndeleterow" runat="server" CommandName="Delete" CssClass="btn btn-primary btn-sm" Text="Delete&raquo;"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                        <RowStyle Font-Size="Smaller" HorizontalAlign="Center" />
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </td>
            </asp:Panel>
        </tr>
    </table>
    <table style="margin-top: 5px; width: 100%;">
        <tr style="text-align: center;">
            <asp:Panel ID="panelPortfolioName" runat="server" Enabled="false" Visible="false">
                <td style="width: 50%; border: solid 1px black;">
                    <asp:Label ID="Label21" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 7: Portfolio Name"></asp:Label>
                    <br />
                    <asp:RadioButtonList ID="rblistPortfolioName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblistPortfolioName_SelectedIndexChanged">
                        <asp:ListItem Text="Create New" Value="NEW" Selected="True" />
                        <asp:ListItem Text="Append to existing" Value="APPEND" />
                    </asp:RadioButtonList>
                    <asp:Label ID="lblNewPortfolioName" runat="server" Font-Size="Small" Style="text-align: right" Text="New Portfolio Name:"></asp:Label>
                    <asp:TextBox ID="textboxPortfolioName" runat="server" Font-Size="Small" Text="" AutoPostBack="true" OnTextChanged="textboxPortfolioName_TextChanged"></asp:TextBox>

                    <asp:Label ID="lblExistingPortfolioName" runat="server" Enabled="false" Visible="false" Font-Size="Small" Style="text-align: right" Text="Select Portfolio :"></asp:Label>
                    <asp:DropDownList ID="ddlExistingPortfolioName" runat="server" Enabled="false" Visible="false" ></asp:DropDownList>

                </td>
            </asp:Panel>
            <asp:Panel ID="panelImport" runat="server" Enabled="false" Visible="false">

                <td style="width: 50%; text-align: center; border: solid 1px black;">
                    <asp:Label ID="Label22" runat="server" CssClass="text-center label-info" Font-Size="Larger" Text="Step 8: Import rows with MAPPED status"></asp:Label>
                    <br />

                    <asp:Button ID="buttonImportFile" runat="server" Font-Size="Small" Text="Import Mapped Data" OnClick="buttonImportFile_Click" />
                </td>
            </asp:Panel>
        </tr>
    </table>
</asp:Content>
