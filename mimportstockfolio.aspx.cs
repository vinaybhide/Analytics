using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class mimportstockfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EMAILID"] == null)
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
            if (!IsPostBack)
            {
                ViewState["SOURCEDATA"] = null;
            }
        }

        protected void buttonReadSourceFile_Click(object sender, EventArgs e)
        {
            try
            {
                if ((FileUploadCSV.HasFile) && (ddlfiletype.SelectedIndex > 0) && (ddlFirstLine.SelectedIndex > 0))
                {
                    int fileSize = FileUploadCSV.PostedFile.ContentLength;
                    // If file size is greater than 2 MB
                    if (fileSize <= 2097152)
                    {
                        StockManager stockManager = new StockManager();
                        DataTable sourceData = null;
                        ViewState["SOURCEDATA"] = null;
                        if (ddlfiletype.SelectedIndex == 1)
                        {
                            //text based file
                            if (ddlFieldSeparator.SelectedIndex > 0)
                            {
                                Stream receiveStream = FileUploadCSV.FileContent;
                                StreamReader reader = null;
                                reader = new StreamReader(receiveStream);
                                char separatorChar = ddlFieldSeparator.SelectedIndex == 1 ? ',' : ddlFieldSeparator.SelectedIndex == 2 ? '|' : '\t';
                                sourceData = stockManager.readSourceCSV(reader, System.Convert.ToBoolean(ddlFirstLine.SelectedValue), separatorChar);
                                if ((sourceData != null) && (sourceData.Rows.Count > 0))
                                {
                                    ViewState["SOURCEDATA"] = sourceData;
                                    ViewState["SOURCE_FILE_NAME"] = FileUploadCSV.FileName.Split('.')[0];
                                    textboxPortfolioName.Text = ViewState["SOURCE_FILE_NAME"].ToString();
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select field separator for text file import');", true);
                            }

                        }
                        else
                        {
                            //handle excel
                        }
                        if (ViewState["SOURCEDATA"] != null)
                        {
                            panelSourceFileData.Enabled = true;
                            panelSourceFileData.Visible = true;

                            gvSource.DataSource = sourceData;
                            gvSource.DataBind();
                            //LoadColumnMappers();

                            LoadSourceTargetColumns();
                            ShowColumnMapperGrid();
                            panelValidateSymbols.Enabled = true;
                            panelValidateSymbols.Visible = true;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('File size must be less than or equal to 2MB');", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select file, file type and first line contents');", true);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/mselectportfolio.aspx");
            //StockManager stockManager = new StockManager();
            //if (stockManager.getPortfolioCount(Session["EMAILID"].ToString()) > 0)
            //    Response.Redirect("~/mselectportfolio.aspx");
            //else
            //    Response.Redirect("~/mnewportfolio.aspx");
        }

        //public void LoadColumnMappers()
        //{
        //    //method to populate the source & target coluns drop downs
        //    DataTable tableSourceData, tableSourceColumns, tableTargetColumns;
        //    try
        //    {
        //        if(ViewState["SOURCEDATA"] != null)
        //        {
        //            panelColumnMap.Enabled = true;
        //            panelColumnMap.Visible = true;
        //            //First get the names of source data columns in a table
        //            tableSourceData = (DataTable)ViewState["SOURCEDATA"];
        //            tableSourceColumns = new DataTable();
        //            tableSourceColumns.Columns.Add("SOURCE_COLUMN", typeof(string));
        //            string[] arraySourceColNames = tableSourceData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
        //            for (int i = 0; i < arraySourceColNames.Length; i++)
        //            {
        //                tableSourceColumns.Rows.Add(arraySourceColNames[i]);
        //            }

        //            //now map this table to dropdown showing source col names
        //            ddlSourceColumns.DataTextField = "SOURCE_COLUMN";
        //            ddlSourceColumns.DataValueField = "SOURCE_COLUMN";
        //            ddlSourceColumns.DataSource = tableSourceColumns;
        //            ddlSourceColumns.DataBind();

        //            //Now add the target columns from our portfolio
        //            tableTargetColumns = new DataTable();
        //            tableTargetColumns.Columns.Add("TARGET_COLUMN", typeof(string));
        //            string[] arrayTargetColNames = { "Exchange", "COMP_NAME", "SYMBOL", "PURCHASE_DATE", "PURCHASE_PRICE", "PURCHASE_QTY", "COMMISSION_TAXES" };
        //            for (int i = 0; i < arrayTargetColNames.Length; i++)
        //            {
        //                tableTargetColumns.Rows.Add(arrayTargetColNames[i]);
        //            }
        //            ddlTargetColumns.DataTextField = "TARGET_COLUMN";
        //            ddlTargetColumns.DataValueField = "TARGET_COLUMN";
        //            ddlTargetColumns.DataSource = tableTargetColumns;
        //            ddlTargetColumns.DataBind();
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        public void LoadSourceTargetColumns()
        {
            //method to populate the source & target coluns drop downs
            DataTable tableSourceData, tableSourceColumns, tableTargetColumns;
            try
            {
                if (ViewState["SOURCEDATA"] != null)
                {
                    panelColumnMap.Enabled = true;
                    panelColumnMap.Visible = true;
                    //First get the names of source data columns in a table
                    tableSourceData = (DataTable)ViewState["SOURCEDATA"];
                    tableSourceColumns = new DataTable();
                    tableSourceColumns.Columns.Add("SOURCE_COLUMN", typeof(string));

                    //following gives all column names in an array
                    string[] arraySourceColNames = tableSourceData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

                    //tableSourceColumns.Rows.Add("NOT MAPPED");
                    tableSourceColumns.Rows.Add("Select Source Column");
                    for (int i = 0; i < arraySourceColNames.Length; i++)
                    {
                        tableSourceColumns.Rows.Add(arraySourceColNames[i]);
                    }


                    ViewState["SOURCE_COLUMN_NAMES"] = tableSourceColumns;

                    //Now add the target columns from our portfolio
                    tableTargetColumns = new DataTable();
                    tableTargetColumns.Columns.Add("TARGET_COLUMN", typeof(string));
                    DataColumn col2 = new DataColumn("SOURCE_COLUMN", typeof(string));
                    col2.DefaultValue = "NOT MAPPED";
                    tableTargetColumns.Columns.Add(col2);

                    string[] arrayTargetColNames = { "EXCHANGE", "COMP_NAME", "SYMBOL", "PURCHASE_DATE", "PURCHASE_QTY", "PURCHASE_PRICE", "HOLDING_VALUE"};
                    for (int i = 0; i < arrayTargetColNames.Length; i++)
                    {
                        tableTargetColumns.Rows.Add(arrayTargetColNames[i]);
                    }
                    ViewState["TARGET_SOURCE_COLUMN_NAMES"] = tableTargetColumns;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
        }

        public void ShowColumnMapperGrid()
        {
            //we will use ViewState["TARGET_SOURCE_COLUMN_NAMES] to load the initial grid view
            if (ViewState["TARGET_SOURCE_COLUMN_NAMES"] != null)
            {
                DataTable tableColumnMapper = (DataTable)ViewState["TARGET_SOURCE_COLUMN_NAMES"];
                gvMappedColumns.DataSource = tableColumnMapper;
                gvMappedColumns.DataBind();
            }
        }

        /// <summary>
        /// Method gets called when a record is added to grid view. We use it to populate control value in individual rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMappedColumns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlsourcelist = (DropDownList)e.Row.FindControl("ddlgvSourceColNames");
                    DataTable tableSourceColumns = (DataTable)ViewState["SOURCE_COLUMN_NAMES"];
                    ddlsourcelist.DataTextField = "SOURCE_COLUMN";
                    ddlsourcelist.DataValueField = "SOURCE_COLUMN";
                    ddlsourcelist.DataSource = tableSourceColumns;
                    ddlsourcelist.DataBind();
                }
            }
        }

        protected void gvMappedColumns_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable tableColumnMapper = (DataTable)ViewState["TARGET_SOURCE_COLUMN_NAMES"];
            Label lblTargetColumnName = (Label)gvMappedColumns.Rows[e.RowIndex].FindControl("lblTargetColName");
            DropDownList ddlsourcecollist = (DropDownList)gvMappedColumns.Rows[e.RowIndex].FindControl("ddlgvSourceColNames");

            DataRow[] selectedRow = tableColumnMapper.Select("TARGET_COLUMN = '" + lblTargetColumnName.Text + "'");
            selectedRow[0]["SOURCE_COLUMN"] = ddlsourcecollist.Items[0].Value;
            tableColumnMapper.AcceptChanges();

            ViewState["TARGET_SOURCE_COLUMN_NAMES"] = tableColumnMapper;


            gvMappedColumns.EditIndex = -1;
            ShowColumnMapperGrid();
        }

        protected void gvMappedColumns_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMappedColumns.EditIndex = e.NewEditIndex;
            ShowColumnMapperGrid();
        }

        protected void gvMappedColumns_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable tableColumnMapper = (DataTable)ViewState["TARGET_SOURCE_COLUMN_NAMES"];
            Label lblTargetColumnName = (Label)gvMappedColumns.Rows[e.RowIndex].FindControl("lblTargetColName");
            DropDownList ddlsourcecollist = (DropDownList)gvMappedColumns.Rows[e.RowIndex].FindControl("ddlgvSourceColNames");

            //Now update the table with values from drop down selection
            DataRow[] selectedRow = tableColumnMapper.Select("TARGET_COLUMN = '" + lblTargetColumnName.Text + "'");
            selectedRow[0]["SOURCE_COLUMN"] = ddlsourcecollist.SelectedValue;
            tableColumnMapper.AcceptChanges();

            ViewState["TARGET_SOURCE_COLUMN_NAMES"] = tableColumnMapper;

            gvMappedColumns.EditIndex = -1;
            ShowColumnMapperGrid();
        }

        protected void ddlgvSourceColNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable tableColumnMapper = (DataTable)ViewState["TARGET_SOURCE_COLUMN_NAMES"];
            //Label lblTargetColumnName = (Label)gvMappedColumns.Rows[gvMappedColumns.EditIndex].FindControl("lblTargetColName");
            LinkButton lblTargetColumnName = (LinkButton)gvMappedColumns.Rows[gvMappedColumns.EditIndex].FindControl("lblTargetColName");
            DropDownList ddlsourcecollist = (DropDownList)gvMappedColumns.Rows[gvMappedColumns.EditIndex].FindControl("ddlgvSourceColNames");

            //Now update the table with values from drop down selection
            DataRow[] selectedRow = tableColumnMapper.Select("TARGET_COLUMN = '" + lblTargetColumnName.Text + "'");
            if (ddlsourcecollist.SelectedIndex > 0)
                selectedRow[0]["SOURCE_COLUMN"] = ddlsourcecollist.SelectedValue;
            else
                selectedRow[0]["SOURCE_COLUMN"] = "NOT MAPPED";
            tableColumnMapper.AcceptChanges();

            ViewState["TARGET_SOURCE_COLUMN_NAMES"] = tableColumnMapper;

            gvMappedColumns.EditIndex = -1;
            ShowColumnMapperGrid();
        }

        /// <summary>
        /// First load source data into our target data format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonValidateSymbols_Click(object sender, EventArgs e)
        {
            DataTable tableTarget = LoadSourceInToTarget();
            ViewState["MERGED_DATA"] = tableTarget;

            panelMergedData.Enabled = true;
            panelMergedData.Visible = true;

            panelPortfolioName.Enabled = true;
            panelPortfolioName.Visible = true;

            panelImport.Enabled = true;
            panelImport.Visible = true;
            FillSymbolList();
            ShowMergedDataGrid();
        }

        public DataTable LoadSourceInToTarget()
        {
            DataTable tableTarget = null;
            if ((ViewState["SOURCEDATA"] != null) && (ViewState["TARGET_SOURCE_COLUMN_NAMES"] != null))
            {
                //first create target table
                tableTarget = CreateTargetTable();
                DataRow newRow;
                DataRow[] filteredRows;
                //load data into target table
                DataTable tableSource = (DataTable)ViewState["SOURCEDATA"];
                DataTable tableTargetSourceColNames = (DataTable)ViewState["TARGET_SOURCE_COLUMN_NAMES"];
                foreach (DataRow sourceRow in tableSource.Rows)
                {
                    newRow = tableTarget.NewRow();
                    //find mapped column
                    for (int i = 0; i < tableSource.Columns.Count; i++)
                    {
                        filteredRows = tableTargetSourceColNames.Select("SOURCE_COLUMN = '" + tableSource.Columns[i].ColumnName + "'");
                        if ((filteredRows != null) && (filteredRows.Length > 0))
                        {
                            //we found matching source col name that was mapped
                            if (filteredRows[0]["TARGET_COLUMN"].ToString().Equals("EXCHANGE") ||
                                filteredRows[0]["TARGET_COLUMN"].ToString().Equals("SYMBOL") ||
                                filteredRows[0]["TARGET_COLUMN"].ToString().Equals("COMP_NAME") ||
                                filteredRows[0]["TARGET_COLUMN"].ToString().Equals("PURCHASE_DATE"))
                            {
                                newRow["SOURCE_" + filteredRows[0]["TARGET_COLUMN"].ToString()] = sourceRow[i];

                            }
                            newRow["MAPPED_" + filteredRows[0]["TARGET_COLUMN"].ToString()] = sourceRow[i];
                        }
                    }
                    tableTarget.Rows.Add(newRow);
                }
            }
            return tableTarget;
        }
        public DataTable CreateTargetTable()
        {
            DataTable tableTarget = new DataTable();

            DataColumn targetCol = new DataColumn("STATUS", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("SOURCE_COMP_NAME", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_COMP_NAME", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("SOURCE_SYMBOL", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_SYMBOL", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("SOURCE_EXCHANGE", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_EXCHANGE", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("SOURCE_PURCHASE_DATE", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_PURCHASE_DATE", typeof(string));
            targetCol.DefaultValue = "NOT MAPPED";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_PURCHASE_QTY", typeof(string));
            targetCol.DefaultValue = "0";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_PURCHASE_PRICE", typeof(string));
            targetCol.DefaultValue = "0.00";
            tableTarget.Columns.Add(targetCol);

            targetCol = new DataColumn("MAPPED_HOLDING_VALUE", typeof(string));
            targetCol.DefaultValue = "0.00";
            tableTarget.Columns.Add(targetCol);

            return tableTarget;
        }

        public void ShowMergedDataGrid()
        {
            if (ViewState["MERGED_DATA"] != null)
            {
                DataTable tableMerged = (DataTable)ViewState["MERGED_DATA"];

                gvMergedData.DataSource = tableMerged;
                gvMergedData.DataBind();
            }
        }

        public void FillSymbolList()
        {
            StockManager stockManager = new StockManager();
            DataTable tableStockMaster;

            tableStockMaster = stockManager.getStockMaster();

            if ((tableStockMaster != null) && (tableStockMaster.Rows.Count > 0))
            {
                ViewState["STOCKMASTER"] = tableStockMaster;
            }
        }

        public DataTable SearchPopulateStocksDropDown(string searchStr, bool bSearchCompName = true)
        {
            DataTable stockMaster = null;
            DataTable returnTable = null;
            try
            {
                if (ViewState["STOCKMASTER"] != null)
                {
                    stockMaster = (DataTable)ViewState["STOCKMASTER"];
                    if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                    {
                        StringBuilder filter = new StringBuilder();

                        if (!(string.IsNullOrEmpty(searchStr)))
                        {
                            if (bSearchCompName)
                                filter.Append("COMP_NAME Like '" + searchStr.Split(' ')[0] + "%'");
                            else
                                filter.Append("SYMBOL Like '" + searchStr.Split(' ')[0] + "%'");
                            DataView dv = stockMaster.DefaultView;
                            dv.RowFilter = filter.ToString();
                            if (dv.Count > 0)
                            {
                                returnTable = dv.ToTable();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
            }
            return returnTable;
        }

        public bool SetMappedColumnsInGV(object sender, GridViewRowEventArgs e)
        {
            bool breturn = false;
            StockManager stockManager = new StockManager();

            Label lblLocalSrcSymbol = (Label)e.Row.FindControl("lblSourceSymbol");
            Label lblLocalSuccess = (Label)e.Row.FindControl("lblSuccess");
            Label lblLocalExchange = (Label)e.Row.FindControl("lblSourceExchange");
            Label lblLocalCompany = (Label)e.Row.FindControl("lblSourceCompName");

            //Label lblLocalMappedCompany = (Label)e.Row.FindControl("lblMappedCompName");
            //Label lblLocalMappedSymbol = (Label)e.Row.FindControl("lblMappedSymbol");
            //Label lblLocalMappedExchange = (Label)e.Row.FindControl("lblMappedExchange");

            DropDownList ddlLocalCompany = (DropDownList)e.Row.FindControl("ddlMappedCompName");
            DropDownList ddlLocalExchange = (DropDownList)e.Row.FindControl("ddlMappedExchange");
            DropDownList ddlLocalSymbol = (DropDownList)e.Row.FindControl("ddlMappedSymbol");

            DataTable tableSearchStock = null;
            if (string.IsNullOrEmpty(lblLocalSrcSymbol.Text) == false)
                tableSearchStock = SearchPopulateStocksDropDown(lblLocalSrcSymbol.Text, bSearchCompName: false);
            else if (string.IsNullOrEmpty(lblLocalCompany.Text) == false)
                tableSearchStock = SearchPopulateStocksDropDown(lblLocalCompany.Text, bSearchCompName: true);
            if ((tableSearchStock != null) && (tableSearchStock.Rows.Count > 0))
            {
                //we found record(s) with matching symbol Populate with company & exchange dropdowns with the resuts
                ddlLocalCompany.Items.Clear();
                ddlLocalSymbol.Items.Clear();
                ddlLocalExchange.Items.Clear();
                ListItem li = new ListItem("Select Comp Name", "-1");
                ddlLocalCompany.Items.Add(li);

                li = new ListItem("Select symbol", "-1");
                ddlLocalSymbol.Items.Add(li);

                li = new ListItem("Select exchange", "-1");
                ddlLocalExchange.Items.Add(li);

                foreach (DataRow stockRow in tableSearchStock.Rows)
                {
                    li = new ListItem(stockRow["COMP_NAME"].ToString(), stockRow["COMP_NAME"].ToString() + "|" + stockRow["SYMBOL"].ToString() + "|" + stockRow["EXCHANGE"].ToString());
                    ddlLocalCompany.Items.Add(li);

                    li = new ListItem(stockRow["SYMBOL"].ToString(), stockRow["COMP_NAME"].ToString() + "|" + stockRow["SYMBOL"].ToString() + "|" + stockRow["EXCHANGE"].ToString());
                    ddlLocalSymbol.Items.Add(li);

                    li = new ListItem(stockRow["EXCHANGE"].ToString(), stockRow["COMP_NAME"].ToString() + "|" + stockRow["SYMBOL"].ToString() + "|" + stockRow["EXCHANGE"].ToString());
                    ddlLocalExchange.Items.Add(li);
                }
                ddlLocalCompany.SelectedIndex = 0;
                ddlLocalSymbol.SelectedIndex = 0;
                ddlLocalExchange.SelectedIndex = 0;

                breturn = true;
            }
            return breturn;
        }

        /// <summary>
        /// Populate the data as per the SYMBOL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMergedData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StockManager stockManager = new StockManager();
            DataRowView drv = e.Row.DataItem as DataRowView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    Label lblSrcCompany = (Label)e.Row.FindControl("lblSourceCompName");

                    //below predicate is used to find any punctuation chars. The case of UNI.LIVER is not getting solved by this
                    
                    //string filteredCompName = new string(lblSrcCompany.Text.Split(' ')[0].Where(c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))).ToArray());
                    string filteredCompName = lblSrcCompany.Text.Split(' ')[0];
                    if (stockManager.SearchOnlineInsertInDB(filteredCompName))
                    {
                        FillSymbolList();

                        if (SetMappedColumnsInGV(sender, e) == false)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Source Company Name or Symbol not found and can not be imported. You can delete this record!');", true);

                            //Intended behaviour is if a stock or company is not found we want to simulate cancel event. Following options are not working
                            //gvMergedData.EditIndex = -1;
                            //ShowMergedDataGrid();

                            //GridViewCancelEditEventHandler temp = gvMergedData_RowCancelingEdit;
                            //if (temp != null)
                            //{
                            //    temp(null, null);
                            //}       
                        }
                    }
                }
            }
        }

        protected void gvMergedData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMergedData.EditIndex = -1;
            ShowMergedDataGrid();
        }

        protected void gvMergedData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMergedData.EditIndex = e.NewEditIndex;
            ShowMergedDataGrid();
        }

        protected void gvMergedData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblLocalSuccess = (Label)gvMergedData.Rows[e.RowIndex].FindControl("lblSuccess");
            Label lblLocalSrcSymbol = (Label)gvMergedData.Rows[e.RowIndex].FindControl("lblSourceSymbol");

            DropDownList ddlLocalCompany = (DropDownList)gvMergedData.Rows[e.RowIndex].FindControl("ddlMappedCompName");
            DropDownList ddlLocalExchange = (DropDownList)gvMergedData.Rows[e.RowIndex].FindControl("ddlMappedExchange");
            DropDownList ddlLocalSymbol = (DropDownList)gvMergedData.Rows[e.RowIndex].FindControl("ddlMappedSymbol");
            TextBox tbLocalPurchaseDate = (TextBox)gvMergedData.Rows[e.RowIndex].FindControl("textboxMappedPurchaseDate");

            //TextBox tbLocalPurchsePrice = (TextBox)gvMergedData.Rows[e.RowIndex].FindControl("textboxMappedPurchasePrice");
            //TextBox tbLocalPurchseQty = (TextBox)gvMergedData.Rows[e.RowIndex].FindControl("textboxMappedPurchaseQty");
            //TextBox tbLocalCommTax = (TextBox)gvMergedData.Rows[e.RowIndex].FindControl("textboxMappedCommissionTaxes");

            if (ddlLocalCompany.SelectedIndex > 0 && ddlLocalSymbol.SelectedIndex > 0 && ddlLocalExchange.SelectedIndex > 0 & (string.IsNullOrEmpty(tbLocalPurchaseDate.Text) == false))
            {
                DataTable tableMerged = (DataTable)ViewState["MERGED_DATA"];

                if (tableMerged.Rows[e.RowIndex]["SOURCE_SYMBOL"].ToString() == lblLocalSrcSymbol.Text)
                {
                    tableMerged.Rows[e.RowIndex]["MAPPED_COMP_NAME"] = ddlLocalCompany.SelectedItem.Text;
                    tableMerged.Rows[e.RowIndex]["MAPPED_SYMBOL"] = ddlLocalSymbol.SelectedItem.Text;
                    tableMerged.Rows[e.RowIndex]["MAPPED_EXCHANGE"] = ddlLocalExchange.SelectedItem.Text;
                    tableMerged.Rows[e.RowIndex]["MAPPED_PURCHASE_DATE"] = tbLocalPurchaseDate.Text;
                    //tableMerged.Rows[e.RowIndex]["MAPPED_PURCHASE_PRICE"] = tbLocalPurchsePrice.Text;
                    //tableMerged.Rows[e.RowIndex]["MAPPED_PURCHASE_QTY"] = tbLocalPurchseQty.Text;
                    //tableMerged.Rows[e.RowIndex]["MAPPED_COMMISSION_TAXES"] = tbLocalCommTax.Text;

                    tableMerged.Rows[e.RowIndex]["STATUS"] = "MAPPED";
                }

                ViewState["MERGED_DATA"] = tableMerged;
                gvMergedData.EditIndex = -1;
                ShowMergedDataGrid();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Company Name, Symbol, Exchange & Purchase date are mandatory. Please cancel the editing or delete the row!');", true);
            }
        }

        protected void gvMergedData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblLocalSrcSymbol = (Label)gvMergedData.Rows[e.RowIndex].FindControl("lblSourceSymbol");
            DataTable tableMerged = (DataTable)ViewState["MERGED_DATA"];
            if (tableMerged.Rows[e.RowIndex]["SOURCE_SYMBOL"].ToString() == lblLocalSrcSymbol.Text)
            {
                tableMerged.Rows.RemoveAt(e.RowIndex);

                ViewState["MERGED_DATA"] = tableMerged;
                gvMergedData.EditIndex = -1;
                ShowMergedDataGrid();
            }
        }


        protected void ddlMappedCompName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlComp = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedCompName");
            string selectedVal = ((DropDownList)sender).SelectedValue;
            DropDownList ddlSymb = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedSymbol");
            DropDownList ddlExch = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedExchange");

            ddlSymb.SelectedValue = selectedVal;
            ddlExch.SelectedValue = selectedVal;
        }

        protected void ddlMappedSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSymb = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedSymbol");
            string selectedVal = ((DropDownList)sender).SelectedValue;
            DropDownList ddlComp = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedCompName");
            DropDownList ddlExch = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedExchange");

            ddlComp.SelectedValue = selectedVal;
            ddlExch.SelectedValue = selectedVal;
        }

        protected void ddlMappedExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlExch = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedExchange");
            string selectedVal = ((DropDownList)sender).SelectedValue;
            DropDownList ddlComp = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedCompName");
            DropDownList ddlSymb = (DropDownList)gvMergedData.Rows[gvMergedData.EditIndex].FindControl("ddlMappedSymbol");

            ddlComp.SelectedValue = selectedVal;
            ddlSymb.SelectedValue = selectedVal;
        }

        public long IsPortfolioNameExist(string portfolioName)
        {
            long lportfolioId = -1;
            StockManager stockManager = new StockManager();
            lportfolioId = stockManager.getPortfolioId(Session["EMAILID"].ToString(), portfolioName);
            return lportfolioId;
        }

        protected void buttonImportFile_Click(object sender, EventArgs e)
        {
            if(rblistPortfolioName.SelectedIndex == 0)
            {
                string filteredPortfolioName = new string(textboxPortfolioName.Text.Where(c => (char.IsLetterOrDigit(c))).ToArray());

                if ((string.IsNullOrWhiteSpace(textboxPortfolioName.Text) == false) && (textboxPortfolioName.Text.Equals(filteredPortfolioName) == true))
                {
                    if (IsPortfolioNameExist(filteredPortfolioName) >= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Portfolio name already exist! Please choose another name.');", true);
                        return;
                    }
                }
                else if ((string.IsNullOrWhiteSpace(textboxPortfolioName.Text) == true) || (textboxPortfolioName.Text.Equals(filteredPortfolioName) == false))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Portfolio name can contain only letters or numbers.');", true);
                    return;
                }
            }
            else if (ddlExistingPortfolioName.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select portfolio from list');", true);
                return;
            }

            if ((ViewState["MERGED_DATA"] != null) && (ViewState["STOCKMASTER"] != null))
            {
                DataTable tableMerged = (DataTable)ViewState["MERGED_DATA"];
                if (tableMerged.Rows.Count > 0)
                {
                    //fist find if at least one of the row has MAPPED status
                    DataRow[] filteredRows = tableMerged.Select("STATUS = 'MAPPED'");
                    if ((filteredRows != null) && (filteredRows.Count() > 0))
                    {
                        DataTable stockMaster = (DataTable)ViewState["STOCKMASTER"];

                        StockManager stockManager = new StockManager();
                        long stockRowid = -1;
                        long lportfolioid;

                        if (rblistPortfolioName.SelectedIndex == 0)
                        {
                            lportfolioid = stockManager.createNewPortfolio(Session["EMAILID"].ToString(), textboxPortfolioName.Text);
                        }
                        else
                        {
                            lportfolioid = long.Parse(ddlExistingPortfolioName.SelectedValue);
                        }
                        if(lportfolioid == -1)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Problem encountered selecting portfolio. Please try again later.');", true);
                            return;
                        }

                        foreach (DataRow importRow in filteredRows)
                        {
                            DataRow[] stockMasterRows = stockMaster.Select("SYMBOL = '" + importRow["MAPPED_SYMBOL"].ToString() + "' AND EXCHANGE = '" + importRow["MAPPED_EXCHANGE"].ToString() + "'");
                            if ((stockMasterRows != null) && (stockMasterRows.Count() == 1))
                            {
                                stockRowid = long.Parse(stockMasterRows[0]["ROWID"].ToString());
                                if (stockManager.insertNode(lportfolioid.ToString(), stockMasterRows[0]["ROWID"].ToString(), importRow["MAPPED_SYMBOL"].ToString(),
                                    importRow["MAPPED_PURCHASE_PRICE"].ToString(), importRow["MAPPED_PURCHASE_DATE"].ToString(),
                                    importRow["MAPPED_PURCHASE_QTY"].ToString(), "0.00", importRow["MAPPED_HOLDING_VALUE"].ToString()))
                                {
                                    importRow["STATUS"] = "SUCCESS";
                                }
                                else
                                {
                                    importRow["STATUS"] = "FAILED";
                                }
                            }
                        }
                        tableMerged.AcceptChanges();
                        ViewState["MERGED_DATA"] = tableMerged;
                        ShowMergedDataGrid();

                        if (rblistPortfolioName.SelectedIndex == 0)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('New portfolio created & MAPPED transactions import completed!');", true);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('MAPPED transactions were appended to existing portfolio!');", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('No rows with MAPPED status. Import can not proceed. Please verify each symbol or company name and update the row to proceed with Import.');", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Nothing to import');", true);
                }
            }
        }

        protected void textboxPortfolioName_TextChanged(object sender, EventArgs e)
        {
            string filteredPortfolioName = new string(textboxPortfolioName.Text.Where(c => (char.IsLetterOrDigit(c))).ToArray());

            if ((string.IsNullOrWhiteSpace(textboxPortfolioName.Text) == false) && (textboxPortfolioName.Text.Equals(filteredPortfolioName) == true))
            {
                if (IsPortfolioNameExist(filteredPortfolioName) >= 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Portfolio name already exist! Please choose another name.');", true);
                }
            }
            else if ((string.IsNullOrWhiteSpace(textboxPortfolioName.Text) == true) || (textboxPortfolioName.Text.Equals(filteredPortfolioName) == false))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Portfolio name can contain only letters or numbers.');", true);
            }
        }

        protected void rblistPortfolioName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rblistPortfolioName.SelectedIndex == 0)
            {
                lblExistingPortfolioName.Enabled = false;
                lblExistingPortfolioName.Visible = false;
                ddlExistingPortfolioName.Items.Clear();
                ddlExistingPortfolioName.Enabled = false;
                ddlExistingPortfolioName.Visible = false;

                lblNewPortfolioName.Enabled = true;
                lblNewPortfolioName.Visible = true;
                textboxPortfolioName.Enabled = true;
                textboxPortfolioName.Visible = true;
            }
            else
            {
                lblNewPortfolioName.Enabled = false;
                lblNewPortfolioName.Visible = false;
                textboxPortfolioName.Enabled = false;
                textboxPortfolioName.Visible = false;

                lblExistingPortfolioName.Enabled = true;
                lblExistingPortfolioName.Visible = true;
                ddlExistingPortfolioName.Items.Clear();
                ddlExistingPortfolioName.Enabled = true;
                ddlExistingPortfolioName.Visible = true;

                StockManager stockManager = new StockManager();
                DataTable tablePortfolioMaster = stockManager.getPortfolioMaster(Session["EMAILID"].ToString());

                if((tablePortfolioMaster != null) && (tablePortfolioMaster.Rows.Count > 0))
                {
                    ddlExistingPortfolioName.DataTextField = "PORTFOLIO_NAME";
                    ddlExistingPortfolioName.DataValueField = "ROWID";
                    ddlExistingPortfolioName.DataSource = tablePortfolioMaster;
                    ddlExistingPortfolioName.DataBind();

                    ListItem li = new ListItem("Select Portfolio", "-1");
                    ddlExistingPortfolioName.Items.Insert(0, li);
                }
                else
                {
                    ListItem li = new ListItem("Portfolio not available", "-1");
                    ddlExistingPortfolioName.Items.Insert(0, li);
                }
            }
        }
    }
}