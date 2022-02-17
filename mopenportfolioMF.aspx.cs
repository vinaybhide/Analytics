using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class mopenportfolioMF : System.Web.UI.Page
    {
        //new
        string strPreviousRowIDFundHouse = string.Empty;
        //end new

        string strPreviousRowID = string.Empty;

        // To keep track the Index of Group Total    
        int intSubTotalIndex = 3; //= 2;

        //new
        int intSubTotalIndexFundHouse = 2; // = 1;
        int summaryIndex = 0;

        //Row index To keep track of first data row for fundname, which will get stored in the respective summary record and will be used in select to navigate to that row
        int intFirstRowIndex = 0;
        int intShiftRowNumbers = 0;

        //For ARR
        DateTime dtFirstNAV;
        DateTime dtCurrentNAV;
        double dblyearsInvested = 0.00;
        double dblARR = 0.00;
        //end new

        // To temporarily store Sub Total    
        double dblSubTotalQuantity = 0;
        double dblSubTotalCost = 0;
        double dblSubTotalValue = 0;

        //new
        double dblSubTotalCostFundHouse = 0;
        double dblSubTotalValueFundHouse = 0;
        //end new

        // To temporarily store Grand Total    
        double dblGrandTotalQuantity = 0;
        double dblGrandTotalCost = 0;
        double dblGrandTotalValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //string fileName = "";
            if (Session["EMAILID"] != null)
            {
                if ((Session["MFPORTFOLIONAME"] != null) && (Session["MFPORTFOLIOMASTERROWID"] != null))
                {
                    if (!IsPostBack)
                    {
                        ViewState["FetchedData"] = null;
                        //Session["SelectedIndex"] = "0";
                        DataTable summaryTable = new DataTable();
                        summaryTable.Columns.Add("FundName", typeof(string)); //FundName

                        summaryTable.Columns.Add("CumUnits", typeof(decimal));
                        summaryTable.Columns.Add("CumCost", typeof(decimal));

                        summaryTable.Columns.Add("CurrVal", typeof(decimal));
                        summaryTable.Columns.Add("YearsInvested", typeof(decimal));
                        summaryTable.Columns.Add("ARR", typeof(decimal));
                        ViewState["SUMMARYTABLE"] = summaryTable;

                        GridViewSummary.DataSource = summaryTable;
                        GridViewSummary.DataBind();

                        //ClientScript.RegisterStartupScript(GetType(), "setpageposition", "setpageposition()", true);
                    }
                    //openPortfolio(fileName);
                    openPortfolio();

                    if (Session["MFSELECTEDINDEXPORTFOLIO"] == null)
                    {
                        Session["MFSELECTEDINDEXPORTFOLIO"] = "0";
                    }
                    int selectedIndex = Int32.Parse(Session["MFSELECTEDINDEXPORTFOLIO"].ToString());
                    if (selectedIndex >= GridViewPortfolio.Rows.Count)
                    {
                        --selectedIndex;
                    }
                    if ((selectedIndex >= 0) && (GridViewPortfolio.Rows.Count > 0))
                    {
                        GridViewPortfolio.SelectedIndex = selectedIndex;

                        DataTable dt = (DataTable)GridViewPortfolio.DataSource;


                        //string purchaseDate = GridViewPortfolio.Rows[selectedIndex].Cells[1].Text;

                        // FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                        string mfName = dt.Rows[selectedIndex]["FundName"].ToString();
                        string purchaseDate = dt.Rows[selectedIndex]["PurchaseDate"].ToString();

                        Session["MFPORTFOLIOROWID"] = dt.Rows[selectedIndex]["ID"].ToString();
                        Session["MFPORTFOLIOFUNDNAME"] = dt.Rows[selectedIndex]["FundName"].ToString();
                        Session["MFPORTFOLIOFUNDHOUSECODE"] = dt.Rows[selectedIndex]["FundHouseCode"].ToString();
                        Session["MFPORTFOLIOFUNDHOUSE"] = dt.Rows[selectedIndex]["FundHouse"].ToString();
                        Session["MFPORTFOLIOSCHEMECODE"] = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();
                        Session["MFSELECTEDINDEXPORTFOLIO"] = selectedIndex.ToString();
                        lblFundHouseCode.Text = dt.Rows[selectedIndex]["FundHouseCode"].ToString();
                        lblFundHouse.Text = dt.Rows[selectedIndex]["FundHouse"].ToString();
                        lblScript.Text = mfName;
                        lblSchemeCode.Text = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();
                        lblDate.Text = System.Convert.ToDateTime(purchaseDate).ToShortDateString();
                    }

                }
                else
                {
                    //Response.Redirect(".\\Default.aspx");
                    //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioNameToOpen + "');", true);
                    Response.Redirect("~/mselectportfolioMF.aspx");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }
        }


        protected void grdViewOrders_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAddFundHouse = false;
            bool IsSubTotalRowNeedToAdd = false;
            bool IsGrandTotalRowNeedtoAdd = false;

            //this is for the data row
            //intShiftRowNumbers++;

            GridView gridViewPortfolio = (GridView)sender;
            DataTable summaryTable = (DataTable)ViewState["SUMMARYTABLE"];

            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundName") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "FundName").ToString())
                    IsSubTotalRowNeedToAdd = true;

            if ((strPreviousRowIDFundHouse != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundHouse") != null))
                if (strPreviousRowIDFundHouse != DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString())
                {
                    IsSubTotalRowNeedToAdd = true;
                    IsSubTotalRowNeedToAddFundHouse = true;
                }


            if ((strPreviousRowID != string.Empty) && (strPreviousRowIDFundHouse != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundName") == null) &&
                (DataBinder.Eval(e.Row.DataItem, "FundHouse") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                IsSubTotalRowNeedToAddFundHouse = true;
                IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
                intSubTotalIndexFundHouse = -1;
            }

            #region Inserting Fund House row
            if ((strPreviousRowIDFundHouse == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundHouse") != null))
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();

                cell.Text = "Transaction details for Portfolio: " + Session["MFPORTFOLIONAME"].ToString();
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 9;
                cell.CssClass = "TableTitleRowStyle";
                row.Cells.Add(cell);
                gridViewPortfolio.Controls[0].Controls.AddAt(0, row);
                intShiftRowNumbers++;

                row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                cell = new TableCell();
                cell.Text = "Fund House : " + DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 9;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndexFundHouse, row);
                intSubTotalIndexFundHouse++;
                intShiftRowNumbers++;

                //add into summary table
                GridViewRow rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cellSummary = new TableCell();
                cellSummary.Text = "Summary for Portfolio: " + Session["MFPORTFOLIONAME"].ToString();
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ColumnSpan = 6;
                cellSummary.CssClass = "TableTitleRowStyle";
                rowSummary.Cells.Add(cellSummary);
                summaryIndex = 0;
                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                summaryIndex = 2;
                //intShiftRowNumbers++;

                rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                cellSummary = new TableCell();
                cellSummary.Text = "Fund House : " + DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                cellSummary.HorizontalAlign = HorizontalAlign.Left;
                cellSummary.ColumnSpan = 7;
                cellSummary.CssClass = "GroupHeaderStyle";
                rowSummary.Cells.Add(cellSummary);
                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                summaryIndex++;
                //intShiftRowNumbers++;
            }
            #endregion

            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundName") != null))
            {
                //GridView gridViewPortfolio= (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "Fund Name : " + DataBinder.Eval(e.Row.DataItem, "FundName").ToString();
                cell.ColumnSpan = 9;
                cell.CssClass = "FundNameHeaderStyle"; //"SubTotalRowStyle"; //"GroupHeaderStyle";
                row.Cells.Add(cell);
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                intSubTotalIndexFundHouse++;
                //Set first NAV dt for group ARR
                dtFirstNAV = System.Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "PurchaseDate").ToString());
                
                //save the row index
                intFirstRowIndex = e.Row.RowIndex;
                intShiftRowNumbers++;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                #region Adding Sub Total Row

                try
                {
                    dblyearsInvested = 0.00;
                    dblARR = 0.00;
                    if ((dblSubTotalValue > 0) && (dblSubTotalCost > 0))
                    {
                        dblyearsInvested = Math.Round(((dtCurrentNAV - dtFirstNAV).TotalDays) / 365.25, 4);
                        dblARR = Math.Round(Math.Pow((dblSubTotalValue / dblSubTotalCost), (1 / dblyearsInvested)) - 1, 4);
                    }
                }
                catch (Exception ex)
                {
                    dblyearsInvested = 0.00;
                    dblARR = 0.00;
                }

                //GridView GridViewPortfolio = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Purchase Unit Quantity Column            
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalQuantity); //sub total for NAV
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative units for: " + strPreviousRowID;
                row.Cells.Add(cell);

                //Adding empty CurrentNAV;NAVDate
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Value at Cost col
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative cost for: " + strPreviousRowID;
                row.Cells.Add(cell);


                //Adding Current Value Column         
                cell = new TableCell();
                cell.Text = "NA";
                if (dblSubTotalValue > 0)
                {
                    cell.Text = string.Format("{0:0.00}", dblSubTotalValue);
                }
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative value for: " + strPreviousRowID;
                row.Cells.Add(cell);

                ////Adding empty years invested, arr
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.ColumnSpan = 2;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);

                //Adding YearsInvested Column         
                cell = new TableCell();
                cell.Text = "NA";
                if (dblyearsInvested > 0)
                {
                    cell.Text = string.Format("{0:0.00}", dblyearsInvested);
                }
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);
                cell.ToolTip = "Cumulative years invested for: " + strPreviousRowID;
                //Adding ARR Column         
                cell = new TableCell();
                cell.Text = "NA";
                if (dblARR > 0)
                {
                    cell.Text = string.Format("{0:0.00%}", dblARR);
                }
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);
                cell.ToolTip = "Cumulative ARR for: " + strPreviousRowID;
                //Adding the Row at the RowIndex position in the Grid      
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                intSubTotalIndexFundHouse++;
                intShiftRowNumbers++;

                //moving these two after summary 
                //dblyearsInvested = 0.00;
                //dblARR = 0.00;


                //add into summary table--------------

                GridViewRow rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                TableCell cellSummary = new TableCell();

                //first add fund name
                cellSummary.Text = "Fund Name : " + strPreviousRowID;

                //cellSummary.Attributes.Add("rowindex", e.Row.RowIndex.ToString());
                
                cellSummary.HorizontalAlign = HorizontalAlign.Right;
                //cellSummary.ColumnSpan = 2;
                
                //cellSummary.CssClass = "FundNameHeaderStyle";
                
                rowSummary.Cells.Add(cellSummary);

                //cumulative quantity
                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblSubTotalQuantity); //sub total for NAV
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                
                //cellSummary.CssClass = "FundNameHeaderStyle";
                
                cellSummary.ToolTip = "Total quantity for: " + strPreviousRowID;
                rowSummary.Cells.Add(cellSummary);

                //Adding Value at Cost col
                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblSubTotalCost);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                
                //cellSummary.CssClass = "FundNameHeaderStyle";
                
                cellSummary.ToolTip = "Total cost for: " + strPreviousRowID;
                rowSummary.Cells.Add(cellSummary);

                //Adding Current Value Column         
                cellSummary = new TableCell();
                cellSummary.Text = "NA";
                if (dblSubTotalValue > 0)
                {
                    cellSummary.Text = string.Format("{0:0.00}", dblSubTotalValue);
                }
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                
                //cellSummary.CssClass = "FundNameHeaderStyle";
                
                cellSummary.ToolTip = "Valuation for: " + strPreviousRowID;
                rowSummary.Cells.Add(cellSummary);

                //Adding YearsInvested Column         
                cellSummary = new TableCell();
                cellSummary.Text = "NA";
                if (dblyearsInvested > 0)
                {
                    cellSummary.Text = string.Format("{0:0.00}", dblyearsInvested);
                }
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                
                //cellSummary.CssClass = "FundNameHeaderStyle";
                
                cellSummary.ToolTip = "Total years invested for : " + strPreviousRowID;
                rowSummary.Cells.Add(cellSummary);

                //Adding ARR Column         
                cellSummary = new TableCell();
                cellSummary.Text = "NA";
                if (dblARR > 0)
                {
                    cellSummary.Text = string.Format("{0:0.00%}", dblARR);
                }
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                
                //cellSummary.CssClass = "FundNameHeaderStyle";
                
                cellSummary.ToolTip = "Total ARR for: " + strPreviousRowID;
                rowSummary.Cells.Add(cellSummary);

                rowSummary.Attributes.Add("onmouseover", "this.style.backgroundColor='#ebeaea'");
                rowSummary.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                rowSummary.Attributes.Add("style", "cursor:pointer;");

                //intShiftRowNumbers++;
                //rowSummary.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridViewSummary, "Select$" + (intShiftRowNumbers - intFirstRowIndex).ToString() + "," + intFirstRowIndex.ToString());
                rowSummary.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridViewSummary, "Select$" + summaryIndex);
                rowSummary.Attributes.Add("portfoliorowindex", intFirstRowIndex.ToString());
                rowSummary.Attributes.Add("nondatarowcount", (intShiftRowNumbers).ToString());

                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);

                summaryIndex++;
                //End summary table

                dblyearsInvested = 0.00;
                dblARR = 0.00;

                #endregion

                #region adding totals for fund house and next fund house
                if (IsSubTotalRowNeedToAddFundHouse)
                {
                    #region Fund House Total Row
                    //GridView gridViewPortfolio = (GridView)sender;
                    // Creating a Row      
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    //Adding Total Cell           
                    cell = new TableCell();
                    cell.Text = "Grand Total for: " + strPreviousRowIDFundHouse;
                    cell.HorizontalAlign = HorizontalAlign.Left;
                    cell.ColumnSpan = 5;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
                    row.Cells.Add(cell);

                    //Adding Value at Cost Column          
                    cell = new TableCell();
                    cell.Text = string.Format("{0:0.00}", dblSubTotalCostFundHouse);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
                    cell.ToolTip = "Total cost for: " + strPreviousRowIDFundHouse;
                    row.Cells.Add(cell);

                    ////Adding empty CurrentNAV;NAVDate
                    //cell = new TableCell();
                    //cell.Text = "";
                    //cell.HorizontalAlign = HorizontalAlign.Center;
                    //cell.ColumnSpan = 2;
                    //cell.CssClass = "SubTotalRowStyle";
                    //row.Cells.Add(cell);

                    //Adding Current Value Column           
                    cell = new TableCell();
                    cell.Text = string.Format("{0:0.00}", dblSubTotalValueFundHouse);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
                    cell.ToolTip = "Total valuation for: " + strPreviousRowIDFundHouse;
                    row.Cells.Add(cell);

                    //Adding empty years invested, arr
                    cell = new TableCell();
                    cell.Text = "";
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    cell.ColumnSpan = 2;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
                    row.Cells.Add(cell);

                    //Adding the Row at the RowIndex position in the Grid     
                    gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndexFundHouse, row);
                    intSubTotalIndexFundHouse++;
                    intSubTotalIndex++;
                    intShiftRowNumbers++;

                    //add into summary table--------------

                    rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cellSummary = new TableCell();

                    //first add fund house
                    cellSummary.Text = "Sub Total for: " + strPreviousRowIDFundHouse;
                    cellSummary.HorizontalAlign = HorizontalAlign.Right;
                    //cellSummary.ColumnSpan = 3;
                    cellSummary.CssClass = "FundHouseSubTotalStyle";
                    rowSummary.Cells.Add(cellSummary);

                    //empty cell for cumulative units
                    cellSummary = new TableCell();
                    cellSummary.Text = "";
                    cellSummary.HorizontalAlign = HorizontalAlign.Center;
                    cellSummary.CssClass = "FundHouseSubTotalStyle";
                    rowSummary.Cells.Add(cellSummary);

                    //Adding Value at Cost Column          
                    cellSummary = new TableCell();
                    cellSummary.Text = string.Format("{0:0.00}", dblSubTotalCostFundHouse);
                    cellSummary.HorizontalAlign = HorizontalAlign.Center;
                    //cell.CssClass = "GroupHeaderStyle";
                    cellSummary.CssClass = "FundHouseSubTotalStyle";
                    cellSummary.ToolTip = "Total cost for: " + strPreviousRowIDFundHouse;
                    rowSummary.Cells.Add(cellSummary);

                    //Adding Current Value Column           
                    cellSummary = new TableCell();
                    cellSummary.Text = string.Format("{0:0.00}", dblSubTotalValueFundHouse);
                    cellSummary.HorizontalAlign = HorizontalAlign.Center;
                    cellSummary.CssClass = "FundHouseSubTotalStyle";
                    cellSummary.ToolTip = "Total value for: " + strPreviousRowIDFundHouse;
                    rowSummary.Cells.Add(cellSummary);

                    //Adding empty years invested
                    cellSummary = new TableCell();
                    cellSummary.Text = "";
                    cellSummary.HorizontalAlign = HorizontalAlign.Center;
                    cellSummary.ColumnSpan = 2;
                    cellSummary.CssClass = "FundHouseSubTotalStyle";
                    rowSummary.Cells.Add(cellSummary);

                    ////Adding empty cell for ARR
                    //cellSummary = new TableCell();
                    //cellSummary.Text = "";
                    //cellSummary.HorizontalAlign = HorizontalAlign.Center;
                    ////cellSummary.ColumnSpan = 2;
                    //cellSummary.CssClass = "FundHouseSubTotalStyle";
                    //rowSummary.Cells.Add(cellSummary);

                    GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                    summaryIndex++;
                    //intShiftRowNumbers++;

                    //end summary table
                    #endregion

                    #region Adding Next Group Header Details
                    if (DataBinder.Eval(e.Row.DataItem, "FundHouse") != null)
                    {
                        row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                        cell = new TableCell();
                        cell.Text = "Fund House : " + DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                        cell.ColumnSpan = 9;
                        cell.HorizontalAlign = HorizontalAlign.Left;
                        cell.CssClass = "GroupHeaderStyle";
                        row.Cells.Add(cell);
                        gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndexFundHouse, row);
                        intSubTotalIndexFundHouse++;
                        intSubTotalIndex++;
                        intShiftRowNumbers++;

                        //adding summary table ------------
                        rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                        cellSummary = new TableCell();
                        cellSummary.Text = "Fund House : " + DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                        cellSummary.HorizontalAlign = HorizontalAlign.Left;
                        cellSummary.ColumnSpan = 6;
                        cellSummary.CssClass = "GroupHeaderStyle";
                        rowSummary.Cells.Add(cellSummary);
                        GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                        summaryIndex++;
                        //intShiftRowNumbers++;
                        //end summary table
                    }
                    #endregion
                    #region Reseting the Sub Total Variables
                    dblSubTotalQuantity = 0;
                    dblSubTotalCost = 0;
                    dblSubTotalValue = 0;
                    dblSubTotalCostFundHouse = 0;
                    dblSubTotalValueFundHouse = 0;
                    #endregion

                }

                #endregion

                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "FundName") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "Fund Name : " + DataBinder.Eval(e.Row.DataItem, "FundName").ToString();
                    cell.ColumnSpan = 9;
                    cell.CssClass = "FundNameHeaderStyle";//cell.CssClass = "SubTotalRowStyle"; //"GroupHeaderStyle";
                    row.Cells.Add(cell);
                    gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                    intSubTotalIndexFundHouse++;
                    intShiftRowNumbers++;

                    dtFirstNAV = System.Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "PurchaseDate").ToString());

                    //save the first row index
                    intFirstRowIndex = e.Row.RowIndex;
                }
                #endregion
                #region Reseting the Sub Total Variables
                dblSubTotalQuantity = 0;
                dblSubTotalCost = 0;
                dblSubTotalValue = 0;
                #endregion
            }

            if (IsGrandTotalRowNeedtoAdd)
            {
                #region Grand Total Row
                //GridView gridViewPortfolio = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Grand Total";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 5;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Value at Cost Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                cell.ToolTip = "Portfolio total cost";
                row.Cells.Add(cell);

                //Adding Current Value Column           
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalValue);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                cell.ToolTip = "Portfolio total valuation";
                row.Cells.Add(cell);

                //Adding empty years invested, arr
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 2;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid     
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                intShiftRowNumbers++;

                //adding summary table ------------
                GridViewRow rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cellSummary = new TableCell();
                cellSummary.Text = "Grand Total";
                cellSummary.HorizontalAlign = HorizontalAlign.Left;
                //cellSummary.ColumnSpan = 3;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                //empty cell for cumulative units
                cellSummary = new TableCell();
                cellSummary.Text = "";
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                //Adding Value at Cost Column          
                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblGrandTotalCost);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                cellSummary.ToolTip = "Portfolio total cost";
                rowSummary.Cells.Add(cellSummary);

                //Adding Current Value Column           
                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblGrandTotalValue);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                cellSummary.ToolTip = "Portfolio total valuation";
                rowSummary.Cells.Add(cellSummary);

                //Adding empty years invested
                cellSummary = new TableCell();
                cellSummary.Text = "";
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                //cellSummary.ColumnSpan = 2;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                //empty cell for ARR
                cellSummary = new TableCell();
                cellSummary.Text = "";
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                //cellSummary.ColumnSpan = 2;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                //summaryIndex++;
                summaryIndex = 2;
                //end summary table
                //intShiftRowNumbers++;

                #endregion
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// portfolio table format
        /// FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void grdViewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //store the current NAV dt

                dtCurrentNAV = System.Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "NAVDate").ToString());

                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "FundName").ToString();
                strPreviousRowIDFundHouse = DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                string fundHouse = DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                string schemeCode = DataBinder.Eval(e.Row.DataItem, "SCHEME_CODE").ToString();

                double dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "PurchaseUnits").ToString());
                double dblCost = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "ValueAtCost").ToString());
                double dblValue = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CurrentValue").ToString());
                // Cumulating Sub Total            
                dblSubTotalQuantity += dblQuantity;
                dblSubTotalCost += dblCost;
                dblSubTotalValue += dblValue;

                //cumulative fund house total
                dblSubTotalCostFundHouse += dblCost;
                dblSubTotalValueFundHouse += dblValue;

                // Cumulating Grand Total           
                dblGrandTotalQuantity += dblQuantity;
                dblGrandTotalCost += dblCost;
                dblGrandTotalValue += dblValue;

                // This is for cumulating the values  
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView gridViewPortfolio = (GridView)sender;

                    //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ddd'");
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ebeaea'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewPortfolio, "Select$" + e.Row.RowIndex);
                    //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewPortfolio, "Select$" +
                    //    e.Row.RowIndex + "," + strPreviousRowID + "$"); // + ";" + fundHouse + ";" + strPreviousRowID + ";" + schemeCode);

                    e.Row.Cells[0].ToolTip = "Purchase date";
                    e.Row.Cells[1].ToolTip = "Purchase NAV";
                    e.Row.Cells[2].ToolTip = "Purchased Units";
                    e.Row.Cells[3].ToolTip = "Current NAV";
                    e.Row.Cells[4].ToolTip = "Current NAV Date";
                    e.Row.Cells[5].ToolTip = "Transaction Cost";
                    e.Row.Cells[6].ToolTip = "Transaction value";
                    e.Row.Cells[7].ToolTip = "Years invested";
                    e.Row.Cells[8].ToolTip = "ARR";

                }

            }
        }

        protected void grdViewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int selectedIndex = System.Convert.ToInt32(e.CommandArgument.ToString());
                GridView gridViewPortfolio = (GridView)sender;

                DataTable dt = (DataTable)gridViewPortfolio.DataSource;


                //string purchaseDate = GridViewPortfolio.Rows[selectedIndex].Cells[1].Text;

                // FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                string mfName = dt.Rows[selectedIndex]["FundName"].ToString();
                string purchaseDate = dt.Rows[selectedIndex]["PurchaseDate"].ToString();

                Session["MFPORTFOLIOROWID"] = dt.Rows[selectedIndex]["ID"].ToString();
                Session["MFPORTFOLIOFUNDNAME"] = dt.Rows[selectedIndex]["FundName"].ToString();
                Session["MFPORTFOLIOFUNDHOUSECODE"] = dt.Rows[selectedIndex]["FundHouseCode"].ToString();
                Session["MFPORTFOLIOFUNDHOUSE"] = dt.Rows[selectedIndex]["FundHouse"].ToString();
                Session["MFPORTFOLIOSCHEMECODE"] = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();
                Session["MFSELECTEDINDEXPORTFOLIO"] = selectedIndex.ToString();
                lblFundHouseCode.Text = dt.Rows[selectedIndex]["FundHouseCode"].ToString();
                lblFundHouse.Text = dt.Rows[selectedIndex]["FundHouse"].ToString();
                lblScript.Text = mfName;
                lblSchemeCode.Text = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();
                lblDate.Text = System.Convert.ToDateTime(purchaseDate).ToShortDateString();
            }
        }

        protected void GridViewSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView gvSummary = (GridView)sender;

                //this is the row index of the summary grid
                int summaryRowIndex = int.Parse(e.CommandArgument.ToString());

                GridViewRow gvSummaryRow = (GridViewRow) gvSummary.Controls[0].Controls[summaryRowIndex];


                //this is the index of the data row where we want to select & set the focus on
                int selectedIndex = System.Convert.ToInt32(gvSummaryRow.Attributes["portfoliorowindex"].ToString());

                //these are the number of additional non data rows that were added to portfolio transaction grid
                int numofNonDataRows = System.Convert.ToInt32(gvSummaryRow.Attributes["nondatarowcount"].ToString());

                //this is the actual index numbers including total summary rows + non data portfolio rows count + row index of the data row (where we want the focus)
                //int actualIndex = gvSummary.Controls[0].Controls.Count + numofNonDataRows + selectedIndex;
                int actualIndex = numofNonDataRows + selectedIndex;

                GridViewPortfolio.SelectedIndex = selectedIndex;
                GridViewPortfolio.Rows[selectedIndex].Focus();

                DataTable dt = (DataTable)GridViewPortfolio.DataSource;


                // FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                string mfName = dt.Rows[selectedIndex]["FundName"].ToString();
                string purchaseDate = dt.Rows[selectedIndex]["PurchaseDate"].ToString();

                Session["MFPORTFOLIOROWID"] = dt.Rows[selectedIndex]["ID"].ToString();
                Session["MFPORTFOLIOFUNDNAME"] = dt.Rows[selectedIndex]["FundName"].ToString();
                Session["MFPORTFOLIOFUNDHOUSECODE"] = dt.Rows[selectedIndex]["FundHouseCode"].ToString();
                Session["MFPORTFOLIOFUNDHOUSE"] = dt.Rows[selectedIndex]["FundHouse"].ToString();
                Session["MFPORTFOLIOSCHEMECODE"] = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();
                Session["MFSELECTEDINDEXPORTFOLIO"] = selectedIndex.ToString();
                lblFundHouseCode.Text = dt.Rows[selectedIndex]["FundHouseCode"].ToString();
                lblFundHouse.Text = dt.Rows[selectedIndex]["FundHouse"].ToString();
                lblScript.Text = mfName;
                lblSchemeCode.Text = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();
                lblDate.Text = System.Convert.ToDateTime(purchaseDate).ToShortDateString();

                ClientScript.RegisterStartupScript(this.GetType(), "setscrollportfolio", "setscrollportfolio('" + actualIndex + "');", true);
            }
        }

        //public void openPortfolio(string portfolioFileName)
        public void openPortfolio()
        {
            DataTable dt;

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "doHourglass1", "document.body.style.cursor = 'wait';", true);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "doHourglass", "doHourglass();", true);
            try
            {
                if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                {
                    //dt = MFAPI.openMFPortfolio(folderPath, portfolioFileName);
                    DataManager dataMgr = new DataManager();
                    dt = dataMgr.openMFPortfolio(Session["EMAILID"].ToString(), Session["MFPORTFOLIONAME"].ToString(), Session["MFPORTFOLIOMASTERROWID"].ToString());
                    ViewState["FetchedData"] = dt;
                }
                else
                {
                    dt = (DataTable)ViewState["FetchedData"];
                }
                GridViewPortfolio.DataSource = dt;
                GridViewPortfolio.DataBind();
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while opening portfolio: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while opening portfolio:" + ex.Message + "');", true);
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resetCursor", "document.body.style.cursor = 'standard';", true);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "resetCursor", "resetCursor();", true);

        }
        protected void ButtonAddNew_Click(object sender, EventArgs e)
        {
            //ResponseHelper.Redirect(Response, "\\addnewscript.aspx", "_self", "menubar=0,scrollbars=1,width=780,height=900,top=10");
            //ResponseHelper.Redirect(Response, ".\\addnewscript.aspx", "", "");
            //if (this.MasterPageFile.Contains("Site.Master"))
            //    //Response.Redirect(".\\addnewscript.aspx");
            //    Response.Redirect("~/addnewscript.aspx");
            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //    Response.Redirect("~/maddnewscript.aspx");
            //else
            Response.Redirect("~/maddnewmftrans.aspx");
        }

        protected void buttonDeleteSelectedScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string portfolioRowId = Session["MFPORTFOLIOROWID"].ToString();

                    DataManager dataMgr = new DataManager();
                    dataMgr.deletePortfolioRow(portfolioRowId);

                    Response.Redirect("~/mopenportfolioMF.aspx");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTxnSelected + "');", true);
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while deleting script:" + ex.Message + "');", true);
            }

        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string mfName = Session["MFPORTFOLIOFUNDNAME"].ToString();
                    string fundHouse = Session["MFPORTFOLIOFUNDHOUSE"].ToString();
                    string schemeCode = Session["MFPORTFOLIOSCHEMECODE"].ToString();
                    string portfolioRowId = Session["MFPORTFOLIOROWID"].ToString();

                    //Columns the grid view
                    //PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR

                    string purchaseDate = System.Convert.ToDateTime(GridViewPortfolio.SelectedRow.Cells[0].Text.ToString()).ToShortDateString();
                    string purchaseNAV = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string purchaseUnits = GridViewPortfolio.SelectedRow.Cells[2].Text.ToString();
                    string valueAtCost = GridViewPortfolio.SelectedRow.Cells[5].Text.ToString();

                    Response.Redirect("~/meditmftrans.aspx?fundhouse=" + Server.UrlEncode(fundHouse) +
                        "&fundname=" + Server.UrlEncode(mfName) + "&schemecode=" + schemeCode + "&purchasedate=" + purchaseDate
                        + "&purchasenav=" + purchaseNAV + "&purchaseunits=" + purchaseUnits + "&valueatcost=" + valueAtCost + "&portfoliorowid=" + portfolioRowId);
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while updating the transaction:" + ex.Message + "');", true);
            }
        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            if ((lblFundHouseCode.Text.Equals(string.Empty)) || (lblFundHouse.Text.Equals(string.Empty)) || (lblScript.Text.Equals(string.Empty)) || (lblSchemeCode.Text.Equals(string.Empty)))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select any transaction row to see graph');", true);
            }
            else
            {
                string url = "";

                if (ddlGrphType.SelectedValue.Equals("-1") == false)
                {
                    if (ddlGrphType.SelectedValue.Equals("BACKTEST"))
                    {
                        //url = "~/backtestSMAMF.aspx" + "?schemecode=" + lblSchemeCode.Text + "&smasmall=" + "10" + "&smalong=" + "20";
                        url = "~/advGraphs/backtestsma_mf.aspx" + "?schemecode=" + lblSchemeCode.Text + "&fundhousecode=" + lblFundHouseCode.Text +
                            "&smasmall=50" + "&smalong=100" + "&buyspan=2" + "&sellspan=20" + "&simulationqty=100" +
                            "&regressiontype=2" + "&forecastperiod=40";
                    }
                    else if (ddlGrphType.SelectedValue.Equals("VALUE Graph"))
                    {
                        url = "~/portfolioValuationMF2.aspx" + "?";
                    }
                    else if (ddlGrphType.SelectedValue.Equals("BAR Graph"))
                    {
                        url = "~/portfolioValuationMF.aspx" + "?";
                    }
                    else //if (ddlGrphType.SelectedValue.Equals("DAILY_NAV"))
                    {
                        url = "~/graphs/mfgraphmain.aspx" + "?fundhousecode=" + lblFundHouseCode.Text + "&schemecode=" + lblSchemeCode.Text +
                                     "&fromdate=" + DateTime.Today.AddDays(-90).ToString("yyyy-MM-dd") + "&todate=" + DateTime.Today.ToString("yyyy-MM-dd") +
                                     "&graphtype=" + ddlGrphType.SelectedValue;

                    }

                    if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    {
                        url += "&parent=mopenportfolioMF.aspx";
                        ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                    }
                }
            }
        }

        protected void ddlGrphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lblFundHouseCode.Text.Equals(string.Empty)) || (lblFundHouse.Text.Equals(string.Empty)) || (lblScript.Text.Equals(string.Empty)) || (lblSchemeCode.Text.Equals(string.Empty)))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select any transaction row to see graph');", true);
            }
            else
            {
                string url = "";

                if (ddlGrphType.SelectedValue.Equals("-1") == false)
                {
                    url = "~/graphs/mfgraphmain.aspx" + "?fundhousecode=" + lblFundHouseCode.Text + "&schemecode=" + lblSchemeCode.Text +
                                 "&fromdate=" + DateTime.Today.AddDays(-90).ToString("yyyy-MM-dd") + "&todate=" + DateTime.Today.ToString("yyyy-MM-dd") +
                                 "&graphtype=" + ddlGrphType.SelectedValue;
                    if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    {
                        url += "&parent=mopenportfolioMF.aspx";
                        ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                    }
                }
            }
        }

        protected void ddlAdvGrphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lblFundHouseCode.Text.Equals(string.Empty)) || (lblFundHouse.Text.Equals(string.Empty)) || (lblScript.Text.Equals(string.Empty)) || (lblSchemeCode.Text.Equals(string.Empty)))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select any transaction row to see graph');", true);
            }
            else
            {
                string url = "";

                if (ddlAdvGrphType.SelectedValue.Equals("-1") == false)
                {
                    if (ddlAdvGrphType.SelectedValue.Equals("BACKTEST"))
                    {
                        //url = "~/backtestSMAMF.aspx" + "?schemecode=" + lblSchemeCode.Text + "&smasmall=" + "10" + "&smalong=" + "20";
                        url = "~/advGraphs/backtestsma_mf.aspx" + "?schemecode=" + lblSchemeCode.Text + "&fundhousecode=" + lblFundHouseCode.Text +
                            "&smasmall=50" + "&smalong=100" + "&buyspan=2" + "&sellspan=20" + "&simulationqty=100" +
                            "&regressiontype=2" + "&forecastperiod=40";
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("VALUE Graph"))
                    {
                        url = "~/advGraphs/mfvaluationline.aspx" + "?";
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("BAR Graph"))
                    {
                        url = "~/advGraphs/mfvaluationbar.aspx" + "?";
                    }
                    if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    {
                        url += "&parent=mopenportfolioMF.aspx";
                        ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                    }
                }
            }
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/mselectportfolioMF.aspx");
        }
    }
}