using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class mopenportfolioMF : System.Web.UI.Page
    {
        //new
        string strPreviousRowIDFundHouse = string.Empty;
        //end new

        string strPreviousRowID = string.Empty;

        // To keep track the Index of Group Total    
        int intSubTotalIndex = 2;
        //new
        int intSubTotalIndexFundHouse = 1;
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
            string fileName = "";
            if (Session["EmailId"] != null)
            {
                if (Session["PortfolioNameMF"] != null)
                {
                    //Master.Portfolio = Session["PortfolioName"].ToString();
                    fileName = Session["PortfolioNameMF"].ToString();
                    if (!IsPostBack)
                    {
                        ViewState["FetchedData"] = null;
                        //ViewState["SelectedIndex"] = null;
                    }
                    openPortfolio(fileName);
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
            bool IsSubTotalRowNeedToAddFundHouse = false;
            bool IsSubTotalRowNeedToAdd = false;
            bool IsGrandTotalRowNeedtoAdd = false;
            GridView gridViewPortfolio = (GridView)sender;

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
                cell.Text = "Fund House : " + DataBinder.Eval(e.Row.DataItem, "FundHouse").ToString();
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 9;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndexFundHouse, row);
                intSubTotalIndexFundHouse++;
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
                catch(Exception ex)
                {
                    dblyearsInvested = 0.00;
                    dblARR = 0.00;
                }

                //GridView GridViewPortfolio = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Purchase Unit Quantity Column            
                cell = new TableCell();
                cell.Text = string.Format("{0:0.0000}", dblSubTotalQuantity); //sub total for NAV
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
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
                cell.Text = string.Format("{0:0.0000}", dblSubTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);


                //Adding Current Value Column         
                cell = new TableCell();
                cell.Text = "NA";
                if (dblSubTotalValue > 0)
                {
                    cell.Text = string.Format("{0:0.0000}", dblSubTotalValue);
                }
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);

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
                    cell.Text = string.Format("{0:0.0000}", dblyearsInvested);
                }
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);

                //Adding ARR Column         
                cell = new TableCell();
                cell.Text = "NA";
                if (dblARR > 0)
                {
                    cell.Text = string.Format("{0:0.0000%}", dblARR);
                }
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid      
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                intSubTotalIndexFundHouse++;
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
                    cell.Text = strPreviousRowIDFundHouse + " Sub Total";
                    cell.HorizontalAlign = HorizontalAlign.Left;
                    cell.ColumnSpan = 5;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
                    row.Cells.Add(cell);

                    //Adding Value at Cost Column          
                    cell = new TableCell();
                    cell.Text = string.Format("{0:0.0000}", dblSubTotalCostFundHouse);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
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
                    cell.Text = string.Format("{0:0.0000}", dblSubTotalValueFundHouse);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    //cell.CssClass = "GroupHeaderStyle";
                    cell.CssClass = "FundHouseSubTotalStyle";
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
                    dtFirstNAV = System.Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "PurchaseDate").ToString());
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
                cell.Text = string.Format("{0:0.0000}", dblGrandTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
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
                cell.Text = string.Format("{0:0.0000}", dblGrandTotalValue);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
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
                #endregion
            }
        }
        //Format of the portfolio table
        //now save row - FundHouse;FundName;SCHEME_CODE;PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR
        //Columns the grid view
        //PurchaseDate;PurchaseNAV;PurchaseUnits;CurrentNAV;NAVDate;ValueAtCost;CurrentValue;YearsInvested;ARR
        //protected void grdViewOrders_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    bool IsSubTotalRowNeedToAdd = false;
        //    bool IsGrandTotalRowNeedtoAdd = false;
        //    GridView gridViewPortfolio = (GridView)sender;

        //    if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundName") != null))
        //        if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "FundName").ToString())
        //            IsSubTotalRowNeedToAdd = true;
        //    if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundName") == null))
        //    {
        //        IsSubTotalRowNeedToAdd = true;
        //        IsGrandTotalRowNeedtoAdd = true;
        //        intSubTotalIndex = 0;
        //    }
        //    #region Inserting first Row and populating fist Group Header details
        //    if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FundName") != null))
        //    {
        //        //GridView gridViewPortfolio= (GridView)sender;
        //        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
        //        TableCell cell = new TableCell();
        //        cell.Text = "Fund Name : " + DataBinder.Eval(e.Row.DataItem, "FundName").ToString();
        //        cell.ColumnSpan = 9;
        //        cell.CssClass = "GroupHeaderStyle";
        //        row.Cells.Add(cell);
        //        gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
        //        intSubTotalIndex++;
        //    }
        //    #endregion
        //    if (IsSubTotalRowNeedToAdd)
        //    {
        //        #region Adding Sub Total Row
        //        //GridView GridViewPortfolio = (GridView)sender;
        //        // Creating a Row          
        //        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

        //        //Adding Total Cell          
        //        TableCell cell = new TableCell();
        //        cell.Text = "Sub Total";
        //        cell.HorizontalAlign = HorizontalAlign.Left;
        //        cell.ColumnSpan = 2;
        //        cell.CssClass = "SubTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding Purchase Unit Quantity Column            
        //        cell = new TableCell();
        //        cell.Text = string.Format("{0:0.00}", dblSubTotalQuantity); //sub total for NAV
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.CssClass = "SubTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding empty CurrentNAV;NAVDate
        //        cell = new TableCell();
        //        cell.Text = "";
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.ColumnSpan = 2;
        //        cell.CssClass = "SubTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding Value at Cost col
        //        cell = new TableCell();
        //        cell.Text = string.Format("{0:0.00}", dblSubTotalCost);
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.CssClass = "SubTotalRowStyle";
        //        row.Cells.Add(cell);


        //        //Adding Current Value Column         
        //        cell = new TableCell();
        //        cell.Text = string.Format("{0:0.00}", dblSubTotalValue);
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);

        //        //Adding empty years invested, arr
        //        cell = new TableCell();
        //        cell.Text = "";
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.ColumnSpan = 2;
        //        cell.CssClass = "SubTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding the Row at the RowIndex position in the Grid      
        //        gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
        //        intSubTotalIndex++;
        //        #endregion

        //        #region Adding Next Group Header Details
        //        if (DataBinder.Eval(e.Row.DataItem, "FundName") != null)
        //        {
        //            row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
        //            cell = new TableCell();
        //            cell.Text = "Fund Name : " + DataBinder.Eval(e.Row.DataItem, "FundName").ToString();
        //            cell.ColumnSpan = 9;
        //            cell.CssClass = "GroupHeaderStyle";
        //            row.Cells.Add(cell);
        //            gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
        //            intSubTotalIndex++;
        //        }
        //        #endregion
        //        #region Reseting the Sub Total Variables
        //        dblSubTotalQuantity = 0;
        //        dblSubTotalCost = 0;
        //        dblSubTotalValue = 0;
        //        #endregion
        //    }
        //    if (IsGrandTotalRowNeedtoAdd)
        //    {
        //        #region Grand Total Row
        //        //GridView gridViewPortfolio = (GridView)sender;
        //        // Creating a Row      
        //        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
        //        //Adding Total Cell           
        //        TableCell cell = new TableCell();
        //        cell.Text = "Grand Total";
        //        cell.HorizontalAlign = HorizontalAlign.Left;
        //        cell.ColumnSpan = 5;
        //        cell.CssClass = "GrandTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding Value at Cost Column          
        //        cell = new TableCell();
        //        cell.Text = string.Format("{0:0.00}", dblGrandTotalCost);
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.CssClass = "GrandTotalRowStyle";
        //        row.Cells.Add(cell);

        //        ////Adding empty CurrentNAV;NAVDate
        //        //cell = new TableCell();
        //        //cell.Text = "";
        //        //cell.HorizontalAlign = HorizontalAlign.Center;
        //        //cell.ColumnSpan = 2;
        //        //cell.CssClass = "SubTotalRowStyle";
        //        //row.Cells.Add(cell);

        //        //Adding Current Value Column           
        //        cell = new TableCell();
        //        cell.Text = string.Format("{0:0.00}", dblGrandTotalValue);
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.CssClass = "GrandTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding empty years invested, arr
        //        cell = new TableCell();
        //        cell.Text = "";
        //        cell.HorizontalAlign = HorizontalAlign.Center;
        //        cell.ColumnSpan = 2;
        //        cell.CssClass = "GrandTotalRowStyle";
        //        row.Cells.Add(cell);

        //        //Adding the Row at the RowIndex position in the Grid     
        //        gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
        //        #endregion
        //    }
        //}

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

                Session["MFName"] = dt.Rows[selectedIndex]["FundName"].ToString();
                Session["FundHouse"] = dt.Rows[selectedIndex]["FundHouse"].ToString();
                Session["SchemeCode"] = dt.Rows[selectedIndex]["SCHEME_CODE"].ToString();

                lblDate.Text = System.Convert.ToDateTime(purchaseDate).ToShortDateString();
                lblScript.Text = mfName;

                //string[] argList = e.CommandArgument.ToString().Split(';');

                //if(argList.Length == 4)
                //{
                //string purchaseDate = GridViewPortfolio.Rows[Convert.ToInt32(argList[0])].Cells[0].Text;
                //Session["MFName"] = argList[2];
                //Session["FundHouse"] = argList[1];
                //Session["SchemeCode"] = argList[3];
                //lblScript.Text = argList[2];
                //lblDate.Text = purchaseDate;
                //}

                //string scriptName = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                //Session["ScriptName"] = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                //Session["CompanyName"] = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                //string purchaseDate = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].Text;
                //lblScript.Text = scriptName;
                //lblDate.Text = purchaseDate;
                ////ViewState["SelectedIndex"] = e.CommandArgument.ToString();
                ////GridViewPortfolio.SelectedIndex = System.Convert.ToInt32(e.CommandArgument.ToString());
            }
        }

        public void openPortfolio(string portfolioFileName)
        {
            DataTable dt;

            string folderPath = Server.MapPath("~/mfdata/");
            try
            {

                if (Session["TestDataFolderMF"] != null)
                {
                    folderPath = Session["TestDataFolderMF"].ToString();
                }

                if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                {
                    dt = MFAPI.openMFPortfolio(folderPath, portfolioFileName);
                    ViewState["FetchedData"] = dt;
                }
                else
                {
                    dt = (DataTable)ViewState["FetchedData"];
                    //if (ViewState["SelectedIndex"] != null)
                    //{
                    //    selectedrow = System.Convert.ToInt32(ViewState["SelectedIndex"].ToString());
                    //}
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
                    string mfName = Session["MFName"].ToString();
                    string fundHouse = Session["FundHouse"].ToString();
                    string schemeCode = Session["SchemeCode"].ToString();

                    //Columns the grid view
                    //PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR

                    string purchaseDate = GridViewPortfolio.SelectedRow.Cells[0].Text.ToString();
                    string purchaseNAV = string.Format("{0:0.0000}", System.Convert.ToDouble(GridViewPortfolio.SelectedRow.Cells[1].Text.ToString()));
                    string purchaseUnits = string.Format("{0:0.0000}", System.Convert.ToDouble(GridViewPortfolio.SelectedRow.Cells[2].Text.ToString()));
                    string valueAtCost = string.Format("{0:0.0000}", System.Convert.ToDouble(GridViewPortfolio.SelectedRow.Cells[5].Text.ToString()));
                    //string currentNAV = GridViewPortfolio.SelectedRow.Cells[3].Text.ToString();
                    //string navDate = GridViewPortfolio.SelectedRow.Cells[4].Text.ToString();
                    //string currentValue = GridViewPortfolio.SelectedRow.Cells[5].Text.ToString();
                    //string yearsInvested = GridViewPortfolio.SelectedRow.Cells[6].Text.ToString();
                    //string arr = GridViewPortfolio.SelectedRow.Cells[7].Text.ToString();

                    string filename = Session["PortfolioNameMF"].ToString();

                    MFAPI.deletePortfolioRow(filename, fundHouse, mfName, schemeCode, purchaseDate, purchaseNAV, purchaseUnits, valueAtCost);

                    //openPortfolio(filename);
                    //if (this.MasterPageFile.Contains("Site.Master"))
                    //    Response.Redirect("~/openportfolio.aspx");
                    //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    //    Response.Redirect("~/mopenportfolio.aspx");
                    //else
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

        protected void buttonValuation_Click(object sender, EventArgs e)
        {

            //string url = "\\portfoliovaluation.aspx" + "?";
            string url = "~/portfolioValuationMF.aspx" + "?";

            //if (this.MasterPageFile.Contains("Site.Master"))
            //{
            //    url += "parent=openportfolio.aspx";
            //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
            //}
            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //{
            //    url += "parent=mopenportfolio.aspx";
            //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
            //}
            url += "parent=mopenportfolioMF.aspx";
            ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");

            //ResponseHelper.Redirect(Response, "\\portfolioValuation.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string mfName = Session["MFName"].ToString();
                    string fundHouse = Session["FundHouse"].ToString();
                    string schemeCode = Session["SchemeCode"].ToString();

                    //Columns the grid view
                    //PurchaseDate;PurchaseNAV;PurchaseUnits;ValueAtCost;CurrentNAV;NAVDate;CurrentValue;YearsInvested;ARR

                    string purchaseDate = System.Convert.ToDateTime(GridViewPortfolio.SelectedRow.Cells[0].Text.ToString()).ToShortDateString();
                    string purchaseNAV = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string purchaseUnits = GridViewPortfolio.SelectedRow.Cells[2].Text.ToString();
                    string valueAtCost = GridViewPortfolio.SelectedRow.Cells[5].Text.ToString();

                    Response.Redirect("~/meditmftrans.aspx?fundhouse=" + Server.UrlEncode(fundHouse) +
                        "&fundname=" + Server.UrlEncode(mfName) + "&schemecode=" + schemeCode + "&purchasedate=" + purchaseDate
                        + "&purchasenav=" + purchaseNAV + "&purchaseunits=" + purchaseUnits + "&valueatcost=" + valueAtCost);
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while updating the transaction:" + ex.Message + "');", true);
            }
        }

        protected void buttonValuationLine_Click(object sender, EventArgs e)
        {
            string url = "~/portfolioValuationMF2.aspx" + "?";

            //if (this.MasterPageFile.Contains("Site.Master"))
            //{
            //    url += "parent=openportfolio.aspx";
            //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
            //}
            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //{
            //    url += "parent=mopenportfolio.aspx";
            //    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
            //}
            url += "parent=mopenportfolioMF.aspx";
            ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
        }
    }
}