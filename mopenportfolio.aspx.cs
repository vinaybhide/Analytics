using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccessLayer;
using System.Drawing;

namespace Analytics
{
    public partial class mopenportfolio : System.Web.UI.Page
    {

        string strPreviousRowID = string.Empty;

        // To keep track the Index of Group Total    
        int intSubTotalIndex = 2; //1; commented for adding summary grid

        //new
        int summaryIndex = 0;

        //Row index To keep track of first data row for fundname, which will get stored in the respective summary record and will be used in select to navigate to that row
        int intFirstRowIndex = 0;
        int intShiftRowNumbers = 0;

        string strPrevCompName = string.Empty;
        string strPrevExchange = string.Empty;
        //end new

        // To temporarily store Sub Total    
        double dblSubTotalQuantity = 0;
        double dblSubTotalCost = 0;
        double dblSubTotalValue = 0;
        // To temporarily store Grand Total    
        double dblGrandTotalQuantity = 0;
        double dblGrandTotalCost = 0;
        double dblGrandTotalValue = 0;

        //Added for total years & arr
        double dblCumYearsInvested = 0;
        double dblCumARR = 0;
        DateTime datetimeQuoteDateTime = DateTime.Now;
        double dblcurrentQuote = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EMAILID"] != null)
            //{
            //    Master.UserID = Session["EMAILID"].ToString();
            //}

            if (Session["EMAILID"] != null)
            {
                if ((Session["STOCKPORTFOLIOMASTERROWID"] != null) && (Session["STOCKPORTFOLIONAME"] != null))
                {
                    //Master.Portfolio = Session["STOCKPORTFOLIONAME"].ToString();

                    if (!IsPostBack)
                    {
                        ViewState["FetchedData"] = null;
                        ViewState["SelectedIndex"] = null;

                        DataTable summaryTable = new DataTable();
                        summaryTable.Columns.Add("COMP_NAME", typeof(string)); //FundName
                        summaryTable.Columns.Add("SYMBOL", typeof(string));
                        summaryTable.Columns.Add("EXCHANGE", typeof(string));
                        summaryTable.Columns.Add("CumQty", typeof(decimal));
                        summaryTable.Columns.Add("CumCost", typeof(decimal));
                        summaryTable.Columns.Add("QuoteDt", typeof(string));
                        summaryTable.Columns.Add("Quote", typeof(decimal));
                        summaryTable.Columns.Add("CurrVal", typeof(decimal));
                        summaryTable.Columns.Add("CumYearsInvested", typeof(decimal));
                        summaryTable.Columns.Add("CumARR", typeof(decimal));
                        ViewState["SUMMARYTABLE"] = summaryTable;

                        GridViewSummary.DataSource = summaryTable;
                        GridViewSummary.DataBind();

                    }
                    DataTable dt;
                    openPortfolio();
                    if (Session["STOCKSELECTEDINDEXPORTFOLIO"] == null)
                    {
                        Session["STOCKSELECTEDINDEXPORTFOLIO"] = "0";
                    }

                    int selectedIndex = Int32.Parse(Session["STOCKSELECTEDINDEXPORTFOLIO"].ToString());
                    if (selectedIndex >= GridViewPortfolio.Rows.Count)
                    {
                        --selectedIndex;
                    }
                    if ((selectedIndex >= 0) && (GridViewPortfolio.Rows.Count > 0))
                    {
                        GridViewPortfolio.SelectedIndex = selectedIndex;

                        dt = (DataTable)GridViewPortfolio.DataSource;


                        //string purchaseDate = GridViewPortfolio.Rows[selectedIndex].Cells[1].Text;

                        // FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
                        string symbol = dt.Rows[selectedIndex]["SYMBOL"].ToString();
                        string purchaseDate = dt.Rows[selectedIndex]["PURCHASE_DATE"].ToString();

                        Session["STOCKPORTFOLIOROWID"] = dt.Rows[selectedIndex]["ID"].ToString();
                        Session["STOCKPORTFOLIOSCRIPTID"] = dt.Rows[selectedIndex]["STOCKID"].ToString();
                        Session["STOCKPORTFOLIOEXCHANGE"] = dt.Rows[selectedIndex]["EXCHANGE"].ToString();
                        Session["STOCKPORTFOLIOTYPE"] = dt.Rows[selectedIndex]["SERIES"].ToString();
                        Session["STOCKPORTFOLIOSCRIPTNAME"] = dt.Rows[selectedIndex]["SYMBOL"].ToString();
                        Session["STOCKPORTFOLIOCOMPNAME"] = dt.Rows[selectedIndex]["COMP_NAME"].ToString();
                        Session["STOCKSELECTEDINDEXPORTFOLIO"] = selectedIndex.ToString(); ;

                        lblCompName.Text = dt.Rows[selectedIndex]["COMP_NAME"].ToString();
                        lblScript.Text = dt.Rows[selectedIndex]["SYMBOL"].ToString();
                        lblExchange.Text = dt.Rows[selectedIndex]["EXCHANGE"].ToString();
                        lblInvestmentType.Text = dt.Rows[selectedIndex]["SERIES"].ToString();

                        lblDate.Text = System.Convert.ToDateTime(purchaseDate).ToShortDateString();
                    }

                }
                else
                {
                    //Response.Redirect(".\\Default.aspx");
                    //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioNameToOpen + "');", true);
                    Response.Redirect("~/mselectportfolio.aspx");
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
            bool IsSubTotalRowNeedToAdd = false;
            bool IsGrandTotalRowNeedtoAdd = false;

            GridView gridViewPortfolio = (GridView)sender;
            
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "SYMBOL") != null))
                if (strPreviousRowID.Equals(DataBinder.Eval(e.Row.DataItem, "SYMBOL").ToString()) == false)
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "SYMBOL") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "SYMBOL") != null))
            {
                //adding gridview title in the transaction table
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "Transaction details for Portfolio: " + Session["STOCKPORTFOLIONAME"].ToString();
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 15;
                cell.CssClass = "TableTitleRowStyle";
                row.Cells.Add(cell);
                gridViewPortfolio.Controls[0].Controls.AddAt(0, row);
                intShiftRowNumbers++;
                ///////

                //Now add a first row with compname & symbol 
                //GridView gridViewPortfolio= (GridView)sender;
                row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                cell = new TableCell();
                cell.Text = "Company:" + DataBinder.Eval(e.Row.DataItem, "COMP_NAME").ToString() + /*+ Environment.NewLine + */
                            " [Symbol: " + DataBinder.Eval(e.Row.DataItem, "SYMBOL").ToString() + "]";

                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 9;
                cell.ColumnSpan = 15;// 10;// 7;
                cell.CssClass = "GroupHeaderStyle";
                //cell.VerticalAlign = VerticalAlign.Bottom;
                row.Cells.Add(cell);
                //row.Attributes.Add("style", "height:75px;");
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                intShiftRowNumbers++;

                intFirstRowIndex = e.Row.RowIndex;

                //now add summary row title
                GridViewRow rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cellSummary = new TableCell();
                cellSummary.Text = "Summary for Portfolio: " + Session["STOCKPORTFOLIONAME"].ToString();
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ColumnSpan = 10;
                cellSummary.CssClass = "TableTitleRowStyle";
                rowSummary.Cells.Add(cellSummary);
                summaryIndex = 0;
                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                summaryIndex = 2;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                #region Adding Sub Total Row
                //GridView GridViewPortfolio = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.ColumnSpan = 5;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding empty purchase_qty col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);
                ////Adding empty commisionpaid col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);
                ////Adding empty investmentcost col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);

                //Current Date
                cell = new TableCell();
                //cell.Text = datetimeQuoteDateTime.ToString("yyyy-MM-dd HH:mm:ss"); ;
                cell.Text = datetimeQuoteDateTime.ToString("dd-MM-yyyy");
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Quote date";
                row.Cells.Add(cell);
                //Current Price
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblcurrentQuote);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Quote";
                row.Cells.Add(cell);

                ////Adding empty currentvalue col
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 3;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);
                ////Adding empty yearsinvested col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);
                ////Adding empty arr col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);

                //Adding Cum Quantity Column            
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalQuantity);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative quantity";
                row.Cells.Add(cell);

                ////Adding empty commisionpaid col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);

                //Adding Cum Cost col
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative cost";
                row.Cells.Add(cell);

                ////Adding empty Quote date col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);
                ////Adding empty Quote
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);

                //Adding Value Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalValue);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative value";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblCumYearsInvested);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative years invested";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblCumARR);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                cell.ToolTip = "Cumulative ARR";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid      
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);

                intSubTotalIndex++;
                intShiftRowNumbers++;

                //now add a row in summary table for compname symbol exchange & other details
                GridViewRow rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                TableCell cellSummary = new TableCell();
                cellSummary.Text = strPrevCompName;
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = strPreviousRowID; //this is symbol
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = strPrevExchange;
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblSubTotalQuantity);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Cumulative quantity";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblSubTotalCost);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Cumulative cost";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = datetimeQuoteDateTime.ToString("dd-MM-yyyy");
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Quote date";
                rowSummary.Cells.Add(cellSummary);
                
                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblcurrentQuote);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Quote";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblSubTotalValue);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Cumulative value";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblCumYearsInvested);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Cumulative years invested";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblCumARR);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ToolTip = "Cumulative ARR";
                rowSummary.Cells.Add(cellSummary);

                rowSummary.Attributes.Add("onmouseover", "this.style.backgroundColor='#ebeaea'");
                rowSummary.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                rowSummary.Attributes.Add("style", "cursor:pointer;");

                rowSummary.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridViewSummary, "Select$" + summaryIndex);
                rowSummary.Attributes.Add("portfoliorowindex", intFirstRowIndex.ToString());
                rowSummary.Attributes.Add("nondatarowcount", (intShiftRowNumbers).ToString());

                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);

                summaryIndex++;
                //end summary
                #endregion

                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "SYMBOL") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    //cell.Text = "Script : " + DataBinder.Eval(e.Row.DataItem, "ScriptID").ToString();
                    cell.Text = "Company:" + DataBinder.Eval(e.Row.DataItem, "COMP_NAME").ToString() + /*+ Environment.NewLine +*/
                                " [Symbol: " + DataBinder.Eval(e.Row.DataItem, "SYMBOL").ToString() + "]";
                    cell.HorizontalAlign = HorizontalAlign.Left;

                    //cell.ColumnSpan = 9;
                    cell.ColumnSpan = 15;//10; // 7;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                    intShiftRowNumbers++;
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
                //cell.ColumnSpan = 6;
                cell.ColumnSpan = 11;//4;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Cost Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                cell.ToolTip = "Total portfolio cost";
                row.Cells.Add(cell);

                ////Adding empty quote date col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "GrandTotalRowStyle";
                //row.Cells.Add(cell);

                ////Adding empty quote col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "GrandTotalRowStyle";
                //row.Cells.Add(cell);

                //Adding Value Column           
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalValue);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                cell.ToolTip = "Portfolio valuation";
                row.Cells.Add(cell);

                //Adding empty yearsinvested col
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 2;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                ////Adding empty arr col
                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.CssClass = "GrandTotalRowStyle";
                //row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid     
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                intShiftRowNumbers++;

                //adding summary table ------------
                GridViewRow rowSummary = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cellSummary = new TableCell();
                cellSummary.Text = "Portfolio Grand Total";
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ColumnSpan = 4;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblGrandTotalCost);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                cellSummary.ToolTip = "Total portfolio cost";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = string.Format("{0:0.00}", dblGrandTotalValue);
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.CssClass = "GrandTotalRowStyle";
                cellSummary.ToolTip = "Portfolio valuation";
                rowSummary.Cells.Add(cellSummary);

                cellSummary = new TableCell();
                cellSummary.Text = "";
                cellSummary.HorizontalAlign = HorizontalAlign.Center;
                cellSummary.ColumnSpan = 2;
                cellSummary.CssClass = "GrandTotalRowStyle";
                rowSummary.Cells.Add(cellSummary);

                GridViewSummary.Controls[0].Controls.AddAt(summaryIndex, rowSummary);
                summaryIndex = 2;

                #endregion
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void grdViewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "SYMBOL").ToString();
                strPrevCompName = DataBinder.Eval(e.Row.DataItem, "COMP_NAME").ToString();
                strPrevExchange = DataBinder.Eval(e.Row.DataItem, "EXCHANGE").ToString();

                double dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "PURCHASE_QTY").ToString());
                double dblCost = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "INVESTMENT_COST").ToString());
                double dblValue = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CURRENTVALUE").ToString());
                // Cumulating Sub Total            
                dblSubTotalQuantity += dblQuantity;
                dblSubTotalCost += dblCost;
                dblSubTotalValue += dblValue;
                // Cumulating Grand Total           
                dblGrandTotalQuantity += dblQuantity;
                dblGrandTotalCost += dblCost;
                dblGrandTotalValue += dblValue;

                dblCumYearsInvested = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CumulativeYearsInvested").ToString());
                dblCumARR = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CumulativeARR").ToString());

                datetimeQuoteDateTime = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "CURRENTDATE").ToString());
                dblcurrentQuote = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CURRENTPRICE").ToString());
                // This is for cumulating the values  
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView gridViewPortfolio = (GridView)sender; ;

                    //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ddd'");
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ebeaea'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewPortfolio, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[0].ToolTip = "Transaction date";
                    e.Row.Cells[1].ToolTip = "Transactin price";
                    e.Row.Cells[2].ToolTip = "Transactin quantity";
                    e.Row.Cells[3].ToolTip = "Commission & taxes paid";
                    e.Row.Cells[4].ToolTip = "Investment cost";
                    e.Row.Cells[5].ToolTip = "Current date";
                    e.Row.Cells[6].ToolTip = "Current price";
                    e.Row.Cells[7].ToolTip = "Current value";
                    e.Row.Cells[8].ToolTip = "Years invested";
                    e.Row.Cells[9].ToolTip = "ARR";
                    e.Row.Cells[10].ToolTip = "Cumulative quantity";
                    e.Row.Cells[11].ToolTip = "Cumulative cost";
                    e.Row.Cells[12].ToolTip = "Cumulative value today";
                    e.Row.Cells[13].ToolTip = "Cumulative years invested";
                    e.Row.Cells[14].ToolTip = "Cumulative ARR";
                }
            }
        }

        protected void grdViewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int selectedIndex = System.Convert.ToInt32(e.CommandArgument.ToString());
                GridView gridViewPortfolioMF = (GridView)sender;

                DataTable dt = (DataTable)gridViewPortfolioMF.DataSource;

                string scriptName = dt.Rows[selectedIndex]["SYMBOL"].ToString();

                Session["STOCKPORTFOLIOROWID"] = dt.Rows[selectedIndex]["ID"].ToString();
                Session["STOCKPORTFOLIOSCRIPTID"] = dt.Rows[selectedIndex]["STOCKID"].ToString();
                Session["STOCKPORTFOLIOEXCHANGE"] = dt.Rows[selectedIndex]["EXCHANGE"].ToString();
                Session["STOCKPORTFOLIOSCRIPTNAME"] = dt.Rows[selectedIndex]["SYMBOL"].ToString();
                Session["STOCKPORTFOLIOCOMPNAME"] = dt.Rows[selectedIndex]["COMP_NAME"].ToString();
                Session["STOCKSELECTEDINDEXPORTFOLIO"] = selectedIndex.ToString();

                string purchaseDate = dt.Rows[selectedIndex]["PURCHASE_DATE"].ToString();
                lblCompName.Text = dt.Rows[selectedIndex]["COMP_NAME"].ToString();
                lblScript.Text = scriptName;
                lblExchange.Text = dt.Rows[selectedIndex]["EXCHANGE"].ToString();
                lblInvestmentType.Text = dt.Rows[selectedIndex]["SERIES"].ToString();
                lblDate.Text = purchaseDate;
            }
        }

        protected void GridViewSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView gvSummary = (GridView)sender;

                //this is the row index of the summary grid
                int summaryRowIndex = int.Parse(e.CommandArgument.ToString());

                GridViewRow gvSummaryRow = (GridViewRow)gvSummary.Controls[0].Controls[summaryRowIndex];


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

                string scriptName = dt.Rows[selectedIndex]["SYMBOL"].ToString();

                Session["STOCKPORTFOLIOROWID"] = dt.Rows[selectedIndex]["ID"].ToString();
                Session["STOCKPORTFOLIOSCRIPTID"] = dt.Rows[selectedIndex]["STOCKID"].ToString();
                Session["STOCKPORTFOLIOEXCHANGE"] = dt.Rows[selectedIndex]["EXCHANGE"].ToString();
                Session["STOCKPORTFOLIOSCRIPTNAME"] = dt.Rows[selectedIndex]["SYMBOL"].ToString();
                Session["STOCKPORTFOLIOCOMPNAME"] = dt.Rows[selectedIndex]["COMP_NAME"].ToString();
                Session["STOCKSELECTEDINDEXPORTFOLIO"] = selectedIndex.ToString();

                string purchaseDate = dt.Rows[selectedIndex]["PURCHASE_DATE"].ToString();
                lblCompName.Text = dt.Rows[selectedIndex]["COMP_NAME"].ToString();
                lblScript.Text = scriptName;
                lblExchange.Text = dt.Rows[selectedIndex]["EXCHANGE"].ToString();
                lblInvestmentType.Text = dt.Rows[selectedIndex]["SERIES"].ToString();
                lblDate.Text = purchaseDate;

                ClientScript.RegisterStartupScript(this.GetType(), "setscrollportfolio", "setscrollportfolio('" + actualIndex + "');", true);
            }
        }
        //protected void GridViewPortfolio_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Session["STOCKPORTFOLIOSCRIPTNAME"] = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
        //    Session["STOCKPORTFOLIOCOMPNAME"] = GridViewPortfolio.SelectedRow.Cells[0].Text.ToString();
        //    ViewState["SelectedIndex"] = GridViewPortfolio.SelectedIndex;
        //}

        public void openPortfolio()
        {
            DataTable dt;
            int selectedrow = -1;
            StockManager stockManager = new StockManager();
            try
            {
                if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                {
                    //dt = stockManager.GetPortfolio_ValuationLineGraph(Session["STOCKPORTFOLIOMASTERROWID"].ToString());
                    //dt.DefaultView.RowFilter = "PORTFOLIO_FLAG = 'True'";
                    //dt = dt.DefaultView.ToTable();
                    dt = stockManager.getStockPortfolioTable(Session["STOCKPORTFOLIOMASTERROWID"].ToString());
                    ViewState["FetchedData"] = dt;
                }
                else
                {
                    dt = (DataTable)ViewState["FetchedData"];
                    if (ViewState["SelectedIndex"] != null)
                    {
                        selectedrow = System.Convert.ToInt32(ViewState["SelectedIndex"].ToString());
                    }
                }
                GridViewPortfolio.DataSource = dt;
                GridViewPortfolio.DataBind();

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while opening portfolio: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while opening portfolio:" + ex.Message + "');", true);
            }
        }

        protected void ButtonAddNew_Click(object sender, EventArgs e)
        {
            //ResponseHelper.Redirect(Response, "\\addnewscript.aspx", "_self", "menubar=0,scrollbars=1,width=780,height=900,top=10");
            //ResponseHelper.Redirect(Response, ".\\addnewscript.aspx", "", "");
            Response.Redirect("~/maddnewscript.aspx");
        }
        protected void buttonDeleteSelectedScript_Click(object sender, EventArgs e)
        {
            try
            {
                if ((GridViewPortfolio.SelectedRow != null) && (Session["STOCKPORTFOLIOROWID"] != null))
                {
                    string portfolioRowId = Session["STOCKPORTFOLIOROWID"].ToString();

                    StockManager stockManager = new StockManager();
                    stockManager.deletePortfolioRow(portfolioRowId);

                    Response.Redirect("~/mopenportfolio.aspx");
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

        protected void buttonGetQuote_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/mgetquoteadd.aspx");
        }

        protected void buttonValuation_Click(object sender, EventArgs e)
        {

            //string url = "\\portfoliovaluation.aspx" + "?";
            string url = "~/advGraphs/stockvaluationline.aspx" + "?";

            url += "parent=mopenportfolio.aspx";
            ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");

            //ResponseHelper.Redirect(Response, "\\portfolioValuation.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string stockportfolioRowId = Session["STOCKPORTFOLIOROWID"].ToString();
                    string companyname = Session["STOCKPORTFOLIOCOMPNAME"].ToString();

                    //string symbol = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string symbol = Session["STOCKPORTFOLIOSCRIPTNAME"].ToString();
                    string exchange = Session["STOCKPORTFOLIOEXCHANGE"].ToString();

                    //string date = System.Convert.ToDateTime(GridViewPortfolio.SelectedRow.Cells[0].Text.ToString()).ToString("yyyy-MM-dd hh:mm:ss");
                    string date = System.Convert.ToDateTime(GridViewPortfolio.SelectedRow.Cells[0].Text.ToString()).ToString("yyyy-MM-dd");
                    string price = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string qty = GridViewPortfolio.SelectedRow.Cells[2].Text.ToString();
                    string commission = GridViewPortfolio.SelectedRow.Cells[3].Text.ToString();
                    string cost = GridViewPortfolio.SelectedRow.Cells[4].Text.ToString();
                    string portfolioname = Session["STOCKPORTFOLIONAME"].ToString();

                    Response.Redirect("~/meditscript.aspx?symbol=" + symbol + "&companyname=" + Server.UrlEncode(companyname) + "&price=" + price + "&date=" + date
                        + "&qty=" + qty + "&comission=" + commission + "&cost=" + cost + "&exch=" + Server.UrlEncode(exchange) +
                        "&rowid=" + stockportfolioRowId);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTxnSelected + "');", true);
                }

            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while updating the script:" + ex.Message + "');", true);
            }
        }

        protected void ddlAdvGrphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lblCompName.Text.Equals(string.Empty)) || (lblScript.Text.Equals(string.Empty)) || (lblExchange.Text.Equals(string.Empty)) ||
                (lblInvestmentType.Text.Equals(string.Empty)))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTxnSelected + "');", true);
            }
            else
            {
                string url = "";

                if (ddlAdvGrphType.SelectedValue.Equals("-1") == false)
                {
                    if (ddlAdvGrphType.SelectedValue.Equals("INTRA_VWAP"))
                    {
                        url = "~/advgraphs/pricevalidator.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=Compact" +
                                "&interval=5m" + "&seriestype=CLOSE" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("DAILY_MACD"))
                    {
                        url = "~/advgraphs/trendidentifier.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=Full" +
                        "&interval=1d" + "&seriestype=CLOSE" + "&fastperiod=12" + "&slowperiod=26" + "&signalperiod=9" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("DAILY_RSI"))
                    {
                        url = "~/advgraphs/momentumidentifier.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&period=14" +
                                    "&seriestype=CLOSE" + "&interval=1d" + "&outputSize=Full" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("DAILY_BBANDS"))
                    {
                        url = "~/advgraphs/trendgauger.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Full" +
                        "&interval=1d" + "&seriestype=CLOSE" + "&period=20" + "&stddev=2" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("DAILY_STOCH_RSI"))
                    {
                        url = "~/advgraphs/buysellindicator.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&seriestype=CLOSE" +
                                "&outputsize=Full" + "&interval=1d" +
                                "&fastkperiod=5" + "&slowdperiod=3" + "&period=14" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("DAILY_DI_ADX"))
                    {
                        url = "~/advgraphs/trenddirection.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&seriestype=CLOSE" +
                                    "&outputsize=Full" + "&interval=1d" + "&period=14" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("DAILY_DX_DM_ADX"))
                    {
                        url = "~/advgraphs/pricedirection.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&seriestype=CLOSE" +
                                    "&outputsize=Full" + "&interval=1d" + "&period=14" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("BACKTEST"))
                    {
                        url = "~/advgraphs/backtestsma_stocks.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text +
                            "&smasmall=" + "10" + "&smalong=" + "20" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlAdvGrphType.SelectedValue.Equals("VALUATION_LINE"))
                    {
                        url = "~/advGraphs/stockvaluationline.aspx" + "?";
                    }

                    url += "&parent=mopenportfolioMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void ddlStdGrphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lblCompName.Text.Equals(string.Empty)) || (lblScript.Text.Equals(string.Empty)) || (lblExchange.Text.Equals(string.Empty)) ||
                (lblInvestmentType.Text.Equals(string.Empty)))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTxnSelected + "');", true);
            }
            else
            {
                string url = "";

                if (ddlStdGrphType.SelectedValue.Equals("-1") == false)
                {
                    if (ddlStdGrphType.SelectedValue.Equals("Daily"))
                    {
                        url = "~/advGraphs/stockdaily.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text +
                                        "&seriestype=CLOSE" + "&outputsize=Full" + "&type=" + lblInvestmentType.Text;

                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("Intra"))
                    {
                        url = "~/advGraphs/stockintra.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Compact" +
                            "&interval=5m" + "&seriestype=" + "CLOSE" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("SMA"))
                    {
                        url = "~/advGraphs/stocksma.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Full" +
                    "&interval=1d" + "&seriestype=" + "CLOSE" + "&smallperiod=20" + "&longperiod=50" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("EMA"))
                    {
                        url = "~/advGraphs/stockema.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Full" +
                                "&interval=1d" + "&seriestype=" + "CLOSE" + "&smallperiod=20" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("VWAP"))
                    {
                        url = "~/advGraphs/stockvwap.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Compact" +
                        "&interval=5m" + "&seriestype=" + "CLOSE" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("RSI"))
                    {
                        url = "~/advGraphs/stockrsi.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputSize=Full" + "&interval=1d" +
                            "&seriestype=CLOSE" + "&period=14" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("ADX"))
                    {
                        url = "~/advGraphs/stockadx.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputSize=Full" + "&interval=1d" +
                                "&seriestype=CLOSE" + "&period=20" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("STOCH"))
                    {
                        url = "~/advGraphs/stockstoch.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=Full" +
                        "&interval=1d" + "&seriestype=CLOSE" + "&fastkperiod=5" + "&slowdperiod=3" + "&slowkmatype=0" + "&slowdmatype=0" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("MACD"))
                    {
                        url = "~/advGraphs/stockmacd.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=Full" +
                        "&interval=1d" + "&seriestype=CLOSE" + "&fastperiod=12" + "&slowperiod=26" + "&signalperiod=9" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("AROON"))
                    {
                        url = "~/advGraphs/stockaroon.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Full" +
                        "&interval=1d" + "&seriestype=CLOSE" + "&period=20" + "&type=" + lblInvestmentType.Text;
                    }
                    else if (ddlStdGrphType.SelectedValue.Equals("BBANDS"))
                    {
                        url = "~/advGraphs/stockbbands.aspx" + "?symbol=" + lblScript.Text + "&exchange=" + lblExchange.Text + "&outputsize=" + "Full" +
                        "&interval=1d" + "&seriestype=CLOSE" + "&period=20" + "&stddev=2" + "&type=" + lblInvestmentType.Text;
                    }

                    url += "&parent=mopenportfolioMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }
    }
}